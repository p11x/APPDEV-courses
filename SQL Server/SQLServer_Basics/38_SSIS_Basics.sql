-- =====================================================
-- SQL Server Integration Services (SSIS) Basics
-- =====================================================
-- SSIS is used for data integration and ETL (Extract, Transform, Load)

-- =====================================================
-- What is SSIS?
-- =====================================================

/*
SSIS (SQL Server Integration Services) is a platform for:
- Data migration
- Data transformation
- ETL processes
- Workflow automation

It uses Visual Studio with SSDT (SQL Server Data Tools) to develop.
*/

-- =====================================================
-- SSIS Components
-- =====================================================

/*
1. Control Flow: Orchestrates tasks (loops, conditions)
2. Data Flow: Moves and transforms data
3. Event Handlers: Handles errors/logs
4. Parameters: Configure packages
*/


-- =====================================================
-- Executing SSIS Packages via T-SQL
-- =====================================================

-- Run an SSIS package using stored procedure
DECLARE @execution_id BIGINT;

EXEC SSISDB.catalog.create_execution
    @package_name = 'MyPackage.dtsx',
    @folder_name = 'MyFolder',
    @project_name = 'MyProject',
    @use32bitruntime = FALSE,
    @execution_id = @execution_id OUTPUT;

EXEC SSISDB.catalog.start_execution @execution_id;


-- =====================================================
-- Example: Creating a Simple ETL with T-SQL
-- =====================================================

-- Create staging table
CREATE TABLE Staging_Orders (
    OrderID INT,
    CustomerName VARCHAR(100),
    OrderDate DATE,
    TotalAmount DECIMAL(10,2),
    LoadDate DATETIME DEFAULT GETDATE()
);

-- Extract: Load data from source (simulated)
INSERT INTO Staging_Orders (OrderID, CustomerName, OrderDate, TotalAmount)
SELECT 
    OrderID,
    CustomerName,
    OrderDate,
    TotalAmount
FROM SourceDB.dbo.Orders
WHERE OrderDate >= '2024-01-01';


-- Transform: Clean and transform data
INSERT INTO DimOrders (OrderKey, OrderID, CustomerName, OrderDate, Amount)
SELECT 
    ROW_NUMBER() OVER (ORDER BY OrderID) AS OrderKey,
    OrderID,
    UPPER(TRIM(CustomerName)) AS CustomerName,
    CAST(OrderDate AS DATE) AS OrderDate,
    ISNULL(TotalAmount, 0) AS Amount
FROM Staging_Orders;


-- Load: Move to destination
-- (Already done in transform step)


-- =====================================================
-- Using OPENROWSET for File Import
-- =====================================================

-- Enable ad hoc distributed queries
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'Ad Hoc Distributed Queries', 1;
RECONFIGURE;

-- Import CSV file
INSERT INTO Staging_Customers
SELECT * FROM OPENROWSET(
    'MSDASQL',
    'Driver={Microsoft Text Driver (*.txt; *.csv)};DefaultDir=C:\Data;',
    'SELECT * FROM Customers.csv'
);

-- Import Excel file
INSERT INTO Staging_Products
SELECT * FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0;Database=C:\Data\Products.xlsx',
    'SELECT * FROM [Sheet1$]'
);


-- =====================================================
-- Using BULK INSERT
-- =====================================================

-- Create format file (XML)
BULK INSERT Customers
FROM 'C:\Data\Customers.txt'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Insert with format file
BULK INSERT Orders
FROM 'C:\Data\Orders.txt'
WITH (
    FORMATFILE = 'C:\Data\Orders_Format.xml'
);


-- =====================================================
-- Automating ETL with SQL Agent
-- =====================================================

-- Create job for nightly ETL
USE msdb;
GO

EXEC sp_add_job
    @job_name = N'Nightly_ETL_Job',
    @enabled = 1,
    @description = 'Runs nightly ETL process';

EXEC sp_add_jobstep
    @job_name = N'Nightly_ETL_Job',
    @step_name = N'Extract_Transform_Load',
    @command = N'
        EXEC usp_ETL_Orders;
        EXEC usp_ETL_Customers;
    ',
    @database_name = 'ETLDatabase';

EXEC sp_add_schedule
    @schedule_name = N'Nightly_2AM',
    @freq_type = 4,
    @freq_interval = 1,
    @freq_subday_type = 1,
    @freq_subday_interval = 0,
    @active_start_time = 020000;

EXEC sp_attach_schedule
    @job_name = N'Nightly_ETL_Job',
    @schedule_name = N'Nightly_2AM';

EXEC sp_add_jobserver
    @job_name = N'Nightly_ETL_Job',
    @server_name = N'(local)';


-- =====================================================
-- Incremental Load Pattern
-- =====================================================

CREATE PROCEDURE usp_IncrementalOrderLoad
    @LastLoadDate DATETIME
AS
BEGIN
    -- Get new/changed records since last load
    INSERT INTO DimOrders (OrderID, CustomerName, OrderDate, Amount)
    SELECT 
        OrderID,
        CustomerName,
        OrderDate,
        TotalAmount
    FROM SourceDB.dbo.Orders
    WHERE ModifiedDate > @LastLoadDate;
    
    -- Update watermark
    UPDATE ETL_Watermarks
    SET LastLoadDate = GETDATE()
    WHERE TableName = 'Orders';
END;


-- =====================================================
-- Logging and Error Handling in ETL
-- =====================================================

CREATE TABLE ETL_Log (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    PackageName VARCHAR(100),
    StepName VARCHAR(100),
    StartTime DATETIME,
    EndTime DATETIME,
    Status VARCHAR(20),
    ErrorMessage VARCHAR(MAX)
);

CREATE PROCEDURE usp_ETL_Log
    @PackageName VARCHAR(100),
    @StepName VARCHAR(100),
    @Status VARCHAR(20),
    @ErrorMessage VARCHAR(MAX) = NULL
AS
BEGIN
    INSERT INTO ETL_Log (PackageName, StepName, StartTime, EndTime, Status, ErrorMessage)
    VALUES (@PackageName, @StepName, GETDATE(), GETDATE(), @Status, @ErrorMessage);
END;


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. SSIS: Visual ETL tool from Microsoft
2. Control Flow: Task orchestration
3. Data Flow: Data movement/transformation
4. OPENROWSET: Import external files
5. BULK INSERT: Fast file imports
6. SQL Agent: Schedule ETL jobs
7. Incremental Load: Efficient large data loads
8. Logging: Track ETL execution
*/
