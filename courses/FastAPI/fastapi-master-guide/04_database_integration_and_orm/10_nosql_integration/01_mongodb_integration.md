# MongoDB Integration

## Overview

MongoDB is a popular document database that works well with FastAPI for flexible schema applications.

## Setup and Configuration

### Basic MongoDB Connection

```python
# Example 1: MongoDB with Motor (async driver)
from motor.motor_asyncio import AsyncIOMotorClient
from pymongo import MongoClient
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional, List
from bson import ObjectId
from datetime import datetime

app = FastAPI()

# Async MongoDB connection (recommended)
async_mongo_client = AsyncIOMotorClient("mongodb://localhost:27017")
async_db = async_mongo_client.fastapi_db

# Sync MongoDB connection (for scripts)
sync_mongo_client = MongoClient("mongodb://localhost:27017")
sync_db = sync_mongo_client.fastapi_db

@app.on_event("startup")
async def startup():
    """Connect to MongoDB on startup"""
    app.mongodb_client = AsyncIOMotorClient("mongodb://localhost:27017")
    app.mongodb = app.mongodb_client.fastapi_db

@app.on_event("shutdown")
async def shutdown():
    """Close MongoDB connection on shutdown"""
    app.mongodb_client.close()
```

## Pydantic Models for MongoDB

### Document Models

```python
# Example 2: Pydantic models for MongoDB
from pydantic import BaseModel, Field, BeforeValidator
from typing import Optional, Any, Annotated
from datetime import datetime

# PyObjectId for MongoDB ObjectId
PyObjectId = Annotated[str, BeforeValidator(str)]

class MongoBaseModel(BaseModel):
    """Base model for MongoDB documents"""
    id: Optional[PyObjectId] = Field(default=None, alias="_id")

    class Config:
        populate_by_name = True
        arbitrary_types_allowed = True
        json_encoders = {ObjectId: str}

class UserDocument(MongoBaseModel):
    """User document model"""
    username: str = Field(..., min_length=3, max_length=50)
    email: str = Field(...)
    hashed_password: str
    is_active: bool = True
    created_at: datetime = Field(default_factory=datetime.utcnow)
    updated_at: Optional[datetime] = None
    metadata: dict = Field(default_factory=dict)

class PostDocument(MongoBaseModel):
    """Post document with embedded comments"""
    title: str = Field(..., min_length=1, max_length=200)
    content: str
    author_id: str
    tags: List[str] = Field(default_factory=list)
    comments: List[dict] = Field(default_factory=list)
    created_at: datetime = Field(default_factory=datetime.utcnow)
    published: bool = False

class CommentDocument(BaseModel):
    """Embedded comment model"""
    id: str = Field(default_factory=lambda: str(ObjectId()))
    author: str
    content: str
    created_at: datetime = Field(default_factory=datetime.utcnow)
```

## CRUD Operations

### Complete MongoDB CRUD

```python
# Example 3: MongoDB CRUD operations
from bson import ObjectId
from typing import Optional, List

class MongoDBRepository:
    """Generic MongoDB repository"""

    def __init__(self, collection, model_class):
        self.collection = collection
        self.model_class = model_class

    async def create(self, document: BaseModel) -> str:
        """Create document"""
        doc_dict = document.model_dump(by_alias=True, exclude={"id"})
        result = await self.collection.insert_one(doc_dict)
        return str(result.inserted_id)

    async def get_by_id(self, doc_id: str) -> Optional[dict]:
        """Get document by ID"""
        if not ObjectId.is_valid(doc_id):
            return None
        doc = await self.collection.find_one({"_id": ObjectId(doc_id)})
        if doc:
            doc["_id"] = str(doc["_id"])
        return doc

    async def find_many(
        self,
        filter_dict: dict = None,
        skip: int = 0,
        limit: int = 20,
        sort_by: str = "created_at",
        sort_order: int = -1
    ) -> List[dict]:
        """Find multiple documents"""
        filter_dict = filter_dict or {}

        cursor = self.collection.find(filter_dict)
        cursor = cursor.sort(sort_by, sort_order)
        cursor = cursor.skip(skip).limit(limit)

        docs = []
        async for doc in cursor:
            doc["_id"] = str(doc["_id"])
            docs.append(doc)

        return docs

    async def update(self, doc_id: str, update_data: dict) -> bool:
        """Update document"""
        if not ObjectId.is_valid(doc_id):
            return False

        update_data["updated_at"] = datetime.utcnow()

        result = await self.collection.update_one(
            {"_id": ObjectId(doc_id)},
            {"$set": update_data}
        )
        return result.modified_count > 0

    async def delete(self, doc_id: str) -> bool:
        """Delete document"""
        if not ObjectId.is_valid(doc_id):
            return False

        result = await self.collection.delete_one({"_id": ObjectId(doc_id)})
        return result.deleted_count > 0

    async def count(self, filter_dict: dict = None) -> int:
        """Count documents"""
        return await self.collection.count_documents(filter_dict or {})

# User repository
class UserRepository(MongoDBRepository):
    def __init__(self):
        super().__init__(async_db.users, UserDocument)

    async def get_by_username(self, username: str) -> Optional[dict]:
        """Get user by username"""
        doc = await self.collection.find_one({"username": username})
        if doc:
            doc["_id"] = str(doc["_id"])
        return doc

    async def get_by_email(self, email: str) -> Optional[dict]:
        """Get user by email"""
        doc = await self.collection.find_one({"email": email})
        if doc:
            doc["_id"] = str(doc["_id"])
        return doc

user_repo = UserRepository()
```

## FastAPI Endpoints

### MongoDB API Endpoints

```python
# Example 4: FastAPI endpoints with MongoDB
from fastapi import HTTPException

@app.post("/users/", status_code=201)
async def create_user(user_data: UserDocument):
    """Create new user"""
    # Check if username exists
    existing = await user_repo.get_by_username(user_data.username)
    if existing:
        raise HTTPException(400, "Username already exists")

    # Check if email exists
    existing = await user_repo.get_by_email(user_data.email)
    if existing:
        raise HTTPException(400, "Email already exists")

    # Hash password and create user
    user_data.hashed_password = hash_password(user_data.hashed_password)
    user_id = await user_repo.create(user_data)

    return {"id": user_id, "message": "User created"}

@app.get("/users/{user_id}")
async def get_user(user_id: str):
    """Get user by ID"""
    user = await user_repo.get_by_id(user_id)
    if not user:
        raise HTTPException(404, "User not found")
    return user

@app.get("/users/")
async def list_users(
    skip: int = 0,
    limit: int = 20,
    search: Optional[str] = None
):
    """List users with filtering"""
    filter_dict = {}

    if search:
        filter_dict["$or"] = [
            {"username": {"$regex": search, "$options": "i"}},
            {"email": {"$regex": search, "$options": "i"}}
        ]

    users = await user_repo.find_many(filter_dict, skip, limit)
    total = await user_repo.count(filter_dict)

    return {"users": users, "total": total}

@app.patch("/users/{user_id}")
async def update_user(user_id: str, update_data: dict):
    """Update user"""
    success = await user_repo.update(user_id, update_data)
    if not success:
        raise HTTPException(404, "User not found")
    return {"message": "User updated"}

@app.delete("/users/{user_id}")
async def delete_user(user_id: str):
    """Delete user"""
    success = await user_repo.delete(user_id)
    if not success:
        raise HTTPException(404, "User not found")
    return {"message": "User deleted"}
```

## Advanced Queries

### Aggregation Pipeline

```python
# Example 5: MongoDB aggregation
async def get_user_stats():
    """Get user statistics using aggregation"""
    pipeline = [
        {
            "$group": {
                "_id": "$is_active",
                "count": {"$sum": 1},
                "users": {"$push": "$username"}
            }
        },
        {
            "$project": {
                "status": {"$cond": ["$_id", "active", "inactive"]},
                "count": 1,
                "users": {"$slice": ["$users", 5]}
            }
        }
    ]

    result = await async_db.users.aggregate(pipeline).to_list(10)
    return result

async def get_posts_with_comments():
    """Get posts with comment counts"""
    pipeline = [
        {
            "$lookup": {
                "from": "comments",
                "localField": "_id",
                "foreignField": "post_id",
                "as": "comments"
            }
        },
        {
            "$addFields": {
                "comment_count": {"$size": "$comments"}
            }
        },
        {
            "$sort": {"comment_count": -1}
        },
        {
            "$limit": 10
        }
    ]

    return await async_db.posts.aggregate(pipeline).to_list(10)
```

## Indexing

### MongoDB Indexes

```python
# Example 6: MongoDB indexes
async def create_indexes():
    """Create MongoDB indexes"""
    # Single field index
    await async_db.users.create_index("username", unique=True)
    await async_db.users.create_index("email", unique=True)

    # Compound index
    await async_db.posts.create_index([("author_id", 1), ("created_at", -1)])

    # Text index for search
    await async_db.posts.create_index([("title", "text"), ("content", "text")])

    # TTL index (auto-delete after 30 days)
    await async_db.sessions.create_index(
        "created_at",
        expireAfterSeconds=30 * 24 * 60 * 60
    )

@app.on_event("startup")
async def startup():
    await create_indexes()
```

## Best Practices

### MongoDB Guidelines

```python
# Example 7: MongoDB best practices
"""
MongoDB Best Practices:

1. Use Motor for async operations
2. Create indexes for frequently queried fields
3. Use projection to limit returned fields
4. Implement pagination for large results
5. Use embedded documents for 1:N relationships
6. Use references for N:N relationships
7. Validate data with Pydantic
8. Handle ObjectId properly
9. Use aggregation for complex queries
10. Monitor query performance
"""

# Good: Embedded document
post = {
    "title": "My Post",
    "content": "Content",
    "comments": [  # Embedded
        {"author": "user1", "content": "Great!"},
        {"author": "user2", "content": "Thanks!"}
    ]
}

# Good: Reference for large related data
post = {
    "title": "My Post",
    "author_id": ObjectId("...")  # Reference to users collection
}
```

## Summary

| Operation | Method | Example |
|-----------|--------|---------|
| Create | `insert_one()` | `await collection.insert_one(doc)` |
| Read | `find_one()` | `await collection.find_one({"_id": id})` |
| Update | `update_one()` | `await collection.update_one(filter, update)` |
| Delete | `delete_one()` | `await collection.delete_one(filter)` |
| Aggregate | `aggregate()` | `await collection.aggregate(pipeline)` |

## Next Steps

Continue learning about:
- [Redis Integration](./02_redis_integration.md) - Caching layer
- [Elasticsearch Integration](./03_elasticsearch_integration.md) - Full-text search
