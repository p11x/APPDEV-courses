# BeautifulSoup

## What You'll Learn
- Parsing HTML
- Finding elements
- Extracting data

## Prerequisites
- Completed web scraping basics

## Finding Elements

```python
from bs4 import BeautifulSoup

html = """
<html>
  <div class="product">
    <h2 class="title">Product 1</h2>
    <span class="price">$99</span>
  </div>
</html>
"""

soup = BeautifulSoup(html, "html.parser")

# Find by class
products = soup.find_all("div", class_="product")
for p in products:
    title = p.find("h2", class_="title").text
    price = p.find("span", class_="price").text
    print(f"{title}: {price}")
```

## CSS Selectors

```python
soup.select(".product .title")  # Class
soup.select("#header")         # ID
soup.select("div > a")         # Child
```

## Summary
- BeautifulSoup for parsing
- CSS selectors for finding
- Extract text, attributes

## Next Steps
→ Continue to `03-selenium-playwright.md`
