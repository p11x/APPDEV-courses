# Phase 3 — Authentication Checklist

Use this checklist to verify your implementation matches the reference code exactly.

## Prerequisites Checklist

- [ ] Phase 2 completed successfully
- [ ] All database models created
- [ ] Database migrations applied

## Security Module Checklist

### app/core/security.py

- [ ] pwd_context with bcrypt configured
- [ ] TokenData Pydantic model
- [ ] Token Pydantic model
- [ ] verify_password() function
- [ ] hash_password() function
- [ ] create_access_token() with exp and type claims
- [ ] create_refresh_token() with longer expiration
- [ ] verify_token() with proper error handling

## Schemas Checklist

### app/schemas/auth.py

- [ ] UserCreate with email (EmailStr), username, password
- [ ] Field descriptions and validators
- [ ] UserResponse with from_attributes=True
- [ ] UserLogin schema
- [ ] TokenResponse schema
- [ ] RefreshTokenRequest schema
- [ ] MessageResponse schema

## Service Checklist

### app/services/auth_service.py

- [ ] AuthService class
- [ ] register() method with duplicate checks
- [ ] login() method with password verification
- [ ] create_tokens() method
- [ ] refresh_access_token() method
- [ ] Proper error handling and return tuples

## Dependencies Checklist

### app/dependencies/auth.py

- [ ] HTTPBearer security scheme
- [ ] get_current_user() dependency
- [ ] get_optional_user() dependency
- [ ] CurrentUser type alias
- [ ] OptionalUser type alias
- [ ] Proper HTTPException for auth failures

## Router Checklist

### app/routers/auth.py

- [ ] /auth/register endpoint (POST)
- [ ] /auth/login endpoint (POST)
- [ ] /auth/refresh endpoint (POST)
- [ ] /auth/logout endpoint (POST)
- [ ] Proper status codes
- [ ] Error handling with HTTPException

## Main App Integration Checklist

### app/main.py

- [ ] Auth router imported
- [ ] Auth router included in app

### app/dependencies/__init__.py

- [ ] Exports get_db_session

## Testing Checklist

### Test Registration

```bash
curl -X POST http://localhost:8000/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "username": "testuser",
    "password": "securepassword123"
  }'
```

- [ ] Returns 201 on success
- [ ] Returns user data without password
- [ ] Returns 400 if email exists
- [ ] Returns 400 if username exists

### Test Login

```bash
curl -X POST http://localhost:8000/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "securepassword123"
  }'
```

- [ ] Returns 200 with tokens on success
- [ ] Returns 401 if user not found
- [ ] Returns 401 if password incorrect

### Test Protected Route

```bash
curl -X GET http://localhost:8000/documents/ \
  -H "Authorization: Bearer <access_token>"
```

- [ ] Returns 200 with data if valid token
- [ ] Returns 401 if no token
- [ ] Returns 401 if invalid token

### Test Token Refresh

```bash
curl -X POST http://localhost:8000/auth/refresh \
  -H "Content-Type: application/json" \
  -d '{"refresh_token": "<refresh_token>"}'
```

- [ ] Returns new tokens on success
- [ ] Returns 401 if refresh token invalid

## Code Quality Checklist

- [ ] All functions have type hints
- [ ] All schemas have field descriptions
- [ ] Proper error messages
- [ ] No hardcoded values (use settings)
- [ ] Security best practices followed

## Security Checklist

- [ ] Passwords hashed with bcrypt
- [ ] JWT tokens have expiration
- [ ] Separate access and refresh tokens
- [ ] Token verification with proper error handling
- [ ] User ownership enforced on protected routes
- [ ] No sensitive data in responses

## Common Issues and Fixes

| Issue | Cause | Fix |
|-------|-------|-----|
| 401 on valid token | Token type wrong | Verify token has "type": "access" |
| Password verify fails | Hash algorithm mismatch | Ensure bcrypt used consistently |
| User not found | Wrong user_id in token | Verify token sub matches user id |

## Next Phase Preparation

Before proceeding to Phase 4, ensure:

- [ ] Users can register
- [ ] Users can login and receive tokens
- [ ] Protected routes work with token
- [ ] Token refresh works
- [ ] Error handling is proper
- [ ] Ready to add document upload

## Sign-off

When all items are checked, you have completed Phase 3:

- [ ] Authentication flow complete
- [ ] All endpoints tested
- [ ] Security best practices followed
- [ ] Ready to proceed to Phase 4
