# GraphQL Implementation

## Overview

GraphQL provides flexible query capabilities for FastAPI applications using Strawberry or Ariadne.

## Strawberry GraphQL

### Complete Implementation

```python
# Example 1: Strawberry GraphQL with FastAPI
import strawberry
from strawberry.fastapi import GraphQLRouter
from typing import List, Optional
from datetime import datetime

@strawberry.type
class User:
    id: int
    username: str
    email: str
    created_at: datetime

    @strawberry.field
    async def posts(self) -> List["Post"]:
        """Resolve user's posts"""
        return await get_user_posts(self.id)

@strawberry.type
class Post:
    id: int
    title: str
    content: str
    author_id: int
    created_at: datetime

    @strawberry.field
    async def author(self) -> User:
        """Resolve post author"""
        return await get_user_by_id(self.author_id)

@strawberry.type
class Query:
    @strawberry.field
    async def user(self, id: int) -> Optional[User]:
        return await get_user_by_id(id)

    @strawberry.field
    async def users(self, limit: int = 10) -> List[User]:
        return await get_users(limit)

    @strawberry.field
    async def post(self, id: int) -> Optional[Post]:
        return await get_post_by_id(id)

    @strawberry.field
    async def posts(self, author_id: Optional[int] = None) -> List[Post]:
        if author_id:
            return await get_posts_by_author(author_id)
        return await get_all_posts()

@strawberry.type
class Mutation:
    @strawberry.mutation
    async def create_user(self, username: str, email: str) -> User:
        return await create_user_in_db(username, email)

    @strawberry.mutation
    async def create_post(
        self,
        title: str,
        content: str,
        author_id: int
    ) -> Post:
        return await create_post_in_db(title, content, author_id)

    @strawberry.mutation
    async def update_post(
        self,
        id: int,
        title: Optional[str] = None,
        content: Optional[str] = None
    ) -> Post:
        return await update_post_in_db(id, title, content)

# Create schema
schema = strawberry.Schema(query=Query, mutation=Mutation)

# FastAPI integration
graphql_router = GraphQLRouter(schema)

app = FastAPI()
app.include_router(graphql_router, prefix="/graphql")
```

### GraphQL with Authentication

```python
# Example 2: Authenticated GraphQL
from strawberry.types import Info
from functools import wraps

def require_auth(func):
    """Decorator for requiring authentication"""
    @wraps(func)
    async def wrapper(root, info: Info, *args, **kwargs):
        user = info.context.get("user")
        if not user:
            raise Exception("Authentication required")
        return await func(root, info, *args, **kwargs)
    return wrapper

@strawberry.type
class ProtectedQuery:
    @strawberry.field
    @require_auth
    async def me(self, info: Info) -> User:
        """Get current user"""
        return info.context["user"]

    @strawberry.field
    @require_auth
    async def my_posts(self, info: Info) -> List[Post]:
        """Get current user's posts"""
        user = info.context["user"]
        return await get_posts_by_author(user.id)

# Context provider
async def get_context(request: Request):
    """Provide GraphQL context"""
    token = request.headers.get("Authorization")
    user = None

    if token and token.startswith("Bearer "):
        user = await verify_token(token[7:])

    return {"user": user, "request": request}

graphql_router = GraphQLRouter(
    schema,
    context_getter=get_context
)
```

## Hybrid REST + GraphQL

```python
# Example 3: REST and GraphQL together
from fastapi import FastAPI

app = FastAPI()

# REST endpoints
@app.get("/api/users/{user_id}")
async def rest_get_user(user_id: int):
    return await get_user_by_id(user_id)

@app.post("/api/users/")
async def rest_create_user(user: UserCreate):
    return await create_user_in_db(user.username, user.email)

# GraphQL endpoint
app.include_router(graphql_router, prefix="/graphql")

# Clients can choose:
# - REST for simple CRUD
# - GraphQL for complex queries
```

## Summary

GraphQL provides flexible data querying for FastAPI applications.

## Next Steps

Continue learning about:
- [gRPC Integration](./02_grpc_integration.md)
- [Real-Time APIs](../07_advanced_patterns_and_modern_apis/05_real_time_apis/)
