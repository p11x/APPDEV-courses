<!-- FILE: 12_caching_and_performance/04_database_performance/03_database_indexing_basics.md -->

## Overview

Database indexes dramatically improve query performance. This file explains how to create and use indexes in SQLAlchemy.

## Code Walkthrough

```python
# indexing.py
from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()

# ============================================
# Creating Indexes
# ============================================

class User(db.Model):
    __tablename__ = "users"
    
    id = db.Column(db.Integer, primary_key=True)
    email = db.Column(db.String(120), unique=True, index=True)  # Index
    name = db.Column(db.String(100))
    created_at = db.Column(db.DateTime)
    
    # Composite index
    __table_args__ = (
        db.Index("idx_name_created", "name", "created_at"),
    )

class Post(db.Model):
    __tablename__ = "posts"
    
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey("users.id"), index=True)
    title = db.Column(db.String(200))
    published = db.Column(db.Boolean, default=False, index=True)
    created_at = db.Column(db.DateTime, index=True)
    
    __table_args__ = (
        db.Index("idx_user_published", "user_id", "published"),
    )

# ============================================
# Query using indexes
# ============================================

# These queries will use the indexes:
# - User.query.filter_by(email="alice@example.com")
# - Post.query.filter_by(user_id=1, published=True)
# - Post.query.order_by(Post.created_at.desc())

# ============================================
# Check if index is used
# ============================================

# Enable query logging
import logging
logging.basicConfig()
logging.getLogger("sqlalchemy.engine").setLevel(logging.INFO)

# Then run your queries and check the logs
# Look for "Using index" in the output

if __name__ == "__main__":
    pass
```

## Quick Reference

| Index Type | Use Case |
|------------|----------|
| Single column | WHERE clause on that column |
| Composite | Multiple columns in WHERE |
| Unique | Enforce uniqueness + fast lookup |

## Next Steps

Continue to [05_profiling_and_optimization/01_profiling_flask_apps.md](../05_profiling_and_optimization/01_profiling_flask_apps.md)
