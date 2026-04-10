# Structured Streaming Operations

## I. INTRODUCTION

### What is Structured Streaming?
Structured Streaming is Apache Spark's newer streaming API that provides exactly-once processing guarantees with DataFrame/Dataset APIs. It treats streaming data as an unbounded table, enabling SQL-like queries on streaming data. Compared to DStream API, it offers better fault tolerance, lower latency, and unified batch/streaming semantics.

### Why is it Important in Big Data?
Structured Streaming enables event-time processing, windowing, and watermark handling. It provides exactly-once semantics for reliability. It integrates seamlessly with batch DataFrames. It supports complex event processing patterns.

### Prerequisites
DataFrame API knowledge required. Streaming basics helpful.

## II. FUNDAMENTALS

### Key Concepts

#### Event Time Processing
Process data based on event timestamps rather than arrival time. Enables handling of late-arriving data.

#### Watermarks
Tracks progress of event time to know when to drop old data. Prevents unbounded state growth.

#### Trigger
Controls when to process new data. Can be frequent micro-batches or continuous.

## III. IMPLEMENTATION

```python
"""
Structured Streaming Operations
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
import random

spark = SparkSession.builder \
    .appName("StructuredStreamingDemo") \
    .master("local[*]") \
    .getOrCreate()

spark.sparkContext.setLogLevel("ERROR")

print("=" * 70)
print("STRUCTURED STREAMING OPERATIONS")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: BASIC STREAM READING
# ============================================================================

print("\n1. BASIC STREAM READING")
print("-" * 50)

# Create simulated streaming data
streaming_df = spark.readStream \
    .format("socket") \
    .option("host", "localhost") \
    .option("port", 9999) \
    .load()

# For demo, use batch data
batch_data = [(i, f"event_{i}", random.randint(1, 100)) for i in range(100)]
df_batch = spark.createDataFrame(batch_data, ["id", "event", "value"])

print(f"Stream schema: id, event, value")
df_batch.show(5)

# ============================================================================
# EXAMPLE 2: STREAM TRANSFORMATIONS
# ============================================================================

print("\n2. STREAM TRANSFORMATIONS")
print("-" * 50)

# Apply transformations
df_transformed = df_batch.select(
    "id",
    F.upper("event").alias("event_upper"),
    F.col("value") * 2.alias("value_doubled")
)

print("\nTransformed stream:")
df_transformed.show(5)

# ============================================================================
# EXAMPLE 3: WINDOW AGGREGATIONS
# ============================================================================

print("\n3. WINDOW AGGREGATIONS")
print("-" * 50)

# Create time-windowed data
ts_data = [
    ("2024-01-01 10:00:00", "page1", 1),
    ("2024-01-01 10:01:00", "page1", 1),
    ("2024-01-01 10:02:00", "page2", 1),
]
df_window = spark.createDataFrame(ts_data, ["timestamp", "page", "count"])

# Tumbling window
df_tumbling = df_window.groupBy(
    F.window("timestamp", "1 minute"),
    "page"
).agg(F.sum("count").alias("total"))

print("\nTumbling window results:")
df_tumbling.show()

# ============================================================================
# EXAMPLE 4: SLIDING WINDOWS
# ============================================================================

print("\n4. SLIDING WINDOWS")
print("-" * 50)

# Sliding window - 2 minute window, 1 minute slide
df_sliding = df_window.groupBy(
    F.window("timestamp", "2 minute", "1 minute"),
    "page"
).agg(F.sum("count").alias("total"))

print("\nSliding window results:")
df_sliding.show()

# ============================================================================
# EXAMPLE 5: WATERMARKS
# ============================================================================

print("\n5. WATERMARKS")
print("-" * 50)

# Define watermark for late data handling
# watermark = df_event.withWatermark("eventTime", "10 minutes")

print("Watermark configuration:")
print("  .withWatermark('timestamp', '10 minutes')")
print("  Late data within watermark is processed")
print("  Data older than watermark is dropped")

# ============================================================================
# EXAMPLE 6: STREAM JOIN
# ============================================================================

print("\n6. STREAM JOINS")
print("-" * 50)

# Streaming data
stream_data = [(1, "click"), (2, "view"), (3, "buy")]
df_stream = spark.createDataFrame(stream_data, ["user_id", "action"])

# Static reference data
ref_data = [(1, "John"), (2, "Jane"), (3, "Bob")]
df_ref = spark.createDataFrame(ref_data, ["user_id", "name"])

# Join stream with static
df_joined = df_stream.join(df_ref, "user_id")

print("\nStream joined with reference:")
df_joined.show()

# ============================================================================
# EXAMPLE 7: OUTPUT MODES
# ============================================================================

print("\n7. OUTPUT MODES")
print("-" * 50)

# Complete mode - entire result each trigger
# Append mode - only new rows
# Update mode - changed rows since last trigger

print("Output modes:")
print("  COMPLETE - Full aggregation each batch")
print("  APPEND  - New rows only (with watermark)")
print("  UPDATE  - Changed rows")

# ============================================================================
# EXAMPLE 8: TRIGGER TYPES
# ============================================================================

print("\n8. TRIGGER TYPES")
print("-" * 50)

# Default - micro-batch every second
# ProcessingTime - fixed interval
# Continuous - millisecond-level latency (experimental)
# Once - single batch then stop

print("Trigger types:")
print("  .trigger() - default (1 second)")
print("  .trigger(processingTime='30 seconds')")
print("  .trigger(continuous='1 second')")
print("  .trigger(once=True)")

# ============================================================================
# EXAMPLE 9: STATEFUL AGGREGATIONS
# ============================================================================

print("\n9. STATEFUL AGGREGATIONS")
print("-" * 50)

# Running total with state
running_data = [(i % 5, random.randint(1, 10)) for i in range(20)]
df_running = spark.createDataFrame(running_data, ["category", "value"])

# GroupBy maintains state across batches
df_stateful = df_running.groupBy("category").agg(
    F.sum("value").alias("running_total")
)

print("\nStateful aggregation:")
df_stateful.show()

# ============================================================================
# EXAMPLE 10: SINK OPERATIONS
# ============================================================================

print("\n10. SINK OPERATIONS")
print("-" * 50)

# Write to various sinks
# console - debugging
# memory - in-memory table
# kafka - publish to topic
# file - parquet, json, orc

print("Stream sinks:")
print("  .writeStream.outputMode('append').format('console').start()")
print("  .writeStream.outputMode('complete').format('memory').start()")
print("  .writeStream.format('kafka').start()")
print("  .writeStream.format('parquet').start()")

print("\n" + "=" * 70)
print("STRUCTURED STREAMING COMPLETE")
print("=" * 70)
```

## IV. CONCLUSION

Structured Streaming provides unified APIs for batch and streaming. Handles late data with watermarks.

```python
# Quick Reference
spark.readStream.format("kafka").option("subscribe", "topic").load()
df.writeStream.format("console").start()
```