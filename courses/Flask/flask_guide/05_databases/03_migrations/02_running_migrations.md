<!-- FILE: 05_databases/03_migrations/02_running_migrations.md -->

## Overview

This file covers the Flask-Migrate commands for managing database migrations: creating migrations, applying them, and rolling back when needed.

## Core Concepts

### Migration Commands

| Command | Description |
|---------|-------------|
| `flask db init` | Initialize migrations |
| `flask db migrate -m "msg"` | Create migration |
| `flask db upgrade` | Apply migrations |
| `flask db downgrade` | Roll back |
| `flask db history` | View history |

## Code Walkthrough

### Migration Workflow

```bash
# 1. Initialize migrations folder
flask db init

# 2. After making model changes, create migration
flask db migrate -m "Add user age field"

# 3. Apply migration to database
flask db upgrade

# 4. Roll back if needed
flask db downgrade

# 5. View migration history
flask db history
```

## Next Steps

Now you can run migrations. Continue to [03_handling_schema_changes.md](03_handling_schema_changes.md) to learn handling schema changes.