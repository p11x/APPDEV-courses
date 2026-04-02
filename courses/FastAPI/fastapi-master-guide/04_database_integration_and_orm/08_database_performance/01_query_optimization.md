# Query Optimization

## Overview

Query optimization is critical for FastAPI application performance. This guide covers techniques to improve database query efficiency.

## Common Performance Issues

### N+1 Query Problem

```python
# Example 1: N+1 problem and solutions
from sqlalchemy.orm import joinedload, selectinload

# BAD: N+1 Query Problem
# 1 query for users + N queries for posts = N+1 queries
@app.get("/users/bad/")
async def get_users_bad(db: Session = Depends(get_db)):
    users = db.query(User).all()  # 1 query
    result = []
    for user in users:
        # N queries (one per user!)
        result.append({
            "id": user.id,
            "username": user.username,
            "post_count": len(user.posts)  # Triggers query
        })
    return result

# GOOD: Eager loading
@app.get("/users/good/")
async def get_users_good(db: Session = Depends(get_db)):
    users = db.query(User).options(
        selectinload(User.posts)  # Single additional query
    ).all()

    return [
        {
            "id": u.id,
            "username": u.username,
            "post_count": len(u.posts)  # Already loaded
        }
        for u in users
    ]

# BEST: Use aggregation
@app.get("/users/best/")
async def get_users_best(db: Session = Depends(get_db)):
    from sqlalchemy import func

    result = db.query(
        User.id,
        User.username,
        func.count(Post.id).label("post_count")
    ).outerjoin(Post).group_by(User.id).all()

    return [
        {"id": r.id, "username": r.username, "post_count": r.post_count}
        for r in result
    ]
```

## Indexing Strategies

### Creating Indexes

```python
# Example 2: Index optimization
from sqlalchemy import Index

class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50), unique=True, index=True)  # Single column index
    email = Column(String(100), unique=True, index=True)
    created_at = Column(DateTime, default=datetime.utcnow)

    # Composite index for common queries
    __table_args__ = (
        Index('idx_username_email', 'username', 'email'),
        Index('idx_created_active', 'created_at', 'is_active'),
    )

# Query using index
@app.get("/users/search/")
async def search_users(username: str, db: Session = Depends(get_db)):
    # Uses idx_username index
    return db.query(User).filter(User.username == username).first()
```

## Query Analysis

### EXPLAIN ANALYZE

```python
# Example 3: Query analysis
from sqlalchemy import text

def analyze_query(db: Session, query):
    """Analyze query execution plan"""
    # PostgreSQL EXPLAIN
    explain_query = f"EXPLAIN ANALYZE {query}"
    result = db.execute(text(explain_query))
    return [row[0] for row in result]

# Usage
@app.get("/debug/query-plan/")
async def get_query_plan(db: Session = Depends(get_db)):
    query = str(
        db.query(User)
        .filter(User.is_active == True)
        .order_by(User.created_at.desc())
        .limit(10)
        .statement
    )
    plan = analyze_query(db, query)
    return {"plan": plan}
```

## Pagination Optimization

### Efficient Pagination

```python
# Example 4: Pagination strategies
from sqlalchemy import func

# Offset pagination (simple but slow for large offsets)
@app.get("/users/offset/")
async def users_offset(
    page: int = 1,
    per_page: int = 20,
    db: Session = Depends(get_db)
):
    """Offset-based pagination"""
    offset = (page - 1) * per_page

    users = db.query(User).offset(offset).limit(per_page).all()
    total = db.query(func.count(User.id)).scalar()

    return {
        "items": users,
        "total": total,
        "page": page,
        "pages": (total + per_page - 1) // per_page
    }

# Cursor pagination (efficient for large datasets)
@app.get("/users/cursor/")
async def users_cursor(
    cursor: Optional[int] = None,
    limit: int = 20,
    db: Session = Depends(get_db)
):
    """Cursor-based pagination"""
    query = db.query(User).order_by(User.id)

    if cursor:
        query = query.filter(User.id > cursor)

    users = query.limit(limit).all()
    next_cursor = users[-1].id if users else None

    return {
        "items": users,
        "next_cursor": next_cursor
    }

# Keyset pagination (most efficient)
@app.get("/users/keyset/")
async def users_keyset(
    last_id: Optional[int] = None,
    last_created: Optional[datetime] = None,
    limit: int = 20,
    db: Session = Depends(get_db)
):
    """Keyset pagination"""
    query = db.query(User).order_by(User.created_at.desc(), User.id.desc())

    if last_id and last_created:
        query = query.filter(
            or_(
                User.created_at < last_created,
                and_(User.created_at == last_created, User.id < last_id)
            )
        )

    users = query.limit(limit).all()

    return {
        "items": users,
        "last_id": users[-1].id if users else None,
        "last_created": users[-1].created_at if users else None
    }
```

## Projection Optimization

### Select Only Needed Columns

```python
# Example 5: Column projection
from sqlalchemy import select

# BAD: Select all columns
@app.get("/users/all-columns/")
async def users_all(db: Session = Depends(get_db)):
    return db.query(User).all()  # SELECT * FROM users

# GOOD: Select only needed columns
@app.get("/users/projected/")
async def users_projected(db: Session = Depends(get_db)):
    return db.query(
        User.id,
        User.username,
        User.email
    ).all()  # SELECT id, username, email FROM users

# GOOD: Use schemas for projection
from pydantic import BaseModel

class UserSummary(BaseModel):
    id: int
    username: str

    class Config:
        from_attributes = True

@app.get("/users/summary/", response_model=list[UserSummary])
async def users_summary(db: Session = Depends(get_db)):
    return db.query(User.id, User.username).all()
```

## Batch Operations

### Bulk Inserts and Updates

```python
# Example 6: Bulk operations
from sqlalchemy import insert, update

# BAD: Individual inserts
@app.post("/users/bulk/bad/")
async def create_users_bad(users: list[UserCreate], db: Session = Depends(get_db)):
    for user_data in users:
        user = User(**user_data.dict())
        db.add(user)
        db.commit()  # Commit per user - SLOW!
    return {"created": len(users)}

# GOOD: Bulk insert
@app.post("/users/bulk/good/")
async def create_users_good(users: list[UserCreate], db: Session = Depends(get_db)):
    db.execute(
        insert(User),
        [user.dict() for user in users]
    )
    db.commit()  # Single commit
    return {"created": len(users)}

# GOOD: Bulk update
@app.patch("/users/bulk-update/")
async def bulk_update_users(
    user_ids: list[int],
    is_active: bool,
    db: Session = Depends(get_db)
):
    db.execute(
        update(User)
        .where(User.id.in_(user_ids))
        .values(is_active=is_active)
    )
    db.commit()
    return {"updated": len(user_ids)}
```

## Best Practices

### Query Optimization Guidelines

```python
# Example 7: Best practices
"""
Query Optimization Best Practices:

1. Use indexes for frequently queried columns
2. Avoid N+1 queries with eager loading
3. Use projection (select only needed columns)
4. Implement proper pagination
5. Use bulk operations for multiple records
6. Analyze slow queries with EXPLAIN
7. Cache frequently accessed data
8. Use connection pooling
9. Monitor query performance
10. Avoid SELECT * in production
"""

# Monitoring decorator
import time
from functools import wraps

def log_slow_queries(threshold_ms: int = 100):
    """Log queries slower than threshold"""
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            start = time.time()
            result = func(*args, **kwargs)
            duration = (time.time() - start) * 1000

            if duration > threshold_ms:
                print(f"SLOW QUERY: {func.__name__} took {duration:.2f}ms")

            return result
        return wrapper
    return decorator
```

## Summary

| Technique | Impact | Use Case |
|-----------|--------|----------|
| Eager loading | High | N+1 prevention |
| Indexing | High | Frequent queries |
| Projection | Medium | Large tables |
| Pagination | High | Large result sets |
| Bulk operations | High | Multiple records |

## Next Steps

Continue learning about:
- [Index Optimization](./02_index_optimization.md) - Index strategies
- [Caching Strategies](./04_caching_strategies.md) - Cache layers
