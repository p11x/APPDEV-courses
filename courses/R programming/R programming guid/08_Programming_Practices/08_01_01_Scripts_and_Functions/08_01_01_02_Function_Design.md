# Function Design in R

## Learning Objectives

- Design effective R functions
- Handle function arguments properly
- Return values correctly
- Implement input validation
- Write scalable function code

## Theory

Well-designed functions are the building blocks of maintainable R code. Good functions follow the single responsibility principle, have clear inputs and outputs, include documentation, and handle edge cases.

Key principles: keep functions short and focused, use meaningful names, validate inputs, provide default values, and choose appropriate return types.

## Step-by-Step

1. Define function purpose and expected inputs
2. Choose appropriate parameters and defaults
3. Implement input validation
4. Write main logic
5. Handle edge cases and errors
6. Return structured output

## Code Examples

### Basic Function Structure

```r
cat("===== BASIC FUNCTION =====\n\n")

add_numbers <- function(a, b) {
  a + b
}

result <- add_numbers(5, 3)
cat("5 + 3 =", result, "\n")

# With default values
power <- function(base, exponent = 2) {
  base^exponent
}

cat("3^2 =", power(3), "\n")
cat("2^8 =", power(2, 8), "\n")
```

### Input Validation

```r
cat("\n===== INPUT VALIDATION =====\n\n")

calculate_mean <- function(x, remove_na = TRUE) {
  # Validate input
  if (!is.numeric(x)) {
    stop("Input must be numeric")
  }
  
  if (remove_na) {
    x <- x[!is.na(x)]
  }
  
  if (length(x) == 0) {
    return(NA_real_)
  }
  
  sum(x) / length(x)
}

cat("Mean of 1:10:", calculate_mean(1:10), "\n")
cat("With NA:", calculate_mean(c(1, 2, NA, 4)), "\n")
```

### Complex Return Values

```r
cat("\n===== RETURN VALUES =====\n\n")

summarize_data <- function(x) {
  # Validate
  if (!is.numeric(x)) stop("Numeric input required")
  
  result <- list(
    n = length(x),
    mean = mean(x, na.rm = TRUE),
    median = median(x, na.rm = TRUE),
    sd = sd(x, na.rm = TRUE),
    min = min(x, na.rm = TRUE),
    max = max(x, na.rm = TRUE),
    q25 = quantile(x, 0.25, na.rm = TRUE),
    q75 = quantile(x, 0.75, na.rm = TRUE)
  )
  
  result
}

summary_result <- summarize_data(rnorm(100))
cat("N:", summary_result$n, "\n")
cat("Mean:", round(summary_result$mean, 2), "\n")
cat("SD:", round(summary_result$sd, 2), "\n")
```

### Using Dots (...) for Flexibility

```r
cat("\n===== FLEXIBLE ARGUMENTS =====\n\n")

flexible_mean <- function(x, ..., na.rm = TRUE) {
  # Can pass additional arguments to underlying functions
  if (na.rm) {
    x <- x[!is.na(x)]
  }
  
  mean(x, ...)
}

cat("Default:", flexible_mean(c(1, 2, 3)), "\n")
cat("Trimmed:", flexible_mean(1:100, trim = 0.1), "\n")
```

### S3 Method Design

```r
cat("\n===== S3 METHODS =====\n\n")

calculate_stats <- function(x, ...) {
  UseMethod("calculate_stats")
}

calculate_stats.numeric <- function(x, ...) {
  list(
    mean = mean(x),
    median = median(x),
    sd = sd(x)
  )
}

calculate_stats.default <- function(x, ...) {
  stop("Object must be numeric")
}

result <- calculate_stats(1:10)
cat("Mean:", result$mean, "\n")
cat("Median:", result$median, "\n")
```

## Real-World Example: Data Processing Function

```r
# Real-world: Data cleaning function
cat("===== DATA CLEANING =====\n\n")

clean_dataframe <- function(df, na_cols = "all", na_action = "remove") {
  # Validate input
  if (!is.data.frame(df)) {
    stop("Input must be a data frame")
  }
  
  n_original <- nrow(df)
  
  # Check columns with all NA
  all_na <- sapply(df, function(col) all(is.na(col)))
  
  # Remove all-NA columns
  df <- df[, !all_na, drop = FALSE]
  
  # Handle NA values
  if (na_action == "remove") {
    df <- na.omit(df)
  } else if (na_action == "mean") {
    # Replace numeric NA with mean
    for (col in names(df)) {
      if (is.numeric(df[[col]])) {
        df[[col]][is.na(df[[col]])] <- mean(df[[col]], na.rm = TRUE)
      }
    }
  }
  
  n_final <- nrow(df)
  
  list(
    df = df,
    removed = n_original - n_final,
    pct_removed = round((n_original - n_final) / n_original * 100, 2)
  )
}

# Test
test_df <- data.frame(
  a = c(1, 2, NA, 4, 5),
  b = c(NA, NA, NA, NA, NA),
  c = c(10, 20, 30, 40, 50)
)

result <- clean_dataframe(test_df)
cat("Removed rows:", result$removed, "\n")
cat("Percentage removed:", result$pct_removed, "%\n")
```

## Best Practices

1. Use descriptive function names (verb_noun format)
2. Validate inputs at the start of functions
3. Provide sensible default values
4. Return consistent data types
5. Document function purpose and parameters
6. Handle missing values explicitly
7. Use ... for flexible additional arguments

## Exercises

1. Write a function to calculate multiple statistics at once
2. Create a function that accepts variable number of arguments
3. Implement a function with S3 methods
4. Write a robust data cleaning function
5. Create a function that returns both results and metadata