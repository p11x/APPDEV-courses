# Profiling Code Performance in R

## Learning Objectives

- Learn to profile R code for performance
- Master Rprof() and related functions
- Interpret profiling results
- Identify performance bottlenecks

## Theory

Code profiling measures where execution time is spent, helping identify bottlenecks. R provides Rprof() for line-by-line profiling and_summary() for results. The profviz package provides visual profiles. Profiling should be done on realistic workloads to identify actual bottlenecks.

## Step-by-Step Guide

### Basic Profiling with Rprof()

```r
# Enable profiling
Rprof()

# Run code to profile
slow_function()

# Stop profiling
Rprof(NULL)

# View results
summaryRprof()
```

### Profiling Options

```r
# Profile with memory allocation
Rprof(memory.profiling = TRUE)

# Profile intervals (in seconds)
Rprof(interval = 0.01)

# Output to file
Rprof(tmp <- "profile.out")
```

### Interpreting Results

```r
summaryRprof()
# $by.self shows time in self
# $by.total shows total time
# $sample.interval shows sampling frequency
# $sampling.time shows total time
```

## Code Examples

### Profiling a Function

```r
profile_function <- function(n = 1000) {
  Rprof()
  
  # Your code here
  data <- rnorm(n)
  result <- mean(data)
  for (i in 1:100) {
    result <- result + sd(data)
  }
  
  Rprof(NULL)
  summaryRprof()
}
```

### Using profvis

```r
# Install and load profviz
library(profvis)

# Profile with visual output
profvis({
  # Your code here
  data <- rnorm(10000)
  result <- data |>
    cumsum() |>
    mean()
})
```

### Identifying Bottlenecks

```r
# Profile to find slow parts
slow_code <- function() {
  Rprof()
  
  # Part 1
  df <- data.frame(x = rnorm(1000))
  
  # Part 2 - likely bottleneck
  result <- numeric(1000)
  for (i in 1:1000) {
    result[i] <- mean(rnorm(100))
  }
  
  Rprof(NULL)
  summaryRprof()$by.self
}
```

### Compare Implementations

```r
compare_versions <- function() {
  library(microbenchmark)
  
  # Vectorized version
  vec_time <- microbenchmark({
    x <- rnorm(10000)
    x^2
  })
  
  # Loop version
  loop_time <- microbenchmark({
    x <- rnorm(10000)
    for (i in seq_along(x)) {
      x[i] <- x[i]^2
    }
  })
  
  list(vectorized = vec_time, loop = loop_time)
}
```

## Best Practices

1. **Profile First**: Don't optimize without profiling.

2. **Measure Real Workload**: Profile on realistic data sizes.

3. **Remove Variability**: Run multiple times and average.

4. **Focus on Hotspots**: Concentrate on identified bottlenecks.

5. **Re-profile After Changes**: Verify improvements.

## Exercises

1. Profile your slowest functions.

2. Use profvis for visual profiling.

3. Compare implementations with microbenchmark.

4. Identify and optimize hotspots.

5. Verify improvements by re-profiling.

## Additional Resources

- [Rprof Documentation](https://stat.ethz.ch/R-manual/R-devel/library/utils/html/Rprof.html)
- [profviz Package](https://r-lib.github.io/profvis/)
- [microbenchmark Package](https://cran.r-project.org/web/packages/microbenchmark/)