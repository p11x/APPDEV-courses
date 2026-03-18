# Phase 4 — Document Ingestion

## Goal

By the end of this phase, you will have:
- File upload endpoint with multipart/form-data
- Support for PDF, TXT, and DOCX files
- File size validation
- Document storage in database
- File storage on disk
- Processing status tracking

## What You'll Build in This Phase

- [ ] File upload endpoint
- [ ] File type validation
- [ ] File size validation
- [ ] File parser utilities
- [ ] Document creation service
- [ ] Background processing trigger

## Prerequisites

- Completed Phase 3 (authentication)
- Understanding of FastAPI file uploads

## File Upload Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    Document Upload Flow                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  POST /documents/upload                                         │
│  Content-Type: multipart/form-data                              │
│  Authorization: Bearer <token>                                  │
│                                                                  │
│  File: <PDF/DOCX/TXT file>                                      │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Validate file type │                                         │
│  │ (pdf, docx, txt)   │                                         │
│  └─────────────────────┘                                         │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Validate file size │                                         │
│  │ (< 10MB default)   │                                         │
│  └─────────────────────┘                                         │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Save file to disk  │                                         │
│  │ /uploads/{user_id} │                                         │
│  └─────────────────────┘                                         │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Create document     │                                         │
│  │ record in DB       │                                         │
│  └─────────────────────┘                                         │
│       │                                                          │
│       ▼                                                          │
│  ┌─────────────────────┐                                         │
│  │ Return document    │                                         │
│  │ with status        │                                         │
│  └─────────────────────┘                                         │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Step-by-Step Implementation

### Step 4.1 — Add Dependencies

First, add document parsing libraries to pyproject.toml:

```toml
# pyproject.toml additions
dependencies = [
    # ... existing ...
    "python-multipart>=0.0.6",  # Already added for forms
    "aiofiles>=23.2.0",
    "pypdf>=3.17.0",
    "python-docx>=1.1.0",
    "httpx>=0.26.0",
]
```

Install:
```bash
pip install -e ".[dev]"
```

### Step 4.2 — Create Document Schemas

```python
# app/schemas/document.py
"""
Pydantic schemas for document endpoints.
"""
from datetime import datetime
from typing import Optional, List
from pydantic import BaseModel, Field, ConfigDict


class DocumentBase(BaseModel):
    """Base document schema."""
    title: str = Field(..., description="Document title")


class DocumentCreate(DocumentBase):
    """Schema for document creation (internal use)."""
    file_name: str
    file_type: str
    file_size: int


class DocumentResponse(BaseModel):
    """Response schema for document data."""
    model_config = ConfigDict(from_attributes=True)
    
    id: str
    user_id: str
    title: str
    file_name: str
    file_type: str
    file_size: int
    status: str
    error_message: Optional[str] = None
    total_tokens: Optional[int] = None
    created_at: datetime
    updated_at: datetime


class DocumentListResponse(BaseModel):
    """Response for list of documents."""
    documents: List[DocumentResponse]
    total: int


class UploadResponse(BaseModel):
    """Response for file upload."""
    message: str
    document: DocumentResponse
```

### Step 4.3 — Create File Parser Utilities

```python
# app/utils/file_parser.py
"""
File parsing utilities for PDF, DOCX, and TXT files.
"""
from pathlib import Path
from typing import Optional
import aiofiles


class FileParser:
    """Parse different file types to extract text."""
    
    @staticmethod
    async def parse_pdf(file_path: Path) -> str:
        """
        Extract text from PDF file.
        
        Args:
            file_path: Path to PDF file
        
        Returns:
            Extracted text content
        """
        import pypdf
        
        text_parts = []
        
        with open(file_path, "rb") as f:
            reader = pypdf.PdfReader(f)
            
            for page in reader.pages:
                text_parts.append(page.extract_text())
        
        return "\n\n".join(text_parts)
    
    @staticmethod
    async def parse_docx(file_path: Path) -> str:
        """
        Extract text from DOCX file.
        
        Args:
            file_path: Path to DOCX file
        
        Returns:
            Extracted text content
        """
        from docx import Document
        
        doc = Document(file_path)
        text_parts = []
        
        for paragraph in doc.paragraphs:
            if paragraph.text.strip():
                text_parts.append(paragraph.text)
        
        # Also extract tables
        for table in doc.tables:
            for row in table.rows:
                row_text = " | ".join(
                    cell.text for cell in row.cells
                )
                if row_text.strip():
                    text_parts.append(row_text)
        
        return "\n\n".join(text_parts)
    
    @staticmethod
    async def parse_txt(file_path: Path) -> str:
        """
        Read plain text file.
        
        Args:
            file_path: Path to TXT file
        
        Returns:
            File content
        """
        async with aiofiles.open(file_path, "r", encoding="utf-8") as f:
            return await f.read()
    
    @classmethod
    async def parse_file(
        cls, 
        file_path: Path, 
        file_type: str
    ) -> str:
        """
        Parse file based on type.
        
        Args:
            file_path: Path to file
            file_type: MIME type or extension
        
        Returns:
            Extracted text content
        """
        # Normalize file type
        file_type = file_type.lower()
        
        if file_type == "application/pdf" or file_type.endswith("pdf"):
            return await cls.parse_pdf(file_path)
        
        elif (
            file_type == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            or file_type.endswith("docx")
        ):
            return await cls.parse_docx(file_path)
        
        elif file_type == "text/plain" or file_type.endswith("txt"):
            return await cls.parse_txt(file_path)
        
        else:
            raise ValueError(f"Unsupported file type: {file_type}")


async def get_text_content(file_path: Path, file_type: str) -> str:
    """
    Convenience function to parse file.
    
    Args:
        file_path: Path to file
        file_type: MIME type
    
    Returns:
        Text content
    """
    return await FileParser.parse_file(file_path, file_type)
```

🔍 **Line-by-Line Breakdown:**

1. `aiofiles` — Async file I/O for non-blocking reads
2. `pypdf.PdfReader` — Extracts text from PDF pages
3. `Document` from docx — Parses DOCX paragraphs and tables
4. `aiofiles.open()` — Async file read for TXT
5. `parse_file()` — Routes to correct parser based on file type
6. `ValueError` — Raised for unsupported file types

### Step 4.4 — Create File Storage Utility

```python
# app/utils/file_storage.py
"""
File storage utilities for uploaded documents.
"""
import uuid
from pathlib import Path
from typing import Optional

import aiofiles
from fastapi import UploadFile


UPLOAD_DIR = Path("uploads")


class FileStorage:
    """Handle file storage operations."""
    
    @staticmethod
    async def save_upload(
        upload_file: UploadFile,
        user_id: str,
        file_name: str,
    ) -> Path:
        """
        Save uploaded file to disk.
        
        Args:
            upload_file: FastAPI UploadFile
            user_id: Owner's user ID
            file_name: Original file name
        
        Returns:
            Path to saved file
        """
        # Create user directory
        user_dir = UPLOAD_DIR / user_id
        user_dir.mkdir(parents=True, exist_ok=True)
        
        # Generate unique filename
        ext = Path(file_name).suffix
        unique_name = f"{uuid.uuid4()}{ext}"
        file_path = user_dir / unique_name
        
        # Save file
        async with aiofiles.open(file_path, "wb") as f:
            content = await upload_file.read()
            await f.write(content)
        
        return file_path
    
    @staticmethod
    async def delete_file(file_path: Path) -> bool:
        """
        Delete file from disk.
        
        Args:
            file_path: Path to file
        
        Returns:
            True if deleted, False if not found
        """
        try:
            file_path.unlink()
            return True
        except FileNotFoundError:
            return False
    
    @staticmethod
    def get_file_path(user_id: str, file_name: str) -> Path:
        """Get path to stored file."""
        return UPLOAD_DIR / user_id / file_name
```

### Step 4.5 — Create Document Service

```python
# app/services/document_service.py
"""
Document service for file operations.
"""
from pathlib import Path
from typing import Optional
from uuid import UUID

from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.models import Document, DocumentStatus
from app.schemas.document import DocumentCreate
from app.utils.file_storage import FileStorage
from app.utils.file_parser import get_text_content


class DocumentService:
    """Service for document operations."""
    
    @staticmethod
    async def create_document(
        session: AsyncSession,
        user_id: UUID,
        file_name: str,
        file_type: str,
        file_size: int,
        title: Optional[str] = None,
    ) -> Document:
        """
        Create new document record.
        
        Args:
            session: Database session
            user_id: Owner's user ID
            file_name: Original file name
            file_type: MIME type
            file_size: File size in bytes
            title: Optional custom title
        
        Returns:
            Created document
        """
        # Use filename as title if not provided
        doc_title = title or Path(file_name).stem
        
        document = Document(
            user_id=str(user_id),
            title=doc_title,
            file_name=file_name,
            file_type=file_type,
            file_size=file_size,
            status=DocumentStatus.PENDING,
        )
        
        session.add(document)
        await session.commit()
        await session.refresh(document)
        
        return document
    
    @staticmethod
    async def get_document(
        session: AsyncSession,
        document_id: str,
        user_id: UUID,
    ) -> Optional[Document]:
        """
        Get document by ID for user.
        
        Args:
            session: Database session
            document_id: Document UUID
            user_id: Owner's user ID
        
        Returns:
            Document if found and owned by user
        """
        result = await session.execute(
            select(Document).where(
                Document.id == document_id,
                Document.user_id == str(user_id),
            )
        )
        return result.scalar_one_or_none()
    
    @staticmethod
    async def list_documents(
        session: AsyncSession,
        user_id: UUID,
        limit: int = 100,
        offset: int = 0,
    ) -> tuple[list[Document], int]:
        """
        List documents for user.
        
        Args:
            session: Database session
            user_id: Owner's user ID
            limit: Max results
            offset: Pagination offset
        
        Returns:
            Tuple of (documents, total count)
        """
        # Count total
        count_result = await session.execute(
            select(Document).where(
                Document.user_id == str(user_id)
            )
        )
        total = len(count_result.scalars().all())
        
        # Get paginated results
        result = await session.execute(
            select(Document)
            .where(Document.user_id == str(user_id))
            .order_by(Document.created_at.desc())
            .limit(limit)
            .offset(offset)
        )
        
        return list(result.scalars().all()), total
    
    @staticmethod
    async def update_status(
        session: AsyncSession,
        document_id: str,
        status: DocumentStatus,
        error_message: Optional[str] = None,
        total_tokens: Optional[int] = None,
    ) -> Optional[Document]:
        """
        Update document processing status.
        
        Args:
            session: Database session
            document_id: Document UUID
            status: New status
            error_message: Error message if failed
            total_tokens: Token count if completed
        
        Returns:
            Updated document
        """
        result = await session.execute(
            select(Document).where(Document.id == document_id)
        )
        document = result.scalar_one_or_none()
        
        if document:
            document.status = status
            if error_message:
                document.error_message = error_message
            if total_tokens:
                document.total_tokens = total_tokens
            
            await session.commit()
            await session.refresh(document)
        
        return document
    
    @staticmethod
    async def delete_document(
        session: AsyncSession,
        document_id: str,
        user_id: UUID,
    ) -> bool:
        """
        Delete document and associated file.
        
        Args:
            session: Database session
            document_id: Document UUID
            user_id: Owner's user ID
        
        Returns:
            True if deleted
        """
        document = await DocumentService.get_document(
            session, document_id, user_id
        )
        
        if not document:
            return False
        
        # Delete file from disk
        # Note: In production, store file_path in DB
        # For now, construct path from file_name
        from app.utils.file_storage import UPLOAD_DIR
        file_path = UPLOAD_DIR / str(user_id) / document.file_name
        
        await FileStorage.delete_file(file_path)
        
        # Delete from database (cascades to chunks)
        await session.delete(document)
        await session.commit()
        
        return True
```

### Step 4.6 — Create Document Router

```python
# app/routers/documents.py
"""
Document endpoints for file upload and management.
"""
from typing import List

from fastapi import APIRouter, Depends, File, HTTPException, UploadFile, status
from fastapi.responses import FileResponse
from sqlalchemy.ext.asyncio import AsyncSession

from app.config import get_settings
from app.dependencies import get_db_session
from app.dependencies.auth import CurrentUser
from app.models import Document
from app.schemas.document import (
    DocumentResponse,
    DocumentListResponse,
    UploadResponse,
)
from app.services.document_service import DocumentService
from app.utils.file_storage import FileStorage

router = APIRouter(prefix="/documents", tags=["Documents"])

# Supported file types
ALLOWED_EXTENSIONS = {".pdf", ".docx", ".txt"}
ALLOWED_MIME_TYPES = {
    "application/pdf",
    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    "text/plain",
}


def validate_file(file: UploadFile) -> None:
    """
    Validate file type and size.
    
    Raises:
        HTTPException: If file is invalid
    """
    settings = get_settings()
    
    # Check file extension
    ext = file.filename.split(".")[-1].lower() if "." in file.filename else ""
    
    if f".{ext}" not in ALLOWED_EXTENSIONS:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=f"File type not allowed. Allowed: {', '.join(ALLOWED_EXTENSIONS)}",
        )
    
    # Check file size (read first chunk to estimate)
    # Note: In production, use content-length header or stream with limit


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
    settings = get_settings()
    
    # Validate file type
    validate_file(file)
    
    # Read file content to check size
    content = await file.read()
    file_size = len(content)
    
    # Reset file pointer
    await file.seek(0)
    
    # Check file size
    max_size = settings.MAX_UPLOAD_SIZE_MB * 1024 * 1024
    
    if file_size > max_size:
        raise HTTPException(
            status_code=status.HTTP_413_REQUEST_ENTITY_TOO_LARGE,
            detail=f"File too large. Maximum size: {settings.MAX_UPLOAD_SIZE_MB}MB",
        )
    
    # Save file to disk
    file_path = await FileStorage.save_upload(
        file, 
        str(current_user.id), 
        file.filename
    )
    
    # Create document record
    document = await DocumentService.create_document(
        session,
        current_user.id,
        file.filename,
        file.content_type or "application/octet-stream",
        file_size,
    )
    
    # TODO: Trigger background processing
    # For now, mark as completed (will be async in Phase 5)
    document.status = DocumentStatus.COMPLETED
    await session.commit()
    
    return UploadResponse(
        message="File uploaded successfully",
        document=DocumentResponse.model_validate(document),
    )


@router.get(
    "/",
    response_model=DocumentListResponse,
    summary="List documents",
)
async def list_documents(
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
    limit: int = 100,
    offset: int = 0,
) -> DocumentListResponse:
    """
    List all documents for current user.
    
    - **limit**: Maximum number of results (default: 100)
    - **offset**: Pagination offset (default: 0)
    """
    documents, total = await DocumentService.list_documents(
        session, 
        current_user.id,
        limit,
        offset,
    )
    
    return DocumentListResponse(
        documents=[DocumentResponse.model_validate(d) for d in documents],
        total=total,
    )


@router.get(
    "/{document_id}",
    response_model=DocumentResponse,
    summary="Get document",
)
async def get_document(
    document_id: str,
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> DocumentResponse:
    """
    Get a specific document by ID.
    
    - **document_id**: Document UUID
    """
    document = await DocumentService.get_document(
        session, 
        document_id, 
        current_user.id
    )
    
    if not document:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Document not found",
        )
    
    return DocumentResponse.model_validate(document)


@router.delete(
    "/{document_id}",
    status_code=status.HTTP_204_NO_CONTENT,
    summary="Delete document",
)
async def delete_document(
    document_id: str,
    current_user: CurrentUser,
    session: AsyncSession = Depends(get_db_session),
) -> None:
    """
    Delete a document and its file.
    
    - **document_id**: Document UUID
    """
    deleted = await DocumentService.delete_document(
        session, 
        document_id, 
        current_user.id
    )
    
    if not deleted:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Document not found",
        )
```

## Testing This Phase

### Upload a Document

```bash
curl -X POST http://localhost:8000/documents/upload \
  -H "Authorization: Bearer <token>" \
  -F "file=@./document.pdf"
```

Expected response:
```json
{
  "message": "File uploaded successfully",
  "document": {
    "id": "uuid-here",
    "user_id": "user-uuid",
    "title": "document",
    "file_name": "document.pdf",
    "file_type": "application/pdf",
    "file_size": 12345,
    "status": "completed",
    ...
  }
}
```

### List Documents

```bash
curl -X GET http://localhost:8000/documents/ \
  -H "Authorization: Bearer <token>"
```

### Get Document

```bash
curl -X GET http://localhost:8000/documents/<document_id> \
  -H "Authorization: Bearer <token>"
```

### Delete Document

```bash
curl -X DELETE http://localhost:8000/documents/<document_id> \
  -H "Authorization: Bearer <token>"
```

## Common Errors in This Phase

### Error 1: File Type Not Allowed

```
{"detail": "File type not allowed. Allowed: .pdf, .docx, .txt"}
```

**Fix:** Upload a supported file type

### Error 2: File Too Large

```
{"detail": "File too large. Maximum size: 10MB"}
```

**Fix:** Compress file or split into smaller files

### Error 3: File Not Found on Delete

**Fix:** File may have already been deleted or path incorrect

## Phase Summary

**What was built:**
- File upload endpoint with validation
- Support for PDF, DOCX, TXT files
- File size validation
- File storage on disk
- Document CRUD operations

**What was learned:**
- FastAPI file upload handling
- Async file I/O with aiofiles
- File type validation
- Document storage patterns

## Next Phase

→ Phase 5 — Embeddings and Vectorstore: Implement text chunking, embedding generation, and vector storage in pgvector.
