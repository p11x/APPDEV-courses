# Topic: Spark Architecture and Components
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Spark Architecture and Components

I. INTRODUCTION
Apache Spark is a unified analytics engine for large-scale data processing. It provides
distributed data processing capabilities across clusters of computers. This module
covers the core architecture, components, and execution model of Apache Spark.

II. CORE CONCEPTS
- Spark Core: The fundamental execution engine
- Spark SQL: For working with structured data
- Spark Streaming: Real-time data processing
- MLlib: Machine learning library
- GraphX: Graph processing

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark import SparkContext
from pyspark.conf import SparkConf
import os
import sys


def create_spark_session(app_name="SparkArchitecture"):
    """
    Create and configure a Spark session with various settings.
    
    Returns:
        SparkSession: Configured Spark session
    """
    spark = SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.driver.memory", "4g") \
        .config("spark.executor.memory", "2g") \
        .config("spark.cores.max", "4") \
        .config("spark.sql.shuffle.partitions", "8") \
        .config("spark.default.parallelism", "4") \
        .getOrCreate()
    
    return spark


def spark_context_operations():
    """
    Demonstrate SparkContext operations and configurations.
    
    SparkContext is the main entry point for Spark functionality.
    It represents the connection to a Spark cluster.
    """
    conf = SparkConf() \
        .setAppName("SparkContextDemo") \
        .setMaster("local[*]") \
        .set("spark.driver.memory", "2g") \
        .set("spark.executor.memory", "1g")
    
    sc = SparkContext(conf=conf)
    
    print("Spark Context Configuration:")
    print(f"  App Name: {sc.appName}")
    print(f"  Master: {sc.master}")
    print(f"  Default Parallelism: {sc.defaultParallelism}")
    print(f"  Spark Version: {sc.version}")
    
    sc.stop()
    return sc


def rdd_creation_and_operations(sc):
    """
    Demonstrate RDD creation and basic operations.
    
    RDDs (Resilient Distributed Datasets) are the fundamental data
    structure in Spark. They represent immutable, distributed collections
    of objects that can be processed in parallel.
    """
    data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    rdd = sc.parallelize(data, numSlices=4)
    
    print("\nRDD Operations:")
    print(f"  Number of partitions: {rdd.getNumPartitions()}")
    print(f"  First element: {rdd.first()}")
    print(f"  Count: {rdd.count()}")
    
    mapped_rdd = rdd.map(lambda x: x * 2)
    print(f"  Mapped RDD collect: {mapped_rdd.collect()}")
    
    filtered_rdd = rdd.filter(lambda x: x % 2 == 0)
    print(f"  Filtered RDD (even numbers): {filtered_rdd.collect()}")
    
    reduced_result = rdd.reduce(lambda a, b: a + b)
    print(f"  Sum reduction: {reduced_result}")
    
    return rdd


def dataframe_creation(spark):
    """
    Demonstrate DataFrame creation and basic operations.
    
    DataFrames provide a higher-level abstraction than RDDs.
    They are distributed collections of rows with named columns.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
        ("Charlie", 35, "NYC", 75000),
        ("Diana", 28, "SF", 55000),
        ("Eve", 32, "NYC", 80000)
    ]
    
    columns = ["name", "age", "city", "salary"]
    df = spark.createDataFrame(data, columns)
    
    print("\nDataFrame Operations:")
    print("  Schema:")
    df.printSchema()
    
    print("  Sample data:")
    df.show()
    
    print(f"  Number of rows: {df.count()}")
    print(f"  Number of partitions: {df.rdd.getNumPartitions()}")
    
    selected_df = df.select("name", "salary")
    print("  Selected columns:")
    selected_df.show()
    
    filtered_df = df.filter(df.age > 28)
    print("  Filtered (age > 28):")
    filtered_df.show()
    
    return df


def spark_components_demo(spark):
    """
    Demonstrate various Spark components.
    """
    print("\nSpark Components:")
    print(f"  Spark Version: {spark.version}")
    print(f"  Master: {spark.sparkContext.master}")
    
    catalog = spark.catalog
    print(f"  Current database: {catalog.currentDatabase()}")
    print(f"  List databases: {catalog.listDatabases()}")
    
    spark_conf = spark.sparkContext.getConf()
    print("  Spark Configuration:")
    for item in spark_conf.getAll():
        print(f"    {item[0]}: {item[1]}")
    
    return spark


def core_implementation():
    """Core implementation demonstrating Spark architecture."""
    print("=" * 60)
    print("SPARK ARCHITECTURE AND COMPONENTS")
    print("=" * 60)
    
    spark = create_spark_session("ArchitectureDemo")
    
    spark_context_operations()
    
    sc = spark.sparkContext
    rdd_creation_and_operations(sc)
    
    dataframe_creation(spark)
    
    spark_components_demo(spark)
    
    spark.stop()
    print("\nSpark session stopped.")


def banking_example():
    """Banking/Finance application - Customer analytics."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Customer Analytics")
    print("=" * 60)
    
    spark = create_spark_session("BankingAnalytics")
    
    transaction_data = [
        ("C001", "2024-01-01", 1500.00, "DEBIT", "ATM"),
        ("C001", "2024-01-02", 5000.00, "CREDIT", "TRANSFER"),
        ("C002", "2024-01-01", 200.00, "DEBIT", "POS"),
        ("C002", "2024-01-03", 10000.00, "CREDIT", "SALARY"),
        ("C003", "2024-01-02", 300.00, "DEBIT", "ONLINE"),
        ("C003", "2024-01-04", 7500.00, "CREDIT", "TRANSFER"),
        ("C001", "2024-01-05", 2000.00, "DEBIT", "POS"),
        ("C004", "2024-01-01", 150.00, "DEBIT", "ATM"),
        ("C004", "2024-01-06", 12000.00, "CREDIT", "SALARY"),
        ("C005", "2024-01-02", 800.00, "DEBIT", "ONLINE"),
    ]
    
    columns = ["customer_id", "date", "amount", "transaction_type", "channel"]
    df = spark.createDataFrame(transaction_data, columns)
    
    print("\nTransaction Summary:")
    df.groupBy("customer_id").agg(
        {"amount": "sum", "amount": "count"}
    ).withColumnRenamed("sum(amount)", "total_amount") \
     .withColumnRenamed("count(amount)", "transaction_count").show()
    
    credit_df = df.filter(df.transaction_type == "CREDIT")
    print("\nCredit Transactions:")
    credit_df.show()
    
    debit_df = df.filter(df.transaction_type == "DEBIT")
    print("\nDebit Transactions:")
    debit_df.show()
    
    spark.stop()
    print("\nBanking analytics complete.")


def healthcare_example():
    """Healthcare application - Patient records analysis."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Patient Analytics")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareAnalytics")
    
    patient_data = [
        ("P001", "John Smith", 45, "M", "Diabetes", 150.5, "2024-01-01"),
        ("P002", "Jane Doe", 32, "F", "Hypertension", 130.2, "2024-01-02"),
        ("P003", "Bob Wilson", 58, "M", "Heart Disease", 180.0, "2024-01-01"),
        ("P004", "Alice Brown", 29, "F", "Diabetes", 145.8, "2024-01-03"),
        ("P005", "Charlie Davis", 67, "M", "Hypertension", 125.5, "2024-01-02"),
        ("P006", "Eva Martinez", 41, "F", "Asthma", 118.3, "2024-01-04"),
        ("P007", "Frank Johnson", 55, "M", "Diabetes", 160.2, "2024-01-01"),
        ("P008", "Grace Lee", 38, "F", "Heart Disease", 175.8, "2024-01-03"),
    ]
    
    columns = ["patient_id", "name", "age", "gender", "condition", "blood_pressure", "admission_date"]
    df = spark.createDataFrame(patient_data, columns)
    
    print("\nPatient Summary by Condition:")
    df.groupBy("condition").agg(
        {"age": "avg", "blood_pressure": "avg", "patient_id": "count"}
    ).show()
    
    print("\nHigh-Risk Patients (BP > 150):")
    high_risk = df.filter(df.blood_pressure > 150)
    high_risk.show()
    
    print("\nPatients by Age Group:")
    df.withColumn("age_group", 
        df.when(df.age < 30, "Under 30")
         .when(df.age < 45, "30-44")
         .when(df.age < 60, "45-59")
         .otherwise("60+")
    ).groupBy("age_group").count().show()
    
    spark.stop()
    print("\nHealthcare analytics complete.")


def main():
    """Main execution function."""
    print("Executing Spark Architecture and Components implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")
        print("Note: Requires PySpark to be installed (pip install pyspark)")


if __name__ == "__main__":
    main()
