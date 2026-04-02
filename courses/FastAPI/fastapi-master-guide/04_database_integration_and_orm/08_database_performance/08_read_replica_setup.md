# Read Replica Setup

## Overview

Read replicas improve database performance by distributing read queries across multiple instances.

## Configuration

### SQLAlchemy with Read Replicas

```python
# Example 1: Read replica configuration
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

# Master for writes
master_engine = create_engine(
    "postgresql://user:pass@master:5432/db",
    pool_size=10
)

# Replicas for reads
replica_engines = [
    create_engine("postgresql://user:pass@replica1:5432/db"),
    create_engine("postgresql://user:pass@replica2:5432/db"),
]

class RoutingSession:
    """Route queries to appropriate database"""

    def __init__(self):
        self._replica_index = 0

    def get_write_session(self):
        return sessionmaker(bind=master_engine)()

    def get_read_session(self):
        engine = replica_engines[self._replica_index]
        self._replica_index = (self._replica_index + 1) % len(replica_engines)
        return sessionmaker(bind=engine)()

routing = RoutingSession()
```

### FastAPI Integration

```python
# Example 2: FastAPI with read replicas
from fastapi import FastAPI, Depends

app = FastAPI()

def get_write_db():
    db = routing.get_write_session()
    try:
        yield db
    finally:
        db.close()

def get_read_db():
    db = routing.get_read_session()
    try:
        yield db
    finally:
        db.close()

@app.get("/items/")
async def list_items(db: Session = Depends(get_read_db)):
    """Read from replica"""
    return db.query(Item).all()

@app.post("/items/")
async def create_item(item: ItemCreate, db: Session = Depends(get_write_db)):
    """Write to master"""
    db_item = Item(**item.dict())
    db.add(db_item)
    db.commit()
    return db_item
```

## Summary

Read replicas improve read performance for high-traffic applications.

## Next Steps

Continue learning about:
- [Horizontal Scaling](./10_horizontal_scaling.md)
- [Database Partitioning](./09_database_partitioning.md)
