# Hash Tables and Dictionaries

## What You'll Learn

- How hash tables work
- Python dict operations
- Avoiding collisions

## Prerequisites

- Completed `02-basic-data-structures.md`

## How Hash Tables Work

A hash table maps keys to values using a hash function. It provides O(1) average-case lookup, insertion, and deletion.

Think of a hash table like a dictionary book: you look up a word (key) and find its definition (value) directly, rather than reading through every page.

## Python Dictionaries

```python
# Creating a dictionary
user: dict[str, str | int] = {
    "name": "Alice",
    "email": "alice@example.com",
    "age": 30
}

# Access - O(1)
name = user["name"]

# Get with default - O(1)
age = user.get("age", 0)

# Insert/Update - O(1)
user["location"] = "NYC"

# Delete - O(1)
del user["age"]

# Check existence - O(1)
if "email" in user:
    print(user["email"])
```

## Custom Hash Table

```python
from dataclasses import dataclass, field

@dataclass
class HashTable[K, V]:
    """Simple hash table implementation."""
    _size: int = 16
    _buckets: list[list[tuple[K, V]]] = field(default_factory=lambda: [[] for _ in range(16)])
    _count: int = 0
    
    def _hash(self, key: K) -> int:
        """Hash function."""
        return hash(key) % self._size
    
    def put(self, key: K, value: V) -> None:
        """Insert or update a key-value pair."""
        index = self._hash(key)
        bucket = self._buckets[index]
        
        # Check if key exists
        for i, (k, v) in enumerate(bucket):
            if k == key:
                bucket[i] = (key, value)
                return
        
        # Add new entry
        bucket.append((key, value))
        self._count += 1
        
        # Resize if load factor is high
        if self._count / self._size > 0.75:
            self._resize()
    
    def get(self, key: K, default: V | None = None) -> V | None:
        """Get value by key."""
        index = self._hash(key)
        bucket = self._buckets[index]
        
        for k, v in bucket:
            if k == key:
                return v
        return default
    
    def _resize(self) -> None:
        """Resize the hash table."""
        old_buckets = self._buckets
        self._size *= 2
        self._buckets = [[] for _ in range(self._size)]
        self._count = 0
        
        for bucket in old_buckets:
            for key, value in bucket:
                self.put(key, value)
```

## Use Cases

- **User lookups** - O(1) by user ID
- **Caching** - Store frequently accessed data
- **Counting** - Frequency counts
- **Deduplication** - Remove duplicates

```python
# Count word frequencies - O(n)
def count_words(text: str) -> dict[str, int]:
    words = text.split()
    counts: dict[str, int] = {}
    
    for word in words:
        counts[word] = counts.get(word, 0) + 1
    
    return counts

# Deduplicate - O(n)
def deduplicate(items: list) -> list:
    seen: set = set()
    result = []
    for item in items:
        if item not in seen:
            seen.add(item)
            result.append(item)
    return result
```

## Summary

- Hash tables provide O(1) average-case operations
- Python dict is a hash table
- Good for fast lookups and counting

## Next Steps

Continue to `04-sorting-algorithms.md`.
