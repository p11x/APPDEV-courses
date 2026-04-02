# Migration Testing

## Overview

Testing migrations ensures database schema changes work correctly.

## Migration Tests

### Testing Migrations

```python
# Example 1: Migration test
import pytest
from alembic.command import upgrade, downgrade
from alembic.config import Config

@pytest.fixture
def alembic_config():
    config = Config("alembic.ini")
    return config

def test_upgrade_head(alembic_config):
    """Test upgrade to latest"""
    upgrade(alembic_config, "head")

def test_downgrade_base(alembic_config):
    """Test downgrade to base"""
    downgrade(alembic_config, "base")

def test_round_trip(alembic_config):
    """Test upgrade and downgrade"""
    upgrade(alembic_config, "head")
    downgrade(alembic_config, "base")
    upgrade(alembic_config, "head")
```

### Testing Specific Migrations

```python
# Example 2: Test specific migration
def test_user_table_migration(alembic_config, db_session):
    """Test user table creation"""
    upgrade(alembic_config, "abc123")  # Specific revision

    # Verify table exists
    result = db_session.execute(
        "SELECT column_name FROM information_schema.columns "
        "WHERE table_name = 'users'"
    )
    columns = [row[0] for row in result]

    assert "id" in columns
    assert "username" in columns
    assert "email" in columns
```

## Summary

Migration testing ensures safe database changes.

## Next Steps

Continue learning about:
- [Migration Best Practices](../../04_database_integration_and_orm/09_migration_tools/07_migration_best_practices.md)
- [Rollback Strategies](../../04_database_integration_and_orm/09_migration_tools/04_rollback_strategies.md)
