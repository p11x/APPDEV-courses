# Select, Filter, Mutate in dplyr

## Learning Objectives

- Master dplyr verbs for data manipulation
- Use select() to choose columns
- Use filter() to choose rows
- Use mutate() to create new columns

## Theoretical Background

### The dplyr Package

dplyr is part of the tidyverse and provides consistent grammar for data manipulation:

1. **select()** - Choose columns
2. **filter()** - Choose rows based on conditions
3. **mutate()** - Create new columns
4. **transmute()** - Create and keep only new columns

## Code Examples

### Standard Example: dplyr Basics

```r
# ===== DPLYR BASICS =====

# Load dplyr (comes with tidyverse)
library(dplyr)

cat("===== SELECT EXAMPLES =====\n\n")

# Sample data
df <- data.frame(
  id = 1:5,
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  age = c(25, 30, 28, 35, 22),
  salary = c(50000, 60000, 55000, 70000, 45000),
  department = c("Sales", "Engineering", "Sales", "Engineering", "HR"),
  stringsAsFactors = FALSE
)

# Select columns by name
cat("Select name and salary:\n")
selected <- select(df, name, salary)
print(head(selected))

# Select columns by range
cat("\nSelect id through salary:\n")
selected2 <- select(df, id:salary)
print(head(selected2))

# Select with helpers
cat("\nSelect starts with 's':\n")
selected3 <- select(df, starts_with("s"))
print(head(selected3))

cat("\n===== FILTER EXAMPLES =====\n\n")

# Filter rows
cat("Age > 25:\n")
filtered <- filter(df, age > 25)
print(filtered)

# Multiple conditions
cat("\nAge > 25 AND salary > 50000:\n")
filtered2 <- filter(df, age > 25 & salary > 50000)
print(filtered2)

# OR condition
cat("\nDepartment is Sales OR Engineering:\n")
filtered3 <- filter(df, department %in% c("Sales", "Engineering"))
print(filtered3)

cat("\n===== MUTATE EXAMPLES =====\n\n")

# Create new column
mutated <- mutate(df, salary_bonus = salary * 0.1)
print(head(mutated))

# Multiple new columns
cat("\nMultiple mutations:\n")
mutated2 <- mutate(df,
                   salary_bonus = salary * 0.1,
                   salary_after_bonus = salary + salary_bonus)
print(head(mutated2))
```

**Output:**
```
===== SELECT EXAMPLES =====

Select name and salary:
     name salary
1  Alice  50000
...
```

### Real-World Example: Sales Analysis with dplyr

```r
# Real-world: Sales data analysis with dplyr
cat("===== SALES ANALYSIS WITH DPLYR =====\n\n")

# Create sample sales data
sales <- data.frame(
  order_id = 1:20,
  customer = sample(c("Alice", "Bob", "Charlie", "Diana", "Edward"), 20, replace = TRUE),
  product = sample(c("Laptop", "Mouse", "Keyboard", "Monitor"), 20, replace = TRUE),
  quantity = sample(1:5, 20, replace = TRUE),
  unit_price = sample(c(999, 29, 79, 349), 20, replace = TRUE),
  region = sample(c("North", "South", "East", "West"), 20, replace = TRUE),
  date = as.Date("2024-01-01") + sample(0:90, 20),
  stringsAsFactors = FALSE
)

# Calculate total
sales <- mutate(sales, total = quantity * unit_price)

cat("Original data (first 10 rows):\n")
print(head(sales, 10))

# Filter and select
cat("\nHigh-value orders (total > 500):\n")
high_value <- sales %>%
  filter(total > 500) %>%
  select(order_id, customer, product, total)
print(high_value)

# Calculate summary by product
cat("\nSales by product:\n")
product_summary <- sales %>%
  group_by(product) %>%
  summarise(
    total_revenue = sum(total),
    avg_order = mean(total),
    num_orders = n()
  ) %>%
  arrange(desc(total_revenue))
print(product_summary)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use the pipe (%>%) for readability
2. Use select_helpers (starts_with, ends_with, contains)
3. Chain operations in logical order

### Common Issues

1. Forgetting to install/load tidyverse
2. Column name typos (non-standard evaluation)
