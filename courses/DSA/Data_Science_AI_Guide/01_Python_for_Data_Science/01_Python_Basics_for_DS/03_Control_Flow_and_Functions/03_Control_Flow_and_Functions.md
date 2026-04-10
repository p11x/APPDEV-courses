# Control Flow and Functions

## Introduction

Control flow and functions are fundamental concepts in Python programming that enable you to write dynamic, efficient, and reusable code. Control flow statements determine the order in which your code executes, while functions provide modular blocks of code that can be reused throughout your programs. In data science, these concepts are essential for implementing algorithms, processing data, and building analytical pipelines.

Control flow in Python includes conditional statements, loops, and exception handling. Conditional statements allow your program to make decisions based on certain conditions, enabling dynamic behavior. Loops enable repetitive execution of code blocks, which is crucial for iterating over datasets and performing batch operations. Exception handling provides graceful error management, ensuring your programs can handle unexpected situations without crashing.

Functions are reusable blocks of code that perform specific tasks. They help organize code, reduce duplication, and make programs easier to understand and maintain. In data science, functions are used extensively for data preprocessing, feature engineering, model training, and result analysis. Understanding how to write effective functions is a critical skill for any data scientist.

This guide covers control flow statements, function definitions, parameters, return values, lambda functions, and practical applications in banking and healthcare domains.

## Fundamentals

### Conditional Statements

Conditional statements allow you to execute different code blocks based on specified conditions. Python provides three types of conditional statements: if, elif, and else. The if statement checks a condition and executes code if the condition is true. The elif (else if) statement provides additional conditions to check. The else statement executes code when none of the previous conditions are true.

```python
# Basic if statement
temperature = 75
if temperature > 70:
    print("It's warm")

# If-else statement
score = 75
if score >= 60:
    print("Pass")
else:
    print("Fail")

# If-elif-else chain
credit_score = 720
if credit_score >= 800:
    risk_level = "Excellent"
elif credit_score >= 700:
    risk_level = "Good"
elif credit_score >= 600:
    risk_level = "Fair"
else:
    risk_level = "Poor"
```

### Loops

Loops enable repetitive execution of code blocks. Python provides two types of loops: for loops and while loops. For loops iterate over sequences like lists, tuples, or ranges. While loops continue executing as long as a condition remains true.

```python
# For loop over a list
 transactions = [100, 200, 300, 400]
for transaction in transactions:
    print(f"Processing: ${transaction}")

# For loop with range
for i in range(5):
    print(f"Iteration {i}")

# While loop
balance = 1000
while balance > 0:
    print(f"Balance: ${balance}")
    balance -= 100

# Nested loops
for i in range(3):
    for j in range(3):
        print(f"({i}, {j})")
```

### Loop Control Statements

Python provides three loop control statements: break, continue, and pass. The break statement terminates the loop early. The continue statement skips the current iteration and moves to the next one. The pass statement is a placeholder that does nothing.

```python
# Break statement
numbers = [1, 2, 3, 4, 5]
for num in numbers:
    if num == 3:
        break
    print(num)

# Continue statement
numbers = [1, 2, 3, 4, 5]
for num in numbers:
    if num % 2 == 0:
        continue
    print(f"Odd number: {num}")

# Pass statement (placeholder)
def empty_function():
    pass
```

### Functions

Functions are reusable blocks of code that perform specific tasks. They are defined using the def keyword followed by the function name and parameters. Functions can accept inputs (parameters), perform operations, and return outputs (values).

```python
# Basic function
def greet():
    print("Hello, World!")

# Function with parameters
def greet_person(name):
    print(f"Hello, {name}!")

# Function with return value
def add(a, b):
    return a + b

# Function with multiple parameters
def calculate_total(items, tax_rate=0.1):
    subtotal = sum(items)
    tax = subtotal * tax_rate
    return subtotal + tax
```

### Function Parameters

Python supports various types of function parameters: positional arguments, keyword arguments, default arguments, variable-length arguments (*args), and keyword-variable arguments (**kwargs).

```python
# Default arguments
def calculate_interest(principal, rate=0.05, years=1):
    return principal * rate * years

# Variable-length arguments
def sum_all(*args):
    return sum(args)

# Keyword arguments
def create_profile(name, age, city="Unknown"):
    return {"name": name, "age": age, "city": city}

# Keyword-variable arguments
def print_info(**kwargs):
    for key, value in kwargs.items():
        print(f"{key}: {value}")
```

### Lambda Functions

Lambda functions are small anonymous functions defined using the lambda keyword. They are typically used for short, simple operations where a full function definition would be excessive.

```python
# Basic lambda
square = lambda x: x ** 2
print(square(5))

# Lambda with multiple parameters
add = lambda a, b: a + b
print(add(3, 4))

# Lambda with conditional
grade = lambda score: "Pass" if score >= 60 else "Fail"
print(grade(75))

# Lambda with sorted
names = ["Bob", "Alice", "John"]
sorted_names = sorted(names, key=lambda x: len(x))
print(sorted_names)
```

## Implementation

### Banking: Transaction Processing

```python
# Transaction data
transactions = [
    {"id": "T001", "customer": "C001", "amount": 500, "type": "deposit"},
    {"id": "T002", "customer": "C002", "amount": -200, "type": "withdrawal"},
    {"id": "T003", "customer": "C001", "amount": -100, "type": "transfer"},
    {"id": "T004", "customer": "C003", "amount": 1000, "type": "deposit"},
    {"id": "T005", "customer": "C002", "amount": -50, "type": "payment"},
]

# Function to process single transaction
def process_transaction(transaction):
    if transaction["type"] == "deposit":
        return f"Added ${transaction['amount']} to account"
    elif transaction["type"] == "withdrawal":
        return f"Withdrew ${abs(transaction['amount'])} from account"
    elif transaction["type"] == "transfer":
        return f"Transferred ${abs(transaction['amount'])} to another account"
    elif transaction["type"] == "payment":
        return f"Payment of ${abs(transaction['amount'])} processed"
    else:
        return "Unknown transaction type"

# Process all transactions
for transaction in transactions:
    result = process_transaction(transaction)
    print(f"{transaction['id']}: {result}")

# Function to calculate account balance
def calculate_balance(transactions, customer_id):
    balance = 0
    for transaction in transactions:
        if transaction["customer"] == customer_id:
            balance += transaction["amount"]
    return balance

# Calculate balances for each customer
customers = ["C001", "C002", "C003"]
for customer in customers:
    balance = calculate_balance(transactions, customer)
    print(f"{customer}: ${balance}")
```

### Banking: Risk Assessment

```python
# Customer risk data
customers = [
    {"id": "C001", "credit_score": 750, "income": 80000, "debt": 15000},
    {"id": "C002", "credit_score": 680, "income": 55000, "debt": 25000},
    {"id": "C003", "credit_score": 520, "income": 35000, "debt": 18000},
    {"id": "C004", "credit_score": 800, "income": 95000, "debt": 10000},
    {"id": "C005", "credit_score": 620, "income": 45000, "debt": 30000},
]

# Function to calculate debt-to-income ratio
def calculate_dti(debt, income):
    return debt / income

# Function to determine risk category
def get_risk_category(credit_score, dti):
    if credit_score >= 750 and dti < 0.3:
        return "Low"
    elif credit_score >= 650 and dti < 0.5:
        return "Medium"
    else:
        return "High"

# Process each customer
print("Risk Assessment Results:")
print("-" * 50)
for customer in customers:
    customer_id = customer["id"]
    credit_score = customer["credit_score"]
    debt = customer["debt"]
    income = customer["income"]
    
    dti = calculate_dti(debt, income)
    risk = get_risk_category(credit_score, dti)
    
    print(f"Customer: {customer_id}")
    print(f"  Credit Score: {credit_score}")
    print(f"  DTI Ratio: {dti:.2%}")
    print(f"  Risk Level: {risk}")
    print()

# Function to filter high-risk customers
def filter_high_risk(customers):
    high_risk = []
    for customer in customers:
        dti = calculate_dti(customer["debt"], customer["income"])
        if get_risk_category(customer["credit_score"], dti) == "High":
            high_risk.append(customer["id"])
    return high_risk

high_risk_customers = filter_high_risk(customers)
print(f"High Risk Customers: {high_risk_customers}")
```

### Healthcare: Patient Triage

```python
# Patient data
patients = [
    {"id": "P001", "name": "John", "condition": "critical", "wait_time": 0},
    {"id": "P002", "name": "Jane", "condition": "severe", "wait_time": 15},
    {"id": "P003", "name": "Bob", "condition": "moderate", "wait_time": 30},
    {"id": "P004", "name": "Alice", "condition": "minor", "wait_time": 45},
    {"id": "P005", "name": "Charlie", "condition": "severe", "wait_time": 10},
]

# Function to determine priority
def get_priority(condition):
    priorities = {
        "critical": 1,
        "severe": 2,
        "moderate": 3,
        "minor": 4
    }
    return priorities.get(condition, 5)

# Function to assess urgency
def assess_urgency(patient):
    priority = get_priority(patient["condition"])
    wait_time = patient["wait_time"]
    
    if priority == 1:
        return "IMMEDIATE"
    elif priority == 2 and wait_time > 20:
        return "URGENT - escalated"
    elif priority == 2:
        return "URGENT"
    elif wait_time > 60:
        return "Monitor closely"
    else:
        return "Standard"

# Process patient triage
print("Patient Triage Results:")
print("-" * 50)
for patient in patients:
    urgency = assess_urgency(patient)
    priority = get_priority(patient["condition"])
    print(f"Patient: {patient['name']} ({patient['id']})")
    print(f"  Condition: {patient['condition']}")
    print(f"  Priority: {priority}")
    print(f"  Wait Time: {patient['wait_time']} min")
    print(f"  Assessment: {urgency}")
    print()

# Sort patients by priority
sorted_patients = sorted(patients, key=lambda p: (get_priority(p["condition"]), p["wait_time"]))
print("Treatment Order:")
for i, patient in enumerate(sorted_patients, 1):
    print(f"  {i}. {patient['name']} - {patient['condition']}")
```

### Healthcare: Lab Results Processing

```python
# Lab results data
lab_results = [
    {"patient_id": "P001", "test": "glucose", "value": 95, "unit": "mg/dL", "date": "2024-01-01"},
    {"patient_id": "P001", "test": "cholesterol", "value": 210, "unit": "mg/dL", "date": "2024-01-01"},
    {"patient_id": "P002", "test": "glucose", "value": 85, "unit": "mg/dL", "date": "2024-01-10"},
    {"patient_id": "P003", "test": "glucose", "value": 140, "unit": "mg/dL", "date": "2024-01-05"},
    {"patient_id": "P003", "test": "cholesterol", "value": 280, "unit": "mg/dL", "date": "2024-01-05"},
]

# Reference ranges
reference_ranges = {
    "glucose": {"low": 70, "high": 100, "unit": "mg/dL"},
    "cholesterol": {"low": 0, "high": 200, "unit": "mg/dL"},
}

# Function to check if value is normal
def is_normal(test, value):
    if test not in reference_ranges:
        return True
    range_info = reference_ranges[test]
    return range_info["low"] <= value <= range_info["high"]

# Function to classify result status
def classify_result(test, value):
    if test not in reference_ranges:
        return "Unknown"
    range_info = reference_ranges[test]
    if value < range_info["low"]:
        return "Low"
    elif value > range_info["high"]:
        return "High"
    else:
        return "Normal"

# Process lab results
print("Lab Results Analysis:")
print("-" * 60)
for result in lab_results:
    patient_id = result["patient_id"]
    test = result["test"]
    value = result["value"]
    unit = result["unit"]
    
    status = classify_result(test, value)
    normal = is_normal(test, value)
    
    flag = " ⚠️" if not normal else ""
    print(f"Patient: {patient_id}")
    print(f"  Test: {test}")
    print(f"  Value: {value} {unit}")
    print(f"  Status: {status}{flag}")
    print()

# Function to find abnormal results
def find_abnormal_results(results):
    abnormal = []
    for result in results:
        if not is_normal(result["test"], result["value"]):
            abnormal.append(result)
    return abnormal

abnormal_results = find_abnormal_results(lab_results)
print(f"Abnormal Results: {len(abnormal_results)} found")
for result in abnormal_results:
    print(f"  - {result['patient_id']}: {result['test']} = {result['value']}")
```

### Data Science: Feature Engineering Functions

```python
import math

# Function to normalize values
def normalize(value, min_val, max_val):
    return (value - min_val) / (max_val - min_val)

# Function to standardize values
def standardize(value, mean, std):
    return (value - mean) / std

# Function to calculate z-score
def z_score(value, mean, std):
    return (value - mean) / std if std != 0 else 0

# Dataset
data = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]
mean = sum(data) / len(data)
std = math.sqrt(sum((x - mean) ** 2 for x in data) / len(data))
min_val = min(data)
max_val = max(data)

print("Feature Engineering Examples:")
print("-" * 50)
print(f"Original data: {data}")
print(f"Mean: {mean:.2f}, Std: {std:.2f}")
print(f"Min: {min_val}, Max: {max_val}")
print()

print("Normalized (0-1 scale):")
normalized = [normalize(x, min_val, max_val) for x in data]
print(normalized)
print()

print("Standardized (z-scores):")
standardized = [standardize(x, mean, std) for x in data]
print([round(z, 2) for z in standardized])
print()

# Function to bin continuous values
def bin_values(value, bins):
    for i, (lower, upper) in enumerate(bins):
        if lower <= value < upper:
            return i
    return len(bins)

bins = [(0, 25), (25, 50), (50, 75), (75, 100)]
binned = [bin_values(x, bins) for x in data]
print("Binned values:")
print(binned)
```

## Applications in Banking

### Banking Application: Loan Approval System

```python
import math

# Loan applications data
loan_applications = [
    {"app_id": "L001", "customer_id": "C001", "amount": 50000, "credit_score": 750, "income": 80000, "debt": 15000},
    {"app_id": "L002", "customer_id": "C002", "amount": 30000, "credit_score": 680, "income": 55000, "debt": 25000},
    {"app_id": "L003", "customer_id": "C003", "amount": 15000, "credit_score": 520, "income": 35000, "debt": 18000},
    {"app_id": "L004", "customer_id": "C004", "amount": 75000, "credit_score": 800, "income": 95000, "debt": 10000},
    {"app_id": "L005", "customer_id": "C005", "amount": 20000, "credit_score": 620, "income": 45000, "debt": 30000},
]

# Loan eligibility rules
MIN_CREDIT_SCORE = 620
MAX_DTI_RATIO = 0.5
MIN_INCOME = 30000

# Function to calculate monthly payment
def calculate_monthly_payment(principal, annual_rate, years):
    monthly_rate = annual_rate / 100 / 12
    months = years * 12
    if monthly_rate == 0:
        return principal / months
    payment = principal * (monthly_rate * (1 + monthly_rate) ** months) / ((1 + monthly_rate) ** months - 1)
    return payment

# Function to calculate DTI ratio
def calculate_dti(debt, income):
    return debt / income

# Function to determine interest rate
def get_interest_rate(credit_score):
    if credit_score >= 750:
        return 4.5
    elif credit_score >= 700:
        return 5.5
    elif credit_score >= 650:
        return 6.5
    else:
        return 8.0

# Function to check eligibility
def check_eligibility(application):
    credit_score = application["credit_score"]
    debt = application["debt"]
    income = application["income"]
    amount = application["amount"]
    
    # Check credit score
    if credit_score < MIN_CREDIT_SCORE:
        return False, "Credit score too low"
    
    # Calculate and check DTI
    dti = calculate_dti(debt, income)
    if dti > MAX_DTI_RATIO:
        return False, "DTI ratio too high"
    
    # Check income
    if income < MIN_INCOME:
        return False, "Income too low"
    
    return True, "Eligible"

# Process applications
print("Loan Application Processing:")
print("=" * 70)

for app in loan_applications:
    app_id = app["app_id"]
    customer_id = app["customer_id"]
    amount = app["amount"]
    credit_score = app["credit_score"]
    income = app["income"]
    debt = app["debt"]
    
    # Check eligibility
    eligible, status = check_eligibility(app)
    
    print(f"\nApplication: {app_id}")
    print(f"  Customer: {customer_id}")
    print(f"  Requested Amount: ${amount:,}")
    print(f"  Credit Score: {credit_score}")
    print(f"  Income: ${income:,}")
    print(f"  Existing Debt: ${debt:,}")
    
    if eligible:
        interest_rate = get_interest_rate(credit_score)
        monthly_payment = calculate_monthly_payment(amount, interest_rate, 5)
        
        print(f"  Status: ✅ APPROVED")
        print(f"  Interest Rate: {interest_rate}%")
        print(f"  Monthly Payment: ${monthly_payment:.2f}")
    else:
        print(f"  Status: ❌ DENIED - {status}")
```

### Banking Application: ATM Withdrawal Logic

```python
# ATM withdrawal limits
DAILY_WITHDRAWAL_LIMIT = 1000
TRANSACTION_LIMIT = 500
MIN_WITHDRAWAL = 20
MAX_WITHDRAWAL = 500

# Customer withdrawal history
withdrawal_history = [
    {"customer_id": "C001", "date": "2024-01-01", "amount": 200},
    {"customer_id": "C001", "date": "2024-01-01", "amount": 300},
    {"customer_id": "C002", "date": "2024-01-02", "amount": 150},
    {"customer_id": "C003", "date": "2024-01-01", "amount": 500},
]

# Function to get daily withdrawal total
def get_daily_withdrawal(customer_id, history, date):
    total = 0
    for withdrawal in history:
        if withdrawal["customer_id"] == customer_id and withdrawal["date"] == date:
            total += withdrawal["amount"]
    return total

# Function to validate withdrawal
def validate_withdrawal(customer_id, amount, history, date):
    errors = []
    
    # Check minimum amount
    if amount < MIN_WITHDRAWAL:
        errors.append(f"Minimum withdrawal is ${MIN_WITHDRAWAL}")
    
    # Check maximum amount
    if amount > MAX_WITHDRAWAL:
        errors.append(f"Maximum single withdrawal is ${MAX_WITHDRAWAL}")
    
    # Check daily limit
    daily_total = get_daily_withdrawal(customer_id, history, date)
    remaining_daily = DAILY_WITHDRAWAL_LIMIT - daily_total
    
    if amount > remaining_daily:
        errors.append(f"Daily limit exceeded. Remaining: ${remaining_daily}")
    
    return errors, remaining_daily

# Function to process withdrawal
def process_withdrawal(customer_id, amount, history, date):
    errors, remaining = validate_withdrawal(customer_id, amount, history, date)
    
    if errors:
        return {"success": False, "errors": errors}
    
    return {"success": True, "amount": amount, "remaining_daily": remaining - amount}

# Test withdrawal scenarios
print("ATM Withdrawal Processing:")
print("=" * 60)

test_cases = [
    ("C001", 200, "2024-01-01"),
    ("C001", 400, "2024-01-01"),
    ("C002", 100, "2024-01-02"),
    ("C003", 400, "2024-01-02"),
]

for customer_id, amount, date in test_cases:
    result = process_withdrawal(customer_id, amount, withdrawal_history, date)
    
    print(f"\nCustomer: {customer_id}")
    print(f"  Requested: ${amount}")
    print(f"  Date: {date}")
    
    if result["success"]:
        print(f"  Status: ✅ APPROVED")
        print(f"  Amount dispensed: ${result['amount']}")
        print(f"  Remaining daily: ${result['remaining_daily']}")
    else:
        print(f"  Status: ❌ DENIED")
        for error in result["errors"]:
            print(f"    - {error}")
```

## Applications in Healthcare

### Healthcare Application: Prescription Processing

```python
# Prescription database
prescriptions = [
    {"rx_id": "RX001", "patient_id": "P001", "medication": "metformin", "dosage": "500mg", "frequency": "twice daily", "status": "active"},
    {"rx_id": "RX002", "patient_id": "P001", "medication": "lisinopril", "dosage": "10mg", "frequency": "once daily", "status": "active"},
    {"rx_id": "RX003", "patient_id": "P002", "medication": "albuterol", "dosage": "90mcg", "frequency": "as needed", "status": "active"},
    {"rx_id": "RX004", "patient_id": "P003", "medication": "metoprolol", "dosage": "25mg", "frequency": "twice daily", "status": "active"},
    {"rx_id": "RX005", "patient_id": "P003", "medication": "aspirin", "dosage": "81mg", "frequency": "once daily", "status": "active"},
]

# Medication database with interactions
medication_interactions = {
    ("metformin", "alcohol"): "Avoid alcohol while taking metformin",
    ("lisinopril", "potassium"): "May increase potassium levels",
    ("metoprolol", "albuterol"): "May reduce effectiveness of albuterol",
}

# Function to calculate refills remaining
def refills_remaining(rx_date, quantity, daily_dose, refills_allowed):
    return max(0, refills_allowed - 1)

# Function to check interactions
def check_interactions(prescriptions, new_medication):
    interactions = []
    for rx in prescriptions:
        if rx["status"] != "active":
            continue
        key = (rx["medication"], new_medication.lower())
        reverse_key = (new_medication.lower(), rx["medication"])
        if key in medication_interactions:
            interactions.append(medication_interactions[key])
        if reverse_key in medication_interactions:
            interactions.append(medication_interactions[reverse_key])
    return interactions

# Function to process prescription refill
def process_refill(prescription):
    if prescription["status"] != "active":
        return {"success": False, "message": "Prescription not active"}
    
    return {"success": True, "message": "Refill approved"}

# Process prescriptions
print("Prescription Management:")
print("=" * 60)

for rx in prescriptions:
    rx_id = rx["rx_id"]
    patient_id = rx["patient_id"]
    medication = rx["medication"]
    dosage = rx["dosage"]
    frequency = rx["frequency"]
    
    print(f"\nPrescription: {rx_id}")
    print(f"  Patient: {patient_id}")
    print(f"  Medication: {medication}")
    print(f"  Dosage: {dosage}")
    print(f"  Frequency: {frequency}")
    print(f"  Status: {rx['status']}")

# Check potential interactions
print("\n" + "=" * 60)
print("Interaction Check:")
print("  Checking if metoprolol interacts with albuterol...")

p1_prescriptions = [rx for rx in prescriptions if rx["patient_id"] == "P003"]
interactions = check_interactions(p1_prescriptions, "albuterol")

if interactions:
    for interaction in interactions:
        print(f"  ⚠️  {interaction}")
else:
    print("  ✓ No interactions found")
```

## Output Results

### Sample Output: Control Flow and Functions

```
Transaction Processing Results:
T001: Added $500 to account
T002: Withdrew $200 from account
T003: Transferred $100 to another account
T004: Added $1000 to account
T005: Payment of $50 processed

Risk Assessment Results:
--------------------------------------------------
Customer: C001
  Credit Score: 750
  DTI Ratio: 18.75%
  Risk Level: Medium

Customer: C002
  Credit Score: 680
  DTI Ratio: 45.45%
  Risk Level: Medium

Patient Triage Results:
--------------------------------------------------
Patient: John (P001)
  Condition: critical
  Priority: 1
  Wait Time: 0 min
  Assessment: IMMEDIATE

Patient: Jane (P002)
  Condition: severe
  Priority: 2
  Wait Time: 15 min
  Assessment: URGENT

Lab Results Analysis:
--------------------------------------------------------------
Patient: P001
  Test: glucose
  Value: 95 mg/dL
  Status: Normal

Patient: P001
  Test: cholesterol
  Value: 210 mg/dL
  Status: High ⚠️
```

### Sample Output: Banking Applications

```
Loan Application Processing:
======================================================================

Application: L001
  Customer: C001
  Requested Amount: $50,000
  Credit Score: 750
  Income: $80,000
  Existing Debt: $15,000
  Status: ✅ APPROVED
  Interest Rate: 4.5%
  Monthly Payment: $932.15

Application: L002
  Customer: C002
  Requested Amount: $30,000
  Credit Score: 680
  Income: $55,000
  Existing Debt: $25,000
  Status: ✅ APPROVED
  Interest Rate: 8.0%
  Monthly Payment: $608.44

Application: L003
  Customer: C003
  Requested Amount: $15,000
  Credit Score: 520
  Income: $35,000
  Existing Debt: $18,000
  Status: ❌ DENIED - Credit score too low
```

## Visualization

### ASCII: Control Flow Structure

```
+====================================================================+
|                   CONTROL FLOW STRUCTURE DIAGRAM                    |
+====================================================================+
|                                                                      |
|  CONDITIONAL STATEMENTS                                               |
|  =====================                                               |
|                                                                      |
|  +---------+                                                       |
|  | Condition|----True----> [Execute Block 1]                        |
|  +---------+                                                       |
|       |                                                            |
|     False                                                           |
|       |                                                            |
|  +---------+                                                       |
|  | Condition 2|----True--> [Execute Block 2]                   |
|  +---------+                                                       |
|       |                                                            |
|     False                                                          |
|       |                                                            |
|  +---------+                                                       |
|  |   Else   |-------> [Execute Default Block]                   |
|  +---------+                                                       |
|                                                                      |
|  FOR LOOP STRUCTURE                                                 |
|  ===============                                                   |
|                                                                      |
|  +---------+                                                       |
|  | for item |-----> +----------+                                   |
|  |   in    |      | Process   |                                   |
|  | sequence|      |   item    |                                   |
|  +---------+      +----------+                                   |
|       |                 |                                         |
|     More |----True-------> +                                         |
|       |                 |                                         |
|     Done +---------> [Exit Loop]                               |
|                                                                      |
|  WHILE LOOP STRUCTURE                                               |
|  ===============                                                   |
|                                                                      |
|  +---------+                                                       |
|  |  while  |----True----> +----------+                           |
|  | condition|             | Process   |                           |
|  +---------+             +----------+                           |
|       |                     |                                    |
|     False-----------------> +                                  |
|                                                                      |
+====================================================================+
```

### ASCII: Function Call Flow

```
+====================================================================+
|                   FUNCTION CALL FLOW DIAGRAM                         |
+====================================================================+
|                                                                      |
|  FUNCTION DEFINITION                                               |
|  ==================                                                |
|                                                                      |
|  +------------------+                                              |
|  | def function    |                                              |
|  | (params):       |                                              |
|  +------------------+                                              |
|       |                                                           |
|  +------------------+                                              |
|  |    Process      |                                              |
|  |    Logic       |                                              |
|  +------------------+                                              |
|       |                                                           |
|  +------------------+                                              |
|  | return value  |                                              |
|  +------------------+                                              |
|                                                                      |
|  FUNCTION CALL SEQUENCE                                            |
|  ================                                                  |
|                                                                      |
|  Caller                                                            |
|     |                                                              |
|     v                                                              |
|  +--------+  arguments                                             |
|  | Call   |----------------+                                       |
|  | foo(x) |                |                                       |
|  +--------+                v                                       |
|                   +------------------+                              |
|                   | Parameter: x = 5 |                           |
|                   +------------------+                              |
|                         |                                         |
|                   +------------------+                              |
|                   | Function Body    |                           |
|                   | Processing...    |                           |
|                   +------------------+                              |
|                         |                                         |
|                   +------------------+                              |
|                   | return result   |                           |
|                   | = 25             |                           |
|                   +------------------+                              |
|                         |                                         |
|     +-------------------+                                         |
|     |                                                              |
|     v                                                              |
|  Caller gets result = 25                                         |
|                                                                      |
+====================================================================+
```

### ASCII: Banking Transaction Flow

```
+====================================================================+
|              BANKING TRANSACTION FLOW DIAGRAM                      |
+====================================================================+
|                                                                      |
|  TRANSACTION PROCESSING PIPELINE                                    |
|  ==============================                                    |
|                                                                      |
|  +-----------+                                                     |
|  |  Customer |                                                     |
|  |  Request  |                                                     |
|  +-----------+                                                     |
|       |                                                            |
|       v                                                            |
|  +-----------+                                                     |
|  | Validate |----Fail----> [Reject + Notify]                      |
|  | Request  |                                                     |
|  +-----------+                                                     |
|       |                                                            |
|      Pass                                                          |
|       |                                                            |
|       v                                                            |
|  +-----------+                                                     |
|  |  Check   |----Fail----> [Request More Info]                    |
|  |  Limits  |                                                     |
|  +-----------+                                                     |
|       |                                                            |
|      Pass                                                          |
|       |                                                            |
|       v                                                            |
|  +-----------+                                                     |
|  | Process  |----Fail----> [Rollback + Error]                       |
|  | Transaction|                                                   |
|  +-----------+                                                     |
|       |                                                            |
|      Success                                                       |
|       |                                                            |
|       v                                                            |
|  +-----------+                                                     |
|  |  Update  |                                                     |
|  | Accounts|                                                     |
|  +-----------+                                                     |
|       |                                                            |
|       v                                                            |
|  +-----------+                                                     |
|  |  Notify  |                                                     |
|  | Customer|                                                     |
|  +-----------+                                                     |
|       |                                                            |
|       v                                                            |
|  [Complete]                                                       |
|                                                                      |
|  VALIDATION LOGIC                                                  |
|  ==============                                                   |
|                                                                      |
|  Input: amount = 500                                               |
|       |                                                          |
|       v                                                          |
|  Is amount > 0?  ----No-----> Error                               |
|       |                                                            |
|      Yes                                                           |
|       |                                                            |
|       v                                                          |
|  Is amount <= max?  ----No-----> Error                               |
|       |                                                            |
|      Yes                                                           |
|       |                                                            |
|       v                                                          |
|  Is daily limit ok?  ----No-----> Error                            |
|       |                                                            |
|      Yes                                                           |
|       |                                                            |
|       v                                                          |
|  [Proceed to Processing]                                        |
|                                                                      |
+====================================================================+
```

## Advanced Topics

### Advanced: Decorators

```python
import time
from functools import wraps

# Timing decorator
def timer(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        start = time.time()
        result = func(*args, **kwargs)
        end = time.time()
        print(f"Function {func.__name__} took {end - start:.4f} seconds")
        return result
    return wrapper

# Caching decorator
def cache(func):
    stored_results = {}
    @wraps(func)
    def wrapper(*args):
        if args not in stored_results:
            stored_results[args] = func(*args)
        return stored_results[args]
    return wrapper

# Validation decorator
def validate_args(**validators):
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            for arg_name, validator in validators.items():
                if arg_name in kwargs:
                    value = kwargs[arg_name]
                    if not validator(value):
                        raise ValueError(f"Invalid value for {arg_name}")
            return func(*args, **kwargs)
        return wrapper
    return decorator

# Usage examples
@timer
def slow_function():
    time.sleep(1)
    return "Done"

@cache
def fibonacci(n):
    if n < 2:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

@validate_args(age=lambda x: 0 < x < 150)
def calculate_bmi(age, weight, height):
    return weight / (height ** 2)
```

### Advanced: Generators

```python
# Generator function for large datasets
def process_in_chunks(data, chunk_size):
    for i in range(0, len(data), chunk_size):
        yield data[i:i + chunk_size]

# Generator for fibonacci sequence
def fibonacci_generator(limit):
    a, b = 0, 1
    count = 0
    while count < limit:
        yield a
        a, b = b, a + b
        count += 1

# Using generators
data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

print("Chunk processing:")
for chunk in process_in_chunks(data, 3):
    print(list(chunk))

print("\nFibonacci sequence:")
for num in fibonacci_generator(10):
    print(num, end=" ")
```

### Advanced: Context Managers

```python
# Context manager for file operations
class FileManager:
    def __init__(self, filename, mode):
        self.filename = filename
        self.mode = mode
        self.file = None
    
    def __enter__(self):
        self.file = open(self.filename, self.mode)
        return self.file
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        if self.file:
            self.file.close()

# Using context manager
with FileManager("data.txt", "w") as f:
    f.write("Hello, World!")

# Using contextmanager decorator
from contextlib import contextmanager

@contextmanager
def timer_context():
    start = time.time()
    yield
    end = time.time()
    print(f"Operation took {end - start:.4f} seconds")

with timer_context():
    time.sleep(1)
```

## Conclusion

Control flow and functions are essential building blocks for writing effective Python code. This guide covered conditional statements, loops, function definitions, parameters, return values, and lambda functions with practical applications in banking and healthcare domains.

Key takeaways include:
- **Conditional statements**: Use if/elif/else for decision making
- **Loops**: Use for when iteration count is known, while for condition-based iteration
- **Functions**: Create reusable code blocks with parameters and return values
- **Lambda functions**: Use for simple, anonymous operations
- **Decorators**: Extend function behavior without modifying code

The banking applications demonstrated transaction processing, risk assessment, and loan approval systems. Healthcare applications showed patient triage and prescription processing. These examples illustrate how control flow and functions solve real-world problems.

Continue to Object Oriented Programming to learn how to create classes and objects for more complex data science applications.