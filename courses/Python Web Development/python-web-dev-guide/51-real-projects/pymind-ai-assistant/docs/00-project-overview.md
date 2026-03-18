# PyMind — Project Overview

## Problem Statement

In the age of AI, users want to interact with their own documents in a natural, conversational way. Traditional keyword search fails to understand context and semantics. Users need:

1. **Semantic understanding** — Find information based on meaning, not exact words
2. **Conversational access** — Ask questions and get answers, not just search results
3. **Private knowledge base** — Use their own documents, not public information
4. **Real-time interaction** — Get streaming responses as the AI generates answers

PyMind solves these problems by combining:
- **Vector embeddings** for semantic search
- **Large Language Models** for natural conversation
- **Retrieval Augmented Generation (RAG)** for grounded, accurate answers

## Target Users

### Primary Persona: Knowledge Worker

**Name:** Sarah, Product Manager
**Background:** Works at a mid-size tech company, has access to dozens of internal documents, wikis, and meeting notes.

**Pain Points:**
- Can't remember which document contains what information
- Keyword search returns irrelevant results
- Spending too much time finding information

**How PyMind Helps:**
- Uploads her documents once
- Asses questions in natural language
- Gets accurate answers with source citations

### Secondary Persona: Developer

**Name:** Alex, Software Engineer
**Background:** Maintains a large codebase with extensive documentation.

**Use Case:**
- Queries internal API docs
- Asks about code patterns
- Gets answers from documentation

## Core User Journeys

### Journey 1: First-Time Setup

```
User Action                    System Response
─────────────────────────────────────────────────────
1. Register account      →   Create user, send confirmation
2. Login                →   Return JWT tokens
3. Explore UI           →   Show empty dashboard
```

### Journey 2: Document Upload

```
User Action                    System Response
─────────────────────────────────────────────────────
1. Click "Upload"        →   Show file picker
2. Select PDF            →   Validate file type/size
3. Submit               →   Save to storage, queue for processing
4. Wait for processing  →   Show progress indicator
5. Processing complete  →   Show "Ready" status
```

### Journey 3: Ask a Question

```
User Action                    System Response
─────────────────────────────────────────────────────
1. Type question         →   Send to API
2. (streaming)          ←   Stream AI response word-by-word
3. Response complete    →   Show answer with sources
4. Ask follow-up       →   Include previous context
```

## Feature Scope

### In Scope (v1.0)

- User registration and JWT authentication
- Document upload (PDF, TXT, MD)
- Automatic document chunking
- OpenAI embeddings generation
- Semantic search over chunks
- RAG-powered chat responses
- Streaming responses via SSE
- Conversation memory
- Per-user document isolation
- Basic test suite

### Out of Scope (v1.0)

- Multi-language support
- Team/organization features
- Real-time collaboration
- Custom embedding models
- On-premise deployment
- Mobile apps

### Deliberately Excluded

- **Web search integration** — Focus on private documents
- **LLM provider choice** — Hardcoded to OpenAI for simplicity
- **Complex file types** — PDF, TXT, MD only initially

## Non-Functional Requirements

### Performance

| Metric | Target |
|--------|--------|
| API response time (p95) | < 200ms |
| Document upload (10MB) | < 5s |
| First token streaming | < 2s |
| Embedding generation | < 3s per 1000 chunks |

### Scale

| Resource | Target |
|----------|--------|
| Concurrent users | 100 |
| Documents per user | 1000 |
| Chunks per document | 10000 |
| Vector dimensions | 1536 |

### Security

- All passwords hashed with bcrypt (cost factor 12)
- JWT tokens expire in 30 minutes (access) / 7 days (refresh)
- All user data strictly isolated
- No PII in logs

## Technology Decision Log

### Why PostgreSQL + pgvector?

**Alternatives Considered:**
- Pinecone — Managed vector DB, but costs add up, data leaves your infrastructure
- Weaviate — Powerful but complex, overkill for this use case
- Qdrant — Good option, but pgvector provides sufficient capability

**Decision:** PostgreSQL with pgvector extension

**Rationale:**
- Single database to manage
- Well-understood, battle-tested
- pgvector provides 99% of use cases
- Easy backup and maintenance

### Why OpenAI?

**Alternatives Considered:**
- Anthropic Claude — Excellent model, but more expensive
- Local models (Llama) — Too slow for production, quality varies
- Self-hosted GPT-J — Requires GPU infrastructure

**Decision:** OpenAI GPT-4o + text-embedding-3-small

**Rationale:**
- Best price/performance for chat
- Embedding model is cheap ($0.02/1M tokens)
- Reliable API with good uptime

### Why Redis for Memory?

**Alternatives Considered:**
- PostgreSQL — Works but slower for high-frequency access
- Memcached — No persistence, no data structures
- In-memory Python dict — Loses data on restart

**Decision:** Redis

**Rationale:**
- Sub-millisecond access
- Native list data structure for message history
- TTL support for automatic expiration
- Persistent with RDB/AOF

### Why FastAPI?

**Alternatives Considered:**
- Django — Too heavy, synchronous by default
- Flask — Requires more boilerplate
- Litestar — Good alternative, but smaller community

**Decision:** FastAPI

**Rationale:**
- First-class async support
- Built-in OpenAPI generation
- Pydantic integration
- Large ecosystem

### Why SQLAlchemy 2.0 Async?

**Alternatives Considered:**
- Django ORM — Tied to Django
- Tortoise ORM — Smaller community
- Raw SQL — Error-prone, hard to maintain

**Decision:** SQLAlchemy 2.0 async

**Rationale:**
- Type-safe queries
- Async native
- Excellent migration support (Alembic)
- Well-documented

## Success Metrics

### User-Facing

- Time to first answer < 30 seconds
- 95% of queries return relevant sources
- < 1% error rate on document upload

### Operational

- 99.9% uptime
- < 1% vector search latency variance
- Complete audit trail for data access

## Future Considerations

### Phase 2 Ideas

- Team workspaces with shared documents
- Custom document loaders (Google Drive, S3)
- Multi-modal documents (images in PDFs)
- Streaming with Vercel AI SDK

### Long-Term Vision

- On-premise deployment option
- Fine-tuned embedding models
- Multi-language RAG
- Voice input/output
