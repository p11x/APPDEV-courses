<!-- FILE: 05_databases/01_sqlalchemy_basics/02_installing_flask_sqlalchemy.md -->

## Overview

**Flask-SQLAlchemy** is the Flask integration of SQLAlchemy. This file covers installing Flask-SQLAlchemy, configuring database connections, and setting up your first database-backed Flask application.

## Prerequisites

- Flask installed
- Basic virtual environment knowledge

## Core Concepts

### Installation

```bash
pip install flask-sqlalchemy
```

### Database URIs

Database connection strings:
- SQLite: `sqlite:///myapp.db`
- PostgreSQL: `postgresql://user:pass@localhost/dbname`
- MySQL: `mysql://user:pass@localhost/dbname`

## Code Walkthrough

### Installation and Setup

```bash
# Install Flask-SQLAlchemy
pip install flask-sqlalchemy
```

### Basic Application

```python
# app.py — Flask-SQLAlchemy setup
from flask import Flask
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)

# Configuration
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

# Initialize SQLAlchemy
db = SQLAlchemy(app)

# Define a model
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    email = db.Column(db.String(120), unique=True, nullable=False)
    
    def __repr__(self):
        return f"<User {self.username}>"

# Create tables
with app.app_context():
    db.create_all()

@app.route("/")
def index():
    return "Database connected!"

if __name__ == "__main__":
    app.run(debug=True)
```

### Different Database Configurations

```python
# SQLite (development)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///dev.db"

# PostgreSQL (production)
app.config["SQLALCHEMY_DATABASE_URI"] = "postgresql://user:password@localhost/mydb"

# MySQL
app.config["SQLALCHEMY_DATABASE_URI"] = "mysql://user:password@localhost/mydb"

# SQLite in-memory (for testing)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///:memory:"
```

## Quick Reference

| Database | URI Format |
|---------|-----------|
| SQLite | `sqlite:///filename.db` |
| PostgreSQL | `postgresql://user:pass@host/db` |
| MySQL | `mysql://user:pass@host/db` |

## Next Steps

Now Flask-SQLAlchemy is installed. Continue to [03_defining_models.md](03_defining_models.md) to learn how to define database models.