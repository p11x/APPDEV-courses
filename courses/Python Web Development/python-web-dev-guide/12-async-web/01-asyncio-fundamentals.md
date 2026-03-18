# Asyncio Fundamentals

## What You'll Learn
- Understanding asyncio
- async/await syntax
- Event loops and coroutines

## Prerequisites
- Completed Django folder (11)

## What Is Asyncio?

asyncio is Python's built-in library for writing concurrent code using the async/await syntax.

## Basic Example

```python
import asyncio

async def fetch_data():
    print("Starting fetch...")
    await asyncio.sleep(2)  # Simulate I/O operation
    print("Data fetched!")
    return {"data": "example"}

async def main():
    result = await fetch_data()
    print(result)

asyncio.run(main())
```

## Summary
- asyncio enables concurrent code
- async/await are keywords for defining and calling async functions
- Use asyncio.run() to execute the main coroutine
