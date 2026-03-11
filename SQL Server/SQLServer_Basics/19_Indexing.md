# SQL Server Indexing

## What is an Index?

An **Index** is a data structure that improves the speed of data retrieval operations on a database table. Think of it like an index in a book - it helps you find information quickly without reading every page.

## Why Use Indexes?

- **Faster Queries**: Quickly locate specific rows
- **Reduced I/O**: Less data needs to be read
- **Improved Performance**: Especially for large tables

## Types of Indexes

### 1. Clustered Index
- Sorts physical data in the table
- Only ONE per table
- Usually created on primary key

### 2. Non-Clustered Index
- Separate from data storage
- Can have MULTIPLE per table
- Contains pointers to actual data

## Creating a Clustered Index

```sql
-- Primary key creates clustered index automatically
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,  -- Clustered index
    Name VARCHAR(50),
    Age INT
);

-- Or create clustered index explicitly
CREATE CLUSTERED INDEX idx_StudentID
ON Students(StudentID);
```

## Creating a Non-Clustered Index

```sql
-- Create non-clustered index on Name
CREATE NONCLUSTERED INDEX idx_StudentName
ON Students(Name);

-- Create index on multiple columns
CREATE NONCLUSTERED INDEX idx_DeptAge
ON Students(Department, Age);
```

## When to Create Indexes

### Good Scenarios:
- Columns used in WHERE clauses
- Columns used in JOIN conditions
- Columns used in ORDER BY
- Columns with high selectivity (many unique values)

### Avoid Indexing:
- Small tables
- Columns frequently updated
- Columns with few unique values (like Gender)

## Example: Performance Comparison

```sql
-- Create sample table without index
CREATE TABLE Products (
    ProductID INT,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Price DECIMAL(10, 2)
);

-- Insert sample data (1000 rows)
INSERT INTO Products 
SELECT 
    number AS ProductID,
    'Product ' + CAST(number AS VARCHAR(10)),
    CASE number % 5 
        WHEN 0 THEN 'Electronics'
        WHEN 1 THEN 'Clothing'
        WHEN 2 THEN 'Food'
        WHEN 3 THEN 'Books'
        ELSE 'Other'
    END,
    CAST(RAND(CHECKSUM(NEWID())) * 100 AS DECIMAL(10,2)
FROM master..spt_values 
WHERE type = 'P' AND number BETWEEN 1 AND 1000;

-- Create index
CREATE NONCLUSTERED INDEX idx_ProductName
ON Products(ProductName);

-- Query using index
SELECT * FROM Products WHERE ProductName = 'Product 500';

-- Query without index (full table scan)
SELECT * FROM Products WHERE Category = 'Electronics';
```

## Viewing Indexes

```sql
-- View indexes on a table
EXEC sp_helpindex 'Students';

-- Or use sys.indexes
SELECT 
    name AS IndexName,
    type_desc AS IndexType,
    is_primary_key AS IsPrimaryKey
FROM sys.indexes
WHERE object_id = OBJECT_ID('Students');
```

## Index with WHERE Clause

```sql
CREATE INDEX idx_PriceRange
ON Products(Price)
WHERE Price > 100;  -- Filtered index
```

## Composite Index

```sql
-- Index on multiple columns
CREATE INDEX idx_DeptSalary
ON Employees(Department, Salary DESC);

-- Best for queries that use both columns
SELECT * FROM Employees 
WHERE Department = 'IT' AND Salary > 50000;

-- But not efficient for
SELECT * FROM Employees WHERE Salary > 50000;
```

## Index Maintenance

### Rebuild Index
```sql
ALTER INDEX idx_StudentName ON Students REBUILD;
```

### Reorganize Index
```sql
ALTER INDEX idx_StudentName ON Students REORGANIZE;
```

### Disable Index
```sql
ALTER INDEX idx_StudentName ON Students DISABLE;
```

### Drop Index
```sql
DROP INDEX idx_StudentName ON Students;
```

## Index Best Practices

| Do | Don't |
|----|-------|
| Index frequently queried columns | Index every column |
| Use for large tables | Index small tables |
| Consider column selectivity | Index columns with few values |
| Use composite for multi-column queries | Create too many indexes |
| Monitor query performance | Over-index (slows INSERT/UPDATE) |

## Understanding Execution Plans

```sql
-- Enable actual execution plan in SSMS
SET STATISTICS IO ON;

-- Run query and check:
-- - Table Scan (bad for large tables)
-- - Index Seek (good)
-- - Index Scan (acceptable)

SELECT * FROM Students WHERE Name = 'John';
```

## Key Points Summary

| Index Type | Description | Per Table |
|-----------|-------------|-----------|
| Clustered | Sorts data physically | One only |
| Non-Clustered | Separate structure | Multiple |
| Unique | No duplicate values | Multiple |
| Composite | Multiple columns | Multiple |

---

*This topic should take about 5-7 minutes to explain in class.*
