# Multi-Tenancy Architecture

## What You'll Learn
- What multi-tenancy means and why it matters for SaaS products
- The three primary multi-tenancy models (shared database, schema-per-tenant, database-per-tenant)
- When to choose each approach based on isolation, cost, and operational complexity
- How to implement a tenant context system in FastAPI using dependency injection

## Prerequisites
- Completed `05-databases/02-sqlalchemy-orm.md` — understanding of SQLAlchemy models and sessions
- Completed `04-fastapi/04-dependency-injection.md` — FastAPI dependency injection patterns
- Understanding of PostgreSQL schema concepts

## Why Multi-Tenancy?

Multi-tenancy is an architecture where a single application instance serves multiple customers (tenants), each isolated from the others. Think of it like an apartment building: one physical building (application), but each tenant has their own locked unit (tenant data).

```
┌─────────────────────────────────────────────────────────────────┐
│                    Multi-Tenant Application                      │
│                                                                  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐        │
│  │   Tenant A   │  │   Tenant B   │  │   Tenant C   │        │
│  │  (Acme Corp) │  │ (Globex Inc) │  │  (Initech)  │        │
│  └──────────────┘  └──────────────┘  └──────────────┘        │
│                                                                  │
│                    Shared Application Code                        │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

**Why bother?** Multi-tenancy reduces infrastructure costs by maximizing resource sharing. Instead of 100 servers for 100 customers, you might run 5 servers that serve all 100 customers with better overall utilization.

## The Three Models

| Aspect | Shared Database | Schema-per-Tenant | Database-per-Tenant |
|--------|---------------|-------------------|---------------------|
| **Isolation** | Low | Medium | High |
| **Cost** | Lowest | Medium | Highest |
| **Complexity** | Low | Medium | High |
| **Noisy Neighbor** | Yes | Possible | No |
| **Backup/Restore** | Harder | Per-schema | Per-database |

### Model 1: Shared Database, Shared Schema (Most Common)

All tenants share tables, but every table has a `tenant_id` column:

```python
from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import DeclarativeBase

class Base(DeclarativeBase):
    pass

class User(Base):
    __tablename__ = "users"
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, nullable=False, index=True)  # Critical!
    name = Column(String)
    email = Column(String)
```

**When to use:** Most SaaS products. Good isolation via `tenant_id` is sufficient for 95% of use cases.

### Model 2: Schema-per-Tenant (PostgreSQL Only)

Each tenant gets their own PostgreSQL schema within the same database:

```sql
-- Creating tenant schemas
CREATE SCHEMA acme_corp;
CREATE SCHEMA globex_inc;

-- Tables exist in each schema
CREATE TABLE acme_corp.users (id SERIAL, name TEXT);
CREATE TABLE globex_inc.users (id SERIAL, name TEXT);
```

**When to use:** When you need stronger isolation than `tenant_id` but can't afford separate databases.

### Model 3: Database-per-Tenant (Highest Isolation)

Each tenant gets a completely separate database:

```
postgresql://tenant-acme:pass@db.acme.com:5432/acme_db
postgresql://tenant-globex:pass@db.globex.com:5432/globex_db
```

**When to use:** Enterprise customers who require contractual data isolation, or when tenants have vastly different database needs.

## Implementing Shared Database with Tenant Context

The most common approach is shared database with a tenant context that flows through every request:

```python
from fastapi import FastAPI, Depends, Header, HTTPException
from sqlalchemy import Column, Integer, String, create_engine
from sqlalchemy.orm import sessionmaker, Session
from typing import Annotated

app = FastAPI()

# Database setup
DATABASE_URL = "postgresql://user:pass@localhost/mysaas"
engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Tenant context - stored in thread-local storage for sync, contextvars for async
from contextvars import ContextVar
tenant_context: ContextVar[int | None] = ContextVar("tenant_id", default=None)

def get_tenant_id() -> int:
    """Extract tenant ID from current context."""
    tid = tenant_context.get()
    if tid is None:
        raise HTTPException(status_code=400, detail="No tenant context")
    return tid

# Dependency to extract tenant from API key
async def get_current_tenant(x_api_key: Annotated[str | None, Header()]) -> int:
    """Validate API key and return tenant ID."""
    if not x_api_key:
        raise HTTPException(status_code=401, detail="API key required")
    
    # In production, lookup tenant from database
    # This is a simplified example
    tenants = {"key-acme": 1, "key-globex": 2}
    tenant_id = tenants.get(x_api_key)
    
    if not tenant_id:
        raise HTTPException(status_code=403, detail="Invalid API key")
    
    return tenant_id

# Tenant-scoped session dependency
async def get_tenant_db(
    tenant_id: int = Depends(get_current_tenant)
) -> Session:
    """Create a database session with tenant context set."""
    # Set tenant context for this request
    token = tenant_context.set(tenant_id)
    try:
        db = SessionLocal()
        yield db
    finally:
        db.close()
        tenant_context.reset(token)
```

Now every route automatically has the correct `tenant_id` in context:

```python
from sqlalchemy.orm import DeclarativeBase

class Base(DeclarativeBase):
    pass

class User(Base):
    __tablename__ = "users"
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, nullable=False, index=True)
    name = Column(String(100))
    email = Column(String(255))

# Every query is automatically tenant-scoped
@app.get("/users")
async def list_users(
    db: Session = Depends(get_tenant_db),
    tenant_id: int = Depends(get_current_tenant)
):
    # This query ONLY returns users for the current tenant
    users = db.query(User).filter(User.tenant_id == tenant_id).all()
    return [{"id": u.id, "name": u.name} for u in users]
```

## The Critical Mistake: Forgetting Tenant Filter

The most common error is accidentally querying all tenants:

```python
# ❌ WRONG: Returns ALL tenants' data!
async def list_users_wrong(db: Session = Depends(get_tenant_db)):
    return db.query(User).all()  # Missing tenant_id filter!

# ✅ CORRECT: Always filter by tenant
async def list_users_correct(
    db: Session = Depends(get_tenant_db),
    tenant_id: int = Depends(get_current_tenant)
):
    return db.query(User).filter(User.tenant_id == tenant_id).all()
```

## Using Row-Level Security (PostgreSQL)

PostgreSQL has built-in RLS that can enforce tenant isolation at the database level:

```sql
-- Enable RLS on users table
ALTER TABLE users ENABLE ROW LEVEL SECURITY;

-- Create policy that filters by tenant_id
CREATE POLICY tenant_isolation_policy ON users
    USING (tenant_id = current_setting('app.tenant_id')::int);
```

Then set the tenant context in PostgreSQL for each connection:

```python
from sqlalchemy import event

@event.listens_for(engine, "connect")
def set_tenant_search_path(dbapi_connection, connection_record):
    """Set tenant context on every new database connection."""
    tenant_id = tenant_context.get()
    if tenant_id:
        cursor = dbapi_connection.cursor()
        cursor.execute(f"SET app.tenant_id = {tenant_id}")
        cursor.close()
```

This provides defense-in-depth: even if your application code has a bug, PostgreSQL won't return wrong tenant's data.

## Production Considerations

- **Scaling**: Shared-database approach scales horizontally easiest. Just add more app servers. Schema-per-tenant requires connection pooling per schema. Database-per-tenant is hardest to scale.
- **Cost**: Database-per-tenant can be 10-50x more expensive. For 1000 tenants, would you rather manage 1 database or 1000?
- **Failure modes**: Schema-per-tenant: one schema corruption affects one tenant. Shared database: one bad query could expose wrong tenant data (use RLS as defense).
- **Monitoring**: Track query performance per tenant. Noisy neighbor problems emerge when one tenant's queries slow down everyone else.
- **Migration complexity**: Schema changes in shared-database require careful planning (add columns, not rename). Schema-per-tenant: run migrations against each schema.

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not Indexing tenant_id

**Wrong:**
```python
class User(Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer)  # No index!
    name = Column(String)
```

**Why it fails:** Every query filters by `tenant_id`. Without an index, PostgreSQL does a full table scan for every single request. At 10,000 tenants with 1M users, your app becomes unusable.

**Fix:**
```python
tenant_id = Column(Integer, nullable=False, index=True)  # Index added
```

### ❌ Mistake 2: Hardcoding tenant ID in models

**Wrong:**
```python
class User(Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, default=1)  # Hardcoded default!
    name = Column(String)
```

**Why it fails:** Defaults silently create data in wrong tenant when context isn't set. Silent data corruption is the worst kind of bug.

**Fix:** Always require `tenant_id` explicitly:

```python
tenant_id = Column(Integer, nullable=False)  # No default - will error if missing
```

### ❌ Mistake 3: Storing tenant ID in a global variable

**Wrong:**
```python
current_tenant_id = None  # Global state!

def list_users():
    return db.query(User).filter(User.tenant_id == current_tenant_id).all()
```

**Why it fails:** Global variables are not thread-safe. In a multi-threaded server, one request could see another request's tenant ID. Use `contextvars` instead:

```python
from contextvars import ContextVar
tenant_context: ContextVar[int | None] = ContextVar("tenant_id", default=None)
```

## Summary

- Multi-tenancy lets one app serve multiple customers while keeping data isolated
- Choose: shared-database (most common, `tenant_id` column), schema-per-tenant (PostgreSQL), or database-per-tenant (highest isolation)
- Always filter queries by `tenant_id` — use RLS as defense-in-depth
- Use `contextvars` for thread-safe tenant context, not global variables
- Index the `tenant_id` column on every table

## Next Steps

→ Continue to `02-database-sharding-and-partitioning.md` to learn how to scale beyond what a single database can handle.
