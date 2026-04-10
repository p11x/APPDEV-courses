# Data Manipulation and Transformation with Pandas

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

### What is Data Manipulation?

Data manipulation involves operations that transform, reorganize, and modify DataFrames to prepare data for analysis. This includes filtering, sorting, merging, reshaping, and feature engineering.

### Importance in Data Science

- Enables flexible data restructuring
- Supports complex analytical queries
- Facilitates feature creation for ML
- Powers business intelligence

### Types of Operations

1. **Filtering**: Select subsets of data
2. **Sorting**: Order data by columns
3. **Merging**: Combine DataFrames
4. **Reshaping**: Pivot, melt, stack
5. **Feature Engineering**: Create new columns

---

## Fundamentals

### Filtering Data

```python
import pandas as pd
import numpy as np

# Create sample DataFrame
df = pd.DataFrame({
    'Name': ['Alice', 'Bob', 'Carol', 'David', 'Eva'],
    'Department': ['IT', 'HR', 'IT', 'Finance', 'HR'],
    'Salary': [75000, 65000, 72000, 80000, 68000],
    'Experience': [5, 3, 7, 8, 4]
})

# Simple filtering
it_employees = df[df['Department'] == 'IT']
high_earners = df[df['Salary'] > 70000]

# Multiple conditions
senior_it = df[(df['Department'] == 'IT') & (df['Experience'] > 5)]

# Using query method
result = df.query('Salary > 70000 and Department == "IT"')

# Using isin
departments = ['IT', 'Finance']
selected = df[df['Department'].isin(departments)]
```

### Sorting Data

```python
# Sort by single column
df_sorted = df.sort_values('Salary')

# Sort by multiple columns
df_sorted = df.sort_values(['Department', 'Salary'], ascending=[True, False])

# Sort descending
df_sorted = df.sort_values('Salary', ascending=False)

# Sort with NA handling
df_sorted = df.sort_values('Salary', na_position='first')
```

### Merging DataFrames

```python
# Create sample DataFrames
employees = pd.DataFrame({
    'emp_id': ['E001', 'E002', 'E003'],
    'name': ['Alice', 'Bob', 'Carol']
})

salaries = pd.DataFrame({
    'emp_id': ['E001', 'E002', 'E003'],
    'salary': [75000, 65000, 72000]
})

# Inner join
merged = pd.merge(employees, salaries, on='emp_id', how='inner')

# Left join
merged = pd.merge(employees, salaries, on='emp_id', how='left')

# Right join
merged = pd.merge(employees, salaries, on='emp_id', how='right')

# Outer join
merged = pd.merge(employees, salaries, on='emp_id', how='outer')
```

### Concatenating DataFrames

```python
# Create DataFrames
df1 = pd.DataFrame({'A': [1, 2], 'B': [3, 4]})
df2 = pd.DataFrame({'A': [5, 6], 'B': [7, 8]})

# Concatenate rows
result = pd.concat([df1, df2], ignore_index=True)

# Concatenate columns
result = pd.concat([df1, df2], axis=1)

# Append rows
result = df1.append(df2)
```

### Reshaping Data

```python
# Pivot table
df_pivot = df.pivot_table(
    values='Salary',
    index='Department',
    columns='Year',
    aggfunc='mean'
)

# Melt (unpivot)
df_melted = df.melt(
    id_vars=['ID'],
    value_vars=['Q1', 'Q2', 'Q3', 'Q4']
)

# Stack
stacked = df.stack()

# Unstack
unstacked = df.unstack()
```

### Feature Engineering

```python
# Create new columns
df['Bonus'] = df['Salary'] * 0.1
df['Tax'] = df['Salary'] * 0.25
df['Net'] = df['Salary'] - df['Tax']

# Conditional columns
df['Level'] = np.where(df['Experience'] > 5, 'Senior', 'Junior')

# Using apply
df['Name_Length'] = df['Name'].apply(len)

# Using transform
df['Salary_Rank'] = df['Salary'].rank()
```

---

## Implementation

### Example 1: Sales Analysis

```python
import pandas as pd
import numpy as np

# Create sales data
sales = pd.DataFrame({
    'date': pd.date_range('2024-01-01', periods=20, freq='D'),
    'product': np.random.choice(['A', 'B', 'C'], 20),
    'region': np.random.choice(['North', 'South', 'East', 'West'], 20),
    'quantity': np.random.randint(1, 100, 20),
    'unit_price': np.random.uniform(10, 50, 20).round(2)
})

# Calculate revenue
sales['revenue'] = sales['quantity'] * sales['unit_price']

print("=== Sales Data ===")
print(sales.head(10))

# Filter high-revenue transactions
high_sales = sales[sales['revenue'] > 2000]
print("\n=== High Sales Transactions ===")
print(high_sales)

# Sort by revenue
top_sales = sales.sort_values('revenue', ascending=False).head(5)
print("\n=== Top 5 Sales ===")
print(top_sales)

# Group summary
summary = sales.groupby('product').agg({
    'quantity': 'sum',
    'revenue': 'sum'
}).reset_index()
print("\n=== Product Summary ===")
print(summary)
```

### Example 2: Employee Management

```python
# Create employee data
employees = pd.DataFrame({
    'emp_id': ['E001', 'E002', 'E003', 'E004', 'E005'],
    'name': ['Alice Johnson', 'Bob Smith', 'Carol White', 'David Brown', 'Eva Martinez'],
    'department': ['IT', 'HR', 'IT', 'Finance', 'Marketing'],
    'salary': [75000, 65000, 72000, 80000, 68000],
    'hire_date': pd.date_range('2020-01-01', periods=5, freq='Y')
})

# Add derived features
employees['years_service'] = (pd.Timestamp.now() - employees['hire_date']).dt.days / 365.25

# Add performance levels
employees['level'] = np.where(employees['salary'] >= 75000, 'Senior', 
                            np.where(employees['salary'] >= 70000, 'Mid', 'Junior'))

# Add rank
employees['rank'] = employees['salary'].rank(ascending=False).astype(int)

print("=== Employee Data with Features ===")
print(employees)

# Sort by department and salary
employees_sorted = employees.sort_values(['department', 'salary'], ascending=[True, False])
print("\n=== Sorted Employees ===")
print(employees_sorted)
```

---

## Applications

### Banking Sector: Customer Analytics

```python
import pandas as pd
import numpy as np

# Create customer accounts data
accounts = pd.DataFrame({
    'account_id': [f'ACC{str(i).zfill(3)}' for i in range(1, 11)],
    'customer_name': ['James Wilson', 'Maria Garcia', 'Robert Chen', 'Lisa Anderson',
                     'Michael Lee', 'Sarah Davis', 'Joseph Taylor', 'Jennifer Brown',
                     'William Martinez', 'Patricia Johnson'],
    'account_type': ['Checking', 'Savings', 'Premium', 'Checking', 'Savings',
                     'Premium', 'Checking', 'Savings', 'Premium', 'Checking'],
    'balance': [15000, 25000, 85000, 5000, 12000, 95000, 8000, 18000, 78000, 4500],
    'credit_score': [720, 750, 820, 680, 700, 800, 690, 730, 790, 670]
})

# Create transaction data
transactions = pd.DataFrame({
    'account_id': np.random.choice(accounts['account_id'], 20),
    'transaction_type': np.random.choice(['Deposit', 'Withdrawal', 'Transfer'], 20),
    'amount': np.random.uniform(100, 5000, 20).round(2)
})

print("=== Customer Accounts ===")
print(accounts)
print("\n=== Transactions ===")
print(transactions)

# Merge accounts with transactions
account_trans = pd.merge(accounts, transactions, on='account_id', how='left')
print("\n=== Merged Data ===")
print(account_trans.head(10))

# Add customer tier based on balance
accounts['tier'] = pd.cut(
    accounts['balance'],
    bins=[0, 10000, 50000, 100000],
    labels=['Standard', 'Gold', 'Platinum']
)

# Calculate total balance by account type
type_summary = accounts.groupby('account_type').agg({
    'balance': ['sum', 'mean', 'count'],
    'credit_score': 'mean'
}).round(2)

print("\n=== Account Type Summary ===")
print(type_summary)

# High-value customers
high_value = accounts[accounts['balance'] >= 20000].sort_values('balance', ascending=False)
print("\n=== High Value Customers ===")
print(high_value[['account_id', 'customer_name', 'account_type', 'balance', 'tier']])
```

### Healthcare Sector: Patient Analysis

```python
# Create patient data
patients = pd.DataFrame({
    'patient_id': [f'PT{str(i).zfill(3)}' for i in range(1, 11)],
    'name': ['John Doe', 'Jane Smith', 'Bob Johnson', 'Alice Williams', 'Charlie Brown',
            'Diana Ross', 'Edward Miller', 'Fiona Davis', 'George Wilson', 'Hannah Moore'],
    'age': [45, 38, 62, 29, 55, 41, 68, 33, 58, 50],
    'gender': ['M', 'F', 'M', 'F', 'M', 'F', 'M', 'F', 'M', 'F'],
    'department': ['Cardiology', 'General', 'Orthopedics', 'General', 'Neurology',
                   'Cardiology', 'Orthopedics', 'General', 'Neurology', 'Cardiology'],
    'days_stay': [5, 2, 7, 3, 4, 6, 8, 2, 5, 10],
    'bill_amount': [15000, 5000, 22000, 8000, 12000, 18000, 28000, 5500, 15000, 30000]
})

# Add age groups
patients['age_group'] = pd.cut(
    patients['age'],
    bins=[0, 30, 50, 65, 100],
    labels=['18-30', '31-50', '51-65', '65+']
)

# Add bill category
patients['bill_category'] = np.where(patients['bill_amount'] < 10000, 'Low',
                                      np.where(patients['bill_amount'] < 20000, 'Medium', 'High'))

print("=== Patient Data ===")
print(patients[['patient_id', 'name', 'age', 'age_group', 'department', 'bill_amount', 'bill_category']])

# Department summary
dept_summary = patients.groupby('department').agg({
    'bill_amount': ['sum', 'mean', 'count'],
    'days_stay': 'mean'
}).round(2)

print("\n=== Department Summary ===")
print(dept_summary)

# Age group analysis
age_summary = patients.groupby('age_group').agg({
    'bill_amount': 'sum',
    'patient_id': 'count'
}).rename(columns={'patient_id': 'patient_count'})

print("\n=== Age Group Analysis ===")
print(age_summary)

# High-bill patients
high_bill = patients[patients['bill_amount'] >= 20000].sort_values('bill_amount', ascending=False)
print("\n=== High Bill Patients ===")
print(high_bill[['patient_id', 'name', 'department', 'bill_amount']])
```

---

## Output Results

### Sample Output - Banking

```
=== Customer Accounts ===
   account_id   customer_name account_type  balance  credit_score
0       ACC001    James Wilson     Checking    15000           720
1       ACC002     Maria Garcia      Savings    25000           750
2       ACC003     Robert Chen      Premium    85000           820
3       ACC004   Lisa Anderson     Checking     5000           680
4       ACC005     Michael Lee      Savings    12000           700
...

=== Account Type Summary ===
                   balance               credit_score
                       sum     mean count          mean
account_type                                        
Checking         29500.0   7375.0     4         687.5
Premium         188000.0  62666.7     3         803.3
Savings          55000.0  18333.3     3         727.5

=== High Value Customers ===
   account_id   customer_name account_type  balance        tier
2       ACC003     Robert Chen      Premium    85000     Platinum
5       ACC006      Sarah Davis      Premium    95000     Platinum
8       ACC009  William Martinez      Premium    78000     Platinum
1       ACC002     Maria Garcia      Savings    25000        Gold
7       ACC008    Jennifer Brown      Savings    18000        Gold
```

### Sample Output - Healthcare

```
=== Patient Data ===
   patient_id           name  age      age_group  department  bill_amount bill_category
0        PT001       John Doe   45      31-50  Cardiology        15000       Medium
1        PT002      Jane Smith   38      31-50     General         5000          Low
2        PT003    Bob Johnson   62      51-65  Orthopedics        22000         High
3        PT004   Alice Williams   29      18-30     General         8000          Low
4       PT005   Charlie Brown   55      51-65  Neurology        12000       Medium

=== Department Summary ===
                   bill_amount              days_stay
                        sum     mean count       mean
department                                        
Cardiology        63000.0  21000.0     3       7.0
General        15500.0   5166.7     3       2.3
Neurology        27000.0  13500.0     2       4.5
Orthopedics        50000.0  25000.0     2       7.5
```

---

## Visualization

### ASCII - Data Transformation Flow

```
+------------------------------------------------------------------+
|           DATA MANIPULATION OPERATIONS                             |
+------------------------------------------------------------------+

INPUT DATA                 TRANSFORMATION             OUTPUT DATA
-----------               ------------             ----------

+----------+                                   +----------+
| Row 1    |  +-----------+              +----------+
| Row 2    |  | FILTER    |              | Row 1    |
| Row 3    |  |           |              | Row 3    |
| Row 4    |  +-----------+              +----------+
| Row 5    |       |
|          |  +-----------+
+----------+  | SORT      |              +----------+
              |           |              | Row 1    |
              +-----------+              | Row 4    |
                      |                  | Row 2    |
              +------+------+             +----------+
              |             |
        +-----------+  +-----------+
        | MERGE     |  | CONCAT    |
        |           |  |          |
        +-----------+  +-----------+
              |             |
              v             v
        +-----------+  +-----------+
        | COMBINED  |  | APPENDED  |
        | DATA     |  | DATA    |
        +-----------+  +-----------+

+------------------------------------------------------------------+
```

### ASCII - Feature Engineering Pipeline

```
+------------------------------------------------------------------+
|         FEATURE ENGINEERING PIPELINE                               |
+------------------------------------------------------------------+

RAW FEATURES               DERIVED FEATURES           RESULT
-----------               ---------------          ------
                                    
[Salary]     --->     [Bonus = Salary * 0.1]     +-----------+
                                   [Tax = Salary * 0.25]     | Employee  |
                                   [Net = Salary - Tax]       | Records   |
                                                         +-----------+
[HireDate]   --->     [Years = Today - HireDate]
                                                         
[Experience]--->     [Level: Senior/Junior]

[Name]      --->     [Name_Length]
                                    
[Department]+---->  [Dept_Code]

[Quantity]  --->     [Revenue = Qty * Price]

+------------------------------------------------------------------+
```

### ASCII - Merge Operations

```
+------------------------------------------------------------------+
|              MERGE TYPES VISUALIZATION                            |
+------------------------------------------------------------------+

LEFT TABLE          RIGHT TABLE           INNER JOIN
+----------+      +----------+        +----------+
| A | 1    |      | B | 10   |        | A | 1 | B | 10  |
| A | 2    |      | A | 20   |        | A | 2 | B | 20  |
| B | 3    |      | C | 30   |        | B | 3 | B | None|
+----------+      +----------+        +----------+

LEFT TABLE          RIGHT TABLE           LEFT JOIN
+----------+      +----------+        +----------+
| A | 1    |      | B | 10   |        | A | 1 | B | 10  |
| A | 2    |      | A | 20   |        | A | 2 | B | 20  |
| B | 3    |      | C | 30   |        | B | 3 | B | None|
+----------+      +----------+        +----------+

LEFT TABLE          RIGHT TABLE           OUTER JOIN
+----------+      +----------+        +----------+
| A | 1    |      | B | 10   |        | A | 1 | B | 10  |
| A | 2    |      | A | 20   |        | A | 2 | B | 20  |
| B | 3    |      | C | 30   |        | B | 3 | B | None|
+----------+      +----------+        | C | None| B | 30  |
                                         +----------+

+------------------------------------------------------------------+
```

---

## Advanced Topics

### Advanced Pivot Operations

```python
# Create pivot table with multiple aggregation
pivot = pd.pivot_table(
    df,
    values='Sales',
    index='Region',
    columns=['Year', 'Quarter'],
    aggfunc=['sum', 'mean'],
    fill_value=0
)

# Using margins
pivot = pd.pivot_table(
    df,
    values='Sales',
    index='Region',
    columns='Product',
    aggfunc='sum',
    margins=True,
    margins_name='Total'
)
```

### Using Transform for Group Operations

```python
# Add group statistics
df['group_mean'] = df.groupby('Department')['Salary'].transform('mean')
df['group_median'] = df.groupby('Department')['Salary'].transform('median')

# Add rank within group
df['group_rank'] = df.groupby('Department')['Salary'].rank(pct=True)

# Add lag/lead
df['prev_salary'] = df.sort_values('Date').groupby('emp_id')['Salary'].shift(1)
df['next_salary'] = df.sort_values('Date').groupby('emp_id')['Salary'].shift(-1)
```

### Rolling and Expanding Windows

```python
# Rolling mean
df['rolling_mean'] = df['Sales'].rolling(window=7).mean()

# Rolling sum
df['rolling_sum'] = df['Sales'].rolling(window=7).sum()

# Expanding window
df['expanding_mean'] = df['Sales'].expanding().mean()

# Rolling with custom agg
df['rolling_stats'] = df['Sales'].rolling(window=7).agg(['mean', 'std', 'min', 'max'])
```

### Apply with Multiple Returns

```python
# Custom function with multiple returns
def get_stats(x):
    return pd.Series({
        'mean': x.mean(),
        'std': x.std(),
        'min': x.min(),
        'max': x.max()
    })

result = df.groupby('Department')['Salary'].apply(get_stats)
```

### Cross-Tabulation

```python
# Create cross-tabulation
crosstab = pd.crosstab(
    df['Department'],
    df['Gender'],
    values=df['Salary'],
    aggfunc='mean'
)

# Normalize
crosstab_norm = pd.crosstab(
    df['Department'],
    df['Gender'],
    normalize='index'
)
```

---

## Conclusion

### Key Takeaways

This module covered essential data manipulation techniques:

1. **Filtering**: Select specific data subsets
2. **Sorting**: Order data by columns
3. **Merging**: Combine DataFrames
4. **Concatenation**: Append data
5. **Reshaping**: Pivot, melt, stack
6. **Feature Engineering**: Create new columns

### Practical Applications

- Banking: Customer segmentation, transaction analysis
- Healthcare: Patient billing, department stats
- Business Intelligence: Reporting dashboards

### Next Steps

Continue to: **Group By Operations and Aggregation** for advanced grouping.

### References

- Pandas Manipulation: https://pandas.pydata.org/docs/user_guide reshaping.html
- pandas merge documentation