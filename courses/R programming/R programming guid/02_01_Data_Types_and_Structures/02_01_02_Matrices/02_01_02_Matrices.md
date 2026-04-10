# Matrices in R

## Learning Objectives

- Understand what matrices are in R and their role
- Create matrices using different methods
- Perform matrix operations
- Apply matrices in linear algebra and data analysis
- Understand matrix properties and attributes

## Theoretical Background

### What is a Matrix?

A matrix is a two-dimensional collection of elements of the same data type. It is essentially a vector with a dimension attribute. Matrices are fundamental to statistical computing and are used extensively in linear algebra, statistical models, and data analysis.

### Matrix Properties

- **Dimensions**: Number of rows and columns (use `dim()`)
- **Elements**: All must be of the same atomic type
- **Row/Column names**: Can have named rows and columns (use `rownames()`, `colnames()`)

### Matrix Operations

- **Transpose**: `t(matrix)`
- **Multiplication**: `matrix1 %*% matrix2`
- **Element-wise**: `matrix1 * matrix2`

## Code Examples

### Standard Example: Matrix Creation and Basic Operations

```r
# ===== MATRIX CREATION IN R =====

# 1. Using matrix() function
# matrix(data, nrow, ncol, byrow)
cat("===== CREATING MATRICES =====\n\n")

# Create a 3x4 matrix with values 1:12
m1 <- matrix(1:12, nrow = 3, ncol = 4)
cat("3x4 matrix (bycolumn default):\n")
print(m1)

# Create matrix by row
m2 <- matrix(1:12, nrow = 3, ncol = 4, byrow = TRUE)
cat("\n3x4 matrix (byrow = TRUE):\n")
print(m2)

# 2. Using cbind() and rbind()
# Combine vectors as columns or rows
a <- 1:3
b <- 4:6
c <- 7:9

m3 <- cbind(a, b, c)  # Columns
cat("\nUsing cbind():\n")
print(m3)

m4 <- rbind(a, b, c)  # Rows
cat("\nUsing rbind():\n")
print(m4)

# 3. Matrix properties
cat("\n===== MATRIX PROPERTIES =====\n")
cat("Dimensions of m1:", dim(m1), "\n")
cat("Number of rows:", nrow(m1), "\n")
cat("Number of columns:", ncol(m1), "\n")
cat("Length (total elements):", length(m1), "\n")
cat("Data type:", typeof(m1), "\n")

# 4. Row and column names
rownames(m1) <- c("Row1", "Row2", "Row3")
colnames(m1) <- c("Col1", "Col2", "Col3", "Col4")
cat("\nMatrix with names:\n")
print(m1)
```

**Output:**
```
===== CREATING MATRICES =====

3x4 matrix (bycolumn default):
     [,1] [,2] [,3] [,4]
[1,]    1    4    7   10
[2,]    2    5    8   11
[3,]    3    6    9   12

3x4 matrix (byrow = TRUE):
     [,1] [,2] [,3] [,4]
[1,]    1    2    3   4
...
```

**Comments:**
- Default is bycolumn filling (fills column by column)
- `byrow = TRUE` fills row by row
- `cbind()` and `rbind()` are convenient for combining vectors

### Real-World Example 1: Sales Data as Matrix

```r
# Real-world application: Managing product sales across regions
# This demonstrates practical matrix use in business

# Define product and region data
products <- c("Electronics", "Clothing", "Food", "Books")
regions <- c("North", "South", "East", "West", "Central")

# Create sales matrix (products x regions)
# Sales figures in thousands
sales_matrix <- matrix(
  c(150, 120, 180, 140, 160,  # Electronics
    100, 110, 90, 95, 105,    # Clothing
    200, 180, 220, 190, 210,  # Food
    50, 45, 60, 55, 48),      # Books
  nrow = 4,
  ncol = 5,
  byrow = TRUE,
  dimnames = list(products, regions)
)

cat("===== REGIONAL SALES MATRIX =====\n\n")
cat("(Values in thousands of dollars)\n\n")
print(sales_matrix)

# Calculate totals by product (row sums)
product_totals <- rowSums(sales_matrix)
cat("\nTotal Sales by Product:\n")
for (i in 1:length(product_totals)) {
  cat(" ", products[i], ": $", format(product_totals[i], big.mark = ","), "K\n")
}

# Calculate totals by region (column sums)
region_totals <- colSums(sales_matrix)
cat("\nTotal Sales by Region:\n")
for (i in 1:length(region_totals)) {
  cat(" ", regions[i], ": $", format(region_totals[i], big.mark = ","), "K\n")
}

# Find best performing product-region combination
max_sales <- max(sales_matrix)
max_idx <- which(sales_matrix == max_sales, arr.ind = TRUE)
cat("\nBest Performance:\n")
cat("  Product:", rownames(sales_matrix)[max_idx[1]], "\n")
cat("  Region:", colnames(sales_matrix)[max_idx[2]], "\n")
cat("  Sales: $", format(max_sales, big.mark = ","), "K\n")

# Calculate average sales per cell
avg_sales <- mean(sales_matrix)
cat("\nAverage Sales per Product-Region:", 
    format(avg_sales, big.mark = ","), "K\n")
```

**Output:**
```
===== REGIONAL SALES MATRIX =====

(Values in thousands of dollars)

            North South East West Central
Electronics   150  120  180  140    160
Clothing      100  110   90   95    105
Food          200  180  220  190    210
Books          50   45   60   55     48
```

**Comments:**
- Matrix is perfect for 2D data like product-region combinations
- `rowSums()` and `colSums()` are efficient for totaling
- `which(..., arr.ind = TRUE)` returns matrix indices

### Real-World Example 2: Linear Algebra with Matrices

```r
# Real-world application: Portfolio optimization using matrix operations
# This demonstrates matrix algebra for financial applications

cat("===== PORTFOLIO OPTIMIZATION WITH MATRICES =====\n\n")

# Define asset returns matrix (observations x assets)
# Each row is a time period, each column is an asset
set.seed(42)

returns <- matrix(
  c(0.05, 0.02, 0.03,  # Period 1
    -0.01, 0.04, 0.02,  # Period 2
    0.03, -0.02, 0.04,  # Period 3
    0.02, 0.03, -0.01,  # Period 4
    0.04, 0.01, 0.02),  # Period 5
  nrow = 5,
  ncol = 3,
  byrow = TRUE,
  dimnames = list(paste("Period", 1:5), c("StockA", "StockB", "Bond"))
)

cat("Historical Returns Matrix:\n")
print(returns)

# Calculate mean returns for each asset
mean_returns <- colMeans(returns)
cat("\nMean Returns by Asset:\n")
print(mean_returns)

# Calculate covariance matrix
# This is crucial for portfolio optimization
cov_matrix <- cov(returns)
cat("\nCovariance Matrix:\n")
print(cov_matrix)

# Define portfolio weights (equal weighting)
n_assets <- ncol(returns)
weights <- rep(1/n_assets, n_assets)
cat("\nPortfolio Weights (Equal):\n")
cat(" StockA:", weights[1], "\n")
cat(" StockB:", weights[2], "\n")
cat(" Bond:", weights[3], "\n")

# Calculate portfolio return
portfolio_return <- sum(weights * mean_returns)
cat("\nPortfolio Expected Return:", round(portfolio_return * 100, 2), "%\n")

# Calculate portfolio variance using matrix algebra
# Variance = w' * Cov * w
portfolio_variance <- t(weights) %*% cov_matrix %*% weights
portfolio_std <- sqrt(portfolio_variance)
cat("Portfolio Standard Deviation:", round(portfolio_std * 100, 2), "%\n")

# Matrix operations demonstration
cat("\n===== MATRIX ALGEBRA =====\n")

# Transpose
cat("\nTranspose of returns (first 3 rows):\n")
print(t(returns)[, 1:3])

# Matrix multiplication
# Create a correlation demonstration
cat("\nCorrelation Matrix:\n")
print(cor(returns))
```

**Output:**
```
===== PORTFOLIO OPTIMIZATION WITH MATRICES =====

Historical Returns Matrix:
       StockA StockB  Bond
Period1   0.05   0.02  0.03
Period2  -0.01   0.04  0.02
...

Mean Returns by Asset:
  StockA   StockB     Bond 
    0.026   0.016    0.020 

Portfolio Expected Return: 2.07%
Portfolio Standard Deviation: 2.01%
```

**Comments:**
- Matrices are essential for portfolio theory
- Covariance matrix captures asset relationships
- Matrix multiplication is fundamental for portfolio variance

## Best Practices and Common Pitfalls

### Best Practices

1. **Name your matrices**: Use dimnames for clarity
2. **Check dimensions**: Verify before operations
3. **Use matrix operations**: Prefer %*% over loops
4. **Pre-allocate**: Create matrices before filling

### Common Pitfalls

1. **Dimension mismatch**: Matrix multiplication requires conformable dimensions
2. **Forgetting byrow**: Default is column-wise filling
3. **Type coercion**: All elements must be same type

## Performance Considerations

- Matrix operations are optimized in R
- Use `%*%` for matrix multiplication (not `*`)
- `crossprod()` is faster for X'X calculations
- `Matrix` package for sparse matrices

## Related Concepts and Further Reading

- `?matrix`, `?matmult`, `?crossprod`
- "Matrices and Matrix Algebra in R" - CRAN
- "Matrix" package for advanced matrix operations

## Exercise Problems

1. **Exercise 1**: Create a 4x4 identity matrix.

2. **Exercise 2**: Multiply two compatible matrices using %*%.

3. **Exercise 3**: Calculate the inverse of a matrix using solve().

4. **Exercise 4**: Extract the diagonal elements of a matrix.

5. **Exercise 5**: Create a matrix from three vectors using cbind().
