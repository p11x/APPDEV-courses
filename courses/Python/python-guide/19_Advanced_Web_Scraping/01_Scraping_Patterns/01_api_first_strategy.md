# 🕸️ API-First Strategy

> Before you scrape, check if the website has an API. Scraping is a last resort — APIs are faster, more stable, and legal.

## 🎯 What You'll Learn

- How to discover hidden APIs that websites use
- What free public APIs exist for common scraping targets
- When scraping is actually appropriate vs when to use an API
- How to read robots.txt and Terms of Service
- Rate limiting etiquette and legal considerations

## 📦 Prerequisites

- Completion of [16_Automation_and_Scripting/02_httpx_scraping.md](../../16_Automation_and_Scripting/02_httpx_scraping.md) or equivalent knowledge of httpx
- Understanding of HTTP methods (GET, POST)
- Basic JSON handling knowledge

---

## The API-First Mental Model

When you need data from a website, always follow this decision tree:

```
                    ┌─────────────────────┐
                    │ Need data from site │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │  Does site have an  │
                    │        API?          │
                    └──────────┬──────────┘
                               │
              ┌────────────────┴────────────────┐
              ▼                                 ▼
        ┌─────────────┐                 ┌─────────────┐
        │    YES      │                 │     NO      │
        └──────┬──────┘                 └──────┬──────┘
               │                                │
               ▼                                ▼
        ┌─────────────┐                 ┌─────────────┐
        │  Use API!   │                 │  Check for  │
        │  ✓ Faster   │                 │  hidden APIs │
        │  ✓ Stable   │                 └──────┬──────┘
        │  ✓ Legal    │                        │
        └─────────────┘                        ▼
                                       ┌─────────────┐
                                       │ Found hidden│
                                       │    API?     │
                                       └──────┬──────┘
                                              │
                              ┌───────────────┴───────────────┐
                              ▼                               ▼
                        ┌─────────────┐                 ┌─────────────┐
                        │    YES      │                 │     NO      │
                        └──────┬──────┘                 └──────┬──────┘
                               │                                │
                               ▼                                ▼
                        ┌─────────────┐                 ┌─────────────┐
                        │ Use hidden  │                 │  Consider   │
                        │    API      │                 │  scraping   │
                        └─────────────┘                 │  as last   │
                                                      │   resort   │
                                                      └─────────────┘
```

---

## Discovering Hidden APIs

Websites often use internal APIs to power their frontends. Here's how to find them:

### Method 1: Browser DevTools Network Tab

1. Open the website in Chrome/Firefox
2. Press `F12` to open Developer Tools
3. Click the **Network** tab
4. Filter by **XHR** or **Fetch** requests
5. Look for endpoints containing: `/api/`, `/v1/`, `/graphql/`, `/data/`
6. Click each request to examine the response

### What to Look For

```bash
# Common API endpoint patterns
/api/v1/users          # User data
/api/v1/products      # Product listings  
/api/graphql          # GraphQL queries
/api/search           # Search results
/api/feed             # Content feed
/data.json            # Raw data files
```

### Method 2: Inspect Mobile Apps

Many websites have mobile APIs that are easier to access:

- Use **mitmproxy** to intercept traffic from a mobile app
- Mobile APIs often have less protection than web interfaces
- This is more advanced — start with Method 1 first

### Example: Finding a Hidden API

Let's say you want to scrape news articles from a news site:

```python
import httpx  # HTTP client for making requests

# When you look at Network tab, you might find:
# https://news-site.com/api/v1/articles?page=1&limit=20

response = httpx.get(
    "https://news-site.com/api/v1/articles",
    params={"page": 1, "limit": 20},  # Query parameters
    headers={"Accept": "application/json"}  # Request JSON response
)

# The API returns clean, structured data - no parsing needed!
articles = response.json()  # Returns a Python dict/list directly

# ✅ Much easier than scraping HTML with BeautifulSoup
for article in articles["data"]:
    print(article["title"])  # Direct access to fields
```

### 💡 Explanation

- `params={"page": 1, "limit": 20}` — adds `?page=1&limit=20` to the URL
- `headers={"Accept": "application/json"}` — tells server we want JSON
- `response.json()` — automatically parses JSON response to Python dict
- No BeautifulSoup needed — data comes pre-structured!

---

## Public APIs That Replace Common Scraping

Many common scraping targets have free public APIs:

### News APIs

| API | Free Tier | URL |
|-----|-----------|-----|
| NewsAPI | 100 requests/day | newsapi.org |
| The Guardian | 500 requests/day | open-platform.theguardian.com |
| New York Times | 5,000 requests/day | developer.nytimes.com |
| Hacker News | Unlimited | hn.algolia.com/api/v1 |

### Finance APIs

| API | Free Tier | URL |
|-----|-----------|-----|
| Yahoo Finance (yfinance) | Unlimited | pypi.org/project/yfinance |
| Alpha Vantage | 25 requests/day | alphavantage.co |
| Open-Meteo | Unlimited | open-meteo.com |

### Social Media APIs

| Platform | API | Notes |
|----------|-----|-------|
| Reddit | PRAW | Python Reddit API Wrapper |
| Stack Overflow | Stack Exchange API | stackprinter.appspot.com |
| GitHub | GitHub REST API | api.github.com |

### Weather APIs

| API | Free Tier | URL |
|-----|-----------|-----|
| OpenWeatherMap | 1000 calls/day | openweathermap.org |
| Open-Meteo | Unlimited | open-meteo.com |

### Books APIs

| API | Description | URL |
|-----|-------------|-----|
| Google Books API | Search and retrieve book data | books.google.com |
| Open Library API | Free, open book metadata | openlibrary.org |

---

## Example: Using the Hacker News API

The Hacker News API is completely free and unlimited:

```python
import httpx  # HTTP client
import asyncio  # For async requests


async def fetch_top_stories() -> list[dict]:
    """Fetch top 10 stories from Hacker News API."""
    
    async with httpx.AsyncClient() as client:
        # Step 1: Get IDs of top stories
        response = await client.get(
            "https://hacker-news.firebaseio.com/v0/topstories.json"
        )
        story_ids: list[int] = response.json()  # List of story IDs
        
        # Step 2: Fetch details for first 10 stories
        tasks = []  # List of async tasks
        for story_id in story_ids[:10]:
            task = client.get(
                f"https://hacker-news.firebaseio.com/v0/item/{story_id}.json"
            )
            tasks.append(task)
        
        # Step 3: Execute all requests concurrently
        responses = await asyncio.gather(*tasks)
        
        # Step 4: Parse each response
        stories = []
        for response in responses:
            story = response.json()
            if story:  # Some items might be None
                stories.append(story)
        
        return stories


async def main() -> None:
    """Main entry point."""
    stories = await fetch_top_stories()
    
    print("📰 Top Hacker News Stories")
    print("=" * 50)
    for i, story in enumerate(stories, 1):
        print(f"{i}. {story['title']}")
        print(f"   🔗 {story.get('url', 'No URL')}")
        print(f"   ⬆️  {story['score']} points | 💬 {story['descendants']} comments")
        print()


# Run the async function
asyncio.run(main())
```

### 💡 Explanation

- `httpx.AsyncClient()` — creates an async HTTP client for concurrent requests
- `asyncio.gather(*tasks)` — runs all HTTP requests in parallel (much faster than sequential!)
- `response.json()` — parses JSON directly to Python dict
- No rate limiting needed — HN API is free and unlimited

### Output

```
📰 Top Hacker News Stories
==================================================
1. Show HN: A new programming language
   🔗 https://example.com
   ⬆️  500 points | 💬 200 comments

2. PostgreSQL 16 Released
   🔗 https://postgresql.org
   ⬆️  450 points | 💬 150 comments
...
```

---

## When Scraping IS Appropriate

Sometimes you genuinely need to scrape instead of using an API:

### Legitimate Reasons to Scrape

1. **No public API exists** — the site doesn't offer any data access
2. **API costs too much** — free tier is insufficient for your hobby project
3. **Data is publicly visible** — no login required, no paywall
4. **API is rate-limited too strictly** — need more data than allowed
5. **Research purposes** — academic research on publicly available data

### What Makes Scraping Ethical

- ✅ Scraping publicly accessible data
- ✅ Respecting robots.txt
- ✅ Rate limiting your requests (1-2 per second)
- ✅ Not circumventing authentication or paywalls
- ✅ Not distributing copyrighted content
- ✅ Not overwhelming the server

### What Makes Scraping Unethical

- ❌ Ignoring robots.txt directives
- ❌ Scraping behind login walls without permission
- ❌ Bypassing paywalls
- ❌ Scraping personal data (PII)
- ❌ DDoS-style requests (hundreds per second)
- ❌ Commercial use of scraped copyrighted content

---

## Reading robots.txt

Every website should have a `robots.txt` file at the root:

```bash
# Example: example.com/robots.txt
User-agent: *                  # Apply to all crawlers
Disallow: /admin/               # Don't crawl admin pages
Disallow: /private/             # Don't crawl private pages
Allow: /public/                 # But do crawl public pages
Crawl-delay: 1                  # Wait 1 second between requests

User-agent: Googlebot           # Specific rules for Google
Disallow: /api/                # Don't crawl API endpoints
```

### Reading robots.txt in Python

```python
from urllib.robotparser import RobotFileParser


def can_scrape_url(url: str, user_agent: str = "*") -> bool:
    """Check if we're allowed to scrape a URL based on robots.txt."""
    
    # Extract base URL for robots.txt
    from urllib.parse import urlparse
    base_url = f"{urlparse(url).scheme}://{urlparse(url).netloc}"
    
    # Create parser and fetch robots.txt
    parser = RobotFileParser()
    parser.set_url(f"{base_url}/robots.txt")
    
    try:
        parser.read()  # Fetch and parse robots.txt
    except Exception:
        # If robots.txt doesn't exist or can't be read, allow by default
        return True
    
    # Check if the specific URL is allowed
    return parser.can_fetch(user_agent, url)


# Example usage
if can_scrape_url("https://example.com/products"):
    print("✅ Allowed to scrape this page")
else:
    print("❌ Not allowed - check robots.txt")
```

### 💡 Explanation

- `RobotFileParser` — built-in Python class for parsing robots.txt
- `parser.can_fetch()` — checks if a specific URL is allowed for a user agent
- Returns `True` if no robots.txt exists (conservative approach)

---

## Rate Limiting Etiquette

Always rate limit your scrapers:

```python
import random  # For random delays
import time   # For sleep function


def polite_delay() -> None:
    """Wait a random amount of time between requests."""
    
    # Wait between 1-3 seconds - polite to the server
    delay = random.uniform(1.0, 3.0)
    time.sleep(delay)


# Example: Making polite requests
for page in range(1, 11):
    # Make request
    response = httpx.get(f"https://example.com/items?page={page}")
    
    # Process data...
    
    # Always delay between requests
    polite_delay()
    
    print(f"Fetched page {page}")
```

### 💡 Explanation

- `random.uniform(1.0, 3.0)` — generates random float between 1 and 3 seconds
- `time.sleep(delay)` — pauses execution for that many seconds
- 1-2 requests per second is the recommended maximum for polite scraping

---

## Legal Grey Areas

### Public Data vs Copyrighted Content

| Scenario | Is it Legal? |
|----------|--------------|
| Scraping public product prices for comparison | ✅ Generally OK |
| Aggregating news headlines with attribution | ✅ Fair use |
| Reproducing copyrighted articles in full | ❌ Copyright violation |
| Scraping personal data (emails, names) | ❌ Privacy laws (GDPR, CCPA) |
| Commercial use of scraped data | ⚠️ Depends on context |

### Best Practices

1. **When in doubt, don't scrape** — use official APIs
2. **Check Terms of Service** — scraping might violate them
3. **Attribute sources** — always credit the original website
4. **Don't compete commercially** — scraping for personal projects is safer
5. **Consult a lawyer** — for anything beyond personal learning

---

## Summary

✅ **Always check for APIs first** — they're faster, stable, and legal

✅ **Use public APIs** — NewsAPI, Yahoo Finance, Hacker News, Open-Meteo

✅ **Discover hidden APIs** — check DevTools Network tab for /api/ endpoints

✅ **Respect robots.txt** — use urllib.robotparser

✅ **Rate limit requests** — 1-2 requests per second maximum

✅ **Scrape ethically** — only public data, with permission, for legitimate purposes

---

## ➡️ Next Steps

Continue to [02_anti_detection_techniques.md](./02_anti_detection_techniques.md) to learn how websites detect scrapers and how to be a polite, undetected scraper.

---

## 🔗 Further Reading

- [Hacker News API Documentation](https://github.com/HackerNews/API)
- [NewsAPI Documentation](https://newsapi.org/docs)
- [Robots.txt Specification](https://www.robotstxt.org/robotstxt.html)
- [Python httpx Documentation](https://www.python-httpx.org/)
