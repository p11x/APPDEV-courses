# Join and Aggregation Operations in Apache Spark

## I. INTRODUCTION

### What are Join and Aggregation Operations?
Join operations combine data from multiple DataFrames based on common keys, while aggregation operations summarize data by grouping rows and computing aggregate metrics like sum, count, average, min, max, etc. These are essential operations for data analysis, enabling you to derive insights from combined datasets and calculate summary statistics.

### Why is it Important in Big Data?
- Enables combining disparate data sources from various systems
- Supports business intelligence and reporting requirements
- Essential for data warehouse operations and analytics
- Powers metrics calculation and dashboard generation
- Enables complex data relationships and comparisons

### Prerequisites
- Basic DataFrame operations knowledge
- Understanding of keys and relationships
- Knowledge of aggregate functions
- Familiarity with data types

## II. FUNDAMENTALS

### Join Types

#### 1. Inner Join
Returns only matching rows from both DataFrames based on the join key. Rows without matches are excluded.

#### 2. Outer Joins
- **Left Outer Join**: All rows from left DataFrame + matching rows from right
- **Right Outer Join**: All rows from right DataFrame + matching rows from left  
- **Full Outer Join**: All rows from both DataFrames, with NULLs for non-matches

#### 3. Cross Join
Creates Cartesian product of both DataFrames - every row from one paired with every row from other.

#### 4. Semi Join (Left Semi)
Returns only left DataFrame rows that have matches in right DataFrame.

#### 5. Anti Join (Left Anti)
Returns only left DataFrame rows that do NOT have matches in right DataFrame.

### Aggregation Operations

#### 1. Basic Aggregations
- `count()`: Count number of rows
- `sum()`: Sum of values
- `avg()`: Average of values
- `min()`: Minimum value
- `max()`: Maximum value

#### 2. Grouped Aggregations
- `groupBy()` with aggregation functions
- Multiple grouping columns supported
- Multiple aggregations in single call

#### 3. Multiple Aggregations
- Using `agg()` with dictionary of functions
- Named aggregation outputs

## III. IMPLEMENTATION

```python
"""
Join and Aggregation Operations Demonstration
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType

def join_operations(spark):
    """Demonstrate join operations"""
    
    print("=" * 70)
    print("JOIN OPERATIONS")
    print("=" * 70)
    
    # Create employee DataFrame
    employees = [
        (1, "Alice", 101),
        (2, "Bob", 102),
        (3, "Charlie", 101),
        (4, "Diana", 103),
        (5, "Eve", 104),
    ]
    emp_df = spark.createDataFrame(employees, ["id", "name", "dept_id"])
    
    # Create department DataFrame
    departments = [
        (101, "Engineering"),
        (102, "Sales"),
        (103, "Marketing"),
        (105, "HR"),
    ]
    dept_df = spark.createDataFrame(departments, ["dept_id", "dept_name"])
    
    print("\n1. INNER JOIN:")
    print("-" * 50)
    print("Returns only matching rows from both DataFrames")
    result = emp_df.join(dept_df, "dept_id")
    result.show()
    
    print("\n2. LEFT OUTER JOIN:")
    print("-" * 50)
    print("All employees + department info where available")
    result = emp_df.join(dept_df, "dept_id", "left")
    result.show()
    
    print("\n3. RIGHT OUTER JOIN:")
    print("-" * 50)
    print("All departments + employee info where available")
    result = emp_df.join(dept_df, "dept_id", "right")
    result.show()
    
    print("\n4. FULL OUTER JOIN:")
    print("-" * 50)
    print("All rows from both DataFrames")
    result = emp_df.join(dept_df, "dept_id", "outer")
    result.show()

def aggregation_operations(spark):
    """Demonstrate aggregation operations"""
    
    print("\n" + "=" * 70)
    print("AGGREGATION OPERATIONS")
    print("=" * 70)
    
    data = [
        ("Sales", "North", 100),
        ("Sales", "South", 150),
        ("Marketing", "North", 80),
        ("Marketing", "South", 120),
        ("Engineering", "North", 200),
    ]
    df = spark.createDataFrame(data, ["dept", "region", "sales"])
    
    print("\n1. BASIC GROUP BY:")
    print("-" * 50)
    df.groupBy("dept").sum("sales").show()
    
    print("\n2. MULTIPLE AGGREGATIONS:")
    print("-" * 50)
    df.groupBy("dept").agg(
        F.sum("sales").alias("total_sales"),
        F.avg("sales").alias("avg_sales"),
        F.count("*").alias("count"),
        F.min("sales").alias("min_sales"),
        F.max("sales").alias("max_sales")
    ).show()
    
    print("\n3. MULTIPLE GROUPING COLUMNS:")
    print("-" * 50)
    df.groupBy("dept", "region").sum("sales").show()

def main():
    spark = SparkSession.builder \
        .appName("JoinAggDemo") \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()
    
    spark.sparkContext.setLogLevel("WARN")
    join_operations(spark)
    aggregation_operations(spark)
    
    print("\n" + "=" * 70)
    print("JOIN AND AGGREGATION COMPLETE")
    print("=" * 70)
    spark.stop()

if __name__ == "__main__":
    main()
```

## IV. APPLICATIONS

### Real-World Example 1: Banking

```python
"""
Banking - Transaction and Account Join with Aggregation
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F

def banking_joins_aggs():
    """Join and aggregate banking data"""
    
    print("=" * 70)
    print("BANKING JOIN AND AGGREGATION")
    print("=" * 70)
    
    spark = SparkSession.builder \
        .appName("BankingJoin") \
        .master("local[*]") \
        .getOrCreate()
    
    # Accounts data
    accounts = [
        ("ACC001", "John", "Checking", "Active"),
        ("ACC002", "Jane", "Savings", "Active"),
        ("ACC003", "Bob", "Checking", "Inactive"),
    ]
    acc_df = spark.createDataFrame(accounts, ["account_id", "name", "account_type", "status"])
    
    # Transactions data
    transactions = [
        ("ACC001", "TXN001", 500, "debit"),
        ("ACC001", "TXN002", 300, "debit"),
        ("ACC002", "TXN003", 1000, "credit"),
        ("ACC002", "TXN004", 500, "debit"),
        ("ACC001", "TXN005", 200, "credit"),
    ]
    txn_df = spark.createDataFrame(transactions, ["account_id", "txn_id", "amount", "type"])
    
    print("\n1. JOIN ACCOUNTS AND TRANSACTIONS:")
    print("-" * 50)
    result = acc_df.join(txn_df, "account_id")
    result.show()
    
    print("\n2. AGGREGATE BY ACCOUNT:")
    print("-" * 50)
    result.groupBy("account_id", "name").agg(
        F.sum("amount").alias("total"),
        F.count("*").alias("transactions"),
        F.avg("amount").alias("avg_amount")
    ).show()
    
    print("\n3. BY ACCOUNT TYPE:")
    print("-" * 50)
    result.groupBy("account_type").agg(
        F.sum("amount").alias("total"),
        F.count("*").alias("transactions")
    ).show()
    
    spark.stop()
    print("\n" + "=" * 70)

banking_joins_aggs()
```

### Real-World Example 2: Healthcare

```python
"""
Healthcare - Patient and Visit Join with Aggregation
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F

def healthcare_joins_aggs():
    """Join and aggregate healthcare data"""
    
    print("=" * 70)
    print("HEALTHCARE JOIN AND AGGREGATION")
    print("=" * 70)
    
    spark = SparkSession.builder \
        .appName("HealthcareJoin") \
        .master("local[*]") \
        .getOrCreate()
    
    # Patients data
    patients = [
        ("P001", "John", "Cardiology"),
        ("P002", "Jane", "General"),
        ("P003", "Bob", "Cardiology"),
    ]
    pat_df = spark.createDataFrame(patients, ["patient_id", "name", "dept"])
    
    # Visits data
    visits = [
        ("P001", "V001", 5000),
        ("P001", "V002", 3000),
        ("P002", "V003", 1000),
        ("P003", "V004", 8000),
    ]
    vis_df = spark.createDataFrame(visits, ["patient_id", "visit_id", "charges"])
    
    print("\n1. JOIN PATIENTS AND VISITS:")
    print("-" * 50)
    result = pat_df.join(vis_df, "patient_id")
    result.show()
    
    print("\n2. AGGREGATE BY PATIENT:")
    print("-" * 50)
    result.groupBy("patient_id", "name", "dept").agg(
        F.sum("charges").alias("total_charges"),
        F.count("*").alias("visits"),
        F.avg("charges").alias("avg_charges")
    ).show()
    
    print("\n3. BY DEPARTMENT:")
    print("-" * 50)
    result.groupBy("dept").agg(
        F.count("*").alias("total_visits"),
        F.sum("charges").alias("total_revenue")
    ).show()
    
    spark.stop()
    print("\n" + "=" * 70)

healthcare_joins_aggs()
```

## V. OUTPUT_RESULTS

```
============================================================
JOIN OPERATIONS
============================================================

1. INNER JOIN:
----------------------------------------
+---+-------+------+--------+
| id|   name|dept_id|dept_name|
+---+-------+------+--------+
|  1|  Alice|   101|Engineering|
|  3|Charlie|   101|Engineering|
|  2|    Bob|   102|     Sales|
|  4|  Diana|   103|  Marketing|
+---+-------+------+--------+
```

## VI. CONCLUSION

### Key Takeaways
- Multiple join types serve different purposes
- groupBy() is key to aggregations
- Use broadcast for small tables to avoid shuffle
- Multiple aggregations via agg() method

### Next Steps
- Learn about window functions for advanced analytics
- Practice with complex joins
- Explore optimization techniques