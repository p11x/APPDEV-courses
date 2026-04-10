# Package Documentation

## Learning Objectives

- Use roxygen2 for inline documentation
- Generate man pages from roxygen2 comments
- Create package vignettes
- Document functions, data, and datasets
- Build documentation for CRAN compliance

## Theoretical Background

### roxygen2 Package

roxygen2 is an in-code documentation system that generates Rd files (R documentation) from special comments in R source files. It allows you to:

1. Write documentation near the code it describes
2. Automatically generate NAMESPACE exports
3. Maintain documentation in one place
4. Support reusable templates

### roxygen2 Tags

Common roxygen2 tags include:
- **@param** - Function parameter
- **@return** - Return value
- **@export** - Export from package
- **@examples** - Runnable examples
- **@title** - Alternative title
- **@description** - Description paragraph
- **@details** - Detailed description
- **@note** - Additional notes
- **@references** - Related references
- **@aliases** - Alternative help names
- **@family** - Related functions group

### Vignettes

Vignettes are long-form documentation articles that explain how to use your package. They are R Markdown files that can include:
- Narrative documentation
- Executable code examples
- Tables, plots, and figures

## Code Examples

### Standard Example: roxygen2 Documentation

```r
# ===== ROXYGEN2 DOCUMENTATION =====

# Install roxygen2 if needed
# install.packages("roxygen2")

cat("===== FUNCTION DOCUMENTATION =====\n")

# Example of a well-documented function using roxygen2
roxygen_example <- '
# Function with roxygen2 comments
# 
# Calculate the mean of a numeric vector with optional trimming.
# 
# This function computes the arithmetic mean after removing
# a specified proportion of observations from each tail.
#
# @param x A numeric vector containing values.
# @param trim The fraction of observations to trim from each end.
#   Default is 0 (no trimming). Must be between 0 and 0.5.
# @param na.rm logical. Should missing values be removed?
#   If FALSE, missing values cause an error.
# @return The trimmed mean of x.
# @export
# @examples
# x <- c(1, 2, 3, 4, 5, 100)
# mean_trim(x)           # 3
# mean_trim(x, trim = 0.2) # 3
# mean_trim(x, na.rm = TRUE)
mean_trim <- function(x, trim = 0, na.rm = FALSE) {
  if (!is.numeric(x)) {
    stop("x must be numeric")
  }
  if (trim < 0 || trim > 0.5) {
    stop("trim must be between 0 and 0.5")
  }
  x <- sort(x)
  n <- length(x)
  k <- floor(n * trim)
  if (k > 0) {
    x <- x[(k + 1):(n - k)]
  }
  return(mean(x, na.rm = na.rm))
}
'
cat(roxygen_example)

# Documenting multiple parameters
cat("\n===== MULTI-PARAM DOCUMENTATION =====\n")
multi_param_example <- '
# Compute summary statistics
# 
# Computes multiple summary statistics for a numeric vector.
#
# @param x Numeric vector to summarize.
# @param na.logical Should NA values be removed? Default FALSE.
# @param quantiles Numeric vector of quantiles to compute.
#   Default c(0, 0.25, 0.5, 0.75, 1).
# @param digits Number of digits to round to. Default 2.
# @return A data frame with statistic names and values.
# @export
# @examples
# x <- c(1, 2, 3, 4, 5, NA)
# summarize(x)
# summarize(x, na.rm = TRUE)
# summarize(x, quantiles = c(0.1, 0.5, 0.9))
summarize <- function(x, na.rm = FALSE, quantiles = c(0, 0.25, 0.5, 0.75, 1), digits = 2) {
  if (!is.numeric(x)) stop("x must be numeric")
  x <- x[!is.na(x)]
  if (length(x) == 0) return(NULL)
  
  df <- data.frame(
    statistic = c("n", "mean", "sd", "min", "max", paste0("q", quantiles)),
    value = c(
      length(x),
      round(mean(x), digits),
      round(sd(x), digits),
      round(min(x), digits),
      round(max(x), digits),
      round(quantile(x, quantiles), digits)
    )
  )
  return(df)
}
'
cat(multi_param_example)
```

### Standard Example: Class and S3 Documentation

```r
# ===== S3 CLASS DOCUMENTATION =====
cat("\n===== S3 CLASS DOCUMENTATION =====\n")

s3_example <- '
# Create an S3 class for regression results
# 
# @param coefficients Named numeric vector of coefficients.
# @param residuals Vector of residuals.
# @param fitted_values Vector of fitted values.
# @param model_formula The model formula used.
# @return A list of class "my_lm".
# @export
# @examples
# # Simple linear regression
# x <- 1:10
# y <- 2 * x + 3 + rnorm(10, 0, 0.5)
# fit <- lm(y ~ x)
# coef <- coef(fit)
# my_lm(coef = coef, resid = residuals(fit), fitted = fitted(fit), formula = y ~ x)
my_lm <- function(coefficients, residuals, fitted_values, model_formula) {
  result <- list(
    coefficients = coefficients,
    residuals = residuals,
    fitted_values = fitted_values,
    formula = model_formula
  )
  class(result) <- "my_lm"
  return(result)
}

# Print method for my_lm class
#
# @param x An object of class "my_lm".
# @param ... Additional arguments (ignored).
# @method print my_lm
# @export
print.my_lm <- function(x, ...) {
  cat("Call: ", deparse(x$formula), "\n\n")
  cat("Coefficients:\n")
  print(x$coefficients)
  cat("\nResidual standard error:", 
      round(sd(x$residuals) * sqrt(length(x$residuals) / (length(x$residuals) - 1)), 4), "\n")
}
'
cat(s3_example)

# Documenting data
cat("\n===== DATA DOCUMENTATION =====\n")
data_example <- '
# Sample patient data
# 
# A dataset containing information about patients
# in a clinical trial.
#
# @format A data frame with 100 rows and 4 variables:
# \describe{
#   \item{id}{Patient ID (integer).}
#   \item{age}{Patient age in years (integer).}
#   \item{sex}{Patient sex: Male or Female (factor).}
#   \item{blood_pressure}{Systolic blood pressure (numeric).}
# }
# @source Generated for demonstration purposes.
"patient_data"
'
cat(data_example)
```

### Standard Example: Vignette Creation

```r
# ===== CREATING VIGNETTES =====
cat("\n===== VIGNETTE STRUCTURE =====\n")

vignette_content <- '
---
title: "Introduction to mypackage"
author: "Your Name"
date: "`r Sys.Date()`"
output: rmarkdown::html_vignette
vignette: >
  %\\VignetteEngine{rmarkdown::render}
  %\\VignetteIndexEntry{Introduction to mypackage}
---

\\`\\`\\`{r setup}
library(mypackage)
\\`\\`\\`

# Introduction

This vignette demonstrates how to use the mypackage package.

## Installation

You can install the development version from GitHub:

\\`\\`\\`{r install, eval = FALSE}
devtools::install_github(\"username/mypackage\")
\\`\\`\\`

## Basic Usage

Here is a simple example of using the main function:

\\`\\`\\`{r example}
# Create some sample data
x <- c(1, 2, 3, 4, 5)

# Compute the trimmed mean
result <- mean_trim(x, trim = 0.1)
result
\\`\\`\\`

## Advanced Features

### Custom Quantiles

You can also specify custom quantiles:

\\`\\`\\`{r quantiles}
summarize(x, quantiles = c(0.1, 0.25, 0.5, 0.75, 0.9))
\\`\\`\\`

## Conclusion

For more information, see the help pages.
'
cat(vignette_content)
```

### Standard Example: Building Documentation

```r
# ===== BUILDING DOCUMENTATION =====
cat("\n===== BUILD COMMANDS =====\n")

build_commands <- "
# Load roxygen2 package
library(roxygen2)

# Document your package (generates man/ files)
roxygenize(\"mypackage\")

# Alternatively, use devtools
devtools::document(\"mypackage\")

# Build source package
devtools::build(\"mypackage\")

# Build binary package
devtools::build(\"mypackage\", binary = TRUE)

# Check package (includes docs check)
devtools::check(\"mypackage\")
"
cat(build_commands)

# Common roxygen2 options
cat("\n===== ROXYGEN2 OPTIONS =====\n")
roxygen_options <- "
# In DESCRIPTION file, add:
RoxygenNote: 7.2.3

# Common roxygen2 options inNAMESPACE:
#' @noRd          # Don't generate documentation
#' @raw           # Don't process, just copy
#' @usage         # Manually specify usage

# roxygen2 block syntax for multiple functions:
#' @rdname same_name
#' @export func1
#' @export func2
"
cat(roxygen_options)
```