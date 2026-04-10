# Aggregation and Summarization in R

## Learning Objectives

- Master aggregation functions in R
- Use apply, lapply, sapply, tapply
- Create summary statistics
- Handle grouped data

## Theoretical Background

### Aggregation Functions

Aggregation reduces data by computing summary statistics:

1. **apply()** - Apply function to margins of array/matrix
2. **lapply()** - Apply function to list/vector, return list
3. **sapply()** - Apply function, simplify to vector/matrix
4. **tapply()** - Apply function by groups
5. **aggregate()** - Grouped statistics for data frames

## Code Examples

### Standard Example: apply Functions

```r
# ===== APPLY FAMILY FUNCTIONS =====

cat("===== APPLY FUNCTIONS =====\n\n")

# Sample matrix
m <- matrix(1:9, nrow = 3)
cat("Matrix m:\n")
print(m)

# apply() - margin 1 = rows, margin 2 = columns
cat("\napply() - row sums:\n")
cat("  apply(m, 1, sum) =", apply(m, 1, sum), "\n")

cat("\napply() - column means:\n")
cat("  apply(m, 2, mean) =", apply(m, 2, mean), "\n")

# lapply() - returns list
cat("\nlapply() on list:\n")
sample_list <- list(a = 1:5, b = 10:15, c = c(1, 2, 10))
result <- lapply(sample_list, mean)
cat("  Result type:", class(result), "\n")
print(result)

# sapply() - simplifies to vector
cat("\nsapply() - simplifies to vector:\n")
result2 <- sapply(sample_list, mean)
cat("  Result type:", class(result2), "\n")
print(result2)

# tapply() - grouped statistics
cat("\ntapply() - grouped by factor:\n")
values <- c(1, 2, 3, 4, 5, 6)
groups <- factor(c("A", "A", "A", "B", "B", "B"))
cat("  tapply(values, groups, sum):", 
    tapply(values, groups, sum), "\n")

# aggregate() - for data frames
cat("\naggregate() for data frames:\n")
df <- data.frame(
  group = c("A", "A", "B", "B", "A", "B"),
  value = c(10, 20, 15, 25, 30, 35)
)
cat("  Original:\n")
print(df)
cat("\n  aggregate(value ~ group, data = df, FUN = sum):\n")
print(aggregate(value ~ group, data = df, FUN = sum))
```

**Output:**
```
===== APPLY FAMILY FUNCTIONS =====

Matrix m:
     [,1] [,2] [,3]
[1,]    1    4    7
[2,]    2    5    8
[3,]    3    6    9

apply() - row sums:
  apply(m, 1, sum) = 12 15 18
```

### Real-World Example: Sales Aggregation

```r
# Real-world: Sales data aggregation
cat("===== SALES DATA AGGREGATION =====\n\n")

sales <- data.frame(
  month = rep(c("Jan", "Feb", "Mar"), each = 4),
  product = c("A", "B", "C", "D"),
  sales = c(1000, 1500, 800, 1200,
           1100, 1600, 900, 1300,
           1200, 1700, 1000, 1400),
  region = c("North", "South", "North", "South",
             "North", "South", "North", "South",
             "North", "South", "North", "South")
)

cat("Original sales data:\n")
print(sales)

# 1. Total sales by product
cat("\n1. Sales by product:\n")
agg_product <- aggregate(sales ~ product, data = sales, FUN = sum)
print(agg_product)

# 2. Average sales by month
cat("\n2. Average sales by month:\n")
agg_month <- aggregate(sales ~ month, data = sales, FUN = mean)
print(agg_month)

# 3. Multiple aggregations
cat("\n3. Multiple statistics by product:\n")
agg_multi <- aggregate(sales ~ product, data = sales, 
                      FUN = function(x) c(sum = sum(x), 
                                          mean = mean(x), 
                                          max = max(x)))
print(agg_multi)

# 4. Two-way aggregation
cat("\n4. Sales by month and region:\n")
agg_2way <- aggregate(sales ~ month + region, data = sales, FUN = sum)
print(agg_2way)

# 5. Using by() for multiple groups
cat("\n5. Using by():\n")
result <- by(sales$sales, list(sales$month, sales$region), sum)
print(result)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Choose appropriate apply function
2. Use simplify = FALSE when needed
3. Use aggregate() for data frames
4. Name output when possible

### Common Pitfalls

1. sapply() failing on empty input
2. Not handling NA in aggregation
3. Wrong margin in apply()
