<!-- FILE: 05_databases/01_sqlalchemy_basics/03_defining_models.md -->

## Overview

**Models** define your database tables as Python classes. Flask-SQLAlchemy provides a declarative base that lets you define tables, columns, relationships, and constraints. This file covers creating comprehensive models with all common field types and relationships.

## Prerequisites

- Flask-SQLAlchemy installed
- Basic understanding of database tables

## Core Concepts

### Column Types

| SQLAlchemy Type | Description |
|-----------------|-------------|
| `Integer` | Integer number |
| `String(n)` | Variable-length string |
| `Text` | Long text |
| `Boolean` | True/False |
| `DateTime` | Date and time |
| `Float` | Floating point |
| `Numeric` | Fixed precision |

### Column Options

- `primary_key=True` — Primary key
- `unique=True` — Unique constraint
- `nullable=False` — NOT NULL
- `default=value` — Default value
- `index=True` — Create index

## Code Walkthrough

### Complete Model Examples

```python
# models.py — Database models
from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False
db = SQLAlchemy(app)

class User(db.Model):
    """User model with all common field types."""
    
    # Primary key
    id = db.Column(db.Integer, primary_key=True)
    
    # String fields
    username = db.Column(db.String(80), unique=True, nullable=False, index=True)
    email = db.Column(db.String(120), unique=True, nullable=False)
    bio = db.Column(db.Text)  # Unlimited text
    
    # Boolean
    is_active = db.Column(db.Boolean, default=True)
    is_admin = db.Column(db.Boolean, default=False)
    
    # Numeric
    balance = db.Column(db.Numeric(10, 2), default=0)
    
    # DateTime
    created_at = db.Column(db.DateTime, default=datetime.utcnow)
    updated_at = db.Column(db.DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # String with choices
    role = db.Column(db.String(20), default="user")
    
    # Relationships
    posts = db.relationship("Post", backref="author", lazy=True)
    
    def __repr__(self):
        return f"<User {self.username}>"
    
    def to_dict(self):
        """Convert user to dictionary."""
        return {
            "id": self.id,
            "username": self.username,
            "email": self.email,
            "is_active": self.is_active,
            "created_at": self.created_at.isoformat() if self.created_at else None
        }

class Post(db.Model):
    """Blog post model."""
    
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(200), nullable=False)
    content = db.Column(db.Text, nullable=False)
    slug = db.Column(db.String(200), unique=True)
    published = db.Column(db.Boolean, default=False)
    created_at = db.Column(db.DateTime, default=datetime.utcnow)
    
    # Foreign key
    user_id = db.Column(db.Integer, db.ForeignKey("user.id"), nullable=False)
    
    def __repr__(self):
        return f"<Post {self.title}>"

# Create all tables
with app.app_context():
    db.create_all()
```

### Relationships

```python
# One-to-Many
class Parent(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    children = db.relationship("Child", backref="parent", lazy=True)

class Child(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    parent_id = db.Column(db.Integer, db.ForeignKey("parent.id"))

# Many-to-Many
tags = db.Table("post_tags",
    db.Column("post_id", db.Integer, db.ForeignKey("post.id")),
    db.Column("tag_id", db.Integer, db.ForeignKey("tag.id"))
)

class Post(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    tags = db.relationship("Tag", secondary=tags, backref="posts")

class Tag(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(50))
```

## Common Mistakes

❌ **Forgetting nullable=False**
```python
# WRONG — Allows NULL values
username = db.Column(db.String(80))
```

✅ **Correct — Specify nullable**
```python
# CORRECT
username = db.Column(db.String(80), nullable=False)
```

## Quick Reference

| Type | Use For |
|------|---------|
| `Integer` | IDs, counts |
| `String(n)` | Short text |
| `Text` | Long text |
| `Boolean` | Flags |
| `DateTime` | Timestamps |
| `Numeric` | Money |

## Next Steps

Now you can define models. Continue to [01_create_and_read.md](../02_crud_operations/01_create_and_read.md) to learn CRUD operations.