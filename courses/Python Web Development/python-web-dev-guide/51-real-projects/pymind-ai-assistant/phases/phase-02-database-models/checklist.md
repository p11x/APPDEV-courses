# Phase 2 — Database and Models Checklist

Use this checklist to verify your implementation matches the reference code exactly.

## Prerequisites Checklist

- [ ] Phase 1 completed successfully
- [ ] PostgreSQL with pgvector running
- [ ] Alembic installed

## Model Files Checklist

### app/models/base.py

- [ ] Base extends DeclarativeBase
- [ ] TimestampMixin with created_at and updated_at
- [ ] UUIDMixin with uuid4 primary key
- [ ] Proper type annotations with Mapped

### app/models/user.py

- [ ] User class extends Base, UUIDMixin, TimestampMixin
- [ ] __tablename__ = "users"
- [ ] email, username, hashed_password fields
- [ ] is_active, is_verified boolean fields
- [ ] documents relationship with cascade
- [ ] conversations relationship with cascade

### app/models/document.py

- [ ] DocumentStatus enum with PENDING, PROCESSING, COMPLETED, FAILED
- [ ] Document class with proper mixins
- [ ] __tablename__ = "documents"
- [ ] ForeignKey to users.id
- [ ] title, file_name, file_type, file_size fields
- [ ] status enum field
- [ ] error_message optional field
- [ ] total_tokens optional field
- [ ] user relationship (back_populates)
- [ ] chunks relationship with order_by

### app/models/chunk.py

- [ ] Chunk class extends Base, UUIDMixin
- [ ] __tablename__ = "chunks"
- [ ] ForeignKey to documents.id
- [ ] content field (String 10000)
- [ ] embedding field with Vector(1536)
- [ ] chunk_index, token_count fields
- [ ] document relationship
- [ ] IVFFlat index for vector search

### app/models/conversation.py

- [ ] Conversation class extends Base, UUIDMixin, TimestampMixin
- [ ] __tablename__ = "conversations"
- [ ] ForeignKey to users.id
- [ ] title field
- [ ] user relationship
- [ ] messages relationship with order_by

### app/models/message.py

- [ ] MessageRole enum with USER, ASSISTANT, SYSTEM
- [ ] Message class extends Base, UUIDMixin
- [ ] __tablename__ = "messages"
- [ ] ForeignKey to conversations.id
- [ ] role enum field
- [ ] content field (Text)
- [ ] token_count field
- [ ] citations optional field
- [ ] conversation relationship

### app/models/__init__.py

- [ ] Exports all models
- [ ] Exports enums
- [ ] Proper __all__ list

## Database Configuration Checklist

### alembic/env.py

- [ ] Imports Base from app.models
- [ ] Imports settings from app.config
- [ ] get_url() function returns DATABASE_URL
- [ ] run_migrations_offline() implemented
- [ ] run_migrations_online() implemented
- [ ] Proper async engine handling

### alembic.ini (auto-generated)

- [ ] sqlalchemy.url configured
- [ ] script_location points to alembic/
- [ ] pre/post scripts configured

## Migration Checklist

### Generate Migration

```bash
alembic revision --autogenerate -m "Initial schema"
```

- [ ] Migration file created in alembic/versions/
- [ ] Import statements for all models
- [ ] create_table operations for all tables

### Run Migration

```bash
alembic upgrade head
```

- [ ] Migration applies successfully
- [ ] No errors in output
- [ ] All tables created in PostgreSQL

## Verification Checklist

### List Tables

```bash
docker-compose exec postgres psql -U pymind -d pymind -c "\dt"
```

Expected tables:
- users
- documents
- chunks
- conversations
- messages
- alembic_version

### Check Extensions

```bash
docker-compose exec postgres psql -U pymind -d pymind -c "\dx vector"
```

Expected: vector extension enabled

### Check Table Structure

```bash
docker-compose exec postgres psql -U pymind -d pymind -c "\d users"
docker-compose exec postgres psql -U pymind -d pymind -c "\d chunks"
```

Expected: Proper columns and constraints

### Test Relationships

Create a simple test script:

```python
from sqlalchemy import select
from app.models import User, Document
from app.core.database import get_session

async def test_models():
    async for session in get_session():
        # Query should work without errors
        result = await session.execute(select(User))
        users = result.scalars().all()
        print(f"Found {len(users)} users")
        break
```

## Common Issues and Fixes

| Issue | Cause | Fix |
|-------|-------|-----|
| Vector type not found | pgvector not installed | Use pgvector Docker image |
| Foreign key error | Table order wrong | Ensure users created first |
| Migration conflict | Duplicate versions | Check alembic versions |
| Import error | Package not installed | pip install -e . |

## Code Quality Checklist

- [ ] All models use proper type hints
- [ ] All fields have Mapped annotations
- [ ] Relationships properly configured
- [ ] Cascade deletes set correctly
- [ ] Indexes created for foreign keys
- [ ] Vector index for similarity search

## Next Phase Preparation

Before proceeding to Phase 3, ensure:

- [ ] All 5 tables exist in database
- [ ] Relationships work correctly
- [ ] Migrations can be generated
- [ ] You understand model structure
- [ ] Ready to add authentication

## Sign-off

When all items are checked, you have completed Phase 2:

- [ ] All models created and tested
- [ ] Database schema complete
- [ ] Migrations working
- [ ] Ready to proceed to Phase 3
