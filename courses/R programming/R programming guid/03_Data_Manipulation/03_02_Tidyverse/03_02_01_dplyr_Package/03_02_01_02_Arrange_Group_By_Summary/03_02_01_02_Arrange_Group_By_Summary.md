# Arrange, Group By, Summary in dplyr

## Learning Objectives

- Use arrange() for sorting
- Use group_by() for grouping
- Use summarise() for aggregation
- Combine with other dplyr verbs

## Theoretical Background

### Aggregation Verbs in dplyr

1. **arrange()** - Sort rows by columns
2. **group_by()** - Group data for operations
3. **summarise()** - Create summary statistics
4. **summarise_all()** / **summarise_at()** - Apply to columns

## Code Examples

### Standard Example: Arrange, Group By, Summary

```r
# Load dplyr
library(dplyr)

cat("===== ARRANGE EXAMPLES =====\n\n")

df <- data.frame(
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  salary = c(50000, 60000, 55000, 70000, 45000),
  age = c(25, 30, 28, 35, 22),
  stringsAsFactors = FALSE
)

# Arrange by single column
cat("Arrange by salary (ascending):\n")
arranged <- arrange(df, salary)
print(arranged)

# Arrange descending
cat("\nArrange by salary (descending):\n")
arranged2 <- arrange(df, desc(salary))
print(arranged2)

# Arrange by multiple columns
cat("\nArrange by salary desc, then age:\n")
arranged3 <- arrange(df, desc(salary), age)
print(arranged3)

cat("\n===== GROUP BY AND SUMMARISE =====\n\n")

# Sample data with groups
sales <- data.frame(
  product = c(rep("A", 4), rep("B", 4), rep("C", 4)),
  region = rep(c("North", "South", "East", "West"), 3),
  sales = c(100, 150, 120, 180, 200, 250, 220, 280, 150, 200, 180, 240)
)

cat("Grouped by product:\n")
grouped <- group_by(sales, product)
print(summarise(grouped, total_sales = sum(sales)))

cat("\nMultiple summaries:\n")
summary <- sales %>%
  group_by(product) %>%
  summarise(
    total = sum(sales),
    average = mean(sales),
    max = max(sales),
    min = min(sales),
    n = n()
  )
print(summary)
```

**Output:**
```
===== ARRANGE EXAMPLES =====

Arrange by salary (ascending):
     name salary age
1 Edward  45000  22
...
```

### Real-World Example: Regional Sales Report

```r
# Real-world: Regional sales summary
cat("===== REGIONAL SALES REPORT =====\n\n")

sales <- data.frame(
  month = rep(c("Jan", "Feb", "Mar"), each = 8),
  region = rep(rep(c("North", "South", "East", "West"), each = 2), 3),
  product = rep(c("ProductA", "ProductB"), 12),
  sales = runif(24, 100, 500),
  units = sample(10:100, 24, replace = TRUE)
)

# Monthly summary by region
cat("Monthly sales by region:\n")
monthly_summary <- sales %>%
  group_by(month, region) %>%
  summarise(
    total_sales = sum(sales),
    total_units = sum(units),
    avg_price = weighted.mean(sales, units)
  ) %>%
  arrange(month, desc(total_sales))
print(monthly_summary)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Always arrange by groups before summarising
2. Use ungroup() after grouped operations
3. Keep group_by() close to summarise()

### Common Issues

1. Forgetting to group before summarise
2. Groups persisting unexpectedly
