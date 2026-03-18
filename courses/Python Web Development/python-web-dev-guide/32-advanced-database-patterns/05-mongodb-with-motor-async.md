# MongoDB with Motor Async

## What You'll Learn
- When to choose document databases over relational databases
- How Motor provides async MongoDB access for Python
- Designing effective document schemas (embedding vs referencing)
- Performing CRUD operations with async/await
- Using aggregation pipelines for complex queries

## Prerequisites
- Completed `04-timescaledb-time-series-data.md` — understanding of time-series vs document approaches
- Completed `05-databases/02-sqlalchemy-orm.md` — understanding of ORM patterns
- Understanding of async/await in Python 3.11+

## When to Use MongoDB

MongoDB is a document database — data is stored as flexible JSON-like documents. Unlike PostgreSQL's rigid tables, documents in a collection can have different fields.

| Scenario | Use MongoDB | Use PostgreSQL |
|----------|------------|----------------|
| Unstructured/variable data | ✅ | ❌ |
| Rapid prototyping | ✅ | ❌ |
| Complex joins | ❌ | ✅ |
| Strong ACID guarantees | ❌ | ✅ |
| Multi-document transactions | ⚠️ (since v4.0) | ✅ |
| Nested hierarchical data | ✅ | ⚠️ |

Common MongoDB use cases:
- User profiles with custom fields
- Catalogs with variable attributes  
- Content management systems
- Logging and event storage

## Installing Motor

```bash
pip install motor pydantic
```

Motor is MongoDB's async Python driver:

```python
import motor.motor_asyncio
from pydantic import BaseModel
from typing import Optional
from datetime import datetime

# Connection setup
client = motor.motor_asyncio.AsyncIOMotorClient("mongodb://localhost:27017")
db = client.my_database
```

## Defining Documents with Pydantic

```python
class Address(BaseModel):
    street: str
    city: str
    country: str
    postal_code: str

class User(BaseModel):
    name: str
    email: str
    age: Optional[int] = None
    address: Optional[Address] = None
    tags: list[str] = []
    created_at: datetime = datetime.utcnow()
    
    class Config:
        # Allow extra fields for flexible schema
        extra = "allow"
```

## CRUD Operations

### Create

```python
async def create_user(user: User) -> str:
    """Insert a new user document."""
    doc = user.model_dump()
    result = await db.users.insert_one(doc)
    return str(result.inserted_id)

# Usage
new_user = User(
    name="Alice",
    email="alice@example.com",
    address=Address(
        street="123 Main St",
        city="San Francisco",
        country="USA",
        postal_code="94102"
    ),
    tags=["premium", "beta-tester"]
)
user_id = await create_user(new_user)
```

🔍 **Line-by-Line Breakdown:**
1. `user.model_dump()` — Converts Pydantic model to dict, ready for MongoDB insertion
2. `db.users.insert_one(doc)` — Inserts document into `users` collection. Returns `InsertOneResult`
3. `result.inserted_id` — The MongoDB-generated `_id` field as ObjectId

### Read

```python
from bson import ObjectId

async def get_user(user_id: str) -> dict | None:
    """Find user by ID."""
    doc = await db.users.find_one({"_id": ObjectId(user_id)})
    return doc

async def find_users_by_tag(tag: str, limit: int = 10) -> list[dict]:
    """Find users with a specific tag."""
    cursor = db.users.find(
        {"tags": tag}
    ).limit(limit)
    
    return await cursor.to_list(length=limit)

async def get_user_count() -> int:
    """Count all users."""
    return await db.users.count_documents({})
```

### Update

```python
async def update_user(user_id: str, updates: dict) -> bool:
    """Update user with partial document."""
    result = await db.users.update_one(
        {"_id": ObjectId(user_id)},
        {"$set": updates}
    )
    return result.modified_count > 0

async def add_tag(user_id: str, tag: str) -> bool:
    """Add a tag to user."""
    result = await db.users.update_one(
        {"_id": ObjectId(user_id)},
        {"$addToSet": {"tags": tag}}  # $addToSet prevents duplicates
    )
    return result.modified_count > 0
```

### Delete

```python
async def delete_user(user_id: str) -> bool:
    """Delete a user."""
    result = await db.users.delete_one({"_id": ObjectId(user_id)})
    return result.deleted_count > 0
```

## Aggregation Pipelines

MongoDB's aggregation framework lets you process data through stages:

```python
async def get_user_stats_by_country() -> list[dict]:
    """Aggregate user count and average age by country."""
    pipeline = [
        # Match only users with addresses
        {"$match": {"address": {"$ne": None}}},
        
        # Group by country
        {
            "$group": {
                "_id": "$address.country",
                "user_count": {"$sum": 1},
                "avg_age": {"$avg": "$age"},
                "users": {"$push": "$name"}
            }
        },
        
        # Sort by count descending
        {"$sort": {"user_count": -1}},
        
        # Limit to top 10
        {"$limit": 10},
        
        # Rename fields for clarity
        {
            "$project": {
                "_id": 0,
                "country": "$_id",
                "user_count": 1,
                "avg_age": 1,
                "user_count_formatted": {
                    "$concat": [{"$toString": "$user_count"}, " users"]
                }
            }
        }
    ]
    
    cursor = db.users.aggregate(pipeline)
    return await cursor.to_list(length=100)
```

🔍 **Line-by-Line Breakdown:**
1. `$match` — Filters documents early to reduce processing
2. `$group` — Groups by country, counts users, calculates average age
3. `$sort` — Orders results (descending by count)
4. `$limit` — Returns only top 10
5. `$project` — Reshapes output, hides `_id`, creates formatted strings

## Embedding vs Referencing

Two ways to model relationships:

### Embedding (denormalized)

```python
class Order(BaseModel):
    order_id: str
    user: User  # Embedded - duplicated user data
    items: list[dict]
    total: float
    status: str
```

**Use when:**
- Data is read together frequently
- Data doesn't change often
- You need atomic updates of the whole thing

### Referencing (normalized)

```python
# Collection: orders
{
    "_id": ObjectId("..."),
    "user_id": ObjectId("..."),  # Reference to users collection
    "items": [...],
    "total": 99.99
}

# Collection: users  
{
    "_id": ObjectId("..."),
    "name": "Alice"
}
```

**Use when:**
- Data changes frequently
- Data is shared across documents
- You need to query documents independently

## Async Context Manager Pattern

```python
from contextlib import asynccontextmanager

@asynccontextmanager
async def get_database():
    """Database connection context manager."""
    client = motor.motor_asyncio.AsyncIOMotorClient("mongodb://localhost:27017")
    try:
        yield client.my_database
    finally:
        client.close()

# Usage
async def main():
    async with get_database() as db:
        user = await db.users.find_one({"name": "Alice"})
        print(user)
```

## Production Considerations

- **Connection pooling**: Motor manages connection pools automatically. Default size is reasonable but tune for high concurrency.
- **Indexing**: Create indexes for frequently queried fields — `db.collection.create_index([("email", 1)], unique=True)`
- **Security**: MongoDB requires authentication in production. Use TLS for connection encryption.
- **Transactions**: MongoDB supports multi-document transactions since v4.0, but they have performance overhead. Use them only when truly needed.
- **Schema flexibility**: The "no schema" approach works early but becomes a liability. Document your expected structure even if MongoDB doesn't enforce it.

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not handling ObjectId conversion

**Wrong:**
```python
# Trying to use string as ObjectId
user = await db.users.find_one({"_id": user_id_string})
```

**Why it fails:** MongoDB's `_id` is an ObjectId type, not a string. Direct comparison fails.

**Fix:**
```python
from bson import ObjectId
user = await db.users.find_one({"_id": ObjectId(user_id_string)})
```

### ❌ Mistake 2: Using sync patterns in async code

**Wrong:**
```python
# motor_asyncio but using sync patterns
db.users.find_one({"name": "Alice"})  # Returns cursor, not result!
```

**Why it fails:** Without `await`, you get a cursor object, not the document. The query never actually executes.

**Fix:**
```python
user = await db.users.find_one({"name": "Alice"})  # await the coroutine
```

### ❌ Mistake 3: Embedding everything

**Wrong:**
```python
# Embedding orders in user document
{
    "name": "Alice",
    "orders": [
        {"id": 1, "items": [...]},
        {"id": 2, "items": [...]},
        # ... 10,000 orders
    ]
}
```

**Why it fails:** Documents have a 16MB size limit. Embedding unbounded arrays causes document bloat and performance issues.

**Fix:** Reference instead:
```python
# User document
{"_id": ObjectId, "name": "Alice"}

# Order documents (separate collection)
{"_id": ObjectId, "user_id": ObjectId, "items": [...]}
```

## Summary

- MongoDB with Motor provides async document database access for Python
- Use Pydantic models to define document schemas while maintaining flexibility
- Understand when to embed (read-heavy, rarely changing) vs reference (shared, frequently updated)
- Aggregation pipelines provide powerful data processing capabilities
- Always handle ObjectId conversion and use async/await properly

## Next Steps

→ Continue to `06-redis-as-primary-database.md` to learn about using Redis as a primary data store for specific use cases.
