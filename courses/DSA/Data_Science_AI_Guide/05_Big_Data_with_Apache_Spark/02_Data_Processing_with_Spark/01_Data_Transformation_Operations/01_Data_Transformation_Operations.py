# Topic: Data Transformation Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Transformation Operations

I. INTRODUCTION
Data transformation operations are the core of data processing in Spark. This module
covers various transformation operations including map, flatMap, filter, distinct,
union, intersection, and more complex transformations for data processing pipelines.

II. CORE CONCEPTS
- Basic transformations (map, filter, flatMap)
- Set operations (union, intersection, subtract, distinct)
- Key-value pair transformations
- Custom transformations
- Chaining transformations
- Pipeline composition

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark import SparkContext
from pyspark.sql import functions as F
from pyspark.sql import types as T
from typing import List, Tuple, Callable, Any


def create_spark_session(app_name: str = "DataTransformDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()


def basic_map_transformation(sc: SparkContext) -> None:
    """
    Demonstrate basic map transformation.
    
    Map applies a function to each element and returns a new RDD.
    """
    data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    rdd = sc.parallelize(data, numSlices=4)
    
    print("\nBasic Map Transformation:")
    
    mapped = rdd.map(lambda x: (x, x * 2, x ** 2))
    print(f"  Original: {rdd.collect()}")
    print(f"  Mapped (x, 2x, x^2): {mapped.collect()}")
    
    square_root = rdd.map(lambda x: x ** 0.5)
    print(f"  Square roots: {square_root.collect()[:5]}...")


def flatmap_transformation(sc: SparkContext) -> None:
    """
    Demonstrate flatMap transformation.
    
    flatMap is similar to map but can return multiple elements
    for each input element.
    """
    sentences = [
        "Spark is powerful",
        "DataFrames provide SQL interface",
        "RDDs are the foundation"
    ]
    rdd = sc.parallelize(sentences)
    
    print("\nFlatMap Transformation:")
    
    words = rdd.flatMap(lambda s: s.split())
    print(f"  Sentences: {sentences}")
    print(f"  Words: {words.collect()}")
    
    char_lists = rdd.flatMap(lambda s: list(s))
    print(f"  Characters: {char_lists.collect()[:20]}...")


def filter_transformation(sc: SparkContext) -> None:
    """
    Demonstrate filter transformation.
    
    Filter returns elements that satisfy a condition.
    """
    numbers = list(range(1, 51))
    rdd = sc.parallelize(numbers)
    
    print("\nFilter Transformation:")
    
    even = rdd.filter(lambda x: x % 2 == 0)
    print(f"  Even numbers: {even.collect()[:10]}...")
    print(f"  Count: {even.count()}")
    
    greater_than_25 = rdd.filter(lambda x: x > 25)
    print(f"  Numbers > 25: {greater_than_25.collect()}")
    
    composite = rdd.filter(lambda x: x % 2 == 0 and x % 3 == 0)
    print(f"  Divisible by 2 and 3: {composite.collect()}")


def distinct_transformation(sc: SparkContext) -> None:
    """
    Demonstrate distinct transformation.
    
    Distinct returns unique elements in the RDD.
    """
    data_with_dups = [1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5]
    rdd = sc.parallelize(data_with_dups)
    
    print("\nDistinct Transformation:")
    print(f"  Original: {rdd.collect()}")
    print(f"  Distinct: {sorted(rdd.distinct().collect())}")


def union_transformation(sc: SparkContext) -> None:
    """
    Demonstrate union transformation.
    
    Union combines elements from two RDDs (with duplicates).
    """
    rdd1 = sc.parallelize([1, 2, 3, 4, 5])
    rdd2 = sc.parallelize([3, 4, 5, 6, 7])
    
    print("\nUnion Transformation:")
    print(f"  RDD1: {rdd1.collect()}")
    print(f"  RDD2: {rdd2.collect()}")
    print(f"  Union: {sorted(rdd1.union(rdd2).collect())}")


def intersection_transformation(sc: SparkContext) -> None:
    """
    Demonstrate intersection transformation.
    
    Intersection returns common elements.
    """
    rdd1 = sc.parallelize([1, 2, 3, 4, 5])
    rdd2 = sc.parallelize([3, 4, 5, 6, 7])
    
    print("\nIntersection Transformation:")
    print(f"  Common elements: {sorted(rdd1.intersection(rdd2).collect())}")


def subtract_transformation(sc: SparkContext) -> None:
    """
    Demonstrate subtract transformation.
    
    Subtract returns elements in the first RDD not in the second.
    """
    rdd1 = sc.parallelize([1, 2, 3, 4, 5])
    rdd2 = sc.parallelize([3, 4, 5, 6, 7])
    
    print("\nSubtract Transformation:")
    print(f"  RDD1 - RDD2: {sorted(rdd1.subtract(rdd2).collect())}")


def cartesian_transformation(sc: SparkContext) -> None:
    """
    Demonstrate cartesian transformation.
    
    Cartesian returns the Cartesian product of two RDDs.
    """
    rdd1 = sc.parallelize([1, 2, 3])
    rdd2 = sc.parallelize(["A", "B"])
    
    print("\nCartesian Transformation:")
    result = rdd1.cartesian(rdd2).collect()
    print(f"  Cartesian product: {result}")


def key_value_transformations(sc: SparkContext) -> None:
    """
    Demonstrate key-value pair transformations.
    """
    data = [
        ("apple", 10), ("banana", 5), ("apple", 20),
        ("banana", 15), ("cherry", 25), ("apple", 30)
    ]
    rdd = sc.parallelize(data)
    
    print("\nKey-Value Transformations:")
    
    grouped = rdd.groupByKey()
    print(f"  Group by key: {dict(grouped.collect())}")
    
    reduced = rdd.reduceByKey(lambda a, b: a + b)
    print(f"  Reduce by key: {sorted(reduced.collect())}")
    
    mapped = rdd.mapValues(lambda x: x * 2)
    print(f"  Map values: {sorted(mapped.collect())}")
    
    keys_only = rdd.keys()
    print(f"  Keys: {sorted(keys_only.collect())}")
    
    values_only = rdd.values()
    print(f"  Values: {sorted(values_only.collect())}")


def mappartitions_transformation(sc: SparkContext) -> None:
    """
    Demonstrate mapPartitions transformation.
    
    mapPartitions applies a function to each partition at once.
    """
    data = list(range(1, 21))
    rdd = sc.parallelize(data, numSlices=4)
    
    print("\nMapPartitions Transformation:")
    
    def process_partition(partition):
        return [x * 10 for x in partition]
    
    result = rdd.mapPartitions(process_partition)
    print(f"  Original partitions: {rdd.glom().collect()}")
    print(f"  Result: {result.collect()}")


def dataframe_transformations(spark: SparkSession) -> None:
    """
    Demonstrate DataFrame transformation operations.
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
    
    print("\nDataFrame Transformations:")
    
    selected = df.select("name", "salary")
    print(f"  Selected columns: {selected.columns}")
    
    with_new_col = df.withColumn("bonus", df.salary * 0.1)
    print(f"  With bonus column: {with_new_col.columns}")
    
    filtered = df.filter(df.age > 28)
    print(f"  Filtered rows: {filtered.count()}")
    
    sorted_df = df.sort(F.desc("salary"))
    print(f"  Sorted by salary (desc): {sorted_df.collect()}")


def chain_transformations(sc: SparkContext) -> None:
    """
    Demonstrate chaining multiple transformations.
    """
    data = list(range(1, 101))
    rdd = sc.parallelize(data)
    
    print("\nChain Transformations:")
    
    result = rdd \
        .filter(lambda x: x % 2 == 0) \
        .map(lambda x: x * 2) \
        .filter(lambda x: x > 50) \
        .take(10)
    
    print(f"  Chained result (first 10): {result}")


def core_implementation():
    """Core implementation demonstrating data transformations."""
    print("=" * 60)
    print("DATA TRANSFORMATION OPERATIONS")
    print("=" * 60)
    
    spark = create_spark_session()
    sc = spark.sparkContext
    
    basic_map_transformation(sc)
    flatmap_transformation(sc)
    filter_transformation(sc)
    distinct_transformation(sc)
    union_transformation(sc)
    intersection_transformation(sc)
    subtract_transformation(sc)
    cartesian_transformation(sc)
    key_value_transformations(sc)
    mappartitions_transformation(sc)
    dataframe_transformations(spark)
    chain_transformations(sc)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Transaction processing."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Transaction Processing")
    print("=" * 60)
    
    spark = create_spark_session("BankingTransform")
    sc = spark.sparkContext
    
    transactions = [
        ("TXN001", "ACC001", 100.00, "DEBIT", "2024-01-01"),
        ("TXN002", "ACC002", 500.00, "CREDIT", "2024-01-01"),
        ("TXN003", "ACC001", 200.00, "DEBIT", "2024-01-02"),
        ("TXN004", "ACC003", 1000.00, "CREDIT", "2024-01-02"),
        ("TXN005", "ACC002", 150.00, "DEBIT", "2024-01-03"),
    ]
    rdd = sc.parallelize(transactions)
    
    print("\nTransaction Transformations:")
    
    debit_txns = rdd.filter(lambda x: x[3] == "DEBIT")
    print(f"  Debit transactions: {debit_txns.count()}")
    
    credit_txns = rdd.filter(lambda x: x[3] == "CREDIT")
    print(f"  Credit transactions: {credit_txns.count()}")
    
    total_by_account = rdd.map(lambda x: (x[1], x[2])) \
                         .reduceByKey(lambda a, b: a + b)
    print(f"  Total by account: {sorted(total_by_account.collect())}")
    
    amounts = rdd.map(lambda x: x[2])
    print(f"  Total amount: {amounts.reduce(lambda a, b: a + b)}")
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Patient record processing."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Patient Records")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareTransform")
    sc = spark.sparkContext
    
    patient_visits = [
        ("P001", "VISIT001", "Checkup", 50.00),
        ("P002", "VISIT002", "Surgery", 5000.00),
        ("P001", "VISIT003", "Lab Test", 200.00),
        ("P003", "VISIT004", "Emergency", 1500.00),
        ("P002", "VISIT005", "Follow-up", 100.00),
    ]
    rdd = sc.parallelize(patient_visits)
    
    print("\nPatient Visit Transformations:")
    
    high_cost = rdd.filter(lambda x: x[3] > 500)
    print(f"  High-cost visits (>500): {high_cost.count()}")
    
    cost_by_patient = rdd.map(lambda x: (x[0], x[3])) \
                         .reduceByKey(lambda a, b: a + b)
    print(f"  Total cost by patient: {sorted(cost_by_patient.collect())}")
    
    visit_types = rdd.map(lambda x: x[2])
    print(f"  Visit types: {visit_types.distinct().collect()}")
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Data Transformation Operations implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()