# Redis as Primary Database

## What You'll Learn
- Redis beyond caching — as a primary data store
- Data structures in Redis (strings, hashes, lists, sets, sorted sets)
- Implementing sessions, rate limiting, and leaderboards with Redis
- Handling persistence and durability
- When to use Redis as primary vs PostgreSQL

## Prerequisites
- Completed `05-mongodb-with-motor-async.md` — document database patterns
- Completed `15-caching/02-redis-fundamentals.md` — basic Redis usage
- Understanding of in-memory data structures

## Redis as a Primary Database

Most developers use Redis only for caching, but Redis is a legitimate primary database for specific use cases. It's an in-memory data structure store with optional persistence.

```
┌─────────────────────────────────────────────────────────────┐
│                    Redis Data Structures                     │
├─────────────────────────────────────────────────────────────┤
│  STRING    │ Simple text/binary values                     │
│  HASH      │ Field-value pairs (like JSON objects)        │
│  LIST      │ Ordered list of strings                      │
│  SET       │ Unordered collection of unique strings       │
│  ZSET      │ Sorted set (leaderboards, priorities)         │
│  BITMAP    │ Bit arrays (user activity, analytics)         │
│  HYPERLOG  │ Probabilistic cardinality (unique visits)    │
│  GEO       │ Geospatial coordinates                       │
│  STREAM     │ Log/message queue (like Kafka-lite)         │
└─────────────────────────────────────────────────────────────┘
```

## Installing and Connecting

```bash
pip install redis pydantic
```

```python
import redis.asyncio as aioredis
from typing import Optional

redis_client = aioredis.from_url(
    "redis://localhost:6379/0",
    encoding="utf-8",
    decode_responses=True
)
```

## Redis Data Types in Practice

### Strings — Sessions and Caching

```python
import json
from datetime import timedelta
from pydantic import BaseModel

class UserSession(BaseModel):
    user_id: int
    username: str
    roles: list[str]
    expires_in: int  # seconds

async def create_session(user_id: int, username: str, roles: list[str]) -> str:
    """Create a user session."""
    session_id = f"session:{user_id}:{username}"
    session_data = UserSession(
        user_id=user_id,
        username=username,
        roles=roles,
        expires_in=3600
    )
    
    await redis_client.setex(
        session_id,
        timedelta(hours=1),
        session_data.model_dump_json()
    )
    return session_id

async def get_session(session_id: str) -> Optional[UserSession]:
    """Get session data."""
    data = await redis_client.get(session_id)
    if data:
        return UserSession.model_validate_json(data)
    return None

async def delete_session(session_id: str) -> None:
    """Logout - delete session."""
    await redis_client.delete(session_id)
```

### Hashes — Objects and Profiles

```python
async def create_user_profile(user_id: int, name: str, email: str, **extra_fields) -> None:
    """Store user profile in a hash."""
    key = f"user:{user_id}"
    data = {
        "name": name,
        "email": email,
        "created_at": "2024-01-01",
        **extra_fields
    }
    await redis_client.hset(key, mapping=data)

async def get_user_profile(user_id: int) -> Optional[dict]:
    """Get user profile."""
    key = f"user:{user_id}"
    data = await redis_client.hgetall(key)
    return data if data else None

async def update_user_field(user_id: int, field: str, value: str) -> None:
    """Update single field."""
    await redis_client.hset(f"user:{user_id}", field, value)
```

### Sorted Sets — Leaderboards and Priorities

```python
async def update_leaderboard(leaderboard: str, user_id: int, score: float) -> None:
    """Update a user's score."""
    await redis_client.zadd(leaderboard, {str(user_id): score})

async def get_leaderboard(leaderboard: str, top_n: int = 10) -> list[dict]:
    """Get top N users."""
    # ZREVRANGE returns highest scores first
    results = await redis_client.zrevrange(
        leaderboard, 0, top_n - 1, withscores=True
    )
    return [
        {"user_id": user_id, "score": score}
        for user_id, score in results
    ]

async def get_user_rank(leaderboard: str, user_id: int) -> int:
    """Get user's rank (0-indexed)."""
    # ZREVRANK returns rank (0 = highest)
    rank = await redis_client.zrevrank(leaderboard, str(user_id))
    return rank if rank is not None else -1
```

### Rate Limiting

```python
from datetime import datetime

async def check_rate_limit(key: str, limit: int, window_seconds: int) -> tuple[bool, int]:
    """
    Check if key is within rate limit.
    Returns (allowed, current_count).
    """
    now = datetime.utcnow().timestamp()
    window_key = f"ratelimit:{key}"
    
    # Remove old entries
    await redis_client.zremrangebyscore(window_key, 0, now - window_seconds)
    
    # Count current requests
    count = await redis_client.zcard(window_key)
    
    if count >= limit:
        return False, count
    
    # Add new request
    await redis_client.zadd(window_key, {f"{now}": now})
    await redis_client.expire(window_key, window_seconds)
    
    return True, count + 1

# Usage in FastAPI
@app.get("/api/data")
async def get_data(request: Request):
    client_ip = request.client.host
    allowed, count = await check_rate_limit(f"ip:{client_ip}", 100, 60)
    
    if not allowed:
        raise HTTPException(status_code=429, detail="Rate limit exceeded")
    
    return {"data": "result", "requests_this_minute": count}
```

### Lists — Task Queues and Logs

```python
async def add_task(queue_name: str, task_data: dict) -> None:
    """Add task to queue."""
    await redis_client.lpush(queue_name, json.dumps(task_data))

async def pop_task(queue_name: str) -> dict | None:
    """Pop task from queue (blocking)."""
    # BRPOP waits for item if queue is empty
    result = await redis_client.brpop(queue_name, timeout=5)
    if result:
        _, task_json = result
        return json.loads(task_json)
    return None
```

## Redis Persistence

By default, Redis is in-memory only. For primary database use, enable persistence:

```bash
# In redis.conf or via command line
# AOF (Append Only File) - every write appended to log
redis-server --appendonly yes

# Or RDB snapshots
redis-server --save 900 1 --save 300 10  # Save every 900s if 1 key changed, or every 300s if 10 keys changed
```

For critical data, use Redis Cluster for replication:

```bash
# redis.conf for master-replica setup
replicaof 192.168.1.1 6379
```

## When to Use Redis as Primary

| Use Case | Redis | PostgreSQL |
|----------|-------|------------|
| Sessions | ✅ Perfect | ❌ Overkill |
| Caching | ✅ Perfect | ❌ Overkill |
| Rate limiting | ✅ Perfect | ❌ Overkill |
| Leaderboards | ✅ Perfect | ❌ Slower |
| User profiles | ⚠️ Limited | ✅ Better |
| Transactional data | ❌ Risky | ✅ Better |
| Analytics data | ✅ Good | ✅ Good |
| Real-time lists | ✅ Perfect | ❌ Slower |

## Production Considerations

- **Memory**: Redis stores everything in RAM. Budget ~1-2 bytes per key + value. Monitor memory usage with `INFO memory`.
- **Persistence**: AOF is safer but slower. RDB is faster but less granular. Consider Redis Cluster for HA.
- **Eviction policies**: Set `maxmemory-policy` to control what happens when full — typically `allkeys-lru` for caching.
- **Persistence**: Redis is single-threaded. Heavy commands (`KEYS`, `SMEMBERS`) block everything. Use `SCAN` for large sets.
- **Keys**: Keep key names short. `user:12345:profile` is better than `users:by_id:12345:user_profile_information`.

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Storing everything as JSON strings

**Wrong:**
```python
# Storing entire object as single JSON string
await redis_client.set("user:1", json.dumps({"name": "Alice", "age": 30}))
# To update age, need to deserialize, modify, serialize
```

**Why it fails:** Can't update individual fields. Forces full object rewrite on any change.

**Fix:** Use hashes for objects:
```python
await redis_client.hset("user:1", mapping={"name": "Alice", "age": "30"})
await redis_client.hincrby("user:1", "age", 1)  # Increment atomically
```

### ❌ Mistake 2: Not setting TTL on keys

**Wrong:**
```python
# No expiration - data lives forever
await redis_client.set("temp_data", value)
```

**Why it fails:** Accumulated data never cleaned up. Memory grows unbounded.

**Fix:** Always set expiration:
```python
await redis_client.setex("temp_data", 3600, value)  # 1 hour TTL
# Or
await redis_client.expire("temp_data", 3600)
```

### ❌ Mistake 3: Using KEYS in production

**Wrong:**
```python
# Finding all user keys
keys = await redis_client.keys("user:*")
```

**Why it fails:** `KEYS` scans entire keyspace and blocks Redis for seconds/minutes on large datasets.

**Fix:** Use SCAN:
```python
# Iterate safely
async for key in redis_client.scan_iter(match="user:*", count=100):
    # Process key
    pass
```

## Summary

- Redis is a versatile primary database for specific use cases (sessions, leaderboards, rate limiting)
- Use appropriate data structures: hashes for objects, sorted sets for rankings, lists for queues
- Always set TTL on keys to prevent unbounded growth
- Use SCAN instead of KEYS for production workloads
- Understand Redis limitations: single-threaded, memory-bound, no complex queries

## Next Steps

→ Continue to `07-database-transactions-and-acid.md` to understand ACID guarantees and how to maintain data consistency.
