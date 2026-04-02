# Eager Loading Strategies

## Overview

Eager loading optimizes database queries by loading related objects in advance, preventing the N+1 query problem.

## Loading Strategies

### joinedload

```python
# Example 1: joinedload - LEFT OUTER JOIN
from sqlalchemy.orm import joinedload

def get_users_with_profiles(db: Session):
    """Load users and profiles in single query"""
    return db.query(User).options(
        joinedload(User.profile)
    ).all()

def get_posts_with_authors(db: Session):
    """Load posts and authors together"""
    return db.query(Post).options(
        joinedload(Post.author)
    ).all()
```

### selectinload

```python
# Example 2: selectinload - IN query for collections
from sqlalchemy.orm import selectinload

def get_users_with_all_posts(db: Session):
    """Load users and all their posts efficiently"""
    return db.query(User).options(
        selectinload(User.posts)
    ).all()

def get_posts_with_all_tags(db: Session):
    """Load posts and all their tags"""
    return db.query(Post).options(
        selectinload(Post.tags)
    ).all()
```

### subqueryload

```python
# Example 3: subqueryload - Subquery for collections
from sqlalchemy.orm import subqueryload

def get_users_with_posts_subquery(db: Session):
    """Load using subquery"""
    return db.query(User).options(
        subqueryload(User.posts)
    ).all()
```

## Chaining Eager Loads

```python
# Example 4: Chained eager loading
def get_users_with_full_data(db: Session):
    """Load multiple levels of relationships"""
    return db.query(User).options(
        selectinload(User.posts).joinedload(Post.author),
        selectinload(User.profile),
        selectinload(User.roles)
    ).all()
```

## Best Practices

1. Use `selectinload` for collections
2. Use `joinedload` for many-to-one
3. Chain loads for nested relationships
4. Monitor query count in development

## Summary

Eager loading is essential for API performance. Choose the right strategy based on relationship type.

## Next Steps

Continue learning about:
- [Lazy Loading](./09_lazy_loading_strategies.md)
- [Query Optimization](../08_database_performance/01_query_optimization.md)
