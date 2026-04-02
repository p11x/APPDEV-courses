# Problem Details

## Overview

RFC 7807 Problem Details provides a standardized format for HTTP API error responses, enabling consistent error handling across clients.

## Basic Problem Details

### Simple Error Response

```python
# Example 1: Problem details format
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional, Dict, Any

app = FastAPI()

class ProblemDetail(BaseModel):
    type: str = "about:blank"
    title: str
    status: int
    detail: Optional[str] = None
    instance: Optional[str] = None

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    if item_id <= 0:
        raise HTTPException(
            status_code=400,
            detail={
                "type": "https://api.example.com/errors/invalid-id",
                "title": "Invalid Item ID",
                "status": 400,
                "detail": "Item ID must be positive",
                "instance": f"/items/{item_id}"
            }
        )
    return {"item_id": item_id}
```

## Custom Problem Details

### Domain-Specific Errors

```python
# Example 2: Custom problem details
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
from pydantic import BaseModel
from typing import Optional

app = FastAPI()

class StockError(BaseModel):
    type: str
    title: str
    status: int
    detail: str
    item_id: int
    requested: int
    available: int

@app.exception_handler(StockError)
async def stock_error_handler(request: Request, exc: StockError):
    return JSONResponse(
        status_code=exc.status,
        content=exc.model_dump()
    )

@app.post("/orders/")
async def create_order(item_id: int, quantity: int):
    available = 5

    if quantity > available:
        raise StockError(
            type="https://api.example.com/errors/insufficient-stock",
            title="Insufficient Stock",
            status=400,
            detail=f"Requested {quantity}, only {available} available",
            item_id=item_id,
            requested=quantity,
            available=available
        )

    return {"order_id": 1, "item_id": item_id, "quantity": quantity}
```

## Best Practices

### Error Response Guidelines

```python
# Example 3: Best practices
from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import JSONResponse

app = FastAPI()

def problem_response(
    status: int,
    title: str,
    detail: str,
    instance: str,
    **extra
):
    """Create standardized problem response"""
    return JSONResponse(
        status_code=status,
        content={
            "type": f"https://api.example.com/errors/{title.lower().replace(' ', '-')}",
            "title": title,
            "status": status,
            "detail": detail,
            "instance": instance,
            **extra
        }
    )

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    if item_id > 1000:
        return problem_response(
            status=404,
            title="Not Found",
            detail=f"Item {item_id} does not exist",
            instance=f"/items/{item_id}"
        )
    return {"item_id": item_id}
```

## Summary

| Field | Purpose | Example |
|-------|---------|---------|
| type | Error type URL | `"https://api.example.com/errors/not-found"` |
| title | Short description | `"Not Found"` |
| status | HTTP status | `404` |
| detail | Detailed message | `"Item 42 does not exist"` |
| instance | Request path | `"/items/42"` |

## Next Steps

Continue learning about:
- [Exception Handling](./05_exceptions_handling.md) - Error handling
- [Custom Responses](./06_custom_responses.md) - Response types
- [Dependencies](../05_dependencies/01_dependency_injection_basics.md) - DI patterns
