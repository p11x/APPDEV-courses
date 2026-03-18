# Phase 7 — Testing Checklist

Use this checklist to verify your implementation matches the reference code exactly.

## Prerequisites Checklist

- [ ] Phase 6 completed
- [ ] pytest installed

## Test Configuration Checklist

### pyproject.toml

- [ ] pytest in dev dependencies
- [ ] pytest-asyncio in dev dependencies
- [ ] pytest-cov in dev dependencies
- [ ] httpx in dev dependencies

### pytest.ini or pyproject.toml

- [ ] asyncio_mode = "auto"
- [ ] testpaths = ["tests"]
- [ ] addopts configured

## Fixtures Checklist

### tests/conftest.py

- [ ] event_loop fixture
- [ ] db_session fixture
- [ ] test_user fixture
- [ ] auth_client fixture
- [ ] unauth_client fixture
- [ ] Test database setup/teardown

## Auth Tests Checklist

### tests/test_auth.py

- [ ] test_register_success
- [ ] test_register_duplicate_email
- [ ] test_login_success
- [ ] test_login_invalid_password
- [ ] test_refresh_token

## Document Tests Checklist

### tests/test_documents.py

- [ ] test_list_documents_empty
- [ ] test_upload_document
- [ ] test_upload_invalid_file_type
- [ ] test_get_document
- [ ] test_delete_document

## Service Tests Checklist

### tests/test_services.py

- [ ] test_create_user
- [ ] test_login_success
- [ ] test_login_invalid_credentials

## Running Tests

```bash
# Run all tests
pytest

# Run with coverage
pytest --cov=app
```

- [ ] Tests pass
- [ ] Coverage report generated

## Code Quality

- [ ] Async tests work
- [ ] Fixtures properly scoped
- [ ] Test isolation

## Sign-off

When all items are checked, you have completed Phase 7:

- [ ] Tests written
- [ ] Tests pass
- [ ] Ready for Phase 8
