<!-- FILE: 16_graphql_with_flask/04_sqlalchemy_integration/01_querying_models_via_graphql.md -->

## Overview

Query SQLAlchemy models through GraphQL resolvers.

## Code Walkthrough

```python
# schema.py
import strawberry
from .models import User, db

@strawberry.type
class UserType:
    id: int
    name: str
    email: str

@strawberry.type
class Query:
    @strawberry.field
    def users(self) -> list[UserType]:
        return User.query.all()
```
