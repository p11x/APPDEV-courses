# Query Plan Analysis

## Overview

Analyzing query execution plans helps identify and fix performance bottlenecks.

## EXPLAIN ANALYZE

### Using EXPLAIN

```python
# Example 1: Query plan analysis
from sqlalchemy import text

def analyze_query(db: Session, query: str):
    """Get query execution plan"""
    result = db.execute(text(f"EXPLAIN ANALYZE {query}"))
    return [row[0] for row in result]

# Analyze specific query
plan = analyze_query(db, "SELECT * FROM users WHERE email = 'test@example.com'")
for line in plan:
    print(line)
```

### Common Plan Patterns

```python
# Example 2: Identifying issues
"""
Bad Patterns:
- Seq Scan on large tables
- Nested Loop with high row counts
- Hash Join with large datasets
- Sort with high memory usage

Good Patterns:
- Index Scan
- Index Only Scan
- Bitmap Index Scan
- Efficient Join methods
"""
```

## Optimization

```python
# Example 3: Query optimization
# BAD: Sequential scan
db.query(User).filter(User.username.ilike('%test%')).all()

# GOOD: Index scan
db.query(User).filter(User.username == 'test').all()

# Create index to support query
CREATE INDEX idx_users_username ON users(username);
```

## Summary

Query plan analysis is essential for identifying and fixing performance issues.

## Next Steps

Continue learning about:
- [Index Optimization](./02_index_optimization.md)
- [Caching Strategies](./04_caching_strategies.md)
