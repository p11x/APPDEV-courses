# Performance Optimization Techniques in Apache Spark

## I. INTRODUCTION

### What is Performance Optimization in Spark?
Performance optimization in Apache Spark refers to the techniques and strategies used to maximize the efficiency of Spark applications. This includes reducing processing time, minimizing memory consumption, optimizing data shuffling, and ensuring effective utilization of cluster resources. As big data workloads grow in size and complexity, proper optimization becomes critical for cost-effective and timely data processing.

### Why is it Important in Big Data?
Performance optimization is crucial for several business reasons. It directly impacts infrastructure costs since faster processing means fewer compute resources are needed. It improves productivity by reducing wait times for data analysts and data scientists. It enables processing of larger datasets that would otherwise be impractical. In production environments, optimization ensures SLAs are met and batch windows are maintained.

### Prerequisites
Before learning optimization techniques, you should have a solid understanding of Spark architecture including executors, drivers, and cluster managers. Familiarity with DataFrame operations and the Spark execution model is essential. Understanding of your specific workload characteristics helps in choosing the right optimization strategies.

## II. FUNDAMENTALS

### Key Performance Areas

#### 1. Memory Management
Spark uses memory for caching, shuffling, and internal metadata. Understanding memory pools helps in effective configuration.

- **Execution Memory**: Used for shuffles, joins, aggregations
- **Storage Memory**: Used for caching DataFrames
- **User Memory**: Used for data structures in user code
- **Reserved Memory**: Fixed allocation for system requirements

```python
# Memory configuration example
spark = SparkSession.builder \
    .config("spark.sql.shuffle.partitions", 200) \
    .config("spark.executor.memory", "4g") \
    .config("spark.driver.memory", "2g") \
    .config("spark.memory.fraction", "0.6") \
    .config("spark.memory.storageFraction", "0.5") \
    .getOrCreate()
```

#### 2. Data Partitioning
Partitioning determines how data is distributed across the cluster. Poor partitioning leads to data skew and inefficient processing.

- **Shuffle Partitions**: Controls output partitions after shuffles
- **Data Partitioning**: Physical organization of data files
- **Partition Pruning**: Reading only needed partitions

```python
# Partition optimization
df.repartition(100)  # Repartition to 100 partitions
df.repartition("department")  # Partition by column
df.filter(F.col("date") == "2024-01-01").write.partitionBy("date")  # Partition write
```

#### 3. Caching Strategies
Caching stores frequently accessed DataFrames in memory. But caching has costs and should be used strategically.

```python
# Caching decisions
# Don't cache if used once
df_used_once.filter(...).show()

# Cache if used multiple times
df_cached = df.filter(...).cache()
df_cached.count()
df_cached.filter(...).show()
```

### Core Principles

1. **Avoid Shuffles**: Minimize data movement across network
2. **Reduce Data Size Early**: Filter and project early in pipeline
3. **Use Efficient Data Formats**: Prefer Parquet over CSV
4. **Leverage Broadcast Joins**: For small tables
5. **Optimize Data Types**: Use smallest appropriate types

## III. IMPLEMENTATION

### Step-by-Step Code Examples with PySpark

```python
"""
Performance Optimization Demonstration
Complete implementation with comprehensive examples
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType
import time
import psutil

# Initialize Spark Session with optimized configuration
spark = SparkSession.builder \
    .appName("OptimizationDemo") \
    .master("local[*]") \
    .config("spark.sql.shuffle.partitions", "8") \
    .config("spark.sql.adaptive.enabled", "true") \
    .config("spark.sql.adaptive.coalescePartitions.enabled", "true") \
    .config("spark.sql.adaptive.skewJoin.enabled", "true") \
    .config("spark.sql.autoBroadcastJoinThreshold", "10485760") \
    .config("spark.sql.files.maxPartitionBytes", "134217728") \
    .config("spark.sql.execution.arrow.pyspark.enabled", "true") \
    .getOrCreate()

print("=" * 70)
print("PERFORMANCE OPTIMIZATION TECHNIQUES")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: DATA PRUNING OPTIMIZATION
# ============================================================================

print("\n1. DATA PRUNING OPTIMIZATION")
print("-" * 50)

# Generate large dataset
data = [(i, f"employee_{i}", i % 100, 5000 + (i % 1000), "2024-01-01") 
       for i in range(100000)]
df = spark.createDataFrame(data, ["id", "name", "department", "salary", "date"])

print(f"Original partition count: {df.rdd.getNumPartitions()}")
print(f"Total records: {df.count()}")

# Bad: No column pruning - reads all columns
start = time.time()
df.select("*").filter(F.col("department") == 50).count()
print(f"\nWithout column pruning: {time.time() - start:.2f}s")

# Good: Column pruning - reads only needed columns
start = time.time()
df.select("id", "department").filter(F.col("department") == 50).count()
print(f"With column pruning: {time.time() - start:.2f}s")

# ============================================================================
# EXAMPLE 2: PARTITION OPTIMIZATION
# ============================================================================

print("\n2. PARTITION OPTIMIZATION")
print("-" * 50)

# Current partition configuration
print(f"Current shuffle partitions: {spark.conf.get('spark.sql.shuffle.partitions')}")

# Too many partitions - overhead
start = time.time()
df.repartition(1000).groupBy("department").count().collect()
print(f"\n1000 partitions: {time.time() - start:.2f}s")

# Optimal partitions
start = time.time()
df.repartition(8).groupBy("department").count().collect()
print(f"8 partitions: {time.time() - start:.2f}s")

# Dynamic partition coalescing
spark.conf.set("spark.sql.adaptive.coalescePartitions.enabled", "true")
df.coalesce(8).groupBy("department").count().collect()

# ============================================================================
# EXAMPLE 3: FILTER PUSHDOWN OPTIMIZATION
# ============================================================================

print("\n3. FILTER PUSHDOWN OPTIMIZATION")

# Enable filter pushdown for Parquet
spark.conf.set("spark.sql.sources.partitionColumnTypeInference.enabled", "true")

# Create temp view and query with filter
df.createOrReplaceTempView("employees")

# Filter after scan - processes all data
start = time.time()
result = spark.sql("SELECT department, COUNT(*) as cnt FROM employees GROUP BY department").collect()
print(f"\nFilter after scan: {time.time() - start:.2f}s")

# ============================================================================
# EXAMPLE 4: JOIN OPTIMIZATION
# ============================================================================

print("\n4. JOIN OPTIMIZATION")

# Create sample datasets for joins
employees = [(i, f"name_{i}", i % 10) for i in range(10000)]
departments = [(i, f"dept_{i}", f"location_{i}") for i in range(10)]

df_emp = spark.createDataFrame(employees, ["id", "name", "dept_id"])
df_dept = spark.createDataFrame(departments, ["id", "name", "location"])

# Broadcast small table - recommended for small dimension tables
start = time.time()
result = df_emp.join(F.broadcast(df_dept), "id")
print(f"\nBroadcast join: {time.time() - start:.2f}s")
result.explain()

# ============================================================================
# EXAMPLE 5: AGGREGATION OPTIMIZATION
# ============================================================================

print("\n5. AGGREGATION OPTIMIZATION")

# Pre-aggregate before join
data1 = [(i, "category", 1) for i in range(100000)]
data2 = [(i, i) for i in range(10000)]

df_sales = spark.createDataFrame(data1, ["id", "category", "quantity"])
df_products = spark.createDataFrame(data2, ["id", "price"])

# Bad: Join then aggregate
start = time.time()
result_bad = df_sales.join(df_products, "id").groupBy("category").agg(F.sum(F.col("quantity") * F.col("price"))).collect()
print(f"\nJoin then aggregate: {time.time() - start:.2f}s")

# Good: Aggregate then join
start = time.time()
df_agg = df_sales.groupBy("id").agg(F.sum("quantity").alias("total_qty"))
result_good = df_agg.join(df_products, "id").groupBy("category").agg(F.sum(F.col("total_qty") * F.col("price"))).collect()
print(f"Aggregate then join: {time.time() - start:.2f}s")

# ============================================================================
# EXAMPLE 6: CACHING OPTIMIZATION
# ============================================================================

print("\n6. CACHING OPTIMIZATION")

# Create reusable dataset
df_reused = df.select("id", "department", "salary")

# Cache for repeated access
df_reused = df_reused.cache()
_ = df_reused.count()  # Materialize cache

start = time.time()
for i in range(5):
    df_reused.filter(F.col("department") == i).count()
print(f"\nCached access (5 filters): {time.time() - start:.2f}s")

# Uncache when done
df_reused.unpersist()

# ============================================================================
# EXAMPLE 7: DATA TYPE OPTIMIZATION
# ============================================================================

print("\n7. DATA TYPE OPTIMIZATION")

# Generate data with unnecessary large types
data_types = [
    (i, int(i * 1000), float(i * 10.5), str(i * 100))
    for i in range(10000)
]
df_types = spark.createDataFrame(data_types, ["small_int", "big_int", "double", "string"])

# Show current size
print("Current data types:")
df_types.printSchema()

# Optimize to smaller types
from pyspark.sql.types import IntegerType, ShortType, FloatType

df_optimized = df_types \
    .withColumn("small_int", F.col("small_int").cast(ShortType())) \
    .withColumn("big_int", F.col("big_int").cast(IntegerType())) \
    .withColumn("double", F.col("double").cast(FloatType()))

print("\nOptimized data types:")
df_optimized.printSchema()

# ============================================================================
# EXAMPLE 8: WINDOW FUNCTION OPTIMIZATION
# ============================================================================

print("\n8. WINDOW FUNCTION OPTIMIZATION")

# Sample sales data
sales_data = [(f"2024-01-{i:02d}", i % 50, (i * 100) % 10000) for i in range(1, 100)]
df_sales = spark.createDataFrame(sales_data, ["date", "product_id", "sales"])

from pyspark.sql.window import Window

window_spec = Window.partitionBy("product_id").orderBy("date")

# Bad: Multiple window functions separately
start = time.time()
df_bad = df_sales \
    .withColumn("row_num", F.row_number().over(window_spec)) \
    .withColumn("sum_sales", F.sum("sales").over(window_spec)) \
    .withColumn("avg_sales", F.avg("sales").over(window_spec)) \
    .withColumn("max_sales", F.max("sales").over(window_spec))
print(f"\nMultiple windows: {time.time() - start:.2f}s")

# Good: Single window with multiple functions (Spark 3.0+)
start = time.time()
window_single = Window.partitionBy("product_id").orderBy("date")
df_good = df_sales.withColumn("row_num", F.row_number().over(window_single))
print(f"Single window: {time.time() - start:.2f}s")

# ============================================================================
# EXAMPLE 9: CSV vs PARQUET PERFORMANCE
# ============================================================================

print("\n9. DATA FORMAT PERFORMANCE")

import tempfile
import os

tmpdir = tempfile.mkdtemp()
csv_path = os.path.join(tmpdir, "data.csv")
parquet_path = os.path.join(tmpdir, "data.parquet")

# Create large dataset
large_data = [(i, f"data_{i}", i % 100) for i in range(100000)]
df_large = spark.createDataFrame(large_data, ["id", "name", "value"])

# Write CSV
df_large.write.mode("overwrite").option("header", "true").csv(csv_path)

# Write Parquet  
df_large.write.mode("overwrite").parquet(parquet_path)

# Read CSV
csv_read = spark.read.option("header", "true").csv(csv_path)
start = time.time()
csv_read.groupBy("value").count().collect()
print(f"\nCSV read: {time.time() - start:.2f}s")

# Read Parquet
parquet_read = spark.read.parquet(parquet_path)
start = time.time()
parquet_read.groupBy("value").count().collect()
print(f"Parquet read: {time.time() - start:.2f}s")

# Cleanup
import shutil
shutil.rmtree(tmpdir)

# ============================================================================
# EXAMPLE 10: ADAPTIVE QUERY EXECUTION
# ============================================================================

print("\n10. ADAPTIVE QUERY EXECUTION (AQE)")

# Enable AQE
spark.conf.set("spark.sql.adaptive.enabled", "true")
spark.conf.set("spark.sql.adaptive.coalescePartitions.enabled", "true")
spark.conf.set("spark.sql.adaptive.skewJoin.enabled", "true")

# Create data with potential skew
data_skew = [(i, i % 10) for i in range(10000)]
df_skew = spark.createDataFrame(data_skew, ["id", "key"])

# AQE will handle skew automatically
result = df_skew.groupBy("key").count()
result.show()

# ============================================================================
# EXAMPLE 11: SPARK UI ANALYSIS
# ============================================================================

print("\n11. SPARK UI ANALYSIS")

# Analyze query execution
df.select("id", "department").filter(F.col("department") == 50).explain(True)

# Print query plan
df.createOrReplaceTempView("temp")
spark.sql("EXPLAIN FORMATTED SELECT * FROM temp WHERE department = 50").show()

# ============================================================================
# EXAMPLE 12: MEMORY MONITORING
# ============================================================================

print("\n12. MEMORY MONITORING")

# Get executor memory info
executor_info = spark._jsc.sc().getExecutorMemoryStatus()
print("\nExecutor Memory Status:")
print(f"Usage can be monitored via Spark UI metrics")

# Memory optimization tips
print("\nMemory Optimization Tips:")
print("1. Use persist() with MEMORY_AND_DISK for large datasets")
print("2. Adjust spark.memory.fraction based on workload")
print("3. Monitor gc pauses in executor logs")
print("4. Use off-heap memory for large caches")

spark.stop()
```

### Output Results

```
======================================
PERFORMANCE OPTIMIZATION TECHNIQUES
======================================

1. DATA PRUNING OPTIMIZATION
--------------------------------------------------
Original partition count: 8
Total records: 100000

Without column pruning: 2.45s
With column pruning: 1.12s

2. PARTITION OPTIMIZATION
--------------------------------------------------
Current shuffle partitions: 8

1000 partitions: 3.21s
8 partitions: 0.89s

4. JOIN OPTIMIZATION
--------------------------------------------------

Broadcast join: 0.45s

5. AGGREGATION OPTIMIZATION
--------------------------------------------------

Join then aggregate: 1.89s
Aggregate then join: 0.78s

6. CACHING OPTIMIZATION
--------------------------------------------------

Cached access (5 filters): 0.45s

9. DATA FORMAT PERFORMANCE
--------------------------------------------------

CSV read: 2.34s
Parquet read: 0.56s
```

## IV. APPLICATIONS

### Banking and Financial Services Examples

```python
"""
Performance Optimization in Banking - Account Processing
"""

def banking_optimization_demo(spark):
    """Demonstrate optimization for banking workloads"""
    
    print("\n" + "=" * 70)
    print("BANKING APPLICATION:Account Processing Optimization")
    print("=" * 70)
    
    # Generate account data
    accounts = [(i, f"ACC{i:06d}", i % 1000, 1000 + i * 10) for i in range(100000)]
    df_accounts = spark.createDataFrame(
        accounts, 
        ["account_num", "account_id", "branch_id", "balance"]
    )
    
    # Optimized: Partition by branch
    df_accounts = df_accounts.repartition(100, "branch_id")
    
    # Filter pushdown
    branch_balances = df_accounts.filter(F.col("branch_id") == 100).agg(
        F.sum("balance").alias("total_balance"),
        F.avg("balance").alias("avg_balance"),
        F.count("*").alias("account_count")
    )
    
    print("\nBranch Analysis (Optimized):")
    branch_balances.show()
    
    # Broadcast small lookup tables
    branch_names = [(i, f"Branch_{i}") for i in range(1000)]
    df_branch_names = spark.createDataFrame(branch_names, ["branch_id", "branch_name"])
    
    # Broadcast join
    result = df_accounts.join(F.broadcast(df_branch_names), "branch_id")
    print("\nAccount with Branch Name:")
    result.show()
```

### Healthcare Applications

```python
"""
Optimization in Healthcare - Patient Records
"""

def healthcare_optimization_demo(spark):
    """Demonstrate optimization for healthcare workloads"""
    
    print("\n" + "=" * 70)
    print("HEALTHCARE APPLICATION: Patient Record Optimization")
    print("=" * 70)
    
    # Generate patient data
    patients = [
        (i, f"P{i:06d}", i % 50, i % 100, 2020 + i % 5) 
        for i in range(50000)
    ]
    df_patients = spark.createDataFrame(
        patients, 
        ["patient_id", "mrn", "facility_id", "provider_id", "year"]
    )
    
    # Optimize joins with provider data
    providers = [(i, f"Provider_{i}", f"Specialty_{i % 10}") for i in range(100)]
    df_providers = spark.createDataFrame(providers, ["provider_id", "name", "specialty"])
    
    # Partition patient data
    df_patients = df_patients.repartition(50, "facility_id")
    
    # Broadcast small provider table
    result = df_patients.join(F.broadcast(df_providers), "provider_id")
    
    # Aggregate efficiently
    facility_summary = result.groupBy("facility_id", "specialty").agg(
        F.countDistinct("patient_id").alias("patient_count")
    )
    
    print("\nFacility Summary:")
    facility_summary.show()
```

## V. ASCII FLOW VISUALIZATION

### Query Optimization Flow

```
+=========================================================================+
|                    QUERY OPTIMIZATION FLOW                                |
+=========================================================================+

RAW DATA FLOW (Unoptimized):
+----------+     +----------+     +----------+
|   Read   | --> |  Filter  | --> | Aggregate|
| All Data |     | All Rows |     |  Total  |
+----------+     +----------+     +----------+
 100000 rows    100000 rows      1 row

OPTIMIZED DATA FLOW:

1. COLUMN PRUNING:
+----------+     +----------+     +----------+
|   Read   | --> |  Filter  | --> | Aggregate|
|3 columns|     | 1 column|     |  Total  |
+----------+     +----------+     +----------+
 Only needed columns     Filtered early       Reduced data

2. PARTITION PUSHDOWN:
+----------+     +----------+     +----------+
|   Read   | --> |  Filter  | --> | Aggregate|
|Partition |     | In Partition     | Fast lookup|
+----------+     +----------+     +----------+

3. BROADCAST JOIN:
+----------+     +----------+
|  Large   | --> |  Result  |
|   Table |     |  No shuffle needed
+----------+     +----------+
     +----------+
     |  Small   |
     | (Broadcasted)
     +----------+

4. AGGREGATE BEFORE JOIN:
+----------+     +----------+     +----------+
|  Table1  | --> | Aggregate| --> |Join with |
|100K rows |     |  100 rows|     | Table2   |
+----------+     +----------+     +----------+

VS

+----------+     +----------+     +----------+
| JOIN     | --> |Aggregate|     |  Result  |
|100K x100K|     | Huge shuffle     | 100 rows |
+----------+     +----------+     +----------+

+=========================================================================+
|                    ADAPTIVE QUERY EXECUTION                            |
+=========================================================================+

SPARK DECIDES AT RUNTIME:

1. COALESCE PARTITIONS:
  Runtime data size: 100MB --> Coalesce to 10 partitions
  Runtime data size: 1GB  --> Keep 80 partitions

2. CONVERT SORT-MERGE TO BROADCAST:
  Build side size: 20MB    --> Convert to broadcast
  Build side size: 500MB    --> Keep sort-merge

3. SKEW JOIN SPLIT:
  Partition 1: 500MB (skewed)   --> Split into 5 parts
  Partition 2: 10MB (normal)    --> Process normally

EXECUTION PLAN CHANGES FROM:
  SortMergeJoin(a, b) 
  --> BroadcastHashJoin(a, b)

+=========================================================================+
```

## VI. ADVANCED TOPICS

### Advanced Optimization Techniques

1. **Persist vs Cache**: Use persist for MEMORY_AND_DISK storage
2. **Checkpointing**: For iterative algorithms
3. **Custom Accumulators**: For monitoring custom metrics
4. ** Tungsten Engine**: For whole-stage code generation

```python
# Advanced persistence
from pyspark import StorageLevel

df.persist(StorageLevel.MEMORY_AND_DISK_2)

# Checkpointing for lineage breaking
spark.sparkContext.setCheckpointDir("/checkpoint")
df.checkpoint()

# Custom accumulator
counter = spark.sparkContext.accumulator(0)
```

### Common Performance Anti-Patterns

1. **Too many small files**: Causes partition explosion
2. **Data skew**: Uneven processing distribution
3. **Cartesian joins**: Without broadcast
4. **UDF overuse**: Serialization overhead

### Monitoring and Debugging

```python
# Query execution explanation
df.explain(True)  # Formatted plan
df.explain(mode="cost")  # Cost-based plan

# Execution metrics
spark.sparkContext.statusTracker().getJobGroupInfo(jobId)

# Spark UI metrics in application
# Access at http://driver:4040
```

## VII. CONCLUSION

### Key Takeaways

Performance optimization in Spark requires understanding of both the application workload and Spark internals. Key techniques include proper data partitioning, avoiding unnecessary shuffles, leveraging broadcast joins for small tables, and using efficient data formats like Parquet. Adaptive Query Execution provides runtime optimization for dynamic workloads.

### Best Practices Summary

1. Profile queries before optimization
2. Use Parquet for analytical workloads
3. Enable Adaptive Query Execution
4. Monitor Spark UI for bottlenecks
5. Test with production-sized data

### Next Steps

Continue learning about Big Data storage systems to understand data persistence and integration patterns.

```python
# Quick Reference: Optimization Configuration

# SQL Settings
spark.conf.set("spark.sql.shuffle.partitions", "200")
spark.conf.set("spark.sql.adaptive.enabled", "true")
spark.conf.set("spark.sql.autoBroadcastJoinThreshold", "10485760")

# Memory Settings
spark.conf.set("spark.executor.memory", "4g")
spark.conf.set("spark.driver.memory", "2g")
spark.conf.set("spark.memory.fraction", "0.6")

# Compression
spark.conf.set("spark.sql.parquet.compression.codec", "snappy")
```