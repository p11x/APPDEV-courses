# Serverless Architecture

## What You'll Learn

- Serverless computing concepts
- Building with AWS Lambda
- Python functions as a service
- Cold starts and optimization

## Prerequisites

- Understanding of REST APIs
- Basic knowledge of cloud services

## Introduction

Serverless computing doesn't mean "no servers"—it means you don't manage the servers. You write code as functions that run in response to events, and the cloud provider handles scaling, capacity planning, and server maintenance.

Think of serverless like hiring a taxi versus owning a car. With a taxi (serverless), you just tell it where to go, and someone else handles the vehicle, fuel, maintenance, and driving. With a car (traditional servers), you're responsible for everything.

## Serverless vs Traditional

| Aspect | Traditional Servers | Serverless |
|--------|---------------------|------------|
| Server Management | You manage | Provider manages |
| Scaling | Manual or auto-scaling | Automatic |
| Pricing | Per server/time | Per request |
| Cold Starts | N/A | Possible latency |
| State | Persistent | Ephemeral |

## AWS Lambda with Python

```python
# Simple Lambda function handler
import json
from datetime import datetime

def lambda_handler(event: dict, context: object) -> dict:
    """
    AWS Lambda handler function.
    
    Args:
        event: The event dict containing request data
        context: Lambda context object with runtime information
    
    Returns:
        dict with statusCode and body
    """
    return {
        "statusCode": 200,
        "body": json.dumps({
            "message": "Hello from Lambda!",
            "timestamp": datetime.now().isoformat(),
            "request_id": getattr(context, 'request_id', 'unknown')
        })
    }
```

🔍 **Line-by-Line Breakdown:**

1. `import json` — Standard library for JSON serialization.
2. `from datetime import datetime` — For generating timestamps.
3. `def lambda_handler(event: dict, context: object) -> dict:` — Entry point for Lambda. AWS invokes this function.
4. `event: dict` — Contains the triggering event data (API Gateway request, S3 event, etc.).
5. `context: object` — Provides runtime information like request ID, memory limit, etc.
6. `return {"statusCode": 200, "body": json.dumps(...)}` — Standard Lambda response format.

## API Gateway Integration

```python
# Lambda with API Gateway event handling
import json
from dataclasses import dataclass
from typing import Optional

@dataclass
class ApiGatewayEvent:
    """Parsed API Gateway request event."""
    http_method: str
    path: str
    query_params: dict[str, str]
    headers: dict[str, str]
    body: Optional[str]
    
    @classmethod
    def from_lambda_event(cls, event: dict) -> "ApiGatewayEvent":
        return cls(
            http_method=event.get("httpMethod", "GET"),
            path=event.get("path", "/"),
            query_params=event.get("queryStringParameters", {}),
            headers=event.get("headers", {}),
            body=event.get("body")
        )

@dataclass
class ApiGatewayResponse:
    """Response format for API Gateway."""
    status_code: int
    body: str
    headers: dict = None
    
    def to_dict(self) -> dict:
        result = {
            "statusCode": self.status_code,
            "body": self.body
        }
        if self.headers:
            result["headers"] = self.headers
        return result

def lambda_handler(event: dict, context: object) -> dict:
    """Handle API Gateway requests."""
    request = ApiGatewayEvent.from_lambda_event(event)
    
    # Route handling
    match (request.http_method, request.path):
        case ("GET", "/users"):
            return handle_get_users(request).to_dict()
        case ("GET", "/users/" | path):
            user_id = path.strip("/").split("/")[-1]
            return handle_get_user(user_id).to_dict()
        case ("POST", "/users"):
            return handle_create_user(request).to_dict()
        case _:
            return ApiGatewayResponse(
                status_code=404,
                body=json.dumps({"error": "Not found"})
            ).to_dict()

def handle_get_users(request: ApiGatewayEvent) -> ApiGatewayResponse:
    users = [{"id": 1, "name": "Alice"}, {"id": 2, "name": "Bob"}]
    return ApiGatewayResponse(
        status_code=200,
        body=json.dumps(users)
    )

def handle_get_user(user_id: str) -> ApiGatewayResponse:
    return ApiGatewayResponse(
        status_code=200,
        body=json.dumps({"id": user_id, "name": f"User {user_id}"})
    )

def handle_create_user(request: ApiGatewayEvent) -> ApiGatewayResponse:
    if not request.body:
        return ApiGatewayResponse(
            status_code=400,
            body=json.dumps({"error": "Request body required"})
        )
    
    data = json.loads(request.body)
    # In real app, save to database
    return ApiGatewayResponse(
        status_code=201,
        body=json.dumps({"id": 123, **data})
    )
```

🔍 **Line-by-Line Breakdown:**

1. `@dataclass` — Data class for structured event parsing.
2. `class ApiGatewayEvent:` — Represents an API Gateway request.
3. `from_lambda_event(cls, event: dict) -> "ApiGatewayEvent":` — Class method to parse Lambda event.
4. `match (request.http_method, request.path):` — Python 3.10+ pattern matching for routing.
5. `case ("GET", "/users"):` — Match specific method and path combination.
6. `def to_dict(self) -> dict:` — Convert response to Lambda-compatible format.

## Lambda Layers and Dependencies

```python
# Using common dependencies with Lambda Layers
# requirements.txt
# boto3>=1.26.0
# requests>=2.28.0
# pandas>=2.0.0

import boto3
import requests
import pandas as pd
from dataclasses import dataclass

@dataclass
class DynamoDBItem:
    partition_key: str
    sort_key: str
    data: dict

class DynamoDBClient:
    """Simple DynamoDB wrapper using boto3."""
    
    def __init__(self, table_name: str):
        self._table_name = table_name
        self._client = boto3.client('dynamodb')
    
    def get_item(self, partition_key: str, sort_key: str) -> dict | None:
        response = self._client.get_item(
            TableName=self._table_name,
            Key={
                'partition_key': {'S': partition_key},
                'sort_key': {'S': sort_key}
            }
        )
        return response.get('Item')
    
    def put_item(self, item: DynamoDBItem) -> dict:
        return self._client.put_item(
            TableName=self._table_name,
            Item={
                'partition_key': {'S': item.partition_key},
                'sort_key': {'S': item.sort_key},
                'data': {'S': str(item.data)}
            }
        )

def lambda_handler(event: dict, context: object) -> dict:
    """Lambda function using external dependencies."""
    # Initialize client (outside handler for connection reuse)
    db = DynamoDBClient("my-table")
    
    # Process request
    return {"statusCode": 200, "body": "Success!"}
```

## Cold Start Optimization

```python
# Optimizing for cold starts
import json
import os
from functools import lru_cache
from typing import Optional

# Global variable for connection reuse
_db_client: Optional[object] = None

def get_db_client():
    """Reuse database connection across invocations."""
    global _db_client
    if _db_client is None:
        # Initialize on first invocation (cold start)
        from some_db import DatabaseClient
        _db_client = DatabaseClient(
            host=os.environ['DB_HOST'],
            port=os.environ['DB_PORT']
        )
    return _db_client

# Use lru_cache for expensive computations
@lru_cache(maxsize=128)
def get_config(config_key: str) -> dict:
    """Cache configuration values."""
    # In production, this might fetch from Parameter Store
    return {"key": config_key, "value": "cached"}

def lambda_handler(event: dict, context: object) -> dict:
    """Optimized Lambda handler."""
    # Use cached config
    config = get_config("app-settings")
    
    # Use reused DB connection
    db = get_db_client()
    
    return {"statusCode": 200, "body": json.dumps(config)}
```

## Lambda with FastAPI

```python
# FastAPI app running on Lambda via Mangum
# requirements.txt
# fastapi
# mangum
# uvicorn

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List

app = FastAPI()

class Item(BaseModel):
    name: str
    price: float
    quantity: int = 1

items_db: List[dict] = []

@app.get("/items")
async def get_items() -> List[dict]:
    return items_db

@app.post("/items")
async def create_item(item: Item) -> dict:
    new_item = {"id": len(items_db) + 1, **item.model_dump()}
    items_db.append(new_item)
    return new_item

# Handler for AWS Lambda
handler = Mangum(app, enable_http_bytes=True)

# For local development, use uvicorn
# uvicorn main:app --reload
```

## Serverless Framework

```yaml
# serverless.yml - Infrastructure as Code
service: my-python-api

provider:
  name: aws
  runtime: python3.11
  stage: dev
  region: us-east-1
  environment:
    TABLE_NAME: ${self:service}-${self:provider.stage}
  iam:
    role:
      statements:
        - Effect: Allow
          Action:
            - dynamodb:PutItem
            - dynamodb:GetItem
          Resource: "arn:aws:dynamodb:${self:provider.region}:*:table/${self:provider.environment.TABLE_NAME}"

functions:
  api:
    handler: main.handler
    events:
      - http:
          path: /{proxy+}
          method: ANY
      - http:
          path: /
          method: GET

resources:
  Resources:
    TodosTable:
      Type: AWS::DynamoDB::Table
      Properties:
        TableName: ${self:provider.environment.TABLE_NAME}
        BillingMode: PAY_PER_REQUEST
        AttributeDefinitions:
          - AttributeName: id
            AttributeType: N
        KeySchema:
          - AttributeName: id
            KeyType: HASH
```

## Summary

- Serverless lets you focus on code, not infrastructure
- Lambda functions respond to events and scale automatically
- Cold starts can add latency on first invocation
- Use connection pooling and global variables to optimize performance
- FastAPI can run on Lambda using Mangum adapter

## Next Steps

Continue to `09-cqrs-architecture.md` to learn about separating read and write operations.
