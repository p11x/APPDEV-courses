<!-- FILE: 16_graphql_with_flask/04_sqlalchemy_integration/03_pagination_in_graphql.md -->

## Overview

Implement pagination in GraphQL queries for large datasets.

## Core Concepts

Cursor-based pagination uses an opaque cursor string instead of offset/limit. It's more efficient than offset pagination for large datasets.

## Code Walkthrough

```python
# pagination.py
import strawberry
from dataclasses import dataclass
from .models import User, db

@strawberry.type
class UserConnection:
    edges: list[UserEdge]
    page_info: PageInfo

@strawberry.type
class UserEdge:
    node: UserType
    cursor: str

@strawberry.type
class PageInfo:
    has_next_page: bool
    end_cursor: str

@strawberry.type
class Query:
    @strawberry.field
    def users(self, first: int = 10, after: str = None) -> UserConnection:
        query = User.query
        if after:
            query = query.filter(User.id > int(after))
        users = query.limit(first + 1).all()
        has_next = len(users) > first
        users = users[:first]
        edges = [UserEdge(node=UserType.from_user(u), cursor=str(u.id)) for u in users]
        return UserConnection(
            edges=edges,
            page_info=PageInfo(
                has_next_page=has_next,
                end_cursor=edges[-1].cursor if edges else None
            )
        )
```

## Common Mistakes

- ❌ Using offset for pagination on large tables
- ✅ Using cursor-based pagination
