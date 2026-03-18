# Centralized Logging

## What You'll Learn
- Using logtail
- Using ELK stack
- Cloud logging

## Prerequisites
- Completed logging basics

## Python-json-logger

```bash
pip install python-json-logger
```

```python
import logging
from pythonjsonlogger import jsonlogger

logger = logging.getLogger()
handler = logging.StreamHandler()
formatter = jsonlogger.JsonFormatter(
    fmt='%(asctime)s %(name)s %(levelname)s %(message)s'
)
handler.setFormatter(formatter)
logger.addHandler(handler)
logger.setLevel(logging.INFO)
```

## Log Services

### Logtail

```bash
pip install logtail
```

```python
from logtail import LogtailHandler

handler = LogtailHandler(source_token="...")
logger = logging.getLogger()
logger.addHandler(handler)
logger.info("Application started")
```

### CloudWatch

```bash
pip install watchtower
```

```python
import watchtower

logger = logging.getLogger()
logger.addHandler(watchtower.CloudWatchLogHandler())
```

## Summary
- Use centralized logging in production
- Choose a logging service
- Include request IDs for tracing

## Next Steps
→ Continue to `03-prometheus-metrics.md`
