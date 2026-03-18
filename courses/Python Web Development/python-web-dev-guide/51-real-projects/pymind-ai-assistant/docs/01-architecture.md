# Architecture

## System Architecture Overview

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              CLIENT (React/Vue)                              │
│                   https://github.com/pymind/pymind-web                     │
└─────────────────────────────────────────────────────────────────────────────┘
                                    │ HTTPS
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                         FASTAPI APPLICATION                                  │
│                          http://localhost:8000                              │
│                                                                              │
│  ┌────────────────────────────────────────────────────────────────────┐    │
│  │                        API Routers                                  │    │
│  │  ┌─────────────┐  ┌─────────────────┐  ┌─────────────────────┐   │    │
│  │  │ Auth Router │  │ Document Router │  │    Chat Router     │   │    │
│  │  └─────────────┘  └─────────────────┘  └─────────────────────┘   │    │
│  └────────────────────────────────────────────────────────────────────┘    │
│                                    │                                        │
│                                    ▼                                        │
│  ┌────────────────────────────────────────────────────────────────────┐    │
│  │                       Service Layer                                 │    │
│  │  ┌──────────┐ ┌──────────────┐ ┌────────────┐ ┌──────────────┐ │    │
│  │  │   Auth   │ │  Document    │ │    RAG     │ │    Chat      │ │    │
│  │  │ Service  │ │   Service    │ │  Service   │ │   Service    │ │    │
│  │  └──────────┘ └──────────────┘ └────────────┘ └──────────────┘ │    │
│  │        │              │              │              │              │    │
│  │        │              │              │              │              │    │
│  │        ▼              ▼              ▼              ▼              │    │
│  │  ┌─────────────────────────────────────────────────────────┐    │    │
│  │  │                  Core Utilities                          │    │    │
│  │  │     Security    │    Database    │    Redis           │    │    │
│  │  └─────────────────────────────────────────────────────────┘    │    │
│  └────────────────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────────────────┘
           │                         │                         │
           │                         │                         │
           ▼                         ▼                         ▼
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│   PostgreSQL     │  │   pgvector      │  │     Redis       │
│   (port 5432)    │  │   (port 5432)   │  │   (port 6379)   │
│                  │  │                  │  │                 │
│  • users         │  │  • chunks        │  │  • session      │
│  • documents     │  │    (vectors)     │  │    memory       │
│  • conversations │  │                  │  │  • token cache  │
│  • messages      │  │                  │  │                 │
└──────────────────┘  └──────────────────┘  └──────────────────┘
                                                  │
                                                  ▼
                                         ┌──────────────────┐
                                         │    OpenAI API    │
                                         │  (External)      │
                                         │                  │
                                         │  • GPT-4o       │
                                         │  • Embeddings    │
                                         └──────────────────┘
```

## Component Responsibilities

### API Routers

**Auth Router (`/auth`):**
- Handle user registration and login
- Issue and refresh JWT tokens
- Provide current user information

**Document Router (`/documents`):**
- Accept file uploads
- Manage document lifecycle
- Track processing status

**Chat Router (`/chat`):**
- Handle chat messages
- Stream AI responses
- Manage conversations

### Services

| Service | Responsibility |
|---------|----------------|
| AuthService | User auth, token management |
| DocumentService | File handling, processing orchestration |
| EmbeddingService | OpenAI API calls, vector operations |
| RAGService | Retrieval, prompt building, LLM calls |
| ChatService | Conversation management, message history |
| MemoryService | Redis operations for conversation context |

### Data Stores

| Store | Purpose | Data Types |
|-------|---------|------------|
| PostgreSQL | Primary storage | Users, documents, metadata, chunks |
| pgvector | Vector similarity | Document chunk embeddings |
| Redis | Caching & state | Session memory, token cache |
| OpenAI API | AI computation | Embeddings, chat completions |

## Request Lifecycle: Authentication

```
┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐
│ Client  │───▶│ FastAPI │───▶│ Router  │───▶│ Service │───▶│  DB    │
│         │    │ Middle  │    │ Handler │    │         │    │         │
└─────────┘    └─────────┘    └─────────┘    └─────────┘    └─────────┘
                     │                                       │
                     │                                       │
                     ▼                                       ▼
              ┌─────────┐                              ┌─────────┐
              │ Extract │                              │ Validate│
              │ Token   │                              │ Credentials
              └─────────┘                              └─────────┘
                                                              │
                                                              ▼
                                                       ┌─────────┐
                                                       │ Return  │
                                                       │ JWT     │
                                                       │ Claims  │
                                                       └─────────┘

Steps:
1. Client sends request with Authorization: Bearer <token>
2. FastAPI middleware extracts token from header
3. Router handler calls auth service
4. Service validates credentials against database
5. Database returns user record
6. Service returns user object to router
7. Router responds to client
```

## Request Lifecycle: Document Upload

```
┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐
│ Client  │───▶│ FastAPI │───▶│ File    │───▶│ Validate│───▶│ Save    │
│         │    │ Router  │    │ Parser  │    │ File   │    │ Metadata│
└─────────┘    └─────────┘    └─────────┘    └─────────┘    └─────────┘
                                                                    │
                                                                    ▼
                                                            ┌─────────────┐
                                                            │  PostgreSQL │
                                                            │  (metadata)│
                                                            └─────────────┘
```

## Request Lifecycle: Chat with Streaming

```
┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐
│ Client  │───▶│ FastAPI │───▶│ Embed   │───▶│ Vector  │───▶│ Retrieve│
│         │    │ Router  │    │ Query   │    │ Search  │    │ Chunks  │
└─────────┘    └─────────┘    └─────────┘    └─────────┘    └─────────┘
      ▲                                                               │
      │                                                               ▼
      │                                                       ┌─────────────┐
      │                                                       │ pgvector    │
      │                                                       │ (similarity)│
      │                                                       └─────────────┘
      │                                                               │
      │                                                               ▼
      │   ┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────────┐
      └───│ Parse   │◀───│ Format  │◀───│ Call    │◀───│ Build       │
           │ SSE     │    │ Response│    │ OpenAI │    │ Prompt      │
           └─────────┘    └─────────┘    └─────────┘    └─────────────┘
                                                             │
                                                             ▼
                                                     ┌─────────────┐
                                                     │  OpenAI     │
                                                     │  API        │
                                                     └─────────────┘
```

## RAG Pipeline Data Flow

```
                         User Query
                              │
                              ▼
                    ┌─────────────────┐
                    │ 1. Embed Query  │
                    │ (text-embedding)│
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 2. Vector       │
                    │ Similarity      │
                    │ Search          │
                    └────────┬────────┘
                             │
                             ▼
        ┌────────────────────┼────────────────────┐
        │                    │                    │
        ▼                    ▼                    ▼
   ┌──────────┐       ┌──────────┐        ┌──────────┐
   │ Chunk 1  │       │ Chunk 2  │        │ Chunk 3  │
   │ Score:0.9│       │ Score:0.7│        │ Score:0.5│
   └──────────┘       └──────────┘        └──────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 3. Build Prompt│
                    │ with Context   │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 4. Call LLM    │
                    │ (GPT-4o)       │
                    └────────┬────────┘
                             │
                             ▼
                   ┌──────────────────┐
                   │ 5. Stream       │
                   │ Response        │
                   └────────┬─────────┘
                            │
                            ▼
                    ┌──────────────────┐
                    │ 6. Save to     │
                    │ Memory (Redis) │
                    └──────────────────┘
```

## Data Stores Detail

### PostgreSQL Schema

```
┌────────────────┐       ┌────────────────┐       ┌────────────────┐
│     users      │       │   documents    │       │    chunks      │
├────────────────┤       ├────────────────┤       ├────────────────┤
│ id (UUID, PK) │◀──────│ user_id (FK)  │       │ id (UUID, PK)  │
│ email         │       │ id (UUID, PK)  │◀──────│ document_id(FK) │
│ hashed_pass   │       │ filename       │       │ user_id (FK)   │
│ is_active     │       │ file_type      │       │ content        │
│ created_at    │       │ file_size      │       │ chunk_index    │
│ updated_at    │       │ status         │       │ token_count    │
└────────────────┘       │ created_at     │       │ created_at     │
                         └────────────────┘       └────────────────┘
                                                          │
                                                          │ (vector)
                                                          ▼
                                                 ┌────────────────┐
                                                 │  pgvector     │
                                                 │ (1536 dims)   │
                                                 └────────────────┘

┌────────────────┐       ┌────────────────┐
│ conversations  │       │    messages    │
├────────────────┤       ├────────────────┤
│ id (UUID, PK) │◀──────│ conversation_id│
│ user_id (FK)  │       │ (FK)          │
│ title         │       │ role          │
│ created_at    │       │ content        │
└────────────────┘       │ sources (JSON) │
                         │ created_at     │
                         └────────────────┘
```

### Redis Keys

```
pymind:session:{user_id}:{session_id}
  └── ["message1", "message2", ...]

pymind:token:{jti}
  └── {"user_id": "uuid", "exp": 1234567890}
```

## Streaming Architecture

```
OpenAI API                          FastAPI                          Client
     │                                 │                                │
     │◀──── Streaming Request ──────────│                                │
     │                                 │                                │
     │─── Stream: "Hello" ────────────▶│─── SSE: data: "Hello" ──────▶│
     │                                 │                                │
     │─── Stream: " how can" ─────────▶│─── SSE: data: " how can" ────▶│
     │                                 │                                │
     │─── Stream: " I help" ───────────▶│─── SSE: data: " I help" ────▶│
     │                                 │                                │
     │─── Stream: " today?" ───────────▶│─── SSE: data: " today?" ────▶│
     │                                 │                                │
     │─── [DONE] ──────────────────────▶│─── SSE: data: [DONE] ────────▶│
```

SSE Format:
```
data: {"content": "Hello", "sources": [...]}
data: {"content": " how can"}
data: {"content": " I help"}
data: {"content": " today?"}
data: [DONE]
```

## Security Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        Request Flow                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  1. TLS Termination (nginx / cloud provider)                   │
│                        │                                        │
│                        ▼                                        │
│  2. CORS Validation (FastAPI middleware)                       │
│                        │                                        │
│                        ▼                                        │
│  3. Authentication (JWT Bearer token)                          │
│     • Extract from Authorization header                         │
│     • Verify signature                                          │
│     • Check expiration                                          │
│     • Verify user exists and is active                          │
│                        │                                        │
│                        ▼                                        │
│  4. Authorization (per-endpoint)                              │
│     • Verify user owns resource                                 │
│     • Check permissions                                         │
│                        │                                        │
│                        ▼                                        │
│  5. Process Request                                            │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```
