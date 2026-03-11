# SQL Server Table Partitioning

## What is Table Partitioning?

**Table Partitioning** splits large tables into smaller, more manageable pieces called partitions. Each partition can be stored on different filegroups while appearing as a single table.

## Why Use Partitioning?

- **Performance**: Query only relevant partitions
- **Manageability**: Archive or delete old data easily
- **Availability**: Backup/restore individual partitions
- **Maintenance**: Rebuild indexes on specific partitions

## Partitioning Concepts

| Concept | Description |
|---------|-------------|
| Partition Function | Defines how data is divided |
| Partition Scheme | Maps partitions to filegroups |
| Partition Column | Column used for partitioning |

## Creating a Partitioned Table

### Step 1: Create Partition Function

```sql
-- Create partition function by range
CREATE PARTITION FUNCTION OrderDateRange (DATE)
AS RANGE LEFT FOR VALUES (
    '2023-01-01',
    '2024-01-01',
    '2025-01-01'
);

-- Creates 4 partitions:
-- Partition 1: <= 2023-01-01
-- Partition 2: > 2023-01-01 AND <= 2024-01-01
-- Partition 3: > 2024-01-01 AND <= 2025-01-01
-- Partition 4: > 2025-01-01
```

### Step 2: Create Partition Scheme

```sql
-- Create partition scheme
CREATE PARTITION SCHEME OrderDateScheme
AS PARTITION OrderDateRange
TO (FG_2023, FG_2024, FG_2025, FG_Archive);

-- Or use PRIMARY for all
CREATE PARTITION SCHEME OrderDateSchemeSimple
AS PARTITION OrderDateRange
ALL TO (PRIMARY);
```

### Step 3: Create Partitioned Table

```sql
-- Create table on partition scheme
CREATE TABLE Orders (
    OrderID INT NOT NULL,
    OrderDate DATE NOT NULL,
    CustomerID INT NOT NULL,
    TotalAmount DECIMAL(10,2)
) ON OrderDateScheme(OrderDate);
```

## Partition Management

### Split a Partition

```sql
-- Split partition at new boundary
ALTER PARTITION FUNCTION OrderDateRange()
SPLIT RANGE ('2026-01-01');
```

### Merge Partitions

```sql
-- Merge two partitions
ALTER PARTITION FUNCTION OrderDateRange()
MERGE RANGE ('2024-01-01');
```

### Move Partition to Another Table

```sql
-- Create archive table
CREATE TABLE Orders_Archive (
    OrderID INT NOT NULL,
    OrderDate DATE NOT NULL,
    CustomerID INT NOT NULL,
    TotalAmount DECIMAL(10,2)
) ON FG_Archive;

-- Move partition
ALTER TABLE Orders
SWITCH PARTITION 1 TO Orders_Archive;
```

## Partition Examples by Range

### By Date

```sql
CREATE PARTITION FUNCTION pf_Year (INT)
AS RANGE RIGHT FOR VALUES (2020, 2021, 2022, 2023, 2024);

CREATE PARTITION SCHEME ps_Year
AS PARTITION pf_Year
ALL TO (PRIMARY);

CREATE TABLE Sales (
    SaleID INT,
    SaleDate DATE,
    Amount DECIMAL(10,2)
) ON ps_Year(YEAR(SaleDate));
```

### By List

```sql
-- Partition by specific values
CREATE PARTITION FUNCTION pf_Region (VARCHAR(20))
AS RANGE FOR VALUES ('North', 'South', 'East', 'West');

CREATE PARTITION SCHEME ps_Region
AS PARTITION pf_Region
ALL TO (PRIMARY);

CREATE TABLE Employees (
    EmployeeID INT,
    Name VARCHAR(50),
    Region VARCHAR(20)
) ON ps_Region(Region);
```

## Viewing Partition Information

```sql
-- View partition details
SELECT 
    OBJECT_NAME(p.object_id) AS TableName,
    p.partition_number,
    p.rows AS RowCount,
    COALESCE(f.name, 'Default') AS PartitionFunction,
    rg.value AS BoundaryValue
FROM sys.partitions p
LEFT JOIN sys.partition_range_values rg ON p.partition_number = rg.boundary_id
LEFT JOIN sys.partition_functions f ON rg.function_id = f.function_id
WHERE OBJECT_NAME(p.object_id) = 'Orders'
  AND p.index_id IN (0, 1);
```

```sql
-- View partition schemes
SELECT * FROM sys.partition_schemes;

-- View partition functions
SELECT * FROM sys.partition_functions;
```

## Partition Maintenance

### Rebuild Specific Partition

```sql
-- Rebuild partition index
ALTER INDEX ALL ON Orders
REBUILD PARTITION = 2;
```

### Truncate Partition

```sql
-- Fast way to delete partition data
ALTER TABLE Orders
TRUNCATE PARTITION 1;
```

### Archive Old Data

```sql
-- Move partition to archive table
ALTER TABLE Orders
SWITCH PARTITION 1
TO Orders_Archive;
```

## Partition Elimination

The optimizer automatically skips irrelevant partitions:

```sql
-- This only reads partition for 2024
SELECT * FROM Orders 
WHERE OrderDate >= '2024-01-01' 
  AND OrderDate < '2025-01-01';
```

## Partitioned Indexes

```sql
-- Create index aligned with table
CREATE INDEX idx_Orders_Date ON Orders(OrderDate);

-- Create index on different scheme
CREATE INDEX idx_Orders_Customer ON Orders(CustomerID)
ON CustomerScheme(CustomerID);
```

## When to Use Partitioning

| Scenario | Partition Strategy |
|----------|-------------------|
| Time-series data | By date (daily, monthly, yearly) |
| Geographic data | By region/country |
| Large tables | By ranges that match queries |
| Archival needs | Separate historical data |

## Key Points Summary

| Concept | Description |
|---------|-------------|
| Partition Function | Defines how data is divided |
| Partition Scheme | Maps partitions to filegroups |
| Range Partition | By ranges of values |
| List Partition | By specific values |
| Partition Elimination | Optimizer skips unused partitions |

---

*This topic should take about 5-7 minutes to explain in class.*
