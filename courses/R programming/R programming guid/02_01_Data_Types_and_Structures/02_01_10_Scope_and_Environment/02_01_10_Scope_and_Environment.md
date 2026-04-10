# Scope and Environment in R

## Learning Objectives

- Understand R's scoping rules
- Master environments and their hierarchy
- Understand closures and lexical scoping
- Apply lexical scoping in function design

## Theoretical Background

### What is Scope?

Scope determines how R looks up values of variables. R uses lexical scoping, meaning variables are resolved based on where they were defined in the code, not where they are called from.

### Environment Hierarchy

```
mermaid
graph TD
    A[Global Environment] --> B[Namespace: base]
    A --> C[Namespace: stats]
    A --> D[Namespace: utils]
    A --> E[Package Environments]
    A --> F[Function Environments]
```

## Code Examples

### Standard Example: Scoping Rules

```r
# ===== SCOPING IN R =====

cat("===== BASIC SCOPING =====\n\n")

# Global variable
x <- 10

# Function with same-named local variable
test_scope <- function() {
  x <- 20  # This is local to the function
  cat("Inside function, x =", x, "\n")
}

test_scope()
cat("Outside function, x =", x, "\n")

# Using <<- for global assignment
cat("\n===== GLOBAL ASSIGNMENT =====\n\n")

modify_global <- function() {
  x <<- 100  # Modifies global x
  cat("Inside function, x =", x, "\n")
}

modify_global()
cat("After function call, x =", x, "\n")

# Environment example
cat("\n===== ENVIRONMENTS =====\n\n")
cat("Current environment:\n")
print(environment())

cat("\nFunction environment:\n")
test_env <- function() {
  print(environment())
}
test_env()
```

**Output:**
```
===== BASIC SCOPING =====

Inside function, x = 20
Outside function, x = 10

===== GLOBAL ASSIGNMENT =====

Inside function, x = 100
After function call, x = 100

===== ENVIRONMENTS =====

Current environment:
<environment: R_GlobalEnv>
```

### Real-World Example: Closure Pattern

```r
# Real-world: Using closures for function factories
cat("===== FUNCTION FACTORY (CLOSURES) =====\n\n")

# Create a function that generates counter functions
make_counter <- function(start = 0) {
  count <- start  # This variable is enclosed
  
  # Return a function that accesses count
  function(delta = 1) {
    count <<- count + delta  # Modifies the enclosed variable
    return(count)
  }
}

# Create two independent counters
counter1 <- make_counter(0)
counter2 <- make_counter(100)

cat("Counter 1:\n")
cat("  Start:", counter1(0), "\n")
cat("  After +1:", counter1(1), "\n")
cat("  After +1:", counter1(1), "\n")

cat("\nCounter 2:\n")
cat("  Start:", counter2(0), "\n")
cat("  After +1:", counter2(1), "\n")

cat("\n===== DATA PIPELINE WITH ENVIRONMENTS =====\n\n")

# Create a simple pipeline function
make_pipeline <- function() {
  # Private state
  data <- NULL
  transformations <- list()
  
  # Add data
  add_data <- function(new_data) {
    data <<- new_data
    invisible(NULL)
  }
  
  # Add transformation
  add_transform <- function(func, name) {
    transformations[[length(transformations) + 1]] <<- list(
      func = func,
      name = name
    )
    invisible(NULL)
  }
  
  # Run pipeline
  run <- function() {
    result <- data
    for (t in transformations) {
      result <- t$func(result)
    }
    return(result)
  }
  
  # Return list of functions (closure)
  list(
    add_data = add_data,
    add_transform = add_transform,
    run = run,
    get_data = function() data
  )
}

# Use the pipeline
pipeline <- make_pipeline()
pipeline$add_data(c(1, 2, 3, 4, 5))
pipeline$add_transform(function(x) x * 2, "double")
pipeline$add_transform(function(x) x + 10, "add_10")

cat("Original data: c(1, 2, 3, 4, 5)\n")
cat("After pipeline:", pipeline$run(), "\n")
```

**Output:**
```
===== FUNCTION FACTORY (CLOSURES) =====

Counter 1:
  Start: 0
  After +1: 1
  After +1: 2

Counter 2:
  Start: 100
  After +1: 101
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use lexical scoping to create private state
2. Avoid <<- unless necessary (global state is dangerous)
3. Use function factories for related functions
4. Be aware of non-standard evaluation

### Common Pitfalls

1. Modifying global variables unintentionally
2. Forgetting that R evaluates in enclosing environment
3. Confusion between = and <- in different contexts
