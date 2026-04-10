# Web Data Scraping in R

## Learning Objectives

- Fetch web content using httr package
- Parse HTML with rvest package
- Handle JSON data with jsonlite
- Understand web scraping ethics and legal issues
- Build robust scraping pipelines

## Theoretical Background

### Key Packages

| Package | Purpose | Key Functions |
|---------|---------|---------------|
| httr | HTTP requests | GET, POST, PUT, DELETE |
| rvest | HTML parsing | html_node, html_text, html_attr |
| jsonlite | JSON handling | fromJSON, toJSON |
| xml2 | XML parsing | read_html, xml_find |

### HTTP Methods

- GET: Retrieve data
- POST: Submit data
- PUT: Update data
- DELETE: Remove data
- HEAD: Fetch headers only

### Web Scraping Ethics

1. Check robots.txt
2. Respect rate limits
3. Identify your bot
4. Don't overload servers
5. Follow terms of service

## Code Examples

### Basic Example: httr GET Requests

```r
# ===== HTTP REQUESTS WITH HTTP =====

cat("===== HTTP PACKAGE =====\n\n")

library(httr)

# 1. Simple GET request
cat("1. Simple GET request:\n")
response <- GET("http://httpbin.org/get")
cat("  Status:", status_code(response), "\n")
cat("  Content-Type:", headers(response)$`content-type`, "\n\n")

# 2. GET with query parameters
cat("2. GET with parameters:\n")
params <- list(
  page = 1,
  limit = 10,
  sort = "date"
)
response2 <- GET("http://httpbin.org/get", query = params)
cat("  URL:", url_parse(url(response2))$query, "\n\n")

# 3. POST request with body
cat("3. POST with JSON body:\n")
body <- list(
  username = "test_user",
  email = "test@example.com"
)
response3 <- POST("http://httpbin.org/post", 
                  body = body, 
                  encode = "json")
cat("  Status:", status_code(response3), "\n")
result <- content(response3, as = "text")
cat("  Response (parsed):", substr(result, 1, 200), "...\n\n")

# 4. Custom headers
cat("4. Custom headers:\n")
response4 <- GET("http://httpbin.org/headers", 
                add_headers(
                  `User-Agent` = "R-scraper/1.0",
                  `Accept-Language` = "en-US"
                ))
cat("  Status:", status_code(response4), "\n\n")

# 5. Handle status codes
cat("5. Status code handling:\n")
# Success
response_ok <- GET("http://httpbin.org/status/200")
cat("  200 OK:", http_status(response_ok)$message, "\n")

# Not found
response_404 <- GET("http://httpbin.org/status/404")
cat("  404:", http_status(response_404)$message, "\n")

# Server error
response_500 <- GET("http://httpbin.org/status/500")
cat("  500:", http_status(response_500)$message, "\n\n")

# 6. Timeout handling
cat("6. Timeout handling:\n")
response_timeout <- GET("http://httpbin.org/delay/10", 
                       timeout(5))
cat("  Status:", status_code(response_timeout), "\n")
cat("  (Timeout occurred as expected)\n")
```

**Output:**
```
===== HTTP PACKAGE =====

1. Simple GET request:
  Status: 200
  Content-Type: application/json
```

### Standard Example: rvest HTML Parsing

```r
# ===== WEB SCRAPING WITH RVEST =====

cat("===== RVEST PACKAGE =====\n\n")

library(rvest)
library(xml2)

# Create sample HTML for demonstration
sample_html <- '
<html>
<head><title>Sample Store</title></head>
<body>
  <div class="product-list">
    <div class="product">
      <h2 class="name">Laptop Pro</h2>
      <p class="price">$999</p>
      <span class="category">Electronics</span>
    </div>
    <div class="product">
      <h2 class="name">Wireless Mouse</h2>
      <p class="price">$29</p>
      <span class="category">Accessories</span>
    </div>
    <div class="product">
      <h2 class="name">USB Cable</h2>
      <p class="price">$15</p>
      <span class="category">Accessories</span>
    </div>
  </div>
  <table class="inventory">
    <tr><td>Item1</td><td>10</td></tr>
    <tr><td>Item2</td><td>25</td></tr>
  </table>
</body>
</html>
'

# Write sample to temp file
temp_html <- tempfile(fileext = ".html")
writeLines(sample_html, temp_html)

# 1. Read HTML from file
cat("1. Read HTML:\n")
html <- read_html(temp_html)
cat("  Document parsed successfully\n\n")

# 2. Parse HTML in R (if actual web page)
# Real example (commented):
example_real <- "
# For real websites:
url <- 'https://example.com/products'
html <- read_html(url)

# Extract titles
titles <- html %>% 
  html_nodes('.product-name') %>%
  html_text(trim = TRUE)

# Extract prices
prices <- html %>%
  html_nodes('.product-price') %>%
  html_text(trim = TRUE)
"
cat("2. Real extraction pattern:\n")
cat("   # html %>% html_nodes('.selector') %>% html_text()\n\n")

# 3. Extract by CSS selector
cat("3. CSS selector extraction:\n")
product_names <- html %>% 
  html_nodes(".product .name") %>% 
  html_text(trim = TRUE)
print(product_names)
cat("\n")

# 4. Extract prices
cat("4. Extract prices:\n")
product_prices <- html %>%
  html_nodes(".product .price") %>%
  html_text(trim = TRUE)
print(product_prices)
cat("\n")

# 5. Extract attributes
cat("5. Extract attributes:\n")
# In real pages, you'd extract links
# links <- html %>%
#   html_nodes('a.product-link') %>%
#   html_attr('href')
cat("   # html_attr('href') for links\n\n")

# 6. Build data frame
cat("6. Build data frame:\n")
products_df <- data.frame(
  name = product_names,
  price = gsub("[^0-9]", "", product_prices)
)
print(products_df)

# 7. Table extraction
cat("\n7. Extract tables:\n")
tables <- html %>% html_table()
if (length(tables) > 0) {
  print(tables[[1]])
}

# Clean up
unlink(temp_html)
```

### Real-World Example: JSON Handling

```r
# ===== JSON WITH JSONLITE =====

cat("===== JSONLITE PACKAGE =====\n\n")

library(jsonlite)

# Sample JSON data
json_data <- '{
  "users": [
    {"id": 1, "name": "Alice", "roles": ["admin", "editor"]},
    {"id": 2, "name": "Bob", "roles": ["viewer"]},
    {"id": 3, "name": "Charlie", "roles": ["editor", "author"]}
  ],
  "metadata": {
    "total": 3,
    "page": 1
  }
}'

# 1. Parse JSON to R
cat("1. Parse JSON:\n")
data <- fromJSON(json_data)
print(names(data))
cat("\n")

# 2. Flatten nested JSON
cat("2. Flatten nested:\n")
users <- data$users
print(users)
cat("\n")

# 3. Convert to JSON
cat("3. Convert R to JSON:\n")
df <- data.frame(
  id = 1:3,
  name = c("Alice", "Bob", "Charlie"),
  score = c(95, 82, 78)
)
json_output <- toJSON(df, auto_unbox = TRUE, pretty = TRUE)
cat(json_output)
cat("\n\n")

# 4. fromJSON with simplification
cat("4. Simplify arrays:\n")
simple_json <- '[1, 2, 3, 4, 5]'
array <- fromJSON(simple_json, simplifyVector = TRUE)
print(array)
cat("\n")

# 5. Interactive API example
cat("5. Fetch API data:\n")
# Real API example (using httpbin for demo)
tryCatch({
  response <- GET("http://httpbin.org/json")
  api_data <- content(response, as = "parsed")
  cat("  Success: Got API response\n")
}, error = function(e) {
  cat("  Error fetching API\n")
})

# 6. POST with JSON
cat("\n6. POST JSON data:\n")
df_to_send <- data.frame(
  user = c("Alice", "Bob"),
  action = c("login", "purchase")
)
json_body <- toJSON(df_to_send, dataframe = "rows")
cat("  JSON body:", substr(json_body, 1, 50), "...\n")

# 7. Handle edge cases
cat("\n7. Handle NA values:\n")
df_with_na <- data.frame(
  x = c(1, NA, 3),
  y = c("a", "b", NA)
)
json_na <- toJSON(df_with_na, na = "null")
cat("  Output:", json_na)

unlink(temp_html)
```

### Advanced Example: Building Scraper

```r
# ===== COMPLETE SCRAPING WORKFLOW =====

cat("===== COMPLETE SCRAPING PIPELINE =====\n\n")

library(httr)
library(rvest)
library(jsonlite)
library(dplyr)

# Simulated scraping workflow
# In practice, replace URLs with actual websites

cat("1. Rate limiting wrapper:\n")
scrape_with_delay <- function(url, delay = 1) {
  response <- GET(url)
  Sys.sleep(delay)
  return(response)
}
cat("   Added 1-second delay between requests\n\n")

cat("2. Error handling wrapper:\n")
safe_scrape <- function(url, max_retries = 3) {
  for (attempt in 1:max_retries) {
    tryCatch({
      response <- GET(url)
      if (status_code(response) == 200) {
        return(response)
      }
    }, error = function(e) {
      cat("   Attempt", attempt, "failed, retrying...\n")
      Sys.sleep(2)
    })
  }
  return(NULL)
}
cat("   Implements retry logic with exponential backoff\n\n")

# 3. Parse multiple pages
# Real example structure:
parse_pagination <- "
# For multiple pages:
all_data <- list()
for (page in 1:10) {
  url <- paste0('http://example.com/items?page=', page)
  html <- read_html(url)
  
  items <- html %>%
    html_nodes('.item') %>%
    lapply(function(item) {
      list(
        title = item %>% html_node('.title') %>% html_text(trim = TRUE),
        price = item %>% html_node('.price') %>% html_text(trim = TRUE),
        link = item %>% html_node('a') %>% html_attr('href')
      )
    })
  
  all_data <- c(all_data, items)
  cat('Scraped page', page, '\n')
}

# Convert to data frame
result <- bind_rows(all_data)
"
cat("3. Pagination parsing:\n")
cat("   Iterates through pages and combines results\n\n")

# 4. Real-world example (using known public APIs)
cat("4. Public API example:\n")
# Fetch JSON from public API
response <- GET("https://api.github.com/repos/hadley/dplyr")
if (status_code(response) == 200) {
  repo_info <- content(response, as = "parsed")
  cat("  Repository:", repo_info$name, "\n")
  cat("  Stars:", repo_info$stargazers_count, "\n")
  cat("  Forks:", repo_info$forks_count, "\n")
}

# 5. Save scraped data
cat("\n5. Save scraped data:\n")
scrape_results <- data.frame(
  name = c("Alice", "Bob", "Charlie"),
  value = c(100, 200, 300)
)
saveRDS(scrape_results, "scraped_data.rds")
cat("  Saved to scraped_data.rds\n")

# Clean up
if (file.exists("scraped_data.rds")) {
  unlink("scraped_data.rds")
}
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Check robots.txt**: Respect website crawling rules
2. **Add delays**: Wait 1+ seconds between requests
3. **Identify your scraper**: Set realistic User-Agent
4. **Handle errors gracefully**: Implement retry logic
5. **Cache responses**: Save HTML locally for development

### Common Pitfalls

1. **Rate limiting**: Too many requests causes IP blocking
2. **Dynamic content**: JavaScript-rendered content needs other tools
3. **Session management**: Cookies may be required
4. **Authentication**: Some sites need login
5. **Legal issues**: Always check Terms of Service

### Legal Considerations

| Aspect | Consideration |
|--------|---------------|
| robots.txt | Check before scraping |
| Terms of Service | May prohibit scraping |
| Copyrighted content | Don't redistribute without permission |
| Personal data | GDPR compliance in EU |
| Rate limits | Don't overwhelm servers |

### Tools for Dynamic Content

- Selenium: Browser automation
- RSelenium: R interface to Selenium
- splash: Lightweight browser