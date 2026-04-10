# Spark Streaming Basics

## I. INTRODUCTION

### What is Spark Streaming?
Spark Streaming is Apache Spark's API for processing real-time data streams. It enables scalable, high-throughput, fault-tolerant processing of live data streams from sources like Kafka, Flume, and TCP sockets. Data is processed in mini-batches, providing near real-time analytics capabilities while leveraging Spark's batch processing engine.

### Why is it Important in Big Data?
Spark Streaming enables organizations to process and analyze data as it arrives, supporting use cases like real-time fraud detection, live customer dashboards, and instant alerting. It provides a unified programming model for both batch and streaming workloads. It scales to handle millions of events per second.

### Prerequisites
Understanding of Spark DataFrames required. Knowledge of DStream concepts helpful. Familiarity with data sources helpful.

## II. FUNDAMENTALS

### Streaming Architecture

#### DStream (Discretized Stream)
The basic abstraction in Spark Streaming. Represents a continuous stream of data divided into micro-batches. Each micro-batch is an RDD. Operations on DStreams are applied to each micro-batch.

#### Micro-Batch Processing
Data is collected over a time window and processed as small batches. Default batch interval is 1 second. Trade-off between latency and throughput.

### Spark Streaming Flow

```
Input Streams → DStream → Transformations → Output → Storage/Sink
```

## III. IMPLEMENTATION

```python
"""
Spark Streaming Basics Demonstration
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.streaming import StreamingContext
import random

spark = SparkSession.builder \
    .appName("StreamingDemo") \
    .master("local[*]") \
    .getOrCreate()

spark.sparkContext.setLogLevel("ERROR")

print("=" * 70)
print("SPARK STREAMING BASICS")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: BASIC STREAMING CONTEXT
# ============================================================================

print("\n1. BASIC STREAMING CONTEXT")
print("-" * 50)

# Create StreamingContext with 5-second batch interval
ssc = StreamingContext(spark.sparkContext, 5)

# Create DStream from socket
lines = ssc.socketTextStream("localhost", 9999)

# For demo, use mock data
# In production: Use Kafka, Flume, or other sources

print("StreamingContext created with 5-second batch interval")
print("Note: Production would connect to real streaming sources")

# ============================================================================
# EXAMPLE 2: STREAM TRANSFORMATIONS
# ============================================================================

print("\n2. STREAM TRANSFORMATIONS")
print("-" * 50)

# Simulate incoming data
mock_data = [
    ("word1 word2 word1",),
    ("word3 word1 word2",),
    ("word2 word3 word1",),
]

# Process as batch for demonstration
df_batch = spark.createDataFrame(mock_data, ["text"])

# Word count transformation
words = df_batch.select(F.explode(F.split("text", " ")).alias("word"))
word_counts = words.groupBy("word").count()

print("\nWord count from stream:")
word_counts.show()

# ============================================================================
# EXAMPLE 3: WINDOW OPERATIONS
# ============================================================================

print("\n3. WINDOW OPERATIONS")
print("-" * 50)

# Create sample time-series data
ts_data = [
    (f"2024-01-01 {i:02d}:00:00", random.randint(1, 100))
    for i in range(24)
]
df_ts = spark.createDataFrame(ts_data, ["timestamp", "value"])

# Window operations
from pyspark.sql.window import Window

window_spec = Window.orderBy("timestamp").rowsBetween(-2, 0)

df_windowed = df_ts.withColumn(
    "moving_avg_3h",
    F.avg("value").over(window_spec)
)

print("\nTime-series with moving average:")
df_windowed.show(10)

# ============================================================================
# EXAMPLE 4: STREAM AGGREGATIONS
# ============================================================================

print("\n4. STREAM AGGREGATIONS")
print("-" * 50)

# Simulated clickstream data
clickstream = [
    ("page_1", "user_1"),
    ("page_2", "user_2"),
    ("page_1", "user_3"),
    ("page_3", "user_1"),
]
df_clicks = spark.createDataFrame(clickstream, ["page", "user"])

# Aggregations
page_stats = df_clicks.groupBy("page").agg(
    F.count("*").alias("views"),
    F.countDistinct("user").alias("unique_users")
).orderBy(F.desc("views"))

print("\nPage views from stream:")
page_stats.show()

# ============================================================================
# EXAMPLE 5: STATEFUL STREAMING
# ============================================================================

print("\n5. STATEFUL STREAMING")
print("-" * 50)

# Running counts with updateStateByKey
# In production: checkpointing enabled

# Simulated stateful data
events = [
    ("session_1", 1),
    ("session_1", 1),
    ("session_2", 1),
]

# Running total calculation
running = {}
for session, count in events:
    running[session] = running.get(session, 0) + count

print("\nRunning session counts:")
for k, v in running.items():
    print(f"  {k}: {v}")

# ============================================================================
# EXAMPLE 6: OUTPUT MODES
# ============================================================================

print("\n6. OUTPUT MODES")
print("-" * 50)

# Complete mode - entire result
# Append mode - new rows only
# Update mode - changed rows only

print("Output modes available:")
print("  - COMPLETE: Full result each batch")
print("  - APPEND: New rows only (default)")
print("  - UPDATE: Changed rows only")

# ============================================================================
# EXAMPLE 7: CHECKPOINTING
# ============================================================================

print("\n7. CHECKPOINTING")
print("-" * 50)

# Enable checkpoint for stateful streaming
checkpoint_dir = "file:///C:/temp/checkpoint"

ssc.checkpoint(checkpoint_dir)

print(f"\nCheckpoint directory: {checkpoint_dir}")
print("Note: Required for stateful streaming")

# ============================================================================
# EXAMPLE 8: SINK OPERATIONS
# ============================================================================

print("\n8. SINK OPERATIONS")
print("-" * 50)

# Write stream to various sinks
# Console - for debugging
# File - for persistence  
# Kafka - for further streaming

print("Stream sinks:")
print("  - Console: print()")
print("  - File: write stream to Parquet/JSON")
print("  - Kafka: send to Kafka topics")
print("  - Database: write to JDBC")

# ============================================================================
# EXAMPLE 9: SOURCE CONFIGURATIONS
# ============================================================================

print("\n9. SOURCE CONFIGURATIONS")
print("-" * 50)

# Kafka source configuration
kafka_config = {
    "kafka.bootstrap.servers": "localhost:9092",
    "subscribe": "topic-name",
    "startingOffsets": "earliest"
}

print("Kafka source configuration:")
for k, v in kafka_config.items():
    print(f"  {k}: {v}")

# ============================================================================
# EXAMPLE 10: ERROR HANDLING
# ============================================================================

print("\n10. ERROR HANDLING")
print("-" * 50)

# Handle streaming errors
def process_safely(rdd):
    if rdd.isEmpty():
        return
    try:
        df = rdd.map(lambda x: (x,)).toDF(["data"])
        df.show()
    except Exception as e:
        print(f"Error processing batch: {e}")

print("Error handling strategies:")
print("  - Try-catch in transformation")
print("  - Dead letter queues")
print("  - Monitoring and alerts")

# Stop context
ssc.stop()

print("\n" + "=" * 70)
print("STREAMING BASICS COMPLETE")
print("=" * 70)
```

### Output Results

```
2. STREAM TRANSFORMATIONS
Word counts from stream

4. STREAM AGGREGATIONS
Page views computed
```

## IV. APPLICATIONS

### Banking - Real-Time Fraud Detection

```python
def banking_streaming_demo():
    # Transaction streaming
    # Anomaly detection
    # Alert generation
    pass
```

### Healthcare - Patient Monitoring

```python
def healthcare_streaming_demo():
    # Vital signs streaming
    # Alert thresholds
    # Real-time dashboards
    pass
```

## V. CONCLUSION

Spark Streaming provides real-time processing. DStreams and micro-batches enable low-latency analytics.

```python
# Quick Reference
ssc = StreamingContext(spark, 1)
lines = ssc.socketTextStream("localhost", 9999)
words = lines.flatMap(lambda x: x.split(" "))
```
