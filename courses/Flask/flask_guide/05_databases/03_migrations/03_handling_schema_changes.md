<!-- FILE: 05_databases/03_migrations/03_handling_schema_changes.md -->

## Overview

As applications evolve, database schemas need to change. This file covers handling common schema changes: adding columns, removing tables, handling relationships, and best practices.

## Core Concepts

### Common Schema Changes

1. Add new column
2. Remove column  
3. Rename column
4. Add/remove table

## Code Walkthrough

### Adding a Column

```python
# models.py — Add new field
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80))
    # New field
    age = db.Column(db.Integer)
```

```bash
flask db migrate -m "Add age to user"
flask db upgrade
```

### Removing a Column

```python
# models.py — Remove field
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80))
    # Remove: age = db.Column(db.Integer)
```

```bash
flask db migrate -m "Remove age from user"
flask db upgrade
```

## Next Steps

You have completed the database chapter. Continue to [01_how_sessions_work.md](../../06_authentication/01_sessions_and_cookies/01_how_sessions_work.md) to learn about authentication.