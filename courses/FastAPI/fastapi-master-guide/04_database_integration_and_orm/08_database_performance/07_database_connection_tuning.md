# Database Connection Tuning

## Overview

Optimizing database connections improves application performance and reliability.

## Connection Configuration

### SQLAlchemy Connection Settings

```python
# Example 1: Connection tuning
from sqlalchemy import create_engine

engine = create_engine(
    DATABASE_URL,
    # Connection pool settings
    pool_size=20,
    max_overflow=10,
    pool_timeout=30,
    pool_recycle=1800,
    pool_pre_ping=True,

    # Connection settings
    connect_args={
        "connect_timeout": 10,
        "options": "-c statement_timeout=30000"
    },

    # Performance settings
    echo=False,
    echo_pool=False,
)
```

### PostgreSQL Specific

```python
# Example 2: PostgreSQL connection tuning
engine = create_engine(
    "postgresql+asyncpg://user:pass@localhost/db",
    connect_args={
        "server_settings": {
            "application_name": "fastapi_app",
            "jit": "off"  # Disable JIT for simple queries
        }
    },
    pool_size=20,
    max_overflow=10,
)
```

## Monitoring

```python
# Example 3: Connection monitoring
from sqlalchemy import event

@event.listens_for(engine, "checkout")
def on_checkout(dbapi_conn, connection_record, connection_proxy):
    print(f"Connection checked out")

@event.listens_for(engine, "checkin")
def on_checkin(dbapi_conn, connection_record):
    print(f"Connection checked in")
```

## Summary

Proper connection tuning prevents bottlenecks and improves reliability.

## Next Steps

Continue learning about:
- [Read Replica Setup](./08_read_replica_setup.md)
- [Connection Pool Tuning](./05_connection_pool_tuning.md)
