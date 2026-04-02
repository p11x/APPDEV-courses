# 🌊 Fast Async Web Scraping with httpx

## 🎯 What You'll Learn

- Using httpx for async HTTP requests
- Concurrent scraping
- BeautifulSoup for parsing HTML

---

## Installation

```bash
pip install httpx beautifulsoup4 lxml
```

---

## Basic httpx

```python
import httpx

# Synchronous
response = httpx.get("https://example.com")
print(response.text)
print(response.status_code)

# Headers
response = httpx.get(
    "https://api.example.com",
    headers={"Authorization": "Bearer token"}
)
```

---

## Async httpx

```python
import asyncio
import httpx

async def fetch(url: str) -> str:
    async with httpx.AsyncClient() as client:
        response = await client.get(url)
        return response.text

async def main():
    # Single request
    text = await fetch("https://example.com")
    print(text)
    
    # Multiple concurrent requests
    urls = ["https://example.com"] * 10
    results = await asyncio.gather(*[fetch(url) for url in urls])

asyncio.run(main())
```

---

## Parsing HTML with BeautifulSoup

```python
from bs4 import BeautifulSoup

html = """
<html>
    <body>
        <h1>Title</h1>
        <div class="content">
            <p class="item">Item 1</p>
            <p class="item">Item 2</p>
        </div>
    </body>
</html>
"""

soup = BeautifulSoup(html, "lxml")

# Find by tag
soup.find("h1")           # First h1
soup.find_all("p")       # All p tags

# Find by class
soup.find_all(class_="item")  # class is reserved!
soup.select(".item")      # CSS selector

# Get text
soup.find("p").get_text()

# Get attributes
soup.find("a")["href"]
```

---

## Complete Scraper

```python
import asyncio
import httpx
from bs4 import BeautifulSoup

async def scrape_articles():
    async with httpx.AsyncClient() as client:
        # Fetch page
        response = await client.get("https://news.example.com")
        
        # Parse
        soup = BeautifulSoup(response.text, "lxml")
        
        # Extract
        articles = []
        for article in soup.select("article"):
            title = article.select_one("h2").get_text()
            link = article.select_one("a")["href"]
            articles.append({"title": title, "link": link})
        
        return articles

# Run
articles = asyncio.run(scrape_articles())
for a in articles:
    print(f"{a['title']} - {a['link']}")
```

---

## ✅ Summary

- httpx supports both sync and async
- Use async for multiple concurrent requests
- BeautifulSoup parses HTML easily
- Use CSS selectors with .select()

## 🔗 Further Reading

- [httpx Documentation](https://www.python-httpx.org/)
- [BeautifulSoup Docs](https://www.crummy.com/software/BeautifulSoup/)
