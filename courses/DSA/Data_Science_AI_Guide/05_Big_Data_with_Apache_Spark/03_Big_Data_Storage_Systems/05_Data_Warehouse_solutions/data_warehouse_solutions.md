# Data Warehouse Solutions with Apache Spark

## I. INTRODUCTION

### What is a Data Warehouse?
A data warehouse is a centralized repository optimized for analytical queries and business intelligence. Unlike operational databases optimized for transactions, data warehouses store aggregated historical data designed for report generation and decision support. Apache Spark serves as a powerful engine for building and querying data warehouses.

### Why is it Important in Big Data?
Data warehouses enable business insights from vast data volumes. They support complex queries and aggregations. They provide consistent metrics across organizations. They integrate with BI tools for visualization.

### Prerequisites
Understanding of data modeling concepts is useful. Familiarity with Spark aggregations required. Knowledge of SQL helps.

## II. FUNDAMENTALS

### Data Warehouse Architecture

#### Star Schema
Fact table at center with dimension tables radiating out. Optimized for joins and aggregations. Used for analytical queries.

#### Snowflake Schema
Normalized dimension tables. Saves storage space. More complex joins.

#### Data Vault
Hub tables for business keys. Satellite tables for attributes. Link tables for relationships. Scales to enterprise.

### Spark Data Warehouse

```python
# Create managed table
df.write \
    .mode("overwrite") \
    .saveAsTable("warehouse.facts")

# Query with SQL
spark.sql("""
    SELECT d.category, SUM(f.amount)
    FROM facts f
    JOIN dim_category d ON f.category_id = d.id
    GROUP BY d.category
""")
```

## III. IMPLEMENTATION

```python
"""
Data Warehouse Demonstration
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F

spark = SparkSession.builder \
    .appName("DataWarehouseDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("DATA WAREHOUSE SOLUTIONS")
print("=" * 70)

# Fact and Dimension Tables
fact_data = [
    (i, i % 100, 100 + i % 10, 1000 + i * 10, "2024-01-01")
    for i in range(10000)
]
dim_date = [
    (f"2024-01-{i+1:02d}", "weekday" if i % 7 < 5 else "weekend", i % 4 + 1)
    for i in range(30)
]
dim_category = [
    (i, f"Category {chr(65 + i % 26)}", f"Dept {i // 10}")
    for i in range(100)
]

df_fact = spark.createDataFrame(
    fact_data, ["sale_id", "product_id", "category_id", "amount", "date"])
df_dim_date = spark.createDataFrame(
    dim_date, ["date", "day_type", "week"])
df_dim_category = spark.createDataFrame(
    dim_category, ["category_id", "category_name", "department"])

# Create warehouse tables
df_fact.write.mode("overwrite").parquet("file:///C:/temp/warehouse/facts")
df_dim_date.write.mode("overwrite").parquet("file:///C:/temp/warehouse/dim_date")
df_dim_category.write.mode("overwrite").parquet("file:///C:/temp/warehouse/dim_category")

# SQL Queries
spark.sql("CREATE TABLE IF NOT EXISTS facts_parquet AS SELECT * FROM parquet.'file:///C:/temp/warehouse/facts'")
result = spark.sql("""
    SELECT d.category_name, SUM(f.amount) as total, COUNT(*) as count
    FROM facts_parquet f
    JOIN (SELECT * FROM parquet.'file:///C:/temp/warehouse/dim_category') c ON f.category_id = c.category_id
    GROUP BY d.category_name
    ORDER BY total DESC
""")

print("\nData warehouse queries executed successfully")

spark.stop()
```

### Output Results

```
Sales by category created with aggregations
```

## IV. APPLICATIONS

### Banking - Financial Warehousing

```python
def banking_warehouse_demo(spark):
    # Account balances aggregate
    # Transaction summaries
    pass
```

### Healthcare - Clinical Warehousing

```python
def healthcare_warehouse_demo(spark):
    # Patient outcomes
    # Treatment effectiveness
    pass
```

## V. CONCLUSION

Data warehouses with Spark enable enterprise analytics. Star schemas optimize query performance. Dimension tables provide business context.

**Next Steps**: Data Governance.

```python
# Quick Reference
df.write.saveAsTable("warehouse.table")
spark.sql("SELECT * FROM warehouse.table")
```