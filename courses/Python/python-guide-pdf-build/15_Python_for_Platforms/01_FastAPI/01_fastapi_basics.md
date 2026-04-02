# 🚀 FastAPI Basics: Build a REST API in 30 Lines

## 🎯 What You'll Learn

- Installing and setting up FastAPI
- Creating your first API endpoints
- Path parameters, query parameters
- Automatic documentation

## 📦 Prerequisites

- Python 3.9+
- Basic HTTP knowledge

---

## Installation

```bash
pip install fastapi uvicorn[standard]
```

---

## Your First API

```python
from fastapi import FastAPI

app = FastAPI()  # Create FastAPI app

@app.get("/")  # GET endpoint
def root():
    return {"message": "Hello, World!"}

@app.get("/hello/{name}")
def hello(name: str):
    return {"message": f"Hello, {name}!"}

# Run with: uvicorn main:app --reload
```

### 💡 Line-by-Line Breakdown

```python
from fastapi import FastAPI

app = FastAPI()  # Create the FastAPI application instance

@app.get("/")  # Decorator: this function handles GET requests to "/"
def root():    # The function that handles requests
    return {"message": "Hello, World!"}  # Returns JSON automatically

@app.get("/hello/{name}")  # {name} is a path parameter
def hello(name: str):     # Type hint tells FastAPI to convert/validate
    return {"message": f"Hello, {name}!"}  # Returns dict = JSON response
```

---

## CRUD Example: Books API

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional
from uuid import uuid4

app = FastAPI()

# In-memory database
books_db: dict = {}

class Book(BaseModel):
    title: str
    author: str
    year: Optional[int] = None

# CREATE
@app.post("/books", status_code=201)
def create_book(book: Book):
    book_id = str(uuid4())
    books_db[book_id] = book
    return {"id": book_id, **book.model_dump()}

# READ all
@app.get("/books")
def list_books():
    return {"books": list(books_db.values())}

# READ one
@app.get("/books/{book_id}")
def get_book(book_id: str):
    if book_id not in books_db:
        raise HTTPException(status_code=404, detail="Book not found")
    return books_db[book_id]

# UPDATE
@app.put("/books/{book_id}")
def update_book(book_id: str, book: Book):
    if book_id not in books_db:
        raise HTTPException(status_code=404, detail="Book not found")
    books_db[book_id] = book
    return book

# DELETE
@app.delete("/books/{book_id}", status_code=204)
def delete_book(book_id: str):
    if book_id not in books_db:
        raise HTTPException(status_code=404, detail="Book not found")
    del books_db[book_id]
```

---

## Query Parameters

```python
@app.get("/search")
def search_books(q: str = "", limit: int = 10):
    """Query params: /search?q=python&limit=5"""
    results = [b for b in books_db.values() if q.lower() in b.title.lower()]
    return {"results": results[:limit]}
```

---

## Running the API

```bash
# Development
uvicorn main:app --reload

# Production  
uvicorn main:app --host 0.0.0.0 --port 8000
```

### Auto Documentation

- **Swagger UI**: http://localhost:8000/docs
- **ReDoc**: http://localhost:8000/redoc

---

## ✅ Summary

- FastAPI creates REST APIs with minimal code
- Use `@app.get()`, `@app.post()`, etc. for endpoints
- Path params: `/items/{item_id}`
- Query params: `?skip=0&limit=10`
- Auto-generated docs at `/docs`

## ➡️ Next Steps

Continue to [02_pydantic_models.md](./02_pydantic_models.md)

## 🔗 Further Reading

- [FastAPI Documentation](https://fastapi.tiangolo.com/)
