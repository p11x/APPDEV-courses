# Database Design

## Entity Relationship Diagram

```
┌──────────────────────────────────────────────────────────────────────────────┐
│                              DATABASE SCHEMA                                  │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌──────────────┐      1:N     ┌──────────────────┐      1:N   ┌─────────┐│
│  │    users     │──────────────▶│    documents     │────────────▶│ chunks  ││
│  └──────────────┘              └──────────────────┘              └─────────┘│
│        │                               │                                 │      │
│        │                               │                                 │      │
│        │      1:N                     │                          ┌─────┴─────┤
│        ▼                               ▼                          │           │
│  ┌──────────────────┐       ┌────────────────────┐               │  vector   │
│  │  conversations   │       │  document_chunks   │               │ (1536 dims│
│  └────────┬─────────┘       │  (join table)      │               │  column)  │
│           │                  └────────────────────┘               │           │
│           │                                                        │           │
│           │ 1:N                                                    │           │
│           ▼                                                        │           │
│  ┌──────────────────┐                                              │           │
│  │    messages      │                                              │           │
│  └──────────────────┘                                              │           │
│                                                                       │           │
└──────────────────────────────────────────────────────────────────────┘
```

## Tables

### users

Stores user account information.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `id` | UUID | PK | Unique user identifier |
| `email` | VARCHAR(255) | UNIQUE, NOT NULL | User email |
| `hashed_password` | VARCHAR(255) | NOT NULL | Bcrypt hash |
| `is_active` | BOOLEAN | DEFAULT TRUE | Account active status |
| `created_at` | TIMESTAMP | NOT NULL | Account creation time |
| `updated_at` | TIMESTAMP | NOT NULL | Last update time |

**Indexes:**
- `idx_users_email` on `email` (unique)

### documents

Stores uploaded document metadata.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `id` | UUID | PK | Unique document identifier |
| `user_id` | UUID | FK → users.id, NOT NULL | Owner |
| `filename` | VARCHAR(500) | NOT NULL | Original filename |
| `file_type` | VARCHAR(50) | NOT NULL | MIME type |
| `file_size` | BIGINT | NOT NULL | Size in bytes |
| `status` | VARCHAR(20) | NOT NULL | processing status |
| `error_message` | TEXT | NULLABLE | Error if failed |
| `created_at` | TIMESTAMP | NOT NULL | Upload time |
| `updated_at` | TIMESTAMP | NOT NULL | Last update time |

**Indexes:**
- `idx_documents_user_id` on `user_id`
- `idx_documents_status` on `status`

**Status Values:**
- `pending` - Uploaded, waiting for processing
- `processing` - Currently being chunked/embedded
- `ready` - Fully processed and searchable
- `failed` - Processing failed

### chunks

Stores document chunks with embeddings.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `id` | UUID | PK | Unique chunk identifier |
| `document_id` | UUID | FK → documents.id, NOT NULL | Parent document |
| `user_id` | UUID | FK → users.id, NOT NULL | Owner (denormalized) |
| `content` | TEXT | NOT NULL | Chunk text content |
| `embedding` | VECTOR(1536) | NOT NULL | OpenAI embedding |
| `chunk_index` | INTEGER | NOT NULL | Order in document |
| `token_count` | INTEGER | NOT NULL | Estimated tokens |
| `created_at` | TIMESTAMP | NOT NULL | Creation time |

**Indexes:**
- `idx_chunks_document_id` on `document_id`
- `idx_chunks_user_id` on `user_id`
- `idx_chunks_embedding` using HNSW (for similarity search)

### conversations

Stores chat conversations.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `id` | UUID | PK | Unique conversation identifier |
| `user_id` | UUID | FK → users.id, NOT NULL | Owner |
| `title` | VARCHAR(255) | NOT NULL | Conversation title |
| `created_at` | TIMESTAMP | NOT NULL | Creation time |

**Indexes:**
- `idx_conversations_user_id` on `user_id`

### messages

Stores individual chat messages.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `id` | UUID | PK | Unique message identifier |
| `conversation_id` | UUID | FK → conversations.id | Parent conversation |
| `role` | VARCHAR(20) | NOT NULL | user or assistant |
| `content` | TEXT | NOT NULL | Message content |
| `sources` | JSONB | NULLABLE | Retrieved sources |
| `created_at` | TIMESTAMP | NOT NULL | Creation time |

**Indexes:**
- `idx_messages_conversation_id` on `conversation_id`

## pgvector Extension Setup

pgvector enables vector similarity search directly in PostgreSQL.

```sql
-- Enable extension (must be superuser or extension already installed)
CREATE EXTENSION IF NOT EXISTS vector;

-- Verify installation
SELECT * FROM pg_extension WHERE extname = 'vector';
```

### Indexing Strategy

For efficient similarity search, we use HNSW (Hierarchical Navigable Small World) index:

```sql
-- Create HNSW index on chunks
CREATE INDEX idx_chunks_embedding 
ON chunks 
USING hnsw (embedding vector_cosine_ops)
WITH (m = 16, ef_construction = 64);

-- m: number of connections per layer (default 16)
-- ef_construction: construction time parameter (default 64)
```

**Why HNSW over IVFFlat?**
- HNSW provides better query quality (higher recall)
- HNSW has faster query times at scale
- IVFFlat requires training data to be organized

**Parameters:**
- `m` (16): More connections = better quality, more memory
- `ef_construction` (64): Higher = better quality, slower build

## Vector Similarity Metrics

pgvector supports three distance metrics:

| Metric | Description | Best For |
|--------|-------------|----------|
| `vector_cosine_ops` | Cosine distance | Semantic similarity |
| `vector_l2_ops` | Euclidean distance | Geometric similarity |
| `inner_product` | Dot product | When magnitude matters |

**Decision:** Cosine distance (`vector_cosine_ops`)

**Rationale:**
- OpenAI embeddings are already normalized
- Best for semantic search use cases
- Works well with text data

## Why pgvector Over Alternatives?

### Comparison

| Feature | pgvector | Pinecone | Weaviate |
|---------|----------|----------|-----------|
| Deployment | Self-hosted / Cloud | Cloud only | Both |
| Cost | $$ | $$$ | $$ |
| Latency | <5ms | <10ms | <5ms |
| Data privacy | Full control | Vendor | Full control |
| Ease of setup | Medium | Easy | Medium |
| Scale | 1M+ vectors | Unlimited | 1M+ vectors |

**Decision for PyMind:**
- Single database to manage
- Data stays in user's infrastructure
- Sufficient for our scale (100 users × 10K chunks)
- No additional vendor dependencies

## Migration Strategy

### Alembic Setup

Alembic provides version-controlled database migrations:

```
alembic/
├── env.py           # Migration environment
├── script.py.mako   # Migration template
└── versions/
    ├── 001_initial_schema.py
    └── ...
```

### Migration Commands

```bash
# Create a new migration
alembic revision --autogenerate -m "add users table"

# Run migrations
alembic upgrade head

# Rollback one migration
alembic downgrade -1

# Show migration history
alembic history
```

## Indexing Strategy

### Primary Indexes

| Table | Index | Purpose |
|-------|-------|---------|
| users | `idx_users_email` | Login lookups |
| documents | `idx_documents_user_id` | User's documents |
| chunks | `idx_chunks_user_id` | User's chunks |
| chunks | `idx_chunks_embedding` | Vector similarity |
| conversations | `idx_conversations_user_id` | User's conversations |
| messages | `idx_messages_conversation_id` | Conversation history |

### Query Patterns

**Document listing:**
```sql
SELECT * FROM documents 
WHERE user_id = $1 
ORDER BY created_at DESC 
LIMIT 20;
```

**Similarity search:**
```sql
SELECT id, content, 
       (embedding <=> $query_embedding) as distance 
FROM chunks 
WHERE user_id = $1 
ORDER BY embedding <=> $query_embedding 
LIMIT 5;
```

## Data Retention

### Policy

- **Active users:** No automatic deletion
- **Failed documents:** Delete after 30 days
- **Conversations:** Keep for 1 year, archive after

### Cleanup Queries

```sql
-- Delete failed documents older than 30 days
DELETE FROM documents 
WHERE status = 'failed' 
AND created_at < NOW() - INTERVAL '30 days';

-- Archive old conversations (move to cold storage)
```

### Chunk Deletion (Cascade)

```sql
-- When document is deleted, chunks are cascade-deleted
ON DELETE CASCADE
```

## Constraints

### Foreign Keys

All foreign keys use `ON DELETE CASCADE` for automatic cleanup:
- `documents.user_id` → users.id
- `chunks.document_id` → documents.id
- `chunks.user_id` → users.id
- `conversations.user_id` → users.id
- `messages.conversation_id` → conversations.id

### Check Constraints

```sql
-- Document status must be valid
ALTER TABLE documents 
ADD CONSTRAINT valid_status 
CHECK (status IN ('pending', 'processing', 'ready', 'failed'));

-- Message role must be valid
ALTER TABLE messages 
ADD CONSTRAINT valid_role 
CHECK (role IN ('user', 'assistant'));
```

## Security Considerations

### Row-Level Security

For multi-tenancy (per-user isolation), we implement at application level:
- All queries filter by `user_id` from JWT
- No direct foreign key access between users

### Sensitive Data

- Passwords are bcrypt hashed (cost factor 12)
- No PII in vector embeddings
- Error messages sanitized before storage
