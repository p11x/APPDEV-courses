# SQL Server Full-Text Search

## What is Full-Text Search?

**Full-Text Search** (FTS) allows you to search for words or phrases in text data. Unlike LIKE, it understands word boundaries, stemming, and can rank results by relevance.

## Why Use Full-Text Search?

- Search for words within text columns
- Find variations of words (running, ran = run)
- Rank results by relevance
- Search multiple tables efficiently
- Better performance on large text columns

## Setting Up Full-Text Search

### Step 1: Enable Full-Text Search

```sql
-- Check if enabled (run in each database)
USE master;
GO

-- Enable on database
ALTER DATABASE MyDatabase SET FULLTEXT_ON;
GO

-- Or create with full-text
CREATE DATABASE MyDB;
GO
USE MyDB;
GO
CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;
GO
```

### Step 2: Create Full-Text Index

```sql
-- Create full-text index on table
CREATE FULLTEXT INDEX ON Products(
    Description  -- Column to index
)
KEY INDEX PK_Products  -- Unique key index
ON ftCatalog  -- Full-text catalog
WITH (STOPLIST = SYSTEM);  -- Use system stoplist
GO

-- Or multiple columns
CREATE FULLTEXT INDEX ON Products(
    ProductName,
    Description,
    Comments
)
KEY INDEX PK_Products
ON ftCatalog;
```

## Full-Text Search Queries

### CONTAINS - Exact Word/Phrase

```sql
-- Find exact word
SELECT ProductName, Description
FROM Products
WHERE CONTAINS(Description, 'Laptop');

-- Find phrase
SELECT ProductName, Description
FROM Products
WHERE CONTAINS(Description, '"high performance"');

-- Find words near each other
SELECT ProductName, Description
FROM Products
WHERE CONTAINS(Description, 'NEAR((laptop, mouse), 5)');
```

### CONTAINSTABLE - Ranked Results

```sql
-- Returns ranked results
SELECT 
    p.ProductName,
    ft.RANK,
    p.Description
FROM Products p
INNER JOIN CONTAINSTABLE(Products, Description, 'Laptop') ft
ON p.ProductID = ft.[KEY]
ORDER BY ft.RANK DESC;
```

### FREETEXT - Natural Language Search

```sql
-- Find matches based on meaning
SELECT ProductName, Description
FROM Products
WHERE FREETEXT(Description, 'computer laptop machine');
```

### FREETEXTTABLE - Ranked Natural Language

```sql
SELECT 
    p.ProductName,
    ft.RANK,
    p.Description
FROM Products p
INNER JOIN FREETEXTTABLE(Products, Description, 'database management') ft
ON p.ProductID = ft.[KEY]
ORDER BY ft.RANK DESC;
```

## Search Options

### Boolean Operators

```sql
-- AND (default)
SELECT * FROM Products
WHERE CONTAINS(Description, 'Laptop AND Mouse');

-- OR
SELECT * FROM Products
WHERE CONTAINS(Description, 'Laptop OR Tablet');

-- NOT
SELECT * FROM Products
WHERE CONTAINS(Description, 'Laptop AND NOT Gaming');
```

### Prefix Searches

```sql
-- Words starting with prefix
SELECT ProductName
FROM Products
WHERE CONTAINS(ProductName, '"lap*"');
```

### Inflectional Search

```sql
-- Find word variations
SELECT ProductName
FROM Products
WHERE CONTAINS(Description, 'FORMSOF(INFLECTIONAL, run)');
-- Matches: run, running, ran, runner
```

### Thesaurus Searches

```sql
-- Define thesaurus in XML and use
SELECT ProductName
FROM Products
WHERE CONTAINS(Description, 'FORMSOF(THESAURUS, computer)');
-- Matches: computer, PC, laptop, desktop
```

## Stopwords and Stoplists

### Default Stopwords

Common words like "the", "and", "is" are ignored:

```sql
-- View system stopwords
SELECT * FROM sys.fulltext_system_stopwords
WHERE language_id = 1033;  -- English
```

### Custom Stoplist

```sql
-- Create custom stoplist
CREATE FULLTEXT STOPLIST CustomStopList
FROM SYSTEM STOPLIST;

-- Add stopword
ALTER FULLTEXT STOPLIST CustomStopList
ADD 'urgent' LANGUAGE English;

-- Remove stopword
ALTER FULLTEXT STOPLIST CustomStopList
DROP 'urgent' LANGUAGE English;

-- Use custom stoplist
CREATE FULLTEXT INDEX ON Products(Description)
KEY INDEX PK_Products
ON ftCatalog
WITH (STOPLIST = CustomStopList);
```

## Performance Considerations

### Population Types

```sql
-- Change population schedule
ALTER FULLTEXT INDEX ON Products
SET CHANGE_TRACKING = AUTO;  -- Auto-update
-- or
SET CHANGE_TRACKING = MANUAL;  -- Manual update

-- Manual population
ALTER FULLTEXT INDEX ON Products START FULL POPULATION;
ALTER FULLTEXT INDEX ON Products START INCREMENTAL POPULATION;

-- Check population status
SELECT 
    OBJECT_NAME(PARENT_OBJECT_ID) AS TableName,
    * 
FROM sys.fulltext_indexes;
```

## Full-Text in Applications

### Simple Search Stored Procedure

```sql
CREATE PROCEDURE sp_FullTextSearch
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT 
        p.ProductID,
        p.ProductName,
        p.Description,
        ft.RANK
    FROM Products p
    INNER JOIN CONTAINSTABLE(Products, (ProductName, Description), @SearchTerm) ft
    ON p.ProductID = ft.[KEY]
    ORDER BY ft.RANK DESC;
END;
GO

EXEC sp_FullTextSearch @SearchTerm = 'laptop';
```

## Key Points Summary

| Function | Use Case |
|----------|----------|
| CONTAINS | Exact word or phrase search |
| CONTAINSTABLE | Ranked exact search |
| FREETEXT | Meaning-based search |
| FREETEXTTABLE | Ranked meaning search |
| FORMSOF | Word variations |
| NEAR | Proximity search |

---

*This topic should take about 5-7 minutes to explain in class.*
