# Phase 5 — Embeddings and Vectorstore

## Goal

By the end of this phase, you will have:
- Text chunking with overlap
- OpenAI embedding generation
- Vector storage in pgvector
- Semantic search functionality
- Background processing pipeline

## Prerequisites

- Completed Phase 4 (document ingestion)
- OpenAI API key configured

## Embedding Pipeline

```
┌─────────────────────────────────────────────────────────────────┐
│                    Embedding Pipeline                             │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Document (stored)                                               │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ 1. Load file        │                                         │
│  │ 2. Parse to text    │                                         │
│  └─────────────────────┘                                         │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ 3. Chunk text        │──────┬─────────────┬──────────┐      │
│  │    (1000 chars,     │      │             │          │      │
│  │     200 overlap)   │      ▼             ▼          ▼      │
│  └─────────────────────┘   Chunk 0      Chunk 1    Chunk 2   │
│                                   │             │          │   │
│                                   ▼             ▼          ▼   │
│  ┌─────────────────────┐      ┌─────────────────────────┐    │
│  │ 4. Generate         │      │  "Machine learning is   │    │
│  │    embeddings       │─────▶│   a subset of AI..."    │    │
│  │    (OpenAI)         │      │  → [0.12, -0.45, ...]   │    │
│  └─────────────────────┘      └─────────────────────────┘    │
│                                   │             │          │   │
│                                   ▼             ▼          ▼   │
│  ┌─────────────────────┐      ┌─────────────────────────┐    │
│  │ 5. Store in DB      │      │  chunks table with      │    │
│  │    (pgvector)       │─────▶│  embedding column        │    │
│  └─────────────────────┘      └─────────────────────────┘    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Step-by-Step Implementation

### Step 5.1 — Add Dependencies

Add to pyproject.toml:
```toml
# For token counting (optional, approximate)
tiktoken>=0.5.0
```

### Step 5.2 — Create Chunking Utility

```python
# app/utils/chunker.py
"""
Text chunking utilities for document splitting.
"""
from typing import Callable


class TextChunker:
    """
    Split text into overlapping chunks for embedding.
    
    Uses RecursiveCharacterTextSplitter strategy to preserve
    semantic boundaries (paragraphs, sentences, words).
    """
    
    def __init__(
        self,
        chunk_size: int = 1000,
        chunk_overlap: int = 200,
        separators: list[str] | None = None,
    ):
        """
        Initialize chunker.
        
        Args:
            chunk_size: Maximum characters per chunk
            chunk_overlap: Overlap between chunks
            separators: List of separators (in priority order)
        """
        self.chunk_size = chunk_size
        self.chunk_overlap = chunk_overlap
        self.separators = separators or ["\n\n", "\n", ". ", " ", ""]
    
    def split_text(self, text: str) -> list[str]:
        """
        Split text into chunks.
        
        Args:
            text: Input text
        
        Returns:
            List of text chunks
        """
        chunks = []
        current_pos = 0
        text_len = len(text)
        
        while current_pos < text_len:
            # Calculate chunk end
            chunk_end = min(current_pos + self.chunk_size, text_len)
            
            # Get chunk text
            chunk = text[current_pos:chunk_end]
            
            # Try to break at separator for cleaner chunks
            if chunk_end < text_len:
                # Look for separator in last 20% of chunk
                search_start = int(chunk_end - self.chunk_size * 0.2)
                search_text = text[search_start:chunk_end]
                
                for sep in self.separators:
                    if sep:
                        sep_pos = search_text.rfind(sep)
                        if sep_pos > 0:
                            # Adjust chunk end
                            actual_end = search_start + sep_pos + len(sep)
                            if actual_end > current_pos:
                                chunk = text[current_pos:actual_end]
                                chunk_end = actual_end
                            break
            
            # Clean chunk
            chunk = chunk.strip()
            if chunk:
                chunks.append(chunk)
            
            # Move position (with overlap)
            current_pos = chunk_end - self.chunk_overlap
            
            # Prevent infinite loop
            if current_pos <= 0:
                break
        
        return chunks
    
    def create_chunks_with_metadata(
        self,
        text: str,
        document_id: str,
    ) -> list[dict]:
        """
        Create chunks with metadata.
        
        Args:
            text: Input text
            document_id: Document ID
        
        Returns:
            List of dicts with content and metadata
        """
        texts = self.split_text(text)
        
        chunks = []
        for i, content in enumerate(texts):
            chunks.append({
                "document_id": document_id,
                "content": content,
                "chunk_index": i,
                "token_count": self.estimate_tokens(content),
            })
        
        return chunks
    
    @staticmethod
    def estimate_tokens(text: str) -> int:
        """
        Estimate token count (rough approximation).
        
        Args:
            text: Input text
        
        Returns:
            Estimated token count
        """
        # Average token is ~4 characters
        return len(text) // 4


# Default chunker instance
default_chunker = TextChunker()
```

### Step 5.3 — Create Embedding Service

```python
# app/services/embedding_service.py
"""
Embedding service for generating document embeddings.
"""
import asyncio
from typing import Optional
from uuid import UUID

from openai import AsyncOpenAI
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.config import get_settings
from app.models import Chunk, Document


class EmbeddingService:
    """Service for generating and storing embeddings."""
    
    # OpenAI embedding model
    EMBEDDING_MODEL = "text-embedding-3-small"
    EMBEDDING_DIMENSIONS = 1536
    BATCH_SIZE = 100
    
    def __init__(self):
        settings = get_settings()
        self.client = AsyncOpenAI(api_key=settings.OPENAI_API_KEY)
    
    async def generate_embedding(self, text: str) -> list[float]:
        """
        Generate embedding for a single text.
        
        Args:
            text: Input text
        
        Returns:
            Embedding vector (1536 dimensions)
        """
        response = await self.client.embeddings.create(
            model=self.EMBEDDING_MODEL,
            input=text,
        )
        
        return response.data[0].embedding
    
    async def generate_embeddings_batch(
        self,
        texts: list[str],
    ) -> list[list[float]]:
        """
        Generate embeddings for multiple texts.
        
        Args:
            texts: List of input texts
        
        Returns:
            List of embedding vectors
        """
        # Process in batches
        all_embeddings = []
        
        for i in range(0, len(texts), self.BATCH_SIZE):
            batch = texts[i:i + self.BATCH_SIZE]
            
            response = await self.client.embeddings.create(
                model=self.EMBEDDING_MODEL,
                input=batch,
            )
            
            batch_embeddings = [item.embedding for item in response.data]
            all_embeddings.extend(batch_embeddings)
        
        return all_embeddings
    
    async def embed_chunks(
        self,
        session: AsyncSession,
        document_id: str,
        chunks: list[dict],
    ) -> int:
        """
        Generate and store embeddings for chunks.
        
        Args:
            session: Database session
            document_id: Document ID
            chunks: List of chunk dicts with content
        
        Returns:
            Number of chunks processed
        """
        # Extract text content
        texts = [chunk["content"] for chunk in chunks]
        
        # Generate embeddings
        embeddings = await self.generate_embeddings_batch(texts)
        
        # Create Chunk records
        for chunk_data, embedding in zip(chunks, embeddings):
            chunk = Chunk(
                id=chunk_data.get("id"),  # Use provided or generate new
                document_id=document_id,
                content=chunk_data["content"],
                embedding=embedding,
                chunk_index=chunk_data["chunk_index"],
                token_count=chunk_data.get("token_count"),
            )
            session.add(chunk)
        
        await session.commit()
        
        return len(chunks)
    
    async def search_similar(
        self,
        session: AsyncSession,
        query_embedding: list[float],
        user_id: str,
        document_id: str | None = None,
        top_k: int = 5,
        similarity_threshold: float = 0.7,
    ) -> list[Chunk]:
        """
        Search for similar chunks using vector similarity.
        
        Uses pgvector's cosine distance operator (<=>).
        
        Args:
            session: Database session
            query_embedding: Query embedding vector
            user_id: Owner's user ID
            document_id: Optional document filter
            top_k: Number of results
            similarity_threshold: Minimum similarity (0-1)
        
        Returns:
            List of similar chunks
        """
        from sqlalchemy import text
        
        # Build query with vector similarity
        query = text("""
            SELECT id, document_id, content, chunk_index, token_count, created_at,
                   (embedding <=> :query_embedding) as distance
            FROM chunks
            WHERE user_id = :user_id
              AND (embedding <=> :query_embedding) < :threshold
            ORDER BY embedding <=> :query_embedding
            LIMIT :top_k
        """)
        
        # Note: We need to join with documents to get user_id
        # Let's use a simpler approach with the model
        
        # Query chunks with their document's user_id
        # For now, filter by document_id if provided
        
        if document_id:
            result = await session.execute(
                select(Chunk)
                .where(Chunk.document_id == document_id)
                .order_by(Chunk.embedding.cosine_distance(query_embedding))
                .limit(top_k)
            )
        else:
            # Get user's document IDs first
            from app.models import Document
            docs_result = await session.execute(
                select(Document.id).where(Document.user_id == user_id)
            )
            doc_ids = [str(d) for d in docs_result.scalars().all()]
            
            if not doc_ids:
                return []
            
            result = await session.execute(
                select(Chunk)
                .where(Chunk.document_id.in_(doc_ids))
                .order_by(Chunk.embedding.cosine_distance(query_embedding))
                .limit(top_k)
            )
        
        chunks = result.scalars().all()
        
        # Filter by similarity threshold (distance < threshold)
        # Note: In production, use raw SQL for precise filtering
        return list(chunks)


# Global instance
embedding_service = EmbeddingService()
```

### Step 5.4 — Create Document Processing Pipeline

```python
# app/services/document_processor.py
"""
Document processing pipeline for embedding generation.
"""
from pathlib import Path

from sqlalchemy.ext.asyncio import AsyncSession

from app.models import Document, DocumentStatus
from app.services.embedding_service import embedding_service
from app.services.document_service import DocumentService
from app.utils.chunker import default_chunker
from app.utils.file_parser import get_text_content
from app.utils.file_storage import UPLOAD_DIR


async def process_document(
    session: AsyncSession,
    document_id: str,
) -> bool:
    """
    Process document: parse, chunk, embed, store.
    
    Args:
        session: Database session
        document_id: Document ID to process
    
    Returns:
        True if successful
    """
    # Get document
    result = await session.execute(
        select(Document).where(Document.id == document_id)
    )
    document = result.scalar_one_or_none()
    
    if not document:
        return False
    
    try:
        # Update status to processing
        await DocumentService.update_status(
            session,
            document_id,
            DocumentStatus.PROCESSING,
        )
        
        # Get file path
        file_path = UPLOAD_DIR / document.user_id / document.file_name
        
        if not file_path.exists():
            raise FileNotFoundError(f"File not found: {file_path}")
        
        # Parse file to text
        text = await get_text_content(file_path, document.file_type)
        
        if not text.strip():
            raise ValueError("No text content extracted from file")
        
        # Chunk text
        chunks = default_chunker.create_chunks_with_metadata(
            text, 
            document_id
        )
        
        # Generate embeddings and store
        await embedding_service.embed_chunks(
            session,
            document_id,
            chunks,
        )
        
        # Calculate total tokens
        total_tokens = sum(
            chunk.get("token_count", 0) 
            for chunk in chunks
        )
        
        # Update status to completed
        await DocumentService.update_status(
            session,
            document_id,
            DocumentStatus.COMPLETED,
            total_tokens=total_tokens,
        )
        
        return True
    
    except Exception as e:
        # Update status to failed
        await DocumentService.update_status(
            session,
            document_id,
            DocumentStatus.FAILED,
            error_message=str(e),
        )
        
        return False


# Import select for query
from sqlalchemy import select
```

### Step 5.5 — Integrate with Upload

Update the document upload to trigger processing:

```python
# app/routers/documents.py (update upload endpoint)

@router.post(
    "/upload",
    response_model=UploadResponse,
    status_code=status.HTTP_201_CREATED,
    summary="Upload document",
)
async def upload_document(
    file: UploadFile = File(...),
    current_user: CurrentUser = Depends(get_current_user),
    session: AsyncSession = Depends(get_db_session),
) -> UploadResponse:
    """
    Upload a document for processing.
    
    Supports PDF, DOCX, and TXT files up to 10MB.
    
    - **file**: File to upload (multipart/form-data)
    """
    # ... existing validation code ...
    
    # Create document record
    document = await DocumentService.create_document(
        session,
        current_user.id,
        file.filename,
        file.content_type or "application/octet-stream",
        file_size,
    )
    
    # Process document (synchronous for now)
    # In production, use background task or Celery
    await process_document(session, str(document.id))
    
    # Refresh to get updated status
    await session.refresh(document)
    
    return UploadResponse(
        message="File uploaded and processed successfully",
        document=DocumentResponse.model_validate(document),
    )
```

Add the import:
```python
from app.services.document_processor import process_document
```

## Testing This Phase

### Upload and Process Document

```bash
curl -X POST http://localhost:8000/documents/upload \
  -H "Authorization: Bearer <token>" \
  -F "file=@./test.pdf"
```

Expected:
- Document created with status "processing"
- Chunks created with embeddings
- Status updated to "completed"
- Total tokens calculated

### Check Chunks

```bash
# Connect to database
docker-compose exec postgres psql -U pymind -d pymind

# Check chunks
SELECT id, document_id, chunk_index, 
       embedding IS NOT NULL as has_embedding,
       token_count 
FROM chunks 
LIMIT 5;
```

### Test Semantic Search

Create a test endpoint:

```python
@router.post("/search", response_model=List[ChunkResponse])
async def search_documents(
    query: str,
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> List[ChunkResponse]:
    """Search documents using semantic similarity."""
    
    # Generate query embedding
    query_embedding = await embedding_service.generate_embedding(query)
    
    # Search
    chunks = await embedding_service.search_similar(
        session,
        query_embedding,
        str(current_user.id),
    )
    
    return [ChunkResponse.model_validate(c) for c in chunks]
```

## Common Errors in This Phase

### Error 1: No Text Extracted

```
ValueError: No text content extracted from file
```

**Fix:** Check file is readable, PDF has extractable text

### Error 2: Embedding Dimension Mismatch

**Fix:** Ensure vector column matches embedding model dimensions (1536)

### Error 3: OpenAI Rate Limit

**Fix:** Add retry logic or reduce batch size

## Phase Summary

**What was built:**
- Text chunking with overlap
- OpenAI embedding generation
- Vector storage in pgvector
- Semantic search
- Document processing pipeline

**What was learned:**
- Recursive text splitting
- OpenAI embeddings API
- pgvector similarity search
- Async batch processing

## Next Phase

→ Phase 6 — RAG Pipeline: Implement the complete RAG pipeline with context retrieval, prompt building, and LLM generation.
