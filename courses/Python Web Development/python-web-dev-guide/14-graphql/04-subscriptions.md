# GraphQL Subscriptions

## What You'll Learn
- Real-time updates with subscriptions

## Prerequisites
- Completed queries and mutations

```python
import asyncio
import strawberry

@strawberry.type
class Post:
    id: int
    title: str

@strawberry.type
class Subscription:
    @strawberry.subscription
    async def on_post_created(self) -> Post:
        # This would connect to a message queue in production
        await asyncio.sleep(1)
        yield Post(id=1, title="New Post!")
```

## Summary
- Subscriptions provide real-time updates
- Uses WebSockets under the hood
