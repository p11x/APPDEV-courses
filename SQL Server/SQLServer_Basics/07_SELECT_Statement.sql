-- =====================================================
-- SQL Server SELECT Statement
-- =====================================================
-- The SELECT statement is used to retrieve data from a database table.
-- It is the most commonly used SQL command.

-- First, let's create a sample table to work with
CREATE TABLE Students (
    ID INT PRIMARY KEY,
    Name VARCHAR(50),
    Age INT,
    Department VARCHAR(50)
);

-- Insert sample data
INSERT INTO Students VALUES (1, 'John', 20, 'Computer Science');
INSERT INTO Students VALUES (2, 'Jane', 21, 'Mathematics');
INSERT INTO Students VALUES (3, 'Mike', 19, 'Physics');
INSERT INTO Students VALUES (4, 'Sarah', 20, 'Chemistry');
INSERT INTO Students VALUES (5, 'David', 22, 'Computer Science');

-- =====================================================
-- SELECT Examples
-- =====================================================

-- Example 1: Select all columns from a table
-- This returns every column and every row
SELECT * 
FROM Students;

-- Expected Output:
-- +----+-------+-----+-------------------+
-- | ID | Name  | Age | Department        |
-- +----+-------+-----+-------------------+
-- | 1  | John  | 20  | Computer Science  |
-- | 2  | Jane  | 21  | Mathematics        |
-- | 3  | Mike  | 19  | Physics            |
-- | 4  | Sarah | 20  | Chemistry          |
-- | 5  | David | 22  | Computer Science  |
-- +----+-------+-----+-------------------+


-- Example 2: Select specific columns
-- Only returns the columns you specify
SELECT Name, Age 
FROM Students;

-- Expected Output:
-- +-------+-----+
-- | Name  | Age |
-- +-------+-----+
-- | John  | 20  |
-- | Jane  | 21  |
-- | Mike  | 19  |
-- | Sarah | 20  |
-- | David | 22  |
-- +-------+-----+


-- Example 3: Select with WHERE clause
-- Filter results based on a condition
SELECT * 
FROM Students 
WHERE Age > 20;

-- Expected Output:
-- +----+-------+-----+-------------------+
-- | ID | Name  | Age | Department        |
-- +----+-------+-----+-------------------+
-- | 2  | Jane  | 21  | Mathematics       |
-- | 5  | David | 22  | Computer Science  |
-- +----+-------+-----+-------------------+


-- Example 4: Select with multiple conditions
SELECT Name, Department 
FROM Students 
WHERE Age >= 20 AND Department = 'Computer Science';

-- Expected Output:
-- +------+-------------------+
-- | Name | Department        |
-- +------+-------------------+
-- | John | Computer Science |
-- | David| Computer Science |
-- +------+-------------------+


-- Example 5: Select with ORDER BY
-- Sort results by a column
SELECT Name, Age 
FROM Students 
ORDER BY Age DESC;

-- Expected Output:
-- +------+-----+
-- | Name | Age |
-- +------+-----+
-- | David| 22  |
-- | Jane | 21  |
-- | John | 20  |
-- | Sarah| 20  |
-- | Mike | 19  |
-- +------+-----+


-- Example 6: Select with LIKE (pattern matching)
-- Find names that start with 'J'
SELECT * 
FROM Students 
WHERE Name LIKE 'J%';

-- Expected Output:
-- +----+------+-----+----------------+
-- | ID | Name | Age| Department     |
-- +----+------+-----+----------------+
-- | 1  | John | 20 | Computer Science|
-- | 2  | Jane | 21 | Mathematics    |
-- +----+------+-----+----------------+


-- =====================================================
-- Key Points:
-- 1. SELECT specifies which columns to retrieve
-- 2. FROM specifies which table to query
-- 3. WHERE filters rows based on conditions
-- 4. ORDER BY sorts the results
-- 5. Use * to select all columns
-- =====================================================
