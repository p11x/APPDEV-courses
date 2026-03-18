# API Error Handling

## What You'll Learn
- Consistent error responses
- Custom exceptions
- Error logging

## Prerequisites
- Completed API versioning

## Standard Error Response

```python
from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import JSONResponse
from pydantic import BaseModel
from typing import Optional

app = FastAPI()

# Standard error model
class ErrorResponse(BaseModel):
    error: str
    message: str
    details: Optional[dict] = None
    code: Optional[str] = None

# Custom exceptions
class UserNotFoundException(Exception):
    def __init__(self, user_id: int):
        self.user_id = user_id
        super().__init__(f"User {user_id} not found")

class ValidationException(Exception):
    def __init__(self, message: str, field: str):
        self.message = message
        self.field = field
        super().__init__(message)

# Exception handlers
@app.exception_handler(UserNotFoundException)
async def user_not_found_handler(request: Request, exc: UserNotFoundException):
    return JSONResponse(
        status_code=404,
        content={
            "error": "USER_NOT_FOUND",
            "message": f"User with id {exc.user_id} not found",
            "details": {"user_id": exc.user_id}
        }
    )

@app.exception_handler(ValidationException)
async def validation_exception_handler(request: Request, exc: ValidationException):
    return JSONResponse(
        status_code=400,
        content={
            "error": "VALIDATION_ERROR",
            "message": exc.message,
            "details": {"field": exc.field}
        }
    )

# Routes
@app.get("/users/{user_id}")
async def get_user(user_id: int):
    if user_id == 999:
        raise UserNotFoundException(user_id)
    return {"id": user_id, "name": "John"}

@app.post("/users")
async def create_user(name: str):
    if len(name) < 2:
        raise ValidationException("Name too short", "name")
    return {"id": 1, "name": name}
```

## HTTPException with Details

```python
from fastapi import HTTPException

@app.get("/product/{product_id}")
async def get_product(product_id: int):
    # Basic error
    if product_id <= 0:
        raise HTTPException(
            status_code=400,
            detail="Product ID must be positive"
        )
    
    # Error with more details
    if product_id > 1000:
        raise HTTPException(
            status_code=404,
            detail={
                "error": "PRODUCT_NOT_FOUND",
                "product_id": product_id,
                "suggestion": "Try a product ID between 1 and 1000"
            }
        )
    
    return {"id": product_id, "name": f"Product {product_id}"}
```

## Summary
- Return consistent error format
- Use custom exceptions
- Include error codes

## Next Steps
→ Continue to `04-pagination-and-filtering.md`
