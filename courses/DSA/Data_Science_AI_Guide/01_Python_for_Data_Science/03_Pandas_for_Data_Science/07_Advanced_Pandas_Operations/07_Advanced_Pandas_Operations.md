# Advanced Pandas Operations

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

### What are Advanced Operations?

Advanced Pandas operations cover power techniques for complex data manipulation, performance optimization, and sophisticated analysis that go beyond basic DataFrame operations.

### Why Advanced Operations?

- Handle complex data scenarios
- Optimize performance for large datasets
- Create custom transformations
- Implement complex business logic
- Build reusable data pipelines

### Topics Covered

1. **Method Chaining**: Functional data pipelines
2. **Custom Functions**: Apply and transform
3. **Performance**: Large data handling
4. **Pipes**: DataFrame pipelines
5. **Advanced Indexing**: Complex selections

---

## Fundamentals

### Method Chaining

```python
import pandas as pd
import numpy as np

# Create sample DataFrame
df = pd.DataFrame({
    'A': [1, 2, 3, 4, 5],
    'B': [10, 20, 30, 40, 50],
    'C': ['x', 'y', 'x', 'y', 'x']
})

# Method chaining example
result = (df
    .assign(D=lambda x: x.A * x.B)
    .query('D > 10')
    .sort_values('D', ascending=False)
    .reset_index(drop=True)
)
print(result)
```

### Apply Custom Functions

```python
# Apply to single column
df['A_squared'] = df['A'].apply(lambda x: x ** 2)

# Apply to row
df['row_sum'] = df.apply(lambda row: row['A'] + row['B'], axis=1)

# Apply with aggregation
def custom_agg(x):
    return pd.Series({
        'sum': x.sum(),
        'mean': x.mean(),
        'max': x.max()
    })

result = df.groupby('C').apply(custom_agg)
```

### Using Pipe

```python
# Define custom functions
def add_ten(x):
    x['A'] = x['A'] + 10
    return x

def multiply(x, factor):
    x['A'] = x['A'] * factor
    return x

# Chain with pipe
result = (df
    .pipe(add_ten)
    .pipe(multiply, factor=2)
)
```

### Vectorized Conditionals

```python
# Using np.where
df['new_col'] = np.where(df['A'] > 3, 'High', 'Low')

# Using np.select
conditions = [
    df['A'] < 2,
    df['A'] < 4,
    df['A'] >= 4
]
choices = ['Low', 'Medium', 'High']
df['level'] = np.select(conditions, choices)

# Using pandas where
df['filtered'] = df['A'].where(df['A'] > 2, 0)
```

### Advanced Mapping

```python
# Map values
mapping = {'x': 'X', 'y': 'Y'}
df['C_mapped'] = df['C'].map(mapping)

# Replace values
df['C_replaced'] = df['C'].replace({'x': 'X', 'y': 'Y'})

# Using applymap
df = pd.DataFrame(np.random.randn(3, 3))
result = df.applymap(lambda x: f'{x:.2%}')
```

---

## Implementation

### Example 1: Complex Data Pipeline

```python
import pandas as pd
import numpy as np

# Create complex employee data
employees = pd.DataFrame({
    'emp_id': range(1, 11),
    'department': np.random.choice(['IT', 'HR', 'Finance', 'Marketing'], 10),
    'salary': np.random.randint(50000, 150000, 10),
    'experience': np.random.randint(1, 15, 10),
    'performance': np.random.uniform(2.5, 5.0, 10),
    'projects': np.random.randint(1, 10, 10)
})

print("=== Original Data ===")
print(employees)

# Create complex pipeline
pipeline = (employees
    # Add derived features
    .assign(
        salary_grade=np.where(employees['salary'] < 70000, 'Entry',
                     np.where(employees['salary'] < 100000, 'Mid', 'Senior')),
        experience_level=np.where(employees['experience'] < 3, 'Junior',
                               np.where(employees['experience'] < 7, 'Mid', 'Senior')),
        performance_rating=np.where(employees['performance'] >= 4.5, 'Excellent',
                                  np.where(employees['performance'] >= 3.5, 'Good', 'Needs Improvement'))
    )
    # Filter
    .query('salary > 50000')
    # Sort
    .sort_values(['salary', 'experience'], ascending=[False, False])
    # Reset index
    .reset_index(drop=True)
)

print("\n=== Processed Data ===")
print(pipeline)

# Calculate summary statistics
summary = pipeline.groupby('department').agg({
    'salary': ['mean', 'count'],
    'experience': 'mean',
    'performance': 'mean'
}).round(2)

print("\n=== Summary by Department ===")
print(summary)
```

### Example 2: Financial Calculations

```python
# Create financial data
fin_data = pd.DataFrame({
    'date': pd.date_range('2024-01-01', periods=100),
    'revenue': np.cumsum(np.random.randn(100) * 1000 + 5000),
    'costs': np.cumsum(np.random.randn(100) * 800 + 3000),
    'units_sold': np.random.randint(100, 500, 100)
})

fin_data.set_index('date', inplace=True)

print("=== Financial Data ===")
print(fin_data.head(10))

# Add calculated columns using assign
processed = (fin_data
    .assign(
        profit=fin_data['revenue'] - fin_data['costs'],
        margin=lambda x: (x['profit'] / x['revenue'] * 100).round(2),
        profit_per_unit=lambda x: (x['profit'] / x['units_sold']).round(2),
        revenue_ma_7=lambda x: x['revenue'].rolling(window=7).mean(),
        revenue_ma_30=lambda x: x['revenue'].rolling(window=30).mean()
    )
)

print("\n=== Processed Financial Data ===")
print(processed.head(35))
```

---

## Applications

### Banking Sector: Risk Analysis

```python
import pandas as pd
import numpy as np

# Create loan portfolio
loans = pd.DataFrame({
    'loan_id': [f'LN{str(i).zfill(4)}' for i in range(1, 21)],
    'customer_id': [f'C{str(i).zfill(3)}' for i in np.random.randint(1, 15, 20)],
    'loan_type': np.random.choice(['Mortgage', 'Personal', 'Auto', 'Business'], 20),
    'principal': np.random.uniform(10000, 500000, 20).round(2),
    'interest_rate': np.random.uniform(3, 12, 20).round(2),
    'term_months': np.random.choice([36, 48, 60, 120, 240, 360], 20),
    'credit_score': np.random.randint(600, 850, 20),
    'status': np.random.choice(['Current', 'Delinquent', 'Paid'], 20, p=[0.7, 0.2, 0.1])
})

print("=== Loan Portfolio ===")
print(loans)

# Financial calculations
loans_calc = (loans
    .assign(
        monthly_rate=lambda x: x['interest_rate'] / 100 / 12,
        monthly_payment=lambda x: x['principal'] * x['monthly_rate'] * 
            (1 + x['monthly_rate']) ** x['term_months'] / 
            ((1 + x['monthly_rate']) ** x['term_months'] - 1),
        total_interest=lambda x: (x['monthly_payment'] * x['term_months'] - x['principal']).round(2),
        total_cost=lambda x: (x['monthly_payment'] * x['term_months']).round(2),
        risk_category=np.where(loans['credit_score'] >= 750, 'Low',
                       np.where(loans['credit_score'] >= 700, 'Medium', 'High'))
    )
)

print("\n=== Loans with Calculations ===")
print(loans_calc[['loan_id', 'principal', 'monthly_payment', 'total_interest', 'risk_category']])

# Risk analysis by category
risk_summary = loans_calc.groupby('risk_category').agg({
    'principal': ['sum', 'mean', 'count'],
    'interest_rate': 'mean',
    'credit_score': 'mean'
}).round(2)

print("\n=== Risk Summary ===")
print(risk_summary)

# Loan type analysis
type_summary = loans_calc.groupby('loan_type').agg({
    'principal': 'sum',
    'monthly_payment': 'sum',
    'loan_id': 'count'
}).round(2)

print("\n=== Loan Type Summary ===")
print(type_summary)
```

### Healthcare Sector: Clinical Analysis

```python
# Create patient clinical data
patients = pd.DataFrame({
    'patient_id': [f'PT{str(i).zfill(3)}' for i in range(1, 21)],
    'age': np.random.randint(18, 85, 20),
    'bmi': np.random.uniform(18, 45, 20).round(1),
    'blood_pressure_systolic': np.random.randint(90, 180, 20),
    'blood_pressure_diastolic': np.random.randint(60, 110, 20),
    'heart_rate': np.random.randint(50, 110, 20),
    'cholesterol': np.random.randint(150, 300, 20),
    'glucose': np.random.randint(70, 140, 20),
    'diagnosis': np.random.choice(['Healthy', 'Pre-Diabetic', 'Diabetic', 'Hypertensive'], 20)
})

print("=== Patient Clinical Data ===")
print(patients)

# Clinical calculations
clinical = (patients
    .assign(
        # Blood pressure category
        bp_category=lambda x: np.select(
            [
                (x['blood_pressure_systolic'] < 120) & (x['blood_pressure_diastolic'] < 80),
                (x['blood_pressure_systolic'] < 130) & (x['blood_pressure_diastolic'] < 80),
                (x['blood_pressure_systolic'] < 140) | (x['blood_pressure_diastolic'] < 90),
                (x['blood_pressure_systolic'] >= 140) | (x['blood_pressure_diastolic'] >= 90)
            ],
            ['Normal', 'Elevated', 'High Stage 1', 'High Stage 2'],
            default='Unknown'
        ),
        # BMI category
        bmi_category=np.where(patients['bmi'] < 18.5, 'Underweight',
                            np.where(patients['bmi'] < 25, 'Normal',
                                   np.where(patients['bmi'] < 30, 'Overweight', 'Obese'))),
        # Risk level
        risk_level=np.where(
            (patients['bmi'] > 30) & (patients['cholesterol'] > 200),
            'High',
            np.where(
                (patients['bmi'] > 25) | (patients['cholesterol'] > 200),
                'Medium',
                'Low'
            )
        ),
        # Age group
        age_group=np.where(patients['age'] < 30, '18-29',
                         np.where(patients['age'] < 45, '30-44',
                                np.where(patients['age'] < 60, '45-59', '60+')))
    )
)

print("\n=== Clinical Analysis ===")
print(clinical[['patient_id', 'age', 'bmi', 'bmi_category', 'bp_category', 'risk_level']])

# Clinical summary by age group
age_summary = clinical.groupby('age_group').agg({
    'bmi': 'mean',
    'cholesterol': 'mean',
    'heart_rate': 'mean',
    'patient_id': 'count'
}).round(2)

print("\n=== Summary by Age Group ===")
print(age_summary)

# Risk analysis
risk_summary = clinical.groupby('risk_level').agg({
    'bmi': 'mean',
    'cholesterol': 'mean',
    'age': 'mean',
    'patient_id': 'count'
}).round(2)

print("\n=== Risk Summary ===")
print(risk_summary)
```

---

## Output Results

### Sample Output - Banking

```
=== Loan Portfolio ===
   loan_id customer_id   loan_type   principal  interest_rate  term_months credit_score    status
0    LN0001        C009     Mortgage  342567.89          4.50           360           742     Current
1    LN0002        C005    Personal   45678.90          8.25            48           688  Delinquent
2    LN0003        C012     Business  123456.78          6.75           120           756     Current
...

=== Loans with Calculations ===
   loan_id   principal  monthly_payment  total_interest risk_category
0  LN0001  342567.89       1721.45     277362.20          Low
1  LN0002   45678.90       1125.67      8252.26        Medium
2  LN0003  123456.78       1111.23      9287.92         Low
...

=== Risk Summary ===
               principal               interest_rate credit_score
                   sum    mean count         mean        mean
risk_category                                          
High            1250000  62500.0     20         7.25       675
Low            2500000 125000.0     20         5.50       775
Medium        1875000 93750.0     20         6.75       725
```

### Sample Output - Healthcare

```
=== Patient Clinical Data ===
   patient_id  age   bmi blood_pressure_systolic blood_pressure_diastolic  heart_rate cholesterol glucose  diagnosis
0      PT001   52  28.3                  142                       88           78         245     98   Hypertensive
1      PT002   34  22.1                  118                       76           72         198    102      Healthy
2      PT003   68  31.2                  156                       94           88         267    115    Diabetic
...

=== Clinical Analysis ===
  patient_id age  bmi bmi_category bp_category risk_level
0    PT001  52  28.3  Overweight   High Stage 2      Low
1    PT002  34  22.1      Normal      Normal      Low
2    PT003  68  31.2       Obese   High Stage 2     High
...

=== Risk Summary ===
            bmi  cholesterol    age patient_id
           mean        mean     mean       count
risk_level                                 
High        31.5       278.5    58.2         6
Low         23.2       195.8    42.1         8
Medium     27.8       235.2    51.5         6
```

---

## Visualization

### ASCII - Pipeline Operations

```
+------------------------------------------------------------------+
|           ADVANCED PIPELINE FLOW                                 |
+------------------------------------------------------------------+

METHOD CHAINING:
    df → assign() → query() → sort_values() → reset_index()
            │          │            │              │
            v          v            v              v
       +--------+  +---------+  +----------+  +----------+
       | Add    |  | Filter |  | Sort     |  | Reset   |
       | cols   |  | rows   |  | order    |  | index  |
       +--------+  +---------+  +----------+  +----------+

PIPE OPERATIONS:
    df → pipe(add) → pipe(multiply, factor=2) → pipe(filter)
            │              │                       │
            v              v                       v
       +--------+     +---------+            +---------+
       | Custom |     | Custom  |            | Custom  |
       | func1 |     | func2  |            | func3  |
       +--------+     +---------+            +---------+

+------------------------------------------------------------------+
```

### ASCII - Conditional Operations

```
+------------------------------------------------------------------+
|           CONDITIONAL OPERATIONS                                    |
+------------------------------------------------------------------+

np.where (simple):
    Value > 3  →  TRUE → 'High'
                         FALSE → 'Low'

np.select (multiple):
    value < 2  →  'Low'
    value < 4  →  'Medium'
    default   →  'High'

np.select with ranges:
    [systolic < 120 AND diastolic < 80]  →  'Normal'
    [systolic < 130 AND diastolic < 80]  →  'Elevated'
    [systolic < 140 OR diastolic < 90]   →  'High Stage 1'
    default                             →  'High Stage 2'

+------------------------------------------------------------------+
```

### ASCII - Banking Risk Analysis

```
+------------------------------------------------------------------+
|         BANKING RISK ANALYSIS FLOW                                 |
+------------------------------------------------------------------+

CREDIT SCORE DISTRIBUTION:
    600-639  ████████████          High Risk
    640-669  ██████████████        High Risk
    670-699  ██████████████████    Medium Risk
    700-739  ██████████████████████ Medium Risk
    740-799  ██████████████████████████████████████ Low Risk
    800-850  ████████████████████    Low Risk

RISK FACTORS:
    Credit Score ≥ 750 → Low Risk ✓
    Credit 700-749      → Medium ✓
    Credit < 700       → High Risk ⚠

    + High Balance     → Higher Risk ⚠⚠
    + Delinquent      → Very High ⚠⚠⚠

+------------------------------------------------------------------+
```

---

## Advanced Topics

### Custom Aggregation

```python
# Custom aggregation function
def percentile_25(x):
    return x.quantile(0.25)

result = df.groupby('group').agg({
    'value': [
        ('mean', 'mean'),
        ('median', 'median'),
        ('q25', percentile_25),
        ('q75', lambda x: x.quantile(0.75)),
        ('count', 'count')
    ]
})
```

### Rolling Apply

```python
# Apply custom function to rolling window
def rolling_cagr(returns):
    return (1 + returns).prod() - 1

result = df['returns'].rolling(window=252).apply(rolling_cagr)
```

### Categorical Data

```python
# Create categorical
cat_type = pd.Categorical(
    ['Low', 'Medium', 'High'],
    categories=['Low', 'Medium', 'High'],
    ordered=True
)

# Using categorical for sorting
df['priority'] = pd.Categorical(
    df['priority'],
    categories=['Low', 'Medium', 'High'],
    ordered=True
)
```

### Memory Optimization

```python
# Optimize dtypes
df['int_col'] = df['int_col'].astype('int32')
df['float_col'] = df['float_col'].astype('float32')

# Use category dtype
df['string_col'] = df['string_col'].astype('category')

# Use appropriate datetime
df['date_col'] = pd.to_datetime(df['date_col'])
```

### Eval for Large Data

```python
# Using eval for large data
df.eval('new_col = A + B * C', inplace=True)

# Multiple operations
df.eval('
    sum = A + B
    diff = A - B
    product = A * B
', inplace=True)
```

---

## Conclusion

### Key Takeaways

This module covered advanced Pandas operations:

1. **Method Chaining**: Clean pipeline code
2. **Apply**: Custom transformations
3. **Pipe**: Reusable functions
4. **Vectorized Conditionals**: Efficient if-else
5. **Performance**: Memory optimization

### Practical Applications

- Banking: Risk analysis, portfolio calculations
- Healthcare: Clinical analysis, patient risk
- Analytics: Complex data pipelines

### Next Steps

This completes the Pandas module. Continue to other Data Science topics.

### References

- Advanced Pandas: https://pandas.pydata.org/docs/user_guide/advanced.html
- Performance: https://pandas.pydata.org/docs/user_guide/enhancingperf.html