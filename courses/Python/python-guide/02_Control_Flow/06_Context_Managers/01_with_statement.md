# With Statement

## What You'll Learn
- Use with for resources
- Guarantee cleanup
- Multiple managers

## Prerequisites
- Read 03_exception_handling.md first

## Overview
with ensures setup/teardown.

## Basic With
Auto cleanup

```python
with open("f.txt") as f:
    data = f.read()
# file auto-closed
```

## Multiple
Stack managers

```python
with open("a") as a, open("b") as b:
    pass
```

## Common Mistakes
- Using after close
- No exception handling

## Summary
- __enter__/__exit__
- Guaranteed cleanup
- Exception safe

## Next Steps
Continue to **[02_builtin_context_managers.md](./02_builtin_context_managers.md)**
