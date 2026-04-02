# Third-Party API Integration

## Overview

Integrating with external APIs is common in FastAPI applications. This guide covers patterns for consuming third-party services reliably.

## HTTP Client Setup

### Async HTTP Client

```python
# Example 1: HTTP client with httpx
from fastapi import FastAPI, HTTPException
import httpx
from typing import Optional
import asyncio

app = FastAPI()

class APIClient:
    """Async HTTP client wrapper"""

    def __init__(self, base_url: str, timeout: int = 30):
        self.base_url = base_url
        self.client = httpx.AsyncClient(
            base_url=base_url,
            timeout=timeout,
            headers={"User-Agent": "FastAPI-Client/1.0"}
        )

    async def get(self, path: str, params: dict = None):
        response = await self.client.get(path, params=params)
        response.raise_for_status()
        return response.json()

    async def post(self, path: str, json: dict = None):
        response = await self.client.post(path, json=json)
        response.raise_for_status()
        return response.json()

    async def close(self):
        await self.client.close()

# Create clients at startup
external_api = APIClient("https://api.example.com")

@app.on_event("shutdown")
async def shutdown():
    await external_api.close()

@app.get("/external/data")
async def get_external_data():
    """Fetch data from external API"""
    try:
        data = await external_api.get("/data")
        return data
    except httpx.HTTPStatusError as e:
        raise HTTPException(e.response.status_code, str(e))
    except httpx.RequestError as e:
        raise HTTPException(503, f"Service unavailable: {str(e)}")
```

## Retry Logic

### Resilient API Calls

```python
# Example 2: Retry with backoff
import asyncio
from functools import wraps
from typing import Callable
import httpx

def retry(
    max_attempts: int = 3,
    backoff: float = 1.0,
    exceptions: tuple = (httpx.RequestError, httpx.HTTPStatusError)
):
    """Retry decorator with exponential backoff"""
    def decorator(func: Callable):
        @wraps(func)
        async def wrapper(*args, **kwargs):
            last_exception = None
            for attempt in range(max_attempts):
                try:
                    return await func(*args, **kwargs)
                except exceptions as e:
                    last_exception = e
                    if attempt < max_attempts - 1:
                        wait_time = backoff * (2 ** attempt)
                        await asyncio.sleep(wait_time)
            raise last_exception
        return wrapper
    return decorator

@retry(max_attempts=3, backoff=1.0)
async def fetch_with_retry(url: str):
    """Fetch with automatic retry"""
    async with httpx.AsyncClient() as client:
        response = await client.get(url)
        response.raise_for_status()
        return response.json()

@app.get("/external/retry")
async def get_with_retry():
    """Endpoint with retry logic"""
    return await fetch_with_retry("https://api.example.com/data")
```

## Rate Limiting

### Client-Side Rate Limiting

```python
# Example 3: Rate-limited API client
import asyncio
from collections import defaultdict
import time

class RateLimitedClient:
    """Client with rate limiting"""

    def __init__(self, requests_per_second: float = 10):
        self.requests_per_second = requests_per_second
        self.min_interval = 1.0 / requests_per_second
        self.last_request_time = defaultdict(float)
        self.client = httpx.AsyncClient()

    async def request(self, method: str, url: str, **kwargs):
        """Make rate-limited request"""
        # Enforce rate limit
        current_time = time.time()
        elapsed = current_time - self.last_request_time[url]
        if elapsed < self.min_interval:
            await asyncio.sleep(self.min_interval - elapsed)

        # Make request
        self.last_request_time[url] = time.time()
        response = await self.client.request(method, url, **kwargs)
        response.raise_for_status()
        return response

    async def get(self, url: str, **kwargs):
        return await self.request("GET", url, **kwargs)

    async def post(self, url: str, **kwargs):
        return await self.request("POST", url, **kwargs)

rate_limited_client = RateLimitedClient(requests_per_second=10)
```

## Response Caching

### Cache External API Responses

```python
# Example 4: Cached external API calls
from fastapi import FastAPI
import redis.asyncio as redis
import json
import hashlib
from datetime import datetime

app = FastAPI()
redis_client = redis.Redis(host="localhost", port=6379)

class CachedAPIClient:
    """Client with response caching"""

    def __init__(self, base_url: str, cache_ttl: int = 300):
        self.base_url = base_url
        self.cache_ttl = cache_ttl
        self.client = httpx.AsyncClient(base_url=base_url)

    def _cache_key(self, method: str, path: str, params: dict = None) -> str:
        """Generate cache key"""
        key_str = f"{method}:{path}:{json.dumps(params or {}, sort_keys=True)}"
        return f"api_cache:{hashlib.md5(key_str.encode()).hexdigest()}"

    async def get(self, path: str, params: dict = None, use_cache: bool = True):
        """Get with caching"""
        cache_key = self._cache_key("GET", path, params)

        # Check cache
        if use_cache:
            cached = await redis_client.get(cache_key)
            if cached:
                return json.loads(cached)

        # Fetch from API
        response = await self.client.get(path, params=params)
        response.raise_for_status()
        data = response.json()

        # Cache response
        if use_cache:
            await redis_client.setex(cache_key, self.cache_ttl, json.dumps(data))

        return data

    async def invalidate(self, path: str):
        """Invalidate cache for path"""
        pattern = f"api_cache:*{path}*"
        async for key in redis_client.scan_iter(match=pattern):
            await redis_client.delete(key)

cached_client = CachedAPIClient("https://api.example.com")
```

## Webhook Handling

### Receiving Webhooks

```python
# Example 5: Webhook receiver
from fastapi import FastAPI, Request, HTTPException, Header
import hmac
import hashlib

app = FastAPI()

WEBHOOK_SECRET = "your-webhook-secret"

def verify_signature(payload: bytes, signature: str) -> bool:
    """Verify webhook signature"""
    expected = hmac.new(
        WEBHOOK_SECRET.encode(),
        payload,
        hashlib.sha256
    ).hexdigest()
    return hmac.compare_digest(f"sha256={expected}", signature)

@app.post("/webhooks/github")
async def github_webhook(
    request: Request,
    x_hub_signature_256: str = Header(...)
):
    """Handle GitHub webhook"""
    body = await request.body()

    # Verify signature
    if not verify_signature(body, x_hub_signature_256):
        raise HTTPException(403, "Invalid signature")

    payload = await request.json()
    event = request.headers.get("X-GitHub-Event")

    # Process event
    if event == "push":
        await handle_push_event(payload)
    elif event == "pull_request":
        await handle_pr_event(payload)

    return {"status": "processed"}

async def handle_push_event(payload: dict):
    """Handle push event"""
    pass

async def handle_pr_event(payload: dict):
    """Handle pull request event"""
    pass
```

## OAuth2 Integration

### External OAuth Provider

```python
# Example 6: OAuth2 integration
from fastapi import FastAPI, HTTPException
from fastapi.responses import RedirectResponse
import httpx

app = FastAPI()

# OAuth2 configuration
OAUTH_CONFIG = {
    "authorization_url": "https://provider.com/authorize",
    "token_url": "https://provider.com/token",
    "userinfo_url": "https://provider.com/userinfo",
    "client_id": "your-client-id",
    "client_secret": "your-client-secret",
    "redirect_uri": "http://localhost:8000/callback"
}

@app.get("/auth/login")
async def login():
    """Initiate OAuth flow"""
    url = (
        f"{OAUTH_CONFIG['authorization_url']}"
        f"?client_id={OAUTH_CONFIG['client_id']}"
        f"&redirect_uri={OAUTH_CONFIG['redirect_uri']}"
        f"&response_type=code"
        f"&scope=openid profile email"
    )
    return RedirectResponse(url=url)

@app.get("/callback")
async def callback(code: str):
    """Handle OAuth callback"""
    async with httpx.AsyncClient() as client:
        # Exchange code for token
        token_response = await client.post(
            OAUTH_CONFIG["token_url"],
            data={
                "grant_type": "authorization_code",
                "code": code,
                "redirect_uri": OAUTH_CONFIG["redirect_uri"],
                "client_id": OAUTH_CONFIG["client_id"],
                "client_secret": OAUTH_CONFIG["client_secret"]
            }
        )

        token_data = token_response.json()

        # Get user info
        user_response = await client.get(
            OAUTH_CONFIG["userinfo_url"],
            headers={"Authorization": f"Bearer {token_data['access_token']}"}
        )

        user_data = user_response.json()

    return {"user": user_data, "token": token_data["access_token"]}
```

## Best Practices

### Integration Guidelines

```python
# Example 7: Third-party API best practices
"""
Third-Party API Best Practices:

1. Error Handling
   ✓ Handle all HTTP status codes
   ✓ Implement retry logic
   ✓ Graceful degradation

2. Performance
   ✓ Use async clients
   ✓ Implement caching
   ✓ Connection pooling

3. Security
   ✓ Store API keys securely
   ✓ Verify webhook signatures
   ✓ Use HTTPS

4. Reliability
   ✓ Circuit breakers
   ✓ Timeout configuration
   ✓ Fallback mechanisms

5. Monitoring
   ✓ Log all API calls
   ✓ Track response times
   ✓ Monitor error rates
"""

# Complete integration client
class ResilientAPIClient:
    """Production-ready API client"""

    def __init__(self, base_url: str):
        self.base_url = base_url
        self.client = httpx.AsyncClient(
            base_url=base_url,
            timeout=30.0,
            limits=httpx.Limits(max_connections=100)
        )

    async def request(
        self,
        method: str,
        path: str,
        **kwargs
    ):
        """Make resilient request"""
        try:
            response = await self.client.request(method, path, **kwargs)
            response.raise_for_status()
            return response.json()
        except httpx.TimeoutException:
            raise HTTPException(504, "Upstream timeout")
        except httpx.HTTPStatusError as e:
            raise HTTPException(e.response.status_code, str(e))
        except httpx.RequestError:
            raise HTTPException(503, "Service unavailable")
```

## Summary

| Pattern | Use Case | Implementation |
|---------|----------|----------------|
| Retry | Transient failures | Exponential backoff |
| Cache | Redundant calls | Redis caching |
| Rate Limit | API quotas | Token bucket |
| Circuit Breaker | Service failures | State management |

## Next Steps

Continue learning about:
- [API Aggregation](./02_api_aggregation.md) - Combining APIs
- [API Gateway](./04_api_gateway.md) - Gateway patterns
- [Caching Strategies](../02_performance_optimization/01_caching_strategies.md)
