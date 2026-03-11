# SQL Server Extended Events

## What are Extended Events?

**Extended Events** is a lightweight performance monitoring system that replaces SQL Trace. It uses minimal resources and provides detailed diagnostic information.

## Why Use Extended Events?

- **Lightweight**: Uses less CPU/memory than Trace
- **Flexible**: Many events and targets available
- **Configurable**: Filter and aggregate events
- **Powerful**: Can correlate data from multiple sources

## Extended Events Components

| Component | Description |
|-----------|-------------|
| Event | Something that happens in SQL Server |
| Action | Additional data to capture |
| Predicate | Filter conditions |
| Target | Where to send data (file, ring buffer) |

## Creating an Extended Events Session

### Basic Session

```sql
-- Create session to capture deadlocks
CREATE EVENT SESSION DeadlockCapture
ON SERVER
ADD EVENT sqlserver.xml_deadlock_report
ADD TARGET package0.event_file(
    SET filename = 'C:\XEvents\Deadlocks.xel',
    max_file_size = 10,
    max_rollover_files = 5
)
WITH (
    MAX_MEMORY = 4096 KB,
    EVENT_RETENTION_MODE = ALLOW_SINGLE_EVENT_LOSS,
    MAX_DISPATCH_LATENCY = 1 SECONDS
);
```

### Capture Slow Queries

```sql
-- Create session for slow queries
CREATE EVENT SESSION SlowQueries
ON SERVER
ADD EVENT sqlserver.sql_statement_completed(
    ACTION(sqlserver.sql_text, sqlserver.session_id)
    WHERE (duration > 1000)  -- > 1 second
)
ADD TARGET package0.ring_buffer
SET max_memory = 4096;
```

## Working with Events

### Start/Stop Session

```sql
-- Start the session
ALTER EVENT SESSION DeadlockCapture 
ON SERVER STATE = START;

-- Stop the session
ALTER EVENT SESSION DeadlockCapture 
ON SERVER STATE = STOP;
```

### View Session Data

```sql
-- Read from ring buffer
SELECT 
    event_data.value('(event/@name)[1]', 'varchar(100)') AS EventName,
    event_data.value('(event/data[@name="duration"]/value)[1]', 'bigint') AS Duration,
    event_data.value('(event/action[@name="sql_text"]/value)[1]', 'varchar(max)') AS SQLText,
    event_data.value('(event/@timestamp)[1]', 'datetime2') AS EventTime
FROM (
    SELECT CAST(event_data AS XML) AS event_data
    FROM sys.dm_xe_session_targets st
    INNER JOIN sys.dm_xe_sessions s ON st.event_session_address = s.address
    WHERE s.name = 'SlowQueries'
        AND st.target_name = 'ring_buffer'
) AS tab;
```

### Read from File

```sql
SELECT 
    event_data.value('(event/@name)[1]', 'varchar(100)') AS EventName,
    event_data.value('(event/@timestamp)[1]', 'datetime2') AS EventTime,
    event_data.value('(event/data[@name="object_name"]/value)[1]', 'varchar(100)') AS ObjectName,
    event_data.value('(event/action[@name="sql_text"]/value)[1]', 'varchar(max)') AS SQLText
FROM sys.fn_xe_file_target_read_file(
    'C:\XEvents\Deadlocks*.xel', 
    NULL, NULL, NULL
);
```

## Common Events to Capture

| Event | Purpose |
|-------|---------|
| sqlserver.sql_statement_completed | Query completion |
| sqlserver.sql_statement_starting | Query start |
| sqlserver.rpc_completed | Stored procedure completion |
| sqlserver.deadlock_graph | Deadlock information |
| sqlserver.error_reported | Error messages |
| sqlserver.wait_info | Wait statistics |

## Useful Targets

### Ring Buffer

```sql
-- Quick in-memory storage
ADD TARGET package0.ring_buffer
SET max_memory = 4096;
```

### Event File

```sql
-- Persistent file storage
ADD TARGET package0.event_file(
    SET filename = 'C:\XEvents\Trace.xel',
    max_file_size = 100,
    max_rollover_files = 5
);
```

### Histogram

```sql
-- Aggregate counts
ADD TARGET package0.histogram(
    SET source = 'sqlserver.sql_statement_completed',
    source_type = 0
);
```

## Monitoring Specific Issues

### Long-Running Queries

```sql
CREATE EVENT SESSION LongQueries
ON SERVER
ADD EVENT sqlserver.sql_statement_completed(
    ACTION(sqlserver.session_id, sqlserver.sql_text)
    WHERE duration > 10000  -- 10 seconds
)
ADD TARGET package0.event_file(
    SET filename = 'C:\XEvents\LongQueries.xel'
)
WITH (MAX_DISPATCH_LATENCY = 1);
```

### Blocking Issues

```sql
CREATE EVENT SESSION Blocking
ON SERVER
ADD EVENT sqlserver.xml_deadlock_report
ADD EVENT sqlserver.blocked_process_report
ADD TARGET package0.event_file(
    SET filename = 'C:\XEvents\Blocking.xel'
);
```

## Managing Extended Events

```sql
-- View all sessions
SELECT * FROM sys.dm_xe_sessions;

-- View session events
SELECT * FROM sys.dm_xe_session_events;

-- Drop session
DROP EVENT SESSION DeadlockCapture ON SERVER;
```

## Key Points Summary

| Concept | Description |
|---------|-------------|
| Event | Occurrence to track |
| Action | Additional data captured |
| Predicate | Filter conditions |
| Target | Output destination |
| Session | Collection of events/targets |

---

*This topic should take about 5-7 minutes to explain in class.*
