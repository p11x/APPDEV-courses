# Error Types in R

## Learning Objectives

- Understand the different types of errors in R
- Learn to identify and classify errors
- Master error recognition for debugging
- Implement proper error handling strategies

## Theory

R has several distinct error types that help developers diagnose problems. Understanding these error types is essential for effective debugging. The main categories include syntax errors, runtime errors, and logical errors, each requiring different debugging approaches.

## Step-by-Step Guide

### Syntax Errors

Syntax errors occur when R cannot parse the code. These are caught during parsing before execution:

```r
# Missing parenthesis
if (x > 0 {
  print("positive")
}

# Missing comma
c(1 2 3)

# Unclosed string
"hello world
```

### Runtime Errors

Runtime errors occur during code execution. These are caught by the tryCatch mechanism:

```r
# Division by zero
10 / 0

# Invalid subsetting
vec <- c(1, 2, 3)
vec[10]

# Non-existent object
nonexistent_object

# Invalid function argument
mean("hello")
```

### Logical Errors

Logical errors produce unexpected results without warnings:

```r
# Off-by-one errors
1:10[10]  # Returns 10, not 10-element vector

# Operator precedence
a <- b <- c <- 1  # All get assigned 1

# Floating point comparison
0.1 + 0.2 == 0.3  # FALSE due to floating point
```

### Warning Messages

Warnings indicate potential problems but don't stop execution:

```r
# Coercion warning
as.numeric(c("1", "2", "a"))

# Longer object warning
c(1, 2) + c(1, 2, 3)

# NULL replacement
x <- NULL
x[1] <- 5
```

## Code Examples

### Error Identification Functions

```r
# Check if object exists
exists("my_object")

# Check object type
is.integer(1L)
is.numeric(1)
is.character("a")
is.vector(1)

# Check for NA values
is.na(NA)
anyNA(vec)

# Check object properties
is.finite(c(1, Inf, NA))
is.infinite(Inf)
```

### Working with Error Messages

```r
# Capture error information
tryCatch(
  mean("hello"),
  error = function(e) {
    message("Error: ", e$message)
    NULL
  }
)

# Custom error class
stopifnot <- function(condition, msg) {
  if (!condition) {
    stop(msg, call. = FALSE)
  }
}
```

### Error Classes

```r
# Custom error class
custom_error <- function(message, class = "custom_error") {
  err <- simpleError(message)
  class(err) <- c(class, "error", "condition")
  err
}

# Throw custom error
throw_custom <- function(x) {
  if (x < 0) {
    stop(custom_error("x must be non-negative"))
  }
  x
}
```

### Debugging Error Flows

```r
# Using traceback to see call sequence
traceback()

# Using browser for interactive debugging
debug(function_to_debug)
function_to_debug(argument)
undebug(function_to_debug)

# Using options for error debugging
options(error = recover)
mean("hello")
```

## Best Practices

1. **Read Error Messages Carefully**: Error messages often indicate the exact problem location.

2. **Use traceback()**: See the call sequence that led to the error.

3. **Isolate Problems**: Create minimal reproducible examples.

4. **Check Class Types**: Use is.* functions before operations.

5. **Handle Expected Errors**: Use tryCatch for predictable error conditions.

6. **Test Incrementally**: Run code after each change to catch errors early.

## Exercises

1. Create code that produces each error type.

2. Use traceback() to understand error propagation.

3. Implement proper error handling for a function.

4. Distinguish between warnings and errors.

5. Debug an existing R script with errors.

## Additional Resources

- [R Debugging Tools](https://cran.r-project.org/doc/manuals/R-intro.html#Debugging)
- [Advanced R - Debugging](https://adv-r.hadley.nz/debugging.html)