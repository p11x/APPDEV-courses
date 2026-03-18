# Phase 1 — Project Setup

## Goal

By the end of this phase, you will have a working FastAPI project with:
- Project structure created
- Dependencies installed via pyproject.toml
- Docker Compose with PostgreSQL, pgvector, and Redis
- Basic FastAPI app with /health endpoint
- Configuration management with pydantic-settings

## What You'll Build in This Phase

- [ ] Python project with pyproject.toml
- [ ] Virtual environment with dependencies
- [ ] Docker Compose infrastructure
- [ ] Basic FastAPI app with lifespan events
- [ ] Health check endpoint
- [ ] Configuration via environment variables

## Prerequisites

- Python 3.11+ installed
- Docker and Docker Compose installed
- OpenAI API key (get from platform.openai.com)

## Concepts Introduced

### Async FastAPI with Lifespan

FastAPI 0.109+ uses the lifespan context manager for startup/shutdown logic:

```python
from contextlib import asynccontextmanager

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup: connect to databases
    await connect_db()
    await connect_redis()
    
    yield  # App runs here
    
    # Shutdown: close connections
    await disconnect_db()
    await disconnect_redis()
```

### Pydantic Settings

We use pydantic-settings for type-safe configuration:

```python
from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    model_config = SettingsConfigDict(env_file=".env")
    
    DATABASE_URL: str
    REDIS_URL: str
    OPENAI_API_KEY: str
```

## Step-by-Step Implementation

### Step 1.1 — Create Project Directory and pyproject.toml

Create the project structure:

```python
# pyproject.toml
[project]
name = "pymind"
version = "0.1.0"
description = "AI Knowledge Assistant with RAG"
requires-python = ">=3.11"
dependencies = [
    "fastapi>=0.109.0",
    "uvicorn[standard]>=0.27.0",
    "sqlalchemy[asyncio]>=2.0.0",
    "asyncpg>=0.29.0",
    "alembic>=1.13.0",
    "pydantic>=2.5.0",
    "pydantic-settings>=2.1.0",
    "python-jose[cryptography]>=3.3.0",
    "passlib[bcrypt]>=1.7.4",
    "python-multipart>=0.0.6",
    "httpx>=0.26.0",
    "openai>=1.10.0",
    "redis>=5.0.0",
    "aiofiles>=23.2.0",
    "PyYAML>=6.0.0",
]

[project.optional-dependencies]
dev = [
    "pytest>=7.4.0",
    "pytest-asyncio>=0.23.0",
    "pytest-cov>=4.1.0",
    "ruff>=0.1.0",
    "mypy>=1.8.0",
]

[build-system]
requires = ["hatchling"]
build-backend = "hatchling.build"

[tool.uv]
dev-dependencies = [
    "pytest>=7.4.0",
    "pytest-asyncio>=0.23.0",
]

[tool.pytest.ini_options]
asyncio_mode = "auto"
testpaths = ["tests"]

[tool.ruff]
line-length = 100
target-version = "py311"

[tool.mypy]
python_version = "3.11"
strict = true
```

🔍 **Line-by-Line Breakdown:**

1. `[project]` — Standard Python project metadata following PEP 621
2. `dependencies` — All runtime dependencies pinned (we'll use compatible versions)
3. `[project.optional-dependencies]` — Dev dependencies for testing/linting
4. `[tool.pytest.ini_options]` — Configure pytest for async testing
5. `[tool.ruff]` — Fast linter configuration
6. `[tool.mypy]` — Strict type checking settings

### Step 1.2 — Create Virtual Environment

```bash
# Create virtual environment
python -m venv .venv

# Activate (Linux/Mac)
source .venv/bin/activate

# Activate (Windows)
.venv\Scripts\activate

# Upgrade pip
pip install --upgrade pip
```

### Step 1.3 — Install Dependencies

```bash
pip install -e ".[dev]"
```

Expected output shows fastapi, uvicorn, sqlalchemy, and other packages installing.

### Step 1.4 — Create Folder Structure

```
pymind/
├── app/
│   ├── __init__.py
│   ├── main.py
│   ├── config.py
│   └── dependencies.py
├── alembic/
│   └── env.py
├── tests/
│   └── __init__.py
├── .env.example
├── pyproject.toml
└── docker-compose.yml
```

```bash
mkdir -p app core models schemas routers services utils tests alembic
touch app/__init__.py app/main.py app/config.py app/dependencies.py
touch tests/__init__.py
```

### Step 1.5 — Create Configuration

```python
# app/config.py
"""
Application configuration using pydantic-settings.
Loads from environment variables with type validation.
"""
from functools import lru_cache
from typing import Literal

from pydantic import Field, field_validator
from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    """
    Application settings with environment variable support.
    All sensitive values come from environment, not config files.
    """
    model_config = SettingsConfigDict(
        env_file=".env",
        env_file_encoding="utf-8",
        case_sensitive=True,
        extra="ignore",
    )
    
    # Database
    DATABASE_URL: str = Field(
        description="PostgreSQL connection string with asyncpg driver"
    )
    
    # Redis
    REDIS_URL: str = Field(
        description="Redis connection string"
    )
    
    # OpenAI
    OPENAI_API_KEY: str = Field(
        description="OpenAI API key from platform.openai.com"
    )
    
    # JWT
    JWT_SECRET_KEY: str = Field(
        description="Secret key for JWT signing. Generate with: openssl rand -hex 32"
    )
    JWT_ALGORITHM: str = "HS256"
    ACCESS_TOKEN_EXPIRE_MINUTES: int = 30
    REFRESH_TOKEN_EXPIRE_DAYS: int = 7
    
    # App
    APP_ENV: Literal["development", "staging", "production"] = "development"
    LOG_LEVEL: str = "INFO"
    MAX_UPLOAD_SIZE_MB: int = 10
    CORS_ORIGINS: str = "http://localhost:3000"
    
    @field_validator("CORS_ORIGINS", mode="before")
    @classmethod
    def parse_cors_origins(cls, v: str) -> str:
        """Parse comma-separated origins into list."""
        if isinstance(v, str):
            return v
        return ",".join(v)
    
    @property
    def is_development(self) -> bool:
        """Check if running in development mode."""
        return self.APP_ENV == "development"


@lru_cache
def get_settings() -> Settings:
    """Get cached settings instance."""
    return Settings()
```

🔍 **Line-by-Line Breakdown:**

1. `BaseSettings` — Pydantic class that reads from environment
2. `model_config = SettingsConfigDict(env_file=".env")` — Load from .env file
3. `Field(description=...)` — Add descriptions for docs/validation
4. `@field_validator` — Custom validation for CORS origins
5. `@lru_cache` — Cache settings to avoid re-reading env on every call
6. `property` — Computed properties for common checks

### Step 1.6 — Create FastAPI App Entry Point

```python
# app/main.py
"""
FastAPI application entry point with lifespan management.
Handles startup/shutdown of database and Redis connections.
"""
from contextlib import asynccontextmanager

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse

from app.config import get_settings


@asynccontextmanager
async def lifespan(app: FastAPI):
    """
    Lifespan context manager for startup/shutdown events.
    Connects to databases on startup, disconnects on shutdown.
    """
    settings = get_settings()
    
    # Startup: Import to register event handlers
    from app.core import database, redis as redis_core
    
    # Initialize database connection
    await database.init_db()
    
    # Initialize Redis connection
    await redis_core.init_redis()
    
    print(f"🚀 PyMind starting in {settings.APP_ENV} mode")
    
    yield
    
    # Shutdown: Close connections
    await redis_core.close_redis()
    await database.close_db()
    
    print("🛑 PyMind shutting down")


def create_app() -> FastAPI:
    """Create and configure FastAPI application."""
    settings = get_settings()
    
    app = FastAPI(
        title="PyMind API",
        description="AI Knowledge Assistant with RAG",
        version="0.1.0",
        lifespan=lifespan,
    )
    
    # Configure CORS
    origins = settings.CORS_ORIGINS.split(",")
    app.add_middleware(
        CORSMiddleware,
        allow_origins=origins,
        allow_credentials=True,
        allow_methods=["*"],
        allow_headers=["*"],
    )
    
    # Health check endpoint
    @app.get("/health")
    async def health_check() -> JSONResponse:
        """Basic health check endpoint."""
        return JSONResponse(
            status_code=200,
            content={
                "status": "healthy",
                "environment": settings.APP_ENV,
            }
        )
    
    return app


app = create_app()
```

🔍 **Line-by-Line Breakdown:**

1. `@asynccontextmanager` — Creates async context manager for lifespan
2. `lifespan()` — Connects DB/Redis on startup, disconnects on shutdown
3. `create_app()` — Factory function for creating configured app
4. `CORSMiddleware` — Allows cross-origin requests from frontend
5. `@app.get("/health")` — Simple health check endpoint

### Step 1.7 — Create Core Database Module

```python
# app/core/database.py
"""
Async database configuration and session management.
Uses SQLAlchemy 2.0 async style with PostgreSQL.
"""
from typing import AsyncGenerator

from sqlalchemy.ext.asyncio import AsyncSession, async_sessionmaker, create_async_engine
from sqlalchemy.orm import DeclarativeBase

from app.config import get_settings


class Base(DeclarativeBase):
    """SQLAlchemy declarative base for models."""
    pass


# Global engine and session maker
engine = None
async_session_maker = None


async def init_db() -> None:
    """Initialize database engine and session maker."""
    global engine, async_session_maker
    
    settings = get_settings()
    
    # Create async engine
    engine = create_async_engine(
        settings.DATABASE_URL,
        echo=settings.is_development,
        pool_pre_ping=True,
        pool_size=10,
        max_overflow=20,
    )
    
    # Create session maker
    async_session_maker = async_sessionmaker(
        engine,
        class_=AsyncSession,
        expire_on_commit=False,
    )


async def close_db() -> None:
    """Close database engine."""
    global engine
    
    if engine:
        await engine.dispose()


async def get_session() -> AsyncGenerator[AsyncSession, None]:
    """
    Dependency that provides an async database session.
    Yields session, automatically commits/rollbacks.
    """
    if async_session_maker is None:
        await init_db()
    
    async with async_session_maker() as session:
        try:
            yield session
            await session.commit()
        except Exception:
            await session.rollback()
            raise
```

🔍 **Line-by-Line Breakdown:**

1. `DeclarativeBase` — SQLAlchemy 2.0 base class for models
2. `create_async_engine` — Creates async PostgreSQL engine
3. `async_sessionmaker` — Factory for creating sessions
4. `pool_pre_ping=True` — Tests connections before use
5. `get_session()` — FastAPI dependency that provides session
6. `expire_on_commit=False` — Prevents session expiration issues

### Step 1.8 — Create Core Redis Module

```python
# app/core/redis.py
"""
Async Redis client for caching and session management.
"""
import redis.asyncio as redis
from redis.asyncio import Redis

from app.config import get_settings


redis_client: Redis | None = None


async def init_redis() -> None:
    """Initialize Redis client."""
    global redis_client
    
    settings = get_settings()
    redis_client = redis.from_url(
        settings.REDIS_URL,
        encoding="utf-8",
        decode_responses=True,
    )


async def close_redis() -> None:
    """Close Redis client."""
    global redis_client
    
    if redis_client:
        await redis_client.close()


async def get_redis() -> Redis:
    """Get Redis client instance."""
    if redis_client is None:
        await init_redis()
    return redis_client
```

### Step 1.9 — Create Core __init__.py

```python
# app/core/__init__.py
"""
Core utilities for database and Redis.
"""
from app.core import database, redis

__all__ = ["database", "redis"]
```

### Step 1.10 — Create Dependencies Module

```python
# app/dependencies.py
"""
FastAPI dependencies for dependency injection.
Provides database sessions, Redis, and authentication.
"""
from typing import Annotated

from fastapi import Depends
from redis.asyncio import Redis
from sqlalchemy.ext.asyncio import AsyncSession

from app.core import database, redis as redis_core
from app.config import get_settings


async def get_db_session() -> AsyncGenerator[AsyncSession, None]:
    """Get database session dependency."""
    async for session in database.get_session():
        yield session


# Type aliases for cleaner dependency injection
DbSession = Annotated[AsyncSession, Depends(get_db_session)]
RedisClient = Annotated[Redis, Depends(redis_core.get_redis)]
```

### Step 1.11 — Create .env.example

```bash
# .env.example
# Copy this to .env and fill in your values

# Database - uses pgvector Docker image
DATABASE_URL=postgresql+asyncpg://pymind:pymind123@localhost:5432/pymind

# Redis
REDIS_URL=redis://localhost:6379/0

# OpenAI - get from https://platform.openai.com/api-keys
OPENAI_API_KEY=sk-your-key-here

# JWT - generate with: openssl rand -hex 32
JWT_SECRET_KEY=your-secret-key-min-32-chars-long

# JWT Settings
JWT_ALGORITHM=HS256
ACCESS_TOKEN_EXPIRE_MINUTES=30
REFRESH_TOKEN_EXPIRE_DAYS=7

# App Settings
APP_ENV=development
LOG_LEVEL=INFO
MAX_UPLOAD_SIZE_MB=10
CORS_ORIGINS=http://localhost:3000
```

### Step 1.12 — Create Docker Compose

```yaml
# docker-compose.yml
services:
  postgres:
    image: pgvector/pgvector:pg16
    container_name: pymind-postgres
    environment:
      POSTGRES_USER: pymind
      POSTGRES_PASSWORD: pymind123
      POSTGRES_DB: pymind
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U pymind"]
      interval: 5s
      timeout: 5s
      retries: 5

  redis:
    image: redis:7-alpine
    container_name: pymind-redis
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 5s
      timeout: 3s
      retries: 5

  app:
    build: .
    container_name: pymind-app
    ports:
      - "8000:8000"
    environment:
      - DATABASE_URL=postgresql+asyncpg://pymind:pymind123@postgres:5432/pymind
      - REDIS_URL=redis://redis:6379/0
      - OPENAI_API_KEY=${OPENAI_API_KEY}
      - JWT_SECRET_KEY=${JWT_SECRET_KEY}
    depends_on:
      postgres:
        condition: service_healthy
      redis:
        condition: service_healthy
    volumes:
      - ./:/app
    command: uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload

volumes:
  postgres_data:
  redis_data:
```

🔍 **Line-by-Line Breakdown:**

1. `postgres` service — Uses pgvector image for vector search
2. `healthcheck` — Ensures services are ready before app starts
3. `app` service — Builds from Dockerfile, mounts source for hot reload
4. `depends_on` with condition — Waits for healthy services

### Step 1.13 — Create Dockerfile

```dockerfile
# Dockerfile
FROM python:3.11-slim as builder

WORKDIR /app

# Install build dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    gcc \
    libpq-dev \
    && rm -rf /var/lib/apt/lists/*

# Install Python dependencies
COPY pyproject.toml .
RUN pip install --no-cache-dir -e .

FROM python:3.11-slim

WORKDIR /app

# Copy installed packages from builder
COPY --from=builder /usr/local/lib/python3.11/site-packages /usr/local/lib/python3.11/site-packages
COPY --from=builder /usr/local/bin /usr/local/bin

# Copy application code
COPY . .

# Create non-root user
RUN useradd --create-home appuser && chown -R appuser:appuser /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD python -c "import httpx; httpx.get('http://localhost:8000/health')"

# Run with uvicorn
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## How It All Connects

```
┌─────────────────────────────────────────────────────────────────┐
│                     Application Startup                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  docker-compose up                                              │
│       │                                                         │
│       ├──▶ PostgreSQL starts                                    │
│       │     └── pgvector extension loaded                       │
│       │                                                         │
│       ├──▶ Redis starts                                         │
│       │                                                         │
│       └──▶ App starts                                          │
│             └── lifespan: init_db()                             │
│             └── lifespan: init_redis()                          │
│                                                                  │
│  GET /health                                                    │
│       └── Returns {"status": "healthy"}                        │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Testing This Phase

### Start Infrastructure

```bash
docker-compose up -d postgres redis
```

Expected: Both containers running and healthy

### Create .env File

```bash
cp .env.example .env
# Edit .env with your values
```

### Run the App

```bash
uvicorn app.main:app --reload
```

### Test Health Endpoint

```bash
curl http://localhost:8000/health
```

Expected output:
```json
{
  "status": "healthy",
  "environment": "development"
}
```

## Common Errors in This Phase

### Error 1: ModuleNotFoundError

```
ModuleNotFoundError: No module named 'pymind'
```

**Fix:** Install package in development mode:
```bash
pip install -e .
```

### Error 2: Database Connection Refused

```
psycopg.OperationalError: connection refused
```

**Fix:** Ensure PostgreSQL is running:
```bash
docker-compose up -d postgres
docker-compose ps  # Check status
```

### Error 3: Redis Connection Refused

```
redis.exceptions.ConnectionError: Connection refused
```

**Fix:** Ensure Redis is running:
```bash
docker-compose up -d redis
```

### Error 4: Invalid JWT Secret

**Fix:** Generate a proper secret:
```bash
openssl rand -hex 32
```

## Phase Summary

**What was built:**
- FastAPI project with modern async architecture
- PostgreSQL + pgvector for document storage and vector search
- Redis for caching and session management
- Docker Compose for local development
- Health check endpoint

**What was learned:**
- Async FastAPI with lifespan events
- Pydantic settings for configuration
- SQLAlchemy 2.0 async patterns
- Docker Compose for multi-service setup

## Next Phase

→ Phase 2 — Database and Models: Create SQLAlchemy models for users, documents, chunks, conversations, and messages with migrations.
