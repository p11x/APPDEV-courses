# Phase 4 — Document Ingestion Checklist

Use this checklist to verify your implementation matches the reference code exactly.

## Prerequisites Checklist

- [ ] Phase 3 completed successfully
- [ ] Authentication working
- [ ] Dependencies installed (pypdf, python-docx, aiofiles)

## Dependencies Checklist

### pyproject.toml additions

- [ ] pypdf>=3.17.0
- [ ] python-docx>=1.1.0
- [ ] aiofiles>=23.2.0

## Schemas Checklist

### app/schemas/document.py

- [ ] DocumentBase schema
- [ ] DocumentCreate schema
- [ ] DocumentResponse with from_attributes=True
- [ ] DocumentListResponse
- [ ] UploadResponse

## Utilities Checklist

### app/utils/file_parser.py

- [ ] FileParser class
- [ ] parse_pdf() method
- [ ] parse_docx() method
- [ ] parse_txt() method
- [ ] parse_file() class method
- [ ] get_text_content() function

### app/utils/file_storage.py

- [ ] FileStorage class
- [ ] save_upload() static method
- [ ] delete_file() static method
- [ ] get_file_path() static method
- [ ] UPLOAD_DIR Path constant

## Service Checklist

### app/services/document_service.py

- [ ] DocumentService class
- [ ] create_document() method
- [ ] get_document() method
- [ ] list_documents() method
- [ ] update_status() method
- [ ] delete_document() method

## Router Checklist

### app/routers/documents.py

- [ ] /documents/upload endpoint (POST)
- [ ] /documents/ endpoint (GET)
- [ ] /documents/{document_id} endpoint (GET)
- [ ] /documents/{document_id} endpoint (DELETE)
- [ ] File type validation
- [ ] File size validation
- [ ] ALLOWED_EXTENSIONS set
- [ ] ALLOWED_MIME_TYPES set
- [ ] validate_file() function

## Testing Checklist

### Test Upload

```bash
curl -X POST http://localhost:8000/documents/upload \
  -H "Authorization: Bearer <token>" \
  -F "file=@./test.pdf"
```

- [ ] Returns 201 on success
- [ ] Returns document data
- [ ] Returns 400 for invalid file type
- [ ] Returns 413 for file too large

### Test List

```bash
curl -X GET http://localhost:8000/documents/ \
  -H "Authorization: Bearer <token>"
```

- [ ] Returns list of documents
- [ ] Returns total count
- [ ] Supports pagination

### Test Get

```bash
curl -X GET http://localhost:8000/documents/<id> \
  -H "Authorization: Bearer <token>"
```

- [ ] Returns document if owned
- [ ] Returns 404 if not found

### Test Delete

```bash
curl -X DELETE http://localhost:8000/documents/<id> \
  -H "Authorization: Bearer <token>"
```

- [ ] Returns 204 on success
- [ ] Returns 404 if not found

## Code Quality Checklist

- [ ] All functions have type hints
- [ ] Async file operations
- [ ] Proper error handling
- [ ] File validation before processing
- [ ] Size limits enforced

## Security Checklist

- [ ] User can only access own documents
- [ ] File type validation
- [ ] File size limits
- [ ] No path traversal vulnerabilities

## Next Phase Preparation

Before proceeding to Phase 5, ensure:

- [ ] Files can be uploaded
- [ ] Files are saved to disk
- [ ] Document records created in DB
- [ ] List/get/delete work
- [ ] Ready for embedding generation

## Sign-off

When all items are checked, you have completed Phase 4:

- [ ] Document upload complete
- [ ] File parsing works
- [ ] CRUD operations work
- [ ] Ready to proceed to Phase 5
