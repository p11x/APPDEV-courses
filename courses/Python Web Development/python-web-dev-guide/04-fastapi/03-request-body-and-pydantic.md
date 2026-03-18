# Request Body and Pydantic

## What You'll Learn
- Creating request bodies with Pydantic models
- Validating incoming data
- Nested models
- Field validation
- Response models
- Handling errors

## Prerequisites
- Completed FastAPI Path and Query Parameters

## Request Bodies

**Request bodies** are data sent from the client to the API (usually in POST/PUT requests). FastAPI uses Pydantic for automatic validation.

### Basic Request Body

```python
from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

# Define the expected data structure
class Item(BaseModel):
    name: str
    description: str | None = None
    price: float
    tax: float | None = None

@app.post("/items/")
def create_item(item: Item) -> dict[str, Any]:
    """Create an item with automatic validation."""
    total_price = item.price + (item.tax if item.tax else 0)
    return {
        "name": item.name,
        "price": item.price,
        "tax": item.tax,
        "total": total_price
    }
```

🔍 **Pydantic Model Breakdown:**

1. `class Item(BaseModel):` — Inherits from Pydantic's BaseModel
2. `name: str` — Required string field
3. `description: str | None = None` — Optional field with default None
4. FastAPI automatically validates the JSON body against this model

## Field Validation

Pydantic provides extensive validation options:

```python
from pydantic import BaseModel, Field
from typing import Optional
from datetime import datetime

class User(BaseModel):
    """User model with field validation."""
    username: str = Field(
        ...,  # Required (no default)
        min_length=3,
        max_length=50,
        description="The user's username"
    )
    
    email: str = Field(
        ...,
        pattern=r"^[\w\.-]+@[\w\.-]+\.\w+$",
        description="User's email address"
    )
    
    age: int = Field(
        ge=0,
        le=150,
        description="User's age"
    )
    
    is_active: bool = Field(default=True)
    
    created_at: datetime = Field(default_factory=datetime.now)
```

🔍 **Field Options:**

1. `Field(..., min_length=3)` — Minimum string length (ellipsis means required)
2. `Field(..., pattern=r"regex")` — Regex pattern
3. `Field(..., ge=0)` — Greater than or equal (for numbers)
4. `Field(..., le=150)` — Less than or equal (for numbers)
5. `Field(default=True)` — Default value
6. `Field(default_factory=datetime.now)` — Callable for default

## Nested Models

Models can contain other models:

```python
from pydantic import BaseModel
from typing import Optional, List

class Address(BaseModel):
    """Address model."""
    street: str
    city: str
    country: str
    zip_code: str

class User(BaseModel):
    """User with nested address."""
    name: str
    email: str
    address: Address
    phone_numbers: List[str] = []

class Company(BaseModel):
    """Company with list of users."""
    name: str
    users: List[User] = []

# POST with nested models
@app.post("/companies/")
def create_company(company: Company) -> dict:
    return {"company": company.model_dump()}
```

Try sending:
```json
{
    "name": "Tech Corp",
    "users": [
        {
            "name": "Alice",
            "email": "alice@example.com",
            "address": {
                "street": "123 Main St",
                "city": "NYC",
                "country": "USA",
                "zip_code": "10001"
            },
            "phone_numbers": ["555-1234"]
        }
    ]
}
```

## Response Models

Specify what the API returns using `response_model`:

```python
from pydantic import BaseModel
from typing import List, Optional

class Item(BaseModel):
    name: str
    price: float

# Response model - only these fields are returned
@app.post("/items/", response_model=Item)
def create_item(item: Item) -> dict:
    # Even if we return more data, only 'name' and 'price' are returned
    return {
        "name": item.name,
        "price": item.price,
        "internal_id": 12345,  # This is NOT returned
        "created_at": "2024-01-01"  # This is NOT returned
    }

# Return a list with response model
@app.get("/items/", response_model=List[Item])
def read_items() -> list:
    return [
        {"name": "Apple", "price": 1.00, "extra": "data"},
        {"name": "Banana", "price": 0.50, "extra": "data"}
    ]
```

## Error Handling

FastAPI automatically returns validation errors:

```python
from fastapi import FastAPI, HTTPException, Request
from pydantic import BaseModel, Field

app = FastAPI()

class Item(BaseModel):
    name: str = Field(min_length=1)
    price: float = Field(gt=0)

items_db = {}

@app.post("/items/{item_id}")
def update_item(item_id: int, item: Item) -> dict:
    """Update item with custom error handling."""
    if item_id not in items_db:
        raise HTTPException(
            status_code=404,
            detail=f"Item {item_id} not found"
        )
    
    items_db[item_id] = item.model_dump()
    return items_db[item_id]

@app.exception_handler(Exception)
async def custom_exception_handler(request: Request, exc: Exception):
    """Custom exception handler."""
    return {"error": str(exc)}
```

## Working with Form Data

For HTML form submissions (not JSON):

```python
from fastapi import FastAPI, Form
from typing import Annotated

app = FastAPI()

@app.post("/login/")
def login(
    username: Annotated[str, Form()],
    password: Annotated[str, Form()]
) -> dict[str, str]:
    return {"username": username, "message": "Login successful!"}
```

## Working with Files

Upload files:

```python
from fastapi import FastAPI, UploadFile, File
from typing import Annotated

app = FastAPI()

@app.post("/upload/")
async def upload_file(
    file: Annotated[UploadFile, File(description="A file to upload")]
) -> dict[str, str]:
    contents = await file.read()
    return {
        "filename": file.filename,
        "size": len(contents)
    }

@app.post("/upload-multiple/")
async def upload_multiple_files(
    files: Annotated[list[UploadFile], File(description="Multiple files")]
) -> dict[str, Any]:
    results = []
    for file in files:
        contents = await file.read()
        results.append({
            "filename": file.filename,
            "size": len(contents)
        })
    return {"files": results}
```

## Complete Example: Blog API

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field, EmailStr
from typing import Optional, List
from datetime import datetime
from uuid import uuid4

app = FastAPI()

# Models
class Author(BaseModel):
    name: str = Field(min_length=1, max_length=100)
    email: EmailStr
    bio: Optional[str] = Field(None, max_length=500)

class Post(BaseModel):
    title: str = Field(min_length=1, max_length=200)
    content: str = Field(min_length=10)
    author: Author
    tags: List[str] = []
    published: bool = False
    created_at: datetime = Field(default_factory=datetime.now)

class PostResponse(BaseModel):
    id: str
    title: str
    author_name: str
    tags: List[str]
    published: bool
    created_at: datetime

# In-memory database
posts_db: dict[str, dict] = {}

# Create post
@app.post("/posts", response_model=PostResponse, status_code=201)
def create_post(post: Post) -> PostResponse:
    """Create a new blog post."""
    post_id = str(uuid4())
    posts_db[post_id] = {
        "id": post_id,
        "title": post.title,
        "content": post.content,
        "author_name": post.author.name,
        "author_email": post.author.email,
        "tags": post.tags,
        "published": post.published,
        "created_at": post.created_at
    }
    return PostResponse(
        id=post_id,
        title=post.title,
        author_name=post.author.name,
        tags=post.tags,
        published=post.published,
        created_at=post.created_at
    )

# Get all posts
@app.get("/posts", response_model=List[PostResponse])
def get_posts(published_only: bool = False) -> List[PostResponse]:
    """Get all posts, optionally filtering by published status."""
    posts = []
    for post_id, post_data in posts_db.items():
        if published_only and not post_data["published"]:
            continue
        posts.append(PostResponse(**post_data))
    return posts

# Get single post
@app.get("/posts/{post_id}", response_model=PostResponse)
def get_post(post_id: str) -> PostResponse:
    """Get a single post by ID."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    return PostResponse(**posts_db[post_id])

# Update post
@app.put("/posts/{post_id}", response_model=PostResponse)
def update_post(post_id: str, post: Post) -> PostResponse:
    """Update an existing post."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    posts_db[post_id].update({
        "title": post.title,
        "content": post.content,
        "author_name": post.author.name,
        "author_email": post.author.email,
        "tags": post.tags,
        "published": post.published
    })
    return PostResponse(**posts_db[post_id])

# Delete post
@app.delete("/posts/{post_id}")
def delete_post(post_id: str) -> dict[str, str]:
    """Delete a post."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    del posts_db[post_id]
    return {"message": "Post deleted"}
```

## Summary
- Use **Pydantic models** for request body validation
- Use `Field()` for field-level validation (min_length, pattern, ge)
- Use **nested models** for complex data structures
- Use **response_model** to control what gets returned
- FastAPI automatically generates validation error responses
- Use `Form()` for form data, `UploadFile` for file uploads

## Next Steps
→ Continue to `04-dependency-injection.md` to learn about FastAPI's powerful dependency injection system.
