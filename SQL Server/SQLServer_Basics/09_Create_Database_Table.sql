-- =====================================================
-- SQL Server: CREATE DATABASE and CREATE TABLE
-- =====================================================

-- =====================================================
-- CREATE DATABASE
-- =====================================================

-- Example: Create a new database
-- This creates a new database named CollegeDB
CREATE DATABASE CollegeDB;

-- To use the database, you must switch to it
USE CollegeDB;

-- Or create with specific options
-- CREATE DATABASE CollegeDB
-- ON PRIMARY (NAME = 'CollegeDB_Data', FILENAME = 'C:\Data\CollegeDB.mdf')
-- LOG ON (NAME = 'CollegeDB_Log', FILENAME = 'C:\Data\CollegeDB.ldf');


-- =====================================================
-- CREATE TABLE
-- =====================================================

-- Example: Create the Students table
-- Define columns with their data types and constraints

CREATE TABLE Students (
    -- Column definitions
    ID INT,           -- Student ID (integer)
    Name VARCHAR(50), -- Student name (up to 50 characters)
    Age INT,          -- Student age (integer)
    Department VARCHAR(50) -- Department name
);

-- =====================================================
-- Column Explanation
-- =====================================================

/*
Column Name       | Data Type   | Description
------------------|-------------|-----------------------
ID                | INT         | Whole number for unique ID
Name              | VARCHAR(50) | Variable-length text (max 50 chars)
Age               | INT         | Whole number
Department        | VARCHAR(50) | Variable-length text
*/


-- =====================================================
-- Better Table with Primary Key
-- =====================================================

-- Let's create a more complete table
DROP TABLE IF EXISTS Students;  -- Remove old table

CREATE TABLE Students (
    ID INT PRIMARY KEY,         -- Primary key: unique identifier
    Name VARCHAR(50) NOT NULL,   -- Cannot be empty
    Age INT,                    -- Can be NULL
    Department VARCHAR(50),      -- Can be NULL
    EnrollmentDate DATE DEFAULT GETDATE()  -- Auto-set to today if not provided
);

-- =====================================================
-- More Example Tables
-- =====================================================

-- Example: Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2),       -- Decimal: 10 digits, 2 after decimal
    Stock INT DEFAULT 0,
    Category VARCHAR(50)
);

-- Example: Orders table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerID INT,
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2),
    Status VARCHAR(20) DEFAULT 'Pending'
);


-- =====================================================
-- Viewing Table Structure
-- =====================================================

-- View table information
EXEC sp_columns Students;

-- Or view all tables in database
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';


-- =====================================================
-- Dropping (Deleting) Tables and Databases
-- =====================================================

-- Delete a table (permanent!)
-- DROP TABLE Products;

-- Delete a database (permanent!)
-- DROP DATABASE CollegeDB;


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. CREATE DATABASE: Creates a new database container
2. CREATE TABLE: Creates a table with columns
3. Each column needs a name and data type
4. PRIMARY KEY: Uniquely identifies each row
5. NOT NULL: Column must have a value
6. DEFAULT: Value if not provided
7. DROP: Permanently deletes table/database
*/


-- =====================================================
-- Complete Example: Full Setup Script
-- =====================================================

-- Create database
CREATE DATABASE OnlineStore;
GO

-- Use the database
USE OnlineStore;
GO

-- Create Customers table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Phone VARCHAR(20),
    City VARCHAR(50),
    CreatedDate DATE DEFAULT GETDATE()
);
GO

-- Create Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    Price DECIMAL(10, 2) NOT NULL,
    StockQuantity INT DEFAULT 0,
    Category VARCHAR(50)
);
GO

-- Verify tables created
SELECT name FROM sys.tables;
GO

PRINT 'Database and tables created successfully!';
GO
