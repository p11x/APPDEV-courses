# Python Fundamentals for Data Science

## Introduction

Python has emerged as the dominant programming language in the field of data science, and for good reason. Its simplicity, readability, and extensive ecosystem of libraries make it the preferred choice for data scientists, machine learning engineers, and analysts worldwide. This comprehensive guide will take you through the fundamental concepts of Python programming that are essential for any data science workflow.

The journey into data science begins with understanding the core Python language. Whether you're analyzing datasets, building machine learning models, or creating visualizations, Python provides the tools and flexibility needed to accomplish these tasks efficiently. The language's design philosophy emphasizes code readability and allows programmers to express concepts in fewer lines of code than would be possible in languages like C++ or Java.

Data science is an interdisciplinary field that combines statistics, programming, and domain expertise. Python serves as the bridge that connects these disciplines, enabling practitioners to seamlessly move from data collection and cleaning to analysis and modeling. The language's interpreted nature means you can execute code line-by-line, making it ideal for exploratory data analysis where you need to quickly test hypotheses and iterate on your approach.

The Python ecosystem for data science is vast and continues to grow. NumPy provides efficient numerical computing capabilities, Pandas offers powerful data manipulation structures, Matplotlib and Seaborn enable creating stunning visualizations, Scikit-learn supplies machine learning algorithms, and TensorFlow or PyTorch handle deep learning tasks. Understanding Python fundamentals is your first step toward mastering these tools.

This module covers essential Python concepts that form the foundation of data science work. You'll learn about variables and data types, operators and expressions, string manipulation, input/output operations, and basic control structures. Each concept is presented with practical examples drawn from real-world data science scenarios, ensuring you can apply what you learn immediately.

## Fundamentals

### Variables and Data Types

In Python, variables serve as containers for storing data values. Unlike other programming languages, Python has no command for declaring a variable; a variable is created the moment you assign a value to it. This dynamic typing means you don't need to specify the data type explicitly, as Python infers it from the assignment.

```python
# Integer variable - used for counting, indexing, and discrete values
transaction_count = 1500
customer_age = 34
account_balance = 25000.50

# Float variable - used for continuous values like prices, temperatures
interest_rate = 0.045
stock_price = 145.67
blood_pressure_systolic = 120.5

# String variable - used for text data like names, addresses, descriptions
customer_name = "John Smith"
email_address = "john.smith@example.com"
product_description = "High-performance laptop with 16GB RAM"

# Boolean variable - used for true/false conditions
is_active_customer = True
has_credit_card = False
is_high_risk = True

# Checking variable types
print(f"transaction_count type: {type(transaction_count)}")
print(f"interest_rate type: {type(interest_rate)}")
print(f"customer_name type: {type(customer_name)}")
print(f"is_active_customer type: {type(is_active_customer)}")

# Output:
# transaction_count type: <class 'int'>
# interest_rate type: <class 'float'>
# customer_name type: <class 'str'>
# is_active_customer type: <class 'bool'>
```

Python supports several basic data types that you'll encounter frequently in data science work. The integer type represents whole numbers without decimal points, perfect for counting records or indexing arrays. Float types handle decimal numbers essential for financial calculations, measurements, and statistical operations. Strings store text data, which is crucial for handling categorical variables and text analysis. Boolean values represent binary conditions that drive logical operations and filtering.

### Operators and Expressions

Operators allow you to perform operations on variables and values. Python supports various operator types including arithmetic, comparison, logical, and assignment operators. Understanding these operators is fundamental to performing calculations and making decisions in your code.

```python
# Arithmetic operators for mathematical calculations
a = 10
b = 3

addition_result = a + b          # 13
subtraction_result = a - b        # 7
multiplication_result = a * b     # 30
division_result = a / b           # 3.333...
floor_division_result = a // b    # 3 (quotient)
modulo_result = a % b             # 1 (remainder)
exponent_result = a ** b          # 1000 (10^3)

# Practical example: Calculating compound interest
principal = 10000
rate = 0.05
time_years = 5

# Simple interest calculation
simple_interest = principal * rate * time_years
total_simple = principal + simple_interest

# Compound interest calculation (annual compounding)
compound_interest = principal * ((1 + rate) ** time_years) - principal
total_compound = principal * (1 + rate) ** time_years

print(f"Principal: ${principal}")
print(f"Simple Interest: ${simple_interest:.2f}")
print(f"Total (Simple): ${total_simple:.2f}")
print(f"Compound Interest: ${compound_interest:.2f}")
print(f"Total (Compound): ${total_compound:.2f}")
```

Comparison operators return Boolean values and are essential for filtering data and making conditional decisions. These operators compare two values and determine the truth of a relationship. The equality operator checks if two values are equal, while the inequality operator checks for difference. Greater than and less than operators compare numerical values, and their equals variants include the boundary.

```python
# Comparison operators
age = 25
income = 50000
credit_score = 720

# Age-based comparisons
is_adult = age >= 18              # True
is_senior = age >= 65            # False
is_prime_age = 25 <= age <= 35    # True

# Financial comparisons
is_high_income = income > 75000  # False
is_low_income = income < 30000    # False
income_threshold = income > 50000 # False

# Credit score evaluations
excellent_credit = credit_score >= 800  # False
good_credit = 700 <= credit_score < 800 # True
fair_credit = credit_score >= 640       # True
poor_credit = credit_score < 640        # False

# Combining comparisons with logical operators
eligible_for_premium = (credit_score >= 700) and (income > 50000)
eligible_for_basic = (credit_score >= 640) or (income > 30000)
not_eligible = not (credit_score >= 640)

print(f"Eligible for premium: {eligible_for_premium}")
print(f"Eligible for basic: {eligible_for_basic}")
print(f"Not eligible: {not_eligible}")
```

### String Manipulation

Strings are fundamental to data science, handling everything from categorical data to text analysis. Python provides extensive string manipulation capabilities through built-in methods and the string formatting system. You'll frequently work with strings when cleaning data, extracting features from text, and preparing output reports.

```python
# String creation and basic operations
customer_name = "Alice Johnson"
company_name = "TechCorp Industries"

# String indexing - accessing individual characters
first_letter = customer_name[0]           # 'A'
last_letter = customer_name[-1]           # 'n'
middle_portion = customer_name[6:10]       # 'John'

# String methods
uppercase_name = customer_name.upper()     # 'ALICE JOHNSON'
lowercase_company = company_name.lower()    # 'techcorp industries'
title_case = customer_name.title()         # 'Alice Johnson'

# Splitting and joining strings
full_name = "John,Michael,Smith"
names_list = full_name.split(",")           # ['John', 'Michael', 'Smith']

words = ["Data", "Science", "Python"]
joined = " ".join(words)                    # 'Data Science Python'

# String formatting for data presentation
template = "Customer: {} | Account: {} | Balance: ${:.2f}"
formatted = template.format(customer_name, "ACC-12345", 5678.90)
# 'Customer: Alice Johnson | Account: ACC-12345 | Balance: $5678.90'

# f-strings (formatted string literals) - modern approach
account_summary = f"Account {account_id} has balance ${balance:,.2f}"
age_message = f"Customer age: {age} years (eligible: {age >= 18})"
```

```python
# Advanced string operations for data cleaning
raw_data = "  JOHN DOE | john.doe@email.com | +1-555-123-4567  "

# Strip whitespace
cleaned_name = raw_data.split("|")[0].strip()      # 'JOHN DOE'
cleaned_email = raw_data.split("|")[1].strip()     # 'john.doe@email.com'
cleaned_phone = raw_data.split("|")[2].strip()      # '+1-555-123-4567'

# Normalize text
normalized_name = cleaned_name.lower()             # 'john doe'
capitalized_name = normalized_name.title()          # 'John Doe'

# Email validation basics
is_valid_email = "@" in cleaned_email and "." in cleaned_email.split("@")[1]

# Phone number cleaning
digits_only = ''.join(filter(str.isdigit, cleaned_phone))  # '15551234567'
```

### Input and Output Operations

Data scientists regularly need to read data from files, APIs, and databases, then write results to various output formats. Understanding Python's input/output operations is crucial for building data pipelines. This section covers file operations, user input, and console output.

```python
# Reading data from files
# Text file reading
with open("customer_data.txt", "r") as file:
    content = file.read()
    lines = content.split("\n")

# CSV file handling with basic operations
import csv

# Reading CSV file
with open("sales_data.csv", "r", newline="") as csvfile:
    reader = csv.reader(csvfile)
    header = next(reader)  # Read header row
    data_rows = list(reader)
    
    print(f"Columns: {header}")
    print(f"Total rows: {len(data_rows)}")
    
    # Process each row
    for row in data_rows[:5]:  # Show first 5 rows
        print(row)

# Writing data to files
output_data = [
    ["Product", "Quantity", "Price", "Total"],
    ["Widget A", "100", "9.99", "999.00"],
    ["Widget B", "50", "19.99", "999.50"],
    ["Widget C", "200", "4.99", "998.00"]
]

with open("inventory_report.csv", "w", newline="") as outfile:
    writer = csv.writer(outfile)
    writer.writerows(output_data)
```

```python
# JSON file operations - common for API data and configuration
import json

# Reading JSON data
with open("config.json", "r") as config_file:
    config = json.load(config_file)
    
# Example JSON structure
sample_config = {
    "database": {
        "host": "localhost",
        "port": 5432,
        "name": "sales_db"
    },
    "logging": {
        "level": "INFO",
        "file": "app.log"
    },
    "parameters": {
        "batch_size": 1000,
        "timeout": 30
    }
}

# Writing JSON
with open("output_config.json", "w") as outfile:
    json.dump(sample_config, outfile, indent=4)

# Pretty printing JSON
print(json.dumps(sample_config, indent=4))
```

## Implementation

### Basic Control Structures

Control structures determine the flow of program execution. Understanding if-elif-else statements, loops, and conditional logic is essential for data processing tasks that require making decisions based on data values.

```python
# Conditional statements for data classification
credit_score = 685

# Simple if-else for binary classification
if credit_score >= 700:
    credit_rating = "Excellent"
else:
    credit_rating = "Needs Improvement"

# Multi-level classification using elif
if credit_score >= 800:
    rating = "Exceptional"
elif credit_score >= 740:
    rating = "Very Good"
elif credit_score >= 670:
    rating = "Good"
elif credit_score >= 580:
    rating = "Fair"
else:
    rating = "Poor"

print(f"Credit Score: {credit_score}")
print(f"Rating: {rating}")

# Processing multiple conditions
transaction_amount = 5000
customer_tier = "Gold"
is_verified = True

# Determine discount and processing priority
if is_verified:
    if customer_tier == "Platinum":
        discount = 0.20
        priority = "Highest"
    elif customer_tier == "Gold":
        discount = 0.15
        priority = "High"
    elif customer_tier == "Silver":
        discount = 0.10
        priority = "Medium"
    else:
        discount = 0.05
        priority = "Standard"
else:
    discount = 0.0
    priority = "Low"
    print("Warning: Unverified customer")

print(f"Discount: {discount*100}%")
print(f"Processing Priority: {priority}")
```

```python
# For loops for iteration and data processing
# Iterating over a list of numbers
data_points = [23, 45, 67, 89, 12, 34, 56, 78, 90, 11]

# Basic iteration
for value in data_points:
    print(f"Processing value: {value}")

# Finding statistics
total = 0
maximum = data_points[0]
minimum = data_points[0]

for value in data_points:
    total += value
    if value > maximum:
        maximum = value
    if value < minimum:
        minimum = value

average = total / len(data_points)
print(f"Sum: {total}, Average: {average:.2f}")
print(f"Max: {maximum}, Min: {minimum}")

# Iterating with enumeration
for index, value in enumerate(data_points):
    print(f"Index {index}: Value {value}")

# Range-based iteration
for i in range(10):
    print(f"Iteration {i}")
```

```python
# While loops for condition-based iteration
# Processing until condition is met
current_balance = 1000
target_balance = 5000
monthly_deposit = 300
months = 0

while current_balance < target_balance:
    current_balance += monthly_deposit
    months += 1
    if months > 100:  # Safety break
        print("Warning: Taking too long")
        break

print(f"Months to reach target: {months}")
print(f"Final balance: ${current_balance}")

# While with break and continue
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
processed = []
skipped = []

for num in numbers:
    if num % 3 == 0:
        skipped.append(num)  # Skip multiples of 3
        continue
    processed.append(num * 2)  # Double remaining numbers

print(f"Processed: {processed}")
print(f"Skipped: {skipped}")
```

### Banking and Healthcare Applications

Python fundamentals are extensively used in both banking and healthcare sectors. From processing transactions to analyzing patient data, the same core concepts apply across domains.

```python
# Banking Application: Transaction Processing
class BankAccount:
    def __init__(self, account_number, holder_name, initial_balance=0):
        self.account_number = account_number
        self.holder_name = holder_name
        self.balance = initial_balance
        self.transactions = []
        self.minimum_balance = 100
    
    def deposit(self, amount):
        if amount <= 0:
            print("Error: Deposit amount must be positive")
            return False
        self.balance += amount
        self.transactions.append({
            "type": "DEPOSIT",
            "amount": amount,
            "balance": self.balance
        })
        return True
    
    def withdraw(self, amount):
        if amount <= 0:
            print("Error: Withdrawal amount must be positive")
            return False
        if self.balance - amount < self.minimum_balance:
            print(f"Error: Cannot withdraw. Minimum balance of ${self.minimum_balance} required")
            return False
        self.balance -= amount
        self.transactions.append({
            "type": "WITHDRAWAL",
            "amount": amount,
            "balance": self.balance
        })
        return True
    
    def get_balance(self):
        return self.balance
    
    def get_transactions(self):
        return self.transactions

# Creating account and performing transactions
account = BankAccount("ACC-001", "John Smith", 5000)
account.deposit(1000)
account.withdraw(500)
account.withdraw(200)

print(f"Account Number: {account.account_number}")
print(f"Holder Name: {account.holder_name}")
print(f"Current Balance: ${account.get_balance()}")
print("\nTransaction History:")
for txn in account.get_transactions():
    print(f"  {txn['type']}: ${txn['amount']} | Balance: ${txn['balance']}")
```

```python
# Healthcare Application: Patient Record Management
class PatientRecord:
    def __init__(self, patient_id, name, date_of_birth, blood_type):
        self.patient_id = patient_id
        self.name = name
        self.date_of_birth = date_of_birth
        self.blood_type = blood_type
        self.vital_signs = []
        self.diagnoses = []
        self.medications = []
    
    def add_vital_signs(self, heart_rate, blood_pressure_systolic, 
                       blood_pressure_diastolic, temperature):
        vital = {
            "heart_rate": heart_rate,
            "bp_systolic": blood_pressure_systolic,
            "bp_diastolic": blood_pressure_diastolic,
            "temperature": temperature
        }
        self.vital_signs.append(vital)
        return vital
    
    def add_diagnosis(self, diagnosis, severity):
        entry = {
            "diagnosis": diagnosis,
            "severity": severity
        }
        self.diagnoses.append(entry)
        return entry
    
    def add_medication(self, medication, dosage, frequency):
        prescription = {
            "medication": medication,
            "dosage": dosage,
            "frequency": frequency
        }
        self.medications.append(prescription)
        return prescription
    
    def get_summary(self):
        return {
            "patient_id": self.patient_id,
            "name": self.name,
            "blood_type": self.blood_type,
            "total_visits": len(self.vital_signs),
            "diagnoses_count": len(self.diagnoses),
            "medications_count": len(self.medications)
        }

# Creating patient record
patient = PatientRecord("P-12345", "Jane Doe", "1985-03-15", "O+")

# Adding vital signs
patient.add_vital_signs(72, 120, 80, 98.6)
patient.add_vital_signs(75, 118, 78, 98.4)

# Adding diagnoses
patient.add_diagnosis("Hypertension", "Moderate")
patient.add_diagnosis("Type 2 Diabetes", "Mild")

# Adding medications
patient.add_medication("Lisinopril", "10mg", "Once daily")
patient.add_medication("Metformin", "500mg", "Twice daily")

summary = patient.get_summary()
print(f"Patient Summary:")
print(f"  ID: {summary['patient_id']}")
print(f"  Name: {summary['name']}")
print(f"  Blood Type: {summary['blood_type']}")
print(f"  Total Visits: {summary['total_visits']}")
print(f"  Active Diagnoses: {summary['diagnoses_count']}")
print(f"  Current Medications: {summary['medications_count']}")
```

### Output Results and Visualization

Understanding how to present data effectively is crucial for data scientists. Python provides various ways to format and display output, from simple console printing to sophisticated formatted reports.

```python
# Basic output formatting
print("=" * 60)
print("SALES REPORT")
print("=" * 60)

sales_data = [
    {"product": "Laptop", "quantity": 50, "price": 999.99, "revenue": 49999.50},
    {"product": "Phone", "quantity": 100, "price": 699.99, "revenue": 69999.00},
    {"product": "Tablet", "quantity": 30, "price": 449.99, "revenue": 13499.70},
    {"product": "Watch", "quantity": 75, "price": 299.99, "revenue": 22499.25},
]

print(f"{'Product':<15} {'Qty':>5} {'Price':>10} {'Revenue':>12}")
print("-" * 45)

total_revenue = 0
for item in sales_data:
    print(f"{item['product']:<15} {item['quantity']:>5} ${item['price']:>8.2f} ${item['revenue']:>10.2f}")
    total_revenue += item['revenue']

print("-" * 45)
print(f"{'TOTAL':<15} {'':>5} {'':>10} ${total_revenue:>10.2f}")
print("=" * 60)
```

```python
# ASCII-based data visualization
def create_bar_chart(data, max_width=40):
    """Create a simple ASCII bar chart"""
    print("\nASCII Bar Chart Visualization")
    print("-" * 50)
    
    for label, value in data:
        bar_length = int((value / max(data[1] for data in data)) * max_width)
        bar = "█" * bar_length
        print(f"{label:<15} |{bar} {value}")
    
    print("-" * 50)

# Sales data for visualization
sales_data = [
    ("Q1 2024", 45000),
    ("Q2 2024", 52000),
    ("Q3 2024", 48000),
    ("Q4 2024", 61000),
]

create_bar_chart(sales_data)

# Histogram representation
def create_histogram(values, bins=5):
    """Create ASCII histogram from numerical data"""
    min_val = min(values)
    max_val = max(values)
    bin_width = (max_val - min_val) / bins
    
    histogram = [0] * bins
    for val in values:
        bin_index = min(int((val - min_val) / bin_width), bins - 1)
        histogram[bin_index] += 1
    
    max_count = max(histogram)
    
    print("\nASCII Histogram")
    print("-" * 40)
    
    for i, count in enumerate(histogram):
        bin_start = min_val + i * bin_width
        bin_end = bin_start + bin_width
        bar = "█" * int((count / max_count) * 30)
        print(f"${bin_start:,.0f}-${bin_end:,.0f}: {bar} ({count})")
    
    print("-" * 40)

# Test histogram with transaction amounts
transaction_amounts = [150, 250, 350, 450, 550, 650, 750, 850, 950, 
                      1200, 1500, 2000, 2500, 3000, 3500]
create_histogram(transaction_amounts)
```

```python
# Table-based output formatting
def print_data_table(data, headers):
    """Print data in formatted table"""
    col_widths = []
    
    # Calculate column widths
    for i, header in enumerate(headers):
        max_width = len(header)
        for row in data:
            max_width = max(max_width, len(str(row[i])))
        col_widths.append(max_width)
    
    # Print header
    header_row = " | ".join(h.ljust(w) for h, w in zip(headers, col_widths))
    print(header_row)
    print("-" * len(header_row))
    
    # Print data rows
    for row in data:
        print(" | ".join(str(cell).ljust(w) for cell, w in zip(row, col_widths)))

# Sample data
customer_data = [
    ["ID", "Name", "City", "Balance", "Status"],
    ["001", "Alice Smith", "New York", "$5,000", "Active"],
    ["002", "Bob Johnson", "Los Angeles", "$3,200", "Active"],
    ["003", "Carol White", "Chicago", "$8,750", "Premium"],
    ["004", "David Brown", "Houston", "$1,500", "Inactive"],
]

print_data_table(customer_data[0], customer_data[1:])
```

### Advanced Topics

Advanced Python features like list comprehensions, generators, and decorators provide powerful tools for data science workflows. These features enable writing more efficient and elegant code.

```python
# List comprehensions - concise way to create lists
# Basic comprehension
squares = [x**2 for x in range(1, 11)]
# [1, 4, 9, 16, 25, 36, 49, 64, 81, 100]

# With condition - filtering data
even_squares = [x**2 for x in range(1, 11) if x % 2 == 0]
# [4, 16, 36, 64, 100]

# Nested comprehension - matrix flattening
matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
flat = [num for row in matrix for num in row]
# [1, 2, 3, 4, 5, 6, 7, 8, 9]

# Dictionary comprehension
prices = {"Laptop": 999, "Phone": 699, "Tablet": 449, "Watch": 299}
discounted = {item: price * 0.9 for item, price in prices.items()}
# {'Laptop': 899.1, 'Phone': 629.1, 'Tablet': 404.1, 'Watch': 269.1}

# Practical data science example
transactions = [
    {"id": 1, "amount": 150, "category": "food"},
    {"id": 2, "amount": 2500, "category": "electronics"},
    {"id": 3, "amount": 80, "category": "food"},
    {"id": 4, "amount": 450, "category": "clothing"},
    {"id": 5, "amount": 3200, "category": "electronics"},
]

# Filter and transform
high_value = [t for t in transactions if t["amount"] > 500]
electronics_total = sum(t["amount"] for t in transactions if t["category"] == "electronics")
category_totals = {}
for t in transactions:
    category_totals[t["category"]] = category_totals.get(t["category"], 0) + t["amount"]

print(f"High value transactions: {len(high_value)}")
print(f"Electronics total: ${electronics_total}")
print(f"Category breakdown: {category_totals}")
```

```python
# Generator functions - memory efficient iteration
def fibonacci(n):
    """Generate Fibonacci sequence"""
    a, b = 0, 1
    for _ in range(n):
        yield a
        a, b = b, a + b

# Using generator
fib = fibonacci(10)
print("Fibonacci sequence:")
for num in fib:
    print(num, end=" ")
# 0 1 1 2 3 5 8 13 21 34

def read_large_file(file_path):
    """Read large file line by line without loading into memory"""
    with open(file_path, 'r') as file:
        for line in file:
            yield line.strip()

def generate_batches(data, batch_size):
    """Yield batches of data for processing"""
    for i in range(0, len(data), batch_size):
        yield data[i:i + batch_size]

# Practical example: Processing large datasets in batches
large_dataset = list(range(1000))
print("\nProcessing in batches of 100:")
for batch_num, batch in enumerate(generate_batches(large_dataset, 100)):
    print(f"Batch {batch_num}: {len(batch)} items (indices {batch[0]}-{batch[-1]})")
```

```python
# Decorators - modifying function behavior
import time
from functools import wraps

def timing_decorator(func):
    """Measure function execution time"""
    @wraps(func)
    def wrapper(*args, **kwargs):
        start = time.time()
        result = func(*args, **kwargs)
        end = time.time()
        print(f"Execution time: {end - start:.4f} seconds")
        return result
    return wrapper

def retry(max_attempts=3, delay=1):
    """Retry function on failure"""
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            for attempt in range(max_attempts):
                try:
                    return func(*args, **kwargs)
                except Exception as e:
                    if attempt == max_attempts - 1:
                        raise e
                    print(f"Attempt {attempt + 1} failed, retrying...")
                    time.sleep(delay)
            return None
        return wrapper
    return decorator

@timing_decorator
def calculate_statistics(numbers):
    """Calculate basic statistics"""
    total = sum(numbers)
    average = total / len(numbers)
    return {"sum": total, "average": average, "count": len(numbers)}

@retry(max_attempts=3)
def fetch_data():
    """Simulate data fetch (could fail)"""
    import random
    if random.random() < 0.7:
        raise Exception("Connection failed")
    return {"data": [1, 2, 3, 4, 5]}

# Testing decorators
data = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]
result = calculate_statistics(data)
print(f"Statistics: {result}")
```

```python
# Context managers - resource management
from contextlib import contextmanager

@contextmanager
def timer(name):
    """Time code block execution"""
    start = time.time()
    yield
    end = time.time()
    print(f"{name} took {end - start:.4f} seconds")

# Using context manager
with timer("Data Processing"):
    # Simulate data processing
    result = sum(range(1000000))
    print(f"Processed result: {result}")

class DataProcessor:
    """Context manager for data processing operations"""
    def __init__(self, filename):
        self.filename = filename
        self.file = None
    
    def __enter__(self):
        self.file = open(self.filename, 'w')
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if self.file:
            self.file.close()
        return False
    
    def write(self, data):
        self.file.write(str(data) + "\n")

# Using custom context manager
with DataProcessor("output.txt") as processor:
    processor.write("Data line 1")
    processor.write("Data line 2")
    processor.write("Data line 3")

print("Data written successfully")
```

## Conclusion

Python fundamentals form the bedrock of all data science work. Through this comprehensive exploration, you've gained essential knowledge covering variables, data types, operators, control structures, and advanced features like comprehensions, generators, and decorators. These skills enable you to manipulate data, implement algorithms, and build data processing pipelines.

The banking and healthcare examples demonstrate how these fundamental concepts translate to real-world applications. Whether processing transactions, managing patient records, or analyzing financial data, the same Python patterns apply. Mastery of these basics prepares you for more advanced topics in data science libraries like NumPy, Pandas, and beyond.

Continue practicing these fundamentals by working on real datasets. Build small projects that involve data cleaning, transformation, and analysis. The proficiency you develop here will accelerate your learning of specialized data science tools and techniques in subsequent modules.