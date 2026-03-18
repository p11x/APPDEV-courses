# Database Sharding and Partitioning

## What You'll Learn
- The difference between sharding and partitioning (they're not the same!)
- When to shard: the mathematical case for horizontal scaling
- PostgreSQL table partitioning for time-series and high-volume data
- Application-level sharding patterns with consistent hashing
- How to handle cross-shard queries and distributed transactions

## Prerequisites
- Completed `01-multi-tenancy-architecture.md` — understanding of multi-tenant data models
- Completed `05-databases/02-sqlalchemy-orm.md` — SQLAlchemy for query building
- Understanding of database indexes and query planning

## Partitioning vs Sharding: What's the Difference?

People use these terms interchangeably, but they mean different things:

| Aspect | Partitioning | Sharding |
|--------|-------------|----------|
| **Scope** | Single database instance | Multiple database instances |
| **Goal** | Manage large tables within one DB | Scale beyond one DB's capacity |
| **Implementation** | PostgreSQL native feature | Application-level routing |
| **Complexity** | Low (DB handles it) | High (you handle it) |

Think of it this way: partitioning is like organizing a warehouse into zones within one building. Sharding is like building multiple warehouses in different cities.

## PostgreSQL Table Partitioning

PostgreSQL has native support for partitioning — the database handles routing queries to the right partition automatically.

### Creating a Partitioned Table

```python
from sqlalchemy import Column, Integer, String, DateTime, create_engine
from sqlalchemy.orm import sessionmaker, DeclarativeBase
from datetime import datetime, timedelta

engine = create_engine("postgresql://user:pass@localhost/mydb")
SessionLocal = sessionmaker(bind=engine)

class Base(DeclarativeBase):
    pass

# Define partitioned table using declarative syntax
class Order(Base):
    __tablename__ = "orders"
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, nullable=False)
    created_at = Column(DateTime, nullable=False, default=datetime.utcnow)
    customer_id = Column(Integer)
    total = Column(Integer)  # In cents
```

🔍 **Line-by-Line Breakdown:**
1. `__tablename__ = "orders"` — SQLAlchemy maps this to PostgreSQL table
2. `id = Column(Integer, primary_key=True)` — Every order needs a unique ID
3. `tenant_id = Column(Integer, nullable=False)` — For multi-tenant filtering
4. `created_at = Column(DateTime)` — The partition key — determines which partition stores the row

Now create the partitioned table in PostgreSQL directly:

```sql
CREATE TABLE orders (
    id INTEGER NOT NULL,
    tenant_id INTEGER NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    customer_id INTEGER,
    total INTEGER,
    PRIMARY KEY (id, created_at)
) PARTITION BY RANGE (created_at);

-- Monthly partitions
CREATE TABLE orders_2024_01 PARTITION OF orders
    FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');

CREATE TABLE orders_2024_02 PARTITION OF orders
    FOR VALUES FROM ('2024-02-01') TO ('2024-03-01');

CREATE TABLE orders_2024_03 PARTITION OF orders
    FOR VALUES FROM ('2024-03-01') TO ('2024-04-01');

-- Catch-all for future data
CREATE TABLE orders_future PARTITION OF orders
    FOR VALUES FROM (MAXVALUE) TO (MAXVALUE);
```

### How Partitioning Works in Queries

PostgreSQL's query planner automatically routes queries to the correct partition:

```python
# This query ONLY scans the January partition!
session.query(Order).filter(
    Order.created_at >= datetime(2024, 1, 1),
    Order.created_at < datetime(2024, 2, 1)
).all()
```

Generated SQL shows partition pruning:
```sql
SELECT * FROM orders_2024_01 WHERE ...
-- PostgreSQL knew to skip other partitions!
```

### Managing Partitions Automatically

Create a function to manage partitions:

```python
from sqlalchemy import text

def ensure_partitions(session, months_ahead: int = 3):
    """Create partitions for upcoming months."""
    for i in range(months_ahead):
        # Calculate first day of next month
        base_date = datetime.utcnow().replace(day=1) + timedelta(days=32 * i)
        month_start = base_date.replace(day=1)
        month_end = (month_start + timedelta(days=32)).replace(day=1)
        
        partition_name = f"orders_{month_start.strftime('%Y_%m')}"
        
        # Check if partition exists
        result = session.execute(text(f"""
            SELECT 1 FROM pg_tables 
            WHERE tablename = '{partition_name}'
        """))
        
        if not result.first():
            # Create partition
            session.execute(text(f"""
                CREATE TABLE {partition_name} PARTITION OF orders
                FOR VALUES FROM ('{month_start.isoformat()}') 
                TO ('{month_end.isoformat()}')
            """))
    
    session.commit()
```

## Application-Level Sharding

When one database can't handle your load, you need sharding — splitting data across multiple database instances.

### Consistent Hash Ring

The most common approach is consistent hashing:

```python
import hashlib
from bisect import bisect_left

class ShardRouter:
    """Routes queries to the correct database shard."""
    
    def __init__(self, shard_configs: list[dict]):
        """
        Args:
            shard_configs: List of {"host": str, "port": int, "shard_id": int}
        """
        self.shards = sorted(shard_configs, key=lambda x: x["shard_id"])
        self.vnodes = 150  # Virtual nodes per physical shard for even distribution
        
        # Build hash ring
        self.ring: list[tuple[int, dict]] = []
        for shard in self.shards:
            for vnode in range(self.vnodes):
                key = f"shard_{shard['shard_id']}_vn_{vnode}"
                hash_val = int(hashlib.md5(key.encode()).hexdigest(), 16)
                self.ring.append((hash_val, shard))
        
        self.ring.sort(key=lambda x: x[0])
        self.hash_values = [h for h, _ in self.ring]
    
    def get_shard(self, tenant_id: int) -> dict:
        """Get the shard config for a tenant."""
        hash_val = int(hashlib.md5(str(tenant_id).encode()).hexdigest(), 16)
        idx = bisect_left(self.hash_values, hash_val) % len(self.shards)
        return self.shards[idx]
```

### Shard-Aware Session Factory

Create sessions for the correct shard:

```python
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

class ShardedSessionFactory:
    def __init__(self, router: ShardRouter):
        self.router = router
        self.engines: dict[int, object] = {}
    
    def get_session(self, tenant_id: int):
        shard = self.router.get_shard(tenant_id)
        shard_id = shard["shard_id"]
        
        # Create or reuse engine for this shard
        if shard_id not in self.engines:
            url = f"postgresql://user:pass@{shard['host']}:{shard['port']}/app_shard_{shard_id}"
            self.engines[shard_id] = create_engine(url)
        
        Session = sessionmaker(bind=self.engines[shard_id])
        return Session()

# Usage
router = ShardRouter([
    {"host": "db-shard-1.internal", "port": 5432, "shard_id": 0},
    {"host": "db-shard-2.internal", "port": 5432, "shard_id": 1},
    {"host": "db-shard-3.internal", "port": 5432, "shard_id": 2},
])

session_factory = ShardedSessionFactory(router)

def get_user_shard(tenant_id: int, user_id: int) -> dict:
    session = session_factory.get_session(tenant_id)
    user = session.query(User).filter(User.id == user_id).first()
    return {"id": user.id, "name": user.name} if user else None
```

## Handling Cross-Shard Queries

The hard part of sharding is querying data that spans shards.

### Approach 1: Scatter-Gather

```python
def search_all_shards(query_fn, tenant_id: int | None = None):
    """Run query on all shards and combine results."""
    results = []
    
    for shard in router.shards:
        session = session_factory.get_session(shard["shard_id"])
        results.extend(query_fn(session))
    
    # Deduplicate and sort
    return sorted(results, key=lambda x: x.id)[:100]  # Top 100
```

### Approach 2: Replica Sets

For reads, use read replicas:

```python
class ReadReplicaRouter:
    def __init__(self, primary: str, replicas: list[str]):
        self.primary = primary
        self.replicas = replicas
        self.round_robin = 0
    
    def get_read_session(self):
        # Round-robin across replicas
        replica = self.replicas[self.round_robin % len(self.replicas)]
        self.round_robin += 1
        return create_session(replica)
    
    def get_write_session(self):
        return create_session(self.primary)
```

## Production Considerations

- **Partitioning overhead**: Each partition is a real table. Managing 60 monthly partitions for 3 years = 60 tables. Works fine, but backup/restore gets complex.
- **Shard rebalancing**: Moving data between shards is painful. Choose your shard key carefully — changing it later requires data migration.
- **Connection pooling**: With sharding, you potentially have 10+ database connections per application instance. Use PgBouncer or similar.
- **Monitoring**: Track query latency per shard. One slow shard drags down the entire application.
- **Backup strategy**: With partitioning, backup each partition separately. With sharding, backup each shard's database separately.

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Partitioning by tenant_id for high-cardinality tenants

**Wrong:**
```sql
CREATE TABLE orders PARTITION BY LIST (tenant_id);
CREATE TABLE orders_tenant_1 PARTITION OF orders FOR VALUES IN (1);
CREATE TABLE orders_tenant_2 PARTITION OF orders FOR VALUES IN (2);
-- With 1000 tenants, you need 1000 partitions!
```

**Why it fails:** PostgreSQL has a limit on partition count (thousands is OK, millions is not). With 1000 tenants each getting their own partition, you hit limits and management becomes impossible.

**Fix:** Partition by time (created_at) or another naturally granular column:
```sql
CREATE TABLE orders PARTITION BY RANGE (created_at);
```

### ❌ Mistake 2: Queries that touch all partitions

**Wrong:**
```python
# This scans ALL partitions every time!
session.query(Order).filter(Order.tenant_id == 5).all()
```

**Why it fails:** Without a partition key in the WHERE clause, PostgreSQL scans every partition. At 36 monthly partitions, that's 36x the I/O.

**Fix:** Always include the partition key in queries:
```python
session.query(Order).filter(
    Order.tenant_id == 5,
    Order.created_at >= datetime(2024, 1, 1)  # Partition key!
).all()
```

### ❌ Mistake 3: No plan for shard rebalancing

**Wrong:** Picking tenant_id as shard key without thinking about growth.

**Why it fails:** If one tenant grows to 100x average size, they become a "noisy neighbor" on their shard. You need to move them, but you have no plan.

**Fix:** Use consistent hashing with virtual nodes — it makes rebalancing smoother when you add/remove shards.

## Summary

- Partitioning splits large tables within one database (PostgreSQL handles routing)
- Sharding splits data across multiple databases (application handles routing)
- Use partitioning for tables > 100GB that naturally range-scan by time/region
- Use sharding when you exceed what one database can handle (typically > 1TB)
- Always include partition key in queries to get partition pruning
- Cross-shard queries require scatter-gather — expensive, so design to minimize them

## Next Steps

→ Continue to `03-read-replicas-and-connection-routing.md` to learn how to scale read operations with replicas while maintaining consistency.
