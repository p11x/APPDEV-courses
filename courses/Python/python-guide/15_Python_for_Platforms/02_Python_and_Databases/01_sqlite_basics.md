# 💾 SQLite Basics: Zero-Setup Database

## 🎯 What You'll Learn

- Creating and connecting to SQLite databases
- CRUD operations with SQL
- Parameterized queries (preventing SQL injection)
- Using transactions

---

## Getting Started

```python
import sqlite3
from pathlib import Path

# Create connection (creates file if doesn't exist)
db_path = Path("myapp.db")
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# Create table
cursor.execute("""
    CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        email TEXT UNIQUE NOT NULL,
        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    )
""")

conn.commit()
conn.close()
```

---

## CRUD Operations

```python
import sqlite3

conn = sqlite3.connect("myapp.db")
cursor = conn.cursor()

# CREATE
cursor.execute(
    "INSERT INTO users (name, email) VALUES (?, ?)",
    ("Alice", "alice@example.com")
)
user_id = cursor.lastrowid  # Get the ID
conn.commit()

# READ
cursor.execute("SELECT * FROM users WHERE id = ?", (user_id,))
user = cursor.fetchone()  # Single row
# user = cursor.fetchall()  # All rows

# UPDATE
cursor.execute(
    "UPDATE users SET name = ? WHERE id = ?",
    ("Alice Smith", user_id)
)
conn.commit()

# DELETE
cursor.execute("DELETE FROM users WHERE id = ?", (user_id,))
conn.commit()

conn.close()
```

---

## ⚠️ SQL Injection Prevention

```python
# ❌ NEVER do this - SQL injection vulnerability!
name = "Alice"
query = f"SELECT * FROM users WHERE name = '{name}'"

# ✅ Always use parameterized queries!
cursor.execute(
    "SELECT * FROM users WHERE name = ?",
    (name,)  # Tuple with parameters
)
```

---

## Using Row Factory

```python
# Get dict-like access instead of tuples
conn = sqlite3.connect("myapp.db")
conn.row_factory = sqlite3.Row

cursor = conn.cursor()
cursor.execute("SELECT * FROM users")

for row in cursor:
    print(row["name"])  # Access by column name!
    print(row["email"])
```

---

## Context Manager

```python
with sqlite3.connect("myapp.db") as conn:
    cursor = conn.cursor()
    cursor.execute("INSERT INTO users (name) VALUES (?)", ("Bob",))
    conn.commit()
# Auto-closes and auto-commits!
```

---

## ✅ Summary

- SQLite is a file-based database - no setup needed
- Use parameterized queries (?) to prevent injection
- Use row_factory for dict-like access
- Use context manager for automatic cleanup

## 🔗 Further Reading

- [sqlite3 Module](https://docs.python.org/3/library/sqlite3.html)
