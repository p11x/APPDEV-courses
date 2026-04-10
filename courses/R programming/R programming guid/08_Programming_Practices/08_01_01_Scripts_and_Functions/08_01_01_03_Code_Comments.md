# Code Comments in R

## Learning Objectives

- Write effective code comments
- Use roxygen2 documentation
- Document function parameters
- Create package documentation
- Maintain comment standards

## Theory

Comments explain why code exists, not what it does. Good comments add context, explain decisions, and help future maintainers understand the code. R uses # for single-line comments and roxygen2 for documentation.

Well-documented code follows consistent patterns: file headers, section dividers, function documentation, and inline comments for complex logic.

## Step-by-Step

1. Add file header with description
2. Use section dividers consistently
3. Document functions with roxygen2
4. Add inline comments for complex code
5. Keep comments updated with code

## Code Examples

### Basic Comments

```r
cat("===== BASIC COMMENTS =====\n\n")

# Calculate the mean of a numeric vector
mean_value <- sum(x) / length(x)

# Filter out incomplete cases
complete_data <- data[complete.cases(data), ]
```

### Section Dividers

```r
cat("\n===== SECTION DIVIDERS =====\n\n")

# =============================================================================
# Configuration Section
# =============================================================================

# Set up logging
log_level <- "INFO"

# =============================================================================
# Helper Functions
# =============================================================================

#' Check if object is a valid dataframe
is_valid_df <- function(x) {
  is.data.frame(x) && nrow(x) > 0
}

# =============================================================================
# Main Processing
# =============================================================================

process_data <- function(df) {
  # Main logic here
}
```

### Roxygen2 Documentation

```r
cat("\n===== ROXYGEN2 DOCUMENTATION =====\n\n")

#' Calculate weighted mean
#' 
#' Computes the weighted mean of a numeric vector
#' 
#' @param x Numeric vector of values
#' @param w Numeric vector of weights (must sum to 1)
#' @return Numeric value of weighted mean
#' @examples
#' weighted_mean(1:5, c(0.1, 0.2, 0.3, 0.3, 0.1))
weighted_mean <- function(x, w) {
  stopifnot(length(x) == length(w))
  sum(x * w)
}

#' Process data with options
#' 
#' @param data A data frame to process
#' @param na.action How to handle NA values:
#'   - "remove" removes rows with NA (default)
#'   - "mean" replaces NA with column mean
#' @param verbose Print progress messages
#' @export
process_data <- function(data, na.action = "remove", verbose = FALSE) {
  # Implementation
}
```

### Inline Comments

```r
cat("\n===== INLINE COMMENTS =====\n\n")

# Sort by date descending (most recent first)
results <- results[order(results$date, decreasing = TRUE), ]

# Calculate Z-score: (value - mean) / sd
z_score <- (x - mean(x)) / sd(x)

# Use seq_len for efficiency vs 1:length(x)
for (i in seq_len(nrow(df))) {
  # Process each row
}
```

### TODO Comments

```r
cat("\n===== TODO COMMENTS =====\n\n")

# TODO: Add error handling for non-numeric columns
# TODO: Optimize for large datasets
# FIXME: This breaks when values are negative
# NOTE: Consider using data.table for speed
#' @importFrom stats filter
```

## Real-World Example: Well-Documented Function

```r
# Real-world: Analysis function with documentation
cat("===== DOCUMENTED FUNCTION =====\n\n")

#' Calculate Moving Average
#'
#' Computes simple or exponential moving average
#'
#' @param x Numeric vector (time series data)
#' @param n Window size for SMA (ignored for EMA)
#' @param type One of "SMA" or "EMA"
#' @param alpha Smoothing factor for EMA (default 0.2)
#' 
#' @return Numeric vector of moving averages
#' @export
#'
#' @examples
#' x <- 1:100 + rnorm(100)
#' sma <- moving_average(x, n = 20, type = "SMA")
#' ema <- moving_average(x, type = "EMA", alpha = 0.1)

moving_average <- function(x, n = 10, type = c("SMA", "EMA"), alpha = 0.2) {
  type <- match.arg(type)
  
  if (!is.numeric(x) || length(x) == 0) {
    stop("x must be a non-empty numeric vector")
  }
  
  if (type == "SMA") {
    # Simple Moving Average: use filter
    stats::filter(x, rep(1/n, n), sides = 1)
  } else {
    # Exponential Moving Average
    result <- numeric(length(x))
    result[1] <- x[1]
    
    for (i in 2:length(x)) {
      result[i] <- alpha * x[i] + (1 - alpha) * result[i - 1]
    }
    
    result
  }
}
```

## Best Practices

1. Comment why, not what - the code shows what
2. Keep comments synchronized with code changes
3. Use consistent comment style throughout
4. Document all exported functions with roxygen2
5. Add section headers for long scripts
6. Use TODO/FIXME for future work
7. Avoid obvious comments

## Exercises

1. Add roxygen2 documentation to 5 existing functions
2. Create a script with section dividers
3. Convert inline comments to roxygen2
4. Add examples to all exported functions
5. Write a style guide for your team