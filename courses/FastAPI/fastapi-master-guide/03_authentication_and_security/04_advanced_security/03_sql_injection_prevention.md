# SQL Injection Prevention

## Overview

SQL injection is one of the most dangerous web vulnerabilities. FastAPI with proper ORM usage provides excellent protection, but understanding the risks is essential.

## Understanding SQL Injection

### Vulnerability Example

```python
# Example 1: VULNERABLE CODE - DO NOT USE
"""
# This code is VULNERABLE to SQL injection!
def get_user_vulnerable(username: str):
    query = f"SELECT * FROM users WHERE username = '{username}'"
    # If username = "admin' OR '1'='1", query becomes:
    # SELECT * FROM users WHERE username = 'admin' OR '1'='1'
    # Returns ALL users!
    return db.execute(query)
"""

# Example 2: SAFE CODE - Use parameterized queries
from fastapi import FastAPI, HTTPException
from sqlalchemy import text
from sqlalchemy.orm import Session

app = FastAPI()

def get_user_safe(db: Session, username: str):
    """
    SAFE: Using parameterized query.
    The :username parameter is properly escaped.
    """
    query = text("SELECT * FROM users WHERE username = :username")
    result = db.execute(query, {"username": username})
    return result.fetchone()

# Even with malicious input, it's treated as literal string
# username = "admin' OR '1'='1"
# Query becomes: WHERE username = 'admin'' OR ''1''=''1'
# No SQL injection possible!
```

## SQLAlchemy Protection

### Safe Query Patterns

```python
# Example 3: SQLAlchemy ORM (automatically safe)
from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy import create_engine, Column, Integer, String
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker, Session

app = FastAPI()

Base = declarative_base()

class User(Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True)
    username = Column(String, unique=True)
    email = Column(String)

# SAFE: Using SQLAlchemy ORM
def get_user_orm(db: Session, username: str) -> User:
    """
    SQLAlchemy ORM automatically escapes parameters.
    This is ALWAYS safe from SQL injection.
    """
    return db.query(User).filter(User.username == username).first()

def search_users_orm(db: Session, search_term: str):
    """
    Safe search with ORM.
    """
    return db.query(User).filter(
        User.username.ilike(f"%{search_term}%")
    ).all()

# SAFE: Using SQLAlchemy Core with parameters
from sqlalchemy import select

def get_user_core(db: Session, username: str):
    """
    SQLAlchemy Core with bound parameters.
    """
    stmt = select(User).where(User.username == username)
    return db.execute(stmt).scalar_one_or_none()
```

### Safe Dynamic Queries

```python
# Example 4: Safe dynamic query building
from fastapi import FastAPI, Query
from sqlalchemy import and_, or_
from typing import Optional, List

app = FastAPI()

def build_safe_filters(
    db: Session,
    username: Optional[str] = None,
    email: Optional[str] = None,
    min_id: Optional[int] = None,
    max_id: Optional[int] = None
):
    """
    Build dynamic filters safely using SQLAlchemy.
    All parameters are automatically escaped.
    """
    query = db.query(User)

    filters = []

    if username:
        filters.append(User.username == username)

    if email:
        filters.append(User.email == email)

    if min_id is not None:
        filters.append(User.id >= min_id)

    if max_id is not None:
        filters.append(User.id <= max_id)

    if filters:
        query = query.filter(and_(*filters))

    return query.all()

@app.get("/users/")
async def list_users(
    username: Optional[str] = None,
    email: Optional[str] = None
):
    """Safe dynamic filtering"""
    return build_safe_filters(db, username, email)
```

## Raw SQL Safety

### Parameterized Queries

```python
# Example 5: Safe raw SQL with SQLAlchemy
from sqlalchemy import text
from fastapi import FastAPI

app = FastAPI()

def safe_raw_query(db: Session, user_id: int):
    """
    SAFE: Using text() with parameters.
    """
    # Named parameters
    query = text("SELECT * FROM users WHERE id = :id")
    result = db.execute(query, {"id": user_id})
    return result.fetchone()

def safe_complex_query(db: Session, min_age: int, max_age: int):
    """
    SAFE: Multiple parameters.
    """
    query = text("""
        SELECT * FROM users
        WHERE age >= :min_age AND age <= :max_age
        ORDER BY created_at DESC
        LIMIT :limit
    """)

    result = db.execute(query, {
        "min_age": min_age,
        "max_age": max_age,
        "limit": 100
    })

    return result.fetchall()

# NEVER do this:
# query = text(f"SELECT * FROM users WHERE id = {user_id}")  # VULNERABLE!
```

## Input Validation

### Pydantic Validation

```python
# Example 6: Input validation with Pydantic
from fastapi import FastAPI
from pydantic import BaseModel, Field, validator
import re

app = FastAPI()

class SearchQuery(BaseModel):
    """Validated search query"""
    query: str = Field(..., min_length=1, max_length=100)
    limit: int = Field(10, ge=1, le=100)

    @validator('query')
    def validate_query(cls, v):
        """Prevent SQL injection patterns"""
        # Remove potentially dangerous characters
        dangerous_patterns = [
            r"('|--|;|/\*|\*/|xp_|exec|execute|select|insert|update|delete|drop)",
        ]

        for pattern in dangerous_patterns:
            if re.search(pattern, v, re.IGNORECASE):
                raise ValueError("Invalid characters in search query")

        return v

@app.get("/search/")
async def search(data: SearchQuery = Depends()):
    """
    Validated input prevents injection.
    """
    # Input is now validated and safe
    return safe_search(db, data.query, data.limit)
```

## Best Practices

### Security Guidelines

```python
# Example 7: SQL injection prevention checklist
"""
SQL Injection Prevention Checklist:

1. ✓ Use ORM (SQLAlchemy) - Automatic protection
2. ✓ Use parameterized queries for raw SQL
3. ✓ Validate all user input with Pydantic
4. ✓ Never use string formatting for SQL
5. ✓ Use least privilege database accounts
6. ✓ Escape special characters when needed
7. ✓ Use stored procedures (with parameters)
8. ✓ Enable SQL query logging for auditing
9. ✓ Regular security audits
10. ✓ Keep database software updated
"""

from fastapi import FastAPI
from sqlalchemy.orm import Session

app = FastAPI()

# 1. Always use ORM when possible
def get_user_safe_orm(db: Session, username: str):
    """SAFE: Using ORM"""
    return db.query(User).filter(User.username == username).first()

# 2. If raw SQL needed, use parameters
def get_user_safe_raw(db: Session, username: str):
    """SAFE: Parameterized query"""
    return db.execute(
        text("SELECT * FROM users WHERE username = :username"),
        {"username": username}
    ).fetchone()

# 3. NEVER do string formatting
def get_user_DANGEROUS(db: Session, username: str):
    """
    NEVER DO THIS - VULNERABLE TO SQL INJECTION!
    """
    # query = f"SELECT * FROM users WHERE username = '{username}'"
    # db.execute(query)
    pass

# 4. Use stored procedures safely
def call_stored_procedure(db: Session, user_id: int):
    """Call stored procedure with parameters"""
    return db.execute(
        text("CALL get_user_details(:user_id)"),
        {"user_id": user_id}
    ).fetchall()
```

## Detection and Monitoring

### Query Logging

```python
# Example 8: SQL query monitoring
import logging
from sqlalchemy import event

# Enable SQL logging
logging.basicConfig()
logging.getLogger('sqlalchemy.engine').setLevel(logging.INFO)

@event.listens_for(Session, "before_cursor_execute")
def log_query(conn, cursor, statement, parameters, context, executemany):
    """Log all SQL queries for auditing"""
    logging.info(f"SQL: {statement}")
    logging.info(f"Params: {parameters}")

    # Detect potential injection attempts
    suspicious_patterns = ["OR 1=1", "DROP TABLE", "--", "/*"]
    for pattern in suspicious_patterns:
        if pattern.lower() in statement.lower():
            logging.warning(f"Suspicious SQL detected: {statement}")
```

## Summary

| Protection | Implementation | Safety Level |
|------------|----------------|--------------|
| ORM (SQLAlchemy) | `db.query(User).filter(...)` | Highest |
| Parameterized queries | `text("... :param")` | High |
| Input validation | Pydantic validators | High |
| String formatting | `f"SELECT..."` | NEVER USE |

## Next Steps

Continue learning about:
- [XSS Protection](./04_xss_protection.md) - Cross-site scripting
- [CSRF Protection](./05_csrf_protection.md) - Cross-site request forgery
- [Input Validation](./02_input_validation_security.md) - Input sanitization
