# Testing and Quality Assurance in R

This chapter covers unit testing with testthat, code coverage, and quality assurance practices.

## 1. Introduction to testthat

The testthat package provides a testing framework for R packages and code.

### Installing testthat

```r
install.packages("testthat")
library(testthat)

# Or development version
devtools::install_github("r-lib/testthat")
```

### Basic Test Structure

```r
# test file: tests/testthat/test_basic.R

test_that("calculate_mean works correctly", {
  result <- calculate_mean(c(1, 2, 3))
  expect_equal(result, 2)
})

test_that("calculate_mean handles NA", {
  result <- calculate_mean(c(1, 2, NA), na.rm = TRUE)
  expect_equal(result, 1.5)
})

test_that("calculator_mean fails on invalid input", {
  expect_error(calculate_mean("not numeric"))
})
```

### Running Tests

```r
# Run all tests
test_dir("tests/testthat")

# Run specific test file
test_file("tests/testthat/test_basic.R")

# Run specific test
test_that_desc("basic tests")

# In RStudio: Test icon or Ctrl+Shift+T
```

## 2. Expectation Functions

### Equality Expectations

```r
test_that("expect_equal examples", {
  # Exact equality
  expect_equal(2 + 2, 4)
  expect_equal(0.1 + 0.2, 0.3, tolerance = 1e-9)
  
  # Numeric equality with tolerance
  expect_equal(1/3, 0.333, tolerance = 0.001)
  
  # For data frames
  df1 <- data.frame(x = 1:2)
  df2 <- data.frame(x = 1:2)
  expect_equal(df1, df2)
  
  # For lists
  expect_equal(list(a = 1), list(a = 1))
})
```

### Matching Expectations

```r
test_that("expect_match examples", {
  # Match string pattern
  expect_match("hello world", "hello")
  expect_match("hello world", ".*world")
  
  # Additional parameters
  expect_match("HELLO", "hello", ignore.case = TRUE)
})
```

### Type and Condition Expectations

```r
test_that("type expectations", {
  # Check class
  expect_type(1, "double")
  expect_type("hello", "character")
  expect_s3_class(mtcars, "data.frame")
  expect_s4_class(mtcars, "data.frame")
  
  # Check for custom class
  obj <- structure(list(), class = "myclass")
  expect_s3_class(obj, "myclass")
  
  # Check output type
  result <- tryCatch(stop("error"), error = function(e) e)
  expect_is(result, "error")
})
```

### True/False Expectations

```r
test_that("condition expectations", {
  # Check truth
  expect_true(2 + 2 == 4)
  expect_false(1 == 2)
  
  # Check for null
  expect_null(NULL)
  expect_null(invisible(NULL))
  
  # Check length
  expect_length(1:10, 10)
  
  # Check dimensions
  expect_equal(nrow(mtcars), 32)
  expect_equal(ncol(mtcars), 11)
})
```

### Error Expectations

```r
test_that("error expectations", {
  # Expect error
  expect_error(stop("error message"))
  expect_error(calculate_mean("not numeric"))
  
  # Specific error message
  expect_error(
    calculate_mean("not numeric"),
    "must be numeric"
  )
  
  # Expect no error
  expect_no_error(calculate_mean(1:10))
})
```

### Output Expectations

```r
test_that("output expectations", {
  # Capture output
  expect_output(print("hello"), "hello")
  expect_output(cat("test output\n"), "test output")
  
  # Capture messages
  expect_message(message("warning text"), "warning text")
  
  # Capture conditions
  expect_condition(stop("error"), "error")
})
```

## 3. Test Organization

### Test Files

```r
# tests/testthat.R
devtools::test_check("mypackage")

# tests/testthat/test_function.R
# All tests for function

# Directory structure
# tests/
#   testthat/
#     test_function_a.R
#     test_function_b.R
#     test_helpers.R
#     testthat.R
```

### Test Helpers

```r
# tests/testthat/helper-data.R
# Create test fixtures
test_data <- data.frame(
  x = 1:10,
  y = rnorm(10)
)

# Helper function
create_test_df <- function(n = 10) {
  data.frame(
    id = 1:n,
    value = rnorm(n)
  )
}

# tests/testthat/helper-packages.R
library(testthat)
library(dplyr)
```

### Skipping Tests

```r
test_that("skip examples", {
  # Skip entire test
  skip_if_not_installed("optional_package")
  
  # Skip on condition
  skip_on_os("windows")
  
  # Skip for R version
  skip_if_not(R.version$major >= 4)
})
```

### Test Descriptions

```r
# Descriptive test names
test_that("calculate_mean returns correct mean", {
  expect_equal(calculate_mean(1:10), 5.5)
})

test_that("calculate_mean handles empty vector", {
  expect_warning(result <- calculate_mean(numeric(0)))
  expect_equal(result, NaN)
})

# Use test_that_desc for BDD style
test_that_desc("calculator handles edge cases")
```

## 4. Test Coverage

### Measuring Coverage

```r
install.packages("covr")
library(covr)

# Package coverage
package_coverage()

# Report
report()

# Function coverage
partcov()

# Interactive report
shine()
```

### Interpreting Coverage

```r
# Run coverage report
cov <- package_coverage()
cov

# Coverage by function
covr::functions(cov)

# Exclude from coverage
# Add to testthat config
# .covrignore
# R/deprecated.R
```

### Coverage Thresholds

```r
# Set minimum coverage in package
# In DESCRIPTION:
# Config/testthat/Coverage: 80%

# In CI/CD
# .github/workflows/test-coverage.yaml
```

## 5. Unit Testing Strategies

### Testing Edge Cases

```r
test_that("edge cases for divide", {
  # Normal case
  expect_equal(divide(10, 2), 5)
  
  # Division by zero
  expect_error(divide(10, 0))
  
  # Negative numbers
  expect_equal(divide(-10, 2), -5)
  
  # Zero dividend
  expect_equal(divide(0, 2), 0)
  
  # Decimal results
  expect_equal(divide(1, 3), 1/3, tolerance = 1e-10)
})
```

### Testing Data Transformation

```r
test_that("filterNA removes NAs", {
  input <- data.frame(
    x = c(1, 2, NA, 4),
    y = c("a", "b", "c", "d")
  )
  
  result <- filterNA(input)
  expect_equal(nrow(result), 3)
  expect_false(any(is.na(result$x)))
})

test_that("mutate_column adds column", {
  input <- data.frame(x = 1:5)
  
  result <- mutate_column(input, "x", "x_squared")
  expect_true("x_squared" %in% names(result))
  expect_equal(result$x_squared, c(1, 4, 9, 16, 25))
})
```

### Testing Statistical Functions

```r
test_that("calculate_se computes standard error", {
  # Known values
  set.seed(42)
  data <- rnorm(100, mean = 0, sd = 1)
  
  se <- calculate_se(data)
  expected_se <- 1 / sqrt(100)
  
  expect_equal(se, expected_se, tolerance = 0.1)
})

test_that("calculate_se is NA for insufficient data", {
  # Only one value
  expect_true(is.na(calculate_se(1)))
  
  # Negative values
  expect_warning(calculate_se(-1:1))
})
```

## 6. Testing Best Practices

### Test Design Principles

```r
# 1. One assertion per test for clarity
test_that("single test", {
  result <- divide(10, 2)
  expect_equal(result, 5)
})

# 2. Test name describes expected behavior
test_that("divide handles division by zero", {
  expect_error(divide(10, 0))
})

# 3. Don't test implementation details
# Test behavior, not how it's implemented

# 4. Keep tests independent
# Each test should work alone
```

### Test Data Management

```r
# Use fixture data
# tests/testthat/fixtures/

# Create test datasets
create_test_data <- function() {
  list(
    small = data.frame(x = 1:5),
    large = data.frame(x = 1:1000),
    empty = data.frame(x = numeric(0))
  )
}

# Reference in tests
test_that("function handles all input sizes", {
  test_data <- create_test_data()
  
  for (data in test_data) {
    expect_no_error(my_function(data))
  }
})
```

### Mocking and Stubbing

```r
# Stub external functions
# Using with_mock from testthat

test_that("data loading works with stub", {
  with_mock(
    `read.csv` = function(...) data.frame(x = 1:5),
    {
      result <- load_data("test.csv")
      expect_equal(nrow(result), 5)
    }
  )
})

# Stub package functions
stub_this <- function() {}

# Test HTTP requests
library(httptest)
```

## 7. Continuous Integration

### GitHub Actions

```r
# .github/workflows/R-CMD-check.yaml
on:
  push:
    branches: [main, master]
  pull_request:
    branches: [main, master]

jobs:
  test:
    runs-on: macos-latest
    strategy:
      matrix:
        r-old: ['release']
    steps:
      - uses: actions/checkout@v3
      - uses: r-lib/actions/setup-r@v2
      - uses: r-lib/actions/setup-renv@v2
      - uses: r-lib/actions/check-standard@v2
```

### Test Coverage in CI

```r
# .github/workflows/coverage.yaml
on:
  push:
    branches: [main]

jobs:
  coverage:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: r-lib/actions/setup-r@v2
      - uses: r-lib/setup-renv/action/@v2
      - name: Test coverage
        run: |
          covr::package_coverage()
          codecov:: codecov_report()
```

## 8. Additional Testing Tools

### Other Testing Frameworks

```r
# tinytest - Alternative
install.packages("tinytest")

# Using tinytest
test_that("basic tests", {
  expect_identical(add(1, 1), 2)
})

# Running
tinytest::test_package("mypackage")
```

### Property-Based Testing

```r
# Using rapid library
install.packages("rapid")

# Define properties
test_property("mean is within range", {
  x <- random_numeric_vector()
  result <- calculate_mean(x)
  expect_gte(result, min(x))
  expect_lte(result, max(x))
})
```

### Snapshot Testing

```r
# Using vdiffr
install.packages("vdiffr")
library(vdiffr)

# Save snapshot
save_pplot(p, "plot_name.png")

# Test snapshot
expect_doppelganger("basic plot", plot(mtcars))

# Update snapshots
vdiffr::manage_snapshots()
```

## Summary

- Use testthat for unit testing
- Follow test design principles: one assertion per test
- Test edge cases and error conditions
- Aim for high test coverage (80%+)
- Run tests in CI/CD pipelines
- Use descriptive test names
- Keep tests independent and idempotent