# roxygen2 Basics

## Learning Objectives

- Understand roxygen2 basics
- Write documentation comments
- Generate Rd files
- Maintain package documentation

## Theory

roxygen2 is a documentation system that uses special comments in R source files. These comments are processed to generate .Rd files used for help pages. It keeps documentation next to code for easier maintenance.

## Step-by-Step

### Basic roxygen2 Comment

```r
#' Title (one sentence)
#'
#' Description (longer text)
#'
#' @param x Description of parameter
#' @return Description of return
#' @export
#' @examples
#' my_function(1)
my_function <- function(x) {
  x + 1
}
```

### roxygen2 Tags

```r
#' @param Name Description
#' @return Description
#' @export
#' @examples
#' @importFrom pkg func
#' @import pkg
#' @keywords keyword
#' @note Note
#' @references Link
#' @author Name email
#' @seealso help
```

## Code Examples

### Full roxygen2 Example

```r
#' Add two numbers
#'
#' Takes two numeric vectors and returns their sum.
#'
#' @param x A numeric vector
#' @param y A numeric vector of length 1
#' @return Numeric vector of sums
#' @export
#' @examples
#' add_numbers(1:5, 2)
#' add_numbers(c(1,2,3), 5)
add_numbers <- function(x, y) {
  x + y
}
```

### Running roxygen2

```r
library(devtools)

# Generate documentation
roxygenise()

# Or in RStudio: Build > More > Document
```

### Package Title and Description

```r
#' mypackage: Package for Data Analysis
#'
#' A short description (one paragraph).
#'
#' @name mypackage
#' @aliases mypackage-package
#' @docType package
#' @keywords package
NULL
```

## Best Practices

1. **Document All Exported**: Document everything exported.

2. **Run Examples**: Examples should run without error.

3. **Use Tags**: Use @param, @return, etc.

4. **Update**: Re-run when adding functions.

5. **Check**: Run check() to verify.

## Exercises

1. Add roxygen2 to existing function.

2. Create @param documentation.

3. Add @examples.

4. Run roxygenise().

5. Test help pages.

## Additional Resources

- [roxygen2 Package](https://roxygen2.r-lib.org/)
- [R Packages - Documentation](https://r-pkgs.org/man.html)
- [Rd Format](https://cran.r-project.org/doc/manuals/R-exts.html#Rd-format)