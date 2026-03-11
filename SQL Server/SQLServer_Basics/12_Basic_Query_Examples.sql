-- =====================================================
-- SQL Server: Basic Query Examples for Beginners
-- =====================================================

-- Create sample tables and data for practice
CREATE DATABASE PracticeDB;
GO

USE PracticeDB;
GO

-- Create Students table
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    Name VARCHAR(50),
    Age INT,
    Department VARCHAR(50),
    Score DECIMAL(5, 2)
);

-- Insert sample data
INSERT INTO Students VALUES 
    (1, 'John Smith', 20, 'Computer Science', 85.50),
    (2, 'Jane Doe', 21, 'Mathematics', 92.00),
    (3, 'Mike Johnson', 19, 'Physics', 78.75),
    (4, 'Sarah Williams', 20, 'Computer Science', 88.00),
    (5, 'David Brown', 22, 'Chemistry', 65.50),
    (6, 'Emily Davis', 19, 'Physics', 91.25),
    (7, 'Chris Wilson', 21, 'Mathematics', 72.00),
    (8, 'Amanda Miller', 20, 'Chemistry', 80.00);

-- =====================================================
-- Example 1: SELECT All Columns
-- =====================================================
-- Get all data from the Students table

SELECT * 
FROM Students;

-- Expected Output:
-- +-----------+---------------+-----+-------------------+--------+
-- | StudentID | Name          | Age | Department        | Score  |
-- +-----------+---------------+-----+-------------------+--------+
-- | 1         | John Smith    | 20  | Computer Science  | 85.50  |
-- | 2         | Jane Doe      | 21  | Mathematics       | 92.00  |
-- | 3         | Mike Johnson  | 19  | Physics           | 78.75  |
-- | 4         | Sarah Williams| 20  | Computer Science  | 88.00  |
-- | 5         | David Brown   | 22  | Chemistry         | 65.50  |
-- | 6         | Emily Davis   | 19  | Physics           | 91.25  |
-- | 7         | Chris Wilson  | 21  | Mathematics       | 72.00  |
-- | 8         | Amanda Miller | 20  | Chemistry         | 80.00  |
-- +-----------+---------------+-----+-------------------+--------+


-- =====================================================
-- Example 2: SELECT Specific Columns
-- =====================================================
-- Get only Name and Department columns

SELECT Name, Department 
FROM Students;

-- Expected Output:
-- +---------------+-------------------+
-- | Name          | Department        |
-- +---------------+-------------------+
-- | John Smith    | Computer Science  |
-- | Jane Doe      | Mathematics       |
-- | Mike Johnson  | Physics           |
-- | Sarah Williams| Computer Science  |
-- | David Brown   | Chemistry         |
-- | Emily Davis   | Physics           |
-- | Chris Wilson  | Mathematics       |
-- | Amanda Miller | Chemistry         |
-- +---------------+-------------------+


-- =====================================================
-- Example 3: WHERE Clause - Filter by Condition
-- =====================================================
-- Get students older than 20

SELECT Name, Age 
FROM Students 
WHERE Age > 20;

-- Expected Output:
-- +---------------+-----+
-- | Name          | Age |
-- +---------------+-----+
-- | Jane Doe      | 21  |
-- | David Brown   | 22  |
-- | Chris Wilson  | 21  |
-- +---------------+-----+


-- =====================================================
-- Example 4: WHERE with String Condition
-- =====================================================
-- Get students in Computer Science department

SELECT Name, Department 
FROM Students 
WHERE Department = 'Computer Science';

-- Expected Output:
-- +---------------+-------------------+
-- | Name          | Department        |
-- +---------------+-------------------+
-- | John Smith    | Computer Science  |
-- | Sarah Williams| Computer Science  |
-- +---------------+-------------------+


-- =====================================================
-- Example 5: Multiple Conditions with AND
-- =====================================================
-- Get students in Physics department AND age is 19

SELECT Name, Age, Department 
FROM Students 
WHERE Department = 'Physics' AND Age = 19;

-- Expected Output:
-- +---------------+-----+------------+
-- | Name          | Age | Department |
-- +---------------+-----+------------+
-- | Mike Johnson  | 19  | Physics    |
-- | Emily Davis   | 19  | Physics    |
-- +---------------+-----+------------+


-- =====================================================
-- Example 6: Multiple Conditions with OR
-- =====================================================
-- Get students in Mathematics OR Chemistry

SELECT Name, Department 
FROM Students 
WHERE Department = 'Mathematics' OR Department = 'Chemistry';

-- Expected Output:
-- +---------------+------------+
-- | Name          | Department |
-- +---------------+------------+
-- | Jane Doe      | Mathematics|
-- | David Brown   | Chemistry  |
-- | Chris Wilson  | Mathematics|
-- | Amanda Miller | Chemistry  |
-- +---------------+------------+


-- =====================================================
-- Example 7: ORDER BY - Sort Results
-- =====================================================
-- Get all students ordered by Score (highest to lowest)

SELECT Name, Score 
FROM Students 
ORDER BY Score DESC;

-- Expected Output:
-- +---------------+--------+
-- | Name          | Score  |
-- +---------------+--------+
-- | Jane Doe      | 92.00  |
-- | Emily Davis   | 91.25  |
-- | Sarah Williams| 88.00  |
-- | John Smith    | 85.50  |
-- | Amanda Miller | 80.00  |
-- | Mike Johnson  | 78.75  |
-- | Chris Wilson  | 72.00  |
-- | David Brown   | 65.50  |
-- +---------------+--------+


-- =====================================================
-- Example 8: ORDER BY Ascending (Default)
-- =====================================================
-- Get students ordered by Name alphabetically

SELECT Name 
FROM Students 
ORDER BY Name ASC;

-- Expected Output:
-- +---------------+
-- | Name          |
-- +---------------+
-- | Amanda Miller |
-- | Chris Wilson  |
-- | David Brown   |
-- | Emily Davis   |
-- | Jane Doe      |
-- | John Smith    |
-- | Mike Johnson  |
-- | Sarah Williams|
-- +---------------+


-- =====================================================
-- Example 9: COUNT(*) - Count Rows
-- =====================================================
-- Count total number of students

SELECT COUNT(*) AS TotalStudents 
FROM Students;

-- Expected Output:
-- +---------------+
-- | TotalStudents |
-- +---------------+
-- | 8             |
-- +---------------+


-- =====================================================
-- Example 10: COUNT with WHERE
-- =====================================================
-- Count students in Computer Science

SELECT COUNT(*) AS ComputerScienceStudents 
FROM Students 
WHERE Department = 'Computer Science';

-- Expected Output:
-- +---------------------------+
-- | ComputerScienceStudents  |
-- +---------------------------+
-- | 2                         |
-- +---------------------------+


-- =====================================================
-- Example 11: SUM - Add Values
-- =====================================================
-- Get total of all scores

SELECT SUM(Score) AS TotalScore 
FROM Students;

-- Expected Output:
-- +------------+
-- | TotalScore |
-- +------------+
-- | 653.00     |
-- +------------+


-- =====================================================
-- Example 12: AVG - Average Value
-- =====================================================
-- Get average score

SELECT AVG(Score) AS AverageScore 
FROM Students;

-- Expected Output:
-- +--------------+
-- | AverageScore |
-- +--------------+
-- | 81.625       |
-- +--------------+


-- =====================================================
-- Example 13: MAX and MIN - Highest and Lowest
-- =====================================================
-- Get highest and lowest scores

SELECT 
    MAX(Score) AS HighestScore,
    MIN(Score) AS LowestScore
FROM Students;

-- Expected Output:
-- +--------------+-------------+
-- | HighestScore | LowestScore |
-- +--------------+-------------+
-- | 92.00        | 65.50       |
-- +--------------+-------------+


-- =====================================================
-- Example 14: DISTINCT - Unique Values
-- =====================================================
-- Get list of unique departments

SELECT DISTINCT Department 
FROM Students;

-- Expected Output:
-- +-------------------+
-- | Department        |
-- +-------------------+
-- | Computer Science  |
-- | Mathematics       |
-- | Physics           |
-- | Chemistry         |
-- +-------------------+


-- =====================================================
-- Example 15: TOP - Limit Results
-- =====================================================
-- Get top 3 students by score

SELECT TOP 3 Name, Score 
FROM Students 
ORDER BY Score DESC;

-- Expected Output:
-- +---------------+--------+
-- | Name          | Score  |
-- +---------------+--------+
-- | Jane Doe      | 92.00  |
-- | Emily Davis   | 91.25  |
-- | Sarah Williams| 88.00  |
-- +---------------+--------+


-- =====================================================
-- Key Points Summary:
-- =====================================================
/*
SELECT         - Choose columns to display
FROM           - Specify table
WHERE          - Filter rows
ORDER BY       - Sort results (ASC/DESC)
COUNT(*)       - Count total rows
SUM()          - Add values
AVG()          - Calculate average
MAX() / MIN()  - Find highest/lowest
DISTINCT       - Get unique values
TOP            - Limit number of results
*/
