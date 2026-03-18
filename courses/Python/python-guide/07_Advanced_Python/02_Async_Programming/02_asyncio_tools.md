# asyncio Tools

## What You'll Learn

- asyncio.gather
- asyncio.create_task
- asyncio.sleep
- asyncio.timeout (Python 3.11+)
- asyncio.TaskGroup (Python 3.11+)

## Prerequisites

- Read [01_async_await_basics.md](./01_async_await_basics.md) first

## gather

```python
import asyncio

async def fetch(url: str):
    await asyncio.sleep(1)
    return url

async def main():
    results = await asyncio.gather(
        fetch("url1"),
        fetch("url2"),
        fetch("url3")
    )
    print(results)
```

## TaskGroup (Python 3.11+)

```python
async def main():
    async with asyncio.TaskGroup() as tg:
        task1 = tg.create_task(fetch("url1"))
        task2 = tg.create_task(fetch("url2"))
```

## Summary

- **gather**: Run tasks concurrently
- **create_task**: Schedule task
- **TaskGroup**: Group tasks (3.11+)

## Next Steps

Continue to **[03_async_generators_context_managers.md](./03_async_generators_context_managers.md)**
