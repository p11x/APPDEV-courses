# Topic: Hadoop HDFS Integration
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Hadoop HDFS Integration

I. INTRODUCTION
This module covers integrating Apache Spark with Hadoop HDFS (Hadoop Distributed 
File System). It includes reading/writing data to HDFS, HDFS file formats, and 
configuration for HDFS access.

II. CORE CONCEPTS
- HDFS architecture and concepts
- Reading data from HDFS
- Writing data to HDFS
- HDFS file formats (Parquet, Avro, ORC)
- HDFS security and permissions
- HDFS optimization strategies

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType
from typing import Optional, List
import os


def create_spark_session(app_name: str = "HDFSDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()


def configure_hdfs(spark: SparkSession) -> None:
    """
    Configure Spark for HDFS access.
    """
    print("\nHDFS Configuration:")
    
    hadoop_conf = spark.sparkContext._jsc.hadoopConfiguration()
    
    hadoop_conf.set("fs.defaultFS", "hdfs://localhost:9000")
    hadoop_conf.set("dfs.replication", "3")
    hadoop_conf.set("dfs.block.size", "134217728")
    
    print(f"  Default FS: {hadoop_conf.get('fs.defaultFS')}")
    print(f"  Replication: {hadoop_conf.get('dfs.replication')}")


def read_from_local(spark: SparkSession) -> None:
    """
    Demonstrate reading from local filesystem (simulating HDFS).
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
        ("Charlie", 35, "NYC", 75000),
        ("Diana", 28, "SF", 55000),
        ("Eve", 32, "NYC", 80000)
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nRead Data (simulated):")
    df.show()
    print(f"  Rows: {df.count()}")


def write_to_parquet(spark: SparkSession, output_path: str = "/tmp/parquet_output") -> None:
    """
    Demonstrate writing to Parquet format.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
        ("Charlie", 35, "NYC", 75000),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nWrite to Parquet:")
    
    df.write.mode("overwrite").parquet(output_path)
    print(f"  Written to: {output_path}")
    
    result = spark.read.parquet(output_path)
    print(f"  Read back: {result.count()} rows")
    result.show()


def write_to_csv(spark: SparkSession, output_path: str = "/tmp/csv_output") -> None:
    """
    Demonstrate writing to CSV format.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nWrite to CSV:")
    
    df.write.mode("overwrite").csv(output_path, header=True)
    print(f"  Written to: {output_path}")


def write_to_json(spark: SparkSession, output_path: str = "/tmp/json_output") -> None:
    """
    Demonstrate writing to JSON format.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nWrite to JSON:")
    
    df.write.mode("overwrite").json(output_path)
    print(f"  Written to: {output_path}")


def hdfs_partitioning(spark: SparkSession) -> None:
    """
    Demonstrate HDFS-style partitioning.
    """
    data = [
        ("2024-01-01", "Sales", 100),
        ("2024-01-02", "Sales", 200),
        ("2024-02-01", "Marketing", 150),
        ("2024-02-02", "Marketing", 250),
    ]
    df = spark.createDataFrame(
        data, ["date", "department", "sales"]
    )
    
    print("\nPartitioning by Column:")
    
    output_path = "/tmp/partitioned_output"
    df.write.partitionBy("department").mode("overwrite").parquet(output_path)
    print(f"  Written with partitioning to: {output_path}")
    
    result = spark.read.parquet(output_path)
    print(f"  Read: {result.count()} rows")
    result.show()


def hdfs_bucketting(spark: SparkSession) -> None:
    """
    Demonstrate HDFS-style bucketing.
    """
    data = [
        (1, "Alice", "Sales"),
        (2, "Bob", "Engineering"),
        (3, "Charlie", "Sales"),
        (4, "Diana", "Marketing"),
    ]
    df = spark.createDataFrame(
        data, ["id", "name", "department"]
    )
    
    print("\nBucketing:")
    
    output_path = "/tmp/bucketed_output"
    df.write.bucketBy(2, "department").sortBy("id").mode("overwrite").parquet(output_path)
    print(f"  Written with bucketing to: {output_path}")


def core_implementation():
    """Core implementation demonstrating HDFS integration."""
    print("=" * 60)
    print("HADOOP HDFS INTEGRATION")
    print("=" * 60)
    
    spark = create_spark_session()
    
    configure_hdfs(spark)
    read_from_local(spark)
    write_to_parquet(spark)
    write_to_csv(spark)
    write_to_json(spark)
    hdfs_partitioning(spark)
    hdfs_bucketting(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - HDFS storage."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - HDFS Storage")
    print("=" * 60)
    
    spark = create_spark_session("BankingHDFS")
    
    accounts = [
        ("ACC001", "John Doe", "SAVINGS", 50000.0),
        ("ACC002", "Jane Smith", "CHECKING", 25000.0),
        ("ACC003", "Bob Wilson", "SAVINGS", 75000.0),
    ]
    df = spark.createDataFrame(
        accounts, ["account_id", "name", "account_type", "balance"]
    )
    
    print("\nAccount Data:")
    
    output_path = "/tmp/banking_accounts"
    df.write.partitionBy("account_type").mode("overwrite").parquet(output_path)
    print(f"  Stored to: {output_path}")
    
    result = spark.read.parquet(output_path)
    result.show()
    
    spark.stop()


def healthcare_example():
    """Healthcare application - HDFS storage."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - HDFS Storage")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareHDFS")
    
    patients = [
        ("P001", "Alice Johnson", "Hypertension", 130.5),
        ("P002", "Bob Brown", "Diabetes", 145.8),
        ("P003", "Charlie Davis", "Asthma", 118.3),
    ]
    df = spark.createDataFrame(
        patients, ["patient_id", "name", "condition", "blood_pressure"]
    )
    
    print("\nPatient Data:")
    
    output_path = "/tmp/healthcare_patients"
    df.write.partitionBy("condition").mode("overwrite").parquet(output_path)
    print(f"  Stored to: {output_path}")
    
    result = spark.read.parquet(output_path)
    result.show()
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Hadoop HDFS Integration implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()