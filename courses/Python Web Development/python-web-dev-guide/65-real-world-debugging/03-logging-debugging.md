# Logging for Debugging

## What You'll Learn

- Using logging module
- Log levels
- Structured logging

## Prerequisites

- Completed `02-using-debugger.md`

## Basic Logging

```python
import logging

# Configure logging
logging.basicConfig(
    level=logging.DEBUG,
    format="%(asctime)s - %(name)s - %(levelname)s - %(message)s"
)

logger = logging.getLogger(__name__)

def divide(a: int, b: int) -> float:
    logger.debug(f"Dividing {a} by {b}")
    if b == 0:
        logger.error("Division by zero!")
        raise ValueError("Cannot divide by zero")
    result = a / b
    logger.debug(f"Result: {result}")
    return result
```

## Log Levels

| Level | Use |
|-------|-----|
| DEBUG | Detailed info for debugging |
| INFO | Confirmation things work |
| WARNING | Something unexpected |
| ERROR | Serious problem |
| CRITICAL | Very serious error |

## Summary

- Use logging instead of print
- Choose appropriate log levels

## Next Steps

Continue to `04-common-errors.md`.
