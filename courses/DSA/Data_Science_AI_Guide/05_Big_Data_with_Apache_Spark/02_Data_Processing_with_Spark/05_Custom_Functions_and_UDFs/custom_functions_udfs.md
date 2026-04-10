# Custom Functions and UDFs in Apache Spark

## I. INTRODUCTION

### What are User Defined Functions?
User Defined Functions (UDFs) in Apache Spark are custom functions that extend the built-in functionality to handle specialized data processing requirements. While Spark provides a rich library of built-in functions for data manipulation, real-world scenarios often require custom logic that cannot be expressed using the standard functions. UDFs allow data scientists and engineers to apply Python, Scala, or Java functions to DataFrame columns, enabling transformations that would otherwise require complex multi-step operations.

### Why is it Important in Big Data?
UDFs are essential in big data processing for several compelling reasons. They provide flexibility to implement domain-specific business logic that is not available in built-in functions, enabling organizations to codify their unique processing requirements. They allow integration with existing Python libraries and ecosystems, making it easy to leverage popular data science tools like NumPy, pandas, and scikit-learn within Spark pipelines. UDFs facilitate complex data validation and cleansing operations that require custom programming logic, which is crucial for data quality management in enterprise environments.

### Prerequisites
Before learning UDFs, you should have a solid understanding of DataFrame operations including creating DataFrames, selecting columns, and applying transformations. Familiarity with Python functions and lambda expressions is essential, as UDFs are essentially Python functions applied to data. Understanding of Spark's lazy evaluation model helps in debugging and optimizing UDF-based transformations.

## II. FUNDAMENTALS

### Types of UDFs in Spark

#### 1. Python UDFs
The most common type of UDF, Python UDFs allow defining custom transformations using Python functions. They provide maximum flexibility but have performance implications due to data serialization between JVM and Python processes.

```python
# Python UDF example
from pyspark.sql.functions import udf
from pyspark.sql.types import StringType

# Simple Python UDF
def categorize_age(age):
    if age < 18:
        return "Minor"
    elif age < 35:
        return "Young Adult"
    elif age < 55:
        return "Middle Age"
    else:
        return "Senior"

# Register UDF
categorize_age_udf = udf(categorize_age, StringType())

# Apply to DataFrame
df.withColumn("age_category", categorize_age_udf("age"))
```

#### 2. Scalar UDFs vs Array-Returning UDFs
Scalar UDFs return a single value for each input row, which is the most common use case. Array-returning UDFs can return multiple values as an array, useful for complex transformations that produce multiple outputs.

```python
# Scalar UDF returning single value
def calculate_bonus(salary, rating):
    return salary * (rating / 100)

calculate_bonus_udf = udf(calculate_bonus, DoubleType())

# Array-returning UDF
from pyspark.sql.types import ArrayType, IntegerType

def get_digit_array(number):
    return [int(d) for d in str(number)]

get_digit_array_udf = udf(get_digit_array, ArrayType(IntegerType()))
```

#### 3. Pandas UDFs (Vectorized UDFs)
Pandas UDFs, introduced in Spark 3.0, use Apache Arrow for efficient data transfer between JVM and Python, providing significant performance improvements over traditional Python UDFs. They operate on pandas Series/DataFrames rather than individual rows.

```python
# Pandas UDF example
from pyspark.sql.functions import pandas_udf
import pandas as pd

@pandas_udf(DoubleType())
def calculate_bonus_pandas(salary: pd.Series, rating: pd.Series) -> pd.Series:
    return salary * (rating / 100)
```

### Key Concepts

#### Registration and Invocation
UDFs must be registered with Spark before use. Registration associates the Python function with a name that can be used in SQL queries. After registration, UDFs can be invoked like built-in functions.

#### Return Type Specification
All UDFs must have a defined return type that tells Spark how to interpret the output. Common types include StringType, IntegerType, DoubleType, BooleanType, and complex types like ArrayType and StructType.

#### Null Handling
UDFs must handle null values explicitly. If null handling is not implemented, UDFs will return null for null inputs, which may not be the desired behavior.

## III. IMPLEMENTATION

### Step-by-Step Code Examples with PySpark

```python
"""
Custom Functions and UDFs Demonstration
Complete implementation with comprehensive examples
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.sql.types import (
    StructType, StructField, StringType, IntegerType, 
    DoubleType, BooleanType, ArrayType, MapType
)
import datetime
import re

# Initialize Spark Session
spark = SparkSession.builder \
    .appName("UDFsDemo") \
    .master("local[*]") \
    .config("spark.sql.execution.arrow.pyspark.enabled", "true") \
    .getOrCreate()

print("=" * 70)
print("CUSTOM FUNCTIONS AND UDFs")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: BASIC STRING UDFs
# ============================================================================

print("\n1. BASIC STRING UDFs")
print("-" * 50)

# Sample employee data
employee_data = [
    ("John Smith", 28, "john.smith@company.com", 5000.0, 3.5),
    ("Alice Johnson", 34, "alice.j@company.com", 7500.0, 4.2),
    ("Bob Williams", 45, "bob.w@company.com", 9000.0, 4.8),
    ("Charlie Brown", 23, "charlie.b@company.com", 3500.0, 2.8),
    ("Diana Martinez", 38, "diana.m@company.com", 8500.0, 4.0),
]

df = spark.createDataFrame(
    employee_data, 
    ["name", "age", "email", "salary", "rating"]
)

print("\nOriginal Employee Data:")
df.show()

# Define string processing UDFs
def extract_domain(email):
    """Extract domain from email address"""
    if email:
        return email.split("@")[1] if "@" in email else "unknown"
    return "unknown"

def get_initials(name):
    """Get name initials"""
    if name:
        parts = name.split()
        return "".join([p[0] for p in parts if p]).upper()
    return "UNKNOWN"

def sanitize_phone(phone):
    """Sanitize phone number format"""
    if phone:
        digits = re.sub(r"\D", "", phone)
        if len(digits) == 10:
            return f"({digits[:3]}) {digits[3:6]}-{digits[6:]}"
        return digits
    return "INVALID"

# Register UDFs
extract_domain_udf = F.udf(extract_domain, StringType())
get_initials_udf = F.udf(get_initials, StringType())

# Apply UDFs
df_with_udf = df.withColumn("domain", extract_domain_udf("email")) \
    .withColumn("initials", get_initials_udf("name"))

print("\nAfter String UDFs:")
df_with_udf.show()

# ============================================================================
# EXAMPLE 2: NUMERIC UDFs
# ============================================================================

print("\n2. NUMERIC UDFs")
print("-" * 50)

# Define numeric UDFs
def calculate_tax(income, tax_rate=0.25):
    """Calculate income tax"""
    if income and income > 0:
        return income * tax_rate
    return 0.0

def calculate_bonus(salary, rating, max_bonus=0.20):
    """Calculate performance bonus"""
    if salary and rating:
        bonus_rate = min(rating / 5.0, 1.0) * max_bonus
        return salary * bonus_rate
    return 0.0

def calculate_net_income(income, deductions):
    """Calculate net income after deductions"""
    if income and deductions:
        return max(income - deductions, 0)
    return income if income else 0

# Register UDFs with explicit return types
calculate_tax_udf = F.udf(calculate_tax, DoubleType())
calculate_bonus_udf = F.udf(calculate_bonus, DoubleType())

# Apply numeric UDFs
df_with_numeric = df.withColumn("tax", calculate_tax_udf("salary")) \
    .withColumn("bonus", calculate_bonus_udf(F.col("salary"), F.col("rating"))) \
    .withColumn("total_comp", F.col("salary") + F.col("bonus")) \
    .withColumn("net_income", F.col("salary") + F.col("bonus") - F.col("tax"))

print("\nAfter Numeric UDFs:")
df_with_numeric.show()

# ============================================================================
# EXAMPLE 3: LOGICAL UDFs
# ============================================================================

print("\n3. LOGICAL UDFs")
print("-" * 50)

# Define logical UDFs
def is_eligible_for_promotion(age, rating, years_experience):
    """Determine promotion eligibility"""
    if age and rating and years_experience:
        return age < 50 and rating >= 3.5 and years_experience >= 2
    return False

def classify_performance(rating):
    """Classify performance rating"""
    if rating >= 4.5:
        return "Exceptional"
    elif rating >= 4.0:
        return "Excellent"
    elif rating >= 3.0:
        return "Good"
    elif rating >= 2.0:
        return "Needs Improvement"
    else:
        return "Unsatisfactory"

def calculate_severity(age, conditions):
    """Calculate health risk severity"""
    if age and conditions:
        base_score = 0
        if age > 50:
            base_score += 2
        elif age > 35:
            base_score += 1
        base_score += len(conditions) if isinstance(conditions, list) else 0
        return "High" if base_score >= 3 else "Medium" if base_score >= 1 else "Low"
    return "Unknown"

# Register logical UDFs
is_eligible_udf = F.udf(is_eligible_for_promotion, BooleanType())
classify_performance_udf = F.udf(classify_performance, StringType())

# Apply logical UDFs
df_with_logical = df.withColumn("performance_level", classify_performance_udf("rating"))
# Note: is_eligible requires years_experience which we don't have in our data

print("\nAfter Logical UDFs:")
df_with_logical.show()

# ============================================================================
# EXAMPLE 4: COMPLEX TYPE UDFs
# ============================================================================

print("\n4. COMPLEX TYPE UDFs")
print("-" * 50)

# Define complex type UDFs
def parse_address(address_str):
    """Parse address string into components"""
    if address_str:
        parts = address_str.split(",")
        return {
            "street": parts[0].strip() if len(parts) > 0 else "",
            "city": parts[1].strip() if len(parts) > 1 else "",
            "state": parts[2].strip() if len(parts) > 2 else ""
        }
    return {"street": "", "city": "", "state": ""}

def extract_keywords(text, min_length=4):
    """Extract keywords from text"""
    if text:
        words = re.findall(r'\w+', text.lower())
        return [w for w in words if len(w) >= min_length]
    return []

def process_contact_info(email, phone):
    """Process and normalize contact information"""
    result = {"email": None, "phone": None}
    if email and "@" in email:
        result["email"] = email.lower().strip()
    if phone:
        digits = re.sub(r"\D", "", phone)
        if len(digits) == 10:
            result["phone"] = digits
    return result

# Register UDF with MapType
def parse_address_to_map(address_str):
    if address_str:
        parts = address_str.split(",")
        return {
            "street": parts[0].strip() if len(parts) > 0 else "",
            "city": parts[1].strip() if len(parts) > 1 else "",
            "state": parts[2].strip() if len(parts) > 2 else ""
        }
    return {"street": "", "city": "", "state": ""}

map_type = MapType(StringType(), StringType())
parse_address_udf = F.udf(parse_address_to_map, map_type)

# Test data with addresses
address_data = [
    ("John Smith", 28, "123 Main St, New York, NY"),
    ("Alice Johnson", 34, "456 Oak Ave, Los Angeles, CA"),
    ("Bob Williams", 45, "789 Pine Rd, Chicago, IL"),
]

df_address = spark.createDataFrame(address_data, ["name", "age", "address"])

print("\nData with Addresses:")
df_address.show()

print("\nAfter Map UDF:")
df_address.withColumn("address_parts", parse_address_udf("address")).show()

# ============================================================================
# EXAMPLE 5: ARRAY-RETURNING UDFs
# ============================================================================

print("\n5. ARRAY-RETURNING UDFs")
print("-" * 50)

# Define array-returning UDFs
def get_monthly_targets(annual_target):
    """Split annual target into monthly targets"""
    if annual_target and annual_target > 0:
        monthly = annual_target / 12
        return [round(monthly, 2) for _ in range(12)]
    return [0.0] * 12

def calculate_running_totals(values):
    """Calculate running totals from list"""
    if values and isinstance(values, list):
        totals = []
        running = 0
        for v in values:
            running += v
            totals.append(round(running, 2))
        return totals
    return []

def expand_product_codes(code):
    """Expand product code into categories"""
    if code:
        return [code[:2], code[2:4], code[4:]]
    return ["", "", ""]

# Register array-returning UDFs
array_type = ArrayType(DoubleType())
get_monthly_targets_udf = F.udf(get_monthly_targets, array_type)

# Test with sales data
sales_data = [
    ("Product A", 12000.0),
    ("Product B", 24000.0),
    ("Product C", 36000.0),
]

df_sales = spark.createDataFrame(sales_data, ["product", "annual_target"])

print("\nSales Data:")
df_sales.show()

print("\nMonthly Targets (Array UDF):")
df_sales.withColumn("monthly_targets", get_monthly_targets_udf("annual_target")).show()

# ============================================================================
# EXAMPLE 6: NULL HANDLING IN UDFs
# ============================================================================

print("\n6. NULL HANDLING IN UDFs")
print("-" * 50)

# Data with nulls
data_with_nulls = [
    ("John", 28, 5000.0),
    ("Alice", None, 7500.0),
    ("Bob", 45, None),
    (None, 23, 3500.0),
    ("Diana", 38, None),
]

df_nulls = spark.createDataFrame(data_with_nulls, ["name", "age", "salary"])

print("\nData with Nulls:")
df_nulls.show()

# UDF with null handling
def safe_divide(numerator, denominator, default=0.0):
    """Safely divide with null handling"""
    if numerator is None or denominator is None:
        return default
    try:
        return numerator / denominator if denominator != 0 else default
    except:
        return default

def handle_name(name, default="UNKNOWN"):
    """Handle null names"""
    return name if name else default

def process_salary(salary, bonus_pct=0.10):
    """Process salary with null handling"""
    if salary is None:
        return None
    return salary * (1 + bonus_pct)

# Register null-handling UDFs
safe_divide_udf = F.udf(safe_divide, DoubleType())
handle_name_udf = F.udf(handle_name, StringType())
process_salary_udf = F.udf(process_salary, DoubleType())

# Apply with explicit null handling using coalesce
df_nulls_processed = df_nulls.withColumn("safe_name", handle_name_udf("name")) \
    .withColumn("bonus", F.coalesce(process_salary_udf("salary"), F.lit(0.0)))

print("\nAfter Null Handling UDFs:")
df_nulls_processed.show()

# ============================================================================
# EXAMPLE 7: CHAINING UDFs
# ============================================================================

print("\n7. CHAINING UDFs")
print("-" * 50)

# Define helper UDFs for chaining
def normalize_email(email):
    """Normalize email address"""
    if email:
        return email.lower().strip()
    return ""

def mask_sensitive(data, visible_chars=4):
    """Mask sensitive information"""
    if data and len(data) > visible_chars:
        return data[:visible_chars] + "*" * (len(data) - visible_chars)
    return "****"

def validate_and_format(phone):
    """Validate and format phone number"""
    if phone:
        digits = re.sub(r"\D", "", phone)
        if len(digits) == 10:
            return f"+1-{digits[:3]}-{digits[3:6]}-{digits[6:]}"
    return "INVALID"

# Register UDFs
normalize_email_udf = F.udf(normalize_email, StringType())
mask_sensitive_udf = F.udf(mask_sensitive, StringType())

# Chain UDFs
df_chained = df.withColumn(
    "normalized_email", 
    normalize_email_udf("email")
).withColumn(
    "masked_email", 
    mask_sensitive_udf("normalized_email")
)

print("\nChained UDF Results:")
df_chained.select("email", "normalized_email", "masked_email").show()

# ============================================================================
# EXAMPLE 8: CONDITIONAL UDFs
# ============================================================================

print("\n8. CONDITIONAL UDFs")
print("-" * 50)

# Define conditional UDFs
def calculate_tier(salary):
    """Calculate employee tier based on salary"""
    if salary is None:
        return "Unknown"
    elif salary < 5000:
        return "Bronze"
    elif salary < 8000:
        return "Silver"
    elif salary < 12000:
        return "Gold"
    else:
        return "Platinum"

def determine_benefits(tier, years_employed):
    """Determine benefits package"""
    if tier == "Platinum":
        return ["Health Insurance", "401k Match", "Stock Options", "Unlimited PTO"]
    elif tier == "Gold":
        return ["Health Insurance", "401k Match", "Limited PTO"]
    elif tier == "Silver":
        return ["Health Insurance", "401k"]
    else:
        return ["Basic Health"]

def apply_discount(category, price, discount_rate):
    """Apply category-specific discount"""
    category_discounts = {"electronics": 0.15, "clothing": 0.25, "furniture": 0.10}
    rate = category_discounts.get(category.lower(), discount_rate)
    return price * (1 - rate)

# Register conditional UDFs
calculate_tier_udf = F.udf(calculate_tier, StringType())

# Apply conditional UDF
df_tier = df.withColumn("tier", calculate_tier_udf("salary"))

print("\nEmployee Tiers:")
df_tier.select("name", "salary", "tier").show()

# ============================================================================
# EXAMPLE 9: UDFs WITH COMPLEX LOGIC
# ============================================================================

print("\n9. UDFs WITH COMPLEX LOGIC")
print("-" * 50)

# Complex logic UDFs
def calculate_risk_score(age, income, debt, assets):
    """Calculate financial risk score"""
    if None in [age, income, debt, assets]:
        return None
    
    debt_to_income = debt / income if income > 0 else 1
    asset_to_debt = assets / debt if debt > 0 else 0
    
    score = 0
    
    if debt_to_income > 0.4:
        score += 2
    elif debt_to_income > 0.2:
        score += 1
        
    if asset_to_debt < 0.5:
        score += 2
    elif asset_to_debt < 1.0:
        score += 1
        
    if age > 55:
        score += 1
    elif age < 25:
        score += 1
    
    if score <= 2:
        return "Low Risk"
    elif score <= 4:
        return "Medium Risk"
    else:
        return "High Risk"

def segment_customer(transactions, avg_transaction_value):
    """Segment customer based on behavior"""
    if transactions is None or avg_transaction_value is None:
        return "Unknown"
    
    if transactions >= 100 and avg_transaction_value >= 100:
        return "VIP"
    elif transactions >= 50 or avg_transaction_value >= 50:
        return "Regular"
    elif transactions >= 10:
        return "Occasional"
    else:
        return "New"

# Register complex UDFs
calculate_risk_score_udf = F.udf(calculate_risk_score, StringType())

# Test data
risk_data = [
    (35, 80000, 20000, 150000),
    (45, 120000, 50000, 200000),
    (28, 45000, 10000, 30000),
    (60, 90000, 80000, 100000),
]

df_risk = spark.createDataFrame(
    risk_data, 
    ["age", "income", "debt", "assets"]
)

print("\nRisk Assessment Data:")
df_risk.show()

print("\nRisk Scores:")
df_risk.withColumn("risk_category", calculate_risk_score_udf(
    "age", "income", "debt", "assets"
)).show())

spark.stop()
```

### Output Results

```
======================================
CUSTOM FUNCTIONS AND UDFs
======================================

1. BASIC STRING UDFs
--------------------------------------------------

Original Employee Data:
+-----------------+---+--------------------+------+------+
|             name|age|               email|salary|rating|
+-----------------+---+--------------------+------+------+
|        John Smith| 28|john.smith@company.com| 5000.0|   3.5|
|    Alice Johnson| 34| alice.j@company.com| 7500.0|   4.2|
|      Bob Williams| 45|   bob.w@company.com| 9000.0|   4.8|
|      Charlie Brown| 23|charlie.b@company.com| 3500.0|   2.8|
|    Diana Martinez| 38|  diana.m@company.com| 8500.0|   4.0|
+-----------------+----+------------------+------+------+

After String UDFs:
+-----------------+---+--------------------+------+------+-------------+--------+
|             name|age|               email|salary|rating|       domain|initials|
+-----------------+---+--------------------+------+------+-------------+--------+
|        John Smith| 28|john.smith@company.com| 5000.0|   3.5|  company.com|      JS|
|    Alice Johnson| 34| alice.j@company.com| 7500.0|   4.2|  company.com|      AJ|
|      Bob Williams| 45|   bob.w@company.com| 9000.0|   4.8|  company.com|      BW|
|      Charlie Brown| 23|charlie.b@company.com| 3500.0|   2.8|  company.com|      CB|
|    Diana Martinez| 38|  diana.m@company.com| 8500.0|   4.0|  company.com|      DM|
+-----------------+---+--------------------+------+------+-------------+--------+

2. NUMERIC UDFs
--------------------------------------------------

After Numeric UDFs:
+-----------------+---+------+------+-----+--------+------------+
|             name|age|salary|rating|  tax|    bonus|total_comp  |
+-----------------+---+------+------+-----+--------+------------+
|        John Smith| 28| 5000.0|   3.5|1250.0| 280.0  |    5280.0  |
|    Alice Johnson| 34| 7500.0|   4.2|1875.0| 504.0  |    8004.0  |
|      Bob Williams| 45| 9000.0|   4.8|2250.0| 691.2  |    9691.2  |
|      Charlie Brown| 23| 3500.0|   2.8|875.0| 78.4   |    3578.4  |
|    Diana Martinez| 38| 8500.0|   4.0|2125.0| 544.0  |    9044.0  |
+-----------------+---+------+------+-----+--------+------------+

8. CONDITIONAL UDFs
--------------------------------------------------

Employee Tiers:
+-----------------+------+----------+
|             name|salary|      tier|
+-----------------+------+----------+
|        John Smith| 5000.0|   Silver |
|    Alice Johnson| 7500.0|   Gold   |
|      Bob Williams| 9000.0|   Gold   |
|      Charlie Brown| 3500.0|  Bronze  |
|    Diana Martinez| 8500.0|   Gold   |
+-----------------+------+----------+

9. UDFs WITH COMPLEX LOGIC
--------------------------------------------------

Risk Assessment Data:
+----+--------+-----+------+
| age|income | debt|assets|
+----+--------+-----+------+
|  35|  80000|20000|150000|
|  45| 120000|50000|200000|
|  28|  45000|10000| 30000|
|  60|  90000|80000|100000|
+----+--------+-----+------+

Risk Scores:
+----+--------+-----+------+----------+
| age|income | debt|assets|risk_category|
+----+--------+-----+------+----------+
|  35|  80000|20000|150000|    Low Risk |
|  45| 120000|50000|200000|  Medium Risk|
|  28|  45000|10000| 30000|    Low Risk |
|  60|  90000|80000|100000|  Medium Risk|
+----+--------+-----+------+----------+
```

## IV. APPLICATIONS

### Banking and Financial Services Examples

```python
"""
UDFs in Banking - Financial Calculations
"""

def banking_udf_demo(spark):
    """Demonstrate UDFs for banking applications"""
    
    print("\n" + "=" * 70)
    print("BANKING APPLICATION: FINANCIAL CALCULATIONS")
    print("=" * 70)
    
    # Sample account data
    accounts = [
        ("ACC001", "John", 15000.0, 5000.0, 5.2),
        ("ACC002", "Alice", 25000.0, 12000.0, 4.8),
        ("ACC003", "Bob", 8000.0, 2000.0, 6.5),
    ]
    
    df = spark.createDataFrame(
        accounts, 
        ["account_id", "name", "balance", "debt", "credit_score"]
    )
    
    # Calculate credit utilization
    def calculate_utilization(balance, credit_limit):
        if credit_limit and credit_limit > 0:
            return min(balance / credit_limit, 1.0)
        return 0.0
    
    # Calculate risk premium
    def calculate_risk_premium(credit_score):
        if credit_score is None:
            return 0.05
        elif credit_score >= 700:
            return 0.03
        elif credit_score >= 650:
            return 0.05
        else:
            return 0.08
    
    # Determine account tier
    def determine_tier(balance, credit_score):
        if balance > 20000 and credit_score >= 700:
            return "Platinum"
        elif balance > 10000 and credit_score >= 650:
            return "Gold"
        else:
            return "Standard"
    
    # Register UDFs
    utilization_udf = F.udf(calculate_utilization, DoubleType())
    risk_premium_udf = F.udf(calculate_risk_premium, DoubleType())
    tier_udf = F.udf(determine_tier, StringType())
    
    # Apply UDFs
    result = df.withColumn("utilization", utilization_udf("balance", "debt")) \
        .withColumn("risk_premium", risk_premium_udf("credit_score")) \
        .withColumn("tier", tier_udf("balance", "credit_score"))
    
    print("\nAccount Analysis:")
    result.show()
```

### Healthcare Applications

```python
"""
UDFs in Healthcare - Clinical Calculations
"""

def healthcare_udf_demo(spark):
    """Demonstrate UDFs for healthcare applications"""
    
    print("\n" + "=" * 70)
    print("HEALTHCARE APPLICATION: CLINICAL CALCULATIONS")
    print("=" * 70)
    
    # Sample patient data
    patients = [
        ("PAT001", 45, 72, 180, ["hypertension", "diabetes"]),
        ("PAT002", 32, 68, 165, []),
        ("PAT003", 58, 85, 200, ["heart_disease", "hypertension"]),
    ]
    
    df = spark.createDataFrame(
        patients, 
        ["patient_id", "age", "weight", "height", "conditions"]
    )
    
    # Calculate BMI
    def calculate_bmi(weight, height):
        if weight and height and height > 0:
            height_m = height / 100
            return weight / (height_m * height_m)
        return None
    
    # Calculate BMI category
    def bmi_category(bmi):
        if bmi is None:
            return "Unknown"
        elif bmi < 18.5:
            return "Underweight"
        elif bmi < 25:
            return "Normal"
        elif bmi < 30:
            return "Overweight"
        else:
            return "Obese"
    
    # Calculate risk score
    def calculate_risk_score(age, conditions):
        if age is None:
            return 0
        score = 0
        if age > 50:
            score += 2
        elif age > 35:
            score += 1
        if conditions:
            score += len(conditions) * 2
        return score
    
    # Register UDFs
    bmi_udf = F.udf(calculate_bmi, DoubleType())
    category_udf = F.udf(bmi_category, StringType())
    risk_udf = F.udf(calculate_risk_score, IntegerType())
    
    # Apply UDFs
    result = df.withColumn("bmi", bmi_udf("weight", "height")) \
        .withColumn("bmi_category", category_udf("bmi_udf")) \
        .withColumn("risk_score", risk_udf("age", "conditions"))
    
    print("\nPatient Risk Assessment:")
    result.show()
```

## V. ASCII FLOW VISUALIZATION

### UDF Execution Flow

```
+=========================================================================+
|                    UDF EXECUTION FLOW                                   |
+=========================================================================+

INPUT DATAFRAME:
+------------+---------+------+
|        name|     age|salary|
+------------+---------+------+
|  John     |      28| 5000 |
|  Alice    |      34| 7500 |
|  Bob      |      45| 9000 |
+------------+---------+------+

STEP 1: UDF DEFINITION
  def calculate_tax(salary):
      if salary and salary > 0:
          return salary * 0.25
      return 0.0

  UDF Registration:
  calculate_tax_udf = udf(calculate_tax, DoubleType())

STEP 2: APPLY UDF TO DATAFRAME
  df_with_tax = df.withColumn(
      "tax", 
      calculate_tax_udf("salary")
  )

STEP 3: SPARK EXECUTION

  Python Process                    JVM
  ┌──────────────────┐           ┌──────────────────┐
  │   Function       │<──Arrow──>│   Row Data        │
  │   Definition     │───────────>│   Processing      │
  └──────────────────┘           └──────────────────┘

  For each row:
  1. Extract salary value from row
  2. Call Python function
  3. Receive result
  4. Write to new column

STEP 4: OUTPUT
  +------------+---------+------+--------+
  |        name|     age|salary|     tax|
  +------------+---------+------+--------+
  |  John     |      28| 5000 | 1250   |
  |  Alice    |      34| 7500 | 1875   |
  |  Bob      |      45| 9000 | 2250   |
  +------------+---------+------+--------+

+=========================================================================+
|                    PYTHON UDF vs PANDAS UDF                            |
+=========================================================================+

Python UDF (Row-by-Row):
  ┌──┐    ┌──┐    ┌──┐    ┌──┐    ┌──┐
  │R1│───>│R2│───>│R3│───>│R4│───>│R5│
  └──┘    └──┘    └──┘    └──┘    └──┘
   │       │       │       │       │
   ▼       ▼       ▼       ▼       ▼
  [Func]  [Func]  [Func]  [Func]  [Func]

  Issues: 
  - Serialization overhead between JVM/Python
  - One row at a time processing
  - Memory inefficient

Pandas UDF (Vectorized):
  ┌─────────┐
  │  Batch  │
  │  Data   │
  └────┬────┘
       │ Arrow Transfer (Apache Arrow)
       ▼
  ┌─────────────────┐
  │  Pandas Series  │
  │  Processing    │
  └─────────────────┘

  Benefits:
  - Single Arrow transfer per batch
  - Vectorized operations
  - Memory efficient

+=========================================================================+
|                    NULL HANDLING FLOW                                  |
+=========================================================================+

Input:                    UDF:                    Output:
+------------+         def process(x):        +------------+
|    name     |         if x is None:      |    result  |
+------------+         return "Unknown"     +------------+
|  John      │         return x.upper()    |    JOHN    |
|  null      │                          |  Unknown   |
|  Alice     │                          |    ALICE   |
+------------+                          +------------+

BEST PRACTICE:
  Always handle nulls explicitly in UDFs
  - Check for None/null at function start
  - Return default values for null inputs
  - Use coalesce() in pipeline for extra safety
```

## VI. ADVANCED TOPICS

### Performance Optimization

UDFs can be performance bottlenecks. Here are optimization strategies:

1. **Use Pandas UDFs**: For better performance on large datasets
2. **Avoid repeated calculations**: Cache intermediate results
3. **Minimize data movement**: Keep UDFs simple and focused
4. **Broadcast small datasets**: When joining with reference data

```python
# Performance optimization example

# Instead of Python UDF (slow)
@udf
def slow_calculation(x):
    return complex_python_logic(x)

# Use Pandas UDF (fast)
@pandas_udf(DoubleType())
def fast_calculation(x: pd.Series) -> pd.Series:
    return x.apply(complex_python_logic)

# Or better, use native Spark functions
from pyspark.sql.functions import exp, log
df.withColumn("result", exp(log(col("x")) * 10)
```

### Best Practices

1. **Prefer built-in functions** when possible
2. **Use Pandas UDFs** for better performance
3. **Handle nulls explicitly** in all UDFs
4. **Test UDFs** with sample data before production
5. **Monitor performance** of UDF-heavy transformations

### Common Pitfalls

1. **Not handling nulls**: Leads to null results
2. **Using non-serializable objects**: Causes errors in distributed execution
3. **Memory issues**: Large intermediate results
4. **Type mismatches**: Runtime errors

## VII. CONCLUSION

### Key Takeaways

User Defined Functions extend Spark's capabilities beyond built-in functions, enabling custom business logic and domain-specific transformations. While UDFs provide flexibility, they should be used judiciously due to performance implications. Pandas UDFs offer significant performance improvements for large-scale operations.

### Best Practices Summary

1. Always define return types explicitly
2. Handle null values in UDFs
3. Prefer built-in functions when possible
4. Use Pandas UDFs for better performance
5. Test UDF logic independently before production

### Next Steps

Continue to the next module to learn performance optimization techniques for Spark applications, ensuring your data pipelines are efficient and scalable.

```python
# Quick Reference: UDF Types and Registration

from pyspark.sql.functions import udf
from pyspark.sql.types import *

# Python UDF
@udf(returnType=StringType())
def my_udf(x):
    return str(x)

# Pandas UDF
@pandas_udf(returnType=DoubleType())
def my_pandas_udf(x: pd.Series) -> pd.Series:
    return x * 2

# SQL Registration
spark.udf.register("my_sql_udf", my_udf, StringType())
spark.sql("SELECT my_sql_udf(col) FROM table")
```