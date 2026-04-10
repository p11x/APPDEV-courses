# Performance Optimization in R

This chapter covers profiling, vectorization, and performance optimization techniques in R.

## 1. Profiling R Code

Profiling identifies performance bottlenecks in your code.

### Using Rprof()

```r
# Enable profiling
Rprof()

# Run your code
result <- run_analysis()

# Stop profiling
Rprof(NULL)

# View results
summaryRprof()
# Shows time spent in each function
```

### Profiling Example

```r
# Profile a function
profile_function <- function(n) {
  data <- generate_data(n)
  result <- process_data(data)
  return(result)
}

# Enable and run
Rprof(tmp <- "profile_output.txt")
profile_function(10000)
Rprof(NULL)

# View results
summaryRprof(tmp)$by.self
summaryRprof(tmp)$by.total
unlink(tmp)

# More readable output
Rprof()
profile_function(5000)
Rprof(NULL)
summaryRprof()
```

### Using profvis

```r
# Install profvis package
install.packages("profvis")

library(profvis)

# Profile code with profvis
profvis({
  # Your code here
  data <- read.csv("large_data.csv")
  
  result <- data %>%
    group_by(category) %>%
    summarise(mean_value = mean(value))
})

# Or in RStudio: Tools > Profile > Start Profiling
# Then run code, then Stop Profiling
```

## 2. microbenchmark

The microbenchmark package provides precise timing comparisons.

### Basic Usage

```r
install.packages("microbenchmark")
library(microbenchmark)

# Compare operations
microbenchmark(
  mean(1:1000),
  sum(1:1000)/1000,
  times = 1000
)

# Results show median and quartiles
# Unit: microseconds
#             expr    min    lq   mean median    uq    max
# 1   mean(1:1000)  4.523 4.890 5.234  5.012 5.234 12.45
# 2 sum(1:1000)/1000 10.234 10.891 11.234 10.98 11.23 18.92
```

### Comparing Implementations

```r
# Compare vectorization approaches
vectorized_mean <- function(data) {
  apply(data, 1, mean)
}

# vs
true_vectorized <- function(data) {
  rowMeans(data)
}

# Benchmark
data <- matrix(rnorm(10000), nrow = 100)

microbenchmark(
  apply_approach = apply(data, 1, mean),
  rowMeans_approach = rowMeans(data),
  times = 1000
)

# Compare data frame vs matrix
df <- data.frame(x = rnorm(10000))
mat <- as.matrix(df)

microbenchmark(
  df = mean(df$x),
  matrix = mean(mat),
  times = 1000
)
```

### Plotting Results

```r
# Store and plot results
results <- microbenchmark(
  base_mean = mean(1:1000),
  sum_div = sum(1:1000)/1000,
  times = 500
)

# Auto-boxplot
plot(results)

# Manual comparison
library(ggplot2)
autoplot(results) +
  labs(title = "Performance Comparison")
```

## 3. Vectorization

Vectorization uses R's optimized vector operations instead of loops.

### Loop vs Vectorization

```r
# Slow: loop approach
sum_loop <- function(x) {
  total <- 0
  for (i in seq_along(x)) {
    total <- total + x[i]
  }
  total
}

# Fast: vectorized
sum_vectorized <- function(x) {
  sum(x)
}

# Compare
x <- rnorm(10000)
microbenchmark(
  loop = sum_loop(x),
  vectorized = sum_vectorized(x),
  times = 100
)
# Vectorized is typically 10-100x faster
```

### More Vectorization Examples

```r
# Instead of:
squared_loop <- function(x) {
  result <- numeric(length(x))
  for (i in seq_along(x)) {
    result[i] <- x[i]^2
  }
  result
}

# Use:
squared_vec <- function(x) {
  x^2
}

# Apply family (still faster than loops)
squared_apply <- function(x) {
  sapply(x, function(val) val^2)
}

# Benchmark
x <- 1:10000
microbenchmark(
  loop = squared_loop(x),
  sapply = squared_apply(x),
  vectorized = squared_vec(x),
  times = 100
)
```

### Vectorized Conditionals

```r
# Instead of ifelse in loop:
ifelse_loop <- function(x) {
  result <- character(length(x))
  for (i in seq_along(x)) {
    if (x[i] > 0) {
      result[i] <- "positive"
    } else {
      result[i] <- "non-positive"
    }
  }
  result
}

# Use ifelse or comparison:
ifelse_vectorized <- function(x) {
  ifelse(x > 0, "positive", "non-positive")
}

# Even better: logical to character
ifelse_best <- function(x) {
  c("non-positive", "positive")[(x > 0) + 1]
}

# Benchmark
x <- rnorm(100000)
microbenchmark(
  loop = ifelse_loop(x),
  ifelse = ifelse_vectorized(x),
  logical = ifelse_best(x),
  times = 100
)
```

## 4. Efficient Data Structures

### Choosing Right Structures

```r
# Data frame vs matrix
# Matrix for homogeneous data, same type
# Data frame for heterogeneous data

# Numeric vector vs list
# Use vectors when possible

# Avoid growing objects
# Bad:
grow_vector <- function(n) {
  result <- NULL
  for (i in 1:n) {
    result <- c(result, i)  # Grows each iteration
  }
  result
}

# Good:
pre_allocate <- function(n) {
  result <- numeric(n)  # Pre-allocate
  for (i in 1:n) {
    result[i] <- i
  }
  result
}

# Best of all:
use_seq <- function(n) {
  1:n
}

# Benchmark
n <- 10000
microbenchmark(
  grow = grow_vector(n),
  allocate = pre_allocate(n),
  seq = use_seq(n),
  times = 100
)
```

### Using Matrix Operations

```r
# Instead of apply
row_sums_apply <- function(m) {
  apply(m, 1, sum)
}

# Use rowSums
row_sums_matrix <- function(m) {
  rowSums(m)
}

# Benchmark
m <- matrix(rnorm(10000), nrow = 100)

microbenchmark(
  apply = row_sums_apply(m),
  rowSums = row_sums_matrix(m),
  times = 1000
)

# Similar for:
colSums(m)
colMeans(m)
rowMeans(m)
rowMeans(m)
```

## 5. Memory and Performance

### Understanding Memory

```r
# Check size of objects
object.size(mtcars)
# 1728 bytes

# More detailed
lobstr::obj_size(mtcars)

# Check memory in use
gc()

# Memory limits
# By default R uses all available memory
```

### Memory-Efficient Operations

```r
# Copy-on-modify behavior
# Each modification creates copy

# In-place modification (avoid)
modify_copy <- function(df) {
  df$new_col <- df$col * 2
  return(df)
}

# Use data.table for efficiency
library(data.table)

# data.table modifies by reference
modify_ref <- function(dt) {
  dt[, new_col := col * 2]
}

# Benchmark
library(microbenchmark)
df <- data.frame(col = rnorm(100000))
dt <- as.data.table(df)

microbenchmark(
  df = modify_copy(df),
  dt = modify_ref(dt),
  times = 100
)
```

### Avoiding Unnecessary Copies

```r
# Create function that modifies in place
add_column <- function(dt, col_name, value) {
  set(dt, j = col_name, value = value)
}

# Compare with assignment
add_column_new <- function(dt, col_name, value) {
  dt[[col_name]] <- value
  dt
}

# Both cause copies in data.frame
# But set() is more efficient with data.table
```

## 6. Additional Optimizations

### Parallel Processing

```r
# Install future and furrr
install.packages("future")
install.packages("furrr")

library(furrr)

plan(multisession)

# Parallel map
平行_map <- function(data) {
  future_map(data, slow_function, .options = furrr_options(seed = TRUE))
}

# Or parallel::mclapply on Linux/Mac
library(parallel)

mclapply(1:10, function(x) {
  Sys.sleep(0.1)  # Simulate work
  x * 2
}, mc.cores = 2)
```

### Caching Results

```r
# memoise for expensive functions
install.packages("memoise")
library(memoise)

# Cache expensive computation
expensive_func <- memoise(function(x) {
  Sys.sleep(1)  # Simulate delay
  mean(x)
})

# First call (slow)
time1 <- system.time(expensive_func(1:1000))

# Second call with same args (fast - cached)
time2 <- system.time(expensive_func(1:1000))

# Clear cache
forget(expensive_func)
```

### Byte Code Compilation

```r
# Enable JIT compilation
enableJIT(3)  # Level 0-3

# Compile function
compile <- function() {}

# Check if function is compiled
isCompiled(mean)
is.primitive(mean)

# Byte-compiled functions load faster
# Are internally stored as bytecode
```

## Summary

- Use `Rprof()` or `profvis` for profiling
- Use `microbenchmark` for precise comparisons
- Vectorize operations whenever possible
- Pre-allocate rather than grow objects
- Use data.table for large datasets
- Consider parallel processing
- Cache expensive computations
- Enable JIT compilation for large scripts