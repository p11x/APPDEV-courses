# API Load Testing

## Overview

Load testing ensures APIs can handle expected traffic volumes.

## Load Testing with Locust

### Basic Load Test

```python
# Example 1: Locust load test
# locustfile.py
from locust import HttpUser, task, between

class FastAPIUser(HttpUser):
    wait_time = between(1, 3)

    def on_start(self):
        response = self.client.post("/login", json={
            "username": "loadtest",
            "password": "password123"
        })
        self.token = response.json()["access_token"]

    @task(3)
    def get_items(self):
        self.client.get("/items/", headers={
            "Authorization": f"Bearer {self.token}"
        })

    @task(1)
    def create_item(self):
        self.client.post("/items/", json={
            "name": "Test Item",
            "price": 10.0
        }, headers={
            "Authorization": f"Bearer {self.token}"
        })
```

### Running Load Tests

```bash
# Run load test
locust -f locustfile.py --host=http://localhost:8000

# Headless mode
locust -f locustfile.py --host=http://localhost:8000 \
    --users 100 --spawn-rate 10 --run-time 60s
```

## Summary

Load testing validates API performance under load.

## Next Steps

Continue learning about:
- [API Security Testing](./11_api_security_testing.md)
- [API Performance Testing](./09_api_performance_testing.md)
