# Debugging Database

## What You'll Learn

- Debugging SQL queries
- Connection issues
- Query performance

## Prerequisites

- Completed `05-debugging-web-apps.md`

## SQLAlchemy Echo

```python
# Enable SQL echo
engine = create_engine("sqlite:///db.sqlite", echo=True)

# Shows all SQL queries
```

## Logging Queries

```python
import logging
logging.basicConfig()
logging.getLogger('sqlalchemy.engine').setLevel(logging.INFO)
```

## Summary

- Enable SQL echo for debugging
- Check connection strings

## Next Steps

Continue to `07-debugging-api-errors.md`.
