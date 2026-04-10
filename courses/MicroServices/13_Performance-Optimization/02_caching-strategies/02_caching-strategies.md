# Caching Strategies for Microservices

## Overview

Caching is one of the most effective techniques for improving microservices performance. By storing frequently accessed data in fast storage, services can dramatically reduce latency and decrease load on backend databases. In microservices architectures, caching must be carefully designed to handle the distributed nature of the system while maintaining data consistency.

Effective caching strategies consider multiple cache layers, cache invalidation patterns, TTL management, and cache population strategies. The choice of caching approach depends on data characteristics, consistency requirements, and performance needs.

This guide covers essential caching strategies for microservices, including multi-layer caching, distributed caching, cache invalidation patterns, and implementation best practices.

## Caching Layers

### 1. Application-Level Caching

Application-level caching stores data within the service process. This provides the fastest access times but is limited to a single instance's memory.

```python
# Example: In-memory cache implementation
from functools import lru_cache
import time

class InMemoryCache:
    def __init__(self, max_size=1000, ttl_seconds=300):
        self.cache = {}
        self.timestamps = {}
        self.max_size = max_size
        self.ttl_seconds = ttl_seconds
    
    def get(self, key):
        if key in self.cache:
            if time.time() - self.timestamps[key] < self.ttl_seconds:
                return self.cache[key]
            else:
                del self.cache[key]
                del self.timestamps[key]
        return None
    
    def set(self, key, value):
        if len(self.cache) >= self.max_size:
            # Evict oldest entry
            oldest_key = min(self.timestamps, key=self.timestamps.get)
            del self.cache[oldest_key]
            del self.timestamps[oldest_key]
        
        self.cache[key] = value
        self.timestamps[key] = time.time()
    
    def invalidate(self, key):
        if key in self.cache:
            del self.cache[key]
            del self.timestamps[key]
```

### 2. Distributed Caching

Distributed caching provides shared caching across multiple service instances. Redis and Memcached are popular choices for distributed caching in microservices architectures.

```python
# Example: Redis-based distributed cache
import redis
import json

class DistributedCache:
    def __init__(self, redis_url: str):
        self.client = redis.from_url(redis_url)
    
    def get(self, key: str):
        value = self.client.get(key)
        if value:
            return json.loads(value)
        return None
    
    def set(self, key: str, value, ttl_seconds: int = 300):
        self.client.setex(
            key, 
            ttl_seconds, 
            json.dumps(value)
        )
    
    def delete(self, key: str):
        self.client.delete(key)
    
    def invalidate_pattern(self, pattern: str):
        keys = self.client.keys(pattern)
        if keys:
            self.client.delete(*keys)
```

## Cache Invalidation Strategies

### 1. Time-Based Invalidation (TTL)

Simple approach where cached data expires after a specified time:

```python
# TTL-based cache with automatic expiration
class TTLBasedCache:
    def __init__(self, default_ttl=300):
        self.cache = {}
        self.default_ttl = default_ttl
    
    def get(self, key):
        entry = self.cache.get(key)
        if entry and entry['expires_at'] > time.time():
            return entry['value']
        return None
```

### 2. Event-Based Invalidation

Invalidate cache when data changes:

```python
# Event-driven cache invalidation
class EventBasedCache:
    def __init__(self, cache_client):
        self.cache = cache_client
        self.subscribers = {}
    
    def subscribe(self, event_type, callback):
        if event_type not in self.subscribers:
            self.subscribers[event_type] = []
        self.subscribers[event_type].append(callback)
    
    def publish(self, event_type, data):
        if event_type in self.subscribers:
            for callback in self.subscribers[event_type]:
                callback(data)
    
    def invalidate_on_change(self, key_prefix, event_type):
        def handler(event_data):
            if event_data.get('type') == 'update':
                self.cache.delete(f"{key_prefix}:{event_data['id']}")
        self.subscribe(event_type, handler)
```

### 3. Write-Through Caching

Update cache when data is written:

```python
# Write-through cache implementation
class WriteThroughCache:
    def __init__(self, cache, database):
        self.cache = cache
        self.database = database
    
    def write(self, key, value):
        # Write to database first
        self.database.save(key, value)
        # Then update cache
        self.cache.set(key, value)
    
    def read(self, key):
        # Try cache first
        cached = self.cache.get(key)
        if cached:
            return cached
        # Fall back to database
        value = self.database.load(key)
        if value:
            self.cache.set(key, value)
        return value
```

## Implementation Example

```python
#!/usr/bin/env python3
"""
Multi-Layer Caching System for Microservices
"""

from dataclasses import dataclass, field
from typing import Dict, Optional, Any, Callable
from enum import Enum
import time
import threading


class CacheLayer(Enum):
    L1_MEMORY = "l1_memory"
    L2_DISTRIBUTED = "l2_distributed"
    L3_DATABASE = "l3_database"


class CacheStrategy(Enum):
    CACHE_FIRST = "cache_first"
    WRITE_THROUGH = "write_through"
    WRITE_BEHIND = "write_behind"


@dataclass
class CacheEntry:
    """Represents a cached item"""
    key: str
    value: Any
    created_at: float
    expires_at: float
    hit_count: int = 0


@dataclass
class CacheMetrics:
    """Cache performance metrics"""
    hits: int = 0
    misses: int = 0
    evictions: int = 0
    hit_rate: float = 0.0


class MultiLayerCache:
    """Multi-layer caching system for microservices"""
    
    def __init__(
        self,
        strategy: CacheStrategy = CacheStrategy.CACHE_FIRST,
        l1_size: int = 1000,
        l1_ttl: int = 60,
        l2_ttl: int = 300
    ):
        self.strategy = strategy
        self.l1_cache: Dict[str, CacheEntry] = {}
        self.l2_cache: Optional[Any] = None  # Would be Redis in production
        self.l1_size = l1_size
        self.l1_ttl = l1_ttl
        self.l2_ttl = l2_ttl
        self.metrics = CacheMetrics()
        self.lock = threading.RLock()
    
    def get(self, key: str) -> Optional[Any]:
        """Get value from cache"""
        # Check L1 (memory)
        entry = self.l1_cache.get(key)
        if entry and entry.expires_at > time.time():
            entry.hit_count += 1
            self.metrics.hits += 1
            return entry.value
        
        # Check L2 (distributed)
        if self.l2_cache:
            l2_value = self.l2_cache.get(key)
            if l2_value:
                # Promote to L1
                self._promote_to_l1(key, l2_value)
                self.metrics.hits += 1
                return l2_value
        
        self.metrics.misses += 1
        return None
    
    def set(self, key: str, value: Any):
        """Set value in cache"""
        with self.lock:
            now = time.time()
            
            entry = CacheEntry(
                key=key,
                value=value,
                created_at=now,
                expires_at=now + self.l1_ttl
            )
            
            # L1 cache
            if len(self.l1_cache) >= self.l1_size:
                self._evict_l1()
            
            self.l1_cache[key] = entry
            
            # L2 cache
            if self.l2_cache:
                self.l2_cache.set(key, value, self.l2_ttl)
    
    def _promote_to_l1(self, key: str, value: Any):
        """Promote value from L2 to L1"""
        now = time.time()
        
        if len(self.l1_cache) >= self.l1_size:
            self._evict_l1()
        
        self.l1_cache[key] = CacheEntry(
            key=key,
            value=value,
            created_at=now,
            expires_at=now + self.l1_ttl
        )
    
    def _evict_l1(self):
        """Evict least recently used entry from L1"""
        if not self.l1_cache:
            return
        
        # Find LRU entry
        lru_key = min(
            self.l1_cache.keys(),
            key=lambda k: self.l1_cache[k].created_at
        )
        
        del self.l1_cache[lru_key]
        self.metrics.evictions += 1
    
    def invalidate(self, key: str):
        """Invalidate cache entry"""
        with self.lock:
            if key in self.l1_cache:
                del self.l1_cache[key]
            
            if self.l2_cache:
                self.l2_cache.delete(key)
    
    def get_metrics(self) -> Dict:
        """Get cache metrics"""
        total = self.metrics.hits + self.metrics.misses
        if total > 0:
            self.metrics.hit_rate = self.metrics.hits / total
        
        return {
            "hits": self.metrics.hits,
            "misses": self.metrics.misses,
            "evictions": self.metrics.evictions,
            "hit_rate": f"{self.metrics.hit_rate * 100:.2f}%",
            "l1_size": len(self.l1_cache),
            "l1_max_size": self.l1_size
        }


class CacheDecorator:
    """Decorator for adding caching to functions"""
    
    def __init__(self, cache: MultiLayerCache):
        self.cache = cache
    
    def cached(self, ttl: int = 60):
        """Decorator to cache function results"""
        def decorator(func: Callable):
            def wrapper(*args, **kwargs):
                # Generate cache key from function name and args
                cache_key = f"{func.__name__}:{str(args)}:{str(kwargs)}"
                
                # Try to get from cache
                result = self.cache.get(cache_key)
                if result is not None:
                    return result
                
                # Execute function
                result = func(*args, **kwargs)
                
                # Store in cache
                self.cache.set(cache_key, result)
                
                return result
            return wrapper
        return decorator


# Example usage
if __name__ == "__main__":
    # Create cache system
    cache = MultiLayerCache(
        strategy=CacheStrategy.CACHE_FIRST,
        l1_size=100,
        l1_ttl=60,
        l2_ttl=300
    )
    
    # Test cache operations
    cache.set("user:123", {"name": "John", "email": "john@example.com"})
    result = cache.get("user:123")
    print(f"Cache result: {result}")
    
    # Get metrics
    metrics = cache.get_metrics()
    print(f"Cache metrics: {metrics}")
```

## Best Practices

1. **Use Multi-Layer Caching**: Combine L1 (memory) and L2 (distributed) caches for optimal performance.

2. **Choose Appropriate TTL**: Set TTL based on data volatility and consistency requirements.

3. **Implement Cache Invalidation**: Choose between TTL-based, event-based, or write-through strategies.

4. **Monitor Cache Performance**: Track hit rates, miss rates, and eviction rates.

5. **Handle Cache Failures Gracefully**: Fall back to primary data source if cache is unavailable.

---

## Output Statement

```
Caching System Report
=====================
Service: user-service
Cache Strategy: CACHE_FIRST

Performance Metrics:
- Hit Rate: 85.3%
- Total Hits: 8,530
- Total Misses: 1,470
- Evictions: 230

L1 Cache Status:
- Current Size: 100/100
- TTL: 60 seconds

L2 Cache Status:
- Connected: Yes
- TTL: 300 seconds

Recommendations:
1. Consider increasing L1 cache size
2. Review TTL settings for user preferences
3. Implement cache warming for frequently accessed data
```