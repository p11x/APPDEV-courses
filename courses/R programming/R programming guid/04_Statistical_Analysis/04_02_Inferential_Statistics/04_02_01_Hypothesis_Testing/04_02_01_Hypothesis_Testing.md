# Hypothesis Testing

## Learning Objectives

- Understand hypothesis testing framework
- Perform t-tests and chi-square tests
- Interpret p-values

## Code Examples

```r
# Hypothesis testing
set.seed(42)

# Two sample t-test
group1 <- rnorm(30, mean = 100, sd = 15)
group2 <- rnorm(30, mean = 105, sd = 15)

cat("===== HYPOTHESIS TESTING =====\n\n")

# T-test
result <- t.test(group1, group2, var.equal = TRUE)
cat("Two-sample t-test:\n")
cat("t-statistic:", result$statistic, "\n")
cat("p-value:", result$p.value, "\n")

if (result$p.value < 0.05) {
  cat("Conclusion: Reject null hypothesis\n")
} else {
  cat("Conclusion: Fail to reject null hypothesis\n")
}
```

## Best Practices

1. State null and alternative hypotheses
2. Choose significance level (alpha)
3. Interpret p-values correctly
