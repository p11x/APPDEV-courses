# AWS Services

## What You'll Learn
- Core AWS services
- Lambda
- S3, RDS

## Prerequisites
- Completed cloud providers

## Common Services

| Service | Use |
|---------|-----|
| EC2 | Virtual servers |
| Lambda | Serverless |
| S3 | Storage |
| RDS | Database |
| CloudFront | CDN |

## Lambda with FastAPI

```bash
pip install mangum
```

```python
from fastapi import FastAPI
from mangum import Mangum

app = FastAPI()

@app.get("/")
def hello():
    return {"message": "Hello from Lambda"}

handler = Mangum(app)
```

## Summary
- AWS has most comprehensive services
- Lambda for serverless
- Use boto3 for SDK

## Next Steps
→ Continue to `03-serverless-frameworks.md`
