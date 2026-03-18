# Building a REST API

## What You'll Learn
- REST API design principles
- Building a complete CRUD API with FastAPI
- Error handling
- Documentation
- Best practices

## Prerequisites
- Completed FastAPI Async/Await section

## What Is REST?

**REST (Representational State Transfer)** is an architectural style for designing APIs. RESTful APIs:

1. Use HTTP methods meaningfully
2. Are stateless (no server-side sessions)
3. Return standard HTTP status codes
4. Use URLs to represent resources

### HTTP Methods

| Method | Purpose | Idempotent? |
|--------|---------|-------------|
| GET | Read data | Yes |
| POST | Create data | No |
| PUT | Replace data | Yes |
| PATCH | Update data | No |
| DELETE | Delete data | Yes |

## Building a Complete REST API

```python
from fastapi import FastAPI, HTTPException, Depends, status
from pydantic import BaseModel, Field, EmailStr
from typing import List, Optional
from datetime import datetime
from uuid import uuid4, UUID

app = FastAPI(
    title="Blog API",
    description="A simple REST API for a blog",
    version="1.0.0"
)

# ============ MODELS ============

class PostBase(BaseModel):
    """Base post model."""
    title: str = Field(..., min_length=1, max_length=200)
    content: str = Field(..., min_length=10)

class PostCreate(PostBase):
    """Model for creating a post."""
    pass

class PostUpdate(BaseModel):
    """Model for updating a post."""
    title: Optional[str] = Field(None, min_length=1, max_length=200)
    content: Optional[str] = Field(None, min_length=10)
    published: Optional[bool] = None

class PostResponse(PostBase):
    """Model for post response."""
    id: UUID
    published: bool
    created_at: datetime
    author_id: UUID
    
    class Config:
        from_attributes = True

class AuthorBase(BaseModel):
    """Base author model."""
    name: str = Field(..., min_length=1, max_length=100)
    email: EmailStr

class AuthorCreate(AuthorBase):
    """Model for creating an author."""
    password: str = Field(..., min_length=8)

class AuthorResponse(AuthorBase):
    """Model for author response."""
    id: UUID
    created_at: datetime
    
    class Config:
        from_attributes = True

# ============ DATABASE (In-Memory) ============

class Database:
    """Simple in-memory database."""
    def __init__(self):
        self.authors: dict[UUID, dict] = {}
        self.posts: dict[UUID, dict] = {}
    
    def create_author(self, author: AuthorCreate) -> dict:
        """Create a new author."""
        author_id = uuid4()
        author_dict = {
            "id": author_id,
            "name": author.name,
            "email": author.email,
            "password": author.password,  # In production, hash this!
            "created_at": datetime.now()
        }
        self.authors[author_id] = author_dict
        return author_dict
    
    def get_author(self, author_id: UUID) -> Optional[dict]:
        return self.authors.get(author_id)
    
    def get_author_by_email(self, email: str) -> Optional[dict]:
        for author in self.authors.values():
            if author["email"] == email:
                return author
        return None
    
    def list_authors(self) -> list[dict]:
        return list(self.authors.values())
    
    def create_post(self, post: PostCreate, author_id: UUID) -> dict:
        """Create a new post."""
        post_id = uuid4()
        post_dict = {
            "id": post_id,
            "title": post.title,
            "content": post.content,
            "published": False,
            "created_at": datetime.now(),
            "author_id": author_id
        }
        self.posts[post_id] = post_dict
        return post_dict
    
    def get_post(self, post_id: UUID) -> Optional[dict]:
        return self.posts.get(post_id)
    
    def list_posts(self, published: Optional[bool] = None) -> list[dict]:
        posts = self.posts.values()
        if published is not None:
            posts = [p for p in posts if p["published"] == published]
        return list(posts)
    
    def update_post(self, post_id: UUID, post_update: PostUpdate) -> Optional[dict]:
        post = self.posts.get(post_id)
        if not post:
            return None
        
        update_data = post_update.model_dump(exclude_unset=True)
        post.update(update_data)
        return post
    
    def delete_post(self, post_id: UUID) -> bool:
        if post_id in self.posts:
            del self.posts[post_id]
            return True
        return False

# Initialize database
db = Database()

# ============ ROUTES ============

# --- Authors ---

@app.post(
    "/authors",
    response_model=AuthorResponse,
    status_code=status.HTTP_201_CREATED,
    summary="Create a new author"
)
def create_author(author: AuthorCreate) -> AuthorResponse:
    """Create a new author."""
    # Check if email exists
    if db.get_author_by_email(author.email):
        raise HTTPException(
            status_code=400,
            detail="Email already registered"
        )
    
    author_dict = db.create_author(author)
    return AuthorResponse(**author_dict)

@app.get(
    "/authors",
    response_model=List[AuthorResponse],
    summary="List all authors"
)
def list_authors() -> List[AuthorResponse]:
    """Get all authors."""
    return [AuthorResponse(**a) for a in db.list_authors()]

@app.get(
    "/authors/{author_id}",
    response_model=AuthorResponse,
    summary="Get an author"
)
def get_author(author_id: UUID) -> AuthorResponse:
    """Get an author by ID."""
    author = db.get_author(author_id)
    if not author:
        raise HTTPException(
            status_code=404,
            detail="Author not found"
        )
    return AuthorResponse(**author)

# --- Posts ---

@app.post(
    "/posts",
    response_model=PostResponse,
    status_code=status.HTTP_201_CREATED,
    summary="Create a new post"
)
def create_post(post: PostCreate, author_id: UUID) -> PostResponse:
    """Create a new post."""
    # Verify author exists
    author = db.get_author(author_id)
    if not author:
        raise HTTPException(
            status_code=404,
            detail="Author not found"
        )
    
    post_dict = db.create_post(post, author_id)
    return PostResponse(**post_dict)

@app.get(
    "/posts",
    response_model=List[PostResponse],
    summary="List all posts"
)
def list_posts(published: Optional[bool] = None) -> List[PostResponse]:
    """Get all posts, optionally filter by published status."""
    posts = db.list_posts(published)
    return [PostResponse(**p) for p in posts]

@app.get(
    "/posts/{post_id}",
    response_model=PostResponse,
    summary="Get a post"
)
def get_post(post_id: UUID) -> PostResponse:
    """Get a post by ID."""
    post = db.get_post(post_id)
    if not post:
        raise HTTPException(
            status_code=404,
            detail="Post not found"
        )
    return PostResponse(**post)

@app.patch(
    "/posts/{post_id}",
    response_model=PostResponse,
    summary="Update a post"
)
def update_post(post_id: UUID, post_update: PostUpdate) -> PostResponse:
    """Update a post (partial update)."""
    post = db.update_post(post_id, post_update)
    if not post:
        raise HTTPException(
            status_code=404,
            detail="Post not found"
        )
    return PostResponse(**post)

@app.delete(
    "/posts/{post_id}",
    status_code=status.HTTP_204_NO_CONTENT,
    summary="Delete a post"
)
def delete_post(post_id: UUID) -> None:
    """Delete a post."""
    if not db.delete_post(post_id):
        raise HTTPException(
            status_code=404,
            detail="Post not found"
        )
    return None

# --- Health Check ---

@app.get(
    "/health",
    summary="Health check"
)
def health_check() -> dict:
    """Check if the API is running."""
    return {
        "status": "healthy",
        "timestamp": datetime.now().isoformat()
    }
```

## API Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /authors | Create author |
| GET | /authors | List authors |
| GET | /authors/{id} | Get author |
| POST | /posts | Create post |
| GET | /posts | List posts |
| GET | /posts/{id} | Get post |
| PATCH | /posts/{id} | Update post |
| DELETE | /posts/{id} | Delete post |
| GET | /health | Health check |

## Testing the API

Run the server:
```bash
uvicorn main:app --reload
```

Then visit `http://127.0.0.1:8000/docs` to see the interactive API documentation.

### Using curl:

```bash
# Create an author
curl -X POST "http://localhost:8000/authors" \
  -H "Content-Type: application/json" \
  -d '{"name": "John Doe", "email": "john@example.com", "password": "password123"}'

# List authors
curl "http://localhost:8000/authors"

# Create a post (use author ID from above)
curl -X POST "http://localhost:8000/posts?author_id=YOUR_AUTHOR_ID" \
  -H "Content-Type: application/json" \
  -d '{"title": "My First Post", "content": "This is the content of my first post!"}'

# List posts
curl "http://localhost:8000/posts"

# Get single post
curl "http://localhost:8000/posts/POST_ID"

# Update post
curl -X PATCH "http://localhost:8000/posts/POST_ID" \
  -H "Content-Type: application/json" \
  -d '{"published": true}'

# Delete post
curl -X DELETE "http://localhost:8000/posts/POST_ID"
```

## REST API Best Practices

1. **Use proper HTTP methods** — GET for reading, POST for creating, etc.
2. **Use appropriate status codes** — 200 OK, 201 Created, 404 Not Found, etc.
3. **Use plural nouns for resources** — `/users` not `/user`
4. **Version your API** — `/api/v1/users`
5. **Document your API** — FastAPI does this automatically!
6. **Use pagination** — Don't return all records at once

## Summary
- REST APIs use HTTP methods meaningfully (GET, POST, PUT, PATCH, DELETE)
- Design resources as plural nouns (/posts, /authors)
- Return appropriate HTTP status codes
- Use Pydantic models for request/response validation
- FastAPI automatically generates interactive documentation

## Next Steps
→ Continue to `../05-databases/01-sql-basics.md` to learn about SQL databases.
