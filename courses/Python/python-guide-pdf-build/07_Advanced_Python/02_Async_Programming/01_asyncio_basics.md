# Asyncio Basics

## What You'll Learn

- Understanding asyncio event loop
- Coroutines with async/await
- Running async code with asyncio.run()
- Basic async patterns

## Prerequisites

- Read [03_generator_expressions.md](../01_Generators_and_Iterators/03_generator_expressions.md) first

## Understanding Async

Asyncio enables concurrent async code without threads.

```python
# asyncio_basics.py

import asyncio


async def greet(name: str) -> str:
    """An async function that returns a greeting."""
    await asyncio.sleep(1)  # Simulate async I/O
    return f"Hello, {name}!"


async def main():
    result = await greet("Alice")
    print(result)


# Run the async main
asyncio.run(main())
```

## Multiple Coroutines

```python
# multiple_coroutines.py

import asyncio


async def task(name: str, delay: float) -> str:
    await asyncio.sleep(delay)
    return f"{name} done"


async def main():
    # Run concurrently
    results = await asyncio.gather(
        task("Task 1", 1),
        task("Task 2", 2),
        task("Task 3", 0.5),
    )
    for r in results:
        print(r)


asyncio.run(main())
```

## Annotated Full Example

```python
# asyncio_demo.py
"""Complete demonstration of asyncio basics."""

import asyncio
from typing import List


async def fetch_data(url: str, delay: float) -> dict:
    """Simulate fetching data from URL."""
    await asyncio.sleep(delay)
    return {"url": url, "data": f"Data from {url}"}


async def main() -> None:
    urls = ["api/users", "api/posts", "api/comments"]
    
    # Fetch all URLs concurrently
    tasks = [fetch_data(url, i * 0.5) for i, url in enumerate(urls)]
    results = await asyncio.gather(*tasks)
    
    for result in results:
        print(result)


if __name__ == "__main__":
    asyncio.run(main())
```

## Summary

- Understanding asyncio event loop
- Coroutines with async/await
- Running async code with asyncio.run()

## Next Steps

Continue to **[02_async_await_syntax.md](./02_async_await_syntax.md)**
