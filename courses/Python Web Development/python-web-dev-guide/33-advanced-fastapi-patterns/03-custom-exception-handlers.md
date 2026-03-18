# Custom Exception Handlers in FastAPI

## What You'll Learn
- FastAPI's exception handling system
- Creating custom exception classes
- Registering exception handlers for specific errors
- HTTPException and validation error handling
- JSON vs custom error response formats

## Prerequisites
- Completed `02-lifespan-events.md` — FastAPI app lifecycle
- Understanding of HTTP status codes
- Basic Python exception handling

## How FastAPI Handles Exceptions

FastAPI has built-in exception handling that converts Python exceptions to HTTP responses:

```
Client Request
      │
      ▼
┌─────────────────────────────────────┐
│  FastAPI tries to process request   │
│                                     │
│  • Validates path parameters        │
│  • Validates query parameters       │
│  • Validates request body           │
│  • Calls your endpoint              │
│                                     │
│  If exception raised:               │
│  • HTTPException → JSON response     │
│  • ValidationError → 422 response   │
│  • Other Exception → 500 response    │
└─────────────────────────────────────┘
      │
      ▼
   Client Response
```

## Built-in HTTPException

```python
from fastapi import FastAPI, HTTPException, status

app = FastAPI()

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    if user_id <= 0:
        # Raise HTTPException with status code and detail
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="User ID must be positive"
        )
    
    if user_id == 999:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="User not found"
        )
    
    return {"user_id": user_id, "name": "John Doe"}
```

🔍 **Line-by-Line Breakdown:**
1. `HTTPException` — FastAPI's built-in exception for HTTP error responses
2. `status_code` — HTTP status (400, 404, etc.) from Starlette
3. `detail` — Error message returned to client as JSON

## Creating Custom Exceptions

### Custom Business Logic Exceptions

```python
from fastapi import HTTPException, status
from dataclasses import dataclass
from datetime import datetime

@dataclass
class BusinessException(Exception):
    """Base exception for business logic errors."""
    message: str
    error_code: str
    status_code: int = status.HTTP_400_BAD_REQUEST

class InsufficientFundsError(BusinessException):
    """Raised when account doesn't have enough money."""
    def __init__(self, available: float, required: float):
        super().__init__(
            message=f"Insufficient funds: have ${available}, need ${required}",
            error_code="INSUFFICIENT_FUNDS",
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY
        )

class UserNotFoundError(BusinessException):
    """Raised when user doesn't exist."""
    def __init__(self, user_id: int):
        super().__init__(
            message=f"User {user_id} not found",
            error_code="USER_NOT_FOUND",
            status_code=status.HTTP_404_NOT_FOUND
        )

# Using custom exceptions in endpoints
@app.post("/accounts/{account_id}/withdraw")
async def withdraw(account_id: int, amount: float):
    # Fetch account
    account = await get_account(account_id)
    if not account:
        raise UserNotFoundError(account_id)
    
    # Check funds
    if account.balance < amount:
        raise InsufficientFundsError(account.balance, amount)
    
    # Process withdrawal
    account.balance -= amount
    await account.save()
    
    return {"account_id": account_id, "new_balance": account.balance}
```

### Exception with Extra Data

```python
from fastapi import Request
from fastapi.responses import JSONResponse

class ValidationWarningException(Exception):
    """Exception for validation warnings that don't block the request."""
    def __init__(self, message: str, warnings: list[str]):
        self.message = message
        self.warnings = warnings

@app.exception_handler(ValidationWarningException)
async def validation_warning_handler(
    request: Request,
    exc: ValidationWarningException
):
    """Return warning information alongside successful response."""
    return JSONResponse(
        status_code=200,
        content={
            "success": True,
            "warning": exc.message,
            "warnings": exc.warnings
        }
    )

@app.get("/data/{data_id}")
async def get_data(data_id: int):
    data = await fetch_data(data_id)
    
    warnings = []
    if data.is_expired:
        warnings.append("This data is expired")
    if data.size > 1000000:
        warnings.append("Large data size may affect performance")
    
    if warnings:
        raise ValidationWarningException(
            message="Data retrieved with warnings",
            warnings=warnings
        )
    
    return data
```

## Registering Exception Handlers

### Basic Handler

```python
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI()

# Register handler for specific exception type
@app.exception_handler(ValueError)
async def value_error_handler(request: Request, exc: ValueError):
    return JSONResponse(
        status_code=400,
        content={
            "error": "Value Error",
            "message": str(exc),
            "path": str(request.url)
        }
    )

@app.get("/divide/{a}/{b}")
async def divide(a: int, b: int):
    if b == 0:
        raise ValueError("Cannot divide by zero")
    return {"result": a / b}
```

### Handler for All HTTPException

```python
from fastapi import HTTPException, status
from fastapi.exceptions import RequestValidationError

@app.exception_handler(HTTPException)
async def http_exception_handler(request: Request, exc: HTTPException):
    """Format all HTTPException responses consistently."""
    return JSONResponse(
        status_code=exc.status_code,
        content={
            "error": {
                "code": exc.status_code,
                "message": exc.detail,
                "path": str(request.url.path),
                "method": request.method
            }
        }
    )

@app.exception_handler(RequestValidationError)
async def validation_exception_handler(request: Request, exc: RequestValidationError):
    """Format validation errors with detailed field information."""
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content={
            "error": "Validation Error",
            "detail": exc.errors(),
            "body": exc.body
        }
    )
```

## Advanced: Custom Error Response Format

### Unified API Response Structure

```python
from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import JSONResponse
from enum import Enum
from pydantic import BaseModel
from typing import Any
from datetime import datetime

class ErrorCode(str, Enum):
    """Standard error codes."""
    NOT_FOUND = "NOT_FOUND"
    VALIDATION_ERROR = "VALIDATION_ERROR"
    INTERNAL_ERROR = "INTERNAL_ERROR"
    UNAUTHORIZED = "UNAUTHORIZED"
    FORBIDDEN = "FORBIDDEN"

class ErrorResponse(BaseModel):
    """Standard error response format."""
    success: bool = False
    error: dict[str, Any]

def create_error_response(
    code: ErrorCode,
    message: str,
    details: dict[str, Any] | None = None,
    status_code: int = 400
) -> JSONResponse:
    """Create standardized error response."""
    error_body = {
        "code": code.value,
        "message": message,
        "timestamp": datetime.utcnow().isoformat()
    }
    
    if details:
        error_body["details"] = details
    
    return JSONResponse(
        status_code=status_code,
        content={"success": False, "error": error_body}
    )

# Use in handlers
@app.exception_handler(HTTPException)
async def unified_http_exception_handler(request: Request, exc: HTTPException):
    code_map = {
        404: ErrorCode.NOT_FOUND,
        401: ErrorCode.UNAUTHORIZED,
        403: ErrorCode.FORBIDDEN,
    }
    
    return create_error_response(
        code=code_map.get(exc.status_code, ErrorCode.INTERNAL_ERROR),
        message=exc.detail,
        status_code=exc.status_code
    )

# Example endpoint using unified responses
@app.get("/products/{product_id}")
async def get_product(product_id: int):
    product = await db.products.get(product_id)
    
    if not product:
        return create_error_response(
            code=ErrorCode.NOT_FOUND,
            message=f"Product {product_id} not found",
            status_code=404
        )
    
    return {
        "success": True,
        "data": product
    }
```

## Production Considerations

- **Never expose internal errors**: Return generic 500 for unexpected errors
- **Log exceptions**: Always log errors before returning to client
- **Consistent format**: Use same error format across all endpoints
- **Include request context**: Add path/method to errors for debugging

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Raising generic Exception

**Wrong:**
```python
@app.get("/user/{user_id}")
async def get_user(user_id: int):
    user = db.get(user_id)
    if not user:
        raise Exception("User not found")  # Returns 500!
    return user
```

**Why it fails:** Generic Exception returns 500 Internal Server Error, not 404.

**Fix:**
```python
@app.get("/user/{user_id}")
async def get_user(user_id: int):
    user = db.get(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return user
```

### ❌ Mistake 2: Not handling validation errors

**Wrong:**
```python
# No custom handler for validation errors
# User sees default FastAPI error format
```

**Why it fails:** Default validation errors might not match your API's format.

**Fix:**
```python
from fastapi.exceptions import RequestValidationError

@app.exception_handler(RequestValidationError)
async def custom_validation_handler(request: Request, exc: RequestValidationError):
    return JSONResponse(
        status_code=422,
        content={
            "success": False,
            "error": {
                "code": "VALIDATION_ERROR",
                "fields": exc.errors()
            }
        }
    )
```

### ❌ Mistake 3: Swallowing exceptions

**Wrong:**
```python
async def get_data(data_id: int):
    try:
        return await fetch_data(data_id)
    except Exception:
        return {"data": None}  # Hides the error!
```

**Why it fails:** Client doesn't know something went wrong.

**Fix:**
```python
async def get_data(data_id: int):
    try:
        return await fetch_data(data_id)
    except DatabaseError as e:
        raise HTTPException(
            status_code=503,
            detail=f"Database unavailable: {e}"
        )
```

## Summary

- Use HTTPException for HTTP-specific errors (400, 404, etc.)
- Create custom exception classes for business logic errors
- Register handlers with `@app.exception_handler(ExceptionType)`
- Return consistent error format across your API
- Log errors before returning to client in production

## Next Steps

→ Continue to `04-response-streaming.md` to learn about streaming responses and server-sent events.
