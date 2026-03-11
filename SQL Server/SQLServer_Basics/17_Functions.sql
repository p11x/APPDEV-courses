-- =====================================================
-- SQL Server: Functions
-- =====================================================
-- Functions are reusable code blocks that return a value

-- =====================================================
-- Types of Functions
-- =====================================================

-- 1. Scalar Functions: Return a single value
-- 2. Table-Valued Functions: Return a table
-- 3. Aggregate Functions: SUM, COUNT, AVG, MIN, MAX
-- 4. System Functions: Built-in SQL Server functions


-- =====================================================
-- Creating a Scalar Function
-- =====================================================

-- Example: Calculate employee bonus based on salary
CREATE FUNCTION fn_CalculateBonus
    (@Salary DECIMAL(10, 2))
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @Bonus DECIMAL(10, 2);
    
    IF @Salary > 50000
        SET @Bonus = @Salary * 0.15;  -- 15% bonus
    ELSE IF @Salary > 30000
        SET @Bonus = @Salary * 0.10;  -- 10% bonus
    ELSE
        SET @Bonus = @Salary * 0.05;  -- 5% @Bonus;
END bonus
    
    RETURN;
GO

-- Using the scalar function
SELECT 
    Name, 
    Salary, 
    dbo.fn_CalculateBonus(Salary) AS Bonus
FROM Employees;
GO


-- =====================================================
-- Creating a Table-Valued Function (Inline)
-- =====================================================

-- Example: Get employees by department
CREATE FUNCTION fn_GetEmployeesByDepartment
    (@Department VARCHAR(50))
RETURNS TABLE
AS
RETURN
(
    SELECT EmployeeID, Name, Department, Salary
    FROM Employees
    WHERE Department = @Department
);
GO

-- Using the table-valued function
SELECT * FROM fn_GetEmployeesByDepartment('IT');


-- =====================================================
-- Creating a Table-Valued Function (Multi-Statement)
-- =====================================================

CREATE FUNCTION fn_GetEmployeeSummary()
RETURNS @Summary TABLE
(
    Department VARCHAR(50),
    TotalEmployees INT,
    AvgSalary DECIMAL(10, 2),
    MaxSalary DECIMAL(10, 2),
    MinSalary DECIMAL(10, 2)
)
AS
BEGIN
    INSERT INTO @Summary
    SELECT 
        Department,
        COUNT(*) AS TotalEmployees,
        AVG(Salary) AS AvgSalary,
        MAX(Salary) AS MaxSalary,
        MIN(Salary) AS MinSalary
    FROM Employees
    GROUP BY Department;
    
    RETURN;
END;
GO

-- Using the function
SELECT * FROM fn_GetEmployeeSummary();


-- =====================================================
-- String Functions
-- =====================================================

-- LEN(): Returns length of string
SELECT LEN('Hello World');  -- Result: 11

-- UPPER()/LOWER(): Change case
SELECT UPPER('hello');  -- Result: HELLO
SELECT LOWER('HELLO');  -- Result: hello

-- SUBSTRING(): Extract part of string
SELECT SUBSTRING('Hello World', 1, 5);  -- Result: Hello

-- LTRIM()/RTRIM(): Remove spaces
SELECT LTRIM('   Hello');   -- Result: Hello
SELECT RTRIM('Hello   ');   -- Result: Hello

-- CONCAT(): Combine strings
SELECT CONCAT('Hello', ' ', 'World');  -- Result: Hello World

-- REPLACE(): Replace text
SELECT REPLACE('Hello World', 'World', 'SQL');  -- Result: Hello SQL


-- =====================================================
-- Date and Time Functions
-- =====================================================

-- GETDATE(): Current date and time
SELECT GETDATE();  -- Result: 2024-01-15 14:30:00.000

-- YEAR(), MONTH(), DAY(): Extract parts
SELECT 
    YEAR(GETDATE()) AS CurrentYear,
    MONTH(GETDATE()) AS CurrentMonth,
    DAY(GETDATE()) AS CurrentDay;

-- DATEADD(): Add time to date
SELECT DATEADD(YEAR, 1, '2024-01-15');  -- Result: 2025-01-15
SELECT DATEADD(MONTH, 3, '2024-01-15'); -- Result: 2024-04-15

-- DATEDIFF(): Difference between dates
SELECT DATEDIFF(DAY, '2024-01-01', '2024-01-15');  -- Result: 14
SELECT DATEDIFF(YEAR, '2020-01-15', '2024-01-15'); -- Result: 4

-- FORMAT(): Format date
SELECT FORMAT(GETDATE(), 'yyyy-MM-dd');  -- Result: 2024-01-15
SELECT FORMAT(GETDATE(), 'MMMM dd, yyyy'); -- Result: January 15, 2024


-- =====================================================
-- Mathematical Functions
-- =====================================================

-- ABS(): Absolute value
SELECT ABS(-10);  -- Result: 10

-- ROUND(): Round number
SELECT ROUND(123.456, 2);  -- Result: 123.46
SELECT ROUND(123.456, 0);  -- Result: 123.00

-- CEILING(): Round up
SELECT CEILING(12.3);  -- Result: 13

-- FLOOR(): Round down
SELECT FLOOR(12.9);  -- Result: 12

-- POWER(): Raise to power
SELECT POWER(2, 3);  -- Result: 8

-- SQRT(): Square root
SELECT SQRT(16);  -- Result: 4


-- =====================================================
-- Aggregate Functions
-- =====================================================

-- COUNT(): Count rows
SELECT COUNT(*) FROM Employees;
SELECT COUNT(DISTINCT Department) FROM Employees;

-- SUM(): Total of values
SELECT SUM(Salary) FROM Employees;

-- AVG(): Average value
SELECT AVG(Salary) FROM Employees;

-- MIN()/MAX(): Minimum/Maximum
SELECT MIN(Salary), MAX(Salary) FROM Employees;


-- =====================================================
-- Practical Example: Using Multiple Functions
-- =====================================================

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerName VARCHAR(50),
    OrderDate DATE,
    Amount DECIMAL(10, 2)
);

INSERT INTO Orders VALUES 
    (1, 'John Smith', '2024-01-15', 150.00),
    (2, 'Jane Doe', '2024-01-16', 250.50),
    (3, 'Bob Wilson', '2024-01-17', 75.25);

-- Query using multiple functions
SELECT 
    COUNT(*) AS TotalOrders,
    SUM(Amount) AS TotalAmount,
    AVG(Amount) AS AverageOrder,
    MIN(Amount) AS SmallestOrder,
    MAX(Amount) AS LargestOrder,
    YEAR(OrderDate) AS OrderYear,
    MONTH(OrderDate) AS OrderMonth
FROM Orders
GROUP BY YEAR(OrderDate), MONTH(OrderDate);


-- =====================================================
-- Dropping Functions
-- =====================================================

-- DROP FUNCTION fn_CalculateBonus;


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. Scalar Functions: Return single value
2. Table-Valued Functions: Return table
3. String Functions: LEN, UPPER, LOWER, SUBSTRING, CONCAT
4. Date Functions: GETDATE, DATEADD, DATEDIFF, FORMAT
5. Math Functions: ABS, ROUND, CEILING, FLOOR, POWER, SQRT
6. Aggregate Functions: COUNT, SUM, AVG, MIN, MAX

Functions can be used in SELECT, WHERE, and ORDER BY clauses!
*/
