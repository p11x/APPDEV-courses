<!-- FILE: 18_rate_limiting_and_security/01_security_fundamentals/03_sql_injection_prevention.md -->

## Overview

Prevent SQL injection attacks in Flask applications using SQLAlchemy.

## Prerequisites

- Understanding of Flask-SQLAlchemy
- Basic database knowledge

## Core Concepts

SQL injection occurs when attacker inserts malicious SQL code through user input. SQLAlchemy prevents this by using parameterized queries automatically.

## The Problem

**Attack scenario:**
```bash
# Attacker enters this as username:
' OR '1'='1

# If you build query like this:
query = f"SELECT * FROM users WHERE name = '{username}'"
# This becomes: SELECT * FROM users WHERE name = '' OR '1'='1'
# Returns ALL users!
```

## The Solution: Always Use ORM

```python
# models.py
from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    email = db.Column(db.String(120), nullable=False)
    
    def __repr__(self):
        return f'<User {self.username}>'
```

### Safe Query Methods

```python
# ❌ NEVER - Vulnerable to SQL injection
def get_user_vulnerable(username):
    query = f"SELECT * FROM users WHERE username = '{username}'"
    result = db.session.execute(query)
    return result.fetchone()

# ✅ ALWAYS - Safe using SQLAlchemy
def get_user_safe(username):
    return User.query.filter_by(username=username).first()

# ✅ Using filter() - also safe
def search_users(term):
    return User.query.filter(User.username.like(f'%{term}%')).all()

# ✅ Using parameterized raw query - safe
def get_user_parameterized(username):
    result = db.session.execute(
        text("SELECT * FROM users WHERE username = :username"),
        {"username": username}
    )
    return result.fetchone()
```

### Safe Insert/Update

```python
# ✅ Create new user - safe
def create_user(username, email):
    user = User(username=username, email=email)
    db.session.add(user)
    db.session.commit()
    return user

# ✅ Update user - safe
def update_email(user_id, new_email):
    user = User.query.get(user_id)
    if user:
        user.email = new_email
        db.session.commit()

# ✅ Delete user - safe
def delete_user(user_id):
    user = User.query.get(user_id)
    if user:
        db.session.delete(user)
        db.session.commit()
```

### Working with Raw SQL Safely

```python
# If you must use raw SQL
from sqlalchemy import text

def safe_raw_query(query, params=None):
    """Execute raw SQL safely with parameterized queries."""
    if params is None:
        params = {}
    
    # This is SAFE because parameters are passed separately
    result = db.session.execute(text(query), params)
    return result.fetchall()

# Usage
# ✅ Safe - parameterization
users = safe_raw_query(
    "SELECT * FROM users WHERE username = :username",
    {"username": user_input}
)

# ❌ NEVER do this - SQL injection vulnerability!
users = safe_raw_query(
    f"SELECT * FROM users WHERE username = '{user_input}'"
)
```

> **🔒 Security Note:** Never concatenate user input into SQL strings. Always use ORM or parameterized queries.

## Common Mistakes

- ❌ Building SQL with f-strings
- ✅ Use SQLAlchemy ORM methods

- ❌ Using string formatting in queries
- ✅ Use parameterized queries with `:param` syntax

- ❌ Trusting user input
- ✅ Always validate before database operations

## Quick Reference

| Method | Safe? |
|--------|-------|
| `User.query.filter_by(field=value)` | ✅ Yes |
| `User.query.filter(User.field.like(...))` | ✅ Yes |
| `db.session.execute(text(sql), params)` | ✅ Yes |
| `f"SELECT * FROM {table}"` | ❌ No |
| `f"WHERE field = '{value}'"` | ❌ No |

## Next Steps

Continue to [02_rate_limiting/01_what_is_rate_limiting.md](../02_rate_limiting/01_what_is_rate_limiting.md) to learn about rate limiting.
