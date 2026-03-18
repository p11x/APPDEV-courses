# Phase 3 — Authentication

## Goal

By the end of this phase, you will have:
- User registration endpoint
- JWT-based login/logout
- Password hashing with bcrypt
- Token refresh mechanism
- Protected routes with dependency injection

## What You'll Build in This Phase

- [ ] Password hashing utilities
- [ ] JWT token creation and verification
- [ ] User registration endpoint
- [ ] User login endpoint
- [ ] Token refresh endpoint
- [ ] Authentication dependency
- [ ] Protected route example

## Prerequisites

- Completed Phase 2 (database models)
- Understanding of JWT concepts
- Understanding of FastAPI dependencies

## Authentication Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    User Registration                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  POST /auth/register                                             │
│  {                                                               │
│    "email": "user@example.com",                                  │
│    "username": "johndoe",                                        │
│    "password": "securepassword123"                              │
│  }                                                               │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Validate input      │                                         │
│  │ Check email exists  │                                         │
│  │ Hash password       │                                         │
│  │ Create user         │                                         │
│  │ Return user data    │                                         │
│  └─────────────────────┘                                         │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                    User Login                                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  POST /auth/login                                                │
│  {                                                               │
│    "email": "user@example.com",                                  │
│    "password": "securepassword123"                              │
│  }                                                               │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Find user by email  │                                         │
│  │ Verify password     │                                         │
│  │ Generate tokens     │                                         │
│  │ Return access +     │                                         │
│  │   refresh tokens    │                                         │
│  └─────────────────────┘                                         │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                    Protected Request                             │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  GET /documents                                                  │
│  Authorization: Bearer <access_token>                           │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Decode JWT         │                                         │
│  │ Get user_id        │                                         │
│  │ Query user         │                                         │
│  │ Return data        │                                         │
│  └─────────────────────┘                                         │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Step-by-Step Implementation

### Step 3.1 — Create Security Utilities

```python
# app/core/security.py
"""
Security utilities for password hashing and JWT tokens.
"""
from datetime import datetime, timedelta, timezone
from typing import Optional

from jose import JWTError, jwt
from passlib.context import CryptContext
from pydantic import BaseModel

from app.config import get_settings

# Password hashing context
pwd_context = CryptContext(
    schemes=["bcrypt"],
    deprecated="auto",
)


class TokenData(BaseModel):
    """JWT token payload."""
    user_id: str
    exp: Optional[datetime] = None


class Token(BaseModel):
    """Token response model."""
    access_token: str
    refresh_token: str
    token_type: str = "bearer"


def verify_password(plain_password: str, hashed_password: str) -> bool:
    """
    Verify a plain password against a hashed password.
    
    Args:
        plain_password: The plain text password from user input
        hashed_password: The hashed password from database
    
    Returns:
        True if password matches, False otherwise
    """
    return pwd_context.verify(plain_password, hashed_password)


def hash_password(password: str) -> str:
    """
    Hash a password using bcrypt.
    
    Args:
        password: Plain text password to hash
    
    Returns:
        Hashed password string
    """
    return pwd_context.hash(password)


def create_access_token(user_id: str, expires_delta: Optional[timedelta] = None) -> str:
    """
    Create a JWT access token.
    
    Args:
        user_id: The user's UUID
        expires_delta: Optional custom expiration time
    
    Returns:
        Encoded JWT token string
    """
    settings = get_settings()
    
    if expires_delta:
        expire = datetime.now(timezone.utc) + expires_delta
    else:
        expire = datetime.now(timezone.utc) + timedelta(
            minutes=settings.ACCESS_TOKEN_EXPIRE_MINUTES
        )
    
    to_encode = {
        "sub": user_id,
        "exp": expire,
        "type": "access",
    }
    
    encoded_jwt = jwt.encode(
        to_encode,
        settings.JWT_SECRET_KEY,
        algorithm=settings.JWT_ALGORITHM,
    )
    
    return encoded_jwt


def create_refresh_token(user_id: str) -> str:
    """
    Create a JWT refresh token with longer expiration.
    
    Args:
        user_id: The user's UUID
    
    Returns:
        Encoded JWT refresh token string
    """
    settings = get_settings()
    
    expire = datetime.now(timezone.utc) + timedelta(
        days=settings.REFRESH_TOKEN_EXPIRE_DAYS
    )
    
    to_encode = {
        "sub": user_id,
        "exp": expire,
        "type": "refresh",
    }
    
    encoded_jwt = jwt.encode(
        to_encode,
        settings.JWT_SECRET_KEY,
        algorithm=settings.JWT_ALGORITHM,
    )
    
    return encoded_jwt


def verify_token(token: str) -> Optional[TokenData]:
    """
    Verify and decode a JWT token.
    
    Args:
        token: JWT token string
    
    Returns:
        TokenData if valid, None if invalid
    """
    settings = get_settings()
    
    try:
        payload = jwt.decode(
            token,
            settings.JWT_SECRET_KEY,
            algorithms=[settings.JWT_ALGORITHM],
        )
        
        user_id: str = payload.get("sub")
        
        if user_id is None:
            return None
        
        return TokenData(user_id=user_id)
    
    except JWTError:
        return None
```

🔍 **Line-by-Line Breakdown:**

1. `pwd_context` — Passlib context for bcrypt hashing
2. `verify_password()` — Compares plain password with hash using bcrypt
3. `hash_password()` — Creates bcrypt hash from plain password
4. `jwt.encode()` — Creates JWT with claims (sub, exp, type)
5. `jwt.decode()` — Verifies and decodes JWT, raises JWTError if invalid

### Step 3.2 — Create Auth Schemas

```python
# app/schemas/auth.py
"""
Pydantic schemas for authentication endpoints.
"""
from pydantic import BaseModel, EmailStr, Field, ConfigDict


# Registration
class UserCreate(BaseModel):
    """Request schema for user registration."""
    email: EmailStr = Field(..., description="User email address")
    username: str = Field(
        ..., 
        min_length=3, 
        max_length=100,
        description="Username (3-100 characters)"
    )
    password: str = Field(
        ..., 
        min_length=8,
        description="Password (minimum 8 characters)"
    )


class UserResponse(BaseModel):
    """Response schema for user data."""
    model_config = ConfigDict(from_attributes=True)
    
    id: str
    email: str
    username: str
    is_active: bool
    is_verified: bool


# Login
class UserLogin(BaseModel):
    """Request schema for user login."""
    email: EmailStr
    password: str


# Token
class TokenResponse(BaseModel):
    """Response schema for authentication tokens."""
    access_token: str
    refresh_token: str
    token_type: str = "bearer"


class RefreshTokenRequest(BaseModel):
    """Request schema for token refresh."""
    refresh_token: str


class MessageResponse(BaseModel):
    """Generic message response."""
    message: str
```

### Step 3.3 — Create Auth Service

```python
# app/services/auth_service.py
"""
Authentication service with business logic.
"""
from typing import Optional

from sqlalchemy import select
from sqlalchemy.exc import IntegrityError
from sqlalchemy.ext.asyncio import AsyncSession

from app.models import User
from app.core import security
from app.schemas.auth import UserCreate, UserLogin, TokenResponse


class AuthService:
    """Service for authentication operations."""
    
    @staticmethod
    async def register(
        session: AsyncSession, 
        user_data: UserCreate
    ) -> tuple[Optional[User], Optional[str]]:
        """
        Register a new user.
        
        Args:
            session: Database session
            user_data: Registration data
        
        Returns:
            Tuple of (User or None, error message or None)
        """
        # Check if email exists
        result = await session.execute(
            select(User).where(User.email == user_data.email)
        )
        existing_email = result.scalar_one_or_none()
        
        if existing_email:
            return None, "Email already registered"
        
        # Check if username exists
        result = await session.execute(
            select(User).where(User.username == user_data.username)
        )
        existing_username = result.scalar_one_or_none()
        
        if existing_username:
            return None, "Username already taken"
        
        # Hash password
        hashed_password = security.hash_password(user_data.password)
        
        # Create user
        user = User(
            email=user_data.email,
            username=user_data.username,
            hashed_password=hashed_password,
        )
        
        try:
            session.add(user)
            await session.commit()
            await session.refresh(user)
            return user, None
            
        except IntegrityError:
            await session.rollback()
            return None, "Registration failed"
    
    @staticmethod
    async def login(
        session: AsyncSession, 
        login_data: UserLogin
    ) -> tuple[Optional[User], Optional[str]]:
        """
        Authenticate user login.
        
        Args:
            session: Database session
            login_data: Login credentials
        
        Returns:
            Tuple of (User or None, error message or None)
        """
        # Find user by email
        result = await session.execute(
            select(User).where(User.email == login_data.email)
        )
        user = result.scalar_one_or_none()
        
        if not user:
            return None, "Invalid email or password"
        
        # Verify password
        if not security.verify_password(login_data.password, user.hashed_password):
            return None, "Invalid email or password"
        
        if not user.is_active:
            return None, "Account is disabled"
        
        return user, None
    
    @staticmethod
    def create_tokens(user_id: str) -> TokenResponse:
        """
        Create access and refresh tokens for user.
        
        Args:
            user_id: User's UUID
        
        Returns:
            TokenResponse with both tokens
        """
        access_token = security.create_access_token(user_id)
        refresh_token = security.create_refresh_token(user_id)
        
        return TokenResponse(
            access_token=access_token,
            refresh_token=refresh_token,
        )
    
    @staticmethod
    async def refresh_access_token(
        session: AsyncSession,
        refresh_token: str
    ) -> tuple[Optional[TokenResponse], Optional[str]]:
        """
        Refresh access token using refresh token.
        
        Args:
            session: Database session
            refresh_token: Valid refresh token
        
        Returns:
            Tuple of (TokenResponse or None, error message or None)
        """
        # Verify refresh token
        token_data = security.verify_token(refresh_token)
        
        if not token_data:
            return None, "Invalid or expired refresh token"
        
        # Get user
        result = await session.execute(
            select(User).where(User.id == token_data.user_id)
        )
        user = result.scalar_one_or_none()
        
        if not user or not user.is_active:
            return None, "User not found or inactive"
        
        # Create new tokens
        return security.create_tokens(str(user.id)), None
```

### Step 3.4 — Create Auth Dependencies

```python
# app/dependencies/auth.py
"""
Authentication dependencies for FastAPI.
"""
from typing import Annotated, Optional

from fastapi import Depends, HTTPException, status
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.core import security
from app.models import User
from app.dependencies import get_db_session

# Security scheme
security_scheme = HTTPBearer()


async def get_current_user(
    credentials: Annotated[HTTPAuthorizationCredentials, Depends(security_scheme)],
    session: Annotated[AsyncSession, Depends(get_db_session)],
) -> User:
    """
    Dependency to get current authenticated user.
    
    Args:
        credentials: Bearer token from Authorization header
        session: Database session
    
    Returns:
        Current user
    
    Raises:
        HTTPException: If token is invalid or user not found
    """
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": "Bearer"},
    )
    
    # Verify token
    token_data = security.verify_token(credentials.credentials)
    
    if not token_data:
        raise credentials_exception
    
    # Get user
    result = await session.execute(
        select(User).where(User.id == token_data.user_id)
    )
    user = result.scalar_one_or_none()
    
    if not user:
        raise credentials_exception
    
    return user


async def get_optional_user(
    credentials: Annotated[
        Optional[HTTPAuthorizationCredentials], 
        Depends(HTTPBearer(auto_error=False))
    ],
    session: Annotated[AsyncSession, Depends(get_db_session)],
) -> Optional[User]:
    """
    Dependency to get current user if authenticated, None otherwise.
    
    Useful for optional authentication (e.g., public endpoints).
    """
    if not credentials:
        return None
    
    token_data = security.verify_token(credentials.credentials)
    
    if not token_data:
        return None
    
    result = await session.execute(
        select(User).where(User.id == token_data.user_id)
    )
    
    return result.scalar_one_or_none()


# Type alias for cleaner injection
CurrentUser = Annotated[User, Depends(get_current_user)]
OptionalUser = Annotated[Optional[User], Depends(get_optional_user)]
```

### Step 3.5 — Create Auth Router

```python
# app/routers/auth.py
"""
Authentication endpoints for registration and login.
"""
from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.ext.asyncio import AsyncSession

from app.dependencies import get_db_session
from app.schemas.auth import (
    UserCreate, 
    UserResponse, 
    UserLogin, 
    TokenResponse,
    RefreshTokenRequest,
    MessageResponse,
)
from app.services.auth_service import AuthService

router = APIRouter(prefix="/auth", tags=["Authentication"])


@router.post(
    "/register",
    response_model=UserResponse,
    status_code=status.HTTP_201_CREATED,
    summary="Register new user",
)
async def register(
    user_data: UserCreate,
    session: AsyncSession = Depends(get_db_session),
) -> UserResponse:
    """
    Register a new user account.
    
    - **email**: Valid email address (must be unique)
    - **username**: Username (3-100 chars, must be unique)
    - **password**: Password (minimum 8 characters)
    """
    user, error = await AuthService.register(session, user_data)
    
    if error:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=error,
        )
    
    return UserResponse.model_validate(user)


@router.post(
    "/login",
    response_model=TokenResponse,
    summary="Login user",
)
async def login(
    login_data: UserLogin,
    session: AsyncSession = Depends(get_db_session),
) -> TokenResponse:
    """
    Login with email and password.
    
    Returns access and refresh tokens on success.
    """
    user, error = await AuthService.login(session, login_data)
    
    if error:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail=error,
        )
    
    return AuthService.create_tokens(str(user.id))


@router.post(
    "/refresh",
    response_model=TokenResponse,
    summary="Refresh access token",
)
async def refresh_token(
    request: RefreshTokenRequest,
    session: AsyncSession = Depends(get_db_session),
) -> TokenResponse:
    """
    Refresh access token using valid refresh token.
    
    Use this endpoint when access token expires to get new tokens.
    """
    tokens, error = await AuthService.refresh_access_token(
        session, 
        request.refresh_token
    )
    
    if error:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail=error,
        )
    
    return tokens


@router.post(
    "/logout",
    response_model=MessageResponse,
    summary="Logout user",
)
async def logout() -> MessageResponse:
    """
    Logout user (client should discard tokens).
    
    Note: For stateless JWT, this is a no-op server-side.
    Implement Redis token blacklist for true logout.
    """
    return MessageResponse(message="Successfully logged out")
```

### Step 3.6 — Register Router in Main App

```python
# app/main.py additions

def create_app() -> FastAPI:
    # ... existing code ...
    
    # Import and include routers
    from app.routers import auth, documents, chat
    
    app.include_router(auth.router)
    app.include_router(documents.router)
    app.include_router(chat.router)
    
    return app
```

### Step 3.7 — Create Protected Route Example

```python
# app/routers/documents.py (excerpt)
"""
Document endpoints with authentication.
"""
from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select
from typing import List

from app.dependencies import get_db_session
from app.dependencies.auth import CurrentUser
from app.models import Document
from app.schemas.document import DocumentResponse

router = APIRouter(prefix="/documents", tags=["Documents"])


@router.get("/", response_model=List[DocumentResponse])
async def list_documents(
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> List[DocumentResponse]:
    """
    List all documents for current user.
    
    Requires authentication.
    """
    result = await session.execute(
        select(Document)
        .where(Document.user_id == str(current_user.id))
        .order_by(Document.created_at.desc())
    )
    documents = result.scalars().all()
    
    return [DocumentResponse.model_validate(d) for d in documents]


@router.get("/{document_id}", response_model=DocumentResponse)
async def get_document(
    document_id: str,
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> DocumentResponse:
    """
    Get a specific document by ID.
    
    Requires authentication. Only returns document if owned by user.
    """
    result = await session.execute(
        select(Document).where(
            Document.id == document_id,
            Document.user_id == str(current_user.id),
        )
    )
    document = result.scalar_one_or_none()
    
    if not document:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Document not found",
        )
    
    return DocumentResponse.model_validate(document)
```

## Testing This Phase

### Register a User

```bash
curl -X POST http://localhost:8000/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "username": "testuser",
    "password": "securepassword123"
  }'
```

Expected response:
```json
{
  "id": "uuid-here",
  "email": "test@example.com",
  "username": "testuser",
  "is_active": true,
  "is_verified": false
}
```

### Login

```bash
curl -X POST http://localhost:8000/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "securepassword123"
  }'
```

Expected response:
```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIs...",
  "refresh_token": "eyJhbGciOiJIUzI1NiIs...",
  "token_type": "bearer"
}
```

### Access Protected Route

```bash
curl -X GET http://localhost:8000/documents/ \
  -H "Authorization: Bearer <access_token>"
```

### Refresh Token

```bash
curl -X POST http://localhost:8000/auth/refresh \
  -H "Content-Type: application/json" \
  -d '{"refresh_token": "<refresh_token>"}'
```

## Common Errors in This Phase

### Error 1: Invalid Token

```
{"detail": "Could not validate credentials"}
```

**Fix:** Check Authorization header format: `Bearer <token>`

### Error 2: Expired Token

```
{"detail": "Token expired"}
```

**Fix:** Use refresh token to get new access token

### Error 3: Duplicate Email

```
{"detail": "Email already registered"}
```

**Fix:** Use different email or login with existing account

### Error 4: Password Too Short

```
Input should be at least 8 characters
```

**Fix:** Provide password with minimum 8 characters

## Phase Summary

**What was built:**
- Password hashing with bcrypt
- JWT access and refresh tokens
- User registration endpoint
- User login endpoint
- Token refresh endpoint
- Authentication dependency
- Protected route example

**What was learned:**
- JWT token structure and claims
- bcrypt password hashing
- FastAPI dependency injection
- HTTP Bearer authentication
- Token refresh flow

## Next Phase

→ Phase 4 — Document Ingestion: Implement file upload, parsing, and storage for PDF, TXT, and DOCX files.
