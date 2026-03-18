# SQL Basics

## What You'll Learn
- What SQL is and why it matters
- Creating and querying tables
- Basic SQL commands (SELECT, INSERT, UPDATE, DELETE)
- Filtering and sorting data
- JOINs to combine tables
- Database design basics

## Prerequisites
- Basic programming knowledge
- Understanding of data structures

## What Is SQL?

**SQL (Structured Query Language)** is the standard language for working with relational databases. It lets you create, read, update, and delete data.

Think of a database like a spreadsheet:
- Tables are like worksheets
- Columns are like fields
- Rows are records

## Popular SQL Databases

| Database | Best For |
|----------|----------|
| SQLite | Prototyping, small apps |
| PostgreSQL | Production apps, complex data |
| MySQL | Web apps, LAMP stack |
| PostgreSQL | Enterprise, JSON, geospatial |

## Basic SQL Commands

### CREATE TABLE

Create a table to store data:

```sql
CREATE TABLE users (
    id INTEGER PRIMARY KEY,
    username TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    password TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### INSERT

Add data to a table:

```sql
-- Insert a single row
INSERT INTO users (username, email, password)
VALUES ('alice', 'alice@example.com', 'hashed_password');

-- Insert multiple rows
INSERT INTO users (username, email, password)
VALUES 
    ('bob', 'bob@example.com', 'pass123'),
    ('charlie', 'charlie@example.com', 'secret456');
```

### SELECT

Retrieve data:

```sql
-- Get all columns
SELECT * FROM users;

-- Get specific columns
SELECT username, email FROM users;

-- With alias
SELECT username AS name, email AS contact FROM users;
```

### WHERE (Filtering)

Filter results:

```sql
-- Simple filter
SELECT * FROM users WHERE username = 'alice';

-- Multiple conditions
SELECT * FROM users WHERE id > 5 AND email LIKE '%@example.com';

-- Common operators: =, <>, <, >, <=, >=, LIKE, IN, BETWEEN
```

### ORDER BY (Sorting)

Sort results:

```sql
-- Ascending (default)
SELECT * FROM users ORDER BY username;

-- Descending
SELECT * FROM users ORDER BY created_at DESC;

-- Multiple columns
SELECT * FROM users ORDER BY username, created_at;
```

### LIMIT and OFFSET

Pagination:

```sql
-- Get first 10
SELECT * FROM users LIMIT 10;

-- Skip first 10, get next 10
SELECT * FROM users LIMIT 10 OFFSET 10;

-- Shorthand
SELECT * FROM users LIMIT 10, 10;
```

## Aggregate Functions

```sql
-- Count rows
SELECT COUNT(*) FROM users;

-- Sum values
SELECT SUM(price) FROM products;

-- Average
SELECT AVG(age) FROM users;

-- Min/Max
SELECT MIN(price), MAX(price) FROM products;

-- Group by
SELECT category, COUNT(*) FROM products GROUP BY category;

-- Having (filter after grouping)
SELECT category, COUNT(*) as count 
FROM products 
GROUP BY category 
HAVING count > 5;
```

## UPDATE (Modifying Data)

```sql
-- Update a single row
UPDATE users 
SET email = 'newemail@example.com' 
WHERE id = 1;

-- Update multiple columns
UPDATE users 
SET username = 'new_username', email = 'new@example.com' 
WHERE id = 1;
```

## DELETE (Removing Data)

```sql
-- Delete specific rows
DELETE FROM users WHERE id = 1;

-- Delete all rows (careful!)
DELETE FROM users;
```

## JOINs (Combining Tables)

### Types of JOINs

```
LEFT JOIN: All from left + matching from right
RIGHT JOIN: All from right + matching from left  
INNER JOIN: Only matching rows from both
FULL OUTER JOIN: All rows from both
```

### Example Tables

```sql
-- Users table
CREATE TABLE users (
    id INTEGER PRIMARY KEY,
    username TEXT,
    email TEXT
);

-- Posts table
CREATE TABLE posts (
    id INTEGER PRIMARY KEY,
    user_id INTEGER,
    title TEXT,
    content TEXT,
    FOREIGN KEY (user_id) REFERENCES users(id)
);
```

### INNER JOIN

```sql
-- Get posts with their authors
SELECT 
    posts.title,
    users.username AS author
FROM posts
INNER JOIN users ON posts.user_id = users.id;
```

### LEFT JOIN

```sql
-- Get all users with their posts (including users with no posts)
SELECT 
    users.username,
    posts.title
FROM users
LEFT JOIN posts ON users.id = posts.user_id;
```

## Creating a Complete Example

### Setup SQLite Database

```bash
sqlite3 blog.db
```

### SQL Commands

```sql
-- Create tables
CREATE TABLE users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    username TEXT UNIQUE NOT NULL,
    email TEXT UNIQUE NOT NULL,
    password TEXT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE posts (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    content TEXT NOT NULL,
    user_id INTEGER NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Insert data
INSERT INTO users (username, email, password) VALUES 
('alice', 'alice@example.com', 'pass123'),
('bob', 'bob@example.com', 'secret456');

INSERT INTO posts (title, content, user_id) VALUES 
('First Post', 'Hello World!', 1),
('Second Post', 'More content here', 1),
('Bob Post', 'Hello from Bob', 2);

-- Query with JOIN
SELECT 
    posts.title,
    posts.content,
    users.username as author,
    posts.created_at
FROM posts
INNER JOIN users ON posts.user_id = users.id
ORDER BY posts.created_at DESC;
```

## Using SQL in Python

With sqlite3 (built-in):

```python
import sqlite3

# Connect to database
conn = sqlite3.connect("blog.db")
cursor = conn.cursor()

# Execute query
cursor.execute("SELECT * FROM users WHERE id = ?", (1,))
user = cursor.fetchone()

# Get all results
cursor.execute("SELECT * FROM users")
users = cursor.fetchall()

# Insert with parameterized query
cursor.execute(
    "INSERT INTO users (username, email, password) VALUES (?, ?, ?)",
    ("charlie", "charlie@example.com", "pass789")
)
conn.commit()

# Close connection
conn.close()
```

## Summary
- **SQL** is the standard language for relational databases
- **CREATE TABLE** defines the structure
- **INSERT** adds data, **SELECT** retrieves it
- **WHERE** filters data, **ORDER BY** sorts it
- **JOIN**s combine data from multiple tables
- Use **parameterized queries** to prevent SQL injection

## Next Steps
→ Continue to `02-sqlalchemy-orm.md` to learn how to use SQLAlchemy ORM for Pythonic database operations.
