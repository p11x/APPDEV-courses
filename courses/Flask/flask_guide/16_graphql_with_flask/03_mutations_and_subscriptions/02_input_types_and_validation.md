<!-- FILE: 16_graphql_with_flask/03_mutations_and_subscriptions/02_input_types_and_validation.md -->

## Overview

Define input types and validation in GraphQL.

## Code Walkthrough

```python
# input_types.py
import strawberry
from dataclasses import dataclass

@strawberry.input
class CreateUserInput:
    name: str
    email: str

@strawberry.type
class Mutation:
    @strawberry.mutation
    def create_user(self, input: CreateUserInput) -> User:
        # Validation happens automatically
        return User(name=input.name, email=input.email)
```
