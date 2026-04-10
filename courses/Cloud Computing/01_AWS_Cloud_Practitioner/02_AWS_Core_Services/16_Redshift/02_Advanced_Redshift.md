---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Redshift Advanced
Purpose: Advanced Redshift configurations, optimization, and architecture
Difficulty: advanced
Prerequisites: 01_Basic_Redshift.md
RelatedFiles: 01_Basic_Redshift.md, 03_Practical_Redshift.md
UseCase: Production data warehousing with performance optimization
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Advanced Redshift configurations enable production-grade data warehousing with optimal performance, cost efficiency, and scalability. Understanding these concepts is essential for building enterprise-grade analytics platforms.

### Why Advanced Configuration Matters

- **Performance**: 10-100x query speed improvements
- **Cost**: Reduce infrastructure costs by 50%+
- **Scalability**: Handle petabyte-scale workloads
- **Reliability**: Maintain 99.9% availability
- **Security**: Encryption, VPC, compliance

### Advanced Use Cases

- **ETL Pipelines**: Data transformation and loading
- **Real-time Analytics**: Live dashboards
- **ML Feature Store**: Machine learning data prep
- **Financial Reporting**: Complex aggregations
- **Log Analysis**: Large-scale log processing

## WHAT

### Distribution Keys

**KEY Distribution**: Distribute by specific column (e.g., user_id).

**ALL Distribution**: Replicate table to all nodes.

**EVEN Distribution**: Round-robin distribution.

| Type | Best For | Example |
|------|----------|---------|
| KEY | Frequently joined tables | user_id, order_id |
| ALL | Small dimension tables | lookup tables |
| EVEN | No clear join pattern | fact tables |

### Sort Keys

**Compound Sort Key**: Sort by multiple columns in order.

**Interleaved Sort Key**: Equal weight to all columns.

| Type | Best For | Example |
|------|----------|---------|
| Compound | Range filters first | date range queries |
| Interleaked | Equality filters | complex ad-hoc queries |

### WLM (Workload Management)

```
    WLM ARCHITECTURE
    ===============

    Queries ──▶ Queue 1 (ETL) ──▶ Slots
              ──▶ Queue 2 (Ad-hoc) ──▶ Slots
              ──▶ Queue 3 (Reporting) ──▶ Slots
```

### Concurrency Scaling

- Auto-adds clusters for concurrent queries
- Pays per second used
- Enabled per queue

## HOW

### Example 1: Design Table Schema

```sql
-- Create sales table with optimized distribution
CREATE TABLE sales_fact (
    sale_id BIGINT IDENTITY(0,1),
    product_id INTEGER NOT NULL,
    customer_id INTEGER NOT NULL,
    sale_date DATE NOT NULL,
    quantity INTEGER,
    amount DECIMAL(10,2),
    store_id INTEGER NOT NULL
)
DISTKEY(customer_id)
SORTKEY(sale_date);

-- Create dimension table
CREATE TABLE product_dim (
    product_id INTEGER PRIMARY KEY,
    product_name VARCHAR(100),
    category VARCHAR(50),
    price DECIMAL(10,2),
    supplier_id INTEGER
)
DISTKEY(product_id);

-- Create small lookup table (ALL distribution)
CREATE TABLE region_lookup (
    region_id INTEGER PRIMARY KEY,
    region_name VARCHAR(50),
    country VARCHAR(50)
)
DISTSTYLE ALL;

-- Create interleaved sort key table
CREATE TABLE events (
    event_id BIGINT IDENTITY(0,1),
    event_type VARCHAR(50),
    user_id INTEGER,
    event_timestamp TIMESTAMP,
    properties JSON
)
SORTKEY (event_type, user_id, event_timestamp);
```

### Example 2: Configure WLM

```sql
-- Create WLM queue configuration
CREATE WLM Q WITH (
    queue_name: 'etl_queue',
    priority: 10,
    user_group: ['etl_users'],
    query_group: ['etl'],
    memory_percent: 40,
    max_query_runtime: 3600,
    concurrent_query_limit: 5
);

-- Create reporting queue
CREATE WLM Q WITH (
    queue_name: 'reporting_queue',
    priority: 5,
    user_group: ['analysts'],
    memory_percent: 30,
    max_query_runtime: 7200
);

-- Set query group
SET query_group TO 'etl';
SELECT * FROM large_table;
```

### Example 3: Materialized Views and late-binding views

```sql
-- Create late-binding view (useful for Spectrum)
CREATE VIEW sales_summary_v AS
SELECT 
    s.sale_date,
    p.product_name,
    p.category,
    SUM(s.amount) as total_sales,
    COUNT(*) as num_sales
FROM sales_fact s
JOIN product_dim p ON s.product_id = p.product_id
WHERE s.sale_date >= '2025-01-01'
WITH NO SCHEMA BINDING;

-- Create auto-refresh materialized view
CREATE MATERIALIZED VIEW mv_daily_sales
AUTO REFRESH YES
AS SELECT 
    sale_date,
    region_id,
    product_id,
    SUM(amount) as daily_sales,
    COUNT(*) as transaction_count,
    AVG(amount) as avg_amount
FROM sales_fact
GROUP BY sale_date, region_id, product_id;

-- Create nested materialized view
CREATE MATERIALIZED VIEW mv_category_summary
AS SELECT 
    category,
    sale_date,
    SUM(total_sales) as category_sales
FROM mv_daily_sales mv
JOIN product_dim p ON mv.product_id = p.product_id
GROUP BY category, sale_date;
```

### Example 4: Cross-Account Spectrum

```bash
# Create role for cross-account access
aws iam create-role \
    --role-name redshift-cross-account-role \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"AWS": "arn:aws:iam::OTHER_ACCOUNT:root"},
            "Action": "sts:AssumeRole"
        }]
    }'

# Attach S3 permissions
aws iam put-role-policy \
    --role-name redshift-cross-account-role \
    --policy-name s3-access \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": ["s3:GetObject", "s3:ListBucket"],
            "Resource": ["arn:aws:s3:::source-bucket", "arn:aws:s3:::source-bucket/*"]
        }]
    }'

# Create external schema
CREATE EXTERNAL SCHEMA cross_account_data
FROM DATA CATALOG
DATABASE 'cross_db'
IAM_ROLE 'arn:aws:iam::123456789012:role/redshift-cross-account-role'
REGION 'us-east-1';

# Query cross-account data
SELECT * FROM cross_account_data.external_table LIMIT 10;
```

## COMMON ISSUES

### 1. Skewed Distribution

**Problem**: Uneven data distribution across nodes.

**Solution**:
```sql
-- Check distribution skew
SELECT 
    slice, 
    num_values, 
    minvalue, 
    maxvalue
FROM stv_block_usage
WHERE tbl = (SELECT oid FROM pg_class WHERE relname = 'sales_fact')
ORDER BY slice;

-- Re-distribute with different key
ALTER TABLE sales_fact ALTER DISTKEY customer_id;

-- Or use EVEN distribution
ALTER TABLE sales_fact ALTER DISTSTYLE EVEN;
```

### 2. Long Running Queries

**Problem**: Queries take hours.

**Solution**:
```sql
-- Enable concurrency scaling
ALTER SYSTEM SET wlm_concurrencyScaling = 1;

-- Increase memory per query
-- Via WLM configuration

-- Optimize with sort keys
ALTER TABLE sales_fect ALTER SORTKEY (sale_date);

-- Use result caching
SET enable_result_cache = 1;
```

### 3. Storage Management

**Problem**: Running out of storage.

**Solution**:
```sql
-- Check storage usage
SELECT 
    schema, 
    "table", 
    size, 
    pct_used
FROM svv_table_info
ORDER BY size DESC;

-- Analyze and compress
ANALYZE COMPRESS;

-- Vacuum to reclaim space
VACUUM (SORT ONLY);

-- Archive old data to S3
UNLOAD ('SELECT * FROM sales WHERE sale_date < ''2023-01-01''')
TO 's3://archive-bucket/sales/2022/'
IAM_ROLE 'arn:aws:iam::123456789012:role/redshift-s3-role'
PARQUET;
```

## PERFORMANCE

### Performance Metrics

| Metric | Target | Alert |
|--------|--------|-------|
| Query Runtime | <30s | >5min |
| Queue Time | <5s | >60s |
| CPU | 70-90% | >95% |
| Disk Usage | <70% | >85% |
| Memory | <80% | >95% |

### Monitoring Queries

```sql
-- Top slow queries
SELECT 
    query,
    query_text,
    starttime,
    elapsed,
    rows
FROM stl_query
WHERE starttime > GETDATE() - INTERVAL '1 day'
ORDER BY elapsed DESC
LIMIT 10;

-- Query planning
EXPLAIN SELECT * FROM sales_fact WHERE sale_date = '2025-01-01';
```

## COMPATIBILITY

### Cross-Platform Comparison

| Feature | AWS Redshift | Azure Synapse | GCP BigQuery |
|---------|--------------|---------------|--------------|
| Serverless | RA3 | Yes | Yes |
| Auto-scaling | Concurrency Scaling | Yes | Yes |
| Storage/Compute | RA3 separate | Separate | Separate |
| Multi-zone | Yes | Yes | Yes |
| SQL | Yes | Yes | Yes |

### Supported Formats

| Format | Use Case |
|--------|----------|
| PARQUET | Columnar, compressed |
| ORC | Hadoop integration |
| CSV | Legacy data |
| JSON | Semi-structured |
| AVRO | Streaming data |

## CROSS-REFERENCES

### Related Services

- Spectrum: Query S3 directly
- Athena: Serverless queries
- Kinesis: Real-time data
- QuickSight: Visualization

### Prerequisites

- Basic Redshift concepts
- SQL knowledge
- S3 basics

### What to Study Next

1. Practical Redshift: ETL patterns
2. Spectrum: External tables
3. Data Lake: Lake House architecture

## EXAM TIPS

### Key Exam Facts

- Distribution keys: Reduce network transfer
- Sort keys: Improve range queries
- WLM: Manage query priorities
- Compression: ANALYZE for optimization
- Vacuum: Reclaim space

### Exam Questions

- **Question**: "Join optimization" = Distribution keys
- **Question**: "Date range queries" = Sort keys
- **Question**: "Query priority" = WLM queues
