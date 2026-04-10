# rvest Package: Web Scraping in R

## Learning Objectives

- Parse HTML pages
- Select elements using CSS and XPath
- Extract text, attributes, and tables
- Navigate through multiple pages
- Handle JavaScript-rendered content

## Theory

rvest is the primary package for web scraping in R. It provides functions to download web pages, parse HTML, and extract data using CSS selectors or XPath expressions. The package is designed to work similarly to Beautiful Soup in Python.

Key functions: html_session() for managing sessions, html_nodes() for selecting elements, html_text() for extracting text, and html_attr() for attributes.

## Step-by-Step

1. Load rvest and create session with html_session()
2. Use html_nodes() to select elements
3. Choose CSS or XPath selector
4. Extract data with html_text() or html_attr()
5. Handle pagination if needed

## Code Examples

### Basic Page Parsing

```r
cat("===== BASIC WEB SCRAPING =====\n\n")

library(rvest)

# Read and parse a simple page
page <- tryCatch({
  read_html("http://httpbin.org/html")
}, error = function(e) {
  cat("Failed to read page\n")
  NULL
})

if (!is.null(page)) {
  cat("Page title:", html_text(html_node(page, "title")), "\n")
  
  # Extract all headings
  headings <- html_nodes(page, "h1, h2, h3")
  cat("Found", length(headings), "headings\n")
}
```

### CSS Selectors

```r
cat("\n===== CSS SELECTORS =====\n\n")

# Extract specific elements
sample_html <- '
<html>
  <body>
    <div class="content">
      <p class="intro">Welcome!</p>
      <p class="body">Main text here.</p>
    </div>
    <a href="http://example.com">Link</a>
    <ul><li>Item 1</li><li>Item 2</li></ul>
  </body>
</html>
'

doc <- read_html(sample_html)

# Select by tag
cat("Paragraphs:")
ps <- html_nodes(doc, "p")
cat(paste(html_text(ps), collapse = ", "), "\n")

# Select by class
cat("\nBy class '.intro':")
intro <- html_text(html_node(doc, ".intro"))
cat(intro)

# Select by attribute
cat("\nLink href:")
href <- html_attr(html_node(doc, "a"), "href")
cat(href)

# Select all list items
cat("\nAll list items:")
lis <- html_text(html_nodes(doc, "li"))
cat(paste(lis, collapse = ", "))
```

### XPath Selectors

```r
cat("\n===== XPATH SELECTORS =====\n\n")

# XPath provides more control
cat("Direct child: /body/p\n")
cat("Descendant: //p\n")
cat("Attribute: //a[@href]\n")
cat("Contains text: //p[contains(text(), 'Welcome')]\n")

# Use XPath
cat("\nUsing XPath:\n")
xpath_result <- html_nodes(doc, xpath = "//div[@class='content']/p")
cat(html_text(xpath_result))
```

### Extracting Tables

```r
cat("\n===== EXTRACTING TABLES =====\n\n")

table_html <- '
<html><body>
<table>
  <tr><th>Name</th><th>Value</th></tr>
  <tr><td>A</td><td>100</td></tr>
  <tr><td>B</td><td>200</td></tr>
  <tr><td>C</td><td>300</td></tr>
</table>
</body></html>
'

doc2 <- read_html(table_html)

# Convert table to data frame
table_df <- html_table(html_node(doc2, "table"))
cat("Table extracted:\n")
print(table_df)
```

### Handling Forms

```r
cat("\n===== FORMS =====\n\n")

cat("Submitting forms with rvest:\n\n")

cat("# Create session\n")
cat("# session <- html_session('http://example.com/form')\n\n")

cat("# Fill form\n")
cat("# form <- html_form(session)[[1]]\n")
cat("# filled <- set_values(form,\n")
cat("#   username = 'myuser',\n")
cat("#   password = 'mypass'\n")
cat("# )\n\n")

cat("# Submit\n")
cat("# result <- submit_form(session, filled)\n")
```

## Real-World Example: Price Comparison

```r
# Real-world: Scrape product prices
cat("===== PRICE COMPARISON =====\n\n")

cat("Template for price comparison:\n\n")

cat("# Scrape product listings\n")
cat("scrape_products <- function(search_term) {\n")
cat("  url <- paste0(\n")
cat("    'https://example.com/search?q=',\n")
cat("    URLEncode(search_term)\n")
cat("  )\n")
cat("  \n")
cat("  session <- html_session(url)\n")
cat("  page <- read_html(session)\n")
cat("  \n")
cat("  # Extract product info\n")
cat("  products <- html_nodes(page, '.product-item')\n")
cat("  \n")
cat("  data.frame(\n")
cat("    name = html_text(html_node(products, '.title')),\n")
cat("    price = html_text(html_node(products, '.price')),\n")
cat("    url = html_attr(html_node(products, 'a'), 'href')\n")
cat("  )\n")
cat("}\n\n")

cat("# Example usage\n")
cat("# prices <- scrape_products('laptop')\n")
cat("# head(prices)\n")
```

## Best Practices

1. Always check robots.txt before scraping
2. Add delays between requests
3. Set proper User-Agent header
4. Use CSS selectors for readability, XPath for power
5. Handle missing elements gracefully
6. Consider API alternatives when available

## Exercises

1. Scrape and extract all links from a webpage
2. Create a function to scrape multiple pages
3. Extract data from a JavaScript-rendered page
4. Handle different HTML structures
5. Build a complete price monitoring system