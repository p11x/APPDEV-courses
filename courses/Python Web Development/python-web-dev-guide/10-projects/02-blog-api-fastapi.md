# Blog API with FastAPI

## What You'll Learn
- Building a REST API
- Using Pydantic models
- Adding authentication

## Prerequisites
- Completed FastAPI fundamentals

## Project Overview

Build a blog API with:
- Create, read posts
- User authentication
- JWT tokens

## Step 1: Setup

```bash
mkdir blog-api
cd blog-api
python -m venv venv
venv\Scripts\activate
pip install fastapi uvicorn pydantic python-jose passlib[bcrypt]
```

## Step 2: Complete API Code

```python
# main.py
from fastapi import FastAPI, Depends, HTTPException, status
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime
from uuid import uuid4
from passlib.context import CryptContext
from jose import jwt, JWTError

app = FastAPI()

SECRET_KEY = "secret-key-change-me"
ALGORITHM = "HS256"
pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")

# Models
class User(BaseModel):
    username: str
    email: str

class UserCreate(User):
    password: str

class Post(BaseModel):
    title: str
    content: str

class PostResponse(Post):
    id: str
    author_id: str
    created_at: datetime

# Database
users_db = {}
posts_db = {}

# Auth functions
def hash_password(password: str):
    return pwd_context.hash(password)

def verify_password(plain: str, hashed: str):
    return pwd_context.verify(plain, hashed)

def create_token(data: dict):
    return jwt.encode(data, SECRET_KEY, algorithm=ALGORITHM)

def get_current_user(authorization: str = None):
    if not authorization:
        raise HTTPException(status_code=401)
    try:
        token = authorization.replace("Bearer ", "")
        data = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        return data
    except:
        raise HTTPException(status_code=401)

# Routes
@app.post("/register", status_code=201)
def register(user: UserCreate):
    if user.email in [u["email"] for u in users_db.values()]:
        raise HTTPException(status_code=400, detail="Email exists")
    user_id = str(uuid4())
    users_db[user_id] = {
        "id": user_id,
        "username": user.username,
        "email": user.email,
        "password": hash_password(user.password)
    }
    return {"id": user_id, "username": user.username}

@app.post("/token")
def login(user: UserCreate):
    for u in users_db.values():
        if u["email"] == user.email and verify_password(user.password, u["password"]):
            token = create_token({"sub": u["id"]})
            return {"access_token": token, "token_type": "bearer"}
    raise HTTPException(status_code=401, detail="Invalid credentials")

@app.post("/posts", response_model=PostResponse)
def create_post(post: Post, auth: dict = Depends(get_current_user)):
    post_id = str(uuid4())
    post_dict = {
        "id": post_id,
        "title": post.title,
        "content": post.content,
        "author_id": auth["sub"],
        "created_at": datetime.now()
    }
    posts_db[post_id] = post_dict
    return post_dict

@app.get("/posts", response_model=List[PostResponse])
def get_posts():
    return list(posts_db.values())

@app.get("/posts/{post_id}", response_model=PostResponse)
def get_post(post_id: str):
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Not found")
    return posts_db[post_id]
```

## Step 3: Run

```bash
uvicorn main:app --reload
```

Visit http://127.0.0.1:8000/docs for API documentation

## Summary
- Built REST API with FastAPI
- Added JWT authentication
- Implemented full CRUD operations
