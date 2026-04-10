---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: ElastiCache Practical
Purpose: Practical ElastiCache implementation and use cases
Difficulty: practical
Prerequisites: 01_Basic_ElastiCache.md, 02_Advanced_ElastiCache.md
RelatedFiles: 01_Basic_ElastiCache.md, 02_Advanced_ElastiCache.md
UseCase: Production caching implementation
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Practical ElastiCache implementation involves real-world patterns, best practices, and common use cases. This knowledge is essential for building performant, scalable applications.

### Why Practical Implementation Matters

- **Production Ready**: Real-world patterns work at scale
- **Cost Effective**: Optimize infrastructure costs
- **Secure**: Follow security best practices
- **Observable**: Proper monitoring and alerting
- **Maintainable**: Documented and repeatable

### Common Production Use Cases

- **Session Store**: User sessions, shopping carts
- **Database Cache**: Query results, metadata
- **Rate Limiting**: API throttling
- **Leaderboards**: Gaming, analytics
- **Real-time Features**: Chat, notifications

## WHAT

### Implementation Patterns

| Pattern | Use Case | Redis Command |
|---------|----------|---------------|
| Cache-Aside | Database caching | GET/SET/MGET |
| Write-Through | Synchronous caching | SET on write |
| TTL Expiry | Time-based cache | EXPIRE/EX |
| Rate Limiter | API throttling | INCR/EXPIRE |
| Pub/Sub | Real-time messaging | PUBLISH/SUBSCRIBE |

### Best Practices

1. **Key Design**: Use namespaced keys (e.g., user:123:profile)
2. **Connection Pooling**: Reuse connections
3. **Error Handling**: Implement retry logic
4. **Circuit Breaker**: Handle failures gracefully
5. **Monitoring**: Track hit ratio, latency

## HOW

### Example 1: Implement Cache-Aside Pattern

```python
# Python cache-aside implementation
import redis
import json
from datetime import timedelta

class CacheAside:
    def __init__(self, redis_client, default_ttl=300):
        self.redis = redis_client
        self.default_ttl = default_ttl
    
    def get_user(self, user_id):
        """Get user with cache-aside pattern"""
        cache_key = f"user:{user_id}"
        
        # Check cache first
        cached = self.redis.get(cache_key)
        if cached:
            return json.loads(cached)
        
        # Fetch from database
        user = self.fetch_from_db(user_id)
        
        if user:
            # Store in cache
            self.redis.setex(
                cache_key,
                self.default_ttl,
                json.dumps(user)
            )
        
        return user
    
    def update_user(self, user_id, data):
        """Update user and invalidate cache"""
        # Update database first
        self.update_in_db(user_id, data)
        
        # Invalidate cache
        cache_key = f"user:{user_id}"
        self.redis.delete(cache_key)
    
    def fetch_from_db(self, user_id):
        """Simulate database fetch"""
        return {"id": user_id, "name": f"User {user_id}"}
    
    def update_in_db(self, user_id, data):
        """Simulate database update"""
        pass

# Usage
redis_client = redis.Redis(host='my-cluster.xxxx.use1.cache.amazonaws.com', port=6379)
cache = CacheAside(redis_client)
user = cache.get_user(123)
```

### Example 2: Implement Rate Limiter

```python
# Token bucket rate limiter
import time
import redis

class RateLimiter:
    def __init__(self, redis_client, max_requests, window_seconds):
        self.redis = redis_client
        self.max_requests = max_requests
        self.window_seconds = window_seconds
    
    def is_allowed(self, identifier):
        """Check if request is allowed"""
        key = f"ratelimit:{identifier}:{int(time.time() // self.window_seconds)}"
        
        # Increment counter
        current = self.redis.incr(key)
        
        # Set expiry on first request
        if current == 1:
            self.redis.expire(key, self.window_seconds)
        
        return current <= self.max_requests
    
    def get_remaining(self, identifier):
        """Get remaining requests"""
        key = f"ratelimit:{identifier}:{int(time.time() // self.window_seconds)}"
        current = int(self.redis.get(key) or 0)
        return max(0, self.max_requests - current)

# Usage
redis_client = redis.Redis(host='my-redis.xxxx.use1.cache.amazonaws.com')
limiter = RateLimiter(redis_client, 100, 60)  # 100 requests per minute

# Middleware example
def handle_request(request):
    identifier = request.ip
    
    if not limiter.is_allowed(identifier):
        return {"error": "Rate limit exceeded"}, 429
    
    return {"success": True}, 200
```

### Example 3: Implement Distributed Lock

```python
# Redis distributed lock
import redis
import time
import uuid

class DistributedLock:
    def __init__(self, redis_client):
        self.redis = redis_client
    
    def acquire(self, lock_name, timeout=30):
        """Acquire lock"""
        lock_key = f"lock:{lock_name}"
        lock_value = str(uuid.uuid4())
        
        # Try to set lock with NX (only if not exists)
        result = self.redis.set(
            lock_key,
            lock_value,
            nx=True,
            ex=timeout
        )
        
        return lock_value if result else None
    
    def release(self, lock_name, lock_value):
        """Release lock (using Lua for atomicity)"""
        lock_key = f"lock:{lock_name}"
        
        # Lua script for atomic check-and-delete
        lua_script = """
        if redis.call("get", KEYS[1]) == ARGV[1] then
            return redis.call("del", KEYS[1])
        else
            return 0
        end
        """
        
        return self.redis.eval(lua_script, 1, lock_key, lock_value)
    
    def execute_with_lock(self, lock_name, func, *args, **kwargs):
        """Execute function with lock"""
        lock_value = self.acquire(lock_name)
        
        if not lock_value:
            raise Exception(f"Could not acquire lock: {lock_name}")
        
        try:
            return func(*args, **kwargs)
        finally:
            self.release(lock_name, lock_value)

# Usage
redis_client = redis.Redis(host='my-redis.xxxx.use1.cache.amazonaws.com')
lock = DistributedLock(redis_client)

def process_order(order_id):
    with lock.acquire(f"order:{order_id}"):
        # Process order
        pass
```

### Example 4: Implement Leaderboard

```python
# Redis sorted set leaderboard
import redis
from typing import List, Dict

class Leaderboard:
    def __init__(self, redis_client, leaderboard_name):
        self.redis = redis_client
        self.name = leaderboard_name
    
    def set_score(self, user_id, score):
        """Set or update user score"""
        self.redis.zadd(self.name, {user_id: score})
    
    def increment_score(self, user_id, increment):
        """Increment user score"""
        self.redis.zincrby(self.name, increment, user_id)
    
    def get_rank(self, user_id, reverse=False):
        """Get user rank (0-indexed)"""
        if reverse:
            return self.redis.zrevrank(self.name, user_id)
        return self.redis.zrank(self.name, user_id)
    
    def get_top(self, count, with_scores=True):
        """Get top N users"""
        return self.redis.zrevrange(
            self.name, 0, count - 1, withscores=with_scores
        )
    
    def get_around(self, user_id, count):
        """Get users around a specific user"""
        rank = self.redis.zrevrank(self.name, user_id)
        if rank is None:
            return []
        
        start = max(0, rank - count // 2)
        end = rank + count // 2
        
        return self.redis.zrevrange(
            self.name, start, end, withscores=True
        )

# Usage
redis_client = redis.Redis(host='my-redis.xxxx.use1.cache.amazonaws.com')
leaderboard = Leaderboard(redis_client, "game_scores")

# Set scores
leaderboard.set_score("player1", 1000)
leaderboard.set_score("player2", 1500)
leaderboard.set_score("player3", 800)

# Get top 10
top_players = leaderboard.get_top(10)
# [('player2', 1500.0), ('player1', 1000.0), ('player3', 800.0)]

# Get player rank
rank = leaderboard.get_rank("player1")  # 1
```

## COMMON ISSUES

### 1. Cache Stampede

**Problem**: Multiple requests miss cache simultaneously.

**Solution**:
```python
# Use distributed lock for cache population
lock_value = lock.acquire(f"cache:{key}")
if lock_value:
    # Only one request populates cache
    data = fetch_from_db(key)
    cache.set(key, data)
    lock.release(f"cache:{key}", lock_value)
```

### 2. Cold Start

**Problem**: New cluster takes time to warm up.

**Solution**:
- Preload frequently accessed data
- Use read replicas for reads during primary rebuild
- Monitor cache hit ratio

### 3. Connection Exhaustion

**Problem**: Too many connections to Redis.

**Solution**:
```python
# Use connection pool
pool = redis.ConnectionPool(
    host='my-redis.xxxx.use1.cache.amazonaws.com',
    max_connections=50
)
redis_client = redis.Redis(connection_pool=pool)
```

## PERFORMANCE

### Performance Benchmarks

| Operation | Latency | Notes |
|-----------|---------|-------|
| SET | 0.5-1ms | Single key |
| GET | 0.5-1ms | Single key |
| MGET | 1-2ms | Multiple keys |
| ZADD | 0.5-1ms | Sorted set |
| HSET | 0.5-1ms | Hash |

### Optimization Tips

1. **Pipeline commands**: Batch multiple commands
2. **Use hashes**: For related data
3. **Compression**: For large values
4. **Serialization**: Use msgpack/protobuf

## COMPATIBILITY

### SDK Support

| Language | Library | Connection |
|----------|---------|-------------|
| Python | redis-py | ConnectionPool |
| Java | Jedis/Lettuce | JedisPool |
| Node.js | ioredis | Cluster |
| Go | go-redis | Client |

## CROSS-REFERENCES

### Related Patterns

- Cache-Aside: Database caching
- Write-Through: Synchronous caching
- Write-Behind: Async caching

### What to Study Next

1. Redis Streams: Message processing
2. Redis Graph: Graph database
3. Redis Search: Full-text search

## EXAM TIPS

### Key Exam Facts

- Cache-aside: Application manages caching
- Write-through: Synchronous with write
- TTL: Time-to-live for expiry
- Connection pooling: Required for production

### Exam Questions

- **Question**: "Prevent cache stampede" = Use lock
- **Question**: "Store related data" = Use hash
- **Question**: "Batch operations" = Use pipeline
