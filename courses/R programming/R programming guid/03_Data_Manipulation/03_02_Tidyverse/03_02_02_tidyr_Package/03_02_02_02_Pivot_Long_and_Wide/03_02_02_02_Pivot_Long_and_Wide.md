# Pivot Long and Wide with tidyr

## Learning Objectives

- Master pivot_longer() and pivot_wider()
- Handle complex reshaping scenarios
- Use names_sep and names_ptype arguments

## Code Examples

### Standard Example: Pivot Functions

```r
library(tidyr)

cat("===== PIVOT_LONGER =====\n\n")

df <- data.frame(
  id = 1:2,
  name = c("Alice", "Bob"),
  test1 = c(85, 90),
  test2 = c(78, 92),
  test3 = c(88, 85),
  stringsAsFactors = FALSE
)

cat("Wide format:\n")
print(df)

cat("\nLong format (pivot_longer):\n")
long <- pivot_longer(df, cols = starts_with("test"), 
                    names_to = "test_num", values_to = "score")
print(long)

cat("\n===== PIVOT_WIDER =====\n\n")

df_long <- data.frame(
  id = c(1, 1, 2, 2),
  type = c("pre", "post", "pre", "post"),
  score = c(75, 85, 80, 88),
  stringsAsFactors = FALSE
)

cat("Long format:\n")
print(df_long)

cat("\nWide format (pivot_wider):\n")
wide <- pivot_wider(df_long, names_from = type, values_from = score)
print(wide)
```

### Real-World Example: Time Series Reshaping

```r
# Real-world: Reshaping time series data
cat("===== TIME SERIES RESHAPING =====\n\n")

# Sample time series in wide format
ts_wide <- data.frame(
  region = c("North", "South", "East"),
  Jan = c(100, 150, 120),
  Feb = c(110, 160, 130),
  Mar = c(120, 170, 140),
  stringsAsFactors = FALSE
)

cat("Wide format:\n")
print(ts_wide)

# Reshape for time series analysis
cat("\nLong format for analysis:\n")
ts_long <- ts_wide %>%
  pivot_longer(-region, names_to = "month", values_to = "sales")
print(ts_long)

# Add month number
month_map <- c("Jan" = 1, "Feb" = 2, "Mar" = 3)
ts_long$month_num <- month_map[ts_long$month]

cat("\nWith month numbers:\n")
print(ts_long)
```

## Best Practices

1. Use names_sep for automatic column name parsing
2. Use values_ptype for type safety
3. Handle duplicates with values_fn
