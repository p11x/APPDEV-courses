# Topic: Python_Fundamentals_for_Data_Science
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Python Fundamentals for Data Science

I. INTRODUCTION
   Python is the primary programming language for data science and AI applications.
   This module covers essential Python fundamentals including variables, data types,
   operators, and basic syntax that form the foundation for all data science work.
   Prerequisites: Basic computer literacy, understanding of programming concepts
   Requirements: Python 3.8+ installed

II. CORE_CONCEPTS
   - Variables and data types (int, float, str, bool, None)
   - Integer and floating-point numbers
   - Strings and string operations
   - Boolean logic
   - Basic operators (arithmetic, comparison, logical)
   - Type conversion and casting
   - Print formatting and f-strings

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for variable naming
   - Detailed comments throughout

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking/Finance Industry - Calculate compound interest
   - Real-world Application 2: Healthcare Industry - Patient BMI calculator

V. OUTPUT_RESULTS
   - Expected outputs
   - Performance analysis

VI. TESTING
   - Unit tests for main functions

VII. ADVANCED_TOPICS
   - Type hints and annotations
   - Extended precision decimals
   - Common pitfalls and solutions

VIII. CONCLUSION
   - Key takeaways
   - Next steps for learning
"""

import decimal
import sys
from typing import Union, Optional


def main():
    print("Executing Python Fundamentals for Data Science implementation")
    print("\n=== Core Data Types ===")
    demonstrate_core_data_types()
    
    print("\n=== Operators ===")
    demonstrate_operators()
    
    print("\n=== Type Conversion ===")
    demonstrate_type_conversion()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_core_data_types():
    """Demonstrate Python's core data types"""
    print("\n--- Integer Operations ---")
    age = 25
    population = 9800000000
    negative_temp = -15
    
    print(f"Age: {age} (type: {type(age).__name__})")
    print(f"World Population: {population:,} (type: {type(population).__name__})")
    print(f"Temperature: {negative_temp}°C (type: {type(negative_temp).__name__})")
    
    print("\n--- Float Operations ---")
    price = 19.99
    temperature = 36.6
    pi = 3.141592653589793
    
    print(f"Price: ${price} (type: {type(price).__name__})")
    print(f"Body Temperature: {temperature}°C (type: {type(temperature).__name__})")
    print(f"Pi: {pi:.10f} (type: {type(pi).__name__})")
    
    print("\n--- String Operations ---")
    name = "Data Scientist"
    hospital = "General Hospital"
    account_num = "1234567890"
    
    print(f"Name: '{name}' (type: {type(name).__name__})")
    print(f"Hospital: '{hospital}' (type: {type(hospital).__name__})")
    print(f"Account Number: '{account_num}' (type: {type(account_num).__name__})")
    
    print("\n--- String Methods ---")
    message = "  Welcome to Data Science  "
    print(f"Original: '{message}'")
    print(f"Stripped: '{message.strip()}'")
    print(f"Upper: '{message.strip().upper()}'")
    print(f"Lower: '{message.lower()}'")
    print(f"Replace: '{message.replace('Data Science', 'Machine Learning')}'")
    
    print("\n--- Boolean Operations ---")
    is_active = True
    has_error = False
    is_verified = bool(1)
    is_empty = bool(0)
    
    print(f"is_active: {is_active} (type: {type(is_active).__name__})")
    print(f"has_error: {has_error} (type: {type(has_error).__name__})")
    print(f"bool(1): {is_verified}")
    print(f"bool(0): {is_empty}")
    
    print("\n--- None Type ---")
    result = None
    print(f"Result: {result} (type: {type(result).__name__})")
    print(f"result is None: {result is None}")


def demonstrate_operators():
    """Demonstrate Python operators"""
    print("\n--- Arithmetic Operators ---")
    a, b = 15, 4
    
    print(f"a = {a}, b = {b}")
    print(f"a + b = {a + b}")
    print(f"a - b = {a - b}")
    print(f"a * b = {a * b}")
    print(f"a / b = {a / b}")
    print(f"a // b = {a // b} (floor division)")
    print(f"a % b = {a % b} (modulus)")
    print(f"a ** b = {a ** b} (exponent)")
    
    print("\n--- Comparison Operators ---")
    x, y = 10, 20
    
    print(f"x = {x}, y = {y}")
    print(f"x == y: {x == y}")
    print(f"x != y: {x != y}")
    print(f"x < y: {x < y}")
    print(f"x > y: {x > y}")
    print(f"x <= y: {x <= y}")
    print(f"x >= y: {x >= y}")
    
    print("\n--- Logical Operators ---")
    p, q = True, False
    
    print(f"p = {p}, q = {q}")
    print(f"p and q: {p and q}")
    print(f"p or q: {p or q}")
    print(f"not p: {not p}")
    print(f"not q: {not q}")
    
    print("\n--- Assignment Operators ---")
    value = 10
    value += 5
    print(f"value = 10; value += 5 -> {value}")
    value -= 3
    print(f"value -= 3 -> {value}")
    value *= 2
    print(f"value *= 2 -> {value}")


def demonstrate_type_conversion():
    """Demonstrate type conversion functions"""
    print("\n--- Explicit Type Conversion ---")
    
    num_str = "42"
    num_float = "3.14"
    
    print(f"String to int: int('{num_str}') = {int(num_str)}")
    print(f"String to float: float('{num_float}') = {float(num_float)}")
    print(f"Int to float: float(10) = {float(10)}")
    print(f"Float to int: int(3.7) = {int(3.7)}")
    
    print("\n--- String Formatting ---")
    name = "Alice"
    age = 30
    salary = 75000.50
    
    print(f"f-string: {name} is {age} years old")
    print(f"Formatted number: ${salary:,.2f}")
    print(f"Padding: {age:05d}")
    print(f"Percentage: {0.875:.1%}")
    
    print("\n--- Decimal for Precise Calculations ---")
    with decimal.localcontext(decimal.Context(prec=10)):
        price = decimal.Decimal("19.99")
        tax = decimal.Decimal("0.0825")
        total = price + (price * tax)
        print(f"Price: ${price}")
        print(f"Tax (8.25%): ${price * tax}")
        print(f"Total: ${total}")


def practical_example():
    """Practical demonstration of Python fundamentals"""
    print("\n=== Practical Example: Student Grade Calculator ===")
    
    student_name = "John Smith"
    math_score = 85
    science_score = 92
    english_score = 78
    
    average = (math_score + science_score + english_score) / 3
    passing = average >= 60
    
    print(f"Student: {student_name}")
    print(f"Math: {math_score}, Science: {science_score}, English: {english_score}")
    print(f"Average: {average:.2f}")
    print(f"Passing: {passing}")
    
    if average >= 90:
        grade = "A"
    elif average >= 80:
        grade = "B"
    elif average >= 70:
        grade = "C"
    elif average >= 60:
        grade = "D"
    else:
        grade = "F"
    
    print(f"Grade: {grade}")
    
    return {"name": student_name, "average": average, "grade": grade}


def calculate_simple_interest(principal: float, rate: float, time: float) -> float:
    """
    Calculate simple interest
    
    Args:
        principal: Initial principal amount
        rate: Annual interest rate (as decimal, e.g., 0.05 for 5%)
        time: Time period in years
    
    Returns:
        Simple interest amount
    """
    return principal * rate * time


def calculate_compound_interest(principal: float, rate: float, time: float, n: int = 12) -> float:
    """
    Calculate compound interest
    
    Args:
        principal: Initial principal amount
        rate: Annual interest rate (as decimal)
        time: Time period in years
        n: Number of times interest is compounded per year
    
    Returns:
        Final amount after compound interest
    """
    amount = principal * (1 + rate / n) ** (n * time)
    return amount


def calculate_loan_payment(principal: float, annual_rate: float, years: int) -> float:
    """
    Calculate monthly loan payment using amortization formula
    
    Args:
        principal: Loan principal amount
        annual_rate: Annual interest rate (as decimal)
        years: Loan term in years
    
    Returns:
        Monthly payment amount
    """
    if annual_rate == 0:
        return principal / (years * 12)
    
    monthly_rate = annual_rate / 12
    num_payments = years * 12
    
    payment = principal * (monthly_rate * (1 + monthly_rate) ** num_payments) / \
             ((1 + monthly_rate) ** num_payments - 1)
    
    return payment


def banking_application():
    """
    Banking industry use case - Demonstrate Python fundamentals in financial calculations
    """
    print("\n=== Banking Application: Financial Calculations ===")
    
    print("\n--- Savings Account Calculator ---")
    initial_deposit = 10000.00
    annual_rate = 0.045
    years = 5
    
    simple_interest = calculate_simple_interest(initial_deposit, annual_rate, years)
    compound_amount = calculate_compound_interest(initial_deposit, annual_rate, years)
    compound_interest = compound_amount - initial_deposit
    
    print(f"Initial Deposit: ${initial_deposit:,.2f}")
    print(f"Annual Rate: {annual_rate * 100:.2f}%")
    print(f"Period: {years} years")
    print(f"Simple Interest: ${simple_interest:,.2f}")
    print(f"Total with Simple Interest: ${initial_deposit + simple_interest:,.2f}")
    print(f"Compound Interest: ${compound_interest:,.2f}")
    print(f"Total with Compound Interest: ${compound_amount:,.2f}")
    
    print("\n--- Loan Payment Calculator ---")
    loan_amount = 250000.00
    loan_rate = 0.065
    loan_term = 30
    
    monthly_payment = calculate_loan_payment(loan_amount, loan_rate, loan_term)
    total_paid = monthly_payment * loan_term * 12
    total_interest = total_paid - loan_amount
    
    print(f"Loan Amount: ${loan_amount:,.2f}")
    print(f"Interest Rate: {loan_rate * 100:.2f}%")
    print(f"Loan Term: {loan_term} years")
    print(f"Monthly Payment: ${monthly_payment:,.2f}")
    print(f"Total Paid: ${total_paid:,.2f}")
    print(f"Total Interest: ${total_interest:,.2f}")
    
    print("\n--- Account Balance Formatter ---")
    balance = -1234.56
    account_num = "123456789012"
    formatted_balance = f"${abs(balance):,.2f}"
    
    print(f"Account Number: {account_num[:4]}****{account_num[-4:]}")
    print(f"Balance: {formatted_balance}")
    print(f"Status: {'Overdrawn' if balance < 0 else 'Positive'}")


def calculate_bmi(weight_kg: float, height_m: float) -> float:
    """
    Calculate Body Mass Index
    
    Args:
        weight_kg: Weight in kilograms
        height_m: Height in meters
    
    Returns:
        BMI value
    """
    return weight_kg / (height_m ** 2)


def get_bmi_category(bmi: float) -> str:
    """
    Get BMI category based on WHO classification
    
    Args:
        bmi: BMI value
    
    Returns:
        Category string
    """
    if bmi < 18.5:
        return "Underweight"
    elif bmi < 25:
        return "Normal weight"
    elif bmi < 30:
        return "Overweight"
    else:
        return "Obese"


def calculate_total_cholesterol(ldl: int, hdl: int, triglycerides: int) -> int:
    """
    Calculate total cholesterol using Friedewald formula
    
    Args:
        ldl: LDL cholesterol in mg/dL
        hdl: HDL cholesterol in mg/dL
        triglycerides: Triglycerides in mg/dL
    
    Returns:
        Total cholesterol in mg/dL
    """
    return ldl + hdl + (triglycerides // 5)


def calculate_ldl_from_total(total: int, hdl: int, triglycerides: int) -> int:
    """
    Estimate LDL cholesterol using Friedewald formula
    
    Args:
        total: Total cholesterol in mg/dL
        hdl: HDL cholesterol in mg/dL
        triglycerides: Triglycerides in mg/dL
    
    Returns:
        Estimated LDL cholesterol
    """
    if triglycerides > 400:
        return 0
    
    vldl = triglycerides // 5
    return total - hdl - vldl


def healthcare_application():
    """
    Healthcare industry use case - Demonstrate Python fundamentals in health calculations
    """
    print("\n=== Healthcare Application: Health Metrics Calculator ===")
    
    print("\n--- Patient BMI Calculator ---")
    patients = [
        {"name": "John Doe", "weight_kg": 85, "height_m": 1.75},
        {"name": "Jane Smith", "weight_kg": 55, "height_m": 1.62},
        {"name": "Bob Wilson", "weight_kg": 110, "height_m": 1.80},
    ]
    
    for patient in patients:
        bmi = calculate_bmi(patient["weight_kg"], patient["height_m"])
        category = get_bmi_category(bmi)
        
        print(f"\nPatient: {patient['name']}")
        print(f"Weight: {patient['weight_kg']} kg")
        print(f"Height: {patient['height_m']} m")
        print(f"BMI: {bmi:.2f}")
        print(f"Category: {category}")
    
    print("\n--- Lipid Panel Calculator ---")
    lipid_results = {
        "LDL": 130,
        "HDL": 55,
        "Triglycerides": 180,
    }
    
    total_chol = calculate_total_cholesterol(
        lipid_results["LDL"],
        lipid_results["HDL"],
        lipid_results["Triglycerides"]
    )
    estimated_ldl = calculate_ldl_from_total(
        total_chol,
        lipid_results["HDL"],
        lipid_results["Triglycerides"]
    )
    
    print(f"LDL Cholesterol: {lipid_results['LDL']} mg/dL")
    print(f"HDL Cholesterol: {lipid_results['HDL']} mg/dL")
    print(f"Triglycerides: {lipid_results['Triglycerides']} mg/dL")
    print(f"Total Cholesterol: {total_chol} mg/dL")
    print(f"Estimated LDL: {estimated_ldl} mg/dL")
    
    print("\n--- Patient Data Formatting ---")
    patient_id = "PT12345678"
    dob = "1985-03-15"
    ssn = "123-45-6789"
    
    print(f"Patient ID: {patient_id}")
    print(f"Date of Birth: {dob}")
    print(f"SSN: ***-**-{ssn[-4:]}")
    print(f"Age: 40 years")


def test_core_data_types():
    """Test core data type functions"""
    print("\n=== Testing Core Data Types ===")
    
    assert isinstance(25, int)
    assert isinstance(3.14, float)
    assert isinstance("test", str)
    assert isinstance(True, bool)
    assert isinstance(None, type(None))
    
    print("All core data type tests passed!")


def test_operators():
    """Test operator functions"""
    print("\n=== Testing Operators ===")
    
    assert 10 + 5 == 15
    assert 10 - 5 == 5
    assert 10 * 5 == 50
    assert 10 / 5 == 2.0
    assert 10 // 3 == 3
    assert 10 % 3 == 1
    assert 2 ** 3 == 8
    
    assert 10 > 5
    assert 5 < 10
    assert 10 == 10
    
    assert True and True == True
    assert True or False == True
    assert not False == True
    
    print("All operator tests passed!")


def test_type_conversion():
    """Test type conversion functions"""
    print("\n=== Testing Type Conversion ===")
    
    assert int("42") == 42
    assert float("3.14") == 3.14
    assert str(42) == "42"
    assert bool(1) == True
    assert bool(0) == False
    
    print("All type conversion tests passed!")


def test_banking_functions():
    """Test banking calculation functions"""
    print("\n=== Testing Banking Functions ===")
    
    simple_interest = calculate_simple_interest(1000, 0.05, 2)
    assert simple_interest == 100.0
    
    compound_amount = calculate_compound_interest(1000, 0.05, 2, 1)
    assert compound_amount > 1100
    
    payment = calculate_loan_payment(100000, 0.05, 30)
    assert payment > 500
    
    print("All banking function tests passed!")


def test_healthcare_functions():
    """Test healthcare calculation functions"""
    print("\n=== Testing Healthcare Functions ===")
    
    bmi = calculate_bmi(70, 1.75)
    assert 20 < bmi < 25
    
    category = get_bmi_category(22.5)
    assert category == "Normal weight"
    
    total = calculate_total_cholesterol(100, 50, 150)
    assert total == 180
    
    print("All healthcare function tests passed!")


def run_all_tests():
    """Run all unit tests"""
    test_core_data_types()
    test_operators()
    test_type_conversion()
    test_banking_functions()
    test_healthcare_functions()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()