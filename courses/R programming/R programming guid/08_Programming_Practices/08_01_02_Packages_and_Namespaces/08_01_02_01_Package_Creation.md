# Package Creation in R

## Learning Objectives

- Understand R package structure
- Create required package files
- Write package DESCRIPTION
- Organize R code in package
- Build and install package

## Theory

R packages are the standard distribution format for R code. A package contains R code, data, documentation, and metadata. Required files: DESCRIPTION, NAMESPACE, and at least one .R file in the R/ directory.

Package creation tools include devtools and roxygen2. The usethis package helps set up package structure. Key steps: create structure, write DESCRIPTION, add code with roxygen2 documentation, create NAMESPACE, build and check.

## Step-by-Step

1. Use devtools::create() or usethis to create structure
2. Edit DESCRIPTION with package metadata
3. Add R functions with roxygen2 documentation
4. Run devtools::document() to generate NAMESPACE
5. Run devtools::check() to verify package
6. Build with devtools::build()

## Code Examples

### Package Structure

```r
cat("===== PACKAGE STRUCTURE =====\n\n")

cat("mypackage/
DESCRIPTION
NAMESPACE
R/
  function1.R
  function2.R
man/
  function1.Rd
  function2.Rd
data/
  mydata.rda
tests/
  testthat/
    test_function1.R
vignettes/
  introduction.Rmd
")
```

### DESCRIPTION File

```r
cat("\n===== DESCRIPTION FILE =====\n\n")

desc_content <- "Package: mypackage
Title: What the Package Does
Version: 0.1.0
Authors@R: person('First', 'Last', email = 'email@example.com', role = c('aut', 'cre'))
Description: A short description of the package.
License: MIT
Encoding: UTF-8
LazyData: true
Roxygen: list(markdown = TRUE)
RoxygenNote: 2.3.0
Imports: 
  ggplot2,
  dplyr
Suggests: 
  testthat,
  knitr
"

cat(desc_content)
```

### Using usethis to Create Package

```r
cat("\n===== CREATE PACKAGE WITH USETHIS =====\n\n")

cat("# Create package directory\n# library(usethis)\n# create_package('path/to/mypackage')\n\n")

cat("# Then add functions\n# use_r('calculate_stats')\n\n")

cat("# Add package dependencies\n# use_package('ggplot2')\n\n")

cat("# Add test infrastructure\n# use_testthat()\n")
```

### Simple Package Function

```r
cat("\n===== SAMPLE FUNCTION =====\n\n")

#' Calculate Summary Statistics
#'
#' @param x Numeric vector
#' @return Data frame with statistics
#' @export
#' @examples
#' summarize(c(1, 2, 3, 4, 5))

summarize <- function(x) {
  stopifnot(is.numeric(x))
  
  data.frame(
    statistic = c("n", "mean", "median", "sd"),
    value = c(
      length(x),
      mean(x),
      median(x),
      sd(x)
    )
  )
}
```

## Real-World Example: Complete Package Setup

```r
# Real-world: Package creation workflow
cat("===== PACKAGE WORKFLOW =====\n\n")

cat("# 1. Create package\n# library(usethis)\n# create_package('C:/packages/mystats')\n\n")

cat("# 2. Edit DESCRIPTION\n# Title, author, description, license\n\n")

cat("# 3. Add functions with roxygen2\n# use_r('summary_stats')\n\n")

cat("# 4. Document functions\n# devtools::document()\n\n")

cat("# 5. Add tests\n# use_testthat()\n# use_test('test_summary_stats')\n\n")

cat("# 6. Check package\n# devtools::check()\n\n")

cat("# 7. Install and test\n# devtools::install()\n\n")

cat("# 8. Build source package\n# devtools::build()\n")
```

## Best Practices

1. Use consistent naming convention (snake_case)
2. Document all exported functions
3. Add examples to functions
4. Include testthat tests
5. Use Imports not Depends for dependencies
6. Specify version requirements
7. Include LICENSE file

## Exercises

1. Create a complete R package from scratch
2. Add 5 functions with full documentation
3. Include data in package
4. Add unit tests
5. Submit package to CRAN