# Database Migrations

## Overview

Database migrations version-control schema changes, enabling safe, repeatable database updates across environments.

## Migration Concepts

### Why Migrations Matter

```python
# Example 1: Migration benefits
"""
Without Migrations:
- Manual schema changes
- Inconsistent environments
- No version control
- Difficult rollbacks

With Migrations:
- Automated schema changes
- Consistent environments
- Version-controlled changes
- Easy rollbacks
- Team collaboration
"""

# Migration lifecycle
"""
1. Developer creates migration
2. Migration reviewed in PR
3. Migration runs in staging
4. Migration runs in production
5. Migration tracked in database
"""
```

### Alembic Setup

```python
# Example 2: Alembic configuration
# alembic.ini
"""
[alembic]
script_location = alembic
sqlalchemy.url = postgresql://user:pass@localhost/db
"""

# alembic/env.py
from alembic import context
from sqlalchemy import engine_from_config
from app.database import Base
from app.models import User, Post  # Import all models

config = context.config
target_metadata = Base.metadata

def run_migrations_online():
    connectable = engine_from_config(
        config.get_section(config.config_ini_section),
        prefix="sqlalchemy."
    )

    with connectable.connect() as connection:
        context.configure(
            connection=connection,
            target_metadata=target_metadata
        )

        with context.begin_transaction():
            context.run_migrations()
```

## Migration Patterns

### Common Migration Operations

```python
# Example 3: Common migration operations
# alembic/versions/001_create_users.py
"""Create users table

Revision ID: 001
"""
from alembic import op
import sqlalchemy as sa

def upgrade():
    op.create_table(
        'users',
        sa.Column('id', sa.Integer(), nullable=False),
        sa.Column('username', sa.String(50), nullable=False),
        sa.Column('email', sa.String(100), nullable=False),
        sa.Column('created_at', sa.DateTime(), server_default=sa.func.now()),
        sa.PrimaryKeyConstraint('id'),
        sa.UniqueConstraint('username'),
        sa.UniqueConstraint('email')
    )
    op.create_index('ix_users_username', 'users', ['username'])

def downgrade():
    op.drop_index('ix_users_username', table_name='users')
    op.drop_table('users')
```

## Best Practices

1. Keep migrations small and focused
2. Always test downgrade()
3. Never edit applied migrations
4. Use descriptive migration names

## Summary

Migrations provide safe, version-controlled database schema management.

## Next Steps

Continue learning about:
- [Query Optimization](./06_query_optimization.md)
- [Alembic Basics](../09_migration_tools/01_alembic_basics.md)
