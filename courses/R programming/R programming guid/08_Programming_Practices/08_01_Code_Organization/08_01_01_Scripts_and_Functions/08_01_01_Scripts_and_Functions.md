# Scripts and Functions in R

This chapter covers best practices for organizing R code through scripts and functions, including proper structure, documentation, and reusability.

## 1. R Scripts Fundamentals

R scripts are plain text files containing R code that can be executed sequentially. They form the foundation of reproducible research and automated analysis pipelines.

### Creating and Running Scripts

```r
# my_analysis.R - Example script structure

# Load required packages
library(dplyr)
library(ggplot2)

# Define data path
data_path <- "data/raw/survey_data.csv"

# Load data
survey_data <- read.csv(data_path)

# Perform analysis
summary_stats <- survey_data %>%
  group_by(response_category) %>%
  summarise(
    count = n(),
    mean_value = mean(value, na.rm = TRUE),
    sd_value = sd(value, na.rm = TRUE)
  )

# Create visualization
ggplot(summary_stats, aes(x = response_category, y = mean_value)) +
  geom_bar(stat = "identity") +
  labs(title = "Summary by Response Category",
       x = "Category",
       y = "Mean Value")

# Save results
write.csv(summary_stats, "output/summary_stats.csv")
```

### Sourcing External Scripts

The `source()` function executes code from external files, enabling code modularization and reuse across multiple scripts.

```r
# Load utility functions from external script
source("R/utils.R")

# Source with echo to see each line as it runs
source("R/data_processing.R", echo = TRUE)

# Source silently without output
source("R/legacy_functions.R", echo = FALSE, warn = FALSE)

# Conditionally source based on package availability
if (requireNamespace("here", quietly = TRUE)) {
  source(here::here("R", "project_utils.R"))
}
```

## 2. Function Construction

Functions encapsulate reusable logic, making code more maintainable, testable, and collaborative.

### Basic Function Structure

```r
# Function with single return value
calculate_mean <- function(x, na.rm = FALSE) {
  if (!is.numeric(x)) {
    stop("Input must be numeric")
  }
  
  if (na.rm) {
    x <- x[!is.na(x)]
  }
  
  if (length(x) == 0) {
    return(NA_real_)
  }
  
  sum(x) / length(x)
}

# Test the function
test_vector <- c(1, 2, 3, 4, 5, NA, 7)
calculate_mean(test_vector, na.rm = TRUE)
# [1] 3.666667
```

### Functions with Multiple Arguments

```r
# Calculate descriptive statistics
descriptive_stats <- function(x, 
                            na.rm = TRUE, 
                            quantiles = c(0.25, 0.5, 0.75),
                            round_digits = 2) {
  if (!is.numeric(x)) {
    stop("Input must be a numeric vector")
  }
  
  if (na.rm) {
    x <- x[!is.na(x)]
  }
  
  if (length(x) == 0) {
    return(NULL)
  }
  
  result <- list(
    n = length(x),
    mean = round(mean(x), round_digits),
    median = round(median(x), round_digits),
    sd = round(sd(x), round_digits),
    min = min(x),
    max = max(x),
    quantiles = setNames(
      round(quantile(x, quantiles), round_digits),
      paste0("q", quantiles * 100)
    )
  )
  
  return(result)
}

# Usage
stats <- descriptive_stats(mtcars$mpg)
print(stats)
```

### Dealing with Default Arguments

```r
# Flexible function with defaults
plot_distribution <- function(data,
                            title = "Distribution Plot",
                            bins = 30,
                            color = "steelblue",
                            alpha = 0.7,
                            show_mean_line = TRUE,
                            show_median_line = TRUE) {
  
  # Validate inputs
  if (!is.numeric(data)) {
    stop("Data must be numeric")
  }
  if (bins < 1 || bins > 100) {
    stop("Bins must be between 1 and 100")
  }
  
  # Remove NAs if present
  data <- data[!is.na(data)]
  
  # Create histogram
  hist_data <- data.frame(value = data)
  
  p <- ggplot(hist_data, aes(x = value)) +
    geom_histogram(bins = bins, fill = color, alpha = alpha) +
    labs(title = title)
  
  # Add reference lines
  if (show_mean_line) {
    p <- p + geom_vline(aes(xintercept = mean(data)),
                       linetype = "dashed",
                       color = "red")
  }
  
  if (show_median_line) {
    p <- p + geom_vline(aes(xintercept = median(data)),
                       linetype = "dotted",
                       color = "green")
  }
  
  return(p)
}

# Usage
plot_distribution(mtcars$wt, title = "Car Weight Distribution")
```

## 3. Parameter Validation

Always validate inputs to functions to provide clear error messages and prevent unexpected behavior.

```r
# Comprehensive parameter validation
validate_data <- function(data,
                          group_var,
                          value_var,
                          min_groups = 1,
                          max_groups = Inf) {
  
  # Check data is a data frame
  if (!is.data.frame(data)) {
    stop("Data must be a data frame or tibble")
  }
  
  # Check grouping variable exists
  if (!group_var %in% names(data)) {
    stop(paste("Group variable", group_var, "not found in data"))
  }
  
  # Check value variable exists
  if (!value_var %in% names(data)) {
    stop(paste("Value variable", value_var, "not found in data"))
  }
  
  # Check value variable is numeric
  if (!is.numeric(data[[value_var]])) {
    stop(paste("Value variable", value_var, "must be numeric"))
  }
  
  # Check minimum groups
  n_groups <- length(unique(data[[group_var]]))
  if (n_groups < min_groups) {
    stop(paste("At least", min_groups, "group(s) required, found", n_groups))
  }
  
  # Check maximum groups
  if (is.finite(max_groups) && n_groups > max_groups) {
    stop(paste("At most", max_groups, "group(s) allowed, found", n_groups))
  }
  
  return(TRUE)
}

# Test validation
test_data <- data.frame(
  group = c("A", "A", "B", "B"),
  value = c(10, 20, 30, 40)
)

validate_data(test_data, "group", "value")
# Returns TRUE
```

## 4. Lazy Evaluation

R uses lazy evaluation, meaning arguments are not evaluated until they are needed. This enables powerful programming patterns.

```r
# Lazy evaluation example
calculate_with_message <- function(x, message = "Default message") {
  cat("Function entered\n")
  
  # message is not evaluated until used
  result <- x + 10
  
  cat(message, "\n")  # Now evaluated
  
  return(result)
}

# Only prints "Function entered" - message never evaluated
calculate_with_message(5, message = stop("This never runs"))

# Using lazy evaluation for optional computation
compute_if_valid <- function(data, compute_expensive = FALSE) {
  cat("Starting computation\n")
  
  if (compute_expensive) {
    Sys.sleep(1)  # Simulate expensive operation
    cat("Expensive computation done\n")
  }
  
  return(nrow(data))
}
```

## 5. Dot-dot-dot (...) Arguments

The ellipsis allows functions to accept any number of additional arguments, which can be passed to other functions.

```r
# Forward additional arguments
wrapper_function <- function(data, ..., na.rm = FALSE) {
  result <- mean(data, na.rm = na.rm, ...)
  return(result)
}

# Usage - passes trim to mean
wrapper_function(c(1:10, NA), trim = 0.2, na.rm = TRUE)

# Collecting dot-dot-dot arguments
calculate_summary <- function(data, ...) {
  args <- list(...)
  
  cat("Additional arguments provided:\n")
  print(names(args))
  
  # Calculate mean with any additional parameters
  result <- mean(data, ...)
  
  return(result)
}

calculate_summary(c(1, 2, 3, 4, 5), na.rm = TRUE, trim = 0.1)
```

## 6. Environment and Scope

Understanding lexical scoping in R allows for sophisticated function design.

```r
# Function factory - creates functions with custom behavior
create_scaler <- function(scale_factor) {
  function(x) {
    x * scale_factor
  }
}

# Create specific scalers
double <- create_scaler(2)
triple <- create_scaler(3)

double(5)  # 10
triple(5)  # 15

# Closure example - maintaining state
counter <- function() {
  count <- 0
  
  function() {
    count <<- count + 1
    count
  }
}

next_id <- counter()
next_id()  # 1
next_id()  # 2
next_id()  # 3
```

## Summary

- R scripts enable reproducible code execution and project organization
- Use `source()` to load external R code files
- Functions should validate inputs and provide meaningful error messages
- Default arguments provide flexibility while maintaining sensible defaults
- Lazy evaluation allows for efficient computation
- Use `...` to forward additional arguments to base functions
- Closures enable stateful functions through lexical scoping