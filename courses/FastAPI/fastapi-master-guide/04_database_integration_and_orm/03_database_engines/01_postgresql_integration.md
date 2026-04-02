# PostgreSQL Integration

## Overview

PostgreSQL is the recommended database for FastAPI applications. This guide covers setup, configuration, and optimization.

## Installation and Setup

### Basic Setup

```python
# Example 1: PostgreSQL connection setup
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession

# Sync connection
SYNC_DATABASE_URL = "postgresql://user:password@localhost:5432/fastapi_db"
sync_engine = create_engine(SYNC_DATABASE_URL)
SyncSession = sessionmaker(bind=sync_engine)

# Async connection (recommended for FastAPI)
ASYNC_DATABASE_URL = "postgresql+asyncpg://user:password@localhost:5432/fastapi_db"
async_engine = create_async_engine(
    ASYNC_DATABASE_URL,
    echo=False,
    pool_size=20,
    max_overflow=10
)
AsyncSession = sessionmaker(
    async_engine,
    class_=AsyncSession,
    expire_on_commit=False
)
```

### Environment Configuration

```python
# Example 2: Environment-based configuration
import os
from pydantic_settings import BaseSettings

class DatabaseSettings(BaseSettings):
    """Database configuration from environment"""
    DB_HOST: str = "localhost"
    DB_PORT: int = 5432
    DB_USER: str = "postgres"
    DB_PASSWORD: str = "password"
    DB_NAME: str = "fastapi_db"
    DB_SSL_MODE: str = "prefer"

    @property
    def async_url(self) -> str:
        return (
            f"postgresql+asyncpg://{self.DB_USER}:{self.DB_PASSWORD}"
            f"@{self.DB_HOST}:{self.DB_PORT}/{self.DB_NAME}"
            f"?ssl={self.DB_SSL_MODE}"
        )

    @property
    def sync_url(self) -> str:
        return (
            f"postgresql://{self.DB_USER}:{self.DB_PASSWORD}"
            f"@{self.DB_HOST}:{self.DB_PORT}/{self.DB_NAME}"
            f"?sslmode={self.DB_SSL_MODE}"
        )

db_settings = DatabaseSettings()
```

## PostgreSQL-Specific Features

### Advanced Features

```python
# Example 3: PostgreSQL-specific features
from sqlalchemy import Column, Integer, String, JSON, ARRAY, DateTime
from sqlalchemy.dialects.postgresql import JSONB, UUID, INET, JSONB
import uuid

class PostgreSQLModels:
    """PostgreSQL-specific column types"""

    # JSONB for efficient JSON storage
    metadata = Column(JSONB)

    # UUID type
    id = Column(UUID(as_uuid=True), primary_key=True, default=uuid.uuid4)

    # Array type
    tags = Column(ARRAY(String))

    # Full-text search
    from sqlalchemy import Index, func

    __table_args__ = (
        Index(
            'idx_search',
            func.to_tsvector('english', title + ' ' + content),
            postgresql_using='gin'
        ),
    )

# PostgreSQL functions
from sqlalchemy import text

async def use_postgres_functions(session: AsyncSession):
    """Use PostgreSQL-specific functions"""

    # JSON operations
    result = await session.execute(
        text("SELECT metadata->>'key' FROM users WHERE id = :id"),
        {"id": 1}
    )

    # Array operations
    result = await session.execute(
        text("SELECT * FROM posts WHERE :tag = ANY(tags)"),
        {"tag": "python"}
    )

    # Full-text search
    result = await session.execute(
        text("""
            SELECT *, ts_rank(to_tsvector('english', content), query) as rank
            FROM posts, to_tsquery('english', :query) query
            WHERE to_tsvector('english', content) @@ query
            ORDER BY rank DESC
        """),
        {"query": "fastapi & python"}
    )
```

## Performance Tuning

### Connection Pool Configuration

```python
# Example 4: PostgreSQL performance tuning
from sqlalchemy import create_engine
from sqlalchemy.pool import QueuePool

engine = create_engine(
    DATABASE_URL,
    poolclass=QueuePool,
    pool_size=20,              # Base pool size
    max_overflow=10,           # Extra connections when pool full
    pool_timeout=30,           # Wait time for connection
    pool_recycle=1800,         # Recycle connections after 30 min
    pool_pre_ping=True,        # Check connection health
    echo=False
)

# PostgreSQL-specific optimizations
async_engine = create_async_engine(
    ASYNC_DATABASE_URL,
    pool_size=20,
    max_overflow=10,
    pool_timeout=30,
    pool_recycle=1800,
    connect_args={
        "server_settings": {
            "application_name": "fastapi_app",
            "jit": "off"  # Disable JIT for simple queries
        }
    }
)
```

## Security Configuration

### SSL and Authentication

```python
# Example 5: PostgreSQL security
import ssl

# SSL configuration
ssl_context = ssl.create_default_context()
ssl_context.check_hostname = False
ssl_context.verify_mode = ssl.CERT_REQUIRED

engine = create_engine(
    DATABASE_URL,
    connect_args={
        "ssl": ssl_context,
        "sslmode": "verify-full",
        "sslrootcert": "/path/to/root.crt"
    }
)

# Connection string with SSL
DATABASE_URL = (
    "postgresql://user:pass@host/db"
    "?sslmode=verify-full"
    "&sslrootcert=/path/to/ca.crt"
    "&sslcert=/path/to/client.crt"
    "&sslkey=/path/to/client.key"
)
```

## Best Practices

### PostgreSQL Guidelines

```python
# Example 6: PostgreSQL best practices
"""
PostgreSQL Best Practices:

1. Use connection pooling (pool_size based on CPU cores * 2 + 1)
2. Enable SSL for production
3. Use prepared statements for repeated queries
4. Create indexes on frequently queried columns
5. Use EXPLAIN ANALYZE for query optimization
6. Set appropriate work_mem and shared_buffers
7. Use JSONB for flexible document storage
8. Implement proper backup strategies
"""

# Recommended PostgreSQL configuration
POSTGRESQL_CONFIG = {
    "shared_buffers": "256MB",
    "effective_cache_size": "768MB",
    "work_mem": "4MB",
    "maintenance_work_mem": "64MB",
    "max_connections": 100,
    "wal_level": "replica",
    "max_wal_size": "1GB"
}
```

## Summary

| Feature | Configuration | Purpose |
|---------|---------------|---------|
| Connection | `postgresql+asyncpg://` | Async driver |
| Pool | `pool_size=20` | Connection reuse |
| SSL | `sslmode=require` | Security |
| JSONB | `Column(JSONB)` | Document storage |
| Arrays | `ARRAY(String)` | List storage |

## Next Steps

Continue learning about:
- [MySQL Integration](./02_mysql_integration.md) - MySQL setup
- [SQLite Integration](./03_sqlite_integration.md) - Development setup
- [Connection String Security](./05_connection_string_security.md) - Security
