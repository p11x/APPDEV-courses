# Topic: Join and Aggregation Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Join and Aggregation Operations

I. INTRODUCTION
Join and aggregation operations are fundamental for combining and summarizing data
in Spark. This module covers various join types (inner, outer, left, right, cross)
and aggregation operations (groupBy, rollup, cube, pivot).

II. CORE CONCEPTS
- Inner, outer, left, right joins
- Cross joins and self joins
- GroupBy aggregations
- Multiple aggregation functions
- Rollup and cube operations
- Pivot operations

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.window import Window
from typing import List


def create_spark_session(app_name: str = "JoinAggDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()


def inner_join_operation(spark: SparkSession) -> None:
    """
    Demonstrate inner join operation.
    """
    employees = [
        (1, "Alice", 100),
        (2, "Bob", 200),
        (3, "Charlie", 300),
        (4, "Diana", 400),
    ]
    df1 = spark.createDataFrame(
        employees, ["emp_id", "name", "dept_id"]
    )
    
    departments = [
        (100, "Engineering"),
        (200, "Sales"),
        (300, "Marketing"),
        (500, "HR"),
    ]
    df2 = spark.createDataFrame(
        departments, ["dept_id", "dept_name"]
    )
    
    print("\nInner Join:")
    
    result = df1.join(df2, "dept_id")
    result.show()
    print(f"  Result count: {result.count()}")


def left_join_operation(spark: SparkSession) -> None:
    """
    Demonstrate left join operation.
    """
    employees = [
        (1, "Alice", 100),
        (2, "Bob", 200),
        (3, "Charlie", 300),
        (4, "Diana", 400),
    ]
    df1 = spark.createDataFrame(
        employees, ["emp_id", "name", "dept_id"]
    )
    
    departments = [
        (100, "Engineering"),
        (200, "Sales"),
        (300, "Marketing"),
    ]
    df2 = spark.createDataFrame(
        departments, ["dept_id", "dept_name"]
    )
    
    print("\nLeft Join:")
    
    result = df1.join(df2, "dept_id", "left")
    result.show()


def right_join_operation(spark: SparkSession) -> None:
    """
    Demonstrate right join operation.
    """
    employees = [
        (1, "Alice", 100),
        (2, "Bob", 200),
    ]
    df1 = spark.createDataFrame(
        employees, ["emp_id", "name", "dept_id"]
    )
    
    departments = [
        (100, "Engineering"),
        (200, "Sales"),
        (300, "Marketing"),
    ]
    df2 = spark.createDataFrame(
        departments, ["dept_id", "dept_name"]
    )
    
    print("\nRight Join:")
    
    result = df1.join(df2, "dept_id", "right")
    result.show()


def full_outer_join(spark: SparkSession) -> None:
    """
    Demonstrate full outer join operation.
    """
    employees = [
        (1, "Alice", 100),
        (2, "Bob", 200),
        (3, "Charlie", 300),
    ]
    df1 = spark.createDataFrame(
        employees, ["emp_id", "name", "dept_id"]
    )
    
    departments = [
        (100, "Engineering"),
        (200, "Sales"),
        (400, "HR"),
    ]
    df2 = spark.createDataFrame(
        departments, ["dept_id", "dept_name"]
    )
    
    print("\nFull Outer Join:")
    
    result = df1.join(df2, "dept_id", "outer")
    result.show()


def cross_join_operation(spark: SparkSession) -> None:
    """
    Demonstrate cross join operation.
    """
    colors = [
        ("Red",),
        ("Green",),
        ("Blue",),
    ]
    df1 = spark.createDataFrame(colors, ["color"])
    
    sizes = [
        ("S",),
        ("M",),
        ("L",),
    ]
    df2 = spark.createDataFrame(sizes, ["size"])
    
    print("\nCross Join:")
    
    result = df1.crossJoin(df2)
    result.show()
    print(f"  Result count: {result.count()}")


def self_join_operation(spark: SparkSession) -> None:
    """
    Demonstrate self join operation.
    """
    employees = [
        (1, "Alice", 100, 100),
        (2, "Bob", 200, 100),
        (3, "Charlie", 300, 200),
        (4, "Diana", 400, 300),
    ]
    df = spark.createDataFrame(
        employees, ["emp_id", "name", "dept_id", "manager_id"]
    )
    
    print("\nSelf Join:")
    
    employees_df = df.alias("emp")
    managers_df = df.alias("mgr")
    
    result = employees_df.join(
        managers_df,
        employees_df.manager_id == managers_df.emp_id
    ).select(
        employees_df.name.alias("employee"),
        managers_df.name.alias("manager")
    )
    result.show()


def groupby_basic_aggregation(spark: SparkSession) -> None:
    """
    Demonstrate basic groupBy aggregation.
    """
    data = [
        ("Sales", "A", 100),
        ("Sales", "B", 200),
        ("Marketing", "A", 150),
        ("Marketing", "B", 250),
        ("Sales", "C", 300),
    ]
    df = spark.createDataFrame(
        data, ["department", "product", "sales"]
    )
    
    print("\nBasic GroupBy Aggregation:")
    
    result = df.groupBy("department").agg(
        {"sales": "sum", "sales": "avg", "product": "count"}
    )
    result.show()
    
    result_with_alias = df.groupBy("department").agg(
        F.sum("sales").alias("total_sales"),
        F.avg("sales").alias("avg_sales"),
        F.min("sales").alias("min_sales"),
        F.max("sales").alias("max_sales"),
    )
    result_with_alias.show()


def groupby_multiple_columns(spark: SparkSession) -> None:
    """
    Demonstrate groupBy with multiple columns.
    """
    data = [
        ("2024-01", "Sales", "A", 100),
        ("2024-01", "Sales", "B", 200),
        ("2024-01", "Marketing", "A", 150),
        ("2024-02", "Sales", "A", 300),
        ("2024-02", "Sales", "B", 250),
    ]
    df = spark.createDataFrame(
        data, ["month", "department", "product", "sales"]
    )
    
    print("\nGroupBy Multiple Columns:")
    
    result = df.groupBy("month", "department").agg(
        F.sum("sales").alias("total_sales")
    )
    result.show()


def rollup_operation(spark: SparkSession) -> None:
    """
    Demonstrate rollup operation for hierarchical aggregation.
    """
    data = [
        ("Sales", "A", 100),
        ("Sales", "B", 200),
        ("Marketing", "A", 150),
        ("Marketing", "B", 250),
    ]
    df = spark.createDataFrame(
        data, ["department", "product", "sales"]
    )
    
    print("\nRollup Operation:")
    
    result = df.rollup("department", "product").agg(
        F.sum("sales").alias("total_sales")
    ).orderBy("department", "product")
    result.show()


def cube_operation(spark: SparkSession) -> None:
    """
    Demonstrate cube operation for multi-dimensional aggregation.
    """
    data = [
        ("Sales", "A", 100),
        ("Sales", "B", 200),
        ("Marketing", "A", 150),
        ("Marketing", "B", 250),
    ]
    df = spark.createDataFrame(
        data, ["department", "product", "sales"]
    )
    
    print("\nCube Operation:")
    
    result = df.cube("department", "product").agg(
        F.sum("sales").alias("total_sales")
    ).orderBy("department", "product")
    result.show()


def pivot_operation(spark: SparkSession) -> None:
    """
    Demonstrate pivot operation.
    """
    data = [
        ("2024-01", "Sales", 1000),
        ("2024-01", "Marketing", 500),
        ("2024-02", "Sales", 1500),
        ("2024-02", "Marketing", 700),
    ]
    df = spark.createDataFrame(
        data, ["month", "department", "sales"]
    )
    
    print("\nPivot Operation:")
    
    result = df.groupBy("department").pivot("month").sum("sales")
    result.show()


def having_clause(spark: SparkSession) -> None:
    """
    Demonstrate having clause (filtering grouped results).
    """
    data = [
        ("Sales", "A", 100),
        ("Sales", "B", 200),
        ("Sales", "C", 50),
        ("Marketing", "A", 150),
        ("Marketing", "B", 250),
    ]
    df = spark.createDataFrame(
        data, ["department", "product", "sales"]
    )
    
    print("\nHaving Clause:")
    
    result = df.groupBy("department").agg(
        F.sum("sales").alias("total_sales")
    ).filter(F.col("total_sales") > 200)
    result.show()


def core_implementation():
    """Core implementation demonstrating joins and aggregations."""
    print("=" * 60)
    print("JOIN AND AGGREGATION OPERATIONS")
    print("=" * 60)
    
    spark = create_spark_session()
    
    inner_join_operation(spark)
    left_join_operation(spark)
    right_join_operation(spark)
    full_outer_join(spark)
    cross_join_operation(spark)
    self_join_operation(spark)
    groupby_basic_aggregation(spark)
    groupby_multiple_columns(spark)
    rollup_operation(spark)
    cube_operation(spark)
    pivot_operation(spark)
    having_clause(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Account aggregation."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Account Aggregation")
    print("=" * 60)
    
    spark = create_spark_session("BankingJoin")
    
    accounts = [
        ("ACC001", "John", "SAVINGS"),
        ("ACC002", "Jane", "CHECKING"),
        ("ACC003", "Bob", "SAVINGS"),
    ]
    df1 = spark.createDataFrame(
        accounts, ["account_id", "name", "account_type"]
    )
    
    transactions = [
        ("TXN001", "ACC001", 100.00),
        ("TXN002", "ACC001", 200.00),
        ("TXN003", "ACC002", 50.00),
        ("TXN004", "ACC003", 300.00),
    ]
    df2 = spark.createDataFrame(
        transactions, ["txn_id", "account_id", "amount"]
    )
    
    print("\nAccount Join:")
    
    result = df1.join(df2, "account_id")
    result.show()
    
    print("\nAccount Aggregation:")
    
    agg_result = df2.groupBy("account_id").agg(
        F.sum("amount").alias("total_amount"),
        F.count("txn_id").alias("txn_count")
    )
    agg_result.show()
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Patient aggregation."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Patient Aggregation")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareJoin")
    
    patients = [
        ("P001", "Alice", "Diabetes"),
        ("P002", "Bob", "Hypertension"),
        ("P003", "Charlie", "Diabetes"),
    ]
    df1 = spark.createDataFrame(
        patients, ["patient_id", "name", "condition"]
    )
    
    visits = [
        ("V001", "P001", 100.00),
        ("V002", "P001", 200.00),
        ("V003", "P002", 150.00),
        ("V004", "P003", 300.00),
    ]
    df2 = spark.createDataFrame(
        visits, ["visit_id", "patient_id", "cost"]
    )
    
    print("\nPatient Join:")
    
    result = df1.join(df2, "patient_id")
    result.show()
    
    print("\nCondition Aggregation:")
    
    agg_result = df1.join(df2, "patient_id").groupBy("condition").agg(
        F.sum("cost").alias("total_cost"),
        F.count("visit_id").alias("visit_count")
    )
    agg_result.show()
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Join and Aggregation Operations implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
