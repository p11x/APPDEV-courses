-- =====================================================
-- SQL Server: Advanced Stored Procedures
-- =====================================================
-- This tutorial covers advanced stored procedure techniques

-- =====================================================
-- Recursive Stored Procedure (Factorial Example)
-- =====================================================

CREATE PROCEDURE sp_Factorial
    @Number INT,
    @Result INT OUTPUT
AS
BEGIN
    -- Base case: 0! = 1
    IF @Number <= 1
        SET @Result = 1;
    ELSE
    BEGIN
        DECLARE @PrevResult INT;
        -- Recursive call
        EXEC sp_Factorial @Number - 1, @PrevResult OUTPUT;
        SET @Result = @Number * @PrevResult;
    END;
END;
GO

-- Test the recursive procedure
DECLARE @FactResult INT;
EXEC sp_Factorial @Number = 5, @Result = @FactResult OUTPUT;
PRINT '5! = ' + CAST(@FactResult AS VARCHAR(10));
GO


-- =====================================================
-- Dynamic SQL in Stored Procedures
-- =====================================================

CREATE PROCEDURE sp_DynamicQuery
    @TableName VARCHAR(100),
    @ColumnName VARCHAR(100),
    @Value SQL_VARIANT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    
    -- Build dynamic SQL query
    SET @SQL = N'SELECT * FROM ' + QUOTENAME(@TableName) + 
               N' WHERE ' + QUOTENAME(@ColumnName) + N' = @Value';
    
    -- Execute with parameter
    EXEC sp_executesql @SQL, 
                       N'@Value SQL_VARIANT', 
                       @Value = @Value;
END;
GO

-- Execute dynamic procedure
EXEC sp_DynamicQuery @TableName = 'Students', @ColumnName = 'Age', @Value = 20;
GO


-- =====================================================
-- Table-Valued Parameters
-- =====================================================

-- Create table type
CREATE TYPE StudentList AS TABLE (
    StudentID INT,
    Name VARCHAR(50),
    Age INT
);
GO

-- Create procedure using table-valued parameter
CREATE PROCEDURE sp_InsertStudents
    @Students StudentList READONLY
AS
BEGIN
    INSERT INTO Students (StudentID, Name, Age)
    SELECT StudentID, Name, Age
    FROM @Students;
    
    PRINT 'Inserted ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' students';
END;
GO

-- Declare and use table variable
DECLARE @NewStudents StudentList;

INSERT INTO @NewStudents VALUES 
    (101, 'Alice', 20),
    (102, 'Bob', 21),
    (103, 'Charlie', 19);

EXEC sp_InsertStudents @Students = @NewStudents;
GO


-- =====================================================
-- Cursor in Stored Procedures
-- =====================================================

CREATE PROCEDURE sp_ProcessEachStudent
AS
BEGIN
    DECLARE @StudentID INT;
    DECLARE @Name VARCHAR(50);
    
    -- Declare cursor
    DECLARE student_cursor CURSOR FOR
        SELECT StudentID, Name FROM Students;
    
    OPEN student_cursor;
    
    FETCH NEXT FROM student_cursor INTO @StudentID, @Name;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Processing: ' + CAST(@StudentID AS VARCHAR(10)) + ' - ' + @Name;
        
        -- Do processing here
        FETCH NEXT FROM student_cursor INTO @StudentID, @Name;
    END;
    
    CLOSE student_cursor;
    DEALLOCATE student_cursor;
END;
GO

EXEC sp_ProcessEachStudent;
GO


-- =====================================================
-- Handling Multiple Result Sets
-- =====================================================

CREATE PROCEDURE sp_GetEmployeeDetails
    @EmployeeID INT
AS
BEGIN
    -- First result set: Employee info
    SELECT EmployeeID, Name, Department
    FROM Employees
    WHERE EmployeeID = @EmployeeID;
    
    -- Second result set: Orders
    SELECT OrderID, OrderDate, TotalAmount
    FROM Orders
    WHERE EmployeeID = @EmployeeID;
END;
GO


-- =====================================================
-- Optional Parameters with Defaults
-- =====================================================

CREATE PROCEDURE sp_GetFilteredEmployees
    @Department VARCHAR(50) = NULL,
    @MinSalary DECIMAL(10,2) = NULL,
    @MaxSalary DECIMAL(10,2) = NULL,
    @OrderBy VARCHAR(20) = 'Name'
AS
BEGIN
    SELECT Name, Department, Salary
    FROM Employees
    WHERE (@Department IS NULL OR Department = @Department)
      AND (@MinSalary IS NULL OR Salary >= @MinSalary)
      AND (@MaxSalary IS NULL OR Salary <= @MaxSalary)
    ORDER BY 
        CASE WHEN @OrderBy = 'Name' THEN Name
             WHEN @OrderBy = 'Salary' THEN CAST(Salary AS VARCHAR)
             ELSE Name END;
END;
GO

-- Various ways to call
EXEC sp_GetFilteredEmployees;  -- All employees
EXEC sp_GetFilteredEmployees @Department = 'IT';  -- IT department
EXEC sp_GetFilteredEmployees @MinSalary = 50000, @MaxSalary = 100000;
EXEC sp_GetFilteredEmployees @Department = 'HR', @OrderBy = 'Salary';


-- =====================================================
-- Using WITH RESULT SETS
-- =====================================================

CREATE PROCEDURE sp_GetFormattedEmployees
AS
BEGIN
    SELECT 
        EmployeeID AS ID,
        Name AS EmployeeName,
        Department AS Dept
    FROM Employees
    WHERE IsActive = 1;
END;
GO

-- Execute with modified column names
EXEC sp_GetFormattedEmployees
WITH RESULT SETS (
    (
        ID INT,
        EmployeeName VARCHAR(100),
        DepartmentName VARCHAR(50)
    )
);
GO


-- =====================================================
-- Stored Procedure with Output Cursor
-- =====================================================

CREATE PROCEDURE sp_GetEmployeeCursor
    @Department VARCHAR(50),
    @EmployeeCursor CURSOR VARYING OUTPUT
AS
BEGIN
    SET @EmployeeCursor = CURSOR FOR
        SELECT EmployeeID, Name, Salary
        FROM Employees
        WHERE Department = @Department;
    
    OPEN @EmployeeCursor;
END;
GO

-- Using the output cursor
DECLARE @Cursor CURSOR;
DECLARE @EmpID INT, @Name VARCHAR(50), @Salary DECIMAL(10,2);

EXEC sp_GetEmployeeCursor @Department = 'IT', @EmployeeCursor = @Cursor OUTPUT;

FETCH NEXT FROM @Cursor INTO @EmpID, @Name, @Salary;

WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT @Name + ' - $' + CAST(@Salary AS VARCHAR(20));
    FETCH NEXT FROM @Cursor INTO @EmpID, @Name, @Salary;
END;

CLOSE @Cursor;
DEALLOCATE @Cursor;


-- =====================================================
-- Error Handling in Procedures
-- =====================================================

CREATE PROCEDURE sp_SafeDivision
    @Dividend DECIMAL(10,2),
    @Divisor DECIMAL(10,2),
    @Result DECIMAL(10,2) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF @Divisor = 0
            THROW 50001, 'Division by zero is not allowed!', 1;
        
        SET @Result = @Dividend / @Divisor;
        
        RETURN 0;  -- Success
        
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();  -- Return error code
    END CATCH
END;
GO

DECLARE @Answer DECIMAL(10,2);
DECLARE @ReturnCode INT;

EXEC @ReturnCode = sp_SafeDivision @Dividend = 100, @Divisor = 4, @Result = @Answer OUTPUT;

IF @ReturnCode = 0
    PRINT 'Result: ' + CAST(@Answer AS VARCHAR(20));
ELSE
    PRINT 'Error occurred!';


-- =====================================================
-- Conditional Execution with CASE
-- =====================================================

CREATE PROCEDURE sp_CalculateBonus
    @EmployeeID INT,
    @BonusType VARCHAR(20)  -- 'Performance', 'Holiday', 'Referral'
AS
BEGIN
    DECLARE @Salary DECIMAL(10,2);
    DECLARE @Bonus DECIMAL(10,2);
    
    SELECT @Salary = Salary FROM Employees WHERE EmployeeID = @EmployeeID;
    
    SET @Bonus = CASE @BonusType
        WHEN 'Performance' THEN @Salary * 0.15
        WHEN 'Holiday' THEN @Salary * 0.10
        WHEN 'Referral' THEN 500.00
        ELSE 0
    END;
    
    PRINT 'Bonus: $' + CAST(@Bonus AS VARCHAR(20));
END;
GO

EXEC sp_CalculateBonus @EmployeeID = 1, @BonusType = 'Performance';
EXEC sp_CalculateBonus @EmployeeID = 1, @BonusType = 'Holiday';


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. Recursive procedures call themselves for repeated operations
2. Dynamic SQL builds queries at runtime using sp_executesql
3. Table-valued parameters pass multiple rows to procedures
4. Cursors iterate through result sets row by row
5. Optional parameters with defaults provide flexibility
6. WITH RESULT SETS modifies returned column definitions
7. Output cursors return result sets to calling procedures
8. TRY/CATCH handles errors gracefully
*/
