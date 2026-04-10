# Topic: RDD Fundamentals
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for RDD Fundamentals

I. INTRODUCTION
Resilient Distributed Datasets (RDDs) are the fundamental data structure in Apache Spark.
They represent immutable, distributed collections of objects that can be processed in parallel
across a cluster. This module covers RDD creation, transformations, actions, and caching.

II. CORE CONCEPTS
- RDD creation (parallelize, textFile, from external sources)
- Transformations (map, filter, flatMap, distinct, etc.)
- Actions (collect, count, reduce, take, save, etc.)
- RDD lineage and fault tolerance
- Caching and persistence

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark import SparkContext
from pyspark.rdd import RDD
from typing import List, Tuple, Callable, Optional
import logging


def create_spark_session(app_name: str = "RDDDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.driver.memory", "2g") \
        .getOrCreate()


def rdd_from_parallelize(sc: SparkContext) -> RDD:
    """
    Create RDD from parallelizing a collection.
    
    This is the most basic way to create an RDD - by parallelizing
    a local Python collection.
    """
    data = list(range(1, 101))
    rdd = sc.parallelize(data, numSlices=4)
    
    print(f"Created RDD with {rdd.count()} elements")
    print(f"Number of partitions: {rdd.getNumPartitions()}")
    print(f"First 5 elements: {rdd.take(5)}")
    
    return rdd


def rdd_from_text_file(sc: SparkContext, 
                       sample_file: Optional[str] = None) -> RDD:
    """
    Create RDD from text file.
    
    Args:
        sc: SparkContext
        sample_file: Optional path to sample text file
    """
    if sample_file and sample_file.startswith("s3://"):
        rdd = sc.textFile(sample_file)
    else:
        sample_data = [
            "Spark is a unified analytics engine",
            "RDD stands for Resilient Distributed Dataset",
            "It provides parallel and fault-tolerant operations",
            "Spark supports multiple languages including Python",
            "DataFrames provide higher-level abstraction"
        ]
        rdd = sc.parallelize(sample_data)
    
    print(f"\nText File RDD:")
    print(f"  Number of lines: {rdd.count()}")
    print(f"  First line: {rdd.first()}")
    
    return rdd


def rdd_map_transformation(rdd: RDD) -> RDD:
    """
    Apply map transformation to RDD.
    
    Map transforms each element using a provided function.
    """
    mapped_rdd = rdd.map(lambda x: (x, x * 2, x ** 2))
    print(f"\nMap Transformation:")
    print(f"  First 5 elements: {mapped_rdd.take(5)}")
    
    return mapped_rdd


def rdd_filter_transformation(rdd: RDD) -> RDD:
    """
    Apply filter transformation to RDD.
    
    Filter returns a new RDD containing only elements
    that satisfy a predicate.
    """
    filtered_rdd = rdd.filter(lambda x: x % 2 == 0)
    print(f"\nFilter Transformation (even numbers):")
    print(f"  Count: {filtered_rdd.count()}")
    print(f"  First 10: {filtered_rdd.take(10)}")
    
    return filtered_rdd


def rdd_flatmap_transformation(rdd: RDD) -> RDD:
    """
    Apply flatMap transformation to RDD.
    
    FlatMap is similar to map but each input element can be
    mapped to zero or more output elements.
    """
    words_rdd = rdd.flatMap(lambda line: line.split())
    print(f"\nFlatMap Transformation (word tokenization):")
    print(f"  Total words: {words_rdd.count()}")
    print(f"  First 10 words: {words_rdd.take(10)}")
    
    return words_rdd


def rdd_distinct_transformation(rdd: RDD) -> RDD:
    """
    Apply distinct transformation to RDD.
    
    Distinct returns a new RDD with distinct elements.
    """
    data_with_duplicates = [1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5]
    rdd = rdd.context.parallelize(data_with_duplicates)
    
    distinct_rdd = rdd.distinct()
    print(f"\nDistinct Transformation:")
    print(f"  Original count: {rdd.count()}")
    print(f"  Distinct count: {distinct_rdd.count()}")
    print(f"  Elements: {sorted(distinct_rdd.collect())}")
    
    return distinct_rdd


def rdd_union_transformation(rdd1: RDD, rdd2: RDD) -> RDD:
    """
    Apply union transformation to RDDs.
    
    Union returns a new RDD containing all elements from both RDDs.
    """
    data1 = [1, 2, 3, 4, 5]
    data2 = [3, 4, 5, 6, 7]
    
    rdd1 = rdd1.context.parallelize(data1)
    rdd2 = rdd1.context.parallelize(data2)
    
    union_rdd = rdd1.union(rdd2)
    print(f"\nUnion Transformation:")
    print(f"  RDD1 count: {rdd1.count()}")
    print(f"  RDD2 count: {rdd2.count()}")
    print(f"  Union count: {union_rdd.count()}")
    print(f"  Elements: {sorted(union_rdd.collect())}")
    
    return union_rdd


def rdd_intersection_transformation(rdd1: RDD, rdd2: RDD) -> RDD:
    """
    Apply intersection transformation to RDDs.
    
    Intersection returns elements common to both RDDs.
    """
    data1 = [1, 2, 3, 4, 5]
    data2 = [3, 4, 5, 6, 7]
    
    rdd1 = rdd1.context.parallelize(data1)
    rdd2 = rdd1.context.parallelize(data2)
    
    intersection_rdd = rdd1.intersection(rdd2)
    print(f"\nIntersection Transformation:")
    print(f"  Elements: {sorted(intersection_rdd.collect())}")
    
    return intersection_rdd


def rdd_subtract_transformation(rdd1: RDD, rdd2: RDD) -> RDD:
    """
    Apply subtract transformation to RDDs.
    
    Subtract returns elements in the first RDD not in the second.
    """
    data1 = [1, 2, 3, 4, 5]
    data2 = [3, 4, 5, 6, 7]
    
    rdd1 = rdd1.context.parallelize(data1)
    rdd2 = rdd1.context.parallelize(data2)
    
    subtract_rdd = rdd1.subtract(rdd2)
    print(f"\nSubtract Transformation:")
    print(f"  Elements: {sorted(subtract_rdd.collect())}")
    
    return subtract_rdd


def rdd_cartesian_transformation(rdd1: RDD, rdd2: RDD) -> RDD:
    """
    Apply cartesian transformation to RDDs.
    
    Cartesian returns the Cartesian product of both RDDs.
    """
    data1 = [1, 2, 3]
    data2 = ["A", "B"]
    
    rdd1 = rdd1.context.parallelize(data1)
    rdd2 = rdd1.context.parallelize(data2)
    
    cartesian_rdd = rdd1.cartesian(rdd2)
    print(f"\nCartesian Transformation:")
    print(f"  Elements: {cartesian_rdd.collect()}")
    
    return cartesian_rdd


def rdd_action_collect(rdd: RDD) -> List:
    """
    Action: collect - returns all elements as a list.
    """
    result = rdd.collect()
    print(f"\nAction - Collect:")
    print(f"  Type: {type(result)}")
    print(f"  Elements: {result[:10]}...")
    return result


def rdd_action_count(rdd: RDD) -> int:
    """
    Action: count - returns the number of elements.
    """
    count = rdd.count()
    print(f"\nAction - Count:")
    print(f"  Count: {count}")
    return count


def rdd_action_take(rdd: RDD, n: int = 5) -> List:
    """
    Action: take - returns first n elements.
    """
    result = rdd.take(n)
    print(f"\nAction - Take({n}):")
    print(f"  Elements: {result}")
    return result


def rdd_action_first(rdd: RDD):
    """
    Action: first - returns the first element.
    """
    result = rdd.first()
    print(f"\nAction - First:")
    print(f"  Element: {result}")
    return result


def rdd_action_reduce(rdd: RDD):
    """
    Action: reduce - aggregates elements using a function.
    """
    sum_result = rdd.reduce(lambda a, b: a + b)
    print(f"\nAction - Reduce (sum):")
    print(f"  Sum: {sum_result}")
    
    max_result = rdd.reduce(lambda a, b: a if a > b else b)
    print(f"  Max: {max_result}")
    
    return sum_result


def rdd_action_fold(rdd: RDD):
    """
    Action: fold - similar to reduce but with initial value.
    """
    result = rdd.fold(0, lambda a, b: a + b)
    print(f"\nAction - Fold:")
    print(f"  Sum with initial 0: {result}")
    return result


def rdd_action_aggregate(rdd: RDD):
    """
    Action: aggregate - more flexible than reduce.
    """
    result = rdd.aggregate(
        (0, 0),
        lambda acc, value: (acc[0] + value, acc[1] + 1),
        lambda acc1, acc2: (acc1[0] + acc2[0], acc1[1] + acc2[1])
    )
    print(f"\nAction - Aggregate:")
    print(f"  Sum and count: {result}")
    print(f"  Average: {result[0] / result[1]}")
    return result


def rdd_action_save_as_text_file(rdd: RDD, output_path: str):
    """
    Action: saveAsTextFile - saves RDD to text files.
    """
    rdd.saveAsTextFile(output_path)
    print(f"\nAction - SaveAsTextFile:")
    print(f"  Saved to: {output_path}")


def rdd_key_value_operations(sc: SparkContext):
    """
    Demonstrate RDD key-value pair operations.
    """
    data = [
        ("apple", 10), ("banana", 5), ("apple", 20),
        ("banana", 15), ("cherry", 25), ("apple", 30)
    ]
    rdd = sc.parallelize(data)
    
    print("\nKey-Value RDD Operations:")
    
    grouped = rdd.groupByKey()
    print(f"  Group by key: {dict(grouped.collect())}")
    
    reduced = rdd.reduceByKey(lambda a, b: a + b)
    print(f"  Reduce by key: {sorted(reduced.collect())}")
    
    mapped = rdd.mapValues(lambda x: x * 2)
    print(f"  Map values: {sorted(mapped.collect())}")
    
    sorted_rdd = rdd.sortByKey()
    print(f"  Sort by key: {sorted(sorted_rdd.collect())}")


def rdd_persistence_demo(sc: SparkContext):
    """
    Demonstrate RDD persistence and caching.
    """
    data = list(range(1, 1001))
    rdd = sc.parallelize(data, numSlices=4)
    
    print("\nRDD Persistence Demo:")
    
    cached_rdd = rdd.cache()
    print(f"  Is cached: {cached_rdd.is_cached}")
    
    count1 = cached_rdd.count()
    count2 = cached_rdd.count()
    print(f"  First count: {count1}")
    print(f"  Second count: {count2}")
    
    persisted_rdd = rdd.persist()
    print(f"  Is persisted: {persisted_rdd.is_cached}")
    
    persisted_rdd.unpersist()


def core_implementation():
    """Core implementation demonstrating RDD fundamentals."""
    print("=" * 60)
    print("RDD FUNDAMENTALS")
    print("=" * 60)
    
    spark = create_spark_session("RDDDemo")
    sc = spark.sparkContext
    
    rdd = rdd_from_parallelize(sc)
    
    rdd_map_transformation(rdd)
    rdd_filter_transformation(rdd)
    
    text_rdd = rdd_from_text_file(sc)
    rdd_flatmap_transformation(text_rdd)
    
    rdd_distinct_transformation(rdd)
    
    rdd_action_collect(rdd)
    rdd_action_count(rdd)
    rdd_action_take(rdd)
    rdd_action_first(rdd)
    rdd_action_reduce(rdd)
    rdd_action_fold(rdd)
    rdd_action_aggregate(rdd)
    
    rdd_key_value_operations(sc)
    rdd_persistence_demo(sc)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Transaction processing."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Transaction Processing")
    print("=" * 60)
    
    spark = create_spark_session("BankingRDD")
    sc = spark.sparkContext
    
    transactions = [
        ("TXN001", "ACC001", 100.00, "DEBIT"),
        ("TXN002", "ACC002", 500.00, "CREDIT"),
        ("TXN003", "ACC001", 200.00, "DEBIT"),
        ("TXN004", "ACC003", 1000.00, "CREDIT"),
        ("TXN005", "ACC002", 150.00, "DEBIT"),
    ]
    
    rdd = sc.parallelize(transactions)
    
    print("\nTransaction Processing:")
    
    debit_rdd = rdd.filter(lambda x: x[3] == "DEBIT")
    print(f"  Debit transactions: {debit_rdd.count()}")
    
    credit_rdd = rdd.filter(lambda x: x[3] == "CREDIT")
    print(f"  Credit transactions: {credit_rdd.count()}")
    
    total_by_account = rdd.map(lambda x: (x[1], x[2])) \
                          .reduceByKey(lambda a, b: a + b)
    print(f"  Total by account: {sorted(total_by_account.collect())}")
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Patient record processing."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Patient Records")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareRDD")
    sc = spark.sparkContext
    
    patient_visits = [
        ("P001", "VISIT001", "Checkup", 50.00),
        ("P002", "VISIT002", "Surgery", 5000.00),
        ("P001", "VISIT003", "Lab Test", 200.00),
        ("P003", "VISIT004", "Emergency", 1500.00),
        ("P002", "VISIT005", "Follow-up", 100.00),
    ]
    
    rdd = sc.parallelize(patient_visits)
    
    print("\nPatient Visit Processing:")
    
    high_cost = rdd.filter(lambda x: x[3] > 500)
    print(f"  High-cost visits (>500): {high_cost.count()}")
    
    cost_by_patient = rdd.map(lambda x: (x[0], x[3])) \
                         .reduceByKey(lambda a, b: a + b)
    print(f"  Total cost by patient: {sorted(cost_by_patient.collect())}")
    
    visits_by_patient = rdd.map(lambda x: (x[0], 1)) \
                          .reduceByKey(lambda a, b: a + b)
    print(f"  Visits by patient: {sorted(visits_by_patient.collect())}")
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing RDD Fundamentals implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
