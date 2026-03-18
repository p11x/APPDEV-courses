# AIOHTTP Server

## What You'll Learn
- Building async servers with aiohttp

## Prerequisites
- Completed aiohttp client

## Installing aiohttp

```bash
pip install aiohttp
```

## Basic Server

```python
from aiohttp import web

async def hello(request):
    return web.Response(text="Hello, World!")

app = web.Application()
app.router.add_get('/', hello)

if __name__ == '__main__':
    web.run_app(app)
```

## Summary
- aiohttp provides async web server functionality
- Similar to Flask but async-native
