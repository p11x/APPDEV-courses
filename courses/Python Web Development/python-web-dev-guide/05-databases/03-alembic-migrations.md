# Alembic Migrations

## What You'll Learn
- What database migrations are
- Setting up Alembic
- Creating and running migrations
- Rollback strategies
- Best practices

## Prerequisites
- Completed SQLAlchemy ORM

## What Are Migrations?

Migrations track changes to your database schema over time. When you modify your models, migrations update the database structure without losing data.

## Installing Alembic

```bash
pip install alembic
```

## Initializing Alembic

```bash
alembic init alembic
```

This creates:
- `alembic.ini` - Alembic configuration
- `alembic/` - Migration scripts folder
- `env.py` - Migration environment

## Configuration

```python
# alembic/env.py
from logging.config import fileConfig
from sqlalchemy import engine_from_config, pool
from alembic import context
import sys
from os.path import dirname, abspath

# Add your project root to path
sys.path.insert(0, dirname(dirname(abspath(__file__))))

# Import your models
from database import Base
from models import User, Post

config = context.config

if config.config_file_name is not None:
    fileConfig(config.config_file_name)

target_metadata = Base.metadata

def run_migrations_offline() -> None:
    """Run migrations in 'offline' mode."""
    url = config.get_main_option("sqlalchemy.url")
    context.configure(
        url=url,
        target_metadata=target_metadata,
        literal_binds=True,
        dialect_opts={"paramstyle": "named"},
    )

    with context.begin_transaction():
        context.run_migrations()

def run_migrations_online() -> None:
    """Run migrations in 'online' mode."""
    connectable = engine_from_config(
        config.get_section(config.config_ini_section, {}),
        prefix="sqlalchemy.",
        poolclass=pool.NullPool,
    )

    with connectable.connect() as connection:
        context.configure(
            connection=connection, target_metadata=target_metadata
        )

        with context.begin_transaction():
            context.run_migrations()

if context.is_offline_mode():
    run_migrations_offline()
else:
    run_migrations_online()
```

## Creating Migrations

After changing your models:

```bash
# Create a new migration
alembic revision --autogenerate -m "Add user bio field"

# Or create empty migration
alembic revision -m "Add user bio field"
```

### Migration File Example

```python
"""Add user bio field

Revision ID: abc123
Revises: 
Create Date: 2024-01-15 10:30:00

"""
from alembic import op
import sqlalchemy as sa

# revision identifiers
revision = 'abc123'
down_revision = 'prev123'
branch_labels = None
depends_on = None

def upgrade() -> None:
    op.add_column('users', sa.Column('bio', sa.String(500), nullable=True))

def downgrade() -> None:
    op.drop_column('users', 'bio')
```

## Running Migrations

```bash
# Upgrade to latest
alembic upgrade head

# Upgrade to specific revision
alembic upgrade abc123

# Upgrade one step
alembic upgrade +1

# Downgrade one step
alembic downgrade -1

# Downgrade to specific revision
alembic downgrade prev123
```

## Common Commands

```bash
# Show current revision
alembic current

# Show revision history
alembic history --verbose

# Stamp migration (without running)
alembic stamp abc123

# Check for pending migrations
alembic check
```

## Summary
- Migrations manage database schema changes
- `alembic revision` creates migration files
- `alembic upgrade` applies migrations
- `alembic downgrade` rolls back changes
