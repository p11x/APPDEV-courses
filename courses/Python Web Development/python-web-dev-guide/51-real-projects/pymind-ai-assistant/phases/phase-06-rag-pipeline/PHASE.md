# Phase 6 — RAG Pipeline

## Goal

By the end of this phase, you will have:
- Complete RAG pipeline (Retrieve, Augment, Generate)
- Chat endpoint with context
- Streaming LLM responses
- Source citations
- Conversation history

## Prerequisites

- Completed Phase 5 (embeddings and vectorstore)
- Understanding of LLM prompts

## RAG Pipeline Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    RAG Pipeline Flow                             │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  User Query: "What does the contract say about liability?"       │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ 1. EMBED QUERY                                             ││
│  │    "What does the contract say about liability?"            ││
│  │    → [0.12, -0.45, 0.78, ...] (1536 dims)                  ││
│  └─────────────────────────────────────────────────────────────┘│
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ 2. RETRIEVE (Vector Search)                                ││
│  │    → Chunk 1 (0.92 similarity): "Liability provisions..." ││
│  │    → Chunk 2 (0.89 similarity): "Indemnification..."       ││
│  │    → Chunk 3 (0.85 similarity): "Warranty..."              ││
│  └─────────────────────────────────────────────────────────────┘│
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ 3. BUILD PROMPT                                            ││
│  │    System: "You are a helpful assistant..."                ││
│  │    Context: "From the document: Liability provisions..."   ││
│  │    Question: "What does the contract say about liability?"  ││
│  └─────────────────────────────────────────────────────────────┘│
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ 4. GENERATE (LLM)                                          ││
│  │    → "According to Section 5.1 of the contract,             ││
│  │        liability is defined as..."                         ││
│  └─────────────────────────────────────────────────────────────┘│
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ 5. STREAM RESPONSE                                         ││
│  │    → "According" → "to" → "Section" → "5.1" → ...          ││
│  └─────────────────────────────────────────────────────────────┘│
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Step-by-Step Implementation

### Step 6.1 — Create RAG Service

```python
# app/services/rag_service.py
"""
RAG (Retrieval Augmented Generation) service.
"""
from typing import AsyncGenerator, Optional
from uuid import UUID

from openai import AsyncOpenAI
from sqlalchemy.ext.asyncio import AsyncSession

from app.config import get_settings
from app.models import Chunk, Conversation, Message, MessageRole
from app.services.embedding_service import embedding_service
from app.services.document_service import DocumentService


# System prompt for RAG
RAG_SYSTEM_PROMPT = """You are a helpful AI assistant that answers questions based on the provided documents.

When answering:
1. Only use information from the provided context
2. Cite your sources using the document references
3. If you cannot find the answer in the context, say so honestly
4. Be concise and accurate

Your responses should be helpful, accurate, and based solely on the provided context."""


class RagService:
    """Service for RAG pipeline operations."""
    
    MAX_CONTEXT_TOKENS = 4000
    TOP_K = 5
    SIMILARITY_THRESHOLD = 0.7
    
    def __init__(self):
        settings = get_settings()
        self.client = AsyncOpenAI(api_key=settings.OPENAI_API_KEY)
    
    async def retrieve_context(
        self,
        session: AsyncSession,
        query: str,
        user_id: str,
        document_id: Optional[str] = None,
    ) -> list[Chunk]:
        """
        Retrieve relevant context from vector store.
        
        Args:
            session: Database session
            query: User query
            user_id: Owner's user ID
            document_id: Optional document filter
        
        Returns:
            List of relevant chunks
        """
        # Generate query embedding
        query_embedding = await embedding_service.generate_embedding(query)
        
        # Search similar chunks
        chunks = await embedding_service.search_similar(
            session,
            query_embedding,
            user_id,
            document_id,
            self.TOP_K,
            self.SIMILARITY_THRESHOLD,
        )
        
        return chunks
    
    def build_prompt(
        self,
        query: str,
        chunks: list[Chunk],
    ) -> list[dict]:
        """
        Build prompt with context.
        
        Args:
            query: User question
            chunks: Retrieved context chunks
        
        Returns:
            Message list for LLM
        """
        # Format context from chunks
        context_parts = []
        
        for i, chunk in enumerate(chunks):
            context_parts.append(
                f"[Document {i+1}]:\n{chunk.content}"
            )
        
        context_text = "\n\n".join(context_parts)
        
        # Build messages
        messages = [
            {
                "role": "system",
                "content": RAG_SYSTEM_PROMPT,
            },
            {
                "role": "user",
                "content": f"""Context from documents:

{context_text}

---

Question: {query}

Answer the question based on the context above. Cite your sources.""",
            },
        ]
        
        return messages
    
    async def generate_response(
        self,
        messages: list[dict],
    ) -> AsyncGenerator[str, None]:
        """
        Generate streaming response from LLM.
        
        Args:
            messages: Prompt messages
        
        Yields:
            Response chunks
        """
        stream = await self.client.chat.completions.create(
            model="gpt-4o",
            messages=messages,
            temperature=0.7,
            stream=True,
        )
        
        async for chunk in stream:
            content = chunk.choices[0].delta.content
            
            if content:
                yield content
    
    async def generate_with_citations(
        self,
        query: str,
        chunks: list[Chunk],
    ) -> tuple[str, list[dict]]:
        """
        Generate response and track citations.
        
        Args:
            query: User question
            chunks: Retrieved chunks
        
        Returns:
            Tuple of (response, citations)
        """
        messages = self.build_prompt(query, chunks)
        
        # Collect full response
        response_parts = []
        
        async for part in self.generate_response(messages):
            response_parts.append(part)
        
        response = "".join(response_parts)
        
        # Build citations
        citations = [
            {
                "document_id": chunk.document_id,
                "content": chunk.content[:200] + "...",
                "chunk_index": chunk.chunk_index,
            }
            for chunk in chunks
        ]
        
        return response, citations


# Global instance
rag_service = RagService()
```

### Step 6.2 — Create Chat Schemas

```python
# app/schemas/chat.py
"""
Pydantic schemas for chat endpoints.
"""
from datetime import datetime
from typing import Optional, List
from pydantic import BaseModel, Field, ConfigDict


class MessageCreate(BaseModel):
    """Create a new message."""
    content: str = Field(..., description="Message content")
    conversation_id: Optional[str] = Field(None, description="Conversation ID (for continuing)")


class MessageResponse(BaseModel):
    """Message response schema."""
    model_config = ConfigDict(from_attributes=True)
    
    id: str
    conversation_id: str
    role: str
    content: str
    token_count: Optional[int] = None
    created_at: datetime


class ConversationCreate(BaseModel):
    """Create a new conversation."""
    title: str = Field(..., description="Conversation title")


class ConversationResponse(BaseModel):
    """Conversation response schema."""
    model_config = ConfigDict(from_attributes=True)
    
    id: str
    user_id: str
    title: str
    created_at: datetime
    updated_at: datetime


class ChatRequest(BaseModel):
    """Chat request schema."""
    message: str = Field(..., description="User message")
    conversation_id: Optional[str] = Field(None, description="Continue existing conversation")
    document_id: Optional[str] = Field(None, description="Filter to specific document")


class ChatResponse(BaseModel):
    """Chat response schema."""
    message: str
    conversation_id: str
    sources: List[dict] = Field(default_factory=list)


class StreamChunk(BaseModel):
    """Streaming chunk."""
    content: str
    done: bool = False
```

### Step 6.3 — Create Chat Service

```python
# app/services/chat_service.py
"""
Chat service for conversation management.
"""
from typing import Optional
from uuid import uuid4

from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.models import Conversation, Message, MessageRole
from app.services.rag_service import rag_service


class ChatService:
    """Service for chat and conversation operations."""
    
    @staticmethod
    async def create_conversation(
        session: AsyncSession,
        user_id: str,
        title: str,
    ) -> Conversation:
        """
        Create a new conversation.
        
        Args:
            session: Database session
            user_id: Owner's user ID
            title: Conversation title
        
        Returns:
            Created conversation
        """
        conversation = Conversation(
            user_id=user_id,
            title=title,
        )
        
        session.add(conversation)
        await session.commit()
        await session.refresh(conversation)
        
        return conversation
    
    @staticmethod
    async def get_conversation(
        session: AsyncSession,
        conversation_id: str,
        user_id: str,
    ) -> Optional[Conversation]:
        """
        Get conversation by ID.
        
        Args:
            session: Database session
            conversation_id: Conversation ID
            user_id: Owner's user ID
        
        Returns:
            Conversation if found
        """
        result = await session.execute(
            select(Conversation).where(
                Conversation.id == conversation_id,
                Conversation.user_id == user_id,
            )
        )
        
        return result.scalar_one_or_none()
    
    @staticmethod
    async def list_conversations(
        session: AsyncSession,
        user_id: str,
        limit: int = 50,
    ) -> list[Conversation]:
        """
        List user's conversations.
        
        Args:
            session: Database session
            user_id: Owner's user ID
            limit: Max results
        
        Returns:
            List of conversations
        """
        result = await session.execute(
            select(Conversation)
            .where(Conversation.user_id == user_id)
            .order_by(Conversation.updated_at.desc())
            .limit(limit)
        )
        
        return list(result.scalars().all())
    
    @staticmethod
    async def add_message(
        session: AsyncSession,
        conversation_id: str,
        role: MessageRole,
        content: str,
        token_count: Optional[int] = None,
        citations: Optional[str] = None,
    ) -> Message:
        """
        Add message to conversation.
        
        Args:
            session: Database session
            conversation_id: Conversation ID
            role: Message role
            content: Message content
            token_count: Optional token count
            citations: Optional citations JSON
        
        Returns:
            Created message
        """
        import json
        
        message = Message(
            conversation_id=conversation_id,
            role=role,
            content=content,
            token_count=token_count,
            citations=json.dumps(citations) if citations else None,
        )
        
        session.add(message)
        
        # Update conversation
        conversation = await ChatService.get_conversation(
            session, conversation_id, None
        )
        
        if conversation:
            conversation.updated_at = message.created_at
        
        await session.commit()
        await session.refresh(message)
        
        return message
    
    @staticmethod
    async def get_conversation_history(
        session: AsyncSession,
        conversation_id: str,
    ) -> list[Message]:
        """
        Get conversation message history.
        
        Args:
            session: Database session
            conversation_id: Conversation ID
        
        Returns:
            List of messages
        """
        result = await session.execute(
            select(Message)
            .where(Message.conversation_id == conversation_id)
            .order_by(Message.created_at)
        )
        
        return list(result.scalars().all())
```

### Step 6.4 — Create Chat Router with Streaming

```python
# app/routers/chat.py
"""
Chat endpoints for RAG-powered conversations.
"""
import json
from typing import AsyncGenerator

from fastapi import APIRouter, Depends, HTTPException
from fastapi.responses import StreamingResponse
from sqlalchemy.ext.asyncio import AsyncSession

from app.dependencies import get_db_session
from app.dependencies.auth import CurrentUser
from app.models import Conversation, Message, MessageRole
from app.schemas.chat import (
    ChatRequest,
    ChatResponse,
    ConversationCreate,
    ConversationResponse,
    MessageResponse,
)
from app.services.chat_service import ChatService
from app.services.rag_service import rag_service
from app.services.embedding_service import embedding_service

router = APIRouter(prefix="/chat", tags=["Chat"])


@router.post(
    "/",
    response_model=ChatResponse,
    summary="Send chat message",
)
async def send_message(
    request: ChatRequest,
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> ChatResponse:
    """
    Send a chat message and get a response.
    
    - **message**: Your question or message
    - **conversation_id**: Optional, to continue existing conversation
    - **document_id**: Optional, to filter sources to specific document
    """
    # Get or create conversation
    if request.conversation_id:
        conversation = await ChatService.get_conversation(
            session,
            request.conversation_id,
            str(current_user.id),
        )
        
        if not conversation:
            raise HTTPException(
                status_code=404,
                detail="Conversation not found",
            )
    else:
        # Create new conversation
        # Use first 50 chars of message as title
        title = request.message[:50] + "..." if len(request.message) > 50 else request.message
        
        conversation = await ChatService.create_conversation(
            session,
            str(current_user.id),
            title,
        )
    
    # Save user message
    user_message = await ChatService.add_message(
        session,
        str(conversation.id),
        MessageRole.USER,
        request.message,
    )
    
    # Get conversation history for context
    history = await ChatService.get_conversation_history(
        session,
        str(conversation.id),
    )
    
    # Build RAG query with history
    # For simplicity, just use current query
    # In production, include recent messages in prompt
    
    # Retrieve context
    chunks = await rag_service.retrieve_context(
        session,
        request.message,
        str(current_user.id),
        request.document_id,
    )
    
    if not chunks:
        # No relevant context found
        ai_response = "I couldn't find any relevant information in your documents to answer that question. Try uploading more documents or rephrasing your question."
        
        citations = []
    else:
        # Generate response with citations
        ai_response, citations = await rag_service.generate_with_citations(
            request.message,
            chunks,
        )
    
    # Save AI response
    ai_message = await ChatService.add_message(
        session,
        str(conversation.id),
        MessageRole.ASSISTANT,
        ai_response,
        citations=citations,
    )
    
    return ChatResponse(
        message=ai_response,
        conversation_id=str(conversation.id),
        sources=[
            {"document_id": c["document_id"], "chunk_index": c["chunk_index"]}
            for c in citations
        ],
    )


@router.post(
    "/stream",
    summary="Send chat message with streaming response",
)
async def stream_message(
    request: ChatRequest,
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> StreamingResponse:
    """
    Send a chat message and get a streaming response.
    
    Uses Server-Sent Events (SSE) for streaming.
    """
    # Get or create conversation (same as above)
    if request.conversation_id:
        conversation = await ChatService.get_conversation(
            session,
            request.conversation_id,
            str(current_user.id),
        )
        
        if not conversation:
            raise HTTPException(
                status_code=404,
                detail="Conversation not found",
            )
    else:
        title = request.message[:50] + "..." if len(request.message) > 50 else request.message
        
        conversation = await ChatService.create_conversation(
            session,
            str(current_user.id),
            title,
        )
    
    # Save user message
    await ChatService.add_message(
        session,
        str(conversation.id),
        MessageRole.USER,
        request.message,
    )
    
    # Retrieve context
    chunks = await rag_service.retrieve_context(
        session,
        request.message,
        str(current_user.id),
        request.document_id,
    )
    
    # Build prompt
    messages = rag_service.build_prompt(request.message, chunks)
    
    async def event_generator() -> AsyncGenerator[str, None]:
        response_parts = []
        
        async for chunk in rag_service.generate_response(messages):
            response_parts.append(chunk)
            
            yield f"data: {json.dumps({'content': chunk})}\n\n"
        
        # Save full response
        full_response = "".join(response_parts)
        
        citations = [
            {"document_id": c.document_id, "chunk_index": c.chunk_index}
            for c in chunks
        ] if chunks else []
        
        await ChatService.add_message(
            session,
            str(conversation.id),
            MessageRole.ASSISTANT,
            full_response,
            citations=citations,
        )
        
        yield f"data: {json.dumps({'done': True, 'conversation_id': str(conversation.id)})}\n\n"
    
    return StreamingResponse(
        event_generator(),
        media_type="text/event-stream",
    )


@router.get(
    "/conversations",
    response_model=list[ConversationResponse],
    summary="List conversations",
)
async def list_conversations(
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
    limit: int = 50,
) -> list[ConversationResponse]:
    """
    List all conversations for current user.
    """
    conversations = await ChatService.list_conversations(
        session,
        str(current_user.id),
        limit,
    )
    
    return [ConversationResponse.model_validate(c) for c in conversations]


@router.get(
    "/conversations/{conversation_id}",
    response_model=list[MessageResponse],
    summary="Get conversation messages",
)
async def get_conversation(
    conversation_id: str,
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> list[MessageResponse]:
    """
    Get all messages in a conversation.
    """
    conversation = await ChatService.get_conversation(
        session,
        conversation_id,
        str(current_user.id),
    )
    
    if not conversation:
        raise HTTPException(
            status_code=404,
            detail="Conversation not found",
        )
    
    messages = await ChatService.get_conversation_history(
        session,
        conversation_id,
    )
    
    return [MessageResponse.model_validate(m) for m in messages]
```

## Testing This Phase

### Test Chat

```bash
curl -X POST http://localhost:8000/chat/ \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "message": "What does my document say about machine learning?"
  }'
```

Expected response:
```json
{
  "message": "According to your document...",
  "conversation_id": "uuid-here",
  "sources": [{"document_id": "...", "chunk_index": 0}]
}
```

### Test Streaming

```bash
curl -X POST http://localhost:8000/chat/stream \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "message": "What is the main topic?"
  }'
```

Expected: Server-Sent Events stream

### Test Conversation List

```bash
curl -X GET http://localhost:8000/chat/conversations \
  -H "Authorization: Bearer <token>"
```

## Common Errors in This Phase

### Error 1: No Context Found

**Fix:** Upload documents and ensure processing completed

### Error 2: Streaming Timeout

**Fix:** Check network, reduce response length

## Phase Summary

**What was built:**
- Complete RAG pipeline
- Chat endpoint with context retrieval
- Streaming response support
- Conversation management
- Source citations

**What was learned:**
- RAG architecture
- Prompt engineering
- SSE streaming
- Citation tracking

## Next Phase

→ Phase 7 — Testing: Write unit tests, integration tests, and end-to-end tests for the API.
