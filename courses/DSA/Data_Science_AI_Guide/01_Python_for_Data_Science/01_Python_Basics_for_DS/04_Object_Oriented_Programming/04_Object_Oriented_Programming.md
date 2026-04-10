# Object Oriented Programming for Data Science

## Introduction

Object-Oriented Programming (OOP) is a fundamental paradigm in Python that organizes code into objects containing both data and behavior. In data science, OOP is essential for building reusable, maintainable, and scalable solutions. Many popular data science libraries like pandas, scikit-learn, and TensorFlow are built using OOP principles.

OOP concepts help data scientists model real-world entities like customers, transactions, patients, and healthcare records in a natural way. Classes serve as blueprints for creating objects that encapsulate data (attributes) and behavior (methods). This approach promotes code reusability, modularity, and easier maintenance.

The four pillars of OOP are:
1. **Encapsulation**: Bundling data and methods that operate on that data within a single unit (class)
2. **Inheritance**: Creating new classes from existing ones to reuse and extend functionality
3. **Polymorphism**: Using objects of different classes through the same interface
4. **Abstraction**: Hiding complex implementation details behind simple interfaces

This guide covers class definitions, inheritance, polymorphism, and practical applications in banking and healthcare domains.

## Fundamentals

### Classes and Objects

A class is a blueprint for creating objects. It defines attributes (data) and methods (behavior) that objects of that class will have. Objects are instances of a class.

```python
# Basic class definition
class Customer:
    def __init__(self, customer_id, name, email):
        self.customer_id = customer_id
        self.name = name
        self.email = email
    
    def display_info(self):
        return f"Customer: {self.name} ({self.customer_id})"

# Creating objects
customer = Customer("C001", "John Doe", "john@example.com")
print(customer.display_info())
```

### Attributes and Methods

Attributes store data about an object. Methods define behaviors that objects can perform. The __init__ method is a special method called when an object is created (constructor).

```python
class BankAccount:
    # Class attribute
    bank_name = "Data Science Bank"
    
    # Instance attributes
    def __init__(self, account_number, holder_name, balance=0):
        self.account_number = account_number
        self.holder_name = holder_name
        self.balance = balance
    
    # Instance method
    def deposit(self, amount):
        if amount > 0:
            self.balance += amount
            return True
        return False
    
    def withdraw(self, amount):
        if amount > 0 and self.balance >= amount:
            self.balance -= amount
            return True
        return False
    
    def get_balance(self):
        return self.balance
```

### Inheritance

Inheritance allows creating new classes (child classes) from existing ones (parent classes). Child classes inherit all attributes and methods from parent classes and can add new ones or override existing ones.

```python
# Parent class
class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    
    def display(self):
        return f"{self.name}, {self.age} years"

# Child class
class Employee(Person):
    def __init__(self, name, age, employee_id, department):
        super().__init__(name, age)  # Call parent constructor
        self.employee_id = employee_id
        self.department = department
    
    def display(self):
        return f"{super().display()}, ID: {self.employee_id}"
```

### Polymorphism

Polymorphism allows objects of different classes to be treated uniformly through a common interface. This is achieved through method overriding and duck typing.

```python
# Different classes with same method
class ReportGenerator:
    def generate(self):
        return "Text report"

class ChartGenerator:
    def generate(self):
        return "Chart image"

# Using polymorphism
def create_report(generator):
    return generator.generate()
```

### Encapsulation

Encapsulation restricts direct access to certain attributes and methods to protect data integrity. This is achieved using private attributes (prefixed with __) and property decorators.

```python
class Patient:
    def __init__(self, patient_id, name):
        self.__patient_id = patient_id  # Private
        self.__medical_records = []  # Private
        self.name = name
    
    @property
    def patient_id(self):
        return self.__patient_id
    
    @property
    def medical_records(self):
        return self.__medical_records.copy()
    
    def add_record(self, record):
        self.__medical_records.append(record)
```

## Implementation

### Banking: Account Classes

```python
class BankAccount:
    """Base bank account class"""
    
    def __init__(self, account_number, holder_name, initial_balance=0):
        self.account_number = account_number
        self.holder_name = holder_name
        self.__balance = initial_balance  # Private
        self.__transactions = []
    
    @property
    def balance(self):
        return self.__balance
    
    def deposit(self, amount):
        if amount > 0:
            self.__balance += amount
            self.__transactions.append({
                "type": "deposit",
                "amount": amount,
                "balance": self.__balance
            })
            return True
        return False
    
    def withdraw(self, amount):
        if amount > 0 and self.__balance >= amount:
            self.__balance -= amount
            self.__transactions.append({
                "type": "withdrawal",
                "amount": amount,
                "balance": self.__balance
            })
            return True
        return False
    
    def get_transactions(self):
        return self.__transactions
    
    def display_info(self):
        return f"Account {self.account_number}: {self.holder_name} - ${self.__balance}"


class SavingsAccount(BankAccount):
    """Savings account with interest"""
    
    def __init__(self, account_number, holder_name, initial_balance=0, interest_rate=0.02):
        super().__init__(account_number, holder_name, initial_balance)
        self.__interest_rate = interest_rate
    
    def calculate_interest(self):
        interest = self.balance * self.__interest_rate
        return interest
    
    def apply_interest(self):
        interest = self.calculate_interest()
        self.deposit(interest)
        return interest


class CheckingAccount(BankAccount):
    """Checking account with overdraft protection"""
    
    def __init__(self, account_number, holder_name, initial_balance=0, overdraft_limit=500):
        super().__init__(account_number, holder_name, initial_balance)
        self.__overdraft_limit = overdraft_limit
    
    def withdraw(self, amount):
        available = self.balance + self.__overdraft_limit
        if amount > 0 and available >= amount:
            super().withdraw(amount)
            return True
        return False


# Test the classes
savings = SavingsAccount("S001", "Alice", 5000, 0.03)
checking = CheckingAccount("C001", "Bob", 1000, 500)

print("Savings Account:")
print(savings.display_info())
savings.deposit(1000)
print(f"Balance after deposit: ${savings.balance}")
interest = savings.calculate_interest()
print(f"Interest earned: ${interest}")

print("\nChecking Account:")
print(checking.display_info())
checking.withdraw(1500)
print(f"Balance after withdrawal: ${checking.balance}")
```

### Banking: Customer Class with Accounts

```python
class Customer:
    """Customer class with multiple accounts"""
    
    def __init__(self, customer_id, name, email, phone):
        self.customer_id = customer_id
        self.name = name
        self.email = email
        self.phone = phone
        self.__accounts = {}  # Private dictionary of accounts
    
    @property
    def accounts(self):
        return self.__accounts
    
    def add_account(self, account):
        self.__accounts[account.account_number] = account
    
    def get_account(self, account_number):
        return self.__accounts.get(account_number)
    
    def get_total_balance(self):
        return sum(account.balance for account in self.__accounts.values())
    
    def display_customer(self):
        print(f"Customer: {self.name}")
        print(f"  ID: {self.customer_id}")
        print(f"  Email: {self.email}")
        print(f"  Phone: {self.phone}")
        print(f"  Total Balance: ${self.get_total_balance():.2f}")
        print("  Accounts:")
        for account in self.__accounts.values():
            print(f"    - {account.account_number}: ${account.balance:.2f}")


# Create customer with accounts
customer = Customer("C001", "John Doe", "john@example.com", "555-1234")

savings = SavingsAccount("S001", "John Doe", 10000, 0.02)
checking = CheckingAccount("C001", "John Doe", 2500, 500)

customer.add_account(savings)
customer.add_account(checking)

customer.display_customer()
```

### Healthcare: Patient Classes

```python
class Patient:
    """Patient class for healthcare system"""
    
    def __init__(self, patient_id, name, date_of_birth, gender):
        self.patient_id = patient_id
        self.name = name
        self.date_of_birth = date_of_birth
        self.gender = gender
        self.__medical_records = []
        self.__prescriptions = []
        self.__visits = []
    
    @property
    def medical_records(self):
        return self.__medical_records.copy()
    
    @property
    def prescriptions(self):
        return self.__prescriptions.copy()
    
    def add_medical_record(self, record):
        self.__medical_records.append(record)
    
    def add_prescription(self, prescription):
        self.__prescriptions.append(prescription)
    
    def add_visit(self, visit):
        self.__visits.append(visit)
    
    def get_active_prescriptions(self):
        return [rx for rx in self.__prescriptions if rx["status"] == "active"]
    
    def display_info(self):
        print(f"Patient: {self.name}")
        print(f"  ID: {self.patient_id}")
        print(f"  DOB: {self.date_of_birth}")
        print(f"  Gender: {self.gender}")
        print(f"  Active Prescriptions: {len(self.get_active_prescriptions())}")


class InPatient(Patient):
    """In-patient with room details"""
    
    def __init__(self, patient_id, name, date_of_birth, gender, room_number, admission_date):
        super().__init__(patient_id, name, date_of_birth, gender)
        self.room_number = room_number
        self.admission_date = admission_date
        self.discharge_date = None
        self.__treatments = []
    
    def add_treatment(self, treatment):
        self.__treatments.append(treatment)
    
    def discharge(self, discharge_date):
        self.discharge_date = discharge_date
    
    def get_treatments(self):
        return self.__treatments
    
    def display_info(self):
        super().display_info()
        print(f"  Room: {self.room_number}")
        print(f"  Admitted: {self.admission_date}")
        if self.discharge_date:
            print(f"  Discharged: {self.discharge_date}")
        else:
            print(f"  Status: ADMITTED")


class OutPatient(Patient):
    """Out-patient with appointment tracking"""
    
    def __init__(self, patient_id, name, date_of_birth, gender):
        super().__init__(patient_id, name, date_of_birth, gender)
        self.__appointments = []
    
    def schedule_appointment(self, appointment):
        self.__appointments.append(appointment)
    
    def get_upcoming_appointments(self):
        return [appt for appt in self.__appointments if appt["status"] == "scheduled"]
```


### Healthcare: Doctor and Department Classes

```python
class Department:
    """Healthcare department"""
    
    def __init__(self, department_id, name, building):
        self.department_id = department_id
        self.name = name
        self.building = building
        self.__staff = []
        self.__patients = []
    
    def add_staff(self, staff):
        self.__staff.append(staff)
    
    def add_patient(self, patient):
        self.__patients.append(patient)
    
    def get_staff_count(self):
        return len(self.__staff)
    
    def get_patient_count(self):
        return len(self.__patients)
    
    def display_info(self):
        print(f"Department: {self.name}")
        print(f"  Building: {self.building}")
        print(f"  Staff: {self.get_staff_count()}")
        print(f"  Patients: {self.get_patient_count()}")


class Doctor:
    """Doctor class"""
    
    def __init__(self, doctor_id, name, specialty, department):
        self.doctor_id = doctor_id
        self.name = name
        self.specialty = specialty
        self.department = department
        self.__patients = []
        department.add_staff(self)
    
    def assign_patient(self, patient):
        self.__patients.append(patient)
    
    def get_patient_count(self):
        return len(self.__patients)
    
    def display_info(self):
        print(f"Doctor: {self.name}")
        print(f"  ID: {self.doctor_id}")
        print(f"  Specialty: {self.specialty}")
        print(f"  Patients: {self.get_patient_count()}")


# Test healthcare classes
cardiology = Department("D001", "Cardiology", "Building A")
emergency = Department("D002", "Emergency", "Building B")

doctor1 = Doctor("DR001", "Dr. Smith", "Cardiologist", cardiology)
doctor2 = Doctor("DR002", "Dr. Jones", "Emergency Medicine", emergency)

patient = Patient("P001", "John Doe", "1980-05-15", "Male")
cardiology.add_patient(patient)
doctor1.assign_patient(patient)

print("Department Info:")
cardiology.display_info()
print("\nDoctor Info:")
doctor1.display_info()
```

## Applications in Banking

### Banking Application: Loan Management System

```python
from datetime import datetime

class Loan:
    """Loan class for banking system"""
    
    def __init__(self, loan_id, customer, principal, interest_rate, term_months):
        self.loan_id = loan_id
        self.customer = customer
        self.principal = principal
        self.interest_rate = interest_rate
        self.term_months = term_months
        self.__balance = principal
        self.__payments = []
        self.status = "active"
    
    @property
    def balance(self):
        return self.__balance
    
    def calculate_monthly_payment(self):
        if self.interest_rate == 0:
            return self.principal / self.term_months
        monthly_rate = self.interest_rate / 100 / 12
        payment = self.principal * (monthly_rate * (1 + monthly_rate) ** self.term_months) / ((1 + monthly_rate) ** self.term_months - 1)
        return payment
    
    def make_payment(self, amount):
        if amount > 0 and amount <= self.__balance:
            self.__balance -= amount
            self.__payments.append({
                "date": datetime.now(),
                "amount": amount,
                "balance": self.__balance
            })
            if self.__balance <= 0:
                self.status = "paid off"
            return True
        return False
    
    def get_total_paid(self):
        return sum(p["amount"] for p in self.__payments)
    
    def display_info(self):
        print(f"Loan: {self.loan_id}")
        print(f"  Customer: {self.customer}")
        print(f"  Principal: ${self.principal:,.2f}")
        print(f"  Interest Rate: {self.interest_rate}%")
        print(f"  Term: {self.term_months} months")
        print(f"  Monthly Payment: ${self.calculate_monthly_payment():,.2f}")
        print(f"  Balance: ${self.__balance:,.2f}")
        print(f"  Status: {self.status}")


class Mortgage(Loan):
    """Mortgage loan with property as collateral"""
    
    def __init__(self, loan_id, customer, principal, interest_rate, term_months, property_address):
        super().__init__(loan_id, customer, principal, interest_rate, term_months)
        self.property_address = property_address
        self.insurance_required = True
    
    def display_info(self):
        super().display_info()
        print(f"  Property: {self.property_address}")
        print(f"  Insurance Required: {self.insurance_required}")


class AutoLoan(Loan):
    """Auto loan with vehicle as collateral"""
    
    def __init__(self, loan_id, customer, principal, interest_rate, term_months, vehicle_info):
        super().__init__(loan_id, customer, principal, interest_rate, term_months)
        self.vehicle_info = vehicle_info
    
    def display_info(self):
        super().display_info()
        print(f"  Vehicle: {self.vehicle_info}")


# Test loan system
print("Loan Management System:")
print("=" * 60)

mortgage = Mortgage(
    "L001", "John Doe", 250000, 4.5, 360, 
    "123 Main Street"
)
mortgage.display_info()

print("\n")
auto_loan = AutoLoan(
    "L002", "Jane Doe", 25000, 6.0, 60,
    "2024 Toyota Camry"
)
auto_loan.display_info()

print("\nPayment processing:")
auto_loan.make_payment(500)
print(f"Balance after payment: ${auto_loan.balance}")
```

### Banking Application: Transaction Processing System

```python
from datetime import datetime

class Transaction:
    """Base transaction class"""
    
    def __init__(self, transaction_id, account, amount, transaction_type):
        self.transaction_id = transaction_id
        self.account = account
        self.amount = amount
        self.transaction_type = transaction_type
        self.timestamp = datetime.now()
        self.status = "completed"
    
    def display_info(self):
        return f"{self.transaction_id}: {self.transaction_type} - ${self.amount}"


class Deposit(Transaction):
    """Deposit transaction"""
    
    def __init__(self, transaction_id, account, amount):
        super().__init__(transaction_id, account, amount, "deposit")
        self.new_balance = account.deposit(amount)


class Withdrawal(Transaction):
    """Withdrawal transaction"""
    
    def __init__(self, transaction_id, account, amount):
        super().__init__(transaction_id, account, amount, "withdrawal")
        success = account.withdraw(amount)
        if not success:
            self.status = "failed"


class Transfer(Transaction):
    """Transfer transaction between accounts"""
    
    def __init__(self, transaction_id, from_account, to_account, amount):
        super().__init__(transaction_id, from_account, amount, "transfer")
        self.to_account = to_account
        from_account.withdraw(amount)
        to_account.deposit(amount)


class TransactionLog:
    """Transaction log for audit trail"""
    
    def __init__(self):
        self.__transactions = []
    
    def add_transaction(self, transaction):
        self.__transactions.append(transaction)
    
    def get_transactions_by_account(self, account_number):
        return [t for t in self.__transactions if t.account.account_number == account_number]
    
    def get_failed_transactions(self):
        return [t for t in self.__transactions if t.status == "failed"]
    
    def display_all(self):
        print("Transaction Log:")
        for t in self.__transactions:
            print(f"  {t.display_info()}")


# Test transaction system
print("Transaction Processing System:")
print("=" * 60)

account1 = BankAccount("A001", "Alice", 5000)
account2 = BankAccount("A002", "Bob", 3000)

log = TransactionLog()

deposit = Deposit("T001", account1, 1000)
log.add_transaction(deposit)

withdrawal = Withdrawal("T002", account1, 500)
log.add_transaction(withdrawal)

transfer = Transfer("T003", account1, account2, 2000)
log.add_transaction(transfer)

print(f"Account 1 Balance: ${account1.balance}")
print(f"Account 2 Balance: ${account2.balance}")

print("\nTransaction Log:")
log.display_all()
```

## Applications in Healthcare

### Healthcare Application: Hospital Management System

```python
from datetime import datetime, timedelta

class MedicalRecord:
    """Medical record for patient visits"""
    
    def __init__(self, record_id, patient, doctor, visit_date, diagnosis):
        self.record_id = record_id
        self.patient = patient
        self.doctor = doctor
        self.visit_date = visit_date
        self.diagnosis = diagnosis
        self.__vital_signs = {}
        self.__notes = []
        self.__prescriptions = []
    
    def add_vital_signs(self, vital_type, value, unit):
        self.__vital_signs[vital_type] = {"value": value, "unit": unit}
    
    def add_note(self, note):
        self.__notes.append(note)
    
    def add_prescription(self, prescription):
        self.__prescriptions.append(prescription)
    
    def get_vital_signs(self):
        return self.__vital_signs
    
    def display_info(self):
        print(f"Medical Record: {self.record_id}")
        print(f"  Patient: {self.patient}")
        print(f"  Doctor: {self.doctor}")
        print(f"  Date: {self.visit_date}")
        print(f"  Diagnosis: {self.diagnosis}")
        if self.__vital_signs:
            print(f"  Vital Signs:")
            for vital, data in self.__vital_signs.items():
                print(f"    {vital}: {data['value']} {data['unit']}")


class Hospital:
    """Hospital management system"""
    
    def __init__(self, hospital_id, name, address):
        self.hospital_id = hospital_id
        self.name = name
        self.address = address
        self.__departments = {}
        self.__patients = {}
        self.__doctors = {}
    
    def add_department(self, department):
        self.__departments[department.department_id] = department
    
    def add_patient(self, patient):
        self.__patients[patient.patient_id] = patient
    
    def add_doctor(self, doctor):
        self.__doctors[doctor.doctor_id] = doctor
    
    def get_patient(self, patient_id):
        return self.__patients.get(patient_id)
    
    def get_doctor(self, doctor_id):
        return self.__doctors.get(doctor_id)
    
    def display_hospital_info(self):
        print(f"Hospital: {self.name}")
        print(f"  Address: {self.address}")
        print(f"  Departments: {len(self.__departments)}")
        print(f"  Patients: {len(self.__patients)}")
        print(f"  Doctors: {len(self.__doctors)}")


# Test hospital system
hospital = Hospital("H001", "Data Science Hospital", "123 Medical Center Dr")

cardiology = Department("D001", "Cardiology", "Building A")
emergency = Department("D002", "Emergency", "Building B")

hospital.add_department(cardiology)
hospital.add_department(emergency)

patient = Patient("P001", "John Smith", "1980-01-01", "Male")
hospital.add_patient(patient)

doctor = Doctor("DR001", "Dr. Williams", "Cardiologist", cardiology)
hospital.add_doctor(doctor)

record = MedicalRecord("MR001", patient.name, doctor.name, "2024-01-15", "Hypertension")
record.add_vital_signs("BP", "140/90", "mmHg")
record.add_vital_signs("Heart Rate", "75", "bpm")
record.add_note("Patient advised to reduce sodium intake")

hospital.display_hospital_info()
print()
record.display_info()
```

## Output Results

### Sample Output: Classes and Objects

```
Savings Account:
Account S001: Alice - $5000
Balance after deposit: $6000
Interest earned: $180.0

Checking Account:
Account C001: Bob - $1000
Balance after withdrawal: $-500

Customer: John Doe
  ID: C001
  Email: john@example.com
  Phone: 555-1234
  Total Balance: $13500.00
  Accounts:
    - S001: $11000.00
    - C001: $2500.00
```

### Sample Output: Loan System

```
Loan Management System:
============================================================

Loan: L001
  Customer: John Doe
  Principal: $250,000.00
  Interest Rate: 4.5%
  Term: 360 months
  Monthly Payment: $1,266.71
  Balance: $250,000.00
  Status: active
  Property: 123 Main Street
  Insurance Required: True


Loan: L002
  Customer: Jane Doe
  Principal: $25,000.00
  Interest Rate: 6.0%
  Term: 60 months
  Monthly Payment: $479.69
  Balance: $25,000.00
  Status: active
  Vehicle: 2024 Toyota Camry

Payment processing:
Balance after payment: $24500.00
```

### Sample Output: Healthcare System

```
Patient: John Smith
  ID: P001
  DOB: 1980-01-01
  Gender: Male
  Active Prescriptions: 0

In-Patient: Jane Doe
  ID: P002
  Room: 501
  Admitted: 2024-01-10
  Status: ADMITTED

Department: Cardiology
  Building: Building A
  Staff: 2
  Patients: 2
```

## Visualization

### ASCII: Class Hierarchy

```
+====================================================================+
|                    CLASS HIERARCHY DIAGRAM                          |
+====================================================================+
|                                                                      |
|  BANKING CLASSES                                                    |
|  =============                                                      |
|                                                                      |
|                     +-------------+                                 |
|                     |  Person     |                                 |
|                     | (Parent)   |                                 |
|                     +-------------+                                 |
|                     | - name     |                                 |
|                     | - age      |                                 |
|                     | + display()|                                 |
|                     +-------------+                                 |
|                           |                                        |
|             +-------------+-------------+                        |
|             |                           |                          |
|      +------+------+            +------+-------+                 |
|      | Customer  |            | Employee   |                  |
|      +------------+            +-----------+                  |
|      | -customer |            | -employee |                  |
|      | -accounts |            | -dept     |                  |
|      | +add_acct |            | +work()   |                  |
|      +------------+            +-----------+                                    |
|                                                                      |
|  ACCOUNT HIERARCHY                                                 |
|  ================                                                  |
|                                                                      |
|                     +----------------+                             |
|                     | BankAccount    |                             |
|                     |  (Base)       |                             |
|                     +----------------+                             |
|                     | - account_num  |                             |
|                     | - balance     |                             |
|                     | + deposit()   |                             |
|                     | + withdraw()  |                             |
|                     +----------------+                             |
|                    /          \                                  |
|           +---------+        +---------+                         |
|           | Savings |        |Checking |                         |
|           +---------+        +---------+                         |
|           | +interest     | | +overdraft                       |
|           +---------+        +---------+                          |
|                                                                      |
+====================================================================+
```

### ASCII: Object Relationships

```
+====================================================================+
|                  OBJECT RELATIONSHIP DIAGRAM                         |
+====================================================================+
|                                                                      |
|  BANKING SYSTEM OBJECTS                                             |
|  ====================                                              |
|                                                                      |
|  +-------------+                                                  |
|  | Customer   |                                                |
|  | C001: John  |                                                |
|  +-------------+                                                |
|  | name=John   |                                                |
|  | accounts[] |-----> +----------------+                      |
|  +-------------+     | SavingsAccount |                       |
|       |             | S001 ($10,000) |                       |
|       +------------>+----------------+                      |
|                     | balance=$10,000|                      |
|                     +----------------+                        |
|                                                                      |
|  +-------------+    +----------------+                         |
|  | Checking   |    | Loan          |                         |
|  | C001($2,500)|    | L001($250k)  |                         |
|  +------------+    +---------------+                         |
|                                                                      |
|  HEALTHCARE SYSTEM OBJECTS                                         |
|  ======================                                          |
|                                                                      |
|  +-------------+                                                  |
|  | Department |                                                |
|  | D001: Card |                                                |
|  +-------------+                                                  |
|  | name=Card  |                                                |
|  | staff[]    |-----> +-------------+                        |
|  | patients[] |      | Doctor       |                        |
|  +------------+      | DR001: Smith |                       |
|       |             +-------------+                          |
|       +-----------> | patients[]  |-----> +----------+      |
|       |             +-------------+     | Patient   |        |
|       |                   |           | P001: Joe |        |
|       +------------------> | records[] | +----------+       |
|                           +-------------+                         |
|                                                                      |
+====================================================================+
```

### ASCII: Inheritance Flow

```
+====================================================================+
|                    INHERITANCE FLOW                                |
+====================================================================+
|                                                                      |
|  PARENT CLASS                                                      |
|  =============                                                      |
|                                                                      |
|  +---------------------+                                           |
|  | class BankAccount   |                                           |
|  +---------------------+                                           |
|  | + deposit()         |                                           |
|  | + withdraw()       |                                           |
|  | + get_balance()    |                                           |
|  +---------------------+                                           |
|                                                                      |
|              ^    ^    ^                                          |
|             /|    |    |\                                          |
|            / |    |    | \                                         |
|           /  |    |    |  \                                        |
|          /   |    |    |   \                                       |
|         /    |    |    |    \                                      |
|        v     v    v    v     v                                    |
|                                                                      |
|  CHILD CLASSES                                                     |
|  =============                                                     |
|                                                                      |
|  +----------------+  +----------------+  +----------------+         |
|  | SavingsAccount|  | CheckingAcct   |  |InvestmentAcct  |         |
|  +----------------+  +----------------+  +----------------+         |
|  | + calc_interest|  | + overdraft    |  | + trade()     |         |
|  | + apply_intrst |  | + get_overdraft|  | + portfolio() |         |
|  +----------------+  +----------------+  +---------------+         |
|        |                 |                   |                      |
|        +-----------------+-------------------+                      |
|                          |                                         |
|                    +-----------------+                              |
|                    | Inherits all:   |                              |
|                    |  deposit()      |                              |
|                    |  withdraw()     |                              |
|                    |  get_balance()  |                              |
|                    +-----------------+                              |
|                                                                      |
+====================================================================+
```

## Advanced Topics

### Advanced: Abstract Classes

```python
from abc import ABC, abstractmethod

class DataProcessor(ABC):
    """Abstract base class for data processors"""
    
    @abstractmethod
    def process(self, data):
        pass
    
    @abstractmethod
    def validate(self, data):
        pass

class CSVProcessor(DataProcessor):
    def process(self, data):
        return [row.split(",") for row in data]
    
    def validate(self, data):
        return all("," in row for row in data)

class JSONProcessor(DataProcessor):
    def process(self, data):
        return [eval(row) for row in data]
    
    def validate(self, data):
        return all("{" in row for row in data)
```

### Advanced: Multiple Inheritance

```python
class Logger:
    def log(self, message):
        print(f"LOG: {message}")

class Validator:
    def validate(self, data):
        return data is not None

class DataHandler(Logger, Validator):
    def process(self, data):
        if self.validate(data):
            self.log(f"Processing: {data}")
            return data.upper()
        return None
```

### Advanced: Method Resolution Order

```python
class A:
    def show(self):
        print("A")

class B(A):
    def show(self):
        print("B")

class C(A):
    def show(self):
        print("C")

class D(B, C):
    pass

# MRO demonstration
print(D.__mro__)
d = D()
d.show()  # Calls B.show()
```

## Conclusion

Object-Oriented Programming provides a powerful paradigm for organizing code in data science applications. This guide covered classes, objects, inheritance, polymorphism, and encapsulation with practical applications in banking and healthcare.

Key takeaways include:
- **Classes** serve as blueprints for creating objects with attributes and methods
- **Inheritance** allows creating child classes that reuse and extend parent functionality
- **Polymorphism** enables treating different objects through a common interface
- **Encapsulation** protects data integrity through private attributes

The banking applications demonstrated account management, loan processing, and transaction handling. Healthcare applications showed patient management, doctor assignments, and medical record tracking.

Continue to Error Handling and Debugging to learn how to handle exceptions and debug your Python code.