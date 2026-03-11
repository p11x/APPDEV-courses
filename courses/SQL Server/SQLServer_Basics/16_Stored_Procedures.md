# SQL Server Stored Procedures

## What is a Stored Procedure?

A **Stored Procedure** is a precompiled set of SQL statements stored in the database. It can accept parameters, perform operations, and return results.

## Why Use Stored Procedures?

- **Performance**: Precompiled and cached for faster execution
- **Security**: Reduce SQL injection risks
- **Reusability**: Write once, use many times
- **Maintainability**: Centralize business logic
- **Reduced Network Traffic**: Execute complex operations on server

## Basic Stored Procedure Syntax

```sql
CREATE PROCEDURE ProcedureName
AS
BEGIN
    -- SQL statements
END;
```

## Simple Stored Procedure

```sql
-- Create a simple stored procedure
CREATE PROCEDURE sp_GetAllStudents
AS
BEGIN
    SELECT * FROM Students;
END;
```

## Executing a Stored Procedure

```sql
-- Execute the procedure
EXEC sp_GetAllStudents;

-- Or simply
sp_GetAllStudents;
```

## Stored Procedure with Parameters

```sql
-- Procedure to get student by ID
CREATE PROCEDURE sp_GetStudentByID
    @StudentID INT
AS
BEGIN
    SELECT * FROM Students 
    WHERE StudentID = @StudentID;
END;
```

**Execute with parameter:**
```sql
EXEC sp_GetStudentByID @StudentID = 1;
```

## Stored Procedure with Multiple Parameters

```sql
CREATE PROCEDURE sp_GetStudentsByDepartment
    @Department VARCHAR(50),
    @MinAge INT = 0  -- Default value
AS
BEGIN
    SELECT Name, Age, Department
    FROM Students
    WHERE Department = @Department 
      AND Age >= @MinAge;
END;
```

**Execute:**
```sql
-- Using default age
EXEC sp_GetStudentsByDepartment @Department = 'Computer Science';

-- With custom age
EXEC sp_GetStudentsByDepartment @Department = 'Mathematics', @MinAge = 20;
```

## INSERT with Stored Procedure

```sql
CREATE PROCEDURE sp_InsertStudent
    @StudentID INT,
    @Name VARCHAR(50),
    @Age INT,
    @Department VARCHAR(50)
AS
BEGIN
    INSERT INTO Students (StudentID, Name, Age, Department)
    VALUES (@StudentID, @Name, @Age, @Department);
END;
```

**Execute:**
```sql
EXEC sp_InsertStudent 
    @StudentID = 6, 
    @Name = 'Emily', 
    @Age = 21, 
    @Department = 'Physics';
```

## UPDATE with Stored Procedure

```sql
CREATE PROCEDURE sp_UpdateStudentAge
    @StudentID INT,
    @NewAge INT
AS
BEGIN
    UPDATE Students
    SET Age = @NewAge
    WHERE StudentID = @StudentID;
END;
```

## DELETE with Stored Procedure

```sql
CREATE PROCEDURE sp_DeleteStudent
    @StudentID INT
AS
BEGIN
    DELETE FROM Students
    WHERE StudentID = @StudentID;
END;
```

## Stored Procedure with OUTPUT Parameter

```sql
CREATE PROCEDURE sp_GetStudentCount
    @Dept VARCHAR(50),
    @Count INT OUTPUT  -- Output parameter
AS
BEGIN
    SELECT @Count = COUNT(*)
    FROM Students
    WHERE Department = @Dept;
END;
```

**Execute with output:**
```sql
DECLARE @Total INT;
EXEC sp_GetStudentCount @Dept = 'Computer Science', @Count = @Total OUTPUT;
PRINT @Total;
```

## Conditional Logic (IF-ELSE)

```sql
CREATE PROCEDURE sp_StudentSummary
    @StudentID INT
AS
BEGIN
    DECLARE @Age INT;
    
    SELECT @Age = Age FROM Students WHERE StudentID = @StudentID;
    
    IF @Age IS NULL
    BEGIN
        PRINT 'Student not found';
    END
    ELSE IF @Age < 18
    BEGIN
        PRINT 'Student is a minor';
    END
    ELSE
    BEGIN
        PRINT 'Student is an adult';
    END
END;
```

## Modifying a Stored Procedure (ALTER)

```sql
ALTER PROCEDURE sp_GetStudentByID
    @StudentID INT
AS
BEGIN
    SELECT StudentID, Name, Age, Department
    FROM Students 
    WHERE StudentID = @StudentID;
END;
```

## Deleting a Stored Procedure

```sql
DROP PROCEDURE sp_GetAllStudents;
```

## System Stored Procedures

SQL Server includes built-in procedures:

```sql
-- List all stored procedures
EXEC sp_help;

-- Get procedure code
EXEC sp_helptext 'sp_GetAllStudents';

-- List tables
EXEC sp_tables;
```

## Key Points Summary

| Feature | Description |
|---------|-------------|
| Precompiled | Stored and optimized |
| Parameters | Input and output |
| Returns | Results or output values |
| Security | Reduce injection risk |
| Performance | Faster execution |

---

*This topic should take about 5-7 minutes to explain in class.*
