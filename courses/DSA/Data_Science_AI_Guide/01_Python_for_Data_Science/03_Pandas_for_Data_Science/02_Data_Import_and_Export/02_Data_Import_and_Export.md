# Data Import and Export with Pandas

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

### Overview of Data Import/Export

Data Import and Export are fundamental operations in pandas that enable reading from and writing to various file formats. These capabilities make pandas a versatile tool for data pipelines in banking, healthcare, and other industries.

### Supported File Formats

Pandas supports numerous file formats:
- **Text formats**: CSV, TSV, TXT
- **Excel formats**: XLS, XLSX, XLSM
- **Data interchange**: JSON, XML, HTML
- **Binary formats**: Pickle, Parquet, Feather, HDF5
- **Database connections**: SQL, BigQuery
- **Statistical formats**: Stata, SAS, SPSS

### Importance in Data Science

Efficient data handling is crucial for:
- Building ETL (Extract, Transform, Load) pipelines
- Integrating data from multiple sources
- Data migration and backup
- Sharing data between systems

---

## Fundamentals

### Reading CSV Files

```python
import pandas as pd

# Basic CSV reading
df = pd.read_csv('data.csv')

# Reading with specific parameters
df = pd.read_csv(
    'data.csv',
    sep=',',
    encoding='utf-8',
    header=0,  # First row as header
    names=['col1', 'col2', 'col3'],  # Custom column names
    index_col='id',  # Column to use as index
    skiprows=1,  # Skip first N rows
    na_values=['NA', 'null', ''],  # Values to treat as NA
    parse_dates=['date_column'],  # Columns to parse as dates
    thousands=',',  # Thousands separator
    decimal='.'  # Decimal separator
)

# Reading large files in chunks
chunks = pd.read_csv('large_file.csv', chunksize=10000)
for chunk in chunks:
    # Process each chunk
    pass
```

### Writing CSV Files

```python
# Basic CSV writing
df.to_csv('output.csv')

# Writing with specific parameters
df.to_csv(
    'output.csv',
    sep=',',
    index=True,  # Include index in output
    header=True,  # Include column headers
    encoding='utf-8',
    na_rep='NA',  # Representation for NA values
    float_format='%.2f',  # Format for float values
    columns=['col1', 'col2'],  # Columns to include
    date_format='%Y-%m-%d'  # Date format
)
```

### Reading Excel Files

```python
# Reading Excel files
df = pd.read_excel('data.xlsx')

# Reading specific sheet
df = pd.read_excel('data.xlsx', sheet_name='Sheet1')

# Reading multiple sheets
sheets = pd.read_excel('data.xlsx', sheet_name=None)

# Reading with specific columns and rows
df = pd.read_excel('data.xlsx', usecols=[0, 1, 2], nrows=100)
```

### Writing Excel Files

```python
# Writing to Excel
df.to_excel('output.xlsx')

# Writing to specific sheet
with pd.ExcelWriter('output.xlsx') as writer:
    df1.to_excel(writer, sheet_name='Sheet1')
    df2.to_excel(writer, sheet_name='Sheet2')
```

### Reading JSON Files

```python
# Reading JSON
df = pd.read_json('data.json')

# Reading JSON lines format
df = pd.read_json('data.jsonl', lines=True)

# Reading with orientation
df = pd.read_json('data.json', orient='records')
```

### Database Connections

```python
import sqlite3

# Reading from SQLite
conn = sqlite3.connect('database.db')
df = pd.read_sql('SELECT * FROM table', conn)
df = pd.read_sql_query('SELECT * FROM table', conn)

# Writing to SQLite
df.to_sql('table', conn, if_exists='replace', index=False)
```

---

## Implementation

### Example 1: CSV Data Pipeline

```python
import pandas as pd
from io import StringIO

# Sample CSV data
csv_data = """employee_id,name,department,salary,hire_date
E001,Alice Johnson,IT,75000,2020-01-15
E002,Bob Smith,HR,65000,2021-03-20
E003,Carol White,Finance,72000,2019-07-10
E004,David Brown,IT,80000,2018-11-05
E005,Eva Martinez,Marketing,68000,2022-02-28"""

# Read CSV from string
df = pd.read_csv(StringIO(csv_data))
print("=== Employee Data ===")
print(df)

# Parse dates
df['hire_date'] = pd.to_datetime(df['hire_date'])

# Calculate years of service
df['years_of_service'] = (pd.Timestamp.now() - df['hire_date']).dt.days / 365.25
print("\n=== With Service Years ===")
print(df)
```

### Example 2: Multi-Sheet Excel Processing

```python
# Create sample DataFrames
employees = pd.DataFrame({
    'emp_id': ['E001', 'E002', 'E003'],
    'name': ['Alice', 'Bob', 'Carol'],
    'department': ['IT', 'HR', 'Finance']
})

salaries = pd.DataFrame({
    'emp_id': ['E001', 'E002', 'E003'],
    'salary': [75000, 65000, 72000],
    'bonus': [7500, 6500, 7200]
})

# Write to Excel with multiple sheets
with pd.ExcelWriter('company_data.xlsx') as writer:
    employees.to_excel(writer, sheet_name='Employees', index=False)
    salaries.to_excel(writer, sheet_name='Salaries', index=False)

# Read back
employees_df = pd.read_excel('company_data.xlsx', sheet_name='Employees')
salaries_df = pd.read_excel('company_data.xlsx', sheet_name='Salaries')

print("=== Employees ===")
print(employees_df)
print("\n=== Salaries ===")
print(salaries_df)
```

---

## Applications

### Banking Sector Application

#### Importing Transaction Data

```python
import pandas as pd
from io import StringIO

# Sample bank transaction data (CSV format)
transaction_csv = """transaction_id,account_id,transaction_date,transaction_type,amount,description
TXN001,ACC1001,2024-01-15,DEPOSIT,5000.00,Salary Deposit
TXN002,ACC1001,2024-01-16,WITHDRAWAL,-500.00,ATM Withdrawal
TXN003,ACC1001,2024-01-17,TRANSFER,-1500.00,Transfer to ACC1002
TXN004,ACC1002,2024-01-17,DEPOSIT,1500.00,Transfer from ACC1001
TXN005,ACC1001,2024-01-18,PAYMENT,-250.00,Utility Bill
TXN006,ACC1003,2024-01-18,DEPOSIT,10000.00,Wire Transfer
TXN007,ACC1002,2024-01-19,WITHDRAWAL,-200.00,ATM Withdrawal
TXN008,ACC1003,2024-01-20,PAYMENT,-3500.00,Loan Payment"""

# Read transactions
transactions = pd.read_csv(StringIO(transaction_csv))
transactions['transaction_date'] = pd.to_datetime(transactions['transaction_date'])

print("=== Bank Transactions ===")
print(transactions)

# Account summary
account_summary = transactions.groupby('account_id').agg({
    'amount': ['sum', 'count'],
    'transaction_type': 'first'
}).reset_index()

print("\n=== Account Summary ===")
print(account_summary)
```

#### Exporting Reports

```python
# Create account report
accounts = pd.DataFrame({
    'account_id': ['ACC1001', 'ACC1002', 'ACC1003'],
    'customer_name': ['James Wilson', 'Maria Garcia', 'Robert Chen'],
    'account_type': ['Checking', 'Savings', 'Premium'],
    'balance': [15000.00, 25000.00, 85000.00],
    'credit_score': [720, 750, 820]
})

# Add calculated fields
accounts['account_tier'] = pd.cut(
    accounts['balance'],
    bins=[0, 10000, 50000, 100000],
    labels=['Standard', 'Gold', 'Platinum']
)

# Export to Excel with formatting
accounts.to_excel('bank_accounts_report.xlsx', index=False)
print("Report exported to bank_accounts_report.xlsx")
```

### Healthcare Sector Application

#### Importing Patient Records

```python
# Sample patient data (CSV format)
patient_csv = """patient_id,name,dob,admission_date,department,diagnosis,room,status
PT3001,John Doe,1979-05-15,2024-01-01,Cardiology,Hypertension,501A,Active
PT3002,Jane Smith,1986-10-22,2024-01-05,General,Flu,302B,Discharged
PT3003,Bob Johnson,1962-03-08,2024-01-09,Orthopedics,Fracture,401A,Active
PT3004,Alice Williams,1995-07-30,2024-01-13,General,Infection,305A,Active
PT3005,Charlie Brown,1969-12-03,2024-01-17,Neurology,Migraine,601B,Discharged"""

# Read patient data
patients = pd.read_csv(StringIO(patient_csv))
patients['dob'] = pd.to_datetime(patients['dob'])
patients['admission_date'] = pd.to_datetime(patients['admission_date'])

print("=== Patient Records ===")
print(patients)

# Calculate age
patients['age'] = (pd.Timestamp.now() - patients['dob']).dt.days / 365.25
print("\n=== Patient Ages ===")
print(patients[['patient_id', 'name', 'age', 'department']])
```

#### Exporting Medical Billing

```python
# Create billing data
billing = pd.DataFrame({
    'patient_id': ['PT3001', 'PT3002', 'PT3003', 'PT3004', 'PT3005'],
    'patient_name': ['John Doe', 'Jane Smith', 'Bob Johnson', 'Alice Williams', 'Charlie Brown'],
    'room_charge': [2500, 1000, 3500, 1500, 2000],
    'medical_services': [6000, 2250, 12500, 4000, 7500],
    'medication': [1750, 600, 2900, 1000, 1400],
    'lab_tests': [1000, 750, 2000, 900, 1500]
})

# Calculate totals
billing['total_charges'] = billing['room_charge'] + billing['medical_services'] + \
                         billing['medication'] + billing['lab_tests']

billing['insurance'] = billing['total_charges'] * 0.8
billing['patient_pay'] = billing['total_charges'] * 0.2

print("=== Medical Billing ===")
print(billing[['patient_id', 'patient_name', 'total_charges', 'insurance', 'patient_pay']])

# Export to Excel
billing.to_excel('medical_billing_report.xlsx', index=False)
print("\nBilling report exported to medical_billing_report.xlsx")
```

---

## Output Results

### Sample Output - Transactions

```
=== Bank Transactions ===
  transaction_id account_id transaction_date transaction_type   amount              description
0         TXN001    ACC1001      2024-01-15         DEPOSIT   5000.00          Salary Deposit
1         TXN002    ACC1001      2024-01-16      WITHDRAWAL  -500.00           ATM Withdrawal
2         TXN003    ACC1001      2024-01-17       TRANSFER  -1500.00  Transfer to ACC1002
3         TXN004    ACC1002      2024-01-17         DEPOSIT   1500.00  Transfer from ACC1001
4         TXN005    ACC1001      2024-01-18        PAYMENT   -250.00          Utility Bill
5         TXN006    ACC1003      2024-01-18         DEPOSIT  10000.00        Wire Transfer
6         TXN007    ACC1002      2024-01-19      WITHDRAWAL   -200.00           ATM Withdrawal
7         TXN008    ACC1003      2024-01-20        PAYMENT  -3500.00         Loan Payment

8 rows
```

### Sample Output - Patients

```
=== Patient Records ===
  patient_id          name        dob admission_date  department     diagnosis  room status
0    PT3001       John Doe 1979-05-15     2024-01-01  Cardiology  Hypertension  501A  Active
1    PT3002     Jane Smith 1986-10-22     2024-01-05     General         Flu  302B Discharged
2    PT3003    Bob Johnson 1962-03-08     2024-01-09  Orthopedics     Fracture  401A  Active
3    PT3004  Alice Williams 1995-07-30     2024-01-13     General    Infection  305A  Active
4    PT3005   Charlie Brown 1969-12-03     2024-01-17   Neurology     Migraine  601B Discharged

5 rows
```

---

## Visualization

### ASCII - Data Import/Export Flow

```
+------------------------------------------------------------------+
|              DATA IMPORT/EXPORT PIPELINE                             |
+------------------------------------------------------------------+

                    +---------------+
                    |   DATA SOURCE |
                    +---------------+
                            |
                            v
        +-------------------+-------------------+
        |                   |                   |
        v                   v                   v
  +----------+        +----------+        +----------+
  |    CSV   |        |   JSON   |        |  Excel   |
  +----------+        +----------+        +----------+
        |                   |                   |
        v                   v                   v
  +----------+        +----------+        +----------+
  |  read_   |        |  read_   |        |  read_   |
  |  csv()   |        |  json()  |        |  excel() |
  +----------+        +----------+        +----------+
        |                   |                   |
        +-------------------+-------------------+
                            |
                            v
                    +---------------+
                    |   DATAFRAME   |
                    +---------------+
                            |
                            v
        +-------------------+-------------------+
        |                   |                   |
        v                   v                   v
  +----------+        +----------+        +----------+
  | to_csv() |        | to_json() |        | to_excel()|
  +----------+        +----------+        +----------+
        |                   |                   |
        v                   v                   v
  +----------+        +----------+        +----------+
  |  OUTPUT  |        |  OUTPUT  |        |  OUTPUT  |
  +----------+        +----------+        +----------+

+------------------------------------------------------------------+
```

### ASCII - File Format Support

```
+------------------------------------------------------------------+
|              SUPPORTED FILE FORMATS                                |
+------------------------------------------------------------------+

TEXT FORMAT           EXCEL FORMAT          DATABASE
-----------          ------------         --------
[CSV] ████████       [XLS] ██████         [SQL] ████████
[TSV] ██████         [XLSX] ███████       [BQ]  ██████
[TXT] ██████         [XLSM] ██████       

INTERCHANGE          BINARY               STATISTICAL
-----------         -------              ----------
[JSON] ████████     [PKL] ███████      [STA] ████
[XML]  ██████       [PAR] ███████      [SAS] ████
[HTML] █████        [HDF] ███████      [SAV] ████
[SQL]  ███████      [FTH] ██████

+------------------------------------------------------------------+
```

### ASCII - Healthcare Data Pipeline

```
+------------------------------------------------------------------+
|           HEALTHCARE DATA PIPELINE                               |
+------------------------------------------------------------------+

SOURCE FILES                     PROCESSING
-----------                      ----------
+----------+                    +----------+
| Patient  |                    |  PARSE   |
| Records  | ------> read_csv() |  DATES   |
| .csv     |                    +----------+
+----------+                           |
                                     v
                              +-------------+
                              |  CALCULATE  |
                              |     AGE    |
                              +-------------+
                                     |
                                     v
                              +-------------+
                              | VALIDATE   |
                              | DATA TYPES |
                              +-------------+
                                     |
                                     v
                              +-------------+
                              |  EXPORT    |
                              |  TO EXCEL |
                              +-------------+
                                     |
                                     v
                              +----------+
                              | BILLING  |
                              | REPORT  |
                              | .xlsx   |
                              +----------+

+------------------------------------------------------------------+
```

---

## Advanced Topics

### Working with Large Files

```python
# Reading large CSV files in chunks
def process_large_csv(filepath, chunksize=10000):
    total_rows = 0
    results = []
    
    for chunk in pd.read_csv(filepath, chunksize=chunksize):
        total_rows += len(chunk)
        # Process chunk
        processed = chunk[chunk['value'] > 100]
        results.append(processed)
    
    return pd.concat(results), total_rows

# Using iterator for large files
df_iterator = pd.read_csv('large_file.csv', chunksize=5000, iterator=True)
df = df_iterator.get_chunk(10000)
```

### Database Integration

```python
# SQLAlchemy integration
from sqlalchemy import create_engine

# Create engine
engine = create_engine('sqlite:///company.db')

# Read from SQL
df = pd.read_sql('SELECT * FROM employees WHERE department = "IT"', engine)

# Write to SQL
df.to_sql('employees', engine, if_exists='replace', index=False)

# Using SQL with parameters
query = """
SELECT department, AVG(salary) as avg_salary, COUNT(*) as count
FROM employees
GROUP BY department
"""
df = pd.read_sql(query, engine)
```

### Reading from URLs

```python
# Read CSV from URL
url = 'https://example.com/data.csv'
df = pd.read_csv(url)

# Read JSON from URL
df = pd.read_json('https://api.example.com/data')

# Read HTML tables
tables = pd.read_html('https://example.com/tables.html')
df = tables[0]
```

### Parquet Format for Large Datasets

```python
# Write to Parquet (columnar format, efficient for large data)
df.to_parquet('data.parquet', engine='pyarrow', compression='snappy')

# Read from Parquet
df = pd.read_parquet('data.parquet')

# Read with columns selection
df = pd.read_parquet('data.parquet', columns=['col1', 'col2'])
```

### Excel with Formatting

```python
# Using openpyxl for formatting
from openpyxl import Workbook
from openpyxl.styles import Font, PatternFill, Alignment

# Create workbook
wb = Workbook()
ws = wb.active

# Write data
for r in dataframe_to_rows(df, index=False, header=True):
    ws.append(r)

# Format header
header_fill = PatternFill(start_color='366092', end_color='366092', fill_type='solid')
header_font = Font(bold=True, color='FFFFFF')

for cell in ws[1]:
    cell.fill = header_fill
    cell.font = header_font
    cell.alignment = Alignment(horizontal='center')

wb.save('formatted_output.xlsx')
```

---

## Conclusion

### Key Takeaways

This module covered essential data import/export operations:

1. **CSV Files**: Most common format for data exchange
2. **Excel Files**: Multi-sheet support for detailed reports
3. **JSON**: Flexible format for nested data
4. **Database Integration**: Direct SQL connections
5. **Large File Handling**: Chunk processing for memory efficiency

### Practical Applications

- Banking: Transaction data import, report generation
- Healthcare: Patient record management, billing exports
- ETL Pipines: Data transformation and migration

### Next Steps

Continue to: **Data Cleaning and Preprocessing** to learn data quality handling.

### References

- Pandas IO Documentation: https://pandas.pydata.org/docs/user_guide/io.html
- openpyxl Documentation: https://openpyxl.readthedocs.io/
- SQLAlchemy Documentation: https://docs.sqlalchemy.org/