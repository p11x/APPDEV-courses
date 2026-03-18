# Error Tracking

## What You'll Learn
- Sentry integration
- Error capturing
- Release tracking

## Prerequisites
- Completed health checks

## Sentry Setup

```bash
pip install sentry-sdk
```

```python
import sentry_sdk
from fastapi import FastAPI

sentry_sdk.init(
    dsn="https://...",
    traces_sample_rate=0.1
)

app = FastAPI()

@app.get("/error")
async def trigger_error():
    """Trigger an error for testing"""
    1 / 0  # This will be captured by Sentry
```

## FastAPI Integration

```python
from sentry_sdk.integrations.fastapi import FastApiIntegration

sentry_sdk.init(
    dsn="https://...",
    integrations=[FastApiIntegration()],
    environment="production"
)
```

## Summary
- Use Sentry for error tracking
- Capture exceptions automatically
- Track releases

## Next Steps
→ Move to `24-performance/`
