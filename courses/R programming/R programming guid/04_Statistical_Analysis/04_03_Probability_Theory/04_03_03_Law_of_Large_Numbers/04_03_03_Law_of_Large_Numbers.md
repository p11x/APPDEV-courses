# Law of Large Numbers

## Learning Objectives

- Understand the Law of Large Numbers
- Verify through simulation
- Apply to real-world scenarios

## Code Examples

```r
# Law of Large Numbers demonstration

cat("===== LAW OF LARGE NUMBERS =====\n\n")

# Simulate rolling a die
set.seed(42)
n_trials <- c(10, 100, 1000, 10000, 100000)

cat("Rolling a fair die:\n")
for (n in n_trials) {
  rolls <- sample(1:6, n, replace = TRUE)
  mean_val <- mean(rolls)
  cat(sprintf("  n = %6d: mean = %.4f (expected = 3.5)\n", 
              n, mean_val))
}

# Visualization of convergence
cat("\nConvergence visualization:\n")
rolls <- sample(1:6, 10000, replace = TRUE)
running_mean <- cumsum(rolls) / seq_along(rolls)
cat("  Initial mean:", running_mean[1], "\n")
cat("  Final mean:", running_mean[10000], "\n")
```

## Best Practices

1. More samples = closer to expected value
2. Use set.seed for reproducibility
