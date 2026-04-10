# Spark Configuration and Optimization

## I. INTRODUCTION

### What is Spark Configuration and Optimization?
Spark configuration and optimization refer to the process of tuning Spark applications for maximum performance and resource utilization. This involves setting appropriate parameters for memory, CPU, parallelism, serialization, and other runtime aspects. Proper configuration can dramatically improve application performance, sometimes by orders of magnitude.

### Why is it Important in Big Data?
Configuration directly impacts:
- **Execution Speed**: Proper settings can reduce job runtime by 10-100x
- **Resource Utilization**: Efficient use of cluster resources reduces costs
- **Stability**: Correct memory settings prevent crashes and OOM errors
- **Scalability**: Proper configuration enables handling larger datasets

### Prerequisites
- Understanding of Spark architecture
- Familiarity with SparkSession and SparkContext
- Knowledge of cluster deployment modes
- Understanding of data processing workflows

## II. FUNDAMENTALS

### Key Configuration Categories

#### 1. Memory Configuration
- `spark.driver.memory`: Memory allocated to driver
- `spark.executor.memory`: Memory per executor
- `spark.memory.fraction`: Fraction for execution/storage
- `spark.memory.storageFraction`: Storage fraction of memory

#### 2. CPU and Parallelism
- `spark.executor.cores`: Cores per executor
- `spark.default.parallelism`: Default task parallelism
- `spark.sql.shuffle.partitions`: Shuffle partitions

#### 3. Serialization
- `spark.serializer`: Serialization method
- `spark.kryo.registrator`: Kryo registration class

#### 4. Shuffle Behavior
- `spark.shuffle.service.enabled`: External shuffle service
- `spark.sql.shuffle.partitions`: Number of shuffle partitions

### Key Terminology

- **Executor**: Worker process running tasks on nodes
- **Task**: Unit of work executed on a partition
- **Stage**: Set of tasks executed in parallel
- **Shuffle**: Data movement between stages
- **Serialization**: Converting objects to bytes

### Core Principles

1. **Balance Resources**: Memory vs CPU tradeoffs
2. **Match Workload**: Configuration to data characteristics
3. **Monitor and Tune**: Iterative optimization based on metrics
4. **Consider Cluster Size**: Scale configuration accordingly

## III. IMPLEMENTATION

### Step-by-Step Code Examples

```python
"""
Spark Configuration and Optimization Demonstration
"""

from pyspark.sql import SparkSession

def basic_configuration():
    """Demonstrate basic Spark configuration options"""
    
    print("=" * 70)
    print("SPARK CONFIGURATION BASICS")
    print("=" * 70)
    
    # Create Spark session with various configurations
    spark = SparkSession.builder \
        .appName("ConfigDemo") \
        .master("local[*]") \
        .config("spark.driver.memory", "2g") \
        .config("spark.executor.memory", "2g") \
        .config("spark.executor.cores", "2") \
        .config("spark.default.parallelism", "8") \
        .config("spark.sql.shuffle.partitions", "8") \
        .config("spark.sql.adaptive.enabled", "true") \
        .config("spark.serializer", "org.apache.spark.serializer.KryoSerializer") \
        .getOrCreate()
    
    # Get configuration details
    conf = spark.sparkContext.getConf()
    
    print("\n1. CONFIGURATION VALUES:")
    print("-" * 50)
    print(f"Driver Memory: {conf.get('spark.driver.memory')}")
    print(f"Executor Memory: {conf.get('spark.executor.memory')}")
    print(f"Default Parallelism: {conf.get('spark.default.parallelism')}")
    print(f"Shuffle Partitions: {conf.get('spark.sql.shuffle.partitions')}")
    print(f"AQE Enabled: {conf.get('spark.sql.adaptive.enabled')}")
    print(f"Serializer: {conf.get('spark.serializer')}")
    
    spark.stop()

def memory_optimization():
    """Demonstrate memory optimization techniques"""
    
    print("\n" + "=" * 70)
    print("MEMORY OPTIMIZATION")
    print("=" * 70)
    
    spark = SparkSession.builder \
        .appName("MemoryOptimization") \
        .master("local[*]") \
        .config("spark.memory.fraction", "0.6") \
        .config("spark.memory.storageFraction", "0.5") \
        .config("spark.rdd.compress", "true") \
        .config("spark.io.compression.codec", "snappy") \
        .config("spark.sql.autoBroadcastJoinThreshold", "10485760") \
        .getOrCreate()
    
    print("\n1. MEMORY CONFIGURATION:")
    print("-" * 50)
    print("  Memory Fraction: 60% for execution/storage")
    print("  Storage Fraction: 50% of memory for caching")
    print("  RDD Compression: Enabled")
    print("  Compression Codec: Snappy")
    print("  Broadcast Join Threshold: 10MB")
    
    # Demonstrate with sample data
    data = [(i, f"value_{i}") for i in range(10000)]
    df = spark.createDataFrame(data, ["id", "value"])
    
    # Cache for repeated access
    df.cache()
    count1 = df.count()
    count2 = df.count()  # Uses cached data
    
    print(f"\n2. CACHE DEMONSTRATION:")
    print("-" * 50)
    print(f"  First count: {count1}")
    print(f"  Second count (cached): {count2}")
    
    spark.stop()

def performance_optimization():
    """Demonstrate performance optimization settings"""
    
    print("\n" + "=" * 70)
    print("PERFORMANCE OPTIMIZATION")
    print("=" * 70)
    
    spark = SparkSession.builder \
        .appName("PerformanceOptimization") \
        .master("local[*]") \
        .config("spark.sql.adaptive.enabled", "true") \
        .config("spark.sql.adaptive.coalescePartitions.enabled", "true") \
        .config("spark.sql.adaptive.skewJoin.enabled", "true") \
        .config("spark.sql.adaptive.skewJoin.skewedPartitionFactor", "5") \
        .config("spark.sql.adaptive.coalescePartitions.minPartitionSize", "1048576") \
        .getOrCreate()
    
    print("\n1. ADAPTIVE QUERY EXECUTION (AQE):")
    print("-" * 50)
    print("  AQE Enabled: Dynamically optimizes query plans")
    print("  Partition Coalescing: Reduces small partitions")
    print("  Skew Join Optimization: Handles data skew")
    
    spark.stop()

def custom_configuration():
    """Demonstrate custom configuration for specific workloads"""
    
    print("\n" + "=" * 70)
    print("CUSTOM WORKLOAD CONFIGURATION")
    print("=" * 70)
    
    # Configuration for large-scale ETL jobs
    etl_config = {
        "spark.executor.memory": "4g",
        "spark.executor.cores": "2",
        "spark.executor.instances": "4",
        "spark.sql.shuffle.partitions": "200",
        "spark.sql.files.maxPartitionBytes": "134217728",
        "spark.sql.adaptive.enabled": "true"
    }
    
    print("\n1. ETL WORKLOAD CONFIGURATION:")
    print("-" * 50)
    for key, value in etl_config.items():
        print(f"  {key}: {value}")
    
    # Configuration for ML workloads
    ml_config = {
        "spark.driver.memory": "4g",
        "spark.executor.memory": "4g",
        "spark.sql.shuffle.partitions": "100",
        "spark.kryoserializer.buffer.max": "512m"
    }
    
    print("\n2. ML WORKLOAD CONFIGURATION:")
    print("-" * 50)
    for key, value in ml_config.items():
        print(f"  {key}: {value}")

def monitoring_configuration():
    """Demonstrate monitoring and metrics configuration"""
    
    print("\n" + "=" * 70)
    print("MONITORING CONFIGURATION")
    print("=" * 70)
    
    spark = SparkSession.builder \
        .appName("MonitoringDemo") \
        .master("local[*]") \
        .config("spark.ui.enabled", "true") \
        .config("spark.ui.port", "4040") \
        .config("spark.ui.retainedStages", "100") \
        .config("spark.metrics.conf.*.sink.console.enabled", "true") \
        .getOrCreate()
    
    print("\n1. UI AND METRICS SETTINGS:")
    print("-" * 50)
    print("  Spark UI: Enabled at http://localhost:4040")
    print("  Retained Stages: 100")
    print("  Console Metrics: Enabled")
    
    spark.stop()

def main():
    """Run all configuration demonstrations"""
    basic_configuration()
    memory_optimization()
    performance_optimization()
    custom_configuration()
    monitoring_configuration()
    print("\n" + "=" * 70)
    print("CONFIGURATION DEMONSTRATIONS COMPLETE")
    print("=" * 70)

if __name__ == "__main__":
    main()
```

## IV. APPLICATIONS

### Standard Example: Optimized Data Processing

```python
"""
Standard Example - Optimized Data Processing Pipeline
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F

def optimized_pipeline():
    """Demonstrate optimized data processing"""
    
    spark = SparkSession.builder \
        .appName("OptimizedPipeline") \
        .master("local[*]") \
        .config("spark.sql.adaptive.enabled", "true") \
        .config("spark.sql.adaptive.coalescePartitions.enabled", "true") \
        .config("spark.sql.shuffle.partitions", "8") \
        .getOrCreate()
    
    # Create sample data
    data = [(i, i % 10, i * 100) for i in range(10000)]
    df = spark.createDataFrame(data, ["id", "category", "value"])
    
    # Optimize: Use partition pruning
    result = df.filter(df.category == 5).groupBy("category").sum("value")
    result.show()
    
    spark.stop()

optimized_pipeline()
```

### Real-World Example 1: Banking/Finance - Transaction Processing Optimization

```python
"""
Banking - Optimized Transaction Processing
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, DoubleType, TimestampType

def banking_optimization():
    """Optimize banking transaction processing"""
    
    print("=" * 70)
    print("BANKING TRANSACTION OPTIMIZATION")
    print("=" * 70)
    
    spark = SparkSession.builder \
        .appName("BankingOptimization") \
        .master("local[*]") \
        .config("spark.executor.memory", "4g") \
        .config("spark.executor.cores", "2") \
        .config("spark.sql.shuffle.partitions", "16") \
        .config("spark.sql.adaptive.enabled", "true") \
        .config("spark.sql.adaptive.coalescePartitions.enabled", "true") \
        .config("spark.serializer", "org.apache.spark.serializer.KryoSerializer") \
        .config("spark.kryoserializer.buffer.max", "256m") \
        .getOrCreate()
    
    # Define optimized schema
    transaction_schema = StructType([
        StructField("transaction_id", StringType(), False),
        StructField("account_id", StringType(), False),
        StructField("amount", DoubleType(), False),
        StructField("transaction_type", StringType(), True),
        StructField("timestamp", TimestampType(), False),
        StructField("merchant_id", StringType(), True),
    ])
    
    # Generate sample data (in production, load from files)
    transaction_data = [
        (f"TXN{i:06d}", f"ACC{i%1000:03d}", float(i*10), 
         "debit" if i % 2 == 0 else "credit", "2024-01-15 10:30:00", f"M{i%50:03d}")
        for i in range(100000)
    ]
    
    df = spark.createDataFrame(transaction_data, transaction_schema)
    
    print("\n1. OPTIMIZATION TECHNIQUES APPLIED:")
    print("-" * 50)
    print("  - AQE Enabled for dynamic optimization")
    print("  - Partition coalescing for reduced overhead")
    print("  - Kryo serialization for faster processing")
    print("  - Appropriate shuffle partitions")
    
    # Optimize: Broadcast small lookup table
    print("\n2. BROADCAST JOIN EXAMPLE:")
    print("-" * 50)
    
    # Create small merchant lookup table
    merchant_data = [(f"M{i:03d}", f"Merchant_{i}", "Category_" + str(i % 5)) 
                     for i in range(50)]
    merchant_df = spark.createDataFrame(merchant_data, ["merchant_id", "name", "category"])
    
    # Join with broadcast
    result = df.join(F.broadcast(merchant_df), "merchant_id")
    
    # Aggregation with optimization
    print("\n3. AGGREGATION RESULT:")
    print("-" * 50)
    
    agg_result = result.groupBy("category").agg(
        F.count("*").alias("transaction_count"),
        F.sum("amount").alias("total_amount"),
        F.avg("amount").alias("avg_amount")
    ).orderBy(F.desc("transaction_count"))
    
    agg_result.show()
    
    spark.stop()
    print("\n" + "=" * 70)

banking_optimization()
```

### Real-World Example 2: Healthcare - Patient Data Optimization

```python
"""
Healthcare - Optimized Patient Data Processing
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType

def healthcare_optimization():
    """Optimize healthcare data processing"""
    
    print("=" * 70)
    print("HEALTHCARE DATA OPTIMIZATION")
    print("=" * 70)
    
    spark = SparkSession.builder \
        .appName("HealthcareOptimization") \
        .master("local[*]") \
        .config("spark.sql.adaptive.enabled", "true") \
        .config("spark.sql.adaptive.skewJoin.enabled", "true") \
        .config("spark.sql.shuffle.partitions", "16") \
        .config("spark.sql.autoBroadcastJoinThreshold", "20971520") \
        .getOrCreate()
    
    # Define patient schema
    patient_schema = StructType([
        StructField("patient_id", StringType(), False),
        StructField("name", StringType(), True),
        StructField("age", IntegerType(), True),
        StructField("department", StringType(), True),
        StructField("diagnosis", StringType(), True),
        StructField("admission_date", StringType(), True),
        StructField("charges", DoubleType(), True),
    ])
    
    # Generate sample patient data
    patient_data = [
        (f"P{i:06d}", f"Patient_{i}", (i % 80) + 20, 
         ["Cardiology", "General", "Respiratory", "Orthopedics"][i % 4],
         f"Diagnosis_{i%20}", f"2024-01-{(i%30)+1:02d}", float(i*10))
        for i in range(50000)
    ]
    
    df = spark.createDataFrame(patient_data, patient_schema)
    
    print("\n1. OPTIMIZATION SETTINGS:")
    print("-" * 50)
    print("  - AQE for dynamic optimization")
    print("  - Skew join handling")
    print("  - 20MB broadcast threshold")
    print("  - 16 shuffle partitions")
    
    # Cache frequently accessed data
    df.cache()
    
    print("\n2. DEPARTMENT ANALYSIS (OPTIMIZED):")
    print("-" * 50)
    
    dept_result = df.groupBy("department").agg(
        F.count("*").alias("patient_count"),
        F.avg("charges").alias("avg_charges"),
        F.min("age").alias("min_age"),
        F.max("age").alias("max_age")
    ).orderBy(F.desc("patient_count"))
    
    dept_result.show()
    
    # Optimize with predicate pushdown
    print("\n3. AGE-BASED FILTERING (With Pushdown):")
    print("-" * 50)
    
    elderly = df.filter(df.age >= 65).groupBy("department").count()
    elderly.show()
    
    spark.stop()
    print("\n" + "=" * 70)

healthcare_optimization()
```

## V. OUTPUT_RESULTS

### Expected Output

```
============================================================
SPARK CONFIGURATION BASICS
============================================================

1. CONFIGURATION VALUES:
----------------------------------------
Driver Memory: 2g
Executor Memory: 2g
Default Parallelism: 8
Shuffle Partitions: 8
AQE Enabled: true
Serializer: org.apache.spark.serializer.KryoSerializer
```

## VI. VISUALIZATION

### Configuration Flow

```
+------------------------------------------------------------------+
|              OPTIMIZATION CONFIGURATION FLOW                    |
+------------------------------------------------------------------+

CLUSTER RESOURCES
       |
       v
+------------------------+
|  MEMORY CONFIG         |
|  - Driver Memory       |
|  - Executor Memory     |
|  - Memory Fraction    |
+------------------------+
       |
       v
+------------------------+
|  CPU CONFIG            |
|  - Executor Cores      |
|  - Parallelism        |
|  - Task Slots         |
+------------------------+
       |
       v
+------------------------+
|  SERIALIZATION         |
|  - Kryo Serialization |
|  - Buffer Sizes        |
+------------------------+
       |
       v
+------------------------+
|  QUERY OPTIMIZATION    |
|  - AQE                 |
|  - Broadcast Joins    |
|  - Partition Strategy  |
+------------------------+

==================================================================
                    OPTIMIZATION DECISION TREE
==================================================================

Is Data Large (> 100GB)?
  |
  +--Yes--> Increase executor memory
  |         Increase shuffle partitions
  |         Enable AQE
  |
  +--No--> Use default or moderate settings

Is Join Involved?
  |
  +--Yes--> Check table sizes
  |         Use broadcast if small
  |         Enable skew join optimization
  |
  +--No--> Focus on aggregation optimization

Is Data Skewed?
  |
  +--Yes--> Use adaptive query execution
  |         Apply salting technique
  |
  +--No--> Standard configuration
```

## VII. ADVANCED_TOPICS

### Advanced Configuration

1. **Dynamic Allocation**:
```python
.config("spark.dynamicAllocation.enabled", "true")
.config("spark.dynamicAllocation.minExecutors", "1")
.config("spark.dynamicAllocation.maxExecutors", "10")
```

2. **Compression**:
```python
.config("spark.rdd.compress", "true")
.config("spark.io.compression.codec", "snappy")
```

3. **SQL Configuration**:
```python
.config("spark.sql.warehouse.dir", "/path/to/warehouse")
.config("spark.sql.files.maxPartitionBytes", "128MB")
```

### Optimization Techniques

| Technique | When to Use |
|-----------|-------------|
| Broadcast Joins | Small table < 10MB |
| Coalesce | Reduce partitions after filtering |
| Cache | Reuse DataFrame multiple times |
| Persist | Complex transformations |
| AQE | Complex queries with runtime info |

## VIII. CONCLUSION

### Key Takeaways

1. **Configuration impacts performance significantly**
2. **AQE provides automatic optimization**
3. **Memory and CPU must be balanced**
4. **Monitor metrics to guide tuning**

### Next Steps

- Learn cluster-specific configurations
- Practice with real workloads
- Understand Spark UI metrics

### Further Reading

- Spark Configuration Docs
- Tuning Guide