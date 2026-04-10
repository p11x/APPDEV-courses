# Central Limit Theorem

## Learning Objectives

- Understand the Central Limit Theorem
- Verify CLT through simulation
- Apply to sampling distributions

## Code Examples

```r
# Central Limit Theorem demonstration

cat("===== CENTRAL LIMIT THEOREM =====\n\n")

# Population (uniform)
set.seed(42)
pop <- runif(10000, min = 0, max = 1)

cat("Population mean:", mean(pop), "\n")
cat("Population sd:", sd(pop), "\n")

# Sample means from different sample sizes
cat("\nSampling distributions:\n")

for (n in c(5, 30, 100)) {
  sample_means <- replicate(1000, mean(sample(pop, n, replace = TRUE)))
  cat(sprintf("  n = %3d: mean = %.4f, sd = %.4f\n",
              n, mean(sample_means), sd(sample_means)))
}

cat("\nConclusion: Sample means approach normal distribution\n")
cat("regardless of population distribution\n")
```

## Best Practices

1. CLT applies to sum/mean of independent random variables
2. Sample size n >= 30 often sufficient
