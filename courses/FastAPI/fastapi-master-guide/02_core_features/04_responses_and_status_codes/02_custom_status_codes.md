# Custom Status Codes

## Overview

HTTP status codes communicate the result of a request. FastAPI provides easy ways to set custom status codes for different scenarios.

## Setting Status Codes

### Decorator Status Codes

```python
# Example 1: Status code in decorator
from fastapi import FastAPI, status

app = FastAPI()

@app.post("/items/", status_code=status.HTTP_201_CREATED)
async def create_item(name: str):
    """
    201 Created - Resource successfully created.
    """
    return {"name": name, "message": "Created"}

@app.delete("/items/{item_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(item_id: int):
    """
    204 No Content - Successful deletion.
    No response body.
    """
    pass

@app.get("/items/", status_code=status.HTTP_200_OK)
async def list_items():
    """
    200 OK - Standard successful response.
    """
    return {"items": []}
```

### Dynamic Status Codes

```python
# Example 2: Dynamic status code
from fastapi import FastAPI, Response
from fastapi.responses import JSONResponse

app = FastAPI()

@app.post("/items/")
async def create_item(name: str, response: Response):
    """
    Set status code dynamically in handler.
    """
    if name == "duplicate":
        response.status_code = 409
        return {"error": "Item already exists"}

    response.status_code = 201
    return {"name": name, "message": "Created"}

@app.post("/items2/")
async def create_item_v2(name: str):
    """
    Alternative: return JSONResponse directly.
    """
    if name == "duplicate":
        return JSONResponse(
            status_code=409,
            content={"error": "Item already exists"}
        )

    return JSONResponse(
        status_code=201,
        content={"name": name, "message": "Created"}
    )
```

## Common Status Codes

### Success Codes

```python
# Example 3: Success status codes
from fastapi import FastAPI, status

app = FastAPI()

@app.get("/items/", status_code=status.HTTP_200_OK)
async def get_items():
    """200 - Successful GET request"""
    return {"items": []}

@app.post("/items/", status_code=status.HTTP_201_CREATED)
async def create_item(name: str):
    """201 - Resource created"""
    return {"name": name}

@app.delete("/items/{item_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(item_id: int):
    """204 - Successful with no content"""
    pass

@app.post("/items/search", status_code=status.HTTP_200_OK)
async def search_items(query: str):
    """200 - Search results"""
    return {"results": []}
```

### Error Codes

```python
# Example 4: Error status codes
from fastapi import FastAPI, HTTPException, status

app = FastAPI()

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    if item_id <= 0:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="Invalid item ID"
        )

    if item_id > 1000:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Item not found"
        )

    return {"item_id": item_id}

@app.post("/items/")
async def create_item(name: str):
    if name == "duplicate":
        raise HTTPException(
            status_code=status.HTTP_409_CONFLICT,
            detail="Item already exists"
        )

    return {"name": name}

@app.get("/protected/")
async def protected_route(authenticated: bool = False):
    if not authenticated:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Authentication required",
            headers={"WWW-Authenticate": "Bearer"}
        )

    return {"message": "Authenticated"}
```

## Status Code Reference

### Complete Reference

```python
# Example 5: Common HTTP status codes
from fastapi import FastAPI, status

app = FastAPI()

# Informational
# 100 Continue
# 101 Switching Protocols

# Success
# 200 OK - Standard success
# 201 Created - Resource created
# 202 Accepted - Request accepted for processing
# 204 No Content - Success with no body

# Redirection
# 301 Moved Permanently
# 302 Found
# 304 Not Modified

# Client Errors
# 400 Bad Request - Invalid syntax
# 401 Unauthorized - Authentication required
# 403 Forbidden - Authenticated but not authorized
# 404 Not Found - Resource doesn't exist
# 405 Method Not Allowed
# 409 Conflict - Resource conflict
# 422 Unprocessable Entity - Validation error
# 429 Too Many Requests - Rate limited

# Server Errors
# 500 Internal Server Error
# 501 Not Implemented
# 502 Bad Gateway
# 503 Service Unavailable

@app.get("/reference/")
async def status_reference():
    """Return common status codes"""
    return {
        "success": {
            "200": "OK",
            "201": "Created",
            "204": "No Content"
        },
        "client_error": {
            "400": "Bad Request",
            "401": "Unauthorized",
            "403": "Forbidden",
            "404": "Not Found",
            "422": "Unprocessable Entity"
        },
        "server_error": {
            "500": "Internal Server Error",
            "503": "Service Unavailable"
        }
    }
```

## Best Practices

### Status Code Guidelines

```python
# Example 6: Best practices
from fastapi import FastAPI, HTTPException, status

app = FastAPI()

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    """
    Use appropriate status codes:
    - 200: Successful retrieval
    - 400: Invalid input
    - 404: Resource not found
    """
    if item_id <= 0:
        raise HTTPException(400, "ID must be positive")

    if item_id > 1000:
        raise HTTPException(404, "Item not found")

    return {"item_id": item_id}

@app.post("/items/", status_code=status.HTTP_201_CREATED)
async def create_item(name: str):
    """
    Return 201 for resource creation.
    """
    return {"name": name, "message": "Created"}

@app.delete("/items/{item_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(item_id: int):
    """
    Return 204 for successful deletion.
    No response body.
    """
    pass
```

## Summary

| Status Code | Use Case | FastAPI |
|-------------|----------|---------|
| 200 | Success | Default |
| 201 | Created | `status_code=201` |
| 204 | No content | `status_code=204` |
| 400 | Bad request | `HTTPException(400)` |
| 404 | Not found | `HTTPException(404)` |
| 422 | Validation | Automatic |

## Next Steps

Continue learning about:
- [Response Models](./03_response_models.md) - Output validation
- [Problem Details](./04_problem_details.md) - Error formats
- [Exception Handling](./05_exceptions_handling.md) - Error handling
