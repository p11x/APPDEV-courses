# yield from

## What You'll Learn

- yield from for delegation
- Refactoring nested generators

## Prerequisites

- Read [02_generators.md](./02_generators.md) first

## yield from

```python
def chain(*iterables):
    for it in iterables:
        yield from it

# Now works!
result = list(chain([1, 2], [3, 4], [5]))
# [1, 2, 3, 4, 5]
```

## Summary

- **yield from**: Delegates to sub-generator
- Simplifies nested generators

## Next Steps

This concludes Generators. Move to **[07_Advanced_Python/02_Async_Programming/01_async_await_basics.md](../02_Async_Programming/01_async_await_basics.md)**
