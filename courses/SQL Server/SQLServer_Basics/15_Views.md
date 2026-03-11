# SQL Server Views

## What is a View?

A **View** is a virtual table based on the result of a stored query. It doesn't store data physically - it dynamically retrieves data from underlying tables when accessed.

## Why Use Views?

- **Simplify complex queries**: Hide complexity from users
- **Security**: Restrict access to specific columns/rows
- **Reusability**: Save frequently used queries
- **Abstraction**: Separate schema from users

## Types of Views

### 1. Simple View
- Based on a single table
- Can perform DML operations (INSERT, UPDATE, DELETE)

### 2. Complex View
- Based on multiple tables
- Contains functions or grouping
- DML operations may be limited

### 3. Indexed View
- Has a unique clustered index
- Materialized for performance

## Creating a Simple View

```sql
-- Create a view to show active employees
CREATE VIEW vw_ActiveEmployees AS
SELECT 
    EmployeeID,
    FirstName,
    LastName,
    Department,
    HireDate
FROM Employees
WHERE IsActive = 1;
```

## Using a View

```sql
-- Query the view like a table
SELECT * FROM vw_ActiveEmployees;

-- Select specific columns
SELECT FirstName, LastName FROM vw_ActiveEmployees;
```

## Creating a Complex View

```sql
-- View with JOIN and aggregation
CREATE VIEW vw_EmployeeSummary AS
SELECT 
    e.Department,
    COUNT(e.EmployeeID) AS TotalEmployees,
    AVG(e.Salary) AS AvgSalary,
    MAX(e.Salary) AS MaxSalary,
    MIN(e.Salary) AS MinSalary
FROM Employees e
GROUP BY e.Department;
```

## Example: Student Information View

```sql
-- Create base tables
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    Name VARCHAR(50),
    Email VARCHAR(100)
);

CREATE TABLE Enrollments (
    EnrollmentID INT PRIMARY KEY,
    StudentID INT,
    CourseID INT,
    Grade VARCHAR(2)
);

CREATE TABLE Courses (
    CourseID INT PRIMARY KEY,
    CourseName VARCHAR(100),
    Credits INT
);

-- Create a view combining all tables
CREATE VIEW vw_StudentGrades AS
SELECT 
    s.StudentID,
    s.Name,
    s.Email,
    c.CourseName,
    e.Grade
FROM Students s
INNER JOIN Enrollments e ON s.StudentID = e.StudentID
INNER JOIN Courses c ON e.CourseID = c.CourseID;

-- Query the view
SELECT * FROM vw_StudentGrades;
```

**Expected Output:**
```
+-----------+-------------+-------------------+------------------+-------+
| StudentID | Name        | Email             | CourseName       | Grade |
+-----------+-------------+-------------------+------------------+-------+
| 1         | John Smith  | john@email.com    | Database Systems | A     |
| 1         | John Smith  | john@email.com    | Web Development  | B     |
| 2         | Jane Doe    | jane@email.com    | Database Systems | A     |
+-----------+-------------+-------------------+------------------+-------+
```

## Updating Through a View

You can update data through simple views:

```sql
-- Create a simple view
CREATE VIEW vw_SimpleStudents AS
SELECT StudentID, Name, Age
FROM Students;

-- Update through view
UPDATE vw_SimpleStudents 
SET Age = 21 
WHERE StudentID = 1;
```

## Modifying a View (ALTER)

```sql
ALTER VIEW vw_ActiveEmployees AS
SELECT 
    EmployeeID,
    FirstName,
    LastName,
    Department,
    HireDate,
    Salary  -- Added Salary column
FROM Employees
WHERE IsActive = 1;
```

## Deleting a View

```sql
DROP VIEW vw_ActiveEmployees;
```

## With CHECK OPTION

Prevents updates that would make rows invisible:

```sql
CREATE VIEW vw_ActiveEmployees AS
SELECT EmployeeID, Name, Department
FROM Employees
WHERE IsActive = 1
WITH CHECK OPTION;

-- This will fail (age > 30 would be hidden from view)
-- UPDATE vw_ActiveEmployees SET Age = 35 WHERE EmployeeID = 1;
```

## Key Points Summary

| Feature | Description |
|---------|-------------|
| Virtual Table | Doesn't store data physically |
| Reusable | Save query for repeated use |
| Security | Limit data exposure |
| Simple/Complex | Single or multiple tables |
| DML Operations | Limited on complex views |

---

*This topic should take about 5 minutes to explain in class.*
