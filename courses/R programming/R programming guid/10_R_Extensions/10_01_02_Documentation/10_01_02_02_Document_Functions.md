# Documenting Functions in R

## Learning Objectives

- Write complete function documentation
- Document parameters and return values
- Add examples and references
- Maintain documentation quality

## Theory

Well-documented functions help users understand purpose, parameters, and usage. Each function should have title, description, details, parameter documentation, return value, examples, and relevant cross-references.

## Step-by-Step

### Title and Description

```r
#' Calculate the mean of a numeric vector
#'
#' Computes the arithmetic mean of a numeric vector.
#' Handles missing values when na.rm = TRUE.
```

### Parameters

```r
#' @param x A numeric vector
#' @param na.rm Logical; should missing values be removed? (default FALSE)
```

### Return Value

```r
#' @return A numeric value (or NA if na.rm = FALSE and NAs present)
```

### Examples

```r
#' @examples
#' mean(c(1, 2, 3))
#' mean(c(1, 2, NA), na.rm = TRUE)
```

## Code Examples

### Complex Function

```r
#' Calculate weighted mean
#'
#' Computes weighted mean from values and weights.
#'
#' @param values Numeric vector of values
#' @param weights Numeric vector of weights (must equal length of values)
#' @param na.rm Remove missing values? (default FALSE)
#' @return Weighted mean as numeric
#' @export
#' @examples
#' weighted_mean(c(1, 2, 3), c(1, 1, 1))
#' weighted_mean(c(1, 2, 3), c(3, 2, 1))
weighted_mean <- function(values, weights, na.rm = FALSE) {
  sum(values * weights, na.rm = na.rm) / sum(weights, na.rm = na.rm)
}
```

### S3 Method Documentation

```r
#' Print method for my_result
#'
#' @param x Object of class my_result
#' @param ... Additional arguments (passed to print)
#' @method print my_result
#' @export
print.my_result <- function(x, ...) {
  print(x$value)
}
```

### Data Documentation

```r
#' Sample dataset
#'
#' Dataset with simulated patient data.
#'
#' @format data.frame:
#' \describe{
#'   \item{id}{Patient ID}
#'   \item{age}{Patient age}
#'   \item{outcome}{Binary outcome}
#' }
#' @source Simulated
patient_data <- data.frame(...)
```

## Best Practices

1. **Be Descriptive**: Clear descriptions.

2. **Complete Parameters**: Document all params.

3. **Working Examples**: Test examples.

4. **Cross-References**: Add @seealso.

5. **Update Regularly**: Keep in sync with code.

## Exercises

1. Document existing functions.

2. Add @param for all parameters.

3. Add working @examples.

4. Document S3 methods.

5. Document datasets.

## Additional Resources

- [R Packages - Man](https://r-pkgs.org/man.html)
- [Roxygen2](https://roxygen2.r-lib.org/)
- [Style Guide](https://cran.r-project.org/web/packages/policies.html)