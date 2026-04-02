# Exception Handling

## Overview

FastAPI provides robust exception handling through HTTPException and custom exception handlers. This enables consistent error responses across your API.

## HTTPException

### Basic Exception Handling

```python
# Example 1: HTTPException usage
from fastapi import FastAPI, HTTPException

app = FastAPI()

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    if item_id <= 0:
        raise HTTPException(
            status_code=400,
            detail="Item ID must be positive"
        )

    if item_id > 1000:
        raise HTTPException(
            status_code=404,
            detail=f"Item {item_id} not found"
        )

    return {"item_id": item_id}

@app.post("/items/")
async def create_item(name: str, price: float):
    if price <= 0:
        raise HTTPException(
            status_code=422,
            detail="Price must be positive"
        )

    return {"name": name, "price": price}
```

### Exception with Headers

```python
# Example 2: Exception with custom headers
from fastapi import FastAPI, HTTPException

app = FastAPI()

@app.get("/protected/")
async def protected_route():
    raise HTTPException(
        status_code=401,
        detail="Authentication required",
        headers={"WWW-Authenticate": "Bearer"}
    )

@app.get("/rate-limited/")
async def rate_limited():
    raise HTTPException(
        status_code=429,
        detail="Too many requests",
        headers={
            "Retry-After": "60",
            "X-RateLimit-Remaining": "0"
        }
    )
```

## Custom Exception Handlers

### Global Exception Handlers

```python
# Example 3: Custom exception handlers
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
from datetime import datetime

app = FastAPI()

class AppException(Exception):
    """Base application exception"""
    def __init__(self, code: str, message: str, status_code: int = 400):
        self.code = code
        self.message = message
        self.status_code = status_code

class NotFoundError(AppException):
    """Resource not found"""
    def __init__(self, resource: str, id: int):
        super().__init__(
            code="NOT_FOUND",
            message=f"{resource} with id {id} not found",
            status_code=404
        )

class ValidationError(AppException):
    """Validation failed"""
    def __init__(self, message: str):
        super().__init__(
            code="VALIDATION_ERROR",
            message=message,
            status_code=422
        )

@app.exception_handler(AppException)
async def app_exception_handler(request: Request, exc: AppException):
    return JSONResponse(
        status_code=exc.status_code,
        content={
            "error": {
                "code": exc.code,
                "message": exc.message,
                "timestamp": datetime.now().isoformat(),
                "path": str(request.url)
            }
        }
    )

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    if item_id > 1000:
        raise NotFoundError("Item", item_id)
    return {"item_id": item_id}
```

### Validation Error Handler

```python
# Example 4: Custom validation error format
from fastapi import FastAPI, Request
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse

app = FastAPI()

@app.exception_handler(RequestValidationError)
async def validation_exception_handler(
    request: Request,
    exc: RequestValidationError
):
    """Custom format for validation errors"""
    errors = []
    for error in exc.errors():
        errors.append({
            "field": ".".join(str(loc) for loc in error["loc"]),
            "message": error["msg"],
            "type": error["type"]
        })

    return JSONResponse(
        status_code=422,
        content={
            "error": {
                "code": "VALIDATION_ERROR",
                "message": "Request validation failed",
                "details": errors
            }
        }
    )

@app.post("/items/")
async def create_item(name: str, price: float):
    return {"name": name, "price": price}
```

## Exception Patterns

### Domain Exceptions

```python
# Example 5: Domain-specific exceptions
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
from enum import Enum

app = FastAPI()

class ErrorCode(str, Enum):
    NOT_FOUND = "NOT_FOUND"
    ALREADY_EXISTS = "ALREADY_EXISTS"
    INSUFFICIENT_FUNDS = "INSUFFICIENT_FUNDS"
    UNAUTHORIZED = "UNAUTHORIZED"

class DomainException(Exception):
    def __init__(self, code: ErrorCode, message: str, details: dict = None):
        self.code = code
        self.message = message
        self.details = details or {}

@app.exception_handler(DomainException)
async def domain_exception_handler(request: Request, exc: DomainException):
    status_map = {
        ErrorCode.NOT_FOUND: 404,
        ErrorCode.ALREADY_EXISTS: 409,
        ErrorCode.INSUFFICIENT_FUNDS: 400,
        ErrorCode.UNAUTHORIZED: 401
    }

    return JSONResponse(
        status_code=status_map.get(exc.code, 500),
        content={
            "error": {
                "code": exc.code.value,
                "message": exc.message,
                "details": exc.details
            }
        }
    )

@app.post("/withdraw/")
async def withdraw(amount: float, balance: float = 100):
    if amount > balance:
        raise DomainException(
            code=ErrorCode.INSUFFICIENT_FUNDS,
            message="Insufficient funds",
            details={"balance": balance, "requested": amount}
        )
    return {"withdrawn": amount}
```

## Best Practices

### Exception Guidelines

```python
# Example 6: Best practices
from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import JSONResponse

app = FastAPI()

# Consistent error response format
def error_response(code: str, message: str, details: dict = None):
    return JSONResponse(
        status_code=400,
        content={
            "success": False,
            "error": {
                "code": code,
                "message": message,
                "details": details or {}
            }
        }
    )

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    """
    Best practices:
    1. Use consistent error format
    2. Include error codes
    3. Provide helpful messages
    4. Log errors appropriately
    """
    if item_id <= 0:
        return error_response(
            code="INVALID_ID",
            message="Item ID must be positive"
        )

    if item_id > 1000:
        return error_response(
            code="NOT_FOUND",
            message=f"Item {item_id} not found"
        )

    return {"item_id": item_id}
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| HTTPException | Simple errors | `raise HTTPException(404, "Not found")` |
| Custom handler | Global errors | `@app.exception_handler(CustomError)` |
| Validation handler | Format errors | `@app.exception_handler(RequestValidationError)` |
| Domain exceptions | Business logic | Custom exception classes |

## Next Steps

Continue learning about:
- [Custom Responses](./06_custom_responses.md) - Response types
- [Dependencies](../05_dependencies/01_dependency_injection_basics.md) - DI patterns
- [Middleware](../06_middleware/01_middleware_overview.md) - Request processing
