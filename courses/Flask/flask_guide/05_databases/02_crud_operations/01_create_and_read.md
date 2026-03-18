<!-- FILE: 05_databases/02_crud_operations/01_create_and_read.md -->

## Overview

**CRUD** (Create, Read, Update, Delete) operations are the foundation of database interactions. This file covers creating and reading data with SQLAlchemy: adding new records, querying with filters, and retrieving data efficiently.

## Prerequisites

- SQLAlchemy models defined
- Database connection configured

## Core Concepts

### Session Management

SQLAlchemy uses a session to track changes:
```python
db.session.add(obj)     # Stage changes
db.session.commit()     # Save changes
db.session.rollback()   # Undo changes
```

### Query Methods

- `Model.query.all()` — Get all records
- `Model.query.get(id)` — Get by primary key
- `Model.query.filter_by(field=value)` — Filter records
- `Model.query.first()` — Get first result

## Code Walkthrough

### Create and Read Operations

```python
# app.py — CRUD: Create and Read
from flask import Flask, jsonify
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False
db = SQLAlchemy(app)

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    email = db.Column(db.String(120), unique=True, nullable=False)
    created_at = db.Column(db.DateTime, default=db.func.now())
    
    def to_dict(self):
        return {
            "id": self.id,
            "username": self.username,
            "email": self.email,
            "created_at": self.created_at.isoformat() if self.created_at else None
        }

# Create tables
with app.app_context():
    db.create_all()

# CREATE: Add new user
@app.route("/users/create/<username>/<email>")
def create_user(username, email):
    # Check if user exists
    existing = User.query.filter_by(username=username).first()
    if existing:
        return jsonify({"error": "Username already exists"}), 400
    
    # Create new user
    user = User(username=username, email=email)
    
    # Add to session and commit
    db.session.add(user)
    db.session.commit()
    
    return jsonify(user.to_dict()), 201

# READ: Get all users
@app.route("/users")
def get_all_users():
    users = User.query.all()
    return jsonify([u.to_dict() for u in users])

# READ: Get user by ID
@app.route("/users/<int:user_id>")
def get_user(user_id):
    user = User.query.get(user_id)
    if not user:
        return jsonify({"error": "User not found"}), 404
    return jsonify(user.to_dict())

# READ: Get user by username
@app.route("/users/username/<username>")
def get_user_by_username(username):
    user = User.query.filter_by(username=username).first()
    if not user:
        return jsonify({"error": "User not found"}), 404
    return jsonify(user.to_dict())

# READ: Query with filters
@app.route("/users/search")
def search_users():
    # Get query parameters
    username = request.args.get("username")
    email = request.args.get("email")
    
    query = User.query
    
    if username:
        query = query.filter(User.username.like(f"%{username}%"))
    if email:
        query = query.filter(User.email.like(f"%{email}%"))
    
    users = query.all()
    return jsonify([u.to_dict() for u in users])

# Import request
from flask import request

if __name__ == "__main__":
    app.run(debug=True)
```

### Session Patterns

```python
# Safe session pattern
try:
    user = User(username="alice", email="alice@example.com")
    db.session.add(user)
    db.session.commit()
except Exception as e:
    db.session.rollback()  # Undo changes on error
    return str(e)
```

## Quick Reference

| Method | Description |
|--------|-------------|
| `db.session.add(obj)` | Stage object |
| `db.session.commit()` | Save to database |
| `db.session.rollback()` | Undo changes |
| `Model.query.all()` | Get all |
| `Model.query.get(id)` | Get by PK |
| `query.filter_by(k=v)` | Filter |

## Next Steps

Now you can create and read data. Continue to [02_update_and_delete.md](02_update_and_delete.md) to learn updating and deleting records.