# Topic: Window Functions and Advanced Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Window Functions and Advanced Operations

I. INTRODUCTION
Window functions allow calculations across sets of rows that are related to the 
current row. This module covers various window functions including ranking, 
lead/lag, running totals, moving averages, and advanced analytical operations.

II. CORE CONCEPTS
- Window specification (partitionBy, orderBy)
- Ranking functions (row_number, rank, dense_rank, ntile)
- Lead and Lag functions
- Running totals and moving averages
- First and last values within window
- Cumulative distribution functions

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.window import Window
from typing import List


def create_spark_session(app_name: str = "WindowFunctionsDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()


def row_number_window(spark: SparkSession) -> None:
    """
    Demonstrate row_number window function.
    """
    data = [
        ("2024-01-01", "ProductA", 100),
        ("2024-01-02", "ProductA", 150),
        ("2024-01-03", "ProductA", 120),
        ("2024-01-01", "ProductB", 200),
        ("2024-01-02", "ProductB", 180),
    ]
    df = spark.createDataFrame(data, ["date", "product", "sales"])
    
    print("\nRow Number Window:")
    
    window_spec = Window.partitionBy("product").orderBy("date")
    
    result = df.withColumn("row_num", F.row_number().over(window_spec))
    result.show()


def rank_functions(spark: SparkSession) -> None:
    """
    Demonstrate ranking functions.
    """
    data = [
        ("Alice", "Sales", 100),
        ("Bob", "Sales", 200),
        ("Charlie", "Sales", 200),
        ("Diana", "Sales", 150),
    ]
    df = spark.createDataFrame(data, ["name", "department", "salary"])
    
    print("\nRank Functions:")
    
    window_spec = Window.orderBy(F.desc("salary"))
    
    result = df.withColumn("rank", F.rank().over(window_spec)) \
               .withColumn("dense_rank", F.dense_rank().over(window_spec)) \
               .withColumn("ntile", F.ntile(2).over(window_spec))
    result.show()


def lead_function(spark: SparkSession) -> None:
    """
    Demonstrate lead function.
    """
    data = [
        ("2024-01-01", 100),
        ("2024-01-02", 150),
        ("2024-01-03", 120),
        ("2024-01-04", 180),
    ]
    df = spark.createDataFrame(data, ["date", "sales"])
    
    print("\nLead Function:")
    
    window_spec = Window.orderBy("date")
    
    result = df.withColumn("next_sales", F.lead("sales", 1).over(window_spec)) \
               .withColumn("sales_diff", 
                          F.col("sales") - F.lead("sales", 1).over(window_spec))
    result.show()


def lag_function(spark: SparkSession) -> None:
    """
    Demonstrate lag function.
    """
    data = [
        ("2024-01-01", 100),
        ("2024-01-02", 150),
        ("2024-01-03", 120),
        ("2024-01-04", 180),
    ]
    df = spark.createDataFrame(data, ["date", "sales"])
    
    print("\nLag Function:")
    
    window_spec = Window.orderBy("date")
    
    result = df.withColumn("prev_sales", F.lag("sales", 1).over(window_spec)) \
               .withColumn("sales_growth",
                          F.col("sales") - F.lag("sales", 1).over(window_spec))
    result.show()


def running_total(spark: SparkSession) -> None:
    """
    Demonstrate running total using window functions.
    """
    data = [
        ("2024-01-01", 100),
        ("2024-01-02", 150),
        ("2024-01-03", 120),
        ("2024-01-04", 180),
    ]
    df = spark.createDataFrame(data, ["date", "sales"])
    
    print("\nRunning Total:")
    
    window_spec = Window.orderBy("date")
    
    result = df.withColumn("running_total", 
                          F.sum("sales").over(window_spec))
    result.show()


def moving_average(spark: SparkSession) -> None:
    """
    Demonstrate moving average using window functions.
    """
    data = [
        ("2024-01-01", 100),
        ("2024-01-02", 150),
        ("2024-01-03", 120),
        ("2024-01-04", 180),
        ("2024-01-05", 160),
    ]
    df = spark.createDataFrame(data, ["date", "sales"])
    
    print("\nMoving Average:")
    
    window_spec = Window.orderBy("date").rowsBetween(-2, 0)
    
    result = df.withColumn("moving_avg_3",
                          F.avg("sales").over(window_spec))
    result.show()


def first_last_values(spark: SparkSession) -> None:
    """
    Demonstrate first and last values within window.
    """
    data = [
        ("2024-01-01", "ProductA", 100),
        ("2024-01-02", "ProductA", 150),
        ("2024-01-03", "ProductA", 120),
        ("2024-01-01", "ProductB", 200),
        ("2024-01-02", "ProductB", 180),
    ]
    df = spark.createDataFrame(data, ["date", "product", "sales"])
    
    print("\nFirst/Last Values:")
    
    window_spec = Window.partitionBy("product").orderBy("date")
    
    result = df.withColumn("first_sales", 
                          F.first("sales").over(window_spec)) \
               .withColumn("last_sales",
                          F.last("sales").over(window_spec))
    result.show()


def cumulative_distribution(spark: SparkSession) -> None:
    """
    Demonstrate cumulative distribution functions.
    """
    data = [
        ("Alice", 100),
        ("Bob", 200),
        ("Charlie", 150),
        ("Diana", 250),
    ]
    df = spark.createDataFrame(data, ["name", "score"])
    
    print("\nCumulative Distribution:")
    
    window_spec = Window.orderBy("score")
    
    result = df.withColumn("cume_dist", F.cume_dist().over(window_spec)) \
               .withColumn("percent_rank", F.percent_rank().over(window_spec))
    result.show()


def rank_with_groupby(spark: SparkSession) -> None:
    """
    Demonstrate ranking combined with groupBy.
    """
    data = [
        ("Sales", "Alice", 100),
        ("Sales", "Bob", 200),
        ("Marketing", "Charlie", 150),
        ("Marketing", "Diana", 250),
    ]
    df = spark.createDataFrame(data, ["department", "name", "sales"])
    
    print("\nRank with GroupBy:")
    
    window_spec = Window.partitionBy("department").orderBy(F.desc("sales"))
    
    result = df.withColumn("rank", F.row_number().over(window_spec))
    result.show()


def core_implementation():
    """Core implementation demonstrating window functions."""
    print("=" * 60)
    print("WINDOW FUNCTIONS AND ADVANCED OPERATIONS")
    print("=" * 60)
    
    spark = create_spark_session()
    
    row_number_window(spark)
    rank_functions(spark)
    lead_function(spark)
    lag_function(spark)
    running_total(spark)
    moving_average(spark)
    first_last_values(spark)
    cumulative_distribution(spark)
    rank_with_groupby(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Transaction window analysis."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Transaction Analysis")
    print("=" * 60)
    
    spark = create_spark_session("BankingWindow")
    
    transactions = [
        ("ACC001", "2024-01-01", 100.00),
        ("ACC001", "2024-01-02", 200.00),
        ("ACC001", "2024-01-03", 150.00),
        ("ACC001", "2024-01-04", 300.00),
    ]
    df = spark.createDataFrame(
        transactions, ["account_id", "date", "amount"]
    )
    
    print("\nTransaction Analysis:")
    
    window_spec = Window.partitionBy("account_id").orderBy("date")
    
    result = df.withColumn("running_balance",
                          F.sum("amount").over(window_spec)) \
               .withColumn("prev_amount",
                          F.lag("amount", 1).over(window_spec))
    result.show()
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Patient visit analysis."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Patient Visit Analysis")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareWindow")
    
    visits = [
        ("P001", "2024-01-01", 100.00),
        ("P001", "2024-01-02", 200.00),
        ("P001", "2024-01-03", 150.00),
        ("P002", "2024-01-01", 300.00),
        ("P002", "2024-01-02", 250.00),
    ]
    df = spark.createDataFrame(
        visits, ["patient_id", "date", "cost"]
    )
    
    print("\nVisit Analysis:")
    
    window_spec = Window.partitionBy("patient_id").orderBy("date")
    
    result = df.withColumn("total_cost",
                          F.sum("cost").over(window_spec)) \
               .withColumn("visit_num",
                          F.row_number().over(window_spec))
    result.show()
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Window Functions implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
