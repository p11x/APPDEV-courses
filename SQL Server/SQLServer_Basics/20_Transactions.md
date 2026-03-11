# SQL Server Transactions

## What is a Transaction?

A **Transaction** is a sequence of database operations that are treated as a single unit. Either all operations succeed, or none of them are applied.

## Why Use Transactions?

- **Data Integrity**: Ensure related changes happen together
- **Consistency**: Keep database in consistent state
- **ACID Properties**: Atomicity, Consistency, Isolation, Durability

## ACID Properties

| Property | Description |
|----------|-------------|
| Atomicity | All or nothing - transaction completes fully or not at all |
| Consistency | Database moves from one valid state to another |
| Isolation | Concurrent transactions don't interfere |
| Durability | Once committed, changes are permanent |

## Transaction Keywords

- **BEGIN TRANSACTION**: Start the transaction
- **COMMIT**: Save all changes
- **ROLLBACK**: Undo all changes
- **SAVEPOINT**: Create a point to rollback to

## Basic Transaction Example

```sql
BEGIN TRANSACTION;

UPDATE Accounts SET Balance = Balance - 100 
WHERE AccountID = 1;

UPDATE Accounts SET Balance = Balance + 100 
WHERE AccountID = 2;

COMMIT;
```

## Transaction with Error Handling

```sql
BEGIN TRY
    BEGIN TRANSACTION;
    
    -- First operation
    INSERT INTO Orders (OrderID, ProductID, Quantity) 
    VALUES (1, 101, 5);
    
    -- Second operation
    UPDATE Products SET Stock = Stock - 5 
    WHERE ProductID = 101;
    
    COMMIT;
    PRINT 'Transaction completed successfully';
    
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'Transaction failed: ' + ERROR_MESSAGE();
END CATCH;
```

## COMMIT vs ROLLBACK

### COMMIT - Save Changes

```sql
BEGIN TRANSACTION;

INSERT INTO Students (StudentID, Name) VALUES (1, 'John');
INSERT INTO Students (StudentID, Name) VALUES (2, 'Jane');

COMMIT;  -- Changes are now permanent
```

### ROLLBACK - Undo Changes

```sql
BEGIN TRANSACTION;

INSERT INTO Students (StudentID, Name) VALUES (3, 'Mike');

ROLLBACK;  -- Changes are undone, table unchanged
```

## Savepoints

Create a savepoint to partially rollback:

```sql
BEGIN TRANSACTION;

INSERT INTO Students VALUES (1, 'John');  -- First insert

SAVE TRANSACTION Point1;  -- Savepoint

INSERT INTO Students VALUES (2, 'Jane');  -- Second insert

-- Rollback to savepoint only
ROLLBACK TRANSACTION Point1;

COMMIT;
```

Result: John is inserted, Jane is not.

## Implicit vs Explicit Transactions

### Implicit Transaction
```sql
SET IMPLICIT_TRANSACTIONS ON;

INSERT INTO Students VALUES (3, 'Mike');  -- Auto-starts transaction

COMMIT;  -- Must explicitly commit
```

### Explicit Transaction
```sql
BEGIN TRANSACTION;
    INSERT INTO Students VALUES (4, 'Sarah');
COMMIT;
```

## Concurrency and Isolation Levels

Control how transactions interact:

```sql
SET TRANSACTION ISOLATION LEVEL 
    READ UNCOMMITTED;  -- Dirty reads allowed

SET TRANSACTION ISOLATION LEVEL 
    READ COMMITTED;    -- Default - no dirty reads

SET TRANSACTION ISOLATION LEVEL 
    REPEATABLE READ;   -- Prevent non-repeatable reads

SET TRANSACTION ISOLATION LEVEL 
    SERIALIZABLE;      -- Highest isolation
```

### Isolation Levels Explained

| Level | Dirty Read | Non-Repeatable Read | Phantom Read |
|-------|------------|---------------------|--------------|
| Read Uncommitted | Yes | Yes | Yes |
| Read Committed | No | Yes | Yes |
| Repeatable Read | No | No | Yes |
| Serializable | No | No | No |

## Distributed Transactions

For operations across multiple servers:

```sql
BEGIN DISTRIBUTED TRANSACTION;

UPDATE Server1.Database1.dbo.Products SET Stock = Stock - 1;
UPDATE Server2.Database2.dbo.Products SET Stock = Stock + 1;

COMMIT;
```

## Transaction Modes

### Auto-commit (Default)
```sql
SET IMPLICIT_TRANSACTIONS OFF;  -- Default mode
-- Each statement automatically commits
```

### Explicit Mode
```sql
SET IMPLICIT_TRANSACTIONS ON;
-- Must explicitly COMMIT or ROLLBACK
```

## Common Transaction Patterns

### Pattern 1: Money Transfer
```sql
BEGIN TRY
    BEGIN TRANSACTION;
    
    -- Deduct from sender
    UPDATE Accounts 
    SET Balance = Balance - 500 
    WHERE AccountID = @SenderID;
    
    -- Add to receiver
    UPDATE Accounts 
    SET Balance = Balance + 500 
    WHERE AccountID = @ReceiverID;
    
    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
END CATCH;
```

### Pattern 2: Batch Processing
```sql
DECLARE @ErrorCount INT = 0;

BEGIN TRANSACTION;

INSERT INTO Orders VALUES (1, 100, 5);
IF @@ERROR <> 0 SET @ErrorCount = @ErrorCount + 1;

INSERT INTO OrderItems VALUES (1, 101, 2);
IF @@ERROR <> 0 SET @ErrorCount = @ErrorCount + 1;

IF @ErrorCount = 0
    COMMIT;
ELSE
    ROLLBACK;
```

## Key Points Summary

| Keyword | Purpose |
|---------|---------|
| BEGIN TRAN | Start transaction |
| COMMIT | Save changes |
| ROLLBACK | Undo changes |
| SAVEPOINT | Partial rollback point |
| TRY/CATCH | Error handling |

---

*This topic should take about 5-7 minutes to explain in class.*
