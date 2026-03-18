# HATEOAS and Rich Responses

## What You'll Learn
- Hypermedia controls
- Related resources
- Embedded relationships

## Prerequisites
- Completed pagination

## HATEOAS (Hypermedia)

```python
from fastapi import FastAPI
from pydantic import BaseModel
from typing import List, Optional

app = FastAPI()

class User(BaseModel):
    id: int
    name: str
    email: str

class UserResponse(BaseModel):
    data: User
    links: dict

@app.get("/users/{user_id}", response_model=UserResponse)
async def get_user(user_id: int):
    """Get user with HATEOAS links"""
    user = User(id=user_id, name="John", email="john@example.com")
    
    return {
        "data": user,
        "links": {
            "self": f"/api/users/{user_id}",
            "posts": f"/api/users/{user_id}/posts",
            "profile": f"/api/users/{user_id}/profile"
        }
    }

@app.get("/users/{user_id}/posts")
async def get_user_posts(user_id: int):
    """Get user's posts"""
    posts = [
        {"id": 1, "title": "First Post"},
        {"id": 2, "title": "Second Post"}
    ]
    
    return {
        "data": posts,
        "links": {
            "user": f"/api/users/{user_id}"
        }
    }
```

## Embedded Relationships

```python
class PostWithAuthor(BaseModel):
    id: int
    title: str
    author: User  # Embedded

@app.get("/posts/{post_id}")
async def get_post(post_id: int):
    """Get post with embedded author"""
    return {
        "id": post_id,
        "title": "Hello World",
        "content": "This is a post",
        "author": {
            "id": 1,
            "name": "John",
            "links": {"self": "/api/users/1"}
        },
        "links": {
            "self": f"/api/posts/{post_id}",
            "author": "/api/users/1"
        }
    }
```

## Summary
- Include related resource links
- Use embedded relationships for efficiency
- Add self links for discoverability

## Next Steps
→ Continue to `06-api-documentation.md`
