# Database Sharding

## Overview

Database sharding distributes data across multiple database instances for horizontal scaling.

## Sharding Strategies

### Range-Based Sharding

```python
# Example 1: Range-based sharding
from fastapi import FastAPI, Depends
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

app = FastAPI()

# Shard configuration
SHARDS = {
    "shard_1": create_engine("postgresql://user:pass@shard1:5432/db"),
    "shard_2": create_engine("postgresql://user:pass@shard2:5432/db"),
    "shard_3": create_engine("postgresql://user:pass@shard3:5432/db"),
}

class ShardRouter:
    """Route queries to appropriate shard"""

    def __init__(self, shards: dict):
        self.shards = shards
        self.sessions = {
            name: sessionmaker(bind=engine)
            for name, engine in shards.items()
        }

    def get_shard_for_user(self, user_id: int) -> str:
        """Determine shard based on user ID range"""
        if user_id <= 1000000:
            return "shard_1"
        elif user_id <= 2000000:
            return "shard_2"
        else:
            return "shard_3"

    def get_session(self, user_id: int):
        """Get session for user's shard"""
        shard = self.get_shard_for_user(user_id)
        return self.sessions[shard]()

router = ShardRouter(SHARDS)

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Get user from appropriate shard"""
    session = router.get_session(user_id)
    try:
        user = session.query(User).filter(User.id == user_id).first()
        return user
    finally:
        session.close()
```

### Hash-Based Sharding

```python
# Example 2: Hash-based sharding
import hashlib

class HashShardRouter:
    """Hash-based shard routing"""

    def __init__(self, shards: list, num_shards: int = 4):
        self.shards = shards
        self.num_shards = num_shards

    def get_shard(self, key: str) -> str:
        """Get shard for key using consistent hashing"""
        hash_value = int(hashlib.md5(key.encode()).hexdigest(), 16)
        shard_index = hash_value % self.num_shards
        return self.shards[shard_index]

    def get_session(self, key: str):
        """Get session for key"""
        shard = self.get_shard(key)
        return self.sessions[shard]()

hash_router = HashShardRouter(
    shards=["shard_0", "shard_1", "shard_2", "shard_3"],
    num_shards=4
)

@app.get("/users/email/{email}")
async def get_user_by_email(email: str):
    """Get user by email using hash-based sharding"""
    session = hash_router.get_session(email)
    try:
        user = session.query(User).filter(User.email == email).first()
        return user
    finally:
        session.close()
```

## Cross-Shard Queries

### Scatter-Gather Pattern

```python
# Example 3: Cross-shard queries
import asyncio

async def cross_shard_query(query_func, *args):
    """Execute query across all shards"""
    tasks = []
    for shard_name, session_factory in router.sessions.items():
        session = session_factory()
        tasks.append(query_func(session, *args))

    results = await asyncio.gather(*tasks, return_exceptions=True)

    # Combine results
    combined = []
    for result in results:
        if not isinstance(result, Exception):
            combined.extend(result)

    return combined

async def query_all_shards(session, search_term: str):
    """Query single shard"""
    return session.query(User).filter(
        User.username.ilike(f"%{search_term}%")
    ).all()

@app.get("/users/search/")
async def search_users(q: str):
    """Search users across all shards"""
    results = await cross_shard_query(query_all_shards, q)
    return {"users": results, "total": len(results)}
```

## Summary

Database sharding enables horizontal scaling for large datasets.

## Next Steps

Continue learning about:
- [Database Replication](./12_database_replication.md)
- [Database Clustering](./13_database_clustering.md)
