# Debugging Techniques in R

This chapter covers debugging tools and techniques in R including browser(), debug(), traceback, and str().

## 1. Interactive Debugging with browser()

The browser() function pauses execution and allows step-by-step inspection.

### Basic browser() Usage

```r
# Add browser() to pause execution
debug_function <- function(x, y) {
  browser()  # Execution pauses here
  
  result <- x + y
  result * 2
}

# Run function - R will enter browser mode
# Commands in browser:
# n - next (execute next line)
# c - continue (finish function)
# s - step into function
# Q - quit browser

debug_function(5, 3)

# Conditional browser
debug_conditional <- function(data) {
  if (nrow(data) > 1000) {
    browser()  # Only pause for large datasets
  }
  mean(data$value)
}

# Using condition argument
debug_once <- function(x) {
  browser(condition = NULL, env = parent.frame())
  # Can trigger with: browser(text = "Pause here")
}
```

### Browser Commands

```r
# Full browser workflow
interactive_debug <- function(df) {
  # Enter browser at start
  browser()
  
  # Examine data
  cat("Data dimensions:", dim(df), "\n")
  cat("Column names:", names(df), "\n")
  
  # Step through each row processing
  for (i in 1:nrow(df)) {
    browser()  # Pause each iteration
    
    row <- df[i, ]
    cat("Row", i, "value:", row$score, "\n")
  }
  
  cat("Processing complete\n")
}

# In browser, use:
# n - next line
# c - continue to next breakpoint
# s - step into function call
# f - finish current function
# Q - quit and return
# where - show call stack
# print(var) - print variable
# ls() - list environment
```

### Debugging with Options

```r
# Enable browser on error
options(error = browser)

# Drop into debugger on error (with stack trace)
options(error = traceback)

# Or: call errorHandler on error
options(error = function() {
  cat("Error occurred!\n")
  browser()
})

# Example: triggers browser on error
divide <- function(a, b) {
  if (b == 0) stop("Division by zero")
  a / b
}

divide(10, 0)  # Enters browser mode
```

## 2. Using debug() and debugonce()

debug() marks a function for debugging; debugonce() debugs for one call.

### Setting Up Debug Mode

```r
# Mark function for debugging
debug(mean)

# Now每次 call mean enters browser
mean(c(1, 2, 3))
# Browse[1]>

# Undebug to stop
undebug(mean)

# Debug with custom function
debug(sum, signature = c(x = "ANY"))
debugonce(mean)  # Debug for next call only
```

### Debug User-Defined Functions

```r
# Debug a custom function
calculate_stats <- function(data, group = NULL) {
  if (is.null(group)) {
    result <- list(
      mean = mean(data),
      median = median(data),
      sd = sd(data)
    )
  } else {
    result <- tapply(data, group, function(x) {
      c(mean = mean(x), sd = sd(x))
    })
  }
  return(result)
}

debug(calculate_stats)
calculate_stats(mtcars$mpg)

# Inside browser you can:
# - Examine variables
# - Step through code
# - Modify variables
# - Continue execution
```

### Debug with Breakpoints

```r
# Set breakpoint (alternative to browser())
# In RStudio: click next to line number

# Or programmatically
setBreakpoint("R/my_function.R")

# List all breakpoints
# In RStudio: View > Breakpoints

# Remove breakpoints
removeBreakpoint("R/my_function.R")
```

## 3. traceback() Function

traceback() shows the call stack after an error.

### Basic traceback

```r
# Create nested function calls
level1 <- function() level2()
level2 <- function() level3()
level3 <- function() stop("Error at deepest level")

# Get traceback after error
tryCatch(
  level1(),
  error = function(e) {
    cat("Error:", e$message, "\n\n")
    cat("Call stack:\n")
    print(traceback())
  }
)

# Shows:
# 8. stop("Error at deepest level")
# 7. level3()
# 6. level2()
# 5. level1()
# 4. eval(expr)
# 3. tryCatchOne
# 2. tryCatchList
# 1. fn(expr)

# More compact traceback
tryCatch(
  level1(),
  error = function(e) {
    print(traceback(max.print = 3))
  }
)
```

### Saving traceback

```r
# Option to save traceback automatically
options(KeepSource = TRUE)

# Print with more detail
tryCatch(
  level1(),
  error = function(e) {
    tb <- traceback()
    cat("Full traceback:\n")
    print(tb)
  }
)

# Using rlang for better traceback
install.packages("rlang")
library(rlang)

# rlang last_trace
rlang::last_error()
rlang::last_trace()
```

## 4. str() for Object Inspection

str() provides compact structure display of R objects.

### Basic str() Usage

```r
# Simple vector
str(c(1, 2, 3))
# num [1:3] 1 2 3

# More complex
str(mtcars)
# 'data.frame': 32 obs. of  11 variables:
#  $ mpg : num  21 21 22.8 21.4 18.7 ...
#  $ cyl : num  6 6 4 6 8 ...
#  $ disp: num  160 160 108 258 ...
#  ...

# Lists
str(list(a = 1, b = "hello"))
# List of 2
#  $ a: num 1
#  $ b: chr "hello"
```

### Controlling str() Output

```r
# Limit depth
str(mtcars, list.len = 5)
# Only first 5 components

# Limit character length
str(long_string <- paste(letters, collapse = ""), nchar.max = 50)
# chr "abcdefghijklmnopqrstuvwxyz"

# Give more detail
str(mtcars, width = 60, strict.width = "cut")

# dump to file
str(mtcars, dump.file = "structure.txt")
```

### str() for Different Objects

```r
# Function
str(mean)
# function (x, ...)
#  -.attr*(, "class")= chr [2] "function" "builtin"

# Model object
model <- lm(mpg ~ cyl + disp, data = mtcars)
str(model)
# List of 12
#  $ coefficients : Named num [1:3] 34.85 -1.58 -0.02 ...
#  $ residuals    : Named num [1:32] -2.57 0.87 1.59 ...
#  ...

# List with nested objects
nested <- list(
  level1 = list(
    level2 = list(
      level3 = "deep value"
    )
  )
)
str(nested, max.level = 2)
# List of 1
#  $ level1: List of 1
#   ..$ level2:List [1]
#   .. ..$ level3: chr "deep value"
```

## 5. Other Debugging Functions

### dump.frames()

```r
# Dump frames to file for post-mortem debugging
options(error = dump.frames)

# Creates "last.dump.rds" on error

# Later, load and examine
load("last.dump.rds")
debugger()

# Or use
rlang::entrace()
```

### recover()

```r
# Interactive error handler
options(error = recover)

# After error, shows call stack menu
# Select frame number to enter

# Example
divide(10, 0)
# Select 1: divide(10, 0)
# Selection: 
```

### browser() with Environment Access

```r
# Inspect function environment
inspect_env <- function(data) {
  cat("Function environment:\n")
  print(ls(envir = environment()))
  print(parent.env(environment()))
  
  browser()
}

# Access variables explicitly
browse_vars <- function() {
  x <- 10
  y <- 20
  browser()
}
```

### Using alternative packages

```r
# debugme package
library(debugme)
mark_debug("package:function")

# In RStudio:
# - View > Debug - Debug breakpoints
# - Click line numbers to set breakpoints
# - Use "Rerun with Debug" from Run menu
```

## 6. Systematic Debugging Approach

### Step-by-Step Process

```r
# 1. Identify the problem
# - What is the expected output?
# - What actually happens?

# 2. Create minimal reproducible example
# - Isolate the failing code
# - Remove unrelated variables

# 3. Check intermediate values
# - Use print() or cat() statements
browser()

# 4. Examine error conditions
# - Check data types, dimensions
str(data)

# 5. Test components individually
# - Run each line separately
# - Test with known values
```

### Using cat() for Debugging

```r
# Add print statements
debug_print <- function(data) {
  cat("Input data:\n")
  print(head(data))
  
  result <- process_data(data)
  cat("Processed result:\n")
  print(head(result))
  
  return(result)
}

# Using message for debugging
debug_message <- function(x) {
  message("Debug: x = ", x)
  # ... code
}
```

### Isolating Issues

```r
# Test function components separately
test_composite <- function(data) {
  # Step 1: Check input
  cat("Input check:\n")
  str(data)
  
  # Step 2: First transformation
  step1 <- transform1(data)
  cat("After transform1:\n")
  str(step1)
  
  # Step 3: Second transformation
  step2 <- transform2(step1)
  cat("After transform2:\n")
  str(step2)
  
  return(step2)
}

# Add early return for testing
test_isolated <- function(data) {
  if (test_mode) {
    return(data)  # Skip processing in test mode
  }
  # ... actual code
}
```

## Summary

- Use `browser()` to pause execution and inspect
- Use `debug()` to enable function debugging
- Use `traceback()` to see call stack after errors
- Use `str()` to inspect object structure
- Systematic debugging: isolate, test, verify
- Consider RStudio's debugging interface
- Use `options(error = recover)` for interactive debugging