# Window Functions and Advanced Operations in Apache Spark

## I. INTRODUCTION

### What are Window Functions?
Window functions perform calculations across sets of rows that are related to the current row. Unlike aggregate functions that return a single value per group, window functions return a value for each row while still allowing access to rows in the same window partition. This powerful capability enables sophisticated analytics that would otherwise require complex self-joins or multiple passes over the data.

### Why is it Important in Big Data?
Window functions are critical in big data processing for several reasons. They enable sophisticated analytics without requiring grouping all rows into a single output, which is essential when working with massive datasets that cannot fit in memory. These functions support running totals, rankings, moving averages, and other time-series analysis that are fundamental to business intelligence. They power advanced reporting features like top-N per group, period-over-period comparisons, and cumulative metrics that are essential for data-driven decision making.

### Prerequisites
Before learning window functions, you should have a solid understanding of DataFrame operations including select, filter, and withColumn. You need to understand how partitions work in Spark, as window functions operate within partition boundaries. Familiarity with SQL window functions is helpful but not required, as PySpark provides both DataFrame API and SQL syntax for window operations.

## II. FUNDAMENTALS

### Window Function Types

#### 1. Ranking Functions
Ranking functions assign ordinal positions to rows within a partition. The row_number function assigns consecutive integers starting from 1 for each partition. The rank function assigns the same rank to equal values, leaving gaps in the sequence. The dense_rank function assigns the same rank to equal values without gaps. The ntile function distributes rows into a specified number of buckets.

```python
# Ranking function examples in PySpark
from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.window import Window

# Create sample data for ranking demonstration
data = [
    ("2024-01-01", "John", 1500.0),
    ("2024-01-01", "Alice", 1800.0),
    ("2024-01-01", "Bob", 1200.0),
    ("2024-01-02", "John", 1600.0),
    ("2024-01-02", "Alice", 1900.0),
    ("2024-01-02", "Bob", 1300.0),
    ("2024-01-03", "John", 1700.0),
    ("2024-01-03", "Alice", 2000.0),
    ("2024-01-03", "Bob", 1400.0),
]

df = spark.createDataFrame(data, ["date", "name", "sales"])
```

#### 2. Analytic Functions
Analytic functions provide access to values in neighboring rows. The lead function returns values from subsequent rows in the partition. The lag function returns values from previous rows in the partition. The first function returns the first value in the window frame. The last function returns the last value in the window frame. These functions are essential for calculating trends, comparisons, and period-over-period changes.

```python
# Analytic function examples
window_spec = Window.orderBy("date")

# Lag function - get previous day's sales
df.withColumn("prev_sales", F.lag("sales").over(window_spec))

# Lead function - get next day's sales
df.withColumn("next_sales", F.lead("sales").over(window_spec))

# First and Last values
window_spec_desc = Window.orderBy(F.desc("sales"))
df.withColumn("first_sales", F.first("sales").over(window_spec_desc))
df.withColumn("last_sales", F.last("sales").over(window_spec_desc))
```

#### 3. Aggregate Functions in Windows
Aggregate functions can be used with window specifications to perform calculations across row groups while maintaining individual row context. This includes sum for running totals, avg for moving averages, count for running counts, min and max for sliding window minimums and maximums.

```python
# Aggregate functions in windows
window_order = Window.orderBy("date").rowsBetween(Window.unboundedPreceding, Window.currentRow)

# Running total
df.withColumn("running_total", F.sum("sales").over(window_order))

# Moving average (last 3 rows)
window_avg = Window.orderBy("date").rowsBetween(-2, 0)
df.withColumn("moving_avg", F.avg("sales").over(window_avg))
```

### Key Concepts

#### Window Specification
A window specification defines how rows are partitioned and ordered. The partitionBy method creates groups for independent calculations. The orderBy defines the logical row ordering within each partition. The rowsBetween and rangeBetween define the window frame for frame-dependent functions.

```python
# Complete window specification
window_spec = Window.partitionBy("department").orderBy("hire_date").rowsBetween(-1, 1)
```

#### Frame Types
The unboundedPreceding frame includes all rows from the start of the partition. The unboundedFollowing includes all rows to the end of the partition. The currentRow includes only the current row. Negative offsets create trailing windows, positive offsets create leading windows.

#### Partition Concept
Partitions define independent calculation groups. Each partition is processed independently, enabling parallel execution. Window functions respect partition boundaries, resetting calculations at partition boundaries.

## III. IMPLEMENTATION

### Step-by-Step Code Examples with PySpark

```python
"""
Window Functions and Advanced Operations Demonstration
Complete implementation with comprehensive examples
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.window import Window
from pyspark.sql.types import StructType, StructField, StringType, DoubleType, DateType
import datetime

# Initialize Spark Session
spark = SparkSession.builder \
    .appName("WindowFunctionsDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("WINDOW FUNCTIONS AND ADVANCED OPERATIONS")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: RANKING FUNCTIONS
# ============================================================================

print("\n1. RANKING FUNCTIONS DEMONSTRATION")
print("-" * 50)

# Create sales data with multiple employees
sales_data = [
    ("2024-01-01", "Electronics", "John", 1500.0),
    ("2024-01-01", "Electronics", "Alice", 1800.0),
    ("2024-01-01", "Electronics", "Bob", 1200.0),
    ("2024-01-01", "Clothing", "Charlie", 900.0),
    ("2024-01-01", "Clothing", "Diana", 1100.0),
    ("2024-01-01", "Clothing", "Eve", 850.0),
    ("2024-01-02", "Electronics", "John", 1600.0),
    ("2024-01-02", "Electronics", "Alice", 1900.0),
    ("2024-01-02", "Electronics", "Bob", 1300.0),
    ("2024-01-02", "Clothing", "Charlie", 950.0),
    ("2024-01-02", "Clothing", "Diana", 1150.0),
    ("2024-01-02", "Clothing", "Eve", 900.0),
    ("2024-01-03", "Electronics", "John", 1700.0),
    ("2024-01-03", "Electronics", "Alice", 2000.0),
    ("2024-01-03", "Electronics", "Bob", 1400.0),
    ("2024-01-03", "Clothing", "Charlie", 1000.0),
    ("2024-01-03", "Clothing", "Diana", 1200.0),
    ("2024-01-03", "Clothing", "Eve", 950.0),
]

df = spark.createDataFrame(sales_data, ["date", "department", "employee", "sales"])

print("\nOriginal Sales Data:")
df.show()

# Create window specification for ranking within department
dept_window = Window.partitionBy("department").orderBy(F.desc("sales"))

# Apply different ranking functions
ranked_df = df.withColumn("row_number", F.row_number().over(dept_window)) \
    .withColumn("rank", F.rank().over(dept_window)) \
    .withColumn("dense_rank", F.dense_rank().over(dept_window)) \
    .withColumn("ntile", F.ntile(3).over(dept_window))

print("\nRanking Results (within department):")
ranked_df.orderBy("department", "sales").show()

# ============================================================================
# EXAMPLE 2: RUNNING TOTALS AND MOVING AVERAGES
# ============================================================================

print("\n2. RUNNING TOTALS AND MOVING AVERAGES")
print("-" * 50)

# Create time series data
time_series_data = [
    ("2024-01-01", 100.0),
    ("2024-01-02", 150.0),
    ("2024-01-03", 200.0),
    ("2024-01-04", 175.0),
    ("2024-01-05", 225.0),
    ("2024-01-06", 250.0),
    ("2024-01-07", 300.0),
    ("2024-01-08", 275.0),
]

ts_df = spark.createDataFrame(time_series_data, ["date", "value"])
ts_df = ts_df.withColumn("date", F.to_date("date"))

print("\nTime Series Data:")
ts_df.show()

# Window for running total
running_window = Window.orderBy("date").rowsBetween(Window.unboundedPreceding, Window.currentRow)

# Calculate running total
ts_with_running = ts_df.withColumn("running_total", F.sum("value").over(running_window))

print("\nRunning Total Calculation:")
ts_with_running.show()

# Window for moving average (3-day)
moving_avg_window = Window.orderBy("date").rowsBetween(-2, 0)
ts_with_mavg = ts_df.withColumn("moving_avg_3day", F.avg("value").over(moving_avg_window))

print("\n3-Day Moving Average:")
ts_with_mavg.show()

# ============================================================================
# EXAMPLE 3: PERIOD-OVER-PERIOD COMPARISON
# ============================================================================

print("\n3. PERIOD-OVER-PERIOD COMPARISON")
print("-" * 50)

# Create monthly sales data
monthly_data = [
    ("2024-01", 10000.0),
    ("2024-02", 12000.0),
    ("2024-03", 11000.0),
    ("2024-04", 15000.0),
    ("2024-05", 18000.0),
    ("2024-06", 16000.0),
]

monthly_df = spark.createDataFrame(monthly_data, ["month", "sales"])

# Get previous month sales using lag
comparison_df = monthly_df.withColumn("prev_month_sales", F.lag("sales").over(Window.orderBy("month")))

# Calculate month-over-month change
comparison_df = comparison_df.withColumn("mom_change", F.col("sales") - F.col("prev_month_sales")) \
    .withColumn("mom_pct_change", 
               (F.col("sales") - F.col("prev_month_sales")) / F.col("prev_month_sales") * 100)

print("\nMonth-over-Month Comparison:")
comparison_df.show()

# ============================================================================
# EXAMPLE 4: TOP-N PER GROUP
# ============================================================================

print("\n4. TOP-N PER GROUP")
print("-" * 50)

# Window for top-N selection
top_n_window = Window.partitionBy("department").orderBy(F.desc("sales"))

# Get top 2 sales per department
df_with_rank = df.withColumn("sales_rank", F.row_number().over(top_n_window))
top_2 = df_with_rank.filter(F.col("sales_rank") <= 2)

print("\nTop 2 Sales per Department:")
top_2.orderBy("department", "sales_rank").show()

# ============================================================================
# EXAMPLE 5: CUMULATIVE DISTRIBUTION
# ============================================================================

print("\n5. CUMULATIVE DISTRIBUTION")
print("-" * 50)

# Calculate percentile rank
percentile_window = Window.orderBy("sales")
df_with_pct = df.withColumn("pct_rank", F.percent_rank().over(percentile_window))

print("\nPercentile Rank within Department:")
df_with_pct.orderBy("department", "sales").show()

# ============================================================================
# EXAMPLE 6: FIRST AND LAST VALUES
# ============================================================================

print("\n6. FIRST AND LAST VALUES IN WINDOW")
print("-" * 50)

# Window spanning entire partition
full_window = Window.partitionBy("department").orderBy("date").rowsBetween(Window.unboundedPreceding, Window.unboundedFollowing)

# Get first and last values
df_with_first_last = df.withColumn("first_sales", F.first("sales").over(full_window)) \
    .withColumn("last_sales", F.last("sales").over(full_window))

print("\nFirst and Last Sales per Department:")
df_with_first_last.orderBy("department", "date").show()

# ============================================================================
# EXAMPLE 7: MULTIPLE WINDOW SPECIFICATIONS
# ============================================================================

print("\n7. MULTIPLE WINDOW SPECIFICATIONS")
print("-" * 50)

# Apply multiple window functions with different specifications
window_by_dept_date = Window.partitionBy("department").orderBy("date")
window_by_dept = Window.partitionBy("department")
window_by_date = Window.partitionBy("date")

multi_window_df = df.withColumn("running_total_dept_date", 
                                F.sum("sales").over(window_by_dept_date)) \
    .withColumn("total_by_dept", 
                F.sum("sales").over(window_by_dept)) \
    .withColumn("total_by_date", 
                F.sum("sales").over(window_by_date)) \
    .withColumn("avg_by_dept", 
                F.avg("sales").over(window_by_dept)) \
    .withColumn("count_by_dept", 
                F.count("sales").over(window_by_dept))

print("\nMultiple Window Functions:")
multi_window_df.orderBy("department", "date", "employee").show()

# ============================================================================
# EXAMPLE 8: ADVANCED FRAME SPECIFICATIONS
# ============================================================================

print("\n8. ADVANCED FRAME SPECIFICATIONS")
print("-" * 50)

# Range-based window (for numeric/date ranges)
range_window = Window.orderBy("value").rangeBetween(-50, 50)
ts_with_range = ts_df.withColumn("neighbors_sum", F.sum("value").over(range_window))

print("\nRange-Based Window (value +/- 50):")
ts_with_range.show()

# Lead with default value
lead_window = Window.orderBy("date")
ts_with_lead = ts_df.withColumn("lead_2", F.lead("value", 2, 0).over(lead_window))

print("\nLead with Default Value:")
ts_with_lead.show()

spark.stop()
```

### Output Results

```
======================================
WINDOW FUNCTIONS AND ADVANCED OPERATIONS
======================================

1. RANKING FUNCTIONS DEMONSTRATION
--------------------------------------------------

Original Sales Data:
+----------+------------+---------+------+
|      date|  department|employee|sales|
+----------+------------+---------+------+
|2024-01-01|  Electronics|     John|1500.0|
|2024-01-01|  Electronics|    Alice|1800.0|
|2024-01-01|  Electronics|      Bob|1200.0|
|2024-01-01|     Clothing|  Charlie| 900.0|
|2024-01-01|     Clothing|    Diana|1100.0|
|2024-01-01|     Clothing|      Eve| 850.0|
+----------+------------+---------+------+

Ranking Results (within department):
+------------+---------+------+-----------+----+-----------+
|department  |employee |sales|row_number|rank|dense_rank|
+------------+---------+------+-----------+----+-----------+
|Clothing    |Diana    |1100 |1         |1   |1          |
|Clothing    |Charlie  |900  |2         |2   |2          |
|Clothing    |Eve      |850  |3         |3   |3          |
|Electronics |Alice    |1800 |1         |1   |1          |
|Electronics |John     |1500 |2         |2   |2          |
|Electronics |Bob      |1200 |3         |3   |3          |
+------------+---------+------+-----------+----+-----------+

Running Total Calculation:
+----------+------+--------------+
|      date|value |running_total|
+----------+------+--------------+
|2024-01-01|100.0 |100.0         |
|2024-01-02|150.0 |250.0         |
|2024-01-03|200.0 |450.0         |
|2024-01-04|175.0 |625.0         |
|2024-01-05|225.0 |850.0         |
|2024-01-06|250.0 |1100.0        |
|2024-01-07|300.0 |1400.0        |
|2024-01-08|275.0 |1675.0        |
+----------+------+--------------+

Month-over-Month Comparison:
+------+-------+------------------+------------+
|month |sales |prev_month_sales|mom_pct_change|
+------+-------+------------------+------------+
|2024-01|10000 |null             |null         |
|2024-02|12000 |10000            |20.0         |
|2024-03|11000 |12000            |-8.33        |
|2024-04|15000 |11000            |36.36        |
|2024-05|18000 |15000            |20.0         |
|2024-06|16000 |18000            |-11.11      |
+------+-------+------------------+------------+

Top 2 Sales per Department:
+------------+----------+---------+------+-----------+
|department  |date      |employee |sales |sales_rank |
+------------+----------+---------+------+-----------+
|Clothing    |2024-01-02|Diana    |1150  |1          |
|Clothing    |2024-01-03|Diana    |1200  |1          |
|Electronics |2024-01-02|Alice    |1900  |1          |
|Electronics |2024-01-03|Alice    |2000  |1          |
+------------+----------+---------+------+-----------+
```

## IV. APPLICATIONS

### Banking and Financial Services Examples

```python
"""
Window Functions in Banking - Account Analysis
"""

def banking_window_functions_demo(spark):
    """Demonstrate window functions for banking analytics"""
    
    print("\n" + "=" * 70)
    print("BANKING APPLICATION: ACCOUNT TRANSACTION ANALYSIS")
    print("=" * 70)
    
    # Sample banking transactions
    transactions = [
        ("ACC001", "2024-01-01", "DEPOSIT", 1000.0),
        ("ACC001", "2024-01-05", "WITHDRAWAL", 200.0),
        ("ACC001", "2024-01-10", "TRANSFER", 500.0),
        ("ACC001", "2024-01-15", "DEPOSIT", 300.0),
        ("ACC002", "2024-01-01", "DEPOSIT", 5000.0),
        ("ACC002", "2024-01-10", "WITHDRAWAL", 1000.0),
        ("ACC002", "2024-01-20", "TRANSFER", 2000.0),
    ]
    
    df = spark.createDataFrame(transactions, ["account_id", "date", "transaction_type", "amount"])
    df = df.withColumn("date", F.to_date("date"))
    
    # Window specification for account ordering
    account_window = Window.partitionBy("account_id").orderBy("date")
    
    # Calculate running balance
    # Deposit adds, withdrawal/transfer subtracts
    df_with_balance = df.withColumn("transaction_sign", 
                                   F.when(F.col("transaction_type") == "DEPOSIT", 1)
                                   .otherwise(-1)) \
        .withColumn("running_balance", 
                   F.sum(F.col("amount") * F.col("transaction_sign")).over(
                       account_window.rowsBetween(Window.unboundedPreceding, Window.currentRow)))
    
    print("\nRunning Account Balance:")
    df_with_balance.show()
    
    # Calculate days since last transaction
    df_with_lag = df.withColumn("days_since_prev", 
                                 F.datediff(F.col("date"), 
                                           F.lag("date").over(account_window)))
    
    print("\nDays Since Previous Transaction:")
    df_with_lag.show()
    
    # Get cumulative transaction count
    df_with_count = df.withColumn("transaction_number", 
                                 F.row_number().over(account_window))
    
    print("\nTransaction Sequence:")
    df_with_count.show()
    
    # Calculate moving average balance (last 3 transactions)
    mavg_window = account_window.rowsBetween(-2, 0)
    df_with_mavg = df_with_balance.withColumn("avg_balance_3txn", 
                                              F.avg("running_balance").over(mavg_window))
    
    print("\nMoving Average Balance:")
    df_with_mavg.show()
```

### Healthcare Applications

```python
"""
Window Functions in Healthcare - Patient Record Analysis
"""

def healthcare_window_functions_demo(spark):
    """Demonstrate window functions for healthcare analytics"""
    
    print("\n" + "=" * 70)
    print("HEALTHCARE APPLICATION: PATIENT VISIT ANALYSIS")
    print("=" * 70)
    
    # Sample patient visits
    visits = [
        ("PAT001", "2024-01-01", "Annual Checkup", 150.0),
        ("PAT001", "2024-02-15", "Flu Symptoms", 200.0),
        ("PAT001", "2024-03-20", "Follow-up", 100.0),
        ("PAT002", "2024-01-10", "Emergency", 500.0),
        ("PAT002", "2024-02-05", "Follow-up", 150.0),
        ("PAT003", "2024-01-05", "Annual Checkup", 200.0),
        ("PAT003", "2024-03-15", "Specialist", 350.0),
    ]
    
    df = spark.createDataFrame(visits, ["patient_id", "date", "visit_type", "cost"])
    df = df.withColumn("date", F.to_date("date"))
    
    # Calculate running total cost per patient
    patient_window = Window.partitionBy("patient_id").orderBy("date")
    
    df_with_cumulative = df.withColumn("cumulative_cost", 
                                       F.sum("cost").over(
                                           patient_window.rowsBetween(
                                               Window.unboundedPreceding, 
                                               Window.currentRow)))
    
    print("\nPatient Cumulative Costs:")
    df_with_cumulative.show()
    
    # Get previous visit cost for comparison
    df_with_prev = df.withColumn("prev_visit_cost", 
                                 F.lag("cost").over(patient_window))
    
    # Calculate cost change
    df_with_change = df_with_prev.withColumn("cost_change", 
                                              F.col("cost") - F.col("prev_visit_cost"))
    
    print("\nVisit Cost Changes:")
    df_with_change.show()
    
    # Rank visits by cost within patient
    df_with_rank = df.withColumn("cost_rank", 
                                  F.row_number().over(
                                      Window.partitionBy("patient_id")
                                      .orderBy(F.desc("cost"))))
    
    print("\nVisit Cost Rankings:")
    df_with_rank.show()
    
    # Calculate moving average cost (last 2 visits)
    mavg_window = patient_window.rowsBetween(-1, 0)
    df_with_mavg = df_with_cumulative.withColumn("avg_cost_2visits", 
                                               F.avg("cost").over(mavg_window))
    
    print("\nMoving Average Cost:")
    df_with_mavg.show()
```

## V. ASCII FLOW VISUALIZATION

### Window Function Execution Flow

```
+=========================================================================+
|                    WINDOW FUNCTION EXECUTION FLOW                       |
+=========================================================================+

INPUT DATA (Partitioned by Department):
+------------+---------+------+
|department  |employee |sales |
+------------+---------+------+
|Electronics |Alice    |1800  |
|Electronics |John    |1500  |
|Electronics |Bob     |1200  |
+------------+---------+------+
|Clothing   |Diana   |1100  |
|Clothing  |Charlie |900   |
|Clothing  |Eve     |850   |
+------------+---------+------+

STEP 1: PARTITION DATA
+------------+---------+------+------------+
|department  |employee |sales |Partition  |
+------------+---------+------+------------+
|Electronics |Alice    |1800  |     1      |
|Electronics |John    |1500  |     1      |
|Electronics |Bob     |1200  |     1      |
+------------+---------+------+------------+
|Clothing   |Diana   |1100  |     2      |
|Clothing  |Charlie |900   |     2      |
|Clothing  |Eve     |850   |     2      |
+------------+---------+------+------------+

STEP 2: ORDER WITHIN PARTITION (by sales DESC)
+------------+---------+------+------------+---------+
|department  |employee |sales |Partition  |Order    |
+------------+---------+------+------------+---------+
|Electronics |Alice    |1800  |     1     |    1    |
|Electronics |John    |1500  |     1     |    2    |
|Electronics |Bob     |1200  |     1     |    3    |
+------------+---------+------+------------+---------+
|Clothing   |Diana   |1100  |     2     |    1    |
|Clothing  |Charlie |900   |     2     |    2    |
|Clothing  |Eve     |850   |     2     |    3    |
+------------+---------+------+------------+---------+

STEP 3: APPLY WINDOW FUNCTIONS

Ranking Functions:
  row_number()     -> [1, 2, 3, 1, 2, 3]
  rank()          -> [1, 2, 3, 1, 2, 3]
  dense_rank()    -> [1, 2, 3, 1, 2, 3]
  ntile(2)        -> [1, 2, 1, 2, 1, 2]

Analytic Functions:
  lead(sales)     -> [1500, 1200, null, 900, 850, null]
  lag(sales)      -> [null, 1800, 1500, null, 1100, 900]

Aggregate Functions (running total):
  sum(sales)      -> [1800, 3300, 4500, 1100, 2000, 2850]

OUTPUT:
+------------+---------+------+----+--------+--------+--------+---------+
|department |employee|sales| row_   | rank   | lead   | running_|
|           |        |     |number |        |        | total   |
+------------+---------+------+----+--------+--------+---------+
|Electronics |Alice   |1800 |   1   |   1    | 1500   | 1800    |
|Electronics |John    |1500 |   2   |   2    | 1200   | 3300    |
|Electronics |Bob     |1200 |   3   |   3    | null   | 4500    |
+------------+---------+------+----+--------+--------+---------+
|Clothing   |Diana   |1100 |   1   |   1    | 900    | 1100    |
|Clothing  |Charlie |900  |   2   |   2    | 850    | 2000    |
|Clothing  |Eve     |850  |   3   |   3    | null   | 2850    |
+------------+---------+------+----+--------+--------+---------+

+=========================================================================+
|                    FRAME TYPES VISUALIZATION                             |
+=========================================================================+

ROWS BETWEEN (-2, 1) - Current row + 2 preceding + 1 following:
+---------+---------+---------+---------+---------+
|   T1    |   T2    |   T3    |   T4    |   T5    |  <- Time
+---------+---------+---------+---------+---------+    <- 
    [-2]      [-1]      [0]       [+1]      N/A     <- Frame
    (excluded)                         

RANGE BETWEEN (-100, 100):
+---------+---------+---------+---------+---------+
|  100    |  150    |  200    |  175    |  225    |  <- Values
+---------+---------+---------+---------+---------+    <- 
    -100    -50      +0       +25      N/A       <- Range
    range   range    in       out              <- 

+=========================================================================+
|                    TOP-N PER GROUP FLOW                                |
+=========================================================================+

INPUT:
+------------+---------+------+
|department  |employee |sales |
+------------+---------+------+
|Electronics |Alice    |1800  |
|Electronics |John    |1500  |
|Electronics |Bob     |1200  |
+------------+---------+------+
|Clothing   |Diana   |1100  |
|Clothing  |Charlie |900   |
|Clothing  |Eve     |850   |
+------------+---------+------+

PROCESS:
1. Partition by department
2. Order by sales DESC  
3. Assign row_number
4. Filter rank <= N

TOP-2 OUTPUT:
+------------+---------+------+--------+
|department  |employee |sales |rank    |
+------------+---------+------+--------+
|Electronics |Alice    |1800  |   1    |
|Electronics |John    |1500  |   2    |
+------------+---------+------+--------+
|Clothing   |Diana   |1100  |   1    |
|Clothing  |Charlie |900   |   2    |
+------------+---------+------+--------+

+=========================================================================+
```

## VI. ADVANCED TOPICS

### Performance Optimization

Window functions can be expensive on large datasets. Here are optimization techniques:

1. **Partition wisely**: Choose partition keys that balance parallelism with data distribution
2. **Use bounded frames**: Prefer rowsBetween over rangeBetween when possible
3. **Cache intermediate results**: For complex window chains, cache to avoid recomputation
4. **Broadcast small dimension tables**: When joining with reference data

```python
# Performance optimization example
# Bad: Wide partition causing data skew
bad_window = Window.partitionBy("date", "category", "region", "product")

# Good: Narrow partition for parallelism
good_window = Window.partitionBy("date", "product_id")

# Cache intermediate results
df_with_window = df.withColumn("rank", F.row_number().over(window)).cache()
result = df_with_window.filter(F.col("rank") <= 10)
```

### Advanced Window Patterns

1. **Two-pass calculations**: First compute intermediate aggregations, then apply windows
2. **Conditional windows**: Different windows for different row types
3. **Nested windows**: Apply multiple window specifications sequentially
4. **Partition caching**: For iterative calculations across windows

```python
# Advanced pattern: Two-pass calculation
# Pass 1: Compute department statistics
dept_stats = df.groupBy("department") \
    .agg(F.avg("sales").alias("dept_avg"), 
         F.stddev("sales").alias("dept_std"))

# Pass 2: Join and compute relative position
df_with_stats = df.join(dept_stats, "department")
window = Window.partitionBy("department")

# Normalize sales within department
df_normalized = df_with_stats.withColumn("normalized_sales",
    (F.col("sales") - F.col("dept_avg")) / F.col("dept_std"))
```

### Integration with Other Spark Features

Window functions integrate seamlessly with other Spark features:

1. **SQL queries**: Use window functions in SQL expressions
2. **Machine learning**: Use window features as ML input features
3. **Streaming**: Limited support in Structured Streaming
4. **Pandas UDFs**: Combine windows with Pandas UDFs for complex operations

```python
# Integration with ML feature engineering
from pyspark.ml.feature import VectorAssembler

# Create lag features for time series prediction
window = Window.orderBy("timestamp").rowsBetween(-7, 0)
features_df = df.withColumn("lag_1", F.lag("value", 1).over(window)) \
    .withColumn("lag_7", F.lag("value", 7).over(window)) \
    .withColumn("rolling_avg_7", F.avg("value").over(window))

# Assemble feature vector
assembler = VectorAssembler(inputCols=["lag_1", "lag_7", "rolling_avg_7"],
                         outputCol="features")
feature_vector = assembler.transform(features_df)
```

## VII. CONCLUSION

### Key Takeaways

Window functions are powerful tools in Apache Spark that enable sophisticated analytics without requiring expensive data shuffling or self-joins. They provide essential capabilities for calculating running totals, rankings, moving averages, and period-over-period comparisons that are fundamental to business intelligence and analytics. Understanding window specifications, frame types, and partition concepts is crucial for effective use.

### Best Practices

1. Always define clear window specifications with appropriate partitioning
2. Use bounded frames to limit data movement
3. Consider performance implications on large datasets
4. Test with small datasets first to validate logic
5. Monitor partition sizes to avoid data skew

### Next Steps

Continue learning about custom functions and UDFs in the next module to extend Spark's capabilities beyond built-in functions. Then explore performance optimization techniques to scale your window-based analytics to production workloads.

```python
# Quick Reference: Window Function API
from pyspark.sql.window import Window

# Create window specifications
window = Window.partitionBy("col1").orderBy("col2")
window_with_frame = Window.partitionBy("col1").orderBy("col2").rowsBetween(-1, 1)
window_range = Window.partitionBy("col1").orderBy("value").rangeBetween(-10, 10)

# Ranking functions
F.row_number().over(window)
F.rank().over(window)
F.dense_rank().over(window)
F.percent_rank().over(window)
F.ntile(n).over(window)

# Analytic functions  
F.lag("col").over(window)
F.lead("col").over(window)
F.first("col").over(window)
F.last("col").over(window)

# Aggregate functions in windows
F.sum("col").over(window)
F.avg("col").over(window)
F.count("col").over(window)
F.min("col").over(window)
F.max("col").over(window)
```