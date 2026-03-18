# 🕵️ Anti-Detection Techniques

> How websites detect bots — and how to be a polite, undetected scraper.

## 🎯 What You'll Learn

- Why websites block scrapers
- What detection signals websites use
- How to set up polite scraper headers
- Using sessions and rotating user agents
- Respecting robots.txt programmatically
- Ethics: what NOT to do

## 📦 Prerequisites

- Completion of [01_api_first_strategy.md](./01_api_first_strategy.md)
- Basic understanding of HTTP headers
- Knowledge of httpx or requests library

---

## Why Websites Block Scrapers

Websites block scrapers for several legitimate reasons:

| Reason | Explanation |
|--------|-------------|
| **Server Load** | Too many requests can crash or slow down the site |
| **Terms of Service** | Scraping may violate the site's ToS |
| **Data Protection** | Protect user data and business intelligence |
| **Revenue Protection** | Content is their product — they want you to use their API |
| **Security** | Prevent abuse, spam, and malicious activity |

### The Goal

We're not trying to bypass security — we're trying to be a **polite visitor** that looks like a normal browser. This means:

- ✅ Looking like a real user
- ✅ Respecting the server's resources
- ✅ Following the rules (robots.txt, ToS)

---

## Detection Signals Websites Use

Websites can detect bots using these signals:

### 1. No User-Agent Header

```python
# ❌ BAD: No User-Agent looks like a bot
response = httpx.get("https://example.com")

# ✅ GOOD: Use a real browser User-Agent
response = httpx.get(
    "https://example.com",
    headers={"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"}
)
```

### 💡 Explanation

- The `User-Agent` header tells the server what browser/device you're using
- Without it, servers know immediately you're not a normal browser

### 2. Requests Too Fast

```python
# ❌ BAD: 100 requests per second will get you blocked instantly
for i in range(100):
    httpx.get(url)
    # No delay!

# ✅ GOOD: Add random delays between requests
import random
import time

for i in range(100):
    httpx.get(url)
    time.sleep(random.uniform(1.0, 3.0))  # Wait 1-3 seconds
```

### 💡 Explanation

- Normal users can't click 100 times per second
- Servers track request frequency and flag anything suspicious

### 3. No Cookies or Session State

```python
# ❌ BAD: Every request appears to be a new visitor
for i in range(10):
    response = httpx.get("https://example.com/page")
    # No cookies maintained!

# ✅ GOOD: Use a session to maintain cookies
with httpx.Client() as session:
    # Session automatically maintains cookies
    for i in range(10):
        response = session.get("https://example.com/page")
```

### 💡 Explanation

- Real users have cookies from previous visits
- A new session every request looks suspicious

### 4. No Referrer Header

```python
# ❌ BAD: Direct access with no referrer
response = httpx.get("https://example.com/product")

# ✅ GOOD: Pretend to come from another page
response = httpx.get(
    "https://example.com/product",
    headers={"Referer": "https://www.google.com/"}
)
```

### 💡 Explanation

- The `Referer` header shows which page you came from
- Real users usually come from Google, another page, or a direct bookmark

### 5. Identical Request Patterns

```python
# ❌ BAD: Exactly the same requests in exactly the same order
for page in range(1, 101):
    response = httpx.get(f"https://example.com?page={page}")

# ✅ GOOD: Add some variation
import random

for page in range(1, 101):
    # Add random query parameter
    params = {"page": page, "sort": random.choice(["price", "name", "rating"])}
    response = httpx.get("https://example.com", params=params)
    time.sleep(random.uniform(1.0, 3.0))
```

### 💡 Explanation

- Bots make identical requests; humans add randomness
- Vary query parameters, timing, and order when possible

### 6. Datacenter IP Ranges

```python
# ❌ BAD: Using a known cloud provider IP (AWS, GCP, DigitalOcean)
# These are immediately flagged as bots

# ✅ GOOD: Use a residential IP or VPN
# Or use a scraping service like ScraperAPI, Bright Data
```

### 💡 Explanation

- Datacenter IPs (AWS, GCP, DigitalOcean) are known to be servers, not users
- For advanced use cases, consider residential proxies (expensive)

---

## Polite Scraper Headers Setup

Here's a complete, production-ready header setup:

```python
import httpx


def create_polite_headers() -> dict[str, str]:
    """Create headers that look like a real browser."""
    
    return {
        # User-Agent: Identifies the browser
        "User-Agent": (
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
            "AppleWebKit/537.36 (KHTML, like Gecko) "
            "Chrome/120.0.0.0 Safari/537.36"
        ),
        # Accept: What content types we can handle
        "Accept": (
            "text/html,application/xhtml+xml,application/xml;"
            "q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8"
        ),
        # Accept-Language: What languages we prefer
        "Accept-Language": "en-US,en;q=0.9",
        # Accept-Encoding: What compression we support
        "Accept-Encoding": "gzip, deflate, br",
        # Referer: Where we came from (usually Google for first visit)
        "Referer": "https://www.google.com/",
        # Connection: Keep connection alive
        "Connection": "keep-alive",
        # Upgrade-Insecure-Requests: Prefer secure connections
        "Upgrade-Insecure-Requests": "1",
        # Sec-Fetch-* : Security headers that browsers send
        "Sec-Fetch-Dest": "document",
        "Sec-Fetch-Mode": "navigate",
        "Sec-Fetch-Site": "none",
        "Sec-Fetch-User": "?1",
    }


def polite_get(url: str, session: httpx.Client | None = None) -> httpx.Response:
    """Make a polite HTTP GET request."""
    
    headers = create_polite_headers()
    
    if session:
        # Use existing session (maintains cookies)
        return session.get(url, headers=headers)
    else:
        # Create new session for single request
        with httpx.Client() as client:
            return client.get(url, headers=headers)


# Example usage
with httpx.Client() as session:
    # First request - looks like a new visitor from Google
    response = polite_get("https://example.com", session)
    
    # Subsequent requests - session maintains cookies
    response = polite_get("https://example.com/about", session)
```

### 💡 Explanation

- `create_polite_headers()` — builds a complete set of browser-like headers
- `httpx.Client()` — creates a session that maintains cookies automatically
- All headers together make the request look like a real Chrome browser on Windows

---

## Random Delays

Always add random delays between requests:

```python
import random
import time


def polite_delay() -> None:
    """Wait a random amount of time between requests."""
    
    # Random delay between 1-3 seconds
    delay = random.uniform(1.0, 3.0)
    time.sleep(delay)


def scrape_multiple_pages(urls: list[str]) -> list[httpx.Response]:
    """Scrape multiple pages with polite delays."""
    
    responses = []
    
    with httpx.Client() as session:
        for i, url in enumerate(urls):
            print(f"Fetching {url} ({i+1}/{len(urls)})")
            
            response = session.get(url, headers=create_polite_headers())
            responses.append(response)
            
            # Always delay between requests
            if i < len(urls) - 1:  # Don't delay after last request
                polite_delay()
    
    return responses
```

### 💡 Explanation

- `random.uniform(1.0, 3.0)` — generates random float between 1 and 3 seconds
- `time.sleep(delay)` — pauses execution
- Adding randomness makes it harder to detect as a bot

---

## Session Persistence

Using a session maintains cookies across requests:

```python
import httpx


def create_session() -> httpx.Client:
    """Create a polite HTTP session."""
    
    headers = {
        "User-Agent": (
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
            "AppleWebKit/537.36 (KHTML, like Gecko) "
            "Chrome/120.0.0.0 Safari/537.36"
        ),
        "Accept-Language": "en-US,en;q=0.9",
    }
    
    # Create client with default headers
    # This client will maintain cookies automatically
    return httpx.Client(
        headers=headers,
        timeout=30.0,  # 30 second timeout
        follow_redirects=True,  # Follow redirects
    )


# Usage
with create_session() as session:
    # Visit homepage first (sets cookies)
    session.get("https://example.com")
    
    # Now visit another page (cookies sent automatically)
    response = session.get("https://example.com/products")
    
    # Session is maintained - looks like a real user browsing
    print(f"Cookies: {session.cookies}")
```

### 💡 Explanation

- `httpx.Client()` — session object that persists across requests
- Cookies are automatically maintained — looks like real user behavior
- `follow_redirects=True` — handles redirects like a real browser

---

## Rotating User-Agents

For larger scrapers, rotate user-agents:

```python
import random
from typing import Callable


USER_AGENTS = [
    # Chrome on Windows
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
    # Chrome on macOS
    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
    # Firefox on Windows
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:121.0) Gecko/20100101 Firefox/121.0",
    # Firefox on macOS
    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/20100101 (KHTML, like Gecko) Firefox/121.0",
    # Safari on macOS
    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.2 Safari/605.1.15",
]


def random_user_agent() -> str:
    """Get a random user agent."""
    return random.choice(USER_AGENTS)


def create_rotating_session() -> httpx.Client:
    """Create a session with rotating user agents."""
    
    return httpx.Client(
        headers={
            "User-Agent": random_user_agent(),  # Different each time
            "Accept-Language": "en-US,en;q=0.9",
        },
        timeout=30.0,
    )


# OR: use fake-useragent library
# pip install fake-useragent

from fake_useragent import UserAgent

ua = UserAgent()
random_user = ua.random  # Gets a random user agent dynamically
print(f"Random User-Agent: {random_user}")
```

### 💡 Explanation

- `USER_AGENTS` list — predefined list of common browser user-agents
- `random.choice()` — picks one at random
- `fake-useragent` library — fetches real current user-agents (more realistic)

---

## Playwright Stealth

For JavaScript-rendered sites, use playwright-stealth:

```bash
pip install playwright playwright-stealth
playwright install chromium
```

```python
from playwright.sync_api import sync_playwright
from playwright_stealth import stealth_sync


def scrape_with_stealth(url: str) -> str:
    """Scrape a page using stealth Playwright."""
    
    with sync_playwright() as p:
        # Launch browser in non-headless mode (or True for headless)
        browser = p.chromium.launch(headless=True)
        
        # Create new page
        page = browser.new_page()
        
        # Apply stealth patches - evades bot detection
        stealth_sync(page)
        
        # Navigate to URL
        page.goto(url)
        
        # Wait for content to load
        page.wait_for_load_state("networkidle")
        
        # Get page content
        content = page.content()
        
        browser.close()
        
        return content


# Usage
html = scrape_with_stealth("https://example.com")
print(html[:500])  # First 500 characters
```

### 💡 Explanation

- `playwright-stealth` — patches Playwright to look like a real browser
- `stealth_sync(page)` — applies all anti-detection patches
- Handles JavaScript rendering and bot detection evasion

---

## Respecting robots.txt Programmatically

Always check robots.txt before scraping:

```python
from urllib.robotparser import RobotFileParser
from urllib.parse import urlparse


def can_fetch(url: str, user_agent: str = "*") -> bool:
    """Check if a URL can be scraped based on robots.txt."""
    
    # Parse the base URL
    parsed = urlparse(url)
    base_url = f"{parsed.scheme}://{parsed.netloc}"
    
    # Create robot parser
    rp = RobotFileParser()
    rp.set_url(f"{base_url}/robots.txt")
    
    try:
        rp.read()  # Fetch and parse robots.txt
    except Exception:
        # If robots.txt doesn't exist, allow by default
        return True
    
    # Check if we can fetch this URL
    return rp.can_fetch(user_agent, url)


def get_crawl_delay(url: str, user_agent: str = "*") -> float | None:
    """Get the crawl-delay from robots.txt (if specified)."""
    
    parsed = urlparse(url)
    base_url = f"{parsed.scheme}://{parsed.netloc}"
    
    rp = RobotFileParser()
    rp.set_url(f"{base_url}/robots.txt")
    
    try:
        rp.read()
        return rp.crawl_delay(user_agent)
    except Exception:
        return None


# Example usage
url = "https://example.com/products"

if can_fetch(url):
    print(f"✅ Allowed to scrape: {url}")
    
    # Check if there's a crawl delay
    delay = get_crawl_delay(url)
    if delay:
        print(f"⏱️  Robots.txt requests {delay} second delay between requests")
else:
    print(f"❌ Not allowed to scrape: {url}")
```

### 💡 Explanation

- `RobotFileParser` — built-in Python class for parsing robots.txt
- `rp.can_fetch()` — checks if a specific URL is allowed
- `rp.crawl_delay()` — gets the required delay between requests

---

## ⚠️ Ethics Section: What NOT to Do

### Never Do These Things

| ❌ Don't Do This | Why It's Bad |
|------------------|---------------|
| Ignore robots.txt | Illegal and unethical |
| Send 100+ requests/second | DDoS attack, will get you blocked |
| Bypass login walls | Illegal (CFAA in US) |
| Scrape personal data (PII) | Privacy laws (GDPR, CCPA) |
| Circumvent CAPTCHAs | Usually against ToS |
| Use stolen proxies | Legal liability |
| Sell scraped data | Copyright infringement |

### What IS Acceptable

| ✅ Do This | Why It's Good |
|-----------|---------------|
| Scrape public, non-copyrighted data | Fair use |
| Use APIs when available | Legal and reliable |
| Rate limit to 1-2 req/sec | Respectful to server |
| Check and follow robots.txt | Ethical |
| Add randomness to requests | Looks like human behavior |
| Use for personal/educational purposes | Generally safe |

---

## Summary

✅ **Use proper headers** — User-Agent, Accept, Referer, Accept-Language

✅ **Add random delays** — 1-3 seconds between requests

✅ **Use sessions** — maintain cookies automatically

✅ **Rotate user-agents** — use fake-useragent library

✅ **Check robots.txt** — use urllib.robotparser

✅ **Use Playwright stealth** — for JavaScript-rendered sites

✅ **Never bypass security** — ethical scraping only

---

## ➡️ Next Steps

Continue to [03_javascript_heavy_sites.md](./03_javascript_heavy_sites.md) to learn how to handle JavaScript-rendered sites with Playwright.

---

## 🔗 Further Reading

- [Playwright Documentation](https://playwright.dev/python/)
- [fake-useragent Library](https://pypi.org/project/fake-useragent/)
- [Robots.txt Specification](https://www.robotstxt.org/robotstxt.html)
- [httpx Documentation](https://www.python-httpx.org/)
