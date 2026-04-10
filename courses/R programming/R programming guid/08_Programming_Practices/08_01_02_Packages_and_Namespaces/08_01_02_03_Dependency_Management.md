# Dependency Management in R Packages

## Learning Objectives

- Understand the different types of dependencies in R packages
- Learn how to specify dependencies in the DESCRIPTION file
- Master the use of import, suggests, and depends fields
- Implement proper dependency management for package development

## Theory

Dependency management is a critical aspect of R package development. R packages can depend on other packages in three main ways: imports, suggests, and depends. The Imports field is for packages required for your package to function, while Suggests is for optional functionality. The Depends field is generally not recommended in modern package development as it can cause conflicts.

## Step-by-Step Guide

### Understanding Dependency Types

The DESCRIPTION file contains several fields for managing dependencies. The most commonly used are Imports, Suggests, and Depends (though Depends should be avoided when possible). Understanding when to use each type is essential for proper package development.

### Specifying Imports

The Imports field should contain packages that are required for your package to work. These packages will be loaded when your package is loaded, and they must be installed for your package to pass R CMD check.

```r
# In DESCRIPTION file:
Imports: 
    ggplot2 (>= 3.4.0),
    dplyr,
    tidyr,
    magrittr
```

### Using Suggests for Optional Dependencies

The Suggests field is for packages that provide optional functionality. Your package will work without them, but certain features will be unavailable.

```r
# In DESCRIPTION file:
Suggests: 
    testthat (>= 3.0.0),
    knitr,
    rmarkdown,
    covr
```

### Version Specifications

You can specify minimum versions, maximum versions, or version ranges for your dependencies:

```r
# Minimum version
Imports: dplyr (>= 1.0.0)

# Version range
Imports: ggplot2 (>= 3.4.0, < 4.0.0)

# GitHub package
Imports: r-lib/gt
```

### Using Namespace Imports

Within your NAMESPACE file, you can control exactly what functions are imported from dependencies:

```r
# Import specific functions
importFrom(dplyr, filter, select, mutate)

# Import everything
import(dplyr)

# Import all except specific functions
importFrom(stats, na.omit)
```

## Code Examples

### Complete DESCRIPTION Example

A realistic DESCRIPTION file with proper dependency management:

```
Package: mypackage
Title: My Package for Data Analysis
Version: 0.1.0
Authors@R: person("John", "Doe", email = "john@example.com", role = c("aut", "cre"))
Description: A package for analyzing data with visualization.
License: MIT
Depends: R (>= 4.1.0)
Imports: 
    ggplot2 (>= 3.4.0),
    dplyr (>= 1.0.0),
    tidyr,
    magrittr,
    rlang
Suggests: 
    testthat (>= 3.0.0),
    knitr,
    rmarkdown,
    covr
Encoding: UTF-8
RoxygenNote: 7.2.3
```

### Package Code with Dependencies

```r
#' Function using imported dependencies
#' @param data A data frame
#' @return A filtered and mutated data frame
#' @importFrom dplyr filter mutate select
#' @export
process_data <- function(data, condition) {
  data |>
    filter(.data[[condition]] > 0) |>
    mutate(value_scaled = scale(value)) |>
    select(name, value_scaled)
}

#' Function with optional dependencies
#' @param data A data frame
#' @param use_viz Use visualization (requires ggplot2)
#' @return Plot or data frame
#' @importFrom dplyr filter
#' @export
analyze_data <- function(data, use_viz = FALSE) {
  filtered <- filter(data, value > 0)
  
  if (use_viz && requireNamespace("ggplot2", quietly = TRUE)) {
    ggplot2::ggplot(filtered, ggplot2::aes(x = name, y = value)) +
      ggplot2::geom_bar(stat = "identity")
  } else {
    filtered
  }
}
```

## Best Practices

1. **Minimize Dependencies**: Only include packages that are truly necessary for your package to function.

2. **Specify Versions**: Always specify minimum versions for critical dependencies to ensure compatibility.

3. **Use Imports Over Depends**: Prefer Imports over Depends to avoid namespace conflicts.

4. **Test Optional Features**: Wrap optional functionality in `requireNamespace()` checks.

5. **Keep Suggests Minimal**: Only suggest packages needed for examples, tests, or vignettes.

6. **Document Dependency Changes**: When adding new dependencies, explain why they are needed.

## Exercises

1. Create a new package and add three packages to Imports with specific versions.

2. Add conditional functionality using Suggests and requireNamespace().

3. Practice using importFrom in your NAMESPACE file.

4. Create a function that checks for optional dependencies and provides graceful degradation.

5. Review your dependency choices and remove any unnecessary packages.

## Additional Resources

- [Writing R Extensions](https://cran.r-project.org/doc/manuals/R-exts.html)
- [R Packages Book](https://r-pkgs.org/)
- [Dependency Management in R](https://cran.r-project.org/web/packages/packagemanager/index.html)