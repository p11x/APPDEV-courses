<!-- FILE: 16_graphql_with_flask/05_graphql_security/03_rate_limiting_graphql.md -->

## Overview

Apply rate limiting to GraphQL endpoints.

## Core Concepts

Rate limiting prevents abuse by restricting how many requests a client can make in a given time window. Use Flask-Limiter with GraphQL.

## Code Walkthrough

```python
# rate_limit.py
from flask_limiter import Limiter
from flask_limiter.util import get_remote_address

limiter = Limiter(
    key_func=get_remote_address,
    default_limits=["200 per day", "50 per hour"]
)

# Apply to GraphQL endpoint
@app.route("/graphql", methods=["POST"])
@limiter.limit("100 per minute")
def graphql():
    return schema.execute(request.json)
```

## Quick Reference

| Limit Type | Description |
|------------|-------------|
| per minute | Short-term rate limit |
| per hour | Medium-term rate limit |
| per day | Long-term rate limit |
