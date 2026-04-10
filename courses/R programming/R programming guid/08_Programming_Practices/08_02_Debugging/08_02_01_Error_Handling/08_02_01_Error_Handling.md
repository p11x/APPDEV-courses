# Error Handling in R

This chapter covers tryCatch, error generation, warnings, and custom error handling patterns in R.

## 1. Basic Error Generation

Errors halt execution and should indicate what went wrong.

### Using stop() and stopifnot()

```r
# Simple error with message
divide <- function(numerator, denominator) {
  if (denominator == 0) {
    stop("Division by zero is not allowed")
  }
  numerator / denominator
}

divide(10, 0)
# Error in divide(10, 0) : Division by zero is not allowed

# Using stopifnot for assertions
validate_positive <- function(x) {
  stopifnot(
    is.numeric(x),
    x > 0,
    length(x) == 1
  )
  TRUE
}

validate_positive(-5)
# Error: x > 0 is not TRUE

# More specific assertions
check_positive <- function(x) {
  if (!is.numeric(x)) {
    stop("Input must be numeric")
  }
  if (any(x <= 0, na.rm = TRUE)) {
    stop("All values must be positive")
  }
  return(TRUE)
}
```

### Error Classes

```r
# Define custom error class
my_error_class <- function(message, class = "custom_error") {
  err <- structure(
    list(message = message),
    class = c(class, "error", "condition")
  )
  err$call <- sys.call()
  err
}

# Throw custom error
throw_validation_error <- function(details) {
  msg <- paste("Validation failed:", details)
  err <- my_error_class(msg, "validation_error")
  stop(err)
}

# Handle custom errors
tryCatch(
  throw_validation_error("missing required field"),
  validation_error = function(e) {
    cat("Caught validation error:", e$message, "\n")
  }
)
```

## 2. tryCatch Fundamentals

tryCatch provides structured exception handling.

### Basic tryCatch Structure

```r
# Basic pattern
result <- tryCatch(
  {
    # Code to try
    data <- read.csv("data.csv")
    mean(data$value)
  },
  error = function(e) {
    # Handle error
    cat("Error occurred:", e$message, "\n")
    NA_real_
  }
)

# With success handler
result <- tryCatch(
  {
    data <- read.csv("data.csv")
    mean(data$value)
  },
  error = function(e) {
    cat("Error:", e$message, "\n")
    NA_real_
  },
  finally = {
    cat("Execution completed\n")
  }
)
```

### Multiple Condition Handlers

```r
# Handle different conditions
result <- tryCatch(
  {
    data <- read.csv("data.csv")
    
    if (nrow(data) == 0) {
      stop("Empty dataset")
    }
    
    mean(data$value)
  },
  error = function(e) {
    cat("Error:", e$message, "\n")
    NA_real_
  },
  warning = function(w) {
    cat("Warning:", w$message, "\n")
    invokeRestart("muffleWarning")
  },
  message = function(m) {
    cat("Message:", m$message, "\n")
    invokeRestart("muffleMessage")
  }
)
```

### tryCatch return Values

```r
# With success and error return values
safe_divide <- function(a, b) {
  tryCatch(
    {
      if (b == 0) stop("Division by zero")
      a / b
    },
    error = function(e) {
      message("Error in safe_divide: ", e$message)
      NA_real_  # Error return value
    }
  )
}

# result is a/b on success, NA on error
result <- safe_divide(10, 2)  # 5
result <- safe_divide(10, 0) # NA
```

## 3. Warning Handling

Warnings indicate potential issues without halting execution.

```r
# Generate warnings
calculate_mean <- function(x, na.rm = FALSE) {
  if (!is.numeric(x)) {
    stop("Input must be numeric")
  }
  
  if (na.rm && any(is.na(x))) {
    warning("NAs present and removed")
  }
  
  mean(x, na.rm = na.rm)
}

# Suppress warnings (temporarily)
result <- suppressWarnings(calculate_mean(c(1, 2, NA)))

# Capture and handle warnings
withWarnings <- function(expr) {
  warnings <- character()
  
  w_handler <- function(w) {
    warnings <<- c(warnings, w$message)
    invokeRestart("muffleWarning")
  }
  
  result <- withCallingHandlers(
    expr,
    warning = w_handler
  )
  
  list(result = result, warnings = warnings)
}

# Usage
test <- withWarnings(warning("First warning"); warning("Second warning"); 42)
test$warnings
# [1] "First warning" "Second warning"
```

## 4. Advanced tryCatch Patterns

### Retrying Failed Operations

```r
# Retry pattern with backoff
retry_function <- function(f, 
                         max_attempts = 3, 
                         delay = 1,
                         exponential = TRUE) {
  
  attempt <- 1
  last_error <- NULL
  
  while (attempt <= max_attempts) {
    result <- tryCatch(
      {
        f()
      },
      error = function(e) {
        last_error <<- e
        NULL
      }
    )
    
    if (!is.null(result)) {
      return(result)
    }
    
    wait_time <- if (exponential) {
      delay * 2^(attempt - 1)
    } else {
      delay
    }
    
    cat("Attempt", attempt, "failed. Retrying in", wait_time, "seconds\n")
    Sys.sleep(wait_time)
    attempt <- attempt + 1
  }
  
  stop("Failed after", max_attempts, "attempts: ", last_error$message)
}

# Usage - retry file download
download_with_retry <- function(url, file, max_attempts = 3) {
  retry_function(
    function() download.file(url, file, method = "auto"),
    max_attempts = max_attempts
  )
}
```

### Creating Custom Error Classes

```r
# Define error classes for different failure types
validation_error <- function(message) {
  structure(
    list(message = message, call = sys.call(-1)),
    class = c("validation_error", "error", "condition")
  )
}

data_error <- function(message) {
  structure(
    list(message = message, call = sys.call(-1)),
    class = c("data_error", "error", "condition")
  )
}

processing_error <- function(message) {
  structure(
    list(message = message, call = sys.call(-1)),
    class = c("processing_error", "error", "condition")
  )
}

# Throw errors
validate_data <- function(df) {
  if (!is.data.frame(df)) {
    stop(validation_error("Input must be a data frame"))
  }
  
  if (nrow(df) == 0) {
    stop(data_error("Data frame is empty"))
  }
  
  TRUE
}

# Handle with specific handlers
tryCatch(
  validate_data("not a dataframe"),
  validation_error = function(e) {
    cat("Validation failed:", e$message, "\n")
  },
  data_error = function(e) {
    cat("Data error:", e$message, "\n")
  },
  error = function(e) {
    cat("General error:", e$message, "\n")
  }
)
```

### Pattern Matching in Errors

```r
# Check error message patterns
handle_errors <- function(code) {
  tryCatch(
    eval(code),
    error = function(e) {
      if (grepl("connection", e$message, ignore.case = TRUE)) {
        cat("Network issue - check connection\n")
        return(NULL)
      }
      
      if (grepl("permission denied", e$message, ignore.case = TRUE)) {
        cat("Permission issue - check file permissions\n")
        return(NULL)
      }
      
      if (grepl("not found", e$message, ignore.case = TRUE)) {
        cat("File not found\n")
        return(NULL)
      }
      
      stop(e)  # Re-throw unrecognized errors
    }
  )
}
```

## 5. Safety Wrapper Patterns

### Creating Safe Versions of Functions

```r
# Safe read - returns NULL on error
safe_read.csv <- function(file, ...) {
  tryCatch(
    read.csv(file, ...),
    error = function(e) {
      message("Error reading ", file, ": ", e$message)
      NULL
    }
  )
}

# Safe file.exists check
file_exists <- function(path) {
  tryCatch(
    file.exists(path),
    error = function(e) FALSE
  )
}

# Safe load - load RData with error handling
safe_load <- function(file) {
  tryCatch(
    load(file)
    # load loads into environment, returns character vector of objects
  ,
    error = function(e) {
      message("Error loading ", file, ": ", e$message)
      character(0)
    }
  )
}

# Safe source - R code execution
safe_source <- function(file, ...) {
  tryCatch(
    source(file, ...)$value,
    error = function(e) {
      message("Error in ", file, ": ", e$message)
      NULL
    }
  )
}
```

### Optional Arguments with Fallbacks

```r
# Function with fallback on error
with_fallback <- function(primary, fallback = NULL) {
  result <- tryCatch(
    primary(),
    error = function(e) {
      message("Primary failed: ", e$message)
      fallback
    }
  )
  
  if (is.null(result)) {
    message("Using fallback")
  }
  
  return(result)
}

# Usage
data <- with_fallback(
  primary = function() read.csv("primary.csv"),
  fallback = read.csv("backup.csv")
)
```

## 6. Condition Handling Utilities

### Custom try wrapper

```r
# Enhanced try function
try_ <- function(expr, 
                 silent = TRUE,
                 error_value = NULL,
                 warning_handler = NULL) {
  
  result <- tryCatch(
    withCallingHandlers(
      expr,
      warning = if (!is.null(warning_handler)) {
        function(w) {
          warning_handler(w)
          invokeRestart("muffleWarning")
        }
      }
    ),
    error = function(e) {
      if (!silent) {
        message("Error: ", e$message)
      }
      error_value
    }
  )
  
  return(result)
}

# Usage
data <- try_(read.csv("data.csv"), error_value = data.frame())
```

### Logging Errors

```r
# Log errors to file
log_error <- function(e, log_file = "error.log") {
  timestamp <- format(Sys.time(), "%Y-%m-%d %H:%M:%S")
  log_entry <- paste0("[", timestamp, "] ", e$message, "\n")
  cat(log_entry, file = log_file, append = TRUE)
}

# Use in error handler
safe_operation <- function(code) {
  tryCatch(
    code,
    error = function(e) {
      log_error(e, "error.log")
      NULL
    }
  )
}
```

## Summary

- Use `stop()` to generate errors with descriptive messages
- Use `warning()` for non-fatal issues
- tryCatch handles errors with custom handlers
- Return meaningful values on error (not just NULL)
- Create custom error classes for specific failure types
- Consider retry logic for intermittent failures
- Use `suppressWarnings()` for expected warnings
- Always provide informative error messages