# Naming Conventions in R

## Learning Objectives

- Learn naming conventions for R objects
- Apply consistent naming across projects
- Master naming best practices
- Avoid naming conflicts

## Theory

Consistent naming conventions make code self-documenting and easier to maintain. Good names clearly communicate the purpose of objects, making code more readable for both the original author and others.

## Step-by-Step Guide

### General Principles

- Use lowercase letters
- Use underscores (_) to separate words
- Use descriptive names
- Be consistent throughout

### File Names

```r
# Lowercase, descriptive, hyphenated
# Good: 01_02_03_load_data.R
#       02_predict_values.R
#       03_visualize_results.R

# Bad:  load.R
#      my_code.R
#      Load_data.R
```

### Variable Names

```r
# Good: descriptive_nouns
patient_name <- "John"
number_of_records <- 100
mean_income <- 50000

# Bad: single letters (except i, j, x, y)
# Bad: abbreviations without context
# Bad: Hungarian notation
n <- 100  # OK for loop counters
df <- data.frame()  # OK for data frames in examples
```

### Function Names

```r
# Verbs that describe action
calculate_mean <- function(x) mean(x)
filter_data <- function(df, condition) df[df$condition, ]
load_csv <- function(filepath) read.csv(filepath)
validate_input <- function(x) TRUE

# Bad: nouns (unless S3/S4 methods)
mean <- function(x) {}  # Conflicts with base R
MyFunction <- function() {}  # Not conventional
```

### Constant Names

```r
# Uppercase with prefix
MAX_THRESHOLD <- 100
DEFAULT_PATH <- "/data"
API_KEY <- Sys.getenv("API_KEY")  # For secrets
```

### S3/S4 Methods

```r
# Use class name in method
predict.lm <- function(object, ...) {}
print.data.frame <- function(x, ...) {}

# Generic uses dot notation
plot.data <- function(x, ...) {}
```

## Code Examples

### Data Frame Column Names

```r
# Good: lowercase, descriptive
df <- data.frame(
  patient_id = 1:10,
  visit_date = Sys.Date(),
  blood_pressure = c(120, 130, 110, 125, NA)
)

# Bad: mixed case or abbreviations
df <- data.frame(
  PatID = 1:10,  # Avoid
  visitDt = Sys.Date(),  # Avoid
  BP = c()  # Avoid
)
```

### Naming Conflicts

```r
# Using prefix to avoid conflicts
# Good: avoid using base R names
base_mean <- base::mean  # Store original

# Using namespace explicitly
dplyr::filter()
stats::sd()
```

### Namespace in Function Arguments

```r
# Good: descriptive argument names
calculate_summary <- function(data_frame, 
                          group_variable,
                          summary_statistic) {
  # Code here
}
```

### Naming Patterns

```r
# Is-prefix for logical variables
is_valid <- TRUE
is_complete <- FALSE
has_data <- TRUE

# .onLoad, .onAttach for namespace files
.onLoad <- function(libname, pkgname) {}

# calc prefix for calculation functions
calc_mean <- function(x) mean(x)
calc_sd <- function(x) sd(x)
```

## Best Practices

1. **Be Descriptive**: Names should communicate purpose.

2. **Use Consistent Style**: Pick one convention.

3. **Avoid Conflicts**: Don't override base R functions.

4. **Check Packages**: Ensure no conflicts with dependencies.

5. **Validate**: Test that names work across platforms.

## Exercises

1. Rename variables in existing code.

2. Review and fix naming in a project.

3. Create naming conventions for your team.

4. Avoid naming conflicts with dependencies.

5. Document naming conventions.

## Additional Resources

- [Tidyverse Names](https://style.tidyverse.org/)
- [CRAN Naming](https://cran.r-project.org/web/packages/policies.html)