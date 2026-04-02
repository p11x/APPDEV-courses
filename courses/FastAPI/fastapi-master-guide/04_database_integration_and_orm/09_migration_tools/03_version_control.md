# Version Control for Migrations

## Overview

Version control integration ensures migrations are tracked, reviewed, and deployed consistently.

## Git Integration

### Migration Workflow

```bash
# Example 1: Git workflow for migrations
# 1. Create feature branch
git checkout -b feature/add-user-profile

# 2. Make model changes
# Edit app/models/user.py

# 3. Generate migration
alembic revision --autogenerate -m "Add user profile fields"

# 4. Review generated migration
# Edit alembic/versions/xxx_add_user_profile_fields.py

# 5. Test migration
alembic upgrade head
alembic downgrade -1
alembic upgrade head

# 6. Commit changes
git add app/models/ alembic/versions/
git commit -m "Add user profile fields"

# 7. Push and create PR
git push origin feature/add-user-profile
```

## CI/CD Integration

```yaml
# Example 2: GitHub Actions for migrations
name: Migrations

on:
  push:
    branches: [main]
    paths:
      - 'alembic/versions/**'

jobs:
  test-migrations:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup Python
        uses: actions/setup-python@v5
        with:
          python-version: '3.11'

      - name: Install dependencies
        run: pip install -r requirements.txt

      - name: Test migrations
        run: |
          alembic upgrade head
          alembic downgrade -1
          alembic upgrade head
```

## Summary

Version control ensures migrations are tracked and reviewed.

## Next Steps

Continue learning about:
- [Migration Best Practices](./07_migration_best_practices.md)
- [Migration Automation](./06_migration_automation.md)
