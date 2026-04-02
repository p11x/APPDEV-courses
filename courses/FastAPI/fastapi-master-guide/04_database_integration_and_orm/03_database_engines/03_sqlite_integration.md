# SQLite Integration

## Overview

SQLite is a lightweight database perfect for development, testing, and small applications. FastAPI works seamlessly with SQLite.

## Setup and Configuration

### Basic SQLite Setup

```python
# Example 1: SQLite with SQLAlchemy
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession

# Sync SQLite (file-based)
SYNC_DATABASE_URL = "sqlite:///./app.db"
sync_engine = create_engine(
    SYNC_DATABASE_URL,
    connect_args={"check_same_thread": False}  # Required for SQLite
)
SyncSession = sessionmaker(bind=sync_engine)

# Async SQLite (with aiosqlite)
ASYNC_DATABASE_URL = "sqlite+aiosqlite:///./app.db"
async_engine = create_async_engine(
    ASYNC_DATABASE_URL,
    connect_args={"check_same_thread": False}
)
AsyncSession = sessionmaker(
    async_engine,
    class_=AsyncSession,
    expire_on_commit=False
)

# In-memory SQLite (for testing)
MEMORY_URL = "sqlite:///:memory:"
memory_engine = create_engine(MEMORY_URL)
```

## FastAPI Integration

### Complete SQLite Setup

```python
# Example 2: FastAPI with SQLite
from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session

app = FastAPI()

DATABASE_URL = "sqlite:///./fastapi.db"
engine = create_engine(DATABASE_URL, connect_args={"check_same_thread": False})
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

Base = declarative_base()

# Models
class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True, index=True)
    username = Column(String, unique=True, index=True)
    email = Column(String, unique=True)
    is_active = Column(Boolean, default=True)

# Create tables
Base.metadata.create_all(bind=engine)

# Dependency
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# Endpoints
@app.post("/users/")
def create_user(username: str, email: str, db: Session = Depends(get_db)):
    user = User(username=username, email=email)
    db.add(user)
    db.commit()
    db.refresh(user)
    return user

@app.get("/users/")
def list_users(db: Session = Depends(get_db)):
    return db.query(User).all()
```

## SQLite-Specific Features

### SQLite Operations

```python
# Example 3: SQLite-specific operations
from sqlalchemy import text

# Full-text search with FTS5
def setup_fts(db: Session):
    """Setup full-text search"""
    db.execute(text("""
        CREATE VIRTUAL TABLE IF NOT EXISTS posts_fts
        USING fts5(title, content, content=posts, content_rowid=id)
    """))
    db.commit()

# JSON operations
class Document(Base):
    __tablename__ = "documents"

    id = Column(Integer, primary_key=True)
    data = Column(JSON)  # SQLite JSON support

@app.get("/documents/search/")
def search_json(key: str, value: str, db: Session = Depends(get_db)):
    """Search JSON fields"""
    return db.execute(
        text(f"SELECT * FROM documents WHERE json_extract(data, '$.{key}') = :value"),
        {"value": value}
    ).fetchall()
```

## Best Practices

### SQLite Guidelines

```python
# Example 4: SQLite best practices
"""
SQLite Best Practices:

1. Use for development and testing
2. Not recommended for production with high concurrency
3. Enable WAL mode for better concurrency
4. Use :memory: for unit tests
5. Backup database file regularly
6. Use PRAGMA for optimization
"""

# Enable WAL mode
def optimize_sqlite():
    """SQLite optimization"""
    with engine.connect() as conn:
        conn.execute(text("PRAGMA journal_mode=WAL"))
        conn.execute(text("PRAGMA synchronous=NORMAL"))
        conn.execute(text("PRAGMA cache_size=-64000"))  # 64MB cache
        conn.execute(text("PRAGMA foreign_keys=ON"))
```

## Summary

| Feature | SQLite | PostgreSQL |
|---------|--------|------------|
| Setup | Simple | Complex |
| Concurrency | Limited | Excellent |
| Scalability | Vertical | Horizontal |
| Best For | Dev/Test | Production |

## Next Steps

Continue learning about:
- [MySQL Integration](./02_mysql_integration.md)
- [PostgreSQL Integration](./01_postgresql_integration.md)
