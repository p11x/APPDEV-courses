-- =====================================================
-- SQL Server: Error Handling
-- =====================================================
-- Error handling allows you to gracefully manage errors
-- that occur during SQL execution

-- =====================================================
-- TRY...CATCH Structure
-- =====================================================

-- Basic syntax:
BEGIN TRY
    -- SQL statements that might fail
END TRY
BEGIN CATCH
    -- Error handling code
END CATCH;


-- =====================================================
-- Basic Error Handling Example
-- =====================================================

BEGIN TRY
    -- Try to insert duplicate key
    CREATE TABLE TestTable (
        ID INT PRIMARY KEY
    );
    
    INSERT INTO TestTable VALUES (1);
    INSERT INTO TestTable VALUES (1);  -- This will fail!
    
END TRY
BEGIN CATCH
    PRINT 'An error occurred!';
    PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
    PRINT 'Error Message: ' + ERROR_MESSAGE();
    PRINT 'Error Line: ' + CAST(ERROR_LINE() AS VARCHAR(10));
END CATCH;


-- =====================================================
-- Error Functions
-- =====================================================

-- Available in CATCH block:
-- ERROR_NUMBER()    - Returns error number
-- ERROR_MESSAGE()  - Returns error message
-- ERROR_SEVERITY() - Returns severity level
-- ERROR_STATE()    - Returns error state
-- ERROR_LINE()     - Returns line number
-- ERROR_PROCEDURE() - Returns procedure name

BEGIN TRY
    SELECT 1/0;  -- Division by zero error
END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_MESSAGE() AS ErrorMessage,
        ERROR_SEVERITY() AS Severity,
        ERROR_STATE() AS ErrorState;
END CATCH;


-- =====================================================
-- Transaction Error Handling
-- =====================================================

CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY,
    Balance DECIMAL(10, 2)
);

INSERT INTO Accounts VALUES (1, 1000.00);
INSERT INTO Accounts VALUES (2, 500.00);

BEGIN TRY
    BEGIN TRANSACTION;
    
    -- Deduct from account 1
    UPDATE Accounts 
    SET Balance = Balance - 500 
    WHERE AccountID = 1;
    
    -- This will fail - account 2 doesn't exist
    UPDATE Accounts 
    SET Balance = Balance + 500 
    WHERE AccountID = 999;
    
    COMMIT;
    PRINT 'Transaction completed';
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK;
    
    PRINT 'Transaction failed!';
    PRINT ERROR_MESSAGE();
    
END CATCH;


-- =====================================================
-- Custom Error Messages
-- =====================================================

-- Add custom error message
EXEC sp_addmessage 
    @msgnum = 50001, 
    @severity = 16, 
    @msgtext = 'Invalid age. Age must be between 1 and 150.',
    @lang = 'us_english';

-- Raise custom error
BEGIN TRY
    DECLARE @Age INT = -5;
    
    IF @Age < 1 OR @Age > 150
        RAISERROR(50001, 16, 1);
        
    PRINT 'Valid age';
    
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH;


-- =====================================================
-- THROW vs RAISERROR
-- =====================================================

-- Using THROW (simpler)
BEGIN TRY
    DECLARE @Value INT = 0;
    IF @Value = 0
        THROW 50000, 'Value cannot be zero!', 1;
END TRY
BEGIN CATCH
    THROW;  -- Re-throw the error
END CATCH;

-- Using RAISERROR (more options)
BEGIN TRY
    RAISERROR('This is a custom error message!', 16, 1);
END TRY
BEGIN CATCH
    PRINT 'Caught the error!';
END CATCH;


-- =====================================================
-- Handling Specific Errors
-- =====================================================

BEGIN TRY
    -- Try to insert into non-existent table
    INSERT INTO NonExistentTable VALUES (1);
    
END TRY
BEGIN CATCH
    -- Check specific error number
    IF ERROR_NUMBER() = 208  -- Invalid object name
    BEGIN
        PRINT 'Table does not exist. Creating it now...';
        -- Create the table
        CREATE TABLE NonExistentTable (
            ID INT PRIMARY KEY
        );
    END
    ELSE
    BEGIN
        PRINT 'Other error occurred: ' + ERROR_MESSAGE();
    END
    
END CATCH;


-- =====================================================
-- Nested TRY...CATCH
-- =====================================================

BEGIN TRY
    -- Outer TRY
    BEGIN TRY
        -- Inner TRY - causes error
        SELECT 1/0;
    END TRY
    BEGIN CATCH
        PRINT 'Inner catch: ' + ERROR_MESSAGE();
        -- Handle or re-throw
        THROW;  -- Re-throws to outer CATCH
    END CATCH
    
END TRY
BEGIN CATCH
    PRINT 'Outer catch: ' + ERROR_MESSAGE();
END CATCH;


-- =====================================================
-- Using in Stored Procedures
-- =====================================================

CREATE PROCEDURE sp_SafeInsert
    @ID INT,
    @Name VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        INSERT INTO TestTable VALUES (@ID);
        PRINT 'Insert successful!';
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 2627  -- Duplicate key
            PRINT 'Duplicate ID!';
        ELSE
            PRINT 'Error: ' + ERROR_MESSAGE();
            
        RETURN ERROR_NUMBER();  -- Return error code
    END CATCH
END;


-- =====================================================
-- Error Severity Levels
-- =====================================================

/*
0-10   : Informational messages
11-16  : User-correctable errors
17     : Insufficient resources
18     : Non-fatal internal error
19-25  : Fatal errors (cannot be caught)
*/


-- =====================================================
-- Best Practices
-- =====================================================

/*
1. Always use TRY...CATCH for critical operations
2. Include meaningful error messages
3. Log errors for debugging
4. Use transactions for related operations
5. Return proper error codes from procedures
6. Don't catch errors you can't handle
*/


-- =====================================================
-- Example: Complete Error-Handled Procedure
-- =====================================================

CREATE PROCEDURE sp_TransferFunds
    @FromAccount INT,
    @ToAccount INT,
    @Amount DECIMAL(10, 2)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if amount is valid
        IF @Amount <= 0
            THROW 50000, 'Amount must be positive!', 1;
        
        -- Check sender has enough balance
        IF (SELECT Balance FROM Accounts WHERE AccountID = @FromAccount) < @Amount
            THROW 50001, 'Insufficient funds!', 1;
        
        -- Deduct from sender
        UPDATE Accounts 
        SET Balance = Balance - @Amount 
        WHERE AccountID = @FromAccount;
        
        -- Add to receiver
        UPDATE Accounts 
        SET Balance = Balance + @Amount 
        WHERE AccountID = @ToAccount;
        
        COMMIT;
        PRINT 'Transfer completed successfully!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
            
        PRINT 'Transfer failed: ' + ERROR_MESSAGE();
        RETURN 1;  -- Return error code
        
    END CATCH
    
    RETURN 0;  -- Success
END;


-- =====================================================
-- Testing the Procedure
-- =====================================================

-- Test successful transfer
EXEC sp_TransferFunds @FromAccount = 1, @ToAccount = 2, @Amount = 100;

-- Test invalid amount
EXEC sp_TransferFunds @FromAccount = 1, @ToAccount = 2, @Amount = -50;

-- Test insufficient funds
EXEC sp_TransferFunds @FromAccount = 1, @ToAccount = 2, @Amount = 10000;


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. TRY...CATCH catches errors in SQL code
2. Use ERROR functions to get error details
3. Always handle transactions properly
4. Use THROW or RAISERROR for custom errors
5. Return error codes from procedures
*/
