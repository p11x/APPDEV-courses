---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Redshift Practical
Purpose: Practical Redshift implementation, ETL patterns, and best practices
Difficulty: practical
Prerequisites: 01_Basic_Redshift.md, 02_Advanced_Redshift.md
RelatedFiles: 01_Basic_Redshift.md, 02_Advanced_Redshift.md
UseCase: Production data warehouse implementation
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Practical Redshift implementation involves real-world ETL patterns, data loading strategies, and operational best practices. This knowledge is essential for building production-grade analytics platforms.

### Why Practical Implementation Matters

- **Reliability**: Production-ready patterns
- **Efficiency**: Optimize data pipelines
- **Cost**: Reduce compute and storage costs
- **Governance**: Data quality and lineage
- **Automation**: Repeatable processes

### Common Production Use Cases

- **Daily ETL**: Nightly data loads
- **CDC**: Change data capture from databases
- **Streaming**: Real-time data ingestion
- **Reporting**: Daily/weekly reports
- **ML Pipelines**: Feature engineering

## WHAT

### ETL Patterns

| Pattern | Description | Use Case |
|---------|-------------|----------|
| Full Load | Load all data | Initial load |
| Incremental | Load new data only | Daily ETL |
| CDC | Capture changes | Real-time |
| Upsert | Insert/update | Deduplication |
| Slowly Changing Dimensions | Track changes | Type 2 history |

### Data Loading Best Practices

1. **Parallel Loading**: Use multiple files
2. **Compression**: Use GZIP/SNAPPY
3. **Proper Format**: Use PARQUET/ORC
4. **Batch Size**: Optimal 1-10MB per slice
5. **Sort Order**: Pre-sort by sort key

## HOW

### Example 1: Daily ETL Pipeline

```sql
-- Create staging table
CREATE TABLE sales_staging (
    sale_id INTEGER,
    product_id INTEGER,
    customer_id INTEGER,
    sale_date DATE,
    quantity INTEGER,
    amount DECIMAL(10,2),
    store_id INTEGER
);

-- Create target table
CREATE TABLE sales_fact (
    sale_id BIGINT IDENTITY(0,1),
    product_id INTEGER NOT NULL,
    customer_id INTEGER NOT NULL,
    sale_date DATE NOT NULL,
    quantity INTEGER,
    amount DECIMAL(10,2),
    store_id INTEGER NOT NULL,
    loaded_at TIMESTAMP DEFAULT GETDATE()
)
SORTKEY(sale_date);

-- ETL procedure
CREATE OR REPLACE PROCEDURE sp_etl_daily_sales()
AS $$
BEGIN
    -- Clear staging
    TRUNCATE sales_staging;
    
    -- Load from S3
    COPY sales_staging
    FROM 's3://etl-bucket/daily/sales/'
    IAM_ROLE 'arn:aws:iam::123456789012:role/redshift-etl-role'
    FORMAT AS CSV
    IGNOREHEADER 1;
    
    -- Insert incremental data
    INSERT INTO sales_fact (
        product_id, customer_id, sale_date,
        quantity, amount, store_id
    )
    SELECT 
        product_id, customer_id, sale_date,
        quantity, amount, store_id
    FROM sales_staging s
    WHERE NOT EXISTS (
        SELECT 1 FROM sales_fact f
        WHERE f.sale_date = s.sale_date
        AND f.product_id = s.product_id
    );
    
    -- Update stats
    ANALYZE sales_fact;
END;
$$ LANGUAGE plpgsql;

-- Execute daily
CALL sp_etl_daily_sales();
```

### Example 2: CDC (Change Data Capture)

```sql
-- Enable change tracking
ALTER TABLE source_db.orders ADD COLUMN cdc_synced BOOLEAN DEFAULT FALSE;

-- Create CDC table
CREATE TABLE orders_cdc (
    operation VARCHAR(10),
   cdc_timestamp TIMESTAMP,
    order_id INTEGER,
    customer_id INTEGER,
    total_amount DECIMAL(10,2),
    status VARCHAR(20)
);

-- Load changes from S3 (from database CDC export)
COPY orders_cdc
FROM 's3://cdc-bucket/orders/'
IAM_ROLE 'arn:aws:iam::123456789012:role/redshift-etl-role'
FORMAT AS JSON 'auto';

-- Process CDC operations
CREATE OR REPLACE PROCEDURE sp_process_cdc()
AS $$
BEGIN
    -- Handle inserts
    INSERT INTO orders_target (order_id, customer_id, total_amount, status, updated_at)
    SELECT order_id, customer_id, total_amount, status, cdc_timestamp
    FROM orders_cdc
    WHERE operation = 'INSERT'
    AND NOT EXISTS (
        SELECT 1 FROM orders_target WHERE order_id = orders_cdc.order_id
    );
    
    -- Handle updates
    UPDATE orders_target t
    SET 
        customer_id = c.customer_id,
        total_amount = c.total_amount,
        status = c.status,
        updated_at = c.cdc_timestamp
    FROM orders_cdc c
    WHERE t.order_id = c.order_id
    AND c.operation = 'UPDATE';
    
    -- Handle deletes
    DELETE FROM orders_target t
    USING orders_cdc c
    WHERE t.order_id = c.order_id
    AND c.operation = 'DELETE';
    
    -- Clear CDC table
    TRUNCATE orders_cdc;
END;
$$ LANGUAGE plpgsql;
```

### Example 3: Slowly Changing Dimensions (Type 2)

```sql
-- Create dimension table with SCD Type 2
CREATE TABLE customer_dim (
    customer_key BIGINT IDENTITY(0,1),
    customer_id INTEGER NOT NULL,
    customer_name VARCHAR(100),
    email VARCHAR(100),
    region VARCHAR(50),
    effective_date DATE,
    expiration_date DATE,
    is_current BOOLEAN DEFAULT TRUE
)
SORTKEY(customer_id);

-- Create procedure for SCD Type 2
CREATE OR REPLACE PROCEDURE sp_scd_type2()
AS $$
BEGIN
    -- Handle new records and changes
    INSERT INTO customer_dim (
        customer_id, customer_name, email, region,
        effective_date, expiration_date, is_current
    )
    SELECT 
        s.customer_id,
        s.customer_name,
        s.email,
        s.region,
        CURRENT_DATE,
        NULL,
        TRUE
    FROM staging_customers s
    LEFT JOIN customer_dim c ON (
        s.customer_id = c.customer_id
        AND c.is_current = TRUE
    )
    WHERE c.customer_id IS NULL
    OR (
        c.customer_id IS NOT NULL
        AND (
            c.customer_name != s.customer_name
            OR c.email != s.email
            OR c.region != s.region
        )
    );
    
    -- Expire old records
    UPDATE customer_dim c
    SET is_current = FALSE,
        expiration_date = CURRENT_DATE - 1
    FROM staging_customers s
    WHERE c.customer_id = s.customer_id
    AND c.is_current = TRUE
    AND (
        c.customer_name != s.customer_name
        OR c.email != s.email
        OR c.region != s.region
    );
    
    -- Update current record if no changes
    UPDATE customer_dim c
    SET customer_name = s.customer_name,
        email = s.email,
        region = s.region
    FROM staging_customers s
    WHERE c.customer_id = s.customer_id
    AND c.is_current = TRUE
    AND c.customer_name = s.customer_name
    AND c.email = s.email
    AND c.region = s.region;
END;
$$ LANGUAGE plpgsql;
```

### Example 4: Automate Maintenance

```sql
-- Create maintenance procedure
CREATE OR REPLACE PROCEDURE sp_maintenance()
AS $$
BEGIN
    -- Vacuum deleted rows
    VACUUM DELETE ONLY;
    
    -- Re-sort if needed
    VACUUM SORT ONLY;
    
    -- Analyze compression
    ANALYZE COMPRESSION;
    
    -- Refresh materialized views
    REFRESH MATERIALIZED VIEW mv_daily_sales;
    
    -- Clear old query logs
    DELETE FROM stl_query 
    WHERE starttime < GETDATE() - 30;
    
    -- Clear old logs
    DELETE FROM stl_connection_log
    WHERE starttime < GETDATE() - 7;
END;
$$ LANGUAGE plpgsql;

-- Schedule via cron (external)
-- 0 6 * * * aws redshift execute-statement --cluster-identifier my-cluster --sql "CALL sp_maintenance()"
```

## COMMON ISSUES

### 1. ETL Timeout

**Problem**: COPY command times out.

**Solution**:
```sql
-- Increase statement timeout
SET statement_timeout = '1h';

-- Split files for parallel loading
-- Use multiple S3 prefixes

-- Use larger batch size
COPY sales FROM 's3://bucket/'
    IAM_ROLE 'role'
    COMPUPDATE OFF
    STATUPDATE OFF;
```

### 2. Data Quality Issues

**Problem**: Invalid data causes failures.

**Solution**:
```sql
-- Add rejected records table
CREATE TABLE sales_rejected (
    raw_data VARCHAR(65535),
    load_time TIMESTAMP DEFAULT GETDATE()
);

-- Use MAXERROR option
COPY sales FROM 's3://bucket/'
    IAM_ROLE 'role'
    MAXERROR 100;

-- Log rejected records
-- In application, capture and log errors
```

### 3. Long Vacuum Times

**Problem**: VACUUM takes too long.

**Solution**:
```sql
-- Use DELETE ONLY for large tables
VACUUM DELETE ONLY sales_fact;

-- Analyze first to see if sort needed
SELECT "table", size, empty_rows
FROM svv_table_info;

-- Schedule during maintenance window
-- Use smaller batch sizes
```

## PERFORMANCE

### ETL Optimization

| Technique | Impact |
|-----------|--------|
| Multiple files | Parallel loading |
| COMPUPDATE OFF | Skip compression analysis |
| STATUPDATE OFF | Skip statistics |
| GZIP compression | 80% size reduction |
| Proper sort key | Faster queries |

### Batch Sizing

| Node Type | Optimal File Size |
|-----------|-------------------|
| dc2.large | 1-10 MB |
| dc2.8xlarge | 10-50 MB |
| ra3.16xlarge | 50-100 MB |

## COMPATIBILITY

### Supported Formats

| Format | Compression | Notes |
|--------|-------------|-------|
| CSV | Optional | Default |
| PARQUET | Auto | Recommended |
| ORC | Auto | Good alternative |
| AVRO | None | Schema support |
| JSON | Optional | Flexible |

### Integration

- S3: Primary data source
- DynamoDB: NoSQL integration
- Database: CDC from RDS
- Kinesis: Streaming data

## CROSS-REFERENCES

### Related Patterns

- Staging: Intermediate storage
- Incremental Load: Delta updates
- SCD Type 2: Historical tracking

### What to Study Next

1. Redshift Spectrum: Query S3 directly
2. Lake House: Delta Lake integration
3. Real-time: Kinesis + Redshift

## EXAM TIPS

### Key Exam Facts

- COPY: Load from S3
- UNLOAD: Export to S3
- SCD Type 2: Historical dimensions
- VACUUM: Reclaim storage
- ANALYZE: Update statistics

### Exam Questions

- **Question**: "Load from S3" = COPY command
- **Question**: "Track history" = SCD Type 2
- **Question**: "Nightly ETL" = Scheduled procedure
