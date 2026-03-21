# Writing Managers

## What You'll Learn
- __enter__, __exit__

## Prerequisites
- Read 02_builtin_context_managers.md first

## Overview
Custom context.

## Protocol
Methods

```python
class CM:
  def __enter__(self): return self
  def __exit__(self,*a): pass
```

## Common Mistakes
- not returning
- not handling

## Summary
- enter returns
- exit cleans
- exception info

## Next Steps
Continue to **[](./)**
