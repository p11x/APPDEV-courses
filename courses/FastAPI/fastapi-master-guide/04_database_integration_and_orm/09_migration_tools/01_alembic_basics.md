# Alembic Basics

## Overview

Alembic is the standard migration tool for SQLAlchemy. It manages database schema changes in a version-controlled manner.

## Installation and Setup

### Initial Configuration

```bash
# Install Alembic
pip install alembic

# Initialize Alembic in your project
alembic init alembic
```

```python
# Example 1: Alembic configuration (alembic.ini)
"""
[alembic]
script_location = alembic
prepend_sys_path = .
sqlalchemy.url = postgresql://user:password@localhost/fastapi_db

[loggers]
keys = root,sqlalchemy,alembic

[handlers]
keys = console

[formatters]
keys = generic
"""

# alembic/env.py configuration
from logging.config import fileConfig
from sqlalchemy import engine_from_config, pool
from alembic import context
from app.database.base import Base  # Import your models
from app.models import User, Post  # Import all models

config = context.config
fileConfig(config.config_file_name)
target_metadata = Base.metadata

def run_migrations_offline():
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

def run_migrations_online():
    """Run migrations in 'online' mode."""
    connectable = engine_from_config(
        config.get_section(config.config_ini_section),
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

## Migration Commands

### Basic Commands

```bash
# Example 2: Common Alembic commands

# Create a new migration (auto-detect changes)
alembic revision --autogenerate -m "Create users table"

# Create empty migration
alembic revision -m "Custom migration"

# Apply all pending migrations
alembic upgrade head

# Apply next migration
alembic upgrade +1

# Rollback one migration
alembic downgrade -1

# Rollback to specific revision
alembic downgrade <revision_id>

# Show current revision
alembic current

# Show migration history
alembic history --verbose

# Show pending migrations
alembic heads
```

## Migration File Structure

### Understanding Migrations

```python
# Example 3: Generated migration file
"""Create users table

Revision ID: abc123def456
Revises: 
Create Date: 2024-01-15 10:30:00.000000
"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy.dialects import postgresql

# revision identifiers
revision = 'abc123def456'
down_revision = None
branch_labels = None
depends_on = None

def upgrade() -> None:
    """Apply migration"""
    op.create_table(
        'users',
        sa.Column('id', sa.Integer(), nullable=False),
        sa.Column('username', sa.String(50), nullable=False),
        sa.Column('email', sa.String(100), nullable=False),
        sa.Column('hashed_password', sa.String(128), nullable=False),
        sa.Column('is_active', sa.Boolean(), default=True),
        sa.Column('created_at', sa.DateTime(), server_default=sa.func.now()),
        sa.PrimaryKeyConstraint('id'),
        sa.UniqueConstraint('username'),
        sa.UniqueConstraint('email')
    )
    op.create_index('ix_users_username', 'users', ['username'])
    op.create_index('ix_users_email', 'users', ['email'])

def downgrade() -> None:
    """Revert migration"""
    op.drop_index('ix_users_email', table_name='users')
    op.drop_index('ix_users_username', table_name='users')
    op.drop_table('users')
```

## Advanced Migrations

### Data Migrations

```python
# Example 4: Data migration
"""Add full_name to users

Revision ID: def789ghi012
Revises: abc123def456
Create Date: 2024-01-16 14:00:00.000000
"""
from alembic import op
import sqlalchemy as sa

revision = 'def789ghi012'
down_revision = 'abc123def456'

def upgrade() -> None:
    # Add column
    op.add_column('users', sa.Column('full_name', sa.String(100), nullable=True))

    # Data migration: populate full_name from username
    op.execute("""
        UPDATE users 
        SET full_name = INITCAP(REPLACE(username, '_', ' '))
        WHERE full_name IS NULL
    """)

    # Make column non-nullable after data migration
    op.alter_column('users', 'full_name', nullable=False)

def downgrade() -> None:
    op.drop_column('users', 'full_name')
```

### Column Operations

```python
# Example 5: Common column operations
def upgrade() -> None:
    # Add column
    op.add_column('users', sa.Column('phone', sa.String(20), nullable=True))

    # Modify column
    op.alter_column('users', 'email',
                    existing_type=sa.String(100),
                    type_=sa.String(200))

    # Rename column
    op.alter_column('users', 'phone', new_column_name='phone_number')

    # Drop column
    op.drop_column('users', 'old_field')

    # Add index
    op.create_index('ix_users_phone', 'users', ['phone_number'])

    # Add foreign key
    op.create_foreign_key(
        'fk_posts_author_id',
        'posts', 'users',
        ['author_id'], ['id']
    )

    # Add unique constraint
    op.create_unique_constraint('uq_users_phone', 'users', ['phone_number'])

    # Add check constraint
    op.create_check_constraint(
        'ck_users_age_positive',
        'users',
        'age >= 0'
    )
```

## FastAPI Integration

### Programmatic Migrations

```python
# Example 6: Alembic with FastAPI
from alembic import command
from alembic.config import Config
from pathlib import Path

def get_alembic_config() -> Config:
    """Get Alembic configuration"""
    alembic_cfg = Config(str(Path(__file__).parent.parent / "alembic.ini"))
    return alembic_cfg

def run_migrations():
    """Run pending migrations"""
    config = get_alembic_config()
    command.upgrade(config, "head")

def get_current_revision() -> str:
    """Get current database revision"""
    config = get_alembic_config()
    return command.current(config)

# Use in FastAPI startup
from fastapi import FastAPI

app = FastAPI()

@app.on_event("startup")
async def startup():
    """Run migrations on startup"""
    run_migrations()
```

## Rollback Strategies

### Safe Rollbacks

```python
# Example 7: Rollback strategies
"""
Rollback Best Practices:

1. Always test downgrade() function
2. Backup data before destructive migrations
3. Use transactions for atomic migrations
4. Document rollback procedures
5. Keep migrations small and focused
"""

def upgrade() -> None:
    # Non-destructive change (safe rollback)
    op.add_column('users', sa.Column('new_field', sa.String(50), nullable=True))

def downgrade() -> None:
    # Safe rollback
    op.drop_column('users', 'new_field')

# For destructive changes, use multi-step migration
# Step 1: Add new column
# Step 2: Migrate data
# Step 3: Remove old column (after verification)
```

## Best Practices

### Migration Guidelines

```python
# Example 8: Migration best practices
"""
Alembic Best Practices:

1. Review auto-generated migrations
2. Test migrations on development first
3. Keep migrations atomic
4. Use descriptive migration messages
5. Never edit applied migrations
6. Version control migration files
7. Document complex migrations
8. Test downgrade() functions
9. Use transactions for safety
10. Backup before destructive changes
"""

# Project structure
"""
alembic/
├── versions/
│   ├── abc123_create_users.py
│   ├── def456_add_posts.py
│   └── ghi789_add_indexes.py
├── env.py
├── README
├── script.py.mako
└── alembic.ini
"""
```

## Summary

| Command | Purpose | Example |
|---------|---------|---------|
| `revision` | Create migration | `alembic revision -m "description"` |
| `upgrade` | Apply migrations | `alembic upgrade head` |
| `downgrade` | Rollback migrations | `alembic downgrade -1` |
| `current` | Show current version | `alembic current` |
| `history` | Show history | `alembic history` |

## Next Steps

Continue learning about:
- [Migration Scripts](./02_migration_scripts.md) - Advanced scripts
- [Version Control](./03_version_control.md) - Git integration
- [Rollback Strategies](./04_rollback_strategies.md) - Safe rollbacks
