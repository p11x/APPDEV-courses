# Async Await Basics

## What You'll Learn

- asyncio module
- async def and await
- Event loop concept

## Prerequisites

- Read [03_yield_from.md](../01_Generators_and_Iterators/03_yield_from.md) first

## Basic Async

```python
import asyncio

async def fetch_data():
    await asyncio.sleep(1)
    return "data"

async def main():
    result = await fetch_data()
    print(result)

asyncio.run(main())
```

## Summary

- **async def**: Define coroutine
- **await**: Wait for coroutine
- **asyncio.run()**: Run event loop

## Next Steps

Continue to **[02_asyncio_tools.md](./02_asyncio_tools.md)**
