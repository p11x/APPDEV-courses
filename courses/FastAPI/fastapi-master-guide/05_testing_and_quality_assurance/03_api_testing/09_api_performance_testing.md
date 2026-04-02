# API Performance Testing

## Overview

API performance testing ensures FastAPI applications meet response time and throughput requirements.

## Response Time Testing

### Endpoint Performance

```python
# Example 1: Response time tests
import pytest
import time
from fastapi.testclient import TestClient

class TestResponseTime:
    """Test API response times"""

    def test_root_response_time(self, client: TestClient):
        """Test root endpoint performance"""
        start = time.time()
        response = client.get("/")
        duration = time.time() - start

        assert response.status_code == 200
        assert duration < 0.1  # Under 100ms

    def test_list_endpoint_response_time(self, client: TestClient):
        """Test list endpoint performance"""
        start = time.time()
        response = client.get("/items/")
        duration = time.time() - start

        assert response.status_code == 200
        assert duration < 0.5  # Under 500ms

    def test_search_response_time(self, client: TestClient):
        """Test search endpoint performance"""
        start = time.time()
        response = client.get("/items/search?q=test")
        duration = time.time() - start

        assert response.status_code == 200
        assert duration < 1.0  # Under 1 second
```

## Concurrent Request Testing

```python
# Example 2: Concurrency tests
import asyncio
import pytest
from httpx import AsyncClient

@pytest.mark.asyncio
async def test_concurrent_requests():
    """Test handling concurrent requests"""
    async with AsyncClient(app=app, base_url="http://test") as client:
        tasks = [client.get("/items/") for _ in range(50)]
        responses = await asyncio.gather(*tasks)

    success_count = sum(1 for r in responses if r.status_code == 200)
    assert success_count >= 48  # 96% success rate
```

## Load Testing

```python
# Example 3: Basic load test
import httpx
import asyncio
import time

async def load_test(url: str, num_requests: int = 100):
    """Simple load test"""
    async with httpx.AsyncClient() as client:
        start = time.time()

        tasks = [client.get(url) for _ in range(num_requests)]
        responses = await asyncio.gather(*tasks)

        duration = time.time() - start

    successful = sum(1 for r in responses if r.status_code == 200)

    return {
        "total": num_requests,
        "successful": successful,
        "duration": duration,
        "rps": num_requests / duration
    }
```

## Summary

Performance testing ensures APIs meet response time and throughput requirements.

## Next Steps

Continue learning about:
- [API Security Testing](./11_api_security_testing.md)
- [Load Testing](./10_api_load_testing.md)
