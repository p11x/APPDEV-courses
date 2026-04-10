# Vectorization in R

## Learning Objectives

- Understand vectorization principles
- Learn to replace loops with vectorized operations
- Master apply family functions
- Implement efficient R code

## Theory

Vectorization is the process of replacing loops with vector operations. R is designed to work with vectors efficiently, and vectorized operations are typically much faster than explicit loops. Understanding when and how to vectorize code is essential for writing efficient R programs.

## Step-by-Step Guide

### Why Vectorize?

Vectorized operations are faster because they use optimized C code internally instead of R's interpreter:

```r
# Slow: explicit loop
x <- 1:1000000
result <- numeric(1000000)
for (i in seq_along(x)) {
  result[i] <- x[i]^2
}

# Fast: vectorized
x <- 1:1000000
result <- x^2
```

### Replacing Common Loops

```r
# Sum
sum_loop <- function(x) {
  total <- 0
  for (i in x) total <- total + i
  total
}
sum_vec <- sum  # Use built-in sum()

# Mean
mean_loop <- function(x) {
  total <- 0
  for (i in x) total <- total + i
  total / length(x)
}
mean_vec <- mean  # Use built-in mean()

# Maximum
max_loop <- function(x) {
  m <- x[1]
  for (i in x) if (i > m) m <- i
  m
}
max_vec <- max  # Use built-in max()
```

### Using Vectorized Functions

Many base R functions are vectorized:

```r
# Arithmetic operations
1:10 + 1        # Element-wise addition
1:10 * 2        # Element-wise multiplication
(1:10)^2        # Element-wise power

# Comparison
1:10 > 5        # Returns logical vector

# Mathematical functions
sqrt(1:10)      # Square root of each element
log(1:10)       # Log of each element
exp(1:10)       # Exponential of each element
```

## Code Examples

### If-Else Vectorization

```r
# Vectorized if-else using ifelse()
x <- -3:3
ifelse(x > 0, "positive", "non-positive")

# Or using dplyr::if_else() for type safety
library(dplyr)
dplyr::if_else(x > 0, "positive", "non-positive")
```

### Replace loops with apply

```r
# Apply over rows
data <- matrix(rnorm(100), nrow = 10)

# Instead of loop
row_means <- numeric(10)
for (i in 1:10) {
  row_means[i] <- mean(data[i, ])
}

# Use apply
row_means <- apply(data, 1, mean)

# Apply over columns
col_means <- apply(data, 2, mean)
```

### Using pmax and pmin

```r
# Parallel max/min
pmax(1:5, c(0, 2, 4, 6, 8))  # Returns: 1 2 4 6 8
pmin(1:5, c(0, 2, 4, 6, 8))  # Returns: 0 2 4 6 8
```

### rowSums and colSums

```r
# Efficient row/column sums
data <- matrix(1:100, nrow = 10)

row_sums <- rowSums(data)
col_sums <- colSums(data)

# rowMeans and colMeans
row_means <- rowMeans(data)
col_means <- colMeans(data)
```

### Vectorized Data Frame Operations

```r
library(dplyr)

# Create sample data
df <- data.frame(
  x = rnorm(1000),
  group = sample(letters[1:3], 1000, replace = TRUE)
)

# Group-wise operations
df |>
  group_by(group) |>
  summarize(
    mean_x = mean(x),
    sd_x = sd(x),
    n = n()
  )
```

## Best Practices

1. **Prefer Built-ins**: Use sum(), mean(), etc. over loops.

2. **Use apply Family**: lapply, sapply, vapply for lists.

3. **dplyr for Data Frames**: Use dplyr verbs for data manipulation.

4. **Test Performance**: Use microbenchmark to compare.

5. **Keep Code Readable**: Don't over-vectorize at expense of clarity.

## Exercises

1. Replace loop code with vectorized operations.

2. Use apply family functions appropriately.

3. Test performance differences with microbenchmark.

4. Vectorize conditional logic.

5. Optimize a slow function.

## Additional Resources

- [R Inferno - Vectorization](https://www.burns-stat.com/the-r-inferno/)
- [Vectorization in R](https://radford.github.io/vectorization.html)
- [dplyr](https://dplyr.tidyverse.org/)