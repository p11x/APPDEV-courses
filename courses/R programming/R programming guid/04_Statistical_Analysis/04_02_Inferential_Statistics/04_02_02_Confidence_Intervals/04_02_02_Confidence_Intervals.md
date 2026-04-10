# Confidence Intervals

## Learning Objectives

- Calculate and interpret confidence intervals
- Understand the relationship to hypothesis testing

## Code Examples

```r
# Confidence intervals
set.seed(42)
data <- rnorm(100, mean = 50, sd = 10)

cat("===== CONFIDENCE INTERVALS =====\n\n")

# T-based confidence interval
result <- t.test(data, conf.level = 0.95)
cat("95% Confidence Interval:\n")
cat("Lower:", result$conf.int[1], "\n")
cat("Upper:", result$conf.int[2], "\n")

# Manual calculation
n <- length(data)
mean_val <- mean(data)
se <- sd(data) / sqrt(n)
alpha <- 0.05
t_crit <- qt(1 - alpha/2, df = n-1)

cat("\nManual calculation:\n")
cat("Mean:", mean_val, "\n")
cat("CI:", mean_val - t_crit * se, "to", mean_val + t_crit * se, "\n")
```

## Best Practices

1. Use appropriate confidence level
2. Understand CI vs hypothesis test relationship
