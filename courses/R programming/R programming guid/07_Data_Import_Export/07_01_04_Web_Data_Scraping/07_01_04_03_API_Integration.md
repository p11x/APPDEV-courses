# API Integration in R

## Learning Objectives

- Integrate with REST APIs
- Handle API authentication
- Parse various response formats
- Manage rate limiting
- Build robust API clients

## Theory

API integration involves making HTTP requests to web services and processing the returned data. REST APIs typically return JSON data that needs to be parsed. Common authentication methods include API keys, OAuth, and Bearer tokens.

The httr package handles HTTP requests, and jsonlite parses JSON responses. Other formats like XML can be processed with xml2.

## Step-by-Step

1. Understand API documentation and endpoints
2. Set up authentication method
3. Build request with appropriate parameters
4. Parse response format
5. Handle errors and rate limits

## Code Examples

### Basic API Call

```r
cat("===== BASIC API CALL =====\n\n")

library(httr)
library(jsonlite)

# Make API request
resp <- tryCatch({
  GET("http://httpbin.org/json")
}, error = function(e) NULL)

if (!is.null(resp)) {
  cat("Status:", status_code(resp), "\n")
  
  # Parse JSON response
  data <- fromJSON(content(resp, as = "text"))
  cat("Parsed response successfully\n")
  cat("Response keys:", names(data), "\n")
}
```

### Authentication Methods

```r
cat("\n===== AUTHENTICATION =====\n\n")

cat("1. API Key in header:\n")
cat("GET('https://api.example.com/data', \n")
cat("    add_headers('X-API-Key' = 'your-key'))\n\n")

cat("2. API Key in query:\n")
cat("GET('https://api.example.com/data', \n")
cat("    query = list(api_key = 'your-key'))\n\n")

cat("3. Bearer token:\n")
cat("GET('https://api.example.com/data', \n")
cat("    add_headers(Authorization = 'Bearer token'))\n\n")

cat("4. OAuth2:\n")
cat("# token <- oauth_token()\n")
cat("# GET('https://api.example.com/data', \n")
cat("#     config(token = token))\n")
```

### Handling Response Formats

```r
cat("\n===== RESPONSE FORMATS =====\n\n")

cat("Parsing different formats:\n\n")

cat("1. JSON:\n")
cat("data <- fromJSON(content(resp, as = 'text'))\n\n")

cat("2. Pretty JSON:\n")
cat("pretty <- toJSON(data, pretty = TRUE)\n\n")

cat("3. XML:\n")
cat("# library(xml2)\n")
cat("# doc <- read_xml(content(resp, as = 'text'))\n")
cat("# items <- xml_find_all(doc, '//item')\n\n")

cat("4. Raw bytes:\n")
cat("# raw_data <- content(resp, as = 'raw')\n")
```

### Rate Limiting

```r
cat("\n===== RATE LIMITING =====\n\n")

cat("Handling rate limits:\n\n")

cat("# Check rate limit headers\n")
cat("rate_remaining <- headers(resp)$'X-RateLimit-Remaining'\n")
cat("rate_reset <- as.POSIXct(\n")
cat("  as.numeric(headers(resp)$'X-RateLimit-Reset'),\n")
cat("  origin = '1970-01-01'\n")
cat(")\n\n")

cat("# Wait and retry with backoff\n")
cat("wait_and_retry <- function(req, max_retries = 3) {\n")
cat("  for (i in 1:max_retries) {\n")
cat("    resp <- tryCatch(GET(req), error = identity)\n")
cat("    if (status_code(resp) == 429) {\n")
cat("      wait_time <- as.numeric(\n")
cat("        headers(resp)$'Retry-After'\n")
cat("      )\n")
cat("      Sys.sleep(wait_time)\n")
cat("    } else {\n")
cat("      return(resp)\n")
cat("    }\n")
cat("  }\n")
cat("  stop('Max retries exceeded')\n")
cat("}\n")
```

### Building API Client Functions

```r
cat("\n===== API CLIENT =====\n\n")

cat("Creating reusable API functions:\n\n")

cat("# Base API client\n")
cat("api_client <- function(endpoint, method = 'GET', ...) {\n")
cat("  base_url <- 'https://api.example.com'\n")
cat("  url <- paste0(base_url, endpoint)\n")
cat("  \n")
cat("  resp <- switch(method,\n")
cat("    GET = GET(url, ...),\n")
cat("    POST = POST(url, ...),\n")
cat("    PUT = PUT(url, ...),\n")
cat("    DELETE = DELETE(url, ...)\n")
cat("  )\n")
cat("  \n")
cat("  if (status_code(resp) >= 400) {\n")
cat("    stop('API error: ', status_code(resp))\n")
cat("  }\n")
cat("  \n")
cat("  content(resp, as = 'parsed')\n")
cat("}\n")
```

## Real-World Example: Weather API

```r
# Real-world: Weather data API
cat("===== WEATHER API =====\n\n")

cat("Template for weather API:\n\n")

cat("# Get weather data\n")
cat("get_weather <- function(city, api_key) {\n")
cat("  url <- 'https://api.weather.com/v1/forecast'\n")
cat("  \n")
cat("  resp <- GET(url, query = list(\n")
cat("    city = city,\n")
cat("    appid = api_key,\n")
cat("    units = 'metric'\n")
cat("  ))\n")
cat("  \n")
cat("  data <- fromJSON(content(resp, as = 'text'))\n")
cat("  \n")
cat("  data.frame(\n")
cat("    date = data$dates,\n")
cat("    temp = data$temperatures,\n")
cat("    conditions = data$conditions\n")
cat("  )\n")
cat("}\n\n")

cat("# Example usage\n")
cat("# weather <- get_weather('New York', Sys.getenv('WEATHER_KEY'))\n")
cat("# print(weather)\n")
```

## Best Practices

1. Store API keys in environment variables
2. Check status code before processing
3. Implement retry logic for transient errors
4. Handle all response formats consistently
5. Add rate limit handling
6. Log requests for debugging

## Exercises

1. Build a complete client for a public API
2. Implement OAuth2 authentication flow
3. Create paginated data fetching
4. Handle malformed responses gracefully
5. Cache API responses for efficiency