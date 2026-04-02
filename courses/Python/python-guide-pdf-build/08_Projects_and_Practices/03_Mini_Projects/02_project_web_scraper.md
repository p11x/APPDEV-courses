# Web Scraper

## What You'll Learn

- Basic web scraper
- Using httpx, BeautifulSoup4

## Prerequisites

- Read [01_project_cli_todo.md](./01_project_cli_todo.md) first

## Web Scraper

```python
# scraper.py
import httpx
from bs4 import BeautifulSoup
from dataclasses import dataclass


@dataclass
class Quote:
    text: str
    author: str


def scrape_quotes() -> list[Quote]:
    """Scrape quotes from example site."""
    url = "http://quotes.toscrape.com"
    
    client = httpx.Client()
    response = client.get(url)
    
    soup = BeautifulSoup(response.text, "html.parser")
    
    quotes = []
    for quote in soup.select(".quote"):
        text = quote.select_one(".text").text
        author = quote.select_one(".author").text
        quotes.append(Quote(text, author))
    
    return quotes


def main():
    quotes = scrape_quotes()
    for quote in quotes[:5]:
        print(f'"{quote.text}"')
        print(f"  - {quote.author}\n")


if __name__ == "__main__":
    main()
```

## Summary

- httpx for HTTP requests
- BeautifulSoup for HTML parsing

## Next Steps

Continue to **[03_project_data_analyzer.md](./03_project_data_analyzer.md)**
