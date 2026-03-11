-- =====================================================
-- SQL Server Data Manipulation: INSERT, UPDATE, DELETE
-- =====================================================

-- Create sample table for demonstration
CREATE TABLE Students (
    ID INT PRIMARY KEY,
    Name VARCHAR(50),
    Age INT,
    Department VARCHAR(50)
);

-- Insert initial data
INSERT INTO Students VALUES (1, 'John', 20, 'Computer Science');
INSERT INTO Students VALUES (2, 'Jane', 21, 'Mathematics');

-- =====================================================
-- INSERT Statement
-- =====================================================
-- INSERT adds new rows to a table

-- Example 1: Insert a single row (all columns)
-- Provide values for all columns in order
INSERT INTO Students VALUES (3, 'Mike', 19, 'Physics');

-- Result: New row added
-- +----+------+-----+------------+
-- | ID | Name | Age | Department |
-- +----+------+-----+------------+
-- | 1  | John | 20  | Comp Sci   |
-- | 2  | Jane | 21  | Math       |
-- | 3  | Mike | 19  | Physics    |
-- +----+------+-----+------------+


-- Example 2: Insert with column list
-- Specify which columns to insert
INSERT INTO Students (ID, Name, Age) VALUES (4, 'Sarah', 20);

-- Result: Department will be NULL
-- +----+-------+-----+------------+
-- | ID | Name  | Age | Department |
-- +----+-------+-----+------------+
-- | 4  | Sarah | 20  | NULL       |
-- +----+-------+-----+------------+


-- Example 3: Insert multiple rows
-- Add multiple rows in one statement
INSERT INTO Students (ID, Name, Age, Department) VALUES 
    (5, 'David', 22, 'Chemistry'),
    (6, 'Emma', 20, 'Biology'),
    (7, 'James', 21, 'Physics');

-- Result: Three new rows added


-- =====================================================
-- UPDATE Statement
-- =====================================================
-- UPDATE modifies existing data in a table

-- Current table state:
-- +----+--------+-----+----------------+
-- | ID | Name   | Age | Department     |
-- +----+--------+-----+----------------+
-- | 1  | John   | 20  | Computer Sci   |
-- | 2  | Jane   | 21  | Mathematics    |
-- | 3  | Mike   | 19  | Physics        |
-- | 4  | Sarah  | 20  | NULL           |
-- | 5  | David  | 22  | Chemistry      |
-- | 6  | Emma   | 20  | Biology        |
-- | 7  | James  | 21  | Physics        |
-- +----+--------+-----+----------------+


-- Example 1: Update a single row
-- Use WHERE to specify which row to update
UPDATE Students
SET Age = 21
WHERE ID = 1;

-- Result:
-- John's age changed from 20 to 21


-- Example 2: Update multiple columns
-- Update more than one column at once
UPDATE Students
SET Age = 23, Department = 'Computer Science'
WHERE Name = 'David';

-- Result:
-- David: Age 22→23, Department 'Chemistry'→'Computer Science'


-- Example 3: Update with WHERE condition
-- Update all rows that match condition
UPDATE Students
SET Department = 'Natural Sciences'
WHERE Department = 'Physics';

-- Result: Both Mike and James get updated


-- =====================================================
-- DELETE Statement
-- =====================================================
-- DELETE removes rows from a table

-- Current table state before DELETE:
-- +----+--------+-----+----------------+
-- | ID | Name   | Age | Department     |
-- +----+--------+-----+----------------+
-- | 1  | John   | 21  | Computer Sci   |
-- | 2  | Jane   | 21  | Mathematics    |
-- | 3  | Mike   | 19  | Nat Sciences   |
-- | 4  | Sarah  | 20  | NULL           |
-- | 5  | David  | 23  | Computer Sci   |
-- | 6  | Emma   | 20  | Biology        |
-- | 7  | James  | 21  | Nat Sciences   |
-- +----+--------+-----+----------------+


-- Example 1: Delete a single row
-- Remove specific row using WHERE
DELETE FROM Students
WHERE ID = 4;

-- Result: Sarah's row is deleted


-- Example 2: Delete with condition
-- Delete multiple rows matching condition
DELETE FROM Students
WHERE Department = 'Natural Sciences';

-- Result: Mike and James are deleted


-- Example 3: CAUTION - Delete all rows
-- Without WHERE, ALL rows are deleted!
-- DELETE FROM Students;  -- This would delete everything!

-- Always backup or use WHERE clause!


-- =====================================================
-- Final Table State
-- =====================================================
-- +----+--------+-----+----------------+
-- | ID | Name   | Age | Department     |
-- +----+--------+-----+----------------+
-- | 1  | John   | 21  | Computer Sci   |
-- | 2  | Jane   | 21  | Mathematics    |
-- | 5  | David  | 23  | Computer Sci   |
-- | 6  | Emma   | 20  | Biology        |
-- +----+--------+-----+----------------+


-- =====================================================
-- Key Points:
-- 1. INSERT: Adds new rows
-- 2. UPDATE: Modifies existing rows
-- 3. DELETE: Removes rows
-- 4. Always use WHERE with UPDATE and DELETE!
-- 5. Test queries before executing
-- =====================================================
