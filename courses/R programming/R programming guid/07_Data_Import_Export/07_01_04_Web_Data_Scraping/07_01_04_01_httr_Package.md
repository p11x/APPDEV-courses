# httr Package: HTTP Requests in R

## Learning Objectives

- Make HTTP GET and POST requests
- Handle HTTP headers and authentication
- Process API responses
- Handle cookies and sessions
- Manage request timeouts and errors

## Theory

httr is a package for performing HTTP requests in R. It provides functions for all HTTP methods: GET, POST, PUT, PATCH, DELETE, and HEAD. The package handles HTTP authentication, cookies, headers, and multipart uploads.

Key functions: GET() for fetching pages, POST() for sending data, content() to extract response body, and handle() for connection management.

## Step-by-Step

1. Load httr package
2. Make request with GET() or POST()
3. Check response status with status_code()
4. Extract content with content()
5. Handle errors appropriately

## Code Examples

### Basic GET Request

```r
cat("===== BASIC HTTP REQUESTS =====\n\n")

library(httr)

# Simple GET request
response <- tryCatch({
  GET("http://httpbin.org/get")
}, error = function(e) {
  cat("Request failed:", e$message, "\n")
  NULL
})

if (!is.null(response)) {
  cat("Status code:", status_code(response), "\n")
  cat("Content-Type:", headers(response)$`content-type`, "\n")
  cat("Response length:", length(content(response)), "\n")
}
```

### Using Query Parameters

```r
cat("\n===== QUERY PARAMETERS =====\n\n")

# GET with query parameters
params <- list(
  q = "R programming",
  limit = "10"
)

resp2 <- tryCatch({
  GET("http://httpbin.org/get", query = params)
}, error = function(e) NULL)

if (!is.null(resp2)) {
  cat("URL:", resp2$url, "\n")
  cat("Status:", status_code(resp2), "\n")
}
```

### POST Requests

```r
cat("\n===== POST REQUESTS =====\n\n")

# POST with form data
form_data <- list(
  username = "testuser",
  password = "testpass"
)

resp3 <- tryCatch({
  POST("http://httpbin.org/post", body = form_data)
}, error = function(e) NULL)

if (!is.null(resp3)) {
  cat("Status:", status_code(resp3), "\n")
  cat("Response posted successfully\n")
}

# POST with JSON
resp4 <- tryCatch({
  POST(
    "http://httpbin.org/post",
    encode = "json",
    body = list(data = list(x = 1, y = 2))
  )
}, error = function(e) NULL)

if (!is.null(resp4)) {
  cat("JSON POST status:", status_code(resp4), "\n")
}
```

### Headers and Authentication

```r
cat("\n===== HEADERS AND AUTH =====\n\n")

# Custom headers
resp5 <- tryCatch({
  GET(
    "http://httpbin.org/headers",
    add_headers(
      "X-Custom-Header" = "myvalue",
      "Accept-Language" = "en-US"
    )
  )
}, error = function(e) NULL)

if (!is.null(resp5)) {
  cat("Custom headers sent\n")
  content	resp5)
}

# Basic authentication
resp6 <- tryCatch({
  GET(
    "http://httpbin.org/basic-auth/user/passwd",
    authenticate("user", "passwd", type = "basic")
  )
}, error = function(e) NULL)

if (!is.null(resp6)) {
  cat("Auth status:", status_code(resp6), "\n")
}

# Bearer token
resp7 <- tryCatch({
  GET(
    "http://httpbin.org/bearer",
    add_headers(Authorization = "Bearer mytoken123")
  )
}, error = function(e) NULL)

if (!is.null(resp7)) {
  cat("Bearer auth status:", status_code(resp7), "\n")
}
```

### Handling Response Content

```r
cat("\n===== RESPONSE CONTENT =====\n\n")

# Parse as text
resp_text <- tryCatch({
  GET("http://httpbin.org/robots.txt")
}, error = function(e) NULL)

if (!is.null(resp_text)) {
  text <- content(resp_text, as = "text")
  cat("As text:", substr(text, 1, 100), "...\n")
}

# Parse as raw bytes
resp_raw <- tryCatch({
  GET("http://httpbin.org/bytes/100")
}, error = function(e) NULL)

if (!is.null(resp_raw)) {
  raw_content <- content(resp_raw, as = "raw")
  cat("Raw bytes length:", length(raw_content), "\n")
}

# Parse as JSON
resp_json <- tryCatch({
  GET("http://httpbin.org/json")
}, error = function(e) NULL)

if (!is.null(resp_json)) {
  json_data <- content(resp_json, as = "parsed")
  cat("JSON parsed successfully\n")
}
```

## Real-World Example: API Data Collection

```r
# Real-world: Collect data from REST API
cat("===== API DATA COLLECTION =====\n\n")

cat("Template for API data collection:\n\n")

cat("# Function to fetch paginated data\n")
cat("fetch_data <- function(page = 1) {\n")
cat("  response <- GET(\n")
cat("    'https://api.example.com/data',\n")
cat("    query = list(\n")
cat("      page = page,\n")
cat("      per_page = 100,\n")
cat("      api_key = Sys.getenv('API_KEY')\n")
cat("    )\n")
cat("  )\n")
cat("  \n")
cat("  if (status_code(response) != 200) {\n")
cat("    stop('API error: ', status_code(response))\n")
cat("  }\n")
cat("  \n")
cat("  content(response, as = 'parsed')\n")
cat("}\n\n")

cat("# Fetch multiple pages\n")
cat("fetch_all <- function() {\n")
cat("  all_data <- list()\n")
cat("  page <- 1\n")
cat("  \n")
cat("  repeat {\n")
cat("    data <- fetch_data(page)\n")
cat("    if (length(data) == 0) break\n")
cat("    all_data[[page]] <- data\n")
cat("    page <- page + 1\n")
cat("    Sys.sleep(1)  # Be respectful\n")
cat("  }\n")
cat("  \n")
cat("  do.call(rbind, all_data)\n")
cat("}\n")
```

## Best Practices

1. Always check status_code() before processing response
2. Use encode = "json" for JSON APIs
3. Add delays between requests to avoid rate limiting
4. Use add_headers() for custom headers
5. Handle errors with tryCatch()
6. Set timeout with timeout() for long requests
7. Store credentials in environment variables, not code

## Exercises

1. Create a function to fetch paginated API data
2. Implement retry logic for failed requests
3. Write a function to download files from URLs
4. Handle different response formats (JSON, XML, text)
5. Implement rate limiting with exponential backoff