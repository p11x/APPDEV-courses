# SQL Server Security Best Practices

## Introduction

Database security protects your data from unauthorized access, theft, and damage. This covers essential security practices.

## Authentication

### SQL Server Authentication Modes

| Mode | Description | Use Case |
|------|-------------|----------|
| Windows Auth | Uses Windows credentials | Corporate networks |
| Mixed Mode | Both Windows & SQL login | Multiple clients |

### Setting Authentication Mode

```sql
-- Via SSMS:
-- Right-click Server > Properties > Security

-- Or via T-SQL (requires restart)
USE master;
GO
EXEC xp_instance_regwrite 
    N'HKEY_LOCAL_MACHINE', 
    N'SOFTWARE\Microsoft\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQLServer',
    N'LoginMode', 
    REG_DWORD, 
    2;  -- 1 = Windows Only, 2 = Mixed
GO
```

## Authentication Best Practices

### Use Windows Authentication

```sql
-- Create Windows group login
USE master;
GO
CREATE LOGIN [DOMAIN\DB_Users] FROM WINDOWS;
GO
```

### Strong Password Policy

```sql
-- Create SQL login with strong password
CREATE LOGIN AppUser 
WITH PASSWORD = 'C0mplex!P@ssw0rd2024!',
     DEFAULT_DATABASE = MyDatabase,
     CHECK_POLICY = ON,
     CHECK_EXPIRATION = ON;
```

### Manage Passwords

```sql
-- Change password
ALTER LOGIN AppUser WITH PASSWORD = 'NewStr0ng!P@ss';

-- Force password change on next login
ALTER LOGIN AppUser WITH CHECK_POLICY = ON, CHECK_EXPIRATION = ON;
ALTER LOGIN AppUser MUST_CHANGE_PASSWORD = ON;
```

## Authorization

### Principals and Securables

```
Principals (Who)
├── Windows Logins
├── SQL Logins
├── Database Users
└── Roles

Securables (What)
├── Servers
├── Databases
├── Schemas
└── Objects
```

### Create Users and Roles

```sql
-- Create database user
USE MyDatabase;
GO
CREATE USER AppUser FOR LOGIN AppUser;
GO

-- Add to role
ALTER ROLE db_datareader ADD MEMBER AppUser;
ALTER ROLE db_datawriter ADD MEMBER AppUser;

-- Create custom role
CREATE ROLE ReportReader;
GRANT SELECT ON SCHEMA::dbo TO ReportReader;
GRANT VIEW DEFINITION TO ReportReader;
```

### Permission Management

```sql
-- Grant permissions
GRANT SELECT, INSERT ON Employees TO AppUser;

-- Deny permissions
DENY DELETE ON Employees TO AppUser;

-- Revoke permissions
REVOKE SELECT ON Employees TO AppUser;

-- View permissions
SELECT * FROM fn_my_permissions('Employees', 'OBJECT');
```

## Principle of Least Privilege

Only grant minimum permissions needed:

```sql
-- Bad: Grant excessive permissions
ALTER ROLE db_owner ADD MEMBER AppUser;

-- Good: Grant specific permissions
GRANT SELECT, INSERT, UPDATE ON dbo.Employees TO AppUser;
GRANT EXECUTE ON dbo.sp_GetEmployee TO AppUser;
```

## Encryption

### Transparent Data Encryption (TDE)

```sql
-- Create master key
USE master;
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'MasterKey!2024';

-- Create certificate
CREATE CERTIFICATE MyServerCert 
WITH SUBJECT = 'My Database Encryption Certificate';

-- Encrypt database
USE MyDatabase;
CREATE DATABASE ENCRYPTION KEY
WITH ALGORITHM = AES_256
ENCRYPTION BY SERVER CERTIFICATE MyServerCert;

ALTER DATABASE MyDatabase
SET ENCRYPTION ON;
```

### Column-Level Encryption

```sql
-- Create master key
USE MyDatabase;
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'ColKey!2024';

-- Create certificate
CREATE CERTIFICATE ColCert 
WITH SUBJECT = 'Column Encryption Certificate';

-- Create symmetric key
CREATE SYMMETRIC KEY CreditCardKey 
WITH ALGORITHM = AES_256
ENCRYPTION BY CERTIFICATE ColCert;

-- Add encrypted column
ALTER TABLE Customers
ADD CreditCardEncrypted VARBINARY(MAX);

-- Encrypt data
OPEN SYMMETRIC KEY CreditCardKey 
DECRYPTION BY CERTIFICATE ColCert;

UPDATE Customers
SET CreditCardEncrypted = ENCRYPTBYKEY(KEY_GUID('CreditCardKey'), CreditCardNumber);

CLOSE SYMMETRIC KEY CreditCardKey;
```

### Always Encrypted (Modern Approach)

```sql
-- Create table with Always Encrypted columns
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(50),
    SSN CHAR(11) ENCRYPTED WITH (
        COLUMN_ENCRYPTION_KEY = CEK1,
        ENCRYPTION_TYPE = DETERMINISTIC,
        ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
    ),
    CreditCard VARCHAR(20) ENCRYPTED WITH (
        COLUMN_ENCRYPTION_KEY = CEK1,
        ENCRYPTION_TYPE = RANDOMIZED,
        ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
    )
);
```

## Auditing

### SQL Server Audit

```sql
-- Create server audit
USE master;
CREATE SERVER AUDIT MyAudit
TO FILE (FILEPATH = 'C:\AuditLogs\', MAXSIZE = 100MB)
WITH (ON_FAILURE = CONTINUE);

-- Create audit specification
CREATE SERVER AUDIT SPECIFICATION MyServerAudit
FOR SERVER AUDIT MyAudit
ADD (SUCCESSFUL_LOGIN_GROUP),
ADD (FAILED_LOGIN_GROUP);

-- Enable audit
ALTER SERVER AUDIT MyAudit WITH (STATE = ON);
```

### Database Audit

```sql
-- Create database audit specification
USE MyDatabase;
CREATE DATABASE AUDIT SPECIFICATION MyDBAudit
FOR SERVER AUDIT MyAudit
ADD (SELECT ON dbo.Employees BY AppUser),
ADD (INSERT ON dbo.Customers BY AppUser);

-- View audit logs
SELECT * FROM sys.fn_get_audit_file('C:\AuditLogs\*.sqlaudit', DEFAULT, DEFAULT);
```

## SQL Injection Prevention

### Parameterized Queries

```sql
-- Bad (vulnerable)
EXEC sp_executesql N'SELECT * FROM Users WHERE Name = ''' + @Name + '''';

-- Good (parameterized)
EXEC sp_executesql 
    N'SELECT * FROM Users WHERE Name = @Name',
    N'@Name NVARCHAR(50)',
    @Name = @Name;
```

### Stored Procedures

```sql
-- Use stored procedures
CREATE PROCEDURE sp_GetUser
    @UserName NVARCHAR(50)
AS
BEGIN
    -- Parameterized internally
    SELECT * FROM Users WHERE UserName = @UserName;
END;
```

## Network Security

### Use SSL/TLS

```sql
-- Enable encrypted connections
-- Via Configuration Manager:
-- 1. Enable Force Protocol Encryption
-- 2. Certificate configuration
```

### Firewall Rules

```sql
-- Windows Firewall (command line)
netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=tcp localport=1433
```

## Security Checklist

| Area | Action |
|------|--------|
| Authentication | Use Windows Auth when possible |
| Passwords | Strong policies, enforce expiration |
| Authorization | Grant minimum permissions |
| Encryption | Use TDE for data at rest |
| Auditing | Enable login and DDL auditing |
| Injection | Use parameterized queries |
| Network | Enable SSL, firewall rules |

## Key Points Summary

- **Authentication**: Control who can connect
- **Authorization**: Control what they can do
- **Encryption**: Protect data at rest and in transit
- **Auditing**: Track access and changes
- **Least Privilege**: Grant minimum permissions needed

---

*This topic should take about 5-7 minutes to explain in class.*
