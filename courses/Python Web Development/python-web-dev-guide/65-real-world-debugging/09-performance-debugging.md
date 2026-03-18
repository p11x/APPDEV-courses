# Performance Debugging

## What You'll Learn

- Profiling code
- Finding bottlenecks
- Performance optimization

## Prerequisites

- Completed `08-memory-debugging.md`

## Profiling

```python
# Using cProfile
import cProfile
import pstats

profiler = cProfile.Profile()
profiler.enable()

# Your code here
result = sum(range(1000000))

profiler.disable()
stats = pstats.Stats(profiler)
stats.sort_stats('cumulative').print_stats(10)
```

## Summary

- Profile to find bottlenecks
- Optimize critical paths

## Next Steps

Continue to `10-debugging-checklist.md`.
