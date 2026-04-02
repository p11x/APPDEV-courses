# Database Performance Testing

## Overview

Testing database performance ensures queries execute efficiently.

## Performance Tests

### Query Performance

```python
# Example 1: Query performance tests
import pytest
import time
from sqlalchemy.orm import Session

class TestDatabasePerformance:
    """Test database performance"""

    def test_user_query_performance(self, db_session: Session):
        """Test user query performance"""
        # Create test data
        for i in range(100):
            db_session.add(User(username=f"user{i}", email=f"user{i}@test.com"))
        db_session.commit()

        # Test query performance
        start = time.time()
        users = db_session.query(User).all()
        duration = time.time() - start

        assert len(users) == 100
        assert duration < 0.1  # Under 100ms

    def test_filtered_query_performance(self, db_session: Session):
        """Test filtered query performance"""
        start = time.time()
        users = db_session.query(User).filter(
            User.username == "user1"
        ).all()
        duration = time.time() - start

        assert duration < 0.05  # Under 50ms

    def test_pagination_performance(self, db_session: Session):
        """Test pagination performance"""
        start = time.time()
        users = db_session.query(User).offset(0).limit(20).all()
        duration = time.time() - start

        assert len(users) <= 20
        assert duration < 0.05
```

## Summary

Database performance tests ensure efficient queries.

## Next Steps

Continue learning about:
- [Query Optimization](../../04_database_integration_and_orm/08_database_performance/01_query_optimization.md)
- [Index Optimization](../../04_database_integration_and_orm/08_database_performance/02_index_optimization.md)
