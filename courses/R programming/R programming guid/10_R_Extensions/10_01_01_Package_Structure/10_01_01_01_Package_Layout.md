# Package Layout in R

## Learning Objectives

- Understand R package directory structure
- Create required and optional files
- Organize package contents
- Use standard conventions
- Build package properly

## Theory

R packages have a standard directory structure. Required: DESCRIPTION file, R/ directory with .R files. Optional: man/ for documentation, data/ for data files, inst/ for additional files, tests/ for unit tests, vignettes/ for usage guides.

The devtools and usethis packages automate package creation. Following conventions ensures compatibility with R CMD check and CRAN.

## Step-by-Step

1. Create package root directory
2. Add DESCRIPTION file
3. Create R/ with functions
4. Add documentation (roxygen2)
5. Add tests/ directory
6. Run R CMD check

## Code Examples

### Package Directory Structure

```r
cat("===== PACKAGE STRUCTURE =====\n\n")

cat("mypackage/
DESCRIPTION        # Package metadata
NAMESPACE          # (auto-generated)
R/
  myfunc.R         # R code files
  helpers.R
man/
  myfunc.Rd        # (auto-generated)
data/
  dataset.rda      # Included data
inst/
  CITATION         # Citation information
  NEWS             # Change log
tests/
  testthat/
    test-myfunc.R  # Unit tests
vignettes/
  intro.Rmd        # Usage guide
NAMESPACE
LICENSE            # License file
README.md         # Readme
")
```

### DESCRIPTION File Example

```r
cat("\n===== DESCRIPTION =====\n\n")

desc <- "Package: mypackage
Title: My Package Title
Version: 0.1.0
Authors@R: 
    person(given = 'First', family = 'Last', 
           email = 'author@example.com', role = c('aut', 'cre'))
Description: A short description of what the package does.
License: MIT
Encoding: UTF-8
LazyData: true
Roxygen: list(markdown = TRUE)
RoxygenNote: 2.3.0
Imports: 
    ggplot2 (>= 3.0.0),
    dplyr
Suggests: 
    testthat,
    knitr
"

cat(desc)
```

### R Code Organization

```r
cat("\n===== R CODE FILES =====\n\n")

cat("# R/myfunc.R - Main exported functions\n")
cat("'@export\n")
cat("function_name <- function() { }\n\n")

cat("# R/internal.R - Internal/helper functions\n")
cat("# Not exported\n\n")

cat("# R/data.R - Dataset loading\n")
cat("#' @examples\n")
cat("# data(mydata)\n")
```

### Data Organization

```r
cat("\n===== DATA FILES =====\n\n")

cat("# data/mydataset.R or data/mydataset.rda\n\n")

cat("# Creating lazy-load dataset:\n")
cat("# save(mydf, file = 'data/mydf.rda')\n\n")

cat("# Using roxygen2 for data documentation:\n")
cat("#' My dataset\n")
cat("#' A sample dataset for examples\n")
cat("#' @format A data frame with ...\n")
cat("#' @source \\url{...}\n")
cat("# NULL\n")
```

### Test Organization

```r
cat("\n===== TESTS =====\n\n")

cat("# tests/testthat/test_myfunc.R\n\n")

cat("test_that('myfunc works', {\n")
cat("  result <- myfunc()\n")
cat("  expect_type(result, 'double')\n")
cat("  expect_gt(result, 0)\n")
cat("})\n")
```

### Vignette Organization

```r
cat("\n===== VIGNETTES =====\n\n")

cat("# vignettes/introduction.Rmd\n\n")

cat("---\n")
cat("title: \"Introduction\"\n")
cat("output: rmarkdown::html_vignette\n")
cat("---\n\n")

cat("%\\VignetteIndexEntry{Introduction}\n")
cat("%\\VignetteEngine{rmarkdown::render}\n")
cat("%\\VignetteEncoding{UTF-8}\n\n")

cat("# Introduction\n\n")
cat("This package provides ...\n")
```

## Real-World Example: Complete Package Setup

```r
# Real-world: Package workflow
cat("===== PACKAGE WORKFLOW =====\n\n")

cat("# 1. Create package structure\n")
cat("# library(devtools)\n")
cat("# create('mypackage')\n\n")

cat("# 2. Add DESCRIPTION details\n")
cat("# Edit DESCRIPTION file\n\n")

cat("# 3. Add R functions\n")
cat("# use_r('main_function')\n")

cat("# 4. Document with roxygen2\n")
cat("# roxygenise()\n\n")

cat("# 5. Add tests\n")
cat("# use_testthat()\n")
cat("# use_test('test_main')\n\n")

cat("# 6. Check package\n")
cat("# check()\n\n")

cat("# 7. Install\n")
cat("# install()\n\n")

cat("# 8. Build\n")
cat("# build() - creates .tar.gz\n")
```

## Best Practices

1. Use standard directory names
2. Keep R functions in separate files with clear names
3. Document with roxygen2
4. Include tests for all exported functions
5. Add examples to all functions
6. Use sensible version numbers
7. Include LICENSE and README

## Exercises

1. Create a complete package structure
2. Add 3 functions with documentation
3. Create a data file in data/
4. Add testthat tests
5. Build and check the package