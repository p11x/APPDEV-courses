# Async Internals

## What You'll Learn

- How async works
- Event loop
- Coroutines

## Prerequisites

- Completed `03-bytecode.md`

## Event Loop

```python
import asyncio

async def main():
    print("Hello")
    await asyncio.sleep(1)
    print("World")

asyncio.run(main())
```

## Summary

- Event loop manages async tasks
- await pauses execution

## Next Steps

Continue to `05-gil-and-threading.md`.
