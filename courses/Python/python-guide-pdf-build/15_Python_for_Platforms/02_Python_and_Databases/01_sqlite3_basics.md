# SQLite3 Basics

## What You'll Learn

- Connecting to SQLite database
- Creating tables
- CRUD operations
- Using context managers

## Prerequisites

- Read [08_fastapi_basics.md](./08_fastapi_basics.md) first

## Connecting and Creating Tables

```python
# sqlite3_basics.py

import sqlite3

# Connect to database (creates if not exists)
conn = sqlite3.connect("example.db")
cursor = conn.cursor()

# Create table
cursor.execute("""
    CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        email TEXT UNIQUE NOT NULL
    )
""")

conn.commit()
conn.close()
```

## CRUD Operations

```python
# crud_operations.py

import sqlite3


def create_user(name: str, email: str) -> int:
    conn = sqlite3.connect("example.db")
    cursor = conn.cursor()
    cursor.execute("INSERT INTO users (name, email) VALUES (?, ?)", (name, email))
    conn.commit()
    user_id = cursor.lastrowid
    conn.close()
    return user_id


def get_user(user_id: int) -> tuple:
    conn = sqlite3.connect("example.db")
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM users WHERE id = ?", (user_id,))
    user = cursor.fetchone()
    conn.close()
    return user


def get_all_users() -> list:
    conn = sqlite3.connect("example.db")
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM users")
    users = cursor.fetchall()
    conn.close()
    return users
```

## Annotated Full Example

```python
# sqlite3_demo.py
"""Complete SQLite database operations."""

import sqlite3
from typing import Optional


def init_db() -> None:
    """Initialize database with schema."""
    conn = sqlite3.connect("demo.db")
    cursor = conn.cursor()
    
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS products (
            id INTEGER PRIMARY KEY,
            name TEXT NOT NULL,
            price REAL NOT NULL
        )
    """)
    
    conn.commit()
    conn.close()


def add_product(name: str, price: float) -> int:
    conn = sqlite3.connect("demo.db")
    cursor = conn.cursor()
    cursor.execute("INSERT INTO products (name, price) VALUES (?, ?)", (name, price))
    conn.commit()
    conn.close()


def get_products() -> list:
    conn = sqlite3.connect("demo.db")
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM products")
    products = cursor.fetchall()
    conn.close()
    return products


def main() -> None:
    init_db()
    add_product("Widget", 9.99)
    add_product("Gadget", 19.99)
    
    for product in get_products():
        print(product)


if __name__ == "__main__":
    main()
```

## Summary

- Connecting to SQLite database
- Creating tables
- CRUD operations

## Next Steps

Continue to **[02_sqlalchemy_core.md](./02_sqlalchemy_core.md)**
