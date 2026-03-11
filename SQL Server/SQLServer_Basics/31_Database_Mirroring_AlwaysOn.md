# SQL Server High Availability: Mirroring and Always On

## Introduction

High Availability (HA) ensures your database is always accessible. SQL Server provides several HA solutions.

## Database Mirroring

**Database Mirroring** creates a redundant copy of a database on another server.

### How It Works

```
┌─────────────┐         ┌─────────────┐
│  Principal  │ ◄──────►│   Mirror    │
│  Server     │ Mirror  │   Server    │
│             │ Connection│           │
└─────────────┘         └─────────────┘
        │
        │ Optional
        ▼
┌─────────────┐
│   Witness   │
│   Server    │
└─────────────┘
```

### Mirroring Modes

| Mode | Description | Failover |
|------|-------------|----------|
| High Availability | Synchronous, auto failover | Automatic |
| High Protection | Synchronous, manual | Manual |
| High Performance | Asynchronous | Manual |

### Setting Up Mirroring

```sql
-- Step 1: Prepare mirror database
-- Backup principal database
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase.bak';

-- Restore on mirror server
RESTORE DATABASE MyDatabase
FROM DISK = 'C:\Backups\MyDatabase.bak'
WITH NORECOVERY;

-- Apply transaction logs
RESTORE LOG MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Log1.tran'
WITH NORECOVERY;

-- Step 2: Set up endpoint (run on both servers)
CREATE ENDPOINT DatabaseMirroring
AS TCP (LISTENER_PORT = 5022)
FOR DATABASE_MIRRORING (
    ROLE = ALL,
    ENCRYPTION = REQUIRED
);

-- Step 3: Establish partnership (run on principal)
ALTER DATABASE MyDatabase
SET PARTNER = 'TCP://MirrorServer:5022';

-- Step 4: Set mirror endpoint (run on mirror)
ALTER DATABASE MyDatabase
SET PARTNER = 'TCP://PrincipalServer:5022';

-- Add witness (optional)
ALTER DATABASE MyDatabase
SET WITNESS = 'TCP://WitnessServer:5022';
```

### Managing Mirroring

```sql
-- Manual failover (from principal)
ALTER DATABASE MyDatabase SET PARTNER FAILOVER;

-- Force service (from mirror)
ALTER DATABASE MyDatabase SET PARTNER FORCE_SERVICE_ALLOW_DATA_LOSS;

-- Remove mirroring
ALTER DATABASE MyDatabase SET PARTNER OFF;
```

## Always On Availability Groups

**Always On** is the modern HA solution, providing failover at the database group level.

### Architecture

```
┌─────────────────────────────────────────┐
│        Availability Group (Listener)    │
│              Port 1433                  │
└────────────┬────────────────────────────┘
             │
     ┌───────┼───────┬────────┐
     ▼       ▼       ▼        ▼
┌────────┐┌────────┐┌────────┐┌────────┐
│Primary ││ Replica││ Replica││ Replica│
│ Node 1 ││ Node 2 ││ Node 3 ││ Node 4 │
└────────┘└────────┘└────────┘└────────┘
```

### Prerequisites

- Windows Server Failover Clustering (WSFC)
- SQL Server Enterprise Edition (for full features)
- Enterprise Edition with Basic Always On (Standard Edition)

### Setting Up Always On

```sql
-- Step 1: Enable Always On (requires restart)
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'hadr enabled', 1;
RECONFIGURE;

-- Step 2: Create availability group (via SSMS or PowerShell)
-- PowerShell is typically used:
New-AgAvailabilityGroup -Name "MyAG" -Database "MyDatabase"

-- Step 3: Add replicas
Add-AzSqlAvailabilityGroupListener -AvailabilityGroupName "MyAG" `
    -ResourceGroupName "MyRG" `
    -Name "MyListener" `
    -Port 1433
```

### Always On vs Mirroring

| Feature | Mirroring | Always On |
|---------|-----------|-----------|
| Enterprise Only | No | Yes (full) |
| Multiple DBs | No | Yes |
| Read Scale-out | No | Yes |
| Automatic Failover | Yes (with witness) | Yes |
| Backup on Secondary | No | Yes |

## Log Shipping

Simpler HA solution:

```sql
-- Configure via SSMS:
-- 1. Backup schedule on primary
-- 2. Copy to secondary
-- 3. Restore on secondary
-- 4. Configure alert
```

### Key Differences

| Feature | Mirroring | Log Shipping | Always On |
|---------|-----------|--------------|-----------|
| Failover Type | Database | Database | Group |
| Auto Failover | Yes | No | Yes |
| Read Replica | No | No | Yes |
| Setup Complexity | Medium | Low | High |

## Failover Clustering

OS-level HA with SQL Server:

```
┌────────────────────────┐
│  Windows Cluster       │
│  ┌──────────────────┐  │
│  │ Virtual IP       │  │
│  └──────────────────┘  │
│       │     │     │      │
│  ┌────┴─┐ ┌───┴──┐ ┌───┴──┐
│  │Node 1│ │Node 2│ │Node 3│
│  │ SQL  │ │ SQL  │ │ SQL  │
│  └──────┘ └──────┘ └──────┘
```

## Choosing HA Solution

| Scenario | Recommended Solution |
|----------|-------------------|
| Simple setup, basic HA | Mirroring |
| Multiple databases, enterprise | Always On |
| Minimal cost, simple | Log Shipping |
| Mission critical, zero downtime | Always On |

## Key Points Summary

- **Database Mirroring**: Good for single database HA
- **Always On**: Enterprise-grade HA with read scaling
- **Log Shipping**: Simple, cost-effective backup solution
- **Failover Clustering**: OS-level protection

---

*This topic should take about 5-7 minutes to explain in class.*
