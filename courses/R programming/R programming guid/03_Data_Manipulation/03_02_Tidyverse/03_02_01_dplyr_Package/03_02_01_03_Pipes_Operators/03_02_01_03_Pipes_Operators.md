# Pipe Operators in R

## Learning Objectives

- Master the pipe operator (%>%)
- Understand pipe semantics
- Use pipes for readable code
- Handle multiple arguments with dot notation

## Theoretical Background

### The Pipe Operator

The pipe (%>%) passes the result of one function to the next:

- **Input**: object passed as first argument
- **Output**: result returned by final function
- **Purpose**: Readable data transformation chains

## Code Examples

### Standard Example: Pipe Basics

```r
library(dplyr)

cat("===== PIPE BASICS =====\n\n")

df <- data.frame(
  x = 1:10,
  y = 11:20
)

# Without pipe
result1 <- mean(df$x)

# With pipe
result2 <- df %>% 
  pull(x) %>% 
  mean()

cat("Without pipe:", result1, "\n")
cat("With pipe:", result2, "\n")

# Chain multiple operations
cat("\n===== CHAINING OPERATIONS =====\n\n")

sales <- data.frame(
  product = c("A", "B", "A", "B", "C", "C"),
  sales = c(100, 200, 150, 180, 220, 250),
  region = c("North", "North", "South", "South", "East", "West")
)

cat("Chain example:\n")
result <- sales %>%
  filter(sales > 100) %>%
  group_by(product) %>%
  summarise(total = sum(sales)) %>%
  arrange(desc(total))
print(result)
```

**Output:**
```
===== PIPE BASICS =====

Without pipe: 5.5
With pipe: 5.5
```

### Real-World Example: Complex Data Pipeline

```r
# Real-world: Complete data processing pipeline
cat("===== DATA PROCESSING PIPELINE =====\n\n")

# Create sample data
set.seed(42)
data <- data.frame(
  id = 1:100,
  category = sample(c("Electronics", "Clothing", "Food"), 100, replace = TRUE),
  price = runif(100, 10, 500),
  quantity = sample(1:10, 100, replace = TRUE),
  date = as.Date("2024-01-01") + sample(0:90, 100),
  stringsAsFactors = FALSE
)

cat("Processing pipeline:\n\n")

pipeline <- data %>%
  # Filter expensive items
  filter(price > 100) %>%
  # Calculate total
  mutate(total = price * quantity) %>%
  # Filter recent
  filter(date > as.Date("2024-02-01")) %>%
  # Group and summarize
  group_by(category) %>%
  summarise(
    total_revenue = sum(total),
    num_items = sum(quantity),
    avg_price = mean(price)
  ) %>%
  # Sort by revenue
  arrange(desc(total_revenue))

print(pipeline)
```

## Best Practices and Common Pitfalls

### Best Practices

1. One step per line for readability
2. Use intermediate variables for debugging
3. Consider alternative to pipes for 1-2 steps

### Common Issues

1. Using . when not needed
2. Forgetting first argument is piped
