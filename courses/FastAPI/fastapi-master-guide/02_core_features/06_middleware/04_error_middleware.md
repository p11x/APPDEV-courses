# Error Middleware

## Overview

Error middleware provides centralized exception handling, consistent error responses, and error logging across your entire application.

## Global Error Handling

### Exception Catching Middleware

```python
# Example 1: Global exception handler
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import traceback
import logging

app = FastAPI()
logger = logging.getLogger(__name__)

@app.middleware("http")
async def error_handling_middleware(request: Request, call_next):
    """Catch all unhandled exceptions"""
    try:
        return await call_next(request)
    except Exception as e:
        # Log the error
        logger.error(
            f"Unhandled exception: {str(e)}\n"
            f"Path: {request.url.path}\n"
            f"{traceback.format_exc()}"
        )

        # Return consistent error response
        return JSONResponse(
            status_code=500,
            content={
                "error": {
                    "code": "INTERNAL_ERROR",
                    "message": "An unexpected error occurred",
                    "request_id": getattr(request.state, "request_id", None)
                }
            }
        )

@app.get("/error/")
async def trigger_error():
    raise ValueError("Test error")
```

### Structured Error Responses

```python
# Example 2: Structured error format
from fastapi import FastAPI, Request, HTTPException
from fastapi.responses import JSONResponse
from datetime import datetime

app = FastAPI()

def error_response(
    status_code: int,
    code: str,
    message: str,
    details: dict = None,
    request: Request = None
):
    """Create consistent error response"""
    return JSONResponse(
        status_code=status_code,
        content={
            "success": False,
            "error": {
                "code": code,
                "message": message,
                "details": details or {},
                "timestamp": datetime.now().isoformat(),
                "path": str(request.url) if request else None
            }
        }
    )

@app.exception_handler(HTTPException)
async def http_exception_handler(request: Request, exc: HTTPException):
    """Handle HTTPException"""
    return error_response(
        status_code=exc.status_code,
        code=f"HTTP_{exc.status_code}",
        message=str(exc.detail),
        request=request
    )

@app.exception_handler(Exception)
async def general_exception_handler(request: Request, exc: Exception):
    """Handle all other exceptions"""
    return error_response(
        status_code=500,
        code="INTERNAL_ERROR",
        message="An unexpected error occurred",
        details={"type": type(exc).__name__},
        request=request
    )

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    if item_id <= 0:
        raise HTTPException(400, "Invalid item ID")
    return {"item_id": item_id}
```

## Error Logging Middleware

### Request Error Logging

```python
# Example 3: Error logging
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import logging
import time
import uuid

app = FastAPI()
logger = logging.getLogger(__name__)

@app.middleware("http")
async def error_logging_middleware(request: Request, call_next):
    """Log errors with context"""
    request_id = str(uuid.uuid4())
    request.state.request_id = request_id
    start = time.time()

    try:
        response = await call_next(request)

        # Log 4xx/5xx responses
        if response.status_code >= 400:
            duration = time.time() - start
            logger.warning(
                f"Error response: {response.status_code} "
                f"request_id={request_id} "
                f"method={request.method} "
                f"path={request.url.path} "
                f"duration={duration:.3f}s"
            )

        return response

    except Exception as e:
        duration = time.time() - start
        logger.error(
            f"Unhandled exception: {str(e)} "
            f"request_id={request_id} "
            f"method={request.method} "
            f"path={request.url.path} "
            f"duration={duration:.3f}s",
            exc_info=True
        )

        return JSONResponse(
            status_code=500,
            content={
                "error": "Internal server error",
                "request_id": request_id
            }
        )

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Best Practices

### Error Handling Guidelines

```python
# Example 4: Best practices
from fastapi import FastAPI, Request, HTTPException
from fastapi.responses import JSONResponse
import logging

app = FastAPI()
logger = logging.getLogger(__name__)

@app.middleware("http")
async def error_middleware(request: Request, call_next):
    """
    Best practices:
    1. Log all errors
    2. Return consistent format
    3. Don't expose internal details
    4. Include request context
    """
    try:
        return await call_next(request)
    except Exception as e:
        logger.error(f"Error: {e}", exc_info=True)

        return JSONResponse(
            status_code=500,
            content={
                "error": {
                    "message": "Internal server error",
                    "request_id": getattr(request.state, "request_id", None)
                }
            }
        )
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| Exception handler | Catch errors | `@app.exception_handler(Exception)` |
| Error logging | Log context | `logger.error(...)` |
| Consistent format | Standard response | `error_response()` |
| Request context | Include details | `request_id`, `path` |

## Next Steps

Continue learning about:
- [Middleware Performance](./05_middleware_performance.md) - Optimization
- [Exception Handling](../04_responses_and_status_codes/05_exceptions_handling.md) - Error handling
