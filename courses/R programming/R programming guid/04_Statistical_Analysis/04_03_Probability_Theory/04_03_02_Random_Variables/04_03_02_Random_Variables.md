# Random Variables

## Learning Objectives

- Understand discrete and continuous random variables
- Calculate expected values and variance

## Code Examples

```r
# Random variables

cat("===== RANDOM VARIABLES =====\n\n")

# Discrete random variable - Binomial
cat("1. Discrete: Binomial Distribution\n")
n <- 10
p <- 0.5
expected <- n * p
variance <- n * p * (1 - p)
cat("  Expected value (np):", expected, "\n")
cat("  Variance (np(1-p)):", variance, "\n")

# Simulate
samples <- rbinom(10000, size = n, prob = p)
cat("  Simulated mean:", mean(samples), "\n")
cat("  Simulated var:", var(samples), "\n")

# Continuous random variable - Normal
cat("\n2. Continuous: Normal Distribution\n")
expected_norm <- 50
sd_norm <- 10
samples_norm <- rnorm(10000, expected_norm, sd_norm)
cat("  Simulated mean:", mean(samples_norm), "\n")
cat("  Simulated sd:", sd(samples_norm), "\n")
```

## Best Practices

1. Use simulation to verify theoretical values
2. Understand expectation vs sample mean
