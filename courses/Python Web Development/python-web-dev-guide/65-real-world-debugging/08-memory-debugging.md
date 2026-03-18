# Memory Debugging

## What You'll Learn

- Detecting memory leaks
- Using memory profilers
- Optimization techniques

## Prerequisites

- Completed `07-debugging-api-errors.md`

## Memory Profiling

```python
# Using memory_profiler
from memory_profiler import profile

@profile
def memory_intensive_function():
    data = [i ** 2 for i in range(100000)]
    return sum(data)
```

## Summary

- Use memory profilers
- Check for leaks

## Next Steps

Continue to `09-performance-debugging.md`.
