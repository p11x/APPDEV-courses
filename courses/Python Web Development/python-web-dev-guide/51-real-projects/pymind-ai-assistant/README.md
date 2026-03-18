# PyMind — AI Knowledge Assistant

<p align="center">
  <img src="https://img.shields.io/badge/Python-3.11+-blue?style=for-the-badge&logo=python" alt="Python" />
  <img src="https://img.shields.io/badge/FastAPI-0.109+-005571?style=for-the-badge&logo=fastapi" alt="FastAPI" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" alt="License" />
</p>

PyMind is a production-grade AI-powered REST API that enables users to upload documents and chat with an AI assistant that answers questions grounded in their own knowledge base using Retrieval Augmented Generation (RAG).

## Features

- **User Authentication** — JWT-based auth with access and refresh tokens
- **Document Management** — Upload PDFs, TXT, and Markdown files
- **Automatic Embedding** — Chunks documents and creates embeddings using OpenAI
- **Vector Search** — pgvector-powered semantic search over document chunks
- **RAG Pipeline** — Context-aware AI responses using retrieved documents
- **Streaming Responses** — Real-time streaming AI responses via Server-Sent Events (SSE)
- **Conversation Memory** — Multi-turn chat with Redis-backed memory
- **Per-User Isolation** — All data is strictly isolated per user

## Tech Stack

| Tool | Purpose | Version |
|------|---------|---------|
| FastAPI | Web framework | 0.109+ |
| PostgreSQL | Primary database | 16+ |
| pgvector | Vector similarity search | 0.2+ |
| SQLAlchemy 2.0 | Async ORM | 2.0+ |
| Alembic | Database migrations | — |
| OpenAI GPT-4o | LLM for chat | — |
| OpenAI Embeddings | Text embeddings | text-embedding-3-small |
| Redis | Memory & caching | 7+ |
| Pydantic v2 | Data validation | 2.0+ |
| python-jose | JWT tokens | — |
| passlib | Password hashing | — |
| Docker | Containerization | — |

## Quick Start

### Prerequisites

- Python 3.11+
- Docker and Docker Compose
- OpenAI API key

### 1. Clone and Configure

```bash
git clone https://github.com/yourusername/pymind.git
cd pymind
cp .env.example .env
```

Edit `.env` with your settings:

```env
DATABASE_URL=postgresql+asyncpg://pymind:pymind123@localhost:5432/pymind
REDIS_URL=redis://localhost:6379/0
OPENAI_API_KEY=sk-your-openai-key-here
JWT_SECRET_KEY=$(openssl rand -hex 32)
JWT_ALGORITHM=HS256
ACCESS_TOKEN_EXPIRE_MINUTES=30
REFRESH_TOKEN_EXPIRE_DAYS=7
APP_ENV=development
LOG_LEVEL=INFO
MAX_UPLOAD_SIZE_MB=10
CORS_ORIGINS=http://localhost:3000
```

### 2. Start Infrastructure

```bash
docker-compose up -d
```

This starts PostgreSQL with pgvector, Redis, and the PyMind application.

### 3. Verify

```bash
curl http://localhost:8000/health
```

Expected: `{"status":"healthy","environment":"development"}`

## API Endpoints Reference

### Authentication

| Method | Path | Description |
|--------|------|-------------|
| POST | `/auth/register` | Register a new user |
| POST | `/auth/login` | Login and get tokens |
| POST | `/auth/refresh` | Refresh access token |
| GET | `/auth/me` | Get current user info |
| POST | `/auth/logout` | Invalidate tokens |

### Documents

| Method | Path | Description |
|--------|------|-------------|
| POST | `/documents/upload` | Upload a document |
| GET | `/documents/` | List user's documents |
| GET | `/documents/{id}` | Get document details |
| DELETE | `/documents/{id}` | Delete a document |
| GET | `/documents/{id}/status` | Get processing status |

### Chat

| Method | Path | Description |
|--------|------|-------------|
| POST | `/chat/stream` | Stream chat response (SSE) |
| POST | `/chat/message` | Non-streaming chat |
| GET | `/chat/conversations` | List conversations |
| GET | `/chat/conversations/{id}/history` | Get conversation messages |
| DELETE | `/chat/conversations/{id}` | Delete conversation |

## Project Structure

```
pymind/
├── docs/                      # Documentation
│   ├── 00-project-overview.md
│   ├── 01-architecture.md
│   ├── 02-database-design.md
│   ├── 03-api-design.md
│   └── 04-rag-pipeline-design.md
│
├── phases/                    # Build tutorials
│   ├── phase-01-project-setup/
│   ├── phase-02-database-and-models/
│   ├── phase-03-authentication/
│   ├── phase-04-document-ingestion/
│   ├── phase-05-embeddings-and-vectorstore/
│   ├── phase-06-rag-pipeline/
│   ├── phase-07-streaming-chat-api/
│   ├── phase-08-conversation-memory/
│   ├── phase-09-testing/
│   └── phase-10-deployment/
│
├── reference/                 # Complete reference code
│   ├── app/
│   │   ├── main.py
│   │   ├── config.py
│   │   ├── dependencies.py
│   │   ├── models/
│   │   ├── schemas/
│   │   ├── routers/
│   │   ├── services/
│   │   ├── core/
│   │   └── utils/
│   ├── alembic/
│   ├── tests/
│   ├── .env.example
│   ├── docker-compose.yml
│   ├── Dockerfile
│   ├── pyproject.toml
│   └── alembic.ini
│
└── README.md
```

## Documentation Phases

Build PyMind step by step:

1. **Phase 1** — Project Setup → [`phases/phase-01-project-setup/PHASE.md`](phases/phase-01-project-setup/PHASE.md)
2. **Phase 2** — Database and Models → [`phases/phase-02-database-and-models/PHASE.md`](phases/phase-02-database-and-models/PHASE.md)
3. **Phase 3** — Authentication → [`phases/phase-03-authentication/PHASE.md`](phases/phase-03-authentication/PHASE.md)
4. **Phase 4** — Document Ingestion → [`phases/phase-04-document-ingestion/PHASE.md`](phases/phase-04-document-ingestion/PHASE.md)
5. **Phase 5** — Embeddings & Vector Store → [`phases/phase-05-embeddings-and-vectorstore/PHASE.md`](phases/phase-05-embeddings-and-vectorstore/PHASE.md)
6. **Phase 6** — RAG Pipeline → [`phases/phase-06-rag-pipeline/PHASE.md`](phases/phase-06-rag-pipeline/PHASE.md)
7. **Phase 7** — Streaming Chat API → [`phases/phase-07-streaming-chat-api/PHASE.md`](phases/phase-07-streaming-chat-api/PHASE.md)
8. **Phase 8** — Conversation Memory → [`phases/phase-08-conversation-memory/PHASE.md`](phases/phase-08-conversation-memory/PHASE.md)
9. **Phase 9** — Testing → [`phases/phase-09-testing/PHASE.md`](phases/phase-09-testing/PHASE.md)
10. **Phase 10** — Deployment → [`phases/phase-10-deployment/PHASE.md`](phases/phase-10-deployment/PHASE.md)

## Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         Client (React/Vue)                       │
└─────────────────────────────────────────────────────────────────┘
                              │ HTTP/SSE
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                     FastAPI Application                          │
├─────────────────────────────────────────────────────────────────┤
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ Auth Router  │  │ Doc Router   │  │ Chat Router  │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│         │                  │                  │                 │
│  ┌──────┴──────────────────┴──────────────────┴─────────┐      │
│  │                   Services Layer                       │      │
│  │  Auth │ Document │ Embedding │ RAG │ Chat │ Memory   │      │
│  └──────┴──────────────────┴──────────────────┴─────────┘      │
└─────────────────────────────────────────────────────────────────┘
         │                    │                    │
         ▼                    ▼                    ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│  PostgreSQL     │  │  pgvector       │  │  Redis         │
│  (users, docs)  │  │  (embeddings)   │  │  (memory)      │
└─────────────────┘  └─────────────────┘  └─────────────────┘
                              │
                              ▼
                    ┌─────────────────┐
                    │  OpenAI API     │
                    │  GPT-4o +       │
                    │  Embeddings     │
                    └─────────────────┘
```

## Example Usage

### 1. Register User

```bash
curl -X POST http://localhost:8000/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"securepass123"}'
```

### 2. Login

```bash
curl -X POST http://localhost:8000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"securepass123"}'
```

Save the `access_token` for subsequent requests.

### 3. Upload Document

```bash
curl -X POST http://localhost:8000/documents/upload \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -F "file=@/path/to/document.pdf"
```

### 4. Chat with AI (Streaming)

```bash
curl -X POST http://localhost:8000/chat/stream \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"message":"What is this document about?"}' \
  -N
```

## Testing

```bash
# Run tests with coverage
pytest --cov=app --cov-report=html

# Run specific test file
pytest tests/test_auth.py -v
```

## Deployment

See [Phase 10 — Deployment](phases/phase-10-deployment/PHASE.md) for detailed deployment instructions to Railway, Render, or AWS.

## Contributing

Contributions are welcome! Please open an issue or submit a PR.

## License

MIT License — see LICENSE file for details.
