# Async Optimization

## What You'll Learn
- Async patterns
- Concurrent requests
- Avoiding blocking

## Prerequisites
- Completed caching for performance

## Concurrent Operations

```python
import asyncio
import httpx

async def fetch_multiple_urls(urls: list) -> list:
    """Fetch multiple URLs concurrently"""
    async with httpx.AsyncClient() as client:
        tasks = [client.get(url) for url in urls]
        responses = await asyncio.gather(*tasks)
        return [r.json() for r in responses]

# Parallel database queries
async def get_user_with_posts(user_id: int):
    user, posts = await asyncio.gather(
        get_user(user_id),
        get_user_posts(user_id)
    )
    return {"user": user, "posts": posts}
```

## Avoiding Blocking

```python
# BAD: Using synchronous library in async
import requests

async def slow():
    r = requests.get("https://api.example.com")  # Blocks event loop!

# GOOD: Use async library
import httpx

async def fast():
    async with httpx.AsyncClient() as client:
        r = await client.get("https://api.example.com")  # Non-blocking
```

## Summary
- Use asyncio.gather for concurrency
- Use async libraries
- Avoid blocking calls

## Next Steps
→ Continue to `05-connection-pooling.md`
