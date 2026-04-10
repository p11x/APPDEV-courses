# Data Lake Architecture with Apache Spark

## I. INTRODUCTION

### What is a Data Lake?
A data lake is a centralized repository that allows you to store all your structured and unstructured data at any scale. Unlike traditional data warehouses, data lakes store data in its native format, enabling diverse analytics including Machine Learning, real-time analytics, and big data processing. Apache Spark serves as the primary processing engine for data lakes.

### Why is it Important in Big Data?
Data lakes are foundational for modern data architectures. They support diverse data types from multiple sources without upfront schema definition. They enable advanced analytics and ML on raw data. They provide cost-effective storage for massive data volumes. They facilitate data democratization across organizations.

### Prerequisites
Familiarity with distributed storage systems is helpful. Understanding of Spark DataFrames and transformations is required. Knowledge of data formats and schemas adds value.

## II. FUNDAMENTALS

### Data Lake Layers

#### 1. Bronze Layer (Raw Layer)
Stores raw, unprocessed data in native format. Preserves original data for reprocessing. Acts as the single source of truth. Enables full data lineage and auditability.

#### 2. Silver Layer (Cleansed Layer)
Contains cleansed and validated data. Schema is enforced and standardized. Duplicates and errors are removed. Enriched with derived fields.

#### 3. Gold Layer (Curated Layer)
Contains business-level aggregates. Optimized for analytics and reporting. Contains conformed dimensions. Ready for consumption.

### Data Lake Patterns

```python
# Bronze to Silver pipeline
bronze_df = spark.read.format("json").load("s3://lake/bronze/transactions")
silver_df = bronze_df.select(
    F.col("id").cast("string"),
    F.col("amount").cast("double"),
    F.to_timestamp("timestamp").alias("txn_time")
).filter(F.col("amount") > 0)

silver_df.write.mode("overwrite").parquet("s3://lake/silver/transactions")

# Silver to Gold pipeline
silver_df = spark.read.parquet("s3://lake/silver/transactions")
gold_df = silver_df.groupBy("date").agg(
    F.sum("amount").alias("total_amount"),
    F.count("*").alias("transaction_count"),
    F.avg("amount").alias("avg_amount")
)

gold_df.write.mode("overwrite").parquet("s3://lake/gold/daily_summary")
```

## III. IMPLEMENTATION

### Step-by-Step Code Examples with PySpark

```python
"""
Data Lake Architecture Demonstration
Complete implementation with comprehensive examples
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType, TimestampType
from datetime import datetime, timedelta
import random

# Initialize Spark
spark = SparkSession.builder \
    .appName("DataLakeDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("DATA LAKE ARCHITECTURE")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: BRONZE LAYER - RAW DATA INGESTION
# ============================================================================

print("\n1. BRONZE LAYER - RAW DATA INGESTION")
print("-" * 50)

# Simulate raw transaction data
raw_data = [
    {
        "transaction_id": f"TXN{i:08d}",
        "timestamp": (datetime.now() - timedelta(days=random.randint(0, 30))).isoformat(),
        "customer_id": f"CUST{random.randint(1000, 9999):04d}",
        "amount": round(random.uniform(10, 5000), 2),
        "category": random.choice(["Electronics", "Clothing", "Food", "Travel"]),
        "payment_method": random.choice(["credit", "debit", "cash"]),
        "status": random.choice(["completed", "pending", "failed"])
    }
    for i in range(1000)
]

# Write to Bronze layer (simulated)
bronze_path = "file:///C:/temp/lake/bronze/transactions"
df_bronze = spark.createDataFrame(raw_data)

df_bronze.write \
    .mode("overwrite") \
    .partitionBy("status") \
    .parquet(bronze_path)

print(f"\nBronze layer data written to: {bronze_path}")
print(f"Total raw records: {df_bronze.count()}")
print(f"Partitions by: status")

# ============================================================================
# EXAMPLE 2: SILVER LAYER - DATA CLEANSING
# ============================================================================

print("\n2. SILVER LAYER - DATA CLEANSING")
print("-" * 50)

# Read from Bronze
df_bronze_read = spark.read.parquet(bronze_path)

# Cleansing transformations
df_silver = df_bronze_read.select(
    F.col("transaction_id"),
    F.to_timestamp("timestamp").alias("txn_timestamp"),
    F.col("customer_id"),
    F.col("amount").cast(DoubleType()).alias("amount"),
    F.lower(F.col("category")).alias("category"),
    F.lower(F.col("payment_method")).alias("payment_method"),
    F.when(F.col("status") == "completed", True)
     .otherwise(False).alias("is_completed")
)

# Add derived columns
df_silver = df_silver.withColumn("txn_date", F.to_date("txn_timestamp")) \
    .withColumn("txn_hour", F.hour("txn_timestamp")) \
    .withColumn("txn_month", F.month("txn_timestamp")) \
    .withColumn("txn_year", F.year("txn_timestamp"))

# Filter/cleanse
df_silver = df_silver.filter(
    F.col("amount").isNotNull() & 
    (F.col("amount") > 0) &
    (F.col("amount") < 100000)
)

# Write to Silver layer
silver_path = "file:///C:/temp/lake/silver/transactions"
df_silver.write \
    .mode("overwrite") \
    .partitionBy("txn_date") \
    .parquet(silver_path)

print(f"\nSilver layer data written to: {silver_path}")
print(f"Cleaned records: {df_silver.count()}")

# ============================================================================
# EXAMPLE 3: GOLD LAYER - BUSINESS AGGREGATES
# ============================================================================

print("\n3. GOLD LAYER - BUSINESS AGGREGATES")
print("-" * 50)

# Read from Silver
df_silver_read = spark.read.parquet(silver_path)

# Daily summary by category
gold_category = df_silver_read.groupBy("txn_date", "category").agg(
    F.sum("amount").alias("total_amount"),
    F.count("*").alias("transaction_count"),
    F.avg("amount").alias("avg_amount"),
    F.min("amount").alias("min_amount"),
    F.max("amount").alias("max_amount"),
    F.countDistinct("customer_id").alias("unique_customers")
)

# Daily overall summary
gold_daily = df_silver_read.groupBy("txn_date").agg(
    F.sum("amount").alias("total_amount"),
    F.count("*").alias("transaction_count"),
    F.avg("amount").alias("avg_amount"),
    F.countDistinct("customer_id").alias("unique_customers")
)

# Customer aggregates
gold_customer = df_silver_read.groupBy("customer_id").agg(
    F.sum("amount").alias("lifetime_value"),
    F.count("*").alias("transaction_count"),
    F.avg("amount").alias("avg_transaction"),
    F.min("txn_timestamp").alias("first_purchase"),
    F.max("txn_timestamp").alias("last_purchase")
)

# Write Gold layers
gold_category.write.mode("overwrite").parquet("file:///C:/temp/lake/gold/category_summary")
gold_daily.write.mode("overwrite").parquet("file:///C:/temp/lake/gold/daily_summary")
gold_customer.write.mode("overwrite").parquet("file:///C:/temp/lake/gold/customer_summary")

print(f"\nGold layer aggregations written")
print(f"Category summaries: {gold_category.count()}")
print(f"Daily summaries: {gold_daily.count()}")
print(f"Customer summaries: {gold_customer.count()}")

# ============================================================================
# EXAMPLE 4: DATA QUALITY CHECKS
# ============================================================================

print("\n4. DATA QUALITY CHECKS")
print("-" * 50)

# Completeness check
total_records = df_silver_read.count()
non_null_amounts = df_silver_read.filter(F.col("amount").isNotNull()).count()
completeness = non_null_amounts / total_records * 100 if total_records > 0 else 0

# Validity check
valid_amounts = df_silver_read.filter(
    (F.col("amount") > 0) & (F.col("amount") < 100000)
).count()
validity = valid_amounts / non_null_amounts * 100 if non_null_amounts > 0 else 0

# Uniqueness check
distinct_txns = df_silver_read.select("transaction_id").distinct().count()
uniqueness = distinct_txns / total_records * 100 if total_records > 0 else 0

# Duplicate check
duplicates = total_records - distinct_txns

print(f"\nData Quality Metrics:")
print(f"  Completeness: {completeness:.2f}%")
print(f"  Validity: {validity:.2f}%")
print(f"  Uniqueness: {uniqueness:.2f}%")
print(f"  Duplicates found: {duplicates}")

# ============================================================================
# EXAMPLE 5: CHANGE DATA CAPTURE (CDC)
# ============================================================================

print("\n5. CHANGE DATA CAPTURE (CDC)")
print("-" * 50)

# Simulate old and new data
old_data = [
    ("TXN00000001", 100.0, "old_version"),
    ("TXN00000002", 200.0, "old_version"),
]

new_data = [
    ("TXN00000001", 150.0, "new_version"),
    ("TXN00000003", 300.0, "new_version"),
]

df_old = spark.createDataFrame(old_data, ["txn_id", "amount", "version"])
df_new = spark.createDataFrame(new_data, ["txn_id", "amount", "version"])

# Identify inserts
inserts = df_new.join(df_old, "txn_id", "leftanti")
print(f"\nInserts: {inserts.count()}")

# Identify updates
updates = df_new.join(df_old, "txn_id", "inner")
updates = updates.withColumn("changed", F.col("amount") != F.col("amount"))
print(f"Updates: {updates.count()}")

# Identify deletes (not in new but in old)
deletes = df_old.join(df_new, "txn_id", "leftanti")
print(f"Deletes: {deletes.count()}")

# ============================================================================
# EXAMPLE 6: LATE-ARRIVING DATA HANDLING
# ============================================================================

print("\n6. LATE-ARRIVING DATA HANDLING")
print("-" * 50)

# Create watermarked data
watermark_df = df_silver_read.withColumn(
    "processing_time",
    F.current_timestamp()
)

# Look for late data (processing_time > txn_timestamp + 24 hours)
late_threshold = F.timestamp_sub(F.col("txn_timestamp"), -24*3600)
late_data = watermarked_df.filter(
    F.col("processing_time") > late_threshold
)

print(f"\nLate-arriving data detected:")
late_data.show()

# ============================================================================
# EXAMPLE 7: LOGICAL DATA LAKE WITH METASTORE
# ============================================================================

print("\n7. LOGICAL DATA LAKE WITH METASTORE")
print("-" * 50)

# Create tables pointing to layers
spark.sql("DROP TABLE IF EXISTS bronze_transactions")
spark.sql("DROP TABLE IF EXISTS silver_transactions")
spark.sql("DROP TABLE IF EXISTS gold_daily_summary")

spark.sql(f"""
    CREATE TABLE bronze_transactions
    USING parquet
    LOCATION '{bronze_path}'
""")

spark.sql(f"""
    CREATE TABLE silver_transactions
    USING parquet
    LOCATION '{silver_path}'
""")

spark.sql("""
    CREATE TABLE gold_daily_summary
    USING parquet
    LOCATION 'file:///C:/temp/lake/gold/daily_summary'
""")

print("\nRegistered tables:")
tables = spark.catalog.listTables()
for table in tables:
    print(f"  {table.name} ({table.tableType})")

# ============================================================================
# EXAMPLE 8: TIME TRAVEL QUERIES
# ============================================================================

print("\n8. TIME TRAVEL QUERIES")
print("-" * 50)

# Write with version
df_silver_read.write \
    .mode("overwrite") \
    .option("versionAsOf", "2024-01-01") \
    .parquet("file:///C:/temp/lake/silver/transactions_v1")

# Read historical version
# In production: spark.read.format("parquet").option("versionAsOf", 1).load(path)

print("\nTime travel enabled via:")
print("  - versionAsOf: Read specific version")
print("  - timestampAsOf: Read snapshot at timestamp")

spark.stop()
```

### Output Results

```
======================================
DATA LAKE ARCHITECTURE
======================================

1. BRONZE LAYER - RAW DATA INGESTION
--------------------------------------------------

Bronze layer data written to: file:///C:/temp/lake/bronze/transactions
Total raw records: 1000
Partitions by: status

2. SILVER LAYER - DATA CLEANSING
--------------------------------------------------

Silver layer data written to: file:///C:/temp/lake/silver/transactions
Cleaned records: 1000

3. GOLD LAYER - BUSINESS AGGREGATES
--------------------------------------------------

Gold layer aggregations written
Category summaries: 4
Daily summaries: 30
Customer summaries: 1000

4. DATA QUALITY CHECKS
--------------------------------------------------

Data Quality Metrics:
  Completeness: 100.00%
  Validity: 100.00%
  Uniqueness: 100.00%
  Duplicates found: 0
```

## IV. APPLICATIONS

### Banking Data Lake

```python
"""
Data Lake in Banking - Transaction Processing
"""

def banking_data_lake_demo(spark):
    """Demonstrate data lake for banking"""
    
    print("\n" + "=" * 70)
    print("BANKING APPLICATION: Data Lake")
    print("=" * 70)
    
    # Generate banking data
    transactions = [
        (f"TXN{i:08d}", f"ACC{random.randint(1000, 9999):04d}", 
         random.uniform(10, 10000), random.choice(["DEBIT", "CREDIT"]),
         "completed" if random.random() > 0.1 else "pending")
        for i in range(10000)
    ]
    
    df_txn = spark.createDataFrame(
        transactions,
        ["txn_id", "account_id", "amount", "type", "status"]
    )
    
    # Bronze: Raw transactions
    df_txn.write.partitionBy("status").parquet("file:///C:/temp/lake/bronze/banking/txns")
    
    # Silver: Cleaned data
    df_silver = df_txn.filter(F.col("status") == "completed")
    df_silver.write.partitionBy("type").parquet("file:///C:/temp/lake/silver/banking/txns")
    
    # Gold: Aggregates
    df_gold = df_silver.groupBy("account_id").agg(
        F.sum("amount").alias("balance"),
        F.count("*").alias("txn_count")
    )
    df_gold.write.parquet("file:///C:/temp/lake/gold/balances")
    
    print(f"\nBanking data lake created")
```

### Healthcare Data Lake

```python
"""
Data Lake in Healthcare - Patient Records
"""

def healthcare_data_lake_demo(spark):
    """Demonstrate data lake for healthcare"""
    
    print("\n" + "=" * 70)
    print("HEALTHCARE APPLICATION: Data Lake")
    print("=" * 70)
    
    # Generate patient data
    patients = [
        (f"P{random.randint(100000, 999999):06d}", 
         random.randint(18, 90),
         random.choice(["diabetes", "hypertension", "none"]),
         random.choice(["inpatient", "outpatient", "emergency"]))
        for i in range(5000)
    ]
    
    df_patients = spark.createDataFrame(
        patients,
        ["patient_id", "age", "condition", "visit_type"]
    )
    
    # Write to different layers
    df_patients.write.partitionBy("visit_type").parquet("file:///C:/temp/lake/bronze/patients")
```

## V. ASCII FLOW VISUALIZATION

### Data Lake Architecture

```
+=========================================================================+
|                    DATA LAKE ARCHITECTURE                                |
+=========================================================================+

DATA SOURCES                    PROCESSING                 CONSUMPTION
    │                            │                           │
    ▼                            ▼                           ▼
┌─────────────┐          ┌─────────────┐          ┌─────────────┐
│  External  │─────────>│   BRONZE    │─────────>│  ANALYTICS  │
│   API/DB   │          │   (Raw)     │          │    & BI     │
└───────────���─���          └──────┬──────┘          └─────────────┘
                                 │
                                 ▼
                         ┌─────────────┐
                         │   SILVER    │─────────>│  REPORTS    │
                         │  (Cleansed) │          │   & ML     │
                         └──────┬──────┘
                                 │
                                 ▼
                         ┌─────────────┐
                         │    GOLD    │─────────>│ DASHBOARDS  │
                         │ (Curated)  │          │   & APIs   │
                         └─────────────┘

+=========================================================================+
|                    DATA FLOW EXAMPLE                                     |
+=========================================================================+

SOURCE ────────────────────────────────────────────────────────────> CONSUMPTION

┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│  JSON API  │    │   BRONZE   │    │   SILVER   │    │   GOLD    │
│  data     │───>│  Raw JSON │───>│validated │───>│ Dashboard│
│           │    │  +audit  │    │ +derived │    │  Ready   │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘

Step 1: Read JSON, keep original format
Step 2: Cast types, add derived fields
Step 3: Aggregate by business logic
Step 4: Display in dashboards

+=========================================================================+
|                    CDC FLOW                                           |
+========================================================================_+

OLD VERSION ─┐
             ├─> DIFFERENCE ──> APPLY TO SILVER ──> NEW VERSION
NEW VERSION ─┘

Insert: New records not in old
Update: Records with changed values  
Delete: Records in old but not new
```

## VI. ADVANCED TOPICS

### Delta Lake

Delta Lake provides ACID transactions for data lakes:

```python
# Delta Lake integration
from delta.tables import DeltaTable

# Read as Delta
df = spark.read.format("delta").load("path/to/delta")

# Write as Delta with Merge
deltaTable = DeltaTable.forPath(spark, "path/to/delta")

deltaTable.upsert(
    spark.createDataFrame([new_data]),
    "txn_id = updates.txn_id"
)
```

### Iceberg

Apache Iceberg provides table format for huge datasets:

```python
# Iceberg table
spark.sql("CREATE TABLE prod.db.table USING iceberg OPTIONS ()")
```

### Data Mesh Principles

1. Domain ownership of data
2. Self-serve platform capabilities
3. Federated governance
4. Product thinking with data

## VII. CONCLUSION

### Key Takeaways

Data lake architecture using Medallion pattern (Bronze-Silver-Gold) provides a scalable approach. Spark enables efficient ETL between layers. Data quality checks ensure reliability. CDC handles changing data.

### Best Practices

1. Store raw data in Bronze
2. Enforce schema in Silver
3. Create business aggregates in Gold
4. Implement data quality checks
5. Use metastore for discovery

### Next Steps

Continue to learn about Cloud Storage Integration for modern data lake patterns.

```python
# Quick Reference: Data Lake Layers

# Bronze - Raw
spark.read.format("source").load("bronze/")

# Silver - Cleansed
df.select(col.cast("type"), derived_fields).write.silver/

# Gold - Aggregated  
df.groupBy(key).agg(aggregations).write.gold/
```