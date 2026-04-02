# Rate Limiting

## Overview

Rate limiting protects APIs from abuse, ensures fair usage, and maintains service availability. This guide covers multiple rate limiting strategies.

## Rate Limiting Algorithms

### Token Bucket Algorithm

```python
# Example 1: Token bucket rate limiter
from fastapi import FastAPI, HTTPException, Request
import time
from collections import defaultdict

app = FastAPI()

class TokenBucket:
    """
    Token bucket algorithm for rate limiting.
    Tokens are added at a fixed rate, consumed per request.
    """

    def __init__(self, capacity: int, refill_rate: float):
        """
        Args:
            capacity: Maximum tokens
            refill_rate: Tokens added per second
        """
        self.capacity = capacity
        self.refill_rate = refill_rate
        self.tokens: dict[str, float] = defaultdict(lambda: capacity)
        self.last_refill: dict[str, float] = defaultdict(time.time)

    def consume(self, key: str, tokens: int = 1) -> bool:
        """
        Try to consume tokens for a request.
        Returns True if allowed, False if rate limited.
        """
        now = time.time()

        # Refill tokens
        elapsed = now - self.last_refill[key]
        self.tokens[key] = min(
            self.capacity,
            self.tokens[key] + elapsed * self.refill_rate
        )
        self.last_refill[key] = now

        # Check if enough tokens
        if self.tokens[key] >= tokens:
            self.tokens[key] -= tokens
            return True

        return False

    def get_wait_time(self, key: str) -> float:
        """Get time until next token is available"""
        if self.tokens[key] >= 1:
            return 0
        return (1 - self.tokens[key]) / self.refill_rate

# Create rate limiter: 100 requests per minute
limiter = TokenBucket(capacity=100, refill_rate=100/60)

@app.middleware("http")
async def rate_limit_middleware(request: Request, call_next):
    """Rate limit by IP address"""
    client_ip = request.client.host

    if not limiter.consume(client_ip):
        wait_time = limiter.get_wait_time(client_ip)
        raise HTTPException(
            status_code=429,
            detail="Rate limit exceeded",
            headers={
                "Retry-After": str(int(wait_time) + 1),
                "X-RateLimit-Remaining": "0"
            }
        )

    response = await call_next(request)
    return response

@app.get("/items/")
async def list_items():
    return {"items": []}
```

### Sliding Window Algorithm

```python
# Example 2: Sliding window rate limiter
from fastapi import FastAPI, HTTPException, Request
import time
from collections import defaultdict
from typing import Optional

app = FastAPI()

class SlidingWindowLimiter:
    """
    Sliding window rate limiter.
    More accurate than fixed window.
    """

    def __init__(self, max_requests: int, window_seconds: int):
        self.max_requests = max_requests
        self.window = window_seconds
        self.requests: dict[str, list[float]] = defaultdict(list)

    def is_allowed(self, key: str) -> tuple[bool, dict]:
        """
        Check if request is allowed.
        Returns (allowed, rate_limit_info)
        """
        now = time.time()
        window_start = now - self.window

        # Remove old requests
        self.requests[key] = [
            t for t in self.requests[key] if t > window_start
        ]

        # Check limit
        current_count = len(self.requests[key])

        if current_count >= self.max_requests:
            return False, {
                "limit": self.max_requests,
                "remaining": 0,
                "reset": int(self.requests[key][0] + self.window)
            }

        # Record request
        self.requests[key].append(now)

        return True, {
            "limit": self.max_requests,
            "remaining": self.max_requests - current_count - 1,
            "reset": int(now + self.window)
        }

# 100 requests per minute
limiter = SlidingWindowLimiter(max_requests=100, window_seconds=60)

@app.get("/items/")
async def list_items(request: Request):
    """Endpoint with sliding window rate limiting"""
    client_ip = request.client.host
    allowed, info = limiter.is_allowed(client_ip)

    if not allowed:
        raise HTTPException(
            status_code=429,
            detail="Rate limit exceeded",
            headers={
                "X-RateLimit-Limit": str(info["limit"]),
                "X-RateLimit-Remaining": "0",
                "X-RateLimit-Reset": str(info["reset"])
            }
        )

    return {"items": [], "rate_limit": info}
```

### Fixed Window Algorithm

```python
# Example 3: Fixed window rate limiter
from fastapi import FastAPI, HTTPException, Request
import time
from collections import defaultdict

app = FastAPI()

class FixedWindowLimiter:
    """
    Fixed window rate limiter.
    Simple but can allow bursts at window boundaries.
    """

    def __init__(self, max_requests: int, window_seconds: int):
        self.max_requests = max_requests
        self.window = window_seconds
        self.windows: dict[str, tuple[int, int]] = {}

    def is_allowed(self, key: str) -> bool:
        """Check if request is allowed"""
        now = time.time()
        window_num = int(now // self.window)

        if key not in self.windows or self.windows[key][0] != window_num:
            self.windows[key] = (window_num, 1)
            return True

        _, count = self.windows[key]
        if count >= self.max_requests:
            return False

        self.windows[key] = (window_num, count + 1)
        return True

limiter = FixedWindowLimiter(max_requests=100, window_seconds=60)

@app.get("/items/")
async def list_items(request: Request):
    if not limiter.is_allowed(request.client.host):
        raise HTTPException(429, "Rate limit exceeded")
    return {"items": []}
```

## Rate Limiting Strategies

### IP-Based Rate Limiting

```python
# Example 4: IP-based rate limiting
from fastapi import FastAPI, Request, HTTPException
from collections import defaultdict
import time

app = FastAPI()

class IPRateLimiter:
    """Rate limit by IP address"""

    def __init__(self, requests_per_minute: int = 60):
        self.limit = requests_per_minute
        self.requests: dict[str, list[float]] = defaultdict(list)

    def check(self, ip: str) -> dict:
        """Check rate limit for IP"""
        now = time.time()
        window_start = now - 60

        # Clean old requests
        self.requests[ip] = [
            t for t in self.requests[ip] if t > window_start
        ]

        remaining = self.limit - len(self.requests[ip])

        if remaining <= 0:
            return {
                "allowed": False,
                "remaining": 0,
                "retry_after": int(self.requests[ip][0] + 60 - now) + 1
            }

        self.requests[ip].append(now)
        return {"allowed": True, "remaining": remaining - 1}

ip_limiter = IPRateLimiter(requests_per_minute=100)

@app.middleware("http")
async def ip_rate_limit(request: Request, call_next):
    """IP-based rate limiting middleware"""
    result = ip_limiter.check(request.client.host)

    if not result["allowed"]:
        raise HTTPException(
            status_code=429,
            detail="Too many requests",
            headers={"Retry-After": str(result["retry_after"])}
        )

    response = await call_next(request)
    response.headers["X-RateLimit-Remaining"] = str(result["remaining"])
    return response
```

### User-Based Rate Limiting

```python
# Example 5: User-based rate limiting
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBearer
from jose import jwt

app = FastAPI()

class UserRateLimiter:
    """Rate limit by authenticated user"""

    def __init__(self):
        self.limits = {
            "free": 100,      # 100 requests per hour
            "pro": 1000,      # 1000 requests per hour
            "enterprise": 10000  # 10000 requests per hour
        }
        self.requests: dict[int, list[float]] = defaultdict(list)

    def check(self, user_id: int, tier: str) -> bool:
        """Check rate limit for user"""
        limit = self.limits.get(tier, self.limits["free"])
        now = time.time()
        hour_ago = now - 3600

        # Clean old requests
        self.requests[user_id] = [
            t for t in self.requests[user_id] if t > hour_ago
        ]

        if len(self.requests[user_id]) >= limit:
            return False

        self.requests[user_id].append(now)
        return True

user_limiter = UserRateLimiter()

async def get_current_user(token: str = Depends(HTTPBearer())):
    """Get authenticated user"""
    payload = jwt.decode(token.credentials, SECRET_KEY, algorithms=["HS256"])
    return {"id": payload["sub"], "tier": payload.get("tier", "free")}

@app.get("/items/")
async def list_items(user: dict = Depends(get_current_user)):
    """User-rate-limited endpoint"""
    if not user_limiter.check(user["id"], user["tier"]):
        raise HTTPException(
            status_code=429,
            detail=f"Rate limit exceeded for {user['tier']} tier"
        )

    return {"items": []}
```

### Endpoint-Specific Rate Limiting

```python
# Example 6: Per-endpoint rate limits
from fastapi import FastAPI, Request
from functools import wraps

app = FastAPI()

class EndpointRateLimiter:
    """Different limits per endpoint"""

    def __init__(self):
        self.limits: dict[str, int] = {
            "/api/search": 30,      # Expensive operation
            "/api/items": 100,      # Standard
            "/api/health": 1000,    # High limit
        }
        self.requests: dict[str, dict[str, list[float]]] = defaultdict(
            lambda: defaultdict(list)
        )

    def limit(self, endpoint: str):
        """Decorator for endpoint rate limiting"""
        def decorator(func):
            @wraps(func)
            async def wrapper(request: Request, *args, **kwargs):
                key = f"{request.client.host}:{endpoint}"
                limit = self.limits.get(endpoint, 60)

                now = time.time()
                hour_ago = now - 3600

                self.requests[endpoint][request.client.host] = [
                    t for t in self.requests[endpoint][request.client.host]
                    if t > hour_ago
                ]

                if len(self.requests[endpoint][request.client.host]) >= limit:
                    raise HTTPException(429, "Endpoint rate limit exceeded")

                self.requests[endpoint][request.client.host].append(now)

                return await func(request, *args, **kwargs)
            return wrapper
        return decorator

endpoint_limiter = EndpointRateLimiter()

@app.get("/api/search")
@endpoint_limiter.limit("/api/search")
async def search(request: Request, q: str):
    """Search with strict rate limit"""
    return {"results": []}

@app.get("/api/items")
@endpoint_limiter.limit("/api/items")
async def list_items(request: Request):
    """Items with standard rate limit"""
    return {"items": []}
```

## Distributed Rate Limiting

### Redis-Based Rate Limiting

```python
# Example 7: Redis-based distributed rate limiting
from fastapi import FastAPI, HTTPException, Request
import redis.asyncio as redis
import time

app = FastAPI()

class RedisRateLimiter:
    """Distributed rate limiting with Redis"""

    def __init__(self, redis_url: str = "redis://localhost"):
        self.redis = redis.from_url(redis_url)

    async def check_rate_limit(
        self,
        key: str,
        max_requests: int,
        window_seconds: int
    ) -> dict:
        """Check rate limit using Redis"""
        now = time.time()
        window_start = now - window_seconds

        pipe = self.redis.pipeline()

        # Remove old entries
        pipe.zremrangebyscore(key, 0, window_start)

        # Count current requests
        pipe.zcard(key)

        # Add current request
        pipe.zadd(key, {str(now): now})

        # Set expiry
        pipe.expire(key, window_seconds)

        results = await pipe.execute()
        current_count = results[1]

        remaining = max(0, max_requests - current_count - 1)

        return {
            "allowed": current_count < max_requests,
            "remaining": remaining,
            "limit": max_requests,
            "reset": int(now + window_seconds)
        }

limiter = RedisRateLimiter()

@app.middleware("http")
async def redis_rate_limit(request: Request, call_next):
    """Redis-based rate limiting"""
    client_ip = request.client.host
    result = await limiter.check_rate_limit(
        f"ratelimit:{client_ip}",
        max_requests=100,
        window_seconds=60
    )

    if not result["allowed"]:
        raise HTTPException(
            status_code=429,
            detail="Rate limit exceeded",
            headers={
                "X-RateLimit-Remaining": "0",
                "X-RateLimit-Reset": str(result["reset"])
            }
        )

    response = await call_next(request)
    response.headers["X-RateLimit-Remaining"] = str(result["remaining"])
    return response
```

## Best Practices

### Rate Limiting Guidelines

```python
# Example 8: Best practices
from fastapi import FastAPI, HTTPException

app = FastAPI()

"""
Rate Limiting Best Practices:

1. Return clear rate limit headers:
   - X-RateLimit-Limit: Maximum requests
   - X-RateLimit-Remaining: Requests remaining
   - X-RateLimit-Reset: Time when limit resets
   - Retry-After: Seconds to wait

2. Use appropriate limits:
   - Public endpoints: Lower limits
   - Authenticated users: Higher limits
   - Premium users: Highest limits

3. Implement different limits:
   - Per IP (anonymous)
   - Per user (authenticated)
   - Per endpoint (expensive operations)

4. Handle rate limits gracefully:
   - Return 429 status code
   - Include retry information
   - Log rate limit events

5. Monitor and adjust:
   - Track rate limit hits
   - Adjust limits based on usage
   - Alert on abuse patterns
"""
```

## Summary

| Algorithm | Accuracy | Memory | Best For |
|-----------|----------|--------|----------|
| Token Bucket | Good | Low | General use |
| Sliding Window | High | Medium | Accurate limits |
| Fixed Window | Low | Low | Simple cases |
| Redis-based | High | External | Distributed systems |

## Next Steps

Continue learning about:
- [Input Validation Security](./02_input_validation_security.md) - Validate inputs
- [SQL Injection Prevention](./03_sql_injection_prevention.md) - Query safety
- [API Security Headers](./07_api_security_headers.md) - Security headers
