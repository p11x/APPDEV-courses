# AWS Lambda

## Overview

AWS Lambda enables serverless FastAPI deployment with automatic scaling and pay-per-use pricing.

## Setup

### Lambda Configuration

```python
# Example 1: FastAPI on Lambda
from fastapi import FastAPI
from mangum import Mangum

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Hello from Lambda!"}

@app.get("/items/{item_id}")
def get_item(item_id: int):
    return {"item_id": item_id}

# Lambda handler
handler = Mangum(app)
```

### SAM Template

```yaml
# Example 2: SAM template
AWSTemplateFormatVersion: '2010-09-09'
Transform: AWS::Serverless-2016-10-31

Globals:
  Function:
    Timeout: 30
    MemorySize: 256

Resources:
  FastAPIFunction:
    Type: AWS::Serverless::Function
    Properties:
      Handler: app.main.handler
      Runtime: python3.11
      CodeUri: .
      Events:
        Api:
          Type: HttpApi
          Path: /{proxy+}
          Method: ANY
```

## Cold Start Optimization

```python
# Example 3: Reduce cold starts
# Use minimal imports
# Initialize connections outside handler
# Use Lambda layers for dependencies
```

## Summary

Lambda provides cost-effective serverless deployment for FastAPI.

## Next Steps

Continue learning about:
- [FastAPI Serverless](./05_fastapi_serverless.md)
- [Serverless Monitoring](./07_serverless_monitoring.md)
