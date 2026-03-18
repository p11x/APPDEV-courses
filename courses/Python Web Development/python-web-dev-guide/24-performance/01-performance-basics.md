# Performance Basics

## What You'll Learn
- Measuring performance
- Profiling
- Common bottlenecks

## Prerequisites
- Completed monitoring folder

## Measuring Performance

```python
import time
from fastapi import FastAPI

app = FastAPI()

@app.middleware("http")
async def add_timing(request, call_next):
    start = time.time()
    response = await call_next(request)
    duration = time.time() - start
    response.headers["X-Process-Time"] = str(duration)
    return response

@app.get("/slow")
async def slow_endpoint():
    time.sleep(2)  # Simulate slow operation
    return {"message": "done"}
```

## Profiling

```bash
pip install py-spy

# Profile a running process
py-spy top -- python app.py
```

## Common Bottlenecks

- Database queries (N+1 problem)
- Synchronous operations
- Large payloads
- Missing indexes

## Summary
- Measure first, optimize second
- Profile to find bottlenecks
- Focus on biggest wins

## Next Steps
→ Continue to `02-database-optimization.md`
