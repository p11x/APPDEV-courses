# Debugging Tools in R

## Learning Objectives

- Master built-in debugging functions
- Learn to use RStudio debugging tools
- Implement systematic debugging workflows
- Apply debugging best practices

## Theory

R provides comprehensive debugging tools from basic print debugging to sophisticated interactive debuggers. Understanding these tools is essential for finding and fixing bugs in R code. The main tools include browser(), debug(), traceback(), and RStudio's visual debugger.

## Step-by-Step Guide

### Using traceback()

The traceback() function shows the call sequence that led to an error:

```r
# Enable tracing before error occurs
options(error = traceback)

# After an error
mean("hello")
traceback()
# 4: mean("hello") at #3:2
# 3: eval(expr, envir, enclos)
# 2: eval(expr, envir, enclos)
# 1: eval(expr, envir, enclos)
```

### Using browser()

browser() pauses execution and enters debugging mode:

```r
debugged_function <- function(x) {
  browser()  # Execution pauses here
  y <- x + 1
  z <- y * 2
  return(z)
}

debugged_function(5)
# Browse [1]>
# Commands: n (next), c (continue), s (step), Q (quit)
```

### Using debug() and undebug()

The debug() function marks a function for debugging:

```r
debug(my_function)
my_function(argument)
undebug(my_function)
```

### Using recover()

recover() provides an interactive menu after errors:

```r
options(error = recover)
mean("hello")
# Choose a frame to examine:
# 1: mean("hello")
# 2: eval(expr, envir, enclos)
# Selection: 
```

## Code Examples

### Setting Conditional Breaks

```r
# Break on condition
debugged_function <- function(df) {
  for (i in seq_len(nrow(df))) {
    if (is.na(df[i, "value"])) {
      browser()  # Will pause here for NA values
    }
    df[i, "value"] <- df[i, "value"] * 2
  }
  df
}
```

### Using debugonce()

```r
# Debug once, then automatically undebug
debugonce(my_function)
my_function(arguments)
```

### Using setBreakpoint()

```r
# Set a breakpoint at a specific line
setBreakpoint("R/file.R:10")
```

### Debugging with RStudio

```r
# Click beside line number in RStudio
# Or use keyboard shortcut F12

# RStudio debugging controls:
# - "C" or F5: Continue
# - "N" or F10: Next
# - "S" or F11: Step Into
# - "Q" or Shift+F5: Stop
```

### Building Debug Helper Functions

```r
# Print variable inspection
d <- function(x, label = deparse(substitute(x))) {
  cat("=== ", label, " ===\n")
  print(head(x))
  cat("Mode:", mode(x), "\n")
  cat("Class:", class(x), "\n")
  cat("Length:", length(x), "\n")
  cat("--------------------\n")
}

# Usage
d(mtcars)
```

## Best Practices

1. **Start with traceback()**: Always begin debugging with traceback().

2. **Minimize Reproducible Examples**: Isolate the bug to minimal code.

3. **Use Systematic Approach**: Change one thing at a time.

4. **Log Important Values**: Add print statements in strategic locations.

5. **Use Version Control**: Save working versions before major changes.

6. **Test Incrementally**: Run tests after each change.

## Exercises

1. Practice using traceback() after errors.

2. Use browser() to step through a function.

3. Add debugging code to your own functions.

4. Use recover() for interactive error debugging.

5. Create debugging helper functions.

## Additional Resources

- [R Debugging Reference Card](https://cran.r-project.org/doc/manuals/R-refcard.html)
- [RStudio Debugging](https://support.rstudio.com/hc/en-us/articles/205612627-Debugging-with-RStudio)