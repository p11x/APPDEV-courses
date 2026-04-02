# Password Hashing

## Overview

Secure password hashing is critical for protecting user credentials. FastAPI applications should use industry-standard hashing algorithms like bcrypt or Argon2.

## Hashing Algorithms

### bcrypt (Recommended)

```python
# Example 1: bcrypt password hashing
from fastapi import FastAPI, HTTPException
from passlib.context import CryptContext
from pydantic import BaseModel

app = FastAPI()

# Configure password context
pwd_context = CryptContext(
    schemes=["bcrypt"],
    deprecated="auto",
    bcrypt__rounds=12  # Cost factor (higher = slower but more secure)
)

class UserCreate(BaseModel):
    username: str
    password: str

def hash_password(password: str) -> str:
    """
    Hash password using bcrypt.
    Never store plain text passwords!
    """
    return pwd_context.hash(password)

def verify_password(plain_password: str, hashed_password: str) -> bool:
    """
    Verify password against hash.
    Returns True if password matches.
    """
    return pwd_context.verify(plain_password, hashed_password)

@app.post("/register")
async def register(user: UserCreate):
    """
    Register user with hashed password.
    """
    # Hash password before storing
    hashed = hash_password(user.password)

    # Store in database
    # db_user = {"username": user.username, "password_hash": hashed}

    return {
        "username": user.username,
        "message": "User registered (password hashed)"
    }

@app.post("/login")
async def login(username: str, password: str):
    """
    Login by verifying password hash.
    """
    # Get user from database
    # user = db.get_user(username)
    stored_hash = "$2b$12$EixZaYVK1fsbw1ZfbX3OXePaWxn96p36WQoeG6Lruj3vjPGga31lW"

    if not verify_password(password, stored_hash):
        raise HTTPException(401, "Invalid credentials")

    return {"message": "Login successful"}
```

### Argon2 (Most Secure)

```python
# Example 2: Argon2 password hashing
from passlib.context import CryptContext

# Argon2 is the most secure option (winner of Password Hashing Competition)
pwd_context = CryptContext(
    schemes=["argon2"],
    deprecated="auto",
    argon2__memory_cost=65536,  # 64MB
    argon2__time_cost=3,        # 3 iterations
    argon2__parallelism=4       # 4 threads
)

def hash_password_argon2(password: str) -> str:
    """
    Hash password using Argon2.
    More resistant to GPU attacks than bcrypt.
    """
    return pwd_context.hash(password)

def verify_password_argon2(plain: str, hashed: str) -> bool:
    """Verify Argon2 hash"""
    return pwd_context.verify(plain, hashed)
```

### PBKDF2

```python
# Example 3: PBKDF2 password hashing
import hashlib
import secrets

def hash_password_pbkdf2(password: str) -> str:
    """
    Hash password using PBKDF2.
    Widely supported, good security.
    """
    salt = secrets.token_hex(16)
    iterations = 600000  # OWASP recommended

    key = hashlib.pbkdf2_hmac(
        'sha256',
        password.encode('utf-8'),
        salt.encode('utf-8'),
        iterations
    )

    return f"{salt}:{iterations}:{key.hex()}"

def verify_password_pbkdf2(password: str, stored_hash: str) -> bool:
    """Verify PBKDF2 hash"""
    salt, iterations, key_hex = stored_hash.split(':')

    key = hashlib.pbkdf2_hmac(
        'sha256',
        password.encode('utf-8'),
        salt.encode('utf-8'),
        int(iterations)
    )

    return key.hex() == key_hex
```

## Password Validation

### Strength Requirements

```python
# Example 4: Password strength validation
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field, validator
import re

app = FastAPI()

class PasswordValidator(BaseModel):
    password: str = Field(..., min_length=12, max_length=128)

    @validator('password')
    def validate_password_strength(cls, v):
        """
        Enforce password requirements:
        - At least 12 characters
        - At least one uppercase letter
        - At least one lowercase letter
        - At least one digit
        - At least one special character
        """
        errors = []

        if len(v) < 12:
            errors.append("Must be at least 12 characters")

        if not re.search(r'[A-Z]', v):
            errors.append("Must contain at least one uppercase letter")

        if not re.search(r'[a-z]', v):
            errors.append("Must contain at least one lowercase letter")

        if not re.search(r'\d', v):
            errors.append("Must contain at least one digit")

        if not re.search(r'[!@#$%^&*(),.?":{}|<>]', v):
            errors.append("Must contain at least one special character")

        # Check for common passwords
        common_passwords = ["password123!", "Password123!", "Admin123!"]
        if v.lower() in [p.lower() for p in common_passwords]:
            errors.append("Password is too common")

        if errors:
            raise ValueError("; ".join(errors))

        return v

@app.post("/register")
async def register(validator: PasswordValidator):
    """Register with validated password"""
    return {"message": "Password meets requirements"}
```

## Migration Strategies

### Upgrading Hash Algorithms

```python
# Example 5: Hash algorithm migration
from passlib.context import CryptContext
from passlib.hash import bcrypt, pbkdf2_sha256

# Support both old and new algorithms
pwd_context = CryptContext(
    schemes=["argon2", "bcrypt", "pbkdf2_sha256"],
    default="argon2",  # New passwords use this
    deprecated=["bcrypt", "pbkdf2_sha256"]  # Old schemes to migrate from
)

def verify_and_upgrade_password(
    plain_password: str,
    stored_hash: str
) -> tuple[bool, str | None]:
    """
    Verify password and upgrade hash if needed.
    Returns (is_valid, new_hash_or_none)
    """
    # Verify password
    is_valid = pwd_context.verify(plain_password, stored_hash)

    if is_valid:
        # Check if hash needs upgrading
        if pwd_context.needs_update(stored_hash):
            # Rehash with new algorithm
            new_hash = pwd_context.hash(plain_password)
            return True, new_hash

    return is_valid, None

@app.post("/login")
async def login(username: str, password: str):
    """Login with automatic hash upgrade"""
    user = get_user(username)
    if not user:
        raise HTTPException(401, "Invalid credentials")

    is_valid, new_hash = verify_and_upgrade_password(
        password,
        user.password_hash
    )

    if not is_valid:
        raise HTTPException(401, "Invalid credentials")

    # Update hash if upgraded
    if new_hash:
        update_user_password_hash(username, new_hash)

    return {"message": "Login successful"}
```

## Best Practices

### Security Guidelines

```python
# Example 6: Password security best practices
from fastapi import FastAPI
from passlib.context import CryptContext

app = FastAPI()

# 1. Use strong hashing algorithm
pwd_context = CryptContext(schemes=["argon2"], deprecated="auto")

# 2. Never log passwords
@app.post("/register")
async def register(username: str, password: str):
    # NEVER do this:
    # logger.info(f"Registering user {username} with password {password}")

    # DO this:
    hashed = pwd_context.hash(password)
    # logger.info(f"Registering user {username}")

    return {"message": "Registered"}

# 3. Use timing-safe comparison (passlib does this automatically)

# 4. Implement password history
class PasswordHistory:
    def __init__(self, max_history: int = 5):
        self.max_history = max_history
        self.history: dict[str, list[str]] = {}

    def add_password(self, user_id: str, password_hash: str):
        """Add password to history"""
        if user_id not in self.history:
            self.history[user_id] = []

        self.history[user_id].append(password_hash)

        # Keep only recent passwords
        if len(self.history[user_id]) > self.max_history:
            self.history[user_id].pop(0)

    def is_password_reused(self, user_id: str, new_password: str) -> bool:
        """Check if password was used before"""
        if user_id not in self.history:
            return False

        for old_hash in self.history[user_id]:
            if pwd_context.verify(new_password, old_hash):
                return True

        return False

# 5. Implement account lockout
LOGIN_ATTEMPTS: dict[str, int] = {}
MAX_ATTEMPTS = 5

def check_and_record_attempt(username: str, success: bool):
    """Track login attempts"""
    if success:
        LOGIN_ATTEMPTS.pop(username, None)
        return True

    attempts = LOGIN_ATTEMPTS.get(username, 0) + 1
    LOGIN_ATTEMPTS[username] = attempts

    if attempts >= MAX_ATTEMPTS:
        raise HTTPException(
            status_code=429,
            detail="Account locked due to too many failed attempts"
        )

    return False
```

## Summary

| Algorithm | Security | Speed | Recommendation |
|-----------|----------|-------|----------------|
| Argon2 | Highest | Slow | Best for new apps |
| bcrypt | High | Medium | Industry standard |
| PBKDF2 | Good | Medium | Fallback option |
| SHA-256 | Low | Fast | Never for passwords |

## Next Steps

Continue learning about:
- [User Authentication](./04_user_authentication.md) - Full auth flow
- [Role Based Access](./05_role_based_access.md) - Authorization
- [Token Expiration](./06_token_expiration_management.md) - Token lifecycle
