# Topic: Spark Configuration Optimization
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Spark Configuration Optimization

I. INTRODUCTION
Proper Spark configuration is critical for optimal performance in production environments.
This module covers Spark configuration options, memory management, CPU settings,
and optimization techniques for various workloads.

II. CORE CONCEPTS
- Memory configuration (driver, executor)
- CPU and parallelism settings
- Shuffle configuration
- Compression and serialization
- Adaptive query execution
- Dynamic allocation

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark import SparkConf
from typing import Dict, Any


def create_optimized_spark_session(
    app_name: str = "OptimizedSpark",
    driver_memory: str = "4g",
    executor_memory: str = "2g",
    max_cores: int = 4
) -> SparkSession:
    """
    Create an optimized Spark session.
    
    Args:
        app_name: Application name
        driver_memory: Driver memory allocation
        executor_memory: Executor memory allocation
        max_cores: Maximum cores to use
        
    Returns:
        Configured SparkSession
    """
    builder = SparkSession.builder \
        .appName(app_name) \
        .master("local[*]")
    
    builder = builder \
        .config("spark.driver.memory", driver_memory) \
        .config("spark.executor.memory", executor_memory) \
        .config("spark.cores.max", str(max_cores)) \
        .config("spark.driver.maxResultSize", "2g") \
        .config("spark.network.timeout", "800s") \
        .config("spark.executor.heartbeatInterval", "30s") \
        .config("spark.sql.shuffle.partitions", "8") \
        .config("spark.default.parallelism", "4")
    
    builder = builder \
        .config("spark.sql.adaptive.enabled", "true") \
        .config("spark.sql.adaptive.coalescePartitions.enabled", "true") \
        .config("spark.sql.adaptive.coalescePartitions.minPartitionNum", "1")
    
    builder = builder \
        .config("spark.serializer", 
                "org.apache.spark.serializer.KryoSerializer") \
        .config("spark.kryo.registrationRequired", "false")
    
    return builder.getOrCreate()


def configure_memory_settings() -> Dict[str, str]:
    """
    Configure memory-related settings.
    
    Returns:
        Dictionary with memory configuration
    """
    memory_config = {
        "spark.driver.memory": "4g",
        "spark.executor.memory": "2g",
        "spark.driver.maxResultSize": "2g",
        "spark.memory.fraction": "0.6",
        "spark.memory.storageFraction": "0.5",
        "spark.memory.offHeap.enabled": "false",
        "spark.executor.pemnemonic": "20g",
    }
    return memory_config


def configure_cpu_settings() -> Dict[str, str]:
    """
    Configure CPU-related settings.
    
    Returns:
        Dictionary with CPU configuration
    """
    cpu_config = {
        "spark.cores.max": "4",
        "spark.default.parallelism": "4",
        "spark.task.cpus": "1",
        "spark.executor.cores": "2",
        "spark.driver.cores": "2",
    }
    return cpu_config


def configure_shuffle_settings() -> Dict[str, str]:
    """
    Configure shuffle settings.
    
    Returns:
        Dictionary with shuffle configuration
    """
    shuffle_config = {
        "spark.sql.shuffle.partitions": "200",
        "spark.sql.shuffle.partitions.default": "200",
        "spark.shuffle.service.enabled": "false",
        "spark.dynamicAllocation.enabled": "false",
        "spark.shuffle.compress": "true",
        "spark.shuffle.spill.compress": "true",
    }
    return shuffle_config


def configure_serialization_settings() -> Dict[str, str]:
    """
    Configure serialization settings.
    
    Returns:
        Dictionary with serialization configuration
    """
    ser_config = {
        "spark.serializer": 
            "org.apache.spark.serializer.KryoSerializer",
        "spark.kryo.registrationRequired": "false",
        "spark.kryo.buffersize": "64k",
        "spark.rdd.compress": "true",
    }
    return ser_config


def configure_adaptive_query_settings() -> Dict[str, str]:
    """
    Configure adaptive query execution settings.
    
    Returns:
        Dictionary with AQE configuration
    """
    aqe_config = {
        "spark.sql.adaptive.enabled": "true",
        "spark.sql.adaptive.coalescePartitions.enabled": "true",
        "spark.sql.adaptive.coalescePartitions.minPartitionNum": "1",
        "spark.sql.adaptive.coalescePartitions.parallelismFirst": "true",
        "spark.sql.adaptive.skewJoin.enabled": "true",
        "spark.sql.adaptive.skewJoin.autoBroadcastEnabled": "true",
        "spark.sql.adaptive.autoBroadcastJoinThreshold": "10485760",
    }
    return aqe_config


def configure_ui_and_logging() -> Dict[str, str]:
    """
    Configure UI and logging settings.
    
    Returns:
        Dictionary with UI/logging configuration
    """
    ui_config = {
        "spark.ui.port": "4040",
        "spark.ui.enabled": "true",
        "spark.ui.retainedStages": "100",
        "spark.ui.retainedJobs": "100",
        "spark.eventLog.enabled": "false",
        "spark.eventLog.dir": "/tmp/spark-events",
    }
    return ui_config


def configure_dynamic_allocation() -> Dict[str, str]:
    """
    Configure dynamic allocation settings.
    
    Returns:
        Dictionary with dynamic allocation config
    """
    dyn_config = {
        "spark.dynamicAllocation.enabled": "false",
        "spark.dynamicAllocation.minExecutors": "1",
        "spark.dynamicAllocation.maxExecutors": "3",
        "spark.dynamicAllocation.initialExecutors": "1",
        "spark.dynamicAllocation.executorIdleTimeout": "600s",
    }
    return dyn_config


def get_current_configuration(spark: SparkSession) -> Dict[str, Any]:
    """
    Get current Spark configuration.
    
    Args:
        spark: SparkSession
        
    Returns:
        Dictionary with current config
    """
    conf = spark.sparkContext.getConf()
    config_dict = {}
    for item in conf.getAll():
        config_dict[item[0]] = item[1]
    return config_dict


def print_configuration(spark: SparkSession, prefix: str = "") -> None:
    """
    Print Spark configuration.
    
    Args:
        spark: SparkSession
        prefix: Prefix for print output
    """
    print(f"\n{prefix}Spark Configuration:")
    conf = spark.sparkContext.getConf()
    print(f"  App Name: {spark.sparkContext.appName}")
    print(f"  Master: {spark.sparkContext.master}")
    print(f"  Default Parallelism: {conf.get('spark.default.parallelism', 'N/A')}")
    print(f"  Driver Memory: {conf.get('spark.driver.memory', 'N/A')}")
    print(f"  Executor Memory: {conf.get('spark.executor.memory', 'N/A')}")
    print(f"  Shuffle Partitions: {conf.get('spark.sql.shuffle.partitions', 'N/A')}")
    print(f"  AQE Enabled: {conf.get('spark.sql.adaptive.enabled', 'N/A')}")


def demonstrate_performance_settings(spark: SparkSession) -> None:
    """
    Demonstrate various performance settings.
    """
    print("\n" + "=" * 50)
    print("PERFORMANCE SETTINGS DEMONSTRATION")
    print("=" * 50)
    
    sc = spark.sparkContext
    
    data = list(range(1, 10001))
    rdd = sc.parallelize(data, numSlices=4)
    
    result = rdd.reduce(lambda a, b: a + b)
    print(f"  RDD reduce (sum 1-10000): {result}")
    
    result = rdd.filter(lambda x: x % 2 == 0).count()
    print(f"  Even numbers count: {result}")
    
    from pyspark.sql import functions as F
    
    spark_data = [
        ("A", 100), ("B", 200), ("C", 150),
        ("A", 150), ("B", 250), ("C", 100)
    ]
    df = spark.createDataFrame(spark_data, ["category", "value"])
    
    grouped = df.groupBy("category").agg(
        F.sum("value").alias("total")
    )
    grouped.show()


def core_implementation():
    """Core implementation demonstrating Spark configuration."""
    print("=" * 60)
    print("SPARK CONFIGURATION OPTIMIZATION")
    print("=" * 60)
    
    spark = create_optimized_spark_session("ConfigDemo")
    
    print_configuration(spark, "Initial ")
    
    memory_config = configure_memory_settings()
    print(f"\nMemory Settings: {memory_config}")
    
    cpu_config = configure_cpu_settings()
    print(f"\nCPU Settings: {cpu_config}")
    
    shuffle_config = configure_shuffle_settings()
    print(f"\nShuffle Settings: {shuffle_config}")
    
    aqe_config = configure_adaptive_query_settings()
    print(f"\nAQE Settings: {aqe_config}")
    
    demonstrate_performance_settings(spark)
    
    spark.stop()


def banking_example():
    """Banking/Finance application - Optimized account processing."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Optimized Processing")
    print("=" * 60)
    
    spark = create_optimized_spark_session(
        "BankingOptimized",
        driver_memory="2g",
        executor_memory="1g"
    )
    
    transactions = [
        (f"TXN{i:04d}", f"ACC{i%100:03d}", 
         float(i * 10), "DEBIT" if i % 2 == 0 else "CREDIT")
        for i in range(1, 1001)
    ]
    
    df = spark.createDataFrame(
        transactions,
        ["transaction_id", "account_id", "amount", "type"]
    )
    
    print(f"Input records: {df.count()}")
    
    result = df.groupBy("account_id", "type").agg(
        {"amount": "sum", "transaction_id": "count"}
    )
    print(f"Grouped results: {result.count()}")
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Optimized patient processing."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Optimized Processing")
    print("=" * 60)
    
    spark = create_optimized_spark_session(
        "HealthcareOptimized",
        driver_memory="2g",
        executor_memory="1g"
    )
    
    visits = [
        (f"P{i%500:03d}", f"VISIT{j:04d}", 
         float(i * 50), f"COND{i%10}")
        for i in range(1, 501) for j in range(1, 3)
    ]
    
    df = spark.createDataFrame(
        visits,
        ["patient_id", "visit_id", "cost", "condition"]
    )
    
    print(f"Input visits: {df.count()}")
    
    result = df.groupBy("patient_id").agg(
        {"cost": "sum", "visit_id": "count"}
    )
    print(f"Patient summaries: {result.count()}")
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Spark Configuration Optimization implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()