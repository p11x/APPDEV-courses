# Testing and Building R Packages

## Learning Objectives

- Use devtools for package development workflow
- Build and install packages from source
- Run R CMD check for compliance
- Write unit tests with testthat
- Set up continuous integration

## Theoretical Background

### devtools Package

devtools provides a collection of development tools that make R package development easier:

1. **load_all()** - Load package without installing
2. **document()** - Generate documentation
3. **build()** - Build package
4. **check()** - Run R CMD check
5. **test()** - Run tests
6. **install()** - Install package

### R CMD Check

R CMD check performs extensive checks on your package:
- Checks DESCRIPTION for completeness
- Validates NAMESPACE file
- Runs all examples
- Executes tests
- Verifies documentation
- Checks code for common errors

### testthat Testing Framework

testthat provides a testing framework inspired by testthat in R:
- **test_that()** - Define a test
- **expect_*()** - Assertions
- **test_file()** - Run tests in a file
- **test_dir()** - Run all tests in a directory
- **describe()** - Group tests (optional)

## Code Examples

### Standard Example: Devtools Workflow

```r
# ===== DEVTOOLS WORKFLOW =====

# Install devtools if needed
# install.packages("devtools")

cat("===== DEVELOPMENT WORKFLOW =====\n")

workflow_example <- "
# 1. Load and reload package
devtools::load_all(\"mypackage\")

# 2. Make changes to code
# edit your R files in the R/ directory

# 3. Document changes (generates man/)
devtools::document(\"mypackage\")

# 4. Test your changes
devtools::test(\"mypackage\")

# 5. Check the package
devtools::check(\"mypackage\")

# 6. Install the package
devtools::install(\"mypackage\")

# 7. Build a tarball
devtools::build(\"mypackage\")
"
cat(workflow_example)

# Using load_all with devtools
cat("\n===== LOAD_ALL EXAMPLE =====\n")
load_all_example <- "
# load_all loads package without full install
# This is faster during development

devtools::load_all(\"mypackage\",
  reload = TRUE,    # Reload if already loaded
  helpers = TRUE,   # Load test helpers
  quiet = FALSE    # Show progress
)

# Now test your functions directly
hello(\"Developer\")
"
cat(load_all_example)
```

### Standard Example: Building Package

```r
# ===== BUILDING PACKAGES =====
cat("\n===== BUILD COMMANDS =====\n")

build_example <- "
# Build from command line (terminal):
# R CMD build mypackage

# Using devtools
devtools::build(\"mypackage\")

# Build binary for distribution
devtools::build(\"mypackage\", binary = TRUE)

# Install from local source
devtools::install(\"mypackage\")

# Force reinstall
devtools::install(\"mypackage\", force = TRUE)
"
cat(build_example)

# Build vignette into package
cat("\n===== VIGNETTE BUILD =====\n")
vignette_example <- "
# Build vignettes into package
devtools::build(\"mypackage\", vignettes = TRUE)

# Install with vignettes
devtools::install(\"mypackage\", build_vignettes = TRUE)

# Build README vignette from inst/doc
devtools::build(\"mypackage\", include_vignettes = \"README.rmd\")
"
cat(vignette_example)
```

### Standard Example: Package Checking

```r
# ===== R CMD CHECK =====
cat("\n===== CHECK PACKAGE =====\n")

check_example <- "
# Run devtools check
devtools::check(\"mypackage\")

# Run specific checks
devtools::check(\"mypackage\",
  cran = TRUE,         # Check as if CRAN
  manual = TRUE,       # Build manual
  check_vignettes = TRUE,
  force_suggests = TRUE
)

# Check with warnings only (don't fail on notes)
devtools::check(\"mypackage\", args = c(\"--no-manual\"))
"
cat(check_example)

# Understanding check output
cat("\n===== CHECK RESULT CODES =====\n")
check_results <- "
R CMD check returns:

0 = OK (no issues)
1 = Warnings present
2 = Errors present
3 = Fatal problem (cannot build)

Common check results:
- ERROR: Major problem that must be fixed
- WARNING: Potential problem should be addressed
- NOTE: Minor issue or style suggestion
- OK: Check passed
"
cat(check_results)
```

### Standard Example: Testing with testthat

```r
# ===== TESTTHAT BASICS =====
cat("\n===== TESTTHAT SETUP =====\n")

# Install testthat if needed
# install.packages("testthat")

testthat_setup <- "
# testthat test directory structure:
tests/
|-- testthat/
|   |-- test-functions.R
|   |-- test-classes.R
|-- testthat.R (optional setup file)
"
cat(testthat_setup)

# Creating tests
cat("\n===== WRITING TESTS =====\n")
test_examples <- '
# File: tests/testthat/test-hello.R

library(testthat)
library(mypackage)

# Test the hello function
test_that("hello returns greeting", {
  result <- hello("World")
  expect_type(result, "character")
  expect_equal(result, "Hello, World!")
})

# Test default behavior
test_that("hello uses default name", {
  result <- hello()
  expect_equal(result, "Hello, World!")
})

# Test error handling
test_that("hello handles invalid input", {
  expect_error(hello(123), "name must be character")
})
'
cat(test_examples)

# More test examples
cat("\n===== ADVANCED TESTS =====\n")
advanced_tests <- '
# Test numeric function
test_that("mean_trim computes correctly", {
  x <- c(1, 2, 3, 4, 5)
  
  # No trim
  expect_equal(mean_trim(x), 3)
  
  # With trim
  expect_equal(mean_trim(x, trim = 0.2), 3)
})

# Test summarize function
test_that("summarize creates proper output", {
  x <- 1:10
  result <- summarize(x)
  
  # Check structure
  expect_s3_class(result, "data.frame")
  
  # Check columns
  expect_true("statistic" %in% names(result))
  expect_true("value" %in% names(result))
  
  # Check n
  n_row <- result$value[result$statistic == "n"]
  expect_equal(n_row, 10)
})

# Test S3 print method
test_that("print.my_lm works", {
  coef <- c("(Intercept)" = 5, "x" = 2)
  fit <- my_lm(coef, residuals = rnorm(10), 
              fitted_values = fitted(lm(1:10 ~ 1)),
              model_formula = y ~ x)
  
  expect_output(print(fit), "Coefficients:")
})
'
cat(advanced_tests)
```

### Standard Example: Running Tests

```r
# ===== RUNNING TESTS =====
cat("\n===== RUN TEST COMMANDS =====\n")

test_commands <- "
# Run all tests
devtools::test(\"mypackage\")

# Run specific test file
devtools::test_file(\"mypackage/tests/testthat/test-hello.R\")

# Run test directory
devtools::test_dir(\"mypackage/tests/testthat\")

# Using testthat directly
library(testthat)
devtools::load_all(\"mypackage\")
test_dir(\"tests/testthat\")

# Run tests with coverage
cov <- devtools::test_coat(\"mypackage\")
"
cat(test_commands)

# Test setup files
cat("\n===== TEST HELPERS =====\n")
test_helpers <- "
# File: tests/testthat.R
# This file is run before all tests

library(testthat)
library(mypackage)

# Helper data
test_data <- list(
  small = 1:5,
  large = 1:1000,
  with_na = c(1:5, NA, 7:10)
)
"
cat(test_helpers)

# Using testthat expectations
cat("\n===== EXPECTATION FUNCTIONS =====\n")
expectations <- "
# Common testthat expectations:

# Equality
expect_equal(actual, expected)
expect_equivalent(actual, expected)  # tolerance for floating point

# Identity
expect_identical(actual, expected)

# Type
expect_type(x, type)
expect_s3_class(x, class)
expect_s4_class(x, class)

# Logic
expect_true(x)
expect_false(x)
expect_null(x)
expect_na(x)

# Error
expect_error(code, pattern)
expect_warning(code, pattern)
expect_message(code, pattern)

# Output
expect_output(code, pattern)
expect_output_file(code, pattern)

# Size
expect_length(x, n)
expect_lt(x, y)
expect_gt(x, y)
expect_lte(x, y)
expect_gte(x, y)
"
cat(expectations)
```