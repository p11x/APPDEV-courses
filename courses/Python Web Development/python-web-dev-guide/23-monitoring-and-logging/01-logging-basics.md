# Logging Basics

## What You'll Learn
- Python logging
- Log levels
- Structured logging

## Prerequisites
- Completed search folder

## Basic Logging

```python
import logging
import sys

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
    handlers=[
        logging.StreamHandler(sys.stdout),
        logging.FileHandler('app.log')
    ]
)

logger = logging.getLogger(__name__)

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    logger.info(f"Fetching user {user_id}")
    
    try:
        user = db.query(User).get(user_id)
        if user:
            logger.info(f"User {user_id} found")
            return user
        logger.warning(f"User {user_id} not found")
        return {"error": "Not found"}, 404
    except Exception as e:
        logger.error(f"Error fetching user {user_id}: {e}")
        raise
```

## Log Levels

| Level | Use |
|-------|-----|
| DEBUG | Detailed debugging info |
| INFO | General events |
| WARNING | Unexpected but handled |
| ERROR | Exceptions |
| CRITICAL | System failures |

## Structured Logging

```python
import json
import logging

class JSONFormatter(logging.Formatter):
    def format(self, record):
        log_data = {
            "timestamp": self.formatTime(record),
            "level": record.levelname,
            "message": record.getMessage(),
            "module": record.module
        }
        return json.dumps(log_data)

logging.basicConfig(
    level=logging.INFO,
    handlers=[logging.StreamHandler()]
)
logging.getLogger().handlers[0].setFormatter(JSONFormatter())
```

## Summary
- Use appropriate log levels
- Include context in logs
- Consider structured logging

## Next Steps
→ Continue to `02-centralized-logging.md`
