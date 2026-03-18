# Password Hashing

## What You'll Learn
- Why hashing matters
- Using bcrypt
- Best practices

## Prerequisites
- Basic Python

## Why Hashing?

Never store passwords in plain text! Use hashing with salt.

```python
from passlib.context import CryptContext

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")

def hash_password(password: str) -> str:
    return pwd_context.hash(password)

def verify_password(plain_password: str, hashed_password: str) -> bool:
    return pwd_context.verify(plain_password, hashed_password)

# Usage
hashed = hash_password("mypassword")
print(verify_password("mypassword", hashed))  # True
print(verify_password("wrong", hashed))  # False
```

## Summary
- Use bcrypt for password hashing
- Always verify, never compare directly
- Never store plain text passwords
