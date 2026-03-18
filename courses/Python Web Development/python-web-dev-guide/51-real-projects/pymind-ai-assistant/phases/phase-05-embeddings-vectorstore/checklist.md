# Phase 5 — Embeddings and Vectorstore Checklist

Use this checklist to verify your implementation matches the reference code exactly.

## Prerequisites Checklist

- [ ] Phase 4 completed successfully
- [ ] Documents can be uploaded
- [ ] OpenAI API key configured

## Chunker Checklist

### app/utils/chunker.py

- [ ] TextChunker class
- [ ] chunk_size, chunk_overlap parameters
- [ ] separators list
- [ ] split_text() method
- [ ] create_chunks_with_metadata() method
- [ ] estimate_tokens() static method
- [ ] default_chunker instance

## Embedding Service Checklist

### app/services/embedding_service.py

- [ ] EmbeddingService class
- [ ] EMBEDDING_MODEL = "text-embedding-3-small"
- [ ] EMBEDDING_DIMENSIONS = 1536
- [ ] BATCH_SIZE = 100
- [ ] generate_embedding() method
- [ ] generate_embeddings_batch() method
- [ ] embed_chunks() method
- [ ] search_similar() method
- [ ] Global embedding_service instance

## Document Processor Checklist

### app/services/document_processor.py

- [ ] process_document() function
- [ ] Parse file to text
- [ ] Chunk text
- [ ] Generate embeddings
- [ ] Store in database
- [ ] Update status
- [ ] Error handling

## Integration Checklist

### app/routers/documents.py

- [ ] Import process_document
- [ ] Call process_document in upload endpoint
- [ ] Refresh document after processing

## Testing Checklist

### Test Upload and Process

```bash
curl -X POST http://localhost:8000/documents/upload \
  -H "Authorization: Bearer <token>" \
  -F "file=@./test.pdf"
```

- [ ] Document status changes to "processing"
- [ ] Chunks created with embeddings
- [ ] Status changes to "completed"
- [ ] Total tokens calculated

### Verify Database

```sql
SELECT COUNT(*) as chunk_count, 
       MIN(LENGTH(embedding)) is not null as has_embedding
FROM chunks 
WHERE document_id = '<doc-id>';
```

- [ ] Chunks exist for document
- [ ] Embeddings are stored (not null)

### Test Search

```python
# Test semantic search
chunks = await embedding_service.search_similar(
    session,
    query_embedding,
    user_id,
)
```

- [ ] Returns relevant chunks
- [ ] Ordered by similarity

## Code Quality Checklist

- [ ] Async embedding generation
- [ ] Batch processing
- [ ] Error handling
- [ ] Status tracking
- [ ] Token estimation

## Performance Checklist

- [ ] Batching reduces API calls
- [ ] Chunk overlap provides context
- [ ] Vector index for search

## Next Phase Preparation

Before proceeding to Phase 6, ensure:

- [ ] Documents processed automatically on upload
- [ ] Chunks stored with embeddings
- [ ] Search returns similar chunks
- [ ] Ready for RAG pipeline

## Sign-off

When all items are checked, you have completed Phase 5:

- [ ] Embedding pipeline complete
- [ ] Semantic search works
- [ ] Ready to proceed to Phase 6
