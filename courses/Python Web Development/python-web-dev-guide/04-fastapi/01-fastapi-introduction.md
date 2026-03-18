# FastAPI Introduction

## What You'll Learn
- What FastAPI is and why it's popular
- Installing FastAPI
- Creating your first FastAPI application
- Understanding automatic documentation
- Comparing FastAPI to Flask

## Prerequisites
- Completed Python fundamentals
- Understanding of HTTP requests and APIs

## What Is FastAPI?

**FastAPI** is a modern, fast (high-performance) Python web framework for building APIs. Released in 2018, it's become incredibly popular for several reasons:

1. **Fast** — One of the fastest Python frameworks available
2. **Automatic documentation** — Generates interactive API docs automatically
3. **Type safety** — Built on Pydantic for data validation
4. **Async support** — Native support for asynchronous programming

Think of FastAPI as Flask's younger, faster sibling:
- Similar simplicity in getting started
- Built-in validation and documentation
- Better performance for I/O-heavy operations

## Installing FastAPI

```bash
pip install fastapi        # The framework
pip install uvicorn        # ASGI server to run the app
```

What just happened:
- `fastapi` — The main framework
- `uvicorn` — The server that runs FastAPI apps (like Flask's built-in server but faster)

## Your First FastAPI Application

Create a file called `main.py`:

```python
from fastapi import FastAPI

app = FastAPI()

@app.get("/")
def read_root() -> dict[str, str]:
    return {"message": "Hello, World!", "app": "FastAPI"}

@app.get("/about")
def about() -> dict[str, str]:
    return {"about": "This is my first FastAPI app!"}
```

Run the server:

```bash
uvicorn main:app --reload
```

You'll see:
```
INFO:     Uvicorn running on http://127.0.0.1:8000
```

Open your browser to `http://127.0.0.1:8000`

🔍 **Line-by-Line Breakdown:**

1. `from fastapi import FastAPI` — Imports the FastAPI class
2. `app = FastAPI()` — Creates a FastAPI application instance
3. `@app.get("/")` — A decorator that registers the function as a handler for GET requests to "/"
4. `def read_root() -> dict[str, str]:` — An async function returning a dictionary (automatically converted to JSON)
5. The return type `dict[str, str]` tells FastAPI this returns a dictionary with string keys and values

## Automatic Documentation

One of FastAPI's best features is automatic interactive documentation.

Visit these URLs when your app is running:

- **Swagger UI**: `http://127.0.0.1:8000/docs`
- **ReDoc**: `http://127.0.0.1:8000/redoc`

These provide:
- Interactive API explorer
- Ability to test endpoints directly
- Auto-generated request/response schemas

## FastAPI vs Flask

| Feature | Flask | FastAPI |
|---------|-------|---------|
| Learning curve | Low | Low |
| Performance | Good | Excellent |
| Auto documentation | No | Yes |
| Type validation | Manual | Built-in (Pydantic) |
| Async support | Extension-based | Native |
| Data validation | Manual | Automatic |
| Best for | HTML websites | REST APIs |

## API Basics with FastAPI

### GET Requests (Reading Data)

```python
from fastapi import FastAPI
from typing import Any

app = FastAPI()

# Simple GET endpoint
@app.get("/")
def root() -> dict[str, str]:
    return {"message": "Welcome to the API"}

# GET with response model
@app.get("/items/{item_id}")
def read_item(item_id: int, q: str | None = None) -> dict[str, Any]:
    """Get an item by ID with optional query parameter."""
    return {
        "item_id": item_id,
        "q": q
    }
```

### POST Requests (Creating Data)

```python
from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

# Define data model
class Item(BaseModel):
    name: str
    description: str | None = None
    price: float
    tax: float | None = None

# POST endpoint
@app.post("/items/")
def create_item(item: Item) -> dict[str, Any]:
    """Create a new item."""
    return {
        "name": item.name,
        "price": item.price,
        "with_tax": item.price * 1.1 if item.tax is None else item.price * (1 + item.tax)
    }
```

🔍 **Pydantic Model Breakdown:**

1. `class Item(BaseModel):` — Inherits from Pydantic's BaseModel for automatic validation
2. `name: str` — Required string field
3. `description: str | None = None` — Optional string with default None
4. `price: float` — Required float field
5. FastAPI automatically validates incoming JSON against this model

## Query Parameters

Query parameters appear after the `?` in the URL:

```python
from fastapi import FastAPI

app = FastAPI()

fake_items_db = [
    {"item_name": "Apple"},
    {"item_name": "Banana"},
    {"item_name": "Cherry"}
]

@app.get("/items")
def read_items(skip: int = 0, limit: int = 10) -> list[dict[str, str]]:
    """Get items with pagination."""
    return fake_items_db[skip:skip + limit]

@app.get("/users/{user_id}/items")
def read_user_items(
    user_id: int,
    q: str | None = None,
    short: bool = False
) -> dict[str, Any]:
    """Get items for a user with optional filters."""
    items = [{"item_id": 1, "name": "Apple"}, {"item_id": 2, "name": "Banana"}]
    if q:
        items = [i for i in items if q.lower() in i["name"].lower()]
    if not short:
        for item in items:
            item["description"] = "A delicious fruit"
    return {"user_id": user_id, "items": items}
```

## Path Parameters

Path parameters are part of the URL path:

```python
from fastapi import FastAPI, HTTPException

app = FastAPI()

items_db = {
    1: {"name": "Apple", "price": 1.00},
    2: {"name": "Banana", "price": 0.50},
    3: {"name": "Cherry", "price": 2.00}
}

@app.get("/items/{item_id}")
def read_item(item_id: int) -> dict:
    """Get item by ID."""
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")
    return {"item_id": item_id, **items_db[item_id]}

@app.put("/items/{item_id}")
def update_item(item_id: int, item: dict) -> dict:
    """Update an item."""
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")
    items_db[item_id] = {"name": item.get("name", items_db[item_id]["name"])}
    return {"item_id": item_id, **items_db[item_id]}
```

## Running FastAPI

### Using Uvicorn

```bash
# Run with uvicorn
uvicorn main:app

# With auto-reload (development)
uvicorn main:app --reload

# On specific host and port
uvicorn main:app --host 0.0.0.0 --port 8000
```

### Using Python

```python
# main.py
if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="127.0.0.1", port=8000)
```

Then run:
```bash
python main.py
```

## Complete Example: Items API

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from typing import Optional

app = FastAPI(
    title="Items API",
    description="A simple API for managing items",
    version="1.0.0"
)

# In-memory database
items: dict[int, dict] = {}
next_id: int = 1

# Pydantic model for request/response
class Item(BaseModel):
    name: str = Field(..., min_length=1, max_length=100)
    description: Optional[str] = Field(None, max_length=500)
    price: float = Field(..., gt=0)
    tax: Optional[float] = Field(None, ge=0)

# Response model (what's returned to client)
class ItemResponse(Item):
    id: int

# GET all items
@app.get("/items", response_model=list[ItemResponse])
def read_items() -> list[ItemResponse]:
    """Get all items."""
    return [ItemResponse(id=id, **item) for id, item in items.items()]

# GET single item
@app.get("/items/{item_id}", response_model=ItemResponse)
def read_item(item_id: int) -> ItemResponse:
    """Get a single item by ID."""
    if item_id not in items:
        raise HTTPException(status_code=404, detail="Item not found")
    return ItemResponse(id=item_id, **items[item_id])

# POST create item
@app.post("/items", response_model=ItemResponse, status_code=201)
def create_item(item: Item) -> ItemResponse:
    """Create a new item."""
    global next_id
    items[next_id] = item.model_dump()
    result = ItemResponse(id=next_id, **items[next_id])
    next_id += 1
    return result

# PUT update item
@app.put("/items/{item_id}", response_model=ItemResponse)
def update_item(item_id: int, item: Item) -> ItemResponse:
    """Update an existing item."""
    if item_id not in items:
        raise HTTPException(status_code=404, detail="Item not found")
    items[item_id] = item.model_dump()
    return ItemResponse(id=item_id, **items[item_id])

# DELETE item
@app.delete("/items/{item_id}")
def delete_item(item_id: int) -> dict[str, str]:
    """Delete an item."""
    if item_id not in items:
        raise HTTPException(status_code=404, detail="Item not found")
    del items[item_id]
    return {"message": "Item deleted"}
```

Try it out:
```bash
uvicorn main:app --reload
```

Then visit `http://127.0.0.1:8000/docs` to see the interactive documentation!

## Summary
- **FastAPI** is a modern Python framework for building APIs
- Install with `pip install fastapi uvicorn`
- Use **Pydantic models** for data validation
- FastAPI provides **automatic documentation** at `/docs`
- Use **path parameters** for URL segments, **query parameters** for `?key=value`
- Return **Pydantic models** or dictionaries for automatic JSON conversion

## Next Steps
→ Continue to `02-path-and-query-parameters.md` to learn more about working with parameters.
