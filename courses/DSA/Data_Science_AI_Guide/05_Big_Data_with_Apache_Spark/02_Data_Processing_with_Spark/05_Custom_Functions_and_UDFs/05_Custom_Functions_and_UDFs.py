# Topic: Custom Functions and UDFs
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Custom Functions and UDFs

I. INTRODUCTION
User Defined Functions (UDFs) allow extending Spark's built-in functions with 
custom logic. This module covers Python UDFs, Pandas UDFs, and best practices 
for writing efficient custom functions.

II. CORE CONCEPTS
- Python UDFs
- Pandas UDFs (vectorized UDFs)
- UDF registration and usage
- Performance considerations
- Type handling in UDFs
- Lambda functions vs UDFs

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StringType, IntegerType, DoubleType, StructType, StructField
from typing import List, Callable, Any
import random


def create_spark_session(app_name: str = "UDFDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()


def basic_python_udf(spark: SparkSession) -> None:
    """
    Demonstrate basic Python UDF.
    """
    data = [
        ("Alice", 25, "NYC"),
        ("Bob", 30, "LA"),
        ("Charlie", 35, "NYC"),
    ]
    df = spark.createDataFrame(data, ["name", "age", "city"])
    
    print("\nBasic Python UDF:")
    
    def uppercase_name(name):
        return name.upper()
    
    uppercase_udf = F.udf(uppercase_name, StringType())
    
    result = df.select(
        uppercase_udf(F.col("name")).alias("uppercase_name"),
        F.col("age"),
        F.col("city")
    )
    result.show()


def lambda_udf(spark: SparkSession) -> None:
    """
    Demonstrate lambda UDF.
    """
    data = [
        ("Alice", 25, 50000),
        ("Bob", 30, 60000),
        ("Charlie", 35, 75000),
    ]
    df = spark.createDataFrame(data, ["name", "age", "salary"])
    
    print("\nLambda UDF:")
    
    bonus_udf = F.udf(lambda salary: salary * 0.1, DoubleType())
    tax_udf = F.udf(lambda salary: salary * 0.2, DoubleType())
    
    result = df.withColumn("bonus", bonus_udf(F.col("salary"))) \
               .withColumn("tax", tax_udf(F.col("salary"))) \
               .withColumn("net", F.col("salary") - F.col("tax") + F.col("bonus"))
    result.show()


def conditional_udf(spark: SparkSession) -> None:
    """
    Demonstrate conditional UDF.
    """
    data = [
        ("Alice", 25, 50000),
        ("Bob", 30, 60000),
        ("Charlie", 35, 75000),
        ("Diana", 28, 55000),
    ]
    df = spark.createDataFrame(data, ["name", "age", "salary"])
    
    print("\nConditional UDF:")
    
    def categorize_salary(salary):
        if salary < 55000:
            return "Low"
        elif salary < 70000:
            return "Medium"
        else:
            return "High"
    
    category_udf = F.udf(categorize_salary, StringType())
    
    result = df.withColumn("category", category_udf(F.col("salary")))
    result.show()


def multi_column_udf(spark: SparkSession) -> None:
    """
    Demonstrate UDF with multiple input columns.
    """
    data = [
        ("Alice", 25, 50000),
        ("Bob", 30, 60000),
    ]
    df = spark.createDataFrame(data, ["name", "age", "salary"])
    
    print("\nMulti-Column UDF:")
    
    def calculate_bonus(age, salary):
        if age < 30:
            return salary * 0.15
        elif age < 40:
            return salary * 0.1
        else:
            return salary * 0.05
    
    bonus_udf = F.udf(calculate_bonus, DoubleType())
    
    result = df.withColumn("bonus", bonus_udf(F.col("age"), F.col("salary")))
    result.show()


def register_udf(spark: SparkSession) -> None:
    """
    Demonstrate UDF registration.
    """
    data = [
        ("Alice", "NYC"),
        ("Bob", "LA"),
    ]
    df = spark.createDataFrame(data, ["name", "city"])
    
    print("\nRegistered UDF:")
    
    spark.udf.register("city_cleaner", 
                       lambda city: city.upper() if city else city,
                       StringType())
    
    result = df.select(
        F.col("name"),
        F.expr("city_cleaner(city)").alias("clean_city")
    )
    result.show()


def udf_with_complex_logic(spark: SparkSession) -> None:
    """
    Demonstrate UDF with complex logic.
    """
    data = [
        ("Alice", "alice@example.com"),
        ("Bob", "bob@example.com"),
        ("Charlie", ""),
    ]
    df = spark.createDataFrame(data, ["name", "email"])
    
    print("\nUDF with Complex Logic:")
    
    def process_email(email):
        if not email or email == "":
            return "NO_EMAIL"
        email = email.lower()
        username = email.split("@")[0] if "@" in email else email
        return f"{username}@domain.com"
    
    email_udf = F.udf(process_email, StringType())
    
    result = df.withColumn("processed_email", email_udf(F.col("email")))
    result.show()


def pandas_udf_basic(spark: SparkSession) -> None:
    """
    Demonstrate Pandas UDF (vectorized UDF).
    """
    try:
        data = [
            (1, 100.0),
            (2, 200.0),
            (3, 150.0),
        ]
        df = spark.createDataFrame(data, ["id", "value"])
        
        print("\nPandas UDF:")
        
        @F.pandas_udf(DoubleType())
        def square_udf(s: "pandas.Series") -> "pandas.Series":
            return s ** 2
        
        result = df.withColumn("squared", square_udf(F.col("value")))
        result.show()
    except Exception as e:
        print(f"  Note: Pandas UDF requires Arrow support. Error: {e}")


def udf_with_struct(spark: SparkSession) -> None:
    """
    Demonstrate UDF returning struct type.
    """
    data = [
        ("Alice", 25),
        ("Bob", 30),
    ]
    df = spark.createDataFrame(data, ["name", "age"])
    
    print("\nUDF with Struct:")
    
    schema = StructType([
        StructField("name", StringType(), False),
        StructField("age", IntegerType(), False),
        StructField("status", StringType(), False)
    ])
    
    def create_record(name, age):
        status = "adult" if age >= 18 else "minor"
        return (name, age, status)
    
    record_udf = F.udf(create_record, schema)
    
    result = df.select(record_udf(F.col("name"), F.col("age")).alias("record"))
    result.show()
    result.printSchema()


def udf_performance_tips(spark: SparkSession) -> None:
    """
    Demonstrate UDF performance considerations.
    """
    data = [(i, i * 10) for i in range(1, 101)]
    df = spark.createDataFrame(data, ["id", "value"])
    
    print("\nUDF Performance Comparison:")
    
    native_result = df.select(
        F.col("id"),
        F.col("value"),
        (F.col("value") * 2).alias("doubled_native")
    )
    
    def double_value(x):
        return x * 2
    
    double_udf = F.udf(double_value, IntegerType())
    
    udf_result = df.select(
        F.col("id"),
        F.col("value"),
        double_udf(F.col("value")).alias("doubled_udf")
    )
    
    print("  Native function (recommended):")
    native_result.show(5)
    
    print("  UDF function (slower):")
    udf_result.show(5)


def core_implementation():
    """Core implementation demonstrating UDFs."""
    print("=" * 60)
    print("CUSTOM FUNCTIONS AND UDFs")
    print("=" * 60)
    
    spark = create_spark_session()
    
    basic_python_udf(spark)
    lambda_udf(spark)
    conditional_udf(spark)
    multi_column_udf(spark)
    register_udf(spark)
    udf_with_complex_logic(spark)
    udf_with_struct(spark)
    udf_performance_tips(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Custom functions for analytics."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Custom Functions")
    print("=" * 60)
    
    spark = create_spark_session("BankingUDF")
    
    transactions = [
        ("TXN001", "ACC001", 5000.00, "CREDIT"),
        ("TXN002", "ACC002", 100.00, "DEBIT"),
        ("TXN003", "ACC001", 2000.00, "DEBIT"),
    ]
    df = spark.createDataFrame(
        transactions, ["txn_id", "account_id", "amount", "type"]
    )
    
    print("\nTransaction Processing:")
    
    def categorize_amount(amount):
        if amount < 500:
            return "Small"
        elif amount < 2000:
            return "Medium"
        else:
            return "Large"
    
    amount_category = F.udf(categorize_amount, StringType())
    
    result = df.withColumn("amount_category", 
                          amount_category(F.col("amount")))
    result.show()
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Custom functions for patient data."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Custom Functions")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareUDF")
    
    patients = [
        ("P001", "Alice", 130.5, "Hypertension"),
        ("P002", "Bob", 145.8, "Diabetes"),
        ("P003", "Charlie", 120.0, "Normal"),
    ]
    df = spark.createDataFrame(
        patients, ["patient_id", "name", "blood_pressure", "condition"]
    )
    
    print("\nPatient Risk Assessment:")
    
    def assess_risk(bp, condition):
        risk_level = "Low"
        if condition == "Diabetes":
            risk_level = "High"
        elif condition == "Hypertension":
            if bp > 140:
                risk_level = "High"
            else:
                risk_level = "Medium"
        return risk_level
    
    risk_udf = F.udf(assess_risk, StringType())
    
    result = df.withColumn("risk_level", 
                          risk_udf(F.col("blood_pressure"), F.col("condition")))
    result.show()
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Custom Functions and UDFs implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
