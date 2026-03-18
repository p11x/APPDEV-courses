# Bytecode

## What You'll Learn

- Understanding bytecode
- .pyc files
- Dis module

## Prerequisites

- Completed `02-garbage-collection.md`

## Viewing Bytecode

```python
import dis

def add(a: int, b: int) -> int:
    return a + b

dis.dis(add)
```

## Summary

- Python compiles to bytecode
- .pyc files cache bytecode

## Next Steps

Continue to `04-virtual-environments.md`.
