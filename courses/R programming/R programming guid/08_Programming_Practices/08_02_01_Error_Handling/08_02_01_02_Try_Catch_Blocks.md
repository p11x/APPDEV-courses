# Try-Catch Blocks in R

## Learning Objectives

- Master the tryCatch function for error handling
- Learn to handle different error conditions
- Implement custom error handlers
- Create robust error recovery mechanisms

## Theory

The tryCatch mechanism in R provides a powerful way to handle errors and warnings. It allows code to be executed with the ability to catch and handle any errors or warnings that occur during execution, enabling graceful error recovery instead of program termination.

## Step-by-Step Guide

### Basic tryCatch Syntax

The tryCatch function evaluates an expression and handles any conditions that occur:

```r
tryCatch(
  {
    # Expression to try
    result <- mean(my_data$value)
  },
  error = function(condition) {
    # Handle error
    message("Error occurred: ", condition$message)
    NA
  }
)
```

### Handling Warnings

Warnings can be handled separately from errors:

```r
tryCatch(
  {
    result <- mean(c(1, 2, NA), na.rm = TRUE)
  },
  warning = function(w) {
    message("Warning: ", w$message)
    NA
  }
)
```

### Multiple Condition Handlers

Different handlers can be specified for different condition types:

```r
tryCatch(
  {
    result <- dangerous_operation()
  },
  error = function(e) {
    # Handle errors
    message("Error: ", e$message)
    NULL
  },
  warning = function(w) {
    # Handle warnings
    message("Warning: ", w$message)
    NA
  },
  finally = {
    # Cleanup code - always runs
    message("Operation complete")
  }
)
```

## Code Examples

### Basic Error Handling

```r
# Safe division function
safe_divide <- function(numerator, denominator) {
  tryCatch(
    {
      if (denominator == 0) {
        stop("Division by zero")
      }
      numerator / denominator
    },
    error = function(e) {
      warning(e$message)
      NA
    }
  )
}

# Usage
safe_divide(10, 2)  # Returns 5
safe_divide(10, 0) # Returns NA with warning
```

### Reading Files with Error Handling

```r
# Safe file reading
read_csv_safe <- function(filepath) {
  tryCatch(
    {
      read.csv(filepath, stringsAsFactors = FALSE)
    },
    error = function(e) {
      if (grepl("No such file", e$message)) {
        message("File not found: ", filepath)
      } else {
        message("Error reading file: ", e$message)
      }
      NULL
    }
  )
}

# Safe RDS reading
read_rds_safe <- function(filepath) {
  tryCatch(
    readRDS(filepath),
    error = function(e) {
      message("Error: ", e$message)
      NULL
    }
  )
}
```

### Custom Error Classes

```r
# Validation error function
validate_input <- function(x, name = "input") {
  if (is.null(x)) {
    stop(sprintf("%s cannot be NULL", name))
  }
  if (length(x) == 0) {
    stop(sprintf("%s cannot be empty", name))
  }
  if (!is.numeric(x)) {
    stop(sprintf("%s must be numeric", name))
  }
  TRUE
}

# Use in function
process_values <- function(values) {
  tryCatch(
    {
      validate_input(values, "values")
      mean(values)
    },
    error = function(e) {
      message("Validation failed: ", e$message)
      NA
    }
  )
}
```

### Database Operations

```r
# Safe database query
query_database <- function(query, conn) {
  tryCatch(
    {
      DBI::dbSendQuery(conn, query)
    },
    error = function(e) {
      message("Query failed: ", e$message)
      NULL
    },
    finally = {
      # Close connection if open
      if (DBI::dbExists(conn)) {
        DBI::dbDisconnect(conn)
      }
    }
  )
}
```

### List-Based tryCatch

```r
# Process multiple files
process_files <- function(filepaths) {
  results <- lapply(filepaths, function(fp) {
    tryCatch(
      read.csv(fp),
      error = function(e) {
        message("Failed to read ", fp, ": ", e$message)
        NULL
      }
    )
  })
  
  # Filter out failures
  results[sapply(results, is.null)] <- NULL
  results
}
```

## Best Practices

1. **Specific Error Handling**: Handle specific errors rather than catching all.

2. **Informative Messages**: Provide meaningful error messages.

3. **Return Consistent Types**: Always return the same type from error handlers.

4. **Use finally for Cleanup**: Release resources in finally block.

5. **Don't Suppress All Errors**: Only handle expected error conditions.

6. **Log Errors**: Consider logging errors for debugging.

## Exercises

1. Implement tryCatch for file reading operations.

2. Create safe versions of risky operations.

3. Handle multiple error types differently.

4. Implement retry logic with tryCatch.

5. Create a validation framework using tryCatch.

## Additional Resources

- [R Language Definition - Conditions](https://cran.r-project.org/doc/manuals/R-intro.html#Conditions)
- [Advanced R - Exception handling](https://adv-r.hadley.nz/exceptions.html)