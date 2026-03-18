# API Documentation

## What You'll Learn
- OpenAPI/Swagger documentation
- Adding descriptions
- Custom documentation

## Prerequisites
- Completed HATEOAS

## FastAPI Automatic Docs

FastAPI automatically generates:
- Swagger UI: `/docs`
- ReDoc: `/redoc`

```python
from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI(
    title="My API",
    description="A sample API for demonstration",
    version="1.0.0",
    docs_url="/docs",
    redoc_url="/redoc"
)

class Item(BaseModel):
    name: str
    description: str | None = None
    price: float
    quantity: int

@app.post("/items", tags=["items"])
async def create_item(item: Item):
    """Create a new item in the store"""
    return {"item": item, "id": 1}

@app.get("/items/{item_id}", tags=["items"])
async def get_item(item_id: int):
    """Get item by ID"""
    return {"item_id": item_id, "name": "Sample Item"}

@app.get("/health", tags=["health"])
async def health_check():
    """Check API health"""
    return {"status": "healthy"}
```

## Custom Documentation

```python
from fastapi import FastAPI
from fastapi.openapi.utils import get_openapi

app = FastAPI()

def custom_openapi():
    if app.openapi_schema:
        return app.openapi_schema
    
    openapi_schema = get_openapi(
        title="E-commerce API",
        version="2.0.0",
        description="API for online store",
        routes=app.routes,
    )
    
    # Add custom info
    openapi_schema["info"]["contact"] = {
        "name": "API Support",
        "email": "support@example.com"
    }
    
    app.openapi_schema = openapi_schema
    return app.openapi_schema

app.openapi = custom_openapi
```

## Summary
- Use built-in documentation
- Add proper descriptions
- Group endpoints with tags

## Next Steps
→ Move to `19-file-handling/`
