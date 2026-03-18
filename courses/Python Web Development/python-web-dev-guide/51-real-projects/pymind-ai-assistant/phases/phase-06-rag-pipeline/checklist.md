# Phase 6 — RAG Pipeline Checklist

Use this checklist to verify your implementation matches the reference code exactly.

## Prerequisites Checklist

- [ ] Phase 5 completed successfully
- [ ] Embeddings stored in database
- [ ] OpenAI API configured

## RAG Service Checklist

### app/services/rag_service.py

- [ ] RagService class
- [ ] MAX_CONTEXT_TOKENS = 4000
- [ ] TOP_K = 5
- [ ] SIMILARITY_THRESHOLD = 0.7
- [ ] RAG_SYSTEM_PROMPT constant
- [ ] retrieve_context() method
- [ ] build_prompt() method
- [ ] generate_response() with streaming
- [ ] generate_with_citations() method
- [ ] Global rag_service instance

## Chat Schemas Checklist

### app/schemas/chat.py

- [ ] MessageCreate schema
- [ ] MessageResponse schema
- [ ] ConversationCreate schema
- [ ] ConversationResponse schema
- [ ] ChatRequest schema
- [ ] ChatResponse schema
- [ ] StreamChunk schema

## Chat Service Checklist

### app/services/chat_service.py

- [ ] ChatService class
- [ ] create_conversation() method
- [ ] get_conversation() method
- [ ] list_conversations() method
- [ ] add_message() method
- [ ] get_conversation_history() method

## Chat Router Checklist

### app/routers/chat.py

- [ ] /chat/ POST endpoint
- [ ] /chat/stream POST endpoint with StreamingResponse
- [ ] /chat/conversations GET endpoint
- [ ] /chat/conversations/{id} GET endpoint
- [ ] SSE event format

## Testing Checklist

### Test Chat Endpoint

```bash
curl -X POST http://localhost:8000/chat/ \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"message": "What is in my document?"}'
```

- [ ] Returns response
- [ ] Returns conversation_id
- [ ] Returns sources/citations

### Test Streaming

```bash
curl -X POST http://localhost:8000/chat/stream \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"message": "Tell me about..."}' \
  -N
```

- [ ] Streams response chunks
- [ ] Sends done signal at end

### Test Conversations

```bash
curl -X GET http://localhost:8000/chat/conversations \
  -H "Authorization: Bearer <token>"
```

- [ ] Returns list of conversations

## Code Quality Checklist

- [ ] Async streaming
- [ ] Proper error handling
- [ ] Citation tracking
- [ ] Conversation persistence

## Next Phase Preparation

Before proceeding to Phase 7, ensure:

- [ ] Chat works end-to-end
- [ ] Streaming works
- [ ] Conversations persist
- [ ] Ready for testing

## Sign-off

When all items are checked, you have completed Phase 6:

- [ ] RAG pipeline complete
- [ ] Chat functionality works
- [ ] Ready to proceed to Phase 7
