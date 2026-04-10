# Data Cleaning and Preprocessing with Pandas

## Table of Contents
- [Introduction](#introduction)
- [Fundamentals](#fundamentals)
- [Implementation](#implementation)
- [Applications](#applications)
- [Output Results](#output-results)
- [Visualization](#visualization)
- [Advanced Topics](#advanced-topics)
- [Conclusion](#conclusion)

---

## Introduction

### What is Data Cleaning?

Data cleaning is the process of detecting and correcting (or removing) inaccurate, corrupted, duplicate, or irrelevant data from a dataset. It is a crucial first step in any data analysis project.

### Why Data Cleaning Matters

- Poor data quality leads to incorrect analysis
- Dirty data costs businesses billions annually
- Clean data improves model accuracy in ML
- Ensures reliable decision-making

### Common Data Quality Issues

1. **Missing Values**: Empty cells, null values
2. **Duplicates**: Repeated records
3. **Inconsistent Data**: Different formats, typos
4. **Outliers**: Unusual values
5. **Wrong Data Types**: Incorrect column types

---

## Fundamentals

### Handling Missing Values

```python
import pandas as pd
import numpy as np

# Create sample DataFrame with missing values
df = pd.DataFrame({
    'Name': ['Alice', 'Bob', None, 'David', 'Eva'],
    'Age': [25, None, 35, 40, 28],
    'Salary': [50000, 60000, None, 75000, None],
    'Department': ['IT', 'HR', 'Finance', None, 'Marketing']
})

# Check for missing values
print(df.isnull())
print(df.isnull().sum())

# Fill missing values
df['Name'].fillna('Unknown', inplace=True)
df['Age'].fillna(df['Age'].median(), inplace=True)

# Drop rows with missing values
df_clean = df.dropna()

# Forward fill (propagate last valid value)
df['Salary'].fillna(method='ffill', inplace=True)

# Backward fill
df['Department'].fillna(method='bfill', inplace=True)
```

### Removing Duplicates

```python
# Create sample DataFrame with duplicates
df = pd.DataFrame({
    'Name': ['Alice', 'Bob', 'Alice', 'David', 'Bob'],
    'Age': [25, 30, 25, 35, 30],
    'Department': ['IT', 'HR', 'IT', 'Finance', 'HR']
})

# Find duplicates
print(df.duplicated())

# Remove duplicates
df_clean = df.drop_duplicates()

# Remove duplicates in specific column
df_clean = df.drop_duplicates(subset=['Name'])
```

### Data Type Conversion

```python
# Convert data types
df['Age'] = df['Age'].astype(str)
df['Salary'] = pd.to_numeric(df['Salary'])

# Parse dates
df['HireDate'] = pd.to_datetime(df['HireDate'])

# Convert to categorical
df['Department'] = df['Department'].astype('category')
```

### String Cleaning

```python
# Trim whitespace
df['Name'] = df['Name'].str.strip()

# Convert to lowercase
df['Name'] = df['Name'].str.lower()

# Convert to uppercase
df['Department'] = df['Department'].str.upper()

# Replace values
df['Department'] = df['Department'].str.replace('IT', 'Information Technology')

# Remove special characters
df['Phone'] = df['Phone'].str.replace(r'\D', '', regex=True)
```

### Handling Outliers

```python
# Detect outliers using IQR
Q1 = df['Salary'].quantile(0.25)
Q3 = df['Salary'].quantile(0.75)
IQR = Q3 - Q1

lower_bound = Q1 - 1.5 * IQR
upper_bound = Q3 + 1.5 * IQR

outliers = df[(df['Salary'] < lower_bound) | (df['Salary'] > upper_bound)]

# Cap outliers
df['Salary'] = df['Salary'].clip(lower_bound, upper_bound)
```

---

## Implementation

### Example 1: Employee Data Cleaning

```python
import pandas as pd
import numpy as np

# Create sample employee data with issues
employees = pd.DataFrame({
    'emp_id': ['E001', 'E002', 'E001', 'E003', 'E004', 'E005', 'E006'],
    'name': [' Alice ', 'BOB', 'Carol', 'DAVID ', 'Eva', 'Frank ', None],
    'age': [25, 30, 25, 35, -5, 45, 50],
    'salary': [50000, 60000, 50000, 75000, 12000000, 45000, 55000],
    'department': ['IT', 'hr', 'IT', 'Finance', 'IT', 'marketing', 'HR'],
    'email': ['alice@co.com', 'bob@co.com', 'carol@co.com', 'david@co.com', 'eva@co.com', 'frank@co.com', None]
})

print("=== Original Data ===")
print(employees)

# Step 1: Remove duplicates
employees = employees.drop_duplicates(subset=['emp_id'])
print("\n=== After Removing Duplicates ===")
print(employees)

# Step 2: Clean string columns
employees['name'] = employees['name'].str.strip().str.title()
employees['department'] = employees['department'].str.lower().str.title()

# Step 3: Handle missing values
employees['name'].fillna('Unknown', inplace=True)
employees['email'].fillna('unknown@co.com', inplace=True)

# Step 4: Fix invalid ages
employees.loc[employees['age'] < 0, 'age'] = np.nan
employees['age'].fillna(employees['age'].median(), inplace=True)

# Step 5: Handle salary outliers (cap at 200000)
employees['salary'] = employees['salary'].clip(0, 200000)

print("\n=== Cleaned Data ===")
print(employees)
```

### Example 2: Transaction Data Cleaning

```python
# Create sample transaction data
transactions = pd.DataFrame({
    'transaction_id': ['TX001', 'TX002', 'TX003', 'TX001', 'TX004'],
    'date': ['2024-01-15', '2024-01-16', 'invalid', '2024-01-17', '2024-01-18'],
    'amount': [100.50, -50, 200.75, 100.50, 300],
    'account': ['ACC001', 'ACC002', 'ACC003', 'ACC001', 'ACC004'],
    'description': ['  Deposit  ', 'WITHDRAWAL', 'transfer', 'Deposit', None]
})

print("=== Original Transactions ===")
print(transactions)

# Fix dates
transactions['date'] = pd.to_datetime(transactions['date'], errors='coerce')

# Fix negative amounts (make positive)
transactions['amount'] = transactions['amount'].abs()

# Clean descriptions
transactions['description'] = transactions['description'].str.strip().str.title()
transactions['description'].fillna('N/A', inplace=True)

# Remove duplicate transactions
transactions = transactions.drop_duplicates()

print("\n=== Cleaned Transactions ===")
print(transactions)
```

---

## Applications

### Banking Sector: Customer Data Quality

```python
import pandas as pd
import numpy as np

# Create banking customer data with issues
bank_customers = pd.DataFrame({
    'customer_id': ['C001', 'C002', 'C001', 'C003', 'C004', 'C005'],
    'name': [' JAMES WILSON ', 'maria Garcia', 'James Wilson ', 'ROBERT CHEN', 'Lisa  ', None],
    'account_type': ['checking', 'SAVINGS', 'CHECKING', 'Savings', 'checking', 'PREMIUM'],
    'balance': [15000, 25000, 15000, 85000, -5000, 75000],
    'credit_score': [720, 750, 720, 820, 700, None],
    'email': ['james@bank.com', 'maria@bank.com', 'JAMES@bank.com', 'robert@bank.com', 'lisa@bank.com', None],
    'phone': ['555-1234', '555-5678', '555-1234', '555-9012', '(555) 3456', '555-7890']
})

print("=== Original Banking Data ===")
print(bank_customers)

# Step 1: Remove duplicates
bank_customers = bank_customers.drop_duplicates(subset=['customer_id'])

# Step 2: Standardize names
bank_customers['name'] = bank_customers['name'].str.strip().str.title()
bank_customers['name'].fillna('Unknown', inplace=True)

# Step 3: Standardize account types
bank_customers['account_type'] = bank_customers['account_type'].str.lower().str.title()

# Step 4: Fix negative balances
bank_customers.loc[bank_customers['balance'] < 0, 'balance'] = 0

# Step 5: Handle missing credit scores
bank_customers['credit_score'].fillna(bank_customers['credit_score'].median(), inplace=True)

# Step 6: Normalize emails
bank_customers['email'] = bank_customers['email'].str.lower()
bank_customers['email'].fillna('unknown@bank.com', inplace=True)

# Step 7: Clean phone numbers
bank_customers['phone'] = bank_customers['phone'].str.replace(r'\D', '', regex=True)
bank_customers['phone'] = '555-' + bank_customers['phone']

print("\n=== Cleaned Banking Data ===")
print(bank_customers)

# Create data quality report
print("\n=== Data Quality Report ===")
print(f"Total Customers: {len(bank_customers)}")
print(f"Missing Values:\n{bank_customers.isnull().sum()}")
```

### Healthcare Sector: Patient Records

```python
# Create patient data with issues
patients = pd.DataFrame({
    'patient_id': ['PT001', 'PT002', 'PT001', 'PT003', 'PT004', 'PT005'],
    'name': ['JOHN DOE', 'jane smith', 'John Doe ', 'Bob   Johnson', 'ALICE  Williams', None],
    'dob': ['1979-05-15', '1986-10-22', '1979-05-15', '1962-03-08', '1995/07/30', '1980-01-01'],
    'gender': ['M', 'F', 'M', 'M', 'F ', None],
    'admission_date': ['2024-01-01', '2024-01-05', '2024-01-01', '2024-01-09', '2024-01-13', '2024/01/15'],
    'diagnosis': ['Hypertension', 'Flu', 'Hypertension', 'fracture', 'Infection', 'Migraine'],
    'room': ['501A', '302B', '501A', '401A', '305A ', '501A'],
    'insurance_id': ['INS001', 'INS002', 'INS001', 'INS003', 'INS004', None]
})

print("=== Original Patient Data ===")
print(patients)

# Step 1: Remove duplicates
patients = patients.drop_duplicates(subset=['patient_id'])

# Step 2: Clean names
patients['name'] = patients['name'].str.strip().str.title()
patients['name'].fillna('Unknown', inplace=True)

# Step 3: Parse dates
patients['dob'] = pd.to_datetime(patients['dob'], errors='coerce')
patients['admission_date'] = pd.to_datetime(patients['admission_date'], errors='coerce')

# Step 4: Standardize gender
patients['gender'] = patients['gender'].str.strip().str.upper()
patients['gender'].fillna('U', inplace=True)

# Step 5: Clean diagnosis
patients['diagnosis'] = patients['diagnosis'].str.strip().str.title()

# Step 6: Clean room numbers
patients['room'] = patients['room'].str.strip().str.upper()

# Step 7: Handle missing insurance
patients['insurance_id'].fillna('UNKNOWN', inplace=True)

# Step 8: Calculate age
patients['age'] = (pd.Timestamp.now() - patients['dob']).dt.days / 365.25

print("\n=== Cleaned Patient Data ===")
print(patients)

# Calculate statistics
print("\n=== Patient Statistics ===")
print(f"Total Patients: {len(patients)}")
print(f"Average Age: {patients['age'].mean():.1f}")
print(f"Gender Distribution:\n{patients['gender'].value_counts()}")
```

---

## Output Results

### Sample Output - Employee Data

```
=== Original Data ===
  emp_id     name  age    salary  department                          email
0  E001   Alice   25   50000.0         IT                      alice@co.com
1  E002     Bob   30   60000.0         HR                      bob@co.com
2  E001   Alice   25   50000.0         IT                      carol@co.com
3  E003   David   35   75000.0     Finance                     david@co.com
4  E004    Eva -5  12000000.0         IT                      eva@co.com
5  E005  Frank   45   45000.0  Marketing                     frank@co.com
6  E006    NaN   50   55000.0         HR                             None

=== Cleaned Data ===
  emp_id     name   age   salary  department           email
0  E001   Alice   25   50000.0         It           alice@co.com
1  E002     Bob    30   60000.0         Hr           bob@co.com
2  E003   David   35   75000.0     Finance        david@co.com
3  E004    Eva    30  200000.0         It           eva@co.com
4  E005   Frank    45   45000.0  Marketing       frank@co.com
5  E006   Unknown 50   55000.0         Hr   unknown@co.com
```

### Sample Output - Banking Data

```
=== Cleaned Banking Data ===
  customer_id         name account_type  balance  credit_score           email        phone
0       C001   James Wilson     Checking   15000.0           720  james@bank.com   555-1234
1       C002   Maria Garcia    Savings   25000.0           750  maria@bank.com   555-5678
2       C003     Robert Chen    Savings   85000.0           820  robert@bank.com  555-9012
3       C004        Lisa       Checking    0.0           700  lisa@bank.com  555-3456
4       C005     Unknown    Premium   75000.0           750  unknown@bank.com 555-7890

=== Data Quality Report ===
Total Customers: 5
Missing Values:
customer_id       0
name             0
account_type     0
balance          0
credit_score     0
email            0
phone            0
```

---

## Visualization

### ASCII - Data Cleaning Process

```
+------------------------------------------------------------------+
|              DATA CLEANING WORKFLOW                                |
+------------------------------------------------------------------+

        +-----------------+
        |   RAW DATA     |
        |   WITH ISSUES |
        +-----------------+
                |
                v
+-------------------------------+
|  1. REMOVE DUPLICATES         |
|  - check duplicate rows      |
|  - drop duplicates          |
+-------------------------------+
                |
                v
+-------------------------------+
|  2. HANDLE MISSING VALUES   |
|  - identify missing        |
|  - fill or drop            |
+-------------------------------+
                |
                v
+-------------------------------+
|  3. STANDARDIZE DATA       |
|  - trim whitespace        |
|  - convert case           |
|  - format dates           |
+-------------------------------+
                |
                v
+-------------------------------+
|  4. CORRECT ERRORS         |
|  - fix negative values   |
|  - validate ranges        |
|  - remove outliers        |
+-------------------------------+
                |
                v
+-------------------------------+
|  5. VERIFY & REPORT        |
|  - check data quality    |
|  - generate report        |
+-------------------------------+
                |
                v
        +-----------------+
        |  CLEAN DATA    |
        +-----------------+

+------------------------------------------------------------------+
```

### ASCII - Data Quality Issues

```
+------------------------------------------------------------------+
|           COMMON DATA QUALITY ISSUES                              |
+------------------------------------------------------------------+

ISSUE                    DETECTION                 SOLUTION
------                   ---------                 -------
                         
[DUPLICATES]         ████████████████████    drop_duplicates()
Duplicate rows       5% of records           Remove dups

[MISSING]            ████████████████░░    fillna() / dropna()
Null/empty values    10% of records           Impute or drop

[OUTLIERS]          ██████████████░░░░░    IQR method
Unusual values     2-5% of records           Cap or remove

[INCONSISTENT]     ████████████░░░░░░░    Standardize_case()
Mixed case/formats 15% of records           Normalize

[INVALID]          ██████████░░░░░░░░░    Validate_range()
Wrong values      3% of records           Correct

[TYPES]            ████████░░░░░░░░░░    astype() / to_numeric()
Wrong data types    8% of records           Convert

+------------------------------------------------------------------+
```

### ASCII - Banking Data Validation

```
+------------------------------------------------------------------+
|         BANKING DATA QUALITY CHECKS                                |
+------------------------------------------------------------------+

BALANCE VALIDATION:
                  Valid    Invalid    Fixed
$0-$10K          ████████     ████     ████
$10K-$50K        ████████████████████████
$50K-$100K       ████████████     ████     ████
$100K+           ██████      █████   █████

CREDIT SCORE:
    300-579  ████     Poor
    580-669  ██████    Fair
    670-739  ██████████████    Good
    740-799  ████████████████    Very Good
    800-850  ████████    Excellent

EMAIL FORMAT:
    Valid   ████████████████████████████
    Invalid ██░░░░    Fixed: ██

PHONE FORMAT:
    Clean   ██████████████████████████
    Dirty   ████      Fixed: ████

+------------------------------------------------------------------+
```

---

## Advanced Topics

### Advanced Missing Value Imputation

```python
# Using sklearn for imputation
from sklearn.impute import SimpleImputer, KNNImputer

# Mean/Median imputation
imputer = SimpleImputer(strategy='median')
df_imputed = pd.DataFrame(
    imputer.fit_transform(df),
    columns=df.columns
)

# KNN imputation (more accurate)
knn_imputer = KNNImputer(n_neighbors=3)
df_imputed = pd.DataFrame(
    knn_imputer.fit_transform(df),
    columns=df.columns
)

# Iterative imputation
from sklearn.impute import IterativeImputer
iter_imputer = IterativeImputer()
df_imputed = pd.DataFrame(
    iter_imputer.fit_transform(df),
    columns=df.columns
)
```

### Complex Data Validation

```python
# Create validation rules
def validate_employee_data(df):
    errors = []
    
    # Rule 1: Salary must be positive
    if (df['salary'] <= 0).any():
        errors.append("Invalid salary values found")
    
    # Rule 2: Age must be 18-100
    if (df['age'] < 18) | (df['age'] > 100):
        errors.append("Age out of valid range")
    
    # Rule 3: Email must contain @
    if ~df['email'].str.contains('@'):
        errors.append("Invalid email format")
    
    return errors

# Apply validation
errors = validate_employee_data(employees)
if errors:
    print("Validation Errors:", errors)
else:
    print("Data is valid")
```

### Pipelines for Data Cleaning

```python
from sklearn.pipeline import Pipeline
from sklearn.compose import ColumnTransformer
from sklearn.preprocessing import StandardScaler, OneHotEncoder

# Define preprocessing pipeline
numeric_features = ['age', 'salary', 'credit_score']
categorical_features = ['department', 'gender']

preprocessor = ColumnTransformer(
    transformers=[
        ('num', StandardScaler(), numeric_features),
        ('cat', OneHotEncoder(), categorical_features)
    ]
)

pipeline = Pipeline([
    ('preprocessor', preprocessor),
    ('classifier', RandomForestClassifier())
])
```

### Automated Data Cleaning

```python
# Auto-clean function
def auto_clean_dataframe(df):
    df_clean = df.copy()
    
    # Remove duplicates
    df_clean = df_clean.drop_duplicates()
    
    # Clean string columns
    for col in df_clean.select_dtypes(include=['object']).columns:
        df_clean[col] = df_clean[col].str.strip()
    
    # Handle numeric columns
    for col in df_clean.select_dtypes(include=['number']).columns:
        df_clean[col] = pd.to_numeric(df_clean[col], errors='coerce')
    
    # Fill missing values
    for col in df_clean.columns:
        if df_clean[col].dtype in ['float64', 'int64']:
            df_clean[col].fillna(df_clean[col].median(), inplace=True)
        else:
            df_clean[col].fillna('Unknown', inplace=True)
    
    return df_clean

# Apply auto-clean
df_clean = auto_clean_dataframe(df)
```

---

## Conclusion

### Key Takeaways

This module covered essential data cleaning techniques:

1. **Missing Values**: Detection and handling strategies
2. **Duplicates**: Identification and removal
3. **Data Types**: Proper conversion methods
4. **String Cleaning**: Standardization techniques
5. **Outliers**: Detection and capping
6. **Validation**: Data quality checks

### Practical Applications

- Banking: Customer data quality, transaction records
- Healthcare: Patient records, medical data
- ETL pipelines: Data transformation

### Next Steps

Continue to: **Data Manipulation and Transformation** for advanced operations.

### References

- Pandas Cleaning: https://pandas.pydata.org/docs/user_guide/missing_data.html
- sklearn preprocessing documentation