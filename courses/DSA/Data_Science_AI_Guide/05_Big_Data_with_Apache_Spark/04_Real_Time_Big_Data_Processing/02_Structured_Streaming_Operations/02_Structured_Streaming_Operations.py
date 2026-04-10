# Topic: Structured Streaming Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Structured Streaming Operations

I. INTRODUCTION
Structured Streaming is a newer streaming API built on top of Spark SQL. It provides
a DataFrame-like API for streaming data with built-in support for event-time 
windows, watermarks, and stateful processing.

II. CORE CONCEPTS
- DataFrame streaming API
- Event-time windows
- Watermarks
- Stateful aggregations
- Streaming joins
- Output modes

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, TimestampType


def create_spark_session(app_name: str = "StructuredStreamDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def streaming_dataframe_basics(spark: SparkSession) -> None:
    """Demonstrate streaming DataFrame basics."""
    print("\nStreaming DataFrame Basics:")
    
    schema = StructType([
        StructField("timestamp", TimestampType(), True),
        StructField("value", IntegerType(), True),
        StructField("category", StringType(), True),
    ])
    
    print(f"  Schema defined: {schema.simpleString()}")
    print("  Note: Use spark.readStream() to create streaming DataFrame")


def event_time_windows(spark: SparkSession) -> None:
    """Demonstrate event-time window operations."""
    print("\nEvent-Time Windows:")
    
    data = [
        ("2024-01-01 10:00:00", "A", 100),
        ("2024-01-01 10:01:00", "B", 200),
    ]
    df = spark.createDataFrame(data, ["timestamp", "category", "value"])
    
    print("  Tumbling window example:")
    result = df.groupBy(
        F.window(F.col("timestamp"), "5 minutes"),
        "category"
    ).agg(F.sum("value").alias("total"))
    print(f"    Result: {result.columns}")
    
    print("  Sliding window example:")
    print("    window(timeColumn, '5 minutes', '1 minute')")
    
    print("  Session window example:")
    print("    session_window(timeColumn, '10 minutes')")


def watermarks(spark: SparkSession) -> None:
    """Demonstrate watermarks."""
    print("\nWatermarks:")
    
    print("  With watermark for late data handling:")
    print("    .withWatermark('timestamp', '10 minutes')")
    
    print("  Watermark benefits:")
    print("    - Handles late-arriving data")
    print("    - Enables state cleanup")
    print("    - Reduces memory usage")


def stateful_aggregations(spark: SparkSession) -> None:
    """Demonstrate stateful aggregations."""
    print("\nStateful Aggregations:")
    
    data = [
        ("A", 100), ("B", 200), ("A", 150)
    ]
    df = spark.createDataFrame(data, ["category", "value"])
    
    print("  GroupBy with state:")
    result = df.groupBy("category").agg(
        F.sum("value").alias("total"),
        F.count("value").alias("count")
    )
    print(f"    Aggregated: {result.columns}")


def streaming_joins(spark: SparkSession) -> None:
    """Demonstrate streaming joins."""
    print("\nStreaming Joins:")
    
    stream1 = [
        ("2024-01-01", "A", 100),
    ]
    stream2 = [
        ("A", "Product1"),
    ]
    df1 = spark.createDataFrame(stream1, ["time", "key", "value"])
    df2 = spark.createDataFrame(stream2, ["key", "product"])
    
    print("  Inner join with stream:")
    result = df1.join(df2, "key")
    print(f"    Joined columns: {result.columns}")


def output_modes(spark: SparkSession) -> None:
    """Demonstrate output modes."""
    print("\nOutput Modes:")
    print("  append: Only new rows added to result table")
    print("  complete: Entire result table rewritten")
    print("  update: Only changed rows updated")


def core_implementation():
    print("=" * 60)
    print("STRUCTURED STREAMING OPERATIONS")
    print("=" * 60)
    
    spark = create_spark_session()
    
    streaming_dataframe_basics(spark)
    event_time_windows(spark)
    watermarks(spark)
    stateful_aggregations(spark)
    streaming_joins(spark)
    output_modes(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Structured Streaming")
    print("=" * 60)
    
    spark = create_spark_session("BankingStructured")
    
    transactions = [
        ("2024-01-01 10:00:00", "ACC001", 100.0),
    ]
    df = spark.createDataFrame(transactions, ["timestamp", "account", "amount"])
    
    windowed = df.groupBy(
        F.window(F.col("timestamp"), "1 hour"),
        "account"
    ).agg(F.sum("amount").alias("total"))
    
    print("  Real-time account aggregation configured")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Structured Streaming")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareStructured")
    
    vitals = [
        ("2024-01-01 10:00:00", "P001", 72),
    ]
    df = spark.createDataFrame(vitals, ["timestamp", "patient", "heart_rate"])
    
    windowed = df.groupBy(
        F.window(F.col("timestamp"), "5 minutes"),
        "patient"
    ).agg(F.avg("heart_rate").alias("avg_hr"))
    
    print("  Real-time vital signs monitoring configured")
    
    spark.stop()


def main():
    print("Executing Structured Streaming Operations implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
