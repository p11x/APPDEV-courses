# SQL Integration with DBI

## Learning Objectives

- Connect R to databases using DBI
- Use dbConnect for database connections
- Execute SQL queries from R
- Read and write data to databases
- Handle different database backends

## Theoretical Background

### DBI Package

DBI (Database Interface) provides a consistent interface for R to communicate with databases:

1. **dbConnect()** - Create database connection
2. **dbSendQuery()** - Execute SQL statement
3. **dbFetch()** - Retrieve results
4. **dbReadTable()** - Read entire table
5. **dbWriteTable()** - Write table to database
6. **dbDisconnect()** - Close connection

### Database Backends

DBI works with multiple database backends:
- **RMySQL** - MySQL
- **RSQLite** - SQLite
- **RPostgreSQL** - PostgreSQL
- **RJDBC** - JDBC connections
- **odbc** - ODBC connections

### SQL in R Workflow

1. Connect to database
2. Send SQL query
3. Fetch results
4. Disconnect

## Code Examples

### Standard Example: SQLite Connection

```r
# ===== SQLITE BASICS =====

# Install required packages
# install.packages(c("DBI", "RSQLite"))

cat("===== SETUP =====\n")

# Load required packages
library(DBI)
library(RSQLite)

# Create an in-memory SQLite database
db <- dbConnect(SQLite(), ":memory:")

cat("SQLite database created\n")
cat("Database class:", class(db), "\n")

# Create a sample table
cat("\n===== CREATE TABLE =====\n")

# Create sample data
sample_data <- data.frame(
  id = 1:5,
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  age = c(25, 30, 35, 28, 32),
  salary = c(50000, 60000, 75000, 55000, 70000),
  department = c("Sales", "Engineering", "Engineering", "HR", "Sales"),
  stringsAsFactors = FALSE
)

# Write to database
dbWriteTable(db, "employees", sample_data)
cat("Table 'employees' created with", nrow(sample_data), "rows\n")

# List tables
cat("\nTables in database:\n")
print(dbListTables(db))
```

### Standard Example: SQL Queries

```r
# ===== SQL QUERIES =====
cat("\n===== EXECUTE QUERIES =====\n")

# Simple SELECT
result <- dbSendQuery(db, "SELECT * FROM employees")
data <- dbFetch(result)
cat("SELECT * FROM employees:\n")
print(data)

# Filter with WHERE
cat("\n===== WHERE CLAUSE =====\n")
result <- dbSendQuery(db, "SELECT * FROM employees WHERE age > 30")
data <- dbFetch(result)
print(data)

# Multiple conditions
cat("\nMultiple conditions:\n")
query <- "SELECT name, salary FROM employees WHERE department = 'Sales' AND salary > 50000"
result <- dbSendQuery(db, query)
data <- dbFetch(result)
print(data)

# Using LIMIT
cat("\nLIMIT:\n")
result <- dbSendQuery(db, "SELECT * FROM employees LIMIT 3")
data <- dbFetch(result)
print(data)

# Clear result
dbClearResult(result)
```

### Standard Example: Aggregations

```r
# ===== AGGREGATIONS =====
cat("\n===== AGGREGATE FUNCTIONS =====\n")

# COUNT
cat("COUNT(*):\n")
result <- dbSendQuery(db, "SELECT COUNT(*) as total FROM employees")
print(dbFetch(result))

# SUM
cat("\nSUM(salary):\n")
result <- dbSendQuery(db, "SELECT SUM(salary) as total_salary FROM employees")
print(dbFetch(result))

# AVG
cat("\nAVG(age):\n")
result <- dbSendQuery(db, "SELECT AVG(age) as avg_age FROM employees")
print(dbFetch(result))

# GROUP BY
cat("\nGROUP BY department:\n")
query <- "SELECT department, COUNT(*) as count, AVG(salary) as avg_salary 
         FROM employees GROUP BY department"
result <- dbSendQuery(db, query)
print(dbFetch(result))

# HAVING
cat("\nHAVING clause:\n")
query <- "SELECT department, COUNT(*) as count 
         FROM employees GROUP BY department 
         HAVING COUNT(*) > 1"
result <- dbSendQuery(db, query)
print(dbFetch(result))
```

### Standard Example: INSERT, UPDATE, DELETE

```r
# ===== DATA MANIPULATION =====
cat("\n===== INSERT =====\n")

# INSERT single row
dbSendQuery(db, "INSERT INTO employees (id, name, age, salary, department) 
               VALUES (6, 'Frank', 28, 45000, 'Marketing')")
cat("Added new employee\n")

# Verify
result <- dbSendQuery(db, "SELECT * FROM employees WHERE id = 6")
print(dbFetch(result))

cat("\n===== UPDATE =====\n")

# UPDATE data
dbSendQuery(db, "UPDATE employees SET salary = 80000 WHERE name = 'Charlie'")
cat("Updated Charlie's salary\n")

# Verify
result <- dbSendQuery(db, "SELECT name, salary FROM employees WHERE name = 'Charlie'")
print(dbFetch(result))

cat("\n===== DELETE =====\n")

# DELETE data
dbSendQuery(db, "DELETE FROM employees WHERE id = 6")
cat("Deleted employee with id = 6\n")

# Verify
cat("Total employees after delete:\n")
result <- dbSendQuery(db, "SELECT COUNT(*) as total FROM employees")
print(dbFetch(result))
```

### Standard Example: Other Databases

```r
# ===== OTHER DATABASES =====
cat("\n===== MYSQL CONNECTION =====\n")

mysql_example <- "
# MySQL connection example
library(RMySQL)

# Connect to MySQL database
db_mysql <- dbConnect(MySQL(),
  host = \"localhost\",
  user = \"username\",
  password = \"password\",
  dbname = \"databasename\"
)

# Query data
result <- dbSendQuery(db_mysql, \"SELECT * FROM mytable\")
data <- dbFetch(result)

# Disconnect
dbDisconnect(db_mysql)
"
cat(mysql_example)

cat("\n===== POSTGRESQL =====\n")
postgres_example <- "
# PostgreSQL connection example
library(RPostgreSQL)

# Connect to PostgreSQL
db_postgres <- dbConnect(PostgreSQL(),
  host = \"localhost\",
  user = \"username\",
  password = \"password\",
  dbname = \"databasename\",
  port = 5432
)

# Query data
result <- dbSendQuery(db_postgres, \"SELECT * FROM mytable\")
data <- dbFetch(result)

# Disconnect
dbDisconnect(db_postgres)
"
cat(postgres_example)

cat("\n===== ODBC =====\n")
odbc_example <- "
# ODBC connection example
library(odbc)

# Connect via ODBC DSN
db_odbc <- dbConnect(odbc::odbc(),
  dsn = \"MyDataSource\",
  uid = \"username\",
  pwd = \"password\"
)

# List tables
tables <- dbListTables(db_odbc)
print(tables)

# Query with parameterization
result <- dbSendQuery(db_odbc, \"SELECT * FROM sales WHERE date > ?\")
param(result, Sys.Date() - 30)
data <- dbFetch(result)

# Disconnect
dbDisconnect(db_odbc)
"
cat(odbc_example)
```

### Standard Example: Using dbApply and Streaming

```r
# ===== ADVANCED OPERATIONS =====
cat("\n===== STREAMING RESULTS =====\n")

# Use dbApply for streaming results
cat("Create large table for demonstration:\n")
dbSendQuery(db, "CREATE TABLE large_table AS 
               SELECT * FROM employees, employees, employees")

result <- dbSendQuery(db, "SELECT * FROM large_table")
cat(nrow(dbFetch(result)), "rows\n", sep = "")

# Clear result
dbClearResult(result)

cat("\n===== TRANSACTIONS =====\n")
transaction_example <- "
# Using transactions
dbBegin(db)

tryCatch({
  dbSendQuery(db, \"INSERT INTO employees VALUES (7, 'George', 40, 90000, 'IT')\")
  dbSendQuery(db, \"UPDATE employees SET salary = salary * 1.1 WHERE department = 'IT'\")
  dbCommit(db)
  cat(\"Transaction committed\")
}, error = function(e) {
  dbRollback(db)
  cat(\"Transaction rolled back:\", conditionMessage(e), \"\n\")
})
"
cat(transaction_example)

# Disconnect
cat("\n===== DISCONNECT =====\n")
cat("Disconnecting from database\n")
dbDisconnect(db)

cat("\n===== BEST PRACTICES =====\n")
best_practices <- "
# DBI best practices:

# 1. Always disconnect when done
on.exit(dbDisconnect(db))

# 2. Use parameterized queries to prevent SQL injection
# Use placeholders instead of paste()

# 3. Clear results after fetching
dbClearResult(result)

# 4. Use transactions for multiple operations

# 5. Close connections explicitly
dbDisconnect(conn)

# 6. Handle errors with tryCatch

# 7. Close connection in finally block
"
cat(best_practices)
```