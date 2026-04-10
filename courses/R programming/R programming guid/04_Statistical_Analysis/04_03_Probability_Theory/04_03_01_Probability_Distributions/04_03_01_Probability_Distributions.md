# Probability Distributions in R

## Learning Objectives

- Understand probability distributions in R
- Generate random samples
- Calculate probabilities and quantiles

## Code Examples

```r
# Probability distributions in R

cat("===== PROBABILITY DISTRIBUTIONS =====\n\n")

# Normal distribution
cat("1. Normal Distribution:\n")
cat("  dnorm (density):", dnorm(0, mean = 0, sd = 1), "\n")
cat("  pnorm (CDF):", pnorm(0, mean = 0, sd = 1), "\n")
cat("  qnorm (quantile):", qnorm(0.5, mean = 0, sd = 1), "\n")
cat("  rnorm (random):", rnorm(5, mean = 0, sd = 1), "\n")

# Common distributions
cat("\n2. Other distributions:\n")
cat("  Binomial (r):", rbinom(5, size = 10, prob = 0.5), "\n")
cat("  Poisson (r):", rpois(5, lambda = 3), "\n")
cat("  Exponential (r):", rexp(5, rate = 1), "\n")
cat("  Uniform (r):", runif(5, min = 0, max = 1), "\n")

# Plotting distributions
cat("\n3. Plotting normal distribution:\n")
x <- seq(-3, 3, length.out = 100)
y <- dnorm(x)
cat("  Created", length(x), "points for plot\n")
```

## Best Practices

1. Use d/p/q/r prefixes for density/CDF/quantile/random
2. Set seed for reproducibility
