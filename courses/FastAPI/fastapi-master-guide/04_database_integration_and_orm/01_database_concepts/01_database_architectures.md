# Database Architectures

## Overview

Choosing the right database architecture is critical for application success. This guide covers different database types, the CAP theorem, and selection criteria for FastAPI applications.

## Database Types

### Relational Databases (SQL)

```python
# Example 1: Relational database with SQLAlchemy
from sqlalchemy import create_engine, Column, Integer, String, ForeignKey
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import relationship, Session

Base = declarative_base()

class User(Base):
    """Relational model with foreign keys"""
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50), unique=True, nullable=False)
    email = Column(String(100), unique=True, nullable=False)

    # Relationship to orders (one-to-many)
    orders = relationship("Order", back_populates="user")

class Order(Base):
    """Related table with foreign key"""
    __tablename__ = "orders"

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey("users.id"))
    total = Column(Integer)

    # Relationship back to user
    user = relationship("User", back_populates="orders")

# Relational database strengths:
# - ACID compliance (Atomicity, Consistency, Isolation, Durability)
# - Complex queries with JOINs
# - Data integrity through constraints
# - Mature ecosystem and tooling
```

### Document Databases (NoSQL)

```python
# Example 2: Document database with MongoDB (motor for async)
from motor.motor_asyncio import AsyncIOMotorClient
from pydantic import BaseModel, Field
from typing import Optional, List
from bson import ObjectId

# MongoDB connection
client = AsyncIOMotorClient("mongodb://localhost:27017")
db = client.fastapi_db

class UserDocument(BaseModel):
    """Document model - no fixed schema"""
    id: Optional[str] = Field(default_factory=lambda: str(ObjectId()))
    username: str
    email: str
    orders: List[dict] = []  # Embedded documents

    class Config:
        json_encoders = {ObjectId: str}

# Create user with embedded orders
async def create_user_with_orders():
    user = {
        "username": "john",
        "email": "john@example.com",
        "orders": [
            {"product": "Laptop", "quantity": 1, "price": 999},
            {"product": "Mouse", "quantity": 2, "price": 25}
        ]
    }
    result = await db.users.insert_one(user)
    return str(result.inserted_id)

# Document database strengths:
# - Flexible schema
# - Horizontal scaling
# - Fast reads for document access
# - Embedded data reduces JOINs
```

### Key-Value Databases

```python
# Example 3: Key-value store with Redis
import redis.asyncio as redis
import json
from typing import Any, Optional

# Redis connection
redis_client = redis.Redis(host='localhost', port=6379, decode_responses=True)

class KeyValueStore:
    """Redis key-value operations"""

    @staticmethod
    async def set_key(key: str, value: Any, expire: int = 3600):
        """Set key with expiration"""
        await redis_client.setex(key, expire, json.dumps(value))

    @staticmethod
    async def get_key(key: str) -> Optional[Any]:
        """Get value by key"""
        value = await redis_client.get(key)
        return json.loads(value) if value else None

    @staticmethod
    async def delete_key(key: str):
        """Delete key"""
        await redis_client.delete(key)

    @staticmethod
    async def increment(key: str, amount: int = 1) -> int:
        """Atomic increment"""
        return await redis_client.incrby(key, amount)

store = KeyValueStore()

# Key-value database strengths:
# - Extremely fast (in-memory)
# - Simple operations
# - Great for caching
# - Session storage
```

## CAP Theorem

### Understanding the CAP Theorem

```python
# Example 4: CAP theorem demonstration
"""
CAP Theorem: You can only guarantee 2 out of 3:

C - Consistency: Every read gets the most recent write
A - Availability: Every request gets a response
P - Partition Tolerance: System works despite network failures

Database choices based on CAP:

CP (Consistency + Partition Tolerance):
- MongoDB (configurable)
- HBase
- Redis Cluster
- Use when: Data accuracy is critical

AP (Availability + Partition Tolerance):
- Cassandra
- DynamoDB
- CouchDB
- Use when: Uptime is critical, eventual consistency OK

CA (Consistency + Availability):
- PostgreSQL
- MySQL
- Traditional RDBMS
- Use when: Network partitions are rare
"""

class CAPDatabase:
    """CAP theorem database selection guide"""

    RECOMMENDATIONS = {
        "financial_transactions": {
            "choice": "CP",
            "databases": ["PostgreSQL", "MongoDB"],
            "reason": "Data consistency is critical"
        },
        "social_media_feed": {
            "choice": "AP",
            "databases": ["Cassandra", "DynamoDB"],
            "reason": "Availability more important than immediate consistency"
        },
        "session_storage": {
            "choice": "AP",
            "databases": ["Redis", "Memcached"],
            "reason": "Fast access, eventual consistency acceptable"
        },
        "user_authentication": {
            "choice": "CP",
            "databases": ["PostgreSQL", "MongoDB"],
            "reason": "Security requires consistency"
        }
    }
```

## Database Selection Criteria

### Decision Framework

```python
# Example 5: Database selection criteria
from dataclasses import dataclass
from typing import List
from enum import Enum

class DataPattern(Enum):
    STRUCTURED = "structured"
    SEMI_STRUCTURED = "semi_structured"
    UNSTRUCTURED = "unstructured"

class QueryPattern(Enum):
    SIMPLE_KEY_VALUE = "simple_key_value"
    COMPLEX_JOINS = "complex_joins"
    FULL_TEXT_SEARCH = "full_text_search"
    GRAPH_TRAVERSAL = "graph_traversal"
    TIME_SERIES = "time_series"

@dataclass
class DatabaseRequirement:
    """Database selection criteria"""
    data_pattern: DataPattern
    query_pattern: QueryPattern
    read_write_ratio: float  # 0.0 = write-heavy, 1.0 = read-heavy
    consistency_required: bool
    scalability_needs: str  # "vertical", "horizontal", "both"
    budget: str  # "low", "medium", "high"

def recommend_database(req: DatabaseRequirement) -> dict:
    """Recommend database based on requirements"""

    recommendations = []

    if req.data_pattern == DataPattern.STRUCTURED:
        if req.query_pattern == QueryPattern.COMPLEX_JOINS:
            recommendations.append({
                "database": "PostgreSQL",
                "reason": "Best for complex SQL queries",
                "trade_off": "Vertical scaling limits"
            })
            recommendations.append({
                "database": "MySQL",
                "reason": "Good read performance",
                "trade_off": "Less feature-rich than PostgreSQL"
            })

    if req.data_pattern == DataPattern.SEMI_STRUCTURED:
        recommendations.append({
            "database": "MongoDB",
            "reason": "Flexible document schema",
            "trade_off": "Less ACID guarantees"
        })

    if req.query_pattern == QueryPattern.SIMPLE_KEY_VALUE:
        recommendations.append({
            "database": "Redis",
            "reason": "Extremely fast key-value access",
            "trade_off": "Memory-bound, not for large datasets"
        })

    if req.query_pattern == QueryPattern.TIME_SERIES:
        recommendations.append({
            "database": "InfluxDB",
            "reason": "Optimized for time-series data",
            "trade_off": "Specialized use case"
        })

    if req.scalability_needs == "horizontal":
        recommendations.append({
            "database": "Cassandra",
            "reason": "Excellent horizontal scaling",
            "trade_off": "Complex operational overhead"
        })

    return {
        "requirements": req,
        "recommendations": recommendations
    }
```

## Scalability Patterns

### Scaling Strategies

```python
# Example 6: Database scalability patterns
from fastapi import FastAPI
import asyncio

app = FastAPI()

class ScalabilityPatterns:
    """Database scalability patterns"""

    # Pattern 1: Read Replicas
    """
    Master Database (writes)
         |
    +------+------+
    |             |
Replica 1    Replica 2 (reads)

Use case: Read-heavy applications
Implementation: Route reads to replicas
"""

    # Pattern 2: Sharding
    """
    Application
         |
    Shard Router
    +-----+-----+
    |     |     |
Shard1 Shard2 Shard3
(User A-C) (User D-F) (User G-Z)

Use case: Large datasets, write scaling
Implementation: Distribute data by key
"""

    # Pattern 3: Caching Layer
    """
    Application
         |
    Cache (Redis)
         |
    Database

Use case: Repeated queries
Implementation: Cache-aside pattern
"""

    # Pattern 4: Database per Service
    """
    Service A          Service B
       |                  |
   Database A         Database B

Use case: Microservices
Implementation: Each service owns its data
"""

# Implementation of read replicas
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

class DatabaseWithReplicas:
    """Database with read replica support"""

    def __init__(self, master_url: str, replica_urls: list[str]):
        self.master_engine = create_engine(master_url)
        self.master_session = sessionmaker(bind=self.master_engine)

        self.replica_engines = [create_engine(url) for url in replica_urls]
        self.replica_sessions = [sessionmaker(bind=e) for e in self.replica_engines]
        self._replica_index = 0

    def get_write_session(self):
        """Get session for write operations"""
        return self.master_session()

    def get_read_session(self):
        """Get session for read operations (round-robin)"""
        session = self.replica_sessions[self._replica_index]()
        self._replica_index = (self._replica_index + 1) % len(self.replica_sessions)
        return session

# Usage
db = DatabaseWithReplicas(
    master_url="postgresql://user:pass@master:5432/db",
    replica_urls=[
        "postgresql://user:pass@replica1:5432/db",
        "postgresql://user:pass@replica2:5432/db"
    ]
)
```

## Trade-offs Analysis

### Comparison Matrix

```python
# Example 7: Database comparison
"""
Database Trade-offs Comparison:

Feature         | PostgreSQL | MongoDB | Redis  | Cassandra
----------------|------------|---------|--------|----------
ACID            | ✓ Full     | Partial | Partial| ✗ No
Scalability     | Vertical   | Both    | Both   | Horizontal
Query Power     | ✓ High     | Medium  | Low    | Low
Schema          | Rigid      | Flexible| N/A    | Flexible
Joins           | ✓ Yes      | Limited | ✗ No   | ✗ No
Speed (reads)   | Good       | Good    | ✓ Fast | Good
Speed (writes)  | Good       | Good    | ✓ Fast | ✓ Fast
Learning Curve  | Medium     | Low     | Low    | High
Operational     | Medium     | Medium  | Low    | High
Cost            | Low        | Medium  | Medium | High
"""

from dataclasses import dataclass

@dataclass
class DatabaseProfile:
    name: str
    acid_compliance: str
    scalability: str
    query_power: str
    read_speed: str
    write_speed: str
    operational_cost: str

PROFILES = {
    "postgresql": DatabaseProfile(
        name="PostgreSQL",
        acid_compliance="full",
        scalability="vertical",
        query_power="high",
        read_speed="good",
        write_speed="good",
        operational_cost="medium"
    ),
    "mongodb": DatabaseProfile(
        name="MongoDB",
        acid_compliance="partial",
        scalability="both",
        query_power="medium",
        read_speed="good",
        write_speed="good",
        operational_cost="medium"
    ),
    "redis": DatabaseProfile(
        name="Redis",
        acid_compliance="partial",
        scalability="both",
        query_power="low",
        read_speed="excellent",
        write_speed="excellent",
        operational_cost="low"
    ),
    "cassandra": DatabaseProfile(
        name="Cassandra",
        acid_compliance="none",
        scalability="horizontal",
        query_power="low",
        read_speed="good",
        write_speed="excellent",
        operational_cost="high"
    )
}
```

## Best Practices

### Database Selection Guidelines

```python
# Example 8: Best practices for database selection
"""
Database Selection Best Practices:

1. Start with PostgreSQL for most applications
   - Mature, reliable, feature-rich
   - Good for structured data with relationships

2. Add Redis for caching and sessions
   - Complement your primary database
   - Not a replacement for persistent storage

3. Consider MongoDB for:
   - Rapidly evolving schemas
   - Document-heavy data
   - Horizontal scaling needs

4. Use specialized databases for specialized needs:
   - Elasticsearch for full-text search
   - InfluxDB for time-series
   - Neo4j for graph data

5. Avoid over-engineering:
   - Don't use NoSQL just because it's trendy
   - SQL databases handle most use cases well
   - Polyglot persistence adds complexity
"""

# Recommended stack for FastAPI
RECOMMENDED_FASTAPI_STACK = {
    "primary": {
        "database": "PostgreSQL",
        "driver": "asyncpg",
        "orm": "SQLAlchemy 2.0"
    },
    "cache": {
        "database": "Redis",
        "driver": "redis-py",
        "use_cases": ["sessions", "rate_limiting", "caching"]
    },
    "search": {
        "database": "Elasticsearch",
        "driver": "elasticsearch-py",
        "use_cases": ["full-text search", "analytics"]
    },
    "queue": {
        "database": "Redis or RabbitMQ",
        "use_cases": ["background tasks", "message queuing"]
    }
}
```

## Summary

| Database Type | Best For | Examples |
|---------------|----------|----------|
| Relational | Structured data, ACID | PostgreSQL, MySQL |
| Document | Flexible schema | MongoDB, CouchDB |
| Key-Value | Caching, sessions | Redis, Memcached |
| Column-Family | Time-series, logs | Cassandra, HBase |
| Graph | Relationships | Neo4j, ArangoDB |

## Next Steps

Continue learning about:
- [Relational vs NoSQL](./02_relational_vs_nosql.md) - Detailed comparison
- [Connection Pooling](./03_connection_pooling.md) - Pool management
- [Transaction Management](./04_transaction_management.md) - ACID transactions
