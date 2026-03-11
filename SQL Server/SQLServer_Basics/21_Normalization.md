# SQL Server Database Normalization

## What is Normalization?

**Normalization** is the process of organizing data in a database to reduce redundancy and improve data integrity. It involves dividing large tables into smaller, related tables.

## Why Use Normalization?

- **Eliminate Redundant Data**: Don't repeat the same information
- **Improve Data Integrity**: Ensure data is accurate
- **Better Maintenance**: Easier to update data
- **Scalability**: Handle larger databases efficiently

## Normal Forms (NF)

| Form | Description |
|------|-------------|
| 1NF | Atomic values, no repeating groups |
| 2NF | No partial dependencies |
| 3NF | No transitive dependencies |
| BCNF | Boyce-Codd Normal Form |
| 4NF | No multi-valued dependencies |

## First Normal Form (1NF)

### Rules:
- Each column contains atomic (indivisible) values
- No repeating groups or arrays
- Each row is unique
- Each column has a unique name

### Example: Before 1NF (Bad)

```
Students Table:
+--------------------------------------------------+
| StudentID | Name      | Courses                  |
+--------------------------------------------------+
| 1         | John      | Math, Science, History  |
| 2         | Jane      | Math, Art               |
+--------------------------------------------------+
```

### After 1NF (Good)

```
Students Table:
+--------+-------+
| ID     | Name  |
+--------+-------+
| 1      | John  |
| 2      | Jane  |
+--------+-------+

Courses Table:
+-----------+-----------+
| CourseID  | Course    |
+-----------+-----------+
| 101       | Math      |
| 102       | Science   |
| 103       | History   |
| 104       | Art       |
+-----------+-----------+

StudentCourses Table:
+------------+----------+
| StudentID | CourseID |
+------------+----------+
| 1         | 101      |
| 1         | 102      |
| 1         | 103      |
| 2         | 101      |
| 2         | 104      |
+------------+----------+
```

## Second Normal Form (2NF)

### Rules:
- Must be in 1NF
- No partial dependencies (non-key columns depend on entire primary key)

### Example: Before 2NF (Bad)

```
OrderItems Table:
+-----------------+------------+--------+
| (OrderID, ProdID)| ProdName  | Quantity|
+-----------------+------------+--------+
| 1, 101          | Laptop    | 2        |
| 1, 102          | Mouse     | 5        |
| 2, 101          | Laptop    | 1        |
+-----------------+------------+--------+

Problem: ProdName depends only on ProdID, not full key
```

### After 2NF (Good)

```
Products Table:
+----------+------------+
| ProdID   | ProdName   |
+----------+------------+
| 101      | Laptop     |
| 102      | Mouse      |
+----------+------------+

OrderItems Table:
+-----------------+----------+----------+
| OrderID         | ProdID   | Quantity |
+-----------------+----------+----------+
| 1               | 101      | 2        |
| 1               | 102      | 5        |
| 2               | 101      | 1        |
+-----------------+----------+----------+
```

## Third Normal Form (3NF)

### Rules:
- Must be in 2NF
- No transitive dependencies (non-key columns shouldn't depend on other non-key columns)

### Example: Before 3NF (Bad)

```
Students Table:
+-----------+----------+---------+-----------+
| StudentID | Name     | ZipCode | City      |
+-----------+----------+---------+-----------+
| 1         | John     | 10001   | New York  |
| 2         | Jane     | 10002   | Boston    |
+-----------+----------+---------+-----------+

Problem: City depends on ZipCode (transitive dependency)
```

### After 3NF (Good)

```
Students Table:
+-----------+----------+---------+
| StudentID | Name     | ZipCode |
+-----------+----------+---------+
| 1         | John     | 10001   |
| 2         | Jane     | 10002   |
+-----------+----------+---------+

ZipCodes Table:
+----------+-----------+
| ZipCode  | City     |
+----------+-----------+
| 10001    | New York |
| 10002    | Boston   |
+----------+-----------+
```

## Example: Complete Normalization

### Original Unnormalized Data

```
EmployeeProjects Table:
+------+-------+--------+----------+--------+
| EmpID| Name  | Dept   | Project  | Hours  |
+------+-------+--------+----------+--------+
| 001  | John  | IT     | Website  | 40     |
| 001  | John  | IT     | Mobile   | 20     |
| 002  | Jane  | HR     | Training | 30     |
+------+-------+--------+----------+--------+
```

### After Normalization

```
Employees Table:
+--------+-------+--------+
| EmpID  | Name  | DeptID |
+--------+-------+--------+
| 001    | John  | 1      |
| 002    | Jane  | 2      |
+--------+-------+--------+

Departments Table:
+--------+----------+
| DeptID | DeptName |
+--------+----------+
| 1      | IT       |
| 2      | HR       |
+--------+----------+

Projects Table:
+----------+-----------+
| ProjID  | Project   |
+----------+-----------+
| 1       | Website   |
| 2       | Mobile   |
| 3       | Training |
+----------+-----------+

Assignments Table:
+----------+--------+--------+
| EmpID    | ProjID | Hours  |
+----------+--------+--------+
| 001      | 1      | 40     |
| 001      | 2      | 20     |
| 002      | 3      | 30     |
+----------+--------+--------+
```

## Denormalization

Sometimes you intentionally add redundancy for performance:

```sql
-- Denormalized: Include department name in employee view
CREATE VIEW vw_EmployeesWithDept AS
SELECT 
    e.EmpID,
    e.Name,
    d.DeptName
FROM Employees e
INNER JOIN Departments d ON e.DeptID = d.DeptID;
```

## Key Points Summary

| Normal Form | Goal |
|-------------|------|
| 1NF | Atomic values, unique rows |
| 2NF | Full dependency on primary key |
| 3NF | No transitive dependencies |
| Higher NF | Less redundancy, more tables |

---

*This topic should take about 5-7 minutes to explain in class.*
