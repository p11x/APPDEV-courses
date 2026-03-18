<!-- FILE: 16_graphql_with_flask/02_strawberry_graphql/02_defining_types_and_queries.md -->

## Overview

Define GraphQL types and queries with Strawberry.

## Code Walkthrough

```python
# schema.py
import strawberry

@strawberry.type
class User:
    id: int
    name: str
    email: str

@strawberry.query
class Query:
    @strawberry.field
    def users(self) -> list[User]:
        return [User(id=1, name="Alice", email="alice@example.com")]

schema = strawberry.Schema(query=Query)
```
