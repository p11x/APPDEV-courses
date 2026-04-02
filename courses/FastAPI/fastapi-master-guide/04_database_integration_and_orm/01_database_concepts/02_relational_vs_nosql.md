# Relational vs NoSQL

## Overview

Understanding when to use relational (SQL) vs NoSQL databases is crucial for building scalable FastAPI applications.

## Relational Databases

### PostgreSQL Example

```python
# Example 1: Relational database with complex relationships
from sqlalchemy import create_engine, Column, Integer, String, ForeignKey, Table, DateTime
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import relationship
from datetime import datetime

Base = declarative_base()

# Many-to-many association table
user_roles = Table(
    'user_roles', Base.metadata,
    Column('user_id', Integer, ForeignKey('users.id')),
    Column('role_id', Integer, ForeignKey('roles.id'))
)

class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50), unique=True, nullable=False)
    email = Column(String(100), unique=True, nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow)

    # One-to-many: user has many posts
    posts = relationship("Post", back_populates="author", cascade="all, delete-orphan")

    # Many-to-many: user has many roles
    roles = relationship("Role", secondary=user_roles, back_populates="users")

    # One-to-one: user has one profile
    profile = relationship("Profile", back_populates="user", uselist=False)

class Post(Base):
    __tablename__ = "posts"

    id = Column(Integer, primary_key=True)
    title = Column(String(200), nullable=False)
    content = Column(String, nullable=False)
    author_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow)

    author = relationship("User", back_populates="posts")
    comments = relationship("Comment", back_populates="post", cascade="all, delete-orphan")

class Comment(Base):
    __tablename__ = "comments"

    id = Column(Integer, primary_key=True)
    content = Column(String(500), nullable=False)
    post_id = Column(Integer, ForeignKey("posts.id"), nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow)

    post = relationship("Post", back_populates="comments")

class Role(Base):
    __tablename__ = "roles"

    id = Column(Integer, primary_key=True)
    name = Column(String(50), unique=True, nullable=False)

    users = relationship("User", secondary=user_roles, back_populates="roles")

class Profile(Base):
    __tablename__ = "profiles"

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey("users.id"), unique=True)
    bio = Column(String(500))
    avatar_url = Column(String(200))

    user = relationship("User", back_populates="profile")

# Relational database strengths demonstrated:
# 1. Complex relationships (1:1, 1:N, N:N)
# 2. Referential integrity with foreign keys
# 3. Cascading deletes
# 4. ACID transactions
# 5. Complex queries with JOINs
```

### Complex SQL Queries

```python
# Example 2: Complex relational queries
from sqlalchemy import select, func, and_, or_
from sqlalchemy.orm import Session

async def complex_relational_queries(db: Session):
    """Demonstrate complex SQL capabilities"""

    # 1. JOIN query: Get users with their post count
    query = (
        select(
            User.username,
            func.count(Post.id).label("post_count")
        )
        .outerjoin(Post, User.id == Post.author_id)
        .group_by(User.id)
    )
    result = db.execute(query).all()

    # 2. Subquery: Get users who have more than 5 posts
    subquery = (
        select(Post.author_id)
        .group_by(Post.author_id)
        .having(func.count(Post.id) > 5)
    )
    query = select(User).where(User.id.in_(subquery))
    active_users = db.execute(query).scalars().all()

    # 3. Window function: Rank posts by comment count
    from sqlalchemy import over
    query = (
        select(
            Post.title,
            func.count(Comment.id).label("comment_count"),
            func.rank().over(
                order_by=func.count(Comment.id).desc()
            ).label("rank")
        )
        .outerjoin(Comment)
        .group_by(Post.id)
    )
    ranked_posts = db.execute(query).all()

    # 4. CTE (Common Table Expression): Recursive query
    from sqlalchemy import literal_column
    cte = (
        select(User.id, User.username, literal_column("1").label("level"))
        .where(User.id == 1)
        .cte(name="user_hierarchy", recursive=True)
    )
    cte_alias = cte.alias()
    cte = cte.union_all(
        select(User.id, User.username, (cte_alias.c.level + 1))
        .where(User.id > cte_alias.c.id)
    )

    return {
        "post_counts": result,
        "active_users": active_users,
        "ranked_posts": ranked_posts
    }
```

## NoSQL Databases

### MongoDB Example

```python
# Example 3: NoSQL document database
from motor.motor_asyncio import AsyncIOMotorClient
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime
from bson import ObjectId

# MongoDB connection
client = AsyncIOMotorClient("mongodb://localhost:27017")
db = client.social_media

class CommentDocument(BaseModel):
    """Embedded comment document"""
    id: str = Field(default_factory=lambda: str(ObjectId()))
    content: str
    author: str
    created_at: datetime = Field(default_factory=datetime.utcnow)

class PostDocument(BaseModel):
    """Post document with embedded comments"""
    id: Optional[str] = Field(default_factory=lambda: str(ObjectId()))
    title: str
    content: str
    author: str
    tags: List[str] = []
    comments: List[CommentDocument] = []  # Embedded documents
    created_at: datetime = Field(default_factory=datetime.utcnow)
    updated_at: Optional[datetime] = None

    class Config:
        json_encoders = {ObjectId: str}

class UserDocument(BaseModel):
    """User document with flexible schema"""
    id: Optional[str] = Field(default_factory=lambda: str(ObjectId()))
    username: str
    email: str
    profile: dict = {}  # Flexible nested object
    preferences: dict = {"theme": "light", "language": "en"}
    social_links: List[dict] = []
    created_at: datetime = Field(default_factory=datetime.utcnow)

    class Config:
        json_encoders = {ObjectId: str}

class MongoDBOperations:
    """MongoDB CRUD operations"""

    def __init__(self):
        self.users = db.users
        self.posts = db.posts

    async def create_user(self, user: UserDocument) -> str:
        """Create user with flexible schema"""
        result = await self.users.insert_one(user.dict())
        return str(result.inserted_id)

    async def create_post_with_comments(self, post: PostDocument) -> str:
        """Create post with embedded comments"""
        result = await self.posts.insert_one(post.dict())
        return str(result.inserted_id)

    async def add_comment(self, post_id: str, comment: CommentDocument):
        """Add comment to post (embedded)"""
        await self.posts.update_one(
            {"_id": ObjectId(post_id)},
            {"$push": {"comments": comment.dict()}}
        )

    async def search_posts(self, tags: List[str], limit: int = 10):
        """Search posts by tags"""
        cursor = self.posts.find(
            {"tags": {"$in": tags}}
        ).sort("created_at", -1).limit(limit)

        return await cursor.to_list(length=limit)

    async def aggregate_user_stats(self):
        """Aggregate pipeline example"""
        pipeline = [
            {
                "$lookup": {
                    "from": "posts",
                    "localField": "username",
                    "foreignField": "author",
                    "as": "posts"
                }
            },
            {
                "$project": {
                    "username": 1,
                    "post_count": {"$size": "$posts"},
                    "email": 1
                }
            },
            {"$sort": {"post_count": -1}}
        ]

        return await self.users.aggregate(pipeline).to_list(length=100)

mongo_ops = MongoDBOperations()
```

### Redis Example

```python
# Example 4: Key-value store for caching
import redis.asyncio as redis
import json
from typing import Any, Optional
from datetime import timedelta

redis_client = redis.Redis(host='localhost', port=6379, decode_responses=True)

class CacheLayer:
    """Redis cache layer"""

    async def get(self, key: str) -> Optional[Any]:
        """Get cached value"""
        value = await redis_client.get(key)
        return json.loads(value) if value else None

    async def set(self, key: str, value: Any, expire: int = 3600):
        """Set cached value with expiration"""
        await redis_client.setex(key, expire, json.dumps(value))

    async def delete(self, key: str):
        """Delete cached value"""
        await redis_client.delete(key)

    async def get_or_set(self, key: str, factory, expire: int = 3600):
        """Get from cache or compute and cache"""
        value = await self.get(key)
        if value is None:
            value = await factory()
            await self.set(key, value, expire)
        return value

    async def invalidate_pattern(self, pattern: str):
        """Invalidate all keys matching pattern"""
        async for key in redis_client.scan_iter(match=pattern):
            await redis_client.delete(key)

cache = CacheLayer()

# Cache-aside pattern for FastAPI
from fastapi import FastAPI, Depends

app = FastAPI()

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Get user with caching"""
    cache_key = f"user:{user_id}"

    async def fetch_from_db():
        # Simulate database query
        return {"id": user_id, "username": "john", "email": "john@example.com"}

    return await cache.get_or_set(cache_key, fetch_from_db, expire=300)
```

## When to Use Each

### Decision Matrix

```python
# Example 5: Database selection guide
"""
Use RELATIONAL (PostgreSQL/MySQL) when:
✓ Data has clear relationships
✓ Need ACID transactions
✓ Complex queries required
✓ Data structure is stable
✓ Reporting and analytics needed

Use NoSQL (MongoDB) when:
✓ Schema changes frequently
✓ Document-oriented data
✓ Horizontal scaling needed
✓ High write throughput
✓ Flexible data models

Use Redis when:
✓ Caching layer needed
✓ Session storage
✓ Real-time leaderboards
✓ Rate limiting
✓ Pub/sub messaging
"""

class DatabaseSelector:
    """Help select appropriate database"""

    @staticmethod
    def recommend(data_type: str, requirements: dict) -> str:
        if data_type == "relational" and requirements.get("acid"):
            return "PostgreSQL"
        elif data_type == "document" and requirements.get("flexible_schema"):
            return "MongoDB"
        elif requirements.get("caching"):
            return "Redis"
        elif requirements.get("search"):
            return "Elasticsearch"
        elif requirements.get("time_series"):
            return "InfluxDB"
        else:
            return "PostgreSQL"  # Default recommendation
```

## Polyglot Persistence

### Multiple Databases

```python
# Example 6: Using multiple databases
from fastapi import FastAPI
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from motor.motor_asyncio import AsyncIOMotorClient
import redis.asyncio as redis

app = FastAPI()

# PostgreSQL for primary data
pg_engine = create_engine("postgresql://user:pass@localhost/main_db")
PgSession = sessionmaker(bind=pg_engine)

# MongoDB for documents
mongo_client = AsyncIOMotorClient("mongodb://localhost:27017")
mongo_db = mongo_client.documents_db

# Redis for caching
redis_client = redis.Redis(host='localhost', port=6379)

class MultiDatabaseService:
    """Service using multiple databases"""

    def __init__(self):
        self.pg_session = PgSession()
        self.mongo = mongo_db
        self.redis = redis_client

    async def get_user_with_posts(self, user_id: int):
        """
        PostgreSQL for user data (relational)
        MongoDB for posts (document-oriented)
        Redis for caching (fast access)
        """
        # Check cache first
        cache_key = f"user_posts:{user_id}"
        cached = await self.redis.get(cache_key)
        if cached:
            return json.loads(cached)

        # Get user from PostgreSQL
        user = self.pg_session.query(User).filter_by(id=user_id).first()

        # Get posts from MongoDB
        posts = await self.mongo.posts.find({"user_id": user_id}).to_list(100)

        # Combine and cache
        result = {
            "user": {"id": user.id, "username": user.username},
            "posts": posts
        }
        await self.redis.setex(cache_key, 300, json.dumps(result))

        return result

multi_db = MultiDatabaseService()
```

## Summary

| Aspect | Relational (SQL) | Document (MongoDB) | Key-Value (Redis) |
|--------|------------------|---------------------|-------------------|
| Schema | Fixed | Flexible | N/A |
| Relations | JOINs | Embedded | None |
| Transactions | Full ACID | Limited | Limited |
| Scaling | Vertical | Horizontal | Horizontal |
| Query Power | High | Medium | Low |
| Speed | Good | Good | Excellent |
| Best For | Structured data | Flexible data | Caching |

## Next Steps

Continue learning about:
- [Connection Pooling](./03_connection_pooling.md) - Pool management
- [Transaction Management](./04_transaction_management.md) - ACID transactions
- [PostgreSQL Integration](../03_database_engines/01_postgresql_integration.md) - PostgreSQL setup
