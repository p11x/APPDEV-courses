# Topic: Filter and Selection Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Filter and Selection Operations

I. INTRODUCTION
Filter and selection operations are essential for extracting specific subsets of data
in Spark. This module covers filtering rows based on conditions, selecting columns,
and various selection patterns using both RDD and DataFrame APIs.

II. CORE CONCEPTS
- Basic filter operations
- Multiple filter conditions
- Column selection patterns
- Conditional column operations
- Null handling in filters
- Pattern matching with like/rlike

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark import SparkContext
from pyspark.sql import functions as F
from pyspark.sql import types as T
from typing import List, Any


def create_spark_session(app_name: str = "FilterSelectionDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()


def rdd_filter_basic(sc: SparkContext) -> None:
    """
    Demonstrate basic RDD filter operations.
    """
    data = list(range(1, 51))
    rdd = sc.parallelize(data)
    
    print("\nRDD Basic Filter:")
    
    even_filter = rdd.filter(lambda x: x % 2 == 0)
    print(f"  Even numbers: {even_filter.take(10)}...")
    print(f"  Count: {even_filter.count()}")
    
    odd_filter = rdd.filter(lambda x: x % 2 != 0)
    print(f"  Odd numbers: {odd_filter.take(10)}...")
    
    greater_than_25 = rdd.filter(lambda x: x > 25)
    print(f"  Greater than 25: {greater_than_25.collect()}")


def rdd_filter_multiple(sc: SparkContext) -> None:
    """
    Demonstrate multiple filter conditions.
    """
    data = list(range(1, 101))
    rdd = sc.parallelize(data)
    
    print("\nRDD Multiple Filters:")
    
    composite = rdd.filter(lambda x: x % 2 == 0 and x % 3 == 0)
    print(f"  Divisible by 2 and 3: {composite.collect()}")
    
    range_filter = rdd.filter(lambda x: 20 <= x <= 50)
    print(f"  Between 20 and 50: {range_filter.collect()}")


def dataframe_filter_basic(spark: SparkSession) -> None:
    """
    Demonstrate basic DataFrame filter operations.
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
    
    print("\nDataFrame Basic Filter:")
    
    age_filter = df.filter(df.age > 28)
    print(f"  Age > 28: {age_filter.count()} rows")
    age_filter.show()
    
    city_filter = df.filter(df.city == "NYC")
    print(f"  City = NYC: {city_filter.count()} rows")
    city_filter.show()


def dataframe_filter_multiple(spark: SparkSession) -> None:
    """
    Demonstrate multiple filter conditions.
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
    
    print("\nDataFrame Multiple Filters:")
    
    composite = df.filter((df.age > 28) & (df.salary > 55000))
    print(f"  Age > 28 AND Salary > 55000: {composite.count()} rows")
    composite.show()
    
    or_condition = df.filter((df.city == "NYC") | (df.city == "LA"))
    print(f"  City = NYC OR LA: {or_condition.count()} rows")
    or_condition.show()
    
    not_condition = df.filter(~(df.city == "NYC"))
    print(f"  City != NYC: {not_condition.count()} rows")
    not_condition.show()


def dataframe_filter_in(spark: SparkSession) -> None:
    """
    Demonstrate IN filter operations.
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
    
    print("\nDataFrame IN Filter:")
    
    in_filter = df.filter(df.city.isin(["NYC", "LA"]))
    print(f"  City in (NYC, LA): {in_filter.count()} rows")
    in_filter.show()
    
    not_in = df.filter(~df.city.isin(["NYC", "LA"]))
    print(f"  City NOT in (NYC, LA): {not_in.count()} rows")
    not_in.show()


def dataframe_filter_like(spark: SparkSession) -> None:
    """
    Demonstrate LIKE filter operations.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "Los Angeles", 60000),
        ("Charlie", 35, "New York", 75000),
        ("Diana", 28, "San Francisco", 55000),
        ("Eve", 32, "Newark", 80000)
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nDataFrame LIKE Filter:")
    
    starts_with = df.filter(df.city.like("New%"))
    print(f"  City like 'New%': {starts_with.count()} rows")
    starts_with.show()
    
    ends_with = df.filter(df.city.like("% Angeles"))
    print(f"  City like '%Angeles': {ends_with.count()} rows")
    ends_with.show()
    
    contains = df.filter(df.city.like("%York%"))
    print(f"  City like '%York%': {contains.count()} rows")
    contains.show()


def dataframe_filter_null(spark: SparkSession) -> None:
    """
    Demonstrate NULL filter operations.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, None, 60000),
        ("Charlie", 35, "NYC", None),
        ("Diana", None, "SF", 55000),
        ("Eve", 32, "NYC", 80000)
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nDataFrame NULL Filter:")
    
    null_city = df.filter(df.city.isNull())
    print(f"  City is NULL: {null_city.count()} rows")
    null_city.show()
    
    not_null = df.filter(df.city.isNotNull())
    print(f"  City is NOT NULL: {not_null.count()} rows")
    not_null.show()
    
    fillna = df.fillna({"age": 0, "city": "Unknown", "salary": 0})
    print(f"  After fillna:")
    fillna.show()


def dataframe_select_columns(spark: SparkSession) -> None:
    """
    Demonstrate column selection operations.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
        ("Charlie", 35, "NYC", 75000),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nColumn Selection:")
    
    selected = df.select("name", "salary")
    print(f"  Selected columns: {selected.columns}")
    selected.show()
    
    with_alias = df.select(
        df.name.alias("employee_name"),
        df.salary.alias("annual_salary")
    )
    print(f"  With aliases:")
    with_alias.show()
    
    expr = df.select(
        F.col("name"),
        F.col("salary") * 1.1.alias("salary_with_bonus"),
        F.round(F.col("salary") / 12, 2).alias("monthly_salary")
    )
    print(f"  With expressions:")
    expr.show()


def dataframe_drop_columns(spark: SparkSession) -> None:
    """
    Demonstrate column drop operations.
    """
    data = [
        ("Alice", 25, "NYC", 50000),
        ("Bob", 30, "LA", 60000),
    ]
    df = spark.createDataFrame(
        data, ["name", "age", "city", "salary"]
    )
    
    print("\nColumn Drop:")
    
    dropped = df.drop("city")
    print(f"  After dropping city: {dropped.columns}")
    dropped.show()
    
    multiple_drop = df.drop("age", "salary")
    print(f"  After dropping age and salary: {multiple_drop.columns}")
    multiple_drop.show()


def dataframe_select_distinct(spark: SparkSession) -> None:
    """
    Demonstrate distinct selection.
    """
    data = [
        ("Alice", "NYC"),
        ("Bob", "LA"),
        ("Charlie", "NYC"),
        ("Diana", "NYC"),
    ]
    df = spark.createDataFrame(
        data, ["name", "city"]
    )
    
    print("\nDistinct Selection:")
    
    distinct_cities = df.select("city").distinct()
    print(f"  Distinct cities: {distinct_cities.count()}")
    distinct_cities.show()
    
    distinct_combination = df.select("name", "city").distinct()
    print(f"  Distinct name-city combinations: {distinct_combination.count()}")
    distinct_combination.show()


def core_implementation():
    """Core implementation demonstrating filter and selection."""
    print("=" * 60)
    print("FILTER AND SELECTION OPERATIONS")
    print("=" * 60)
    
    spark = create_spark_session()
    sc = spark.sparkContext
    
    rdd_filter_basic(sc)
    rdd_filter_multiple(sc)
    dataframe_filter_basic(spark)
    dataframe_filter_multiple(spark)
    dataframe_filter_in(spark)
    dataframe_filter_like(spark)
    dataframe_filter_null(spark)
    dataframe_select_columns(spark)
    dataframe_drop_columns(spark)
    dataframe_select_distinct(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Transaction filtering."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Transaction Filtering")
    print("=" * 60)
    
    spark = create_spark_session("BankingFilter")
    
    transactions = [
        ("TXN001", "ACC001", 100.00, "DEBIT", "2024-01-01"),
        ("TXN002", "ACC002", 5000.00, "CREDIT", "2024-01-01"),
        ("TXN003", "ACC001", 10000.00, "CREDIT", "2024-01-02"),
        ("TXN004", "ACC003", 200.00, "DEBIT", "2024-01-03"),
        ("TXN005", "ACC002", 50.00, "DEBIT", "2024-01-04"),
    ]
    df = spark.createDataFrame(
        transactions,
        ["txn_id", "account_id", "amount", "type", "date"]
    )
    
    print("\nTransaction Filters:")
    
    high_value = df.filter(df.amount > 1000)
    print(f"  High value (>1000): {high_value.count()} transactions")
    high_value.show()
    
    debits = df.filter(df.type == "DEBIT")
    print(f"  Debit transactions: {debits.count()}")
    
    credits = df.filter(df.type == "CREDIT")
    print(f"  Credit transactions: {credits.count()}")
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Patient filtering."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Patient Filtering")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareFilter")
    
    patients = [
        ("P001", "Alice", "Hypertension", 130.5, 45),
        ("P002", "Bob", "Diabetes", 145.8, 38),
        ("P003", "Charlie", "Hypertension", 128.2, 52),
        ("P004", "Diana", "Diabetes", 150.2, 41),
    ]
    df = spark.createDataFrame(
        patients,
        ["patient_id", "name", "condition", "blood_pressure", "age"]
    )
    
    print("\nPatient Filters:")
    
    high_bp = df.filter(df.blood_pressure > 140)
    print(f"  High BP (>140): {high_bp.count()} patients")
    high_bp.show()
    
    diabetes = df.filter(df.condition == "Diabetes")
    print(f"  Diabetes patients: {diabetes.count()}")
    
    seniors = df.filter(df.age >= 50)
    print(f"  Senior patients (age>=50): {seniors.count()}")
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Filter and Selection Operations implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()