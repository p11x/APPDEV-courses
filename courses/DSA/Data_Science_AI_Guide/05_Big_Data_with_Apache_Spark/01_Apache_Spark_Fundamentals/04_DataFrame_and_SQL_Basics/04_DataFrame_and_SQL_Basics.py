# Topic: DataFrame and SQL Basics
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for DataFrame and SQL Basics

I. INTRODUCTION
DataFrames and Spark SQL provide a higher-level abstraction for working with
structured data in Spark. They offer a SQL-like interface and optimization through
the Catalyst optimizer. This module covers DataFrame creation, transformations,
and SQL operations.

II. CORE CONCEPTS
- DataFrame creation from various sources
- Schema and data types
- DataFrame transformations (select, filter, groupBy, etc.)
- Spark SQL operations
- Working with views and temporary tables
- DataFrame APIs vs SQL queries

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import DataFrame
from pyspark.sql import functions as F
from pyspark.sql import types as T
from pyspark.sql.window import Window
from typing import List, Optional


def create_spark_session(app_name: str = "DataFrameSQLDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()


def create_dataframe_from_list(spark: SparkSession) -> DataFrame:
    """
    Create DataFrame from a list of data.
    
    This is the most basic way to create a DataFrame.
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
    
    print("DataFrame from List:")
    print(f"  Columns: {df.columns}")
    print(f"  Schema: {df.dtypes}")
    df.show()
    
    return df


def create_dataframe_with_schema(spark: SparkSession) -> DataFrame:
    """
    Create DataFrame with explicit schema.
    
    Defining schema explicitly provides better performance
    and avoids schema inference overhead.
    """
    schema = T.StructType([
        T.StructField("id", T.IntegerType(), False),
        T.StructField("name", T.StringType(), True),
        T.StructField("age", T.IntegerType(), True),
        T.StructField("salary", T.DoubleType(), True),
    ])
    
    data = [
        (1, "Alice", 25, 50000.0),
        (2, "Bob", 30, 60000.0),
        (3, "Charlie", 35, 75000.0),
    ]
    
    df = spark.createDataFrame(data, schema)
    
    print("\nDataFrame with Schema:")
    df.printSchema()
    df.show()
    
    return df


def dataframe_select_operations(df: DataFrame) -> DataFrame:
    """
    Demonstrate select operations.
    
    Select specific columns from the DataFrame.
    """
    print("\nSelect Operations:")
    
    selected = df.select("name", "salary")
    print("  Selected columns:")
    selected.show()
    
    with_alias = df.select(
        df.name.alias("employee_name"),
        df.salary.alias("annual_salary")
    )
    print("  With aliases:")
    with_alias.show()
    
    expr_selected = df.select(
        F.col("name"),
        F.col("salary") * 1.1.alias("salary_with_bonus")
    )
    print("  With expressions:")
    expr_selected.show()
    
    return df


def dataframe_filter_operations(df: DataFrame) -> DataFrame:
    """
    Demonstrate filter operations.
    
    Filter rows based on conditions.
    """
    print("\nFilter Operations:")
    
    filtered = df.filter(df.age > 28)
    print("  Filter age > 28:")
    filtered.show()
    
    composite_filter = df.filter(
        (df.age > 28) & (df.salary > 55000)
    )
    print("  Filter age > 28 AND salary > 55000:")
    composite_filter.show()
    
    city_filter = df.filter(df.city.isin(["NYC", "LA"]))
    print("  Filter city in (NYC, LA):")
    city_filter.show()
    
    return df


def dataframe_withcolumn_operations(df: DataFrame) -> DataFrame:
    """
    Demonstrate withColumn operations.
    
    Add or replace columns in DataFrame.
    """
    print("\nWithColumn Operations:")
    
    with_bonus = df.withColumn("bonus", df.salary * 0.1)
    print("  Added bonus column:")
    with_bonus.show()
    
    with_category = df.withColumn(
        "category",
        F.when(df.salary < 60000, "Low")
         .when(df.salary < 75000, "Medium")
         .otherwise("High")
    )
    print("  Added category column:")
    with_category.show()
    
    renamed = df.withColumnRenamed("salary", "annual_salary")
    print("  Renamed salary column:")
    renamed.show()
    
    return df


def dataframe_drop_operations(df: DataFrame) -> DataFrame:
    """
    Demonstrate drop operations.
    
    Remove columns from DataFrame.
    """
    print("\nDrop Operations:")
    
    dropped = df.drop("city")
    print("  Dropped city column:")
    print(f"  Columns: {dropped.columns}")
    dropped.show()
    
    return df


def dataframe_sorting_operations(df: DataFrame) -> DataFrame:
    """
    Demonstrate sorting operations.
    
    Sort DataFrame by columns.
    """
    print("\nSorting Operations:")
    
    sorted_asc = df.sort("salary")
    print("  Sort by salary (ascending):")
    sorted_asc.show()
    
    sorted_desc = df.sort(F.desc("salary"))
    print("  Sort by salary (descending):")
    sorted_desc.show()
    
    multi_sort = df.sort(F.asc("city"), F.desc("salary"))
    print("  Sort by city (asc), salary (desc):")
    multi_sort.show()
    
    return df


def dataframe_groupby_operations(df: DataFrame) -> DataFrame:
    """
    Demonstrate groupBy operations.
    
    Group data and perform aggregations.
    """
    print("\nGroupBy Operations:")
    
    by_city = df.groupBy("city").agg(
        {"salary": "avg", "age": "avg", "name": "count"}
    )
    print("  Group by city:")
    by_city.show()
    
    with_alias = df.groupBy("city").agg(
        F.avg("salary").alias("avg_salary"),
        F.min("salary").alias("min_salary"),
        F.max("salary").alias("max_salary"),
        F.count("name").alias("employee_count")
    )
    print("  Group by city with aliases:")
    with_alias.show()
    
    return df


def dataframe_join_operations(spark: SparkSession) -> DataFrame:
    """
    Demonstrate join operations.
    
    Join multiple DataFrames together.
    """
    employees = [
        ("E001", "Alice", "D001"),
        ("E002", "Bob", "D001"),
        ("E003", "Charlie", "D002"),
    ]
    df1 = spark.createDataFrame(
        employees, ["emp_id", "name", "dept_id"]
    )
    
    departments = [
        ("D001", "Engineering"),
        ("D002", "Sales"),
        ("D003", "HR"),
    ]
    df2 = spark.createDataFrame(
        departments, ["dept_id", "dept_name"]
    )
    
    print("\nJoin Operations:")
    
    inner_join = df1.join(df2, "dept_id")
    print("  Inner join:")
    inner_join.show()
    
    left_join = df1.join(df2, "dept_id", "left")
    print("  Left join:")
    left_join.show()
    
    right_join = df1.join(df2, "dept_id", "right")
    print("  Right join:")
    right_join.show()
    
    return inner_join


def dataframe_udf_demo(spark: SparkSession) -> DataFrame:
    """
    Demonstrate User Defined Functions.
    """
    data = [
        ("Alice", 25, "NYC"),
        ("Bob", 30, "LA"),
        ("Charlie", 35, "NYC"),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city"]
    )
    
    print("\nUDF Demo:")
    
    uppercase_udf = F.udf(lambda x: x.upper() if x else x)
    result = df.select(
        uppercase_udf(F.col("name")).alias("name_upper")
    )
    result.show()
    
    return df


def spark_sql_operations(spark: SparkSession) -> None:
    """
    Demonstrate Spark SQL operations.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
        ("Charlie", 35, "NYC", 75000),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nSpark SQL Operations:")
    
    df.createOrReplaceTempView("employees")
    
    result = spark.sql("""
        SELECT name, salary, city
        FROM employees
        WHERE age > 28
        ORDER BY salary DESC
    """)
    print("  SQL query result:")
    result.show()
    
    spark.sql("""
        CREATE OR REPLACE TEMP VIEW high_earners AS
        SELECT name, salary
        FROM employees
        WHERE salary > 55000
    """)
    
    high_earners = spark.sql("SELECT * FROM high_earners")
    print("  High earners view:")
    high_earners.show()


def dataframe_window_functions(spark: SparkSession) -> DataFrame:
    """
    Demonstrate window functions.
    """
    data = [
        ("2024-01-01", "ProductA", 100),
        ("2024-01-02", "ProductA", 150),
        ("2024-01-03", "ProductA", 120),
        ("2024-01-01", "ProductB", 200),
        ("2024-01-02", "ProductB", 180),
    ]
    df = spark.createDataFrame(
        data, ["date", "product", "sales"]
    )
    
    print("\nWindow Functions:")
    
    window_spec = Window.partitionBy("product") \
                        .orderBy("date")
    
    df_with_rank = df.withColumn(
        "rank", 
        F.row_number().over(window_spec)
    )
    print("  With row number:")
    df_with_rank.show()
    
    df_with_lag = df.withColumn(
        "previous_sales",
        F.lag("sales", 1).over(window_spec)
    )
    print("  With lag:")
    df_with_lag.show()
    
    df_with_running_total = df.withColumn(
        "running_total",
        F.sum("sales").over(window_spec)
    )
    print("  With running total:")
    df_with_running_total.show()
    
    return df


def dataframe_null_handling(spark: SparkSession) -> DataFrame:
    """
    Demonstrate null handling in DataFrames.
    """
    data = [
        ("Alice", 25, None),
        ("Bob", None, 60000),
        ("Charlie", 35, 75000),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "salary"]
    )
    
    print("\nNull Handling:")
    
    filled = df.fillna({"age": 0, "salary": 0})
    print("  Filled nulls:")
    filled.show()
    
    filtered = df.filter(F.col("age").isNotNull())
    print("  Filtered nulls:")
    filtered.show()
    
    return df


def core_implementation():
    """Core implementation demonstrating DataFrame and SQL basics."""
    print("=" * 60)
    print("DATAFRAME AND SQL BASICS")
    print("=" * 60)
    
    spark = create_spark_session()
    
    df = create_dataframe_from_list(spark)
    
    dataframe_select_operations(df)
    dataframe_filter_operations(df)
    dataframe_withcolumn_operations(df)
    dataframe_drop_operations(df)
    dataframe_sorting_operations(df)
    dataframe_groupby_operations(df)
    
    dataframe_join_operations(spark)
    spark_sql_operations(spark)
    dataframe_window_functions(spark)
    dataframe_null_handling(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Account analytics."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Account Analytics")
    print("=" * 60)
    
    spark = create_spark_session("BankingAnalytics")
    
    accounts = [
        ("ACC001", "John Doe", "SAVINGS", 50000.0),
        ("ACC002", "Jane Smith", "CHECKING", 25000.0),
        ("ACC003", "Bob Wilson", "SAVINGS", 75000.0),
        ("ACC004", "Alice Brown", "INVESTMENT", 100000.0),
    ]
    df = spark.createDataFrame(
        accounts, 
        ["account_id", "name", "account_type", "balance"]
    )
    
    print("\nAccount Analysis:")
    
    by_type = df.groupBy("account_type").agg(
        F.avg("balance").alias("avg_balance"),
        F.min("balance").alias("min_balance"),
        F.max("balance").alias("max_balance"),
    )
    print("  By account type:")
    by_type.show()
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Patient records."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Patient Records")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareAnalytics")
    
    patients = [
        ("P001", "Alice", "Hypertension", 130.5),
        ("P002", "Bob", "Diabetes", 145.8),
        ("P003", "Charlie", "Hypertension", 128.2),
        ("P004", "Diana", "Diabetes", 150.2),
    ]
    df = spark.createDataFrame(
        patients,
        ["patient_id", "name", "condition", "blood_pressure"]
    )
    
    print("\nPatient Analysis:")
    
    by_condition = df.groupBy("condition").agg(
        F.avg("blood_pressure").alias("avg_bp"),
        F.count("patient_id").alias("patient_count")
    )
    print("  By condition:")
    by_condition.show()
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing DataFrame and SQL Basics implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()