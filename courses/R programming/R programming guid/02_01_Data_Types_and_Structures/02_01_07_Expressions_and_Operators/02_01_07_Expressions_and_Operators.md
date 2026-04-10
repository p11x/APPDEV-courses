# Expressions and Operators in R

## Learning Objectives

- Understand R expressions and how they are evaluated
- Master all operator types in R
- Apply operators for data manipulation
- Understand operator precedence

## Theoretical Background

### What are Expressions and Operators?

An expression is a combination of values, variables, operators, and functions that R evaluates to produce a result. Operators are symbols that tell R to perform specific operations.

### Types of Operators

1. **Arithmetic**: +, -, *, /, %%, %/%, ^, sqrt()
2. **Relational**: ==, !=, <, >, <=, >=
3. **Logical**: !, &, |, &&, ||
4. **Assignment**: <-, =, <<-, ->
5. **Special**: $, [, [[, :, %in%

## Code Examples

### Standard Example: Using All Operators

```r
# ===== ARITHMETIC OPERATORS =====
cat("===== ARITHMETIC OPERATORS =====\n")
cat("10 + 5 =", 10 + 5, "\n")
cat("10 - 5 =", 10 - 5, "\n")
cat("10 * 5 =", 10 * 5, "\n")
cat("10 / 5 =", 10 / 5, "\n")
cat("10 %% 3 (modulus) =", 10 %% 3, "\n")
cat("10 %/% 3 (integer division) =", 10 %/% 3, "\n")
cat("2^3 (power) =", 2^3, "\n")

# ===== RELATIONAL OPERATORS =====
cat("\n===== RELATIONAL OPERATORS =====\n")
cat("5 == 5:", 5 == 5, "\n")
cat("5 != 3:", 5 != 3, "\n")
cat("5 < 3:", 5 < 3, "\n")
cat("5 > 3:", 5 > 3, "\n")

# ===== LOGICAL OPERATORS =====
cat("\n===== LOGICAL OPERATORS =====\n")
cat("!TRUE:", !TRUE, "\n")
cat("TRUE & FALSE:", TRUE & FALSE, "\n")
cat("TRUE | FALSE:", TRUE | FALSE, "\n")

# ===== ASSIGNMENT =====
cat("\n===== ASSIGNMENT OPERATORS =====\n")
x <- 10          # Preferred
y = 20           # Works
z <<- 30         # Global assignment
cat("x =", x, ", y =", y, ", z =", z, "\n")

# ===== SPECIAL OPERATORS =====
cat("\n===== SPECIAL OPERATORS =====\n")
vec <- 1:10
cat("1:5 creates:", 1:5, "\n")
cat("3 %in% vec:", 3 %in% vec, "\n")
cat("15 %in% vec:", 15 %in% vec, "\n")
```

**Output:**
```
===== ARITHMETIC OPERATORS =====
10 + 5 = 15
10 - 5 = 5
10 * 5 = 50
10 / 5 = 2
10 %% 3 = 1
10 %/% 3 = 3
2^3 = 8

===== RELATIONAL OPERATORS =====
5 == 5: TRUE
```

### Real-World Example: Data Filtering with Operators

```r
# Real-world: Using operators for data filtering
df <- data.frame(
  name = c("Alice", "Bob", "Charlie", "Diana", "Eve"),
  age = c(25, 35, 28, 42, 22),
  salary = c(50000, 75000, 60000, 85000, 45000),
  department = c("Sales", "Engineering", "Sales", "Engineering", "HR")
)

cat("===== DATA FILTERING WITH OPERATORS =====\n\n")
print(df)

# Age > 30 AND salary > 60000
cat("\nAge > 30 AND salary > 60000:\n")
filtered <- df[df$age > 30 & df$salary > 60000, ]
print(filtered)

# Salary in range
cat("\nSalary between 50000 and 70000:\n")
filtered2 <- df[df$salary >= 50000 & df$salary <= 70000, ]
print(filtered2)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use `<-` for assignment (not `=`)
2. Use parentheses for clarity
3. Use `all.equal()` for floating-point comparison
4. Vectorized operators are faster

### Common Pitfalls

1. Using `=` instead of `<-` in some contexts
2. Floating-point comparison issues
3. Operator precedence confusion
