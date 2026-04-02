# GraphQL API Design

## Overview

GraphQL provides a flexible query language that allows clients to request exactly the data they need.

## Implementation with Strawberry

### Schema Definition

```python
# Example 1: GraphQL schema
import strawberry
from typing import List, Optional
from datetime import datetime

@strawberry.type
class User:
    id: int
    username: str
    email: str
    created_at: datetime

@strawberry.type
class Post:
    id: int
    title: str
    content: str
    author: User
    created_at: datetime

@strawberry.type
class Query:
    @strawberry.field
    async def user(self, id: int) -> Optional[User]:
        """Get user by ID"""
        return await get_user_by_id(id)

    @strawberry.field
    async def users(self, limit: int = 10) -> List[User]:
        """List users"""
        return await get_users(limit)

    @strawberry.field
    async def post(self, id: int) -> Optional[Post]:
        """Get post by ID"""
        return await get_post_by_id(id)

@strawberry.type
class Mutation:
    @strawberry.mutation
    async def create_user(self, username: str, email: str) -> User:
        """Create new user"""
        return await create_user_in_db(username, email)

    @strawberry.mutation
    async def create_post(self, title: str, content: str, author_id: int) -> Post:
        """Create new post"""
        return await create_post_in_db(title, content, author_id)

schema = strawberry.Schema(query=Query, mutation=Mutation)
```

### FastAPI Integration

```python
# Example 2: FastAPI with GraphQL
from fastapi import FastAPI
from strawberry.fastapi import GraphQLRouter

app = FastAPI()

graphql_router = GraphQLRouter(schema)
app.include_router(graphql_router, prefix="/graphql")

# REST and GraphQL together
@app.get("/api/users/{user_id}")
async def rest_get_user(user_id: int):
    """REST endpoint"""
    return await get_user_by_id(user_id)
```

## Query Examples

```graphql
# Example 3: GraphQL queries
query GetUser($id: Int!) {
  user(id: $id) {
    id
    username
    email
    posts {
      id
      title
    }
  }
}

mutation CreateUser($username: String!, $email: String!) {
  createUser(username: $username, email: $email) {
    id
    username
  }
}
```

## Summary

GraphQL provides flexible data querying that complements REST APIs.

## Next Steps

Continue learning about:
- [RESTful API Design](./01_restful_api_design.md)
- [Versioning Strategies](./03_versioning_strategies.md)
