# API Design

## Overview

PyMind exposes a RESTful API with JSON request/response bodies. Some endpoints use Server-Sent Events (SSE) for streaming responses.

## Base URL

```
Development: http://localhost:8000
Production:  https://api.pymind.ai
```

## Authentication

All endpoints except `/auth/register` and `/auth/login` require a valid JWT access token.

```
Authorization: Bearer <access_token>
```

## Response Format

### Success Response

```json
{
  "data": { ... },
  "message": "Optional success message"
}
```

### Error Response

```json
{
  "error": "error_code",
  "detail": "Human-readable error message"
}
```

### Common Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| `invalid_credentials` | 401 | Wrong email/password |
| `token_expired` | 401 | JWT token expired |
| `invalid_token` | 401 | Malformed JWT token |
| `user_not_found` | 404 | Resource not found |
| `document_not_found` | 404 | Document not found |
| `file_too_large` | 413 | Upload exceeds limit |
| `invalid_file_type` | 400 | Unsupported file type |
| `rate_limit_exceeded` | 429 | Too many requests |

## Endpoints

---

## Authentication

### POST /auth/register

Register a new user account.

**Request:**

```json
{
  "email": "user@example.com",
  "password": "securePassword123"
}
```

**Response (201):**

```json
{
  "data": {
    "id": "uuid",
    "email": "user@example.com",
    "created_at": "2024-01-15T10:30:00Z"
  },
  "message": "Registration successful"
}
```

**Validation:**
- Email: Valid email format, unique
- Password: Minimum 8 characters

---

### POST /auth/login

Authenticate and receive access tokens.

**Request:**

```json
{
  "email": "user@example.com",
  "password": "securePassword123"
}
```

**Response (200):**

```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refresh_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "bearer",
  "expires_in": 1800
}
```

**Errors:**
- 401: Invalid credentials

---

### POST /auth/refresh

Refresh an expired access token.

**Headers:**

```
Authorization: Bearer <refresh_token>
```

**Response (200):**

```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "bearer",
  "expires_in": 1800
}
```

---

### GET /auth/me

Get current authenticated user.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (200):**

```json
{
  "data": {
    "id": "uuid",
    "email": "user@example.com",
    "is_active": true,
    "created_at": "2024-01-15T10:30:00Z"
  }
}
```

---

### POST /auth/logout

Invalidate current session.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (200):**

```json
{
  "message": "Logged out successfully"
}
```

---

## Documents

### POST /documents/upload

Upload a new document for processing.

**Headers:**

```
Authorization: Bearer <access_token>
Content-Type: multipart/form-data
```

**Request (multipart/form-data):**

```
file: <binary>
```

**Response (202):**

```json
{
  "data": {
    "id": "uuid",
    "filename": "document.pdf",
    "file_type": "application/pdf",
    "file_size": 1024000,
    "status": "pending",
    "created_at": "2024-01-15T10:30:00Z"
  },
  "message": "Document uploaded. Processing started."
}
```

**Validation:**
- File types: `application/pdf`, `text/plain`, `text/markdown`
- Max size: 10 MB

---

### GET /documents/

List user's documents.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Query Parameters:**

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `limit` | int | 20 | Max results |
| `offset` | int | 0 | Pagination offset |
| `status` | string | all | Filter by status |

**Response (200):**

```json
{
  "data": [
    {
      "id": "uuid",
      "filename": "document.pdf",
      "file_type": "application/pdf",
      "file_size": 1024000,
      "status": "ready",
      "created_at": "2024-01-15T10:30:00Z"
    }
  ],
  "pagination": {
    "total": 50,
    "limit": 20,
    "offset": 0
  }
}
```

---

### GET /documents/{document_id}

Get document details.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (200):**

```json
{
  "data": {
    "id": "uuid",
    "filename": "document.pdf",
    "file_type": "application/pdf",
    "file_size": 1024000,
    "status": "ready",
    "chunk_count": 45,
    "created_at": "2024-01-15T10:30:00Z",
    "updated_at": "2024-01-15T10:32:00Z"
  }
}
```

**Errors:**
- 404: Document not found

---

### DELETE /documents/{document_id}

Delete a document and its chunks.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (204):** No content

**Errors:**
- 404: Document not found

---

### GET /documents/{document_id}/status

Get document processing status.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (200):**

```json
{
  "data": {
    "id": "uuid",
    "status": "ready",
    "chunk_count": 45,
    "error_message": null
  }
}
```

---

## Chat

### POST /chat/stream

Send a chat message and receive streaming response.

**Headers:**

```
Authorization: Bearer <access_token>
Content-Type: application/json
```

**Request:**

```json
{
  "message": "What is this document about?",
  "conversation_id": "uuid (optional)",
  "document_ids": ["uuid1", "uuid2"] (optional)
}
```

**Response (200):** Server-Sent Events

```
data: {"content": "The", "sources": [{"document_id": "uuid", "chunk": "..."}]}
data: {"content": " document", "sources": [...]}
data: {"content": " contains", "sources": [...]}
...
data: {"content": " [DONE]", "sources": []}
```

**SSE Format:**

Each chunk is a JSON object:
```json
{
  "content": "word or phrase",
  "sources": [
    {
      "document_id": "uuid",
      "chunk": "relevant text...",
      "score": 0.95
    }
  ],
  "done": false
}
```

---

### POST /chat/message

Send a chat message and receive a complete (non-streaming) response.

**Headers:**

```
Authorization: Bearer <access_token>
Content-Type: application/json
```

**Request:**

```json
{
  "message": "What is this document about?",
  "conversation_id": "uuid (optional)",
  "document_ids": ["uuid1", "uuid2"] (optional)
}
```

**Response (200):**

```json
{
  "data": {
    "message": "The document discusses...",
    "conversation_id": "uuid",
    "sources": [
      {
        "document_id": "uuid",
        "chunk": "relevant text...",
        "score": 0.95
      }
    ]
  }
}
```

---

### GET /chat/conversations

List user's conversations.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (200):**

```json
{
  "data": [
    {
      "id": "uuid",
      "title": "What is the project about?",
      "message_count": 5,
      "created_at": "2024-01-15T10:30:00Z",
      "updated_at": "2024-01-15T10:35:00Z"
    }
  ]
}
```

---

### GET /chat/conversations/{conversation_id}/history

Get conversation message history.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (200):**

```json
{
  "data": {
    "id": "uuid",
    "title": "What is the project about?",
    "messages": [
      {
        "role": "user",
        "content": "What is this document about?",
        "created_at": "2024-01-15T10:30:00Z"
      },
      {
        "role": "assistant",
        "content": "The document discusses...",
        "sources": [...],
        "created_at": "2024-01-15T10:30:05Z"
      }
    ]
  }
}
```

---

### DELETE /chat/conversations/{conversation_id}

Delete a conversation and its messages.

**Headers:**

```
Authorization: Bearer <access_token>
```

**Response (204):** No content

---

## Health Check

### GET /health

Application health check (no auth required).

**Response (200):**

```json
{
  "status": "healthy",
  "environment": "development"
}
```

---

## Rate Limiting

| Endpoint | Limit |
|----------|-------|
| `/auth/*` | 10/minute |
| `/documents/upload` | 5/minute |
| `/chat/*` | 30/minute |

Rate limit headers:
```
X-RateLimit-Limit: 30
X-RateLimit-Remaining: 29
X-RateLimit-Reset: 1705329000
```

---

## Example: Complete Chat Flow

### 1. Register

```bash
curl -X POST http://localhost:8000/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"pass123"}'
```

Response:
```json
{
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "user@example.com",
    "created_at": "2024-01-15T10:30:00Z"
  }
}
```

### 2. Login

```bash
curl -X POST http://localhost:8000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"pass123"}'
```

Response:
```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIs...",
  "refresh_token": "eyJhbGciOiJIUzI1NiIs...",
  "token_type": "bearer",
  "expires_in": 1800
}
```

### 3. Upload Document

```bash
curl -X POST http://localhost:8000/documents/upload \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -F "file=@document.pdf"
```

### 4. Stream Chat

```bash
curl -X POST http://localhost:8000/chat/stream \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -H "Content-Type: application/json" \
  -d '{"message":"What is this about?"}' \
  -N
```

Response (streaming):
```
data: {"content":"This","sources":[]}
data: {"content":" document","sources":[]}
data: {"content":" is","sources":[]}
data: {"content":" about","sources":[]}
data: {"content":" PyMind","sources":[{"document_id":"uuid","chunk":"...","score":0.95}]}
data: {"content":".","sources":[]}
data: {"content":" [DONE]"}
```
