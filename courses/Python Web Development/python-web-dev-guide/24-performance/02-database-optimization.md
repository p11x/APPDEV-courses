# Database Optimization

## What You'll Learn
- Query optimization
- Indexes
- N+1 problem

## Prerequisites
- Completed performance basics

## Avoiding N+1

```python
# BAD: N+1 queries
users = db.query(User).all()
for user in users:
    print(user.posts)  # Each user triggers a new query!

# GOOD: Eager loading
from sqlalchemy.orm import joinedload

users = db.query(User).options(joinedload(User.posts)).all()
for user in users:
    print(user.posts)  # Already loaded!
```

## Using Indexes

```python
from sqlalchemy import Index

class User(Base):
    __tablename__ = "users"
    
    id = Column(Integer, primary_key=True)
    email = Column(String, index=True)  # Index for lookups
    name = Column(String)
    
    __table_args__ = (
        Index('idx_email_name', 'email', 'name'),  # Composite index
    )
```

## Query Optimization

```python
# BAD: Load all columns
user = db.query(User).first()

# GOOD: Select specific columns
user = db.query(User.email).first()

# Use pagination
users = db.query(User).limit(20).offset(0).all()
```

## Summary
- Use eager loading to avoid N+1
- Add indexes for frequently queried columns
- Select only needed columns

## Next Steps
→ Continue to `03-caching-for-performance.md`
