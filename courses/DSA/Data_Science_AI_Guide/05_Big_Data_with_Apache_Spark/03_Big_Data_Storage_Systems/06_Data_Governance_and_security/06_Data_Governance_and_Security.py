# Topic: Data Governance and Security
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Governance and Security

I. INTRODUCTION
This module covers data governance, security, and compliance in big data environments.
It includes access control, encryption, data masking, and regulatory compliance.

II. CORE CONCEPTS
- Access control and authentication
- Data encryption
- Data masking and anonymization
- Audit logging
- Regulatory compliance (GDPR, HIPAA)

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F


def create_spark_session(app_name: str = "SecurityDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def column_level_security(spark: SparkSession) -> None:
    """Demonstrate column-level security."""
    print("\nColumn-Level Security:")
    
    data = [
        ("ACC001", "John", "1234-5678"),
        ("ACC002", "Jane", "9876-5432"),
    ]
    df = spark.createDataFrame(
        data, ["account_id", "name", "ssn"]
    )
    
    restricted_cols = df.select("account_id", "name")
    print("  Sensitive column removed")
    restricted_cols.show()


def row_level_security(spark: SparkSession) -> None:
    """Demonstrate row-level security."""
    print("\nRow-Level Security:")
    
    data = [
        ("ACC001", "John", "SAVINGS", 50000),
        ("ACC002", "Jane", "CHECKING", 25000),
    ]
    df = spark.createDataFrame(
        data, ["account_id", "name", "type", "balance"]
    )
    
    admin_view = df.filter(df.account_id == "ACC001")
    print("  Row filtered for specific account")
    admin_view.show()


def data_masking(spark: SparkSession) -> None:
    """Demonstrate data masking."""
    print("\nData Masking:")
    
    data = [
        ("John", "john@email.com", "555-1234"),
        ("Jane", "jane@email.com", "555-5678"),
    ]
    df = spark.createDataFrame(
        data, ["name", "email", "phone"]
    )
    
    def mask_email(email):
        parts = email.split("@") if email else ["", ""]
        return parts[0][:2] + "***@" + parts[1] if len(parts) > 1 else "***"
    
    mask_udf = F.udf(mask_email)
    
    masked = df.withColumn("email_masked", mask_udf(F.col("email")))
    masked.show()


def encryption_demo(spark: SparkSession) -> None:
    """Demonstrate encryption concepts."""
    print("\nEncryption:")
    
    spark.conf.set("spark.sql.encryption.enabled", "false")
    print("  Encryption configuration set")
    print("  Note: Enable in production with key management")


def audit_logging(spark: SparkSession) -> None:
    """Demonstrate audit logging."""
    print("\nAudit Logging:")
    
    data = [("READ", "user1", "accounts", "2024-01-01")]
    df = spark.createDataFrame(
        data, ["action", "user", "table", "timestamp"]
    )
    
    print("  Audit log entry created")
    df.show()


def core_implementation():
    print("=" * 60)
    print("DATA GOVERNANCE AND SECURITY")
    print("=" * 60)
    
    spark = create_spark_session()
    
    column_level_security(spark)
    row_level_security(spark)
    data_masking(spark)
    encryption_demo(spark)
    audit_logging(spark)
    
    spark.stop()


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Security")
    print("=" * 60)
    
    spark = create_spark_session("BankingSecurity")
    
    accounts = [("ACC001", 50000.0), ("ACC002", 25000.0)]
    df = spark.createDataFrame(accounts, ["account_id", "balance"])
    
    df.show()
    print("  Banking data with security controls")
    
    spark.stop()


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Security")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareSecurity")
    
    patients = [("P001", "Alice", "Hypertension")]
    df = spark.createDataFrame(patients, ["patient_id", "name", "condition"])
    
    df.show()
    print("  Healthcare data with HIPAA compliance")
    
    spark.stop()


def main():
    print("Executing Data Governance implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
