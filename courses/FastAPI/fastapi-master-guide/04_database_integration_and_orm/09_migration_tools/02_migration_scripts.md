# Migration Scripts

## Overview

Alembic migration scripts define database schema changes in a version-controlled manner.

## Creating Migrations

### Auto-Generated Migrations

```bash
# Example 1: Generate migration from model changes
alembic revision --autogenerate -m "Add users table"
```

```python
# Generated migration file
"""Add users table

Revision ID: abc123
Revises: def456
Create Date: 2024-01-15 10:30:00
"""
from alembic import op
import sqlalchemy as sa

revision = 'abc123'
down_revision = 'def456'

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

def downgrade():
    op.drop_table('users')
```

### Manual Migrations

```python
# Example 2: Manual migration for data changes
"""Migrate user data

Revision ID: xyz789
"""
from alembic import op
import sqlalchemy as sa

def upgrade():
    # Add new column with default
    op.add_column('users',
        sa.Column('is_active', sa.Boolean(), server_default='true')
    )

    # Data migration
    op.execute("""
        UPDATE users
        SET is_active = true
        WHERE is_active IS NULL
    """)

def downgrade():
    op.drop_column('users', 'is_active')
```

## Common Operations

```python
# Example 3: Common migration operations
def upgrade():
    # Add column
    op.add_column('users', sa.Column('phone', sa.String(20)))

    # Modify column
    op.alter_column('users', 'email',
        existing_type=sa.String(100),
        type_=sa.String(200)
    )

    # Add index
    op.create_index('ix_users_phone', 'users', ['phone'])

    # Add foreign key
    op.create_foreign_key(
        'fk_posts_author',
        'posts', 'users',
        ['author_id'], ['id']
    )

    # Add unique constraint
    op.create_unique_constraint('uq_users_phone', 'users', ['phone'])
```

## Summary

Migration scripts provide version-controlled database schema changes.

## Next Steps

Continue learning about:
- [Rollback Strategies](./04_rollback_strategies.md)
- [Migration Best Practices](./07_migration_best_practices.md)
