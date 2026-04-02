# User Authentication

## Overview

Complete user authentication flow including registration, login, password management, and session handling.

## Implementation

### Complete Auth System

```python
# Example 1: Complete user authentication
from fastapi import FastAPI, HTTPException, Depends
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
from passlib.context import CryptContext
from jose import jwt, JWTError
from datetime import datetime, timedelta
from pydantic import BaseModel, EmailStr, Field

app = FastAPI()

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

SECRET_KEY = "your-secret-key"

class UserCreate(BaseModel):
    username: str = Field(..., min_length=3, max_length=50)
    email: EmailStr
    password: str = Field(..., min_length=12)

class UserLogin(BaseModel):
    username: str
    password: str

# User database simulation
users_db: dict[str, dict] = {}

@app.post("/register")
async def register(user: UserCreate):
    """Register new user"""
    if user.username in users_db:
        raise HTTPException(400, "Username already exists")
    
    hashed_password = pwd_context.hash(user.password)
    
    users_db[user.username] = {
        "username": user.username,
        "email": user.email,
        "hashed_password": hashed_password,
        "created_at": datetime.utcnow().isoformat()
    }
    
    return {"message": "User registered successfully"}

@app.post("/token")
async def login(form_data: OAuth2PasswordRequestForm = Depends()):
    """Login and get access token"""
    user = users_db.get(form_data.username)
    
    if not user or not pwd_context.verify(form_data.password, user["hashed_password"]):
        raise HTTPException(401, "Invalid credentials")
    
    token = jwt.encode(
        {
            "sub": form_data.username,
            "exp": datetime.utcnow() + timedelta(minutes=30)
        },
        SECRET_KEY,
        algorithm="HS256"
    )
    
    return {"access_token": token, "token_type": "bearer"}

async def get_current_user(token: str = Depends(oauth2_scheme)):
    """Get current authenticated user"""
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=["HS256"])
        username = payload.get("sub")
        if username not in users_db:
            raise HTTPException(401, "Invalid token")
        return users_db[username]
    except JWTError:
        raise HTTPException(401, "Invalid token")

@app.get("/me")
async def get_profile(user: dict = Depends(get_current_user)):
    """Get current user profile"""
    return {
        "username": user["username"],
        "email": user["email"]
    }
```

## Summary

This provides a complete authentication flow with registration, login, and protected endpoints.

## Next Steps

Continue learning about:
- [Role Based Access](./05_role_based_access.md)
- [Token Expiration](./06_token_expiration_management.md)
