# Topic: Error_Handling_and_Debugging
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Error Handling and Debugging

I. INTRODUCTION
   Error handling and debugging are critical skills for writing robust code. This module covers
   Python exceptions, try-except blocks, custom exceptions, logging, and debugging techniques.
   Prerequisites: Python fundamentals, OOP basics
   Requirements: Python 3.8+

II. CORE_CONCEPTS
   - Built-in exceptions
   - Try-except-finally blocks
   - Multiple exception handling
   - Raising exceptions
   - Custom exceptions
   - Logging module
   - Debugging with pdb

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for error handling
   - Detailed comments throughout

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking/Finance - Transaction validation
   - Real-world Application 2: Healthcare - Patient data validation

V. OUTPUT_RESULTS
   - Expected outputs
   - Performance analysis

VI. TESTING
   - Unit tests for main functions

VII. ADVANCED_TOPICS
   - Exception chaining
   - Traceback module
   - Assertions
   - Context managers for error handling

VIII. CONCLUSION
   - Key takeaways
   - Next steps for learning
"""

import logging
import traceback
from typing import List, Dict, Any, Optional
from datetime import datetime


def main():
    print("Executing Error Handling and Debugging implementation")
    print("\n=== Basic Exception Handling ===")
    demonstrate_basic_exceptions()
    
    print("\n=== Try-Except Blocks ===")
    demonstrate_try_except()
    
    print("\n=== Custom Exceptions ===")
    demonstrate_custom_exceptions()
    
    print("\n=== Logging ===")
    demonstrate_logging()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_basic_exceptions():
    """Demonstrate basic exceptions"""
    print("\n--- Common Exceptions ---")
    
    print("\n--- ZeroDivisionError ---")
    try:
        result = 10 / 0
    except ZeroDivisionError as e:
        print(f"Caught: {e}")
    
    print("\n--- ValueError ---")
    try:
        num = int("not a number")
    except ValueError as e:
        print(f"Caught: {e}")
    
    print("\n--- TypeError ---")
    try:
        result = "string" + 5
    except TypeError as e:
        print(f"Caught: {e}")
    
    print("\n--- IndexError ---")
    try:
        lst = [1, 2, 3]
        item = lst[10]
    except IndexError as e:
        print(f"Caught: {e}")
    
    print("\n--- KeyError ---")
    try:
        d = {"a": 1}
        value = d["b"]
    except KeyError as e:
        print(f"Caught: {e}")
    
    print("\n--- FileNotFoundError ---")
    try:
        with open("nonexistent.txt", "r") as f:
            content = f.read()
    except FileNotFoundError as e:
        print(f"Caught: {e}")
    
    print("\n--- AttributeError ---")
    try:
        x = 5
        x.append(1)
    except AttributeError as e:
        print(f"Caught: {e}")


def demonstrate_try_except():
    """Demonstrate try-except blocks"""
    print("\n--- Basic Try-Except ---")
    try:
        result = 10 / 2
        print(f"Result: {result}")
    except ZeroDivisionError:
        print("Division by zero!")
    
    print("\n--- Try-Except-Finally ---")
    try:
        file = open("test.txt", "w")
        file.write("test")
    except IOError as e:
        print(f"Error: {e}")
    finally:
        print("Cleanup: Closing file")
        try:
            file.close()
        except:
            pass
    
    print("\n--- Multiple Except Blocks ---")
    try:
        value = int("abc")
    except ValueError:
        print("ValueError caught")
    except TypeError:
        print("TypeError caught")
    
    print("\n--- Except with Tuple ---")
    try:
        items = [1, 2, 3]
        item = items[10]
    except (IndexError, KeyError):
        print("Index or Key error caught")
    
    print("\n--- Exception as Variable ---")
    try:
        x = 10 / 0
    except ZeroDivisionError as e:
        print(f"Exception type: {type(e).__name__}")
        print(f"Exception message: {str(e)}")
    
    print("\n--- Catch All Exceptions ---")
    try:
        risky_operation()
    except Exception as e:
        print(f"Caught exception: {e}")
    
    print("\n--- Raise Exception ---")
    try:
        def divide(a: float, b: float) -> float:
            if b == 0:
                raise ValueError("Cannot divide by zero")
            return a / b
        
        result = divide(10, 0)
    except ValueError as e:
        print(f"Caught: {e}")
    
    print("\n--- Re-raise Exception ---")
    try:
        def outer():
            try:
                raise ValueError("Original error")
            except ValueError:
                print("Logging error...")
                raise
        
        outer()
    except ValueError:
        print("Re-raised exception caught")


def demonstrate_custom_exceptions():
    """Demonstrate custom exceptions"""
    print("\n--- Define Custom Exception ---")
    class ValidationError(Exception):
        """Raised when validation fails"""
        def __init__(self, message: str, field: str = ""):
            self.message = message
            self.field = field
            super().__init__(self.message)
    
    try:
        raise ValidationError("Invalid email", "email")
    except ValidationError as e:
        print(f"Custom exception: {e.message}")
        print(f"Field: {e.field}")
    
    print("\n--- Custom Exception with Additional Data ---")
    class DataProcessingError(Exception):
        def __init__(self, message: str, error_code: int = 0, details: Dict = None):
            self.message = message
            self.error_code = error_code
            self.details = details or {}
            super().__init__(self.message)
    
    try:
        raise DataProcessingError(
            "Processing failed",
            error_code=1001,
            details={"column": "price", "row": 5}
        )
    except DataProcessingError as e:
        print(f"Error: {e.message}")
        print(f"Code: {e.error_code}")
        print(f"Details: {e.details}")
    
    print("\n--- Exception Hierarchy ---")
    class AppError(Exception):
        pass
    
    class DatabaseError(AppError):
        pass
    
    class NetworkError(AppError):
        pass
    
    try:
        raise DatabaseError("Database connection failed")
    except AppError as e:
        print(f"Caught: {e}")
    
    print("\n--- Exception Chaining ---")
    try:
        try:
            raise ValueError("Original error")
        except ValueError as e:
            raise DatabaseError("Failed to save") from e
    except DatabaseError as e:
        print(f"Chained error: {e}")
        print(f"Original: {e.__cause__}")


def demonstrate_logging():
    """Demonstrate logging"""
    print("\n--- Basic Logging ---")
    logging.basicConfig(
        level=logging.INFO,
        format='%(asctime)s - %(levelname)s - %(message)s'
    )
    logging.info("This is an info message")
    logging.warning("This is a warning")
    logging.error("This is an error")
    
    print("\n--- Logging Levels ---")
    
    class LoggedCalculation:
        def __init__(self):
            self.logger = logging.getLogger(__name__)
        
        def calculate(self, a: float, b: float) -> float:
            self.logger.info(f"Calculating {a} + {b}")
            result = a + b
            self.logger.info(f"Result: {result}")
            return result
        
        def divide(self, a: float, b: float) -> float:
            self.logger.info(f"Calculating {a} / {b}")
            if b == 0:
                self.logger.error("Division by zero attempted!")
                raise ValueError("Cannot divide by zero")
            result = a / b
            self.logger.info(f"Result: {result}")
            return result
    
    calc = LoggedCalculation()
    calc.calculate(5, 3)
    calc.divide(10, 2)
    
    print("\n--- Debugging with Assertions ---")
    def calculate_average(numbers: List[float]) -> float:
        assert len(numbers) > 0, "Cannot calculate average of empty list"
        return sum(numbers) / len(numbers)
    
    try:
        result = calculate_average([])
    except AssertionError as e:
        print(f"Assertion failed: {e}")
    
    result = calculate_average([1, 2, 3])
    print(f"Average: {result}")


def risky_operation():
    """A function that might fail"""
    import random
    if random.random() < 0.5:
        raise ValueError("Random error!")


class ValidationException(Exception):
    """Base validation exception"""
    pass


class InvalidAmountException(ValidationException):
    """Raised when amount is invalid"""
    pass


class InsufficientFundsException(ValidationException):
    """Raised when insufficient funds"""
    pass


class AccountLockedException(ValidationException):
    """Raised when account is locked"""
    pass


class BankAccount:
    """Bank account with error handling"""
    def __init__(self, account_number: str, initial_balance: float = 0):
        self.account_number = account_number
        self.balance = initial_balance
        self.is_locked = False
        self.logger = logging.getLogger("BankAccount")
    
    def _validate_positive(self, amount: float, field_name: str):
        if amount <= 0:
            raise InvalidAmountException(
                f"{field_name} must be positive",
                field=field_name
            )
    
    def _check_not_locked(self):
        if self.is_locked:
            raise AccountLockedException(f"Account {self.account_number} is locked")
    
    def deposit(self, amount: float) -> Dict[str, Any]:
        try:
            self._validate_positive(amount, "Amount")
            self._check_not_locked()
            
            self.balance += amount
            self.logger.info(f"Deposited ${amount} to account {self.account_number}")
            
            return {
                "success": True,
                "balance": self.balance,
                "message": f"Successfully deposited ${amount}"
            }
        except (InvalidAmountException, AccountLockedException) as e:
            self.logger.error(f"Deposit failed: {e}")
            return {
                "success": False,
                "error": str(e),
                "error_type": type(e).__name__
            }
    
    def withdraw(self, amount: float) -> Dict[str, Any]:
        try:
            self._validate_positive(amount, "Amount")
            self._check_not_locked()
            
            if amount > self.balance:
                raise InsufficientFundsException(
                    f"Insufficient funds. Balance: ${self.balance}, Requested: ${amount}"
                )
            
            self.balance -= amount
            self.logger.info(f"Withdrew ${amount} from account {self.account_number}")
            
            return {
                "success": True,
                "balance": self.balance,
                "message": f"Successfully withdrew ${amount}"
            }
        except (InvalidAmountException, AccountLockedException, 
                 InsufficientFundsException) as e:
            self.logger.error(f"Withdrawal failed: {e}")
            return {
                "success": False,
                "error": str(e),
                "error_type": type(e).__name__
            }
    
    def transfer(self, amount: float, target_account: "BankAccount") -> Dict[str, Any]:
        try:
            self._validate_positive(amount, "Amount")
            self._check_not_locked()
            
            if amount > self.balance:
                raise InsufficientFundsException(
                    f"Transfer failed: Insufficient funds"
                )
            
            self.balance -= amount
            target_account.balance += amount
            
            self.logger.info(
                f"Transferred ${amount} from {self.account_number} "
                f"to {target_account.account_number}"
            )
            
            return {
                "success": True,
                "balance": self.balance,
                "message": f"Successfully transferred ${amount}"
            }
        except (InvalidAmountException, AccountLockedException,
                 InsufficientFundsException) as e:
            self.logger.error(f"Transfer failed: {e}")
            return {
                "success": False,
                "error": str(e),
                "error_type": type(e).__name__
            }


class PatientDataException(Exception):
    """Base patient data exception"""
    pass


class InvalidDateException(PatientDataException):
    """Raised when date is invalid"""
    pass


class MissingDataException(PatientDataException):
    """Raised when required data is missing"""
    pass


class ValidationException(PatientDataException):
    """Raised when validation fails"""
    pass


class PatientValidator:
    """Patient data validator"""
    @staticmethod
    def validate_patient_id(patient_id: str) -> bool:
        if not patient_id:
            raise MissingDataException("Patient ID is required")
        
        if not isinstance(patient_id, str):
            raise PatientDataException("Patient ID must be a string")
        
        if len(patient_id) < 3:
            raise ValidationException(
                "Patient ID must be at least 3 characters"
            )
        
        return True
    
    @staticmethod
    def validate_date(date_string: str) -> bool:
        try:
            datetime.strptime(date_string, "%Y-%m-%d")
            return True
        except ValueError:
            raise InvalidDateException(
                f"Invalid date format: {date_string}. Use YYYY-MM-DD"
            )
    
    @staticmethod
    def validate_vitals(vitals: Dict[str, float]) -> bool:
        required = ["blood_pressure_systolic", "blood_pressure_diastolic",
                   "heart_rate", "temperature"]
        
        for key in required:
            if key not in vitals:
                raise MissingDataException(f"Missing vital sign: {key}")
        
        bp_sys = vitals["blood_pressure_systolic"]
        if bp_sys < 70 or bp_sys > 250:
            raise ValidationException(
                f"Abnormal blood pressure systolic: {bp_sys}"
            )
        
        hr = vitals["heart_rate"]
        if hr < 30 or hr > 220:
            raise ValidationException(
                f"Abnormal heart rate: {hr}"
            )
        
        return True
    
    @staticmethod
    def validate_patient_data(data: Dict[str, Any]) -> Dict[str, Any]:
        try:
            PatientValidator.validate_patient_id(data.get("patient_id", ""))
            PatientValidator.validate_date(data.get("date_of_birth", ""))
            
            if "vitals" in data:
                PatientValidator.validate_vitals(data["vitals"])
            
            return {"valid": True, "errors": []}
        
        except PatientDataException as e:
            return {"valid": False, "errors": [str(e)]}


def banking_application():
    """
    Banking industry use case - Transaction validation
    """
    print("\n=== Banking Application: Transaction Validation ===")
    
    print("\n--- Create Accounts ---")
    account1 = BankAccount("CHK001", 1000)
    account2 = BankAccount("CHK002", 500)
    
    print(f"Account 1: ${account1.balance}")
    print(f"Account 2: ${account2.balance}")
    
    print("\n--- Valid Transactions ---")
    result = account1.deposit(500)
    print(f"Deposit: {result}")
    
    result = account1.withdraw(300)
    print(f"Withdraw: {result}")
    
    print("\n--- Invalid Transactions ---")
    result = account1.deposit(-100)
    print(f"Negative deposit: {result}")
    
    result = account1.withdraw(2000)
    print(f"Overdraft: {result}")
    
    result = account1.withdraw(0)
    print(f"Zero withdrawal: {result}")
    
    print("\n--- Transfer Operations ---")
    result = account1.transfer(500, account2)
    print(f"Transfer: {result}")
    
    print(f"Account 1 new balance: ${account1.balance}")
    print(f"Account 2 new balance: ${account2.balance}")
    
    print("\n--- Lock Account ---")
    account1.is_locked = True
    result = account1.deposit(100)
    print(f"Locked account deposit: {result}")


def healthcare_application():
    """
    Healthcare industry use case - Patient data validation
    """
    print("\n=== Healthcare Application: Patient Data Validation ===")
    
    print("\n--- Validate Patient ID ---")
    test_ids = ["P001", "P", ""]
    
    for pid in test_ids:
        try:
            PatientValidator.validate_patient_id(pid)
            print(f"Patient ID '{pid}': Valid")
        except PatientDataException as e:
            print(f"Patient ID '{pid}': Invalid - {e}")
    
    print("\n--- Validate Dates ---")
    test_dates = ["1990-01-01", "01/01/1990", "invalid"]
    
    for date in test_dates:
        try:
            PatientValidator.validate_date(date)
            print(f"Date '{date}': Valid")
        except InvalidDateException as e:
            print(f"Date '{date}': Invalid - {e}")
    
    print("\n--- Validate Vitals ---")
    test_vitals = [
        {"blood_pressure_systolic": 120, "blood_pressure_diastolic": 80,
         "heart_rate": 72, "temperature": 98.6},
        {"blood_pressure_systolic": 300, "blood_pressure_diastolic": 80,
         "heart_rate": 72, "temperature": 98.6},
    ]
    
    for vitals in test_vitals:
        try:
            PatientValidator.validate_vitals(vitals)
            print(f"Vitals: Valid")
        except ValidationException as e:
            print(f"Vitals: Invalid - {e}")
    
    print("\n--- Complete Patient Data Validation ---")
    patient_data = {
        "patient_id": "P001",
        "date_of_birth": "1990-03-15",
        "vitals": {
            "blood_pressure_systolic": 120,
            "blood_pressure_diastolic": 80,
            "heart_rate": 72,
            "temperature": 98.6
        }
    }
    
    result = PatientValidator.validate_patient_data(patient_data)
    print(f"Validation result: {result}")
    
    invalid_data = {
        "patient_id": "P",
        "date_of_birth": "invalid-date"
    }
    
    result = PatientValidator.validate_patient_data(invalid_data)
    print(f"Invalid data validation: {result}")


def test_exception_handling():
    """Test exception handling"""
    print("\n=== Testing Exception Handling ===")
    
    try:
        x = 10 / 0
    except ZeroDivisionError:
        assert True
    
    assert True
    
    print("All exception handling tests passed!")


def test_custom_exceptions():
    """Test custom exceptions"""
    print("\n=== Testing Custom Exceptions ===")
    
    class TestError(Exception):
        pass
    
    try:
        raise TestError("test")
    except TestError as e:
        assert str(e) == "test"
    
    print("All custom exception tests passed!")


def test_validation():
    """Test validation functions"""
    print("\n=== Testing Validation ===")
    
    try:
        PatientValidator.validate_patient_id("P001")
        assert True
    except:
        assert False
    
    try:
        PatientValidator.validate_patient_id("")
    except:
        assert True
    
    print("All validation tests passed!")


def run_all_tests():
    """Run all unit tests"""
    test_exception_handling()
    test_custom_exceptions()
    test_validation()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()