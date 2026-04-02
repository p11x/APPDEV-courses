# FastAPI Serverless

## Overview

Deploy FastAPI as serverless functions on AWS Lambda, Azure Functions, or Google Cloud Functions for cost-effective, auto-scaling APIs.

## AWS Lambda with Mangum

### Basic Setup

```python
# Example 1: FastAPI with Mangum for Lambda
# app/main.py
from fastapi import FastAPI
from mangum import Mangum

app = FastAPI(title="Serverless FastAPI")

@app.get("/")
def root():
    return {"message": "Hello from Lambda!"}

@app.get("/items/{item_id}")
def read_item(item_id: int, q: str = None):
    return {"item_id": item_id, "q": q}

@app.post("/items/")
def create_item(name: str, price: float):
    return {"name": name, "price": price}

# Lambda handler
handler = Mangum(app, lifespan="off")
```

### Requirements

```text
# requirements.txt
fastapi>=0.100.0
mangum>=0.17.0
pydantic>=2.0.0
```

### SAM Template

```yaml
# Example 2: SAM template.yaml
AWSTemplateFormatVersion: '2010-09-09'
Transform: AWS::Serverless-2016-10-31

Globals:
  Function:
    Timeout: 30
    MemorySize: 256
    Runtime: python3.11

Resources:
  FastAPIFunction:
    Type: AWS::Serverless::Function
    Properties:
      Handler: app.main.handler
      CodeUri: .
      Description: FastAPI Serverless Application
      Events:
        Root:
          Type: HttpApi
          Properties:
            Path: /
            Method: ANY
        Proxy:
          Type: HttpApi
          Properties:
            Path: /{proxy+}
            Method: ANY
      Environment:
        Variables:
          ENVIRONMENT: production

Outputs:
  ApiUrl:
    Description: API Gateway URL
    Value: !Sub "https://${ServerlessHttpApi}.execute-api.${AWS::Region}.amazonaws.com"
```

## Cold Start Optimization

### Reducing Cold Starts

```python
# Example 3: Optimized for cold starts
from fastapi import FastAPI
from mangum import Mangum
import os

# Minimal app initialization
app = FastAPI()

# Lazy loading for heavy imports
def get_database():
    """Lazy load database connection"""
    from app.database import SessionLocal
    return SessionLocal()

@app.get("/")
async def root():
    return {"status": "ok"}

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    # Only import when needed
    db = get_database()
    try:
        item = db.query(Item).filter(Item.id == item_id).first()
        return item
    finally:
        db.close()

# Use lifespan for connection pooling
from contextlib import asynccontextmanager

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup: Initialize connections
    from app.database import init_db
    await init_db()
    yield
    # Shutdown: Clean up
    from app.database import close_db
    await close_db()

app = FastAPI(lifespan=lifespan)

handler = Mangum(app)
```

### Layer Optimization

```dockerfile
# Example 4: Lambda layer for dependencies
# Dockerfile for Lambda layer
FROM public.ecr.aws/lambda/python:3.11

COPY requirements.txt .
RUN pip install -r requirements.txt -t /opt/python/

# Package as layer
# zip -r layer.zip python/
```

## Azure Functions

### Azure Setup

```python
# Example 5: FastAPI on Azure Functions
# function_app.py
import azure.functions as func
from fastapi import FastAPI
from mangum import Mangum

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Hello from Azure Functions!"}

@app.get("/health")
def health():
    return {"status": "healthy"}

# Create Azure Function
handler = Mangum(app)

# function.json configuration
```

```json
// Example 6: Azure Functions configuration
{
  "scriptFile": "__init__.py",
  "bindings": [
    {
      "authLevel": "function",
      "type": "httpTrigger",
      "direction": "in",
      "name": "req",
      "methods": ["get", "post", "put", "delete"],
      "route": "{*route}"
    },
    {
      "type": "http",
      "direction": "out",
      "name": "$return"
    }
  ]
}
```

## Google Cloud Functions

### GCP Deployment

```python
# Example 7: FastAPI on Google Cloud Functions
# main.py
from fastapi import FastAPI
from mangum import Mangum
import functions_framework

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Hello from Google Cloud Functions!"}

@functions_framework.http
def api(request):
    """Cloud Functions handler"""
    handler = Mangum(app)
    return handler(request)
```

```yaml
# Example 8: GCP deployment configuration
# cloudbuild.yaml
steps:
  - name: 'gcr.io/cloud-builders/gcloud'
    args:
      - 'functions'
      - 'deploy'
      - 'fastapi-api'
      - '--runtime=python311'
      - '--trigger-http'
      - '--allow-unauthenticated'
      - '--entry-point=api'
      - '--region=us-central1'
```

## Database Connections

### Serverless Database

```python
# Example 9: Serverless database connection
from fastapi import FastAPI
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.pool import QueuePool
import os

app = FastAPI()

# Connection pooling for serverless
engine = create_engine(
    os.environ.get("DATABASE_URL"),
    poolclass=QueuePool,
    pool_size=1,  # Minimal for serverless
    max_overflow=0,
    pool_pre_ping=True  # Verify connections
)

SessionLocal = sessionmaker(bind=engine)

def get_db():
    """Database session dependency"""
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()
```

## Environment Variables

### Configuration

```python
# Example 10: Environment configuration
from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    # Application
    APP_NAME: str = "FastAPI Serverless"
    DEBUG: bool = False

    # Database
    DATABASE_URL: str

    # External services
    REDIS_URL: str = ""

    # AWS specific
    AWS_REGION: str = "us-east-1"

    class Config:
        env_file = ".env"

settings = Settings()
```

## Error Handling

### Serverless Error Handling

```python
# Example 11: Error handling for serverless
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import logging

app = FastAPI()
logger = logging.getLogger()

@app.exception_handler(Exception)
async def global_exception_handler(request: Request, exc: Exception):
    """Global exception handler for serverless"""
    logger.error(f"Error: {exc}", exc_info=True)

    return JSONResponse(
        status_code=500,
        content={
            "error": "Internal server error",
            "request_id": request.headers.get("x-request-id")
        }
    )

@app.exception_handler(404)
async def not_found_handler(request: Request, exc):
    return JSONResponse(
        status_code=404,
        content={"error": "Not found"}
    )
```

## Testing

### Local Testing

```python
# Example 12: Local testing with SAM
"""
# Test locally with SAM CLI
sam local start-api

# Test specific event
sam local invoke FastAPIFunction -e events/event.json
"""

# tests/test_serverless.py
import pytest
from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)

def test_root():
    response = client.get("/")
    assert response.status_code == 200
    assert response.json() == {"message": "Hello from Lambda!"}

def test_get_item():
    response = client.get("/items/1")
    assert response.status_code == 200
    assert response.json()["item_id"] == 1
```

## Best Practices

### Serverless Guidelines

```python
# Example 13: Best practices
"""
Serverless Best Practices:

1. Minimize cold starts
   - Keep dependencies minimal
   - Use Lambda layers
   - Avoid heavy initialization

2. Connection management
   - Use connection pooling
   - Close connections properly
   - Consider RDS Proxy

3. Stateless design
   - Don't rely on local storage
   - Use external caches (Redis)
   - Store state in database

4. Error handling
   - Log to CloudWatch
   - Return proper HTTP codes
   - Include request IDs

5. Monitoring
   - Custom CloudWatch metrics
   - X-Ray tracing
   - Set up alarms
"""
```

## Deployment

### Deploy Commands

```bash
# Example 14: Deployment commands

# Build with SAM
sam build

# Deploy
sam deploy --guided

# Deploy with existing config
sam deploy

# View logs
sam logs -n FastAPIFunction --tail

# Local development
sam local start-api
```

## Summary

| Platform | Handler | Deployment |
|----------|---------|------------|
| AWS Lambda | Mangum | SAM/Serverless |
| Azure Functions | Mangum | Azure CLI |
| GCP Cloud Functions | Mangum | gcloud |

## Next Steps

Continue learning about:
- [AWS Lambda](./02_aws_lambda.md) - Detailed Lambda setup
- [Cold Start Optimization](./16_serverless_cold_start.md) - Performance
- [Serverless Monitoring](./07_serverless_monitoring.md) - Observability
