# GraphQL with FastAPI

## What You'll Learn
- Combining GraphQL with FastAPI
```python
from fastapi import FastAPI
from strawberry.fastapi import GraphQLRouter

app = FastAPI()

graphql_router = GraphQLRouter(schema)
app.include_router(graphql_router, prefix="/graphql")
```

## Summary
- FastAPI integrates with Strawberry
- Single endpoint for all GraphQL operations
