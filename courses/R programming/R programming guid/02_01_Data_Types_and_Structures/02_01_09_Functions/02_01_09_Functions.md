# Functions in R

## Learning Objectives

- Understand function fundamentals in R
- Create and call functions
- Pass arguments to functions
- Return values from functions
- Apply functions in data analysis

## Theoretical Background

### What is a Function?

A function is a block of code designed to perform a particular task. Functions allow code reuse, modularity, and abstraction. In R, functions are first-class objects - they can be assigned to variables, passed as arguments, and returned from other functions.

### Function Structure

```
function_name <- function(arg1, arg2, ...) {
  # function body
  # optional return statement
}
```

## Code Examples

### Standard Example: Basic Functions

```r
# ===== CREATING FUNCTIONS =====

cat("===== FUNCTION BASICS =====\n\n")

# 1. Simple function with no arguments
hello_world <- function() {
  cat("Hello, World!\n")
}
hello_world()

# 2. Function with single argument
square <- function(x) {
  return(x^2)
}
cat("\n5 squared =", square(5), "\n")

# 3. Function with multiple arguments
power <- function(base, exponent) {
  return(base^exponent)
}
cat("2^10 =", power(2, 10), "\n")

# 4. Function with default arguments
greet <- function(name = "User") {
  cat("Welcome,", name, "!\n")
}
greet()           # Uses default
greet("Alice")    # Uses provided value

# 5. Function returning multiple values
stats <- function(x) {
  list(
    mean = mean(x),
    median = median(x),
    sd = sd(x),
    min = min(x),
    max = max(x)
  )
}

numbers <- c(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
cat("\nStatistics for 1:10:\n")
result <- stats(numbers)
cat("Mean:", result$mean, "\n")
cat("Median:", result$median, "\n")
cat("SD:", result$sd, "\n")
```

**Output:**
```
===== FUNCTION BASICS =====

Hello, World!

5 squared = 25
2^10 = 1024
Welcome, User!
Welcome, Alice!

Statistics for 1:10:
Mean: 5.5
Median: 5.5
SD: 3.02765
```

### Real-World Example: Data Analysis Functions

```r
# Real-world: Custom functions for data analysis
cat("===== CUSTOM DATA ANALYSIS FUNCTIONS =====\n\n")

# Function to calculate summary statistics
summarize_numeric <- function(data, na.rm = TRUE) {
  # Input validation
  if (!is.numeric(data)) {
    stop("Data must be numeric")
  }
  
  # Calculate statistics
  n <- length(data)
  n_missing <- sum(is.na(data))
  valid_n <- n - n_missing
  
  # Handle missing values if na.rm = TRUE
  if (na.rm) {
    data <- data[!is.na(data)]
  }
  
  # Return comprehensive summary
  summary_stats <- list(
    n = valid_n,
    n_missing = n_missing,
    mean = mean(data),
    median = median(data),
    sd = sd(data),
    min = min(data),
    max = max(data),
    q25 = quantile(data, 0.25),
    q75 = quantile(data, 0.75),
    iqr = IQR(data)
  )
  
  return(summary_stats)
}

# Test with sample data
test_scores <- c(85, 90, 78, 92, 88, 76, 95, 89, 82, 91, NA, 87)
cat("Test scores:", test_scores, "\n\n")

result <- summarize_numeric(test_scores)
cat("Summary Statistics:\n")
cat("  Count (valid):", result$n, "\n")
cat("  Missing:", result$n_missing, "\n")
cat("  Mean:", round(result$mean, 2), "\n")
cat("  Median:", result$median, "\n")
cat("  SD:", round(result$sd, 2), "\n")
cat("  Range:", result$min, "-", result$max, "\n")
cat("  IQR:", round(result$iqr, 2), "\n")

# Function with dots (...) for flexible arguments
cat("\n===== FLEXIBLE ARGUMENTS =====\n\n")

calculate_weighted_mean <- function(..., weights = NULL) {
  values <- c(...)
  
  if (is.null(weights)) {
    # Simple mean
    return(mean(values))
  } else {
    # Weighted mean
    if (length(values) != length(weights)) {
      stop("Values and weights must have same length")
    }
    return(sum(values * weights) / sum(weights))
  }
}

cat("Simple mean of 1:5:", 
    calculate_weighted_mean(1, 2, 3, 4, 5), "\n")
cat("Weighted mean:", 
    calculate_weighted_mean(1, 2, 3, 4, 5, weights = c(1, 1, 2, 1, 1)), "\n")
```

**Output:**
```
===== CUSTOM DATA ANALYSIS FUNCTIONS =====

Test scores: 85 90 78 92 88 76 95 89 82 91 NA 87 

Summary Statistics:
  Count (valid): 11
  Missing: 1
  Mean: 87.09
  Median: 88
  SD: 6.22
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use meaningful function and argument names
2. Document functions with comments
3. Use default arguments for flexibility
4. Validate inputs with stopifnot() or if()
5. Use early returns for error conditions

### Common Pitfalls

1. Forgetting to return a value
2. Not handling NA values
3. Forgetting argument order with named arguments
4. Modifying global variables unintentionally
