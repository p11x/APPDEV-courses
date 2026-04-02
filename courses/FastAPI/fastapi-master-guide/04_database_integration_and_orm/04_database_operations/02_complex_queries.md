# Complex Queries

## Overview

SQLAlchemy supports complex queries including joins, subqueries, aggregations, and window functions.

## Join Operations

### Various Join Types

```python
# Example 1: Join operations
from sqlalchemy import select, join
from sqlalchemy.orm import Session

def get_users_with_posts(db: Session):
    """Inner join - users with posts"""
    return db.query(User).join(Post).all()

def get_all_users_with_post_count(db: Session):
    """Left join with aggregation"""
    from sqlalchemy import func

    return db.query(
        User.id,
        User.username,
        func.count(Post.id).label("post_count")
    ).outerjoin(Post).group_by(User.id).all()

def get_posts_with_authors(db: Session):
    """Join with specific columns"""
    return db.query(
        Post.title,
        Post.content,
        User.username.label("author")
    ).join(User, Post.author_id == User.id).all()
```

## Subqueries

### Using Subqueries

```python
# Example 2: Subquery patterns
from sqlalchemy import select, subquery

def get_users_with_many_posts(db: Session, min_posts: int = 5):
    """Subquery to filter users"""
    # Subquery: count posts per user
    post_counts = db.query(
        Post.author_id,
        func.count(Post.id).label("count")
    ).group_by(Post.author_id).subquery()

    # Main query: users with more than min_posts
    return db.query(User).join(
        post_counts,
        User.id == post_counts.c.author_id
    ).filter(post_counts.c.count >= min_posts).all()
```

## Aggregations

### Group By and Having

```python
# Example 3: Aggregation queries
from sqlalchemy import func

def get_category_stats(db: Session):
    """Group by with aggregation"""
    return db.query(
        Product.category,
        func.count(Product.id).label("count"),
        func.avg(Product.price).label("avg_price"),
        func.min(Product.price).label("min_price"),
        func.max(Product.price).label("max_price")
    ).group_by(Product.category).all()

def get_categories_with_many_products(db: Session, min_count: int = 10):
    """Having clause"""
    return db.query(
        Product.category,
        func.count(Product.id).label("count")
    ).group_by(Product.category).having(
        func.count(Product.id) >= min_count
    ).all()
```

## Window Functions

```python
# Example 4: Window functions
from sqlalchemy import over

def get_ranked_products(db: Session):
    """Rank products by price within category"""
    return db.query(
        Product.name,
        Product.category,
        Product.price,
        func.rank().over(
            partition_by=Product.category,
            order_by=Product.price.desc()
        ).label("rank")
    ).all()
```

## Summary

SQLAlchemy supports all SQL query patterns including joins, subqueries, and window functions.

## Next Steps

Continue learning about:
- [Bulk Operations](./03_bulk_operations.md)
- [Views and Indexes](./06_views_and_indexes.md)
