# Control Structures in R

## Learning Objectives

- Understand control flow structures in R
- Master conditional statements (if-else, switch)
- Master loops (for, while, repeat)
- Apply control structures in programming

## Theoretical Background

### What are Control Structures?

Control structures determine the order in which statements are executed. R provides:

1. **Conditional statements**: if, if-else, ifelse, switch
2. **Loops**: for, while, repeat
3. **Loop control**: break, next

## Code Examples

### Standard Example: Conditional Statements

```r
# ===== IF-ELSE STRUCTURE =====
cat("===== IF-ELSE STATEMENTS =====\n\n")

x <- 10

# Simple if
if (x > 0) {
  cat("x is positive\n")
}

# if-else
if (x > 100) {
  cat("x is large\n")
} else {
  cat("x is small\n")
}

# Nested if-else
score <- 85
if (score >= 90) {
  grade <- "A"
} else if (score >= 80) {
  grade <- "B"
} else if (score >= 70) {
  grade <- "C"
} else {
  grade <- "F"
}
cat("Score:", score, "-> Grade:", grade, "\n")

# ifelse for vectors
cat("\nifelse on vectors:\n")
numbers <- c(-5, 0, 5, 10)
result <- ifelse(numbers > 0, "positive", "non-positive")
cat("Input:", numbers, "\n")
cat("Output:", result, "\n")
```

**Output:**
```
===== IF-ELSE STATEMENTS =====

x is positive
x is small
Score: 85 -> Grade: B

ifelse on vectors:
Input: -5 0 5 10 
Output: non-positive non-positive positive positive
```

### Real-World Example: Grade Classification

```r
# Real-world: Processing student scores with control structures
scores <- c(95, 82, 67, 88, 45, 73, 91, 58, 79, 100)

cat("===== STUDENT SCORE CLASSIFICATION =====\n\n")

# Process each score using for loop
for (i in 1:length(scores)) {
  score <- scores[i]
  
  # Determine grade using if-else
  if (score >= 90) {
    grade <- "A"
    message <- "Excellent!"
  } else if (score >= 80) {
    grade <- "B"
    message <- "Good job!"
  } else if (score >= 70) {
    grade <- "C"
    message <- "Satisfactory"
  } else if (score >= 60) {
    grade <- "D"
    message <- "Needs improvement"
  } else {
    grade <- "F"
    message <- "Failing"
  }
  
  cat(sprintf("Student %d: Score = %d, Grade = %s (%s)\n", 
              i, score, grade, message))
}

# Calculate summary statistics
cat("\n===== CLASS SUMMARY =====\n\n")
passing <- sum(scores >= 60)
failing <- sum(scores < 60)
cat("Passing students:", passing, "\n")
cat("Failing students:", failing, "\n")
cat("Pass rate:", round(passing / length(scores) * 100, 1), "%\n")
```

**Output:**
```
===== STUDENT SCORE CLASSIFICATION =====

Student 1: Score = 95, Grade = A (Excellent!)
...
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use vectorized operations when possible
2. Keep if-else chains shallow
3. Use switch for multiple branches
4. Avoid growing vectors in loops

### Common Pitfalls

1. Forgetting braces around multi-statement blocks
2. Using = instead of == in conditions
3. Vector recycling in logical operations
