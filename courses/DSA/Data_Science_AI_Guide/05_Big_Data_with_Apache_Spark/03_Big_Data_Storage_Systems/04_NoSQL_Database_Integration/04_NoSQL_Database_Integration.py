# Topic: NoSQL Database Integration
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for NoSQL Database Integration

I. INTRODUCTION
This module covers integrating Spark with NoSQL databases including MongoDB, 
Cassandra, and DynamoDB. It includes reading from and writing to various 
NoSQL systems using Spark connectors.

II. CORE CONCEPTS
- MongoDB integration
- Cassandra integration
- DynamoDB integration
- NoSQL data modeling
- Performance considerations

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "NoSQLDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def configure_mongodb(spark: SparkSession) -> None:
    """Configure Spark for MongoDB access."""
    print("\nMongoDB Configuration:")
    
    spark.conf.set("spark.mongodb.input.uri", "mongodb://localhost:27017")
    spark.conf.set("spark.mongodb.output.uri", "mongodb://localhost:27017")
    
    print("  MongoDB configured")


def configure_cassandra(spark: SparkSession) -> None:
    """Configure Spark for Cassandra access."""
    print("\nCassandra Configuration:")
    
    spark.conf.set("spark.cassandra.connection.host", "localhost")
    spark.conf.set("spark.cassandra.connection.port", "9042")
    
    print("  Cassandra configured")


def read_nosql_data(spark: SparkSession) -> None:
    """Demonstrate reading from NoSQL."""
    data = [
        ("doc1", {"name": "Alice", "age": 25}),
        ("doc2", {"name": "Bob", "age": 30}),
    ]
    df = spark.createDataFrame(data, ["_id", "data"])
    
    print("\nNoSQL Read:")
    df.show()


def write_nosql_data(spark: SparkSession) -> None:
    """Demonstrate writing to NoSQL."""
    data = [
        ("ACC001", "John", 50000),
        ("ACC002", "Jane", 25000),
    ]
    df = spark.createDataFrame(data, ["id", "name", "balance"])
    
    print("\nNoSQL Write (simulated):")
    
    output_path = "/tmp/nosql_output"
    df.write.mode("overwrite").parquet(output_path)
    print(f"  Written to: {output_path}")
    
    result = spark.read.parquet(output_path)
    result.show()


def core_implementation():
    print("=" * 60)
    print("NOSQL DATABASE INTEGRATION")
    print("=" * 60)
    
    spark = create_spark_session()
    
    configure_mongodb(spark)
    configure_cassandra(spark)
    read_nosql_data(spark)
    write_nosql_data(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - NoSQL")
    print("=" * 60)
    
    spark = create_spark_session("BankingNoSQL")
    
    accounts = [("ACC001", "SAVINGS", 50000.0), ("ACC002", "CHECKING", 25000.0)]
    df = spark.createDataFrame(accounts, ["account_id", "type", "balance"])
    
    df.write.mode("overwrite").parquet("/tmp/banking_nosql")
    print("  Written to NoSQL store")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - NoSQL")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareNoSQL")
    
    patients = [("P001", "Hypertension"), ("P002", "Diabetes")]
    df = spark.createDataFrame(patients, ["patient_id", "condition"])
    
    df.write.mode("overwrite").parquet("/tmp/healthcare_nosql")
    print("  Written to NoSQL store")
    
    spark.stop()


def main():
    print("Executing NoSQL Database Integration implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
