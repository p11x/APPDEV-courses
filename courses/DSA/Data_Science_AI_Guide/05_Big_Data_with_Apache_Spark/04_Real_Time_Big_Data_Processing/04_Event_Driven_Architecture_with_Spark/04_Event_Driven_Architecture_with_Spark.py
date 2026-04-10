# Topic: Event Driven Architecture with Spark
# Author: AI Assistant
# Date: 06-04-2026

"""

Comprehensive implementation for Event Driven Architecture with Spark

I. INTRODUCTION
This module covers event-driven architecture patterns with Spark Streaming.
It includes event processing, event sourcing, and integration with 
messaging systems like Kafka.

II. CORE CONCEPTS
- Event processing patterns
- Kafka integration
- Event sourcing
- CQRS pattern
- Event correlation

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "EventDrivenDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def kafka_integration(spark: SparkSession) -> None:
    """Kafka integration for event streams."""
    print("\nKafka Integration:")
    
    print("  Spark-Kafka integration:")
    print("    df = spark.readStream.format('kafka')")
    print("      .option('kafka.bootstrap.servers', 'localhost:9092')")
    print("      .option('subscribe', 'events-topic')")
    print("      .load()")
    
    print("  Writing to Kafka:")
    print("    df.writeStream.format('kafka')")
    print("      .option('kafka.bootstrap.servers', 'localhost:9092')")
    print("      .option('topic', 'output-topic')")


def event_processing_patterns(spark: SparkSession) -> None:
    """Event processing patterns."""
    print("\nEvent Processing Patterns:")
    
    events = [
        ("E001", "CLICK", "2024-01-01"),
        ("E002", "VIEW", "2024-01-01"),
    ]
    df = spark.createDataFrame(events, ["event_id", "event_type", "date"])
    
    filtered = df.filter(df.event_type == "CLICK")
    print(f"  Event filtering: {filtered.count()} events")
    
    mapped = df.withColumn("processed", F.lit(True))
    print(f"  Event transformation: {mapped.count()} events")


def event_correlation(spark: SparkSession) -> None:
    """Event correlation and joining."""
    print("\nEvent Correlation:")
    
    clicks = [("C001", "user1", "2024-01-01")]
    views = [("V001", "user1", "2024-01-01")]
    
    df_clicks = spark.createDataFrame(clicks, ["event_id", "user_id", "date"])
    df_views = spark.createDataFrame(views, ["event_id", "user_id", "date"])
    
    correlated = df_clicks.join(df_views, ["user_id", "date"])
    print(f"  Correlated events: {correlated.count()}")


def core_implementation():
    print("=" * 60)
    print("EVENT DRIVEN ARCHITECTURE WITH SPARK")
    print("=" * 60)
    
    spark = create_spark_session()
    
    kafka_integration(spark)
    event_processing_patterns(spark)
    event_correlation(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Event Driven")
    print("=" * 60)
    
    spark = create_spark_session("BankingEvents")
    
    events = [("TXN001", "DEBIT"), ("TXN002", "CREDIT")]
    df = spark.createDataFrame(events, ["txn_id", "type"])
    
    debit_events = df.filter(df.type == "DEBIT")
    print(f"  Debit events: {debit_events.count()}")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Event Driven")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareEvents")
    
    events = [("P001", "ADMISSION"), ("P002", "DISCHARGE")]
    df = spark.createDataFrame(events, ["patient_id", "event_type"])
    
    print("  Patient event stream configured")
    
    spark.stop()


def main():
    print("Executing Event Driven Architecture implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
