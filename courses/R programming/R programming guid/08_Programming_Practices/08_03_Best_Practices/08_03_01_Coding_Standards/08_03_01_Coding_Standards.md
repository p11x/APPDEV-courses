# Coding Standards in R

This chapter covers coding standards, style guides, and naming conventions for clean, maintainable R code.

## 1. Tidyverse Style Guide

The tidyverse style guide provides consistent conventions for R code.

### File Organization

```r
# load packages at top of script
library(dplyr)
library(ggplot2)
library(tidyr)

# Then define functions
my_function <- function(x) {
  # code
}

# Then run analysis
result <- my_function(data)
```

### Code Layout

```r
# Use spaces around operators
x <- 5          # Good
x<-5            # Bad

# But not inside brackets
mean(x)         # Good
mean( x )       # Bad

# Line length: max 80 characters
# Use indentation (two spaces)

# Good formatting
result <- data %>%
  filter(condition) %>%
  group_by(group) %>%
  summarise(mean = mean(value)) %>%
  ungroup()

# Breaking long lines
ggplot(data, aes(x = long_variable_name, 
                 y = another_long_name)) +
  geom_point()
```

### Comments

```r
# Header comment with file purpose
#
# Analysis of customer churn
# Author: Jane Smith
# Date: 2024-01-15

# Section comments
# Load and clean data ----

# Inline comments (use sparingly)
x <- x + 1  # Increment counter

# Document "why" not "what"
# Using log scale because values span orders of magnitude
plot(log(value) ~ time)
```

## 2. Naming Conventions

### Variable Names

```r
# snake_case for variables and functions
customer_id <- 12345
order_total <- 99.99
plot_scatter <- function() {}

# Avoid:
# camelCase: customerId
# mixed: Customer.ID
# all lower: customerid

# Descriptive names
# Good
transactions_2023
player_score_avg
plot_distribution

# Bad (too short)
# x
# ts
# p
```

### Function Names

```r
# Verb + noun pattern
calculate_mean()
filter_data()
plot_results()

# Make consistent
get_data()      # get
set_data()      # set
update_data()   # update

# Helpers start with dot
.utils.R
.internal_function <- function() {}
```

### Constants and Configuration

```r
# ALL_CAPS for constants
MAX_ITERATIONS <- 1000
DEFAULT_THRESHOLD <- 0.05

# Configuration objects
CONFIG <- list(
  threshold = 0.05,
  max_iterations = 1000,
  output_dir = "results/"
)
```

### Prefixes and Suffixes

```r
# Common prefixes
is_     # logical check: is_valid(), is_numeric()
has_    # checks: has_values()
get_    # retrieve: get_data()
calc_   # calculate: calc_mean()
plot_   # plotting: plot_hist()

# Common suffixes
# _df   data frame
# _dt   data table
# _mat  matrix
# _vec  vector
# _lst  list
```

## 3. Object Conventions

### Data Frame Column Names

```r
# Consistent naming in data frames
# Good: lowercase, snake_case
df <- data.frame(
  customer_id = 1,
  order_total = 99.99,
  order_date = as.Date("2024-01-15")
)

# Avoid spaces or special characters
# Bad:
# df <- data.frame(
#   "Customer ID" = 1,      # Space
#   "order.total" = 99,    # Dot
#   "OrderTotal" = 99      # Mixed case
# )
```

### Factor and Character Values

```r
# Consistent categories
# Use lowercase for values (except proper nouns)
gender <- c("male", "female", "other")
status <- c("pending", "approved", "rejected")

# Standardize on input
# Convert with:
df$status <- tolower(df$Status)
df$gender <- tools::toTitleCase(df$gender)
```

## 4. Pipe Style

### Basic Pipe Usage

```r
# Simple chain
data %>%
  select(col1, col2) %>%
  filter(condition) %>%
  mutate(new_col = col1 * 2)

# When pipe spans multiple lines
data %>%
  select(
    col1,
    col2,
    col3
  ) %>%
  filter(condition) %>%
  group_by(group) %>%
  summarise(
    mean = mean(value),
    n = n()
  )
```

### Pipe with Multiple Arguments

```r
# Bad - pipes obscure arguments
data |> 
  filter(x > 5) |> 
  ggplot(aes(x, y)) + 
  geom_point()

# Better - break long pipes
data_filtered <- data |>
  filter(x > 5)

ggplot(data_filtered, aes(x, y)) +
  geom_point()

# When using many arguments
left_join(data1, data2, by = c("id" = "customer_id"))
```

### Pipe Assignment

```r
# Option 1: Assignment at start
result <- data %>%
  filter(condition) %>%
  mutate(new_col = col * 2)

# Option 2: Assignment at end (for pipes starting with object)
data %>%
  filter(condition) %>%
  mutate(new_col = col * 2) -> result

# Option 3: Intermediate assignments
cleaned <- data %>% filter(valid)
result <- cleaned %>% summarise(mean(value))
```

## 5. Function Style

### Function Definition

```r
# Good function structure
calculate_summary <- function(data,
                               group_var = NULL,
                               na.rm = TRUE) {
  # Input validation
  if (!is.data.frame(data)) {
    stop("data must be a data frame")
  }
  
  # Function body
  if (is.null(group_var)) {
    result <- summarise(data, mean = mean(value, na.rm = na.rm))
  } else {
    result <- data %>%
      group_by(.data[[group_var]]) %>%
      summarise(mean = mean(value, na.rm = na.rm))
  }
  
  return(result)
}
```

### Return Values

```r
# Explicit return for early exit
find_value <- function(data, target) {
  for (i in seq_along(data)) {
    if (data[i] == target) {
      return(i)  # Early return
    }
  }
  return(NULL)  # Not found
}

# Return statement for clarity
process_data <- function(df) {
  df <- janitor::clean_names(df)
  df$date <- as.Date(df$date)
  
  return(df)
}
```

### Argument Patterns

```r
# Use consistent argument names
# Good
mean(x, na.rm = FALSE)
sd(x, na.rm = FALSE)
median(x, na.rm = FALSE)

# Default TRUE for data modification
# Default FALSE for optional behavior

# Logical flags as last arguments
plot_scatter <- function(data, x, y, 
                         add_regression = FALSE,
                         add_labels = FALSE,
                         color = "blue") {
  # code
}
```

## 6. Code Organization

### Script Structure

```r
# 1. Header ------------------------------------------------------------
#
# Script: analysis.R
# Purpose: Customer analysis
# Author: Author Name
# Date: 2024-01-15

# 2. Load packages ---------------------------------------------------
library(dplyr)
library(ggplot2)

# 3. Load data --------------------------------------------------------
data <- read.csv("data.csv")

# 4. Define functions --------------------------------------------------
my_function <- function() {}

# 5. Analysis ---------------------------------------------------------
result <- data %>%
  group_by(category) %>%
  summarise(count = n())

# 6. Save output ------------------------------------------------------
write.csv(result, "output.csv")
```

### Related Code Grouping

```r
# Group constants together
# Configuration ----
THRESHOLD <- 0.05
MIN_SAMPLES <- 30
MAX_ITERATIONS <- 100

# Data processing ----
raw_data <- load_raw_data()
cleaned_data <- clean_data(raw_data)

# Analysis ----
model_results <- fit_model(cleaned_data)
predictions <- predict_outcomes(model_results)

# Visualization ----
plot_distribution(cleaned_data)
plot_model_fit(model_results)
```

### Project Structure

```r
# R/project.R
# Define main entry points clearly

#' Run Complete Analysis
#' @param input_file Path to input data
#' @export
run_analysis <- function(input_file) {
  data <- load_data(input_file)
  cleaned <- preprocess(data)
  results <- analyze(cleaned)
  save_results(results)
}

#' Load and validate input data
load_data <- function(file) {}

#' Preprocess data
preprocess <- function(data) {}

#' Run statistical analysis
analyze <- function(data) {}

#' Save analysis results
save_results <- function(results) {}
```

## 7. Code Quality Rules

### Input Validation

```r
# Always validate function inputs
validate_input <- function(data, 
                          required_cols = NULL) {
  if (!is.data.frame(data)) {
    stop("data must be a data frame")
  }
  
  if (!is.null(required_cols)) {
    missing <- setdiff(required_cols, names(data))
    if (length(missing) > 0) {
      stop("Missing required columns: ", 
          paste(missing, collapse = ", "))
    }
  }
  
  TRUE
}
```

### Error Messages

```r
# Clear, actionable error messages
# Bad
stop("Error")

# Good
stop("calculate_mean requires numeric input, got ", class(x))

# Include what's expected
# Bad
stop("Invalid value")

# Good
stop("threshold must be between 0 and 1, got ", threshold)
```

### Avoid Global State

```r
# Bad: relies on global variable
calculate <- function() {
  result <- global_data * 2  # Depends on external variable
}

# Good: pass as argument
calculate <- function(data) {
  result <- data * 2
}
```

## Summary

- Use tidyverse style: snake_case, two-space indent
- Limit line length to 80 characters
- Use descriptive variable and function names
- Comment why, not what
- Use pipes for readable data transformations
- Structure scripts consistently
- Validate inputs and provide clear error messages
- Avoid global state where possible