# Performance Testing

## Overview

Performance testing ensures your FastAPI application can handle expected load and identifies bottlenecks before they affect users.

## Load Testing Setup

### Locust Configuration

```python
# Example 1: Locust load testing
# locustfile.py
from locust import HttpUser, task, between
import json

class FastAPIUser(HttpUser):
    """Simulated user for load testing"""
    wait_time = between(1, 3)  # Wait 1-3 seconds between tasks

    def on_start(self):
        """Login when user starts"""
        response = self.client.post("/auth/login", json={
            "username": "loadtest",
            "password": "testpassword123"
        })
        self.token = response.json()["access_token"]
        self.headers = {"Authorization": f"Bearer {self.token}"}

    @task(3)  # Weight: run 3x more often
    def get_users(self):
        """Test GET endpoint"""
        self.client.get("/users/", headers=self.headers)

    @task(2)
    def get_user_detail(self):
        """Test parameterized GET"""
        self.client.get("/users/1", headers=self.headers)

    @task(1)
    def create_user(self):
        """Test POST endpoint"""
        import uuid
        self.client.post("/users/", json={
            "username": f"user_{uuid.uuid4().hex[:8]}",
            "email": f"user_{uuid.uuid4().hex[:8]}@test.com",
            "password": "LoadTestPass123!"
        }, headers=self.headers)

# Run with: locust -f locustfile.py --host=http://localhost:8000
```

### HTTPX Load Testing

```python
# Example 2: Async load testing with httpx
import asyncio
import httpx
import time
from dataclasses import dataclass
from typing import List

@dataclass
class LoadTestResult:
    total_requests: int
    successful: int
    failed: int
    avg_response_time: float
    min_response_time: float
    max_response_time: float
    requests_per_second: float

async def make_request(client: httpx.AsyncClient, url: str) -> tuple[bool, float]:
    """Make single request and return success/time"""
    start = time.time()
    try:
        response = await client.get(url)
        elapsed = time.time() - start
        return response.status_code == 200, elapsed
    except Exception:
        return False, time.time() - start

async def load_test(
    url: str,
    num_requests: int = 100,
    concurrency: int = 10
) -> LoadTestResult:
    """Run load test"""
    start_time = time.time()
    results: List[tuple[bool, float]] = []

    async with httpx.AsyncClient() as client:
        # Create semaphore for concurrency control
        semaphore = asyncio.Semaphore(concurrency)

        async def bounded_request():
            async with semaphore:
                return await make_request(client, url)

        # Run all requests
        tasks = [bounded_request() for _ in range(num_requests)]
        results = await asyncio.gather(*tasks)

    # Calculate statistics
    total_time = time.time() - start_time
    successful = sum(1 for success, _ in results if success)
    failed = num_requests - successful
    response_times = [elapsed for _, elapsed in results]

    return LoadTestResult(
        total_requests=num_requests,
        successful=successful,
        failed=failed,
        avg_response_time=sum(response_times) / len(response_times),
        min_response_time=min(response_times),
        max_response_time=max(response_times),
        requests_per_second=num_requests / total_time
    )

# Usage
async def run_load_test():
    result = await load_test(
        "http://localhost:8000/users/",
        num_requests=1000,
        concurrency=50
    )

    print(f"Total requests: {result.total_requests}")
    print(f"Successful: {result.successful}")
    print(f"Failed: {result.failed}")
    print(f"Avg response time: {result.avg_response_time:.3f}s")
    print(f"Requests/second: {result.requests_per_second:.1f}")
```

## Response Time Testing

### Endpoint Performance

```python
# Example 3: Response time tests
import pytest
import time
from fastapi.testclient import TestClient

class TestResponseTime:
    """Test endpoint response times"""

    @pytest.fixture
    def client(self):
        return TestClient(app)

    def test_root_response_time(self, client):
        """Test root endpoint performance"""
        start = time.time()
        response = client.get("/")
        elapsed = time.time() - start

        assert response.status_code == 200
        assert elapsed < 0.1  # Under 100ms

    def test_users_list_response_time(self, client):
        """Test list endpoint performance"""
        start = time.time()
        response = client.get("/users/")
        elapsed = time.time() - start

        assert response.status_code == 200
        assert elapsed < 0.5  # Under 500ms

    def test_user_detail_response_time(self, client):
        """Test detail endpoint performance"""
        start = time.time()
        response = client.get("/users/1")
        elapsed = time.time() - start

        assert response.status_code in [200, 404]
        assert elapsed < 0.2  # Under 200ms

    def test_create_response_time(self, client):
        """Test create endpoint performance"""
        import uuid
        start = time.time()
        response = client.post("/users/", json={
            "username": f"perf_{uuid.uuid4().hex[:8]}",
            "email": f"perf_{uuid.uuid4().hex[:8]}@test.com",
            "password": "PerfTestPass123!"
        })
        elapsed = time.time() - start

        assert response.status_code == 201
        assert elapsed < 1.0  # Under 1 second
```

## Concurrency Testing

### Concurrent Request Handling

```python
# Example 4: Concurrency tests
import asyncio
import pytest
from httpx import AsyncClient, ASGITransport

class TestConcurrency:
    """Test concurrent request handling"""

    @pytest.fixture
    async def async_client(self):
        transport = ASGITransport(app=app)
        async with AsyncClient(transport=transport, base_url="http://test") as client:
            yield client

    @pytest.mark.asyncio
    async def test_concurrent_reads(self, async_client):
        """Test multiple concurrent reads"""
        async def make_request():
            response = await async_client.get("/users/")
            return response.status_code

        # Make 10 concurrent requests
        tasks = [make_request() for _ in range(10)]
        results = await asyncio.gather(*tasks)

        # All should succeed
        assert all(status == 200 for status in results)

    @pytest.mark.asyncio
    async def test_concurrent_writes(self, async_client):
        """Test multiple concurrent writes"""
        import uuid

        async def create_user(index: int):
            response = await async_client.post("/users/", json={
                "username": f"concurrent_{index}_{uuid.uuid4().hex[:8]}",
                "email": f"concurrent_{index}_{uuid.uuid4().hex[:8]}@test.com",
                "password": "ConcurrentTest123!"
            })
            return response.status_code

        # Make 5 concurrent creates
        tasks = [create_user(i) for i in range(5)]
        results = await asyncio.gather(*tasks)

        # All should succeed
        assert all(status == 201 for status in results)

    @pytest.mark.asyncio
    async def test_race_condition(self, async_client):
        """Test for race conditions"""
        # Create a resource
        response = await async_client.post("/items/", json={
            "name": "limited",
            "quantity": 1
        })
        item_id = response.json()["id"]

        async def try_purchase():
            response = await async_client.post(
                f"/items/{item_id}/purchase"
            )
            return response.status_code

        # Try to purchase same item concurrently
        tasks = [try_purchase() for _ in range(5)]
        results = await asyncio.gather(*tasks)

        # Only one should succeed (409 for others)
        success_count = sum(1 for status in results if status == 200)
        assert success_count == 1
```

## Memory Testing

### Memory Usage Tests

```python
# Example 5: Memory tests
import pytest
import psutil
import os

class TestMemoryUsage:
    """Test memory consumption"""

    @pytest.fixture
    def memory_monitor(self):
        """Monitor memory usage"""
        process = psutil.Process(os.getpid())
        return process.memory_info().rss

    def test_list_endpoint_memory(self, client, memory_monitor):
        """Test memory usage of list endpoint"""
        initial_memory = memory_monitor

        # Make multiple requests
        for _ in range(100):
            client.get("/users/")

        # Memory shouldn't grow significantly
        final_memory = memory_monitor
        memory_growth = final_memory - initial_memory

        # Allow 10MB growth
        assert memory_growth < 10 * 1024 * 1024

    def test_large_response_memory(self, client):
        """Test handling large responses"""
        # Assuming endpoint returns paginated results
        response = client.get("/users/?limit=1000")

        assert response.status_code == 200
        # Response should be paginated, not all at once
        assert len(response.json()) <= 100
```

## Database Performance

### Query Performance

```python
# Example 6: Database performance tests
import time
from sqlalchemy import event

class TestDatabasePerformance:
    """Test database query performance"""

    @pytest.fixture
    def query_counter(self, db_session):
        """Count queries executed"""
        queries = []

        @event.listens_for(db_session, "before_cursor_execute")
        def receive_before_cursor_execute(
            conn, cursor, statement, parameters, context, executemany
        ):
            queries.append(statement)

        return queries

    def test_no_n_plus_one_queries(self, client, query_counter):
        """Test for N+1 query problem"""
        response = client.get("/users/")

        assert response.status_code == 200

        # Should be 1-2 queries, not N+1
        assert len(query_counter) <= 2

    def test_efficient_pagination(self, client, query_counter):
        """Test efficient pagination"""
        response = client.get("/users/?skip=0&limit=20")

        assert response.status_code == 200
        # Should use LIMIT, not fetch all
        assert any("LIMIT" in q.upper() for q in query_counter)

    def test_query_time(self, db_session):
        """Test individual query time"""
        start = time.time()
        users = db_session.query(User).limit(100).all()
        elapsed = time.time() - start

        assert elapsed < 0.1  # Under 100ms
```

## Stress Testing

### Finding Limits

```python
# Example 7: Stress tests
import asyncio
from typing import List

async def stress_test(
    url: str,
    max_concurrent: int = 100,
    duration_seconds: int = 60
) -> dict:
    """Stress test to find breaking point"""
    import httpx
    import time

    results = {
        "concurrent": 0,
        "success_rate": 0,
        "avg_response_time": 0,
        "errors": []
    }

    for concurrency in range(10, max_concurrent + 1, 10):
        print(f"Testing with {concurrency} concurrent requests...")

        async with httpx.AsyncClient() as client:
            start = time.time()
            success = 0
            failed = 0
            response_times = []

            while time.time() - start < duration_seconds:
                tasks = []
                for _ in range(concurrency):
                    req_start = time.time()
                    try:
                        response = await client.get(url)
                        elapsed = time.time() - req_start
                        response_times.append(elapsed)

                        if response.status_code == 200:
                            success += 1
                        else:
                            failed += 1
                    except Exception as e:
                        failed += 1
                        results["errors"].append(str(e))

                await asyncio.sleep(0.1)  # Brief pause

            success_rate = success / (success + failed) * 100
            avg_time = sum(response_times) / len(response_times) if response_times else 0

            print(f"  Success rate: {success_rate:.1f}%")
            print(f"  Avg response time: {avg_time:.3f}s")

            if success_rate < 95:
                print(f"Breaking point found at {concurrency} concurrent requests")
                break

            results["concurrent"] = concurrency
            results["success_rate"] = success_rate
            results["avg_response_time"] = avg_time

    return results
```

## Best Practices

### Performance Testing Guidelines

```python
# Example 8: Performance testing best practices
"""
Performance Testing Best Practices:

1. Establish baselines
   - Measure current performance
   - Set target metrics
   - Track over time

2. Test realistic scenarios
   - Use production-like data
   - Simulate real user behavior
   - Test peak load patterns

3. Monitor all resources
   - CPU usage
   - Memory consumption
   - Database connections
   - Network I/O

4. Test incrementally
   - Start with low load
   - Gradually increase
   - Find breaking points

5. Automate tests
   - Run in CI/CD
   - Track trends
   - Alert on regression

6. Profile bottlenecks
   - Use profiling tools
   - Identify slow queries
   - Find memory leaks
"""

# Performance test configuration
PERFORMANCE_THRESHOLDS = {
    "response_time_p95": 0.5,  # 95th percentile under 500ms
    "response_time_p99": 1.0,  # 99th percentile under 1s
    "success_rate": 99.0,       # 99% success rate
    "requests_per_second": 100, # At least 100 RPS
    "error_rate": 1.0           # Less than 1% errors
}
```

## Summary

| Test Type | Purpose | Tool |
|-----------|---------|------|
| Load Testing | Normal load | Locust, httpx |
| Stress Testing | Breaking points | Custom scripts |
| Concurrency | Parallel handling | asyncio |
| Memory | Resource usage | psutil |
| Database | Query performance | SQLAlchemy events |

## Next Steps

Continue learning about:
- [API Performance Testing](../03_api_testing/09_api_performance_testing.md)
- [Load Testing](../03_api_testing/10_api_load_testing.md)
- [Security Testing](../07_security_testing/01_security_testing_overview.md)
