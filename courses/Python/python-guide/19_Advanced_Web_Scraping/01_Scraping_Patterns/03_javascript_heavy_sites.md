# 🔄 JavaScript-Heavy Sites

> Many modern sites render content with JavaScript — httpx alone won't work.

## 🎯 What You'll Learn

- How to detect if a site needs JavaScript rendering
- Finding the underlying API (best approach)
- Using Playwright for full browser automation
- Handling infinite scroll and login walls
- Playwright vs Selenium comparison

## 📦 Prerequisites

- Completion of [02_anti_detection_techniques.md](./02_anti_detection_techniques.md)
- Basic understanding of HTML and CSS selectors
- Knowledge of httpx for making HTTP requests

---

## Detecting JavaScript-Rendered Content

Before you decide to use Playwright, check if the site actually needs JavaScript:

### Method 1: View Page Source vs Inspect Element

```python
# Two ways to see page content:

# 1. View Page Source (shows raw HTML, NO JavaScript executed)
# In browser: Right-click → View Page Source
# What you see: The HTML BEFORE JavaScript runs

# 2. Inspect Element (shows rendered content, WITH JavaScript)
# In browser: Right-click → Inspect
# What you see: The HTML AFTER JavaScript modifies it
```

### If They're Different = JavaScript Rendering

```python
# Example: A dynamically loaded page
# 
# View Page Source shows:
# <div id="content"></div>
#
# Inspect Element shows:
# <div id="content">
#   <div class="product">Product 1</div>
#   <div class="product">Product 2</div>
#   ...
# </div>
#
# The products were added by JavaScript AFTER the page loaded!
```

### Method 2: Check Network Tab

Open DevTools → Network → filter by XHR or Fetch:

```python
# If you see API calls being made AFTER page load,
# the site is loading data via JavaScript
#
# Look for requests to:
# - /api/* 
# - /graphql
# - /data.json
#
# If found → use the API instead! (See 01_api_first_strategy.md)
```

---

## Strategy 1: Find the Underlying API (Best!)

The BEST approach is always to find the API that JavaScript uses:

### How to Find It

1. Open DevTools → Network tab
2. Filter by **Fetch** or **XHR**
3. Reload the page
4. Look for requests that return JSON data
5. Copy as cURL, then convert to Python

### Example: Finding a Product API

```python
# In DevTools Network tab, you might find:
# GET https://shop.example.com/api/products?page=1

# Instead of using Playwright, just call the API directly!
import httpx

# This is MUCH faster and more reliable than scraping
response = httpx.get(
    "https://shop.example.com/api/products",
    params={"page": 1, "limit": 50},
    headers={"Accept": "application/json"}
)

products = response.json()  # Direct access to data!

for product in products["data"]:
    print(f"{product['name']}: ${product['price']}")
```

### 💡 Explanation

- API calls return structured JSON — no parsing needed
- APIs are faster than full browser automation
- APIs are more stable — they don't change as often as CSS selectors

---

## Strategy 2: Playwright for Full Browser Automation

When there's no API available, use Playwright:

### Installation

```bash
pip install playwright
playwright install chromium
```

### Basic Playwright Usage

```python
from playwright.sync_api import sync_playwright


def scrape_with_playwright(url: str) -> str:
    """Scrape a JavaScript-rendered page."""
    
    # Start Playwright
    with sync_playwright() as p:
        # Launch browser (headless=True = no visible window)
        browser = p.chromium.launch(headless=True)
        
        # Create a new page
        page = browser.new_page()
        
        # Navigate to URL
        page.goto(url)
        
        # Wait for page to load
        page.wait_for_load_state("networkidle")
        
        # Get the rendered HTML
        html = page.content()
        
        # Close browser
        browser.close()
        
        return html


# Usage
html = scrape_with_playwright("https://example.com")
print(html[:1000])
```

### 💡 Explanation

- `sync_playwright()` — context manager for Playwright
- `p.chromium.launch()` — starts Chrome browser
- `page.goto(url)` — navigates to URL (like typing in address bar)
- `page.wait_for_load_state("networkidle")` — waits until all network requests finish
- `page.content()` — gets the fully rendered HTML

### Waiting for Elements

```python
from playwright.sync_api import sync_playwright


def wait_for_element(url: str, selector: str) -> str:
    """Wait for a specific element to appear."""
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()
        
        # Navigate to page
        page.goto(url)
        
        # Wait for element to appear AFTER JavaScript runs
        page.wait_for_selector(selector)
        
        # Get the element's text
        text = page.locator(selector).inner_text()
        
        browser.close()
        
        return text


# Example: Wait for a product to load
product_name = wait_for_element(
    "https://example.com/product/123",
    ".product-title"  # CSS selector for product title
)
print(f"Product: {product_name}")
```

### 💡 Explanation

- `page.wait_for_selector(selector)` — waits until element exists in DOM
- `page.locator(selector)` — finds element(s) by CSS selector
- `.inner_text()` — gets the text content of the element

### Running JavaScript in Page Context

```python
from playwright.sync_api import sync_playwright


def execute_javascript(url: str) -> dict:
    """Execute JavaScript in the page context."""
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()
        
        page.goto(url)
        
        # Run custom JavaScript in the page
        # This can access and manipulate the page!
        result = page.evaluate("""
            () => {
                // Get all product prices
                const prices = document.querySelectorAll('.price');
                const priceArray = Array.from(prices).map(el => el.innerText);
                
                // Get page title
                const title = document.title;
                
                return {
                    title: title,
                    prices: priceArray
                };
            }
        """)
        
        browser.close()
        
        return result


# Usage
data = execute_javascript("https://example.com/products")
print(f"Title: {data['title']}")
print(f"Prices: {data['prices']}")
```

### 💡 Explanation

- `page.evaluate()` — runs JavaScript in the browser context
- Can access DOM elements, window, document
- Returns the result back to Python

### Intercepting Network Requests

```python
from playwright.sync_api import sync_playwright


def intercept_api_calls(url: str, target_endpoint: str) -> list[dict]:
    """Capture API calls made by JavaScript."""
    
    intercepted = []  # Store captured requests
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()
        
        # Intercept network requests
        def handle_request(request):
            if target_endpoint in request.url:
                intercepted.append({
                    "url": request.url,
                    "method": request.method,
                    "post_data": request.post_data
                })
        
        # Register the interceptor
        page.on("request", handle_request)
        
        # Navigate to page (JavaScript will make API calls)
        page.goto(url)
        
        # Wait for some time for calls to complete
        page.wait_for_timeout(3000)  # Wait 3 seconds
        
        browser.close()
        
        return intercepted


# Example: Capture all API calls to /api/
calls = intercept_api_calls("https://example.com/dashboard", "/api/")
for call in calls:
    print(f"{call['method']} {call['url']}")
```

### 💡 Explanation

- `page.on("request", handle_request)` — listens for all network requests
- Filter by URL to find API calls
- `page.wait_for_timeout()` — waits for JavaScript to complete its requests

---

## Strategy 3: Selenium (Legacy — Use Playwright Instead)

Selenium is older and slower. Here's a quick comparison:

| Feature | Playwright | Selenium |
|---------|-----------|----------|
| Speed | Faster | Slower |
| Reliability | More reliable | Less reliable |
| API | Modern | Legacy |
| Stealth | Better by default | Needs extra config |
| Maintenance | Actively maintained | Older |

### If You Must Use Selenium

```python
# pip install selenium webdriver-manager
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager


def scrape_with_selenium(url: str) -> str:
    """Legacy method - prefer Playwright."""
    
    # Setup Chrome driver
    service = Service(ChromeDriverManager().install())
    driver = webdriver.Chrome(service=service)
    
    try:
        driver.get(url)
        
        # Wait for element
        from selenium.webdriver.support.ui import WebDriverWait
        from selenium.webdriver.support import expected_conditions as EC
        
        # Wait up to 10 seconds for element
        element = WebDriverWait(driver, 10).until(
            EC.presence_of_element_located(("css selector", ".content"))
        )
        
        return driver.page_source
    
    finally:
        driver.quit()


# ⚠️ WARNING: This is legacy code. Use Playwright instead!
```

---

## Handling Infinite Scroll

Many sites load more content as you scroll:

### Method: Scroll and Wait

```python
from playwright.sync_api import sync_playwright


def handle_infinite_scroll(url: str, selector: str, max_scrolls: int = 10) -> list[str]:
    """Handle infinite scroll by scrolling down multiple times."""
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()
        
        page.goto(url)
        
        items = []  # Store scraped items
        
        for i in range(max_scrolls):
            # Wait for content to load
            page.wait_for_timeout(2000)  # Wait 2 seconds
            
            # Scroll to bottom
            page.evaluate("window.scrollTo(0, document.body.scrollHeight)")
            
            # Try to get new items
            new_items = page.locator(selector).all_inner_texts()
            
            # Check if we got new content
            if len(new_items) > len(items):
                items = new_items
                print(f"Scroll {i+1}: Found {len(items)} items")
            else:
                # No new content - we've reached the end
                print("No more content to load")
                break
        
        browser.close()
        
        return items


# Example: Scrape all items from infinite scroll page
products = handle_infinite_scroll(
    "https://example.com/products",
    ".product-card",
    max_scrolls=20
)
print(f"Total products: {len(products)}")
```

### 💡 Explanation

- `page.evaluate("window.scrollTo(0, document.body.scrollHeight)")` — scrolls to bottom
- `page.wait_for_timeout(2000)` — waits for new content to load
- Check if new items were found — if not, we've reached the end

---

## Handling Login Walls

Some content requires login:

### Method: Fill Login Form

```python
from playwright.sync_api import sync_playwright
import json


def login_and_scrape(url: str, username: str, password: str) -> str:
    """Login to a site and scrape protected content."""
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()
        
        # Step 1: Go to login page
        page.goto("https://example.com/login")
        
        # Step 2: Fill login form
        page.fill('input[name="username"]', username)
        page.fill('input[name="password"]', password)
        
        # Step 3: Click login button
        page.click('button[type="submit"]')
        
        # Step 4: Wait for redirect to dashboard
        page.wait_for_url("**/dashboard")
        
        # Step 5: Navigate to protected content
        page.goto(url)
        
        # Step 6: Wait for content to load
        page.wait_for_selector(".protected-content")
        
        # Step 7: Get the content
        content = page.content()
        
        browser.close()
        
        return content


# Example: Save session cookies for reuse
def save_session_cookies(session_file: str = "session.json"):
    """Save cookies after login for reuse."""
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()
        
        # Login
        page.goto("https://example.com/login")
        page.fill('input[name="username"]', "myuser")
        page.fill('input[name="password"]', "mypassword")
        page.click('button[type="submit"]')
        page.wait_for_url("**/dashboard")
        
        # Save cookies
        cookies = page.context.cookies()
        with open(session_file, "w") as f:
            json.dump(cookies, f)
        
        browser.close()
        print(f"Saved cookies to {session_file}")


def load_session_and_scrape(url: str, session_file: str = "session.json"):
    """Load saved cookies and scrape without logging in again."""
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        context = browser.new_context()
        
        # Load cookies
        with open(session_file) as f:
            cookies = json.load(f)
        context.add_cookies(cookies)
        
        page = context.new_page()
        
        # Navigate to protected page
        page.goto(url)
        content = page.content()
        
        browser.close()
        
        return content
```

### 💡 Explanation

- `page.fill(selector, text)` — fills input fields
- `page.click(selector)` — clicks buttons/links
- `page.wait_for_url()` — waits for navigation to complete
- `page.context.cookies()` — gets all cookies from session
- `context.add_cookies()` — restores cookies for future sessions

---

## Full Example: Scrape JS-Rendered Product Listing

```python
from playwright.sync_api import sync_playwright
from bs4 import BeautifulSoup


def scrape_product_listing(url: str) -> list[dict]:
    """Scrape a JavaScript-rendered product listing page."""
    
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()
        
        # Set realistic headers
        page.set_extra_http_headers({
            "Accept-Language": "en-US,en;q=0.9"
        })
        
        # Navigate to page
        page.goto(url)
        
        # Wait for products to load
        page.wait_for_selector(".product-card")
        
        # Wait a bit more for all products to render
        page.wait_for_timeout(2000)
        
        # Get page HTML (fully rendered)
        html = page.content()
        
        browser.close()
        
        # Parse with BeautifulSoup
        soup = BeautifulSoup(html, "html.parser")
        
        products = []
        for card in soup.select(".product-card"):
            product = {
                "name": card.select_one(".product-name").get_text(strip=True),
                "price": card.select_one(".product-price").get_text(strip=True),
                "rating": card.select_one(".product-rating").get_text(strip=True) if card.select_one(".product-rating") else None,
            }
            products.append(product)
        
        return products


# Usage
products = scrape_product_listing("https://books.toscrape.com/catalogue/category/books_1/index.html")
for p in products:
    print(f"{p['name']}: {p['price']}")
```

### 💡 Explanation

- Playwright handles JavaScript rendering
- BeautifulSoup parses the resulting HTML
- Best of both worlds: JS rendering + familiar parsing

---

## Summary

✅ **Check if JavaScript is needed** — View Source vs Inspect Element

✅ **Find the underlying API first** — always better than scraping

✅ **Use Playwright** — modern, fast, reliable browser automation

✅ **Wait for elements** — use wait_for_selector and networkidle

✅ **Handle infinite scroll** — scroll and wait for new content

✅ **Handle login** — fill forms, save cookies for reuse

✅ **Don't use Selenium** — Playwright is better in every way

---

## ➡️ Next Steps

Continue to [02_Data_Pipelines/01_etl_pattern.md](../02_Data_Pipelines/01_etl_pattern.md) to learn how to build ETL pipelines for scraped data.

---

## 🔗 Further Reading

- [Playwright Python Documentation](https://playwright.dev/python/)
- [Playwright Wait For Selector](https://playwright.dev/python/docs/api/class-page#page-wait-for-selector)
- [BeautifulSoup Documentation](https://www.crummy.com/software/BeautifulSoup/bs4/doc/)
- [books.toscrape.com](http://books.toscrape.com/) - Legal scraping practice site
