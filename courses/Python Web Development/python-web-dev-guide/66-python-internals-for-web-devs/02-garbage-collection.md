# Garbage Collection

## What You'll Learn

- How Python manages memory
- Reference counting
- GC cycles

## Prerequisites

- Completed `01-python-interpreters.md`

## Reference Counting

Python uses reference counting for memory management. Each object tracks how many references point to it.

```python
import sys

a = []  # Reference count = 1
b = a    # Reference count = 2
del a    # Reference count = 1
b = None # Reference count = 0, object is freed
```

## Summary

- Python uses reference counting + garbage collection
- Circular references need GC

## Next Steps

Continue to `03-bytecode.md`.
