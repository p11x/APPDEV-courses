<!-- FILE: 16_graphql_with_flask/04_sqlalchemy_integration/02_solving_n_plus_one_with_dataloaders.md -->

## Overview

Solve N+1 query problems using DataLoaders in Strawberry GraphQL.

## Core Concepts

The N+1 problem occurs when querying related objects - for each parent, N additional queries are made for children. DataLoaders batch multiple requests into a single database query.

## Code Walkthrough

```python
# dataloaders.py
from strawberry import Info
from strawberry.dataloader import DataLoader
from .models import Post, db
import asyncio

async def load_posts(keys: list[int]) -> list[list[Post]]:
    # Batch load posts by user_id - single query for all keys
    posts = Post.query.filter(Post.user_id.in_(keys)).all()
    # Group by user_id
    posts_by_user = {}
    for post in posts:
        posts_by_user.setdefault(post.user_id, []).append(post)
    return [posts_by_user.get(key, []) for key in keys]

# In your schema
@strawberry.type
class Query:
    @strawberry.field
    def users(self, info: Info) -> list[UserType]:
        users = User.query.all()
        # This triggers the DataLoader for each user
        return users
```

## Common Mistakes

- ❌ Making individual queries in resolvers
- ✅ Using DataLoaders to batch queries
