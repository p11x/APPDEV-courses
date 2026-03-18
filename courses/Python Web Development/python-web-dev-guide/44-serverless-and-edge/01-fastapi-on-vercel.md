# FastAPI on Vercel

## What You'll Learn
- Serverless deployment basics
- Configuring FastAPI for Vercel
- Edge functions
- Environment variables

## Prerequisites
- FastAPI basics
- Vercel account

## Installation

```bash
pip install vercel
```

## Configuration

```python
# vercel.json
{
    "builds": [
        {
            "src": "api/index.py",
            "use": "@vercel/python"
        }
    ],
    "routes": [
        {
            "src": "/(.*)",
            "dest": "/api/index.py"
        }
    ]
}
```

## Creating API

```python
# api/index.py
from fastapi import FastAPI
from fastapi.responses import JSONResponse
from mangum import Mangum

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Hello from Vercel!"}

@app.get("/api/users/{user_id}")
def get_user(user_id: int):
    return {"user_id": user_id, "name": "John"}

# Handler for Vercel
handler = Mangum(app)
```

## Environment Variables

```python
import os

DATABASE_URL = os.environ.get("DATABASE_URL")
API_KEY = os.environ.get("API_KEY")
```

## Summary

- Vercel provides serverless deployment for Python
- Use Mangum to wrap FastAPI for AWS Lambda compatibility
- Configure vercel.json for builds and routes
- Use environment variables for secrets
