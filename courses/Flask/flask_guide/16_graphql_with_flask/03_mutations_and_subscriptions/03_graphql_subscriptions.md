<!-- FILE: 16_graphql_with_flask/03_mutations_and_subscriptions/03_graphql_subscriptions.md -->

## Overview

Implement GraphQL subscriptions for real-time updates.

## Code Walkthrough

```python
# subscriptions.py
import strawberry
from asyncio import sleep

@strawberry.type
class Subscription:
    @strawberry.subscription
    async def on_message(self) -> str:
        while True:
            await sleep(1)
            yield "New message"
```
