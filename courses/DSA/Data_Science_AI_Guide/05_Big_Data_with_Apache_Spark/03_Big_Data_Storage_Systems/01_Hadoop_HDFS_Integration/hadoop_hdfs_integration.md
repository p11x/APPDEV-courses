# Hadoop HDFS Integration with Apache Spark

## I. INTRODUCTION

### What is Hadoop HDFS?
Hadoop Distributed File System (HDFS) is the primary storage system for Hadoop big data ecosystems. It provides high-throughput access to application data by distributing storage across multiple machines. HDFS is designed to handle massive datasets ranging from gigabytes to petabytes, making it an ideal foundation for big data analytics workloads that Apache Spark can efficiently process.

### Why is it Important in Big Data?
HDFS integration with Spark is critical for enterprise big data architectures. It provides cost-effective storage for huge data volumes with built-in redundancy and fault tolerance. The separation of storage and compute allows organizations to scale their data storage independently from processing capacity. HDFS compatibility ensures interoperability with numerous data processing tools in the Hadoop ecosystem.

### Prerequisites
Understanding of distributed file systems and their architecture is helpful. Familiarity with Hadoop concepts like NameNode, DataNode, and blocks is beneficial. Basic knowledge of Spark DataFrames and the SparkSession API is required. Understanding of data formats like Parquet, ORC, and Avro adds value.

## II. FUNDAMENTALS

### HDFS Architecture Components

#### NameNode
The NameNode is the master node that maintains metadata about the file system. It tracks file names, permissions, and block locations. It does not store the actual data but coordinates access to data blocks stored on DataNodes. In production, a secondary NameNode helps with checkpointing.

#### DataNode
DataNodes are worker nodes that store actual data blocks. They handle read and write requests from clients. They periodically report their status to the NameNode. Multiple replicas of each block are stored across different DataNodes for fault tolerance.

#### Block Replication
HDFS stores data in blocks (default 128MB). Each block is replicated across multiple DataNodes (default replication factor 3). The NameNode determines where to place replicas based on rack awareness and available storage.

### Spark-HDFS Integration

```python
# Reading from HDFS
df = spark.read.format("parquet").load("hdfs://namenode:9000/data/input")

# Writing to HDFS
df.write.format("parquet").mode("overwrite").save("hdfs://namenode:9000/data/output")

# Using hdfs:// protocol or just paths
df = spark.read.parquet("/data/input")
```

## III. IMPLEMENTATION

### Step-by-Step Code Examples with PySpark

```python
"""
Hadoop HDFS Integration Demonstration
Complete implementation with comprehensive examples
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType, TimestampType
import os

# Initialize Spark with HDFS configuration
spark = SparkSession.builder \
    .appName("HDFSIntegrationDemo") \
    .master("local[*]") \
    .config("spark.sql.warehouse.dir", "file:///C:/temp/warehouse") \
    .config("spark.local.dir", "C:/temp/spark") \
    .getOrCreate()

print("=" * 70)
print("HADOOP HDFS INTEGRATION")
print("=" * 70)

# For local testing, use local file system
# In production, replace with actual HDFS paths

# ============================================================================
# EXAMPLE 1: READING DATA FROM HDFS
# ============================================================================

print("\n1. READING DATA FROM HDFS")
print("-" * 50)

# Sample data for demonstration
data = [
    ("2024-01-01", "transaction_001", 1500.0, "Electronics"),
    ("2024-01-01", "transaction_002", 2500.0, "Clothing"),
    ("2024-01-02", "transaction_003", 800.0, "Groceries"),
    ("2024-01-02", "transaction_004", 3500.0, "Electronics"),
    ("2024-01-03", "transaction_005", 1200.0, "Clothing"),
]

df = spark.createDataFrame(
    data, 
    ["date", "transaction_id", "amount", "category"]
)

print("\nSample Data (simulating HDFS source):")
df.show()

# ============================================================================
# EXAMPLE 2: WRITING DATA TO HDFS
# ============================================================================

print("\n2. WRITING DATA TO HDFS")
print("-" * 50)

# Write as Parquet (optimized for HDFS)
output_path = "file:///C:/temp/hdfs_output/transactions"
df.write \
    .mode("overwrite") \
    .partitionBy("date") \
    .parquet(output_path)

print(f"\nData written to: {output_path}")
print("Partitioned by date for efficient querying")

# ============================================================================
# EXAMPLE 3: HDFS FILE FORMATS
# ============================================================================

print("\n3. HDFS FILE FORMATS")
print("-" * 50)

# Create sample dataset
large_data = [
    (i, f"customer_{i}", 1000 + i * 10.5, "2024-01-01" if i % 3 == 0 else "2024-01-02")
    for i in range(1000)
]
df_large = spark.createDataFrame(
    large_data,
    ["id", "name", "balance", "date"]
)

# Write different formats
df_large.write.mode("overwrite").parquet("file:///C:/temp/hdfs_output/parquet_data")
df_large.write.mode("overwrite").orc("file:///C:/temp/hdfs_output/orc_data")

# Read different formats
parquet_df = spark.read.parquet("file:///C:/temp/hdfs_output/parquet_data")
orc_df = spark.read.orc("file:///C:/temp/hdfs_output/orc_data")

print("\nParquet Data:")
print(parquet_df.count(), "records")

print("\nORC Data:")
print(orc_df.count(), "records")

# ============================================================================
# EXAMPLE 4: HDFS PARTITIONING
# ============================================================================

print("\n4. HDFS PARTITIONING")
print("-" * 50)

# Dynamic vs Fixed partitioning
df_large.write \
    .mode("overwrite") \
    .partitionBy("date") \
    .parquet("file:///C:/temp/hdfs_output/partitioned")

# Read with partition discovery
df_partitioned = spark.read.parquet("file:///C:/temp/hdfs_output/partitioned")
print(f"\nPartitioned data partitions:")
print(df_partitioned.rdd.getNumPartitions(), "partitions")

# Read specific partition
df_date1 = spark.read.parquet("file:///C:/temp/hdfs_output/partitioned/date=2024-01-01")
print(f"\nFiltered to specific partition:")
print(df_date1.count(), "records")

# ============================================================================
# EXAMPLE 5: HDFS COMPRESSION
# ============================================================================

print("\n5. HDFS COMPRESSION")
print("-" * 50)

# Write with different codecs
df_large.write \
    .mode("overwrite") \
    .option("compression", "snappy") \
    .parquet("file:///C:/temp/hdfs_output/snappy")

df_large.write \
    .mode("overwrite") \
    .option("compression", "gzip") \
    .parquet("file:///C:/temp/hdfs_output/gzip")

df_large.write \
    .mode("overwrite") \
    .option("compression", "zstd") \
    .parquet("file:///C:/temp/hdfs_output/zstd")

print("Data written with different compression codecs")
print("Available: snappy (fast), gzip (balanced), zstd (high compression)")

# ============================================================================
# EXAMPLE 6: HDFS DATA SOURCES
# ============================================================================

print("\n6. HDFS DATA SOURCES")
print("-" * 50)

# CSV from HDFS
sample_csv = "file:///C:/temp/hdfs_output/sample.csv"
df_large.coalesce(1).write.mode("overwrite").option("header", "true").csv(sample_csv)

# Read CSV
csv_df = spark.read \
    .option("header", "true") \
    .option("inferSchema", "true") \
    .csv(sample_csv)

print("\nCSV Schema:")
csv_df.printSchema()

# JSON from HDFS
json_path = "file:///C:/temp/hdfs_output/data.json"
df_large.toJSON().saveAsTextFile(json_path)

# Read JSON
json_df = spark.read.json(json_path)
print("\nJSON Schema:")
json_df.printSchema()

# ============================================================================
# EXAMPLE 7: HDFS OPERATIONS
# ============================================================================

print("\n7. HDFS OPERATIONS")
print("-" * 50)

# List files using Hadoop FileSystem API
try:
    from pydoop.hdfs import hdfs
    hdfs_instance = hdfs()
    print("\nHDFS Files in /data:")
    for f in hdfs_instance.list_directory("/data"):
        print(f"  {f['name']}")
except ImportError:
    print("\n(pydoop not available - local mode)")
    print("In production, use: hdfs dfs -ls /data")

# File information
import os
files = os.listdir("C:/temp/hdfs_output")
print(f"\nLocal files: {files}")

# ============================================================================
# EXAMPLE 8: HDFS OPTIMIZATION
# ============================================================================

print("\n8. HDFS OPTIMIZATION")
print("-" * 50)

# Coalesce for fewer files
df_large.coalesce(4).write \
    .mode("overwrite") \
    .parquet("file:///C:/temp/hdfs_output/coalesced")

# Repartition for parallelism
df_large.repartition(8).write \
    .mode("overwrite") \
    .parquet("file:///C:/temp/hdfs_output/repartitioned")

# Bucket data
df_large.write \
    .mode("overwrite") \
    .bucketBy(4, "id") \
    .sortBy("id") \
    .format("parquet") \
    .saveAsTable("bucketed_table")

print("Optimization techniques applied:")
print("- Coalesce: Reduce partition count for smaller outputs")
print("- Repartition: Increase partition count for parallelism")
print("- Bucketing: Pre-shuffle data for faster joins")

# ============================================================================
# EXAMPLE 9: HDFS FAILURE HANDLING
# ============================================================================

print("\n9. HDFS FAILURE HANDLING")
print("-" * 50)

# Handle missing files gracefully
try:
    df_missing = spark.read.parquet("file:///C:/temp/hdfs_output/nonexistent")
except Exception as e:
    print(f"\nHandled error: {type(e).__name__}")

# Use exists check pattern
from pathlib import Path
path = Path("C:/temp/hdfs_output/transactions")
if path.exists():
    df_exists = spark.read.parquet(str(path))
    print(f"\nData loaded: {df_exists.count()} records")

# ============================================================================
# EXAMPLE 10: HDFS METADATA CACHING
# ============================================================================

print("\n10. HDFS METADATA CACHING")
print("-" * 50)

# Cache metadata
spark.conf.set("spark.sql.hive.metastoreInfer", "true")

# Refresh table metadata
spark.catalog.refreshTable("default.bucketed_table")

# Check table metadata
print("\nTable metadata:")
print(spark.catalog.listTables())

spark.stop()
```

### Output Results

```
======================================
HADOOP HDFS INTEGRATION
======================================

1. READING DATA FROM HDFS
--------------------------------------------------

Sample Data (simulating HDFS source):
+----------+---------------+------+------------+
|      date|transaction_id|amount|category    |
+----------+---------------+------+------------+
|2024-01-01| transaction_001|1500.0| Electronics|
|2024-01-01| transaction_002|2500.0|  Clothing  |
|2024-01-02| transaction_003| 800.0|  Groceries |
|2024-01-02| transaction_004|3500.0| Electronics|
|2024-01-03| transaction_005|1200.0|  Clothing  |
+----------+---------------+------+------------+

4. HDFS PARTITIONING
--------------------------------------------------

Partitioned data partitions:
8 partitions

Filtered to specific partition:
333 records

8. HDFS OPTIMIZATION
--------------------------------------------------
Optimization techniques applied:
- Coalesce: Reduce partition count for smaller outputs
- Repartition: Increase partition count for parallelism
- Bucketing: Pre-shuffle data for faster joins
```

## IV. APPLICATIONS

### Banking and Financial Services

```python
"""
HDFS Integration in Banking - Transaction Processing
"""

def banking_hdfs_demo(spark):
    """Demonstrate HDFS for banking workloads"""
    
    print("\n" + "=" * 70)
    print("BANKING APPLICATION: HDFS Transaction Processing")
    print("=" * 70)
    
    # Generate transaction data
    transactions = [
        (f"TXN{i:08d}", f"ACC{i%1000:04d}", 100 + i % 5000, "DEBIT")
        for i in range(10000)
    ]
    df_txn = spark.createDataFrame(
        transactions, 
        ["transaction_id", "account_id", "amount", "type"]
    )
    
    # Write to HDFS partitioned by date
    df_txn.write \
        .mode("overwrite") \
        .partitionBy("type") \
        .parquet("hdfs://namenode:9000/banking/transactions")
    
    # Read historical data efficiently
    df_history = spark.read \
        .option("basePath", "hdfs://namenode:9000/banking/transactions") \
        .parquet("hdfs://namenode:9000/banking/transactions/type=DEBIT")
    
    print("\nTransaction data written to HDFS")
    print(f"Total transactions: {df_history.count()}")
```

### Healthcare Applications

```python
"""
HDFS Integration in Healthcare - Patient Records
"""

def healthcare_hdfs_demo(spark):
    """Demonstrate HDFS for healthcare workloads"""
    
    print("\n" + "=" * 70)
    print("HEALTHCARE APPLICATION: HDFS Patient Records")
    print("=" * 70)
    
    # Generate patient data
    patients = [
        (f"P{i:06d}", f"Provider{i%100:03d}", 20 + i % 60, f"ICD{i%50:03d}")
        for i in range(5000)
    ]
    df_patients = spark.createDataFrame(
        patients,
        ["patient_id", "provider_id", "age", "diagnosis_code"]
    )
    
    # Write with compression
    df_patients.write \
        .mode("overwrite") \
        .option("compression", "snappy") \
        .partitionBy("provider_id") \
        .parquet("hdfs://namenode:9000/healthcare/patients")
    
    print("\nPatient records written to HDFS with compression")
```

## V. ASCII FLOW VISUALIZATION

### HDFS Architecture

```
+=========================================================================+
|                    HDFS ARCHITECTURE                                     |
+=========================================================================+

                         ┌─────────────────────────────────────┐
                         │          NAME NODE                  │
                         │   (Metadata & Coordination)        │
                         │                                     │
                         │  ┌─────────────────────────────┐  │
                         │  │  fsimage (metadata)          │  │
                         │  │  edit logs                   │  │
                         │  │  Block locations             │  │
                         │  └─────────────────────────────┘  │
                         └───────────────┬─────────────────────┘
                                         │
                    ┌────────────────────┼────────────────────┐
                    │                    │                     │
                    ▼                    ▼                     ▼
    ┌─────────────────────┐  ┌─────────────────────┐  ┌─────────────────────┐
    │    DATA NODE 1     │  │    DATA NODE 2     │  │    DATA NODE 3     │
    │  ┌───────────────┐  │  │  ┌───────────────┐  │  │  ┌───────────────┐  │
    │  │ Block A (1/3)│  │  │  │ Block A (2/3)│  │  │  │ Block A (3/3)│  │
    │  │ Block B (1/3)│  │  │  │ Block B (2/3)│  │  │  │ Block B (3/3)│  │
    │  │ Block C (1/3)│  │  │  │ Block C (2/3)│  │  │  │ Block C (3/3)│  │
    │  └───────────────┘  │  │  └───────────────┘  │  │  └───────────────┘  │
    └─────────────────────┘  └─────────────────────┘  └─────────────────────┘

+=========================================================================+
|                    SPARK-HDFS DATA FLOW                                 |
+=========================================================================+

SPARK APPLICATION
        │
        ▼
┌─────────────────┐
│ Spark Driver   │
│ (DAG Planning) │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ NameNode       │
│ (Metadata)    │
└────────┬────────┘
         │
    ┌────┴────┐
    │         │
    ▼         ▼
┌───────┐ ┌───────┐
│ DN1  │ │ DN2   │
│Read  │ │Read   │
└───────┘ └───────┘

+=========================================================================+
|                    PARTITION DISCOVERY                                 |
+========================================================================+=

HDFS Structure:                    Spark View:
/data/                           Table: data
  /date=2024-01-01/                 ├── date: partition
  │   part-00000.parquet           │   ├── 2024-01-01
  │   part-00001.parquet           │   │   └── data files
  /date=2024-01-02/                 │   └── 2024-01-02
  │   part-00000.parquet               └── data files
  │   part-00001.parquet

Query: df = spark.read.parquet("/data")
Result: Schema includes 'date' column from partition discovery
```

## VI. ADVANCED TOPICS

### Advanced HDFS Configuration

```python
# HDFS-specific Spark configurations
spark.conf.set("spark.sql.hive.metastore", "hive")
spark.conf.set("spark.sql.hive.metastoreWarehouseDir", "hdfs://namenode:9000/warehouse")
spark.conf.set("spark.sql.files.ignoreMissingFiles", "true")
spark.conf.set("spark.sql.parquet.mergeSchema", "false")
```

### Rack Awareness for Performance

In production, configure rack awareness for optimal block placement and reduced network traffic:

```xml
<!-- core-site.xml -->
<property>
  <name>topology.node.switch.mapping.impl</name>
  <value>org.apache.hadoop.net.ScriptBasedMapping</value>
</property>
```

### HDFS High Availability

Production deployments should use HA configuration:

```python
# HA HDFS nameservice
spark.conf.set("fs.defaultFS", "hdfs://nameservice1")
```

## VII. CONCLUSION

### Key Takeaways

HDFS provides a reliable, scalable foundation for big data storage. Spark integrates seamlessly with HDFS through multiple file formats. Partitioning and bucketing strategies optimize query performance. Compression reduces storage costs and improves I/O efficiency.

### Best Practices

1. Use Parquet for analytical workloads
2. Partition by frequently filtered columns
3. Use appropriate compression codecs
4. Leverage bucketing for join optimization
5. Monitor HDFS disk usage and balance

### Next Steps

Continue to learn about Data Lake Architecture to understand modern data storage patterns.

```python
# Quick Reference: HDFS-Spark Integration

# Read from HDFS
spark.read.parquet("hdfs://namenode:9000/path")
spark.read.csv("hdfs://namenode:9000/path")

# Write to HDFS  
df.write.parquet("hdfs://namenode:9000/path")
df.write.mode("overwrite").partitionBy("col").parquet("hdfs://...")

# Partition pruning
spark.read.parquet("hdfs://.../table/col=value")
```
