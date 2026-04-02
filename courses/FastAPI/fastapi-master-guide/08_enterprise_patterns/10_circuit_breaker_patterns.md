# Circuit Breaker Patterns

## Overview

Circuit breaker pattern prevents cascading failures in distributed systems.

## Implementation

### Circuit Breaker

```python
# Example 1: Circuit breaker implementation
from enum import Enum
from datetime import datetime, timedelta
from functools import wraps
from fastapi import FastAPI, HTTPException

app = FastAPI()

class CircuitState(Enum):
    CLOSED = "closed"      # Normal operation
    OPEN = "open"          # Failing, reject requests
    HALF_OPEN = "half_open"  # Testing recovery

class CircuitBreaker:
    """Circuit breaker for service calls"""

    def __init__(
        self,
        failure_threshold: int = 5,
        recovery_timeout: int = 30,
        expected_exceptions: tuple = (Exception,)
    ):
        self.failure_threshold = failure_threshold
        self.recovery_timeout = recovery_timeout
        self.expected_exceptions = expected_exceptions

        self.state = CircuitState.CLOSED
        self.failure_count = 0
        self.last_failure_time = None
        self.success_count = 0

    def __call__(self, func):
        @wraps(func)
        async def wrapper(*args, **kwargs):
            return await self.call(func, *args, **kwargs)
        return wrapper

    async def call(self, func, *args, **kwargs):
        """Execute function with circuit breaker"""
        if self.state == CircuitState.OPEN:
            if self._should_attempt_reset():
                self.state = CircuitState.HALF_OPEN
            else:
                raise HTTPException(
                    status_code=503,
                    detail="Service unavailable (circuit open)"
                )

        try:
            result = await func(*args, **kwargs)
            self._on_success()
            return result
        except self.expected_exceptions as e:
            self._on_failure()
            raise e

    def _on_success(self):
        """Handle successful call"""
        if self.state == CircuitState.HALF_OPEN:
            self.success_count += 1
            if self.success_count >= 3:
                self.state = CircuitState.CLOSED
                self.failure_count = 0
                self.success_count = 0
        else:
            self.failure_count = 0

    def _on_failure(self):
        """Handle failed call"""
        self.failure_count += 1
        self.last_failure_time = datetime.utcnow()

        if self.failure_count >= self.failure_threshold:
            self.state = CircuitState.OPEN

    def _should_attempt_reset(self):
        """Check if should attempt reset"""
        if self.last_failure_time is None:
            return True
        return datetime.utcnow() - self.last_failure_time > timedelta(
            seconds=self.recovery_timeout
        )

# Usage
user_service_breaker = CircuitBreaker(
    failure_threshold=5,
    recovery_timeout=30
)

@user_service_breaker
async def call_user_service(user_id: int):
    """Call user service with circuit breaker"""
    async with httpx.AsyncClient() as client:
        response = await client.get(
            f"http://user-service:8001/users/{user_id}"
        )
        response.raise_for_status()
        return response.json()

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Get user with circuit breaker protection"""
    return await call_user_service(user_id)
```

## Summary

Circuit breakers prevent cascading failures.

## Next Steps

Continue learning about:
- [Service Discovery](./09_service_discovery.md)
- [Distributed Tracing](./07_distributed_tracing.md)
