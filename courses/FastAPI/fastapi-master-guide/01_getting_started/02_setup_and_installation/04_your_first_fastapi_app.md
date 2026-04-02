# Your First FastAPI Application

## Overview

This guide walks you through building your first FastAPI application from scratch. By the end, you'll have a working API with multiple endpoints, validation, and automatic documentation.

## Step 1: Project Setup

### Create Project Structure

```bash
# Create project directory
mkdir my_first_fastapi
cd my_first_fastapi

# Create virtual environment
python -m venv venv

# Activate virtual environment
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate

# Install dependencies
pip install "fastapi[standard]"

# Create project structure
mkdir app tests
touch app/__init__.py
touch app/main.py
touch app/models.py
touch app/routers
touch requirements.txt
```

### Create requirements.txt

```text
# requirements.txt
fastapi>=0.100.0
uvicorn[standard]>=0.23.0
python-multipart>=0.0.6
```

## Step 2: Basic Application

### Create Main Application File

```python
# app/main.py - Your first FastAPI application
from fastapi import FastAPI

# Create the FastAPI application instance
# This is the main entry point for your API
app = FastAPI(
    title="My First API",
    description="A simple API built with FastAPI",
    version="1.0.0"
)

# Root endpoint - the first route users will see
@app.get("/")
async def root():
    """
    Root endpoint that returns a welcome message.

    FastAPI automatically:
    - Converts the returned dict to JSON
    - Sets Content-Type header to application/json
    - Generates documentation for this endpoint
    """
    return {
        "message": "Welcome to My First FastAPI API!",
        "documentation": "/docs",
        "version": "1.0.0"
    }

# Health check endpoint - useful for monitoring
@app.get("/health")
async def health_check():
    """
    Health check endpoint.
    Load balancers and monitoring tools use this
    to verify the service is running.
    """
    return {"status": "healthy", "timestamp": "2024-01-01T00:00:00Z"}

# Run the application:
# uvicorn app.main:app --reload
# Then visit: http://localhost:8000
# For docs: http://localhost:8000/docs
```

### Run Your Application

```bash
# Start the development server
uvicorn app.main:app --reload

# You should see:
# INFO:     Uvicorn running on http://127.0.0.1:8000 (Press CTRL+C to quit)
# INFO:     Started reloader process
# INFO:     Started server process
# INFO:     Waiting for application startup.
# INFO:     Application startup complete.
```

### Test Your Application

```bash
# Test with curl
curl http://localhost:8000/

# Response:
# {"message":"Welcome to My First FastAPI API!","documentation":"/docs","version":"1.0.0"}

# Test health endpoint
curl http://localhost:8000/health

# Visit in browser:
# http://localhost:8000/docs - Interactive API documentation
# http://localhost:8000/redoc - Alternative documentation
```

## Step 3: Add Data Models

### Create Pydantic Models

```python
# app/models.py - Data models for validation
from pydantic import BaseModel, Field, EmailStr
from typing import Optional
from datetime import datetime
from enum import Enum

# Enum for predefined values
class ItemCategory(str, Enum):
    """
    Enumeration for item categories.
    Using str ensures JSON serialization works correctly.
    """
    ELECTRONICS = "electronics"
    CLOTHING = "clothing"
    BOOKS = "books"
    HOME = "home"
    SPORTS = "sports"

# Base model for shared fields
class ItemBase(BaseModel):
    """
    Base item model with common fields.
    Used for both creating and updating items.
    """
    name: str = Field(
        ...,
        min_length=1,
        max_length=100,
        description="Name of the item",
        examples=["Laptop", "T-Shirt"]
    )
    description: Optional[str] = Field(
        None,
        max_length=500,
        description="Detailed description of the item"
    )
    price: float = Field(
        ...,
        gt=0,
        description="Price must be greater than 0",
        examples=[29.99, 149.99]
    )
    category: ItemCategory = Field(
        ...,
        description="Category of the item"
    )
    in_stock: bool = Field(
        True,
        description="Whether the item is in stock"
    )

# Model for creating items (no ID needed)
class ItemCreate(ItemBase):
    """
    Model for creating new items.
    Inherits all fields from ItemBase.
    """
    pass

# Model for updating items (all fields optional)
class ItemUpdate(BaseModel):
    """
    Model for updating items.
    All fields are optional for partial updates.
    """
    name: Optional[str] = Field(None, min_length=1, max_length=100)
    description: Optional[str] = Field(None, max_length=500)
    price: Optional[float] = Field(None, gt=0)
    category: Optional[ItemCategory] = None
    in_stock: Optional[bool] = None

# Complete item model with ID
class Item(ItemBase):
    """
    Complete item model with database fields.
    Used for API responses.
    """
    id: int = Field(..., description="Unique identifier")
    created_at: datetime = Field(
        default_factory=datetime.now,
        description="Creation timestamp"
    )
    updated_at: Optional[datetime] = Field(
        None,
        description="Last update timestamp"
    )

    class Config:
        # Enable ORM mode for SQLAlchemy compatibility
        from_attributes = True
        # Example for documentation
        json_schema_extra = {
            "example": {
                "id": 1,
                "name": "Gaming Laptop",
                "description": "High-performance gaming laptop",
                "price": 1299.99,
                "category": "electronics",
                "in_stock": True,
                "created_at": "2024-01-01T00:00:00"
            }
        }

# User models for authentication example
class UserBase(BaseModel):
    """Base user model"""
    username: str = Field(..., min_length=3, max_length=50)
    email: str = Field(..., description="User email address")
    full_name: Optional[str] = None

class UserCreate(UserBase):
    """User creation model with password"""
    password: str = Field(..., min_length=8, description="User password")

class User(UserBase):
    """User response model (without password)"""
    id: int
    is_active: bool = True

    class Config:
        from_attributes = True
```

## Step 4: Add More Endpoints

### Update Main Application

```python
# app/main.py - Updated with more endpoints
from fastapi import FastAPI, HTTPException, Query, Path
from typing import Optional, List
from datetime import datetime

from .models import Item, ItemCreate, ItemUpdate, ItemCategory

app = FastAPI(
    title="My First API",
    description="A complete CRUD API built with FastAPI",
    version="1.0.0"
)

# In-memory database (for demonstration)
# In production, use a real database
items_db: dict[int, dict] = {}
current_id: int = 0

# ==========================================
# Root and Health Endpoints
# ==========================================

@app.get("/")
async def root():
    """Welcome endpoint with API information"""
    return {
        "message": "Welcome to My First FastAPI API!",
        "documentation": "/docs",
        "endpoints": {
            "items": "/items",
            "health": "/health"
        }
    }

@app.get("/health")
async def health_check():
    """Health check for monitoring"""
    return {
        "status": "healthy",
        "version": "1.0.0",
        "timestamp": datetime.now().isoformat()
    }

# ==========================================
# Item CRUD Endpoints
# ==========================================

@app.post("/items/", response_model=Item, status_code=201)
async def create_item(item: ItemCreate):
    """
    Create a new item.

    - **name**: Item name (1-100 characters)
    - **price**: Item price (must be > 0)
    - **category**: One of: electronics, clothing, books, home, sports
    - **description**: Optional detailed description
    - **in_stock**: Availability status (default: true)
    """
    global current_id
    current_id += 1

    # Create item with ID and timestamp
    new_item = Item(
        id=current_id,
        **item.model_dump(),
        created_at=datetime.now()
    )

    # Store in database
    items_db[current_id] = new_item.model_dump()

    return new_item

@app.get("/items/", response_model=List[Item])
async def list_items(
    # Query parameters with validation
    skip: int = Query(0, ge=0, description="Number of items to skip"),
    limit: int = Query(10, ge=1, le=100, description="Max items to return"),
    category: Optional[ItemCategory] = Query(None, description="Filter by category"),
    min_price: Optional[float] = Query(None, ge=0, description="Minimum price filter"),
    max_price: Optional[float] = Query(None, gt=0, description="Maximum price filter"),
    in_stock: Optional[bool] = Query(None, description="Filter by stock status")
):
    """
    List all items with optional filtering.

    Supports:
    - Pagination with skip/limit
    - Category filtering
    - Price range filtering
    - Stock status filtering
    """
    # Start with all items
    filtered_items = list(items_db.values())

    # Apply filters
    if category:
        filtered_items = [i for i in filtered_items if i["category"] == category]

    if min_price is not None:
        filtered_items = [i for i in filtered_items if i["price"] >= min_price]

    if max_price is not None:
        filtered_items = [i for i in filtered_items if i["price"] <= max_price]

    if in_stock is not None:
        filtered_items = [i for i in filtered_items if i["in_stock"] == in_stock]

    # Apply pagination
    paginated = filtered_items[skip:skip + limit]

    return paginated

@app.get("/items/{item_id}", response_model=Item)
async def get_item(
    item_id: int = Path(..., ge=1, description="The ID of the item to retrieve")
):
    """
    Get a specific item by ID.

    - **item_id**: The unique identifier of the item

    Returns 404 if item not found.
    """
    if item_id not in items_db:
        raise HTTPException(
            status_code=404,
            detail=f"Item with id {item_id} not found"
        )

    return items_db[item_id]

@app.put("/items/{item_id}", response_model=Item)
async def update_item(
    item_id: int = Path(..., ge=1),
    item_update: ItemUpdate = ...
):
    """
    Update an existing item.

    Supports partial updates - only provided fields will be updated.
    Returns 404 if item not found.
    """
    if item_id not in items_db:
        raise HTTPException(
            status_code=404,
            detail=f"Item with id {item_id} not found"
        )

    # Get current item
    current_item = items_db[item_id]

    # Update only provided fields
    update_data = item_update.model_dump(exclude_unset=True)
    for field, value in update_data.items():
        if value is not None:
            current_item[field] = value

    # Update timestamp
    current_item["updated_at"] = datetime.now()

    # Save and return
    items_db[item_id] = current_item
    return current_item

@app.delete("/items/{item_id}", status_code=204)
async def delete_item(
    item_id: int = Path(..., ge=1)
):
    """
    Delete an item by ID.

    Returns 204 No Content on success.
    Returns 404 if item not found.
    """
    if item_id not in items_db:
        raise HTTPException(
            status_code=404,
            detail=f"Item with id {item_id} not found"
        )

    del items_db[item_id]
    # No content returned for 204

# ==========================================
# Search and Filter Endpoints
# ==========================================

@app.get("/items/search/", response_model=List[Item])
async def search_items(
    q: str = Query(..., min_length=1, description="Search query"),
    category: Optional[ItemCategory] = None
):
    """
    Search items by name or description.

    - **q**: Search term (required, minimum 1 character)
    - **category**: Optional category filter
    """
    results = []

    for item in items_db.values():
        # Search in name and description
        name_match = q.lower() in item["name"].lower()
        desc_match = item.get("description") and q.lower() in item["description"].lower()

        if name_match or desc_match:
            if category is None or item["category"] == category:
                results.append(item)

    return results

@app.get("/items/stats/")
async def item_statistics():
    """
    Get statistics about items in the database.
    Useful for dashboard displays.
    """
    items = list(items_db.values())

    if not items:
        return {"total_items": 0, "message": "No items in database"}

    # Calculate statistics
    total_items = len(items)
    total_value = sum(item["price"] for item in items)
    in_stock_count = sum(1 for item in items if item["in_stock"])

    # Category breakdown
    categories = {}
    for item in items:
        cat = item["category"]
        categories[cat] = categories.get(cat, 0) + 1

    return {
        "total_items": total_items,
        "total_value": round(total_value, 2),
        "in_stock": in_stock_count,
        "out_of_stock": total_items - in_stock_count,
        "categories": categories,
        "average_price": round(total_value / total_items, 2) if total_items > 0 else 0
    }
```

## Step 5: Organize with Routers

### Create Item Router

```python
# app/routers/__init__.py
# Empty file to make routers a package
```

```python
# app/routers/items.py - Item-related endpoints
from fastapi import APIRouter, HTTPException, Query, Path
from typing import Optional, List
from datetime import datetime

from ..models import Item, ItemCreate, ItemUpdate, ItemCategory

# Create router with prefix and tags
router = APIRouter(
    prefix="/items",
    tags=["items"],
    responses={
        404: {"description": "Item not found"},
        422: {"description": "Validation error"}
    }
)

# In-memory storage (shared with main app in real scenario)
items_db: dict[int, dict] = {}
current_id: int = 0

@router.post("/", response_model=Item, status_code=201)
async def create_item(item: ItemCreate):
    """Create a new item"""
    global current_id
    current_id += 1

    new_item = Item(
        id=current_id,
        **item.model_dump(),
        created_at=datetime.now()
    )
    items_db[current_id] = new_item.model_dump()
    return new_item

@router.get("/", response_model=List[Item])
async def list_items(
    skip: int = Query(0, ge=0),
    limit: int = Query(10, ge=1, le=100),
    category: Optional[ItemCategory] = None
):
    """List all items with optional filtering"""
    items = list(items_db.values())

    if category:
        items = [i for i in items if i["category"] == category]

    return items[skip:skip + limit]

@router.get("/{item_id}", response_model=Item)
async def get_item(item_id: int = Path(..., ge=1)):
    """Get a specific item by ID"""
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")
    return items_db[item_id]

@router.put("/{item_id}", response_model=Item)
async def update_item(item_id: int, item_update: ItemUpdate):
    """Update an item"""
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")

    current = items_db[item_id]
    update_data = item_update.model_dump(exclude_unset=True)
    current.update(update_data)
    current["updated_at"] = datetime.now()

    return current

@router.delete("/{item_id}", status_code=204)
async def delete_item(item_id: int):
    """Delete an item"""
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")
    del items_db[item_id]
```

### Create Users Router

```python
# app/routers/users.py - User-related endpoints
from fastapi import APIRouter, HTTPException, Query, Path
from typing import Optional, List

from ..models import User, UserCreate

router = APIRouter(
    prefix="/users",
    tags=["users"],
    responses={404: {"description": "User not found"}}
)

# In-memory user storage
users_db: dict[int, dict] = {}
user_id_counter: int = 0

@router.post("/", response_model=User, status_code=201)
async def create_user(user: UserCreate):
    """
    Create a new user.

    Note: In production, passwords should be hashed!
    """
    global user_id_counter
    user_id_counter += 1

    # Don't store password in response
    new_user = User(
        id=user_id_counter,
        username=user.username,
        email=user.email,
        full_name=user.full_name
    )

    # Store with password for authentication
    users_db[user_id_counter] = {
        **new_user.model_dump(),
        "password": user.password  # Hash this in production!
    }

    return new_user

@router.get("/", response_model=List[User])
async def list_users(
    skip: int = Query(0, ge=0),
    limit: int = Query(10, ge=1, le=100)
):
    """List all users"""
    users = [
        User(**{k: v for k, v in u.items() if k != "password"})
        for u in list(users_db.values())[skip:skip + limit]
    ]
    return users

@router.get("/{user_id}", response_model=User)
async def get_user(user_id: int = Path(..., ge=1)):
    """Get a specific user"""
    if user_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")

    user_data = users_db[user_id]
    return User(**{k: v for k, v in user_data.items() if k != "password"})
```

### Update Main Application with Routers

```python
# app/main.py - Updated to use routers
from fastapi import FastAPI
from datetime import datetime

from .routers import items, users

app = FastAPI(
    title="My First API",
    description="A complete CRUD API with multiple routers",
    version="1.0.0"
)

# Include routers
# This adds all routes from items router under /items prefix
app.include_router(items.router)
# This adds all routes from users router under /users prefix
app.include_router(users.router)

@app.get("/")
async def root():
    """API root with navigation"""
    return {
        "message": "Welcome to My First FastAPI API!",
        "documentation": "/docs",
        "endpoints": {
            "items": "/items",
            "users": "/users",
            "health": "/health"
        }
    }

@app.get("/health")
async def health_check():
    """Health check endpoint"""
    return {
        "status": "healthy",
        "version": "1.0.0",
        "timestamp": datetime.now().isoformat()
    }
```

## Step 6: Error Handling

### Custom Exception Handler

```python
# app/exceptions.py - Custom exception handling
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
from typing import Any

class ItemNotFoundError(Exception):
    """Custom exception for missing items"""
    def __init__(self, item_id: int):
        self.item_id = item_id
        self.message = f"Item with id {item_id} not found"

class InsufficientStockError(Exception):
    """Custom exception for stock issues"""
    def __init__(self, item_id: int, requested: int, available: int):
        self.item_id = item_id
        self.requested = requested
        self.available = available
        self.message = f"Insufficient stock for item {item_id}"

def register_exception_handlers(app: FastAPI):
    """Register custom exception handlers"""

    @app.exception_handler(ItemNotFoundError)
    async def item_not_found_handler(request: Request, exc: ItemNotFoundError):
        return JSONResponse(
            status_code=404,
            content={
                "error": "item_not_found",
                "message": exc.message,
                "item_id": exc.item_id
            }
        )

    @app.exception_handler(InsufficientStockError)
    async def insufficient_stock_handler(request: Request, exc: InsufficientStockError):
        return JSONResponse(
            status_code=400,
            content={
                "error": "insufficient_stock",
                "message": exc.message,
                "item_id": exc.item_id,
                "requested": exc.requested,
                "available": exc.available
            }
        )

    @app.exception_handler(Exception)
    async def general_exception_handler(request: Request, exc: Exception):
        # Log the error here
        return JSONResponse(
            status_code=500,
            content={
                "error": "internal_server_error",
                "message": "An unexpected error occurred"
            }
        )
```

## Step 7: Configuration Management

### Settings Management

```python
# app/config.py - Application configuration
from pydantic_settings import BaseSettings
from functools import lru_cache

class Settings(BaseSettings):
    """
    Application settings.
    Loads from environment variables and .env file.
    """
    # Application settings
    app_name: str = "My First FastAPI App"
    app_version: str = "1.0.0"
    debug: bool = False

    # Database settings
    database_url: str = "sqlite:///./app.db"

    # Security settings
    secret_key: str = "change-this-in-production"
    access_token_expire_minutes: int = 30

    # CORS settings
    allowed_origins: list[str] = ["http://localhost:3000"]

    class Config:
        env_file = ".env"
        env_file_encoding = "utf-8"
        case_sensitive = False

@lru_cache()
def get_settings() -> Settings:
    """Get cached settings instance"""
    return Settings()
```

### Updated Main with Configuration

```python
# app/main.py - With configuration
from fastapi import FastAPI, Depends
from fastapi.middleware.cors import CORSMiddleware

from .config import Settings, get_settings
from .routers import items, users
from .exceptions import register_exception_handlers

def create_application(settings: Settings) -> FastAPI:
    """Application factory function"""
    application = FastAPI(
        title=settings.app_name,
        version=settings.app_version,
        debug=settings.debug
    )

    # Add CORS middleware
    application.add_middleware(
        CORSMiddleware,
        allow_origins=settings.allowed_origins,
        allow_credentials=True,
        allow_methods=["*"],
        allow_headers=["*"],
    )

    # Include routers
    application.include_router(items.router)
    application.include_router(users.router)

    # Register exception handlers
    register_exception_handlers(application)

    return application

# Create application instance
settings = get_settings()
app = create_application(settings)

@app.get("/")
async def root(settings: Settings = Depends(get_settings)):
    """Root endpoint with configuration info"""
    return {
        "app_name": settings.app_name,
        "version": settings.app_version,
        "documentation": "/docs"
    }
```

## Step 8: Testing Your Application

### Basic Tests

```python
# tests/test_main.py - Application tests
import pytest
from httpx import AsyncClient
from app.main import app

@pytest.mark.asyncio
async def test_root():
    """Test root endpoint"""
    async with AsyncClient(app=app, base_url="http://test") as client:
        response = await client.get("/")
    assert response.status_code == 200
    data = response.json()
    assert "app_name" in data

@pytest.mark.asyncio
async def test_create_item():
    """Test creating an item"""
    async with AsyncClient(app=app, base_url="http://test") as client:
        item_data = {
            "name": "Test Item",
            "price": 19.99,
            "category": "electronics",
            "description": "A test item"
        }
        response = await client.post("/items/", json=item_data)

    assert response.status_code == 201
    data = response.json()
    assert data["name"] == "Test Item"
    assert data["price"] == 19.99
    assert "id" in data

@pytest.mark.asyncio
async def test_create_item_validation():
    """Test validation on item creation"""
    async with AsyncClient(app=app, base_url="http://test") as client:
        # Missing required fields
        invalid_data = {"name": ""}
        response = await client.post("/items/", json=invalid_data)

    assert response.status_code == 422

@pytest.mark.asyncio
async def test_get_nonexistent_item():
    """Test getting non-existent item"""
    async with AsyncClient(app=app, base_url="http://test") as client:
        response = await client.get("/items/99999")

    assert response.status_code == 404
```

### Running Tests

```bash
# Run all tests
pytest

# Run with verbose output
pytest -v

# Run specific test
pytest tests/test_main.py::test_create_item

# Run with coverage
pytest --cov=app --cov-report=html
```

## Step 9: Run the Complete Application

### Final Project Structure

```
my_first_fastapi/
├── app/
│   ├── __init__.py
│   ├── main.py           # Application entry point
│   ├── config.py         # Configuration management
│   ├── models.py         # Pydantic models
│   ├── exceptions.py     # Custom exception handlers
│   └── routers/
│       ├── __init__.py
│       ├── items.py      # Item endpoints
│       └── users.py      # User endpoints
├── tests/
│   ├── __init__.py
│   └── test_main.py      # Application tests
├── .env                  # Environment variables
├── requirements.txt      # Dependencies
└── README.md            # Documentation
```

### Running the Application

```bash
# Start development server
uvicorn app.main:app --reload --host 0.0.0.0 --port 8000

# Or use Python directly
python -m uvicorn app.main:app --reload

# Access your API:
# - http://localhost:8000/ - Root endpoint
# - http://localhost:8000/docs - Swagger UI documentation
# - http://localhost:8000/redoc - ReDoc documentation
# - http://localhost:8000/items/ - Items endpoint
# - http://localhost:8000/users/ - Users endpoint
```

### Testing with Swagger UI

1. Open http://localhost:8000/docs
2. Click on any endpoint to expand it
3. Click "Try it out"
4. Enter parameters and click "Execute"
5. View the response

## Summary

You've built a complete FastAPI application with:

| Feature | Implementation |
|---------|----------------|
| Multiple Endpoints | Root, health, CRUD operations |
| Data Validation | Pydantic models with validation |
| Error Handling | Custom exceptions and HTTP errors |
| Documentation | Auto-generated Swagger/ReDoc |
| Organization | Routers for logical grouping |
| Configuration | Environment-based settings |
| Testing | Basic test suite |

## Next Steps

Continue learning about:
- [Routing](../03_basic_concepts/01_routing.md) - Deep dive into routing
- [Request/Response Cycle](../03_basic_concepts/02_request_response_cycle.md) - Understanding the flow
- [Parameters](../03_basic_concepts/03_parameters_explained.md) - Working with parameters
