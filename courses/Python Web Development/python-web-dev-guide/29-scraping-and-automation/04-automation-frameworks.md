# Automation Frameworks

## What You'll Learn
- RPA concepts
- Task automation
- Scheduling

## Prerequisites
- Completed Selenium/Playwright

## Scheduling

```python
import schedule
import time

def job():
    print("Running scheduled task...")
    scrape_data()

# Schedule
schedule.every().day.at("10:00").do(job)
schedule.every().hour.do(job)

while True:
    schedule.run_pending()
    time.sleep(60)
```

## RPA with Python

```python
# Automate form filling
from playwright.async_api import async_playwright

async def fill_form(data: dict):
    async with async_playwright() as p:
        browser = await p.chromium.launch()
        page = await browser.new_page()
        await page.goto("https://form.example.com")
        await page.fill("#name", data["name"])
        await page.fill("#email", data["email"])
        await page.click("#submit")
        await browser.close()
```

## Summary
- Schedule repetitive tasks
- Use RPA for forms
- Automate workflows

## Next Steps
→ Continue to `05-rate-limiting-and-ethics.md`
