# PostgreSQL Setup

## What You'll Learn
- Installing PostgreSQL
- Creating databases and users
- Connecting from Python
- Using PostgreSQL with Flask and FastAPI

## Prerequisites
- Completed SQL Basics

## Installing PostgreSQL

### Windows
Download from https://www.postgresql.org/download/windows/

### macOS
```bash
brew install postgresql
brew services start postgresql
```

### Linux (Ubuntu)
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
```

## Basic PostgreSQL Commands

```bash
# Connect to PostgreSQL
psql -U postgres

# Create a database
CREATE DATABASE myapp;

# Create a user
CREATE USER myuser WITH PASSWORD 'mypassword';

# Grant privileges
GRANT ALL PRIVILEGES ON DATABASE myapp TO myuser;

# Connect to database
\c myapp

# Create extension for UUIDs
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
```

## Connecting from Python

```bash
pip install psycopg2-binary
```

### Connection String

```
postgresql://username:password@localhost:5432/database_name
```

### Example

```python
from sqlalchemy import create_engine

DATABASE_URL = "postgresql://myuser:mypassword@localhost:5432/myapp"

engine = create_engine(DATABASE_URL)
```

## Summary
- PostgreSQL is a powerful production database
- Use `psycopg2` to connect from Python
- Connection string: `postgresql://user:pass@host:port/db`
