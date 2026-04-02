# Prerequisites

## Overview

Before diving into FastAPI development, ensure you have the necessary foundation and tools. This guide covers everything you need to know before writing your first FastAPI application.

## Required Knowledge

### 1. Python Fundamentals

FastAPI requires solid Python knowledge. You should be comfortable with:

#### Python 3.9+
FastAPI requires Python 3.9 or higher. Verify your Python version:

```bash
# Check Python version
python --version
# Should output: Python 3.9.x or higher

# Alternative check
python3 --version
```

#### Essential Python Concepts

```python
# Example 1: Understanding Python basics required for FastAPI

# 1. Functions and decorators
def my_decorator(func):
    """Decorators are used extensively in FastAPI for route definitions"""
    def wrapper(*args, **kwargs):
        print("Before function call")
        result = func(*args, **kwargs)
        print("After function call")
        return result
    return wrapper

@my_decorator
def say_hello(name: str) -> str:
    return f"Hello, {name}!"

# 2. Type hints (critical for FastAPI)
def process_user(name: str, age: int, active: bool = True) -> dict:
    """
    Type hints in FastAPI serve multiple purposes:
    - Data validation
    - Documentation generation
    - Editor autocompletion
    """
    return {"name": name, "age": age, "active": active}

# 3. Dictionaries and JSON-like structures
user_data = {
    "id": 1,
    "username": "johndoe",
    "email": "john@example.com",
    "preferences": {
        "theme": "dark",
        "notifications": True
    },
    "tags": ["admin", "premium"]
}

# 4. List comprehensions
active_users = [user for user in ["alice", "bob", "charlie"] if user != "bob"]

# 5. Exception handling
def safe_divide(a: float, b: float) -> float | None:
    try:
        return a / b
    except ZeroDivisionError:
        return None
    finally:
        print("Division operation completed")
```

### 2. Type Hints (Python Typing)

Type hints are fundamental to FastAPI. Understanding them is essential:

```python
# Example 2: Python type hints - essential for FastAPI
from typing import Optional, List, Dict, Union
from pydantic import BaseModel

# Basic type hints
name: str = "John"
age: int = 30
price: float = 19.99
is_active: bool = True

# Optional types (can be None)
middle_name: Optional[str] = None
nickname: str | None = None  # Python 3.10+ syntax

# Collection types
numbers: List[int] = [1, 2, 3, 4, 5]
scores: list[float] = [95.5, 87.3, 92.1]  # Python 3.9+ syntax
user_data: Dict[str, Union[str, int]] = {"name": "John", "age": 30}
config: dict[str, any] = {"debug": True, "version": 2}  # Python 3.9+

# Function type hints
def calculate_total(prices: list[float], tax_rate: float = 0.1) -> float:
    """
    Function with type hints.
    FastAPI uses these for:
    - Automatic request parsing
    - Validation
    - Documentation
    """
    subtotal = sum(prices)
    return subtotal * (1 + tax_rate)

# Class type hints
class User:
    def __init__(self, id: int, name: str, email: str):
        self.id = id
        self.name = name
        self.email = email

    def to_dict(self) -> dict[str, any]:
        return {"id": self.id, "name": self.name, "email": self.email}
```

### 3. Async/Await Basics

Understanding asynchronous Python is important for FastAPI:

```python
# Example 3: Async/await - important for FastAPI performance
import asyncio
from typing import Any

# Synchronous function - blocks during execution
def sync_fetch_data(url: str) -> dict:
    """This blocks while waiting for network response"""
    import time
    time.sleep(1)  # Simulates network delay
    return {"url": url, "status": "success"}

# Asynchronous function - doesn't block
async def async_fetch_data(url: str) -> dict:
    """
    This allows other requests to be processed while waiting.
    FastAPI can handle many concurrent requests with async.
    """
    await asyncio.sleep(1)  # Non-blocking wait
    return {"url": url, "status": "success"}

# Running async code
async def main():
    # These run concurrently, not sequentially
    results = await asyncio.gather(
        async_fetch_data("url1"),
        async_fetch_data("url2"),
        async_fetch_data("url3"),
    )
    print(f"Got {len(results)} results")  # All 3 results in ~1 second

# To run: asyncio.run(main())
```

### 4. HTTP Basics

Understanding HTTP methods and status codes:

```python
# Example 4: HTTP concepts essential for API development

# HTTP Methods (Verbs)
"""
GET     - Retrieve data (safe, idempotent)
POST    - Create new resource (not idempotent)
PUT     - Update entire resource (idempotent)
PATCH   - Partial update (not always idempotent)
DELETE  - Remove resource (idempotent)
"""

# Common HTTP Status Codes
"""
200 OK                  - Successful GET, PUT, PATCH
201 Created             - Successful POST (resource created)
204 No Content          - Successful DELETE
400 Bad Request         - Invalid request data
401 Unauthorized        - Authentication required
403 Forbidden           - Authenticated but not authorized
404 Not Found           - Resource doesn't exist
422 Unprocessable Entity - Validation error (FastAPI default)
500 Internal Server Error - Server-side error
"""

# Example: HTTP concepts in FastAPI
from fastapi import FastAPI, HTTPException, status

app = FastAPI()

@app.get("/items/{item_id}", status_code=status.HTTP_200_OK)
async def get_item(item_id: int):
    """
    GET request - retrieves data
    Returns 200 OK on success
    """
    if item_id <= 0:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="Item ID must be positive"
        )
    return {"id": item_id, "name": "Sample Item"}

@app.post("/items/", status_code=status.HTTP_201_CREATED)
async def create_item(name: str, price: float):
    """
    POST request - creates new resource
    Returns 201 Created on success
    """
    return {"id": 1, "name": name, "price": price}

@app.delete("/items/{item_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(item_id: int):
    """
    DELETE request - removes resource
    Returns 204 No Content (no response body)
    """
    pass  # In real app, would delete from database
```

## Software Requirements

### 1. Python Installation

#### Windows
```bash
# Download from python.org or use Microsoft Store
# Verify installation
python --version

# Ensure pip is available
pip --version
```

#### macOS
```bash
# Using Homebrew (recommended)
brew install python@3.11

# Or download from python.org
python3 --version
```

#### Linux (Ubuntu/Debian)
```bash
# Update package list
sudo apt update

# Install Python and pip
sudo apt install python3.11 python3.11-venv python3-pip

# Verify installation
python3 --version
```

### 2. Package Manager (pip)

pip comes with Python. Verify it works:

```bash
# Check pip version
pip --version

# Upgrade pip to latest version
python -m pip install --upgrade pip
```

### 3. Virtual Environment

Virtual environments isolate project dependencies:

```bash
# Create virtual environment
python -m venv fastapi_env

# Activate virtual environment
# Windows:
fastapi_env\Scripts\activate

# macOS/Linux:
source fastapi_env/bin/activate

# Verify you're in the virtual environment
which python  # Should point to fastapi_env

# Deactivate when done
deactivate
```

### 4. Code Editor

Recommended editors with Python/FastAPI support:

#### VS Code (Recommended)
Essential extensions:
- Python (Microsoft)
- Pylance (Microsoft)
- REST Client or Thunder Client (for API testing)

#### PyCharm
- Professional or Community edition
- Built-in FastAPI support

#### Other Options
- Vim/Neovim with Python plugins
- Sublime Text with Python packages

## Recommended Knowledge

### 1. JSON
JavaScript Object Notation - data format for APIs:

```python
# Example 5: JSON in Python - used constantly with APIs
import json

# Python dictionary
user = {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "active": True,
    "roles": ["admin", "user"]
}

# Convert to JSON string
json_string = json.dumps(user, indent=2)
print(json_string)

# Parse JSON string back to Python
parsed_user = json.loads(json_string)
print(parsed_user["name"])  # "John Doe"

# FastAPI handles JSON conversion automatically
from fastapi import FastAPI
app = FastAPI()

@app.get("/user")
async def get_user():
    # FastAPI automatically converts this dict to JSON
    return user
```

### 2. Basic Database Concepts

While not required initially, understanding databases helps:

```python
# Example 6: Database concepts (SQLAlchemy preview)
from sqlalchemy import Column, Integer, String, Float
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

class Item(Base):
    """
    Database model - defines table structure.
    Similar concepts appear in Django ORM, Peewee, etc.
    """
    __tablename__ = "items"

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)
    price = Column(Float)
    description = Column(String, nullable=True)

# FastAPI works with any database:
# - SQL databases (PostgreSQL, MySQL, SQLite)
# - NoSQL databases (MongoDB, Redis)
# - ORMs (SQLAlchemy, Tortoise, SQLModel)
```

### 3. API Design Principles

Understanding REST API conventions:

```python
# Example 7: REST API conventions
"""
REST API Endpoint Patterns:

GET    /items          - List all items
GET    /items/{id}     - Get specific item
POST   /items          - Create new item
PUT    /items/{id}     - Update entire item
PATCH  /items/{id}     - Partial update
DELETE /items/{id}     - Delete item

Nested Resources:
GET    /users/{id}/items    - Get items for a user
POST   /users/{id}/items    - Create item for a user

Query Parameters:
GET    /items?limit=10&offset=0      - Pagination
GET    /items?sort=price&order=asc   - Sorting
GET    /items?category=electronics   - Filtering
"""

from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

@app.get("/items")
async def list_items(
    limit: int = Query(10, ge=1, le=100),
    offset: int = Query(0, ge=0),
    category: Optional[str] = None,
    sort: str = Query("id", regex="^(id|name|price)$")
):
    """
    Demonstrates REST conventions:
    - Pagination with limit/offset
    - Filtering with query parameters
    - Sorting options
    """
    return {
        "items": [],
        "limit": limit,
        "offset": offset,
        "category": category,
        "sort": sort
    }
```

## Development Tools

### 1. Testing Tools

```bash
# Install testing dependencies
pip install pytest httpx

# Basic test structure
# test_main.py
import pytest
from httpx import AsyncClient
from main import app

@pytest.mark.asyncio
async def test_read_root():
    async with AsyncClient(app=app, base_url="http://test") as ac:
        response = await ac.get("/")
    assert response.status_code == 200
    assert response.json() == {"message": "Welcome to FastAPI!"}
```

### 2. API Testing Tools

Options for testing your APIs:
- **FastAPI Swagger UI** (built-in): http://localhost:8000/docs
- **Postman**: GUI-based API testing
- **curl**: Command-line HTTP client
- **HTTPie**: User-friendly command-line HTTP client

```bash
# Example: Testing with curl
# GET request
curl http://localhost:8000/items/1

# POST request with JSON
curl -X POST http://localhost:8000/items/ \
  -H "Content-Type: application/json" \
  -d '{"name": "Laptop", "price": 999.99}'
```

## Checklist Before Starting

- [ ] Python 3.9+ installed and working
- [ ] pip available and up to date
- [ ] Code editor configured with Python support
- [ ] Understanding of Python basics (functions, classes, decorators)
- [ ] Familiarity with type hints
- [ ] Basic understanding of HTTP methods
- [ ] Comfort with command line/terminal

## Common Issues

### Python Not Found
```bash
# If 'python' command not found, try:
python3 --version

# Or add Python to PATH (Windows)
# Check "Add Python to PATH" during installation
```

### Permission Errors
```bash
# Use --user flag if permission denied
pip install --user fastapi

# Or use virtual environments (recommended)
python -m venv venv
source venv/bin/activate  # Linux/Mac
venv\Scripts\activate     # Windows
```

## Next Steps

Once prerequisites are met:
- [Installation Guide](./02_installation_guide.md) - Install FastAPI and dependencies
- [Development Environment](./03_development_environment.md) - Set up your workspace
