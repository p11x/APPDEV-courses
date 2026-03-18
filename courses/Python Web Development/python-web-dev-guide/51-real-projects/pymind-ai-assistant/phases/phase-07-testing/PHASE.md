# Phase 7 — Testing

## Goal

By the end of this phase, you will have:
- Unit tests for services
- Integration tests for API endpoints
- Test fixtures and conftest.py
- Test database setup
- Coverage configuration

## Prerequisites

- Completed Phase 6 (RAG pipeline)
- pytest installed

## Test Structure

```
tests/
├── __init__.py
├── conftest.py          # Fixtures
├── test_auth.py         # Authentication tests
├── test_documents.py    # Document tests
├── test_chat.py         # Chat tests
└── test_services.py     # Service unit tests
```

## Step-by-Step Implementation

### Step 7.1 — Create Test Configuration

```python
# pytest.ini (or in pyproject.toml)
[tool.pytest.ini_options]
asyncio_mode = "auto"
testpaths = ["tests"]
python_files = ["test_*.py"]
python_classes = ["Test*"]
python_functions = ["test_*"]
addopts = "-v --tb=short"
```

### Step 7.2 — Create Conftest with Fixtures

```python
# tests/conftest.py
"""
Pytest fixtures for testing.
"""
import asyncio
from typing import AsyncGenerator, Generator
from uuid import uuid4

import pytest
import pytest_asyncio
from httpx import AsyncClient, ASGITransport
from sqlalchemy.ext.asyncio import AsyncSession, create_async_engine, async_sessionmaker

from app.main import app
from app.models import Base
from app.core.database import get_session
from app.dependencies.auth import get_current_user
from app.models import User


# Test database URL
TEST_DATABASE_URL = "postgresql+asyncpg://pymind:pymind123@localhost:5432/pymind_test"


# Create test engine
test_engine = create_async_engine(
    TEST_DATABASE_URL,
    echo=False,
    pool_pre_ping=True,
)

TestSessionLocal = async_sessionmaker(
    test_engine,
    class_=AsyncSession,
    expire_on_commit=False,
)


@pytest.fixture(scope="session")
def event_loop() -> Generator:
    """Create event loop for async tests."""
    loop = asyncio.get_event_loop_policy().new_event_loop()
    yield loop
    loop.close()


@pytest_asyncio.fixture(scope="function")
async def db_session() -> AsyncGenerator[AsyncSession, None]:
    """Create test database session."""
    async with test_engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)
    
    async with TestSessionLocal() as session:
        yield session
    
    async with test_engine.begin() as conn:
        await conn.run_sync(Base.metadata.drop_all)


@pytest_asyncio.fixture
async def test_user(db_session: AsyncSession) -> User:
    """Create test user."""
    from app.core.security import hash_password
    
    user = User(
        id=uuid4(),
        email="test@example.com",
        username="testuser",
        hashed_password=hash_password("testpassword123"),
        is_active=True,
    )
    
    db_session.add(user)
    await db_session.commit()
    await db_session.refresh(user)
    
    return user


@pytest_asyncio.fixture
async def auth_client(
    test_user: User,
) -> AsyncGenerator[AsyncClient, None]:
    """Create authenticated test client."""
    
    # Override get_current_user to return test user
    async def override_get_current_user():
        return test_user
    
    app.dependency_overrides[get_current_user] = override_get_current_user
    
    transport = ASGITransport(app=app)
    async with AsyncClient(transport=transport, base_url="http://test") as client:
        yield client
    
    app.dependency_overrides.clear()


@pytest_asyncio.fixture
async def unauth_client() -> AsyncGenerator[AsyncClient, None]:
    """Create unauthenticated test client."""
    transport = ASGITransport(app=app)
    async with AsyncClient(transport=transport, base_url="http://test") as client:
        yield client
```

### Step 7.3 — Create Auth Tests

```python
# tests/test_auth.py
"""
Tests for authentication endpoints.
"""
import pytest
from httpx import AsyncClient


@pytest.mark.asyncio
async def test_register_success(unauth_client: AsyncClient) -> None:
    """Test user registration."""
    response = await unauth_client.post(
        "/auth/register",
        json={
            "email": "newuser@example.com",
            "username": "newuser",
            "password": "securepassword123",
        },
    )
    
    assert response.status_code == 201
    data = response.json()
    assert data["email"] == "newuser@example.com"
    assert data["username"] == "newuser"
    assert "password" not in data


@pytest.mark.asyncio
async def test_register_duplicate_email(unauth_client: AsyncClient, test_user) -> None:
    """Test registration with duplicate email."""
    response = await unauth_client.post(
        "/auth/register",
        json={
            "email": "test@example.com",  # Already exists
            "username": "otheruser",
            "password": "password123",
        },
    )
    
    assert response.status_code == 400
    assert "email" in response.json()["detail"].lower()


@pytest.mark.asyncio
async def test_login_success(unauth_client: AsyncClient, test_user) -> None:
    """Test successful login."""
    response = await unauth_client.post(
        "/auth/login",
        json={
            "email": "test@example.com",
            "password": "testpassword123",
        },
    )
    
    assert response.status_code == 200
    data = response.json()
    assert "access_token" in data
    assert "refresh_token" in data
    assert data["token_type"] == "bearer"


@pytest.mark.asyncio
async def test_login_invalid_password(unauth_client: AsyncClient, test_user) -> None:
    """Test login with wrong password."""
    response = await unauth_client.post(
        "/auth/login",
        json={
            "email": "test@example.com",
            "password": "wrongpassword",
        },
    )
    
    assert response.status_code == 401


@pytest.mark.asyncio
async def test_refresh_token(unauth_client: AsyncClient, test_user) -> None:
    """Test token refresh."""
    # First login
    login_response = await unauth_client.post(
        "/auth/login",
        json={
            "email": "test@example.com",
            "password": "testpassword123",
        },
    )
    tokens = login_response.json()
    
    # Then refresh
    response = await unauth_client.post(
        "/auth/refresh",
        json={"refresh_token": tokens["refresh_token"]},
    )
    
    assert response.status_code == 200
    assert "access_token" in response.json()
```

### Step 7.4 — Create Document Tests

```python
# tests/test_documents.py
"""
Tests for document endpoints.
"""
import io
from uuid import uuid4

import pytest
from httpx import AsyncClient


@pytest.mark.asyncio
async def test_list_documents_empty(auth_client: AsyncClient) -> None:
    """Test listing documents when none exist."""
    response = await auth_client.get("/documents/")
    
    assert response.status_code == 200
    data = response.json()
    assert data["documents"] == []
    assert data["total"] == 0


@pytest.mark.asyncio
async def test_upload_document(auth_client: AsyncClient) -> None:
    """Test document upload."""
    file_content = b"Test document content for testing"
    files = {"file": ("test.txt", io.BytesIO(file_content), "text/plain")}
    
    response = await auth_client.post("/documents/upload", files=files)
    
    assert response.status_code == 201
    data = response.json()
    assert data["document"]["file_name"] == "test.txt"
    assert data["document"]["file_type"] == "text/plain"


@pytest.mark.asyncio
async def test_upload_invalid_file_type(auth_client: AsyncClient) -> None:
    """Test upload with invalid file type."""
    file_content = b"test"
    files = {"file": ("test.exe", io.BytesIO(file_content), "application/x-executable")}
    
    response = await auth_client.post("/documents/upload", files=files)
    
    assert response.status_code == 400
    assert "not allowed" in response.json()["detail"].lower()


@pytest.mark.asyncio
async def test_get_document(auth_client: AsyncClient, test_user) -> None:
    """Test getting a document."""
    # First create a document
    file_content = b"Test content"
    files = {"file": ("test.txt", io.BytesIO(file_content), "text/plain")}
    
    create_response = await auth_client.post("/documents/upload", files=files)
    doc_id = create_response.json()["document"]["id"]
    
    # Then get it
    response = await auth_client.get(f"/documents/{doc_id}")
    
    assert response.status_code == 200
    assert response.json()["id"] == doc_id


@pytest.mark.asyncio
async def test_delete_document(auth_client: AsyncClient) -> None:
    """Test deleting a document."""
    # Create document
    file_content = b"Test content"
    files = {"file": ("test.txt", io.BytesIO(file_content), "text/plain")}
    
    create_response = await auth_client.post("/documents/upload", files=files)
    doc_id = create_response.json()["document"]["id"]
    
    # Delete it
    response = await auth_client.delete(f"/documents/{doc_id}")
    
    assert response.status_code == 204
    
    # Verify deleted
    get_response = await auth_client.get(f"/documents/{doc_id}")
    assert get_response.status_code == 404
```

### Step 7.5 — Create Service Unit Tests

```python
# tests/test_services.py
"""
Unit tests for service layer.
"""
import pytest
from unittest.mock import AsyncMock, MagicMock

from app.services.auth_service import AuthService
from app.services.document_service import DocumentService
from app.schemas.auth import UserCreate, UserLogin


@pytest.mark.asyncio
async def test_create_user(db_session, test_user) -> None:
    """Test user service registration."""
    user_data = UserCreate(
        email="new@example.com",
        username="newuser",
        password="password123",
    )
    
    user, error = await AuthService.register(db_session, user_data)
    
    assert user is not None
    assert error is None
    assert user.email == "new@example.com"
    assert user.username == "newuser"


@pytest.mark.asyncio
async def test_login_success(db_session, test_user) -> None:
    """Test user login."""
    login_data = UserLogin(
        email="test@example.com",
        password="testpassword123",
    )
    
    user, error = await AuthService.login(db_session, login_data)
    
    assert user is not None
    assert error is None
    assert user.email == "test@example.com"


@pytest.mark.asyncio
async def test_login_invalid_credentials(db_session, test_user) -> None:
    """Test login with wrong credentials."""
    login_data = UserLogin(
        email="test@example.com",
        password="wrongpassword",
    )
    
    user, error = await AuthService.login(db_session, login_data)
    
    assert user is None
    assert error is not None
```

### Step 7.6 — Create Test Requirements

```toml
# pyproject.toml additions
[project.optional-dependencies]
dev = [
    # ... existing ...
    "pytest>=7.4.0",
    "pytest-asyncio>=0.23.0",
    "pytest-cov>=4.1.0",
    "httpx>=0.26.0",
    "aiosqlite>=0.19.0",  # For SQLite testing
]
```

## Running Tests

```bash
# Run all tests
pytest

# Run with coverage
pytest --cov=app --cov-report=html

# Run specific test file
pytest tests/test_auth.py

# Run with verbose output
pytest -v
```

## Phase Summary

**What was built:**
- Test fixtures and conftest.py
- Authentication tests
- Document tests
- Service unit tests
- Coverage configuration

**What was learned:**
- pytest fixtures
- Async testing
- Test isolation
- Coverage reporting
