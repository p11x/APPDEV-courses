# SQL Server Disaster Recovery

## What is Disaster Recovery?

**Disaster Recovery (DR)** is the process of restoring data and resuming operations after a catastrophic event like hardware failure, natural disaster, or cyber attack.

## Disaster Recovery Metrics

### RTO - Recovery Time Objective

Maximum acceptable time to restore service:

```
RTO = 4 hours  ← Business defines this
     ↓
[Disaster] → [Recovery] → [Service Restored]
              3 hours ✓
```

### RPO - Recovery Point Objective

Maximum acceptable data loss (time):

```
RPO = 1 hour
     ↓
[Last Backup] ────── [Now] ────── [Disaster]
       30 min lost  ← Acceptable!
```

## Disaster Recovery Strategies

### Strategy 1: Backup and Restore

Simplest - restore from backups:

```
Full Backup (Sunday)
    ↓
Diff Backup (Daily)
    ↓
Log Backup (Hourly)
    ↓
[Disaster] → Restore Full → Restore Diff → Restore Logs → RPO: 1 hour
```

### Strategy 2: Always On Availability Groups

Automatic failover:

```
┌─────────────────┐
│  Primary Server │
│  (Active)       │
└────────┬────────┘
         │ Sync
         ▼
┌─────────────────┐
│ Secondary Server│
│ (Standby)       │
└─────────────────┘

[Disaster] → Auto Failover → RTO: seconds
```

### Strategy 3: Database Mirroring

Real-time data mirroring:

```
┌──────────┐         ┌──────────┐
│ Principal │◄───────►│  Mirror  │
│  Server   │  Sync   │  Server  │
└──────────┘         └──────────┘
         ↓
   [Disaster] → Failover → RTO: seconds
```

### Strategy 4: Log Shipping

Standby server with log shipping:

```
Primary Server → Transaction Logs → Standby Server
   (Active)        Every 15 min      (Read-only)

[Disaster] → Failover → RTO: minutes
```

## Creating a Disaster Recovery Plan

### Step 1: Document Current Environment

```sql
-- List all databases
SELECT name AS DatabaseName, 
       state_desc AS Status,
       user_access_desc AS Access
FROM sys.databases;

-- List backup devices
SELECT * FROM sys.backup_devices;

-- Check recovery model
SELECT name AS DBName, 
       recovery_model_desc AS RecoveryModel
FROM sys.databases;
```

### Step 2: Test Restoration Process

```sql
-- Restore to test server
RESTORE DATABASE MyDatabase_Test
FROM DISK = 'C:\Backups\MyDatabase_Full.bak'
WITH MOVE 'MyDatabase' TO 'C:\TestData\MyDatabase_Test.mdf',
     MOVE 'MyDatabase_Log' TO 'C:\TestLogs\MyDatabase_Test.ldf',
     REPLACE,
     RECOVERY;
```

### Step 3: Document Recovery Procedures

Create runbooks with step-by-step procedures:

| Scenario | Recovery Steps |
|----------|---------------|
| Database corruption | Restore latest full + diff + logs |
| Server failure | Rebuild server, restore databases |
| Complete loss | Restore from off-site backups |

## High Availability vs Disaster Recovery

| Feature | HA | DR |
|---------|----|----|
| Location | Same data center | Different site |
| Purpose | Minimize downtime | Survive disaster |
| RTO | Minutes | Hours/Days |
| RPO | Minimal | May allow data loss |

## Azure SQL Database DR

```sql
-- Active Geo-Replication (Azure SQL)
-- Create readable secondary
ALTER DATABASE MyDB ADD SECONDARY ON SERVER (SecondaryServer);

-- Failover to secondary
ALTER DATABASE MyDB FAILOVER;

-- Auto-failover groups
CREATE FAILOVER GROUP MyFG 
    ON (MyDB, SecondaryServer)
    WITH (
        FAILOVER_MODE = AUTOMATIC,
        GRACE_PERIOD_MINUTES = 10
    );
```

## Testing DR Plans

### Regular Testing Schedule

- Monthly: Full restoration test
- Quarterly: Complete DR exercise
- After any major change: Review and update

### What to Test

1. Backup restoration
2. Failover procedures
3. Recovery time verification
4. Data integrity checks

## Key Points Summary

| Metric | Description |
|--------|-------------|
| RTO | Maximum downtime acceptable |
| RPO | Maximum data loss acceptable |
| HA | Same-site redundancy |
| DR | Off-site protection |

---

*This topic should take about 5-7 minutes to explain in class.*
