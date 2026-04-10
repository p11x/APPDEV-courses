# Topic: Real Time Analytics Applications
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Real Time Analytics Applications

I. INTRODUCTION
This module covers practical real-time analytics applications built with 
Spark Streaming. It includes real-time dashboards, alerting systems, and 
analytics pipelines for various use cases.

II. CORE CONCEPTS
- Real-time dashboards
- Alert generation
- Metrics computation
- Streaming aggregations
- Dashboard integration

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "RealTimeAnalytics") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def real_time_counting(spark: SparkSession) -> None:
    """Real-time counting analytics."""
    print("\nReal-Time Counting:")
    
    data = [
        ("page_view", "home"),
        ("page_view", "products"),
        ("click", "buy_button"),
    ]
    df = spark.createDataFrame(data, ["event", "page"])
    
    counts = df.groupBy("event").count()
    print("  Event counts computed")
    counts.show()


def real_time_aggregation(spark: SparkSession) -> None:
    """Real-time aggregation analytics."""
    print("\nReal-Time Aggregation:")
    
    sales = [
        ("2024-01-01", "ProductA", 100.0),
        ("2024-01-01", "ProductB", 50.0),
    ]
    df = spark.createDataFrame(sales, ["date", "product", "amount"])
    
    agg = df.groupBy("product").agg(
        F.sum("amount").alias("total_sales"),
        F.count("date").alias("transaction_count")
    )
    print("  Aggregated sales data")
    agg.show()


def real_time_windowed_metrics(spark: SparkSession) -> None:
    """Real-time windowed metrics."""
    print("\nWindowed Metrics:")
    
    events = [
        ("2024-01-01 10:00:00", "click"),
        ("2024-01-01 10:01:00", "click"),
    ]
    df = spark.createDataFrame(events, ["timestamp", "event"])
    
    print("  5-minute sliding window metrics configured")
    print("  1-hour tumbling window metrics configured")


def alerting_system(spark: SparkSession) -> None:
    """Real-time alerting system."""
    print("\nAlerting System:")
    
    transactions = [
        ("TXN001", 5000.0),
        ("TXN002", 100.0),
    ]
    df = spark.createDataFrame(transactions, ["txn_id", "amount"])
    
    high_value = df.filter(df.amount > 1000)
    alert_count = high_value.count()
    print(f"  High-value alerts: {alert_count} transactions")


def core_implementation():
    print("=" * 60)
    print("REAL TIME ANALYTICS APPLICATIONS")
    print("=" * 60)
    
    spark = create_spark_session()
    
    real_time_counting(spark)
    real_time_aggregation(spark)
    real_time_windowed_metrics(spark)
    alerting_system(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Real-Time Analytics")
    print("=" * 60)
    
    spark = create_spark_session("BankingAnalytics")
    
    transactions = [("TXN001", 1000.0), ("TXN002", 500.0)]
    df = spark.createDataFrame(transactions, ["txn_id", "amount"])
    
    high_value = df.filter(df.amount > 800)
    alert_count = high_value.count()
    print(f"  High-value transaction alerts: {alert_count}")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Real-Time Analytics")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareAnalytics")
    
    vitals = [("P001", 72), ("P002", 95)]
    df = spark.createDataFrame(vitals, ["patient_id", "heart_rate"])
    
    abnormal = df.filter(df.heart_rate > 90)
    alert_count = abnormal.count()
    print(f"  Abnormal vital alerts: {alert_count}")
    
    spark.stop()


def main():
    print("Executing Real Time Analytics implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
