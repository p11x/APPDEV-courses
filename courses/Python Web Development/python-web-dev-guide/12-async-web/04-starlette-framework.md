# Starlette Framework

## What You'll Learn
- Using Starlette for async web apps

## Prerequisites
- Completed aiohttp server

## Installing Starlette

```bash
pip install starlette
```

## Basic App

```python
from starlette.applications import Starlette
from starlette.routing import Route
from starlette.responses import JSONResponse

async def homepage(request):
    return JSONResponse({"message": "Hello!"})

routes = [
    Route("/", endpoint=homepage),
]

app = Starlette(routes=routes)
```

## Summary
- Starlette is the async framework FastAPI is built on
- Lightweight and flexible
