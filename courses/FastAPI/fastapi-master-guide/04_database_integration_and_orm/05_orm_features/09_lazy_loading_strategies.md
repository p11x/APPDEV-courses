# Lazy Loading Strategies

## Overview

Lazy loading defers loading related objects until they are accessed, reducing initial query overhead.

## Implementation

### Default Lazy Loading

```python
# Example 1: Lazy loading behavior
from sqlalchemy.orm import relationship

class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50))

    # Lazy loading (default)
    posts = relationship("Post", lazy="select")

# Lazy loading in action
user = db.query(User).first()  # Query 1: Get user
posts = user.posts  # Query 2: Get posts (lazy loaded)
```

### Lazy Loading Options

```python
# Example 2: Different lazy loading strategies
class User(Base):
    # select: Default, loads on access
    posts = relationship("Post", lazy="select")

    # selectin: Single IN query for collections
    comments = relationship("Comment", lazy="selectin")

    # joined: LEFT OUTER JOIN
    profile = relationship("Profile", lazy="joined")

    # dynamic: Returns query object
    recent_posts = relationship("Post", lazy="dynamic")
```

### Dynamic Lazy Loading

```python
# Example 3: Dynamic relationship
class User(Base):
    # Returns query instead of list
    posts = relationship("Post", lazy="dynamic")

# Usage
user = db.query(User).first()

# Get all posts
all_posts = user.posts.all()

# Filter posts
recent = user.posts.filter(Post.created_at > yesterday).all()

# Paginate posts
page = user.posts.offset(0).limit(10).all()
```

## N+1 Problem

```python
# Example 4: Avoiding N+1 queries
# BAD: N+1 queries
users = db.query(User).all()
for user in users:
    print(user.posts)  # Query for each user!

# GOOD: Eager loading
users = db.query(User).options(selectinload(User.posts)).all()
for user in users:
    print(user.posts)  # No additional queries
```

## Summary

Use lazy loading wisely to balance performance and memory usage.

## Next Steps

Continue learning about:
- [Eager Loading](./08_eager_loading_strategies.md)
- [Query Optimization](../../08_database_performance/01_query_optimization.md)
