# Data Transformation in R

## Learning Objectives

- Perform data transformation operations in base R
- Use apply family functions
- Transform columns and rows
- Handle missing values during transformation

## Theoretical Background

### Data Transformation Operations

Data transformation involves modifying, adding, removing, or reordering data. In base R, this includes:

1. **Column transformation**: modify existing columns
2. **Column creation**: add new computed columns
3. **Row operations**: sort, remove duplicates
4. **Type conversion**: change data types

## Code Examples

### Standard Example: Basic Transformations

```r
# ===== DATA TRANSFORMATION IN R =====

cat("===== COLUMN TRANSFORMATION =====\n\n")

# Sample data frame
df <- data.frame(
  id = 1:5,
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  salary = c(50000, 60000, 55000, 70000, 45000),
  stringsAsFactors = FALSE
)

print(df)

# 1. Add new column
df$bonus <- df$salary * 0.1
cat("\nAfter adding bonus column:\n")
print(df)

# 2. Transform existing column
df$salary_with_bonus <- df$salary + df$bonus
cat("\nAfter adding salary_with_bonus:\n")
print(df)

# 3. Using transform()
cat("\nUsing transform():\n")
df <- transform(df, 
                tax = salary_with_bonus * 0.2,
                take_home = salary_with_bonus - tax)
print(df)

# ===== TYPE CONVERSION =====
cat("\n\n===== TYPE CONVERSION =====\n\n")

# Numeric to character
nums <- 1:5
chars <- as.character(nums)
cat("Numeric:", nums, "\n")
cat("As character:", chars, "\n")

# Character to numeric
chars <- c("1", "2", "3")
nums2 <- as.numeric(chars)
cat("Character to numeric:", nums2, "\n")

# Using type.convert()
char_df <- data.frame(a = c("1", "2", "3"), b = c("4", "5", "6"))
cat("\nBefore type.convert():\n")
print(sapply(char_df, class))

char_df <- type.convert(char_df)
cat("\nAfter type.convert():\n")
print(sapply(char_df, class))
```

**Output:**
```
===== COLUMN TRANSFORMATION =====

  id     name salary
1  1    Alice  50000
```

### Real-World Example: Employee Data Processing

```r
# Real-world: Employee data transformation
cat("===== EMPLOYEE DATA TRANSFORMATION =====\n\n")

employees <- data.frame(
  employee_id = 1001:1010,
  name = c("John", "Mary", "David", "Susan", "Robert",
           "Lisa", "Michael", "Jennifer", "William", "Sarah"),
  hire_date = as.Date(c("2020-01-15", "2019-03-22", "2021-06-10",
                        "2018-11-05", "2022-02-28", "2020-07-14",
                        "2019-09-30", "2021-01-20", "2017-05-12",
                        "2022-04-05")),
  base_salary = c(55000, 62000, 58000, 75000, 52000,
                 65000, 70000, 60000, 80000, 56000),
  department = c("Sales", "Engineering", "Sales", "Engineering", "HR",
                 "Engineering", "Engineering", "Sales", "Finance", "HR"),
  stringsAsFactors = FALSE
)

# 1. Calculate years of service
employees$years_service <- as.numeric(
  difftime(Sys.Date(), employees$hire_date, units = "weeks")
) / 52.25

cat("Years of service:\n")
print(employees[, c("name", "hire_date", "years_service")])

# 2. Create salary tier based on experience
employees$salary_tier <- cut(
  employees$years_service,
  breaks = c(0, 1, 3, 5, Inf),
  labels = c("Entry", "Mid", "Senior", "Principal")
)

cat("\nWith salary tier:\n")
print(employees[, c("name", "years_service", "salary_tier")])

# 3. Department average salary
dept_avg <- tapply(employees$base_salary, employees$department, mean)
cat("\nAverage salary by department:\n")
print(dept_avg)

# 4. Add department comparison
employees$vs_dept_avg <- sapply(1:nrow(employees), function(i) {
  dept <- employees$department[i]
  ifelse(employees$base_salary[i] > dept_avg[dept], "Above", "Below")
})

cat("\nWith department comparison:\n")
print(employees[, c("name", "department", "base_salary", "vs_dept_avg")])
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use transform() for chained operations
2. Use within() for complex transformations
3. Handle NA values explicitly
4. Create new columns rather than overwriting

### Common Pitfalls

1. Forgetting type conversions
2. Not handling NA in calculations
3. Modifying original data unintentionally
