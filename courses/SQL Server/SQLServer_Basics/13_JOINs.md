# SQL Server JOINs

## What is a JOIN?

A **JOIN** is used to combine rows from two or more tables based on a related column. It allows you to retrieve data that spans across multiple tables.

## Why Use JOINs?

In relational databases, data is often split across multiple tables for efficiency. JOINs help you reconnect this data when needed.

## Types of JOINs

### 1. INNER JOIN
Returns only the rows that have matching values in both tables.

### 2. LEFT JOIN (LEFT OUTER JOIN)
Returns all rows from the left table and matching rows from the right table.

### 3. RIGHT JOIN (RIGHT OUTER JOIN)
Returns all rows from the right table and matching rows from the left table.

### 4. FULL OUTER JOIN
Returns all rows when there's a match in either table.

### 5. CROSS JOIN
Returns all possible combinations of rows from both tables.

## Setting Up Sample Tables

```sql
-- Create Students table
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    Name VARCHAR(50),
    DepartmentID INT
);

-- Create Departments table
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(50)
);

-- Insert sample data
INSERT INTO Students VALUES 
    (1, 'John', 101),
    (2, 'Jane', 102),
    (3, 'Mike', 101),
    (4, 'Sarah', NULL),
    (5, 'David', 103);

INSERT INTO Departments VALUES 
    (101, 'Computer Science'),
    (102, 'Mathematics'),
    (103, 'Physics'),
    (104, 'Chemistry');
```

## Visual Representation

```
Students Table              Departments Table
+--------+--------+        +------+-----------------+
| StudentID | Name |        | DeptID | DeptName     |
+--------+--------+        +--------+--------------+
| 1         | John |        | 101    | Comp Sci     |
| 2         | Jane |        | 102    | Mathematics  |
| 3         | Mike |        | 103    | Physics      |
| 4         | Sarah|        | 104    | Chemistry    |
| 5         | David|        +--------+--------------+
+--------+--------+
```

## INNER JOIN Example

Returns students with valid department assignments:

```sql
SELECT 
    s.StudentID,
    s.Name,
    d.DepartmentName
FROM Students s
INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID;
```

**Expected Output:**
```
+-----------+--------------+-------------------+
| StudentID | Name         | DepartmentName    |
+-----------+--------------+-------------------+
| 1         | John         | Computer Science |
| 2         | Jane         | Mathematics       |
| 3         | Mike         | Computer Science |
| 5         | David        | Physics          |
+-----------+--------------+-------------------+
```

## LEFT JOIN Example

Returns all students, even without department:

```sql
SELECT 
    s.StudentID,
    s.Name,
    d.DepartmentName
FROM Students s
LEFT JOIN Departments d ON s.DepartmentID = d.DepartmentID;
```

**Expected Output:**
```
+-----------+------------+-------------------+
| StudentID | Name       | DepartmentName    |
+-----------+------------+-------------------+
| 1         | John       | Computer Science  |
| 2         | Jane       | Mathematics       |
| 3         | Mike       | Computer Science  |
| 4         | Sarah      | NULL              |
| 5         | David      | Physics           |
+-----------+------------+-------------------+
```

## RIGHT JOIN Example

Returns all departments, even without students:

```sql
SELECT 
    s.StudentID,
    s.Name,
    d.DepartmentName
FROM Students s
RIGHT JOIN Departments d ON s.DepartmentID = d.DepartmentID;
```

**Expected Output:**
```
+-----------+--------------+-------------------+
| StudentID | Name         | DepartmentName    |
+-----------+--------------+-------------------+
| 1         | John         | Computer Science |
| 2         | Jane         | Mathematics      |
| 3         | Mike         | Computer Science |
| 5         | David        | Physics          |
| NULL      | NULL         | Chemistry        |
+-----------+--------------+-------------------+
```

## FULL OUTER JOIN Example

Returns all students and all departments:

```sql
SELECT 
    s.StudentID,
    s.Name,
    d.DepartmentName
FROM Students s
FULL OUTER JOIN Departments d ON s.DepartmentID = d.DepartmentID;
```

**Expected Output:**
```
+-----------+------------+-------------------+
| StudentID | Name       | DepartmentName    |
+-----------+------------+-------------------+
| 1         | John       | Computer Science |
| 2         | Jane       | Mathematics      |
| 3         | Mike       | Computer Science |
| 4         | Sarah      | NULL             |
| 5         | David      | Physics          |
| NULL      | NULL       | Chemistry        |
+-----------+------------+-------------------+
```

## Multiple JOINs

You can join more than two tables:

```sql
SELECT 
    s.Name,
    d.DepartmentName,
    e.EmployeeName
FROM Students s
INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
LEFT JOIN EmployeeMentors e ON s.StudentID = e.StudentID;
```

## Key Points Summary

| JOIN Type | Returns |
|-----------|---------|
| INNER JOIN | Only matching rows |
| LEFT JOIN | All left + matching right |
| RIGHT JOIN | All right + matching left |
| FULL OUTER JOIN | All rows from both |
| CROSS JOIN | All combinations |

---

*This topic should take about 5-7 minutes to explain in class.*
