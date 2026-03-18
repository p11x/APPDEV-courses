# Rate Limiting and Ethics

## What You'll Learn
- Respectful scraping
- Rate limiting
- Legal considerations

## Prerequisites
- Completed automation frameworks

## Rate Limiting

```python
import time

def rate_limited(func):
    def wrapper(*args, **kwargs):
        time.sleep(1)  # 1 request per second
        return func(*args, **kwargs)
    return wrapper

@rate_limited
def scrape_page(url):
    # Your scraping code
    pass
```

## Ethics

- Check robots.txt
- Don't overload servers
- Identify your bot
- Respect terms of service

## Summary
- Be respectful
- Use rate limiting
- Check legality

## Next Steps
→ Move to `30-ai-integration/`
