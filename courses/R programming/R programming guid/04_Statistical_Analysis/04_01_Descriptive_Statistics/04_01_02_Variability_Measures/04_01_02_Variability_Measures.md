# Variability Measures

## Learning Objectives

- Calculate variance and standard deviation
- Understand range and IQR
- Measure data spread

## Code Examples

```r
# Variability measures
data <- c(10, 20, 30, 40, 50, 60, 70)

cat("===== VARIABILITY =====\n\n")
cat("Range:", range(data), "\n")
cat("IQR:", IQR(data), "\n")
cat("Variance:", var(data), "\n")
cat("Standard Deviation:", sd(data), "\n")

# Coefficient of variation
cv <- sd(data) / mean(data) * 100
cat("Coefficient of Variation:", cv, "%\n")
```

## Best Practices

1. Use IQR for robust measures
2. SD for normal distributions
