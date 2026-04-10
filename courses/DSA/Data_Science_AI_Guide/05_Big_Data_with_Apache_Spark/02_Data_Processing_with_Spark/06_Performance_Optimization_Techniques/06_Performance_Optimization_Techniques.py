# Topic: Performance Optimization Techniques
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Performance Optimization Techniques

I. INTRODUCTION
This module covers various Spark performance optimization techniques including
caching, partitioning, shuffle optimization, broadcast joins, and query optimization
strategies for large-scale data processing.

II. CORE CONCEPTS
- Caching and persistence strategies
- Partition management
- Broadcast joins
- Shuffle optimization
- Adaptive query execution
- Catalyst optimizer

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.window import Window
import time


def create_spark_session(app_name: str = "PerfOptimizeDemo") -> SparkSession:
    """Create and configure a Spark session."""
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .config("spark.sql.adaptive.enabled", "true") \
        .getOrCreate()


def caching_strategies(spark: SparkSession) -> None:
    """
    Demonstrate caching strategies.
    """
    data = [(i, f"value_{i}", i * 10) for i in range(1, 1001)]
    df = spark.createDataFrame(data, ["id", "value", "score"])
    
    print("\nCaching Strategies:")
    
    print("  Without cache:")
    start = time.time()
    result1 = df.filter(df.id > 500).select("id", "value")
    count1 = result1.count()
    time1 = time.time() - start
    print(f"    Time: {time1:.3f}s, Count: {count1}")
    
    print("  With cache:")
    cached_df = df.filter(df.id > 500).cache()
    start = time.time()
    result2 = cached_df.select("id", "value")
    count2 = result2.count()
    time2 = time.time() - start
    print(f"    Time: {time2:.3f}s, Count: {count2}")
    
    start = time.time()
    result3 = cached_df.select("id", "value")
    count3 = result3.count()
    time3 = time.time() - start
    print(f"    Second run: {time3:.3f}s")
    
    cached_df.unpersist()


def persistence_levels(spark: SparkSession) -> None:
    """
    Demonstrate different persistence levels.
    """
    from pyspark import StorageLevel
    
    data = [(i, f"value_{i}") for i in range(1, 101)]
    df = spark.createDataFrame(data, ["id", "value"])
    
    print("\nPersistence Levels:")
    
    print("  MEMORY_AND_DISK:")
    df.persist(StorageLevel.MEMORY_AND_DISK)
    print(f"    Cached: {df.is_cached}")
    df.unpersist()
    
    print("  MEMORY_ONLY:")
    df.persist(StorageLevel.MEMORY_ONLY)
    print(f"    Cached: {df.is_cached}")
    df.unpersist()


def partition_strategies(spark: SparkSession) -> None:
    """
    Demonstrate partition strategies.
    """
    data = [(i, f"value_{i}", i % 10) for i in range(1, 1001)]
    df = spark.createDataFrame(data, ["id", "value", "category"])
    
    print("\nPartition Strategies:")
    
    print("  Repartition by column:")
    by_category = df.repartition(10, "category")
    print(f"    Partitions: {by_category.rdd.getNumPartitions()}")
    
    print("  Coalesce:")
    coalesced = df.coalesce(2)
    print(f"    Partitions: {coalesced.rdd.getNumPartitions()}")
    
    print("  PartitionBy:")
    from pyspark.sql import Window
    window_spec = Window.partitionBy("category")
    partitioned = df.withColumn("partition_id", F.row_number().over(window_spec))
    print(f"    Applied partitioning logic")


def broadcast_join_demo(spark: SparkSession) -> None:
    """
    Demonstrate broadcast join optimization.
    """
    employees = [
        (1, "Alice", 100),
        (2, "Bob", 200),
        (3, "Charlie", 300),
    ]
    employees_df = spark.createDataFrame(
        employees, ["emp_id", "name", "dept_id"]
    )
    
    departments = [
        (100, "Engineering"),
        (200, "Sales"),
        (300, "Marketing"),
        (400, "HR"),
    ]
    departments_df = spark.createDataFrame(
        departments, ["dept_id", "dept_name"]
    )
    
    print("\nBroadcast Join:")
    
    print("  Regular join:")
    start = time.time()
    result1 = employees_df.join(departments_df, "dept_id")
    count1 = result1.count()
    print(f"    Time: {time.time()-start:.3f}s")
    
    print("  Broadcast join:")
    start = time.time()
    result2 = employees_df.join(
        F.broadcast(departments_df), "dept_id"
    )
    count2 = result2.count()
    print(f"    Time: {time.time()-start:.3f}s")
    
    result2.show()


def shuffle_optimization(spark: SparkSession) -> None:
    """
    Demonstrate shuffle optimization.
    """
    data = [(i, f"value_{i}", i % 5) for i in range(1, 1001)]
    df = spark.createDataFrame(data, ["id", "value", "category"])
    
    print("\nShuffle Optimization:")
    
    print("  Default shuffle partitions:")
    print(f"    Partitions: {df.rdd.getNumPartitions()}")
    
    print("  Reduced shuffle partitions:")
    df_reduced = df.repartition(2)
    print(f"    Partitions: {df_reduced.rdd.getNumPartitions()}")
    
    print("  Increase shuffle partitions:")
    df_increased = df.repartition(20)
    print(f"    Partitions: {df_increased.rdd.getNumPartitions()}")


def aqe_optimization(spark: SparkSession) -> None:
    """
    Demonstrate Adaptive Query Execution (AQE).
    """
    spark.conf.set("spark.sql.adaptive.enabled", "true")
    spark.conf.set("spark.sql.adaptive.coalescePartitions.enabled", "true")
    
    data = [(i, f"value_{i}", i % 10) for i in range(1, 1001)]
    df = spark.createDataFrame(data, ["id", "value", "category"])
    
    print("\nAQE Optimization:")
    print(f"  AQE enabled: {spark.conf.get('spark.sql.adaptive.enabled')}")
    
    result = df.filter(df.id > 900).groupBy("category").count()
    print(f"  Result count: {result.count()}")
    result.show()


def predicate_pushdown(spark: SparkSession) -> None:
    """
    Demonstrate predicate pushdown.
    """
    data = [(i, f"value_{i}", i * 10) for i in range(1, 1001)]
    df = spark.createDataFrame(data, ["id", "value", "score"])
    
    print("\nPredicate Pushdown:")
    
    print("  Filter before select:")
    start = time.time()
    result1 = df.filter(df.id > 500).select("id", "value")
    count1 = result1.count()
    print(f"    Time: {time.time()-start:.3f}s, Count: {count1}")
    
    print("  Select before filter:")
    start = time.time()
    result2 = df.select("id", "value").filter(df.id > 500)
    count2 = result2.count()
    print(f"    Time: {time.time()-start:.3f}s, Count: {count2}")


def column_pruning(spark: SparkSession) -> None:
    """
    Demonstrate column pruning.
    """
    data = [(i, f"value_{i}", i * 10, i * 100) for i in range(1, 101)]
    df = spark.createDataFrame(data, ["id", "value", "score", "rank"])
    
    print("\nColumn Pruning:")
    
    print("  Select specific columns:")
    result = df.select("id", "value")
    print(f"    Columns: {result.columns}")
    
    print("  Drop columns:")
    result = df.drop("score")
    print(f"    Columns: {result.columns}")


def core_implementation():
    """Core implementation demonstrating performance optimization."""
    print("=" * 60)
    print("PERFORMANCE OPTIMIZATION TECHNIQUES")
    print("=" * 60)
    
    spark = create_spark_session()
    
    caching_strategies(spark)
    persistence_levels(spark)
    partition_strategies(spark)
    broadcast_join_demo(spark)
    shuffle_optimization(spark)
    aqe_optimization(spark)
    predicate_pushdown(spark)
    column_pruning(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Performance optimization."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Performance Optimization")
    print("=" * 60)
    
    spark = create_spark_session("BankingOptimize")
    
    accounts = [
        (f"ACC{i:03d}", f"Customer_{i}", float(i * 1000))
        for i in range(1, 501)
    ]
    df = spark.createDataFrame(
        accounts, ["account_id", "name", "balance"]
    )
    
    print("\nAccount Processing:")
    
    cached = df.filter(df.balance > 25000).cache()
    print(f"  Cached accounts: {cached.count()}")
    
    result = cached.groupBy().agg(
        F.sum("balance").alias("total"),
        F.count("account_id").alias("count")
    )
    result.show()
    
    cached.unpersist()
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Performance optimization."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Performance Optimization")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareOptimize")
    
    patients = [
        (f"P{i:03d}", f"Patient_{i}", f"Condition_{i%5}", float(i * 100))
        for i in range(1, 501)
    ]
    df = spark.createDataFrame(
        patients, ["patient_id", "name", "condition", "cost"]
    )
    
    print("\nPatient Processing:")
    
    cached = df.filter(df.cost > 25000).cache()
    print(f"  High-cost patients: {cached.count()}")
    
    result = cached.groupBy("condition").agg(
        F.sum("cost").alias("total_cost"),
        F.count("patient_id").alias("patient_count")
    )
    result.show()
    
    cached.unpersist()
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Performance Optimization implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
