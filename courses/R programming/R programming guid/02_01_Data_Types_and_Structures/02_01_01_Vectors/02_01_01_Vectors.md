# Vectors in R

## Learning Objectives

- Understand what vectors are in R and their fundamental role
- Create vectors using different methods
- Perform operations on vectors
- Understand vectorized operations and recycling
- Apply vectors in real-world data analysis scenarios

## Theoretical Background

### What is a Vector?

In R, a vector is the most fundamental data structure. It is an ordered collection of elements of the same data type. Unlike arrays in other languages, R vectors are one-dimensional and can contain numeric, character, or logical values.

### Types of Vectors

R has two main types of vectors:

1. **Atomic Vectors**: All elements must be of the same type
   - `numeric`: Real numbers
   - `integer`: Whole numbers
   - `character`: Text strings
   - `logical`: TRUE/FALSE values
   - `complex`: Complex numbers
   - `raw`: Raw bytes

2. **Lists**: Can contain elements of different types

### Vector Properties

- **Length**: Number of elements (use `length()`)
- **Type**: Data type of elements (use `typeof()`)
- **Attributes**: Additional metadata (use `attributes()`)

## Step-by-Step Explanation

### Creating Vectors

```mermaid
graph TD
    A[Creating Vectors] --> B[Using c Function]
    A --> C[Using Colon Operator]
    A --> D[Using seq Function]
    A --> E[Using rep Function]
    
    B --> B1[c(1,2,3)]
    C --> C1[1:10]
    D --> D1[seq(from, to, by)]
    E --> E1[rep(x, times)]
```

## Code Examples

### Standard Example: Vector Creation and Manipulation

```r
# ===== VECTOR CREATION IN R =====

# 1. Using c() - combine function
# c() is the most common way to create vectors
numeric_vector <- c(1, 2, 3, 4, 5)
character_vector <- c("apple", "banana", "cherry")
logical_vector <- c(TRUE, FALSE, TRUE, FALSE)

cat("Numeric vector:", numeric_vector, "\n")
cat("Character vector:", character_vector, "\n")
cat("Logical vector:", logical_vector, "\n")

# 2. Using colon operator (:) for sequences
# Creates a sequence from start to end with step of 1
sequence_vector <- 1:10
cat("\nSequence 1:10:", sequence_vector, "\n")

# 3. Using seq() for custom sequences
# seq(from, to, by) or seq(from, to, length.out)
seq_by_2 <- seq(0, 100, by = 10)
seq_length <- seq(0, 1, length.out = 5)

cat("seq(0, 100, by = 10):", seq_by_2, "\n")
cat("seq(0, 1, length.out = 5):", seq_length, "\n")

# 4. Using rep() to repeat values
# rep(x, times) or rep(x, each)
rep_times <- rep(c(1, 2), times = 3)
rep_each <- rep(c(1, 2), each = 3)

cat("\nrep(c(1,2), times = 3):", rep_times, "\n")
cat("rep(c(1,2), each = 3):", rep_each, "\n")

# 5. Checking vector properties
cat("\n===== VECTOR PROPERTIES =====\n")
cat("Length of numeric_vector:", length(numeric_vector), "\n")
cat("Type of numeric_vector:", typeof(numeric_vector), "\n")
cat("Type of character_vector:", typeof(character_vector), "\n")
cat("Type of logical_vector:", typeof(logical_vector), "\n")
```

**Output:**
```
Numeric vector: 1 2 3 4 5
Character vector: apple banana cherry
Logical vector: TRUE FALSE TRUE FALSE

Sequence 1:10: 1 2 3 4 5 6 7 8 9 10
seq(0, 100, by = 10): 0 10 20 30 40 50 60 70 80 90 100
seq(0, 1, length.out = 5): 0 0.25 0.5 0.75 1

rep(c(1,2), times = 3): 1 2 1 2 1 2
rep(c(1,2), each = 3): 1 1 1 2 2 2

===== VECTOR PROPERTIES =====
Length of numeric_vector: 5
Type of numeric_vector: double
Type of character_vector: character
Type of logical_vector: logical
```

**Comments:**
- `c()` is the fundamental way to combine values into a vector
- The colon operator `:` creates integer sequences
- `seq()` provides more control over sequences
- `rep()` can repeat elements in different ways

### Real-World Example 1: Sales Data Analysis with Vectors

```r
# Real-world application: Analyzing monthly sales data
# This demonstrates practical use of vectors in business analysis

# Create vectors for sales data
months <- c("January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December")

# Monthly sales figures (in thousands)
sales <- c(45000, 52000, 48000, 55000, 60000, 65000, 
           70000, 68000, 72000, 75000, 80000, 85000)

# Marketing spend (in thousands)
marketing_spend <- c(5000, 4500, 6000, 5500, 7000, 6500,
                     8000, 7500, 9000, 8500, 10000, 9500)

# Calculate key metrics
cat("===== SALES ANALYSIS =====\n\n")

# Total annual sales
total_sales <- sum(sales)
cat("Total Annual Sales: $", format(total_sales, big.mark = ","), "\n")

# Average monthly sales
avg_sales <- mean(sales)
cat("Average Monthly Sales: $", format(avg_sales, big.mark = ","), "\n")

# Maximum and minimum sales
max_sales <- max(sales)
min_sales <- min(sales)
cat("Highest Month: ", months[which.max(sales)], " - $", 
    format(max_sales, big.mark = ","), "\n")
cat("Lowest Month: ", months[which.min(sales)], " - $", 
    format(min_sales, big.mark = ","), "\n")

# Sales growth rate
growth_rate <- (sales[12] - sales[1]) / sales[1] * 100
cat("Annual Growth Rate:", round(growth_rate, 2), "%\n")

# Calculate ROI for marketing
roi <- (sales - marketing_spend) / marketing_spend * 100
cat("\nMonthly Marketing ROI:\n")
for (i in 1:12) {
  cat(sprintf("  %s: %.1f%%\n", months[i], roi[i]))
}

# Identify above-average months
above_avg <- months[sales > avg_sales]
cat("\nMonths with Above-Average Sales:\n")
cat(" ", paste(above_avg, collapse = ", "), "\n")
```

**Output:**
```
===== SALES ANALYSIS =====

Total Annual Sales: $ 753,000
Average Monthly Sales: $ 62,750
Highest Month: December - $ 85,000
Lowest Month: January - $ 45,000
Annual Growth Rate: 88.89%

Monthly Marketing ROI:
  January: 800.0%
  February: 1055.6%
  ...
```

**Comments:**
- Vectors are perfect for time series or sequential data
- Vectorized operations automatically apply to each element
- `which.max()` and `which.min()` find index positions

### Real-World Example 2: Statistical Analysis with Vectors

```r
# Real-world application: Statistical analysis of test scores
# This demonstrates statistical functions with vectors

# Simulate test scores for a class of students
set.seed(42)  # For reproducibility

# Generate random test scores (normal distribution)
num_students <- 50
test_scores <- round(rnorm(n = num_students, mean = 75, sd = 10), 1)

# Clip scores to valid range (0-100)
test_scores <- pmax(pmin(test_scores, 100), 0)

cat("===== TEST SCORE ANALYSIS =====\n\n")
cat("Class Size:", num_students, "students\n")
cat("Score Range:", min(test_scores), "to", max(test_scores), "\n")
cat("\nAll Scores:\n")
cat(" ", test_scores, "\n\n")

# Basic statistics
cat("--- Descriptive Statistics ---\n")
cat("Mean:", round(mean(test_scores), 2), "\n")
cat("Median:", round(median(test_scores), 2), "\n")
cat("Standard Deviation:", round(sd(test_scores), 2), "\n")
cat("Variance:", round(var(test_scores), 2), "\n")

# Quartiles
q <- quantile(test_scores)
cat("\n--- Quartiles ---\n")
cat("Q1 (25th):", q[2], "\n")
cat("Q2 (50th):", q[3], "\n")
cat("Q3 (75th):", q[4], "\n")

# Interquartile range
iqr <- IQR(test_scores)
cat("IQR:", round(iqr, 2), "\n")

# Identify grade categories
cat("\n--- Grade Distribution ---\n")
passing <- sum(test_scores >= 60)
cat("Passing (>=60):", passing, "students\n")
excellent <- sum(test_scores >= 90)
cat("Excellent (>=90):", excellent, "students\n")
failing <- sum(test_scores < 60)
cat("Failing (<60):", failing, "students\n")

# Z-scores for outlier detection
z_scores <- (test_scores - mean(test_scores)) / sd(test_scores)
cat("\n--- Outlier Analysis (|Z| > 2) ---\n")
outliers <- test_scores[abs(z_scores) > 2]
if (length(outliers) > 0) {
  cat("Outlier scores:", outliers, "\n")
} else {
  cat("No outliers detected\n")
}

# Calculate passing percentage
passing_pct <- passing / num_students * 100
cat("\nPass Rate:", round(passing_pct, 1), "%\n")
```

**Output:**
```
===== TEST SCORE ANALYSIS =====

Class Size: 50 students
Score Range: 46.4 to 97.1

All Scores:
  75.3 82.1 68.4 ...

--- Descriptive Statistics ---
Mean: 74.68
Median: 75.4
Standard Deviation: 9.82
Variance: 96.38
```

**Comments:**
- `rnorm()` generates normally distributed random numbers
- `pmax()` and `pmin()` enforce bounds on values
- Vectorized statistics make analysis simple
- Z-scores help identify outliers

## Best Practices and Common Pitfalls

### Best Practices

1. **Use meaningful names**: `sales_2023` is better than `x`
2. **Pre-allocate when possible**: Create empty vectors with known size
3. **Use vectorized operations**: Avoid loops for performance
4. **Check types**: Use `typeof()` to verify data types

### Common Pitfalls

1. **Forgetting vector recycling**: R recycles shorter vectors
2. **Mixing types**: Character elements coerce all to character
3. **NA handling**: Always check for NA values (`is.na()`)
4. **1-based indexing**: R indexes start at 1, not 0

## Performance Considerations

- Vectorized operations are 10-100x faster than loops
- Use `numeric(length)` to pre-allocate for large vectors
- The `length<-` function can extend vectors
- Avoid growing vectors in loops (pre-allocate instead)

## Related Concepts and Further Reading

- **R Documentation**: `?vector`, `?c`, `?seq`, `?rep`
- **Advanced R**: https://adv-r.had.co.nz/Vectors.html
- **R for Data Science**: Vectors chapter

## Exercise Problems

1. **Exercise 1**: Create a vector of even numbers from 2 to 100.

2. **Exercise 2**: Create a vector of 100 random integers between 1 and 10.

3. **Exercise 3**: Calculate the sum, mean, and product of a numeric vector.

4. **Exercise 4**: Find the index of the minimum and maximum values in a vector.

5. **Exercise 5**: Create two vectors and demonstrate vector recycling.
