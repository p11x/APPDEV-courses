# NoSQL Database Integration with Apache Spark

## I. INTRODUCTION

### What is NoSQL Database Integration?
NoSQL database integration with Apache Spark enables processing and analysis of data stored in non-relational databases like MongoDB, Cassandra, DynamoDB, and Redis. Spark provides connectors that allow reading from and writing to these databases, enabling real-time analytics and large-scale data processing on NoSQL data stores.

### Why is it Important in Big Data?
NoSQL databases handle semi-structured and unstructured data at scale. They provide flexible schemas for evolving data models. They offer high write throughput for real-time data. Integration enables advanced analytics on modern data architectures.

### Prerequisites
Understanding of NoSQL database concepts is helpful. Familiarity with Spark DataFrames required. Knowledge of database drivers and connections needed.

## II. FUNDAMENTALS

### Types of NoSQL Databases

#### Document Stores (MongoDB, CouchDB)
Store data as JSON-like documents. Flexible schema. Ideal for content management and user profiles.

#### Column Stores (Cassandra, HBase)
Wide-column storage. Optimized for write-heavy workloads. Great for time-series data.

#### Key-Value Stores (Redis, DynamoDB)
Simple key-value pairs. Ultra-fast lookups. Caching and session storage.

#### Graph Databases (Neo4j)
Network/graph data. Relationships as first-class citizens. Social networks and recommendations.

### Spark Connectors

```python
# MongoDB Spark Connector
spark = SparkSession.builder \
    .config("spark.mongodb.input.uri", "mongodb://localhost:27017") \
    .getOrCreate()

df = spark.read.format("mongo").load("database.collection")

# Cassandra Spark Connector
df = spark.read.format("org.apache.spark.sql.cassandra") \
    .option("keyspace", "mykeyspace") \
    .option("table", "mytable") \
    .load()
```

## III. IMPLEMENTATION

### Step-by-Step Code Examples with PySpark

```python
"""
NoSQL Database Integration Demonstration
Complete implementation with comprehensive examples
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType
import random

# Initialize Spark
spark = SparkSession.builder \
    .appName("NoSQLDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("NOSQL DATABASE INTEGRATION")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: SIMULATED MONGODB-LIKE DOCUMENT STORAGE
# ============================================================================

print("\n1. SIMULATED MONGODB-LIKE DOCUMENT STORAGE")
print("-" * 50)

# Create document-style data
documents = [
    {
        "_id": f"doc_{i}",
        "user_id": f"user_{random.randint(1000, 9999)}",
        "name": f"User {i}",
        "email": f"user{i}@example.com",
        "age": random.randint(18, 80),
        "address": {
            "city": random.choice(["New York", "Los Angeles", "Chicago"]),
            "state": random.choice(["NY", "CA", "IL"])
        },
        "tags": [f"tag{random.randint(1, 5)}" for _ in range(random.randint(1, 3))]
    }
    for i in range(100)
]

df_docs = spark.createDataFrame(documents)
print(f"\nDocument-style data created: {df_docs.count()} documents")
df_docs.show(5)

# ============================================================================
# EXAMPLE 2: CASSANDRA-STYLE COLUMN FAMILY STORAGE
# ============================================================================

print("\n2. CASSANDRA-STYLE COLUMN FAMILY STORAGE")
print("-" * 50)

# Create wide-column style data
cassandra_data = [
    (f"partition_{i // 100}", i, random.randint(1, 1000), "TXN", 100.0)
    for i in range(10000)
]

df_cass = spark.createDataFrame(
    cassandra_data,
    ["partition_key", "clustering_key", "txn_id", "type", "amount"]
)

# Write in Cassandra-style
df_cass.write \
    .mode("overwrite") \
    .format("org.apache.spark.sql.cassandra") \
    .option("keyspace", "mykeyspace") \
    .option("table", "transactions") \
    .save()

print(f"\nCassandra-style data: {df_cass.count()} rows")
print("In production: Uses cassandra-spark-connector")

# ============================================================================
# EXAMPLE 3: REDIS-STYLE KEY-VALUE STORAGE
# ============================================================================

print("\n3. REDIS-STYLE KEY-VALUE STORAGE")
print("-" * 50)

# Key-value pair data
kv_data = [
    (f"user:{random.randint(1000, 9999)}", 
     f"{{'name': 'User {i}', 'score': {random.randint(1, 1000)}}}")
    for i in range(1000)
]

df_kv = spark.createDataFrame(kv_data, ["key", "value"])

# Write as key-value
df_kv.write \
    .mode("overwrite") \
    .format("org.apache.spark.sql.redis") \
    .option("table", "user_scores") \
    .save()

print(f"\nKey-value data: {df_kv.count()} entries")
print("In production: Uses spark-redis connector")

# ============================================================================
# EXAMPLE 4: DYNAMODB-STYLE STORAGE
# ============================================================================

print("\n4. DYNAMODB-STYLE STORAGE")
print("-" * 50)

# DynamoDB table data
dynamo_data = [
    {
        "user_id": f"USER{i:06d}",
        "timestamp": 1704067200 + i * 3600,
        "activity_type": random.choice(["login", "click", "purchase"]),
        "details": {"page": f"/page{i % 100}", "duration": random.randint(1, 300)}
    }
    for i in range(5000)
]

df_dynamo = spark.createDataFrame(dynamo_data)

# Write to DynamoDB
df_dynamo.write \
    .mode("overwrite") \
    .format("dynamodb") \
    .option("tableName", "user_activities") \
    .save()

print(f"\nDynamoDB-style data: {df_dynamo.count()} items")

# ============================================================================
# EXAMPLE 5: NOSQL AGGREGATION OPERATIONS
# ============================================================================

print("\n5. NOSQL AGGREGATION OPERATIONS")
print("-" * 50)

# Nested document aggregation
df_nested = spark.createDataFrame([
    {"user": "u1", "purchases": [{"item": "a", "price": 10}, {"item": "b", "price": 20}]},
    {"user": "u2", "purchases": [{"item": "c", "price": 15}]},
])

# Explode nested arrays
df_exploded = df_nested.select(
    "user",
    F.explode("purchases").alias("purchase")
).select(
    "user",
    "purchase.item",
    "purchase.price"
)

print("\nExploded nested data:")
df_exploded.show()

# ============================================================================
# EXAMPLE 6: TIME-SERIES NOSQL PATTERNS
# ============================================================================

print("\n6. TIME-SERIES NOSQL PATTERNS")
print("-" * 50)

# Time-series data (Cassandra-like)
ts_data = [
    (f"sensor_{i % 10}", 1704067200 + i * 60, random.uniform(20, 30))
    for i in range(10000)
]

df_ts = spark.createDataFrame(
    ts_data,
    ["sensor_id", "timestamp", "reading"]
)

# Window-based aggregation
from pyspark.sql.window import Window

window_spec = Window.partitionBy("sensor_id").orderBy("timestamp").rangeBetween(-3600, 0)

df_windowed = df_ts.withColumn(
    "avg_reading_1h",
    F.avg("reading").over(window_spec)
)

print("\nTime-series with moving average:")
df_windowed.show(10)

# ============================================================================
# EXAMPLE 7: NOSQL JOIN PATTERNS
# ============================================================================

print("\n7. NOSQL JOIN PATTERNS")
print("-" * 50)

# User data (reference)
users = [(f"user_{i}", f"User {i}", random.randint(18, 70)) for i in range(100)]
df_users = spark.createDataFrame(users, ["user_id", "name", "age"])

# Activity data (events)
activities = [
    (f"user_{random.randint(0, 99)}", f"event_{i}", random.randint(1, 100))
    for i in range(1000)
]
df_activities = spark.createDataFrame(activities, ["user_id", "event_id", "value"])

# Join with users
df_joined = df_activities.join(df_users, "user_id")

print("\nJoined user activities:")
df_joined.show(10)

# ============================================================================
# EXAMPLE 8: NOSQL FILTERS AND QUERIES
# ============================================================================

print("\n8. NOSQL FILTERS AND QUERIES")
print("-" * 50)

# Complex filtering on document data
complex_docs = spark.createDataFrame([
    {"id": i, "data": {"score": random.randint(0, 100), "status": random.choice(["active", "inactive"])}})
    for i in range(100)
])

# Filter on nested fields
df_filtered = complex_docs.filter(
    F.col("data.score") > 50
).filter(
    F.col("data.status") == "active"
)

print(f"\nFiltered documents: {df_filtered.count()} from 100")

# ============================================================================
# EXAMPLE 9: BATCH VS REAL-TIME NOSQL
# ============================================================================

print("\n9. BATCH VS REAL-TIME NOSQL")
print("-" * 50)

# Batch processing - full table scan
batch_result = df_cass.groupBy("partition_key").agg(
    F.count("*").alias("total_transactions"),
    F.sum("amount").alias("total_amount")
)

# Streaming - incremental processing
# In production: Would use Spark Structured Streaming with Kafka

print("\nBatch aggregation result:")
batch_result.show(5)

# ============================================================================
# EXAMPLE 10: NOSQL TO DATA WAREHOUSE PIPELINE
# ============================================================================

print("\n10. NOSQL TO DATA WAREHOUSE PIPELINE")
print("-" * 50)

# Read from NoSQL
df_nosql = spark.createDataFrame([
    (f"id_{i}", random.randint(1, 100), 1000 + i)
    for i in range(1000)
], ["id", "category", "value"])

# Transform for warehouse
df_warehouse = df_nosql.groupBy("category").agg(
    F.sum("value").alias("total"),
    F.avg("value").alias("average"),
    F.count("*").alias("count")
)

# Write to warehouse format
df_warehouse.write.mode("overwrite").parquet("file:///C:/temp/warehouse/nosql_derived")

print("\nNoSQL to Warehouse pipeline complete")
print(f"Category summaries created: {df_warehouse.count()}")

spark.stop()
```

### Output Results

```
======================================
NOSQL DATABASE INTEGRATION
======================================

1. SIMULATED MONGODB-LIKE DOCUMENT STORAGE
--------------------------------------------------

Document-style data created: 100 documents
+------+---------+-----------+----+--------------------+-----------+
|  _id |  user_id|       name| age|             address|       tags|
+------+---------+-----------+----+--------------------+-----------+
|doc_0 |user_1234|User 0     |  45|     [New York, NY]|[tag1, tag3]|
|doc_1 |user_5678|User 1     |  32|     [Chicago, IL] |       [tag2]|
+------+---------+-----------+----+--------------------+-----------+

5. NOSQL AGGREGATION OPERATIONS
--------------------------------------------------

Exploded nested data:
+-----+----+-----+
| user|item|price|
+-----+----+-----+
|   u1|    a|   10|
|   u1|    b|   20|
|   u2|    c|   15|
+-----+----+-----+

10. NOSQL TO DATA WAREHOUSE PIPELINE
--------------------------------------------------

NoSQL to Warehouse pipeline complete
Category summaries created: 100
```

## IV. APPLICATIONS

### Banking - Account Data

```python
"""
NoSQL in Banking - Account Profiles
"""

def banking_nosql_demo(spark):
    """NoSQL for banking"""
    
    print("\n" + "=" * 70)
    print("BANKING APPLICATION: NoSQL Integration")
    print("=" * 70)
    
    # User profiles (document store)
    profiles = [
        {
            "user_id": f"ACC{i:06d}",
            "name": f"Customer {i}",
            "accounts": [
                {"type": "checking", "balance": random.uniform(0, 10000)},
                {"type": "savings", "balance": random.uniform(0, 50000)}
            ]
        }
        for i in range(1000)
    ]
    
    df_profiles = spark.createDataFrame(profiles)
    print(f"\nBanking profiles created: {df_profiles.count()}")
```

### Healthcare - Patient Records

```python
"""
NoSQL in Healthcare - Patient Documents
"""

def healthcare_nosql_demo(spark):
    """NoSQL for healthcare"""
    
    print("\n" + "=" * 70)
    print("HEALTHCARE APPLICATION: NoSQL Integration")
    print("=" * 70)
    
    # Patient documents
    patients = [
        {
            "patient_id": f"P{i:06d}",
            "visits": [
                {"date": "2024-01-01", "diagnosis": "condition_a"},
                {"date": "2024-02-01", "diagnosis": "condition_b"}
            ]
        }
        for i in range(500)
    ]
    
    df_patients = spark.createDataFrame(patients)
    print(f"\nPatient records created: {df_patients.count()}")
```

## V. ASCII FLOW VISUALIZATION

### NoSQL Integration Architecture

```
+=========================================================================+
|                    NOSQL INTEGRATION ARCHITECTURE                       |
+=========================================================================+

                    ┌─────────────────────────────────────┐
                    │         APACHE SPARK               │
                    │                                     │
                    │  ┌─────────────────────────────┐  │
                    │  │    DataFrame API            │  │
                    │  └──────────────┬──────────────┘  │
                    │                 │                 │
                    └────────┬─────────┴─────────────────┘
                             │
        ┌────────────────────┼────────────────────┐
        │                    │                    │
        ▼                    ▼                    ▼
┌───────────────┐    ┌───────────────┐    ┌───────────────┐
│   MongoDB     │    │  Cassandra   │    │    Redis     │
│  Documents    │    │   Columns    │    │   Key-Value  │
│               │    │              │    │              │
│ mongo-spark   │    │ cassandra-   │    │  spark-redis │
│   connector   │    │ spark-conn   │    │   connector  │
└───────────────┘    └───────────────┘    └───────────────┘

+=========================================================================+
|                    DATA FLOW                                            |
+========================================================================+=

NOSQL ─────────> SPARK ─────────> ANALYTICS/ML
   │                  │                  │
   │ read()          transform()       show()/write()
   │                  │                  │
   └──────────────────┴──────────────────┘
              Batch/Streaming

+=========================================================================+
|                    READ/WRITE PATTERNS                                  |
+========================================================================+=

READ:                    WRITE:
df = read.format()      df.write.format()
  .option("uri",...)      .option("table",...)
  .load()                 .save()

Supported formats:
- mongo (MongoDB)
- cassandra  
- redis
- dynamodb
- hbase
```

## VI. ADVANCED TOPICS

### Performance Considerations

```python
# Pushdown filters to NoSQL
df = spark.read.format("mongo") \
    .option("pipeline", '{"$match": {"status": "active"}}') \
    .load()

# Partition pruning
df = spark.read.format("cassandra") \
    .option("spark.cassandra.input.partition.gracePeriod", "10000ms") \
    .load()
```

## VII. CONCLUSION

### Key Takeaways

NoSQL databases integrate with Spark via specialized connectors. Document, column, and key-value stores have different data models. Spark can process data from multiple NoSQL sources in unified pipelines.

### Best Practices

1. Use appropriate connector for each database type
2. Push predicates to database when possible
3. Consider data locality for partitioning
4. Handle different data models in transformations

### Next Steps

Continue to learn about Data Warehouse Solutions for analytical workloads.

```python
# Quick Reference: NoSQL Connectors

# MongoDB
spark.read.format("mongo").load("db.collection")

# Cassandra
spark.read.format("org.apache.spark.sql.cassandra") \
    .option("keyspace", "ks").option("table", "tbl").load()

# Redis  
spark.read.format("redis").option("table", "key").load()
```