# Connection Pooling

## Overview

Connection pooling manages database connections efficiently, reducing overhead and improving performance.

## Connection Pool Concepts

### Pool Architecture

```python
# Example 1: Understanding connection pools
"""
Connection Pool Architecture:

     Application
         |
    Connection Pool
    +---+---+---+---+
    | C | C | C | C |  C = Connection
    +---+---+---+---+
         |
    Database Server

Pool Parameters:
- pool_size: Base number of connections
- max_overflow: Extra connections when pool full
- pool_timeout: Wait time for connection
- pool_recycle: Recycle connections after N seconds
- pool_pre_ping: Test connections before use
"""

from sqlalchemy import create_engine
from sqlalchemy.pool import QueuePool, NullPool, StaticPool

# Standard connection pool (recommended for production)
engine = create_engine(
    DATABASE_URL,
    poolclass=QueuePool,
    pool_size=10,           # 10 base connections
    max_overflow=20,        # Up to 20 extra connections
    pool_timeout=30,        # Wait 30 seconds for connection
    pool_recycle=1800,      # Recycle after 30 minutes
    pool_pre_ping=True      # Verify connection health
)

# NullPool (no pooling, for testing)
engine_null = create_engine(
    DATABASE_URL,
    poolclass=NullPool      # Each operation gets new connection
)

# StaticPool (single connection, for SQLite memory)
engine_static = create_engine(
    "sqlite:///:memory:",
    poolclass=StaticPool    # Single connection reused
)
```

## Pool Configuration

### Production Configuration

```python
# Example 2: Production pool configuration
from sqlalchemy import create_engine
from sqlalchemy.pool import QueuePool
import os

class DatabaseConfig:
    """Database configuration with pool settings"""

    def __init__(self):
        self.url = os.getenv("DATABASE_URL")
        self.pool_size = int(os.getenv("DB_POOL_SIZE", "20"))
        self.max_overflow = int(os.getenv("DB_MAX_OVERFLOW", "10"))
        self.pool_timeout = int(os.getenv("DB_POOL_TIMEOUT", "30"))
        self.pool_recycle = int(os.getenv("DB_POOL_RECYCLE", "1800"))

    def create_engine(self):
        return create_engine(
            self.url,
            poolclass=QueuePool,
            pool_size=self.pool_size,
            max_overflow=self.max_overflow,
            pool_timeout=self.pool_timeout,
            pool_recycle=self.pool_recycle,
            pool_pre_ping=True,
            echo=False
        )

config = DatabaseConfig()
engine = config.create_engine()
```

### Async Pool Configuration

```python
# Example 3: Async connection pool
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker

async_engine = create_async_engine(
    "postgresql+asyncpg://user:pass@localhost/db",
    pool_size=20,
    max_overflow=10,
    pool_timeout=30,
    pool_recycle=1800,
    pool_pre_ping=True,
    echo=False
)

AsyncSessionLocal = sessionmaker(
    async_engine,
    class_=AsyncSession,
    expire_on_commit=False
)
```

## Pool Monitoring

### Monitoring Pool Status

```python
# Example 4: Pool monitoring
from fastapi import FastAPI
from sqlalchemy import event

app = FastAPI()

@event.listens_for(engine, "checkout")
def on_checkout(dbapi_connection, connection_record, connection_proxy):
    """Log connection checkout"""
    pool = engine.pool
    print(f"Connection checked out. Pool size: {pool.size()}")

@event.listens_for(engine, "checkin")
def on_checkin(dbapi_connection, connection_record):
    """Log connection checkin"""
    pool = engine.pool
    print(f"Connection checked in. Pool size: {pool.size()}")

@app.get("/pool-status/")
async def pool_status():
    """Get connection pool status"""
    pool = engine.pool
    return {
        "pool_size": pool.size(),
        "checked_in": pool.checkedin(),
        "checked_out": pool.checkedout(),
        "overflow": pool.overflow(),
        "invalidated": pool.invalidated()
    }
```

## Best Practices

### Pool Guidelines

```python
# Example 5: Pool best practices
"""
Connection Pool Best Practices:

1. Size pool based on workload
   Formula: pool_size = (CPU cores * 2) + effective_spindle_count

2. Set appropriate timeouts
   - pool_timeout: 30 seconds (default)
   - Don't make it too long

3. Enable pool_pre_ping
   - Detects stale connections
   - Prevents connection errors

4. Recycle connections
   - pool_recycle: 1800 (30 minutes)
   - Prevents timeout issues

5. Monitor pool usage
   - Track checked_out connections
   - Alert on pool exhaustion

6. Use NullPool for testing
   - Avoids connection leaks in tests
"""

# Recommended configuration for different environments
POOL_CONFIGS = {
    "development": {
        "pool_size": 5,
        "max_overflow": 10,
        "pool_timeout": 30,
        "pool_recycle": 1800
    },
    "staging": {
        "pool_size": 10,
        "max_overflow": 20,
        "pool_timeout": 30,
        "pool_recycle": 1800
    },
    "production": {
        "pool_size": 20,
        "max_overflow": 30,
        "pool_timeout": 30,
        "pool_recycle": 1800
    }
}
```

## Summary

| Parameter | Purpose | Recommended |
|-----------|---------|-------------|
| pool_size | Base connections | CPU cores * 2 + 1 |
| max_overflow | Extra connections | pool_size / 2 |
| pool_timeout | Wait time | 30 seconds |
| pool_recycle | Recycle interval | 1800 seconds |
| pool_pre_ping | Health check | True |

## Next Steps

Continue learning about:
- [Transaction Management](./04_transaction_management.md) - ACID transactions
- [Session Management](../02_sqlalchemy_basics/05_session_management.md) - Sessions
