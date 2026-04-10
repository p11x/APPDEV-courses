# Topic: Control_Flow_and_Functions
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Control Flow and Functions

I. INTRODUCTION
   Control flow and functions are essential programming concepts that enable decision-making,
   looping, and code reusability. This module covers conditional statements, loops,
   iteration, and function definitions including advanced topics like closures and decorators.
   Prerequisites: Python fundamentals, data structures
   Requirements: Python 3.8+

II. CORE_CONCEPTS
   - Conditional statements (if, elif, else)
   - Loops (for, while)
   - Loop control (break, continue, pass)
   - Function definitions and calls
   - Parameters and return values
   - Lambda functions
   - Closures and decorators

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for control flow
   - Detailed comments throughout

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking/Finance - Loan approval system
   - Real-world Application 2: Healthcare - Patient triage system

V. OUTPUT_RESULTS
   - Expected outputs
   - Performance analysis

VI. TESTING
   - Unit tests for main functions

VII. ADVANCED_TOPICS
   - Generator functions
   - Decorators with arguments
   - Recursive functions
   - Memoization

VIII. CONCLUSION
   - Key takeaways
   - Next steps for learning
"""

from typing import List, Dict, Any, Callable, Optional, Tuple
from functools import lru_cache
import time


def main():
    print("Executing Control Flow and Functions implementation")
    print("\n=== Conditional Statements ===")
    demonstrate_conditionals()
    
    print("\n=== Loops ===")
    demonstrate_loops()
    
    print("\n=== Functions ===")
    demonstrate_functions()
    
    print("\n=== Lambda Functions ===")
    demonstrate_lambda()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_conditionals():
    """Demonstrate conditional statements"""
    print("\n--- Basic If-Else ---")
    age = 25
    
    if age >= 18:
        print("Adult")
    else:
        print("Minor")
    
    print("\n--- If-Elif-Else Chain ---")
    score = 85
    
    if score >= 90:
        grade = "A"
    elif score >= 80:
        grade = "B"
    elif score >= 70:
        grade = "C"
    elif score >= 60:
        grade = "D"
    else:
        grade = "F"
    
    print(f"Score: {score}, Grade: {grade}")
    
    print("\n--- Nested Conditionals ---")
    is_member = True
    purchase_amount = 150
    
    if is_member:
        if purchase_amount > 100:
            discount = 0.20
        else:
            discount = 0.10
    else:
        discount = 0.0
    
    print(f"Is Member: {is_member}, Amount: ${purchase_amount}, Discount: {discount*100}%")
    
    print("\n--- Ternary Operator ---")
    status = "Active" if True else "Inactive"
    print(f"Status: {status}")
    
    value = "Positive" if 10 > 0 else "Negative"
    print(f"Value: {value}")
    
    print("\n--- Multiple Conditions ---")
    age = 30
    income = 50000
    
    if age >= 21 and income >= 30000:
        print("Eligible for loan")
    else:
        print("Not eligible")
    
    role = "admin"
    if role in ["admin", "superuser"]:
        print("Full access")
    elif role == "user":
        print("Limited access")
    else:
        print("No access")


def demonstrate_loops():
    """Demonstrate loops"""
    print("\n--- For Loop with Range ---")
    for i in range(5):
        print(f"Index: {i}")
    
    print("\n--- For Loop with Sequence ---")
    fruits = ["apple", "banana", "cherry"]
    for fruit in fruits:
        print(f"Fruit: {fruit}")
    
    print("\n--- For Loop with Enumerate ---")
    for index, fruit in enumerate(fruits):
        print(f"{index}: {fruit}")
    
    print("\n--- For Loop with Zip ---")
    names = ["Alice", "Bob", "Charlie"]
    ages = [25, 30, 35]
    for name, age in zip(names, ages):
        print(f"{name}: {age}")
    
    print("\n--- While Loop ---")
    count = 0
    while count < 5:
        print(f"Count: {count}")
        count += 1
    
    print("\n--- Loop Control: Break ---")
    for i in range(10):
        if i == 5:
            print("Breaking at 5")
            break
        print(i)
    
    print("\n--- Loop Control: Continue ---")
    for i in range(5):
        if i == 2:
            print("Skipping 2")
            continue
        print(i)
    
    print("\n--- Loop Control: Pass ---")
    for i in range(5):
        if i == 2:
            pass
        print(i)
    
    print("\n--- List Comprehension with Condition ---")
    numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    even_squares = [x**2 for x in numbers if x % 2 == 0]
    print(f"Even squares: {even_squares}")
    
    print("\n--- Dictionary Iteration ---")
    data = {"a": 1, "b": 2, "c": 3}
    for key in data:
        print(f"Key: {key}, Value: {data[key]}")
    
    for key, value in data.items():
        print(f"{key}: {value}")
    
    print("\n--- Nested Loops ---")
    matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
    for row in matrix:
        for element in row:
            print(element, end=" ")
        print()


def demonstrate_functions():
    """Demonstrate function definitions"""
    print("\n--- Basic Function ---")
    def greet(name: str) -> str:
        return f"Hello, {name}!"
    
    result = greet("World")
    print(result)
    
    print("\n--- Function with Multiple Parameters ---")
    def calculate_area(length: float, width: float) -> float:
        return length * width
    
    area = calculate_area(5, 3)
    print(f"Area: {area}")
    
    print("\n--- Function with Default Parameters ---")
    def greet_with_default(name: str = "World") -> str:
        return f"Hello, {name}!"
    
    print(greet_with_default())
    print(greet_with_default("Alice"))
    
    print("\n--- Function with *args ---")
    def sum_all(*args) -> int:
        return sum(args)
    
    print(f"Sum: {sum_all(1, 2, 3, 4, 5)}")
    
    print("\n--- Function with **kwargs ---")
    def print_info(**kwargs):
        for key, value in kwargs.items():
            print(f"{key}: {value}")
    
    print_info(name="John", age=30, city="NYC")
    
    print("\n--- Function with Mixed Parameters ---")
    def create_user(name: str, age: int, *args, **kwargs):
        print(f"Name: {name}, Age: {age}")
        print(f"Args: {args}")
        print(f"Kwargs: {kwargs}")
    
    create_user("Alice", 25, "extra1", "extra2", email="alice@example.com")
    
    print("\n--- Return Multiple Values ---")
    def get_stats(numbers: List[int]) -> Tuple[int, int, float]:
        return min(numbers), max(numbers), sum(numbers) / len(numbers)
    
    min_val, max_val, avg_val = get_stats([1, 2, 3, 4, 5])
    print(f"Min: {min_val}, Max: {max_val}, Avg: {avg_val}")
    
    print("\n--- Function Inside Function ---")
    def outer():
        def inner():
            print("Inner function called")
        inner()
    
    outer()
    
    print("\n--- Recursive Function ---")
    def factorial(n: int) -> int:
        if n <= 1:
            return 1
        return n * factorial(n - 1)
    
    print(f"Factorial of 5: {factorial(5)}")


def demonstrate_lambda():
    """Demonstrate lambda functions"""
    print("\n--- Basic Lambda ---")
    double = lambda x: x * 2
    print(f"Double 5: {double(5)}")
    
    print("\n--- Lambda with Multiple Parameters ---")
    add = lambda x, y: x + y
    print(f"Add 3, 4: {add(3, 4)}")
    
    print("\n--- Lambda with Condition ---")
    max_func = lambda x, y: x if x > y else y
    print(f"Max 10, 20: {max_func(10, 20)}")
    
    print("\n--- Lambda with Sorting ---")
    names = ["Charlie", "Alice", "Bob"]
    sorted_names = sorted(names, key=lambda x: len(x))
    print(f"Sorted by length: {sorted_names}")
    
    print("\n--- Lambda with Filter ---")
    numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    even_numbers = list(filter(lambda x: x % 2 == 0, numbers))
    print(f"Even numbers: {even_numbers}")
    
    print("\n--- Lambda with Map ---")
    squared = list(map(lambda x: x**2, numbers))
    print(f"Squared: {squared}")
    
    print("\n--- Lambda with Reduce ---")
    from functools import reduce
    product = reduce(lambda x, y: x * y, numbers)
    print(f"Product: {product}")


def practical_example():
    """Practical demonstration of control flow and functions"""
    print("\n=== Practical Example: Data Processing Pipeline ===")
    
    data = [
        {"name": "Product A", "price": 100, "category": "Electronics"},
        {"name": "Product B", "price": 50, "category": "Clothing"},
        {"name": "Product C", "price": 200, "category": "Electronics"},
        {"name": "Product D", "price": 75, "category": "Food"},
    ]
    
    def filter_by_category(items: List[Dict], category: str) -> List[Dict]:
        return [item for item in items if item["category"] == category]
    
    def calculate_total(items: List[Dict]) -> float:
        return sum(item["price"] for item in items)
    
    def apply_discount(items: List[Dict], discount: float) -> List[Dict]:
        return [
            {**item, "discounted_price": item["price"] * (1 - discount)}
            for item in items
        ]
    
    electronics = filter_by_category(data, "Electronics")
    print(f"Electronics items: {len(electronics)}")
    
    total = calculate_total(electronics)
    print(f"Total price: ${total:.2f}")
    
    discounted = apply_discount(electronics, 0.1)
    print("Discounted electronics:")
    for item in discounted:
        print(f"  {item['name']}: ${item['discounted_price']:.2f}")
    
    return {"electronics": electronics, "total": total, "discounted": discounted}


class LoanEligibilityChecker:
    """Loan eligibility checker for banking application"""
    def __init__(self):
        self.min_age = 18
        self.min_income = 30000
        self.min_credit_score = 600
    
    def check_age(self, age: int) -> bool:
        return age >= self.min_age
    
    def check_income(self, income: float) -> bool:
        return income >= self.min_income
    
    def check_credit_score(self, credit_score: int) -> bool:
        return credit_score >= self.min_credit_score
    
    def check_employment(self, years_employed: int) -> bool:
        return years_employed >= 2
    
    def calculate_max_loan(self, income: float, credit_score: int) -> float:
        base_multiplier = 4 if credit_score >= 700 else 3
        return income * base_multiplier
    
    def check_eligibility(self, age: int, income: float, credit_score: int,
                       years_employed: int) -> Dict[str, Any]:
        checks = {
            "age": self.check_age(age),
            "income": self.check_income(income),
            "credit_score": self.check_credit_score(credit_score),
            "employment": self.check_employment(years_employed),
        }
        
        all_passed = all(checks.values())
        max_loan = self.calculate_max_loan(income, credit_score) if all_passed else 0
        
        return {
            "eligible": all_passed,
            "checks": checks,
            "max_loan": max_loan,
            "reason": "" if all_passed else "Did not meet all requirements"
        }


class LoanCalculator:
    """Loan payment calculator"""
    @staticmethod
    def calculate_monthly_payment(principal: float, annual_rate: float, 
                                  years: int) -> float:
        if annual_rate == 0:
            return principal / (years * 12)
        
        monthly_rate = annual_rate / 12
        num_payments = years * 12
        
        payment = principal * (monthly_rate * (1 + monthly_rate) ** num_payments) / \
                 ((1 + monthly_rate) ** num_payments - 1)
        
        return payment
    
    @staticmethod
    def calculate_total_interest(principal: float, monthly_payment: float,
                                years: int) -> float:
        total_paid = monthly_payment * years * 12
        return total_paid - principal


def banking_application():
    """
    Banking industry use case - Loan approval system
    """
    print("\n=== Banking Application: Loan Approval System ===")
    
    print("\n--- Loan Eligibility Check ---")
    checker = LoanEligibilityChecker()
    
    applicants = [
        {"name": "John Smith", "age": 35, "income": 75000, 
         "credit_score": 720, "years_employed": 5},
        {"name": "Jane Doe", "age": 17, "income": 45000,
         "credit_score": 680, "years_employed": 1},
        {"name": "Bob Wilson", "age": 45, "income": 55000,
         "credit_score": 590, "years_employed": 10},
    ]
    
    for applicant in applicants:
        result = checker.check_eligibility(
            applicant["age"],
            applicant["income"],
            applicant["credit_score"],
            applicant["years_employed"]
        )
        
        print(f"\nApplicant: {applicant['name']}")
        print(f"  Age: {applicant['age']}, Income: ${applicant['income']:,.0f}")
        print(f"  Credit Score: {applicant['credit_score']}, Years Employed: {applicant['years_employed']}")
        print(f"  Eligible: {result['eligible']}")
        
        if result["eligible"]:
            print(f"  Max Loan Amount: ${result['max_loan']:,.2f}")
        else:
            print(f"  Reason: {result['reason']}")
    
    print("\n--- Loan Calculator ---")
    calculator = LoanCalculator()
    
    loan_scenarios = [
        {"principal": 250000, "rate": 0.065, "years": 30},
        {"principal": 50000, "rate": 0.045, "years": 5},
        {"principal": 10000, "rate": 0.12, "years": 3},
    ]
    
    for scenario in loan_scenarios:
        principal = scenario["principal"]
        rate = scenario["rate"]
        years = scenario["years"]
        
        monthly = calculator.calculate_monthly_payment(principal, rate, years)
        total_interest = calculator.calculate_total_interest(principal, monthly, years)
        total = monthly * years * 12
        
        print(f"\nLoan: ${principal:,.0f} at {rate*100:.1f}% for {years} years")
        print(f"  Monthly Payment: ${monthly:,.2f}")
        print(f"  Total Paid: ${total:,.2f}")
        print(f"  Total Interest: ${total_interest:,.2f}")


class PatientTriage:
    """Patient triage system for healthcare"""
    PRIORITY_CRITICAL = 1
    PRIORITY_URGENT = 2
    PRIORITY_STANDARD = 3
    PRIORITY_ROUTINE = 4
    
    def __init__(self):
        self.triage_criteria = {
            "critical": ["cardiac_arrest", "severe_bleeding", "unconscious"],
            "urgent": ["chest_pain", "difficulty_breathing", "severe_pain"],
            "standard": ["moderate_injury", "fever", "dehydration"],
            "routine": ["checkup", "prescription", "mild_symptoms"],
        }
    
    def assess_symptoms(self, symptoms: List[str]) -> int:
        symptoms_lower = [s.lower() for s in symptoms]
        
        for priority, keywords in self.triage_criteria.items():
            for symptom in symptoms_lower:
                if any(kw in symptom for kw in keywords):
                    if priority == "critical":
                        return self.PRIORITY_CRITICAL
                    elif priority == "urgent":
                        return self.PRIORITY_URGENT
                    elif priority == "standard":
                        return self.PRIORITY_STANDARD
                    else:
                        return self.PRIORITY_ROUTINE
        
        return self.PRIORITY_STANDARD
    
    def get_priority_label(self, priority: int) -> str:
        labels = {
            self.PRIORITY_CRITICAL: "Critical",
            self.PRIORITY_URGENT: "Urgent",
            self.PRIORITY_STANDARD: "Standard",
            self.PRIORITY_ROUTINE: "Routine",
        }
        return labels.get(priority, "Unknown")
    
    def assess_vitals(self, vitals: Dict[str, float]) -> Tuple[int, str]:
        bp_sys = vitals.get("blood_pressure_systolic", 120)
        heart_rate = vitals.get("heart_rate", 70)
        temperature = vitals.get("temperature", 98.6)
        oxygen = vitals.get("oxygen_saturation", 98)
        
        if bp_sys > 180 or heart_rate > 120 or oxygen < 90:
            return self.PRIORITY_CRITICAL, "Critical vital signs"
        elif bp_sys > 140 or heart_rate > 100 or oxygen < 94:
            return self.PRIORITY_URGENT, "Abnormal vital signs"
        elif bp_sys >= 90 and bp_sys <= 140 and \
             60 <= heart_rate <= 100 and oxygen >= 95:
            return self.PRIORITY_STABLE, "Stable vital signs"
        
        return self.PRIORITY_STANDARD, "Normal vital signs"
    
    def calculate_emergency_score(self, symptoms: List[str], 
                                 vitals: Dict[str, float]) -> int:
        symptom_priority = self.assess_symptoms(symptoms)
        vital_priority, _ = self.assess_vitals(vitals)
        
        return min(symptom_priority, vital_priority)


def healthcare_application():
    """
    Healthcare industry use case - Patient triage system
    """
    print("\n=== Healthcare Application: Patient Triage System ===")
    
    print("\n--- Symptom-Based Triage ---")
    triage = PatientTriage()
    
    patients = [
        {"id": "P001", "symptoms": ["chest pain", " shortness of breath"]},
        {"id": "P002", "symptoms": ["annual checkup"]},
        {"id": "P003", "symptoms": ["high fever", "cough"]},
    ]
    
    for patient in patients:
        priority = triage.assess_symptoms(patient["symptoms"])
        label = triage.get_priority_label(priority)
        
        print(f"\n{patient['id']}: {patient['symptoms']}")
        print(f"  Priority: {label} ({priority})")
    
    print("\n--- Vital Signs Assessment ---")
    vitals_list = [
        {"id": "V001", "blood_pressure_systolic": 190, "heart_rate": 110,
         "temperature": 98.6, "oxygen_saturation": 88},
        {"id": "V002", "blood_pressure_systolic": 130, "heart_rate": 75,
         "temperature": 98.6, "oxygen_saturation": 98},
        {"id": "V003", "blood_pressure_systolic": 150, "heart_rate": 95,
         "temperature": 101.5, "oxygen_saturation": 93},
    ]
    
    for vitals in vitals_list:
        priority, description = triage.assess_vitals(vitals)
        label = triage.get_priority_label(priority)
        
        print(f"\n{vitals['id']}: BP={vitals['blood_pressure_systolic']}, "
              f"HR={vitals['heart_rate']}, O2={vitals['oxygen_saturation']}%")
        print(f"  Assessment: {label} - {description}")
    
    print("\n--- Combined Emergency Score ---")
    combined_cases = [
        {
            "id": "E001",
            "symptoms": ["severe chest pain", "pain radiating to arm"],
            "vitals": {"blood_pressure_systolic": 160, "heart_rate": 100,
                      "temperature": 98.6, "oxygen_saturation": 92}
        },
    ]
    
    for case in combined_cases:
        score = triage.calculate_emergency_score(case["symptoms"], case["vitals"])
        label = triage.get_priority_label(score)
        
        print(f"\n{case['id']}: Emergency Score = {score} ({label})")


def test_conditionals():
    """Test conditional functions"""
    print("\n=== Testing Conditionals ===")
    
    age = 20
    if age >= 18:
        assert True
    
    score = 85
    if score >= 90:
        grade = "A"
    elif score >= 80:
        grade = "B"
    else:
        grade = "C"
    assert grade == "B"
    
    print("All conditional tests passed!")


def test_loops():
    """Test loop functions"""
    print("\n=== Testing Loops ===")
    
    total = 0
    for i in range(5):
        total += i
    assert total == 10
    
    numbers = [1, 2, 3, 4, 5]
    assert sum(numbers) == 15
    
    even_count = sum(1 for x in numbers if x % 2 == 0)
    assert even_count == 2
    
    print("All loop tests passed!")


def test_functions():
    """Test function definitions"""
    print("\n=== Testing Functions ===")
    
    def add(a, b):
        return a + b
    
    assert add(2, 3) == 5
    
    def factorial(n):
        if n <= 1:
            return 1
        return n * factorial(n - 1)
    
    assert factorial(5) == 120
    
    def sum_all(*args):
        return sum(args)
    
    assert sum_all(1, 2, 3, 4, 5) == 15
    
    print("All function tests passed!")


def test_lambda():
    """Test lambda functions"""
    print("\n=== Testing Lambda ===")
    
    double = lambda x: x * 2
    assert double(5) == 10
    
    add = lambda x, y: x + y
    assert add(3, 4) == 7
    
    numbers = [1, 2, 3, 4, 5]
    evens = list(filter(lambda x: x % 2 == 0, numbers))
    assert evens == [2, 4]
    
    print("All lambda tests passed!")


def test_banking():
    """Test banking functions"""
    print("\n=== Testing Banking Functions ===")
    
    checker = LoanEligibilityChecker()
    
    assert checker.check_age(25) == True
    assert checker.check_age(17) == False
    
    assert checker.check_income(50000) == True
    assert checker.check_income(20000) == False
    
    assert checker.check_credit_score(700) == True
    assert checker.check_credit_score(500) == False
    
    result = checker.check_eligibility(35, 75000, 720, 5)
    assert result["eligible"] == True
    
    print("All banking tests passed!")


def test_healthcare():
    """Test healthcare functions"""
    print("\n=== Testing Healthcare Functions ===")
    
    triage = PatientTriage()
    
    priority = triage.assess_symptoms(["chest pain"])
    assert priority == triage.PRIORITY_URGENT
    
    priority = triage.assess_symptoms(["annual checkup"])
    assert priority == triage.PRIORITY_ROUTINE
    
    vitals = {"blood_pressure_systolic": 130, "heart_rate": 75,
             "temperature": 98.6, "oxygen_saturation": 98}
    priority, _ = triage.assess_vitals(vitals)
    assert priority <= triage.PRIORITY_STABLE
    
    print("All healthcare tests passed!")


def run_all_tests():
    """Run all unit tests"""
    test_conditionals()
    test_loops()
    test_functions()
    test_lambda()
    test_banking()
    test_healthcare()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()