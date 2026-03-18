# Async Generators and Context Managers

## What You'll Learn

- async for
- async with
- __aiter__, __anext__
- __aenter__, __aexit__

## Prerequisites

- Read [02_asyncio_tools.md](./02_asyncio_tools.md) first

## async for

```python
async def fetch_all():
    urls = ["url1", "url2", "url3"]
    
    async for url in fetch_urls(urls):
        print(url)
```

## async with

```python
async with async_open("file.txt") as f:
    content = await f.read()
```

## Summary

- **async for**: Iterate async iterables
- **async with**: Async context managers

## Next Steps

This concludes Async Programming. Move to **[07_Advanced_Python/03_Typing_Advanced/01_generics.md](../03_Typing_Advanced/01_generics.md)**
