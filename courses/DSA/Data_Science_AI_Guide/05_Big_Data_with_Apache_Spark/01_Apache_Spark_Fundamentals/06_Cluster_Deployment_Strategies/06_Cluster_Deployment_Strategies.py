# Topic: Cluster Deployment Strategies
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Cluster Deployment Strategies

I. INTRODUCTION
This module covers the deployment of Apache Spark applications across different
cluster managers and deployment modes. It includes local mode, standalone cluster,
YARN, Kubernetes, and Mesos deployment strategies with best practices.

II. CORE CONCEPTS
- Local mode deployment
- Standalone cluster deployment
- YARN cluster deployment
- Kubernetes deployment
- Cluster resource management
- Application submission and monitoring

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark import SparkContext
from typing import Dict, Optional, List
import os
import subprocess


def create_spark_session_local(app_name: str = "LocalMode") -> SparkSession:
    """
    Create Spark session in local mode.
    
    Local mode runs Spark on a single machine, useful for
    development and testing.
    """
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .config("spark.driver.memory", "2g") \
        .getOrCreate()


def create_spark_session_standalone(
    app_name: str, 
    master_url: str = "spark://localhost:7077"
) -> SparkSession:
    """
    Create Spark session for standalone cluster.
    
    Args:
        app_name: Application name
        master_url: Standalone cluster master URL
        
    Returns:
        SparkSession configured for standalone cluster
    """
    return SparkSession.builder \
        .appName(app_name) \
        .master(master_url) \
        .config("spark.driver.memory", "2g") \
        .config("spark.executor.memory", "2g") \
        .config("spark.executor.cores", "2") \
        .config("spark.executor.instances", "2") \
        .getOrCreate()


def create_spark_session_yarn(
    app_name: str,
    deploy_mode: str = "cluster"
) -> SparkSession:
    """
    Create Spark session for YARN cluster.
    
    Args:
        app_name: Application name
        deploy_mode: 'cluster' or 'client'
        
    Returns:
        SparkSession configured for YARN
    """
    return SparkSession.builder \
        .appName(app_name) \
        .master("yarn") \
        .config("spark.submit.deployMode", deploy_mode) \
        .config("spark.driver.memory", "2g") \
        .config("spark.executor.memory", "2g") \
        .config("spark.executor.cores", "2") \
        .config("spark.executor.instances", "2") \
        .config("spark.yarn.am.memory", "1g") \
        .config("spark.yarn.max.executor.failures", "3") \
        .getOrCreate()


def create_spark_session_k8s(
    app_name: str,
    k8s_master: str = "k8s://https://kubernetes:6443"
) -> SparkSession:
    """
    Create Spark session for Kubernetes deployment.
    
    Args:
        app_name: Application name
        k8s_master: Kubernetes API server URL
        
    Returns:
        SparkSession configured for Kubernetes
    """
    return SparkSession.builder \
        .appName(app_name) \
        .master(k8s_master) \
        .config("spark.driver.memory", "2g") \
        .config("spark.executor.memory", "2g") \
        .config("spark.executor.cores", "2") \
        .config("spark.executor.instances", "2") \
        .config("spark.kubernetes.container.image", "spark:latest") \
        .config("spark.kubernetes.namespace", "spark") \
        .config("spark.kubernetes.authenticate.driver.serviceAccountName", "spark") \
        .getOrCreate()


def get_cluster_info(spark: SparkSession) -> Dict[str, str]:
    """
    Get cluster information.
    
    Args:
        spark: SparkSession
        
    Returns:
        Dictionary with cluster information
    """
    sc = spark.sparkContext
    
    info = {
        "app_name": sc.appName,
        "master": sc.master,
        "version": sc.version,
        "default_parallelism": str(sc.defaultParallelism),
    }
    
    conf = sc.getConf()
    info["driver_memory"] = conf.get("spark.driver.memory", "N/A")
    info["executor_memory"] = conf.get("spark.executor.memory", "N/A")
    info["executor_cores"] = conf.get("spark.executor.cores", "N/A")
    
    return info


def print_cluster_info(spark: SparkSession, prefix: str = "") -> None:
    """
    Print cluster information.
    
    Args:
        spark: SparkSession
        prefix: Prefix for print output
    """
    info = get_cluster_info(spark)
    print(f"\n{prefix}Cluster Information:")
    for key, value in info.items():
        print(f"  {key}: {value}")


def demonstrate_local_execution(spark: SparkSession) -> None:
    """
    Demonstrate execution in local mode.
    """
    print("\n" + "=" * 50)
    print("LOCAL MODE EXECUTION")
    print("=" * 50)
    
    data = list(range(1, 1001))
    rdd = spark.sparkContext.parallelize(data, numSlices=4)
    
    result = rdd.reduce(lambda a, b: a + b)
    print(f"  Sum of 1-1000: {result}")
    
    filtered = rdd.filter(lambda x: x % 2 == 0).count()
    print(f"  Even numbers: {filtered}")
    
    df = spark.createDataFrame(
        [(i, f"value_{i}") for i in range(1, 101)],
        ["id", "value"]
    )
    print(f"  DataFrame count: {df.count()}")


def demonstrate_cluster_execution(spark: SparkSession) -> None:
    """
    Demonstrate execution in cluster mode.
    """
    print("\n" + "=" * 50)
    print("CLUSTER MODE EXECUTION")
    print("=" * 50)
    
    print_cluster_info(spark, "Cluster ")
    
    data = [(i, i**2, i**3) for i in range(1, 101)]
    df = spark.createDataFrame(data, ["number", "square", "cube"])
    
    print(f"  DataFrame count: {df.count()}")
    
    result = df.select("number", "square").filter(df.number > 50)
    print(f"  Filtered count: {result.count()}")


def configure_executor_settings() -> Dict[str, str]:
    """
    Configure executor settings for cluster deployment.
    
    Returns:
        Dictionary with executor configuration
    """
    executor_config = {
        "spark.executor.memory": "2g",
        "spark.executor.cores": "2",
        "spark.executor.instances": "2",
        "spark.executor.memoryOverhead": "512m",
        "spark.executor.heartbeatInterval": "30s",
    }
    return executor_config


def configure_driver_settings() -> Dict[str, str]:
    """
    Configure driver settings for cluster deployment.
    
    Returns:
        Dictionary with driver configuration
    """
    driver_config = {
        "spark.driver.memory": "2g",
        "spark.driver.cores": "2",
        "spark.driver.maxResultSize": "1g",
    }
    return driver_config


def configure_yarn_specific() -> Dict[str, str]:
    """
    Configure YARN-specific settings.
    
    Returns:
        Dictionary with YARN configuration
    """
    yarn_config = {
        "spark.yarn.queue": "default",
        "spark.yarn.archive": "hdfs:///spark-jars.zip",
        "spark.yarn.dist.files": "",
        "spark.yarn.appMasterEnv.PYSPARK_PYTHON": "python3",
        "spark.yarn.executorEnv.PYSPARK_PYTHON": "python3",
    }
    return yarn_config


def configure_k8s_specific() -> Dict[str, str]:
    """
    Configure Kubernetes-specific settings.
    
    Returns:
        Dictionary with Kubernetes configuration
    """
    k8s_config = {
        "spark.kubernetes.container.image": "spark:3.4.0",
        "spark.kubernetes.namespace": "spark",
        "spark.kubernetes.authenticate.serviceAccountName": "spark",
        "spark.kubernetes.driver.pod.name": "spark-driver",
        "spark.executor.pod.name.template": "executor-{{podId}}",
    }
    return k8s_config


def create_spark_context_config() -> Dict[str, str]:
    """
    Create configuration for SparkContext.
    
    Returns:
        Dictionary with SparkContext configuration
    """
    config = {
        "spark.app.name": "ClusterDeploymentDemo",
        "spark.master": "local[*]",
        "spark.ui.port": "4040",
        "spark.sql.adaptive.enabled": "true",
        "spark.serializer": "org.apache.spark.serializer.KryoSerializer",
    }
    return config


def core_implementation():
    """Core implementation demonstrating cluster deployment."""
    print("=" * 60)
    print("CLUSTER DEPLOYMENT STRATEGIES")
    print("=" * 60)
    
    print("\n--- Local Mode ---")
    spark_local = create_spark_session_local("LocalDemo")
    print_cluster_info(spark_local)
    demonstrate_local_execution(spark_local)
    spark_local.stop()
    
    print("\n--- Executor Configuration ---")
    executor_config = configure_executor_settings()
    for key, value in executor_config.items():
        print(f"  {key}: {value}")
    
    print("\n--- Driver Configuration ---")
    driver_config = configure_driver_settings()
    for key, value in driver_config.items():
        print(f"  {key}: {value}")


def banking_example():
    """Banking/Finance application - Cluster deployment demo."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Cluster Deployment")
    print("=" * 60)
    
    spark = create_spark_session_local("BankingClusterDemo")
    
    print_cluster_info(spark, "Banking ")
    
    accounts = [
        (f"ACC{i:04d}", f"Customer_{i}", float(i * 1000))
        for i in range(1, 101)
    ]
    df = spark.createDataFrame(
        accounts, ["account_id", "name", "balance"]
    )
    
    high_balance = df.filter(df.balance > 50000)
    print(f"  High balance accounts: {high_balance.count()}")
    
    by_range = df.withColumn(
        "balance_range",
        df.balance.cast("int") / 10000
    ).groupBy("balance_range").count()
    print(f"  Balance ranges: {by_range.count()}")
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Cluster deployment demo."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Cluster Deployment")
    print("=" * 60)
    
    spark = create_spark_session_local("HealthcareClusterDemo")
    
    print_cluster_info(spark, "Healthcare ")
    
    patients = [
        (f"P{i:04d}", f"Patient_{i}", f"Condition_{i%10}", float(i * 100))
        for i in range(1, 101)
    ]
    df = spark.createDataFrame(
        patients, ["patient_id", "name", "condition", "cost"]
    )
    
    high_cost = df.filter(df.cost > 5000)
    print(f"  High cost cases: {high_cost.count()}")
    
    by_condition = df.groupBy("condition").agg(
        {"cost": "sum", "patient_id": "count"}
    )
    print(f"  Conditions: {by_condition.count()}")
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Cluster Deployment Strategies implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()