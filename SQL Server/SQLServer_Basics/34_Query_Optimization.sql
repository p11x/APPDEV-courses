-- =====================================================
-- SQL Server: Query Optimization
-- =====================================================
-- Advanced techniques for optimizing complex queries

-- =====================================================
-- Understanding Execution Plans
-- =====================================================

-- View estimated execution plan
SET SHOWPLAN_TEXT ON;
GO

SELECT * FROM Orders WHERE OrderDate >= '2024-01-01';
GO

SET SHOWPLAN_TEXT OFF;
GO

-- View actual execution plan (in SSMS or use XML)
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

SELECT 
    o.OrderID,
    o.OrderDate,
    c.CustomerName,
    p.ProductName,
    od.Quantity,
    od.UnitPrice
FROM Orders o
INNER JOIN Customers c ON o.CustomerID = c.CustomerID
INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
INNER JOIN Products p ON od.ProductID = p.ProductID
WHERE o.OrderDate >= '2024-01-01';

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
GO


-- =====================================================
-- Query Hints
-- =====================================================

-- Force index usage
SELECT * FROM Orders WITH (INDEX(idx_OrderDate))
WHERE OrderDate >= '2024-01-01';

-- Force join type
SELECT * 
FROM Orders o
INNER LOOP JOIN Customers c ON o.CustomerID = c.CustomerID;

-- Force hash join
SELECT * 
FROM Orders o
INNER HASH JOIN Customers c ON o.CustomerID = c.CustomerID;

-- Force merge join
SELECT * 
FROM Orders o
INNER MERGE JOIN Customers c ON o.CustomerID = c.CustomerID;

-- Recompile each execution
SELECT * FROM Orders
WHERE OrderDate >= '2024-01-01'
OPTION (RECOMPILE);

-- Optimize for unknown parameter
EXEC sp_GetOrders @Status = 'Pending'
OPTION (OPTIMIZE FOR UNKNOWN);


-- =====================================================
-- Set Options for Performance
-- =====================================================

-- Enable advanced options
SET ANSI_DEFAULTS ON;
SET ARITHABORT ON;
SET CURSOR_CLOSE_ON_COMMIT OFF;
SET IMPLICIT_TRANSACTIONS OFF;
SET NOCOUNT ON;
SET NUMERIC_ROUNDABORT OFF;
SET QUOTED_IDENTIFIER ON;


-- =====================================================
-- Writing Efficient Queries
-- =====================================================

-- BAD: Using functions on columns
SELECT * FROM Orders
WHERE YEAR(OrderDate) = 2024 AND MONTH(OrderDate) = 1;

-- GOOD: Use range queries
SELECT * FROM Orders
WHERE OrderDate >= '2024-01-01' AND OrderDate < '2024-02-01';


-- BAD: SELECT *
SELECT * FROM Orders WHERE OrderID = 100;

-- GOOD: Select only needed columns
SELECT OrderID, OrderDate, TotalAmount 
FROM Orders WHERE OrderID = 100;


-- BAD: Multiple OR conditions
SELECT * FROM Products
WHERE Category = 'Electronics' OR Category = 'Books' OR Category = 'Clothing';

-- GOOD: Use IN
SELECT * FROM Products
WHERE Category IN ('Electronics', 'Books', 'Clothing');


-- BAD: Not using EXISTS
SELECT * FROM Customers c
WHERE (SELECT COUNT(*) FROM Orders o WHERE o.CustomerID = c.CustomerID) > 5;

-- GOOD: Use EXISTS
SELECT * FROM Customers c
WHERE EXISTS (SELECT 1 FROM Orders o WHERE o.CustomerID = c.CustomerID);


-- =====================================================
-- Optimizing JOINs
-- =====================================================

-- Put smaller table on left for INNER JOIN
SELECT *
FROM SmallCategory c  -- Small
INNER JOIN LargeProducts p ON c.CategoryID = p.CategoryID;  -- Large

-- Use appropriate join type
-- Hash join: large tables, no index
-- Merge join: sorted inputs
-- Loop join: small table with index


-- =====================================================
-- Using CTEs (Common Table Expressions)
-- =====================================================

-- Before: Nested subqueries
SELECT *
FROM (
    SELECT ProductID, Name, CategoryID
    FROM Products
    WHERE Price > 100
) p
INNER JOIN (
    SELECT CategoryID, CategoryName
    FROM Categories
    WHERE CategoryName LIKE 'E%'
) c ON p.CategoryID = c.CategoryID;

-- After: CTE
WITH ExpensiveProducts AS (
    SELECT ProductID, Name, CategoryID
    FROM Products
    WHERE Price > 100
),
ElectronicsCategories AS (
    SELECT CategoryID, CategoryName
    FROM Categories
    WHERE CategoryName LIKE 'E%'
)
SELECT p.ProductID, p.Name, c.CategoryName
FROM ExpensiveProducts p
INNER JOIN ElectronicsCategories c 
    ON p.CategoryID = c.CategoryID;


-- =====================================================
-- Using Window Functions
-- =====================================================

-- Before: Self-join for ranking
SELECT 
    e1.Name,
    e1.Department,
    e1.Salary,
    (SELECT COUNT(*) FROM Employees e2 
     WHERE e2.Department = e1.Department 
       AND e2.Salary > e1.Salary) + 1 AS RankInDept
FROM Employees e1;

-- After: Window function
SELECT 
    Name,
    Department,
    Salary,
    RANK() OVER (PARTITION BY Department ORDER BY Salary DESC) AS RankInDept
FROM Employees;


-- =====================================================
-- Set-Based vs Row-by-Row
-- =====================================================

-- BAD: Row-by-row (RBAR - Row By Agonizing Row)
DECLARE @Counter INT = 1;
WHILE @Counter <= 1000
BEGIN
    INSERT INTO LogTable VALUES (@Counter, GETDATE());
    SET @Counter = @Counter + 1;
END;

-- GOOD: Set-based
INSERT INTO LogTable (ID, LogDate)
SELECT TOP 1000 
    ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
    GETDATE()
FROM sys.all_objects;  -- or any large table


-- =====================================================
-- Temporary Tables vs Table Variables
-- =====================================================

-- Use temp table for large datasets
CREATE TABLE #TempResults (
    ID INT,
    Name VARCHAR(50),
    Total DECIMAL(10,2)
);

INSERT INTO #TempResults
SELECT CustomerID, CustomerName, SUM(TotalAmount)
FROM Orders
GROUP BY CustomerID, CustomerName;

-- Use for complex operations
CREATE INDEX IX_Temp ON #TempResults(ID);

SELECT * FROM #TempResults;

DROP TABLE #TempResults;


-- =====================================================
-- Optimizing Aggregations
-- =====================================================

-- Use COUNT_BIG for large tables
SELECT COUNT_BIG(*) FROM Orders;  -- Faster than COUNT(*)

-- Pre-aggregate with indexed views
CREATE VIEW vw_OrderDailyTotals
WITH SCHEMABINDING
AS
SELECT 
    CAST(OrderDate AS DATE) AS OrderDate,
    COUNT_BIG(*) AS OrderCount,
    SUM(TotalAmount) AS TotalAmount
FROM dbo.Orders
GROUP BY CAST(OrderDate AS DATE);

CREATE UNIQUE CLUSTERED INDEX IX_Date 
ON vw_OrderDailyTotals(OrderDate);


-- =====================================================
-- Plan Guides
-- =====================================================

-- Create plan guide for specific query
EXEC sp_create_plan_guide 
    @name = N'Guide_Optimize_Orders',
    @stmt = N'SELECT * FROM Orders WHERE OrderDate >= @StartDate',
    @type = N'SQL',
    @module_or_batch = NULL,
    @hints = N'OPTION (OPTIMIZE FOR (@StartDate = ''2024-01-01''))';


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. Analyze execution plans to find bottlenecks
2. Use query hints sparingly - they can hurt more than help
3. Set proper session options for performance
4. Avoid functions on columns in WHERE clauses
5. Use EXISTS instead of COUNT(*) for existence checks
6. Prefer set-based operations over row-by-row
7. Use CTEs for readable, maintainable code
8. Use window functions for analytical queries
9. Choose between temp tables and table variables wisely
10. Consider indexed views for frequent aggregations
*/
