# RPostgreSQL: PostgreSQL Database Connections

## Learning Objectives

- Connect to PostgreSQL databases from R
- Execute SQL queries using RPostgreSQL
- Work with PostgreSQL-specific features
- Handle PostgreSQL data types
- Use PostgreSQL for data analysis

## Theory

RPostgreSQL provides an interface to PostgreSQL databases using DBI. PostgreSQL is an advanced open-source relational database with features like JSON support, arrays, and window functions.

Connection parameters include host, port (default 5432), database name, user, and password. PostgreSQL supports both password and certificate-based authentication.

## Step-by-Step

1. Install and load RPostgreSQL package
2. Establish connection with dbConnect()
3. Use dbSendQuery() or dbGetQuery()
4. Fetch results with dbFetch()
5. Write data with dbWriteTable()
6. Close connection properly

## Code Examples

### Basic Connection

```r
cat("===== POSTGRESQL CONNECTION =====\n\n")

library(RPostgreSQL)
library(DBI)

# Load PostgreSQL driver
drv <- dbDriver("PostgreSQL")

# Connection attempt
con <- tryCatch({
  dbConnect(
    drv,
    host = "localhost",
    port = 5432,
    dbname = "postgres",
    user = "postgres",
    password = ""
  )
}, error = function(e) {
  cat("Note: PostgreSQL connection not available\n")
  cat("Error:", conditionMessage(e), "\n")
  NULL
})

if (!is.null(con)) {
  cat("Connected to PostgreSQL\n")
  
  # List tables
  tables <- dbListTables(con)
  cat("Tables:", paste(tables, collapse = ", "), "\n")
  
  # List fields in a table
  # fields <- dbListFields(con, "my_table")
  
  dbDisconnect(con)
  cat("Disconnected\n")
}
dbUnloadDriver(drv)
```

### PostgreSQL Query Features

```r
cat("\n===== POSTGRESQL QUERIES =====\n\n")

cat("# Query Examples for PostgreSQL:\n\n")

cat("1. Using LIMIT and OFFSET:\n")
cat("SELECT * FROM users LIMIT 100 OFFSET 50\n\n")

cat("2. Using DISTINCT ON:\n")
cat("SELECT DISTINCT ON (email) * FROM logins\n\n")

cat("3. Using array column:\n")
cat("SELECT id, tags[1] FROM products WHERE 'electronics' = ANY(tags)\n\n")

cat("4. Using JSON column:\n")
cat("SELECT data->>'name' FROM orders WHERE data->>'status' = 'pending'\n\n")

cat("5. Using window functions:\n")
cat("SELECT name, salary, \n")
cat("       AVG(salary) OVER (PARTITION BY department) as dept_avg\n")
cat("FROM employees\n")
```

### Reading and Writing Data

```r
cat("\n===== DATA OPERATIONS =====\n\n")

cat("# Read table into R:\n")
cat("# df <- dbReadTable(con, 'my_table')\n\n")

cat("# Read with SQL:\n")
cat("# df <- dbGetQuery(con, 'SELECT * FROM table WHERE col > 100')\n\n")

cat("# Write data frame:\n")
cat("# dbWriteTable(con, 'new_table', df, overwrite = TRUE)\n\n")

cat("# Append to existing table:\n")
cat("# dbWriteTable(con, 'existing', new_rows, append = TRUE)\n\n")

cat("# Execute non-SELECT query:\n")
cat("# dbExecute(con, 'UPDATE table SET col = value WHERE id = 1')\n")
```

### PostgreSQL-Specific Operations

```r
cat("\n===== POSTGRESQL SPECIFIC =====\n\n")

cat("PostgreSQL-specific functions:\n\n")

cat("1. Copy from data frame:\n")
cat("# dbWriteTable(con, 'temp', df)\n")
cat("# dbExecute(con, \"COPY temp TO STDOUT WITH CSV HEADER\")\n\n")

cat("2. Handle arrays:\n")
cat("# df <- data.frame(id = 1:2, tags = I(list(c('a','b'), c('c'))))\n")
cat("# dbWriteTable(con, 'with_arrays', df)\n\n")

cat("3. Handle JSON:\n")
cat("# df <- data.frame(id = 1, data = I(list(list(name='test'))))\n\n")

cat("4. Temporary tables:\n")
cat("# dbExecute(con, 'CREATE TEMP TABLE temp_data AS SELECT * FROM large_table')\n")
```

## Real-World Example: Analytics Pipeline

```r
# Real-world: PostgreSQL for analytics
cat("===== ANALYTICS PIPELINE =====\n\n")

cat("# Analytics Pipeline with PostgreSQL:\n\n")

cat("# 1. Create summary table:\n")
cat("CREATE TABLE IF NOT EXISTS daily_summary AS\n")
cat("SELECT \n")
cat("  DATE(created_at) as date,\n")
cat("  COUNT(*) as total_events,\n")
cat("  COUNT(DISTINCT user_id) as unique_users,\n")
cat("  SUM(revenue) as total_revenue\n")
cat("FROM events\n")
cat("GROUP BY DATE(created_at)\n\n")

cat("# 2. Query for reporting:\n")
cat("SELECT \n")
cat("  date,\n")
cat("  total_revenue / NULLIF(unique_users, 0) as arpu\n")
cat("FROM daily_summary\n")
cat("WHERE date >= CURRENT_DATE - INTERVAL '30 days'\n")
cat("ORDER BY date DESC\n\n")

cat("# 3. Use R to fetch and visualize:\n")
cat("# library(ggplot2)\n")
cat("# df <- dbGetQuery(con, 'SELECT * FROM daily_summary')\n")
cat("# ggplot(df, aes(date, total_revenue)) + geom_line()\n")
```

## Best Practices

1. Use parameterized queries for security
2. Create indexes for frequently queried columns
3. Use COPY for bulk data operations
4. Leverage PostgreSQL's analytical features (window functions)
5. Close connections and unload driver
6. Use tryCatch for connection error handling

## Exercises

1. Create a connection function with connection pooling
2. Implement a function to query data with pagination
3. Write data to PostgreSQL using COPY command
4. Handle PostgreSQL arrays in R
5. Create a query builder function for common patterns