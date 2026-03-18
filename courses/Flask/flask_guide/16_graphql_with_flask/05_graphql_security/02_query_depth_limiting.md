<!-- FILE: 16_graphql_with_flask/05_graphql_security/02_query_depth_limiting.md -->

## Overview

Prevent deeply nested GraphQL queries that could cause performance issues.

## Core Concepts

Query depth limiting restricts how deep a GraphQL query can go. This prevents malicious or overly complex queries that could overload your server.

## Code Walkthrough

```python
# depth_limit.py
from strawberry import Schema
from strawberry.extensions import QueryDepthLimiter

schema = Schema(
    query=Query,
    mutation=Mutation,
    extensions=[
        lambda: QueryDepthLimiter(max_depth=3)  # Maximum 3 levels deep
    ]
)
```

## Common Mistakes

- ❌ Allowing unlimited query depth
- ✅ Setting reasonable depth limits (2-5)
