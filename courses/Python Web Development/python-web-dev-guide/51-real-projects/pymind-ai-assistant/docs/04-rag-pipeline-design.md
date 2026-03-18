# RAG Pipeline Design

## What is RAG?

Retrieval Augmented Generation (RAG) combines:
1. **Retrieval** — Find relevant document chunks using semantic search
2. **Augmentation** — Add retrieved context to the prompt
3. **Generation** — Have the LLM answer based on the augmented prompt

This ensures answers are grounded in the user's documents rather than hallucinated.

## Why RAG Matters

```
┌─────────────────────────────────────────────────────────────────────┐
│                    WITHOUT RAG (Pure LLM)                          │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  User: "What does contract say about liability?"                   │
│                         │                                            │
│                         ▼                                            │
│              ┌─────────────────────┐                                 │
│              │      GPT-4o        │                                 │
│              │  (general model)    │                                 │
│              └─────────────────────┘                                 │
│                         │                                            │
│                         ▼                                            │
│  "Based on my training, liability typically refers to..."          │
│  ❌ May hallucinate or cite non-existent clauses                    │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│                    WITH RAG (Augmented)                             │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  User: "What does contract say about liability?"                    │
│                         │                                            │
│                         ▼                                            │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 1. EMBED QUERY: "What does contract say about liability?"  │   │
│  │    → [0.12, -0.45, 0.78, ...]                               │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                         │                                            │
│                         ▼                                            │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 2. VECTOR SEARCH: Find similar chunks                       │   │
│  │    → Chunk 1 (0.92 similarity): "Liability provisions..."  │   │
│  │    → Chunk 2 (0.87 similarity): "Indemnification terms..."  │   │
│  │    → Chunk 3 (0.85 similarity): "Warranty conditions..."     │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                         │                                            │
│                         ▼                                            │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 3. BUILD PROMPT:                                           │   │
│  │    System: "You are a helpful assistant..."                │   │
│  │    Context: "From the document: Liability provisions..."    │   │
│  │    Question: "What does contract say about liability?"      │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                         │                                            │
│                         ▼                                            │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 4. GENERATE: GPT-4o with context                           │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                         │                                            │
│                         ▼                                            │
│  "Based on the contract, liability is addressed in Section 5.1..." │
│  ✅ Grounded in actual document content                           │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

## Chunking Strategy

### Overview

Documents are split into smaller chunks before embedding:

```
┌─────────────────────────────────────────────────────────────────────┐
│                        DOCUMENT                                     │
│  ┌─────────────────────────────────────────────────────────────────┐│
│  │ Chapter 1: Introduction to Machine Learning                   ││
│  │                                                               ││
│  │ Machine learning is a subset of artificial intelligence...    ││
│  │ It enables computers to learn from data without being         ││
│  │ explicitly programmed. The field has evolved significantly    ││
│  │ over the past decade.                                         ││
│  │                                                               ││
│  │ Types of machine learning include supervised learning,         ││
│  │ unsupervised learning, and reinforcement learning.            ││
│  │                                                               ││
│  └─────────────────────────────────────────────────────────────────┘│
│                              │                                       │
│                              ▼                                       │
│  ┌──────────┐   ┌──────────┐   ┌──────────┐   ┌──────────┐        │
│  │ Chunk 0  │   │ Chunk 1  │   │ Chunk 2  │   │ Chunk 3  │        │
│  │  (0-200) │   │(100-300) │   │(200-400) │   │(300-500) │        │
│  └──────────┘   └──────────┘   └──────────┘   └──────────┘        │
│   overlap ◄─────────────────────────────► overlap                │
└─────────────────────────────────────────────────────────────────────┘
```

### Implementation

```python
from langchain.text_splitter import RecursiveCharacterTextSplitter

# Configuration
CHUNK_SIZE = 1000      # Characters per chunk
CHUNK_OVERLAP = 200    # Overlap between chunks

splitter = RecursiveCharacterTextSplitter(
    chunk_size=CHUNK_SIZE,
    chunk_overlap=CHUNK_OVERLAP,
    separators=["\n\n", "\n", ". ", " ", ""],
    length_function=len,
)
```

### Why These Values?

| Parameter | Value | Rationale |
|-----------|-------|-----------|
| `chunk_size` | 1000 | Fits well within token limits (OpenAI 4k-32k context) |
| `chunk_overlap` | 200 | Ensures context continuity between chunks |
| `separators` | [.py, .js] | Preserves paragraphs and sentences |

**Chunk Size Trade-offs:**
- Too small: Lost context, poor quality answers
- Too large: Can't fit many chunks in context, lower recall

## Embedding Model

### Choice: text-embedding-3-small

| Model | Dimensions | Price (per 1M tokens) | Quality |
|-------|------------|---------------------|---------|
| text-embedding-3-small | 1536 | $0.02 | Good |
| text-embedding-ada-002 | 1536 | $0.10 | Better |
| text-embedding-3-large | 3072 | $0.13 | Best |

**Decision:** `text-embedding-3-small`

**Rationale:**
- 5x cheaper than ada-002
- Sufficient quality for our use case
- 1536 dimensions compatible with pgvector

### Embedding Process

```python
from openai import AsyncOpenAI

client = AsyncOpenAI()

async def embed_text(text: str) -> list[float]:
    """Generate embedding for a text."""
    response = await client.embeddings.create(
        model="text-embedding-3-small",
        input=text,
    )
    return response.data[0].embedding

async def embed_batch(texts: list[str]) -> list[list[float]]:
    """Generate embeddings for multiple texts."""
    response = await client.embeddings.create(
        model="text-embedding-3-small",
        input=texts,
    )
    return [item.embedding for item in response.data]
```

### Batch Processing

Embed in batches to improve throughput:

```python
BATCH_SIZE = 100  # OpenAI limit

async def embed_chunks(chunks: list[str]) -> list[list[float]]:
    """Embed all chunks in batches."""
    all_embeddings = []
    
    for i in range(0, len(chunks), BATCH_SIZE):
        batch = chunks[i:i + BATCH_SIZE]
        embeddings = await embed_batch(batch)
        all_embeddings.extend(embeddings)
    
    return all_embeddings
```

## Vector Similarity Search

### Cosine Similarity

We use cosine similarity to find semantically similar chunks:

```sql
SELECT id, content,
       (embedding <=> $query_embedding) as distance
FROM chunks
WHERE user_id = $1
ORDER BY embedding <=> $query_embedding
LIMIT 5;
```

### Why Cosine?

| Metric | Best For | PyMind Use Case |
|--------|----------|-----------------|
| Cosine | Semantic similarity | ✅ Perfect match |
| L2 | Geometric distance | Less intuitive |
| Dot product | Magnitude matters | Not needed |

### Search Parameters

```python
TOP_K = 5              # Number of chunks to retrieve
SIMILARITY_THRESHOLD = 0.7  # Minimum similarity

async def search_similar(
    query_embedding: list[float],
    user_id: str,
    top_k: int = TOP_K,
    threshold: float = SIMILARITY_THRESHOLD,
) -> list[dict]:
    """Search for similar chunks."""
    
    # Query with threshold via raw SQL
    query = text("""
        SELECT id, content, 
               (embedding <=> :query) as distance
        FROM chunks
        WHERE user_id = :user_id
          AND (embedding <=> :query) < :threshold
        ORDER BY embedding <=> :query
        LIMIT :top_k
    """)
    
    # Filter results below threshold
    # Lower distance = higher similarity
```

## Prompt Template

### Structure

```
System Prompt:
You are a helpful AI assistant. Use the provided context to answer the user's question.
If you cannot find the answer in the context, say so honestly.

Context:
---
{context}
---

User Question:
{question}

Answer:
```

### Implementation

```python
SYSTEM_PROMPT = """You are a helpful AI assistant. 
Use the provided context to answer the user's question.
If you cannot find the answer in the context, say so honestly.
Cite the source(s) when possible."""

def build_prompt(query: str, context: list[dict]) -> list[dict]:
    """Build message list for OpenAI API."""
    
    # Format context
    context_text = "\n\n---\n\n".join([
        f"[Source {i+1}]: {chunk['content']}"
        for i, chunk in enumerate(context)
    ])
    
    messages = [
        {"role": "system", "content": SYSTEM_PROMPT},
        {"role": "user", "content": f"Context:\n{context_text}\n\nQuestion: {query}"},
    ]
    
    return messages
```

### Context Window Management

We limit context to prevent exceeding token limits:

```python
MAX_CONTEXT_TOKENS = 4000  # Reserve 4000 for response

def estimate_tokens(text: str) -> int:
    """Rough token estimation (1 token ≈ 4 chars)."""
    return len(text) // 4

def truncate_context(context: list[dict], max_tokens: int = MAX_CONTEXT_TOKENS) -> list[dict]:
    """Truncate context to fit in token budget."""
    
    truncated = []
    current_tokens = 0
    
    for chunk in context:
        chunk_tokens = estimate_tokens(chunk['content'])
        
        if current_tokens + chunk_tokens > max_tokens:
            break
            
        truncated.append(chunk)
        current_tokens += chunk_tokens
    
    return truncated
```

## Streaming Response

### Architecture

```
OpenAI ──chunk1──▶ FastAPI ──SSE──▶ Client
         ──chunk2──▶              ──SSE──▶
         ──chunk3──▶              ──SSE──▶
            ...
         ──[DONE]──▶              ──SSE──▶
```

### Implementation

```python
from fastapi import StreamingResponse
import json

async def stream_response(query: str, user_id: str):
    """Stream LLM response to client."""
    
    # Get embeddings
    query_embedding = await embed_text(query)
    
    # Retrieve chunks
    chunks = await search_similar(query_embedding, user_id)
    
    # Build prompt
    messages = build_prompt(query, chunks)
    
    # Stream from OpenAI
    stream = await client.chat.completions.create(
        model="gpt-4o",
        messages=messages,
        stream=True,
    )
    
    async def event_generator():
        async for chunk in stream:
            content = chunk.choices[0].delta.content or ""
            
            if content:
                yield f"data: {json.dumps({'content': content})}\n\n"
        
        # Send done signal
        yield "data: [DONE]\n\n"
    
    return StreamingResponse(
        event_generator(),
        media_type="text/event-stream"
    )
```

## Hallucination Mitigation

### Strategies

1. **Grounded Context**
   - Only use retrieved chunks as context
   - Cite sources in responses

2. **Prompt Engineering**
   - Clear instruction to use only context
   - Honesty instruction: "If unsure, say so"

3. **Threshold Filtering**
   - Don't use low-similarity chunks
   - Default threshold: 0.7

4. **Response Validation**
   - Check if answer references non-existent sources

### Source Citations

Include sources in response:

```python
{
  "content": "The document states that...",
  "sources": [
    {
      "document_id": "doc-uuid",
      "chunk": "The document states that...",
      "score": 0.92
    }
  ]
}
```

## Evaluation Metrics

### RAG Quality

| Metric | Description | Target |
|--------|-------------|--------|
| Context Precision | Are retrieved chunks relevant? | > 0.8 |
| Context Recall | Does retrieval find relevant info? | > 0.7 |
| Faithfulness | Does answer match context? | > 0.85 |
| Answer Relevance | Does answer address question? | > 0.8 |

### How to Measure

1. **Automated:** Use RAGAS library
2. **Human:** Manual evaluation of sample queries
3. **A/B Testing:** Compare different retrieval strategies

## Future Improvements

### Phase 2 Ideas

- **Hybrid Search:** Combine vector + keyword search
- **Re-ranking:** Use cross-encoder to re-rank results
- **Multi-modal:** Support images in documents
- **Custom Embeddings:** Fine-tuned domain embeddings
- **Agentic RAG:** Multi-step reasoning over documents
