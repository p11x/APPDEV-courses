<!-- FILE: 05_databases/02_crud_operations/02_update_and_delete.md -->

## Overview

This file covers updating and deleting database records with SQLAlchemy. Learn how to modify existing records and remove them from the database safely.

## Prerequisites

- Create and read operations knowledge

## Code Walkthrough

### Update and Delete

```python
# app.py — CRUD: Update and Delete
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
    is_active = db.Column(db.Boolean, default=True)
    
    def to_dict(self):
        return {"id": self.id, "username": self.username, "email": self.email, "is_active": self.is_active}

with app.app_context():
    db.create_all()

# UPDATE: Update single field
@app.route("/users/<int:user_id>/update", methods=["PUT"])
def update_user(user_id):
    user = User.query.get(user_id)
    if not user:
        return jsonify({"error": "User not found"}), 404
    
    data = request.get_json()
    
    if "username" in data:
        user.username = data["username"]
    if "email" in data:
        user.email = data["email"]
    if "is_active" in data:
        user.is_active = data["is_active"]
    
    db.session.commit()
    return jsonify(user.to_dict())

# UPDATE: Alternative using filter
@app.route("/users/<username>/update-email", methods=["PUT"])
def update_email(username):
    user = User.query.filter_by(username=username).first()
    if not user:
        return jsonify({"error": "User not found"}), 404
    
    data = request.get_json()
    user.email = data.get("email")
    
    db.session.commit()
    return jsonify(user.to_dict())

# DELETE: Delete by ID
@app.route("/users/<int:user_id>", methods=["DELETE"])
def delete_user(user_id):
    user = User.query.get(user_id)
    if not user:
        return jsonify({"error": "User not found"}), 404
    
    db.session.delete(user)
    db.session.commit()
    
    return jsonify({"message": "User deleted"}), 200

# DELETE: Alternative using filter
@app.route("/users/delete/<username>", methods=["DELETE"])
def delete_by_username(username):
    user = User.query.filter_by(username=username).first()
    if not user:
        return jsonify({"error": "User not found"}), 404
    
    db.session.delete(user)
    db.session.commit()
    
    return jsonify({"message": "User deleted"}), 200

if __name__ == "__main__":
    app.run(debug=True)
```

## Quick Reference

| Operation | Code |
|-----------|------|
| Update | `user.field = value; db.session.commit()` |
| Delete | `db.session.delete(user); db.session.commit()` |

## Next Steps

Now you can update and delete. Continue to [03_querying_with_filters.md](03_querying_with_filters.md) to learn advanced querying.