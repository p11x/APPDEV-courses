# Logging with Structlog

## What You'll Learn
- Structured logging fundamentals
- Configuring structlog
- Adding context to logs
- Integration with FastAPI
- Log levels and filtering

## Prerequisites
- Python logging basics

## Structured vs Plain Text

Plain text logs are hard to parse:
```
2024-01-15 10:30:45 INFO User logged in: john
```

Structured logs are machine-readable:
```json
{"timestamp": "2024-01-15T10:30:45", "level": "INFO", "event": "user_login", "user_id": 123}
```

## Installation

```bash
pip install structlog
```

## Basic Configuration

```python
import structlog
import logging

# Configure structlog
structlog.configure(
    processors=[
        structlog.stdlib.filter_by_level,
        structlog.stdlib.add_logger_name,
        structlog.stdlib.add_log_level,
        structlog.stdlib.PositionalArgumentsFormatter(),
        structlog.processors.TimeStamper(fmt="iso"),
        structlog.processors.StackInfoRenderer(),
        structlog.processors.format_exc_info,
        structlog.processors.UnicodeDecoder(),
        structlog.processors.JSONRenderer()
    ],
    wrapper_class=structlog.stdlib.BoundLogger,
    context_class=dict,
    logger_factory=structlog.stdlib.LoggerFactory(),
    cache_logger_on_first_use=True,
)

# Get logger
logger = structlog.get_logger()
```

## Usage in FastAPI

```python
from fastapi import FastAPI, Request
import structlog

app = FastAPI()
logger = structlog.get_logger()

@app.middleware("http")
async def add_context(request: Request, call_next):
    """Add request context to logs."""
    structlog.contextvars.clear_context_vars()
    structlog.contextvars.bind_contextvars(
        request_id=request.headers.get("X-Request-ID", "unknown"),
        path=request.url.path,
        method=request.method,
    )
    
    response = await call_next(request)
    
    structlog.contextvars.bind_contextvars(
        status_code=response.status_code
    )
    
    return response

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    logger.info("fetching_user", user_id=user_id)
    return {"user_id": user_id}

@app.post("/users")
async def create_user(user_data: dict):
    logger.info("creating_user", user_data=user_data)
    return {"status": "created"}
```

## Summary

- Structured logging creates machine-readable JSON logs
- Use structlog for Python applications
- Add context (request_id, user_id) to all logs
- Configure processors for timestamps, stack traces, etc.
