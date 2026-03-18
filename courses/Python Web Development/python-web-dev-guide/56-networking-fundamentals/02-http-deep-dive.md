# HTTP Deep Dive

## What You'll Learn

- HTTP methods and status codes
- Headers and cookies
- HTTPS and TLS
- HTTP/2 and HTTP/3

## Prerequisites

- Completed `01-network-models-and-protocols.md`

## HTTP Methods

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    HTTP METHODS                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  GET     — Retrieve data                                                   │
│  POST    — Create new resource                                             │
│  PUT     — Replace entire resource                                         │
│  PATCH   — Partial update                                                  │
│  DELETE  — Remove resource                                                │
│                                                                             │
│  IDEMPOTENT: GET, PUT, DELETE (same result multiple times)               │
│  NOT IDEMPOTENT: POST, PATCH                                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Status Codes

```
1xx — Informational
2xx — Success
3xx — Redirection
4xx — Client Error
5xx — Server Error
```

Common codes:
- 200 OK
- 201 Created
- 301 Moved Permanently
- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 500 Internal Server Error
- 502 Bad Gateway
- 503 Service Unavailable

## Summary

- HTTP methods: GET, POST, PUT, PATCH, DELETE
- Status codes indicate result
- Headers carry metadata

## Next Steps

→ Continue to more networking topics.
