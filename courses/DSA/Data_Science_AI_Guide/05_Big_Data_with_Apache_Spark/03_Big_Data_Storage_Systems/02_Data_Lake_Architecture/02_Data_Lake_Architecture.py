# Topic: Data Lake Architecture
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Lake Architecture

I. INTRODUCTION
A data lake is a centralized repository that allows you to store all your structured
and unstructured data at any scale. This module covers data lake architecture patterns,
implementation with Spark, and data organization strategies.

II. CORE CONCEPTS
- Data lake vs data warehouse
- Layered architecture (bronze, silver, gold)
- Data ingestion strategies
- Schema evolution
- Data catalog and metadata management

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "DataLakeDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def bronze_layer_ingestion(spark: SparkSession) -> None:
    """Demonstrate bronze layer - raw data ingestion."""
    data = [
        ("2024-01-01", "TXN001", "ACC001", 100.00, "DEBIT"),
        ("2024-01-02", "TXN002", "ACC002", 500.00, "CREDIT"),
    ]
    df = spark.createDataFrame(
        data, ["date", "txn_id", "account_id", "amount", "type"]
    )
    
    print("\nBronze Layer (Raw Data):")
    
    output_path = "/tmp/datalake/bronze/transactions"
    df.write.mode("overwrite").parquet(output_path)
    print(f"  Written to: {output_path}")
    
    result = spark.read.parquet(output_path)
    result.show()


def silver_layer_processing(spark: SparkSession) -> None:
    """Demonstrate silver layer - cleaned and transformed data."""
    data = [
        ("2024-01-01", "TXN001", "ACC001", 100.00, "DEBIT"),
        ("2024-01-02", "TXN002", "ACC002", 500.00, "CREDIT"),
    ]
    df = spark.createDataFrame(
        data, ["date", "txn_id", "account_id", "amount", "type"]
    )
    
    print("\nSilver Layer (Cleaned Data):")
    
    cleaned = df.withColumn("amount_clean", F.abs(F.col("amount"))) \
                .withColumn("date_parsed", F.to_date(F.col("date")))
    
    output_path = "/tmp/datalake/silver/transactions"
    cleaned.write.mode("overwrite").parquet(output_path)
    print(f"  Written to: {output_path}")
    
    cleaned.show()


def gold_layer_aggregation(spark: SparkSession) -> None:
    """Demonstrate gold layer - aggregated business-level data."""
    data = [
        ("2024-01-01", "TXN001", "ACC001", 100.00, "DEBIT"),
        ("2024-01-02", "TXN002", "ACC002", 500.00, "CREDIT"),
    ]
    df = spark.createDataFrame(
        data, ["date", "txn_id", "account_id", "amount", "type"]
    )
    
    print("\nGold Layer (Aggregated Data):")
    
    aggregated = df.groupBy("account_id", "type").agg(
        F.sum("amount").alias("total_amount"),
        F.count("txn_id").alias("transaction_count")
    )
    
    output_path = "/tmp/datalake/gold/account_summary"
    aggregated.write.mode("overwrite").parquet(output_path)
    print(f"  Written to: {output_path}")
    
    aggregated.show()


def core_implementation():
    print("=" * 60)
    print("DATA LAKE ARCHITECTURE")
    print("=" * 60)
    
    spark = create_spark_session()
    
    bronze_layer_ingestion(spark)
    silver_layer_processing(spark)
    gold_layer_aggregation(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Data Lake")
    print("=" * 60)
    
    spark = create_spark_session("BankingLake")
    
    accounts = [
        ("ACC001", "John", "SAVINGS", 50000.0),
        ("ACC002", "Jane", "CHECKING", 25000.0),
    ]
    df = spark.createDataFrame(accounts, ["account_id", "name", "type", "balance"])
    
    df.write.mode("overwrite").parquet("/tmp/datalake/bronze/accounts")
    print("  Bronze layer written")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Data Lake")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareLake")
    
    patients = [
        ("P001", "Alice", "Hypertension"),
        ("P002", "Bob", "Diabetes"),
    ]
    df = spark.createDataFrame(patients, ["patient_id", "name", "condition"])
    
    df.write.mode("overwrite").parquet("/tmp/datalake/bronze/patients")
    print("  Bronze layer written")
    
    spark.stop()


def main():
    print("Executing Data Lake Architecture implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
