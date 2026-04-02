# Response Caching

## Overview

Response caching stores API responses to reduce computation and database load.

## HTTP Caching

### Cache Headers

```python
# Example 1: HTTP cache headers
from fastapi import FastAPI, Response
from fastapi.responses import JSONResponse

app = FastAPI()

@app.get("/items/")
async def list_items(response: Response):
    response.headers["Cache-Control"] = "public, max-age=3600"
    response.headers["ETag"] = '"abc123"'
    return {"items": []}

@app.get("/items/{item_id}")
async def get_item(item_id: int, response: Response):
    response.headers["Cache-Control"] = "private, max-age=300"
    return {"item_id": item_id}
```

### Conditional Requests

```python
# Example 2: ETag and conditional requests
from fastapi import FastAPI, Request, Response
from fastapi.responses import Response as FastAPIResponse
import hashlib

app = FastAPI()

def generate_etag(data: dict) -> str:
    content = json.dumps(data, sort_keys=True)
    return hashlib.md5(content.encode()).hexdigest()

@app.get("/items/{item_id}")
async def get_item(item_id: int, request: Request, response: Response):
    item = await fetch_item(item_id)
    etag = generate_etag(item)

    # Check if client has current version
    if request.headers.get("if-none-match") == etag:
        return FastAPIResponse(status_code=304)

    response.headers["ETag"] = etag
    return item
```

## Summary

Response caching significantly improves API performance.

## Next Steps

Continue learning about:
- [Caching Layers](./02_caching_layers.md)
- [Database Optimization](./04_database_optimization.md)
