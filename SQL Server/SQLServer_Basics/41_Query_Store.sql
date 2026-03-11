-- =====================================================
-- SQL Server Query Store
-- =====================================================
-- Query Store captures query performance data over time

-- =====================================================
-- Enable Query Store
-- =====================================================

ALTER DATABASE MyDatabase
SET QUERY_STORE = ON
WITH (
    OPERATION_MODE = READ_WRITE,
    CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30),
    DATA_FLUSH_INTERVAL_SECONDS = 60,
    MAX_STORAGE_SIZE_MB = 100,
    INTERVAL_LENGTH_MINUTES = 1
);
GO


-- =====================================================
-- Query Store Views
-- =====================================================

-- View top resource-consuming queries
SELECT 
    qsq.query_id,
    qsq.query_text_id,
    qrs.count_executions,
    qrs.avg_duration,
    qrs.avg_cpu_time,
    qrs.avg_logical_io_reads,
    qsqt.query_sql_text
FROM sys.query_store_query qsq
INNER JOIN sys.query_store_runtime_stats qrs 
    ON qsq.query_id = qrs.query_id
INNER JOIN sys.query_store_query_text qsqt 
    ON qsq.query_text_id = qsqt.query_text_id
ORDER BY qrs.avg_cpu_time DESC;
GO


-- =====================================================
-- Find Queries with Regressed Performance
-- =====================================================

-- Compare recent vs older performance
SELECT 
    qsq.query_id,
    qsqt.query_sql_text,
    rs1.avg_duration AS avg_duration_recent,
    rs2.avg_duration AS avg_duration_older,
    rs1.avg_duration - rs2.avg_duration AS duration_increase
FROM sys.query_store_query qsq
CROSS APPLY (
    SELECT runtime_stats_id, avg_duration
    FROM sys.query_store_runtime_stats
    WHERE query_id = qsq.query_id
    ORDER BY execution_type_desc, last_execution_time DESC
    OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
) rs1
CROSS APPLY (
    SELECT runtime_stats_id, avg_duration
    FROM sys.query_store_runtime_stats
    WHERE query_id = qsq.query_id
    ORDER BY execution_type_desc, last_execution_time ASC
    OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
) rs2
INNER JOIN sys.query_store_query_text qsqt 
    ON qsq.query_text_id = qsqt.query_text_id
WHERE rs1.avg_duration > rs2.avg_duration * 1.5
ORDER BY duration_increase DESC;
GO


-- =====================================================
-- Force a Plan
-- =====================================================

-- Find query and its plans
SELECT 
    qsq.query_id,
    qsp.plan_id,
    qsp.is_forced_plan,
    qsp.avg_duration,
    qsp.avg_cpu_time
FROM sys.query_store_query qsq
INNER JOIN sys.query_store_plan qsp 
    ON qsq.query_id = qsp.query_id
WHERE query_text_id IN (
    SELECT query_text_id 
    FROM sys.query_store_query_text
    WHERE query_sql_text LIKE '%SELECT * FROM Orders%'
);

-- Force a specific plan
EXEC sp_query_store_force_plan 
    @query_id = 1,  -- Replace with actual query_id
    @plan_id = 1;    -- Replace with actual plan_id
GO


-- =====================================================
-- Unforce a Plan
-- =====================================================

EXEC sp_query_store_unforce_plan 
    @query_id = 1,
    @plan_id = 1;
GO


-- =====================================================
-- Clear Query Store Data
-- =====================================================

-- Clear runtime stats
ALTER DATABASE MyDatabase 
SET QUERY_STORE CLEAR ALL;

-- Clear specific query
EXEC sp_query_store_remove_query @query_id = 1;

-- Clear specific plan
EXEC sp_query_store_remove_plan @plan_id = 1;


-- =====================================================
-- Query Store Options
-- =====================================================

-- View current settings
SELECT 
    name AS OptionName,
    value,
    value_for_secondary
FROM sys.database_settings
WHERE name LIKE '%query_store%';
GO


-- =====================================================
-- Wait Statistics Analysis
-- =====================================================

-- Query store wait statistics
SELECT 
    wait_category_desc,
    total_query_wait_time_avg_ms,
    query_wait_stats_row_number
FROM sys.query_store_wait_stats
ORDER BY total_query_wait_time_avg_ms DESC;
GO


-- =====================================================
-- Automatic Tuning
-- =====================================================

-- Enable automatic plan correction
ALTER DATABASE MyDatabase
SET AUTOMATIC_TUNING (FORCE_LAST_GOOD_PLAN = ON);
GO


-- =====================================================
-- Query Store Reports
-- =====================================================

-- Regressed queries report
SELECT 
    query_sql_text,
    sum(count_executions) AS total_executions,
    avg_duration,
    avg_cpu_time,
    avg_logical_io_reads
FROM sys.query_store_query qsq
INNER JOIN sys.query_store_runtime_stats qrs 
    ON qsq.query_id = qrs.query_id
INNER JOIN sys.query_store_query_text qsqt 
    ON qsq.query_text_id = qsqt.query_text_id
WHERE query_sql_text NOT LIKE '%sp_help%'
GROUP BY query_sql_text, avg_duration, avg_cpu_time, avg_logical_io_reads
ORDER BY avg_cpu_time DESC;
GO


-- =====================================================
-- Trending Analysis
-- =====================================================

-- Query performance over time
SELECT 
    DATEADD(minute, (qrs.last_execution_time - '20100101'), 0) AS time_bucket,
    AVG(qrs.avg_duration) AS avg_duration,
    AVG(qrs.avg_cpu_time) AS avg_cpu_time,
    COUNT(*) AS execution_count
FROM sys.query_store_query qsq
INNER JOIN sys.query_store_runtime_stats qrs 
    ON qsq.query_id = qrs.query_id
WHERE qsq.query_id = 1
GROUP BY DATEADD(minute, (qrs.last_execution_time - '20100101'), 0)
ORDER BY time_bucket;
GO


-- =====================================================
-- Key Points:
-- =====================================================
/*
1. Enable Query Store to capture query performance history
2. Query Store helps identify regressed queries
3. Force plans to improve performance
4. Automatic tuning can use last good plan
5. Clear old data to manage storage
6. Analyze wait statistics for bottlenecks
*/
