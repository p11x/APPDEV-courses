# requests and httpx

## What You'll Learn

- HTTP GET/POST with requests
- Async HTTP with httpx
- Response object attributes
- Error handling

## Prerequisites

- Read [03_pyproject_toml.md](./03_pyproject_toml.md) first

## requests

```python
import requests

# GET request
response = requests.get("https://api.example.com/data")
print(response.status_code)
print(response.json())

# POST request
response = requests.post(
    "https://api.example.com/data",
    json={"key": "value"}
)
```

## httpx (Async)

```python
import httpx

# Synchronous
response = httpx.get("https://api.example.com/data")

# Asynchronous
async def fetch():
    async with httpx.AsyncClient() as client:
        response = await client.get("https://api.example.com/data")
```

## Summary

- **requests**: Simple synchronous HTTP
- **httpx**: Async-capable HTTP client

## Next Steps

Continue to **[02_pathlib_deep_dive.md](./02_pathlib_deep_dive.md)**
