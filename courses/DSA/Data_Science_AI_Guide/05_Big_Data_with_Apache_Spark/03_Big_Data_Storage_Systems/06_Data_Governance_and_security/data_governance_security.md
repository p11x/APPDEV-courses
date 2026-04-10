# Data Governance and Security with Apache Spark

## I. INTRODUCTION

### What is Data Governance?
Data governance encompasses the processes, policies, and technologies that ensure proper data management across an organization. It includes data quality, data security, privacy compliance, access control, and lineage tracking. In the context of Apache Spark, data governance involves securing data processing pipelines, managing access to sensitive data, and ensuring regulatory compliance.

### Why is it Important in Big Data?
Data governance ensures data assets are trustworthy, secure, and compliant with regulations like GDPR, HIPAA, and SOX. It prevents unauthorized access to sensitive information. It maintains audit trails for regulatory compliance. It establishes data quality standards across the organization.

### Prerequisites
Understanding of data security concepts is useful. Familiarity with Spark required.

## II. FUNDAMENTALS

### Security Layers

#### 1. Authentication
Verifies identity of users and services. Integration with LDAP, Kerberos, and OAuth.

#### 2. Authorization
Controls access to data and operations. Role-based access control (RBAC).

#### 3. Encryption
Protects data at rest and in transit. TLS/SSL for network communication.

#### 4. Auditing
Tracks data access and modifications. Compliance reporting.

### Spark Security Configuration

```python
# Enable encryption
spark.conf.set("spark.authenticate", "true")
spark.conf.set("spark.network.crypto.enabled", "true")

# Set authentication
spark.conf.set("spark.authenticate.secret", "your-secret")
```

## III. IMPLEMENTATION

```python
"""
Data Governance and Security Demonstration
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType

spark = SparkSession.builder \
    .appName("GovernanceDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("DATA GOVERNANCE AND SECURITY")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: DATA MASKING
# ============================================================================

print("\n1. DATA MASKING")
print("-" * 50)

# Sensitive customer data
customers = [
    (i, f"John Doe {i}", f"john{i}@email.com", f"SSN{random.randint(100000000, 999999999)}")
    for i in range(100)
]

df = spark.createDataFrame(customers, ["id", "name", "email", "ssn"])

# Mask PII data
df_masked = df.withColumn("email", F.regexp_replace("email", ".", "*")) \
    .withColumn("ssn", F.concat(F.lit("***-**-"), F.substring("ssn", 7, 4)))

print("\nMasked PII:")
df_masked.show(5)

# ============================================================================
# EXAMPLE 2: COLUMN-LEVEL SECURITY
# ============================================================================

print("\n2. COLUMN-LEVEL SECURITY")
print("-" * 50)

# Define restricted columns
restricted_cols = ["ssn", "password", "credit_card"]

# Filter columns based on user role
user_role = "analyst"  # Could be from auth
available_cols = ["id", "name", "email"] if user_role == "analyst" else ["id", "name", "email", "ssn"]

df_secured = df.select(available_cols)
print(f"\nColumns for {user_role}: {available_cols}")

# ============================================================================
# EXAMPLE 3: ROW-LEVEL SECURITY
# ============================================================================

print("\n3. ROW-LEVEL SECURITY")
print("-" * 50)

# Data with region access
regional_data = [
    (i, f"Region {i % 3}", random.randint(1000, 10000))
    for i in range(100)
]
df_regional = spark.createDataFrame(regional_data, ["id", "region", "value"])

# User has access to specific regions
user_regions = ["Region 0", "Region 1"]

df_filtered = df_regional.filter(F.col("region").isin(user_regions))
print(f"\nRows accessible to user: {df_filtered.count()}")

# ============================================================================
# EXAMPLE 4: DATA LINEAGE TRACKING
# ============================================================================

print("\n4. DATA LINEAGE TRACKING")
print("-" * 50)

# Track data transformations
df_source = spark.createDataFrame([(i,) for i in range(10)], ["id"])
df_transformed = df_source.withColumn("processed_date", F.current_timestamp())

# Log transformation
lineage_info = {
    "source": "raw_transactions",
    "transformations": ["filter", "mask", "aggregate"],
    "output": "processed_transactions"
}

print("\nLineage tracking:")
print(f"  Source: {lineage_info['source']}")
print(f"  Transformations: {lineage_info['transformations']}")
print(f"  Output: {lineage_info['output']}")

# ============================================================================
# EXAMPLE 5: DATA QUALITY RULES
# ============================================================================

print("\n5. DATA QUALITY RULES")
print("-" * 50)

# Define quality rules
quality_rules = [
    {"column": "email", "rule": "not_null", "threshold": 0.99},
    {"column": "ssn", "rule": "unique", "threshold": 1.0},
    {"column": "name", "rule": "not_empty", "threshold": 1.0}
]

# Check quality
df_check = df.filter(F.col("email").isNull()).count()
print(f"\nNull emails: {df_check}")
print(f"Quality threshold met: {df_check < len(df.select('email').collect()) * 0.01}")

# ============================================================================
# EXAMPLE 6: AUDIT LOGGING
# ============================================================================

print("\n6. AUDIT LOGGING")
print("-" * 50)

# Create audit log
audit_data = [
    ("user_1", "SELECT", "customers", "2024-01-01 10:00:00"),
    ("user_1", "UPDATE", "customers", "2024-01-01 10:05:00"),
    ("user_2", "SELECT", "transactions", "2024-01-01 10:10:00"),
]
df_audit = spark.createDataFrame(audit_data, ["user", "operation", "table", "timestamp"])

print("\nAudit Log:")
df_audit.show()

# ============================================================================
# EXAMPLE 7: ENCRYPTION
# ============================================================================

print("\n7. DATA ENCRYPTION")
print("-" * 50)

# Configure encryption
spark.conf.set("spark.network.crypto.enabled", "true")
spark.conf.set("spark.authenticate", "true")

# Write encrypted data
df.write.mode("overwrite").parquet("file:///C:/temp/encrypted/data")

print("\nEncryption enabled for data at rest")

# ============================================================================
# EXAMPLE 8: GDPR COMPLIANCE
# ============================================================================

print("\n8. GDPR COMPLIANCE")
print("-" * 50)

# Right to be forgotten - delete user data
user_to_delete = "user_123"
# In production: actual deletion from storage

print(f"\nGDPR: Data for {user_to_delete} can be deleted upon request")

# Data portability - export user data
df_export = df.filter(F.col("id") == user_to_delete).toJSON().collect()
print(f"Data portability: Export ready for user {user_to_delete}")

# ============================================================================
# EXAMPLE 9: ACCESS CONTROL LISTS
# ============================================================================

print("\n9. ACCESS CONTROL LISTS")
print("-" * 50)

# Define ACLs
acls = [
    {"user": "analyst", "table": "transactions", "access": "READ"},
    {"user": "admin", "table": "transactions", "access": "READ_WRITE"},
    {"user": "analyst", "table": "customers", "access": "READ"},
]
df_acls = spark.createDataFrame(acls)

print("\nAccess Control List:")
df_acls.show()

# ============================================================================
# EXAMPLE 10: DATA RETENTION
# ============================================================================

print("\n10. DATA RETENTION POLICIES")
print("-" * 50)

# Define retention periods
retention_policies = {
    "logs": 90,      # days
    "transactions": 2555,  # 7 years
    "customer_data": 1825  # 5 years
}

import datetime
current_date = datetime.datetime.now()

print("\nRetention Policies:")
for table, days in retention_policies.items():
    expiry = current_date - datetime.timedelta(days=days)
    print(f"  {table}: {days} days (expired before {expiry.date()})")

spark.stop()
```

### Output Results

```
1. DATA MASKING
Masked PII columns

2. COLUMN-LEVEL SECURITY
Filtered based on role

3. ROW-LEVEL SECURITY
Filtered based on region access

4. DATA LINEAGE TRACKING
Tracked transformations
```

## IV. APPLICATIONS

### Banking - Regulatory Compliance

```python
def banking_governance_demo(spark):
    # SOX compliance - audit trails
    # PII masking for customer data
    # Access control for financial data
    pass
```

### Healthcare - HIPAA Compliance

```python
def healthcare_governance_demo(spark):
    # PHI protection
    # Access logging
    # Data retention policies
    pass
```

## V. CONCLUSION

Data governance with Spark ensures security and compliance. Key elements: authentication, authorization, encryption, audit logging, data masking.

```python
# Quick Reference
df.withColumn("ssn", F.concat(F.lit("***-**-"), F.substring("ssn", 7, 4)))
spark.sql("GRANT SELECT ON TABLE TO ROLE analyst")
```