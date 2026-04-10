# Topic: Performance Monitoring and Scaling
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Performance Monitoring and Scaling

I. INTRODUCTION
This module covers monitoring Spark Streaming applications and implementing 
auto-scaling strategies. It includes metrics collection, performance tuning, 
and dynamic resource allocation.

II. CORE CONCEPTS
- Streaming metrics monitoring
- UI and logging
- Performance tuning
- Auto-scaling strategies
- Resource management

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "MonitorScaleDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def streaming_metrics(spark: SparkSession) -> None:
    """Streaming metrics collection."""
    print("\nStreaming Metrics:")
    
    print("  Key metrics to monitor:")
    print("    - Processing time per batch")
    print("    - Input rate (records/sec)")
    print("    - Delay (seconds behind)")
    print("    - Batch duration")
    
    print("  Metrics accessible via:")
    print("    - Streaming UI (port 4040)")
    print("    - spark.sparkContext.statusTracker()")
    print("    - Custom metric collectors")


def ui_monitoring(spark: SparkSession) -> None:
    """Spark UI for streaming monitoring."""
    print("\nStreaming UI:")
    
    spark.conf.set("spark.ui.port", "4040")
    
    print("  Streaming UI shows:")
    print("    - Active batches")
    print("    - Batch statistics")
    print("    - Input statistics")
    print("    - Scheduler delays")


def performance_tuning(spark: SparkSession) -> None:
    """Performance tuning for streaming."""
    print("\nPerformance Tuning:")
    
    spark.conf.set("spark.streaming.backpressure.enabled", "true")
    spark.conf.set("spark.streaming.kafka.maxRatePerPartition", "1000")
    
    print("  Tuning parameters:")
    print("    - spark.streaming.backpressure.enabled")
    print("    - spark.streaming.kafka.maxRatePerPartition")
    print("    - spark.sql.shuffle.partitions")
    print("    - spark.default.parallelism")


def auto_scaling(spark: SparkSession) -> None:
    """Auto-scaling strategies."""
    print("\nAuto-Scaling:")
    
    spark.conf.set("spark.dynamicAllocation.enabled", "true")
    spark.conf.set("spark.dynamicAllocation.minExecutors", "1")
    spark.conf.set("spark.dynamicAllocation.maxExecutors", "10")
    
    print("  Dynamic allocation:")
    print("    - Scale based on workload")
    print("    - Configure min/max executors")
    print("    - Set scaling factors")


def resource_management(spark: SparkSession) -> None:
    """Resource management for streaming."""
    print("\nResource Management:")
    
    print("  Memory management:")
    spark.conf.set("spark.driver.memory", "2g")
    spark.conf.set("spark.executor.memory", "2g")
    
    print("  CPU management:")
    spark.conf.set("spark.executor.cores", "2")
    spark.conf.set("spark.cores.max", "4")


def core_implementation():
    print("=" * 60)
    print("PERFORMANCE MONITORING AND SCALING")
    print("=" * 60)
    
    spark = create_spark_session()
    
    streaming_metrics(spark)
    ui_monitoring(spark)
    performance_tuning(spark)
    auto_scaling(spark)
    resource_management(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Monitoring")
    print("=" * 60)
    
    spark = create_spark_session("BankingMonitor")
    
    print("  Banking monitoring:")
    print("    - Transaction throughput")
    print("    - Alert latency")
    print("    - Fraud detection timing")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Monitoring")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareMonitor")
    
    print("  Healthcare monitoring:")
    print("    - Patient alert latency")
    print("    - Vital signs processing rate")
    print("    - Emergency trigger speed")
    
    spark.stop()


def main():
    print("Executing Performance Monitoring and Scaling implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
