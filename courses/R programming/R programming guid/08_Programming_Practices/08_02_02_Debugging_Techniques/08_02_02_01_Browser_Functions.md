# Browser Functions for Debugging

## Learning Objectives

- Master browser() debugging function
- Learn navigation commands in browser mode
- Understand variable inspection in browser
- Implement effective debugging workflows

## Theory

The browser() function is R's built-in interactive debugger. When execution reaches browser(), it pauses and enters interactive debugging mode where you can inspect variables, execute code, and step through code line by line. This is the most powerful debugging tool in R.

## Step-by-Step Guide

### Basic browser() Usage

Insert browser() anywhere in your code to pause execution:

```r
my_function <- function(x, y) {
  result <- x + y
  browser()  # Execution pauses here
  result * 2
}

my_function(3, 4)
# Browse [1]>
```

### Browser Navigation Commands

In browser mode, several navigation commands are available:

- n or Enter: Execute next line
- c: Continue execution to next browser() or end
- s: Step into function calls
- f: Finish current function
- Q: Quit browser and return to prompt

### Examining Variables

In browser mode, examine variables directly:

```r
Browse [1]> ls()
Browse [1]> x
Browse [1]> print(x)
Browse [1]> str(x)
Browse [1]> head(dataframe)
```

## Code Examples

### Conditional browser()

```r
debug_process <- function(data, threshold = 100) {
  for (i in seq_along(data)) {
    value <- data[[i]]
    
    # Break only when value exceeds threshold
    if (value > threshold) {
      cat("Breaking at index", i, "with value", value, "\n")
      browser()
    }
  }
  data
}
```

### Debugging with Call Stack Access

```r
nested_function <- function(x) {
  browser()
  mean(x)
}

wrapper <- function(x) {
  y <- x + 1
  nested_function(y)
}

# In browser, access parent environment
Browse [1]> sys.parent()
Browse [1]> sys.frames()
Browse [1]> parent.frame()
```

### Interactive Variable Modification

```r
# Modify variables during debugging
correct_data <- function(data) {
  browser()
  data$value[data$value < 0] <- 0  # Fix negative values
  data
}
```

### Browser in Package Development

```r
# Add browser() to functions during development
process_data <- function(df) {
  browser()
  
  # Your development code here
  
  df |>
    filter(!is.na(value)) |>
    mutate(value = scale(value))
}
```

### Using where Parameter

```r
# browser() can be conditional
conditional_debug <- function(condition = TRUE) {
  browser(condition = condition)
  # Code continues only if condition is TRUE
}
```

## Best Practices

1. **Remove Before Production**: Remove browser() calls from production code.

2. **Use Conditional browser()**: Only pause when needed.

3. **Check Parent Frame**: Use parent.frame() to inspect calling environment.

4. **Use sys functions**: Access call stack with sys.* functions.

5. **Document Debug Points**: Add comments about why you need debugging.

## Exercises

1. Create functions with browser() statements.

2. Practice all navigation commands.

3. Inspect different variable types in browser.

4. Debug a nested function call.

5. Remove all browser() calls from production code.

## Additional Resources

- [R browser() Documentation](https://stat.ethz.ch/R-manual/R-devel/library/base/html/browser.html)
- [Advanced R - Debugging Techniques](https://adv-r.hadley.nz/debugging.html)