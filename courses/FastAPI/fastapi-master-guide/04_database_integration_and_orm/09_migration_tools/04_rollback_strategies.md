# Rollback Strategies

## Overview

Rollback strategies allow reverting database migrations when issues occur.

## Rollback Commands

### Basic Rollback

```bash
# Example 1: Rollback commands
# Rollback one migration
alembic downgrade -1

# Rollback to specific revision
alembic downgrade abc123

# Rollback all migrations
alembic downgrade base

# Show current revision
alembic current
```

## Rollback Safety

### Safe Rollback Implementation

```python
# Example 2: Safe rollback patterns
def upgrade():
    """Add column with safe rollback"""
    op.add_column('users',
        sa.Column('new_field', sa.String(50), nullable=True)
    )

def downgrade():
    """Safe rollback - drop column"""
    op.drop_column('users', 'new_field')

# Non-destructive migration (easy rollback)
def upgrade():
    op.add_column('users', sa.Column('bio', sa.Text()))

def downgrade():
    op.drop_column('users', 'bio')
```

### Data Preservation

```python
# Example 3: Preserve data during rollback
def upgrade():
    # Create new table
    op.create_table('users_v2',
        sa.Column('id', sa.Integer(), primary_key=True),
        sa.Column('name', sa.String(100)),
    )

    # Copy data
    op.execute("""
        INSERT INTO users_v2 (id, name)
        SELECT id, username FROM users
    """)

def downgrade():
    # Copy data back
    op.execute("""
        UPDATE users
        SET username = (SELECT name FROM users_v2 WHERE users_v2.id = users.id)
    """)

    op.drop_table('users_v2')
```

## Best Practices

1. Always test downgrade() function
2. Backup before destructive migrations
3. Use transactions for atomicity
4. Document rollback procedures

## Summary

Rollback strategies ensure safe migration recovery.

## Next Steps

Continue learning about:
- [Migration Testing](./05_migration_testing.md)
- [Migration Best Practices](./07_migration_best_practices.md)
