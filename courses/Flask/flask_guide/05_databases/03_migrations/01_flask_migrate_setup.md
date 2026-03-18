<!-- FILE: 05_databases/03_migrations/01_flask_migrate_setup.md -->

## Overview

**Flask-Migrate** manages database schema changes (migrations). As your models evolve, migrations update the database without losing data. This file covers installing and initializing Flask-Migrate.

## Prerequisites

- Flask-SQLAlchemy installed
- Database configured

## Core Concepts

Migrations track database changes:
1. Make model changes
2. Generate migration
3. Apply migration

## Code Walkthrough

### Installation and Setup

```bash
pip install flask-migrate
```

```python
# app.py — Flask-Migrate setup
from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

db = SQLAlchemy(app)
migrate = Migrate(app, db)  # Initialize migrate

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80))

# Commands:
# flask db init        - Create migrations folder
# flask db migrate -m "message" - Create migration
# flask db upgrade     - Apply migration
# flask db downgrade   - Undo migration
```

## Next Steps

Now migrations are set up. Continue to [02_running_migrations.md](02_running_migrations.md) to learn running migrations.