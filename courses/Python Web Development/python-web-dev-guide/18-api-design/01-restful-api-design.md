# RESTful API Design

## What You'll Learn
- REST principles
- URL structure
- HTTP methods
- Status codes

## Prerequisites
- Completed security folder

## REST Principles

REST (Representational State Transfer) uses:
- **Resources**: Nouns (users, products, orders)
- **HTTP Methods**: Verbs (GET, POST, PUT, DELETE)
- **Stateless**: Each request contains all information

## URL Structure

```
GET    /api/v1/users          # List users
GET    /api/v1/users/123      # Get user 123
POST   /api/v1/users          # Create user
PUT    /api/v1/users/123      # Update user
DELETE /api/v1/users/123      # Delete user
```

## HTTP Methods

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List, Optional
from enum import Enum

app = FastAPI()

# In-memory database
users_db: dict = {}
user_id_counter = 1

class User(BaseModel):
    id: Optional[int] = None
    name: str
    email: str

# GET - Retrieve resources
@app.get("/users", response_model=List[User])
async def get_users():
    """List all users"""
    return list(users_db.values())

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Get single user"""
    if user_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")
    return users_db[user_id]

# POST - Create resource
@app.post("/users", response_model=User, status_code=201)
async def create_user(user: User):
    """Create new user"""
    global user_id_counter
    user.id = user_id_counter
    users_db[user_id_counter] = user
    user_id_counter += 1
    return user

# PUT - Update (replace) resource
@app.put("/users/{user_id}", response_model=User)
async def update_user(user_id: int, user: User):
    """Update user"""
    if user_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")
    user.id = user_id
    users_db[user_id] = user
    return user

# DELETE - Remove resource
@app.delete("/users/{user_id}", status_code=204)
async def delete_user(user_id: int):
    """Delete user"""
    if user_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")
    del users_db[user_id]
    return None
```

## Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Error |

## Query Parameters

```python
from typing import Optional

@app.get("/users")
async def get_users(
    skip: int = 0,
    limit: int = 10,
    search: Optional[str] = None
):
    """Get users with pagination and search"""
    users = list(users_db.values())
    
    if search:
        users = [u for u in users if search.lower() in u.name.lower()]
    
    return users[skip:skip + limit]
```

## Summary
- Use nouns for resources
- Use proper HTTP methods
- Return appropriate status codes
- Support pagination

## Next Steps
→ Continue to `02-api-versioning.md`
