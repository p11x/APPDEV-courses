# Fill Missing Values with tidyr

## Learning Objectives

- Understand NA handling strategies
- Use fill() to fill missing values
- Handle forward and backward fill
- Use complete() for implicit values

## Code Examples

### Standard Example: fill() Function

```r
library(tidyr)

cat("===== FILL MISSING VALUES =====\n\n")

df <- data.frame(
  id = c(1, 2, 3, 4, 5),
  group = c("A", "A", NA, "B", "B"),
  value = c(10, NA, 30, NA, 50),
  stringsAsFactors = FALSE
)

cat("Data with missing values:\n")
print(df)

# Forward fill (down)
cat("\nForward fill (direction = 'down'):\n")
filled <- df %>% fill(group, .direction = "down")
print(filled)

# Backward fill (up)
cat("\nBackward fill (direction = 'up'):\n")
filled2 <- df %>% fill(group, .direction = "up")
print(filled2)

# Both directions
cat("\nFill in both directions:\n")
filled3 <- df %>% fill(group, .direction = "downup")
print(filled3)
```

### Real-World Example: Sales Data

```r
# Real-world: Fill missing sales data
cat("===== SALES DATA FILLING =====\n\n")

sales <- data.frame(
  date = as.Date(c("2024-01-01", "2024-01-02", "2024-01-03", "2024-01-04")),
  product = c("A", "A", "A", "B"),
  sales = c(100, NA, 120, 150),
  region = c("North", NA, "North", "South"),
  stringsAsFactors = FALSE
)

cat("Original data:\n")
print(sales)

# Fill missing values
cat("\nAfter filling:\n")
filled_sales <- sales %>%
  fill(region, .direction = "down") %>%
  fill(sales, .direction = "down")
print(filled_sales)

# Using complete() to fill gaps
cat("\n===== USING COMPLETE() =====\n\n")

df2 <- data.frame(
  group = c("A", "A", "B"),
  time = c(1, 3, 2),
  value = c(10, 30, 20),
  stringsAsFactors = FALSE
)

cat("Incomplete data:\n")
print(df2)

cat("\nComplete with implicit values:\n")
complete_df <- df2 %>% complete(group, time, fill = list(value = 0))
print(complete_df)
```

## Best Practices

1. Choose fill direction based on context
2. Use .direction argument carefully
3. Use complete() for creating implicit missing values
