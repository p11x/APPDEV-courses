# FastAPI Overview

## What is FastAPI?

FastAPI is a modern, high-performance web framework for building APIs with Python based on standard Python type hints. Created by Sebastián Ramírez in 2018, it has rapidly become one of the most popular Python frameworks due to its exceptional performance, developer experience, and automatic documentation generation.

## Key Features

### 1. High Performance
FastAPI delivers performance comparable to NodeJS and Go, thanks to its foundation on Starlette (for web parts) and Pydantic (for data parts). It leverages Python's async capabilities through ASGI (Asynchronous Server Gateway Interface).

### 2. Automatic Documentation
FastAPI automatically generates interactive API documentation:
- **Swagger UI**: Available at `/docs`
- **ReDoc**: Available at `/redoc`

### 3. Type Hints and Validation
Using Python type hints, FastAPI provides:
- Editor support (autocompletion, type checking)
- Data validation
- Data serialization
- Automatic documentation

### 4. Standards-Based
Built on and fully compatible with:
- OpenAPI (formerly Swagger) for API creation
- JSON Schema for data schema documentation

## Architecture Overview

### ASGI (Asynchronous Server Gateway Interface)
FastAPI is an ASGI framework, meaning it supports both synchronous and asynchronous code. This enables:
- Non-blocking I/O operations
- WebSocket support
- HTTP/2 support
- Background tasks

```python
# Example demonstrating async capabilities
from fastapi import FastAPI
import asyncio

app = FastAPI()

@app.get("/async-endpoint")
async def async_handler():
    # This function can handle concurrent requests efficiently
    # The 'async' keyword allows non-blocking operations
    await asyncio.sleep(0.1)  # Simulating an async operation
    return {"message": "This endpoint supports async operations"}
```

### Starlette
FastAPI is built on top of Starlette, which provides:
- Request and response classes
- Routing system
- Middleware support
- WebSocket handling
- Static files serving

```python
# Starlette features accessible through FastAPI
from fastapi import FastAPI, Request, Response
from starlette.middleware.cors import CORSMiddleware

app = FastAPI()

# Adding Starlette middleware through FastAPI
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/middleware-demo")
async def middleware_demo(request: Request):
    # Access Starlette's Request object
    return {"client_host": request.client.host}
```

### Pydantic
Pydantic handles data validation and settings management using Python type annotations:
- Data validation at runtime
- Automatic conversion of input data
- JSON Schema generation
- Custom validators

```python
# Pydantic model integration
from fastapi import FastAPI
from pydantic import BaseModel, EmailStr

app = FastAPI()

class User(BaseModel):
    # Pydantic validates these fields automatically
    username: str
    email: EmailStr  # Validates email format
    age: int

@app.post("/users/")
async def create_user(user: User):
    # FastAPI uses Pydantic to validate incoming JSON
    # If validation fails, FastAPI returns a 422 error automatically
    return {"user": user.username, "email": user.email}
```

## When to Use FastAPI

### Ideal Use Cases

1. **REST APIs**
   - Building web APIs for mobile or frontend applications
   - Microservices architecture

2. **Machine Learning Model Serving**
   - Exposing ML models as APIs
   - Data science applications

3. **Backend Services**
   - High-performance backend systems
   - Real-time applications

4. **Prototyping**
   - Quick API development
   - MVP development

```python
# Example: ML Model Serving
from fastapi import FastAPI
from pydantic import BaseModel
import numpy as np

app = FastAPI()

class PredictionRequest(BaseModel):
    features: list[float]

class PredictionResponse(BaseModel):
    prediction: float
    confidence: float

@app.post("/predict", response_model=PredictionResponse)
async def predict(request: PredictionRequest):
    # Simulate ML model prediction
    features_array = np.array(request.features)
    prediction = float(np.mean(features_array))
    return PredictionResponse(
        prediction=prediction,
        confidence=0.95
    )
```

### When to Consider Alternatives

- **Simple static websites**: Consider Flask or Django
- **Full-stack applications with templates**: Django might be more suitable
- **Legacy system integration**: Existing frameworks already in use

## Target Audience

FastAPI is designed for:

1. **Backend Developers**
   - Python developers building APIs
   - Developers transitioning from other frameworks

2. **Data Scientists**
   - Those needing to deploy models as APIs
   - Building data pipelines

3. **Full-Stack Developers**
   - Creating backend services for frontend applications
   - Building complete web applications

4. **DevOps Engineers**
   - Building microservices
   - Creating automation APIs

## Getting Started Preview

Here's a minimal FastAPI application to illustrate the basics:

```python
# main.py - Your first FastAPI application
from fastapi import FastAPI

# Create an instance of the FastAPI class
# This instance will be the main point of interaction for creating your API
app = FastAPI(
    title="My First API",
    description="A simple API to demonstrate FastAPI basics",
    version="0.1.0"
)

# Define a path operation using a decorator
# @app.get("/") tells FastAPI that this function handles GET requests to "/"
@app.get("/")
async def root():
    """
    Root endpoint that returns a welcome message.
    FastAPI will use this docstring in the auto-generated documentation.
    """
    return {"message": "Welcome to FastAPI!"}

# Run this file with: uvicorn main:app --reload
# Then visit: http://localhost:8000
# For documentation: http://localhost:8000/docs
```

## Summary

| Feature | Description |
|---------|-------------|
| Performance | High-performance ASGI framework |
| Documentation | Auto-generated Swagger UI and ReDoc |
| Validation | Automatic data validation via Pydantic |
| Type Safety | Full editor support with type hints |
| Standards | OpenAPI and JSON Schema compliant |
| Async Support | Native async/await support |

## Next Steps

Continue to the next sections to learn:
- [Why FastAPI?](./02_why_fastapi.md) - Detailed advantages and benefits
- [Comparison with Other Frameworks](./03_comparison_with_other_frameworks.md) - How FastAPI compares to alternatives
