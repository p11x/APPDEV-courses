# MySQL Integration

## Overview

MySQL is a popular relational database that works well with FastAPI through SQLAlchemy.

## Setup

### MySQL Connection

```python
# Example 1: MySQL connection setup
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

# MySQL connection string
DATABASE_URL = "mysql+pymysql://user:password@localhost:3306/fastapi_db?charset=utf8mb4"

engine = create_engine(
    DATABASE_URL,
    pool_size=10,
    max_overflow=20,
    pool_recycle=3600,
    pool_pre_ping=True
)

SessionLocal = sessionmaker(bind=engine)
```

### FastAPI Integration

```python
# Example 2: FastAPI with MySQL
from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session

app = FastAPI()

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

@app.on_event("startup")
async def startup():
    from app.models import Base
    Base.metadata.create_all(bind=engine)

@app.get("/users/")
async def list_users(db: Session = Depends(get_db)):
    from app.models import User
    return db.query(User).all()
```

## MySQL-Specific Features

```python
# Example 3: MySQL-specific configurations
from sqlalchemy.dialects.mysql import LONGTEXT, JSON

class Document(Base):
    __tablename__ = "documents"

    id = Column(Integer, primary_key=True)
    content = Column(LONGTEXT)  # MySQL-specific large text
    metadata = Column(JSON)  # MySQL JSON support
```

## Summary

MySQL integration with FastAPI is straightforward using PyMySQL driver.

## Next Steps

Continue learning about:
- [Connection String Security](./05_connection_string_security.md)
- [Engine Configuration](./06_engine_configuration.md)
