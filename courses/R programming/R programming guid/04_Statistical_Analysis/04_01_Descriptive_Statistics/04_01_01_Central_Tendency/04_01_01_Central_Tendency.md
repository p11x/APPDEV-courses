# Central Tendency Measures

## Learning Objectives

- Calculate mean, median, and mode
- Understand when to use each measure
- Apply central tendency to real data

## Theoretical Background

### Measures of Central Tendency

Central tendency describes the center of a data distribution:

1. **Mean**: Arithmetic average (sum/n)
2. **Median**: Middle value when sorted
3. **Mode**: Most frequent value

## Code Examples

### Standard Example: Central Tendency

```r
# Sample data
data <- c(10, 20, 30, 40, 50, 60, 70, 80, 90, 100)

cat("===== CENTRAL TENDENCY =====\n\n")
cat("Data:", data, "\n\n")

# Mean
cat("Mean:", mean(data), "\n")

# Median
cat("Median:", median(data), "\n")

# Mode (custom function)
get_mode <- function(x) {
  ux <- unique(x)
  ux[which.max(tabulate(match(x, ux)))]
}
cat("Mode:", get_mode(data), "\n")

# With outliers
data_outlier <- c(10, 20, 30, 40, 50, 60, 70, 80, 90, 1000)
cat("\nWith outlier (1000):\n")
cat("Mean:", mean(data_outlier), "\n")
cat("Median:", median(data_outlier), "\n")
```

### Real-World Example: Salary Analysis

```r
# Real-world: Salary central tendency
salaries <- c(35000, 40000, 45000, 50000, 55000, 60000, 
              65000, 70000, 75000, 80000, 250000)

cat("Salary data:", salaries, "\n")
cat("\nMean (average):", mean(salaries), "\n")
cat("Median (middle):", median(salaries), "\n")
```

## Best Practices

1. Use median for skewed data
2. Mean is sensitive to outliers
3. Mode for categorical data
