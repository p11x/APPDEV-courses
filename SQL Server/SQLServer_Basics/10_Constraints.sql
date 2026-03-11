-- =====================================================
-- SQL Server: Constraints
-- =====================================================
-- Constraints are rules applied to columns to ensure data integrity

-- =====================================================
-- PRIMARY KEY Constraint
-- =====================================================
-- Uniquely identifies each row in a table
-- Cannot have duplicate or NULL values
-- Only ONE primary key per table

CREATE TABLE Employees (
    -- Method 1: Column-level primary key
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50)
);

-- Method 2: Table-level primary key (for composite keys)
CREATE TABLE OrderItems (
    OrderID INT,
    ProductID INT,
    Quantity INT,
    PRIMARY KEY (OrderID, ProductID)  -- Composite key
);


-- =====================================================
-- NOT NULL Constraint
-- =====================================================
-- Ensures column must have a value (cannot be empty)

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,    -- Must have a value
    LastName VARCHAR(50) NOT NULL,     -- Must have a value
    Email VARCHAR(100),                -- Can be NULL
    Phone VARCHAR(20)                  -- Can be NULL
);

-- This would fail (FirstName is required):
-- INSERT INTO Customers VALUES (1, NULL, 'Smith', 'email@test.com', '123');


-- =====================================================
-- UNIQUE Constraint
-- =====================================================
-- Ensures all values in a column are different
-- Can have multiple unique columns
-- Allows NULL (one per column by default)

CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    Username VARCHAR(50) UNIQUE,        -- Each username must be unique
    Email VARCHAR(100) UNIQUE,          -- Each email must be unique
    Phone VARCHAR(20)                   -- Can have duplicates
);

-- This would fail (duplicate username):
-- INSERT INTO Users VALUES (1, 'john123', 'john@email.com', '123');


-- =====================================================
-- DEFAULT Constraint
-- =====================================================
-- Provides a default value when no value is specified

CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) DEFAULT 0.00,    -- Default to 0 if not specified
    Stock INT DEFAULT 0,                  -- Default to 0
    Status VARCHAR(20) DEFAULT 'Active',  -- Default to 'Active'
    CreatedDate DATE DEFAULT GETDATE()    -- Default to current date
);

-- Example: Insert without specifying defaults
INSERT INTO Products (ProductID, ProductName) VALUES (1, 'Widget');

-- Result: Price=0, Stock=0, Status='Active', CreatedDate=today


-- =====================================================
-- CHECK Constraint
-- =====================================================
-- Limits the values that can be placed in a column

CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Age INT CHECK (Age >= 0 AND Age <= 150),        -- Age must be 0-150
    Gender VARCHAR(10) CHECK (Gender IN ('Male', 'Female', 'Other')),
    Email VARCHAR(100)
);

-- This would fail (Age is negative):
-- INSERT INTO Students VALUES (1, 'John', -5, 'Male', 'john@email.com');


-- =====================================================
-- FOREIGN KEY Constraint
-- =====================================================
-- Creates a relationship between two tables
-- Ensures referential integrity

-- Parent table (referenced)
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(50) NOT NULL
);

-- Child table (with foreign key)
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    DepartmentID INT,                    -- Foreign key column
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- This would fail (Department doesn't exist):
-- INSERT INTO Employees VALUES (1, 'John', 999);


-- =====================================================
-- Complete Example with All Constraints
-- =====================================================

-- Drop tables if they exist
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Departments;

-- Create Department table first (parent)
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(50) NOT NULL UNIQUE,
    Location VARCHAR(50) DEFAULT 'Not Assigned'
);

-- Create Employee table (child)
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Salary DECIMAL(10, 2) CHECK (Salary >= 0),
    HireDate DATE DEFAULT GETDATE(),
    DepartmentID INT,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- Insert sample data into Departments
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES 
    (1, 'IT'),
    (2, 'HR'),
    (3, 'Finance');

-- Insert sample employees
INSERT INTO Employees (EmployeeID, FirstName, LastName, Email, Salary, DepartmentID)
VALUES (1, 'John', 'Doe', 'john@company.com', 50000, 1);

INSERT INTO Employees (EmployeeID, FirstName, LastName, Email, Salary, DepartmentID)
VALUES (2, 'Jane', 'Smith', 'jane@company.com', 55000, 2);


-- =====================================================
-- Viewing Constraints
-- =====================================================

-- View all constraints on a table
SELECT 
    constraint_name,
    constraint_type
FROM information_schema.table_constraints
WHERE table_name = 'Employees';


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. PRIMARY KEY: Unique identifier for each row
2. NOT NULL: Column must have a value
3. UNIQUE: No duplicate values allowed
4. DEFAULT: Value when none provided
5. CHECK: Validates data against condition
6. FOREIGN KEY: Links tables together

Constraints keep your data accurate and reliable!
*/
