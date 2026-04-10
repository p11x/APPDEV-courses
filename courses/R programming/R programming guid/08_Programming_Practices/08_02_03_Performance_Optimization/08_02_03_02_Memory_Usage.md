# Memory Usage in R

## Learning Objectives

- Understand R's memory management
- Learn to monitor memory usage
- Master memory optimization techniques
- Implement memory-efficient code

## Theory

R manages memory dynamically and uses a garbage collector to free unused memory. Understanding how R allocates and uses memory helps write efficient code. Key considerations include object sizes, copying behavior, and when R makes copies of data.

## Step-by-Step Guide

### Checking Object Size

```r
# Object size
object.size(x)
object.size(dataframe)

# More detailed
library(pryr)
object_size(x)
```

### Memory Information

```r
# Memory stats
gc()

# Current memory usage
memory.size()
memory.limit()
```

### Copy-on-Modify Behavior

```r
# In R, modifications often create copies
x <- 1:10
tracemem(x)
x[1] <- 2
# R prints when copy is made
```

## Code Examples

### Efficient Data Structures

```r
# Use appropriate types
# Instead of character column for categories
df <- data.frame(
  id = 1:1000,
  category = sample(c("A", "B", "C"), 1000, replace = TRUE)
)

# Use factor to save memory
df$category <- factor(df$category)

# Or use data.table for large data
library(data.table)
DT <- data.table(
  id = 1:1000000,
  value = rnorm(1000000),
  group = sample(1:100, 1000000, replace = TRUE)
)
```

### Memory-Efficient Operations

```r
# Pre-allocate vectors
result <- numeric(1000)
for (i in 1:1000) {
  result[i] <- i^2
}

# Instead of growing vectors
# AVOID: result <- numeric(0); for (i) result <- c(result, i)

# Use colClasses when reading data
read.csv("file.csv", colClasses = c("integer", "numeric", "character"))
```

### Clear Memory

```r
# Remove unused objects
rm(large_object)
gc()

# Remove all objects
rm(list = ls())
gc()
```

### Memory Profiling

```r
# Using Rprofmem for memory allocation
Rprofmem()
your_code_here()
Rprofmem(NULL)
summaryRprofmem()
```

### Memory Monitoring

```r
# Use lsof to see memory map (Linux/Mac)
# On Windows:
tracemem(x)

# Track memory during function
memory_profile <- function() {
  before <- gc()[2, 2]
  # your code
  after <- gc()[2, 2]
  cat("Memory used:", after - before, "MB\n")
}
```

## Best Practices

1. **Remove Unused Objects**: rm() large unused objects.

2. **Pre-allocate**: Reserve space before loops.

3. **Use Factors**: For categorical data.

4. **Use data.table**: For large datasets.

5. **Specify colClasses**: When reading data.

## Exercises

1. Check memory usage of your data objects.

2. Convert character columns to factors.

3. Optimize memory use in data reading.

4. Profile memory allocation in functions.

5. Clear memory and verify.

## Additional Resources

- [Memory in R](https://cran.r-project.org/doc/manuals/R-ints.html#Memory)
- [data.table](https://data.table.dev/)
- [pryr Package](https://github.com/hadley/pryr)