<!-- FILE: 12_caching_and_performance/04_database_performance/02_eager_loading_with_sqlalchemy.md -->

## Overview

This file provides detailed examples of eager loading strategies in SQLAlchemy.

## Code Walkthrough

```python
# eager_loading.py
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy.orm import joinedload, selectinload, subqueryload

db = SQLAlchemy()

# ============================================
# Method 1: joinedload (LEFT OUTER JOIN)
# ============================================
# Good for: One-to-one, Many-to-one (few results)

users = User.query.options(
    joinedload(User.profile),
    joinedload(User.posts)
).all()

# Generates: SELECT ... FROM users LEFT JOIN profiles ... LEFT JOIN posts ...

# ============================================
# Method 2: selectinload (IN query)
# ============================================
# Good for: One-to-many, Many-to-many (many results)

users = User.query.options(
    selectinload(User.posts),
    selectinload(User.comments)
).all()

# Generates: 
# 1. SELECT ... FROM users
# 2. SELECT ... FROM posts WHERE user_id IN (1, 2, 3, ...)

# ============================================
# Method 3: subqueryload (subquery)
# ============================================
# Good for: When you need distinct results

users = User.query.options(
    subqueryload(User.posts)
).all()

# ============================================
# Default eager loading in model
# ============================================

class User(db.Model):
    __tablename__ = "users"
    
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(100))
    
    # Always load posts with user
    posts = db.relationship(
        "Post",
        back_populates="user",
        lazy="selectin"  # or "joined", "subquery", "select"
    )

# ============================================
# Query-time eager loading
# ============================================

@app.route("/users")
def get_users():
    # Override default
    users = User.query.options(
        joinedload(User.posts)  # Override lazy loading
    ).all()
    
    return {"users": [u.name for u in users]}

if __name__ == "__main__":
    pass
```

## Next Steps

Continue to [03_database_indexing_basics.md](03_database_indexing_basics.md)
