# Topic: Data Warehouse Solutions
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Warehouse Solutions

I. INTRODUCTION
This module covers data warehouse concepts and implementation with Spark. It includes
star and snowflake schemas, dimension tables, fact tables, and integration with
warehouse systems like Redshift and Snowflake.

II. CORE CONCEPTS
- Star schema design
- Snowflake schema design
- Fact and dimension tables
- OLAP operations
- Data warehouse integration

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "DataWarehouseDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def create_dimension_tables(spark: SparkSession) -> None:
    """Create dimension tables for warehouse."""
    print("\nDimension Tables:")
    
    dim_date = [
        (1, "2024-01-01", "January", 1, 2024),
        (2, "2024-01-02", "January", 1, 2024),
    ]
    df_date = spark.createDataFrame(
        dim_date, ["date_key", "date", "month", "quarter", "year"]
    )
    print("  Date dimension created")
    
    dim_product = [
        ("P001", "ProductA", "Electronics", "Gadgets"),
        ("P002", "ProductB", "Clothing", "Shirts"),
    ]
    df_product = spark.createDataFrame(
        dim_product, ["product_key", "product_name", "category", "subcategory"]
    )
    print("  Product dimension created")


def create_fact_table(spark: SparkSession) -> None:
    """Create fact table for warehouse."""
    print("\nFact Table:")
    
    fact_sales = [
        (1, "P001", "2024-01-01", 100, 5000.00),
        (2, "P002", "2024-01-02", 50, 2500.00),
    ]
    df = spark.createDataFrame(
        fact_sales, ["sale_id", "product_key", "date", "quantity", "revenue"]
    )
    df.show()


def star_schema_operations(spark: SparkSession) -> None:
    """Demonstrate star schema operations."""
    print("\nStar Schema Operations:")
    
    dim_product = [
        ("P001", "ProductA", "Electronics"),
        ("P002", "ProductB", "Clothing"),
    ]
    df_dim = spark.createDataFrame(
        dim_product, ["product_key", "name", "category"]
    )
    
    fact_sales = [
        ("P001", 100), ("P002", 50)
    ]
    df_fact = spark.createDataFrame(
        fact_sales, ["product_key", "quantity"]
    )
    
    result = df_fact.join(df_dim, "product_key")
    print("  Star schema join completed")
    result.show()


def core_implementation():
    print("=" * 60)
    print("DATA WAREHOUSE SOLUTIONS")
    print("=" * 60)
    
    spark = create_spark_session()
    
    create_dimension_tables(spark)
    create_fact_table(spark)
    star_schema_operations(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Data Warehouse")
    print("=" * 60)
    
    spark = create_spark_session("BankingDW")
    
    dim_account = [("ACC001", "SAVINGS"), ("ACC002", "CHECKING")]
    df = spark.createDataFrame(dim_account, ["account_id", "type"])
    df.write.mode("overwrite").parquet("/tmp/dim_account")
    print("  Dimension table written")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Data Warehouse")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareDW")
    
    dim_condition = [("C001", "Hypertension"), ("C002", "Diabetes")]
    df = spark.createDataFrame(dim_condition, ["condition_id", "name"])
    df.write.mode("overwrite").parquet("/tmp/dim_condition")
    print("  Dimension table written")
    
    spark.stop()


def main():
    print("Executing Data Warehouse Solutions implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
