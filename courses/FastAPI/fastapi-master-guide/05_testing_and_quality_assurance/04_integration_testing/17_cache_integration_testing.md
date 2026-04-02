# Cache Integration Testing

## Overview

Testing cache integration ensures cached data is correctly stored, retrieved, and invalidated.

## Redis Cache Tests

### Testing Cache Operations

```python
# Example 1: Redis cache testing
import pytest
from unittest.mock import AsyncMock, patch
import redis.asyncio as redis

@pytest.fixture
async def redis_client():
    """Redis test client"""
    client = redis.Redis(host="localhost", port=6379, decode_responses=True)
    yield client
    await client.flushdb()
    await client.close()

@pytest.mark.asyncio
async def test_cache_set_get(redis_client):
    """Test cache set and get"""
    await redis_client.set("key", "value", ex=60)
    result = await redis_client.get("key")
    assert result == "value"

@pytest.mark.asyncio
async def test_cache_expiration(redis_client):
    """Test cache expiration"""
    await redis_client.set("key", "value", ex=1)
    import asyncio
    await asyncio.sleep(2)
    result = await redis_client.get("key")
    assert result is None

@pytest.mark.asyncio
async def test_cache_invalidation(redis_client):
    """Test cache invalidation"""
    await redis_client.set("key", "value")
    await redis_client.delete("key")
    result = await redis_client.get("key")
    assert result is None
```

### Mock Cache Testing

```python
# Example 2: Mock cache for unit tests
@pytest.fixture
def mock_cache():
    """Mock cache client"""
    cache = {}
    async def get(key):
        return cache.get(key)
    async def set(key, value, ex=None):
        cache[key] = value
    async def delete(key):
        cache.pop(key, None)

    return AsyncMock(get=get, set=set, delete=delete)

@pytest.mark.asyncio
async def test_service_with_cache(mock_cache):
    """Test service using cache"""
    service = UserService(cache=mock_cache)

    await service.get_user(1)  # Cache miss
    mock_cache.set.assert_called_once()

    await service.get_user(1)  # Cache hit
    mock_cache.get.assert_called()
```

## Summary

Cache testing ensures reliable data caching and invalidation.

## Next Steps

Continue learning about:
- [Database Transaction Testing](./18_database_transaction_testing.md)
- [Integration Testing](./08_integration_testing.md)
