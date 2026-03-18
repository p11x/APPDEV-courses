<!-- FILE: 12_caching_and_performance/04_database_performance/01_n_plus_one_problem.md -->

## Overview

The N+1 query problem is a common performance issue in Flask applications using SQLAlchemy. This file explains what it is, how it happens, and how to fix it.

## Core Concepts

The N+1 problem occurs when loading related objects:
- 1 query to get the parent objects
- N queries to get related objects (one per parent)

Example: Loading 100 users with their posts = 101 queries!

## Code Walkthrough

```python
# n_plus_one.py
from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy.orm import joinedload

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///test.db"
db = SQLAlchemy(app)

# Models
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(100))
    posts = db.relationship("Post", back_populates="user")

class Post(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(200))
    user_id = db.Column(db.Integer, db.ForeignKey("user.id"))
    user = db.relationship("User", back_populates="posts")

# ============================================
# PROBLEM: N+1 Queries
# ============================================

@app.route("/users-slow")
def get_users_slow():
    """SLOW: N+1 problem - 1 + N queries"""
    users = User.query.all()  # 1 query
    
    result = []
    for user in users:
        # N more queries - one per user!
        for post in user.posts:
            result.append({"user": user.name, "post": post.title})
    
    return {"count": len(result)}

# ============================================
# SOLUTION 1: Eager Loading
# ============================================

@app.route("/users-fast")
def get_users_fast():
    """FAST: Eager loading - only 2 queries"""
    users = User.query.options(joinedload(User.posts)).all()
    
    result = []
    for user in users:
        for post in user.posts:
            result.append({"user": user.name, "post": post.title})
    
    return {"count": len(result)}

# ============================================
# SOLUTION 2: Subquery Loading  
# ============================================

@app.route("/users-subquery")
def get_users_subquery():
    """FAST: Subquery loading"""
    users = User.query.options(
        db.subqueryload(User.posts)
    ).all()
    
    return {"count": len(users)}

if __name__ == "__main__":
    app.run(debug=True)
```

## Next Steps

Continue to [02_eager_loading_with_sqlalchemy.md](02_eager_loading_with_sqlalchemy.md)
