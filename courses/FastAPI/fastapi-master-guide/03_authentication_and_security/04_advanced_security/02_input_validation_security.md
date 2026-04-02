# Input Validation Security

## Overview

Input validation is the first line of defense against many attacks. FastAPI with Pydantic provides excellent built-in validation.

## Pydantic Validation

### Comprehensive Validation

```python
# Example 1: Secure input validation
from fastapi import FastAPI
from pydantic import BaseModel, Field, validator, EmailStr
from typing import Optional
import re

app = FastAPI()

class SecureUserInput(BaseModel):
    """Comprehensive input validation"""
    
    username: str = Field(
        ...,
        min_length=3,
        max_length=50,
        regex=r'^[a-zA-Z0-9_]+$'
    )
    
    email: EmailStr
    
    password: str = Field(..., min_length=12, max_length=128)
    
    age: Optional[int] = Field(None, ge=13, le=120)
    
    @validator('password')
    def validate_password(cls, v):
        """Strong password requirements"""
        if not re.search(r'[A-Z]', v):
            raise ValueError('Must contain uppercase letter')
        if not re.search(r'[a-z]', v):
            raise ValueError('Must contain lowercase letter')
        if not re.search(r'\d', v):
            raise ValueError('Must contain digit')
        if not re.search(r'[!@#$%^&*]', v):
            raise ValueError('Must contain special character')
        return v
    
    @validator('username')
    def sanitize_username(cls, v):
        """Additional username validation"""
        if v.lower() in ['admin', 'root', 'system']:
            raise ValueError('Reserved username')
        return v.lower()

@app.post("/users/")
async def create_user(user: SecureUserInput):
    """Create user with validated input"""
    return {"username": user.username, "email": user.email}
```

## Best Practices

1. Validate all input at the boundary
2. Use Pydantic for type checking
3. Sanitize strings to prevent injection
4. Validate file uploads
5. Set appropriate field limits

## Summary

FastAPI's Pydantic integration provides robust input validation out of the box.

## Next Steps

Continue learning about:
- [SQL Injection Prevention](./03_sql_injection_prevention.md)
- [XSS Protection](./04_xss_protection.md)
