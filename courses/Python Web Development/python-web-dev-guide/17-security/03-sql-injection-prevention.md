# SQL Injection Prevention

## What You'll Learn
- Understanding SQL injection attacks
- Using parameterized queries
- ORM protection
- Input validation for databases

## Prerequisites
- Completed XSS and CSRF protection

## What Is SQL Injection?

SQL injection manipulates database queries by injecting malicious SQL code through user input.

## Vulnerable Code

```python
# VULNERABLE - Never do this!
@app.get("/user")
def get_user(username: str):
    # Directly inserting user input into SQL
    query = f"SELECT * FROM users WHERE username = '{username}'"
    result = db.execute(query)
    return result.fetchall()
    
# If username = "admin' OR '1'='1"
# Query becomes: SELECT * FROM users WHERE username = 'admin' OR '1'='1'
# This returns ALL users!
```

## Parameterized Queries

```python
import sqlite3

@app.get("/user")
def get_user(username: str):
    # SAFE - Using parameterized query
    conn = sqlite3.connect("app.db")
    cursor = conn.cursor()
    
    # ? placeholder is escaped automatically
    cursor.execute(
        "SELECT id, username, email FROM users WHERE username = ?",
        (username,)
    )
    
    result = cursor.fetchone()
    conn.close()
    
    if result:
        return {"id": result[0], "username": result[1], "email": result[2]}
    return {"error": "User not found"}
```

🔍 **Line-by-Line Breakdown:**
1. `"SELECT ... WHERE username = ?"` — Uses placeholder `?` instead of f-string
2. `(username,)` — Pass values as tuple, not in the query string
3. Database driver handles escaping automatically

## SQLAlchemy Protection

```python
from sqlalchemy import select
from sqlalchemy.orm import Session

# SAFE - SQLAlchemy uses parameterized queries
@app.get("/users/{user_id}")
async def get_user(user_id: int, db: Session):
    # SQLAlchemy automatically parameterizes
    result = db.execute(
        select(User).where(User.id == user_id)
    )
    user = result.scalar_one_or_none()
    
    if user:
        return {"id": user.id, "username": user.username}
    return {"error": "Not found"}

# Using filters - always safe
@app.get("/users")
async def search_users(query: str, db: Session):
    # SAFE - SQLAlchemy parameterizes automatically
    result = db.execute(
        select(User).where(User.username.like(f"%{query}%"))
    )
    return [{"id": u.id, "username": u.username} for u in result]
```

## Raw SQL with Parameters

```python
from sqlalchemy import text

@app.get("/products")
async def get_products(category: str, min_price: float, db: Session):
    # SAFE - Using text() with parameters
    result = db.execute(
        text("SELECT * FROM products WHERE category = :cat AND price >= :price"),
        {"cat": category, "price": min_price}
    )
    
    return [dict(row._mapping) for row in result]
```

## Common Mistakes to Avoid

```python
# WRONG - String formatting in SQL
query = f"SELECT * FROM users WHERE {column} = '{value}'"

# WRONG - Using format()
query = "SELECT * FROM users WHERE id = {}".format(user_input)

# WRONG - Using %s with string interpolation
query = "SELECT * FROM users WHERE name = '%s'" % name

# CORRECT - Parameterized
query = "SELECT * FROM users WHERE name = ?"
cursor.execute(query, (name,))

# CORRECT - SQLAlchemy
result = db.query(User).filter(User.name == name)
```

## Summary
- Never use f-strings or format() in SQL
- Always use parameterized queries
- Use ORM (SQLAlchemy) which protects by default
- Validate and whitelist input for dynamic column names

## Next Steps
→ Continue to `04-secure-password-handling.md`
