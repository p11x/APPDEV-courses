# DataFrame and SQL Basics in Apache Spark

## I. INTRODUCTION

### What is DataFrame and SQL in Spark?
DataFrames are the primary structured data abstraction in Spark, representing distributed collections of rows with named columns. They provide a higher-level API compared to RDDs, with optimized execution through Spark SQL's catalyst optimizer. DataFrames allow users to express data processing logic in a declarative manner similar to pandas or SQL, while maintaining Spark's distributed processing capabilities.

### Why is it Important in Big Data?
DataFrames provide significant advantages for big data processing:
- **Optimized Performance**: Spark SQL catalyst optimizer automatically optimizes execution plans
- **Ease of Use**: Familiar SQL-like operations attract users with SQL backgrounds
- **Schema Awareness**: Built-in schema support eliminates manual type inference
- **Interoperability**: Seamless integration with pandas, R, and various data sources
- **Unified API**: Single API for batch and streaming data processing

### Prerequisites
- Basic understanding of Spark architecture
- Knowledge of Python programming
- Familiarity with SQL queries
- Understanding of data types and schemas

## II. FUNDAMENTALS

### Core DataFrame Concepts

#### 1. Schema
A DataFrame has a defined schema specifying column names and data types. Schemas can be:
- Inferred automatically from data
- Defined explicitly using StructType
- Created from existing data sources

#### 2. Operations Categories
- **Transformation Operations**: Select, filter, transform (lazy evaluation)
- **Action Operations**: Count, collect, show (trigger execution)
- **Aggregation Operations**: Group by, aggregate functions

#### 3. Execution Flow
1. User creates DataFrame
2. Operations build logical plan
3. Catalyst optimizer creates physical plan
4. Tungsten engine executes efficiently

### Key Terminology

- **Column**: Named data field with specific data type
- **Row**: Single record in the DataFrame
- **Partition**: Chunk of data distributed across cluster
- **Transformation**: Operation creating new DataFrame
- **Action**: Operation returning results
- **Catalyst Optimizer**: Query optimization engine

### Core Principles

1. **Schema Evolution**: DataFrames can handle evolving schemas
2. **Optimization**: Automatic query optimization
3. **Type Safety**: Compile-time type checking (Dataset)
4. **Lazy Evaluation**: Execution deferred until action

## III. IMPLEMENTATION

### Step-by-Step Code Examples with PySpark

```python
"""
DataFrame and SQL Basics - Comprehensive Demonstration
This example covers core DataFrame operations and SQL integration
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType, DateType
from pyspark.sql.window import Window

def create_spark_session():
    """Initialize Spark session for DataFrame operations"""
    spark = SparkSession.builder \
        .appName("DataFrameSQLDemo") \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()
    spark.sparkContext.setLogLevel("WARN")
    return spark

def dataframe_creation(spark):
    """Demonstrate different ways to create DataFrames"""
    
    print("=" * 70)
    print("DATAFRAME CREATION METHODS")
    print("=" * 70)
    
    # Method 1: Create from list of dictionaries (Schema inferred)
    print("\n1. CREATE FROM DICTIONARY LIST (Schema Inferred):")
    print("-" * 50)
    
    data = [
        {"name": "Alice", "age": 25, "city": "New York"},
        {"name": "Bob", "age": 30, "city": "Los Angeles"},
        {"name": "Charlie", "age": 35, "city": "Chicago"},
    ]
    df_from_dict = spark.createDataFrame(data)
    print(f"Schema: {df_from_dict.schema}")
    df_from_dict.show()
    
    # Method 2: Create with explicit schema
    print("\n2. CREATE WITH EXPLICIT SCHEMA:")
    print("-" * 50)
    
    schema = StructType([
        StructField("id", IntegerType(), False),
        StructField("name", StringType(), True),
        StructField("salary", DoubleType(), True),
        StructField("department", StringType(), True)
    ])
    
    data_with_schema = [
        (1, "John", 5000.0, "Engineering"),
        (2, "Jane", 6000.0, "Sales"),
        (3, "Mike", 4500.0, "Marketing"),
    ]
    
    df_with_schema = spark.createDataFrame(data_with_schema, schema)
    print(f"Schema: {df_with_schema.schema}")
    df_with_schema.show()
    
    # Method 3: Create using Row objects
    print("\n3. CREATE FROM ROW OBJECTS:")
    print("-" * 50)
    
    from pyspark.sql import Row
    
    rows = [
        Row(name="Alice", age=25, score=95.5),
        Row(name="Bob", age=30, score=88.0),
        Row(name="Charlie", age=28, score=92.0),
    ]
    df_from_rows = spark.createDataFrame(rows)
    df_from_rows.show()

def dataframe_operations(spark):
    """Demonstrate core DataFrame operations"""
    
    print("\n" + "=" * 70)
    print("DATAFRAME CORE OPERATIONS")
    print("=" * 70)
    
    # Create sample DataFrame
    data = [
        ("Alice", 25, "Engineer", 5000),
        ("Bob", 30, "Sales", 4500),
        ("Charlie", 35, "Manager", 8000),
        ("Diana", 28, "Engineer", 5500),
        ("Eve", 32, "Marketing", 4800),
    ]
    columns = ["name", "age", "department", "salary"]
    df = spark.createDataFrame(data, columns)
    
    # 1. SELECT - Choose columns
    print("\n1. SELECT OPERATION:")
    print("-" * 50)
    
    # Select specific columns
    df.select("name", "salary").show()
    
    # Select with expression
    df.select(F.col("name"), F.col("salary") * 1.1).show()
    
    # 2. FILTER - Filter rows
    print("\n2. FILTER OPERATION:")
    print("-" * 50)
    
    # Filter using where
    df.filter(F.col("age") > 28).show()
    
    # Filter with multiple conditions
    df.filter((F.col("department") == "Engineer") & (F.col("salary") > 5000)).show()
    
    # 3. WITH COLUMN - Add or replace column
    print("\n3. WITH COLUMN OPERATION:")
    print("-" * 50)
    
    df_with_new = df.withColumn("salary_bonus", F.col("salary") * 0.1)
    df_with_new.withColumn("total_salary", F.col("salary") + F.col("salary_bonus")).show()
    
    # 4. DROP - Remove columns
    print("\n4. DROP OPERATION:")
    print("-" * 50)
    
    df.select("name", "age", "salary").drop("age").show()
    
    # 5. ORDER BY - Sort data
    print("\n5. ORDER BY OPERATION:")
    print("-" * 50)
    
    df.orderBy(F.desc("salary")).show()
    df.sort(F.asc("age"), F.desc("salary")).show()
    
    # 6. LIMIT - Limit rows
    print("\n6. LIMIT OPERATION:")
    print("-" * 50)
    
    df.limit(3).show()
    
    # 7. DISTINCT - Remove duplicates
    print("\n7. DISTINCT OPERATION:")
    print("-" * 50)
    
    data_with_dups = [
        ("Engineer", 5000),
        ("Sales", 4500),
        ("Engineer", 5000),
        ("Manager", 8000),
        ("Sales", 4500),
    ]
    df_dups = spark.createDataFrame(data_with_dups, ["dept", "salary"])
    print("With duplicates:")
    df_dups.show()
    print("Distinct:")
    df_dups.distinct().show()

def dataframe_aggregation(spark):
    """Demonstrate aggregation operations"""
    
    print("\n" + "=" * 70)
    print("DATAFRAME AGGREGATION OPERATIONS")
    print("=" * 70)
    
    # Create sample DataFrame for aggregation
    data = [
        ("Sales", 1000, "Q1"),
        ("Sales", 1500, "Q2"),
        ("Engineering", 2000, "Q1"),
        ("Engineering", 2500, "Q2"),
        ("Marketing", 800, "Q1"),
        ("Marketing", 1200, "Q2"),
    ]
    df = spark.createDataFrame(data, ["department", "revenue", "quarter"])
    
    print("Original Data:")
    df.show()
    
    # 1. GROUP BY with aggregations
    print("\n1. GROUP BY OPERATION:")
    print("-" * 50)
    
    # Group by department
    dept_agg = df.groupBy("department").agg(
        F.sum("revenue").alias("total_revenue"),
        F.avg("revenue").alias("avg_revenue"),
        F.max("revenue").alias("max_revenue"),
        F.min("revenue").alias("min_revenue"),
        F.count("*").alias("count")
    )
    dept_agg.show()
    
    # 2. Multiple groupings
    print("\n2. MULTIPLE GROUPINGS:")
    print("-" * 50)
    
    multi_group = df.groupBy("department", "quarter").sum("revenue")
    multi_group.show()
    
    # 3. PIVOT operation
    print("\n3. PIVOT OPERATION:")
    print("-" * 50)
    
    pivoted = df.groupBy("department").pivot("quarter").sum("revenue")
    pivoted.show()

def sql_operations(spark):
    """Demonstrate SQL operations on DataFrames"""
    
    print("\n" + "=" * 70)
    print("SQL OPERATIONS ON DATAFRAMES")
    print("=" * 70)
    
    # Create DataFrame
    data = [
        ("Alice", 25, "Engineer", 5000),
        ("Bob", 30, "Sales", 4500),
        ("Charlie", 35, "Manager", 8000),
        ("Diana", 28, "Engineer", 5500),
        ("Eve", 32, "Marketing", 4800),
    ]
    columns = ["name", "age", "department", "salary"]
    df = spark.createDataFrame(data, columns)
    
    # Register as temporary view
    print("\n1. REGISTER TEMPORARY VIEW:")
    print("-" * 50)
    
    df.createOrReplaceTempView("employees")
    print("Created temp view: employees")
    
    # Execute SQL queries
    print("\n2. SQL QUERY EXECUTION:")
    print("-" * 50)
    
    result = spark.sql("""
        SELECT department, COUNT(*) as count, AVG(salary) as avg_salary
        FROM employees
        GROUP BY department
        ORDER BY avg_salary DESC
    """)
    result.show()
    
    # SQL with WHERE clause
    print("\n3. SQL WITH WHERE CLAUSE:")
    print("-" * 50)
    
    result2 = spark.sql("""
        SELECT name, salary
        FROM employees
        WHERE age > 28 AND department = 'Engineer'
    """)
    result2.show()
    
    # SQL with JOIN
    print("\n4. SQL JOIN:")
    print("-" * 50)
    
    # Create second DataFrame
    dept_data = [
        ("Engineer", "Tech"),
        ("Sales", "Business"),
        ("Manager", "Tech"),
        ("Marketing", "Business"),
    ]
    dept_df = spark.createDataFrame(dept_data, ["department", "division"])
    dept_df.createOrReplaceTempView("departments")
    
    join_result = spark.sql("""
        SELECT e.name, e.department, d.division
        FROM employees e
        JOIN departments d ON e.department = d.department
    """)
    join_result.show()
    
    # SQL with subquery
    print("\n5. SQL WITH SUBQUERY:")
    print("-" * 50)
    
    subquery_result = spark.sql("""
        SELECT department, avg_salary
        FROM (
            SELECT department, AVG(salary) as avg_salary
            FROM employees
            GROUP BY department
        )
        WHERE avg_salary > 5000
    """)
    subquery_result.show()

def window_functions(spark):
    """Demonstrate window functions"""
    
    print("\n" + "=" * 70)
    print("WINDOW FUNCTIONS")
    print("=" * 70)
    
    # Create sample data
    data = [
        ("Alice", "Sales", 1000),
        ("Bob", "Sales", 1500),
        ("Charlie", "Sales", 1200),
        ("Diana", "Engineering", 2000),
        ("Eve", "Engineering", 2500),
        ("Frank", "Engineering", 1800),
    ]
    df = spark.createDataFrame(data, ["name", "department", "salary"])
    
    # Define window specification
    print("\n1. ROW NUMBER (Ranking):")
    print("-" * 50)
    
    window_spec = Window.partitionBy("department").orderBy(F.desc("salary"))
    
    df.withColumn("row_number", F.row_number().over(window_spec)).show()
    
    # 2. RANK and DENSE_RANK
    print("\n2. RANK AND DENSE_RANK:")
    print("-" * 50)
    
    df.withColumn("rank", F.rank().over(window_spec)) \
      .withColumn("dense_rank", F.dense_rank().over(window_spec)).show()
    
    # 3. Running total (cumulative sum)
    print("\n3. RUNNING TOTAL (CUMULATIVE SUM):")
    print("-" * 50)
    
    running_total_window = Window.partitionBy("department").orderBy("salary").rowsBetween(Window.unboundedPreceding, Window.currentRow)
    
    df.withColumn("running_total", F.sum("salary").over(running_total_window)).show()
    
    # 4. Lead and Lag
    print("\n4. LEAD AND LAG:")
    print("-" * 50)
    
    df.withColumn("next_salary", F.lead("salary", 1).over(window_spec)) \
      .withColumn("prev_salary", F.lag("salary", 1).over(window_spec)).show()

# Main function to run all demonstrations
def main():
    """Run all DataFrame and SQL demonstrations"""
    spark = create_spark_session()
    
    try:
        dataframe_creation(spark)
        dataframe_operations(spark)
        dataframe_aggregation(spark)
        sql_operations(spark)
        window_functions(spark)
        
        print("\n" + "=" * 70)
        print("ALL DATAFRAME AND SQL DEMONSTRATIONS COMPLETE")
        print("=" * 70)
        
    finally:
        spark.stop()

if __name__ == "__main__":
    main()
```

### Best Practices for DataFrame Operations

1. **Use Column Objects**: Prefer `F.col("name")` over string references
2. **Chain Operations**: Use method chaining for readability
3. **Avoid Collect**: Keep data distributed when possible
4. **Use Broadcast Joins**: For small tables in joins

## IV. APPLICATIONS

### Standard Example: Sales Data Analysis

```python
"""
Standard Sales Data Analysis using DataFrames and SQL
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F

def sales_analysis():
    """Analyze sales data using DataFrame operations"""
    
    spark = SparkSession.builder \
        .appName("SalesAnalysis") \
        .master("local[*]") \
        .getOrCreate()
    
    # Create sample sales data
    sales_data = [
        ("2024-01-01", "ProductA", "Store1", 100, 25.00),
        ("2024-01-01", "ProductB", "Store1", 50, 50.00),
        ("2024-01-01", "ProductA", "Store2", 80, 25.00),
        ("2024-01-02", "ProductA", "Store1", 120, 25.00),
        ("2024-01-02", "ProductB", "Store2", 60, 50.00),
        ("2024-01-02", "ProductC", "Store1", 30, 75.00),
        ("2024-01-03", "ProductA", "Store2", 90, 25.00),
        ("2024-01-03", "ProductC", "Store2", 40, 75.00),
    ]
    columns = ["date", "product", "store", "quantity", "unit_price"]
    df = spark.createDataFrame(sales_data, columns)
    
    # Add total revenue column
    df = df.withColumn("revenue", F.col("quantity") * F.col("unit_price"))
    
    print("=" * 70)
    print("SALES DATA ANALYSIS")
    print("=" * 70)
    
    # Register as temp view for SQL queries
    df.createOrReplaceTempView("sales")
    
    # 1. Total revenue by product
    print("\n1. REVENUE BY PRODUCT:")
    print("-" * 50)
    
    product_revenue = spark.sql("""
        SELECT product, SUM(revenue) as total_revenue, SUM(quantity) as total_quantity
        FROM sales
        GROUP BY product
        ORDER BY total_revenue DESC
    """)
    product_revenue.show()
    
    # 2. Revenue by store
    print("\n2. REVENUE BY STORE:")
    print("-" * 50)
    
    store_revenue = df.groupBy("store").agg(
        F.sum("revenue").alias("total_revenue")
    ).orderBy(F.desc("total_revenue"))
    store_revenue.show()
    
    # 3. Daily revenue
    print("\n3. DAILY REVENUE:")
    print("-" * 50)
    
    daily_revenue = df.groupBy("date").agg(
        F.sum("revenue").alias("daily_revenue"),
        F.count("*").alias("transactions")
    ).orderBy("date")
    daily_revenue.show()
    
    spark.stop()

sales_analysis()
```

### Real-World Example 1: Banking/Finance - Account Analysis

```python
"""
Banking/Finance - Customer Account Analysis using DataFrames
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, DoubleType, DateType

def banking_account_analysis():
    """Analyze banking customer accounts using DataFrames"""
    
    spark = SparkSession.builder \
        .appName("BankingAccountAnalysis") \
        .master("local[*]") \
        .getOrCreate()
    
    # Define schema for account data
    account_schema = StructType([
        StructField("account_id", StringType(), False),
        StructField("customer_name", StringType(), True),
        StructField("account_type", StringType(), True),
        StructField("balance", DoubleType(), True),
        StructField("credit_score", IntegerType(), True),
        StructField("branch", StringType(), True),
        StructField("open_date", DateType(), True)
    ])
    
    # Sample account data
    account_data = [
        ("ACC001", "John Smith", "Checking", 5000.00, 720, "Main", "2020-01-15"),
        ("ACC002", "Jane Doe", "Savings", 15000.00, 750, "Downtown", "2019-06-20"),
        ("ACC003", "Bob Johnson", "Checking", 2500.00, 680, "Main", "2021-03-10"),
        ("ACC004", "Alice Williams", "Savings", 25000.00, 800, "Westside", "2018-11-05"),
        ("ACC005", "Charlie Brown", "Checking", 8000.00, 710, "Downtown", "2020-08-25"),
        ("ACC006", "Diana Martinez", "Checking", 1500.00, 650, "Main", "2022-01-05"),
        ("ACC007", "Edward Lee", "Savings", 12000.00, 730, "Westside", "2019-04-12"),
        ("ACC008", "Fiona Garcia", "Checking", 3500.00, 690, "Downtown", "2021-07-18"),
    ]
    
    df = spark.createDataFrame(account_data, account_schema)
    
    print("=" * 70)
    print("BANKING CUSTOMER ACCOUNT ANALYSIS")
    print("=" * 70)
    
    # Register as temp view
    df.createOrReplaceTempView("accounts")
    
    # 1. Account type distribution
    print("\n1. ACCOUNT TYPE DISTRIBUTION:")
    print("-" * 50)
    
    type_dist = df.groupBy("account_type").agg(
        F.count("account_id").alias("count"),
        F.sum("balance").alias("total_balance"),
        F.avg("balance").alias("avg_balance")
    )
    type_dist.show()
    
    # 2. Branch performance
    print("\n2. BRANCH PERFORMANCE:")
    print("-" * 50)
    
    branch_perf = spark.sql("""
        SELECT branch, 
               COUNT(*) as account_count,
               SUM(balance) as total_deposits,
               AVG(credit_score) as avg_credit_score
        FROM accounts
        GROUP BY branch
        ORDER BY total_deposits DESC
    """)
    branch_perf.show()
    
    # 3. High-value customers (balance > $10000)
    print("\n3. HIGH-VALUE CUSTOMERS (Balance > $10,000):")
    print("-" * 50)
    
    high_value = df.filter(F.col("balance") > 10000).select(
        "account_id", "customer_name", "account_type", "balance"
    ).orderBy(F.desc("balance"))
    high_value.show()
    
    # 4. Credit score analysis
    print("\n4. CREDIT SCORE ANALYSIS:")
    print("-" * 50)
    
    credit_analysis = df.groupBy(
        F.when(F.col("credit_score") >= 750, "Excellent")
         .when(F.col("credit_score") >= 700, "Good")
         .when(F.col("credit_score") >= 650, "Fair")
         .otherwise("Poor").alias("credit_category")
    ).agg(
        F.count("*").alias("count"),
        F.avg("balance").alias("avg_balance")
    )
    credit_analysis.show()
    
    # 5. Window function - Rank customers by balance within branch
    print("\n5. CUSTOMER RANKING BY BRANCH:")
    print("-" * 50)
    
    from pyspark.sql.window import Window
    
    window_spec = Window.partitionBy("branch").orderBy(F.desc("balance"))
    
    ranked = df.withColumn("rank", F.row_number().over(window_spec)).select(
        "branch", "customer_name", "balance", "rank"
    )
    ranked.show()
    
    spark.stop()
    print("\n" + "=" * 70)

banking_account_analysis()
```

### Real-World Example 2: Healthcare - Patient Analytics

```python
"""
Healthcare - Patient Records Analytics using DataFrames
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType, DateType

def healthcare_patient_analytics():
    """Analyze healthcare patient data using DataFrames"""
    
    spark = SparkSession.builder \
        .appName("HealthcareAnalytics") \
        .master("local[*]") \
        .getOrCreate()
    
    # Define patient schema
    patient_schema = StructType([
        StructField("patient_id", StringType(), False),
        StructField("name", StringType(), True),
        StructField("age", IntegerType(), True),
        StructField("gender", StringType(), True),
        StructField("department", StringType(), True),
        StructField("diagnosis", StringType(), True),
        StructField("admission_date", DateType(), True),
        StructField("discharge_date", DateType(), True),
        StructField("total_charges", DoubleType(), True),
        StructField("insurance_provider", StringType(), True)
    ])
    
    # Sample patient data
    patient_data = [
        ("P001", "John Smith", 45, "M", "Cardiology", "Heart Disease", "2024-01-10", "2024-01-15", 5000.00, "BlueCross"),
        ("P002", "Jane Doe", 32, "F", "General", "Flu", "2024-01-12", "2024-01-12", 300.00, "Aetna"),
        ("P003", "Robert Brown", 58, "M", "Cardiology", "Arrhythmia", "2024-01-08", "2024-01-20", 8000.00, "United"),
        ("P004", "Emily White", 28, "F", "Respiratory", "Asthma", "2024-01-13", "2024-01-14", 1500.00, "BlueCross"),
        ("P005", "Michael Green", 65, "M", "Orthopedics", "Hip Replacement", "2024-01-05", "2024-01-18", 15000.00, "Medicare"),
        ("P006", "Sarah Davis", 42, "F", "General", "Appendectomy", "2024-01-14", "2024-01-16", 4500.00, "Aetna"),
        ("P007", "David Wilson", 55, "M", "Cardiology", "Hypertension", "2024-01-11", "2024-01-13", 3500.00, "United"),
        ("P008", "Lisa Martinez", 38, "F", "Respiratory", "Pneumonia", "2024-01-15", "2024-01-18", 5500.00, "BlueCross"),
    ]
    
    df = spark.createDataFrame(patient_data, patient_schema)
    
    # Calculate length of stay
    df = df.withColumn("length_of_stay", 
        F.datediff(F.col("discharge_date"), F.col("admission_date")))
    
    print("=" * 70)
    print("HEALTHCARE PATIENT ANALYTICS")
    print("=" * 70)
    
    # Register as temp view
    df.createOrReplaceTempView("patients")
    
    # 1. Department statistics
    print("\n1. DEPARTMENT STATISTICS:")
    print("-" * 50)
    
    dept_stats = df.groupBy("department").agg(
        F.count("patient_id").alias("patient_count"),
        F.sum("total_charges").alias("total_revenue"),
        F.avg("total_charges").alias("avg_charges"),
        F.avg("length_of_stay").alias("avg_length_of_stay")
    ).orderBy(F.desc("total_revenue"))
    dept_stats.show()
    
    # 2. Diagnosis frequency
    print("\n2. DIAGNOSIS FREQUENCY:")
    print("-" * 50)
    
    diagnosis_freq = spark.sql("""
        SELECT diagnosis, department, COUNT(*) as count
        FROM patients
        GROUP BY diagnosis, department
        ORDER BY count DESC
    """)
    diagnosis_freq.show()
    
    # 3. Insurance provider analysis
    print("\n3. INSURANCE PROVIDER ANALYSIS:")
    print("-" * 50)
    
    insurance_analysis = df.groupBy("insurance_provider").agg(
        F.count("patient_id").alias("patient_count"),
        F.sum("total_charges").alias("total_claims"),
        F.avg("total_charges").alias("avg_claim")
    ).orderBy(F.desc("total_claims"))
    insurance_analysis.show()
    
    # 4. Age group analysis
    print("\n4. AGE GROUP ANALYSIS:")
    print("-" * 50)
    
    age_group_analysis = df.withColumn(
        "age_group",
        F.when(F.col("age") < 30, "Young Adult (0-29)")
         .when((F.col("age") >= 30) & (F.col("age") < 45), "Middle Age (30-44)")
         .when((F.col("age") >= 45) & (F.col("age") < 60), "Senior (45-59)")
         .otherwise("Elderly (60+)")
    ).groupBy("age_group").agg(
        F.count("*").alias("count"),
        F.avg("total_charges").alias("avg_charges")
    ).orderBy("age_group")
    age_group_analysis.show()
    
    # 5. Gender distribution by department
    print("\n5. GENDER DISTRIBUTION BY DEPARTMENT:")
    print("-" * 50)
    
    gender_dept = df.groupBy("department", "gender").count()
    gender_dept.show()
    
    # 6. Window function - Top patients by charges per department
    print("\n6. TOP PATIENTS BY CHARGES PER DEPARTMENT:")
    print("-" * 50)
    
    window_spec = Window.partitionBy("department").orderBy(F.desc("total_charges"))
    
    top_patients = df.withColumn("rank", F.row_number().over(window_spec)).filter(
        F.col("rank") <= 2
    ).select("department", "name", "diagnosis", "total_charges", "rank")
    top_patients.show()
    
    spark.stop()
    print("\n" + "=" * 70)

healthcare_patient_analytics()
```

## V. OUTPUT_RESULTS

### Expected Output for Sales Analysis

```
============================================================
SALES DATA ANALYSIS
============================================================

1. REVENUE BY PRODUCT:
----------------------------------------
+----------+-------------+--------------+
|   product| total_revenue| total_quantity|
+----------+-------------+--------------+
|  ProductA|       9750.0|            390|
|  ProductB|       5500.0|            110|
|  ProductC|       5250.0|             70|
+----------+-------------+--------------+

2. REVENUE BY STORE:
----------------------------------------
+------+-------------+
| store| total_revenue|
+------+-------------+
|Store1|        8500.0|
|Store2|       12000.0|
+------+-------------+
```

### Banking Example Output

```
============================================================
BANKING CUSTOMER ACCOUNT ANALYSIS
============================================================

1. ACCOUNT TYPE DISTRIBUTION:
----------------------------------------
+------------+-----+-------------+------------+
|account_type|count|total_balance|  avg_balance|
+------------+-----+-------------+------------+
|      Savings|    3|    52000.00| 17333.33|
|    Checking|    5|    21000.00|  4200.00|
+------------+-----+-------------+------------+
```

## VI. VISUALIZATION

### DataFrame Execution Flow

```
+------------------------------------------------------------------+
|              DATAFRAME EXECUTION FLOW                            |
+------------------------------------------------------------------+

USER CODE (PySpark)
    |
    v
+------------------------+
|  DataFrame Operations  |
|  - select()            |
|  - filter()            |
|  - groupBy()           |
+------------------------+
    |
    v
+------------------------+
|  Logical Plan (DAG)    |
|  - Unresolved          |
+------------------------+
    |
    v
+------------------------+
|  Catalyst Optimizer    |
|  - Analyzer            |
|  - Optimizer           |
|  - Planner             |
+------------------------+
    |
    v
+------------------------+
|  Physical Plan         |
|  - RDD operations      |
|  - Execution stages   |
+------------------------+
    |
    v
+------------------------+
|  Tungsten Execution    |
|  - Code generation     |
|  - Memory management   |
+------------------------+
    |
    v
+------------------------+
|  Distributed Results   |
+------------------------+

==================================================================
                    CATALYST OPTIMIZER STAGES
==================================================================

1. ANALYSIS
   - Resolve column references
   - Type checking

2. LOGICAL OPTIMIZATION
   - Constant folding
   - Predicate pushdown
   - Projection pruning
   - Boolean expression simplification

3. PHYSICAL PLANNING
   - Join strategy selection
   - Algorithm selection
   - Partition strategy

4. CODE GENERATION
   - JVM bytecode generation
   - Whole-stage codegen
```

## VII. ADVANCED_TOPICS

### Advanced DataFrame Operations

#### 1. Coalesce and Repartition
```python
# Reduce partitions
df.coalesce(2)

# Increase or change partitions
df.repartition(10)
```

#### 2. Union and Intersect
```python
# Combine DataFrames
df1.union(df2)

# Common elements
df1.intersect(df2)
```

#### 3. Pivot and Unpivot
```python
# Pivot for aggregation
df.groupBy("dept").pivot("quarter").sum("sales")

# Unpivot (using stack)
df.selectExpr("name", "stack(2, 'salary', salary, 'bonus', bonus)")
```

### Optimization Techniques

1. **Predicate Pushdown**: Filter early to reduce data movement
2. **Column Pruning**: Select only needed columns
3. **Broadcast Joins**: Use for small tables
4. **Adaptive Query Execution (AQE)**: Enable for runtime optimization

### Common Pitfalls and Solutions

| Issue | Solution |
|-------|----------|
| Slow joins | Use broadcast joins for small tables |
| Out of memory | Increase partitions, reduce collected data |
| Type mismatches | Define explicit schemas |
| Performance issues | Enable AQE, use proper data types |

## VIII. CONCLUSION

### Key Takeaways

1. **DataFrames provide optimized execution** through Catalyst optimizer
2. **SQL integration** enables familiar query patterns
3. **Schema awareness** improves performance and reliability
4. **Window functions** enable advanced analytics

### Next Steps

- Explore Dataset API for type safety
- Learn about Spark MLlib integration
- Practice with large-scale datasets
- Understand DataFrame caching strategies

### Further Reading

- Spark SQL Documentation
- "Spark: The Definitive Guide" by Bill Chambers
- PySpark API Documentation