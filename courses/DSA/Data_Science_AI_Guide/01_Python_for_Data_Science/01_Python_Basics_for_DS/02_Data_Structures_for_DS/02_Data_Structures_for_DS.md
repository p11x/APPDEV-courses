# Data Structures for Data Science

## Introduction

Data structures are the fundamental building blocks of any data science workflow. They provide organized ways to store, access, and manipulate data, which is essential for performing efficient analysis and building machine learning models. In data science, the choice of data structure can significantly impact memory usage, processing speed, and the overall quality of insights derived from data.

Python offers a rich variety of built-in data structures that cater to different use cases in data science. Understanding these structures deeply is crucial for any data scientist or analyst. The primary data structures include lists, tuples, sets, dictionaries, and more complex ones like numpy arrays and pandas DataFrames. Each has unique characteristics that make it suitable for specific tasks.

In the context of data science, data structures can be categorized into two main groups: linear and non-linear structures. Linear structures store data in a sequential manner, where each element is connected to its previous and next element. Examples include lists, tuples, and linked lists. Non-linear structures, on the other hand, store data in a hierarchical or interconnected manner, such as trees and graphs. For data scientists, the most frequently used structures are lists, dictionaries, pandas Series, and pandas DataFrames.

This guide covers the essential data structures used in data science, their implementations, and practical applications in domains like banking and healthcare. By the end, you will have a solid understanding of when to use each data structure and how to optimize your data manipulation workflows.

## Fundamentals

### Lists

Lists are one of the most versatile and commonly used data structures in Python. They are ordered, mutable sequences that can hold items of different types. Lists are created using square brackets `[]` or the `list()` constructor. They support indexing, slicing, and various operations like appending, inserting, and removing elements.

Key characteristics of lists include:
- Ordered collection: Elements maintain their insertion order
- Mutable: Can be modified in place
- Dynamic: Can grow or shrink as needed
- Heterogeneous: Can contain elements of different types

Lists are ideal for scenarios where you need to maintain a collection of items that may change over time, such as storing a series of observations or results from data processing.

### Tuples

Tuples are similar to lists but are immutable, meaning they cannot be modified after creation. They are created using parentheses `()` or the `tuple()` constructor. Tuples are useful when you need to store a fixed sequence of values that should not be accidentally changed, such as coordinate pairs or database records.

Key characteristics of tuples include:
- Immutable: Cannot be changed after creation
- Ordered: Elements maintain their insertion order
- Hashable: Can be used as dictionary keys (unlike lists)
- Lightweight: Use less memory than lists

In data science, tuples are commonly used to represent records, such as rows in a dataset where each row contains multiple attributes.

### Sets

Sets are unordered collections of unique elements. They are created using curly braces `{}` or the `set()` constructor. Sets are particularly useful for eliminating duplicates and performing membership tests. Since sets are unordered, they do not support indexing.

Key characteristics of sets include:
- Unordered: No guaranteed order of elements
- Unique: Duplicate elements are automatically removed
- Fast operations: O(1) lookup and membership tests
- Support mathematical set operations: union, intersection, difference

Sets are valuable in data science for finding unique values in a column, removing duplicates, and performing set-based operations like finding common elements between groups.

### Dictionaries

Dictionaries are key-value pairs where each key maps to a value. They are created using curly braces `{}` with key-value pairs separated by colons, or the `dict()` constructor. Dictionaries provide O(1) lookup time for accessing values by their keys.

Key characteristics of dictionaries include:
- Key-value mapping: Each key uniquely identifies a value
- Mutable: Can be modified in place
- Fast lookup: O(1) access by key
- Heterogeneous: Keys and values can be of different types

In data science, dictionaries are used for mapping categorical values, storing configurations, and representing JSON data.

### NumPy Arrays

NumPy arrays are homogeneous, multidimensional arrays that provide efficient numerical operations. They are the foundation for numerical computing in Python and are extensively used in data science for handling large datasets. NumPy arrays offer significant performance advantages over Python lists due to their contiguous memory layout and vectorized operations.

Key characteristics of NumPy arrays include:
- Homogeneous: All elements are of the same type
- Multidimensional: Can have any number of dimensions
- Vectorized operations: Perform operations on entire arrays without loops
- Memory efficient: Use less memory than Python lists

### Pandas DataFrames

Pandas DataFrames are two-dimensional labeled data structures that resemble tables in a relational database or spreadsheet. They are the most commonly used data structure in data science for data manipulation and analysis. DataFrames provide intuitive ways to handle tabular data with rows and columns.

Key characteristics of DataFrames include:
- Tabular structure:Rows and columns with labels
- Mixed types: Different columns can have different types
- Rich API: Extensive methods for data manipulation
- Label-based indexing: Access data by row and column labels

## Implementation

### List Operations

```python
# Creating lists
numbers = [1, 2, 3, 4, 5]
mixed = [1, "hello", 3.14, True]

# List indexing and slicing
first_element = numbers[0]  # 1
last_element = numbers[-1]  # 5
sliced = numbers[1:4]  # [2, 3, 4]

# List operations
numbers.append(6)  # Add element to end
numbers.insert(0, 0)  # Insert at index 0
numbers.remove(3)  # Remove first occurrence of 3
popped = numbers.pop()  # Remove and return last element

# List comprehension for data processing
squares = [x**2 for x in range(10)]
even_numbers = [x for x in range(20) if x % 2 == 0]

# List operations for data analysis
data_points = [10, 20, 30, 40, 50]
mean_value = sum(data_points) / len(data_points)
max_value = max(data_points)
min_value = min(data_points)

# Filtering data
transactions = [150, 200, 50, 300, 100, 450]
high_value = [t for t in transactions if t > 200]

print("List Examples:")
print(f"Numbers: {numbers}")
print(f"Squares: {squares}")
print(f"Mean: {mean_value}")
print(f"High value transactions: {high_value}")
```

### Tuple Operations

```python
# Creating tuples
point = (10, 20)
rgb_color = (255, 128, 0)
employee = ("John", "Doe", 1985, 50000)

# Tuple unpacking
name, surname, year, salary = employee

# Tuple operations
x, y = point
print(f"Point coordinates: x={x}, y={y}")

# Tuple as data record
dataset_row = ("2024-01-01", "AAPL", 150.25, 1000)
date, symbol, price, volume = dataset_row

# Named tuple for clearer code
from collections import namedtuple
Stock = namedtuple("Stock", ["date", "symbol", "price", "volume"])
stock = Stock("2024-01-01", "AAPL", 150.25, 1000)
print(f"Stock: {stock.date} - {stock.symbol} @ ${stock.price}")

# Tuple comparison
tuple1 = (1, 2, 3)
tuple2 = (1, 2, 4)
print(f"Tuple comparison: {tuple1 < tuple2}")  # True

# Converting between list and tuple
list_to_tuple = tuple([1, 2, 3])
tuple_to_list = list((1, 2, 3))

print("\nTuple Examples:")
print(f"Employee: {employee}")
print(f"Unpacked: {name}, {salary}")
print(f"Stock record: {stock}")
```

### Set Operations

```python
# Creating sets
unique_ids = {1, 2, 3, 4, 5}
prime_numbers = {2, 3, 5, 7, 11}

# Set operations
set_a = {1, 2, 3, 4}
set_b = {3, 4, 5, 6}

union = set_a | set_b  # {1, 2, 3, 4, 5, 6}
intersection = set_a & set_b  # {3, 4}
difference = set_a - set_b  # {1, 2}
symmetric_diff = set_a ^ set_b  # {1, 2, 5, 6}

# Membership test
print(f"3 in set_a: {3 in set_a}")

# Adding and removing elements
unique_ids.add(6)
unique_ids.discard(1)  # Remove without error if not exists
# unique_ids.remove(1)  # Would raise KeyError

# Set comprehension
squares_set = {x**2 for x in range(10)}
unique_squares = {x**2 for x in range(-5, 6)}

# Finding unique values in a list
transactions = [101, 102, 101, 103, 102, 104, 101]
unique_transactions = set(transactions)
print(f"Unique transaction IDs: {unique_transactions}")

# Finding common elements
customer_group_1 = {"C001", "C002", "C003", "C004"}
customer_group_2 = {"C002", "C003", "C005", "C006"}
common_customers = customer_group_1 & customer_group_2
print(f"Common customers: {common_customers}")

print("\nSet Examples:")
print(f"Union: {union}")
print(f"Intersection: {intersection}")
print(f"Unique transactions: {unique_transactions}")
```

### Dictionary Operations

```python
# Creating dictionaries
employee = {
    "name": "John Doe",
    "age": 35,
    "department": "Engineering",
    "salary": 75000
}

# Accessing values
name = employee["name"]
age = employee.get("age", 0)  # With default

# Adding and modifying entries
employee["position"] = "Senior Engineer"
employee["salary"] = 85000

# Removing entries
removed_salary = employee.pop("salary", None)

# Dictionary iteration
for key, value in employee.items():
    print(f"{key}: {value}")

# Dictionary comprehension
prices = {"AAPL": 150, "GOOGL": 2800, "MSFT": 300}
doubled_prices = {symbol: price * 2 for symbol, price in prices.items()}

# Dictionary for counting
from collections import Counter
transaction_types = ["deposit", "withdrawal", "transfer", "deposit", "deposit", "withdrawal"]
transaction_counts = Counter(transaction_types)
print(f"Transaction counts: {transaction_counts}")

# Nested dictionaries
customers = {
    "C001": {"name": "Alice", "balance": 5000},
    "C002": {"name": "Bob", "balance": 3000}
}

# Lookup with default
default_balance = customers.get("C003", {"name": "Unknown", "balance": 0})
print(f"Customer C003: {default_balance}")

print("\nDictionary Examples:")
print(f"Employee: {employee}")
print(f"Doubled prices: {doubled_prices}")
print(f"Transaction counts: {transaction_counts}")
```

### NumPy Array Operations

```python
import numpy as np

# Creating arrays
arr1d = np.array([1, 2, 3, 4, 5])
arr2d = np.array([[1, 2, 3], [4, 5, 6], [7, 8, 9]])

# Array operations
arr_sum = np.sum(arr1d)
arr_mean = np.mean(arr1d)
arr_std = np.std(arr1d)
arr_max = np.max(arr1d)
arr_min = np.min(arr1d)

# Vectorized operations
squared = arr1d ** 2
exponential = np.exp(arr1d)

# Array slicing
first_row = arr2d[0, :]
first_col = arr2d[:, 0]
sub_array = arr2d[1:, 1:]

# Array broadcasting
arr_a = np.array([1, 2, 3])
arr_b = np.array([10, 20, 30])
result = arr_a + arr_b

# Creating arrays with functions
zeros = np.zeros(5)
ones = np.ones((3, 3))
range_arr = np.arange(0, 10, 2)
linspace = np.linspace(0, 1, 5)

# Random arrays
random_arr = np.random.random((3, 3))
random_int = np.random.randint(1, 100, 10)

# Statistical operations
data = np.array([10, 20, 30, 40, 50, 60, 70, 80, 90])
percentile_25 = np.percentile(data, 25)
percentile_75 = np.percentile(data, 75)

print("\nNumPy Array Examples:")
print(f"1D array: {arr1d}")
print(f"2D array:\n{arr2d}")
print(f"Sum: {arr_sum}, Mean: {arr_mean}")
print(f"Squared: {squared}")
print(f"Random array: {random_int}")
```

### Pandas DataFrame Operations

```python
import pandas as pd
import numpy as np

# Creating DataFrames
data = {
    "name": ["Alice", "Bob", "Charlie", "David"],
    "age": [25, 30, 35, 40],
    "salary": [50000, 60000, 70000, 80000],
    "department": ["IT", "HR", "Finance", "IT"]
}
df = pd.DataFrame(data)

# Accessing columns
names = df["name"]
ages = df["age"]

# Accessing rows
first_row = df.iloc[0]
by_index = df.loc[0]

# Filtering
it_employees = df[df["department"] == "IT"]
high_salary = df[df["salary"] > 60000]

# Adding columns
df["bonus"] = df["salary"] * 0.1
df["total_compensation"] = df["salary"] + df["bonus"]

# Statistical summary
stats = df.describe()

# Grouping
dept_avg = df.groupby("department")["salary"].mean()

# Sorting
sorted_df = df.sort_values("salary", ascending=False)

# Handling missing values
df_with_na = df.copy()
df_with_na.loc[1, "age"] = None
filled_df = df_with_na.fillna(0)

# Exporting data
csv_output = df.to_csv(index=False)

print("\nPandas DataFrame Examples:")
print(df)
print(f"\nIT Employees:\n{it_employees}")
print(f"\nDepartment averages:\n{dept_avg}")
```

## Applications in Banking

### Banking Application: Customer Transaction Analysis

```python
import pandas as pd
import numpy as np
from collections import defaultdict

# Simulated banking transaction data
transactions = [
    {"id": "T001", "customer_id": "C001", "type": "deposit", "amount": 5000, "date": "2024-01-01"},
    {"id": "T002", "customer_id": "C002", "type": "withdrawal", "amount": 1000, "date": "2024-01-02"},
    {"id": "T003", "customer_id": "C001", "type": "transfer", "amount": 2000, "date": "2024-01-03"},
    {"id": "T004", "customer_id": "C003", "type": "deposit", "amount": 10000, "date": "2024-01-04"},
    {"id": "T005", "customer_id": "C002", "type": "transfer", "amount": 500, "date": "2024-01-05"},
    {"id": "T006", "customer_id": "C001", "type": "withdrawal", "amount": 3000, "date": "2024-01-06"},
    {"id": "T007", "customer_id": "C004", "type": "deposit", "amount": 7500, "date": "2024-01-07"},
    {"id": "T008", "customer_id": "C003", "type": "withdrawal", "amount": 2000, "date": "2024-01-08"},
]

# Convert to DataFrame
df = pd.DataFrame(transactions)

# Total deposits per customer
deposits = df[df["type"] == "deposit"]
customer_deposits = deposits.groupby("customer_id")["amount"].sum()
print("Customer Deposits:")
print(customer_deposits)

# Total withdrawals per customer
withdrawals = df[df["type"] == "withdrawal"]
customer_withdrawals = withdrawals.groupby("customer_id")["amount"].sum()
print("\nCustomer Withdrawals:")
print(customer_withdrawals)

# Net flow per customer
customer_deposits_reindexed = customer_deposits.reindex(df["customer_id"].unique(), fill_value=0)
customer_withdrawals_reindexed = customer_withdrawals.reindex(df["customer_id"].unique(), fill_value=0)
net_flow = customer_deposits_reindexed - customer_withdrawals_reindexed
print("\nNet Flow per Customer:")
print(net_flow)

# Using dictionary for quick lookup
customer_balances = defaultdict(int)
for _, row in df.iterrows():
    if row["type"] == "deposit":
        customer_balances[row["customer_id"]] += row["amount"]
    elif row["type"] in ["withdrawal", "transfer"]:
        customer_balances[row["customer_id"]] -= row["amount"]

print("\nCustomer Balances (Dictionary):")
for customer, balance in customer_balances.items():
    print(f"{customer}: ${balance}")

# Set operations for customer segmentation
vip_customers = {"C001", "C002"}
new_customers = {"C003", "C004"}
all_customers = vip_customers | new_customers
overlap = vip_customers & new_customers

print(f"\nVIP Customers: {vip_customers}")
print(f"New Customers: {new_customers}")
print(f"All Customers: {all_customers}")
print(f"Overlap: {overlap}")
```

### Banking Application: Risk Assessment

```python
import pandas as pd
import numpy as np

# Customer risk profiles
risk_data = {
    "customer_id": ["C001", "C002", "C003", "C004", "C005"],
    "credit_score": [750, 680, 520, 800, 620],
    "income": [80000, 55000, 35000, 95000, 45000],
    "debt": [15000, 25000, 18000, 10000, 30000],
    "num_accounts": [3, 2, 1, 4, 2]
}
df = pd.DataFrame(risk_data)

# Calculate debt-to-income ratio
df["debt_to_income"] = df["debt"] / df["income"]

# Risk categorization using lists
def categorize_risk(row):
    if row["credit_score"] >= 750 and row["debt_to_income"] < 0.3:
        return "Low"
    elif row["credit_score"] >= 650 and row["debt_to_income"] < 0.5:
        return "Medium"
    else:
        return "High"

# Apply risk categorization
df["risk_category"] = df.apply(categorize_risk, axis=1)

# Using tuples for fixed loan terms
loan_terms = [
    ("C001", 50000, 4.5, 60),  # (customer_id, amount, rate, months)
    ("C002", 30000, 6.0, 48),
    ("C003", 15000, 8.5, 36)
]

# Calculate monthly payments
def calculate_payment(amount, rate, months):
    monthly_rate = rate / 100 / 12
    payment = amount * (monthly_rate * (1 + monthly_rate)**months) / ((1 + monthly_rate)**months - 1)
    return payment

for customer_id, amount, rate, months in loan_terms:
    payment = calculate_payment(amount, rate, months)
    print(f"Customer {customer_id}: ${payment:.2f}/month")

# Using dictionaries for loan eligibility
loan_rules = {
    "min_credit_score": 620,
    "max_debt_to_income": 0.5,
    "min_income": 30000
}

# Dictionary for loan decisions
loan_decisions = {}
for _, row in df.iterrows():
    eligible = (
        row["credit_score"] >= loan_rules["min_credit_score"] and
        row["debt_to_income"] <= loan_rules["max_debt_to_income"] and
        row["income"] >= loan_rules["min_income"]
    )
    loan_decisions[row["customer_id"]] = "Approved" if eligible else "Denied"

print("\nLoan Decisions:")
for customer, decision in loan_decisions.items():
    print(f"{customer}: {decision}")
```

## Applications in Healthcare

### Healthcare Application: Patient Records Management

```python
import pandas as pd
from datetime import datetime, timedelta
from collections import defaultdict

# Patient records using dictionaries
patient_records = {
    "P001": {
        "name": "John Smith",
        "age": 45,
        "conditions": ["hypertension", "diabetes"],
        "medications": ["metformin", "lisinopril"],
        "visits": [
            {"date": "2024-01-01", "type": "checkup", "doctor": "Dr. Wilson"},
            {"date": "2024-02-15", "type": "follow-up", "doctor": "Dr. Wilson"}
        ]
    },
    "P002": {
        "name": "Jane Doe",
        "age": 32,
        "conditions": ["asthma"],
        "medications": ["albuterol"],
        "visits": [
            {"date": "2024-01-10", "type": "checkup", "doctor": "Dr. Brown"}
        ]
    },
    "P003": {
        "name": "Bob Johnson",
        "age": 58,
        "conditions": ["heart_disease", "hypertension"],
        "medications": ["aspirin", "metoprolol", "lisinopril"],
        "visits": [
            {"date": "2024-01-05", "type": "emergency", "doctor": "Dr. Smith"},
            {"date": "2024-02-01", "type": "follow-up", "doctor": "Dr. Smith"}
        ]
    }
}

# Access patient information
patient_id = "P001"
record = patient_records[patient_id]
print(f"Patient: {record['name']}")
print(f"Age: {record['age']}")
print(f"Conditions: {', '.join(record['conditions'])}")
print(f"Medications: {', '.join(record['medications'])}")

# Using sets for condition tracking
all_conditions = set()
for record in patient_records.values():
    all_conditions.update(record["conditions"])
print(f"\nAll unique conditions in system: {all_conditions}")

# Patient condition lookup using dictionary
condition_patients = defaultdict(list)
for patient_id, record in patient_records.items():
    for condition in record["conditions"]:
        condition_patients[condition].append(patient_id)

print("\nPatients by condition:")
for condition, patients in condition_patients.items():
    print(f"  {condition}: {patients}")

# Using tuples for immutable lab results
lab_results = (
    ("P001", "2024-01-01", "glucose", 95, "mg/dL"),
    ("P001", "2024-01-01", "cholesterol", 210, "mg/dL"),
    ("P002", "2024-01-10", "glucose", 85, "mg/dL"),
    ("P003", "2024-01-05", "glucose", 140, "mg/dL")
)

print("\nLab Results:")
for patient_id, date, test, value, unit in lab_results:
    print(f"  {patient_id} - {date} - {test}: {value} {unit}")
```

### Healthcare Application: Hospital Resource Allocation

```python
import pandas as pd
import numpy as np
from collections import deque

# Hospital departments data using lists
departments = [
    {"id": "D001", "name": "Emergency", "capacity": 20, "current": 15, "wait_time": 25},
    {"id": "D002", "name": "ICU", "capacity": 10, "current": 8, "wait_time": 0},
    {"id": "D003", "name": "General Ward", "capacity": 50, "current": 42, "wait_time": 15},
    {"id": "D004", "name": "Pediatrics", "capacity": 15, "current": 12, "wait_time": 10},
    {"id": "D005", "name": "Cardiology", "capacity": 12, "current": 10, "wait_time": 30}
]

# Convert to DataFrame
df = pd.DataFrame(departments)

# Calculate availability
df["available"] = df["capacity"] - df["current"]
df["occupancy_rate"] = df["current"] / df["capacity"] * 100

print("Department Status:")
print(df[["name", "capacity", "current", "available", "occupancy_rate"]])

# Using list for emergency queue
emergency_queue = deque(["P001", "P002", "P003", "P004"])
emergency_queue.append("P005")  # Add new emergency
first_patient = emergency_queue.popleft()  # Remove first patient
print(f"\nNext patient: {first_patient}")
print(f"Queue: {list(emergency_queue)}")

# Priority queue using tuples (priority, patient_id)
from heapq import heappush, heappop
priority_queue = []
heappush(priority_queue, (2, "P002"))  # Normal priority
heappush(priority_queue, (1, "P001"))  # Critical priority
heappush(priority_queue, (3, "P003"))  # Low priority

print("\nPriority order:")
while priority_queue:
    priority, patient = heappop(priority_queue)
    print(f"  {patient} (Priority: {priority})")

# Using dictionary for bed management
bed_management = {}
for dept in departments:
    dept_id = dept["id"]
    bed_list = list(range(1, dept["capacity"] + 1))
    occupied = list(range(1, dept["current"] + 1))
    available = [b for b in bed_list if b not in occupied]
    bed_management[dept_id] = {
        "total": bed_list,
        "occupied": occupied,
        "available": available
    }

print("\nBed Availability:")
for dept_id, beds in bed_management.items():
    print(f"  {dept_id}: {len(beds['available'])} available")
```

## Output Results

### Sample Output: Data Structure Operations

```
List Examples:
Numbers: [0, 1, 2, 4, 5, 6]
Squares: [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
Mean: 30.0
High value transactions: [300, 450]

Tuple Examples:
Employee: ('John', 'Doe', 1985, 50000)
Unpacked: John 50000
Stock record: 2024-01-01 - AAPL @ $150.25

Set Examples:
Union: {1, 2, 3, 4, 5, 6}
Intersection: {3, 4}
Unique transactions: {101, 102, 103, 104}

Dictionary Examples:
Employee: {'name': 'John Doe', 'age': 35, 'department': 'Engineering', 'position': 'Senior Engineer'}
Doubled prices: {'AAPL': 300, 'GOOGL': 5600, 'MSFT': 600}
Transaction counts: Counter({'deposit': 3, 'withdrawal': 2, 'transfer': 1})

NumPy Array Examples:
1D array: [1 2 3 4 5]
2D array:
[[1 2 3]
 [4 5 6]
 [7 8 9]]
Sum: 15, Mean: 3.0
Squared: [ 1  4  9 16 25]
Random array: [42 15 87 63 24  5 92 31 77 56]
```

### Sample Output: Banking Application

```
Customer Deposits:
customer_id
C001    5000
C003   10000
Name: amount, dtype: int64

Customer Withdrawals:
customer_id
C001    3000
C002    1000
C003    2000
Name: amount, dtype: int64

Net Flow per Customer:
C001    2000
C002   -1000
C003    8000
C004    0

Customer Balances (Dictionary):
C001: $2000
C002: $-500
C003: $8000
C004: $7500

Loan Decisions:
C001: Approved
C002: Approved
C003: Denied
C004: Approved
C005: Approved
```

### Sample Output: Healthcare Application

```
Patient: John Smith
Age: 45
Conditions: hypertension, diabetes
Medications: metformin, lisinopril

All unique conditions in system: {'diabetes', 'hypertension', 'asthma', 'heart_disease'}

Patients by condition:
  hypertension: ['P001', 'P003']
  diabetes: ['P001']
  asthma: ['P002']
  heart_disease: ['P003']

Lab Results:
P001 - 2024-01-01 - glucose: 95 mg/dL
P001 - 2024-01-01 - cholesterol: 210 mg/dL
P002 - 2024-01-10 - glucose: 85 mg/dL
P003 - 2024-01-05 - glucose: 140 mg/dL

Department Status:
            name  capacity  current  available  occupancy_rate
0      Emergency       20       15           5            75.0
1            ICU       10        8           2            80.0
2   General Ward       50       42           8            84.0
3     Pediatrics       15       12           3            80.0
4     Cardiology       12       10           2            83.3

Next patient: P001
Queue: ['P002', 'P003', 'P004', 'P005']

Priority order:
P001 (Priority: 1)
P002 (Priority: 2)
P003 (Priority: 3)
```

## Visualization

### ASCII Visualization: Data Structure Comparison

```
+====================================================================+
|                    DATA STRUCTURE PERFORMANCE                      |
+====================================================================+
|                                                                      |
|  LIST OPERATIONS COMPLEXITY                                          |
|  -------------------                                                |
|  Access by Index:    O(1)  [#####]  5/5                             |
|  Search:            O(n)  [#####]  1/5                              |
|  Append:            O(1)  [#####]  5/5                              |
|  Insert:            O(n)  [#####]  1/5                              |
|  Remove:            O(n)  [#####]  1/5                              |
|                                                                      |
|  SET OPERATIONS COMPLEXITY                                            |
|  ---------------------                                              |
|  Add:               O(1)  [#####]  5/5                             |
|  Remove:            O(1)  [#####]  5/5                              |
|  Membership:       O(1)  [#####]  5/5                              |
|  Union:            O(n+m) [####]  3/5                               |
|  Intersection:     O(min(n,m)) [####] 3/5                          |
|                                                                      |
|  DICTIONARY OPERATIONS COMPLEXITY                                      |
|  ---------------------------                                        |
|  Get:              O(1)  [#####]  5/5                                |
|  Set:              O(1)  [#####]  5/5                                |
|  Delete:           O(1)  [#####]  5/5                                |
|  Iteration:        O(n)  [#####]  1/5                                |
|                                                                      |
|  NUMPY ARRAY PERFORMANCE                                             |
|  --------------------                                              |
|  Vectorized Add:   O(n)  [#####] 5/5          vs Python List: O(n)   |
|  Element Multiply:  O(n)  [#####] 5/5          vs Python List: O(n)  |
|  Memory Usage:     Low  [#####]  5/5          vs Python List: High   |
|                                                                      |
|  LEGEND:  [#####] = 5/5 (Best), [###  ] = 3/5, [##   ] = 2/5       |
|          [    ] = 1/5 (Worst)                                       |
+====================================================================+
```

### ASCII Visualization: List vs Set vs Dictionary

```
+====================================================================+
|                   MEMORY AND USAGE COMPARISON                        |
+====================================================================+
|                                                                      |
|  LIST                       SET                     DICTIONARY     |
|  ----                       ---                     ---------       |
|  Ordered: YES            Ordered: NO            Ordered: KEY      |
|  Unique:  NO              Unique: YES             Unique: KEY       |
|  [1,2,2,3] -> [1,2,3]    {1,2,3} -> {1,2,3}     {1:"a",1:"b"}     |
|  [#####]                 [#####]                 [#####]          |
|                                                                      |
|  BEST FOR:                                                         |
|  ------                                                           |
|  + List: Ordered collections, sequences with duplicates            |
|  + Set: Unique values, membership tests, math operations          |
|  + Dict: Key-value lookups, JSON-like data, configurations         |
|                                                                      |
|  USE CASE EXAMPLES:                                               |
|  ---------------                                                  |
|  + List: [1,2,3] + [4] = [1,2,3,4]                             |
|  + Set: {1,2} ∩ {2,3} = {2}                                      |
|  + Dict: d["key"] = value (O(1) lookup)                           |
|                                                                      |
|  PERFORMANCE SCALE (n = 1,000,000 elements):                     |
|  --------------------                                         |
|  List append:     ████████████████████ 100% OK                   |
|  Set lookup:     ████████████████████ 100% OK                   |
|  Dict lookup:    ████████████████████ 100% OK                   |
|  List search:    ████ 20% OK                                     |
|                                                                      |
+====================================================================+
```

### ASCII Visualization: Banking Data Flow

```
+====================================================================+
|              BANKING TRANSACTION FLOW VISUALIZATION                |
+====================================================================+
|                                                                      |
|  CUSTOMER DATABASE (Dictionary)                                     |
|  ======================                                            |
|                                                                      |
|  { "C001": {balance: 5000, transactions: [T001, T003, T006]},    |
|    "C002": {balance: 2500, transactions: [T002, T005]},          |
|    "C003": {balance: 8000, transactions: [T004, T008]},          |
|    "C004": {balance: 7500, transactions: [T007]} }               |
|                                                                      |
|  TRANSACTION TYPES (Set)                                           |
|  ------------------                                                |
|  {"deposit", "withdrawal", "transfer", "payment"}                    |
|                                                                      |
|  PROCESSING QUEUE (List/Deque)                                      |
|  -----------------------                                          |
|  [T001, T002, T003, T004, T005, T006, T007, T008]                |
|    ↑                                                       ↑       |
|  HEAD                                          TAIL                 |
|                                                                      |
|  FLOW DIAGRAM:                                                     |
|  +--------+    +----------+    +---------+    +----------+         |
|  | Input  | -> | Validate | -> | Process | -> | Update   |         |
|  | Queue  |    |          |    |         |    | Balance  |         |
|  +--------+    +----------+    +---------+    +----------+         |
|    T001         Valid? Yes       +1000         C001: 6000        |
|    T002         Valid? Yes       -1000         C002: 1500        |
|    T003         Valid? Yes       -2000         C001: 4000        |
|    ...                                                                     |
|                                                                      |
|  KEY METRICS:                                                       |
|  ----------                                                        |
|  Total Transactions: ████████████████████ 8 completed              |
|  Deposits:          ████████████ 3 (37.5%)                         |
|  Withdrawals:       ████████ 2 (25%)                               |
|  Transfers:        ████████ 2 (25%)                               |
|  Payments:         ███ 1 (12.5%)                                  |
|                                                                      |
+====================================================================+
```

### ASCII Visualization: Healthcare Patient Data

```
+====================================================================+
|            HEALTHCARE PATIENT MANAGEMENT FLOW                     |
+====================================================================+
|                                                                      |
|  PATIENT DATA STRUCTURE                                             |
|  ===================                                                |
|                                                                      |
|  { "P001": {name: "John", conditions: {"hypertension",            |
|             "diabetes"}, visits: [(2024-01-01), (2024-02-15)]},     |
|    "P002": {name: "Jane", conditions: {"asthma"}, ...},           |
|    "P003": {name: "Bob", conditions: {"heart_disease",                |
|             "hypertension"}, ...} }                                |
|                                                                      |
|  CONDITION INDEX (Dictionary of Sets)                               |
|  ==============================                                    |
|                                                                      |
|  { "hypertension"  -> {P001, P003},                                 |
|    "diabetes"     -> {P001},                                        |
|    "asthma"       -> {P002},                                          |
|    "heart_disease"-> {P003} }                                        |
|                                                                      |
|  QUEUE MANAGEMENT                                                   |
|  =============                                                      |
|                                                                      |
|  Priority Queue: (Critical) [P003, P001, P002] -> Normal            |
|    ↑                                                            |
|  Next to See: P003 (Critical)                                        |
|                                                                      |
|  Emergency Queue: [P001, P002, P003, P004, P005]                   |
|    ↑                                                            |
|  Current: P001                                                      |
|                                                                      |
|  DEPARTMENT UTILIZATION                                             |
|  =====================                                             |
|  Emergency:  ████████████████████░░░  75% (15/20 beds)              |
|  ICU:        ████████████████████░░░  80% (8/10 beds)              |
|  General:   ████████████████████████░  84% (42/50)              |
|  Pediatrics:█████████████████████░░░░░░  80% (12/15)              |
|  Cardiology:████████████████████████░░░  83% (10/12)              |
|                                                                      |
|  VISIT TRACKING (List of Tuples)                                     |
|  ==========================                                        |
|  (patient_id, date, department, doctor, status)                    |
|  [("P001", "2024-01-01", "Emergency", "Dr.Wilson", "Complete"),   |
|   ("P002", "2024-01-10", "Pediatrics", "Dr.Brown", "Complete"),  |
|   ("P003", "2024-01-05", "Cardiology", "Dr.Smith", "Complete")]  |
|                                                                      |
+====================================================================+
```

## Advanced Topics

### Advanced: Custom Data Structures

```python
from collections import deque
from heapq import heappush, heappop

# Priority Queue Implementation
class PriorityQueue:
    def __init__(self):
        self._heap = []
    
    def enqueue(self, priority, item):
        heappush(self._heap, (priority, item))
    
    def dequeue(self):
        if self._heap:
            return heappop(self._heap)[1]
        return None
    
    def is_empty(self):
        return len(self._heap) == 0
    
    def __len__(self):
        return len(self._heap)

# Circular Buffer for streaming data
class CircularBuffer:
    def __init__(self, capacity):
        self.capacity = capacity
        self.buffer = [None] * capacity
        self.head = 0
        self.tail = 0
        self.size = 0
    
    def append(self, item):
        self.buffer[self.tail] = item
        self.tail = (self.tail + 1) % self.capacity
        if self.size < self.capacity:
            self.size += 1
        else:
            self.head = (self.head + 1) % self.capacity
    
    def get_all(self):
        if self.size == 0:
            return []
        result = []
        for i in range(self.size):
            idx = (self.head + i) % self.capacity
            result.append(self.buffer[idx])
        return result

# LRU Cache Implementation
class LRUCache:
    def __init__(self, capacity):
        self.capacity = capacity
        self.cache = {}
        self.order = deque()
    
    def get(self, key):
        if key in self.cache:
            self.order.remove(key)
            self.order.append(key)
            return self.cache[key]
        return None
    
    def put(self, key, value):
        if key in self.cache:
            self.order.remove(key)
        elif len(self.cache) >= self.capacity:
            oldest = self.order.popleft()
            del self.cache[oldest]
        self.cache[key] = value
        self.order.append(key)
```

### Advanced: NumPy and Pandas for Large Data

```python
import numpy as np
import pandas as pd

# Efficient array operations
def batch_process(arr, batch_size=1000):
    for i in range(0, len(arr), batch_size):
        yield arr[i:i+batch_size]

# Using memory-mapped arrays for large files
large_array = np.memmap('large_data.dat', dtype='float32', mode='w+', shape=(1000000,))
large_array[:] = np.random.random(1000000)

# Efficient DataFrame operations using categoricals
df = pd.DataFrame({
    "category": ["A"] * 10000 + ["B"] * 10000 + ["C"] * 10000,
    "value": np.random.random(30000)
})
df["category"] = df["category"].astype("category")

# Using chunked processing for large files
def process_large_file(filepath, chunksize=10000):
    total = 0
    for chunk in pd.read_csv(filepath, chunksize=chunksize):
        total += chunk["value"].sum()
    return total

# Vectorized operations for performance
data = np.random.random(1000000)
result = np.where(data > 0.5, 1, 0)

# Using numba for JIT compilation
try:
    from numba import jit
    
    @jit(nopython=True)
    def fast_sum(arr):
        total = 0.0
        for i in range(len(arr)):
            total += arr[i]
        return total
    
    result = fast_sum(data)
except ImportError:
    print("Numba not available")
```

### Advanced: Tree and Graph Structures

```python
from collections import defaultdict, deque

# Binary Search Tree
class TreeNode:
    def __init__(self, value):
        self.value = value
        self.left = None
        self.right = None

class BinarySearchTree:
    def __init__(self):
        self.root = None
    
    def insert(self, value):
        if not self.root:
            self.root = TreeNode(value)
            return
        node = self.root
        while True:
            if value < node.value:
                if node.left is None:
                    node.left = TreeNode(value)
                    return
                node = node.left
            else:
                if node.right is None:
                    node.right = TreeNode(value)
                    return
                node = node.right
    
    def inorder_traversal(self):
        result = []
        self._inorder(self.root, result)
        return result
    
    def _inorder(self, node, result):
        if node:
            self._inorder(node.left, result)
            result.append(node.value)
            self._inorder(node.right, result)

# Graph using adjacency list
class Graph:
    def __init__(self):
        self.adjacency_list = defaultdict(list)
    
    def add_edge(self, u, v):
        self.adjacency_list[u].append(v)
        self.adjacency_list[v].append(u)
    
    def bfs(self, start):
        visited = set()
        queue = deque([start])
        result = []
        while queue:
            node = queue.popleft()
            if node not in visited:
                visited.add(node)
                result.append(node)
                queue.extend(self.adjacency_list[node])
        return result
    
    def dfs(self, start):
        visited = set()
        result = []
        self._dfs(start, visited, result)
        return result
    
    def _dfs(self, node, visited, result):
        visited.add(node)
        result.append(node)
        for neighbor in self.adjacency_list[node]:
            if neighbor not in visited:
                self._dfs(neighbor, visited, result)
```

## Conclusion

Data structures form the backbone of data science operations. This guide covered the fundamental built-in Python data structures including lists, tuples, sets, and dictionaries, along with the essential NumPy arrays and pandas DataFrames used in data science workflows.

Key takeaways include:
- **Lists**: Best for ordered collections that need modification
- **Sets**: Essential for unique values and membership operations
- **Dictionaries**: Ideal for key-value lookups and mappings
- **NumPy arrays**: Critical for numerical computing and vectorized operations
- **Pandas DataFrames**: The primary structure for tabular data analysis

The banking and healthcare applications demonstrated how these data structures solve real-world problems:
- Banking: Transaction processing, risk assessment, customer segmentation
- Healthcare: Patient records, resource allocation, condition tracking

Advanced topics like custom data structures, efficient large-scale processing, and graph structures provide foundations for more complex data science applications. Mastery of these structures enables efficient data manipulation, analysis, and the building of robust data science solutions.

Continue to the next module: Control Flow and Functions to learn how to organize your data processing logic.