# Packages and Namespaces in R

This chapter covers package management, namespace concepts, and the search path in R.

## 1. Loading Packages

Packages extend R with new functions, datasets, and functionality. There are two primary functions for loading packages.

### library() vs require()

```r
# library() - stops with error if package not found
library(dplyr)  # Error in library(dplyr) : there is no package called 'dplyr'

# require() - returns TRUE/FALSE, suitable for conditional loading
if (require(dplyr)) {
  # Use dplyr functions
  df %>% filter(condition)
} else {
  # Install package or use base R alternative
  install.packages("dplyr")
  library(dplyr)
}

# Safer approach with requireNamespace
if (requireNamespace("dplyr", quietly = TRUE)) {
  library(dplyr)
} else {
  message("Consider installing dplyr for enhanced data manipulation")
}
```

### Loading Multiple Packages

```r
# Load core packages for data analysis workflow
required_packages <- c(
  "dplyr",    # Data manipulation
  "tidyr",   # Data tidying
  "ggplot2", # Visualization
  "readr",   # Data import
  "stringr"  # String manipulation
)

# Install missing packages
missing <- required_packages[!(required_packages %in% installed.packages()[, "Package"])]
if (length(missing) > 0) {
  install.packages(missing)
}

# Load all packages
for (pkg in required_packages) {
  library(pkg)
}
```

### Package Conflicts

```r
# Conflict detection
conflicts(detail = TRUE)
# Shows all conflicts between packages in search path

# Resolve conflicts explicitly
dplyr::filter   # Explicitly use dplyr's filter
base::filter    # Explicitly use base R's filter

# Common conflicts
# - dplyr::filter vs stats::filter
# - dplyr::select vs MASS::select
# - dplyr::rename vs reshape::rename
```

## 2. Understanding Namespaces

Namespaces prevent name collisions and control what is exported from packages.

### Namespace Concepts

```r
# View namespace of a package
getNamespace("dplyr")
getNamespaceExports("dplyr")

# Check if function exists in package
exists("filter", where = "package:dplyr", mode = "function")

# Get function from specific namespace
filter_fn <- getFromNamespace("filter", "dplyr")
```

### Importing Specific Functions

```r
# Import only what you need
library(core packages = c("select", "filter", "mutate"))

# Or use :: for explicit calling
dplyr::select(mtcars, mpg, cyl)
dplyr::filter(mtcars, mpg > 20)
dplyr::mutate(mtcars, mpg_double = mpg * 2)
```

### Namespace Files in Packages

When creating packages, NAMESPACE file controls exports:

```r
# NAMESPACE file example
export("my_function")
export("another_function")
exportClasses("MyClass")
exportMethods("print", "summary")

# Import specific functions from dependencies
importFrom(dplyr, select, filter, mutate)
importFrom(ggplot2, ggplot, aes, geom_point)
```

## 3. The Search Path

R maintains a search path defining where to look for functions and objects.

### Understanding Search Path

```r
# View current search path
search()
# [1] ".GlobalEnv"        "package:stats"    
# [3] "package:graphics"  "package:grDevices"
# [5] "package:utils"     "package:datasets"  
# [7] "package:methods"   "Autoloads"        
# [9] "package:base"

# After loading packages
library(dplyr)
search()
# [1] ".GlobalEnv"        "package:dplyr"    
# [3] "package:stats"     ...

# Find where function is located
find("filter")
# [1] "package:dplyr"  "package:stats"

# Find where object is found
find("mean")
# [1] "package:base"
```

### Manipulating Search Path

```r
# Attach adds package to search path (like library but for data)
my_data <- data.frame(x = 1:10, y = rnorm(10))
attach(my_data)
search()  # Includes "my_data"

# Access variables directly
x  # Works now
y

# Always detach when done
detach(my_data)

# Using: with() for temporary access
with(my_data, mean(x))
# [1] 5.5
```

## 4. Package Installation Management

### Installing Packages

```r
# Install from CRAN
install.packages("tidyverse")

# Install specific version
install.packages("ggplot2", version = "3.4.0")

# Install from GitHub
install_github("tidyverse/ggplot2")

# Install from Bioconductor
BiocManager::install("DESeq2")

# Install from local source
install.packages("path/to/package.tar.gz", repos = NULL, type = "source")
```

### Managing Installed Packages

```r
# List installed packages
installed.packages()[, c("Package", "Version", "LibPath")]

# Update.packages()
update.packages()

# Remove packages
remove.packages("package_name")

# Check package location
find.package("dplyr")
```

### Package Dependencies

```r
# View package dependencies
packageDescription("dplyr")$Depends
packageDescription("dplyr")$Imports
packageDescription("dplyr")$Suggests

# All reverse dependencies (what depends on this package)
tools::package_dependencies("dplyr", reverse = TRUE)
```

## 5. Package Version Management

```r
# Check version
packageVersion("dplyr")
# [1] ‘1.1.0’

# Compare versions
packageVersion("dplyr") >= "1.0.0"
# TRUE

# Specific version comparison
ver <- packageVersion("dplyr")
ver[[1]] == 1 && ver[[2]] >= 1
```

## 6. Namespace Isolation

Understanding how namespaces provide isolation:

```r
# Exported vs non-exported functions
# Exported: available to users
# Non-exported: internal only, but accessible

# Access non-exported function
dplyr:::filter_impl  # Triple colon gets internal function

# Why this matters - understanding package internals
# But don't rely on non-exported functions in production code
```

### S3 Method Dispatch and Namespaces

```r
# S3 methods are registered in namespace
# View registered methods for a generic
getS3method("print", "data.frame")
getS3method("summary", "lm")

# Method dispatch respects namespace
methods("print")
```

## 7. Package Citations

```r
# Cite a package
citation("dplyr")

# In publications
toBibtex(citation("dplyr"))
```

## Summary

- Use `library()` for required packages, `require()` for optional
- Namespaces prevent name conflicts between packages
- Search path determines order of function lookup
- Use `::` for explicit namespace access
- Attach/detach for temporary variable access (with caution)
- Package version management via `packageVersion()`
- Internal functions accessible with `:::` but avoid in production