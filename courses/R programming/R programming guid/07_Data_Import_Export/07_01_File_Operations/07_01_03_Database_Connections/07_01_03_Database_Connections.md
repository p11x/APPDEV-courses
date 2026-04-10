# Database Connections in R

## Learning Objectives

- Connect to databases using DBI package
- Use RMySQL for MySQL/MariaDB connections
- Use RPostgreSQL for PostgreSQL connections
- Execute queries and retrieve results
- Manage connections and transactions

## Theoretical Background

### Database Interface (DBI)

The DBI package provides a unified interface for database operations:

- dbConnect(): Establish connection
- dbSendQuery(): Execute query
- dbFetch(): Retrieve results
- dbDisconnect(): Close connection
- dbReadTable(): Read entire table
- dbWriteTable(): Write data frame

### Supported Database Packages

| Database | Package | Notes |
|----------|---------|-------|
| MySQL | RMySQL | Most common |
| PostgreSQL | RPostgreSQL | Full support |
| SQLite | RSQLite | File-based |
| SQL Server | odbc | Via ODBC |
| Oracle | ROracle | JDBC-based |

### Connection Patterns

```r
# Basic workflow
con <- dbConnect(driver, host, dbname, user, password)
result <- dbSendQuery(con, "SELECT * FROM table")
data <- dbFetch(result)
dbClearResult(result)
dbDisconnect(con)
```

## Code Examples

### Basic Example: DBI with SQLite

```r
# ===== DBI WITH SQLITE =====

cat("===== DBI + SQLite =====\n\n")

# SQLite is file-based, no server needed
library(DBI)
library(RSQLite)

# Create in-memory database
con <- dbConnect(SQLite(), ":memory:")

# 1. Create table
cat("1. Create table:\n")
dbExecute(con, "
  CREATE TABLE employees (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    department TEXT,
    salary REAL
  )
")
cat("Table created successfully\n\n")

# 2. Insert data
cat("2. Insert data:\n")
dbExecute(con, "
  INSERT INTO employees (name, department, salary)
  VALUES ('Alice', 'Sales', 55000),
         ('Bob', 'Engineering', 72000),
         ('Charlie', 'HR', 85000),
         ('Diana', 'Marketing', 61000)
")
cat("Rows inserted:", dbGetRowsAffected(con), "\n\n")

# 3. Query data
cat("3. Simple query:\n")
result <- dbSendQuery(con, "SELECT * FROM employees")
employees <- dbFetch(result)
dbClearResult(result)
print(employees)
cat("\n")

# 4. Parameterized query
cat("4. Parameterized query:\n")
result <- dbSendQuery(con, "SELECT * FROM employees 
                     WHERE salary > ?")
dbBind(result, list(60000))
high_earners <- dbFetch(result)
dbClearResult(result)
print(high_earners)
cat("\n")

# 5. Read table directly
cat("5. Read entire table:\n")
employees_direct <- dbReadTable(con, "employees")
print(employees_direct)

# Disconnect
dbDisconnect(con)
```

**Output:**
```
===== DBI + SQLite =====

1. Create table:
Table created successfully

3. Simple query:
  id     name  department salary
1  1    Alice       Sales  55000
2  2      Bob Engineering  72000
3  3  Charlie          HR  85000
4  4   Diana   Marketing  61000
```

### Standard Example: MySQL with RMySQL

```r
# ===== RMYSQL CONNECTIONS =====

cat("===== RMySQL PACKAGE =====\n\n")

library(DBI)
library(RMySQL)

# Typical MySQL connection
# con <- dbConnect(MySQL(),
#   host = "localhost",
#   dbname = "company",
#   user = "root",
#   password = "password")

# For demonstration, use mock connection
# In production, replace with actual credentials
cat("Note: MySQL requires running database server\n")
cat("Example connection structure:\n\n")

# Connection example (commented out for safety)
example_con <- "
# Real connection:
con <- dbConnect(MySQL(),
  host = 'localhost',
  port = 3306,
  dbname = 'company_db',
  user = 'your_username',
  password = 'your_password'
)

# Query example
result <- dbSendQuery(con, 'SELECT * FROM employees 
                      WHERE department = ?')
dbBind(result, list('Sales'))
df <- dbFetch(result)

# Always disconnect
dbDisconnect(con)
"

cat(example_con)

# SQLite simulation for complete example
library(DBI)
library(RSQLite)

con <- dbConnect(SQLite(), ":memory:")
dbExecute(con, "CREATE TABLE sales (id INT, product TEXT, 
           qty INT, revenue REAL)")
dbExecute(con, "INSERT INTO sales VALUES 
           (1, 'Widget', 100, 5000),
           (2, 'Gadget', 50, 7500),
           (3, 'Gizmo', 75, 3750),
           (4, 'Widget', 200, 10000)")

# Advanced queries with aggregation
cat("\n===== AGGREGATION QUERIES =====\n\n")

# 1. GROUP BY
cat("1. GROUP BY product:\n")
result <- dbSendQuery(con, "
  SELECT product, SUM(qty) as total_qty, 
         SUM(revenue) as total_revenue
  FROM sales
  GROUP BY product
")
print(dbFetch(result))
dbClearResult(result)
cat("\n")

# 2. HAVING
cat("2. HAVING total_revenue > 5000:\n")
result <- dbSendQuery(con, "
  SELECT product, SUM(revenue) as total_revenue
  FROM sales
  GROUP BY product
  HAVING SUM(revenue) > 5000
")
print(dbFetch(result))
dbClearResult(result)
cat("\n")

# 3. JOIN
cat("3. Create and join tables:\n")
dbExecute(con, "CREATE TABLE products (id INT, category TEXT)")
dbExecute(con, "INSERT INTO products VALUES (1, 'Electronics'), 
           (2, 'Electronics'), (3, 'Accessories')")

result <- dbSendQuery(con, "
  SELECT s.product, s.revenue, p.category
  FROM sales s
  JOIN products p ON s.product_id = p.id
")
# Note: Simplified for demo
dbClearResult(result)

# 4. Subquery
cat("4. Subquery:\n")
result <- dbSendQuery(con, "
  SELECT product, revenue
  FROM sales
  WHERE revenue > (SELECT AVG(revenue) FROM sales)
")
above_avg <- dbFetch(result)
print(above_avg)
dbClearResult(result)

dbDisconnect(con)
```

### Real-World Example: PostgreSQL

```r
# ===== RPOSTGRESQL CONNECTIONS =====

cat("===== RPostgreSQL PACKAGE =====\n\n")

library(DBI)
library(RPostgreSQL)

# Example PostgreSQL connection
cat("PostgreSQL connection structure:\n\n")
example_pg <- "
# Load driver
drv <- dbDriver('PostgreSQL')

# Create connection
con <- dbConnect(drv,
  host = 'localhost',
  port = 5432,
  dbname = 'analytics',
  user = 'postgres',
  password = 'password'
)

# List tables
dbListTables(con)

# Read table
data <- dbReadTable(con, 'transactions')

# Execute raw SQL
result <- dbSendQuery(con, '
  SELECT date_trunc(\\'month\', transaction_date) as month,
         SUM(amount) as total
  FROM transactions
  WHERE status = \'completed\'
  GROUP BY date_trunc(\'month\', transaction_date)
  ORDER BY month
')

# Disconnect properly
dbDisconnect(con)
dbUnloadDriver(drv)
"

cat(example_pg)

# Complete workflow demonstration with SQLite
library(DBI)
library(RSQLite)

con <- dbConnect(SQLite(), ":memory:")
dbExecute(con, "CREATE TABLE orders (
  order_id INT, customer_id INT, 
  order_date TEXT, amount REAL)")
dbExecute(con, "INSERT INTO orders VALUES 
           (1, 101, '2024-01-15', 250.00),
           (2, 102, '2024-01-16', 175.50),
           (3, 101, '2024-01-20', 320.00),
           (4, 103, '2024-01-21', 90.00)")

# Transaction management
cat("\n===== TRANSACTION MANAGEMENT =====\n\n")

# Begin transaction
dbExecute(con, "BEGIN TRANSACTION")

# Insert with error handling
cat("1. Insert new order:\n")
dbExecute(con, "INSERT INTO orders VALUES (5, 104, '2024-01-22', 450.00)")
cat("  Order inserted\n")

# Commit or rollback
dbExecute(con, "COMMIT")

# Verify
result <- dbSendQuery(con, "SELECT * FROM orders WHERE order_id = 5")
print(dbFetch(result))
dbClearResult(result)

# Demonstrate rollback
cat("\n2. Demonstrate rollback:\n")
dbExecute(con, "BEGIN TRANSACTION")
dbExecute(con, "DELETE FROM orders WHERE order_id = 5")
cat("  Deleted order 5\n")

#Rollback
dbExecute(con, "ROLLBACK")
cat("  Rolled back\n")

# Verify still exists
result <- dbSendQuery(con, "SELECT * FROM orders WHERE order_id = 5")
print(dbFetch(result))
dbClearResult(result)

dbDisconnect(con)
```

### Advanced Example: Data Operations

```r
# ===== DATA OPERATIONS =====

cat("===== DATABASE DATA OPERATIONS =====\n\n")

library(DBI)
library(RSQLite)

con <- dbConnect(SQLite(), ":memory:")
dbExecute(con, "CREATE TABLE sales (
  id INT PRIMARY KEY, 
  region TEXT,
  product TEXT,
  quantity INT,
  revenue REAL
)")
dbExecute(con, "INSERT INTO sales VALUES 
           (1, 'North', 'Widget', 100, 5000),
           (2, 'South', 'Widget', 50, 2500),
           (3, 'North', 'Gadget', 30, 4500),
           (4, 'South', 'Gadget', 40, 6000),
           (5, 'East', 'Gizmo', 75, 3750)")

# 1. Write data frame to database
cat("1. dbWriteTable:\n")
df_new <- data.frame(
  id = 6:8,
  region = c("West", "North", "South"),
  product = c("Widget", "Gizmo", "Gadget"),
  quantity = c(80, 60, 25),
  revenue = c(4000, 3000, 3750)
)
dbWriteTable(con, "sales_new", df_new, overwrite = TRUE)
cat("  Written", nrow(df_new), "rows to sales_new\n\n")

# 2. Read table from database
cat("2. dbReadTable:\n")
full_data <- dbReadTable(con, "sales")
print(full_data)
cat("\n")

# 3. Append to existing table
cat("3. Append to sales:\n")
dbWriteTable(con, "sales_new", df_new, append = TRUE)
result <- dbSendQuery(con, "SELECT * FROM sales_new")
print(dbFetch(result))
dbClearResult(result)
cat("\n")

# 4. Table manipulation
cat("4. dbRemoveTable:\n")
dbRemoveTable(con, "sales_new")
cat("  Table removed\n\n")

# 5. Check table exists
cat("5. Table existence:\n")
cat("  sales exists:", dbExistsTable(con, "sales"), "\n")
cat("  sales_new exists:", dbExistsTable(con, "sales_new"), "\n\n")

# 6. List tables
cat("6. List tables:\n")
print(dbListTables(con))

dbDisconnect(con)
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always disconnect**: Close connections in finally block
2. **Use parameterized queries**: Prevent SQL injection
3. **Limit results**: Use LIMIT clause
4. **Transaction management**: Use BEGIN/COMMIT for multiple operations
5. **Connection pooling**: For high-volume applications

### Common Pitfalls

1. **Connection leaks**: Forgetting to disconnect
2. **SQL injection**: Using paste() for queries
3. **Large result sets**: Fetching all at once
4. **Encoding issues**: Specify encoding for non-ASCII
5. **Firewall**: Database server access blocked

### Performance Tips

| Method | Speed Improvement |
|--------|-------------------|
| dbGetQuery() | Single line queries |
| dbBind() | Reuse query plan |
| LIMIT clause | Reduce data transfer |
| Index columns | Faster WHERE |