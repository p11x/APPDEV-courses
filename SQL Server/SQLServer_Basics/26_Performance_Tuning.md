# SQL Server Performance Tuning

## Introduction

Performance tuning involves optimizing SQL Server to achieve maximum speed and efficiency. This covers techniques to improve query performance.

## Understanding Execution Plans

### Viewing Execution Plans

```sql
-- Enable actual execution plan (Ctrl+M in SSMS)
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

-- Run query and view:
-- - Estimated vs Actual plan
-- - Cost percentages
-- - Index usage

SELECT * FROM Orders WHERE OrderDate >= '2024-01-01';
```

### Common Operators in Plans

| Operator | Meaning |
|----------|---------|
| Table Scan | Reading all rows (slow for large tables) |
| Index Scan | Reading all index entries |
| Index Seek | Using index to find specific rows |
| Nested Loops | Row-by-row processing |
| Hash Match | Hash-based joining |
| Merge Join | Sorted join |

## Query Optimization Tips

### 1. Use EXISTS Instead of IN

```sql
-- Slower
SELECT * FROM Customers 
WHERE CustomerID IN (SELECT CustomerID FROM Orders);

-- Faster
SELECT * FROM Customers c
WHERE EXISTS (SELECT 1 FROM Orders o WHERE o.CustomerID = c.CustomerID);
```

### 2. Use Proper JOIN Order

```sql
-- Put small tables on left for INNER JOIN
SELECT * 
FROM SmallTable s
INNER JOIN LargeTable l ON s.ID = l.ID;

-- For LEFT JOIN, put large table on left
SELECT *
FROM LargeTable l
LEFT JOIN SmallTable s ON l.ID = s.ID;
```

### 3. Avoid SELECT *

```sql
-- Slower: Retrieves all columns
SELECT * FROM Orders;

-- Faster: Retrieves only needed columns
SELECT OrderID, OrderDate, TotalAmount FROM Orders;
```

### 4. Use WHERE Clauses Early

```sql
-- Filter early in the query
SELECT p.Name, o.TotalAmount
FROM Products p
INNER JOIN Orders o ON p.ProductID = o.ProductID
WHERE o.OrderDate >= '2024-01-01';  -- Filter first!
```

### 5. Avoid Functions on Columns

```sql
-- Slower: Function prevents index use
SELECT * FROM Orders 
WHERE YEAR(OrderDate) = 2024;

-- Faster: Use range query
SELECT * FROM Orders 
WHERE OrderDate >= '2024-01-01' 
  AND OrderDate < '2025-01-01';
```

## Index Optimization

### Clustered vs Non-Clustered

| Feature | Clustered | Non-Clustered |
|---------|-----------|---------------|
| One per table | Yes | Yes (249+) |
| Sorts data | Yes | No |
| Stores data | Yes | Pointer only |

### Create Optimal Indexes

```sql
-- Covering index (includes all columns needed)
CREATE INDEX idx_Orders_Covering
ON Orders (OrderDate, CustomerID)
INCLUDE (TotalAmount, Status);

-- Composite index (multi-column)
CREATE INDEX idx_Employee_DeptSal
ON Employees (Department, Salary DESC);

-- Filtered index
CREATE INDEX idx_ActiveOrders
ON Orders(OrderDate)
WHERE Status = 'Active';
```

### Index Maintenance

```sql
-- Rebuild index (defragments)
ALTER INDEX ALL ON Employees REBUILD;

-- Reorganize index (lighter defrag)
ALTER INDEX ALL ON Employees REORGANIZE;

-- Check index usage
SELECT 
    OBJECT_NAME(s.object_id) AS TableName,
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON s.object_id = i.object_id
WHERE OBJECTPROPERTY(s.object_id, 'IsUserTable') = 1;
```

## Statistics

Statistics help the optimizer make good decisions:

```sql
-- Update statistics
UPDATE STATISTICS Orders;

-- Or for all tables
EXEC sp_updatestats;

-- View statistics
DBCC SHOW_STATISTICS ('Orders', 'idx_Orders_Date');
```

## Query Performance Monitoring

### Dynamic Management Views

```sql
-- Find expensive queries
SELECT TOP 10 
    qs.execution_count,
    qs.total_elapsed_time / 1000 AS total_ms,
    qs.total_logical_reads,
    SUBSTRING(qt.text, qs.statement_start_offset/2, 
        (CASE WHEN qs.statement_end_offset = -1 
            THEN LEN(qt.text) * 2 
            ELSE qs.statement_end_offset END - qs.statement_start_offset)/2) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
ORDER BY qs.total_elapsed_time DESC;
```

```sql
-- Find missing index suggestions
SELECT 
    migs.avg_total_user_cost * (migs.avg_user_impact / 100.0) * (migs.user_seeks + migs.user_scans) AS improvement_measure,
    'CREATE INDEX ' + MIDB.equality_columns + COALESCE(' INCLUDE (' + MIDB.inequality_columns + ')', '') AS create_index_sql,
    migs.*
FROM sys.dm_db_missing_index_groups mig
INNER JOIN sys.dm_db_missing_index_group_stats migs ON mig.index_group_handle = migs.group_handle
CROSS APPLY sys.dm_db_missing_index_columns(mig.index_handle) MIDB
ORDER BY migs.avg_total_user_cost * migs.avg_user_impact / 100.0 * (migs.user_seeks + migs.user_scans) DESC;
```

## Parameter Sniffing

### Problem

```sql
-- Procedure with parameter (first call caches plan)
CREATE PROCEDURE sp_GetOrders
    @Status VARCHAR(20) = NULL
AS
BEGIN
    SELECT * FROM Orders
    WHERE (@Status IS NULL OR Status = @Status);
END;

-- First call with NULL (returns many rows)
EXEC sp_GetOrders NULL;  -- Creates plan for large result

-- Second call with specific value (uses same slow plan)
EXEC sp_GetOrders 'Pending';  -- Still slow!
```

### Solutions

```sql
-- Solution 1: Use OPTION (RECOMPILE)
ALTER PROCEDURE sp_GetOrders
    @Status VARCHAR(20) = NULL
AS
BEGIN
    SELECT * FROM Orders
    WHERE (@Status IS NULL OR Status = @Status)
    OPTION (RECOMPILE);  -- New plan each time
END;

-- Solution 2: Use local variables
ALTER PROCEDURE sp_GetOrders
    @Status VARCHAR(20) = NULL
AS
BEGIN
    DECLARE @LocalStatus VARCHAR(20) = @Status;
    
    SELECT * FROM Orders
    WHERE (@LocalStatus IS NULL OR Status = @LocalStatus);
END;
```

## Performance Tuning Checklist

| Area | Action |
|------|--------|
| Indexes | Create covering indexes |
| Queries | Avoid SELECT *, use WHERE early |
| Joins | Use proper join order |
| Functions | Avoid on indexed columns |
| Statistics | Keep updated |
| Monitoring | Check DMVs regularly |

## Key Points Summary

- **Execution Plans** show how SQL Server processes queries
- **Indexes** dramatically improve read performance
- **Statistics** help optimizer make smart decisions
- **Query Patterns** matter - avoid functions on columns
- **Monitoring** is essential for finding bottlenecks

---

*This topic should take about 5-7 minutes to explain in class.*
