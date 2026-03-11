-- =====================================================
-- SQL Server: JSON Data Handling
-- =====================================================
-- SQL Server provides native JSON support starting from SQL Server 2016

-- =====================================================
-- JSON Data Type
-- =====================================================

-- Create table with JSON column
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(100),
    ContactInfo JSON  -- JSON column
);

-- Insert JSON data
INSERT INTO Customers VALUES 
    (1, 'John Doe', '{"email": "john@email.com", "phone": "555-1234"}'),
    (2, 'Jane Smith', '{"email": "jane@email.com", "phone": "555-5678", "address": "123 Main St"}');


-- =====================================================
-- FOR JSON Clause
-- =====================================================

-- Convert query results to JSON

-- FOR JSON AUTO
SELECT 
    EmployeeID,
    Name,
    Department,
    Salary
FROM Employees
FOR JSON AUTO;

-- Result:
-- [
--   {"EmployeeID":1,"Name":"John","Department":"IT","Salary":50000},
--   {"EmployeeID":2,"Name":"Jane","Department":"HR","Salary":60000}
-- ]


-- FOR JSON PATH (more control)
SELECT 
    EmployeeID AS 'info.id',
    Name AS 'info.name',
    Department AS 'details.department',
    Salary AS 'details.compensation'
FROM Employees
FOR JSON PATH, ROOT('Employees');

-- Result:
-- {
--   "Employees": [
--     {
--       "info": {"id": 1, "name": "John"},
--       "details": {"department": "IT", "compensation": 50000}
--     }
--   ]
-- }


-- =====================================================
-- JSON_VALUE - Extract Single Value
-- =====================================================

SELECT 
    Name,
    JSON_VALUE(ContactInfo, '$.email') AS Email,
    JSON_VALUE(ContactInfo, '$.phone') AS Phone
FROM Customers;

-- Result:
-- Name         Email              Phone
-- John Doe     john@email.com     555-1234
-- Jane Smith   jane@email.com     555-5678


-- =====================================================
-- JSON_QUERY - Extract Object/Array
-- =====================================================

-- Create table with complex JSON
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerID INT,
    OrderData JSON
);

INSERT INTO Orders VALUES 
(1, 100, '{
    "items": [
        {"product": "Laptop", "qty": 1, "price": 999},
        {"product": "Mouse", "qty": 2, "price": 29}
    ],
    "shipping": {"method": "express", "address": "123 Main St"}
}');

-- Extract object
SELECT 
    OrderID,
    JSON_QUERY(OrderData, '$.shipping') AS ShippingInfo
FROM Orders;

-- Extract array
SELECT 
    OrderID,
    JSON_QUERY(OrderData, '$.items') AS Items
FROM Orders;


-- =====================================================
-- JSON_EXISTS - Check if Path Exists
-- =====================================================

SELECT 
    Name,
    ContactInfo,
    CASE 
        WHEN JSON_EXISTS(ContactInfo, '$.address') = 1 
        THEN 'Has Address' 
        ELSE 'No Address' 
    END AS AddressStatus
FROM Customers;


-- =====================================================
-- OPENJSON - Parse JSON Array
-- =====================================================

DECLARE @JSONArray NVARCHAR(MAX) = '
[
    {"ProductID": 1, "ProductName": "Laptop", "Price": 999},
    {"ProductID": 2, "ProductName": "Mouse", "Price": 29},
    {"ProductID": 3, "ProductName": "Keyboard", "Price": 79}
]';

-- Parse as table
SELECT *
FROM OPENJSON(@JSONArray)
WITH (
    ProductID INT '$.ProductID',
    ProductName VARCHAR(50) '$.ProductName',
    Price DECIMAL(10,2) '$.Price'
);


-- =====================================================
-- Update JSON Data
-- =====================================================

-- Update a value in JSON
UPDATE Customers
SET ContactInfo = JSON_MODIFY(ContactInfo, '$.phone', '555-9999')
WHERE CustomerID = 1;

-- Add new property
UPDATE Customers
SET ContactInfo = JSON_MODIFY(ContactInfo, '$.city', 'New York')
WHERE CustomerID = 1;

-- Remove property
UPDATE Customers
SET ContactInfo = JSON_MODIFY(ContactInfo, '$.phone', NULL)
WHERE CustomerID = 2;


-- =====================================================
-- Nested JSON Queries
-- =====================================================

SELECT 
    OrderID,
    item.value('$.product', 'VARCHAR(50)') AS Product,
    item.value('$.qty', 'INT') AS Quantity,
    item.value('$.price', 'DECIMAL(10,2)') AS Price
FROM Orders
CROSS APPLY OPENJSON(OrderData, '$.items') AS item;


-- =====================================================
-- Build JSON from Aggregations
-- =====================================================

SELECT 
    Department,
    COUNT(*) AS EmployeeCount,
    AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY Department
FOR JSON PATH;

-- Nested JSON from subquery
SELECT 
    d.DepartmentName,
    (SELECT Name, Salary FROM Employees e WHERE e.DepartmentID = d.DepartmentID FOR JSON PATH) AS Employees
FROM Departments d
FOR JSON PATH, ROOT('Department');


-- =====================================================
-- JSON in Constraints
-- =====================================================

-- Check for valid JSON
ALTER TABLE Customers
ADD CONSTRAINT CK_ContactInfo_ValidJSON
CHECK (ISJSON(ContactInfo) = 1);


-- =====================================================
-- Index JSON Data
-- =====================================================

-- Create computed column for indexing
ALTER TABLE Customers
ADD Email AS JSON_VALUE(ContactInfo, '$.email');

-- Create index on computed column
CREATE INDEX IX_Customers_Email ON Customers(Email);


-- =====================================================
-- Real-World Example: Event Logging
-- =====================================================

CREATE TABLE EventLog (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventType VARCHAR(50),
    EventData JSON,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Insert event with JSON data
INSERT INTO EventLog (EventType, EventData) VALUES 
('UserLogin', '{"userId": 123, "ip": "192.168.1.1", "browser": "Chrome"}'),
('OrderPlaced', '{"orderId": 456, "items": 3, "total": 150.00}'),
('Error', '{"errorCode": 500, "message": "Database timeout", "stack": "..."}');

-- Query specific events
SELECT 
    EventID,
    EventType,
    JSON_VALUE(EventData, '$.userId') AS UserID,
    EventData
FROM EventLog
WHERE EventType = 'UserLogin';


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. JSON_VALUE extracts scalar values from JSON
2. JSON_QUERY extracts objects/arrays from JSON
3. OPENJSON parses JSON into table rows
4. JSON_MODIFY updates JSON data
5. FOR JSON converts query results to JSON
6. ISJSON validates JSON format
7. JSON indexes improve query performance
*/
