# Selenium and Playwright

## What You'll Learn
- Browser automation
- JavaScript rendering
- Testing

## Prerequisites
- Completed BeautifulSoup

## Playwright

```bash
pip install playwright
playwright install
```

```python
import asyncio
from playwright.async_api import async_playwright

async def scrape():
    async with async_playwright() as p:
        browser = await p.chromium.launch()
        page = await browser.new_page()
        await page.goto("https://example.com")
        title = await page.title()
        print(title)
        await browser.close()

asyncio.run(scrape())
```

## Summary
- Use for JavaScript sites
- Playwright is faster/modern
- Headless by default

## Next Steps
→ Continue to `04-automation-frameworks.md`
