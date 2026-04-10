# Group By Operations and Aggregation with Pandas

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

### What is Group By?

Group By is a powerful operation that splits data into groups based on criteria, applies functions to each group, and combines results. It's essential for summary statistics and segmented analysis.

### Why Use Group By?

- Generate summary statistics by category
- Compare segments across multiple dimensions
- Identify patterns in data
- Create aggregated reports

### Aggregation Functions

Pandas supports many aggregation functions:
- `sum()`, `mean()`, `median()`
- `min()`, `max()`, `std()`, `var()`
- `count()`, `nunique()`
- `first()`, `last()`

---

## Fundamentals

### Basic GroupBy

```python
import pandas as pd
import numpy as np

# Create sample data
df = pd.DataFrame({
    'Department': ['IT', 'HR', 'IT', 'Finance', 'HR', 'Finance'],
    'Employee': ['Alice', 'Bob', 'Carol', 'David', 'Eva', 'Frank'],
    'Salary': [75000, 65000, 72000, 80000, 68000, 75000],
    'Performance': [4.5, 4.0, 4.2, 4.8, 3.9, 4.3]
})

# Simple groupby
grouped = df.groupby('Department')
print(grouped.size())

# Group and aggregate
dept_sum = df.groupby('Department')['Salary'].sum()
print(dept_sum)

# Multiple aggregations
dept_stats = df.groupby('Department').agg({
    'Salary': ['sum', 'mean', 'count'],
    'Performance': 'mean'
})
print(dept_stats)
```

### GroupBy with Multiple Columns

```python
# Group by multiple columns
grouped = df.groupby(['Department', 'Employee'])

# Multi-level aggregation
result = df.groupby(['Department']).agg({
    'Salary': ['sum', 'mean', 'min', 'max'],
    'Performance': 'mean'
}).round(2)
```

### Using Agg with String Functions

```python
# String aggregation
result = df.groupby('Department')['Employee'].agg(
    lambda x: ', '.join(x)
)

# Named aggregations
result = df.groupby('Department').agg(
    total_salary=('Salary', 'sum'),
    avg_salary=('Salary', 'mean'),
    employee_count=('Employee', 'count')
)
```

### Filter After GroupBy

```python
# Filter groups
def filter_func(x):
    return x['Salary'].sum() > 70000

filtered = df.groupby('Department').filter(filter_func)
```

### Transform with GroupBy

```python
# Add group statistics as new column
df['group_mean'] = df.groupby('Department')['Salary'].transform('mean')
df['group_std'] = df.groupby('Department')['Salary'].transform('std')

# Rank within groups
df['salary_rank'] = df.groupby('Department')['Salary'].rank(ascending=False)
```

---

## Implementation

### Example 1: Sales Analysis

```python
# Create sales data
sales = pd.DataFrame({
    'date': pd.date_range('2024-01-01', periods=30),
    'region': np.random.choice(['North', 'South', 'East', 'West'], 30),
    'product': np.random.choice(['Product A', 'Product B', 'Product C'], 30),
    'salesperson': np.random.choice(['John', 'Mary', 'Tom', 'Alice'], 30),
    'quantity': np.random.randint(1, 50, 30),
    'revenue': np.random.uniform(100, 5000, 30).round(2)
})

print("=== Sales Data ===")
print(sales.head(10))

# Aggregate by region
region_summary = sales.groupby('region').agg({
    'quantity': 'sum',
    'revenue': 'sum',
    'salesperson': 'count'
}).rename(columns={'salesperson': 'num_transactions'})

region_summary = region_summary.sort_values('revenue', ascending=False)
print("\n=== Regional Summary ===")
print(region_summary)

# Aggregate by product
product_summary = sales.groupby('product').agg({
    'quantity': ['sum', 'mean'],
    'revenue': ['sum', 'mean']
}).round(2)
print("\n=== Product Summary ===")
print(product_summary)

# Aggregate by salesperson
seller_summary = sales.groupby('salesperson').agg({
    'revenue': 'sum',
    'quantity': 'sum',
    'product': 'nunique'
}).rename(columns={'product': 'products_sold'})
print("\n=== Salesperson Summary ===")
print(seller_summary)
```

### Example 2: Employee Performance

```python
# Create employee data
employees = pd.DataFrame({
    'department': ['IT', 'HR', 'IT', 'Finance', 'HR', 'Finance', 'IT', 'HR'],
    'team': ['Dev', 'Recruit', 'Dev', 'Account', 'Recruit', 'Account', 'QA', 'Payroll'],
    'employee': ['Alice', 'Bob', 'Carol', 'David', 'Eva', 'Frank', 'Grace', 'Henry'],
    'salary': [75000, 65000, 72000, 80000, 68000, 75000, 70000, 62000],
    'performance': [4.5, 4.0, 4.2, 4.8, 3.9, 4.3, 4.1, 3.8],
    'projects': [5, 3, 4, 7, 3, 5, 4, 2]
})

# Department + Team aggregation
dept_team = employees.groupby(['department', 'team']).agg({
    'salary': 'mean',
    'performance': 'mean',
    'employee': 'count'
}).round(2).rename(columns={'employee': 'headcount'})

print("=== Department + Team Summary ===")
print(dept_team)

# Top performers by department
top_performers = employees.loc[
    employees.groupby('department')['performance'].idxmax()
]
print("\n=== Top Performer by Department ===")
print(top_performers[['department', 'employee', 'performance']])
```

---

## Applications

### Banking Sector: Account Analysis

```python
import pandas as pd
import numpy as np

# Create account transactions
accounts = pd.DataFrame({
    'account_id': ['ACC001', 'ACC001', 'ACC002', 'ACC002', 'ACC002', 'ACC003', 'ACC003'],
    'transaction_type': ['Deposit', 'Withdrawal', 'Deposit', 'Transfer', 'Withdrawal', 'Deposit', 'Payment'],
    'amount': [5000, 500, 3000, 1000, 200, 15000, 3500],
    'date': pd.date_range('2024-01-01', periods=7),
    'category': ['Salary', 'ATM', 'Salary', 'Transfer', 'ATM', 'Wire', 'Bill']
})

print("=== Transactions ===")
print(accounts)

# Account summary
account_summary = accounts.groupby('account_id').agg({
    'amount': ['sum', 'mean', 'count', 'std'],
    'transaction_type': 'first'
}).round(2)

print("\n=== Account Summary ===")
print(account_summary)

# Transaction type analysis
type_summary = accounts.groupby(['account_id', 'transaction_type']).agg({
    'amount': 'sum'
}).unstack(fill_value=0)

print("\n=== Transaction Types by Account ===")
print(type_summary)

# Create customer segments
customers = pd.DataFrame({
    'customer_id': [f'C{str(i).zfill(3)}' for i in range(1, 11)],
    'segment': np.random.choice(['Premium', 'Standard', 'Basic'], 10),
    'region': np.random.choice(['North', 'South', 'East', 'West'], 10),
    'balance': np.random.uniform(1000, 100000, 10).round(2),
    'credit_score': np.random.randint(600, 850, 10)
})

print("\n=== Customer Segments ===")
print(customers)

# Segment analysis
segment_summary = customers.groupby('segment').agg({
    'balance': ['sum', 'mean', 'count'],
    'credit_score': 'mean'
}).round(2)

print("\n=== Segment Analysis ===")
print(segment_summary)

# Region + Segment analysis
region_segment = customers.groupby(['region', 'segment']).agg({
    'balance': 'sum',
    'customer_id': 'count'
}).rename(columns={'customer_id': 'count'}).sort_values('balance', ascending=False)

print("\n=== Region + Segment ===")
print(region_segment)
```

### Healthcare Sector: Billing Analysis

```python
# Create patient billing data
patients = pd.DataFrame({
    'patient_id': [f'PT{str(i).zfill(3)}' for i in range(1, 11)],
    'department': ['Cardiology', 'General', 'Orthopedics', 'General', 'Neurology',
                  'Cardiology', 'Orthopedics', 'General', 'Neurology', 'Cardiology'],
    'diagnosis': ['Hypertension', 'Flu', 'Fracture', 'Infection', 'Migraine',
                'Arrhythmia', 'Arthritis', 'Viral Infection', 'Epilepsy', 'Heart Failure'],
    'days_stay': [5, 2, 7, 3, 4, 6, 8, 2, 5, 10],
    'room_charge': [2500, 1000, 3500, 1500, 2000, 3000, 4000, 1000, 2500, 5000],
    'medical_charge': [6000, 2250, 12500, 4000, 7500, 15000, 16000, 2800, 9000, 22000],
    'medication_charge': [1750, 600, 2900, 1000, 1400, 2800, 3200, 750, 1800, 4500]
})

# Calculate total bill
patients['total_bill'] = patients['room_charge'] + patients['medical_charge'] + patients['medication_charge']

print("=== Patient Billing ===")
print(patients[['patient_id', 'department', 'days_stay', 'total_bill']])

# Department analysis
dept_billing = patients.groupby('department').agg({
    'total_bill': ['sum', 'mean', 'count'],
    'days_stay': 'mean'
}).round(2)

print("\n=== Department Billing ===")
print(dept_billing)

# Diagnosis analysis
diagnosis_billing = patients.groupby('diagnosis').agg({
    'total_bill': 'max',
    'days_stay': 'max'
}).sort_values('total_bill', ascending=False)

print("\n=== Diagnosis by Bill (Sorted) ===")
print(diagnosis_billing)

# Daily charge rate
patients['daily_rate'] = (patients['total_bill'] / patients['days_stay']).round(2)

# Department daily rate analysis
daily_rate_summary = patients.groupby('department').agg({
    'daily_rate': 'mean',
    'days_stay': 'mean'
}).round(2)

print("\n=== Daily Rate by Department ===")
print(daily_rate_summary)
```

---

## Output Results

### Sample Output - Banking

```
=== Transactions ===
  account_id transaction_type  amount        date    category
0     ACC001         Deposit   5000.0 2024-01-01      Salary
1     ACC001      Withdrawal    500.0 2024-01-02        ATM
2     ACC002         Deposit   3000.0 2024-01-03      Salary
...

=== Account Summary ===
                 amount
                   sum    mean count    std
account_id
ACC001          5500.0 2750.0     2 3181.98
ACC003         18500.09250.0     2 7781.29
ACC002          4200.0 1400.0     3 1400.00

=== Segment Analysis ===
                balance                    credit_score
                    sum     mean count          mean
segment
Basic          89250.0  8925.0     3       705.0
Premium      385000.0  55000.0     7       775.3
Standard     17500.0 17500.0     1       720.0
```

### Sample Output - Healthcare

```
=== Department Billing ===
                total_bill             days_stay
                   sum    mean count       mean
department
Cardiology     84300.0 21075.0     4       6.75
General        23200.0  5800.0     4       2.50
Neurology     32300.0 16150.0     2       4.50
Orthopedics    39700.0 19850.0     2       7.50

=== Daily Rate by Department ===
              daily_rate  days_stay
department
Cardiology    3285.71      6.75
General      2320.00      2.50
Neurology    3588.89      4.50
Orthopedics   2646.67      7.50
```

---

## Visualization

### ASCII - GroupBy Process

```
+------------------------------------------------------------------+
|              GROUP BY OPERATION FLOW                              |
+------------------------------------------------------------------+

INPUT DATA                                         GROUP BY RESULT
-----------                                         ------------

+----------+                                       +----------+
| Dept | Sal|    +-----------+                | Dept     | SALARY  |
| IT   | 75 |    |  SPLIT    |                | --------------------|
| HR   | 65 | --->|  by Dept  |-------------> | IT      |  217000 |
| IT   | 72 |    |           |                | HR      |  133000 |
| Fin  | 80 |    +-----------+                | Fin     |  155000 |
| HR   | 68 |           |                    +----------+
| Fin  | 75 |           |
                 | +-----------+
                 | |  AGG     |
                 | | (sum)    |
                 +-----------+
                       |
                       v
                 +-----------+
                 | COMBINE   |
                 | RESULTS  |
                 +-----------+

+------------------------------------------------------------------+
```

### ASCII - Aggregation Functions

```
+------------------------------------------------------------------+
|           AGGREGATION FUNCTIONS                                   |
+------------------------------------------------------------------+

AGGREGATION       FUNCTION          EXAMPLE OUTPUT
-----------      ---------       -------------
[SUM]          Add values       +-----------+
                               | Dept  | Total |
                               | IT    | 217K  |
                               +-------+-------+

[MEAN]          Average        +-----------+
                               | Dept  | Avg   |
                               | IT    | 72.3K |
                               +-------+-------+

[COUNT]         Count rows     +-----------+
                               | Dept  | Count |
                               | IT    |   3   |
                               +-------+-------+

[MIN/MAX]       Range          +-----------+
                               | Dept  | Min/Max|
                               | IT    | 65/80 |
                               +-------+-------+

[STD]           Spread        +-----------+
                               | Dept  | Std   |
                               | IT    | 5.03 |
                               +-------+-------+

[AGG]           Custom        +-----------+
                               | Dept  | Mean, |
                               |       | Sum,  |
                               |       | Count|
                               +-------+-------+

+------------------------------------------------------------------+
```

### ASCII - Banking Analysis

```
+------------------------------------------------------------------+
|         BANKING CUSTOMER SEGMENTS                                  |
+------------------------------------------------------------------+

PREMIUM CUSTOMERS:         STANDARD:          BASIC:
+--------------------+ +-------------+    +-----------+
| Balance > $50,000   | | $10K-$50K  |    | < $10K   |
| Credit Score > 750   | | 650-750    |    | < 650    |
| High activity      | | Medium     |    | Low       |
+--------------------+ +-------------+    +-----------+

REGIONAL DISTRIBUTION:
North:     ████████████████░░  40%
South:     ████████████░░░░░  30%
East:      ██████████░░░░░░░  20%
West:      ██████████░░░░░░░  20%

ACCOUNT TYPES BY SEGMENT:
| Premium | Standard | Basic
|--------|----------|-------
|Savings | ██████████████████ | 
|CHECK   | ██████████████    | ████
|PREMIUM | ██████████       |░░

+------------------------------------------------------------------+
```

---

## Advanced Topics

### Multi-Index Aggregation

```python
# Multi-index groupby
result = df.groupby(['Year', 'Quarter', 'Region']).agg({
    'Sales': 'sum',
    'Customers': 'nunique'
})

# Using Named Aggregation
result = df.groupby('Department').agg(
    total_salary=('Salary', 'sum'),
    avg_performance=('Performance', 'mean'),
    employee_count=('Employee', 'count')
)
```

### Custom Aggregation Functions

```python
# Custom aggregation
def weighted_avg(x):
    return np.average(x, weights=df.loc[x.index, 'weight'])

result = df.groupby('group').agg({
    'value': weighted_avg
})

# Using lambda
result = df.groupby('group').agg({
    'Salary': [('min_salary', 'min'), ('max_salary', 'max')]
})
```

### Rolling GroupBy

```python
# Rolling groupby
result = df.groupby('group')['value'].rolling(window=3).sum()

# Expanding groupby
result = df.groupby('group')['value'].expanding().mean()
```

### Pipe with GroupBy

```python
# Chain operations
result = (df.groupby('group')
    .filter(lambda x: x['value'].sum() > 100)
    .groupby('group')
    .agg({'value': 'sum'})
)
```

### Cross-Tabulation

```python
# Cross-tabulation
crosstab = pd.crosstab(
    df['Department'],
    df['Gender'],
    values=df['Salary'],
    aggfunc='mean'
).round(2)
```

---

## Conclusion

### Key Takeaways

This module covered group by operations:

1. **Basic GroupBy**: Single column grouping
2. **Multi-Column**: Multiple column aggregation
3. **Aggregation Functions**: Built-in and custom
4. **Transform**: Add group statistics
5. **Filter**: Remove groups based on criteria

### Practical Applications

- Banking: Customer segmentation, transaction analysis
- Healthcare: Department billing, diagnosis patterns
- Analytics: Business intelligence reporting

### Next Steps

Continue to: **Time Series Analysis with Pandas** for date handling.

### References

- Pandas GroupBy: https://pandas.pydata.org/docs/user_guide/groupby.html
- Aggregation documentation