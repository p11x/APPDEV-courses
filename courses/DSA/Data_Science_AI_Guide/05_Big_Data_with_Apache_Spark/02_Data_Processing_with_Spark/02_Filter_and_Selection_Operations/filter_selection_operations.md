# Filter and Selection Operations in Apache Spark

## I. INTRODUCTION

### What are Filter and Selection Operations?
Filter and selection operations allow you to extract subsets of data from DataFrames based on conditions. Filter operations select rows that meet specific criteria, while selection operations choose specific columns from the dataset. These are fundamental operations for data processing, enabling you to focus on relevant records and columns while excluding unwanted data.

### Why is it Important in Big Data?
- Reduces data volume early in processing pipeline
- Improves performance by filtering before expensive operations
- Enables business logic implementation through conditional processing
- Supports data quality filtering and cleaning
- Essential for extracting meaningful insights from large datasets

### Prerequisites
- Basic DataFrame operations knowledge
- Understanding of column expressions
- Familiarity with SQL-like conditions
- Knowledge of data types and schemas

## II. FUNDAMENTALS

### Filter Operations

#### 1. filter() / where()
- Filter rows based on condition
- Accepts SQL-like expressions
- Returns new DataFrame with filtered data
- Can combine multiple conditions with AND/OR

#### 2. isin() 
- Filter by multiple values
- Equivalent to SQL IN clause
- Efficient for checking membership in a list

#### 3. isNull() / isNotNull()
- Handle null values
- Filter missing data or non-null records

#### 4. like() / rlike()
- Pattern matching for strings
- rlike supports regex patterns

### Selection Operations

#### 1. select()
- Choose specific columns
- Supports column expressions and aliases

#### 2. selectExpr()
- SQL expressions in select
- Enables computed columns

#### 3. drop()
- Remove specific columns
- Useful for excluding sensitive data

## III. IMPLEMENTATION

```python
"""
Filter and Selection Operations Demonstration
This example covers all filter and selection operations in PySpark
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType

def filter_operations(spark):
    """Demonstrate filter operations"""
    
    print("=" * 70)
    print("FILTER OPERATIONS")
    print("=" * 70)
    
    # Create sample employee data
    data = [
        ("Alice", 25, "Engineer", 5000),
        ("Bob", 30, "Manager", 8000),
        ("Charlie", 35, "Engineer", 6000),
        ("Diana", 28, "Sales", 4500),
        ("Eve", 32, "Marketing", 5500),
        ("Frank", 45, "Director", 12000),
        ("Grace", 29, "Engineer", 5200),
    ]
    df = spark.createDataFrame(data, ["name", "age", "department", "salary"])
    
    # 1. Basic filter using column reference
    print("\n1. FILTER BY CONDITION:")
    print("-" * 50)
    print("Selecting employees with age greater than 28")
    df.filter(df.age > 28).show()
    
    # 2. Filter with WHERE (SQL style) - equivalent to filter
    print("\n2. FILTER WITH WHERE (SQL STYLE):")
    print("-" * 50)
    print("Filtering with salary greater than 5000 using WHERE clause")
    df.where("salary > 5000").show()
    
    # 3. Multiple filter conditions using AND
    print("\n3. MULTIPLE CONDITIONS (AND):")
    print("-" * 50)
    print("Engineers with salary over 5000")
    df.filter((df.department == "Engineer") & (df.salary > 5000)).show()
    
    # 4. Multiple conditions using OR
    print("\n4. MULTIPLE CONDITIONS (OR):")
    print("-" * 50)
    print("Employees in Engineering or Sales departments")
    df.filter((df.department == "Engineer") | (df.department == "Sales")).show()
    
    # 5. Filter with ISIN for multiple values
    print("\n5. FILTER WITH ISIN:")
    print("-" * 50)
    print("Employees in specific departments (Engineer or Sales)")
    df.filter(df.department.isin(["Engineer", "Sales"])).show()
    
    # 6. Filter NULL values
    print("\n6. FILTER NULL VALUES:")
    print("-" * 50)
    data_with_null = [("A", 25, None, 5000), ("B", None, "Sales", None), ("C", 30, "Engineering", 6000)]
    df_null = spark.createDataFrame(data_with_null, ["name", "age", "dept", "salary"])
    print("Records with non-null salary:")
    df_null.filter(df_null.salary.isNotNull()).show()
    
    # 7. String pattern matching with like
    print("\n7. STRING PATTERN MATCHING:")
    print("-" * 50)
    data_names = [("John",), ("Johnny",), ("Jon",), ("Jane",), ("Johnson",)]
    df_names = spark.createDataFrame(data_names, ["name"])
    print("Names starting with 'Jo':")
    df_names.filter(df_names.name.like("Jo%")).show()
    
    # 8. Complex condition with not
    print("\n8. NEGATION CONDITION:")
    print("-" * 50)
    print("Employees NOT in Engineering department:")
    df.filter(~(df.department == "Engineer")).show()

def selection_operations(spark):
    """Demonstrate selection operations"""
    
    print("\n" + "=" * 70)
    print("SELECTION OPERATIONS")
    print("=" * 70)
    
    # Create sample data
    data = [
        ("Alice", 25, "Engineer", 5000),
        ("Bob", 30, "Manager", 8000),
    ]
    df = spark.createDataFrame(data, ["name", "age", "department", "salary"])
    
    # 1. Select specific columns
    print("\n1. SELECT SPECIFIC COLUMNS:")
    print("-" * 50)
    print("Selecting name and salary columns")
    df.select("name", "salary").show()
    
    # 2. Select with column expressions
    print("\n2. SELECT WITH EXPRESSIONS:")
    print("-" * 50)
    print("Using upper() function on name column with alias")
    df.select(F.upper("name").alias("NAME"), "salary").show()
    
    # 3. Select using selectExpr for SQL-like expressions
    print("\n3. SELECT USING SELECTEXPR:")
    print("-" * 50)
    print("Computing salary increase by 10%")
    df.selectExpr("name", "salary * 1.1 as increased_salary").show()
    
    # 4. Drop columns - remove specific columns
    print("\n4. DROP COLUMNS:")
    print("-" * 50)
    print("Removing age column from output")
    df.drop("age").show()
    
    # 5. Select with column list
    print("\n5. SELECT MULTIPLE COLUMNS:")
    print("-" * 50)
    print("Selecting all columns except age")
    cols_to_select = ["name", "department", "salary"]
    df.select(cols_to_select).show()
    
    # 6. Select with Column object
    print("\n6. SELECT WITH COLUMN OBJECT:")
    print("-" * 50)
    print("Using col() function for column selection")
    df.select(F.col("name"), F.col("salary") * 1.05).show()

def advanced_filter_examples(spark):
    """Demonstrate advanced filter patterns"""
    
    print("\n" + "=" * 70)
    print("ADVANCED FILTER PATTERNS")
    print("=" * 70)
    
    # Create sample data with various data types
    data = [
        ("A", 100, "2024-01-15"),
        ("B", 200, "2024-01-16"),
        ("C", 150, "2024-01-17"),
        ("D", 300, "2024-01-18"),
    ]
    df = spark.createDataFrame(data, ["id", "value", "date"])
    
    # 1. Between filter for range
    print("\n1. BETWEEN FILTER:")
    print("-" * 50)
    print("Values between 100 and 200:")
    df.filter(df.value.between(100, 200)).show()
    
    # 2. Contains filter for strings
    print("\n2. CONTAINS FILTER:")
    print("-" * 50)
    # Note: contains would need actual data
    print("String contains operations")
    
    # 3. Startswith and endswith
    print("\n3. STARTSWITH / ENDSWITH:")
    print("-" * 50)
    dates_df = spark.createDataFrame([("2024-01-01",), ("2024-01-15",), ("2024-02-01",)], ["date_str"])
    print("Dates starting with '2024-01':")
    dates_df.filter(dates_df.date_str.startswith("2024-01")).show()

# Main function
def main():
    """Run all filter and selection demonstrations"""
    spark = SparkSession.builder \
        .appName("FilterSelectDemo") \
        .master("local[*]") \
        .config("spark.sql.shuffle.partitions", "4") \
        .getOrCreate()
    
    spark.sparkContext.setLogLevel("WARN")
    
    filter_operations(spark)
    selection_operations(spark)
    advanced_filter_examples(spark)
    
    print("\n" + "=" * 70)
    print("FILTER AND SELECTION COMPLETE")
    print("=" * 70)
    spark.stop()

if __name__ == "__main__":
    main()
```

## IV. APPLICATIONS

### Real-World Example 1: Banking - Transaction Filtering

```python
"""
Banking - Transaction Filtering and Selection
Demonstrates real-world banking data filtering operations
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, DoubleType, TimestampType

def banking_transaction_filters():
    """Filter banking transactions based on various criteria"""
    
    print("=" * 70)
    print("BANKING TRANSACTION FILTERS")
    print("=" * 70)
    
    # Create Spark session optimized for banking workloads
    spark = SparkSession.builder \
        .appName("BankingFilterDemo") \
        .master("local[*]") \
        .getOrCreate()
    
    # Define transaction schema
    transaction_schema = StructType([
        StructField("transaction_id", StringType(), False),
        StructField("account_id", StringType(), False),
        StructField("transaction_type", StringType(), True),
        StructField("amount", DoubleType(), False),
        StructField("merchant", StringType(), True),
        StructField("status", StringType(), True),
        StructField("timestamp", StringType(), True)
    ])
    
    # Sample transaction data
    transaction_data = [
        ("TXN001", "ACC001", "debit", 5000.00, "Amazon", "completed", "2024-01-15 10:30:00"),
        ("TXN002", "ACC001", "credit", 2500.00, "Payroll", "completed", "2024-01-15 11:00:00"),
        ("TXN003", "ACC002", "debit", 10000.00, "Car Dealer", "pending", "2024-01-15 12:00:00"),
        ("TXN004", "ACC001", "debit", 500.00, "Walmart", "completed", "2024-01-15 13:00:00"),
        ("TXN005", "ACC003", "debit", 15000.00, "Luxury Store", "flagged", "2024-01-15 14:00:00"),
        ("TXN006", "ACC002", "credit", 8000.00, "Salary", "completed", "2024-01-15 15:00:00"),
        ("TXN007", "ACC001", "debit", 200.00, "Gas Station", "completed", "2024-01-15 16:00:00"),
        ("TXN008", "ACC004", "debit", 20000.00, "Unknown", "flagged", "2024-01-15 17:00:00"),
    ]
    
    # Create DataFrame with schema
    df = spark.createDataFrame(transaction_data, transaction_schema)
    
    print("\n1. ALL TRANSACTIONS:")
    print("-" * 50)
    df.show()
    
    # Filter high-value transactions (>$5000)
    print("\n2. HIGH-VALUE TRANSACTIONS (>$5000):")
    print("-" * 50)
    high_value = df.filter(df.amount > 5000)
    print(f"Found {high_value.count()} high-value transactions:")
    high_value.show()
    
    # Filter pending transactions
    print("\n3. PENDING TRANSACTIONS:")
    print("-" * 50)
    pending = df.filter(df.status == "pending")
    pending.show()
    
    # Filter flagged transactions for review
    print("\n4. FLAGGED TRANSACTIONS (POTENTIAL FRAUD):")
    print("-" * 50)
    flagged = df.filter(df.status == "flagged")
    print(f"Found {flagged.count()} flagged transactions requiring review:")
    flagged.show()
    
    # Filter by multiple accounts
    print("\n5. SPECIFIC ACCOUNT TRANSACTIONS (ACC001):")
    print("-" * 50)
    account_transactions = df.filter(df.account_id == "ACC001")
    account_transactions.show()
    
    # Filter debit transactions over threshold
    print("\n6. HIGH-VALUE DEBIT TRANSACTIONS:")
    print("-" * 50)
    high_debit = df.filter(
        (df.transaction_type == "debit") & (df.amount > 1000)
    )
    high_debit.show()
    
    # Select only specific columns for reporting
    print("\n7. TRANSACTION SUMMARY (SELECTED COLUMNS):")
    print("-" * 50)
    summary = df.select("transaction_id", "account_id", "amount", "status")
    summary.show()
    
    spark.stop()
    print("\n" + "=" * 70)

banking_transaction_filters()
```

### Real-World Example 2: Healthcare - Patient Data Filtering

```python
"""
Healthcare - Patient Data Filtering and Selection
Demonstrates healthcare data filtering for patient records
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, DoubleType, DateType

def healthcare_patient_filters():
    """Filter healthcare patient data based on various criteria"""
    
    print("=" * 70)
    print("HEALTHCARE PATIENT FILTERS")
    print("=" * 70)
    
    # Create Spark session
    spark = SparkSession.builder \
        .appName("HealthcareFilterDemo") \
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
        StructField("admission_date", StringType(), True),
        StructField("charges", DoubleType(), True),
        StructField("insurance_provider", StringType(), True)
    ])
    
    # Sample patient data
    patient_data = [
        ("P001", "John Smith", 65, "M", "Cardiology", "Heart Disease", "2024-01-10", 5000.00, "BlueCross"),
        ("P002", "Jane Doe", 32, "F", "General", "Annual Checkup", "2024-01-12", 200.00, "Aetna"),
        ("P003", "Robert Johnson", 58, "M", "Cardiology", "Heart Disease", "2024-01-08", 8000.00, "United"),
        ("P004", "Emily White", 28, "F", "Respiratory", "Asthma", "2024-01-13", 1500.00, "BlueCross"),
        ("P005", "Michael Green", 72, "M", "Orthopedics", "Joint Replacement", "2024-01-05", 15000.00, "Medicare"),
        ("P006", "Sarah Davis", 42, "F", "General", "Flu", "2024-01-14", 300.00, "Aetna"),
        ("P007", "David Wilson", 55, "M", "Cardiology", "Hypertension", "2024-01-11", 3500.00, "United"),
        ("P008", "Lisa Martinez", 38, "F", "Respiratory", "Pneumonia", "2024-01-15", 4500.00, "BlueCross"),
        ("P009", "James Brown", 78, "M", "Orthopedics", "Fracture", "2024-01-09", 7000.00, "Medicare"),
        ("P010", "Jennifer Taylor", 29, "F", "General", "Healthy", "2024-01-16", 150.00, "Aetna"),
    ]
    
    # Create DataFrame
    df = spark.createDataFrame(patient_data, patient_schema)
    
    print("\n1. ALL PATIENTS:")
    print("-" * 50)
    df.show()
    
    # Filter elderly patients (age >= 65)
    print("\n2. ELDERLY PATIENTS (age >= 65):")
    print("-" * 50)
    elderly = df.filter(df.age >= 65)
    print(f"Found {elderly.count()} elderly patients:")
    elderly.select("patient_id", "name", "age", "department").show()
    
    # Filter high-cost patients (charges > $5000)
    print("\n3. HIGH-COST PATIENTS (charges > $5000):")
    print("-" * 50)
    high_cost = df.filter(df.charges > 5000)
    print(f"Found {high_cost.count()} high-cost patients:")
    high_cost.select("patient_id", "name", "department", "charges").show()
    
    # Filter by department
    print("\n4. CARDIOLOGY PATIENTS:")
    print("-" * 50)
    cardiology = df.filter(df.department == "Cardiology")
    cardiology.show()
    
    # Filter by multiple departments
    print("\n5. RESPIRATORY AND ORTHOPEDICS PATIENTS:")
    print("-" * 50)
    specialists = df.filter(df.department.isin(["Respiratory", "Orthopedics"]))
    specialists.show()
    
    # Filter patients by insurance provider
    print("\n6. MEDICARE PATIENTS:")
    print("-" * 50)
    medicare = df.filter(df.insurance_provider == "Medicare")
    medicare.show()
    
    # Complex filter: elderly with high charges
    print("\n7. ELDERLY PATIENTS WITH HIGH CHARGES:")
    print("-" * 50)
    critical = df.filter((df.age >= 60) & (df.charges > 3000))
    print(f"Found {critical.count()} patients requiring attention:")
    critical.show()
    
    # Select specific columns for reporting
    print("\n8. PATIENT SUMMARY (SELECTED COLUMNS):")
    print("-" * 50)
    summary = df.select("patient_id", "name", "age", "department", "diagnosis")
    summary.show()
    
    # Filter null values in charges
    print("\n9. PATIENTS WITH CHARGES DATA:")
    print("-" * 50)
    with_charges = df.filter(df.charges.isNotNull())
    print(f"Patients with charge information: {with_charges.count()}")
    
    spark.stop()
    print("\n" + "=" * 70)

healthcare_patient_filters()
```

## V. OUTPUT_RESULTS

### Expected Output for Filter Operations

```
============================================================
FILTER OPERATIONS
============================================================

1. FILTER BY CONDITION:
----------------------------------------
+-------+---+----------+------+
|   name|age|  department|salary|
+-------+---+----------+------+
|    Bob| 30|    Manager|  8000|
|Charlie| 35|   Engineer|  6000|
|   Diana| 28|      Sales|  4500|
|    Eve| 32|  Marketing|  5500|
|  Frank| 45|   Director| 12000|
|  Grace| 29|   Engineer|  5200|
+-------+---+----------+------+

3. MULTIPLE CONDITIONS (AND):
----------------------------------------
+-------+---+----------+------+
|   name|age|  department|salary|
+-------+---+----------+------+
|Charlie| 35|   Engineer|  6000|
|  Frank| 45|   Director| 12000|
|  Grace| 29|   Engineer|  5200|
+-------+---+----------+------+
```

## VI. VISUALIZATION

### Filter and Selection Flow

```
+------------------------------------------------------------------+
|              FILTER AND SELECTION FLOW                           |
+==================================================================+

INPUT DATAFRAME
    |
    | +--------+ +--------+ +--------+ +--------+
    | | Row 1  | | Row 2  | | Row 3  | | Row 4  |
    | +--------+ +--------+ +--------+ +--------+
    |
    v
+------------------------+
|  FILTER OPERATION     |
|  condition: age > 28  |
+------------------------+
    |
    v
+------------------------+
|  FILTERED DATAFRAME    |
|  (only matching rows) |
+------------------------+
    |
    v
+------------------------+
|  SELECT OPERATION     |
|  columns: name, salary |
+------------------------+
    |
    v
OUTPUT DATAFRAME
+-------------+
| name|salary|
+-------------+

==================================================================
                    COMBINED CONDITIONS
==================================================================

    Condition 1        AND        Condition 2
    (age > 28)      &        (dept == 'Engineer')
         |                   |
         v                   v
    [True/False]           [True/False]
         |                   |
         +--------+--------+
                    |
                    v
              [Result]
            True AND True = True
```

## VII. ADVANCED_TOPICS

### Advanced Filter Operations

#### 1. Using between() for range filters
```python
# Filter values within a range (inclusive)
df.filter(df.age.between(25, 35))
```

#### 2. Using rlike() for regex patterns
```python
# Filter using regular expression
df.filter(df.name.rlike("^J.*"))
```

#### 3. Creating dynamic filters with parameters
```python
# Parameterized filter
threshold = 5000
df.filter(df.salary > threshold)
```

### Performance Tips

1. **Filter Early**: Apply filters before joins to reduce data volume
2. **Predicate Pushdown**: Let Spark push filters to data source when possible
3. **Avoid Functions in Filters**: Use column references instead of UDFs
4. **Use Partition Pruning**: Filter on partitioned columns for better performance

## VIII. CONCLUSION

### Key Takeaways

1. **filter() and where() are equivalent**: Both can be used interchangeably
2. **Multiple conditions**: Use & (AND) and | (OR) operators, or use parentheses for grouping
3. **isin()**: Efficient for checking membership in a list of values
4. **Filter before expensive operations**: Reduces data volume early in the pipeline
5. **Selection methods**: select() chooses columns, drop() removes them

### Next Steps

- Learn about complex filters with multiple conditions
- Explore null handling strategies
- Practice with larger datasets
- Understand partition pruning benefits

### Further Reading

- Spark SQL Programming Guide
- Filter Operations Documentation