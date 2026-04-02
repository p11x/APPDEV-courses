# Serverless Concepts

## Overview

Serverless computing runs code without managing servers. FastAPI can be deployed serverlessly on AWS Lambda, Azure Functions, and Google Cloud Functions.

## Serverless Benefits

### Key Advantages

```python
# Example 1: Serverless characteristics
"""
Serverless Benefits:

1. No server management
   - No patching, scaling, or provisioning
   - Focus on code, not infrastructure

2. Pay-per-use
   - Only pay for actual execution time
   - No cost when idle

3. Auto-scaling
   - Scales automatically with demand
   - Handle traffic spikes easily

4. High availability
   - Built-in redundancy
   - Managed by cloud provider

Serverless Considerations:

1. Cold starts
   - First request may be slower
   - Can be mitigated with provisioned concurrency

2. Execution limits
   - Timeout limits (15 min on Lambda)
   - Memory constraints

3. Stateless
   - No persistent local state
   - Use external services for state
"""
```

## FastAPI Serverless

### Basic Deployment

```python
# Example 2: FastAPI serverless with Mangum
from fastapi import FastAPI
from mangum import Mangum

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Hello from serverless!"}

@app.get("/items/{item_id}")
def get_item(item_id: int):
    return {"item_id": item_id}

# Lambda handler
handler = Mangum(app)
```

## Platform Comparison

| Platform | Runtime | Cold Start | Max Timeout |
|----------|---------|------------|-------------|
| AWS Lambda | Python 3.11 | 100-500ms | 15 min |
| Azure Functions | Python 3.11 | 200-800ms | 10 min |
| GCP Functions | Python 3.11 | 100-600ms | 9 min |

## Summary

Serverless is ideal for APIs with variable traffic and when you want to minimize infrastructure management.

## Next Steps

Continue learning about:
- [AWS Lambda](./02_aws_lambda.md)
- [FastAPI Serverless](./05_fastapi_serverless.md)
