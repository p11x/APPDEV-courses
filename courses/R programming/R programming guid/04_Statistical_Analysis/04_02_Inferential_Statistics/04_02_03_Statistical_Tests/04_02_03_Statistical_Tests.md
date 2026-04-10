# Statistical Tests

## Learning Objectives

- Perform common statistical tests
- Choose appropriate tests for data

## Code Examples

```r
# Common statistical tests
set.seed(42)

cat("===== STATISTICAL TESTS =====\n\n")

# 1. One-sample t-test
data <- rnorm(50, mean = 100, sd = 15)
cat("1. One-sample t-test:\n")
print(t.test(data, mu = 100))

# 2. Paired t-test
before <- rnorm(30, mean = 100, sd = 15)
after <- before + rnorm(30, mean = 2, sd = 2)
cat("\n2. Paired t-test:\n")
print(t.test(before, after, paired = TRUE))

# 3. Correlation test
x <- rnorm(50)
y <- x + rnorm(50, mean = 0.2, sd = 0.5)
cat("\n3. Correlation test:\n")
print(cor.test(x, y))

# 4. Chi-square test
table_data <- matrix(c(10, 20, 30, 40), nrow = 2)
cat("\n4. Chi-square test:\n")
print(chisq.test(table_data))
```

## Best Practices

1. Check test assumptions
2. Use appropriate test for data type
