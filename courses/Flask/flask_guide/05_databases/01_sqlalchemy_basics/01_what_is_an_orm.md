<!-- FILE: 05_databases/01_sqlalchemy_basics/01_what_is_an_orm.md -->

## Overview

Modern web applications need to store and retrieve data. **SQLAlchemy** is Python's most popular ORM (Object-Relational Mapper), and Flask-SQLAlchemy integrates it with Flask. This file explains what an ORM is, why it's useful, and how it fits into Flask applications.

## Prerequisites

- Basic Python knowledge
- Understanding of database concepts (tables, rows, SQL)

## Core Concepts

### What is an ORM?

An **ORM** maps Python objects to database tables:
- Classes → Tables
- Objects → Rows
- Attributes → Columns

This lets you work with Python instead of SQL.

### SQL vs ORM Comparison

**SQL:**
```sql
SELECT * FROM users WHERE id = 1;
INSERT INTO users (name, email) VALUES ('Alice', 'alice@example.com');
```

**ORM:**
```python
user = User.query.get(1)
user = User(name="Alice", email="alice@example.com")
db.session.add(user)
```

### Why Use ORM?

- **Portability** — Switch databases without changing code
- **Security** — Built-in SQL injection protection
- **Productivity** — Work with familiar Python syntax
- **Maintainability** — Clear, readable code

## Code Walkthrough

### Conceptual Example

```python
# Without ORM (raw SQL)
cursor.execute("SELECT * FROM users WHERE email = ?", (email,))
row = cursor.fetchone()
if row and row[2] == password_hash:
    login_user(row[0])

# With ORM (SQLAlchemy)
user = User.query.filter_by(email=email).first()
if user and user.check_password(password):
    login_user(user)
```

### Flask-SQLAlchemy Setup

```python
# app.py — Basic SQLAlchemy setup
from flask import Flask
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)

# Configure database
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

# Initialize extension
db = SQLAlchemy(app)

# Define model
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    email = db.Column(db.String(120), unique=True, nullable=False)
    
    def __repr__(self):
        return f"<User {self.username}>"

# Create tables
with app.app_context():
    db.create_all()

# Use in routes
@app.route("/users")
def list_users():
    users = User.query.all()
    return {"users": [{"id": u.id, "username": u.username} for u in users]}
```

## Quick Reference

| Concept | Description |
|---------|-------------|
| `db.Model` | Base class for models |
| `db.Column` | Define table column |
| `db.Integer`, `db.String` | Column types |
| `primary_key=True` | Primary key column |
| `unique=True` | Unique constraint |
| `nullable=False` | NOT NULL |

## Next Steps

Now you understand ORM. Continue to [02_installing_flask_sqlalchemy.md](02_installing_flask_sqlalchemy.md) to install and configure Flask-SQLAlchemy.