# Unit Testing for R Packages

## Learning Objectives

- Set up testthat in packages
- Write test files
- Run test suites
- Apply testing best practices

## Theory

Testing is essential for R packages. testthat provides the standard testing framework. Tests should cover exported functions, edge cases, and expected errors. Run tests during development and before release.

## Step-by-Step

### Setting Up Tests

```r
library(usethis)

# Set up testthat
use_testthat()

# Create first test file
use_test("my_function")
```

### Basic Test Structure

```r
library(testthat)

test_that("my_function works", {
  expect_equal(my_function(1, 2), 3)
})

test_that("my_function handles NA", {
  expect_equal(my_function(NA, 2), NA_real_)
})
```

## Code Examples

### Test File Template

```r
# tests/testthat/test-function.R

library(testthat)
library(mypackage)

test_that("calculate_mean returns correct value", {
  expect_equal(calculate_mean(c(1, 2, 3)), 2)
})

test_that("calculate_mean handles NA", {
  expect_equal(calculate_mean(c(1, 2, NA), 1.5))
})

test_that("calculate_mean gives error for invalid input", {
  expect_error(calculate_mean("a"))
})
```

### Testing with Expectations

```r
# Type checks
expect_type(x, "double")
expect_s3_class(x, "data.frame")
expect_s4_class(x, "matrix")

# Comparisons
expect_gt(x, y)
expect_gte(x, y)
expect_lt(x, y)

# Error handling
expect_error(risky_function())
expect_warning(warning_function())
```

### Running Tests

```r
# Run all tests
devtools::test()

# Run specific file
devtools::test_file("tests/testthat/test-func.R")

# With coverage
covr::package_coverage()
```

## Best Practices

1. **Test Exported Functions**: All exported functions need tests.

2. **Test Edge Cases**: Test empty, NA, etc.

3. **Test Errors**: Test error conditions.

4. **Descriptive Names**: Name tests clearly.

5. **Run Often**: Run during development.

## Exercises

1. Set up testthat.

2. Write first test.

3. Test error conditions.

4. Run test suite.

5. Check test coverage.

## Additional Resources

- [testthat](https://testthat.r-lib.org/)
- [R Packages Testing](https://r-pkgs.org/tests.html)
- [Testthat Cheatsheet](https://testthat.r-lib.org/cheatsheet.html)