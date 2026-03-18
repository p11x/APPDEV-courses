# HTTPX Client

## What You'll Learn
- Making async HTTP requests with httpx

## Prerequisites
- Completed asyncio fundamentals

## Installing httpx

```bash
pip install httpx
```

## Basic Usage

```python
import httpx

async def fetch_data():
    async with httpx.AsyncClient() as client:
        response = await client.get('https://api.example.com/data')
        return response.json()
```

## Summary
- httpx is an async HTTP client
- Use AsyncClient for concurrent requests
