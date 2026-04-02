# API Versioning

## Overview

API versioning manages changes while maintaining backward compatibility for existing clients.

## Versioning Strategies

### URL Path Versioning

```python
# Example 1: URL path versioning
from fastapi import FastAPI, APIRouter

app = FastAPI()

# Version 1
v1 = APIRouter(prefix="/api/v1", tags=["v1"])

@v1.get("/users")
async def v1_list_users():
    return {"version": 1, "users": []}

@v1.get("/users/{user_id}")
async def v1_get_user(user_id: int):
    return {"version": 1, "user_id": user_id}

# Version 2
v2 = APIRouter(prefix="/api/v2", tags=["v2"])

@v2.get("/users")
async def v2_list_users(page: int = 1, per_page: int = 20):
    return {"version": 2, "users": [], "pagination": {"page": page}}

@v2.get("/users/{user_id}")
async def v2_get_user(user_id: int, include_details: bool = False):
    result = {"version": 2, "user_id": user_id}
    if include_details:
        result["details"] = {}
    return result

app.include_router(v1)
app.include_router(v2)
```

### Header Versioning

```python
# Example 2: Header-based versioning
from fastapi import FastAPI, Header, HTTPException

app = FastAPI()

@app.get("/users")
async def list_users(x_api_version: str = Header("1")):
    if x_api_version == "1":
        return {"version": 1, "users": []}
    elif x_api_version == "2":
        return {"version": 2, "users": [], "meta": {}}
    else:
        raise HTTPException(400, f"Unsupported version: {x_api_version}")
```

## Best Practices

1. Support at least 2 versions simultaneously
2. Document deprecation timelines
3. Communicate breaking changes
4. Provide migration guides

## Summary

API versioning ensures backward compatibility while enabling evolution.

## Next Steps

Continue learning about:
- [GraphQL Implementation](./09_graphql_implementation.md)
- [API Deprecation](../03_api_design_patterns/10_api_deprecation_strategy.md)
