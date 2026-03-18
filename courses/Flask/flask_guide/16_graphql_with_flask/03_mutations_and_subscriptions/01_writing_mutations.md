<!-- FILE: 16_graphql_with_flask/03_mutations_and_subscriptions/01_writing_mutations.md -->

## Overview

Write GraphQL mutations with Strawberry.

## Code Walkthrough

```python
# mutations.py
import strawberry

@strawberry.type
class Mutation:
    @strawberry.mutation
    def create_user(self, name: str, email: str) -> User:
        user = User(name=name, email=email)
        return user
```
