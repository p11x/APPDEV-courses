<!-- FILE: 16_graphql_with_flask/05_graphql_security/01_authentication_in_graphql.md -->

## Overview

Implement authentication in GraphQL endpoints.

## Core Concepts

GraphQL operates through a single endpoint, so authentication must be applied at the resolver level or through context. Use Flask-Login's current_user in resolver context.

## Code Walkthrough

```python
# auth.py
import strawberry
from flask import request
from flask_login import current_user
from functools import wraps

def login_required(f):
    @wraps(f)
    def wrapper(*args, **kwargs):
        if not current_user.is_authenticated:
            raise Exception("Not authenticated")
        return f(*args, **kwargs)
    return wrapper

@strawberry.type
class Query:
    @strawberry.field
    def me(self) -> UserType:
        if current_user.is_authenticated:
            return UserType.from_user(current_user)
        return None

@strawberry.type
class Mutation:
    @strawberry.mutation
    @login_required
    def update_profile(self, name: str) -> UserType:
        current_user.name = name
        db.session.commit()
        return UserType.from_user(current_user)
```

## Quick Reference

| Method | Description |
|--------|-------------|
| current_user | Flask-Login current user |
| @login_required | Decorator for protected mutations |
| info.context | Access request context |
