# Unit Testing in R

## Learning Objectives

- Learn testthat package for unit testing
- Create test files and test cases
- Run and manage tests
- Apply testing best practices

## Theory

Unit testing verifies that individual functions work correctly. The testthat package provides a testing framework for R. Tests should be written for all important functions and run regularly to catch regressions.

## Step-by-Step Guide

### Setting Up testthat

```r
# Install testthat
install.packages("testthat")

# Add to package
usethis::use_testthat()
```

### Creating Test Files

```r
# Create test file
usethis::use_test("function_name")

# Test files go in tests/testthat/
# Test function: test_function_name.R
```

### Basic Test Structure

```r
library(testthat)

test_that("description of test", {
  # Test code
  expect_equal(1 + 1, 2)
})

test_that("another test", {
  expect_true(TRUE)
  expect_false(FALSE)
})
```

## Code Examples

### Test Functions

```r
test_that("calculate_mean works", {
  result <- calculate_mean(c(1, 2, 3))
  expect_equal(result, 2)
  
  result2 <- calculate_mean(numeric(0))
  expect_true(is.na(result2))
})

test_that("filter_data filters correctly", {
  df <- data.frame(x = 1:5, y = letters[1:5])
  result <- filter_data(df, x > 2)
  expect_equal(nrow(result), 3)
  expect_true(all(result$x > 2))
})
```

### Expectation Functions

```r
# Equality
expect_equal(actual, expected)
expect_identical(actual, expected)

# Comparisons
expect_gt(x, y)
expect_gte(x, y)
expect_lt(x, y)
expect_lte(x, y)

# Logical
expect_true(x)
expect_false(x)

# Type checks
expect_is(x, "numeric")
expect_type(x, "double")

# Error checking
expect_error(risky_function())
expect_warning(warning_function())
```

### Setup and Teardown

```r
# test_within runs setup and teardown
test_that("function works with test data", {
  test_data <- create_test_data()
  
  within({
    # Tests here
    result <- process(test_data)
    expect_equal(nrow(result), 10)
  }, {
    # Cleanup
    rm(test_data)
  })
})
```

### Running Tests

```r
# Run all tests
devtools::test()

# Run specific file
devtools::test_file("tests/testthat/test_func.R")

# Run with coverage
covr::report()
```

## Best Practices

1. **Test All Exported Functions**: Every function should have tests.

2. **Test Edge Cases**: Empty vectors, NA values, etc.

3. **Test Errors**: Verify error handling works.

4. **Descriptive Names**: Test names should describe what's tested.

5. **Run Tests Often**: Run tests during development.

## Exercises

1. Set up testthat in your project.

2. Write tests for existing functions.

3. Test edge cases.

4. Test error conditions.

5. Run tests with coverage.

## Additional Resources

- [testthat Package](https://testthat.r-lib.org/)
- [Writing Testable Code](https://r-pkgs.org/tests.html)
- [testthat Cheatsheet](https://raw.githubusercontent.com/r-lib/testthat/main/README/cheatsheet.svg)