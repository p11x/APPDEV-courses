# SQL Server Transactions and Locking

## Introduction

In Tutorial 20, we covered basic transactions. Now let's dive deeper into transaction management and understand how locking works in SQL Server.

## Transaction Isolation Levels

Isolation levels determine how transactions interact with each other:

### Setting Isolation Levels

```sql
-- Set isolation level for current session
SET TRANSACTION ISOLATION LEVEL 
    READ UNCOMMITTED;

SET TRANSACTION ISOLATION LEVEL 
    READ COMMITTED;

SET TRANSACTION ISOLATION LEVEL 
    REPEATABLE READ;

SET TRANSACTION ISOLATION LEVEL 
    SNAPSHOT;

SET TRANSACTION ISOLATION LEVEL 
    SERIALIZABLE;
```

### Isolation Level Comparison

| Level | Dirty Read | Non-Repeatable | Phantom |
|-------|------------|----------------|---------|
| Read Uncommitted | Yes | Yes | Yes |
| Read Committed | No | Yes | Yes |
| Repeatable Read | No | No | Yes |
| Snapshot | No | No | No |
| Serializable | No | No | No |

## Understanding Locking

SQL Server uses locks to ensure transaction isolation:

### Lock Types

| Lock Type | Description |
|-----------|-------------|
| Shared (S) | Used for reading data |
| Exclusive (X) | Used for modifying data |
| Update (U) | Used during updates |
| Intent | Shows intent to lock |

### Viewing Locks

```sql
-- Enable lock monitoring
EXEC sp_lock;

-- Or query sys.dm_tran_locks
SELECT 
    request_session_id AS SessionID,
    resource_type AS ResourceType,
    resource_database_id AS DBID,
    request_mode AS Mode,
    request_status AS Status
FROM sys.dm_tran_locks;
```

## Lock Examples

### Shared Lock Example

```sql
-- Session 1: Start transaction with READ COMMITTED
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION;

SELECT * FROM Accounts WHERE AccountID = 1;
-- This places a Shared (S) lock

COMMIT;
-- Lock is released
```

### Exclusive Lock Example

```sql
-- Session 1: Start transaction
BEGIN TRANSACTION;

UPDATE Accounts SET Balance = Balance - 100 
WHERE AccountID = 1;

-- This places an Exclusive (X) lock
-- Other sessions cannot read or write this row

COMMIT;
```

## Deadlocks

A **deadlock** occurs when two transactions each hold a lock that the other needs:

```sql
-- Session 1
BEGIN TRANSACTION;
UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;  -- Lock A
-- (waits for Session 2 to release lock B)
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;  -- Wants lock B

-- Session 2 (executed concurrently)
BEGIN TRANSACTION;
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;  -- Lock B
-- (waits for Session 1 to release lock A)
UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;  -- Wants lock A
```

### Handling Deadlocks

```sql
-- Set deadlock priority
SET DEADLOCK_PRIORITY LOW;    -- This session will be chosen to die
SET DEADLOCK_PRIORITY NORMAL; -- Default
SET DEADLOCK_PRIORITY HIGH;   -- This session will survive

-- SQL Server will choose one to be the victim
```

## Locking Hints

Use hints to control locking behavior:

```sql
-- Force no lock (dirty read)
SELECT * FROM Accounts WITH (NOLOCK);

-- Force row-level lock
SELECT * FROM Accounts WITH (ROWLOCK);

-- Force page-level lock
SELECT * FROM Accounts WITH (PAGELOCK);

-- Force table lock
SELECT * FROM Accounts WITH (TABLOCK);

-- Force serializable
SELECT * FROM Accounts WITH (SERIALIZABLE);

-- No lock escalation
SELECT * FROM Accounts WITH (NOEXPAND);
```

## Transaction Modes

### Autocommit Mode (Default)

```sql
SET IMPLICIT_TRANSACTIONS OFF;  -- Default

-- Each statement auto-commits
INSERT INTO Accounts VALUES (1, 100);  -- Auto-committed
UPDATE Accounts SET Balance = 200 WHERE ID = 1;  -- Auto-committed
```

### Explicit Mode

```sql
SET IMPLICIT_TRANSACTIONS ON;

-- Must explicitly commit or rollback
INSERT INTO Accounts VALUES (2, 200);  -- Starts transaction
-- Must COMMIT or ROLLBACK
COMMIT;
```

### Implicit Mode

```sql
SET IMPLICIT_TRANSACTIONS ON;

-- Transaction automatically starts for certain statements
-- Must explicitly commit at end
```

## Snapshot Isolation

Enables reading versioned data:

```sql
-- Enable snapshot isolation at database level
ALTER DATABASE MyDatabase SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE MyDatabase SET READ_COMMITTED_SNAPSHOT ON;
```

### Using Snapshot Isolation

```sql
-- Session 1: Start snapshot transaction
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;

SELECT * FROM Accounts;  -- Reads committed data as of transaction start
-- Can read without blocking

COMMIT;
```

## Optimistic vs Pessimistic Concurrency

### Pessimistic Locking

```sql
-- Lock early, release late
BEGIN TRANSACTION;

SELECT * FROM Accounts WITH (UPDLOCK) WHERE ID = 1;
-- User is thinking...

UPDATE Accounts SET Balance = 500 WHERE ID = 1;

COMMIT;
```

### Optimistic Locking

```sql
-- Check if changed since read
BEGIN TRANSACTION;

SELECT Balance, RowVersion FROM Accounts WHERE ID = 1;

-- Check in UPDATE
UPDATE Accounts 
SET Balance = 500, 
    RowVersion = RowVersion + 1
WHERE ID = 1 AND RowVersion = @OriginalRowVersion;

IF @@ROWCOUNT = 0
    PRINT 'Data changed by another user!';

COMMIT;
```

## Key Points Summary

| Concept | Description |
|---------|-------------|
| Isolation Levels | Control transaction visibility |
| Lock Types | S, X, U, Intent locks |
| Deadlock | Circular wait situation |
| Lock Hints | Force specific locking behavior |
| Snapshot | Version-based isolation |
| Optimistic | Check-then-update |

---

*This topic should take about 5-7 minutes to explain in class.*
