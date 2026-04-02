# Views and Indexes

## Overview

Database views and indexes optimize query performance and simplify complex queries.

## Database Views

### Creating Views

```python
# Example 1: SQLAlchemy views
from sqlalchemy import Column, Integer, String, select
from sqlalchemy.orm import column_property

class UserStats(Base):
    """Materialized view for user statistics"""
    __tablename__ = "user_stats"

    user_id = Column(Integer, primary_key=True)
    username = Column(String(50))
    post_count = Column(Integer)
    comment_count = Column(Integer)

# Create view with raw SQL
def create_user_stats_view(db: Session):
    db.execute("""
        CREATE OR REPLACE VIEW user_stats AS
        SELECT
            u.id as user_id,
            u.username,
            COUNT(DISTINCT p.id) as post_count,
            COUNT(DISTINCT c.id) as comment_count
        FROM users u
        LEFT JOIN posts p ON u.id = p.author_id
        LEFT JOIN comments c ON u.id = c.author_id
        GROUP BY u.id, u.username
    """)
    db.commit()
```

## Index Management

### Creating Indexes

```python
# Example 2: Index management
from sqlalchemy import Index

class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50))
    email = Column(String(100))
    created_at = Column(DateTime)

    # Single column index
    __table_args__ = (
        Index('ix_users_username', 'username'),
        Index('ix_users_email', 'email'),
        Index('ix_users_created', 'created_at'),
        # Composite index
        Index('ix_users_username_email', 'username', 'email'),
    )
```

## Summary

Views simplify complex queries, indexes improve performance.

## Next Steps

Continue learning about:
- [Index Optimization](../../08_database_performance/02_index_optimization.md)
- [Query Plan Analysis](../../08_database_performance/03_query_plan_analysis.md)
