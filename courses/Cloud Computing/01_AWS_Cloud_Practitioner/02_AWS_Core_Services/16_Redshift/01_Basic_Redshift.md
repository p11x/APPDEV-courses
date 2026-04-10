---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Redshift
Purpose: Understanding Amazon Redshift data warehouse
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Redshift.md, 03_Practical_Redshift.md
UseCase: Petabyte-scale data warehousing and analytics
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Amazon Redshift is a fully managed, petabyte-scale data warehouse service that enables organizations to analyze large volumes of data using SQL and existing business intelligence tools.

### Why Redshift Matters

- **Petabyte Scale**: Handle exabytes of data
- **Cost Effective**: Pay per queries/second
- **SQL Interface**: Standard SQL queries
- **Massively Parallel**: Multiple nodes process queries
- **Columnar Storage**: Optimized for analytics
- **Integration**: Works with BI tools

### Industry Statistics

- Powers analytics for 10,000+ companies
- Up to 10x compression with columnar storage
- 100+ PB supported in single cluster
- Integrates with 50+ BI tools

### When NOT to Use Redshift

- Real-time streaming: Use Kinesis/Kinesis Data Analytics
- Transactional workloads: Use RDS
- Small datasets (<100GB): Use RDS or Athena
- Unstructured data: Use S3 + Athena

## WHAT

### Redshift Core Concepts

**Cluster**: Core Redshift resource - collection of compute nodes.

**Node Types**:

| Type | Description | Use Case |
|------|-------------|----------|
| ra3.xlplus | Optimized storage | Cost-effective |
| ra3.16xlarge | High performance | Large datasets |
| dc2.large | Compute optimized | Performance |
| dc2.8xlarge | High compute | Heavy workloads |

**Leader Node**: Coordinates query execution, connects to client.

**Compute Nodes**: Process queries, store data locally.

**Node Slices**: Each compute node divided into slices.

### Architecture Diagram

```
                    REDSHIFT ARCHITECTURE
                    ====================

    ┌──────────────────────────────────────────────────────┐
    │                 BI TOOLS / SQL CLIENTS              │
    │  - Tableau, Power BI, Looker                         │
    │  - SQL clients, Jupyter                             │
    └──────────────────────────────────────────────────────┘
                            │
                            ▼
    ┌──────────────────────────────────────────────────────┐
    │              LEADER NODE                            │
    │  - Query planning & optimization                     │
    │  - Result aggregation                                │
    │  - Client connection handling                        │
    └──────────────────────────────────────────────────────┘
                            │
          ┌─────────────────┼─────────────────┐
          ▼                 ▼                 ▼
    ┌──────────┐     ┌──────────┐     ┌──────────┐
    │ Compute  │     │ Compute  │     │ Compute  │
    │  Node 1  │     │  Node 2  │     │  Node N  │
    │ ┌──────┐ │     │ ┌──────┐ │     │ ┌──────┐ │
    │ │Slice1│ │     │ │Slice1│ │     │ │Slice1│ │
    │ │Slice2│ │     │ │Slice2│ │     │ │Slice2│ │
    │ └──────┘ │     │ └──────┘ │     │ └──────┘ │
    └──────────┘     └──────────┘     └──────────┘
          │                 │                 │
          └─────────────────┴─────────────────┘
                            │
    ┌──────────────────────────────────────────────────────┐
    │              COLUMNAR STORAGE                       │
    │  - Compression                                       │
    │  - Zone maps (min/max indexes)                       │
    │  - 128 MB blocks                                     │
    └──────────────────────────────────────────────────────┘
```

### Key Features

| Feature | Description |
|---------|-------------|
| Columnar Storage | Optimized for analytics |
| Massively Parallel | Multiple nodes |
| Compression | Up to 10x |
| Zone Maps | Block-level indexes |
| Result Caching | Faster repeat queries |

## HOW

### Example 1: Create Redshift Cluster

```bash
# Create Redshift cluster
aws redshift create-cluster \
    --cluster-identifier my-redshift-cluster \
    --cluster-type multi-node \
    --node-type dc2.large \
    --number-of-nodes 2 \
    --master-username admin \
    --master-user-password SecretPassword123 \
    --cluster-subnet-group-name my-subnet-group \
    --vpc-security-group-ids sg-12345678 \
    --availability-zone us-east-1a

# Create single-node cluster (dev edition)
aws redshift create-cluster \
    --cluster-identifier my-dev-cluster \
    --cluster-type single-node \
    --node-type dc2.large \
    --master-username admin \
    --master-user-password SecretPassword123

# Describe cluster status
aws redshift describe-clusters \
    --cluster-identifier my-redshift-cluster

# Get connection endpoint
# "Endpoint": {"Address": "my-cluster.xxxx.us-east-1.redshift.amazonaws.com", "Port": 5439}
```

### Example 2: Connect and Query

```bash
# Connect using psql
psql -h my-cluster.xxxx.us-east-1.redshift.amazonaws.com \
     -U admin -d dev \
     -p 5439

# Create table
CREATE TABLE sales (
    sale_id INTEGER IDENTITY(1,1),
    product_id INTEGER NOT NULL,
    sale_date DATE NOT NULL,
    quantity INTEGER,
    amount DECIMAL(10,2),
    region VARCHAR(20)
);

# Insert data
INSERT INTO sales (product_id, sale_date, quantity, amount, region)
VALUES 
    (1001, '2025-01-15', 5, 250.00, 'US'),
    (1002, '2025-01-15', 3, 150.00, 'EU'),
    (1001, '2025-01-16', 10, 500.00, 'US');

# Query data
SELECT 
    product_id,
    SUM(amount) as total_sales,
    COUNT(*) as num_sales
FROM sales
WHERE sale_date >= '2025-01-01'
GROUP BY product_id
ORDER BY total_sales DESC;
```

### Example 3: Load Data from S3

```bash
# Create IAM role for Redshift
aws iam create-role \
    --role-name redshift-s3-role \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"Service": "redshift.amazonaws.com"},
            "Action": "sts:AssumeRole"
        }]
    }'

# Attach S3 read policy
aws iam attach-role-policy \
    --role-name redshift-s3-role \
    --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess

# Create external schema (for Spectrum)
CREATE EXTERNAL SCHEMA spectrum
FROM DATA CATALOG
DATABASE 'spectrum_db'
IAM_ROLE 'arn:aws:iam::123456789012:role/redshift-s3-role'
REGION 'us-east-1';

# Copy from S3
COPY sales
FROM 's3://my-bucket/data/sales/'
IAM_ROLE 'arn:aws:iam::123456789012:role/redshift-s3-region'
FORMAT AS CSV
IGNOREHEADER 1;
```

### Example 4: Configure Views and Materialized Views

```bash
# Create view
CREATE VIEW sales_summary AS
SELECT 
    product_id,
    sale_date,
    SUM(amount) as daily_sales,
    COUNT(*) as transaction_count
FROM sales
GROUP BY product_id, sale_date;

# Create materialized view (with refresh)
CREATE MATERIALIZED VIEW mv_daily_sales
BACKUP NO
SORTKEY (sale_date)
AS SELECT 
    sale_date,
    region,
    product_id,
    SUM(amount) as total_sales,
    COUNT(*) as num_sales
FROM sales
GROUP BY sale_date, region, product_id;

# Refresh materialized view
REFRESH MATERIALIZED VIEW mv_daily_sales;

# Create auto-m materialized view (auto-refresh)
CREATE MATERIALIZED VIEW mv_auto_sales
AUTO REFRESH YES
AS SELECT 
    sale_date,
    SUM(amount) as total_sales
FROM sales
GROUP BY sale_date;
```

## COMMON ISSUES

### 1. Query Performance Slow

**Problem**: Queries take too long.

**Solution**:
```sql
-- Analyze tables for query optimization
ANALYZE sales;

-- Vacuum to reclaim space and re-sort
VACUUM DELETE ONLY sales;
VACUUM SORT ONLY sales;

-- Check query queue
SELECT * FROM stl_query 
ORDER BY starttime DESC LIMIT 10;
```

### 2. Disk Space Full

**Problem**: Cluster runs out of storage.

**Solution**:
```sql
-- Check disk usage
SELECT * FROM stv_diskusage WHERE tbl = 184;

-- Analyze compression
SELECT "table", size, pct_used, empty_rows, max_blocks
FROM svv_table_info
ORDER BY size DESC;

-- Delete old data
DELETE FROM sales WHERE sale_date < '2024-01-01';
VACUUM DELETE ONLY;
```

### 3. WLM Queue Congestion

**Problem**: Queries waiting in queue.

**Solution**:
```sql
-- View WLM configuration
SELECT * FROM stl_wlm_config;

-- Set WLM queue memory
-- Via console: Redshift > Workload management

-- Monitor queue
SELECT q.queue_time, q.query_text, q.elapsed
FROM stl_query q
WHERE q.queue_time > 0
ORDER BY q.queue_time DESC;
```

### 4. Connection Issues

**Problem**: Cannot connect to cluster.

**Solution**:
```bash
# Check cluster status
aws redshift describe-clusters \
    --cluster-identifier my-cluster \
    --query 'Clusters[0].ClusterStatus'

# Verify security group
aws ec2 describe-security-groups \
    --group-id sg-12345678

# Test connectivity
telnet my-cluster.xxxx.us-east-1.redshift.amazonaws.com 5439
```

## PERFORMANCE

### Performance Characteristics

| Metric | Value |
|--------|-------|
| Max Cluster Size | 128 nodes |
| Max Storage | 100+ PB |
| Max Concurrency | 50 queries |
| Query Caching | 24 hours |
| Compression | Up to 10x |

### Performance Optimization

| Technique | Impact |
|-----------|--------|
| Sort Keys | 10-100x for range queries |
| Distribution Keys | Reduce network traffic |
| Compression Encoding | 2-4x storage savings |
| WLM | Priority queuing |
| Result Caching | Instant repeat queries |

### Cost Optimization

| Strategy | Savings |
|----------|---------|
| Pause Cluster | 100% when paused |
| Reserved Instances | Up to 75% |
| RA3 Nodes | Pay for storage only |
| Spectrum | Only pay for queries |

## COMPATIBILITY

### Region Availability

- All commercial AWS Regions
- GovCloud available
- China requires account

### Integration

| Service | Integration |
|---------|-------------|
| S3 | Load/unload data |
| DynamoDB | Query external tables |
| CloudWatch | Monitoring |
| Quicksight | Visualization |
| SageMaker | ML integration |

## CROSS-REFERENCES

### Related Services

- Athena: Serverless queries
- Spectrum: Query S3 directly
- Kinesis: Stream data
- QuickSight: Visualization

### Alternatives

| Need | Use |
|------|-----|
| Serverless DW | Athena |
| Real-time | Kinesis + Redshift |
| Small data | RDS + SQL |
| Data Lake | S3 + Spectrum |

### What to Study Next

1. Advanced Redshift: Tuning, optimization
2. Practical Redshift: ETL, best practices
3. Spectrum: Query S3 data

## EXAM TIPS

### Key Exam Facts

- Redshift = Columnar data warehouse
- RA3 nodes: Pay for compute + storage separately
- Leader node: Coordinates queries
- Compression: Automatic with ANALYZE
- Sort keys: Improve range queries

### Exam Questions

- **Question**: "OLAP workloads" = Redshift
- **Question**: "Columnar storage" = Redshift
- **Question**: "Pause/resume" = RA3 nodes
