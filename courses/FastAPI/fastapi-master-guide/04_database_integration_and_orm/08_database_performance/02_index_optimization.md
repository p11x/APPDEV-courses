# Index Optimization

## Overview

Proper indexing is crucial for database query performance. This guide covers index strategies for FastAPI applications with SQLAlchemy.

## Index Types

### Basic Index Types

```python
# Example 1: SQLAlchemy index types
from sqlalchemy import Column, Integer, String, Index, UniqueConstraint
from sqlalchemy.orm import DeclarativeBase

class Base(DeclarativeBase):
    pass

class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50), unique=True)  # Unique index
    email = Column(String(100), index=True)     # Regular index
    created_at = Column(DateTime, index=True)   # Index for sorting

    # Composite index
    __table_args__ = (
        Index('idx_username_email', 'username', 'email'),
        Index('idx_created_active', 'created_at', 'is_active'),
    )

class Product(Base):
    __tablename__ = "products"

    id = Column(Integer, primary_key=True)
    name = Column(String(200), index=True)
    category = Column(String(50))
    price = Column(Float)

    # Partial index (PostgreSQL)
    __table_args__ = (
        Index(
            'idx_expensive_products',
            'price',
            postgresql_where=Column('price') > 100
        ),
    )
```

## Index Strategies

### Query-Based Indexing

```python
# Example 2: Index design for common queries
from sqlalchemy import Index

class Order(Base):
    __tablename__ = "orders"

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey("users.id"))
    status = Column(String(20))
    created_at = Column(DateTime)
    total = Column(Float)

    # Index for user's orders query
    __table_args__ = (
        Index('idx_user_orders', 'user_id', 'created_at'),
        Index('idx_status_created', 'status', 'created_at'),
        Index('idx_user_status', 'user_id', 'status'),
    )

# Query patterns that benefit from indexes:
# 1. SELECT * FROM orders WHERE user_id = ? ORDER BY created_at
# 2. SELECT * FROM orders WHERE status = ? ORDER BY created_at
# 3. SELECT * FROM orders WHERE user_id = ? AND status = ?
```

## Index Monitoring

```python
# Example 3: Monitor index usage
from sqlalchemy import text

async def analyze_index_usage(db: Session):
    """Analyze index usage statistics"""
    query = text("""
        SELECT
            schemaname,
            tablename,
            indexname,
            idx_scan as index_scans,
            idx_tup_read as tuples_read,
            idx_tup_fetch as tuples_fetched
        FROM pg_stat_user_indexes
        ORDER BY idx_scan DESC
    """)
    result = db.execute(query)
    return result.fetchall()

async def find_unused_indexes(db: Session):
    """Find indexes that are never used"""
    query = text("""
        SELECT
            indexname,
            tablename,
            idx_scan
        FROM pg_stat_user_indexes
        WHERE idx_scan = 0
        AND indexname NOT LIKE '%pkey%'
    """)
    return db.execute(query).fetchall()
```

## Best Practices

1. Index columns used in WHERE, ORDER, JOIN
2. Avoid over-indexing (slows writes)
3. Monitor and remove unused indexes
4. Use composite indexes wisely

## Summary

Proper indexing is essential for query performance. Monitor usage and optimize based on query patterns.

## Next Steps

Continue learning about:
- [Query Plan Analysis](./03_query_plan_analysis.md)
- [Caching Strategies](./04_caching_strategies.md)
