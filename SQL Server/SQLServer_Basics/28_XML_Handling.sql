-- =====================================================
-- SQL Server: XML Handling
-- =====================================================
-- SQL Server has built-in support for XML data

-- =====================================================
-- XML Data Type
-- =====================================================

-- Create table with XML column
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Specifications XML  -- XML column
);

-- Insert XML data
INSERT INTO Products VALUES 
    (1, 'Laptop', '<Specs><RAM>16GB</RAM><Storage>512GB SSD</Storage></Specs>'),
    (2, 'Phone', '<Specs><RAM>8GB</RAM><Storage>128GB</Storage></Specs>');

-- Query XML data
SELECT ProductName, Specifications FROM Products;


-- =====================================================
-- FOR XML Clause
-- =====================================================

-- Convert query results to XML

-- Basic FOR XML
SELECT 
    EmployeeID AS ID,
    Name,
    Department
FROM Employees
FOR XML RAW;  -- Raw XML elements

-- FOR XML AUTO
SELECT 
    EmployeeID,
    Name,
    Department
FROM Employees
FOR XML AUTO;  -- Auto-generates element names

-- FOR XML PATH (most flexible)
SELECT 
    EmployeeID AS '@ID',
    Name,
    Department
FROM Employees
FOR XML PATH('Employee');  -- Custom root element


-- =====================================================
-- FOR XML with ROOT
-- =====================================================

SELECT 
    EmployeeID,
    Name,
    Department
FROM Employees
FOR XML PATH('Employee'), ROOT('Employees');

-- Result:
-- <Employees>
--   <Employee>
--     <EmployeeID>1</EmployeeID>
--     <Name>John</Name>
--     <Department>IT</Department>
--   </Employee>
-- </Employees>


-- =====================================================
-- XML Elements vs Attributes
-- =====================================================

-- Elements (default)
SELECT 
    EmployeeID,
    Name,
    Department
FROM Employees
FOR XML PATH('Employee');

-- Attributes
SELECT 
    EmployeeID AS '@ID',
    Name AS '@Name',
    Department AS '@Dept'
FROM Employees
FOR XML PATH('Employee');


-- =====================================================
-- Shredding XML to Relational Data
-- =====================================================

DECLARE @XMLData XML = '
<Employees>
    <Employee ID="1">
        <Name>John</Name>
        <Department>IT</Department>
    </Employee>
    <Employee ID="2">
        <Name>Jane</Name>
        <Department>HR</Department>
    </Employee>
</Employees>';

-- Use XMLTABLE method
SELECT 
    Emp.value('@ID', 'INT') AS EmployeeID,
    Emp.value('Name[1]', 'VARCHAR(50)') AS Name,
    Emp.value('Department[1]', 'VARCHAR(50)') AS Department
FROM @XMLData.nodes('/Employees/Employee') AS Tbl(Emp);


-- =====================================================
-- XQuery Methods
-- =====================================================

DECLARE @XMLData XML = '<Product><Price>99.99</Price><InStock>True</InStock></Product>';

-- query() - Extract XML
SELECT @XMLData.query('/Product/Price');

-- value() - Extract scalar value
SELECT @XMLData.value('(/Product/Price)[1]', 'DECIMAL(10,2)');

-- exist() - Check if node exists
SELECT @XMLData.exist('/Product/InStock[. = "True"]');

-- nodes() - Shred XML
SELECT Tbl.Col.value('.', 'VARCHAR(50)')
FROM @XMLData.nodes('/Product/*') AS Tbl(Col);


-- =====================================================
-- Practical Example: Storing Order Items
-- =====================================================

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerID INT,
    OrderItems XML  -- Stores multiple items
);

-- Insert order with multiple items as XML
INSERT INTO Orders VALUES 
(1, 100, '
<OrderItems>
    <Item ProductID="101" Quantity="2" Price="29.99" />
    <Item ProductID="102" Quantity="1" Price="49.99" />
    <Item ProductID="103" Quantity="3" Price="9.99" />
</OrderItems>');

-- Query: Get total for an order
DECLARE @OrderXML XML;
SELECT @OrderXML = OrderItems FROM Orders WHERE OrderID = 1;

SELECT 
    @OrderXML.value('sum(/OrderItems/Item/@Price * /OrderItems/Item/@Quantity)', 'DECIMAL(10,2)') 
    AS TotalAmount;


-- =====================================================
-- XML Indexes
-- =====================================================

-- Create primary XML index
CREATE PRIMARY XML INDEX PXML_ProductSpec
ON Products(Specifications);

-- Create secondary XML indexes (path, value, property)
CREATE XML INDEX SXML_ProductPath
ON Products(Specifications)
USING XML INDEX PXML_ProductSpec
FOR PATH;

CREATE XML INDEX SXML_ProductValue
ON Products(Specifications)
USING XML INDEX PXML_ProductSpec
FOR VALUE;


-- =====================================================
-- OPENXML
-- =====================================================

DECLARE @DocHandle INT;
DECLARE @XMLDoc VARCHAR(MAX);

SET @XMLDoc = '
<Employees>
    <Employee ID="1"><Name>John</Name><Salary>50000</Salary></Employee>
    <Employee ID="2"><Name>Jane</Name><Salary>60000</Salary></Employee>
</Employees>';

-- Prepare document
EXEC sp_xml_preparedocument @DocHandle OUTPUT, @XMLDoc;

-- Query as relational
SELECT *
FROM OPENXML(@DocHandle, '/Employees/Employee')
WITH (
    EmployeeID INT '@ID',
    Name VARCHAR(50) 'Name',
    Salary DECIMAL(10,2) 'Salary'
);

-- Clean up
EXEC sp_xml_removedocument @DocHandle;


-- =====================================================
-- Using XML with Namespaces
-- =====================================================

DECLARE @XMLWithNS XML = '
<Products xmlns:p="http://example.com/products">
    <p:Product ID="1">
        <p:Name>Laptop</p:Name>
        <p:Price>999</p:Price>
    </p:Product>
</Products>';

;WITH XMLNAMESPACES(DEFAULT 'http://example.com/products')
SELECT 
    prod.value('@ID', 'INT') AS ProductID,
    prod.value('(p:Name)[1]', 'VARCHAR(50)') AS ProductName,
    prod.value('(p:Price)[1]', 'DECIMAL(10,2)') AS Price
FROM @XMLWithNS.nodes('/Products/Product') AS Tbl(prod);


-- =====================================================
-- Converting Table to XML
-- =====================================================

-- Full document with attributes
SELECT 
    EmployeeID AS '@ID',
    Name AS '@Name',
    Department AS '@Dept'
FROM Employees
FOR XML PATH('Employee'), ROOT('Company');

-- Full document with elements
SELECT 
    EmployeeID,
    Name,
    Department
FROM Employees
FOR XML PATH('Employee'), ROOT('Company');


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. XML data type stores XML documents
2. FOR XML converts relational data to XML
3. XQuery methods: query(), value(), exist(), nodes()
4. XML indexes improve query performance
5. OPENXML provides rowset view of XML
6. XMLNAMESPACES handles prefixed XML
*/
