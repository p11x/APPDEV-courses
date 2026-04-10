# Topic: Cloud Storage Integration
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Cloud Storage Integration

I. INTRODUCTION
This module covers integrating Spark with cloud storage systems including AWS S3,
Azure Blob Storage, and Google Cloud Storage. It includes configuration, reading,
and writing operations for cloud-based data storage.

II. CORE CONCEPTS
- AWS S3 integration
- Azure Blob Storage integration
- Google Cloud Storage integration
- Cloud storage optimization
- Security configurations

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "CloudStorageDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def configure_s3(spark: SparkSession) -> None:
    """Configure Spark for AWS S3 access."""
    print("\nS3 Configuration:")
    
    spark.sparkContext._jsc.hadoopConfiguration().set(
        "fs.s3n.impl", "org.apache.hadoop.fs.s3native.NativeS3FileSystem"
    )
    spark.sparkContext._jsc.hadoopConfiguration().set(
        "fs.s3a.impl", "org.apache.hadoop.fs.s3a.S3AFileSystem"
    )
    
    print("  S3 filesystem configured")


def configure_azure(spark: SparkSession) -> None:
    """Configure Spark for Azure Blob Storage."""
    print("\nAzure Blob Storage Configuration:")
    
    spark.sparkContext._jsc.hadoopConfiguration().set(
        "fs.azure.account.key.yourstorageaccount.blob.core.windows.net",
        "your-access-key"
    )
    
    print("  Azure Blob Storage configured")


def configure_gcs(spark: SparkSession) -> None:
    """Configure Spark for Google Cloud Storage."""
    print("\nGCS Configuration:")
    
    spark.sparkContext._jsc.hadoopConfiguration().set(
        "fs.gs.impl", "com.google.cloud.hadoop.fs.gcs.GoogleHadoopFileSystem"
    )
    
    print("  Google Cloud Storage configured")


def read_from_cloud(spark: SparkSession) -> None:
    """Demonstrate reading from cloud storage."""
    data = [
        ("Alice", 25, "NYC"),
        ("Bob", 30, "LA"),
    ]
    df = spark.createDataFrame(data, ["name", "age", "city"])
    
    print("\nCloud Read (simulated):")
    df.show()


def write_to_cloud(spark: SparkSession) -> None:
    """Demonstrate writing to cloud storage."""
    data = [
        ("Alice", 25, 50000),
        ("Bob", 30, 60000),
    ]
    df = spark.createDataFrame(data, ["name", "age", "salary"])
    
    print("\nCloud Write (to local for demo):")
    
    output_path = "/tmp/cloud_output"
    df.write.mode("overwrite").parquet(output_path)
    print(f"  Written to: {output_path}")
    
    result = spark.read.parquet(output_path)
    result.show()


def core_implementation():
    print("=" * 60)
    print("CLOUD STORAGE INTEGRATION")
    print("=" * 60)
    
    spark = create_spark_session()
    
    configure_s3(spark)
    configure_azure(spark)
    configure_gcs(spark)
    read_from_cloud(spark)
    write_to_cloud(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Cloud Storage")
    print("=" * 60)
    
    spark = create_spark_session("BankingCloud")
    
    accounts = [("ACC001", 50000.0), ("ACC002", 25000.0)]
    df = spark.createDataFrame(accounts, ["account_id", "balance"])
    
    df.write.mode("overwrite").parquet("/tmp/banking_cloud")
    print("  Written to cloud (simulated)")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Cloud Storage")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareCloud")
    
    patients = [("P001", "Alice"), ("P002", "Bob")]
    df = spark.createDataFrame(patients, ["patient_id", "name"])
    
    df.write.mode("overwrite").parquet("/tmp/healthcare_cloud")
    print("  Written to cloud (simulated)")
    
    spark.stop()


def main():
    print("Executing Cloud Storage Integration implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
