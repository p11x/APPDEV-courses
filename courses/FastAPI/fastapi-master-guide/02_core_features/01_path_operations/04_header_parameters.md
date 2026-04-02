# Header Parameters

## Overview

HTTP headers carry metadata about requests and responses. FastAPI provides easy access to headers for authentication, content negotiation, caching, and custom application logic.

## Basic Header Parameters

### Reading Standard Headers

```python
# Example 1: Common HTTP headers
from fastapi import FastAPI, Header
from typing import Optional

app = FastAPI()

@app.get("/items/")
async def get_items(
    # FastAPI converts hyphens to underscores
    user_agent: Optional[str] = Header(None),
    accept_language: Optional[str] = Header(None),
    accept_encoding: Optional[str] = Header(None)
):
    """
    Header names use underscores (converted from hyphens).
    User-Agent → user_agent
    Accept-Language → accept_language
    """
    return {
        "user_agent": user_agent,
        "accept_language": accept_language,
        "accept_encoding": accept_encoding
    }

@app.get("/client-info/")
async def client_info(
    host: Optional[str] = Header(None),
    connection: Optional[str] = Header(None),
    cache_control: Optional[str] = Header(None)
):
    """Standard HTTP headers"""
    return {
        "host": host,
        "connection": connection,
        "cache_control": cache_control
    }
```

### Custom Headers

```python
# Example 2: Custom application headers
from fastapi import FastAPI, Header, HTTPException
from typing import Optional

app = FastAPI()

@app.get("/protected/")
async def protected_endpoint(
    # Required custom header
    x_api_key: str = Header(..., description="API authentication key"),
    # Optional custom header
    x_request_id: Optional[str] = Header(None, description="Request tracking ID"),
    # Optional with default
    x_client_version: str = Header("1.0", description="Client version")
):
    """
    Custom headers follow X- prefix convention.
    Required headers return 422 if missing.
    """
    # Validate API key
    if x_api_key != "valid-api-key":
        raise HTTPException(status_code=401, detail="Invalid API key")

    return {
        "api_key": x_api_key[:8] + "...",  # Masked for security
        "request_id": x_request_id,
        "client_version": x_client_version
    }

@app.get("/tracking/")
async def tracking_endpoint(
    x_request_id: str = Header(..., description="Unique request identifier"),
    x_correlation_id: Optional[str] = Header(None, description="Correlation ID"),
    x_trace_id: Optional[str] = Header(None, description="Distributed trace ID")
):
    """
    Tracking headers for distributed systems.
    Used for logging, monitoring, and debugging.
    """
    return {
        "request_id": x_request_id,
        "correlation_id": x_correlation_id,
        "trace_id": x_trace_id
    }
```

## Header Validation

### Header Parameter Constraints

```python
# Example 3: Header validation
from fastapi import FastAPI, Header
from typing import Optional

app = FastAPI()

@app.get("/items/")
async def get_items(
    # String validation
    x_api_key: str = Header(
        ...,
        min_length=32,
        max_length=64,
        regex="^[a-zA-Z0-9]+$",
        description="API key (32-64 alphanumeric characters)"
    ),
    # Optional with pattern
    x_client_id: Optional[str] = Header(
        None,
        regex="^[a-z]{2,10}-\\d{4,8}$",
        description="Client ID format: xx-0000"
    )
):
    """
    Header validation works like query/path parameters.
    Invalid headers return 422.
    """
    return {"api_key": "***", "client_id": x_client_id}
```

## Multi-Value Headers

### List Headers

```python
# Example 4: Headers with multiple values
from fastapi import FastAPI, Header
from typing import List, Optional

app = FastAPI()

@app.get("/items/")
async def get_items(
    # Accept multiple values for same header
    x_tags: List[str] = Header(
        [],
        description="Multiple tag values"
    ),
    # Standard multi-value header
    accept: List[str] = Header(
        ["application/json"],
        description="Accepted content types"
    )
):
    """
    List parameters collect multiple header values.
    X-Tags: python
    X-Tags: fastapi
    Result: x_tags = ["python", "fastapi"]
    """
    return {"tags": x_tags, "accept": accept}
```

## Header Aliases

### Using Aliases

```python
# Example 5: Header aliases
from fastapi import FastAPI, Header
from typing import Optional

app = FastAPI()

@app.get("/items/")
async def get_items(
    # Alias preserves exact header name
    api_key: str = Header(
        ...,
        alias="X-API-Key",
        description="API authentication key"
    ),
    # Complex header name
    rate_limit_remaining: int = Header(
        ...,
        alias="X-RateLimit-Remaining",
        description="Remaining rate limit"
    )
):
    """
    Aliases preserve exact header names including case.
    Useful for standard header formats.
    """
    return {"api_key": "***", "rate_limit": rate_limit_remaining}
```

## Authentication Headers

### Common Auth Patterns

```python
# Example 6: Authentication headers
from fastapi import FastAPI, Header, HTTPException, Depends
from typing import Optional
import jwt

app = FastAPI()

# Bearer token authentication
@app.get("/protected/")
async def protected_endpoint(
    authorization: str = Header(..., description="Bearer token")
):
    """
    Bearer token in Authorization header.
    Format: Authorization: Bearer <token>
    """
    if not authorization.startswith("Bearer "):
        raise HTTPException(
            status_code=401,
            detail="Invalid authorization header format"
        )

    token = authorization.split(" ")[1]

    # Verify token (simplified)
    try:
        payload = jwt.decode(token, "secret", algorithms=["HS256"])
        return {"user": payload.get("sub")}
    except jwt.InvalidTokenError:
        raise HTTPException(status_code=401, detail="Invalid token")

# API key authentication
@app.get("/api-key-protected/")
async def api_key_protected(
    x_api_key: str = Header(..., description="API key")
):
    """
    API key in custom header.
    X-API-Key: your-api-key-here
    """
    valid_keys = ["key123", "key456"]
    if x_api_key not in valid_keys:
        raise HTTPException(status_code=403, detail="Invalid API key")

    return {"authenticated": True}

# Multiple auth methods
async def get_auth_token(
    authorization: Optional[str] = Header(None),
    x_api_key: Optional[str] = Header(None)
):
    """
    Dependency supporting multiple auth methods.
    """
    if authorization and authorization.startswith("Bearer "):
        return authorization.split(" ")[1]
    elif x_api_key:
        return x_api_key
    else:
        raise HTTPException(
            status_code=401,
            detail="No authentication provided"
        )

@app.get("/multi-auth/")
async def multi_auth(token: str = Depends(get_auth_token)):
    """Endpoint accepting multiple auth methods"""
    return {"token": "***"}
```

## Content Negotiation

### Accept and Content-Type

```python
# Example 7: Content negotiation headers
from fastapi import FastAPI, Header, HTTPException
from fastapi.responses import JSONResponse, PlainTextResponse
from typing import Optional

app = FastAPI()

@app.get("/data/")
async def get_data(
    accept: str = Header("application/json", description="Response format")
):
    """
    Content negotiation based on Accept header.
    Returns format client prefers.
    """
    data = {"items": [1, 2, 3], "total": 3}

    if "application/json" in accept:
        return JSONResponse(content=data)
    elif "text/plain" in accept:
        return PlainTextResponse(content=str(data))
    else:
        raise HTTPException(
            status_code=406,
            detail=f"Not acceptable: {accept}"
        )

@app.post("/upload/")
async def upload_data(
    content_type: str = Header(..., description="Request content type"),
    body: bytes = ...
):
    """
    Validate Content-Type for uploads.
    """
    if content_type not in ["application/json", "text/csv"]:
        raise HTTPException(
            status_code=415,
            detail=f"Unsupported media type: {content_type}"
        )

    return {"content_type": content_type, "size": len(body)}
```

## Response Headers

### Setting Response Headers

```python
# Example 8: Setting headers in responses
from fastapi import FastAPI, Response
from fastapi.responses import JSONResponse

app = FastAPI()

@app.get("/items/")
async def get_items(response: Response):
    """
    Set response headers using Response object.
    """
    # Set custom headers
    response.headers["X-Total-Count"] = "100"
    response.headers["X-Request-ID"] = "abc123"
    response.headers["Cache-Control"] = "max-age=3600"

    return {"items": []}

@app.get("/items2/")
async def get_items_v2():
    """
    Set headers using JSONResponse directly.
    """
    return JSONResponse(
        content={"items": []},
        headers={
            "X-Total-Count": "100",
            "X-Request-ID": "abc123",
            "Cache-Control": "max-age=3600"
        }
    )

# CORS headers
@app.get("/cors-example/")
async def cors_example(response: Response):
    """
    Manual CORS headers (usually handled by middleware).
    """
    response.headers["Access-Control-Allow-Origin"] = "*"
    response.headers["Access-Control-Allow-Methods"] = "GET, POST"
    return {"message": "CORS enabled"}
```

## Rate Limiting Headers

### Rate Limit Implementation

```python
# Example 9: Rate limiting with headers
from fastapi import FastAPI, Header, HTTPException
from typing import Optional
import time

app = FastAPI()

# Simple rate limiting storage
rate_limits: dict[str, list[float]] = {}

@app.get("/rate-limited/")
async def rate_limited_endpoint(
    x_api_key: str = Header(..., description="API key for rate limiting")
):
    """
    Rate limiting using custom headers.
    Tracks requests per API key.
    """
    current_time = time.time()
    window_size = 60  # 1 minute window
    max_requests = 100

    # Initialize or clean old requests
    if x_api_key not in rate_limits:
        rate_limits[x_api_key] = []

    rate_limits[x_api_key] = [
        t for t in rate_limits[x_api_key]
        if current_time - t < window_size
    ]

    # Check limit
    if len(rate_limits[x_api_key]) >= max_requests:
        remaining = 0
        reset_time = int(rate_limits[x_api_key][0] + window_size)
        raise HTTPException(
            status_code=429,
            detail="Rate limit exceeded",
            headers={
                "X-RateLimit-Limit": str(max_requests),
                "X-RateLimit-Remaining": "0",
                "X-RateLimit-Reset": str(reset_time)
            }
        )

    # Record request
    rate_limits[x_api_key].append(current_time)

    return {
        "message": "Success",
        "rate_limit": {
            "limit": max_requests,
            "remaining": max_requests - len(rate_limits[x_api_key]),
            "reset": int(current_time + window_size)
        }
    }
```

## Common Header Patterns

### Pagination Headers

```python
# Example 10: Pagination with headers
from fastapi import FastAPI, Query, Response
from typing import List

app = FastAPI()

@app.get("/items/")
async def list_items(
    response: Response,
    page: int = Query(1, ge=1),
    per_page: int = Query(20, ge=1, le=100)
):
    """
    Pagination info in response headers.
    Some APIs prefer this over response body.
    """
    total_items = 100  # Simulated
    total_pages = (total_items + per_page - 1) // per_page

    # Set pagination headers
    response.headers["X-Total-Count"] = str(total_items)
    response.headers["X-Page"] = str(page)
    response.headers["X-Per-Page"] = str(per_page)
    response.headers["X-Total-Pages"] = str(total_pages)

    if page < total_pages:
        response.headers["X-Next-Page"] = str(page + 1)
    if page > 1:
        response.headers["X-Prev-Page"] = str(page - 1)

    return {"items": list(range(per_page))}
```

## Best Practices

### Header Parameter Guidelines

```python
# Example 11: Best practices
from fastapi import FastAPI, Header
from typing import Optional

app = FastAPI()

# GOOD: Use standard header names
@app.get("/items/")
async def get_items(
    # Standard headers
    authorization: Optional[str] = Header(None),
    accept: str = Header("application/json"),
    # Custom headers with X- prefix
    x_request_id: Optional[str] = Header(None),
    x_api_version: str = Header("1.0")
):
    """Use established header naming conventions"""
    return {"items": []}

# GOOD: Provide sensible defaults
@app.get("/content/")
async def get_content(
    accept_language: str = Header("en", description="Preferred language"),
    accept_encoding: str = Header("gzip", description="Accepted encodings")
):
    """Defaults make API easier to use"""
    return {"language": accept_language}

# GOOD: Document custom headers
@app.get("/tracked/")
async def tracked_endpoint(
    x_request_id: str = Header(
        ...,
        description="Unique request identifier for tracing",
        examples=["req_abc123", "req_xyz789"]
    )
):
    """Document custom headers thoroughly"""
    return {"request_id": x_request_id}
```

## Summary

| Header Type | Example | Use Case |
|-------------|---------|----------|
| Authentication | `Authorization: Bearer <token>` | User auth |
| API Key | `X-API-Key: <key>` | API access |
| Content Negotiation | `Accept: application/json` | Response format |
| Tracking | `X-Request-ID: <uuid>` | Request tracing |
| Rate Limiting | `X-RateLimit-Remaining: 95` | Rate limits |

## Next Steps

Continue learning about:
- [Complex Routing](./05_complex_routing.md) - Advanced routing patterns
- [Request Body](../03_request_body/01_basic_request_body.md) - JSON body handling
- [Dependencies](../05_dependencies/01_dependency_injection_basics.md) - Reusable components
