# SQLAlchemy ORM

## What You'll Learn
- What an ORM is and why use one
- Setting up SQLAlchemy
- Creating models
- Querying data
- Relationships between tables

## Prerequisites
- Completed SQL Basics

## What Is an ORM?

**ORM (Object-Relational Mapping)** lets you work with databases using Python objects instead of SQL. Instead of writing:

```sql
SELECT * FROM users WHERE id = 1
```

You write:

```python
user = session.query(User).get(1)
```

## Setting Up SQLAlchemy

```bash
pip install sqlalchemy
```

### Basic Setup

```python
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, declarative_base

# Create engine
engine = create_engine("sqlite:///blog.db", echo=True)

# Create session factory
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Create base class for models
Base = declarative_base()

# Create all tables
Base.metadata.create_all(bind=engine)
```

## Creating Models

```python
from sqlalchemy import Column, Integer, String, Text, DateTime, ForeignKey
from sqlalchemy.orm import relationship
from datetime import datetime
from base import Base

class User(Base):
    """User model."""
    __tablename__ = "users"
    
    id = Column(Integer, primary_key=True, index=True)
    username = Column(String(80), unique=True, nullable=False)
    email = Column(String(120), unique=True, nullable=False)
    password_hash = Column(String(256), nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow)
    
    # Relationship to posts
    posts = relationship("Post", back_populates="author")

class Post(Base):
    """Post model."""
    __tablename__ = "posts"
    
    id = Column(Integer, primary_key=True, index=True)
    title = Column(String(200), nullable=False)
    content = Column(Text, nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow)
    user_id = Column(Integer, ForeignKey("users.id"))
    
    # Relationship to user
    author = relationship("User", back_populates="posts")
```

## CRUD Operations

### Create

```python
from sqlalchemy.orm import Session

def create_user(db: Session, username: str, email: str, password: str) -> User:
    """Create a new user."""
    user = User(username=username, email=email, password_hash=password)
    db.add(user)
    db.commit()
    db.refresh(user)
    return user

def create_post(db: Session, title: str, content: str, user_id: int) -> Post:
    """Create a new post."""
    post = Post(title=title, content=content, user_id=user_id)
    db.add(post)
    db.commit()
    db.refresh(post)
    return post
```

### Read

```python
def get_user(db: Session, user_id: int) -> User | None:
    """Get user by ID."""
    return db.query(User).filter(User.id == user_id).first()

def get_user_by_email(db: Session, email: str) -> User | None:
    """Get user by email."""
    return db.query(User).filter(User.email == email).first()

def get_all_users(db: Session) -> list[User]:
    """Get all users."""
    return db.query(User).all()

def get_posts_by_user(db: Session, user_id: int) -> list[Post]:
    """Get all posts by a user."""
    return db.query(Post).filter(Post.user_id == user_id).all()

# Advanced queries
def search_posts(db: Session, query: str) -> list[Post]:
    """Search posts by title or content."""
    return db.query(Post).filter(
        (Post.title.contains(query)) | (Post.content.contains(query))
    ).all()

def get_paginated_posts(db: Session, skip: int = 0, limit: int = 10) -> list[Post]:
    """Get posts with pagination."""
    return db.query(Post).offset(skip).limit(limit).all()
```

### Update

```python
def update_user(db: Session, user_id: int, **kwargs) -> User | None:
    """Update user fields."""
    user = db.query(User).filter(User.id == user_id).first()
    if user:
        for key, value in kwargs.items():
            setattr(user, key, value)
        db.commit()
        db.refresh(user)
    return user

def update_post(db: Session, post_id: int, title: str = None, content: str = None) -> Post | None:
    """Update post."""
    post = db.query(Post).filter(Post.id == post_id).first()
    if post:
        if title:
            post.title = title
        if content:
            post.content = content
        db.commit()
        db.refresh(post)
    return post
```

### Delete

```python
def delete_user(db: Session, user_id: int) -> bool:
    """Delete a user."""
    user = db.query(User).filter(User.id == user_id).first()
    if user:
        db.delete(user)
        db.commit()
        return True
    return False

def delete_post(db: Session, post_id: int) -> bool:
    """Delete a post."""
    post = db.query(Post).filter(Post.id == post_id).first()
    if post:
        db.delete(post)
        db.commit()
        return True
    return False
```

## Using with FastAPI

```python
from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session

app = FastAPI()

# Dependency to get database session
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

@app.post("/users/", response_model=UserResponse)
def create_user(user: UserCreate, db: Session = Depends(get_db)):
    """Create a new user."""
    db_user = get_user_by_email(db, email=user.email)
    if db_user:
        raise HTTPException(status_code=400, detail="Email already registered")
    return create_user(db, user.username, user.email, user.password)

@app.get("/users/", response_model=list[UserResponse])
def read_users(skip: int = 0, limit: int = 10, db: Session = Depends(get_db)):
    """Get users with pagination."""
    users = db.query(User).offset(skip).limit(limit).all()
    return users
```

## Summary
- **ORM** maps Python objects to database tables
- **Models** are Python classes defining tables
- Use **session.query()** to query data
- Use **relationship()** for table relationships
- Use **ForeignKey** to link tables

## Next Steps
→ Continue to `03-alembic-migrations.md` to learn database migrations.
