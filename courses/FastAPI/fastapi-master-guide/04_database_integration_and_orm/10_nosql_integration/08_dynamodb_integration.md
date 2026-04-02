# DynamoDB Integration

## Overview

DynamoDB is AWS's fully managed NoSQL database, ideal for serverless FastAPI applications.

## Setup

### DynamoDB Connection

```python
# Example 1: DynamoDB with boto3
import boto3
from boto3.dynamodb.conditions import Key
from fastapi import FastAPI

app = FastAPI()

# DynamoDB resource
dynamodb = boto3.resource('dynamodb', region_name='us-east-1')
users_table = dynamodb.Table('users')
```

## CRUD Operations

### DynamoDB Operations

```python
# Example 2: CRUD operations
from pydantic import BaseModel

class UserCreate(BaseModel):
    username: str
    email: str

@app.post("/users/")
async def create_user(user: UserCreate):
    """Create user in DynamoDB"""
    users_table.put_item(Item={
        'username': user.username,
        'email': user.email,
        'created_at': datetime.utcnow().isoformat()
    })
    return user

@app.get("/users/{username}")
async def get_user(username: str):
    """Get user by username"""
    response = users_table.get_item(Key={'username': username})
    if 'Item' not in response:
        raise HTTPException(404, "User not found")
    return response['Item']

@app.get("/users/")
async def list_users():
    """Scan all users"""
    response = users_table.scan()
    return response['Items']

@app.delete("/users/{username}")
async def delete_user(username: str):
    """Delete user"""
    users_table.delete_item(Key={'username': username})
    return {"deleted": username}
```

### Query Operations

```python
# Example 3: Query with conditions
@app.get("/users/search/")
async def search_users(email_domain: str):
    """Query users by email domain"""
    response = users_table.query(
        IndexName='email-index',
        KeyConditionExpression=Key('email').begins_with(email_domain)
    )
    return response['Items']
```

## Summary

DynamoDB provides scalable NoSQL storage for serverless applications.

## Next Steps

Continue learning about:
- [MongoDB Integration](../01_mongodb_integration.md)
- [Redis Integration](../02_redis_integration.md)
