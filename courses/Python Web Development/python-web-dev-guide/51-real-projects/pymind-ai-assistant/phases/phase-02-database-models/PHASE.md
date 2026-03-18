# Phase 2 — Database and Models

## Goal

By the end of this phase, you will have:
- SQLAlchemy models for all entities (User, Document, Chunk, Conversation, Message)
- pgvector extension for vector similarity search
- Alembic migrations to create tables
- Proper relationships between models

## What You'll Build in This Phase

- [ ] SQLAlchemy models with relationships
- [ ] pgvector extension for embeddings
- [ ] Alembic migration setup
- [ ] Database tables created

## Prerequisites

- Completed Phase 1 (working FastAPI app with PostgreSQL)
- Understanding of SQLAlchemy basics

## Database Schema Overview

```
┌──────────────────┐       ┌──────────────────┐
│      users       │       │    documents     │
├──────────────────┤       ├──────────────────┤
│ id (UUID)        │◄──────│ id (UUID)        │
│ email            │       │ user_id (FK)     │
│ username         │       │ title            │
│ hashed_password  │       │ file_name        │
│ created_at       │       │ file_type        │
│ updated_at       │       │ file_size        │
└──────────────────┘       │ status           │
                           │ created_at        │
                           │ updated_at        │
                           └─────────┬─────────┘
                                     │
                                     │ 1:N
                                     ▼
                           ┌──────────────────┐
                           │      chunks      │
                           ├──────────────────┤
                           │ id (UUID)        │
                           │ document_id (FK) │
                           │ content          │
                           │ embedding (vector)│
                           │ chunk_index      │
                           │ token_count      │
                           │ created_at       │
                           └─────────┬─────────┘
                                     │
                                     │ 1:N
                                     ▼
                           ┌──────────────────┐
                           │  conversations   │
                           ├──────────────────┤
                           │ id (UUID)        │
                           │ user_id (FK)     │
                           │ title            │
                           │ created_at       │
                           │ updated_at       │
                           └─────────┬─────────┘
                                     │
                                     │ 1:N
                                     ▼
                           ┌──────────────────┐
                           │     messages     │
                           ├──────────────────┤
                           │ id (UUID)        │
                           │ conversation_id  │
                           │ role (user/ai)   │
                           │ content          │
                           │ created_at       │
                           └──────────────────┘
```

## Step-by-Step Implementation

### Step 2.1 — Create Base Model

```python
# app/models/base.py
"""
Base model class with common fields for all tables.
Provides UUID primary key and timestamps.
"""
from datetime import datetime, timezone
from uuid import uuid4

from sqlalchemy import DateTime
from sqlalchemy.orm import DeclarativeBase, Mapped, mapped_column
from sqlalchemy.dialects.postgresql import UUID


class Base(DeclarativeBase):
    """Base class for all database models."""
    pass


class TimestampMixin:
    """Mixin that adds created_at and updated_at columns."""
    
    created_at: Mapped[datetime] = mapped_column(
        DateTime(timezone=True),
        default=lambda: datetime.now(timezone.utc),
        nullable=False,
    )
    
    updated_at: Mapped[datetime] = mapped_column(
        DateTime(timezone=True),
        default=lambda: datetime.now(timezone.utc),
        onupdate=lambda: datetime.now(timezone.utc),
        nullable=False,
    )


class UUIDMixin:
    """Mixin that adds UUID primary key."""
    
    id: Mapped[uuid4] = mapped_column(
        UUID(as_uuid=True),
        primary_key=True,
        default=uuid4,
    )
```

🔍 **Line-by-Line Breakdown:**

1. `DeclarativeBase` — SQLAlchemy 2.0 base class for declarative model definitions
2. `TimestampMixin` — Adds `created_at` and `updated_at` auto-columns
3. `mapped_column()` — SQLAlchemy 2.0 column definition
4. `default=lambda:` — Callable default, called on insert
5. `onupdate=` — Auto-update on modification
6. `UUID(as_uuid=True)` — PostgreSQL UUID type, returns Python uuid.UUID

### Step 2.2 — Create User Model

```python
# app/models/user.py
"""
User model for authentication and ownership.
"""
from datetime import datetime
from typing import TYPE_CHECKING

from sqlalchemy import String, Boolean
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.models.base import Base, TimestampMixin, UUIDMixin

if TYPE_CHECKING:
    from app.models.document import Document
    from app.models.conversation import Conversation


class User(Base, UUIDMixin, TimestampMixin):
    """
    User model for authentication.
    
    Each user owns documents and conversations.
    """
    __tablename__ = "users"
    
    # Authentication fields
    email: Mapped[str] = mapped_column(
        String(255),
        unique=True,
        index=True,
        nullable=False,
    )
    
    username: Mapped[str] = mapped_column(
        String(100),
        unique=True,
        index=True,
        nullable=False,
    )
    
    hashed_password: Mapped[str] = mapped_column(
        String(255),
        nullable=False,
    )
    
    is_active: Mapped[bool] = mapped_column(
        Boolean,
        default=True,
        nullable=False,
    )
    
    is_verified: Mapped[bool] = mapped_column(
        Boolean,
        default=False,
        nullable=False,
    )
    
    # Relationships
    documents: Mapped[list["Document"]] = relationship(
        "Document",
        back_populates="user",
        cascade="all, delete-orphan",
    )
    
    conversations: Mapped[list["Conversation"]] = relationship(
        "Conversation",
        back_populates="user",
        cascade="all, delete-orphan",
    )
    
    def __repr__(self) -> str:
        return f"<User(id={self.id}, email={self.email})>"
```

🔍 **Line-by-Line Breakdown:**

1. `__tablename__` — Table name in PostgreSQL
2. `Mapped[str]` — Type hint for SQLAlchemy column
3. `unique=True` — Creates UNIQUE constraint
4. `index=True` — Creates B-tree index for faster lookups
5. `relationship()` — SQLAlchemy ORM relationship
6. `cascade="all, delete-orphan"` — Deletes related records on parent delete

### Step 2.3 — Create Document Model

```python
# app/models/document.py
"""
Document model for uploaded files.
"""
from datetime import datetime
from enum import Enum
from typing import TYPE_CHECKING, Optional

from sqlalchemy import String, Integer, ForeignKey, Enum as SQLEnum
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.models.base import Base, TimestampMixin, UUIDMixin

if TYPE_CHECKING:
    from app.models.user import User
    from app.models.chunk import Chunk


class DocumentStatus(str, Enum):
    """Document processing status."""
    PENDING = "pending"
    PROCESSING = "processing"
    COMPLETED = "completed"
    FAILED = "failed"


class Document(Base, UUIDMixin, TimestampMixin):
    """
    Document model representing uploaded files.
    
    Documents are owned by users and contain multiple chunks
    after processing.
    """
    __tablename__ = "documents"
    
    # Ownership
    user_id: Mapped[str] = mapped_column(
        ForeignKey("users.id", ondelete="CASCADE"),
        nullable=False,
        index=True,
    )
    
    # File information
    title: Mapped[str] = mapped_column(
        String(500),
        nullable=False,
    )
    
    file_name: Mapped[str] = mapped_column(
        String(500),
        nullable=False,
    )
    
    file_type: Mapped[str] = mapped_column(
        String(100),
        nullable=False,
    )
    
    file_size: Mapped[int] = mapped_column(
        Integer,
        nullable=False,
    )
    
    # Processing status
    status: Mapped[DocumentStatus] = mapped_column(
        SQLEnum(DocumentStatus),
        default=DocumentStatus.PENDING,
        nullable=False,
    )
    
    error_message: Mapped[Optional[str]] = mapped_column(
        String(1000),
        nullable=True,
    )
    
    # Token count (after processing)
    total_tokens: Mapped[Optional[int]] = mapped_column(
        Integer,
        nullable=True,
    )
    
    # Relationships
    user: Mapped["User"] = relationship(
        "User",
        back_populates="documents",
    )
    
    chunks: Mapped[list["Chunk"]] = relationship(
        "Chunk",
        back_populates="document",
        cascade="all, delete-orphan",
        order_by="Chunk.chunk_index",
    )
    
    def __repr__(self) -> str:
        return f"<Document(id={self.id}, title={self.title})>"
```

🔍 **Line-by-Line Breakdown:**

1. `str, Enum` — Enum that can serialize to string for JSON/PostgreSQL
2. `ForeignKey()` — PostgreSQL foreign key with CASCADE delete
3. `SQLEnum()` — PostgreSQL enum type
4. `nullable=True` — Optional field (can be None)
5. `order_by="Chunk.chunk_index"` — Default ordering for chunks

### Step 2.4 — Create Chunk Model with Vector Embedding

```python
# app/models/chunk.py
"""
Chunk model with vector embeddings for semantic search.
"""
from typing import TYPE_CHECKING, Optional

from sqlalchemy import String, Integer, ForeignKey, Index
from sqlalchemy.dialects.postgresql import UUID, ARRAY, Vector
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.models.base import Base, UUIDMixin

if TYPE_CHECKING:
    from app.models.document import Document


class Chunk(Base, UUIDMixin):
    """
    Document chunk with vector embedding for semantic search.
    
    Each document is split into chunks, each with an embedding
    vector for similarity search.
    """
    __tablename__ = "chunks"
    
    # Ownership
    document_id: Mapped[str] = mapped_column(
        ForeignKey("documents.id", ondelete="CASCADE"),
        nullable=False,
        index=True,
    )
    
    # Content
    content: Mapped[str] = mapped_column(
        String(10000),  # Max chunk size
        nullable=False,
    )
    
    # Embedding vector (1536 dimensions for text-embedding-3-small)
    embedding: Mapped[Optional[list[float]]] = mapped_column(
        Vector(1536),
        nullable=True,  # Null until processed
    )
    
    # Metadata
    chunk_index: Mapped[int] = mapped_column(
        Integer,
        nullable=False,
    )
    
    token_count: Mapped[Optional[int]] = mapped_column(
        Integer,
        nullable=True,
    )
    
    # Relationships
    document: Mapped["Document"] = relationship(
        "Document",
        back_populates="chunks",
    )
    
    # Index for vector similarity search
    __table_args__ = (
        Index(
            "ix_chunks_user_embedding",
            "document_id",
            postgresql_using="ivfflat",
            postgresql_with={"lists": 100},
            postgresql_ops={"document_id": "int4_ops"},
        ),
    )
    
    def __repr__(self) -> str:
        return f"<Chunk(id={self.id}, index={self.chunk_index})>"
```

🔍 **Line-by-Line Breakdown:**

1. `Vector(1536)` — pgvector column for 1536-dimensional embeddings
2. `nullable=True` — Embedding populated after processing
3. `Index` — PostgreSQL index for vector search (IVFFlat algorithm)
4. `postgresql_using="ivfflat"` — Approximate nearest neighbor index

### Step 2.5 — Create Conversation Model

```python
# app/models/conversation.py
"""
Conversation model for chat sessions.
"""
from typing import TYPE_CHECKING

from sqlalchemy import String, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.models.base import Base, TimestampMixin, UUIDMixin

if TYPE_CHECKING:
    from app.models.user import User
    from app.models.message import Message


class Conversation(Base, UUIDMixin, TimestampMixin):
    """
    Conversation model representing a chat session.
    
    Each conversation belongs to a user and contains messages.
    """
    __tablename__ = "conversations"
    
    # Ownership
    user_id: Mapped[str] = mapped_column(
        ForeignKey("users.id", ondelete="CASCADE"),
        nullable=False,
        index=True,
    )
    
    # Conversation info
    title: Mapped[str] = mapped_column(
        String(500),
        nullable=False,
    )
    
    # Relationships
    user: Mapped["User"] = relationship(
        "User",
        back_populates="conversations",
    )
    
    messages: Mapped[list["Message"]] = relationship(
        "Message",
        back_populates="conversation",
        cascade="all, delete-orphan",
        order_by="Message.created_at",
    )
    
    def __repr__(self) -> str:
        return f"<Conversation(id={self.id}, title={self.title})>"
```

### Step 2.6 — Create Message Model

```python
# app/models/message.py
"""
Message model for individual chat messages.
"""
from enum import Enum
from typing import TYPE_CHECKING

from sqlalchemy import String, ForeignKey, Enum as SQLEnum, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.models.base import Base, UUIDMixin

if TYPE_CHECKING:
    from app.models.conversation import Conversation


class MessageRole(str, Enum):
    """Message sender role."""
    USER = "user"
    ASSISTANT = "assistant"
    SYSTEM = "system"


class Message(Base, UUIDMixin):
    """
    Message model for individual chat messages.
    
    Stores the conversation history for RAG context.
    """
    __tablename__ = "messages"
    
    # Ownership
    conversation_id: Mapped[str] = mapped_column(
        ForeignKey("conversations.id", ondelete="CASCADE"),
        nullable=False,
        index=True,
    )
    
    # Content
    role: Mapped[MessageRole] = mapped_column(
        SQLEnum(MessageRole),
        nullable=False,
    )
    
    content: Mapped[str] = mapped_column(
        Text,
        nullable=False,
    )
    
    # Optional: Token count for cost tracking
    token_count: Mapped[int] = mapped_column(
        nullable=True,
    )
    
    # Optional: Citations from RAG (JSON string)
    citations: Mapped[str] = mapped_column(
        Text,
        nullable=True,
    )
    
    # Relationships
    conversation: Mapped["Conversation"] = relationship(
        "Conversation",
        back_populates="messages",
    )
    
    def __repr__(self) -> str:
        return f"<Message(id={self.id}, role={self.role})>"
```

### Step 2.7 — Create Models __init__.py

```python
# app/models/__init__.py
"""
Database models for PyMind application.
"""
from app.models.base import Base, TimestampMixin, UUIDMixin
from app.models.user import User
from app.models.document import Document, DocumentStatus
from app.models.chunk import Chunk
from app.models.conversation import Conversation
from app.models.message import Message, MessageRole

__all__ = [
    "Base",
    "TimestampMixin", 
    "UUIDMixin",
    "User",
    "Document",
    "DocumentStatus",
    "Chunk",
    "Conversation",
    "Message",
    "MessageRole",
]
```

### Step 2.8 — Configure Alembic

```python
# alembic/env.py
"""
Alembic environment configuration for database migrations.
"""
from logging.config import fileConfig

from sqlalchemy import engine_from_config, pool
from alembic import context

# Import models for autogenerate
from app.models import Base
from app.config import get_settings

# Alembic Config object
config = context.config

# Setup logging
if config.config_file_name is not None:
    fileConfig(config.config_file_name)

# Target metadata for autogenerate
target_metadata = Base.metadata

# Get settings
settings = get_settings()


def get_url() -> str:
    """Get database URL from settings."""
    return settings.DATABASE_URL


def run_migrations_offline() -> None:
    """Run migrations in 'offline' mode."""
    url = get_url()
    context.configure(
        url=url,
        target_metadata=target_metadata,
        literal_binds=True,
        dialect_opts={"paramstyle": "named"},
    )
    
    with context.begin_transaction():
        context.run_migrations()


def run_migrations_online() -> None:
    """Run migrations in 'online' mode."""
    configuration = config.get_section(config.config_ini_section)
    configuration["sqlalchemy.url"] = get_url()
    
    connectable = engine_from_config(
        configuration,
        prefix="sqlalchemy.",
        poolclass=pool.NullPool,
    )
    
    with connectable.connect() as connection:
        context.configure(
            connection=connection,
            target_metadata=target_metadata,
        )
        
        with context.begin_transaction():
            context.run_migrations()


if context.is_offline_mode():
    run_migrations_offline()
else:
    run_migrations_online()
```

### Step 2.9 — Create Initial Migration

```bash
# Generate initial migration
alembic revision --autogenerate -m "Initial schema"

# Apply migration
alembic upgrade head
```

### Step 2.10 — Enable pgvector Extension

```sql
-- Create extension (run manually or in first migration)
CREATE EXTENSION IF NOT EXISTS vector;
```

## How It All Connects

```
┌─────────────────────────────────────────────────────────────────┐
│                    Database Relationships                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  User (1)                                                       │
│    │                                                            │
│    ├── documents (N) ──── Document                             │
│    │                      │                                     │
│    │                      └── chunks (N) ──── Chunk            │
│    │                                         │                 │
│    │                                         └── embedding     │
│    │                                            (Vector)       │
│    │                                                            │
│    └── conversations (N) ──── Conversation                      │
│                               │                                  │
│                               └── messages (N) ──── Message     │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Testing This Phase

### Create Tables

```bash
# Generate migration
alembic revision --autogenerate -m "Initial schema"

# Run migration
alembic upgrade head
```

### Verify Tables

```bash
# Connect to database
docker-compose exec postgres psql -U pymind -d pymind

# List tables
\dt

# Check vector extension
\dx vector

# Check table structure
\d users
\d documents
\d chunks
```

### Test Relationships

```python
# Quick test of relationships
from app.models import User, Document, Chunk
from app.core.database import get_session

async def test_relationships():
    async for session in get_session():
        # Query user with documents
        user = session.query(User).first()
        for doc in user.documents:
            print(f"Document: {doc.title}")
            for chunk in doc.chunks:
                print(f"  Chunk: {chunk.chunk_index}")
        break
```

## Common Errors in This Phase

### Error 1: pgvector Extension Not Found

```
psycopg2.errors.UndefinedObject: type "vector" does not exist
```

**Fix:** Enable pgvector extension:
```sql
CREATE EXTENSION IF NOT EXISTS vector;
```

### Error 2: Vector Dimension Mismatch

```
psycopg2.errors.StringDataRightTruncation: value too long for type vector(1536)
```

**Fix:** Ensure embedding dimension matches model (1536 for text-embedding-3-small)

### Error 3: Migration Conflicts

**Fix:** Check for duplicate migrations or conflicting alembic versions

## Phase Summary

**What was built:**
- SQLAlchemy models with proper relationships
- pgvector extension for embeddings
- Alembic migration system
- All database tables created

**What was learned:**
- SQLAlchemy 2.0 async patterns
- PostgreSQL UUID and array types
- pgvector for similarity search
- Alembic migrations

## Next Phase

→ Phase 3 — Authentication: Implement JWT authentication, password hashing, and user registration/login endpoints.
