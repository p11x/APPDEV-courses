# SQL Server Advanced Indexing Strategies

## Introduction

Building on Tutorial 19, this covers advanced indexing techniques for optimal database performance.

## Columnstore Indexes

Columnstore indexes store data by columns rather than rows, ideal for analytical workloads:

### Create Columnstore Index

```sql
-- Create table
CREATE TABLE Sales (
    SaleID INT,
    SaleDate DATE,
    ProductID INT,
    CustomerID INT,
    Amount DECIMAL(10,2)
);

-- Create clustered columnstore index
CREATE CLUSTERED COLUMNSTORE INDEX CCS_Sales
ON Sales;

-- Create nonclustered columnstore index
CREATE NONCLUSTERED COLUMNSTORE INDEX NCS_Sales
ON Sales (SaleDate, ProductID, Amount);
```

### Columnstore Benefits

| Feature | Benefit |
|---------|---------|
| Columnar Storage | 10x compression |
| Batch Mode | 10x-100x faster aggregations |
| Segment Elimination | Skip irrelevant columns |

## Filtered Indexes

Create indexes on specific data subsets:

```sql
-- Index only active customers
CREATE INDEX idx_ActiveCustomers
ON Customers(CustomerID, Name)
WHERE IsActive = 1;

-- Index only high-value orders
CREATE INDEX idx_HighValueOrders
ON Orders(OrderDate, TotalAmount)
WHERE TotalAmount > 10000;
```

## Composite Index Design

### Column Order Matters

```sql
-- Index for: WHERE Department = 'IT' AND Salary > 50000
-- Best order: High selectivity first
CREATE INDEX idx_DeptSalary 
ON Employees(Department, Salary DESC);

-- Index for: WHERE Department = 'IT' ORDER BY Salary
-- Put equality column first
CREATE INDEX idx_DeptSalary2 
ON Employees(Department, Salary);
```

### Covering Index

Include all columns needed by queries:

```sql
-- Cover query completely (no table lookup)
CREATE INDEX idx_Covering
ON Orders (OrderDate, CustomerID)
INCLUDE (OrderID, TotalAmount, Status);
```

## Index Intersection

SQL Server can use multiple indexes together:

```sql
-- Query uses both indexes
SELECT * FROM Orders 
WHERE OrderDate = '2024-01-01' 
  AND CustomerID = 100;

-- SQL Server may use:
-- idx_Orders_Date (for OrderDate)
-- idx_Orders_Customer (for CustomerID)
-- Then merges results
```

## Indexing Strategies by Query Type

### Equality Queries (=)

```sql
-- Single column equality
CREATE INDEX idx_CustomerID ON Orders(CustomerID);

-- Multiple equality
CREATE INDEX idx_OrderDate_Cust
ON Orders(OrderDate, CustomerID);
```

### Range Queries (>, <, BETWEEN)

```sql
-- Range on one column
CREATE INDEX idx_OrderDate ON Orders(OrderDate);

-- Put range column last in composite
CREATE INDEX idx_DateCust
ON Orders(CustomerID, OrderDate);
```

### LIKE Queries

```sql
-- Leading wildcard (cannot use index)
-- WHERE Name LIKE '%john' - SLOW

-- Trailing wildcard (can use index)
CREATE INDEX idx_Name ON Customers(Name);
-- WHERE Name LIKE 'john%' - FAST
```

## Index Maintenance

### fragmentation Analysis

```sql
-- Check index fragmentation
SELECT 
    OBJECT_NAME(ps.object_id) AS TableName,
    i.name AS IndexName,
    ps.avg_fragmentation_in_percent,
    ps.page_count
FROM sys.dm_db_index_physical_stats(
    DB_ID(), NULL, NULL, NULL, 'DETAILED'
) ps
INNER JOIN sys.indexes i ON ps.object_id = i.object_id 
    AND ps.index_id = i.index_id
WHERE ps.avg_fragmentation_in_percent > 30;
```

### Maintenance Operations

```sql
-- Rebuild (for high fragmentation)
ALTER INDEX ALL ON Employees REBUILD;

-- Reorganize (for low-medium fragmentation)
ALTER INDEX idx_EmployeeName ON Employees REORGANIZE;

-- Rebuild specific partition
ALTER INDEX idx_Orders_Date ON Orders
REBUILD PARTITION = 1;
```

### Fill Factor

```sql
-- Create index with fill factor
CREATE INDEX idx_Orders_Date
ON Orders(OrderDate)
WITH (FILLFACTOR = 80);  -- 20% free space for updates

-- Change default for table
ALTER INDEX ALL ON Employees
REBUILD WITH (FILLFACTOR = 90);
```

## Advanced Index Types

### Unique Index

```sql
CREATE UNIQUE INDEX idx_Email 
ON Users(Email);
```

### Computed Column Index

```sql
-- Create computed column
ALTER TABLE Products
ADD DiscountPrice AS (Price * 0.9);

-- Index the computed column
CREATE INDEX idx_DiscountPrice 
ON Products(DiscountPrice);
```

### XML Index

```sql
-- Primary XML index
CREATE PRIMARY XML INDEX PXML_ProductSpecs
ON Products(Specifications);

-- Secondary XML indexes
CREATE XML INDEX SXML_ProductPath
ON Products(Specifications)
USING XML INDEX PXML_ProductSpecs FOR PATH;
```

## Index Usage Analysis

```sql
-- Find missing indexes
SELECT 
    migs.avg_total_user_cost * migs.avg_user_impact * (migs.user_seeks + migs.user_scans) AS improvement,
    'CREATE INDEX ' + midb.equality_columns + COALESCE(' INCLUDE (' + midb.inequality_columns + ')', '') AS create_index,
    migs.*
FROM sys.dm_db_missing_index_groups mig
INNER JOIN sys.dm_db_missing_index_group_stats migs ON mig.index_group_handle = migs.group_handle
CROSS APPLY sys.dm_db_missing_index_columns(mig.index_handle) midb
ORDER BY improvement DESC;
```

```sql
-- Find unused indexes
SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    ius.user_seeks,
    ius.user_scans,
    ius.user_updates
FROM sys.indexes i
LEFT JOIN sys.dm_db_index_usage_stats ius 
    ON i.object_id = ius.object_id AND i.index_id = ius.index_id
WHERE ius.user_seeks = 0 
  AND ius.user_scans = 0 
  AND i.type > 0
ORDER BY ius.user_updates DESC;
```

## Key Points Summary

| Index Type | Use Case |
|-----------|----------|
| Columnstore | Analytics, data warehousing |
| Filtered | Subset of data frequently accessed |
| Covering | All columns in query |
| Composite | Multiple WHERE conditions |
| Fill Factor | Update-heavy tables |

---

*This topic should take about 5-7 minutes to explain in class.*
