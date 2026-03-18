# Logging and Debugging

## What You'll Learn

- logging module
- Debug levels
- FileHandler
- pdb debugger
- breakpoint()

## Prerequisites

- Read [02_docstrings_and_docs.md](./02_docstrings_and_docs.md) first

## Logging

```python
import logging

logging.basicConfig(level=logging.DEBUG)

logging.debug("Debug info")
logging.info("Info")
logging.warning("Warning")
logging.error("Error")
logging.critical("Critical")
```

## Debugging

```python
import pdb

# Set breakpoint
pdb.set_trace()

# Or use built-in (Python 3.7+)
breakpoint()
```

## Summary

- **logging**: Structured logging
- **pdb**: Interactive debugger

## Next Steps

This concludes Best Practices. Move to **[08_Projects_and_Practices/03_Mini_Projects/01_project_cli_todo.md](../03_Mini_Projects/01_project_cli_todo.md)**
