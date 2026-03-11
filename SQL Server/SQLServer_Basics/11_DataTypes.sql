-- =====================================================
-- SQL Server: Data Types
-- =====================================================
-- Data types define what type of data a column can hold

-- =====================================================
-- Numeric Data Types
-- =====================================================

-- INT (Integer): Whole numbers from -2,147,483,648 to 2,147,483,647
CREATE TABLE ExampleInt (
    Age INT,
    Quantity INT,
    EmployeeID INT
);
-- Example: 1, 100, -50, 0


-- BIGINT: Large whole numbers
-- Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
CREATE TABLE ExampleBigInt (
    Population BIGINT,
    LargeNumber BIGINT
);


-- SMALLINT: Small whole numbers
-- Range: -32,768 to 32,767
CREATE TABLE ExampleSmallInt (
    DaysSmall SMALLINT,
    QuantitySmall SMALLINT
);


-- DECIMAL(p, s) or NUMERIC(p, s): Exact numeric values
-- p = precision (total digits), s = scale (digits after decimal)
CREATE TABLE ExampleDecimal (
    Price DECIMAL(10, 2),     -- 10 total digits, 2 after decimal
    Percentage NUMERIC(5, 2), -- 5 total digits, 2 after decimal
    Score DECIMAL(3, 1)      -- 3 total digits, 1 after decimal
);
-- Example: 1234.56, 99.99, 5.5


-- FLOAT: Approximate numeric values (for scientific calculations)
CREATE TABLE ExampleFloat (
    Temperature FLOAT,
    ScientificValue FLOAT(53)  -- 53-bit precision
);
-- Example: 123.45, 3.14159


-- =====================================================
-- String Data Types
-- =====================================================

-- CHAR(n): Fixed-length string (pads with spaces)
CREATE TABLE ExampleChar (
    Code CHAR(5),     -- Always 5 characters
    Status CHAR(10)   -- Always 10 characters
);
-- Example: 'ABC' stored as 'ABC  ' (with padding)


-- VARCHAR(n): Variable-length string (up to n characters)
CREATE TABLE ExampleVarchar (
    Name VARCHAR(50),       -- Up to 50 characters
    Description VARCHAR(200) -- Up to 200 characters
);
-- Example: 'John', 'Database Administrator'


-- VARCHAR(MAX): Large variable-length string (up to 2GB)
CREATE TABLE ExampleVarcharMax (
    LongText VARCHAR(MAX),  -- For large text
    ArticleContent VARCHAR(MAX)
);


-- TEXT: Large text data (older type, use VARCHAR(MAX) instead)
-- Note: Deprecated in favor of VARCHAR(MAX)


-- =====================================================
-- Date and Time Data Types
-- =====================================================

-- DATE: Date only (YYYY-MM-DD)
CREATE TABLE ExampleDate (
    BirthDate DATE,
    HireDate DATE,
    EventDate DATE
);
-- Example: '2024-01-15'


-- TIME: Time only (HH:MM:SS)
CREATE Table ExampleTime (
    StartTime TIME,
    EndTime TIME,
    MeetingTime TIME
);
-- Example: '14:30:00'


-- DATETIME: Date and time (includes milliseconds)
CREATE Table ExampleDateTime (
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
-- Example: '2024-01-15 14:30:00.000'


-- DATETIME2: Enhanced datetime (more precision)
CREATE Table ExampleDateTime2 (
    Timestamp DATETIME2(3),  -- 3 decimal places for milliseconds
    Created DATETIME2
);
-- Example: '2024-01-15 14:30:00.123'


-- =====================================================
-- Other Common Data Types
-- =====================================================

-- BIT: Boolean (0, 1, or NULL)
CREATE TABLE ExampleBit (
    IsActive BIT,       -- TRUE/FALSE
    HasDiscount BIT,    -- 1 = Yes, 0 = No
    IsVerified BIT
);
-- Example: 0, 1


-- UNIQUEIDENTIFIER: Globally unique identifier (GUID)
CREATE TABLE ExampleGUID (
    RowID UNIQUEIDENTIFIER,
    SessionID UNIQUEIDENTIFIER
);
-- Example: '6F9619FF-8B86-D011-B42D-00C04FC964FF'


-- MONEY: Currency values
CREATE TABLE ExampleMoney (
    Salary MONEY,
    Price MONEY,
    Balance MONEY
);
-- Example: $1,234.56


-- =====================================================
-- Complete Example Table with Various Data Types
-- =====================================================

DROP TABLE IF EXISTS SampleEmployee;

CREATE TABLE SampleEmployee (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Phone VARCHAR(20),
    BirthDate DATE,
    HireDate DATETIME DEFAULT GETDATE(),
    Salary DECIMAL(10, 2),
    IsActive BIT DEFAULT 1,
    Notes VARCHAR(MAX),
    ProfileImage VARBINARY(MAX)
);

-- Insert sample data
INSERT INTO SampleEmployee 
    (EmployeeID, FirstName, LastName, Email, Phone, BirthDate, Salary)
VALUES 
    (1, 'John', 'Doe', 'john.doe@email.com', '555-1234', '1990-05-15', 50000.00),
    (2, 'Jane', 'Smith', 'jane.smith@email.com', '555-5678', '1985-08-20', 55000.00),
    (3, 'Mike', 'Johnson', 'mike.j@email.com', '555-9012', '1992-11-30', 48000.00);

-- View the data
SELECT * FROM SampleEmployee;


-- =====================================================
-- Choosing the Right Data Type
-- =====================================================

/*
Data Type    | Use When...
-------------|-----------------------------------
INT          | Whole numbers (age, quantity, ID)
DECIMAL      | Precise numbers (money, measurements)
VARCHAR      | Text of unknown or variable length
CHAR         | Fixed-length codes (ZIP, country code)
DATE         | Dates without time
DATETIME     | Dates with time
BIT          | Yes/No, True/False flags
*/


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. Choose appropriate data type for your data
2. INT for whole numbers
3. DECIMAL for exact decimal values
4. VARCHAR for variable-length text
5. DATE for calendar dates
6. BIT for true/false values
7. Use the smallest data type that fits your needs
*/
