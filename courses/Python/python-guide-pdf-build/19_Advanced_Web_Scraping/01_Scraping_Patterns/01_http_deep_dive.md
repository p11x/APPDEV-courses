# HTTP Deep Dive

## What You'll Learn

- HTTP headers and cookies
- TLS fingerprinting
- HTTP/2 understanding
- Session management

## Prerequisites

- Read [08_pipeline_monitoring.md](../02_Data_Pipelines/08_pipeline_monitoring.md) first

## HTTP Headers

```python
# http_headers.py

import requests

# Custom headers
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
    "Accept": "text/html,application/xhtml+xml",
    "Accept-Language": "en-US,en;q=0.9",
    "Referer": "https://example.com",
}

response = requests.get("https://example.com", headers=headers)
print(response.headers)
```

## Session Management

```python
# session_demo.py

import requests

session = requests.Session()

# Set default headers
session.headers.update({
    "User-Agent": "MyBot/1.0",
})

# Cookies persist across requests
session.cookies.set("session_id", "abc123")

# Make requests
response1 = session.get("https://httpbin.org/cookies")
print(response1.json())

response2 = session.get("https://httpbin.org/cookies")
print(response2.json())
```

## Annotated Full Example

```python
# http_demo.py
"""Complete HTTP demonstration."""

import requests


def fetch_with_headers(url: str) -> dict:
    """Fetch URL with custom headers."""
    headers = {
        "User-Agent": "Mozilla/5.0 (compatible; MyBot/1.0)",
        "Accept": "application/json",
    }
    
    response = requests.get(url, headers=headers, timeout=10)
    response.raise_for_status()
    
    return {
        "status": response.status_code,
        "headers": dict(response.headers),
        "content": response.text[:200]
    }


def main() -> None:
    # Check httpbin for headers
    result = fetch_with_headers("https://httpbin.org/get")
    print(f"Status: {result['status']}")
    print(f"User-Agent sent: {result['content'][:100]}")


if __name__ == "__main__":
    main()
```

## Summary

- HTTP headers and cookies
- Session management
- TLS fingerprinting basics

## Next Steps

Continue to **[02_session_and_cookies.md](./02_session_and_cookies.md)**
