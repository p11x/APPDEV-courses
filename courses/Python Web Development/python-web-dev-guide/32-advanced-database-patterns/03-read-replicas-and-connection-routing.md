# Read Replicas and Connection Routing

## What You'll Learn
- How read replicas work and why they matter for scaling
- The difference between synchronous and asynchronous replication
- Implementing automatic read/write splitting in Python
- Handling replication lag and eventual consistency
- PgBouncer for connection pooling with replicas

## Prerequisites
- Completed `02-database-sharding-and-partitioning.md` — understanding of database scaling patterns
- Completed `05-databases/02-sqlalchemy-orm.md` — SQLAlchemy session management
- Understanding of ACID properties and eventual consistency

## Why Read Replicas?

Most applications are read-heavy: 80-90% of database operations are reads, only 10-20% are writes. A single primary database can handle writes, but adding read replicas lets you scale read capacity horizontally:

```
                    ┌─────────────────┐
                    │  Application    │
                    └────────┬────────┘
                             │
              ┌──────────────┼──────────────┐
              │              │              │
              ▼              ▼              ▼
       ┌──────────┐   ┌──────────┐   ┌──────────┐
       │  Replica │   │  Replica │   │  Replica │
       │    #1    │   │    #2    │   │    #3    │
       └────┬─────┘   └────┬─────┘   └────┬─────┘
            │              │              │
            └──────────────┼──────────────┘
                           │
                           ▼
                  ┌─────────────────┐
                  │     Primary     │
                  │   (writes +    │
                  │    reads)      │
                  └─────────────────┘
```

## Synchronous vs Asynchronous Replication

| Aspect | Synchronous | Asynchronous |
|--------|-------------|--------------|
| **Latency** | Higher (waits for replica) | Lower (returns immediately) |
| **Consistency** | Strong (replica has data) | Eventual (replica may lag) |
| **Availability** | Lower (any replica down blocks) | Higher (tolerates replica failures) |

PostgreSQL uses asynchronous replication by default — the primary returns "success" after writing locally, without waiting for replicas to confirm.

## Implementing Read/Write Splitting

### Using SQLAlchemy with Manual Routing

Create separate engines for primary and replicas:

```python
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, Session
from contextlib import contextmanager

# Primary (handles writes and critical reads)
primary_engine = create_engine(
    "postgresql://user:pass@primary.db.internal:5432/mydb",
    pool_size=10,
    max_overflow=20,
)

# Read replicas
replica_engines = [
    create_engine(
        f"postgresql://user:pass@replica{i}.db.internal:5432/mydb",
        pool_size=10,
        pool_pre_ping=True,  # Verify connections
    )
    for i in range(1, 4)
]

SessionLocal = sessionmaker(bind=primary_engine)

class ReadReplicaRouter:
    def __init__(self, replica_engines: list):
        self.replicas = replica_engines
        self.round_robin = 0
    
    def get_read_session(self) -> Session:
        """Round-robin across replicas for load distribution."""
        engine = self.replicas[self.round_robin % len(self.replicas)]
        self.round_robin += 1
        return sessionmaker(bind=engine)()
    
    def get_write_session(self) -> Session:
        """Always use primary for writes."""
        return SessionLocal()

replica_router = ReadReplicaRouter(replica_engines)
```

Now route queries appropriately:

```python
from fastapi import FastAPI, Depends
from typing import Annotated

app = FastAPI()

async def get_db_read():
    """Dependency for read queries - uses replica."""
    session = replica_router.get_read_session()
    try:
        yield session
    finally:
        session.close()

async def get_db_write():
    """Dependency for write queries - uses primary."""
    session = replica_router.get_write_session()
    try:
        yield session
    finally:
        session.close()

# Use the right dependency based on operation
@app.get("/users/{user_id}")
async def get_user(
    user_id: int,
    db: Session = Depends(get_db_read)
):
    """Read query - use replica"""
    user = db.query(User).filter(User.id == user_id).first()
    return {"id": user.id, "name": user.name}

@app.post("/users")
async def create_user(
    name: str,
    db: Session = Depends(get_db_write)
):
    """Write query - use primary"""
    user = User(name=name)
    db.add(user)
    db.commit()
    return {"id": user.id, "name": user.name}
```

### Automatic Read/Write Detection

A more sophisticated approach automatically detects read vs write:

```python
from sqlalchemy import event
from sqlalchemy.orm import Session
import random

class RoutingSession(Session):
    """Session that automatically routes to replica for reads."""
    
    def __init__(self, *args, **kwargs):
        self._replica_engines = kwargs.pop("replica_engines", [])
        self._primary_engine = kwargs.pop("primary_engine")
        self._use_replica = False
        super().__init__(*args, **kwargs)
    
    def execute(self, *args, **kwargs):
        # Decide which engine to use based on statement type
        if self._replica_engines and self._should_use_replica(args):
            # Would route to replica for SELECT statements
            pass
        return super().execute(*args, **kwargs)
    
    def _should_use_replica(self, args) -> bool:
        """Heuristic: if there's a SELECT, use replica."""
        # Implementation would parse SQL to detect SELECT
        return True  # Simplified

# In practice, use a library like `sqlalchemy-replicado` or handle at middleware level
```

## Handling Replication Lag

With asynchronous replication, replicas can lag behind the primary. This creates "eventual consistency" — data you just wrote might not immediately appear on reads:

```python
from datetime import datetime, timedelta
import random

class StickyReplicaSession(Session):
    """Session that sticks to one replica for consistency."""
    
    def __init__(self, *args, **kwargs):
        self._replica = None
        self._last_write_time: datetime | None = None
        super().__init__(*args, **kwargs)
    
    def commit(self):
        """After commit, force primary for a window."""
        result = super().commit()
        self._last_write_time = datetime.utcnow()
        return result
    
    def query(self, *args, **kwargs):
        """If recently wrote, bypass replica to avoid stale reads."""
        if self._should_bypass_replica():
            return super().query(*args, **kwargs)
        # Would route to replica
        return super().query(*args, **kwargs)
    
    def _should_bypass_replica(self) -> bool:
        if self._last_write_time is None:
            return False
        # Bypass replica for 2 seconds after write
        lag = datetime.utcnow() - self._last_write_time
        return lag < timedelta(seconds=2)
```

## PgBouncer for Connection Pooling

Opening a new database connection for every request is expensive. PgBouncer sits between your app and database, maintaining a pool of connections:

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   FastAPI   │────▶│  PgBouncer  │────▶│ PostgreSQL  │
│   (100s of │     │ (20 pooled  │     │  (20 real   │
│   requests) │     │  conns)    │     │   conns)    │
└─────────────┘     └─────────────┘     └─────────────┘
```

### PgBouncer Configuration

```ini
# pgbouncer.ini
[databases]
mydb = host=primary.db.internal port=5432 dbname=mydb

[pgbouncer]
listen_port = 6432
listen_addr = 0.0.0.0
auth_type = md5
auth_file = userlist.txt

pool_mode = transaction  # Connections live for one transaction
max_client_conn = 1000
default_pool_size = 20
min_pool_size = 5
```

### Python Connection to PgBouncer

```python
# Connect to PgBouncer (not directly to PostgreSQL)
engine = create_engine(
    "postgresql://user:pass@localhost:6432/mydb",  # Localhost:6432
    pool_size=5,  # PgBouncer handles the real pool
    max_overflow=10,
)
```

## Production Considerations

- **Replication lag monitoring**: Track replica lag (`pg_replication_slots`, `pg_stat_replication`) and alert if it exceeds threshold (e.g., 5 seconds).
- **Write-after-read consistency**: If a user creates data then immediately views it, route the read to primary to avoid showing stale data.
- **Connection exhaustion**: With multiple app servers and multiple replicas, you can exhaust connection limits. PgBouncer is essential.
- **Failover**: When primary fails, promote a replica. Your routing logic needs to detect this and update endpoints.
- **Cross-region replication**: Replicas in different regions have higher lag. Decide if your app can tolerate 100ms vs 10ms staleness.

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Using replica for everything

**Wrong:**
```python
async def get_user(user_id: int, db: Session = Depends(get_db)):
    # Always uses primary - defeats scaling purpose!
    return db.query(User).filter(User.id == user_id).first()
```

**Why it fails:** All traffic goes to primary anyway, defeating the point of replicas.

**Fix:** Separate read and write dependencies:
```python
async def get_user_read(user_id: int, db: Session = Depends(get_db_read)):
    return db.query(User).filter(User.id == user_id).first()
```

### ❌ Mistake 2: Not checking replica lag

**Wrong:** Assuming replica is always in sync.

**Why it fails:** Under load, replicas can lag by seconds. Users might see data disappear after creating it.

**Fix:** Add lag monitoring and fall back to primary for recent writes:
```python
# Check lag before critical reads
result = session.execute(text("SELECT now() - pg_last_xact_replay_timestamp() AS lag"))
lag = result.scalar()
if lag and lag.total_seconds() > 5:
    # Too far behind, use primary
    return get_from_primary(user_id)
```

### ❌ Mistake 3: Opening new connections without pooling

**Wrong:**
```python
async def get_user(user_id: int):
    # New connection every request!
    engine = create_engine("postgresql://...")
    session = sessionmaker(bind=engine)()
```

**Why it fails:** Connection setup takes 10-50ms. At 100 requests/second, you spend all your time opening connections.

**Fix:** Use PgBouncer or proper pooling:
```python
engine = create_engine("postgresql://...", pool_size=20)  # Reuse connections
```

## Summary

- Read replicas scale read capacity horizontally — add more replicas to handle more read traffic
- Route writes to primary, reads to replicas — use dependency injection to enforce this
- Asynchronous replication means replicas can lag — handle eventual consistency
- Use PgBouncer to pool connections and avoid exhaustion
- Monitor replica lag and alert on thresholds

## Next Steps

→ Continue to `04-timescaledb-time-series-data.md` to learn how to handle time-series data at scale with TimescaleDB.
