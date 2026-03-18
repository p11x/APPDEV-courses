# Authentication and JWT

## What You'll Learn
- What JWT (JSON Web Tokens) are
- Implementing JWT authentication in FastAPI
- Password hashing
- Protected routes
- Token refresh

## Prerequisites
- Completed FastAPI Dependency Injection

## What Is JWT?

**JSON Web Token (JWT)** is a compact, URL-safe way to transmit claims between parties. It's commonly used for authentication.

A JWT has three parts:
1. **Header** — Algorithm and token type
2. **Payload** — The data (claims)
3. **Signature** — Verifies the token wasn't tampered with

```
xxxxx.yyyyy.zzzzz
header.payload.signature
```

## Installing Dependencies

```bash
pip install python-jose passlib[bcrypt] python-multipart
```

- `python-jose` — JWT encoding/decoding
- `passlib[bcrypt]` — Password hashing

## Password Hashing

Never store passwords in plain text! Use hashing:

```python
from passlib.context import CryptContext

# Password hashing context
pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")

def hash_password(password: str) -> str:
    """Hash a password."""
    return pwd_context.hash(password)

def verify_password(plain_password: str, hashed_password: str) -> bool:
    """Verify a password against a hash."""
    return pwd_context.verify(plain_password, hashed_password)

# Test
hashed = hash_password("secret123")
print(hashed)  # $2b$12$...

print(verify_password("secret123", hashed))  # True
print(verify_password("wrong", hashed))  # False
```

## Creating JWT Tokens

```python
from datetime import datetime, timedelta
from jose import jwt
from typing import Any

SECRET_KEY = "your-secret-key-change-this-in-production"
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_MINUTES = 30

def create_access_token(data: dict[str, Any], expires_delta: timedelta | None = None) -> str:
    """Create a JWT access token."""
    to_encode = data.copy()
    
    if expires_delta:
        expire = datetime.utcnow() + expires_delta
    else:
        expire = datetime.utcnow() + timedelta(minutes=15)
    
    to_encode.update({"exp": expire})
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)
    return encoded_jwt

def decode_token(token: str) -> dict[str, Any]:
    """Decode and verify a JWT token."""
    return jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
```

## Complete Authentication Example

```python
from fastapi import FastAPI, Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
from pydantic import BaseModel
from passlib.context import CryptContext
from jose import jwt, JWTError
from datetime import datetime, timedelta
from typing import Any, Optional

app = FastAPI()

# Security
SECRET_KEY = "your-secret-key-change-this-in-production"
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_MINUTES = 30

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

# Models
class User(BaseModel):
    username: str
    email: str | None = None
    full_name: str | None = None
    disabled: bool = False

class UserInDB(User):
    hashed_password: str

class Token(BaseModel):
    access_token: str
    token_type: str

# Fake database
fake_users_db = {
    "johndoe": {
        "username": "johndoe",
        "full_name": "John Doe",
        "email": "johndoe@example.com",
        "hashed_password": pwd_context.hash("secret123"),
        "disabled": False,
    }
}

# Helper functions
def verify_password(plain_password: str, hashed_password: str) -> bool:
    return pwd_context.verify(plain_password, hashed_password)

def get_user(db: dict, username: str) -> Optional[UserInDB]:
    if username in db:
        user_dict = db[username]
        return UserInDB(**user_dict)
    return None

def authenticate_user(fake_db: dict, username: str, password: str) -> Optional[UserInDB]:
    user = get_user(fake_db, username)
    if not user:
        return None
    if not verify_password(password, user.hashed_password):
        return None
    return user

def create_access_token(data: dict[str, Any], expires_delta: timedelta | None = None) -> str:
    to_encode = data.copy()
    if expires_delta:
        expire = datetime.utcnow() + expires_delta
    else:
        expire = datetime.utcnow() + timedelta(minutes=15)
    to_encode.update({"exp": expire})
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)
    return encoded_jwt

# Dependency to get current user
async def get_current_user(token: str = Depends(oauth2_scheme)) -> User:
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": "Bearer"},
    )
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        username: str = payload.get("sub")
        if username is None:
            raise credentials_exception
    except JWTError:
        raise credentials_exception
    
    user = get_user(fake_users_db, username)
    if user is None:
        raise credentials_exception
    return User(
        username=user.username,
        email=user.email,
        full_name=user.full_name,
        disabled=user.disabled
    )

# Routes
@app.post("/token", response_model=Token)
async def login(form_data: OAuth2PasswordRequestForm = Depends()) -> dict[str, str]:
    """Login endpoint - returns JWT token."""
    user = authenticate_user(fake_users_db, form_data.username, form_data.password)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Incorrect username or password",
            headers={"WWW-Authenticate": "Bearer"},
        )
    
    access_token_expires = timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)
    access_token = create_access_token(
        data={"sub": user.username}, expires_delta=access_token_expires
    )
    return {"access_token": access_token, "token_type": "bearer"}

@app.get("/users/me", response_model=User)
async def read_users_me(current_user: User = Depends(get_current_user)) -> User:
    """Get current user info - requires authentication."""
    return current_user

@app.get("/items")
async def read_items() -> list[dict]:
    """Public endpoint - no auth required."""
    return [{"item": "Apple"}, {"item": "Banana"}]

@app.get("/protected-items")
async def read_protected_items(current_user: User = Depends(get_current_user)) -> list[dict]:
    """Protected endpoint - requires valid JWT."""
    return [
        {"item": "Apple", "owner": current_user.username},
        {"item": "Banana", "owner": current_user.username}
    ]
```

🔍 **Authentication Flow:**

1. User POSTs to `/token` with username and password
2. Server validates credentials and creates JWT token
3. Token is returned to client
4. Client includes token in Authorization header: `Bearer <token>`
5. `get_current_user` dependency extracts and verifies token
6. Protected routes use `Depends(get_current_user)`

## Testing the Auth

```bash
# 1. Get token
curl -X POST "http://localhost:8000/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "username=johndoe&password=secret123"

# Response:
# {"access_token": "eyJ...", "token_type": "bearer"}

# 2. Access protected route
curl -X GET "http://localhost:8000/protected-items" \
  -H "Authorization: Bearer eyJ..."
```

## Refreshing Tokens

```python
@app.post("/refresh-token", response_model=Token)
async def refresh_token(current_user: User = Depends(get_current_user)) -> dict[str, str]:
    """Get a new access token."""
    access_token_expires = timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)
    access_token = create_access_token(
        data={"sub": current_user.username}, 
        expires_delta=access_token_expires
    )
    return {"access_token": access_token, "token_type": "bearer"}
```

## Summary
- **JWT** is a compact way to transmit authentication data
- Use **password hashing** (bcrypt) to store passwords securely
- Create tokens with `jwt.encode()` and decode with `jwt.decode()`
- Use **OAuth2PasswordBearer** to extract tokens from requests
- Create a **dependency** (`get_current_user`) to protect routes
- Protected routes use `Depends(get_current_user)`

## Next Steps
→ Continue to `06-async-await-in-fastapi.md` to learn about asynchronous programming in FastAPI.
