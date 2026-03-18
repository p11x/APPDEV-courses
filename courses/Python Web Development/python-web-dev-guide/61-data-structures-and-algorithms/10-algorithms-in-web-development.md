# Algorithms in Web Development

## What You'll Learn

- Applying algorithms in web apps
- Database indexing
- Caching strategies

## Prerequisites

- Completed `09-greedy-algorithms.md`

## Database Indexing

Database indexes use B-trees for O(log n) lookups:

```sql
-- Without index: O(n) full table scan
SELECT * FROM users WHERE email = 'alice@example.com';

-- With index: O(log n) index lookup
CREATE INDEX idx_users_email ON users(email);
```

## Caching

LRU (Least Recently Used) cache:

```python
from collections import OrderedDict
from typing import TypeVar, Generic

K = TypeVar('K')
V = TypeVar('V')

class LRUCache(Generic[K, V]):
    def __init__(self, capacity: int):
        self.cache: OrderedDict[K, V] = OrderedDict()
        self.capacity = capacity
    
    def get(self, key: K) -> V | None:
        if key not in self.cache:
            return None
        
        self.cache.move_to_end(key)
        return self.cache[key]
    
    def put(self, key: K, value: V) -> None:
        if key in self.cache:
            self.cache.move_to_end(key)
        
        self.cache[key] = value
        
        if len(self.cache) > self.capacity:
            self.cache.popitem(last=False)
```

## Rate Limiting

Token bucket algorithm:

```python
import time
from dataclasses import dataclass

@dataclass
class TokenBucket:
    capacity: int
    refill_rate: float  # tokens per second
    tokens: float
    last_refill: float
    
    def allow_request(self, tokens_needed: int = 1) -> bool:
        now = time.time()
        elapsed = now - self.last_refill
        
        # Refill tokens
        self.tokens = min(
            self.capacity,
            self.tokens + elapsed * self.refill_rate
        )
        self.last_refill = now
        
        if self.tokens >= tokens_needed:
            self.tokens -= tokens_needed
            return True
        
        return False
```

## Pagination

Offset-based pagination:

```python
def get_page(items: list, page: int, page_size: int) -> list:
    start = (page - 1) * page_size
    end = start + page_size
    return items[start:end]
```

Cursor-based pagination (better for large datasets):

```python
def get_after_cursor(items: list, cursor: str, limit: int) -> list:
    """Get items after a cursor."""
    # Find cursor position
    for i, item in enumerate(items):
        if str(item["id"]) == cursor:
            return items[i+1:i+1+limit]
    return items[:limit]
```

## Sorting and Filtering

Efficient filtering:

```python
def filter_and_sort(
    items: list[dict],
    filters: dict,
    sort_by: str,
    descending: bool = False
) -> list[dict]:
    # Apply filters
    result = items
    for key, value in filters.items():
        result = [item for item in result if item.get(key) == value]
    
    # Sort
    result.sort(key=lambda x: x.get(sort_by, 0), reverse=descending)
    
    return result
```

## Hashing for Passwords

Using bcrypt:

```python
import bcrypt

def hash_password(password: str) -> str:
    """Hash a password."""
    salt = bcrypt.gensalt()
    return bcrypt.hashpw(password.encode(), salt).decode()

def verify_password(password: str, hashed: str) -> bool:
    """Verify a password."""
    return bcrypt.checkpw(password.encode(), hashed.encode())
```

## Summary

- Use indexes for database performance
- Implement caching with LRU
- Use cursor pagination for large datasets
- Hash passwords securely

## Next Steps

This concludes the Data Structures and Algorithms folder. Continue to other topics in your learning journey.
