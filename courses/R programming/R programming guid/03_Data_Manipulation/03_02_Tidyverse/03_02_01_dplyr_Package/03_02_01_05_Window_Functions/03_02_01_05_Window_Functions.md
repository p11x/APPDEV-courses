# Window Functions in dplyr

## Learning Objectives

- Understand window functions vs aggregate functions
- Use ranking functions
- Use lead and lag functions
- Use cumulative functions

## Theoretical Background

### Window Functions

Window functions perform calculations across rows while maintaining the row structure:

- **Aggregate**: Reduces to single value per group
- **Window**: Keeps row structure, calculates per row

### Window Function Types

1. **Ranking**: row_number, min_rank, dense_rank
2. **Offset**: lead, lag
3. **Cumulative**: cumsum, cummean
4. **Distribution**: percent_rank, cume_dist

## Code Examples

### Standard Example: Window Functions

```r
library(dplyr)

cat("===== WINDOW FUNCTION EXAMPLES =====\n\n")

df <- data.frame(
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  salary = c(50000, 60000, 55000, 70000, 45000)
)

# Ranking
cat("Ranking by salary:\n")
ranked <- df %>%
  mutate(
    rank = row_number(salary),
    rank_dense = dense_rank(salary),
    rank_min = min_rank(salary)
  ) %>%
  arrange(desc(salary))
print(ranked)

# Lead and lag
cat("\nLead and lag:\n")
shifted <- df %>%
  arrange(salary) %>%
  mutate(
    lag_salary = lag(salary),
    lead_salary = lead(salary)
  )
print(shifted)

# Cumulative
cat("\nCumulative salary:\n")
cumdf <- df %>%
  arrange(salary) %>%
  mutate(
    cumsum = cumsum(salary),
    cummean = cummean(salary)
  )
print(cumdf)
```

**Output:**
```
===== WINDOW FUNCTION EXAMPLES =====

Ranking by salary:
     name salary rank rank_dense rank_min
1  Diana  70000    1         1        1
...
```

### Real-World Example: Sales Analysis

```r
# Real-world: Sales over time with window functions
cat("===== SALES TREND ANALYSIS =====\n\n")

sales <- data.frame(
  month = 1:12,
  sales = c(100, 120, 110, 150, 140, 160, 180, 170, 190, 200, 220, 250)
)

cat("Sales with trend analysis:\n")
result <- sales %>%
  mutate(
    prev_month = lag(sales),
    change = sales - prev_month,
    pct_change = (sales - prev_month) / prev_month * 100,
    running_total = cumsum(sales),
    avg_to_date = cummean(sales),
    rank = min_rank(-sales)
  )
print(result)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Always use window functions with explicit grouping
2. Use arrange() before window functions
3. Use partition for group-wise operations

### Common Issues

1. Forgetting arrange before ranking
2. NA propagation with lead/lag
