# Connection Pool Tuning

## Overview

Proper connection pool configuration is critical for database performance and reliability.

## Pool Configuration

### SQLAlchemy Pool Settings

```python
# Example 1: Connection pool configuration
from sqlalchemy import create_engine
from sqlalchemy.pool import QueuePool

engine = create_engine(
    DATABASE_URL,
    poolclass=QueuePool,
    pool_size=20,          # Base pool size
    max_overflow=10,       # Extra connections when pool full
    pool_timeout=30,       # Seconds to wait for connection
    pool_recycle=1800,     # Recycle connections after 30 minutes
    pool_pre_ping=True,    # Verify connections before use
)
```

### Pool Monitoring

```python
# Example 2: Monitor pool status
from fastapi import FastAPI
from sqlalchemy import event

app = FastAPI()

@event.listens_for(engine, "checkout")
def on_checkout(dbapi_conn, connection_record, connection_proxy):
    """Log connection checkout"""
    print(f"Connection checked out. Pool size: {engine.pool.size()}")

@app.get("/pool-status")
async def pool_status():
    """Get pool statistics"""
    pool = engine.pool
    return {
        "pool_size": pool.size(),
        "checked_in": pool.checkedin(),
        "checked_out": pool.checkedout(),
        "overflow": pool.overflow()
    }
```

## Tuning Guidelines

| Parameter | Recommendation |
|-----------|----------------|
| pool_size | CPU cores * 2 + 1 |
| max_overflow | pool_size / 2 |
| pool_timeout | 30 seconds |
| pool_recycle | 1800 seconds |

## Summary

Proper pool tuning prevents connection exhaustion and improves performance.

## Next Steps

Continue learning about:
- [Read Replica Setup](./08_read_replica_setup.md)
- [Database Monitoring](./12_database_monitoring.md)
