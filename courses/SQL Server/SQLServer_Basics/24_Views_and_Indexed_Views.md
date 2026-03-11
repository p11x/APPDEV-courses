# SQL Server Views and Indexed Views

## What are Views Revisited?

A **View** is a virtual table based on a stored query. We've covered basic views in Tutorial 15. Now let's explore more advanced features.

## Advanced View Features

### 1. Updatable Views

Simple views can be updated directly:

```sql
CREATE VIEW vw_SimpleEmployees AS
SELECT EmployeeID, Name, Department
FROM Employees
WHERE IsActive = 1;

-- Update through view
UPDATE vw_SimpleEmployees 
SET Department = 'IT' 
WHERE EmployeeID = 1;
```

### 2. Views with CHECK OPTION

Prevent updates that would hide rows from the view:

```sql
CREATE VIEW vw_ActiveEmployees AS
SELECT EmployeeID, Name, Department
FROM Employees
WHERE IsActive = 1
WITH CHECK OPTION;

-- This will fail (would be hidden from view)
UPDATE vw_ActiveEmployees SET IsActive = 0 WHERE EmployeeID = 1;
```

## What is an Indexed View?

An **Indexed View** (also called a materialized view) stores its result set physically in the database. The index is automatically maintained when underlying tables change.

## Benefits of Indexed Views

- **Performance**: Pre-computed results, no need to recalculate
- **Aggregation**: Great for frequently aggregated data
- **Complex Joins**: Materialize expensive join operations
- **Enterprise Edition**: Automatically used by optimizer in all editions

## Creating an Indexed View

```sql
-- Step 1: Create the view with SCHEMABINDING
CREATE VIEW vw_OrderSummary WITH SCHEMABINDING
AS
SELECT 
    o.OrderDate,
    COUNT_BIG(*) AS OrderCount,
    SUM(od.Quantity) AS TotalQuantity,
    SUM(od.UnitPrice * od.Quantity) AS TotalAmount
FROM dbo.Orders o
INNER JOIN dbo.OrderDetails od ON o.OrderID = od.OrderID
GROUP BY o.OrderDate;
GO

-- Step 2: Create a unique clustered index on the view
CREATE UNIQUE CLUSTERED INDEX idx_OrderSummary_Date
ON vw_OrderSummary(OrderDate);
GO

-- Step 3: Optionally add non-clustered indexes
CREATE NONCLUSTERED INDEX idx_OrderSummary_Amount
ON vw_OrderSummary(TotalAmount DESC);
```

## Indexed View Requirements

| Requirement | Description |
|-------------|-------------|
| SCHEMABINDING | Must use WITH SCHEMABINDING |
| Two-part names | Must use schema.table (dbo.TableName) |
| No OUTER JOINs | Cannot use LEFT/RIGHT/FULL JOIN |
| No subqueries | Cannot include subqueries |
| No COUNT(*) | Cannot use COUNT(*) (use COUNT_BIG) |
| No FLOAT/TEXT | Cannot use these data types |
| NOEXPAND hint | Must use WITH (NOEXPAND) in Enterprise |

## Using Indexed Views

```sql
-- Query uses the indexed view automatically (Enterprise Edition)
SELECT OrderDate, TotalAmount
FROM vw_OrderSummary
WHERE OrderDate >= '2024-01-01';

-- Other editions must use NOEXPAND hint
SELECT OrderDate, TotalAmount
FROM vw_OrderSummary WITH (NOEXPAND)
WHERE OrderDate >= '2024-01-01';
```

## Partitioned Views

Distribute data across multiple tables:

```sql
-- Create tables for different time periods
CREATE TABLE Orders_2023 (
    OrderID INT PRIMARY KEY,
    OrderDate DATE,
    Amount DECIMAL(10,2)
);

CREATE TABLE Orders_2024 (
    OrderID INT PRIMARY KEY,
    OrderDate DATE,
    Amount DECIMAL(10,2)
);

-- Create partitioned view
CREATE VIEW vw_AllOrders AS
SELECT * FROM Orders_2023
UNION ALL
SELECT * FROM Orders_2024;
GO

-- Query the view
SELECT * FROM vw_AllOrders WHERE OrderDate >= '2024-01-01';
```

## System Views

SQL Server provides built-in views for metadata:

```sql
-- View all tables
SELECT * FROM INFORMATION_SCHEMA.TABLES;

-- View all columns
SELECT * FROM INFORMATION_SCHEMA.COLUMNS;

-- View table constraints
SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS;

-- View indexes
SELECT * FROM sys.indexes;

-- View dependencies
SELECT * FROM sys.sql_dependencies;
```

## Creating Views with Metadata

```sql
-- View showing table structure
CREATE VIEW vw_TableMetadata AS
SELECT 
    TABLE_NAME AS TableName,
    COLUMN_NAME AS ColumnName,
    DATA_TYPE AS DataType,
    IS_NULLABLE AS AllowsNull,
    CHARACTER_MAXIMUM_LENGTH AS MaxLength
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'dbo';
```

## Security with Views

Grant access through views, not tables:

```sql
-- Create view with limited columns
CREATE VIEW vw_EmployeeContact AS
SELECT 
    EmployeeID,
    Name,
    Email
FROM Employees;

-- Grant only view access
GRANT SELECT ON vw_EmployeeContact TO UserName;

-- Deny table access
DENY SELECT ON Employees TO UserName;
```

## Materialized View Alternative (Table + Job)

When indexed views aren't suitable:

```sql
-- Create summary table
CREATE TABLE DailySalesSummary (
    SaleDate DATE PRIMARY KEY,
    TotalSales DECIMAL(15,2),
    TransactionCount INT
);

-- Create job to refresh (using stored procedure)
CREATE PROCEDURE sp_RefreshDailySummary
AS
BEGIN
    TRUNCATE TABLE DailySalesSummary;
    
    INSERT INTO DailySalesSummary
    SELECT 
        SaleDate,
        SUM(Amount) AS TotalSales,
        COUNT(*) AS TransactionCount
    FROM Sales
    GROUP BY SaleDate;
END;
```

## Managing Views

```sql
-- View definition
EXEC sp_helptext 'vw_ActiveEmployees';

-- View dependencies
EXEC sp_depends 'vw_ActiveEmployees';

-- Modify view
ALTER VIEW vw_ActiveEmployees AS
SELECT EmployeeID, Name, Department, Salary
FROM Employees
WHERE IsActive = 1;

-- Drop view
DROP VIEW vw_ActiveEmployees;
```

## Key Points Summary

| Feature | Use Case |
|---------|----------|
| Basic View | Simplify complex queries |
| CHECK OPTION | Prevent hidden updates |
| Indexed View | Pre-compute expensive operations |
| Partitioned View | Distribute data across tables |
| System Views | Query metadata |
| Security Views | Restrict data access |

---

*This topic should take about 5-7 minutes to explain in class.*
