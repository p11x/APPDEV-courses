-- =====================================================
-- SQL Server: Triggers
-- =====================================================
-- Triggers are special stored procedures that run automatically
-- when certain events occur in the database

-- =====================================================
-- Types of Triggers
-- =====================================================

-- 1. DML Triggers: Run on INSERT, UPDATE, DELETE
-- 2. DDL Triggers: Run on CREATE, ALTER, DROP
-- 3. Logon Triggers: Run when user logs in


-- =====================================================
-- Creating a DML Trigger (AFTER INSERT)
-- =====================================================

-- Create an audit table to log changes
CREATE TABLE EmployeeAudit (
    AuditID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT,
    ActionType VARCHAR(10),
    ActionDate DATETIME DEFAULT GETDATE()
);

-- Create table for testing
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    Name VARCHAR(50),
    Salary DECIMAL(10, 2)
);

-- Trigger: Log when new employee is added
CREATE TRIGGER trg_AfterInsertEmployee
ON Employees
AFTER INSERT
AS
BEGIN
    INSERT INTO EmployeeAudit (EmployeeID, ActionType)
    SELECT EmployeeID, 'INSERT'
    FROM inserted;
    
    PRINT 'New employee added and logged';
END;

-- Test the trigger
INSERT INTO Employees VALUES (1, 'John Smith', 50000);
INSERT INTO Employees VALUES (2, 'Jane Doe', 60000);

-- Check audit table
SELECT * FROM EmployeeAudit;


-- =====================================================
-- Creating a DML Trigger (AFTER UPDATE)
-- =====================================================

-- Trigger: Log salary changes
CREATE TRIGGER trg_AfterUpdateSalary
ON Employees
AFTER UPDATE
AS
BEGIN
    -- Check if salary was changed
    IF UPDATE(Salary)
    BEGIN
        INSERT INTO EmployeeAudit (EmployeeID, ActionType)
        SELECT EmployeeID, 'UPDATE'
        FROM inserted;
        
        PRINT 'Salary update logged';
    END;
END;

-- Test the trigger
UPDATE Employees SET Salary = 55000 WHERE EmployeeID = 1;

-- Check audit table
SELECT * FROM EmployeeAudit;


-- =====================================================
-- Creating a DML Trigger (AFTER DELETE)
-- =====================================================

-- Trigger: Log when employee is deleted
CREATE TRIGGER trg_AfterDeleteEmployee
ON Employees
AFTER DELETE
AS
BEGIN
    INSERT INTO EmployeeAudit (EmployeeID, ActionType)
    SELECT EmployeeID, 'DELETE'
    FROM deleted;
    
    PRINT 'Employee deletion logged';
END;

-- Test the trigger
DELETE FROM Employees WHERE EmployeeID = 2;

-- Check audit table
SELECT * FROM EmployeeAudit;


-- =====================================================
-- Creating an INSTEAD OF Trigger
-- =====================================================

-- Instead of INSERT, UPDATE, or DELETE, run trigger code

-- Create a view
CREATE VIEW vw_EmployeeNames AS
SELECT EmployeeID, Name FROM Employees;

-- Trigger: Instead of insert, redirect to audit
CREATE TRIGGER trg_InsteadOfInsert
ON vw_EmployeeNames
INSTEAD OF INSERT
AS
BEGIN
    PRINT 'Cannot insert directly through view';
    PRINT 'Use the Employees table instead';
END;

-- Try to insert through view (will show message)
INSERT INTO vw_EmployeeNames VALUES (3, 'Mike Johnson');


-- =====================================================
-- DDL Triggers
-- =====================================================

-- Trigger: Prevent table deletion
CREATE TRIGGER trg_PreventTableDrop
ON DATABASE
FOR DROP_TABLE
AS
BEGIN
    ROLLBACK;
    PRINT 'Cannot drop tables. This action is blocked.';
END;

-- Test (uncomment to try)
-- DROP TABLE Employees;  -- This will be blocked!


-- =====================================================
-- Using UPDATE() Function
-- =====================================================

-- Check which column was updated
CREATE TRIGGER trg_LogColumnChanges
ON Employees
AFTER UPDATE
AS
BEGIN
    IF UPDATE(Name)
        PRINT 'Name column was updated';
    
    IF UPDATE(Salary)
        PRINT 'Salary column was updated';
END;

-- Test
UPDATE Employees SET Name = 'John Doe' WHERE EmployeeID = 1;
UPDATE Employees SET Salary = 70000 WHERE EmployeeID = 1;


-- =====================================================
-- Managing Triggers
-- =====================================================

-- Disable a trigger
DISABLE TRIGGER trg_AfterInsertEmployee ON Employees;

-- Enable a trigger
ENABLE TRIGGER trg_AfterInsertEmployee ON Employees;

-- View trigger information
EXEC sp_helptext 'trg_AfterInsertEmployee';

-- Delete a trigger
DROP TRIGGER trg_AfterInsertEmployee;


-- =====================================================
-- Practical Example: Auto-update timestamp
-- =====================================================

-- Add LastModified column
ALTER TABLE Employees ADD LastModified DATETIME;

-- Create trigger to auto-update timestamp
CREATE TRIGGER trg_AutoUpdateTimestamp
ON Employees
AFTER UPDATE
AS
BEGIN
    UPDATE Employees
    SET LastModified = GETDATE()
    FROM Employees e
    INNER JOIN inserted i ON e.EmployeeID = i.EmployeeID;
END;

-- Test
UPDATE Employees SET Salary = 75000 WHERE EmployeeID = 1;

-- Check result
SELECT * FROM Employees;


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. AFTER Trigger: Runs after DML operation completes
2. INSTEAD OF Trigger: Runs instead of DML operation
3. inserted: Contains new row values (for INSERT/UPDATE)
4. deleted: Contains old row values (for DELETE/UPDATE)
5. DDL Triggers: Protect database structure
6. Can be disabled/enabled as needed
*/


-- =====================================================
-- Clean up (optional)
-- =====================================================
-- DROP TRIGGER trg_AfterInsertEmployee;
-- DROP TRIGGER trg_AfterUpdateSalary;
-- DROP TRIGGER trg_AfterDeleteEmployee;
-- DROP TRIGGER trg_PreventTableDrop;
-- DROP TABLE EmployeeAudit;
