# Decorator Anatomy

## What You'll Learn
- How decorators work
- functools.wraps

## Prerequisites
- Read 04_function_design.md first

## Overview
Decorator pattern.

## Basic
Wrapper

```python
@decorator
def f(): pass

# equivalent to:
f = decorator(f)
```

## Common Mistakes
- not returning
- losing metadata

## Summary
- replaces function
- wraps preserves
- nested func

## Next Steps
Continue to **[](./)**
