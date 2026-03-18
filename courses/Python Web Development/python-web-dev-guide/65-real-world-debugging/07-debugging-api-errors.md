# Debugging API Errors

## What You'll Learn

- Understanding API errors
- Debugging HTTP errors
- Testing APIs

## Prerequisites

- Completed `06-debugging-database.md`

## Common API Errors

- 400 Bad Request - Invalid request
- 401 Unauthorized - Auth issues
- 404 Not Found - Resource missing
- 500 Server Error - Backend issue

## Using curl

```bash
# Test API endpoint
curl -X GET http://localhost:8000/api/users
```

## Summary

- Check HTTP status codes
- Test with curl or Postman

## Next Steps

Continue to `08-memory-debugging.md`.
