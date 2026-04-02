# Development Environment Setup

## Overview

A well-configured development environment improves productivity and code quality. This guide covers setting up your IDE, tools, and workflows for efficient FastAPI development.

## IDE Configuration

### VS Code Setup (Recommended)

#### Required Extensions

```json
// .vscode/extensions.json
// VS Code will prompt to install these when opening the project
{
    "recommendations": [
        "ms-python.python",           // Python support
        "ms-python.vscode-pylance",   // Type checking and IntelliSense
        "ms-python.black-formatter",  // Code formatting
        "charliermarsh.ruff",         // Linting
        "tamasfe.even-better-toml",   // TOML file support
        "redhat.vscode-yaml",         // YAML support
        "humao.rest-client",          // REST API testing
        "ms-python.mypy-type-checker" // Type checking
    ]
}
```

#### VS Code Settings

```json
// .vscode/settings.json
{
    // Python settings
    "python.defaultInterpreterPath": "${workspaceFolder}/venv/bin/python",
    "python.terminal.activateEnvironment": true,
    "python.analysis.typeCheckingMode": "basic",
    "python.analysis.autoImportCompletions": true,

    // Formatting settings
    "[python]": {
        "editor.defaultFormatter": "ms-python.black-formatter",
        "editor.formatOnSave": true,
        "editor.codeActionsOnSave": {
            "source.organizeImports": "explicit"
        }
    },

    // Linting settings
    "ruff.lint.args": ["--config=pyproject.toml"],
    "ruff.format.args": ["--config=pyproject.toml"],

    // File associations
    "files.associations": {
        "*.md": "markdown",
        "pyproject.toml": "toml"
    },

    // Exclude from search
    "files.exclude": {
        "**/__pycache__": true,
        "**/*.pyc": true,
        "**/.pytest_cache": true,
        "**/.mypy_cache": true,
        "**/.ruff_cache": true,
        "**/venv": true,
        "**/.env": true
    },

    // Editor settings
    "editor.rulers": [88],  // Black default line length
    "editor.tabSize": 4,
    "editor.insertSpaces": true
}
```

#### Launch Configuration for Debugging

```json
// .vscode/launch.json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "FastAPI",
            "type": "debugpy",
            "request": "launch",
            "module": "uvicorn",
            "args": [
                "app.main:app",
                "--reload",
                "--host", "0.0.0.0",
                "--port", "8000"
            ],
            "jinja": true,
            "justMyCode": true
        },
        {
            "name": "FastAPI (Debug Mode)",
            "type": "debugpy",
            "request": "launch",
            "module": "uvicorn",
            "args": [
                "app.main:app",
                "--reload",
                "--log-level", "debug"
            ],
            "jinja": true,
            "justMyCode": false  // Step into library code
        },
        {
            "name": "Current File",
            "type": "debugpy",
            "request": "launch",
            "program": "${file}",
            "console": "integratedTerminal"
        }
    ]
}
```

### PyCharm Setup

```python
# PyCharm configuration tips:

# 1. Set Python interpreter
# File > Settings > Project > Python Interpreter
# Select your virtual environment

# 2. Enable type checking
# File > Settings > Editor > Inspections > Python
# Enable: "Type checker" and "Unresolved references"

# 3. Configure FastAPI support
# File > Settings > Languages & Frameworks > FastAPI
# Enable auto-detection

# 4. Set up run configurations
# Run > Edit Configurations
# Add new "FastAPI" configuration
# Module: app.main:app
```

## Code Quality Tools

### Black (Code Formatter)

```bash
# Install Black
pip install black

# Configuration in pyproject.toml
cat >> pyproject.toml << 'EOF'

[tool.black]
line-length = 88
target-version = ['py39']
include = '\.pyi?$'
exclude = '''
/(
    \.git
  | \.hg
  | \.mypy_cache
  | \.tox
  | \.venv
  | _build
  | buck-out
  | build
  | dist
)/
'''
EOF

# Usage
black .                    # Format all files
black app/main.py         # Format specific file
black --check .           # Check without modifying
black --diff .            # Show differences
```

### Ruff (Linter)

```bash
# Install Ruff
pip install ruff

# Configuration in pyproject.toml
cat >> pyproject.toml << 'EOF'

[tool.ruff]
# Enable pycodestyle (E), pyflakes (F), isort (I), and more
select = ["E", "F", "I", "N", "W", "UP", "B", "A", "C4", "SIM"]
ignore = ["E501"]  # Line length handled by Black

# Allow autofix for all rules
fixable = ["ALL"]

[tool.ruff.isort]
known-first-party = ["app"]

[tool.ruff.flake8-bugbear]
extend-immutable-calls = ["fastapi.Depends", "fastapi.Query"]
EOF

# Usage
ruff check .              # Lint all files
ruff check --fix .        # Auto-fix issues
ruff format .             # Alternative formatter
```

### MyPy (Type Checker)

```bash
# Install MyPy
pip install mypy

# Configuration in pyproject.toml
cat >> pyproject.toml << 'EOF'

[tool.mypy]
python_version = "3.9"
warn_return_any = true
warn_unused_configs = true
disallow_untyped_defs = true
disallow_incomplete_defs = true
check_untyped_defs = true

[[tool.mypy.overrides]]
module = [
    "uvicorn.*",
    "starlette.*",
]
ignore_missing_imports = true
EOF

# Usage
mypy app/                 # Type check app directory
mypy app/main.py         # Type check specific file
```

### Pre-commit Hooks

```bash
# Install pre-commit
pip install pre-commit

# Create configuration
cat > .pre-commit-config.yaml << 'EOF'
repos:
  - repo: https://github.com/pre-commit/pre-commit-hooks
    rev: v4.4.0
    hooks:
      - id: trailing-whitespace
      - id: end-of-file-fixer
      - id: check-yaml
      - id: check-added-large-files

  - repo: https://github.com/psf/black
    rev: 23.7.0
    hooks:
      - id: black

  - repo: https://github.com/charliermarsh/ruff-pre-commit
    rev: v0.0.282
    hooks:
      - id: ruff
        args: [--fix, --exit-non-zero-on-fix]

  - repo: https://github.com/pre-commit/mirrors-mypy
    rev: v1.4.1
    hooks:
      - id: mypy
        additional_dependencies: [types-requests]
EOF

# Install hooks
pre-commit install

# Run manually
pre-commit run --all-files
```

## Project Configuration

### Complete pyproject.toml

```toml
# pyproject.toml - Complete project configuration
[project]
name = "fastapi-app"
version = "0.1.0"
description = "A FastAPI application"
requires-python = ">=3.9"
dependencies = [
    "fastapi>=0.100.0",
    "uvicorn[standard]>=0.23.0",
    "python-multipart>=0.0.6",
    "pydantic-settings>=2.0.0",
]

[project.optional-dependencies]
dev = [
    "pytest>=7.0.0",
    "pytest-asyncio>=0.21.0",
    "httpx>=0.24.0",
    "black>=23.0.0",
    "mypy>=1.0.0",
    "ruff>=0.0.280",
    "pre-commit>=3.0.0",
]
db = [
    "sqlalchemy>=2.0.0",
    "alembic>=1.11.0",
    "asyncpg>=0.28.0",
    "aiosqlite>=0.19.0",
]

[build-system]
requires = ["setuptools>=68.0", "wheel"]
build-backend = "setuptools.backends._legacy:_Backend"

# Tool configurations
[tool.black]
line-length = 88
target-version = ['py39']

[tool.ruff]
select = ["E", "F", "I", "N", "W", "UP", "B", "A", "C4", "SIM"]
ignore = ["E501"]

[tool.mypy]
python_version = "3.9"
warn_return_any = true
disallow_untyped_defs = true

[tool.pytest.ini_options]
asyncio_mode = "auto"
testpaths = ["tests"]
python_files = "test_*.py"
python_functions = "test_*"
addopts = "-v --tb=short"
```

## Testing Setup

### Test Configuration

```python
# tests/conftest.py - Pytest configuration for FastAPI
import pytest
from httpx import AsyncClient
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.pool import StaticPool

from app.main import app
from app.database import Base, get_db

# Test database setup
SQLALCHEMY_DATABASE_URL = "sqlite://"

engine = create_engine(
    SQLALCHEMY_DATABASE_URL,
    connect_args={"check_same_thread": False},
    poolclass=StaticPool,
)
TestingSessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Create tables for testing
Base.metadata.create_all(bind=engine)

def override_get_db():
    """
    Dependency override for testing.
    Uses test database instead of production.
    """
    try:
        db = TestingSessionLocal()
        yield db
    finally:
        db.close()

# Override the dependency
app.dependency_overrides[get_db] = override_get_db

@pytest.fixture
async def client():
    """
    Test client fixture.
    Provides an async HTTP client for testing endpoints.
    """
    async with AsyncClient(app=app, base_url="http://test") as ac:
        yield ac

@pytest.fixture
def sample_user():
    """Sample user data for testing"""
    return {
        "username": "testuser",
        "email": "test@example.com",
        "password": "testpassword123"
    }

@pytest.fixture
def sample_item():
    """Sample item data for testing"""
    return {
        "name": "Test Item",
        "price": 19.99,
        "description": "A test item"
    }
```

### Example Tests

```python
# tests/test_main.py - Basic API tests
import pytest
from httpx import AsyncClient

@pytest.mark.asyncio
async def test_root_endpoint(client: AsyncClient):
    """Test the root endpoint returns welcome message"""
    response = await client.get("/")
    assert response.status_code == 200
    data = response.json()
    assert "message" in data
    assert "Welcome" in data["message"]

@pytest.mark.asyncio
async def test_health_check(client: AsyncClient):
    """Test health check endpoint"""
    response = await client.get("/health")
    assert response.status_code == 200
    assert response.json() == {"status": "healthy"}

@pytest.mark.asyncio
async def test_create_item(client: AsyncClient, sample_item):
    """Test creating a new item"""
    response = await client.post("/items/", json=sample_item)
    assert response.status_code == 201
    data = response.json()
    assert data["name"] == sample_item["name"]
    assert data["price"] == sample_item["price"]
    assert "id" in data

@pytest.mark.asyncio
async def test_create_item_validation(client: AsyncClient):
    """Test validation error when creating invalid item"""
    invalid_item = {"name": "", "price": -10}
    response = await client.post("/items/", json=invalid_item)
    assert response.status_code == 422  # Validation error
    data = response.json()
    assert "detail" in data

@pytest.mark.asyncio
async def test_get_nonexistent_item(client: AsyncClient):
    """Test 404 for non-existent item"""
    response = await client.get("/items/99999")
    assert response.status_code == 404
```

### Running Tests

```bash
# Run all tests
pytest

# Run with verbose output
pytest -v

# Run specific test file
pytest tests/test_main.py

# Run specific test function
pytest tests/test_main.py::test_root_endpoint

# Run with coverage
pip install pytest-cov
pytest --cov=app --cov-report=html

# Run tests in parallel
pip install pytest-xdist
pytest -n auto  # Use all CPU cores
```

## Database Setup

### SQLAlchemy Configuration

```python
# app/database.py - Database configuration
from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker
from typing import Generator

# Database URL - can be loaded from environment
SQLALCHEMY_DATABASE_URL = "sqlite:///./app.db"
# For PostgreSQL: "postgresql://user:password@localhost/dbname"
# For MySQL: "mysql://user:password@localhost/dbname"

# Create engine
engine = create_engine(
    SQLALCHEMY_DATABASE_URL,
    # SQLite specific configuration
    connect_args={"check_same_thread": False},
    # Echo SQL for debugging
    echo=False,
)

# Create session factory
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Base class for models
Base = declarative_base()

def get_db() -> Generator:
    """
    Database session dependency.
    Ensures session is closed after request.
    """
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()
```

### Async Database Setup

```python
# app/async_database.py - Async database configuration
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.ext.asyncio import async_sessionmaker
from sqlalchemy.orm import declarative_base
from typing import AsyncGenerator

# Async database URL
ASYNC_DATABASE_URL = "sqlite+aiosqlite:///./app.db"
# For PostgreSQL: "postgresql+asyncpg://user:password@localhost/dbname"

# Create async engine
async_engine = create_async_engine(
    ASYNC_DATABASE_URL,
    echo=False,
)

# Create async session factory
AsyncSessionLocal = async_sessionmaker(
    async_engine,
    class_=AsyncSession,
    expire_on_commit=False,
)

Base = declarative_base()

async def get_async_db() -> AsyncGenerator[AsyncSession, None]:
    """Async database session dependency"""
    async with AsyncSessionLocal() as session:
        try:
            yield session
        finally:
            await session.close()
```

## API Testing Tools

### Using FastAPI's Built-in Docs

```bash
# Start your application
uvicorn app.main:app --reload

# Access interactive documentation
# Swagger UI: http://localhost:8000/docs
# ReDoc: http://localhost:8000/redoc

# OpenAPI JSON: http://localhost:8000/openapi.json
```

### HTTPie (Command Line)

```bash
# Install HTTPie
pip install httpie

# Basic GET request
http GET http://localhost:8000/items/

# POST with JSON
http POST http://localhost:8000/items/ \
    name="Laptop" \
    price:=999.99 \
    description="A powerful laptop"

# With headers
http GET http://localhost:8000/items/ \
    Authorization:"Bearer your-token"

# With query parameters
http GET http://localhost:8000/items/ \
    limit==10 \
    offset==0
```

### REST Client (VS Code Extension)

```http
// requests.http - REST Client file for VS Code
// Create this file and click "Send Request" above each request

### Get all items
GET http://localhost:8000/items/
Accept: application/json

### Get specific item
GET http://localhost:8000/items/1
Accept: application/json

### Create new item
POST http://localhost:8000/items/
Content-Type: application/json

{
    "name": "New Item",
    "price": 29.99,
    "description": "Created via REST Client"
}

### Update item
PUT http://localhost:8000/items/1
Content-Type: application/json

{
    "name": "Updated Item",
    "price": 39.99
}

### Delete item
DELETE http://localhost:8000/items/1
```

## Docker Setup (Optional)

```dockerfile
# Dockerfile - Containerize your FastAPI application
FROM python:3.11-slim

WORKDIR /app

# Install system dependencies
RUN apt-get update && apt-get install -y \
    gcc \
    && rm -rf /var/lib/apt/lists/*

# Install Python dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy application code
COPY . .

# Expose port
EXPOSE 8000

# Run application
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
```

```yaml
# docker-compose.yml - Multi-container setup
version: '3.8'

services:
  app:
    build: .
    ports:
      - "8000:8000"
    environment:
      - DATABASE_URL=postgresql://postgres:postgres@db:5432/app
    depends_on:
      - db
    volumes:
      - .:/app
    command: uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload

  db:
    image: postgres:15
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=app
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

## Summary

| Tool | Purpose | Configuration |
|------|---------|---------------|
| VS Code | IDE | Settings, extensions |
| Black | Formatting | pyproject.toml |
| Ruff | Linting | pyproject.toml |
| MyPy | Type checking | pyproject.toml |
| Pytest | Testing | conftest.py |
| Pre-commit | Git hooks | .pre-commit-config.yaml |

## Next Steps

With your environment configured:
- [Your First FastAPI App](./04_your_first_fastapi_app.md) - Build your first application
- [Routing](../03_basic_concepts/01_routing.md) - Learn about FastAPI routing
