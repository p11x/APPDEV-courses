# Request/Response Cycle

## Overview

Understanding how FastAPI handles requests and responses is fundamental to building effective APIs. This guide explains the complete lifecycle of a request from client to server and back.

## The Complete Request Cycle

### Visual Flow

```
Client Request
    ↓
[1. Request Arrives] → Uvicorn receives HTTP request
    ↓
[2. Middleware] → CORS, authentication, logging
    ↓
[3. Routing] → Match URL to route handler
    ↓
[4. Dependencies] → Execute dependency injection
    ↓
[5. Validation] → Validate parameters and body
    ↓
[6. Handler] → Execute route function
    ↓
[7. Response] → Serialize and send response
    ↓
[8. Middleware] → Process response middleware
    ↓
Client Receives Response
```

## Stage 1: Request Arrival

### HTTP Request Structure

```python
# Example 1: Understanding HTTP request components
from fastapi import FastAPI, Request

app = FastAPI()

@app.get("/debug/request")
async def inspect_request(request: Request):
    """
    The Request object contains all HTTP request information.
    FastAPI wraps Starlette's Request class.
    """
    return {
        # Request method
        "method": request.method,  # GET, POST, etc.

        # URL components
        "url": str(request.url),  # Full URL
        "path": request.url.path,  # Path only
        "query": request.url.query,  # Query string
        "scheme": request.url.scheme,  # http or https
        "hostname": request.url.hostname,

        # Headers
        "headers": dict(request.headers),  # All headers
        "content_type": request.headers.get("content-type"),
        "user_agent": request.headers.get("user-agent"),

        # Client information
        "client_host": request.client.host if request.client else None,
        "client_port": request.client.port if request.client else None,

        # Path parameters (filled by router)
        "path_params": request.path_params,

        # Query parameters
        "query_params": dict(request.query_params),

        # Cookies
        "cookies": request.cookies,
    }
```

## Stage 2: Middleware Processing

### What is Middleware?

Middleware are functions that run before and after each request. They can:
- Modify requests before they reach route handlers
- Modify responses before they're sent to clients
- Short-circuit the request (return early)

```python
# Example 2: Middleware in the request cycle
from fastapi import FastAPI, Request
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.responses import JSONResponse
import time
import logging

app = FastAPI()

# Custom middleware class
class TimingMiddleware(BaseHTTPMiddleware):
    """
    Middleware that measures request processing time.
    Runs before and after every request.
    """

    async def dispatch(self, request: Request, call_next):
        # BEFORE: Request arrives
        start_time = time.time()

        # Log incoming request
        logging.info(f"Incoming: {request.method} {request.url.path}")

        # Process the request (call next middleware or route handler)
        response = await call_next(request)

        # AFTER: Response is ready
        process_time = time.time() - start_time

        # Add timing header to response
        response.headers["X-Process-Time"] = str(process_time)

        logging.info(f"Completed: {response.status_code} in {process_time:.4f}s")

        return response

# Add middleware to app
app.add_middleware(TimingMiddleware)

# Request logging middleware (function-based)
@app.middleware("http")
async def log_requests(request: Request, call_next):
    """
    Alternative middleware using decorator.
    Runs for every HTTP request.
    """
    # Before processing
    print(f"➡️ {request.method} {request.url.path}")

    # Process request
    response = await call_next(request)

    # After processing
    print(f"⬅️ {response.status_code}")

    return response

@app.get("/items/")
async def list_items():
    """This endpoint is processed through middleware"""
    return {"items": ["item1", "item2"]}
```

### Common Middleware Patterns

```python
# Example 3: CORS middleware
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

# CORS (Cross-Origin Resource Sharing) middleware
# Required for browser-based API calls from different domains
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:3000", "https://myapp.com"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Authentication middleware
from fastapi import Request, HTTPException
from starlette.status import HTTP_401_UNAUTHORIZED

@app.middleware("http")
async def auth_middleware(request: Request, call_next):
    """
    Simple authentication middleware.
    Checks for API key in headers.
    """
    # Skip auth for public endpoints
    public_paths = ["/", "/health", "/docs", "/openapi.json"]
    if request.url.path in public_paths:
        return await call_next(request)

    # Check for API key
    api_key = request.headers.get("X-API-Key")
    if not api_key:
        return JSONResponse(
            status_code=HTTP_401_UNAUTHORIZED,
            content={"error": "Missing API key"}
        )

    # Validate API key
    if api_key != "valid-api-key":
        return JSONResponse(
            status_code=HTTP_401_UNAUTHORIZED,
            content={"error": "Invalid API key"}
        )

    return await call_next(request)
```

## Stage 3: Routing

### URL Matching

```python
# Example 4: How FastAPI matches routes
from fastapi import FastAPI

app = FastAPI()

# When a request arrives, FastAPI matches the URL pattern:

# Request: GET /users/42
# Matches: /users/{user_id}
# user_id = 42

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    return {"user_id": user_id}

# Request: GET /users/42/posts/7
# Matches: /users/{user_id}/posts/{post_id}
# user_id = 42, post_id = 7

@app.get("/users/{user_id}/posts/{post_id}")
async def get_user_post(user_id: int, post_id: int):
    return {"user_id": user_id, "post_id": post_id}

# Route precedence matters!
# More specific routes should come first

# This will match /items/search
@app.get("/items/search")
async def search_items():
    return {"action": "search"}

# This will match /items/{item_id} for anything else
@app.get("/items/{item_id}")
async def get_item(item_id: int):
    return {"item_id": item_id}
```

## Stage 4: Dependency Injection

### How Dependencies Work

```python
# Example 5: Dependency injection in the request cycle
from fastapi import FastAPI, Depends, HTTPException, Header
from typing import Optional

app = FastAPI()

# Database session dependency
async def get_db():
    """
    Dependency that provides database session.
    Executed before route handler, cleaned up after.
    """
    db = DatabaseSession()
    try:
        yield db  # Provide to route handler
    finally:
        db.close()  # Cleanup after response

# Authentication dependency
async def get_current_user(
    authorization: str = Header(...),
    db = Depends(get_db)
):
    """
    Dependency that authenticates user.
    Can depend on other dependencies.
    """
    if not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Invalid token")

    token = authorization.split(" ")[1]
    user = db.verify_token(token)

    if not user:
        raise HTTPException(status_code=401, detail="Invalid token")

    return user

# Pagination dependency
class Pagination:
    """Dependency class for pagination"""
    def __init__(self, skip: int = 0, limit: int = 100):
        self.skip = skip
        self.limit = min(limit, 100)  # Max 100 items

# Route with multiple dependencies
@app.get("/users/me/items")
async def get_user_items(
    # Dependencies are resolved in order
    user = Depends(get_current_user),  # First: authenticate
    pagination: Pagination = Depends(Pagination),  # Second: get pagination
    db = Depends(get_db)  # Third: get database
):
    """
    Dependencies are injected before the route runs.
    Each dependency can depend on others.
    """
    items = db.get_items(
        user_id=user.id,
        skip=pagination.skip,
        limit=pagination.limit
    )
    return {"items": items, "user": user.username}
```

## Stage 5: Parameter Validation

### Automatic Validation

```python
# Example 6: FastAPI's automatic validation
from fastapi import FastAPI, Path, Query, Body
from pydantic import BaseModel, Field, EmailStr
from typing import Optional, List
from datetime import datetime

app = FastAPI()

class UserCreate(BaseModel):
    """Request body model with validation"""
    username: str = Field(
        ...,
        min_length=3,
        max_length=50,
        regex="^[a-zA-Z0-9_]+$"
    )
    email: EmailStr  # Automatic email validation
    age: int = Field(..., ge=13, le=120)
    password: str = Field(..., min_length=8)
    tags: List[str] = Field([], max_items=10)

@app.post("/users/")
async def create_user(user: UserCreate):
    """
    FastAPI validates:
    1. Path parameters (type conversion and validation)
    2. Query parameters (type, range, patterns)
    3. Request body (Pydantic model validation)
    4. Headers and cookies

    If validation fails, returns 422 with detailed errors.
    """
    # If we reach here, all validation passed
    return {
        "username": user.username,
        "email": user.email,
        "created": True
    }

# Validation error response example:
"""
When POST /users/ with invalid data:
{
    "username": "ab",  # Too short
    "email": "not-an-email",
    "age": 10,  # Below minimum
    "password": "short"  # Too short
}

FastAPI automatically returns:
{
    "detail": [
        {
            "loc": ["body", "username"],
            "msg": "String should have at least 3 characters",
            "type": "string_too_short"
        },
        {
            "loc": ["body", "email"],
            "msg": "value is not a valid email address",
            "type": "value_error"
        },
        {
            "loc": ["body", "age"],
            "msg": "Input should be greater than or equal to 13",
            "type": "greater_than_equal"
        },
        {
            "loc": ["body", "password"],
            "msg": "String should have at least 8 characters",
            "type": "string_too_short"
        }
    ]
}
Status code: 422 Unprocessable Entity
"""
```

## Stage 6: Route Handler Execution

### Sync vs Async Handlers

```python
# Example 7: Synchronous and asynchronous handlers
from fastapi import FastAPI
import asyncio
import time

app = FastAPI()

# Asynchronous handler (recommended for I/O-bound operations)
@app.get("/async/items")
async def get_items_async():
    """
    Async handlers can use 'await' for non-blocking operations.
    Other requests can be processed while waiting.
    """
    # Simulate async database call
    await asyncio.sleep(0.1)
    return {"items": ["async1", "async2"]}

# Synchronous handler (for CPU-bound or simple operations)
@app.get("/sync/items")
def get_items_sync():
    """
    Sync handlers block during execution.
    Use for CPU-intensive tasks or when async isn't needed.
    """
    # Simulate sync computation
    time.sleep(0.1)
    return {"items": ["sync1", "sync2"]}

# Handler with async I/O
import httpx

@app.get("/external-data")
async def get_external_data():
    """
    Async is ideal for external API calls.
    Multiple requests can run concurrently.
    """
    async with httpx.AsyncClient() as client:
        # These requests run concurrently if multiple users call this endpoint
        response = await client.get("https://api.example.com/data")
        return response.json()
```

## Stage 7: Response Construction

### Automatic Response Creation

```python
# Example 8: How FastAPI creates responses
from fastapi import FastAPI, Response, status
from fastapi.responses import JSONResponse, PlainTextResponse, HTMLResponse
from pydantic import BaseModel
from typing import List, Optional

app = FastAPI()

# Simple dict response (auto-converted to JSON)
@app.get("/dict-response")
async def dict_response():
    """
    Dictionaries are automatically converted to JSON.
    Content-Type: application/json
    """
    return {"message": "Hello", "count": 42}

# List response
@app.get("/list-response")
async def list_response():
    """Lists are also auto-converted to JSON"""
    return [1, 2, 3, 4, 5]

# Pydantic model response
class Item(BaseModel):
    name: str
    price: float

@app.get("/model-response", response_model=Item)
async def model_response():
    """
    Pydantic models are serialized to JSON.
    response_model validates and filters output.
    """
    return Item(name="Laptop", price=999.99)

# Custom response types
@app.get("/text", response_class=PlainTextResponse)
async def text_response():
    """Returns plain text instead of JSON"""
    return "This is plain text"

@app.get("/html", response_class=HTMLResponse)
async def html_response():
    """Returns HTML content"""
    return "<h1>Hello World</h1>"

# Custom JSON response with status code
@app.post("/items/")
async def create_item(name: str):
    """Custom response with specific status code"""
    return JSONResponse(
        status_code=status.HTTP_201_CREATED,
        content={"message": "Item created", "name": name},
        headers={"X-Custom-Header": "value"}
    )

# Response with cookies
@app.get("/set-cookie")
async def set_cookie():
    """Set cookies in response"""
    response = JSONResponse({"message": "Cookie set"})
    response.set_cookie(
        key="session_id",
        value="abc123",
        httponly=True,
        max_age=3600
    )
    return response
```

### Response Model Processing

```python
# Example 9: Response model filtering
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional, List
from datetime import datetime

app = FastAPI()

# Internal model (includes sensitive data)
class UserInternal(BaseModel):
    id: int
    username: str
    email: str
    password_hash: str  # Sensitive!
    api_key: str  # Sensitive!
    created_at: datetime
    last_login: Optional[datetime]

# Public model (safe for API responses)
class UserPublic(BaseModel):
    id: int
    username: str
    email: str
    created_at: datetime

# Response model filters out sensitive fields
@app.get("/users/{user_id}", response_model=UserPublic)
async def get_user(user_id: int):
    """
    Even though we return UserInternal,
    response_model=UserPublic filters the output.
    Sensitive fields are automatically excluded.
    """
    # Database returns internal model
    user_internal = UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="hashed_password_here",
        api_key="secret_api_key",
        created_at=datetime.now(),
        last_login=datetime.now()
    )

    # Response model filters to only public fields
    return user_internal

# List response model
@app.get("/users/", response_model=List[UserPublic])
async def list_users():
    """response_model can be a List of models"""
    return [
        UserInternal(
            id=1,
            username="user1",
            email="user1@example.com",
            password_hash="hash1",
            api_key="key1",
            created_at=datetime.now(),
            last_login=None
        )
    ]

# Include/exclude specific fields
@app.get(
    "/users/{user_id}/basic",
    response_model=UserPublic,
    response_model_include={"id", "username"}  # Only include these
)
async def get_user_basic(user_id: int):
    """Only include id and username in response"""
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="hash",
        api_key="key",
        created_at=datetime.now(),
        last_login=None
    )

@app.get(
    "/users/{user_id}/public",
    response_model=UserPublic,
    response_model_exclude={"created_at"}  # Exclude this field
)
async def get_user_public(user_id: int):
    """Exclude created_at from response"""
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="hash",
        api_key="key",
        created_at=datetime.now(),
        last_login=None
    )
```

## Stage 8: Error Handling

### Exception Processing

```python
# Example 10: How FastAPI handles errors
from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import JSONResponse
from pydantic import BaseModel, ValidationError

app = FastAPI()

# Standard HTTPException
@app.get("/items/{item_id}")
async def get_item(item_id: int):
    """
    HTTPException is automatically converted to JSON response.
    """
    if item_id <= 0:
        raise HTTPException(
            status_code=400,
            detail="Item ID must be positive"
        )

    if item_id > 1000:
        raise HTTPException(
            status_code=404,
            detail=f"Item {item_id} not found",
            headers={"X-Error": "ItemNotFound"}
        )

    return {"item_id": item_id}

# Custom exception handler
class InsufficientFundsError(Exception):
    def __init__(self, balance: float, required: float):
        self.balance = balance
        self.required = required

@app.exception_handler(InsufficientFundsError)
async def insufficient_funds_handler(request: Request, exc: InsufficientFundsError):
    """
    Custom exception handler for application-specific errors.
    """
    return JSONResponse(
        status_code=400,
        content={
            "error": "insufficient_funds",
            "balance": exc.balance,
            "required": exc.required,
            "deficit": exc.required - exc.balance
        }
    )

@app.post("/withdraw")
async def withdraw(amount: float):
    balance = 100.0
    if amount > balance:
        raise InsufficientFundsError(balance=balance, required=amount)
    return {"withdrawn": amount, "new_balance": balance - amount}

# Validation error handler (automatic)
@app.post("/validate")
async def validate_data(data: dict):
    """
    When validation fails, FastAPI automatically returns 422.
    You don't need to handle this manually.
    """
    return data
```

## Complete Request Cycle Example

```python
# Example 11: Complete request cycle with all stages
from fastapi import FastAPI, Depends, HTTPException, Request, Response
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime
import time
import logging

app = FastAPI()

# ==========================================
# STAGE 2: Middleware
# ==========================================

@app.middleware("http")
async def add_process_time_header(request: Request, call_next):
    """Middleware runs before and after every request"""
    start_time = time.time()

    # Log request
    logging.info(f"➡️ {request.method} {request.url.path}")

    # Process request
    response = await call_next(request)

    # Add timing header
    process_time = time.time() - start_time
    response.headers["X-Process-Time"] = str(round(process_time, 4))

    logging.info(f"⬅️ {response.status_code} in {process_time:.4f}s")

    return response

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

# ==========================================
# STAGE 4: Dependencies
# ==========================================

class Pagination:
    """Dependency for pagination parameters"""
    def __init__(
        self,
        skip: int = 0,
        limit: int = Field(10, ge=1, le=100)
    ):
        self.skip = skip
        self.limit = limit

# ==========================================
# Data Models (STAGE 5: Validation)
# ==========================================

class ItemCreate(BaseModel):
    """Model for creating items with validation"""
    name: str = Field(..., min_length=1, max_length=100)
    price: float = Field(..., gt=0)
    description: Optional[str] = None

class Item(ItemCreate):
    """Response model with additional fields"""
    id: int
    created_at: datetime

    class Config:
        from_attributes = True

# ==========================================
# Route Handlers (STAGE 6 & 7)
# ==========================================

items_db = {}
current_id = 0

# STAGE 3: Route matching happens automatically

@app.get("/")
async def root():
    """STAGE 6: Handler executes"""
    return {"message": "Welcome"}

@app.post("/items/", response_model=Item, status_code=201)
async def create_item(
    # STAGE 5: Request body is validated here
    item: ItemCreate
):
    """
    Complete flow:
    1. Request arrives
    2. Middleware processes
    3. Route matches
    4. Dependencies resolve
    5. Body validates
    6. Handler executes
    7. Response serializes
    8. Middleware processes response
    """
    global current_id
    current_id += 1

    new_item = Item(
        id=current_id,
        **item.model_dump(),
        created_at=datetime.now()
    )
    items_db[current_id] = new_item

    return new_item

@app.get("/items/", response_model=List[Item])
async def list_items(
    # STAGE 4: Dependencies injected
    pagination: Pagination = Depends()
):
    """List items with pagination"""
    items = list(items_db.values())
    return items[pagination.skip:pagination.skip + pagination.limit]

@app.get("/items/{item_id}", response_model=Item)
async def get_item(item_id: int):
    """Get specific item"""
    if item_id not in items_db:
        # STAGE 8: Error handling
        raise HTTPException(status_code=404, detail="Item not found")

    return items_db[item_id]
```

## Request Flow Summary

| Stage | Action | Example |
|-------|--------|---------|
| 1. Arrival | HTTP request received | Uvicorn accepts connection |
| 2. Middleware | Pre-processing | CORS, logging, auth |
| 3. Routing | URL pattern matching | `/items/{id}` matches |
| 4. Dependencies | Inject dependencies | Database session, auth |
| 5. Validation | Check parameters/body | Pydantic validation |
| 6. Handler | Execute route function | Business logic runs |
| 7. Response | Serialize output | Dict to JSON |
| 8. Middleware | Post-processing | Add headers, logging |

## Next Steps

Continue learning about:
- [Parameters](./03_parameters_explained.md) - Detailed parameter handling
- [Path Operations](../../02_core_features/01_path_operations/01_basic_routes.md) - Advanced routing
- [Request Body](../../02_core_features/03_request_body/01_basic_request_body.md) - Working with request bodies
