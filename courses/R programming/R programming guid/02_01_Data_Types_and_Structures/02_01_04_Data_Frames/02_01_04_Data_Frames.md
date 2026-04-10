# Data Frames in R

## Learning Objectives

- Understand what data frames are and their importance in R
- Create and manipulate data frames
- Access and modify data frame elements
- Apply data frames in real-world data analysis
- Understand data frame properties and structure

## Theoretical Background

### What is a Data Frame?

A data frame is the most important data structure in R for data analysis. It is a list of vectors of the same length, where each vector represents a column. Think of it as a spreadsheet or SQL table - rows represent observations and columns represent variables.

### Data Frame Properties

- **Rectangular**: Same number of elements in each column
- **Heterogeneous columns**: Each column can have different types
- **Row names**: Optional row identifiers
- **Column names**: Required for identification
- **List-based**: Internally a list of equal-length vectors

### Data Frame vs Matrix

| Feature | Matrix | Data Frame |
|---------|--------|------------|
| Column types | All same | Can differ |
| Column access | By index/name | By index/name |
| Row access | By index | By index |
| Manipulation | Less flexible | Very flexible |

## Code Examples

### Standard Example: Data Frame Creation and Access

```r
# ===== DATA FRAME CREATION IN R =====

cat("===== CREATING DATA FRAMES =====\n\n")

# 1. Basic data frame creation using data.frame()
employees <- data.frame(
  id = c(101, 102, 103, 104, 105),
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  department = c("Engineering", "Marketing", "Sales", "Engineering", "HR"),
  salary = c(75000, 65000, 70000, 80000, 55000),
  years_exp = c(5, 3, 7, 8, 2),
  stringsAsFactors = FALSE  # Keep strings as character, not factors
)

cat("Employees Data Frame:\n")
print(employees)

# 2. Data frame structure
cat("\n===== DATA FRAME STRUCTURE =====\n\n")
cat("Dimensions:", dim(employees), "\n")
cat("Rows:", nrow(employees), "\n")
cat("Columns:", ncol(employees), "\n")
cat("Column names:", names(employees), "\n")
cat("Column types:\n")
str(employees)

# 3. Accessing columns
cat("\n===== ACCESSING DATA FRAME ELEMENTS =====\n\n")

# Access column by name (returns vector)
cat("Names column:", employees$name, "\n")
cat("Using employees[['name']]:", employees[["name"]], "\n")

# Access column by index
cat("Using employees[[3]]:", employees[[3]], "\n")

# Access using [,] (returns data frame)
cat("\nUsing [,] accessor:\n")
cat("First column:", employees[, 1], "\n")
cat("First row:\n")
print(employees[1, ])

# 4. Accessing specific cells
cat("\nSpecific cell access:\n")
cat("Employee 1 name:", employees[1, "name"], "\n")
cat("Employee 3 salary:", employees$salary[3], "\n")
```

**Output:**
```
===== CREATING DATA FRAMES =====

Employees Data Frame:
  id     name department salary years_exp
1 101    Alice Engineering   75000         5
2 102      Bob  Marketing   65000         3
3 103 Charlie      Sales   70000         7
4 104    Diana Engineering   80000         8
5 105   Edward         HR   55000         2
```

**Comments:**
- `stringsAsFactors = FALSE` prevents automatic factor conversion
- `str()` is excellent for inspecting data frame structure
- `$` operator is the most common way to access columns

### Real-World Example 1: Customer Sales Analysis

```r
# Real-world application: Analyzing customer transaction data
# This demonstrates practical data frame operations

# Create sample sales data
sales_data <- data.frame(
  transaction_id = c("TXN001", "TXN002", "TXN003", "TXN004", "TXN005",
                    "TXN006", "TXN007", "TXN008", "TXN009", "TXN010"),
  customer_id = c("CUST001", "CUST002", "CUST001", "CUST003", "CUST002",
                 "CUST004", "CUST001", "CUST003", "CUST005", "CUST004"),
  product = c("Laptop", "Mouse", "Keyboard", "Monitor", "Laptop",
             "Webcam", "Keyboard", "Mouse", "Laptop", "Monitor"),
  quantity = c(1, 2, 1, 1, 2, 1, 3, 4, 1, 2),
  unit_price = c(999, 29, 79, 349, 999, 89, 79, 29, 999, 349),
  date = as.Date(c("2024-01-15", "2024-01-16", "2024-01-17", "2024-01-18",
                  "2024-01-19", "2024-01-20", "2024-01-21", "2024-01-22",
                  "2024-01-23", "2024-01-24")),
  stringsAsFactors = FALSE
)

# Calculate total amount
sales_data$total <- sales_data$quantity * sales_data$unit_price

cat("===== SALES DATA ANALYSIS =====\n\n")
print(sales_data)

# Summary statistics by product
cat("\n===== SALES BY PRODUCT =====\n\n")
library(dplyr)

sales_by_product <- sales_data %>%
  group_by(product) %>%
  summarise(
    total_quantity = sum(quantity),
    total_revenue = sum(total),
    avg_price = mean(unit_price),
    transactions = n()
  ) %>%
  arrange(desc(total_revenue))

print(sales_by_product)

# Top customers
cat("\n===== TOP CUSTOMERS =====\n\n")
top_customers <- sales_data %>%
  group_by(customer_id) %>%
  summarise(
    total_spent = sum(total),
    num_transactions = n()
  ) %>%
  arrange(desc(total_spent))

print(top_customers)

# Date analysis
cat("\n===== DAILY SALES =====\n\n")
daily_sales <- sales_data %>%
  group_by(date) %>%
  summarise(
    daily_revenue = sum(total),
    num_transactions = n()
  )

print(daily_sales)
```

**Output:**
```
===== SALES DATA ANALYSIS =====

   transaction_id customer_id  product quantity unit_price ...
```

**Comments:**
- Data frames work seamlessly with dplyr
- Adding calculated columns is straightforward
- Grouping and summarization are powerful

### Real-World Example 2: Employee Performance Tracking

```r
# Real-world application: HR performance management system
# This demonstrates data frame manipulation for HR

# Create employee performance data
performance <- data.frame(
  employee_id = 1001:1010,
  name = c("John Smith", "Mary Johnson", "Robert Brown", "Patricia Davis",
           "Michael Wilson", "Linda Martinez", "David Anderson", "Susan Taylor",
           "James Thomas", "Barbara Garcia"),
  department = c("Sales", "Sales", "Marketing", "Marketing", "Engineering",
                "Engineering", "HR", "HR", "Finance", "Finance"),
  sales_q1 = c(150000, 180000, NA, NA, NA, NA, NA, NA, NA, NA),
  sales_q2 = c(165000, 195000, NA, NA, NA, NA, NA, NA, NA, NA),
  performance_score = c(4.2, 4.5, 3.8, 4.1, 4.3, 4.0, 3.9, 4.2, 4.1, 3.7),
  years_at_company = c(5, 7, 3, 4, 8, 6, 4, 5, 2, 3),
  stringsAsFactors = FALSE
)

# Add department head flag
performance$is_manager <- c(TRUE, FALSE, TRUE, FALSE, TRUE, FALSE, 
                           FALSE, TRUE, FALSE, TRUE)

cat("===== EMPLOYEE PERFORMANCE DATA =====\n\n")
print(performance)

# Filter high performers
cat("\n===== HIGH PERFORMERS (score >= 4.0) =====\n\n")
high_performers <- subset(performance, performance_score >= 4.0)
print(high_performers[, c("name", "department", "performance_score")])

# Calculate average performance by department
cat("\n===== PERFORMANCE BY DEPARTMENT =====\n\n")
dept_avg <- aggregate(performance_score ~ department, 
                     data = performance, 
                     FUN = mean)
dept_avg <- dept_avg[order(dept_avg$performance_score, decreasing = TRUE), ]
print(dept_avg)

# Create summary data frame
cat("\n===== DEPARTMENT SUMMARY =====\n\n")
dept_summary <- data.frame(
  department = unique(performance$department),
  num_employees = sapply(unique(performance$department), 
                        function(d) sum(performance$department == d)),
  avg_performance = aggregate(performance_score ~ department, 
                              data = performance, FUN = mean)$performance_score,
  avg_tenure = aggregate(years_at_company ~ department, 
                        data = performance, FUN = mean)$years_at_company
)
print(dept_summary)

# Add ranking column
performance$performance_rank <- rank(-performance$performance_score, 
                                     ties.method = "first")

cat("\n===== EMPLOYEES BY RANK =====\n\n")
print(performance[order(performance$performance_rank), 
                 c("name", "department", "performance_score", "performance_rank")])
```

**Output:**
```
===== EMPLOYEE PERFORMANCE DATA =====

   employee_id         name department ...
```

**Comments:**
- Data frames handle missing values (NA) gracefully
- `subset()` is useful for filtering rows
- `aggregate()` provides grouped summaries

## Best Practices and Common Pitfalls

### Best Practices

1. **Use stringsAsFactors = FALSE**: Modern R recommendation
2. **Check structure with str()**: Always inspect new data
3. **Use head() and tail()**: Preview data without printing all
4. **Use tibbles**: Modern data frame from tidyverse

### Common Pitfalls

1. **Factor confusion**: Can cause unexpected behavior
2. **NA handling**: Always check for missing values
3. **Column name typos**: Use autocompletion

## Performance Considerations

- `data.table` package for large datasets
- Tibbles have slight overhead but better behavior
- Use `colClasses` when reading files for speed

## Related Concepts and Further Reading

- `?data.frame`, `?tibble`, `?data.table`
- R for Data Science - Data Transformation
- data.table package documentation

## Exercise Problems

1. **Exercise 1**: Create a data frame with 5 rows and 3 columns.

2. **Exercise 2**: Add a new column to an existing data frame.

3. **Exercise 3**: Filter rows where a numeric column exceeds a threshold.

4. **Exercise 4**: Use aggregate() to group and calculate means.

5. **Exercise 5**: Convert a matrix to a data frame.
