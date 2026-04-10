# Topic: Spark Installation and Setup
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Spark Installation and Setup

I. INTRODUCTION
This module covers the installation, configuration, and setup of Apache Spark
for big data processing. It includes environment setup, dependency management,
and configuration best practices for production deployments.

II. CORE CONCEPTS
- Spark installation methods (pip, binary, source)
- Environment variables and paths
- Java and Python dependencies
- Spark configuration files
- Local and cluster mode setup

III. IMPLEMENTATION (PySpark code)
"""

import os
import sys
import subprocess
from pathlib import Path
from typing import Dict, Optional, List


def check_java_installation() -> Dict[str, str]:
    """
    Check if Java is installed and get version information.
    
    Java is required for running Spark. This function checks
    the Java installation and returns version details.
    """
    try:
        result = subprocess.run(
            ["java", "-version"],
            capture_output=True,
            text=True,
            check=False
        )
        java_version = result.stderr.strip() if result.stderr else result.stdout
        return {
            "installed": True,
            "version": java_version,
            "status": "Java is installed"
        }
    except FileNotFoundError:
        return {
            "installed": False,
            "version": None,
            "status": "Java is not installed"
        }


def check_python_version() -> Dict[str, str]:
    """
    Check Python version compatibility with Spark.
    
    Spark 3.x requires Python 3.6 or higher.
    """
    version = sys.version_info
    return {
        "version": f"{version.major}.{version.minor}.{version.micro}",
        "major": version.major,
        "minor": version.minor,
        "compatible": version.major == 3 and version.minor >= 6
    }


def setup_spark_environment(spark_home: Optional[str] = None) -> Dict[str, str]:
    """
    Set up Spark environment variables.
    
    Args:
        spark_home: Path to Spark installation directory
        
    Returns:
        Dictionary with environment variable settings
    """
    if spark_home is None:
        spark_home = os.environ.get("SPARK_HOME", "/usr/local/spark")
    
    env_vars = {
        "SPARK_HOME": spark_home,
        "PYSPARK_PYTHON": sys.executable,
        "PYSPARK_DRIVER_PYTHON": sys.executable,
        "HADOOP_HOME": os.environ.get("HADOOP_HOME", ""),
        "JAVA_HOME": os.environ.get("JAVA_HOME", ""),
    }
    
    for key, value in env_vars.items():
        if value:
            os.environ[key] = value
    
    spark_bin = os.path.join(spark_home, "bin")
    if os.path.exists(spark_bin):
        os.environ["PATH"] = spark_bin + os.pathsep + os.environ.get("PATH", "")
    
    return env_vars


def create_spark_session_config() -> Dict[str, str]:
    """
    Create configuration dictionary for Spark session.
    
    Returns a comprehensive configuration dictionary for
    setting up a Spark session with optimized settings.
    """
    config = {
        "spark.app.name": "SparkInstallationDemo",
        "spark.master": "local[*]",
        "spark.driver.memory": "4g",
        "spark.executor.memory": "2g",
        "spark.cores.max": "4",
        "spark.sql.shuffle.partitions": "8",
        "spark.default.parallelism": "4",
        "spark.driver.maxResultSize": "2g",
        "spark.network.timeout": "800s",
        "spark.executor.heartbeatInterval": "30s",
        "spark.sql.adaptive.enabled": "true",
        "spark.sql.adaptive.coalescePartitions.enabled": "true",
    }
    return config


def install_pyspark() -> bool:
    """
    Install PySpark using pip.
    
    Returns:
        True if installation successful, False otherwise
    """
    try:
        result = subprocess.run(
            [sys.executable, "-m", "pip", "install", "pyspark>=3.0.0"],
            capture_output=True,
            text=True
        )
        return result.returncode == 0
    except Exception:
        return False


def verify_pyspark_installation() -> Dict[str, any]:
    """
    Verify PySpark installation and import capabilities.
    """
    try:
        import pyspark
        return {
            "installed": True,
            "version": pyspark.__version__,
            "status": f"PySpark {pyspark.__version__} is installed"
        }
    except ImportError:
        return {
            "installed": False,
            "version": None,
            "status": "PySpark is not installed"
        }


def create_data_directories(base_path: str = "/tmp/spark") -> Dict[str, str]:
    """
    Create necessary directories for Spark operation.
    
    Args:
        base_path: Base directory path for Spark data
        
    Returns:
        Dictionary with created directory paths
    """
    directories = {
        "base": base_path,
        "data": os.path.join(base_path, "data"),
        "checkpoint": os.path.join(base_path, "checkpoint"),
        "logs": os.path.join(base_path, "logs"),
        "temp": os.path.join(base_path, "temp"),
        "warehouse": os.path.join(base_path, "warehouse"),
    }
    
    for dir_path in directories.values():
        Path(dir_path).mkdir(parents=True, exist_ok=True)
    
    return directories


def configure_spark_ui(spark_conf: Dict[str, str]) -> Dict[str, str]:
    """
    Configure Spark UI settings for monitoring.
    
    Args:
        spark_conf: Base Spark configuration
        
    Returns:
        Updated configuration with UI settings
    """
    ui_config = {
        "spark.ui.port": "4040",
        "spark.ui.enabled": "true",
        "spark.ui.killEnabled": "true",
        "spark.ui.retainedStages": "100",
        "spark.ui.retainedJobs": "100",
        "spark.ui.retainedTasks": "1000",
    }
    spark_conf.update(ui_config)
    return spark_conf


def configure_memory_settings(spark_conf: Dict[str, str], 
                              driver_memory: str = "4g",
                              executor_memory: str = "2g") -> Dict[str, str]:
    """
    Configure memory settings for Spark executors.
    
    Args:
        spark_conf: Base Spark configuration
        driver_memory: Driver memory allocation
        executor_memory: Executor memory allocation
        
    Returns:
        Updated configuration with memory settings
    """
    memory_config = {
        "spark.driver.memory": driver_memory,
        "spark.executor.memory": executor_memory,
        "spark.driver.maxResultSize": "2g",
        "spark.memory.fraction": "0.6",
        "spark.memory.storageFraction": "0.5",
    }
    spark_conf.update(memory_config)
    return spark_conf


def configure_shuffle_settings(spark_conf: Dict[str, str]) -> Dict[str, str]:
    """
    Configure shuffle settings for optimal performance.
    
    Args:
        spark_conf: Base Spark configuration
        
    Returns:
        Updated configuration with shuffle settings
    """
    shuffle_config = {
        "spark.sql.shuffle.partitions": "200",
        "spark.shuffle.service.enabled": "false",
        "spark.dynamicAllocation.enabled": "false",
        "spark.shuffle.compress": "true",
    }
    spark_conf.update(shuffle_config)
    return spark_conf


def core_implementation():
    """Core implementation demonstrating Spark installation and setup."""
    print("=" * 60)
    print("SPARK INSTALLATION AND SETUP")
    print("=" * 60)
    
    print("\n--- System Requirements Check ---")
    
    java_info = check_java_installation()
    print(f"Java: {java_info['status']}")
    if java_info.get("version"):
        print(f"  Version: {java_info['version']}")
    
    python_info = check_python_version()
    print(f"Python: {python_info['version']} (Compatible: {python_info['compatible']})")
    
    pyspark_info = verify_pyspark_installation()
    print(f"PySpark: {pyspark_info['status']}")
    if pyspark_info.get("version"):
        print(f"  Version: {pyspark_info['version']}")
    
    print("\n--- Directory Setup ---")
    dirs = create_data_directories()
    for name, path in dirs.items():
        print(f"  {name}: {path}")
    
    print("\n--- Configuration ---")
    config = create_spark_session_config()
    for key, value in config.items():
        print(f"  {key}: {value}")
    
    print("\n--- Environment Variables ---")
    env_vars = setup_spark_environment()
    for key, value in env_vars.items():
        print(f"  {key}: {value}")


def banking_example():
    """Banking/Finance application - Account setup simulation."""
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Account Configuration")
    print("=" * 60)
    
    from pyspark.sql import SparkSession
    
    config = {
        "spark.app.name": "BankingAccountSetup",
        "spark.master": "local[*]",
        "spark.sql.extensions": "io.delta.sql.DeltaSparkSessionExtension",
    }
    
    spark = SparkSession.builder
    for key, value in config.items():
        spark = spark.config(key, value)
    spark = spark.getOrCreate()
    
    print("\nBanking Spark Configuration:")
    sc = spark.sparkContext
    print(f"  App Name: {sc.appName}")
    print(f"  Master: {sc.master}")
    
    sample_accounts = [
        ("ACC001", "John Doe", 10000.00, "SAVINGS"),
        ("ACC002", "Jane Smith", 25000.00, "CHECKING"),
        ("ACC003", "Bob Wilson", 5000.00, "SAVINGS"),
    ]
    
    df = spark.createDataFrame(
        sample_accounts, 
        ["account_id", "name", "balance", "account_type"]
    )
    
    print("\nSample Accounts:")
    df.show()
    
    spark.stop()


def healthcare_example():
    """Healthcare application - Patient system setup."""
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - System Configuration")
    print("=" * 60)
    
    from pyspark.sql import SparkSession
    
    spark = SparkSession.builder \
        .appName("HealthcarePatientSystem") \
        .master("local[*]") \
        .config("spark.sql.adaptive.enabled", "true") \
        .getOrCreate()
    
    print("\nHealthcare System Configuration:")
    conf = spark.sparkContext.getConf()
    print(f"  Application ID: {spark.sparkContext.applicationId}")
    print(f"  Spark Version: {spark.version}")
    
    patient_records = [
        ("P001", "Alice Johnson", "2024-01-15", "Hypertension"),
        ("P002", "Mark Brown", "2024-01-20", "Diabetes"),
        ("P003", "Sarah Davis", "2024-02-01", "Asthma"),
    ]
    
    df = spark.createDataFrame(
        patient_records,
        ["patient_id", "name", "admission_date", "condition"]
    )
    
    print("\nPatient Records:")
    df.show()
    
    spark.stop()


def main():
    """Main execution function."""
    print("Executing Spark Installation and Setup implementation")
    
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except ImportError as e:
        print(f"Import Error: {e}")
        print("Install PySpark: pip install pyspark")
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
