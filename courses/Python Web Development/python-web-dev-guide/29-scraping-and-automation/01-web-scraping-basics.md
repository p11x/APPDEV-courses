# Web Scraping Basics

## What You'll Learn
- Using requests
- BeautifulSoup
- Ethical scraping

## Prerequisites
- Completed cloud services folder

## Basic Scraping

```python
import requests
from bs4 import BeautifulSoup

response = requests.get("https://example.com")
soup = BeautifulSoup(response.text, "html.parser")

# Find all links
links = soup.find_all("a")
for link in links:
    print(link.get("href"))
```

## Summary
- Check robots.txt first
- Respect rate limits
- Use APIs when available

## Next Steps
→ Continue to `02-beautifulsoup.md`
