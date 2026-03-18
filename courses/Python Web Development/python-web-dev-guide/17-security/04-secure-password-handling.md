# Secure Password Handling

## What You'll Learn
- Password hashing algorithms
- Using bcrypt and Argon2
- Password validation
- Implementing secure registration

## Prerequisites
- Completed SQL injection prevention

## Password Hashing

Never store passwords in plain text! Always hash them.

```bash
pip install bcrypt passlib
```

## Using bcrypt

```python
import bcrypt

def hash_password(password: str) -> str:
    """Hash a password using bcrypt"""
    # Generate salt and hash
    salt = bcrypt.gensalt(rounds=12)  # Higher = more secure, slower
    hashed = bcrypt.hashpw(password.encode('utf-8'), salt)
    return hashed.decode('utf-8')

def verify_password(password: str, hashed: str) -> bool:
    """Verify password against hash"""
    return bcrypt.checkpw(
        password.encode('utf-8'),
        hashed.encode('utf-8')
    )

# Usage
hashed = hash_password("my_secure_password")
print(f"Hash: {hashed}")  # $2b$12$...

is_valid = verify_password("my_secure_password", hashed)
print(f"Valid: {is_valid}")  # True
```

🔍 **Line-by-Line Breakdown:**
1. `bcrypt.gensalt(rounds=12)` — Generate salt with work factor 12
2. `bcrypt.hashpw()` — Hash password with salt
3. `bcrypt.checkpw()` — Constant-time comparison (prevents timing attacks)

## Using Passlib

```python
from passlib.context import CryptContext

# Configure password hashing
pwd_context = CryptContext(
    schemes=["bcrypt", "argon2"],
    default="bcrypt",
    deprecated="auto"
)

def hash_password(password: str) -> str:
    """Hash password using configured algorithm"""
    return pwd_context.hash(password)

def verify_password(plain_password: str, hashed_password: str) -> bool:
    """Verify password against hash"""
    return pwd_context.verify(plain_password, hashed_password)
```

## Password Validation

```python
import re
from typing import Optional

def validate_password(password: str) -> tuple[bool, Optional[str]]:
    """
    Validate password strength
    Returns (is_valid, error_message)
    """
    if len(password) < 8:
        return False, "Password must be at least 8 characters"
    
    if not re.search(r"[A-Z]", password):
        return False, "Password must contain at least one uppercase letter"
    
    if not re.search(r"[a-z]", password):
        return False, "Password must contain at least one lowercase letter"
    
    if not re.search(r"[0-9]", password):
        return False, "Password must contain at least one digit"
    
    if not re.search(r"[!@#$%^&*(),.?\":{}|<>]", password):
        return False, "Password must contain at least one special character"
    
    return True, None

# Usage
is_valid, error = validate_password("MySecurePass123!")
if is_valid:
    print("Password is valid")
else:
    print(f"Error: {error}")
```

## Secure Registration

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, validator
from sqlalchemy.orm import Session
import bcrypt

app = FastAPI()

class UserCreate(BaseModel):
    username: str
    email: str
    password: str
    
    @validator('password')
    def validate_password_strength(cls, v):
        is_valid, error = validate_password(v)
        if not is_valid:
            raise ValueError(error)
        return v

def hash_password(password: str) -> str:
    salt = bcrypt.gensalt(rounds=12)
    return bcrypt.hashpw(password.encode('utf-8'), salt).decode('utf-8')

@app.post("/register")
async def register(user: UserCreate, db: Session):
    # Check if user exists
    existing = db.query(User).filter(
        (User.username == user.username) | (User.email == user.email)
    ).first()
    
    if existing:
        raise HTTPException(status_code=400, detail="User already exists")
    
    # Hash password
    hashed = hash_password(user.password)
    
    # Create user
    new_user = User(
        username=user.username,
        email=user.email,
        password_hash=hashed
    )
    db.add(new_user)
    db.commit()
    
    return {"message": "User created", "username": user.username}
```

## Summary
- Never store plain text passwords
- Use bcrypt or Argon2 for hashing
- Implement password strength validation
- Use constant-time comparison for verification

## Next Steps
→ Continue to `05-input-validation-and-sanitization.md`
