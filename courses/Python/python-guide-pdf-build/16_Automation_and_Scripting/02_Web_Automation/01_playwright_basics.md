# 🌐 Web Automation with Playwright

## 🎯 What You'll Learn

- Installing and setting up Playwright
- Basic browser automation
- Extracting data from web pages

---

## Installation

```bash
pip install playwright
playwright install
```

---

## Basic Usage

```python
from playwright.sync_api import sync_playwright

with sync_playwright() as p:
    # Launch browser
    browser = p.chromium.launch()
    page = browser.new_page()
    
    # Navigate
    page.goto("https://example.com")
    
    # Get content
    title = page.title()
    print(title)
    
    # Take screenshot
    page.screenshot(path="screenshot.png")
    
    browser.close()
```

---

## Interacting with Elements

```python
# Click
page.click("button#submit")

# Fill input
page.fill("input[name='email']", "test@example.com")

# Select
page.select_option("select#country", "US")

# Wait for element
page.wait_for_selector("div.results")
```

---

## Extracting Data

```python
# Get text content
titles = page.query_selector_all("h2")
for title in titles:
    print(title.inner_text())

# Get attributes
links = page.query_selector_all("a")
for link in links:
    print(link.get_attribute("href"))
```

---

## Waiting

```python
# Wait for navigation
page.goto("https://example.com")
page.wait_for_load_state("networkidle")

# Wait for selector
page.wait_for_selector("#result")

# Wait for response
page.wait_for_response(lambda r: "/api/data" in r.url)
```

---

## ✅ Summary

- Playwright is modern browser automation
- Use sync_playwright() for simple scripts
- Use page.goto(), click(), fill() for interaction
- Use query_selector_all() to extract data

## 🔗 Further Reading

- [Playwright Python Docs](https://playwright.dev/python/)
