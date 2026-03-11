# SQL Server Backup and Restore

## Introduction

Backup and restore are critical for data protection. This covers SQL Server backup strategies and restore operations.

## Backup Types

| Type | Description | Size |
|------|-------------|------|
| Full | Complete database backup | Largest |
| Differential | Changes since last full | Medium |
| Transaction Log | Log records since last backup | Smallest |

## Creating Backups

### Full Backup

```sql
-- Simple full backup
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Full.bak'
WITH FORMAT,
     NAME = 'MyDatabase-Full Backup';
```

### Differential Backup

```sql
-- Backup changes since last full
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Diff_001.bak'
WITH DIFFERENTIAL,
     NAME = 'MyDatabase-Differential Backup 1';
```

### Transaction Log Backup

```sql
-- Backup transaction log
BACKUP LOG MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Log_001.trn'
WITH NAME = 'MyDatabase-Log Backup 1';
```

## Backup Options

```sql
-- Compressed backup
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Compressed.bak'
WITH COMPRESSION;

-- Encrypted backup
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Encrypted.bak'
WITH ENCRYPTION (
    ALGORITHM = AES_256,
    SERVER CERTIFICATE = BackupCert
);

-- Check sum for integrity
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase.bak'
WITH CHECKSUM;
```

## Backup Strategy: 3-2-1 Rule

```
3 - Three copies of data
2 - Two different media types
1 - One off-site backup
```

### Sample Backup Schedule

```sql
-- Sunday 2 AM: Full backup
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Full_Sun.bak'
WITH INIT;  -- Overwrite

-- Monday-Saturday 2 AM: Differential
BACKUP DATABASE MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Diff.bak'
WITH DIFFERENTIAL;

-- Every hour: Transaction log
BACKUP LOG MyDatabase
TO DISK = 'C:\Backups\MyDatabase_Log.trn';
```

## Restoring Backups

### Restore Full Backup

```sql
-- Restore full backup
RESTORE DATABASE MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Full.bak'
WITH REPLACE,  -- Overwrite existing
     NORECOVERY;  -- Wait for more backups
```

### Restore with Differential

```sql
-- First: Restore full
RESTORE DATABASE MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Full.bak'
WITH NORECOVERY;

-- Then: Restore differential
RESTORE DATABASE MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Diff_001.bak'
WITH RECOVERY;
```

### Restore with Transaction Logs

```sql
-- Restore full
RESTORE DATABASE MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Full.bak'
WITH NORECOVERY;

-- Restore first log backup
RESTORE LOG MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Log_001.trn'
WITH NORECOVERY;

-- Restore second log backup
RESTORE LOG MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Log_002.trn'
WITH RECOVERY;
```

## Point-in-Time Restore

```sql
-- Restore to specific time
RESTORE LOG MyDatabase
FROM DISK = 'C:\Backups\MyDatabase_Log_001.trn'
WITH STOPAT = '2024-01-15 14:30:00',
     RECOVERY;
```

## Restore to Different Location

```sql
-- Restore to new location
RESTORE DATABASE MyDatabase_New
FROM DISK = 'C:\Backups\MyDatabase_Full.bak'
WITH MOVE 'MyDatabase' TO 'C:\Data\MyDatabase_New.mdf',
     MOVE 'MyDatabase_Log' TO 'C:\Logs\MyDatabase_New.ldf',
     REPLACE;
```

## Database Recovery Models

### Full Recovery Model

```sql
-- Set to full (default)
ALTER DATABASE MyDatabase SET RECOVERY FULL;

-- Requires regular log backups
BACKUP LOG MyDatabase TO DISK = 'C:\Backups\MyDatabase_Log.trn';
```

### Simple Recovery Model

```sql
-- Set to simple
ALTER DATABASE MyDatabase SET RECOVERY SIMPLE;

-- No log backups possible
-- Can only restore to last full/differential backup
```

### Bulk-Logged Recovery Model

```sql
-- Set to bulk-logged
ALTER DATABASE MyDatabase SET RECOVERY BULK_LOGGED;

-- Minimal logging for bulk operations
-- Can restore to any point within log backup
```

## Verifying Backups

```sql
-- Verify backup integrity
RESTORE VERIFYONLY 
FROM DISK = 'C:\Backups\MyDatabase_Full.bak';

-- Check header
RESTORE HEADERONLY 
FROM DISK = 'C:\Backups\MyDatabase_Full.bak';

-- List files in backup
RESTORE FILELISTONLY 
FROM DISK = 'C:\Backups\MyDatabase_Full.bak';
```

## Automated Maintenance

### Create Maintenance Plan (via SSMS or T-SQL)

```sql
-- Create backup job using SQL Agent
USE msdb;
GO

EXEC dbo.sp_add_job
    @job_name = N'DatabaseBackupJob';

EXEC dbo.sp_add_jobstep
    @job_name = N'DatabaseBackupJob',
    @step_name = N'Backup Database',
    @command = N'BACKUP DATABASE MyDatabase TO DISK = ''C:\Backups\MyDatabase.bak'' WITH INIT',
    @database_name = 'master';

EXEC dbo.sp_add_schedule
    @schedule_name = N'DailyBackup',
    @freq_type = 4,
    @freq_interval = 1,
    @freq_subday_type = 1,
    @freq_subday_interval = 0,
    @active_start_time = 020000;

EXEC dbo.sp_attach_schedule
    @job_name = N'DatabaseBackupJob',
    @schedule_name = N'DailyBackup';

EXEC dbo.sp_add_jobserver
    @job_name = N'DatabaseBackupJob';
```

## Key Points Summary

| Recovery Model | Log Backups | Point-in-Time |
|---------------|--------------|---------------|
| Full | Yes | Yes |
| Bulk-Logged | Yes | Limited |
| Simple | No | No |

---

*This topic should take about 5-7 minutes to explain in class.*
