# SQL Server ETL Processes

## What is ETL?

**ETL** stands for Extract, Transform, Load - the process of moving data from source systems to a data warehouse.

```
┌──────────┐    ┌────────────┐    ┌─────────┐
│ Extract  │───►│ Transform  │───►│  Load   │
│          │    │            │    │         │
│ Sources  │    │ Clean/     │    │ Data    │
│ (OLTP)   │    │ Convert    │    │ Warehouse│
└──────────┘    └────────────┘    └─────────┘
```

## ETL Phases

### 1. Extract

Get data from source systems:

```sql
-- Extract from multiple sources
-- Source 1: SQL Server
INSERT INTO Staging_Customers_SQL
SELECT * FROM SourceDB.dbo.Customers
WHERE LastModified > @LastExtractionDate;

-- Source 2: Oracle
INSERT INTO Staging_Customers_ORA
SELECT * FROM OPENQUERY(OracleLink, 
    'SELECT * FROM customers WHERE modified_date > SYSDATE - 1');
```

### 2. Transform

Clean, validate, and convert data:

```sql
-- Data cleansing
INSERT INTO Staging_Customers_Clean
SELECT 
    CustomerID,
    UPPER(TRIM(CustomerName)) AS CustomerName,  -- Remove spaces, uppercase
    CASE 
        WHEN Email IS NULL THEN 'unknown@company.com'
        WHEN Email LIKE '%@%.%' THEN Email
        ELSE 'invalid@company.com'
    END AS Email,
    ISNULL(Phone, 'N/A') AS Phone,
    CAST(BirthDate AS DATE) AS BirthDate,
    GETDATE() AS LoadDate
FROM Staging_Customers_SQL
WHERE CustomerName IS NOT NULL;  -- Filter bad records
```

### 3. Load

Insert into destination:

```sql
-- Load to dimension table
INSERT INTO DimCustomer (
    CustomerKey,
    CustomerID,
    CustomerName,
    Email,
    Phone,
    BirthDate,
    LoadDate
)
SELECT 
    ISNULL((SELECT MAX(CustomerKey) FROM DimCustomer), 0) + ROW_NUMBER() OVER (ORDER BY CustomerID),
    CustomerID,
    CustomerName,
    Email,
    Phone,
    BirthDate,
    LoadDate
FROM Staging_Customers_Clean;
```

## Change Data Capture (CDC)

Track changes in source:

```sql
-- Enable CDC on source database
USE MySourceDB;
EXEC sp_cdc_enable_db;

-- Enable on table
EXEC sp_cdc_enable_table 
    @source_schema = 'dbo',
    @source_name = 'Customers',
    @role_name = NULL;

-- Query CDC data
SELECT * FROM cdc.dbo_Customers_CT
WHERE __$operation = 2;  -- Insert = 2, Update = 3, Delete = 4
```

## Incremental Load Pattern

```sql
-- Create watermark table
CREATE TABLE ETL_Watermark (
    TableName VARCHAR(100),
    WatermarkValue DATETIME,
    LastUpdated DATETIME DEFAULT GETDATE()
);

-- Initialize watermark
INSERT INTO ETL_Watermark VALUES ('Customers', '2024-01-01', GETDATE());

-- Get incremental data
DECLARE @Watermark DATETIME;
SELECT @Watermark = WatermarkValue FROM ETL_Watermark WHERE TableName = 'Customers';

INSERT INTO Staging_Customers
SELECT * FROM SourceDB.dbo.Customers
WHERE ModifiedDate > @Watermark;

-- Update watermark
UPDATE ETL_Watermark 
SET WatermarkValue = GETDATE(), LastUpdated = GETDATE()
WHERE TableName = 'Customers';
```

## Data Quality Checks

```sql
-- Create validation table
CREATE TABLE ETL_Validation (
    CheckID INT IDENTITY(1,1) PRIMARY KEY,
    CheckName VARCHAR(100),
    TableName VARCHAR(100),
    PassCount INT,
    FailCount INT,
    CheckDate DATETIME DEFAULT GETDATE()
);

-- Run validation checks
-- Check 1: No null customer IDs
INSERT INTO ETL_Validation (CheckName, TableName, PassCount, FailCount)
SELECT 
    'Null CustomerID Check',
    'Staging_Customers',
    SUM(CASE WHEN CustomerID IS NOT NULL THEN 1 ELSE 0 END),
    SUM(CASE WHEN CustomerID IS NULL THEN 1 ELSE 0 END)
FROM Staging_Customers;

-- Check 2: Valid email format
INSERT INTO ETL_Validation (CheckName, TableName, PassCount, FailCount)
SELECT 
    'Valid Email Check',
    'Staging_Customers',
    SUM(CASE WHEN Email LIKE '%@%.%' THEN 1 ELSE 0 END),
    SUM(CASE WHEN Email NOT LIKE '%@%.%' THEN 1 ELSE 0 END)
FROM Staging_Customers;
```

## ETL Best Practices

| Practice | Description |
|----------|-------------|
| Staging Area | Use staging tables for processing |
| Logging | Track execution and errors |
| Incremental | Load only changed data |
| Validation | Check data quality |
| Transactions | Use for rollback |

## Key Points Summary

| Phase | Purpose | Example |
|-------|---------|---------|
| Extract | Get source data | SELECT from source |
| Transform | Clean/convert | Remove nulls, uppercase |
| Load | Insert to destination | INSERT to warehouse |

---

*This topic should take about 5-7 minutes to explain in class.*
