# R Package Structure

## Learning Objectives

- Understand the directory structure of an R package
- Create and configure a basic DESCRIPTION file
- Write a proper NAMESPACE file
- Organize R code in the R/ folder
- Create placeholder documentation in man/ folder

## Theoretical Background

### What is an R Package

An R package is a collection of R functions, data, and documentation that extends R's capabilities. Creating a package allows you to:
- Share reusable code with others
- Organize your own projects systematically
- Version and distribute your work

### Required Package Components

A minimum R package requires:

1. **DESCRIPTION** - Package metadata (name, version, dependencies, author)
2. **NAMESPACE** - Controls what is exported and imported
3. **R/** - Directory containing R code files
4. **man/** - Directory containing help files (can be auto-generated)

### Optional Components

- **data/** - Data files
- **inst/** - Auxiliary files (copied to package root)
- **vignettes/** - Long-form documentation
- **tests/** - Unit tests
- **src/** - Source code (C/C++, Fortran)
- **NEWS** - Change log
- **LICENSE** - License information

## Code Examples

### Standard Example: Creating Package Structure

```r
# ===== PACKAGE STRUCTURE EXAMPLE =====

# Step 1: Create package directory structure
# Run these commands in your terminal:

# mkdir mypackage
# mkdir mypackage/R
# mkdir mypackage/man
# mkdir mypackage/DESCRIPTION
# mkdir mypackage/NAMESPACE

# Step 2: Create DESCRIPTION file
# This is the minimal DESCRIPTION file content:

description_content <- '
Package: mypackage
Type: Package
Title: My First R Package
Version: 0.1.0
Author: Your Name <your@email.com>
Maintainer: Your Name <your@email.com>
Description: A short description of what the package does.
License: MIT
Encoding: UTF-8
RoxygenNote: 7.2.3
'

# Step 3: Create NAMESPACE file
namespace_content <- '
# Export all functions
exportPattern("^[^\\\\.]")

# Import required packages
importFrom(stats, quantile)
importFrom(graphics, plot)
'

# Step 4: Create an R function file (R/hello.R)
r_code_content <- '
# Hello function - greets the user
# 
# @param name Character name to greet
# @return Character greeting message
# @export
# @examples
# hello("World")
hello <- function(name) {
  if (missing(name)) {
    name <- "World"
  }
  message <- paste0("Hello, ", name, "!")
  return(message)
}

# Add numbers function
#
# @param a Numeric first number
# @param b Numeric second number  
# @return Numeric sum
# @export
# @examples
# add_numbers(2, 3)
add_numbers <- function(a, b) {
  return(a + b)
}
'

# Step 5: Create a basic man page (man/hello.Rd)
man_page_content <- '
\\name{hello}
\\alias{hello}
\\title{Hello Function}
\\description{Greets the user by name.}
\\usage{
hello(name)
}
\\arguments{
  \\item{name}{Character name to greet. Defaults to "World".}
}
\\value{
Character greeting message.
}
\\examples{
hello("Alice")
}
'

cat("Package structure components created!\n")
cat("\n===== DESCRIPTION FILE =====\n")
cat(description_content)
cat("\n\n===== NAMESPACE FILE =====\n")
cat(namespace_content)
cat("\n\n===== R CODE EXAMPLE =====\n")
cat(r_code_content)
```

### Standard Example: Using DESCRIPTION Fields

```r
# ===== DESCRIPTION FILE DETAILS =====

# Full DESCRIPTION file example with all common fields
cat("===== COMPLETE DESCRIPTION =====\n")

full_description <- '
Package: mypackage
Type: Package
Title: What the Package Does
Version: 0.1.0.9000
Authors@R: c(
    person("First", "Last", email = "first.last@example.com", role = c(\"aut\", \"cre\")),
    person(\"Second\", \"Author\", email = \"second@example.com\", role = \"aut\")
  )
Maintainer: First Last <first.last@example.com>
Description: A longer description of the package functionality.
    This can span multiple lines and include references to papers
    or software via backticks like `function()`.
License: MIT + file LICENSE
URL: https://github.com/username/mypackage
BugReports: https://github.com/username/mypackage/issues
Encoding: UTF-8
RoxygenNote: 7.2.3
Depends: R (>= 3.5.0)
Imports: 
    dplyr (>= 1.0.0),
    ggplot2 (>= 3.0.0),
    tidyr (>= 1.0.0)
Suggests: 
    testthat (>= 3.0.0),
    knitr (>= 1.30),
    rmarkdown (>= 2.10)
LinkingTo: Rcpp (>= 1.0.0)
Remotes: 
    r-lib/package@branch
Language: en-US
'
cat(full_description)
```

### Standard Example: Package Directory Check

```r
# ===== CHECK PACKAGE STRUCTURE =====
cat("\n===== STANDARD PACKAGE TREE =====\n")

# Example package structure visualization
package_tree <- "
mypackage/
|-- DESCRIPTION          # Required: Package metadata
|-- NAMESPACE           # Required: Exports/imports
|-- R/                  # Required: R code
|   |-- hello.R
|   |-- utils.R
|-- man/                # Optional: Help docs
|   |-- hello.Rd
|-- data/               # Optional: Data files
|   |-- dataset.rda
|-- inst/               # Optional: Installed files
|   |-- extdata/
|-- tests/              # Optional: Unit tests
|   |-- testthat/
|       |-- test-hello.R
|-- vignettes/          # Optional: Long-form docs
|   |-- intro.Rmd
|-- src/                # Optional: Compiled code
|   |-- mycode.cpp
|-- NEWS               # Optional: Change log
|-- LICENSE            # Optional: License file
|-- README.md          # Optional: Readme
|-- .Rbuildignore      # Optional: Build exclude rules
"
cat(package_tree)

# Common .Rbuildignore patterns
cat("\n===== .Rbuildignore EXAMPLE =====\n")
rbuildignore_content <- "
^\\.Rproj\\.user$
^\\.git$
^\\.svn$
^data-raw$
^inst/doc$
^vignettes/.*\\.Rmd$
^docs$
^\\.github$
.*\\.Rproj$
^\\.DS_Store$
"
cat(rbuildignore_content)
```

### Standard Example: Package Metadata Fields

```r
# ===== UNDERSTANDING DEPENDENCIES =====
cat("\n===== DEPENDENCY TYPES =====\n")

dependency_explanation <- "
Fields in DESCRIPTION for dependencies:

1. Depends:
   - Packages that must be present when your package loads
   - Typically includes R version requirement
   - Example: Depends: R (>= 3.5.0)

2. Imports:
   - Packages required for your package to run
   - NOT attached to search path (safer)
   - Example: Imports: dplyr (>= 1.0.0), ggplot2

3. Suggests:
   - Optional packages for additional features
   - Used for testing, vignettes, etc.
   - Example: Suggests: testthat, knitr

4. LinkingTo:
   - Packages with C/C++ code to link against
   - Example: LinkingTo: Rcpp

5. Enhances:
   - Packages that your package enhances
   - Rarely used
   - Example: Enhances: survival
"
cat(dependency_explanation)

# Version numbering explained
cat("\n===== VERSION NUMBERING =====\n")
version_info <- "
R package version format: Major.Minor.Patch[.Development]

Examples:
- 0.1.0   = First development release
- 1.0.0   = First stable release  
- 1.0.1   = Bug fixes
- 1.1.0   = New features, backward compatible
- 2.0.0   = Breaking changes
- 0.1.0.9000 = Development version (9000 suffix)

Even-numbered minor versions = stable
Odd-numbered minor versions = development
"
cat(version_info)
```