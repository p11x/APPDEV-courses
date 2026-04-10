# Date and Time Manipulation in R

## Learning Objectives

- Understand date and time classes in R
- Perform date arithmetic
- Use POSIXlt and POSIXct
- Handle time zones

## Theoretical Background

### Date/Time Classes in R

1. **Date**: Dates without time (Date class)
2. **POSIXct**: Calendar time (seconds since 1970)
3. **POSIXlt**: Local time (list of components)

## Code Examples

### Standard Example: Date Operations

```r
# ===== DATE AND TIME BASICS =====

cat("===== CREATING DATES =====\n\n")

# Create dates
today <- Sys.Date()
cat("Today's date:", today, "\n")
cat("Class:", class(today), "\n")

# Create from string
date_str <- as.Date("2024-01-15")
cat("Date from string:", date_str, "\n")

# Date arithmetic
cat("\n===== DATE ARITHMETIC =====\n\n")
date1 <- as.Date("2024-01-01")
date2 <- as.Date("2024-01-15")

cat("Date difference:", date2 - date1, "days\n")

# Add days
cat("Add 10 days:", date1 + 10, "\n")

# POSIXct datetime
cat("\n===== DATETIME =====\n\n")
now <- Sys.time()
cat("Current time:", now, "\n")

# Parse datetime
datetime <- as.POSIXct("2024-01-15 10:30:00")
cat("Parsed datetime:", datetime, "\n")
```

**Output:**
```
===== CREATING DATES =====

Today's date: 2026-04-10
Class: Date
```

### Real-World Example: Employee Tenure

```r
# Real-world: Employee tenure calculation
cat("===== EMPLOYEE TENURE ANALYSIS =====\n\n")

employees <- data.frame(
  name = c("Alice", "Bob", "Charlie", "Diana"),
  hire_date = as.Date(c("2020-01-15", "2019-03-22", "2021-06-10", "2018-11-05")),
  salary = c(75000, 82000, 68000, 95000),
  stringsAsFactors = FALSE
)

today <- Sys.Date()

# Calculate tenure in days
employees$tenure_days <- as.numeric(today - employees$hire_date)

# Calculate tenure in years
employees$tenure_years <- employees$tenure_days / 365.25

# Classify tenure
employees$tenure_class <- cut(
  employees$tenure_years,
  breaks = c(0, 1, 3, 5, Inf),
  labels = c("New", "Growing", "Established", "Veteran")
)

cat("Employee tenure:\n")
print(employees[, c("name", "hire_date", "tenure_years", "tenure_class")])
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use consistent date formats (ISO 8601)
2. Consider time zones with POSIXct
3. Use difftime for duration calculations

### Common Issues

1. Date format confusion
2. Leap year handling
3. Time zone differences
