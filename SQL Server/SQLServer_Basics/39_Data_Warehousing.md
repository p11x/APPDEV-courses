# SQL Server Data Warehousing

## What is a Data Warehouse?

A **Data Warehouse** is a centralized repository that stores data from multiple sources for analytics and reporting. It's optimized for read operations and complex queries.

## Data Warehouse Characteristics

| Characteristic | Description |
|----------------|-------------|
| Subject-Oriented | Organized by subjects (sales, inventory) |
| Integrated | Data from multiple sources |
| Time-Variant | Historical data maintained |
| Non-Volatile | Data not deleted or changed |

## Data Warehouse vs Operational Database

| Feature | Operational DB | Data Warehouse |
|---------|----------------|----------------|
| Purpose | Daily transactions | Analytics |
| Data | Current | Historical |
| Design | Normalized | Dimensional |
| Queries | Simple, fast | Complex, aggregations |
| Updates | Real-time | Periodic |

## Dimensional Modeling

### Fact Tables

Store quantitative data (measurements):

```sql
CREATE TABLE FactSales (
    SalesKey INT PRIMARY KEY,
    DateKey INT,
    ProductKey INT,
    CustomerKey INT,
    StoreKey INT,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    TotalAmount AS (Quantity * UnitPrice) PERSISTED,
    FOREIGN KEY (DateKey) REFERENCES DimDate(DateKey),
    FOREIGN KEY (ProductKey) REFERENCES DimProduct(ProductKey),
    FOREIGN KEY (CustomerKey) REFERENCES DimCustomer(CustomerKey),
    FOREIGN KEY (StoreKey) REFERENCES DimStore(StoreKey)
);
```

### Dimension Tables

Store descriptive attributes:

```sql
CREATE TABLE DimProduct (
    ProductKey INT PRIMARY KEY,
    ProductID INT,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Subcategory VARCHAR(50),
    Brand VARCHAR(50),
    Color VARCHAR(20),
    ListPrice DECIMAL(10,2),
    Cost DECIMAL(10,2),
    StartDate DATE,
    EndDate DATE,
    IsCurrent BIT DEFAULT 1
);

CREATE TABLE DimDate (
    DateKey INT PRIMARY KEY,
    Date DATE,
    Year SMALLINT,
    Quarter TINYINT,
    Month TINYINT,
    Day TINYINT,
    DayOfWeek TINYINT,
    DayName VARCHAR(10),
    MonthName VARCHAR(10),
    WeekOfYear TINYINT,
    FiscalYear SMALLINT,
    FiscalQuarter TINYINT
);
```

## Star Schema

```
            ┌─────────────┐
            │  DimDate    │
            └──────┬──────┘
                   │
         ┌─────────┼─────────┐
         │         │         │
         ▼         ▼         ▼
    ┌────────┐ ┌──────────┐ ┌───────────┐
    │DimProduct│ │DimCustomer│ │ DimStore │
    └────┬───┘ └────┬─────┘ └─────┬─────┘
         │         │            │
         └─────────┼────────────┘
                   │
            ┌──────┴──────┐
            │ FactSales  │
            └─────────────┘
```

## Snowflake Schema

Normalized dimension tables (more complex):

```
DimProduct
   ↑
DimProductCategory ←── DimProductSubcategory
```

## Creating a Simple Star Schema

```sql
-- Create dimension tables
CREATE TABLE DimCustomer (
    CustomerKey INT PRIMARY KEY,
    CustomerID INT,
    CustomerName VARCHAR(100),
    Email VARCHAR(100),
    City VARCHAR(50),
    State VARCHAR(50),
    Country VARCHAR(50)
);

CREATE TABLE DimProduct (
    ProductKey INT PRIMARY KEY,
    ProductID INT,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Price DECIMAL(10,2)
);

CREATE TABLE DimDate (
    DateKey INT PRIMARY KEY,
    Date DATE,
    Year INT,
    Month INT,
    MonthName VARCHAR(20)
);

-- Create fact table
CREATE TABLE FactOrders (
    FactKey INT IDENTITY(1,1) PRIMARY KEY,
    DateKey INT FOREIGN KEY REFERENCES DimDate(DateKey),
    CustomerKey INT FOREIGN KEY REFERENCES DimCustomer(CustomerKey),
    ProductKey INT FOREIGN KEY REFERENCES DimProduct(ProductKey),
    Quantity INT,
    Amount DECIMAL(10,2)
);
```

## Slowly Changing Dimensions (SCD)

### Type 1: Overwrite

```sql
-- Update customer - overwrite old value
UPDATE DimCustomer
SET City = 'New York'
WHERE CustomerKey = 1;
-- Original city lost
```

### Type 2: Add Version

```sql
-- Add new row with version
INSERT INTO DimCustomer 
    (CustomerKey, CustomerID, CustomerName, City, StartDate, EndDate, IsCurrent)
VALUES 
    (2, 1002, 'John Doe', 'Boston', '2024-01-01', '2024-06-30', 0);

INSERT INTO DimCustomer 
    (CustomerKey, CustomerID, CustomerName, City, StartDate, EndDate, IsCurrent)
VALUES 
    (3, 1002, 'John Doe', 'Chicago', '2024-07-01', NULL, 1);
```

## Key Points Summary

| Concept | Description |
|---------|-------------|
| Fact Table | Quantitative data (sales, quantities) |
| Dimension | Descriptive attributes |
| Star Schema | Fact + Dimensions (denormalized) |
| Snowflake | Normalized dimensions |
| SCD | Track historical changes |

---

*This topic should take about 5-7 minutes to explain in class.*
