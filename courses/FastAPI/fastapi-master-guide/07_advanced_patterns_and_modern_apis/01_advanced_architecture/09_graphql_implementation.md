# GraphQL Implementation

## Overview

GraphQL provides a flexible query language for APIs, allowing clients to request exactly the data they need.

## FastAPI with Strawberry

### Basic Setup

```python
# Example 1: GraphQL with Strawberry
import strawberry
from fastapi import FastAPI
from strawberry.fastapi import GraphQLRouter
from typing import List, Optional

@strawberry.type
class User:
    id: int
    username: str
    email: str

@strawberry.type
class Query:
    @strawberry.field
    def user(self, id: int) -> Optional[User]:
        # Fetch from database
        return User(id=id, username="john", email="john@example.com")

    @strawberry.field
    def users(self) -> List[User]:
        return [
            User(id=1, username="john", email="john@example.com"),
            User(id=2, username="jane", email="jane@example.com"),
        ]

@strawberry.type
class Mutation:
    @strawberry.mutation
    def create_user(self, username: str, email: str) -> User:
        # Create user in database
        return User(id=1, username=username, email=email)

schema = strawberry.Schema(query=Query, mutation=Mutation)
graphql_app = GraphQLRouter(schema)

app = FastAPI()
app.include_router(graphql_app, prefix="/graphql")
```

## Hybrid REST + GraphQL

```python
# Example 2: Combined REST and GraphQL
from fastapi import FastAPI

app = FastAPI()

# REST endpoints
@app.get("/api/users/{user_id}")
async def rest_get_user(user_id: int):
    return {"user_id": user_id}

# GraphQL endpoint
app.include_router(graphql_app, prefix="/graphql")
```

## Summary

GraphQL complements REST by providing flexible data querying capabilities.

## Next Steps

Continue learning about:
- [WebSocket Patterns](./10_websocket_patterns.md)
- [API Design Patterns](../03_api_design_patterns/01_restful_api_design.md)
