# Strawberry GraphQL

## What You'll Learn
- Using Strawberry for GraphQL in Python

## Prerequisites
- Completed GraphQL fundamentals

```bash
pip install strawberry-graphql
```

```python
import strawberry

@strawberry.type
class User:
    name: str
    email: str

@strawberry.type
class Query:
    @strawberry.field
    def user(self) -> User:
        return User(name="Alice", email="alice@example.com")

schema = strawberry.Schema(query=Query)
```

## Summary
- Strawberry is a GraphQL library for Python
- Uses type hints for schema definition
