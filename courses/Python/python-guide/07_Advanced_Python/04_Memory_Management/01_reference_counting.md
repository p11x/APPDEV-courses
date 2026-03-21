# Reference Counting

## What You'll Learn
- CPython memory model
- del

## Prerequisites
- Read 03_typing_advanced.md first

## Overview
How CPython manages memory.

## Refcount
Check count

```python
import sys
sys.getrefcount(x)
```

## Common Mistakes
- cycle refs
- ignoring

## Summary
- count to 0 = freed
- del decreases
- GC handles cycles

## Next Steps
Continue to **[](./)**
