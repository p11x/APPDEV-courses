# API Documentation Tools

## Overview

API documentation tools generate interactive documentation from FastAPI code.

## OpenAPI/Swagger

### Auto-Generated Documentation

```python
# Example 1: Enhanced API documentation
from fastapi import FastAPI
from pydantic import BaseModel, Field

app = FastAPI(
    title="My API",
    description="A comprehensive API with great documentation",
    version="1.0.0",
    terms_of_service="https://example.com/terms",
    contact={
        "name": "API Support",
        "email": "support@example.com",
        "url": "https://example.com/support"
    },
    license_info={
        "name": "MIT",
        "url": "https://opensource.org/licenses/MIT"
    }
)

class Item(BaseModel):
    """Item model with documentation"""
    name: str = Field(
        ...,
        description="The name of the item",
        examples=["Laptop", "Phone"]
    )
    price: float = Field(
        ...,
        description="Price in USD",
        gt=0,
        examples=[999.99, 49.99]
    )

    class Config:
        json_schema_extra = {
            "examples": [
                {"name": "Laptop", "price": 999.99},
                {"name": "Phone", "price": 499.99}
            ]
        }

@app.get(
    "/items/{item_id}",
    summary="Get an item",
    description="Retrieve a specific item by its ID",
    response_description="The requested item",
    responses={
        200: {"description": "Item found"},
        404: {"description": "Item not found"}
    },
    tags=["items"]
)
async def get_item(
    item_id: int = Path(..., description="The ID of the item to retrieve")
):
    """Get item endpoint with full documentation"""
    return {"item_id": item_id}
```

## ReDoc Alternative

### ReDoc Configuration

```python
# Example 2: ReDoc documentation
from fastapi import FastAPI
from fastapi.openapi.docs import get_redoc_html

app = FastAPI(docs_url=None, redoc_url=None)

@app.get("/docs", include_in_schema=False)
async def get_swagger_docs():
    return get_swagger_ui_html(openapi_url="/openapi.json")

@app.get("/redoc", include_in_schema=False)
async def get_redoc_docs():
    return get_redoc_html(openapi_url="/openapi.json")
```

## Summary

Auto-generated documentation improves API usability.

## Next Steps

Continue learning about:
- [API Testing Tools](./09_api_testing_tools.md)
- [API Design Tools](./11_api_design_tools.md)
