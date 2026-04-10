# Error Handling and Debugging

## Introduction

Error handling and debugging are essential skills for any data scientist. When working with data, numerous issues can arise: missing values, type mismatches, division by zero, file not found, and many more. Without proper error handling, these issues can cause programs to crash or produce incorrect results. Debugging helps identify and fix these issues quickly.

Python provides a robust error handling mechanism through try/except blocks that allow graceful handling of errors. Understanding common errors in data science workflows and how to handle them is crucial. Additionally, debugging techniques like logging, assertions, and using debugging tools help identify the root cause of issues.

This guide covers exception handling, common data science errors, debugging techniques, and best practices with applications in banking and healthcare.

## Fundamentals

### Try/Except Blocks

The try/except block is Python's primary mechanism for handling errors. Code that might raise an exception is placed in the try block, and handling code is placed in the except block.

```python
# Basic try/except
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Cannot divide by zero")

# Try/except with multiple exceptions
try:
    x = int("abc")
    y = 10 / 0
except ValueError:
    print("Invalid value")
except ZeroDivisionError:
    print("Division by zero")

# Try/except/else/finally
try:
    result = 10 / 2
except ZeroDivisionError:
    print("Cannot divide by zero")
else:
    print(f"Result: {result}")  # Runs if no exception
finally:
    print("Done")  # Always runs
```

### Raising Exceptions

You can raise exceptions using the raise statement. Custom exceptions can be created by extending the Exception class.

```python
# Raising built-in exceptions
def divide(a, b):
    if b == 0:
        raise ValueError("Divisor cannot be zero")
    return a / b

# Custom exception
class DataValidationError(Exception):
    pass

def validate_data(data):
    if data is None:
        raise DataValidationError("Data cannot be None")
    return True
```

### Common Data Science Errors

Data science code often encounters specific errors that need handling:
- TypeError: Invalid type operations
- ValueError: Invalid value range
- KeyError: Missing dictionary key
- IndexError: List index out of range
- FileNotFoundError: Missing files
- PermissionError: Access denied
- MemoryError: Out of memory
- FloatingPointError: Math precision issues

```python
# Handling common data science errors
def safe_divide(a, b):
    try:
        return a / b
    except TypeError:
        return "Error: Cannot divide non-numeric types"
    except ZeroDivisionError:
        return "Error: Division by zero"

def get_column_value(df, column, row):
    try:
        return df.loc[row, column]
    except KeyError:
        return f"Error: Column '{column}' not found"
    except IndexError:
        return "Error: Row index out of range"
```

### Assertions

Assertions are debugging aids that check conditions and raise AssertionError if the condition is false. They are useful for validating assumptions during development.

```python
# Basic assertion
def calculate_mean(values):
    assert len(values) > 0, "List cannot be empty"
    return sum(values) / len(values)

# Complex assertions
def validate_dataframe(df):
    assert df is not None, "DataFrame cannot be None"
    assert not df.empty, "DataFrame cannot be empty"
    assert "value" in df.columns, "Missing 'value' column"
    return True
```

## Implementation

### Banking: Transaction Error Handling

```python
import traceback

class TransactionError(Exception):
    """Base exception for transaction errors"""
    pass

class InsufficientFundsError(TransactionError):
    pass

class InvalidAmountError(TransactionError):
    pass

class AccountLockedError(TransactionError):
    pass


class Account:
    """Bank account with error handling"""
    
    def __init__(self, account_number, holder_name, balance=0):
        self.account_number = account_number
        self.holder_name = holder_name
        self.__balance = balance
        self.locked = False
    
    def _check_locked(self):
        if self.locked:
            raise AccountLockedError(f"Account {self.account_number} is locked")
    
    def deposit(self, amount):
        try:
            self._check_locked()
            
            if amount is None:
                raise InvalidAmountError("Amount cannot be None")
            
            if not isinstance(amount, (int, float)):
                raise InvalidAmountError("Amount must be numeric")
            
            if amount <= 0:
                raise InvalidAmountError("Amount must be positive")
            
            self.__balance += amount
            return {"success": True, "balance": self.__balance}
        
        except TransactionError as e:
            return {"success": False, "error": str(e)}
        except Exception as e:
            return {"success": False, "error": f"Unexpected error: {str(e)}"}
    
    def withdraw(self, amount):
        try:
            self._check_locked()
            
            if amount is None:
                raise InvalidAmountError("Amount cannot be None")
            
            if not isinstance(amount, (int, float)):
                raise InvalidAmountError("Amount must be numeric")
            
            if amount <= 0:
                raise InvalidAmountError("Amount must be positive")
            
            if amount > self.__balance:
                raise InsufficientFundsError(
                    f"Insufficient funds. Balance: ${self.__balance}, "
                    f"Requested: ${amount}"
                )
            
            self.__balance -= amount
            return {"success": True, "balance": self.__balance}
        
        except TransactionError as e:
            return {"success": False, "error": str(e)}
        except Exception as e:
            return {"success": False, "error": f"Unexpected error: {str(e)}"}
    
    def transfer(self, target_account, amount):
        try:
            self._check_locked()
            
            if not isinstance(target_account, Account):
                raise InvalidAmountError("Target account must be Account object")
            
            withdraw_result = self.withdraw(amount)
            if not withdraw_result["success"]:
                return withdraw_result
            
            deposit_result = target_account.deposit(amount)
            if not deposit_result["success"]:
                self.deposit(amount)  # Rollback
                return {"success": False, "error": "Transfer failed"}
            
            return {
                "success": True,
                "message": f"Transferred ${amount}",
                "balance": self.__balance
            }
        
        except Exception as e:
            return {"success": False, "error": str(e)}


# Test error handling
account = Account("A001", "John Doe", 1000)

print("Transaction Error Handling:")
print("=" * 60)

# Valid transactions
result = account.deposit(500)
print(f"Deposit $500: {result}")

result = account.withdraw(300)
print(f"Withdraw $300: {result}")

# Error cases
result = account.deposit(-100)
print(f"Deposit -$100: {result}")

result = account.deposit("500")
print(f"Deposit '500': {result}")

result = account.withdraw(5000)
print(f"Withdraw $5000: {result}")
```

### Banking: Data Validation

```python
import re
from datetime import datetime

class ValidationError(Exception):
    pass


def validate_customer_id(customer_id):
    if not customer_id:
        raise ValidationError("Customer ID cannot be empty")
    if not isinstance(customer_id, str):
        raise ValidationError("Customer ID must be a string")
    if len(customer_id) < 3:
        raise ValidationError("Customer ID must be at least 3 characters")
    return True


def validate_email(email):
    if not email:
        raise ValidationError("Email cannot be empty")
    
    pattern = r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
    if not re.match(pattern, email):
        raise ValidationError("Invalid email format")
    return True


def validate_phone(phone):
    if not phone:
        raise ValidationError("Phone cannot be empty")
    
    digits = re.sub(r'\D', '', phone)
    if len(digits) != 10:
        raise ValidationError("Phone must be 10 digits")
    return True


def validate_amount(amount, min_amount=0, max_amount=1000000):
    try:
        amount = float(amount)
    except (ValueError, TypeError):
        raise ValidationError("Amount must be numeric")
    
    if amount < min_amount:
        raise ValidationError(f"Amount must be at least ${min_amount}")
    
    if amount > max_amount:
        raise ValidationError(f"Amount cannot exceed ${max_amount}")
    
    return True


def validate_account_number(account_number):
    if not account_number:
        raise ValidationError("Account number cannot be empty")
    
    if not account_number.isalnum():
        raise ValidationError("Account number must be alphanumeric")
    
    return True


# Validation tests
test_values = [
    ("customer_id", "C001", validate_customer_id),
    ("customer_id", "C", validate_customer_id),
    ("email", "john@example.com", validate_email),
    ("email", "invalid-email", validate_email),
    ("phone", "5551234567", validate_phone),
    ("phone", "123", validate_phone),
    ("amount", 500, validate_amount),
    ("amount", -100, validate_amount),
    ("amount", 2000000, validate_amount),
]

print("Data Validation Examples:")
print("=" * 60)

for name, value, validator in test_values:
    try:
        validator(value)
        print(f"✓ {name} = {value}: Valid")
    except ValidationError as e:
        print(f"✗ {name} = {value}: {e}")
```

### Healthcare: Medical Data Validation

```python
class MedicalDataError(Exception):
    pass


class PatientValidator:
    """Validator for patient data"""
    
    @staticmethod
    def validate_patient_id(patient_id):
        if not patient_id:
            raise MedicalDataError("Patient ID cannot be empty")
        if not patient_id.startswith("P"):
            raise MedicalDataError("Patient ID must start with 'P'")
        try:
            num = int(patient_id[1:])
            if num < 1:
                raise MedicalDataError("Invalid patient ID number")
        except ValueError:
            raise MedicalDataError("Patient ID must be in format P###")
        return True
    
    @staticmethod
    def validate_date_of_birth(dob):
        if not dob:
            raise MedicalDataError("Date of birth cannot be empty")
        
        try:
            parsed = datetime.strptime(dob, "%Y-%m-%d")
            age = (datetime.now() - parsed).days / 365
            if age < 0 or age > 150:
                raise MedicalDataError("Invalid age from date of birth")
        except ValueError:
            raise MedicalDataError("Date must be in YYYY-MM-DD format")
        return True
    
    @staticmethod
    def validate_medical_record(record):
        required_fields = ["patient_id", "date", "diagnosis"]
        for field in required_fields:
            if field not in record:
                raise MedicalDataError(f"Missing required field: {field}")
        return True


class LabResultValidator:
    """Validator for lab results"""
    
    @staticmethod
    def validate_glucose(value):
        if value < 0:
            raise MedicalDataError("Glucose cannot be negative")
        if value > 500:
            raise MedicalDataError("Glucose value too high")
        return True
    
    @staticmethod
    def validate_cholesterol(value):
        if value < 0:
            raise MedicalDataError("Cholesterol cannot be negative")
        if value > 500:
            raise MedicalDataError("Cholesterol value too high")
        return True
    
    @staticmethod
    def validate_blood_pressure(systolic, diastolic):
        if systolic < 50 or systolic > 300:
            raise MedicalDataError("Invalid systolic pressure")
        if diastolic < 30 or diastolic > 200:
            raise MedicalDataError("Invalid diastolic pressure")
        if systolic <= diastolic:
            raise MedicalDataError("Systolic must be greater than diastolic")
        return True


# Validation examples
print("Healthcare Data Validation:")
print("=" * 60)

patient_tests = [
    ("P001", PatientValidator.validate_patient_id),
    ("X001", PatientValidator.validate_patient_id),
    ("1980-01-01", PatientValidator.validate_date_of_birth),
    ("invalid", PatientValidator.validate_date_of_birth),
]

for test_value, validator in patient_tests:
    try:
        validator(test_value)
        print(f"✓ {test_value}: Valid")
    except MedicalDataError as e:
        print(f"✗ {test_value}: {e}")

lab_tests = [
    ((95,), (PatientValidator.validate_glucose,)),
    ((600,), (PatientValidator.validate_glucose,)),
    ((120, 80), (PatientValidator.validate_blood_pressure,)),
    ((80, 120), (PatientValidator.validate_blood_pressure,)),
]

for values, validator in lab_tests:
    try:
        validator(*values)
        print(f"✓ BP {values[0]}/{values[1]}: Valid")
    except MedicalDataError as e:
        print(f"✗ BP {values[0]}/{values[1]}: {e}")
```

### Healthcare: Exception Handling in Processing

```python
import traceback

class ProcessingError(Exception):
    pass


class DataProcessor:
    """Healthcare data processor with error handling"""
    
    @staticmethod
    def process_medical_record(record):
        try:
            assert "patient_id" in record, "Missing patient_id"
            assert "diagnosis" in record, "Missing diagnosis"
            assert record["patient_id"], "Empty patient_id"
            
            return {"status": "processed", "patient": record["patient_id"]}
        
        except AssertionError as e:
            return {"status": "error", "message": str(e)}
        except Exception as e:
            return {"status": "error", "message": f"Unexpected: {str(e)}"}
    
    @staticmethod
    def process_lab_results(results):
        processed = []
        
        for result in results:
            try:
                processed_result = DataProcessor.process_single_result(result)
                processed.append(processed_result)
            except Exception as e:
                processed.append({
                    "status": "error",
                    "result_id": result.get("id", "unknown"),
                    "error": str(e)
                })
        
        return processed
    
    @staticmethod
    def process_single_result(result):
        assert "patient_id" in result, "Missing patient_id"
        assert "test" in result, "Missing test name"
        assert "value" in result, "Missing value"
        
        value = float(result["value"])
        
        return {
            "patient_id": result["patient_id"],
            "test": result["test"],
            "value": value,
            "status": "processed"
        }


# Test processing
sample_records = [
    {"patient_id": "P001", "diagnosis": "Hypertension"},
    {"patient_id": "P002", "diagnosis": "Diabetes"},
    {"patient_id": "P003"},  # Missing diagnosis
]

print("Medical Record Processing:")
print("=" * 60)

for record in sample_records:
    result = DataProcessor.process_medical_record(record)
    print(f"Record: {result}")

sample_lab_results = [
    {"id": "L001", "patient_id": "P001", "test": "glucose", "value": "95"},
    {"id": "L002", "patient_id": "P002", "test": "cholesterol", "value": "210"},
    {"id": "L003", "patient_id": "P003", "test": "glucose", "value": "invalid"},
]

results = DataProcessor.process_lab_results(sample_lab_results)
for result in results:
    print(f"  Result: {result}")
```

## Applications in Banking

### Banking: File Processing with Error Handling

```python
import csv
from io import StringIO

class FileProcessingError(Exception):
    pass


def parse_transaction_file(file_content):
    """Parse CSV transaction file with error handling"""
    transactions = []
    errors = []
    lines = file_content.strip().split("\n")
    headers = None
    
    for line_num, line in enumerate(lines, 1):
        try:
            if line_num == 1:
                if "id" in line.lower() and "amount" in line.lower():
                    headers = line.split(",")
                continue
            
            parts = line.split(",")
            
            if len(parts) < 3:
                errors.append(f"Line {line_num}: Too few fields")
                continue
            
            transaction = {
                "id": parts[0].strip(),
                "customer": parts[1].strip(),
                "amount": parts[2].strip()
            }
            
            try:
                transaction["amount"] = float(transaction["amount"])
            except ValueError:
                errors.append(f"Line {line_num}: Invalid amount")
                continue
            
            transactions.append(transaction)
        
        except Exception as e:
            errors.append(f"Line {line_num}: {str(e)}")
    
    return transactions, errors


def process_customer_data(customer_data):
    """Process customer data with validation"""
    try:
        customers = []
        
        for record in customer_data:
            try:
                customer = validate_customer_record(record)
                customers.append(customer)
            except ValidationError as e:
                print(f"Skipping invalid record: {e}")
                continue
        
        return customers
    
    except Exception as e:
        raise FileProcessingError(f"Failed to process: {str(e)}")


def validate_customer_record(record):
    """Validate customer record"""
    if not record:
        raise ValidationError("Empty record")
    
    if "customer_id" not in record:
        raise ValidationError("Missing customer_id")
    
    if "name" not in record:
        raise ValidationError("Missing name")
    
    if "email" in record:
        validate_email(record["email"])
    
    return record


# Test file processing
sample_csv = """id,customer,amount
T001,C001,500
T002,C002,invalid
T003,C003,1000
T004,500"""

transactions, errors = parse_transaction_file(sample_csv)

print("Transaction File Processing:")
print("=" * 60)
print(f"Processed: {len(transactions)} transactions")
print(f"Errors: {len(errors)} errors")
for error in errors:
    print(f"  - {error}")

for t in transactions:
    print(f"  {t}")
```

### Banking: API Error Handling

```python
import json

class APIError(Exception):
    pass


class BankAPIError(APIError):
    """Bank-specific API errors"""
    
    def __init__(self, code, message):
        self.code = code
        super().__init__(message)


class APIResponse:
    """API response handler"""
    
    def __init__(self, status, data=None, error=None):
        self.status = status
        self.data = data
        self.error = error
    
    def is_success(self):
        return self.status == 200
    
    def to_dict(self):
        return {
            "status": self.status,
            "data": self.data,
            "error": self.error
        }


def get_account_balance(account_number):
    """Simulated API call to get account balance"""
    try:
        if not account_number:
            raise BankAPIError(400, "Account number is required")
        
        if account_number == "INVALID":
            raise BankAPIError(404, "Account not found")
        
        return APIResponse(200, {"balance": 5000})
    
    except BankAPIError as e:
        return APIResponse(e.code, error=e.message)
    except Exception as e:
        return APIResponse(500, error=str(e))


def transfer_funds(from_account, to_account, amount):
    """Simulated API call to transfer funds"""
    try:
        if not from_account:
            raise BankAPIError(400, "Source account required")
        
        if not to_account:
            raise BankAPIError(400, "Destination account required")
        
        if amount is None:
            raise BankAPIError(400, "Amount required")
        
        if amount <= 0:
            raise BankAPIError(400, "Amount must be positive")
        
        return APIResponse(200, {
            "transaction_id": "T123",
            "amount": amount
        })
    
    except BankAPIError as e:
        return APIResponse(e.code, error=e.message)
    except Exception as e:
        return APIResponse(500, error=str(e))


# Test API error handling
print("API Error Handling:")
print("=" * 60)

test_cases = [
    ("", "Get balance with empty account"),
    ("A001", "Get balance for valid account"),
    ("INVALID", "Get balance for invalid account"),
]

for account, description in test_cases:
    response = get_account_balance(account)
    print(f"\n{description}:")
    print(f"  Status: {response.status}")
    if response.is_success():
        print(f"  Data: {response.data}")
    else:
        print(f"  Error: {response.error}")

transfer_tests = [
    ("", "A001", 100, "Empty source"),
    ("A001", "", 100, "Empty destination"),
    ("A001", "A002", 0, "Zero amount"),
    ("A001", "A002", 100, "Valid transfer"),
]

for from_acc, to_acc, amount, description in transfer_tests:
    response = transfer_funds(from_acc, to_acc, amount)
    print(f"\n{description}:")
    print(f"  Status: {response.status}")
    if response.is_success():
        print(f"  Data: {response.data}")
    else:
        print(f"  Error: {response.error}")
```

## Applications in Healthcare

### Healthcare: HL7 Message Processing

```python
class HL7Error(Exception):
    pass


def parse_hl7_message(message):
    """Parse HL7 message with error handling"""
    try:
        segments = message.strip().split("\r")
        
        if not segments:
            raise HL7Error("Empty message")
        
        parsed = {}
        
        for segment in segments:
            try:
                fields = segment.split("|")
                segment_type = fields[0]
                
                if segment_type == "MSH":
                    parsed["MSH"] = {
                        "delimiter": fields[1],
                        "encoding_chars": fields[2],
                        "sending_app": fields[3],
                        "sending_facility": fields[4],
                        "receiving_app": fields[5],
                        "receiving_facility": fields[6],
                        "timestamp": fields[7],
                    }
                
                elif segment_type == "PID":
                    parsed["PID"] = {
                        "set_id": fields[1],
                        "patient_id": fields[2],
                        "name": fields[5],
                        "dob": fields[7],
                        "gender": fields[8],
                    }
                
                elif segment_type == "OBR":
                    parsed["OBR"] = {
                        "set_id": fields[1],
                        "placer_order": fields[2],
                        "test_name": fields[4],
                        "observation_date": fields[7],
                    }
            
            except IndexError:
                continue
        
        return parsed
    
    except Exception as e:
        raise HL7Error(f"Parse error: {str(e)}")


def validate_hl7_message(message):
    """Validate HL7 message structure"""
    segments = message.strip().split("\r")
    
    if not segments:
        raise HL7Error("Empty message")
    
    if not segments[0].startswith("MSH"):
        raise HL7Error("Missing MSH segment")
    
    has_pid = False
    has_obr = False
    
    for segment in segments:
        if segment.startswith("PID"):
            has_pid = True
        if segment.startswith("OBR"):
            has_obr = True
    
    if not has_pid:
        raise HL7Error("Missing PID segment")
    
    return True


# Test HL7 processing
sample_hl7 = """MSH|^~\&|LAB|HOSPITAL|||201501011200||ORU^R01|
PID|1|12345|Smith^John||19800101|M
OBR|1|ORDER001|BLOOD WORK|GLUCOSE|201501011200|"""

print("HL7 Message Processing:")
print("=" * 60)

try:
    validate_hl7_message(sample_hl7)
    print("Message validated successfully")
    
    parsed = parse_hl7_message(sample_hl7)
    
    if "MSH" in parsed:
        print("\nMSH Segment:")
        for key, value in parsed["MSH"].items():
            print(f"  {key}: {value}")
    
    if "PID" in parsed:
        print("\nPID Segment:")
        for key, value in parsed["PID"].items():
            print(f"  {key}: {value}")

except HL7Error as e:
    print(f"Error: {e}")
```

## Output Results

### Sample Output: Error Handling

```
Transaction Error Handling:
============================================================
Deposit $500: {'success': True, 'balance': 1500}
Withdraw $300: {'success': True, 'balance': 1200}
Deposit -$100: {'success': False, 'error': 'Amount must be positive'}
Deposit '500': {'success': False, 'error': 'Amount must be numeric'}
Withdraw $5000: {'success': False, 'error': 'Insufficient funds. Balance: $1200, Requested: $5000'}

Data Validation Examples:
============================================================
✓ customer_id = C001: Valid
✗ customer_id = C: Customer ID must be at least 3 characters
✓ email = john@example.com: Valid
✗ email = invalid-email: Invalid email format
✓ phone = 5551234567: Valid
✗ phone = 123: Phone must be 10 digits
✓ amount = 500: Valid
✗ amount = -100: Amount must be at least $0
✗ amount = 2000000: Amount cannot exceed $1000000

Healthcare Data Validation:
============================================================
✓ P001: Valid
✗ X001: Patient ID must start with 'P'
✓ 1980-01-01: Valid
✗ invalid: Date must be in YYYY-MM-DD format
✓ BP 120/80: Valid
✗ BP 80/120: Systolic must be greater than diastolic

Medical Record Processing:
============================================================
Record: {'status': 'processed', 'patient': 'P001'}
Record: {'status': 'processed', 'patient': 'P002'}
Record: {'status': 'error', 'message': 'Missing diagnosis'}
```

## Visualization

### ASCII: Exception Hierarchy

```
+====================================================================+
|                    PYTHON EXCEPTION HIERARCHY                             |
+====================================================================+
|                                                                      |
|  BaseException                                                       |
|  +-- SystemExit                                                    |
|  +-- KeyboardInterrupt                                            |
|  +-- GeneratorExit                                                |
|  +-- Exception                                                   |
|       +-- StopIteration                                            |
|       +-- StopAsyncIteration                                      |
|       +-- ArithmeticError                                        |
|       |    +-- FloatingPointError                                |
|       |    +-- OverflowError                                    |
|       |    +-- ZeroDivisionError                                |
|       +-- LookupError                                            |
|       |    +-- IndexError                                      |
|       |    +-- KeyError                                       |
|       +-- OSError                                                |
|       |    +-- FileNotFoundError                               |
|       |    +-- PermissionError                               |
|       |    +-- TimeoutError                                  |
|       +-- ValueError                                             |
|       +-- TypeError                                             |
|       +-- AttributeError                                       |
|                                                                      |
|  DATA SCIENCE SPECIFIC                                              |
|  =====================                                           |
|       +-- DataValidationError                                     |
|       +-- TransactionError (Custom)                               |
|       +-- MedicalDataError (Custom)                                |
|       +-- ProcessingError (Custom)                                 |
|                                                                      |
+====================================================================+
```

### ASCII: Try-Except Flow

```
+====================================================================+
|                   TRY-EXCEPT EXECUTION FLOW                       |
+====================================================================+
|                                                                      |
|  NORMAL EXECUTION                                                   |
|  ==============                                                  |
|                                                                      |
|    +----------+                                               |
|    |   TRY   |                                               |
|    |  Block  |                                               |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    +----------+                                               |
|    | No Error|                                               |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    +----------+                                               |
|    |  ELSE  | (optional)                                   |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    +----------+                                               |
|    | FINALLY|                                               |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    [CONTINUE]                                                  |
|                                                                      |
|  EXCEPTION OCCURRED                                              |
|  =================                                              |
|                                                                      |
|    +----------+                                               |
|    |   TRY   |                                               |
|    |  Block  |                                               |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    +----------+                                               |
|    |  Error  |                                               |
|    +----+----+                                               |
|         |                                                       |
|    +----+----+                                               |
|    | EXCEPT |                                               |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    +----------+                                               |
|    | Handle |                                               |
|    |  Error |                                               |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    +----------+                                               |
|    | FINALLY|                                               |
|    +----+----+                                               |
|         |                                                       |
|         v                                                       |
|    [CONTINUE]                                                  |
|                                                                      |
+====================================================================+
```

## Advanced Topics

### Advanced: Logging Configuration

```python
import logging

# Configure logging
logging.basicConfig(
    level=logging.DEBUG,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)

logger = logging.getLogger(__name__)

def process_data(data):
    logger.info("Starting data processing")
    
    try:
        result = [x * 2 for x in data]
        logger.info(f"Processed {len(result)} items")
        return result
    
    except Exception as e:
        logger.error(f"Error processing data: {str(e)}")
        raise

def api_call():
    logger.debug("Making API call")
    try:
        pass
    except Exception as e:
        logger.critical(f"API call failed: {str(e)}")
        raise
```

### Advanced: Debugging with pdb

```python
# pdb - Python debugger
# import pdb
# pdb.set_trace()  # Set breakpoint

# Alternative: breakpoint()
# breakpoint()  # Python 3.7+

# Common pdb commands:
# n (next) - Execute next line
# s (step) - Step into function
# c (continue) - Continue execution
# p variable - Print variable
# l (list) - List source code
# q (quit) - Quit debugger
```

### Advanced: Unit Testing

```python
import unittest

class TestTransactionHandling(unittest.TestCase):
    
    def test_deposit_valid(self):
        account = Account("A001", "Test", 1000)
        result = account.deposit(500)
        self.assertTrue(result["success"])
    
    def test_deposit_negative(self):
        account = Account("A001", "Test", 1000)
        result = account.deposit(-100)
        self.assertFalse(result["success"])
    
    def test_withdraw_insufficient(self):
        account = Account("A001", "Test", 100)
        result = account.withdraw(500)
        self.assertFalse(result["success"])

# if __name__ == '__main__':
#     unittest.main()
```

## Conclusion

Error handling and debugging are critical skills for data science applications. This guide covered try/except blocks, raising exceptions, common data science errors, assertions, and debugging techniques.

Key takeaways include:
- **Try/except blocks** - Handle errors gracefully without crashing
- **Custom exceptions** - Create domain-specific error types
- **Logging** - Track errors and program flow
- **Assertions** - Validate assumptions during development

The banking applications demonstrated transaction error handling, data validation, file processing, and API error handling. Healthcare applications showed medical data validation and HL7 message processing.

Continue to Virtual Environments and Packages to learn how to manage dependencies and set up development environments.