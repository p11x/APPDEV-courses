# Health Checks

## What You'll Learn
- Basic health endpoint
- Dependency checks
- Liveness vs readiness

## Prerequisites
- Completed Prometheus metrics

## Basic Health Check

```python
from fastapi import FastAPI

app = FastAPI()

@app.get("/health")
async def health_check():
    """Basic health check"""
    return {"status": "healthy"}
```

## Detailed Health Check

```python
from fastapi import FastAPI
import httpx

app = FastAPI()

async def check_database() -> bool:
    """Check database connection"""
    try:
        # Simple query
        return True
    except:
        return False

async def check_external_api() -> bool:
    """Check external service"""
    try:
        async with httpx.AsyncClient() as client:
            r = await client.get("https://api.example.com/health")
            return r.status_code == 200
    except:
        return False

@app.get("/health")
async def health_check():
    """Detailed health check"""
    db_healthy = await check_database()
    api_healthy = await check_external_api()
    
    status = "healthy" if db_healthy and api_healthy else "unhealthy"
    
    return {
        "status": status,
        "checks": {
            "database": "up" if db_healthy else "down",
            "external_api": "up" if api_healthy else "down"
        }
    }

@app.get("/ready")
async def readiness_check():
    """Readiness check for Kubernetes"""
    if await check_database():
        return {"ready": True}
    return {"ready": False}, 503
```

## Summary
- Use /health for basic checks
- Use /ready for Kubernetes
- Check dependencies

## Next Steps
→ Continue to `05-error-tracking.md`
