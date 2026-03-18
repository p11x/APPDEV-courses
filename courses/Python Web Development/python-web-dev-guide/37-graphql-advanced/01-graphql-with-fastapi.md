# GraphQL with FastAPI

## What You'll Learn
- GraphQL fundamentals
- Defining schemas with Strawberry
- Resolvers and data fetching
- Mutations and subscriptions
- Integration with FastAPI

## Prerequisites
- Completed FastAPI basics
- Understanding of REST APIs

## What Is GraphQL?

GraphQL is a query language for APIs that lets clients request exactly the data they need:

```
REST:          GET /users/1       → Returns fixed structure
               GET /users/1/posts → Returns fixed structure

GraphQL:       POST /graphql      → Returns exactly what asked
{
  user(id: 1) {
    name
    posts { title }
  }
}
```

## Installing Strawberry

```bash
pip install strawberry-graphql[asgi]
```

## Defining Schema

```python
import strawberry
from typing import Optional
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
    published: bool
    created_at: datetime

# Query root
@strawberry.type
class Query:
    @strawberry.field
    def hello(self) -> str:
        return "Hello, World!"
    
    @strawberry.field
    def user(self, id: int) -> Optional[User]:
        # Fetch from database
        return User(id=id, username="john", email="john@example.com", created_at=datetime.now())
    
    @strawberry.field
    def users(self) -> list[User]:
        return [
            User(id=1, username="john", email="john@example.com", created_at=datetime.now()),
            User(id=2, username="jane", email="jane@example.com", created_at=datetime.now()),
        ]
    
    @strawberry.field
    def posts(self) -> list[Post]:
        return [
            Post(id=1, title="Hello", content="World", author=User(id=1, username="john", email="john@example.com", created_at=datetime.now()), published=True, created_at=datetime.now())
        ]

schema = strawberry.Schema(query=Query)
```

## FastAPI Integration

```python
from fastapi import FastAPI
from strawberry.fastapi import GraphQLRouter

app = FastAPI()

graphql_router = GraphQLRouter(schema)

app.include_router(graphql_router, prefix="/graphql")
```

## Mutations

```python
@strawberry.type
class Mutation:
    @strawberry.mutation
    def create_user(self, username: str, email: str) -> User:
        # Save to database
        new_user = User(
            id=len(users) + 1,
            username=username,
            email=email,
            created_at=datetime.now()
        )
        return new_user
    
    @strawberry.mutation
    def create_post(self, title: str, content: str, author_id: int) -> Post:
        author = User(id=author_id, username="john", email="john@example.com", created_at=datetime.now())
        post = Post(
            id=1,
            title=title,
            content=content,
            author=author,
            published=True,
            created_at=datetime.now()
        )
        return post

schema = strawberry.Schema(query=Query, mutation=Mutation)
```

## Input Types

```python
@strawberry.input
class CreateUserInput:
    username: str
    email: str

@strawberry.input
class CreatePostInput:
    title: str
    content: str
    author_id: int

@strawberry.type
class Mutation:
    @strawberry.mutation
    def create_user(self, input: CreateUserInput) -> User:
        return User(id=1, username=input.username, email=input.email, created_at=datetime.now())
```

## Summary

- GraphQL lets clients request exactly the data they need
- Use Strawberry to define types and resolvers
- Implement queries for fetching data and mutations for updates
- Integrate with FastAPI using GraphQLRouter
