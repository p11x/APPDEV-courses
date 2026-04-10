# Pandas Series and DataFrames

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

### What is Pandas?

Pandas is a powerful open-source data manipulation and analysis library for Python. Built on top of NumPy, it provides flexible and expressive data structures designed to make working with structured data (tabular, time series, etc.) intuitive and easy. The name "Pandas" is derived from "Panel Data," an econometric term for multidimensional data sets.

### Importance in Data Science

Pandas is the de facto standard for data manipulation in Python, used across industries for:
- Data cleaning and preparation
- Data analysis and exploration
- Data transformation and feature engineering
- Integration with various data sources (CSV, Excel, SQL, etc.)
- Time series analysis

### Series vs DataFrames

Pandas provides two primary data structures:

1. **Series**: A one-dimensional labeled array capable of holding any data type.
2. **DataFrame**: A two-dimensional labeled data structure with columns of potentially different types.

---

## Fundamentals

### Creating a Pandas Series

```python
import pandas as pd
import numpy as np

# Create a Series from a list
s = pd.Series([1, 2, 3, 4, 5])
print(s)

# Create a Series with custom index
s = pd.Series([10, 20, 30, 40], index=['a', 'b', 'c', 'd'])
print(s)

# Create a Series from a dictionary
data = {'apple': 150, 'banana': 100, 'orange': 75}
fruits = pd.Series(data)
print(fruits)

# Create a Series with specific data type
s = pd.Series([1.5, 2.5, 3.5], dtype=float)
```

### Creating a DataFrame

```python
# Create a DataFrame from a dictionary
data = {
    'Name': ['John', 'Emma', 'Mike', 'Sarah'],
    'Age': [28, 34, 42, 29],
    'Salary': [55000, 72000, 68000, 51000]
}
df = pd.DataFrame(data)
print(df)

# Create a DataFrame with custom index
df = pd.DataFrame(data, index=['EMP001', 'EMP002', 'EMP003', 'EMP004'])

# Create a DataFrame from a list of dictionaries
records = [
    {'Name': 'John', 'Age': 28, 'Department': 'IT'},
    {'Name': 'Emma', 'Age': 34, 'Department': 'HR'},
    {'Name': 'Mike', 'Age': 42, 'Department': 'Finance'}
]
df = pd.DataFrame(records)
```

### Accessing Data

```python
# Access a column
names = df['Name']

# Access multiple columns
subset = df[['Name', 'Salary']]

# Access a row by index label
row = df.loc['EMP001']

# Access a row by integer position
row = df.iloc[0]

# Access a specific cell
value = df.loc['EMP001', 'Salary']

# Boolean indexing
high_earners = df[df['Salary'] > 60000]
```

### Basic Operations

```python
# Get DataFrame info
print(df.info())
print(df.describe())

# Check shape
print(df.shape)

# Check column names
print(df.columns)

# Check index
print(df.index)

# Transpose
print(df.T)

# Select dtypes
numeric_df = df.select_dtypes(include=[np.number])
```

---

## Implementation

### Example 1: Employee Management System

```python
import pandas as pd
import numpy as np

# Create Employee DataFrame
employees = pd.DataFrame({
    'emp_id': ['E001', 'E002', 'E003', 'E004', 'E005'],
    'name': ['Alice Johnson', 'Bob Smith', 'Carol White', 'David Brown', 'Eva Martinez'],
    'department': ['IT', 'HR', 'Finance', 'IT', 'Marketing'],
    'salary': [75000, 65000, 72000, 80000, 68000],
    'experience_years': [5, 3, 7, 8, 4],
    'performance_score': [4.5, 4.0, 4.2, 4.8, 3.9]
})

# Display basic info
print("=== Employee DataFrame ===")
print(employees)
print(f"\nShape: {employees.shape}")
print(f"\nData Types:\n{employees.dtypes}")

# Select top performers
top_performers = employees[employees['performance_score'] >= 4.5]
print(f"\n=== Top Performers ===")
print(top_performers)
```

### Example 2: Sales Data Analysis

```python
# Create Sales DataFrame
sales_data = pd.DataFrame({
    'date': pd.date_range('2024-01-01', periods=10, freq='D'),
    'product': ['Widget A', 'Widget B', 'Widget A', 'Widget C', 'Widget B',
                'Widget A', 'Widget C', 'Widget B', 'Widget A', 'Widget C'],
    'quantity': [100, 50, 75, 30, 60, 90, 25, 45, 110, 35],
    'unit_price': [19.99, 29.99, 19.99, 49.99, 29.99, 19.99, 49.99, 29.99, 19.99, 49.99]
})

# Calculate total revenue
sales_data['total_revenue'] = sales_data['quantity'] * sales_data['unit_price']

# Group by product
product_summary = sales_data.groupby('product').agg({
    'quantity': 'sum',
    'total_revenue': 'sum'
}).reset_index()

print("=== Sales Summary by Product ===")
print(product_summary)
```

---

## Applications

### Banking Sector Application

#### Customer Account Management

```python
import pandas as pd
import numpy as np
from datetime import datetime, timedelta

# Create Bank Customer DataFrame
customers = pd.DataFrame({
    'account_id': [f'ACC{str(i).zfill(4)}' for i in range(1001, 1011)],
    'customer_name': ['James Wilson', 'Maria Garcia', 'Robert Chen', 'Lisa Anderson',
                     'Michael Lee', 'Sarah Davis', 'Joseph Taylor', 'Jennifer Brown',
                     'William Martinez', 'Patricia Johnson'],
    'account_type': ['Savings', 'Checking', 'Savings', 'Premium', 'Savings',
                    'Checking', 'Premium', 'Savings', 'Checking', 'Premium'],
    'balance': [15000.00, 5230.50, 25000.00, 85000.00, 12000.00,
               7800.25, 95000.00, 18000.00, 4500.75, 78000.00],
    'account_opened': pd.date_range('2020-01-01', periods=10, freq='180D'),
    'credit_score': [720, 680, 750, 820, 700, 690, 800, 730, 670, 790]
})

print("=== Bank Customer Accounts ===")
print(customers)
print(f"\nTotal Customers: {len(customers)}")
print(f"Total Balance: ${customers['balance'].sum():,.2f}")

# Identify high-value customers
high_value = customers[customers['balance'] >= 20000]
print(f"\n=== High-Value Customers (Balance >= $20,000) ===")
print(high_value[['account_id', 'customer_name', 'balance', 'credit_score']])

# Account type distribution
account_types = customers['account_type'].value_counts()
print(f"\n=== Account Type Distribution ===")
print(account_types)
```

#### Loan Portfolio Analysis

```python
# Create Loan Portfolio DataFrame
loans = pd.DataFrame({
    'loan_id': [f'LN{str(i).zfill(4)}' for i in range(2001, 2011)],
    'account_id': ['ACC1001', 'ACC1003', 'ACC1005', 'ACC1007', 'ACC1002',
                   'ACC1004', 'ACC1006', 'ACC1008', 'ACC1009', 'ACC1010'],
    'loan_type': ['Mortgage', 'Personal', 'Auto', 'Mortgage', 'Personal',
                 'Auto', 'Mortgage', 'Personal', 'Auto', 'Mortgage'],
    'principal': [250000, 25000, 35000, 320000, 15000,
                  28000, 400000, 20000, 42000, 350000],
    'interest_rate': [3.5, 8.5, 6.0, 3.25, 9.0, 6.5, 3.0, 8.0, 5.75, 3.5],
    'term_months': [360, 48, 60, 360, 36, 48, 360, 48, 60, 360],
    'status': ['Current', 'Current', 'Current', 'Current', 'Delinquent',
               'Current', 'Current', 'Current', 'Delinquent', 'Current']
})

# Calculate monthly payment
def calc_monthly_payment(principal, rate, months):
    if rate == 0:
        return principal / months
    monthly_rate = rate / 100 / 12
    payment = principal * (monthly_rate * (1 + monthly_rate) ** months) / ((1 + monthly_rate) ** months - 1)
    return payment

loans['monthly_payment'] = loans.apply(
    lambda x: calc_monthly_payment(x['principal'], x['interest_rate'], x['term_months']),
    axis=1
)

print("=== Loan Portfolio ===")
print(loans[['loan_id', 'account_id', 'loan_type', 'principal', 'monthly_payment', 'status']])

# Loan portfolio summary
loan_summary = loans.groupby('loan_type').agg({
    'principal': 'sum',
    'monthly_payment': 'sum',
    'loan_id': 'count'
}).rename(columns={'loan_id': 'loan_count'})

print(f"\n=== Loan Portfolio Summary by Type ===")
print(loan_summary)
```

### Healthcare Sector Application

#### Patient Records Management

```python
# Create Patient DataFrame
patients = pd.DataFrame({
    'patient_id': [f'PT{str(i).zfill(4)}' for i in range(3001, 3011)],
    'name': ['John Doe', 'Jane Smith', 'Bob Johnson', 'Alice Williams', 'Charlie Brown',
            'Diana Ross', 'Edward Miller', 'Fiona Davis', 'George Wilson', 'Hannah Moore'],
    'age': [45, 38, 62, 29, 55, 41, 68, 33, 58, 50],
    'gender': ['M', 'F', 'M', 'F', 'M', 'F', 'M', 'F', 'M', 'F'],
    'admission_date': pd.date_range('2024-01-01', periods=10, freq='4D'),
    'department': ['Cardiology', 'General', 'Orthopedics', 'General', 'Neurology',
                   'Cardiology', 'Orthopedics', 'General', 'Neurology', 'Cardiology'],
    'room': ['501A', '302B', '401A', '305A', '601B', '502A', '403B', '308A', '602A', '503A'],
    'diagnosis': ['Hypertension', 'Flu', 'Fracture', 'Infection', 'Migraine',
                  'Arrhythmia', ' arthritis', 'Viral Infection', 'Epilepsy', 'Heart Failure'],
    'stay_days': [5, 2, 7, 3, 4, 6, 8, 2, 5, 10]
})

print("=== Patient Records ===")
print(patients)
print(f"\nTotal Patients: {len(patients)}")

# Department distribution
dept_dist = patients['department'].value_counts()
print(f"\n=== Patients by Department ===")
print(dept_dist)
```

#### Medical Billing Analysis

```python
# Create Billing DataFrame
billing = pd.DataFrame({
    'patient_id': patients['patient_id'],
    'room_charge': [500 * days for days in patients['stay_days']],
    'medical_services': [1200, 450, 2500, 800, 1500, 3000, 3200, 380, 1800, 4500],
    'medication': [350, 120, 580, 200, 280, 890, 720, 95, 420, 1150],
    'lab_tests': [200, 150, 400, 180, 300, 450, 380, 120, 350, 520],
    'insurance_covered': [0.80, 0.90, 0.75, 0.85, 0.70, 0.80, 0.85, 0.95, 0.75, 0.80]
})

billing['total_charges'] = (billing['room_charge'] + billing['medical_services'] + 
                          billing['medication'] + billing['lab_tests'])
billing['patient_responsibility'] = billing['total_charges'] * (1 - billing['insurance_covered'])
billing['insurance_payment'] = billing['total_charges'] * billing['insurance_covered']

print("=== Medical Billing Summary ===")
billing_summary = billing[['patient_id', 'total_charges', 'insurance_payment', 'patient_responsibility']]
print(billing_summary)

# Total revenue analysis
print(f"\n=== Financial Summary ===")
print(f"Total Charges: ${billing['total_charges'].sum():,.2f}")
print(f"Insurance Payments: ${billing['insurance_payment'].sum():,.2f}")
print(f"Patient Payments: ${billing['patient_responsibility'].sum():,.2f}")
```

---

## Output Results

### Sample Output Analysis

```
=== Employee DataFrame ===
  emp_id           name department  salary  experience_years  performance_score
0  E001    Alice Johnson          IT   75000                 5               4.5
1  E002       Bob Smith          HR   65000                 3               4.0
2  E003      Carol White      Finance   72000                 7               4.2
3  E004      David Brown          IT   80000                 8               4.8
4  E005    Eva Martinez    Marketing   68000                 4               3.9

Shape: (5, 6)
Data Types:
emp_id               object
name                 object
department           object
salary                int64
experience_years      int64
performance_score    float64

=== Sales Summary by Product ===
    product  quantity  total_revenue
0  Widget A       375      7496.25
1  Widget B       155     4649.45
2  Widget C        90     4499.10
```

### Banking Results Analysis

```
=== Bank Customer Accounts ===
   account_id customer_name account_type    balance account_opened  credit_score
0    ACC1001   James Wilson      Savings  15000.00     2020-01-01           720
1    ACC1002    Maria Garcia     Checking   5230.50     2020-06-02           680
2    ACC1003     Robert Chen      Savings  25000.00     2020-11-28           750
3    ACC1004   Lisa Anderson     Premium  85000.00     2021-05-16           820
4    ACC1005    Michael Lee      Savings  12000.00     2021-10-31           700
5    ACC1006     Sarah Davis     Checking   7800.25     2022-04-18           690
6    ACC1007  Joseph Taylor      Premium  95000.00     2022-10-04           800
7    ACC1008   Jennifer Brown     Savings  18000.00     2023-04-02           730
8    ACC1009  William Martinez   Checking   4500.75     2023-09-19           670
9    ACC1010  Patricia Johnson    Premium  78000.00     2024-03-07           790

Total Customers: 10
Total Balance: $349,531.50
```

### Healthcare Results Analysis

```
=== Patient Records ===
  patient_id          name  age gender admission_date   department  room     diagnosis  stay_days
0    PT3001       John Doe   45      M     2024-01-01  Cardiology  501A  Hypertension          5
1    PT3002      Jane Smith   38      F     2024-01-05     General  302B         Flu          2
2    PT3003    Bob Johnson   62      M     2024-01-09  Orthopedics  401A     Fracture          7
3    PT3004   Alice Williams   29      F     2024-01-13     General  305A    Infection          3
4    PT3005   Charlie Brown   55      M     2024-01-17   Neurology  601B     Migraine          4
5    PT3006     Diana Ross   41      F     2024-01-21  Cardiology  502A   Arrhythmia          6
6    PT3007   Edward Miller   68      M     2024-01-25  Orthopedics  403B   arthritis          8
7    PT3008     Fiona Davis   33      F     2024-01-29     General  308A Viral Infection          2
8    PT3009    George Wilson   58      M     2024-02-02   Neurology  602A     Epilepsy          5
9    PT3010    Hannah Moore   50      F     2024-02-06  Cardiology  503A Heart Failure         10

Total Patients: 10
```

---

## Visualization

### ASCII Visualization - DataFrame Structure

```
+------------------------------------------------------------------+
|                    DATAFRAME STRUCTURE                           |
+------------------------------------------------------------------+
|                                                                  |
|    Columns:     emp_id    name    department   salary   exp_yr  |
|                 +-------+----------+-----------+--------+-----+  |
|    Index        |       |          |           |        |     |  |
|    ----->       |  E001 | Alice    |    IT     | 75000  |  5  |  |
|                 |  E002 | Bob      |    HR     | 65000  |  3  |  |
|                 |  E003 | Carol    | Finance   | 72000  |  7  |  |
|                 |  E004 | David    |    IT     | 80000  |  8  |  |
|                 |  E005 | Eva      |Marketing  | 68000  |  4  |  |
|                 +-------+----------+-----------+--------+-----+  |
|                                                                  |
+------------------------------------------------------------------+
```

### ASCII Visualization - Banking Account Distribution

```
+------------------------------------------------------------------+
|              ACCOUNT TYPE DISTRIBUTION                           |
+------------------------------------------------------------------+

Savings    : ████████████████░░░░░░░░░  4 accounts (40%)
Checking  : ██████████░░░░░░░░░░░░░░  3 accounts (30%)
Premium   : ██████████████░░░░░░░░░░  3 accounts (30%)

+------------------------------------------------------------------+
|                    BALANCE DISTRIBUTION                          |
+------------------------------------------------------------------+

$0-$10K   : ████████░░░░░░░░░░░░░░  2 accounts (20%)
$10K-$25K : ██████████████░░░░░░░░░  4 accounts (40%)
$25K-$50K : ████████░░░░░░░░░░░░░  2 accounts (20%)
$50K+     : ████████░░░░░░░░░░░░░  2 accounts (20%)

+------------------------------------------------------------------+
```

### ASCII Visualization - Healthcare Department

```
+------------------------------------------------------------------+
|              PATIENT DISTRIBUTION BY DEPARTMENT                  |
+------------------------------------------------------------------+

Cardiology: ████████████████████████  3 patients (30%)
General   : ████████████████████████  3 patients (30%)
Orthopedics: ██████████████████░░░░░  2 patients (20%)
Neurology : ██████████████████░░░░░░  2 patients (20%)

+------------------------------------------------------------------+
|              AVERAGE STAY DAYS BY DEPARTMENT                    |
+------------------------------------------------------------------+

Cardiology: ██████████████████████████████  7.0 days
General   : ████████████████░░░░░░░░░░░  2.3 days
Orthopedics: ██████████████████████████████████████  7.5 days
Neurology : ██████████████████████████░░░░░  4.5 days

+------------------------------------------------------------------+
```

### ASCII Visualization - Sales Performance

```
+------------------------------------------------------------------+
|              PRODUCT SALES PERFORMANCE                           |
+------------------------------------------------------------------+

Widget A: ████████████████████████████████ 375 units ($7,496)
Widget B: ████████████████████░░░░░░░░░░░░░ 155 units ($4,649)
Widget C: ██████████████░░░░░░░░░░░░░░░░  90 units ($4,499)

+------------------------------------------------------------------+
|              SALES TREND (10-DAY PERIOD)                         |
+------------------------------------------------------------------+

Day 1 ▓▓▓▓▓▓▓▓▓▓░░░░░░░░░ Day 6 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░
Day 2 ▓▓▓▓▓▓░░░░░░░░░░ Day 7 ▓▓▓▓▓▓▓▓▓░░░░░░░
Day 3 ▓▓▓▓▓▓▓▓░░░░░░░░ Day 8 ▓▓▓▓▓▓▓▓▓▓░░░░░░░
Day 4 ▓▓▓▓░░░░░░░░░░░ Day 9 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░░░
Day 5 ▓▓▓▓▓▓▓▓░░░░░░░ Day 10▓▓▓▓▓▓▓▓▓▓▓▓░░░░░
+------------------------------------------------------------------+
```

---

## Advanced Topics

### MultiIndex DataFrames

```python
# Create MultiIndex DataFrame
arrays = [
    np.array(['Banking', 'Banking', 'Banking', 'Healthcare', 'Healthcare', 'Healthcare']),
    np.array(['Accounts', 'Loans', 'Cards', 'Patients', 'Staff', 'Facilities'])
]
index = pd.MultiIndex.from_arrays(arrays, names=['Sector', 'Category'])

data = pd.DataFrame({
    'Count': [1500, 850, 3200, 450, 180, 12],
    'Revenue': [150000, 2500000, 480000, 0, 0, 0],
    'Expenses': [50000, 1200000, 290000, 0, 0, 0]
}, index=index)

print("=== MultiIndex DataFrame ===")
print(data)

# Access MultiIndex level
print("\nBanking Sector:")
print(data.loc['Banking'])
```

### Pivot Tables

```python
# Create sample data for pivot table
sales = pd.DataFrame({
    'quarter': ['Q1', 'Q1', 'Q1', 'Q2', 'Q2', 'Q2', 'Q3', 'Q3', 'Q3', 'Q4', 'Q4', 'Q4'],
    'product': ['A', 'B', 'A', 'B', 'A', 'B', 'A', 'B', 'A', 'B', 'A', 'B'],
    'region': ['North', 'North', 'South', 'South', 'North', 'South', 'North', 'South', 'North', 'South', 'North', 'South'],
    'sales': [1000, 1500, 1200, 800, 1100, 1300, 1400, 900, 1600, 1000, 1800, 1200]
})

# Create pivot table
pivot = pd.pivot_table(sales, values='sales', index='region', columns='quarter', aggfunc='sum')
print("=== Pivot Table ===")
print(pivot)
```

### Advanced Data Selection

```python
# Using query method
employees = pd.DataFrame({
    'name': ['Alice', 'Bob', 'Charlie', 'David'],
    'department': ['IT', 'HR', 'IT', 'Finance'],
    'salary': [75000, 65000, 80000, 72000]
})

# Query for high-salary IT employees
result = employees.query("department == 'IT' and salary > 70000")
print("=== Query Results ===")
print(result)

# Using eval for computations
df = pd.DataFrame({'a': [1, 2, 3], 'b': [4, 5, 6]})
df['c'] = df.eval('a + b')
df['d'] = df.eval('a * b')
print("\n=== Eval Results ===")
print(df)
```

### Categorical Data Type

```python
# Create categorical data
df = pd.DataFrame({
    'name': ['Alice', 'Bob', 'Charlie', 'David'],
    'department': pd.Categorical(['IT', 'HR', 'IT', 'Finance'], 
                                  categories=['IT', 'HR', 'Finance', 'Marketing'])
})

print("=== Categorical Data ===")
print(df['department'].cat.categories)
print(df['department'].cat.codes)
```

---

## Conclusion

### Key Takeaways

This module covered the foundational concepts of Pandas Series and DataFrames:

1. **Series**: One-dimensional labeled arrays that form the building blocks for data manipulation
2. **DataFrames**: Two-dimensional structures that represent tabular data with labeled axes
3. **Data Access**: Methods for selecting, filtering, and modifying data
4. **Operations**: Basic arithmetic, statistical, and transformation operations

### Practical Applications

The concepts learned here apply directly to:
- Banking: Customer account management, loan portfolios, transaction analysis
- Healthcare: Patient records, billing, department management
- Retail: Sales tracking, inventory management, customer analytics

### Next Steps

Continue to the next module: **Data Import and Export** to learn how to read and write data in various formats.

### References

- Pandas Documentation: https://pandas.pydata.org/docs/
- Python Data Science Handbook
- "Python for Data Analysis" by Wes McKinney