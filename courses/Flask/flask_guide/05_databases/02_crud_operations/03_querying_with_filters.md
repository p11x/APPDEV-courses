<!-- FILE: 05_databases/02_crud_operations/03_querying_with_filters.md -->

## Overview

Advanced querying lets you filter, sort, and paginate database records efficiently. SQLAlchemy provides a powerful query API for complex data retrieval.

## Prerequisites

- Basic CRUD operations
- SQLAlchemy models

## Code Walkthrough

### Advanced Query Examples

```python
# app.py — Advanced queries
from flask import Flask, jsonify, request
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False
db = SQLAlchemy(app)

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True)
    email = db.Column(db.String(120))
    age = db.Column(db.Integer)
    is_active = db.Column(db.Boolean, default=True)
    created_at = db.Column(db.DateTime)

# Filter methods
@app.route("/users/filter")
def filter_users():
    # filter_by (keyword arguments)
    active = User.query.filter_by(is_active=True).all()
    
    # filter (SQLAlchemy expressions)
    adults = User.query.filter(User.age >= 18).all()
    
    # Multiple filters
    active_adults = User.query.filter(
        User.is_active == True,
        User.age >= 18
    ).all()
    
    # LIKE queries
    starts_with_a = User.query.filter(User.username.like("A%")).all()
    
    # IN queries
    specific = User.query.filter(User.id.in_([1, 2, 3])).all()
    
    # Ordering
    by_name = User.query.order_by(User.username).all()
    by_name_desc = User.query.order_by(User.username.desc()).all()
    
    # Limiting
    top_5 = User.query.order_by(User.id.desc()).limit(5).all()
    
    # Pagination
    page = request.args.get("page", 1, type=int)
    per_page = 10
    paginated = User.query.paginate(page=page, per_page=per_page)
    
    return jsonify({
        "total": paginated.total,
        "page": page,
        "items": [{"id": u.id, "username": u.username} for u in paginated.items]
    })

# Counting
@app.route("/users/count")
def count_users():
    total = User.query.count()
    active = User.query.filter_by(is_active=True).count()
    return jsonify({"total": total, "active": active})

if __name__ == "__main__":
    app.run(debug=True)
```

## Quick Reference

| Method | Description |
|--------|-------------|
| `filter_by(k=v)` | Simple filter |
| `filter(Model.field.op())` | Complex filter |
| `order_by()` | Sort results |
| `limit(n)` | Limit results |
| `offset(n)` | Skip results |
| `paginate()` | Pagination |

## Next Steps

Now you can query effectively. Continue to [01_flask_migrate_setup.md](../03_migrations/01_flask_migrate_setup.md) to learn database migrations.