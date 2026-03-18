<!-- FILE: 11_async_flask_and_background_tasks/02_async_views/03_async_database_queries.md -->

## Overview

This file teaches you how to perform asynchronous database operations in Flask using SQLAlchemy's async support. You'll learn about async drivers, how to define async models, and how to write queries that don't block the event loop.

## Prerequisites

- Flask 3.x with async support
- SQLAlchemy 2.0+ (for async support)
- A database (SQLite for testing, PostgreSQL/MySQL for production)
- Understanding of Flask-SQLAlchemy basics

## Core Concepts

### Why Async Databases?

Even though databases are I/O operations (waiting for disk/network), using synchronous database drivers in async Flask routes blocks the event loop. This defeats the purpose of using async.

The solution is to use async database drivers and SQLAlchemy's async mode:

| Database | Sync Driver | Async Driver |
|----------|-------------|--------------|
| PostgreSQL | `psycopg2` | `asyncpg` |
| MySQL | `pymysql` or `mysqlclient` | `aiomysql` |
| SQLite | Built-in (sync) | `aiosqlite` |

### SQLAlchemy Async Mode

SQLAlchemy 2.0 introduced full async support. The key components are:

1. `create_async_engine()` - Creates an async database engine
2. `AsyncSession` - Async version of SQLAlchemy Session
3. `async_sessionmaker` - Factory for creating async sessions
4. `async_scoped_session` - For request-scoped async sessions

### How Async SQLAlchemy Differs

```python
# SYNC (what you might already use)
from sqlalchemy.orm import Session

session = Session(engine)
user = session.query(User).filter_by(id=1).first()
session.close()

# ASYNC (non-blocking)
from sqlalchemy.ext.asyncio import AsyncSession

async with async_sessionmaker(engine)() as session:
    result = await session.execute(select(User).where(User.id == 1))
    user = result.scalar_one_or_none()
```

## Code Walkthrough

### Step 1: Install Dependencies

```bash
# Core dependencies
pip install Flask>=3.0
pip install SQLAlchemy>=2.0
pip install Flask-SQLAlchemy>=3.0

# Async driver for SQLite (easiest for testing)
pip install aiosqlite

# For PostgreSQL (production)
pip install asyncpg

# For MySQL (production)
pip install aiomysql
```

### Step 2: Set Up Async Flask with SQLAlchemy

```python
# app.py - Complete async Flask app with SQLAlchemy
import asyncio
from flask import Flask, jsonify
from sqlalchemy import select
from sqlalchemy.orm import DeclarativeBase, Mapped, mapped_column
from sqlalchemy import Integer, String, DateTime
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession, async_sessionmaker
from datetime import datetime

# ============================================
# Base class for models
# ============================================
class Base(DeclarativeBase):
    pass

# ============================================
# Define your models
# ============================================
class User(Base):
    __tablename__ = "users"
    
    # Column definitions
    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    username: Mapped[str] = mapped_column(String(80), unique=True, nullable=False)
    email: Mapped[str] = mapped_column(String(120), nullable=False)
    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    
    def to_dict(self):
        return {
            "id": self.id,
            "username": self.username,
            "email": self.email,
            "created_at": self.created_at.isoformat() if self.created_at else None
        }

class Post(Base):
    __tablename__ = "posts"
    
    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    title: Mapped[str] = mapped_column(String(200), nullable=False)
    content: Mapped[str] = mapped_column(String, nullable=False)
    user_id: Mapped[int] = mapped_column(Integer, nullable=False)
    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    
    def to_dict(self):
        return {
            "id": self.id,
            "title": self.title,
            "content": self.content,
            "user_id": self.user_id,
            "created_at": self.created_at.isoformat() if self.created_at else None
        }

# ============================================
# Database configuration
# ============================================
# For SQLite (testing) - use aiosqlite
DATABASE_URL = "sqlite+aiosqlite:///./test.db"

# For PostgreSQL (production) - use asyncpg
# DATABASE_URL = "postgresql+asyncpg://user:password@localhost/dbname"

# For MySQL (production) - use aiomysql
# DATABASE_URL = "mysql+aiomysql://user:password@localhost/dbname"

# Create async engine
engine = create_async_engine(
    DATABASE_URL,
    echo=True,  # Print SQL to console (useful for debugging)
    future=True
)

# Create async session factory
async_session = async_sessionmaker(
    engine,
    class_=AsyncSession,
    expire_on_commit=False  # Don't expire objects after commit
)

# ============================================
# Database helper functions
# ============================================
async def init_db():
    """Initialize database tables"""
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)

async def get_session() -> AsyncSession:
    """Get an async database session"""
    async with async_session() as session:
        yield session

# ============================================
# Flask app setup
# ============================================
app = Flask(__name__)

# Add async session to app context
from contextlib import asynccontextmanager

@asynccontextmanager
async def get_db():
    async with async_session() as session:
        try:
            yield session
        finally:
            await session.close()

# Make session available via g
from flask import g

@app.before_request
async def before_request():
    g.db = async_session()

@app.teardown_appcontext
async def teardown_db(exception):
    db = g.pop('db', None)
    if db is not None:
        await db.close()

# ============================================
# Routes - Async database operations
# ============================================

@app.route("/")
async def index():
    return jsonify({"message": "Async Flask API", "version": "1.0"})

# ----------------------------------------
# CREATE: Add a new user
# ----------------------------------------
@app.route("/users", methods=["POST"])
async def create_user():
    """Create a new user"""
    from flask import request
    
    data = request.get_json()
    
    async with async_session() as session:
        # Create new user object
        new_user = User(
            username=data["username"],
            email=data["email"]
        )
        
        # Add and commit
        session.add(new_user)
        await session.commit()
        
        # Refresh to get the ID
        await session.refresh(new_user)
    
    return jsonify(new_user.to_dict()), 201

# ----------------------------------------
# READ: Get all users
# ----------------------------------------
@app.route("/users")
async def list_users():
    """Get all users"""
    async with async_session() as session:
        # Use select() instead of query()
        result = await session.execute(select(User))
        users = result.scalars().all()
    
    return jsonify([user.to_dict() for user in users])

# ----------------------------------------
# READ: Get single user
# ----------------------------------------
@app.route("/users/<int:user_id>")
async def get_user(user_id):
    """Get a single user by ID"""
    async with async_session() as session:
        # Query for user
        result = await session.execute(
            select(User).where(User.id == user_id)
        )
        user = result.scalar_one_or_none()
    
    if user is None:
        return jsonify({"error": "User not found"}), 404
    
    return jsonify(user.to_dict())

# ----------------------------------------
# READ: Get user's posts
# ----------------------------------------
@app.route("/users/<int:user_id>/posts")
async def get_user_posts(user_id):
    """Get all posts for a user"""
    async with async_session() as session:
        result = await session.execute(
            select(Post).where(Post.user_id == user_id)
        )
        posts = result.scalars().all()
    
    return jsonify([post.to_dict() for post in posts])

# ----------------------------------------
# UPDATE: Update a user
# ----------------------------------------
@app.route("/users/<int:user_id>", methods=["PUT"])
async def update_user(user_id):
    """Update a user"""
    from flask import request
    
    data = request.get_json()
    
    async with async_session() as session:
        # Find user
        result = await session.execute(
            select(User).where(User.id == user_id)
        )
        user = result.scalar_one_or_none()
        
        if user is None:
            return jsonify({"error": "User not found"}), 404
        
        # Update fields
        if "username" in data:
            user.username = data["username"]
        if "email" in data:
            user.email = data["email"]
        
        await session.commit()
        await session.refresh(user)
    
    return jsonify(user.to_dict())

# ----------------------------------------
# DELETE: Delete a user
# ----------------------------------------
@app.route("/users/<int:user_id>", methods=["DELETE"])
async def delete_user(user_id):
    """Delete a user"""
    async with async_session() as session:
        # Find user
        result = await session.execute(
            select(User).where(User.id == user_id)
        )
        user = result.scalar_one_or_none()
        
        if user is None:
            return jsonify({"error": "User not found"}), 404
        
        await session.delete(user)
        await session.commit()
    
    return jsonify({"message": "User deleted"}), 200

# ----------------------------------------
# CREATE: Add a post
# ----------------------------------------
@app.route("/posts", methods=["POST"])
async def create_post():
    """Create a new post"""
    from flask import request
    
    data = request.get_json()
    
    async with async_session() as session:
        # Verify user exists
        user_result = await session.execute(
            select(User).where(User.id == data["user_id"])
        )
        user = user_result.scalar_one_or_none()
        
        if user is None:
            return jsonify({"error": "User not found"}), 404
        
        # Create post
        new_post = Post(
            title=data["title"],
            content=data["content"],
            user_id=data["user_id"]
        )
        
        session.add(new_post)
        await session.commit()
        await session.refresh(new_post)
    
    return jsonify(new_post.to_dict()), 201

# ----------------------------------------
# Complex query: Users with their posts
# ----------------------------------------
@app.route("/users-with-posts")
async def users_with_posts():
    """Get all users with their posts (joined loading)"""
    from sqlalchemy.orm import selectinload
    
    async with async_session() as session:
        result = await session.execute(
            select(User).options(selectinload(User.posts))
        )
        users = result.scalars().all()
    
    # Convert to dict with posts
    return jsonify([
        {
            **user.to_dict(),
            "posts": [post.to_dict() for post in user.posts]
        }
        for user in users
    ])

# ============================================
# Initialize and run
# ============================================
if __name__ == "__main__":
    # Note: For production, use hypercorn instead:
    # hypercorn app:app --host 0.0.0.0 --port 5000
    
    # For development with Flask dev server:
    # We'll initialize the database first
    asyncio.run(init_db())
    
    print("Database initialized!")
    print("Run with: hypercorn app:app --reload --port 5000")
    app.run(debug=True, port=5000)
```

### Step 3: Add a posts relationship to User

```python
# Add this to the User class for relationship loading
from sqlalchemy.orm import relationship

class User(Base):
    __tablename__ = "users"
    
    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    username: Mapped[str] = mapped_column(String(80), unique=True, nullable=False)
    email: Mapped[str] = mapped_column(String(120), nullable=False)
    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    
    # Relationship - note: uses "lazy='selectin'" for async compatibility
    posts: Mapped[list["Post"]] = relationship("Post", back_populates="user", lazy="selectin")
```

### Testing the Routes

```bash
# Start the server with hypercorn (recommended for async)
pip install hypercorn
hypercorn app:app --reload --port 5000

# Create a user
curl -X POST http://localhost:5000/users \
  -H "Content-Type: application/json" \
  -d '{"username": "john", "email": "john@example.com"}'

# List all users
curl http://localhost:5000/users

# Get a user
curl http://localhost:5000/users/1

# Create a post
curl -X POST http://localhost:5000/posts \
  -H "Content-Type: application/json" \
  -d '{"title": "Hello World", "content": "My first post!", "user_id": 1}'

# Get user's posts
curl http://localhost:5000/users/1/posts

# Get all users with their posts
curl http://localhost:5000/users-with-posts
```

### Line-by-Line Breakdown

- `create_async_engine(DATABASE_URL, echo=True)` - Creates async engine; `echo=True` shows SQL
- `async_sessionmaker(engine, class_=AsyncSession)` - Creates sessions that can be used with `async with`
- `await session.execute(select(User).where(User.id == user_id))` - Async query
- `result.scalar_one_or_none()` - Gets a single result or None
- `session.add(new_user); await session.commit()` - Async insert
- `await session.delete(user); await session.commit()` - Async delete

## Common Mistakes

### ❌ Using sync SQLAlchemy with async routes

```python
# WRONG: Using regular SQLAlchemy in async route
from sqlalchemy.orm import sessionmaker

Session = sessionmaker(bind=engine)  # Sync session!

@app.route("/broken")
async def broken_route():
    session = Session()  # Sync session - blocks event loop!
    user = session.query(User).first()
    return jsonify({"name": user.username})
```

### ✅ Use async SQLAlchemy

```python
# CORRECT: Using async SQLAlchemy
from sqlalchemy.ext.asyncio import AsyncSession, async_sessionmaker

async_session = async_sessionmaker(engine, class_=AsyncSession)

@app.route("/fixed")
async def fixed_route():
    async with async_session() as session:
        result = await session.execute(select(User))
        user = result.scalar_one()
    return jsonify({"name": user.username})
```

### ❌ Forgetting to await session operations

```python
# WRONG: Forgetting await
async def bad_route():
    async with async_session() as session:
        session.add(user)
        session.commit()  # Forgot await!
    return "ok"
```

### ✅ Always await

```python
# CORRECT: Await everything
async def good_route():
    async with async_session() as session:
        session.add(user)
        await session.commit()  # Properly awaited
    return "ok"
```

### ❌ Not using proper async drivers

```python
# WRONG: Using sync driver URL
DATABASE_URL = "sqlite:///./test.db"  # Missing +aiosqlite!
```

### ✅ Use async driver in URL

```python
# CORRECT: Include async driver
DATABASE_URL = "sqlite+aiosqlite:///./test.db"  # Async driver!
```

## Quick Reference

| Database | Async Driver | URL Format |
|----------|-------------|------------|
| SQLite | aiosqlite | `sqlite+aiosqlite:///db.sqlite` |
| PostgreSQL | asyncpg | `postgresql+asyncpg://user:pass@host/db` |
| MySQL | aiomysql | `mysql+aiomysql://user:pass@host/db` |

**Key operations:**

```python
# Create engine
engine = create_async_engine(DATABASE_URL)

# Create session factory
async_session = async_sessionmaker(engine, class_=AsyncSession)

# Query
result = await session.execute(select(Model).where(Model.id == id))

# Insert
session.add(obj); await session.commit()

# Update
await session.commit()

# Delete
session.delete(obj); await session.commit()
```

## Next Steps

Continue to [03_celery_basics/01_what_is_celery.md](../../11_async_flask_and_background_tasks/03_celery_basics/01_what_is_celery.md) to learn about Celery for background task processing.