# Data Reshaping with tidyr

## Learning Objectives

- Understand tidy data concepts
- Reshape data between wide and long formats
- Use gather and spread functions
- Handle missing data

## Theoretical Background

### What is Tidy Data?

Tidy data has:
1. Each variable in its own column
2. Each observation in its own row
3. Each value in its own cell

### Reshaping Functions

1. **gather()** - Wide to long (columns to rows)
2. **spread()** - Long to wide (rows to columns)
3. **pivot_longer()** - Modern gather
4. **pivot_wider()** - Modern spread

## Code Examples

### Standard Example: gather and spread

```r
library(tidyr)

cat("===== GATHER EXAMPLE =====\n\n")

# Wide format data
wide <- data.frame(
  person = c("Alice", "Bob"),
  Q1 = c(100, 200),
  Q2 = c(150, 250),
  Q3 = c(120, 220),
  stringsAsFactors = FALSE
)

cat("Wide format:\n")
print(wide)

# Gather to long format
cat("\nLong format (gathered):\n")
long <- gather(wide, quarter, sales, Q1:Q3)
print(long)

cat("\n===== SPREAD EXAMPLE =====\n\n")

# Long format
long2 <- data.frame(
  person = c("Alice", "Alice", "Bob", "Bob"),
  quarter = c("Q1", "Q2", "Q1", "Q2"),
  sales = c(100, 150, 200, 250),
  stringsAsFactors = FALSE
)

cat("Long format:\n")
print(long2)

# Spread to wide format
cat("\nWide format (spread):\n")
wide2 <- spread(long2, quarter, sales)
print(wide2)
```

**Output:**
```
===== GATHER EXAMPLE =====

Wide format:
  person  Q1  Q2  Q3
1  Alice 100 150 120
...
```

### Real-World Example: Survey Data Reshaping

```r
# Real-world: Reshaping survey response data
cat("===== SURVEY DATA RESHAPING =====\n\n")

# Survey in wide format
survey_wide <- data.frame(
  respondent_id = c(1, 2, 3),
  Q1_satisfaction = c(4, 5, 3),
  Q2_quality = c(3, 4, 4),
  Q3_value = c(5, 3, 4),
  Q4_recommend = c(4, 5, 5),
  stringsAsFactors = FALSE
)

cat("Survey wide format:\n")
print(survey_wide)

# Reshape to long
cat("\nReshaped to long format:\n")
survey_long <- survey_wide %>%
  gather(question, response, -respondent_id)
print(survey_long)

# Analysis by question
cat("\nAverage response by question:\n")
summary <- survey_long %>%
  group_by(question) %>%
  summarise(avg_response = mean(response))
print(summary)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use gather/spread for simple transformations
2. Use pivot_longer/pivot_wider for complex cases
3. Ensure unique identifiers before spreading

### Common Issues

1. Duplicate column names
2. Missing identifiers
3. Type conversion issues
