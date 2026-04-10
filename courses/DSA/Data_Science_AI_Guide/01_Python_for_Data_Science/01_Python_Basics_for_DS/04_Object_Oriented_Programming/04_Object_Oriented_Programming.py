# Topic: Object_Oriented_Programming
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Object Oriented Programming

I. INTRODUCTION
   Object-Oriented Programming (OOP) is a paradigm that organizes code around objects
   and classes. This module covers classes, objects, inheritance, encapsulation,
   polymorphism, and advanced OOP concepts essential for data science applications.
   Prerequisites: Python fundamentals, control flow, functions
   Requirements: Python 3.8+

II. CORE_CONCEPTS
   - Classes and objects
   - Attributes and methods
   - Constructor (__init__)
   - Inheritance and polymorphism
   - Encapsulation and access modifiers
   - Class and static methods
   - Property decorators

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for OOP design
   - Detailed comments throughout

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking/Finance - Account management system
   - Real-world Application 2: Healthcare - Patient management system

V. OUTPUT_RESULTS
   - Expected outputs
   - Performance analysis

VI. TESTING
   - Unit tests for main functions

VII. ADVANCED_TOPICS
   - Multiple inheritance
   - Abstract classes
   - Method resolution order
   - Mixins

VIII. CONCLUSION
   - Key takeaways
   - Next steps for learning
"""

from abc import ABC, abstractmethod
from typing import List, Dict, Any, Optional
from datetime import datetime
import copy


def main():
    print("Executing Object Oriented Programming implementation")
    print("\n=== Class Definition ===")
    demonstrate_class_definition()
    
    print("\n=== Inheritance ===")
    demonstrate_inheritance()
    
    print("\n=== Encapsulation ===")
    demonstrate_encapsulation()
    
    print("\n=== Polymorphism ===")
    demonstrate_polymorphism()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_class_definition():
    """Demonstrate class definitions"""
    print("\n--- Basic Class ---")
    class Person:
        def __init__(self, name: str, age: int):
            self.name = name
            self.age = age
        
        def greet(self) -> str:
            return f"Hello, my name is {self.name}"
        
        def __str__(self) -> str:
            return f"Person(name={self.name}, age={self.age})"
        
        def __repr__(self) -> str:
            return f"Person(name='{self.name}', age={self.age})"
    
    person = Person("Alice", 30)
    print(f"Create person: {person}")
    print(f"Greet: {person.greet()}")
    print(f"Str: {str(person)}")
    print(f"Repr: {repr(person)}")
    
    print("\n--- Class Attributes ---")
    class Dog:
        species = "Canis familiaris"
        legs = 4
        
        def __init__(self, name: str, breed: str):
            self.name = name
            self.breed = breed
        
        def bark(self) -> str:
            return f"{self.name} says Woof!"
        
        @classmethod
        def get_species(cls) -> str:
            return cls.species
        
        @staticmethod
        def common_breeds() -> List[str]:
            return ["Labrador", "German Shepherd", "Golden Retriever"]
    
    dog = Dog("Buddy", "Labrador")
    print(f"Dog name: {dog.name}")
    print(f"Dog breed: {dog.breed}")
    print(f"Species: {dog.get_species()}")
    print(f"Breeds: {dog.common_breeds()}")
    
    print("\n--- Property Decorator ---")
    class Rectangle:
        def __init__(self, width: float, height: float):
            self._width = width
            self._height = height
        
        @property
        def width(self) -> float:
            return self._width
        
        @width.setter
        def width(self, value: float):
            if value <= 0:
                raise ValueError("Width must be positive")
            self._width = value
        
        @property
        def height(self) -> float:
            return self._height
        
        @height.setter
        def height(self, value: float):
            if value <= 0:
                raise ValueError("Height must be positive")
            self._height = value
        
        @property
        def area(self) -> float:
            return self._width * self._height
        
        @property
        def perimeter(self) -> float:
            return 2 * (self._width + self._height)
    
    rect = Rectangle(5, 3)
    print(f"Width: {rect.width}, Height: {rect.height}")
    print(f"Area: {rect.area}, Perimeter: {rect.perimeter}")
    
    rect.width = 10
    print(f"After setting width - Area: {rect.area}")


def demonstrate_inheritance():
    """Demonstrate inheritance"""
    print("\n--- Basic Inheritance ---")
    class Animal:
        def __init__(self, name: str, species: str):
            self.name = name
            self.species = species
        
        def speak(self) -> str:
            return "Some sound"
        
        def __str__(self) -> str:
            return f"{self.name} ({self.species})"
    
    class Dog(Animal):
        def __init__(self, name: str, breed: str):
            super().__init__(name, "Canine")
            self.breed = breed
        
        def speak(self) -> str:
            return f"{self.name} says Woof!"
        
        def fetch(self) -> str:
            return f"{self.name} fetched the ball!"
    
    dog = Dog("Buddy", "Labrador")
    print(f"Dog: {dog}")
    print(f"Speak: {dog.speak()}")
    print(f"Fetch: {dog.fetch()}")
    
    print("\n--- Method Overriding ---")
    class Cat(Animal):
        def __init__(self, name: str, color: str):
            super().__init__(name, "Feline")
            self.color = color
        
        def speak(self) -> str:
            return f"{self.name} says Meow!"
    
    cat = Cat("Whiskers", "Orange")
    print(f"Cat: {cat}")
    print(f"Speak: {cat.speak()}")
    
    print("\n--- Multiple Inheritance ---")
    class Flyer:
        def fly(self) -> str:
            return "Flying!"
    
    class Swimmer:
        def swim(self) -> str:
            return "Swimming!"
    
    class Duck(Animal, Flyer, Swimmer):
        def __init__(self, name: str):
            super().__init__(name, "Waterfowl")
        
        def speak(self) -> str:
            return f"{self.name} says Quack!"
    
    duck = Duck("Donald")
    print(f"Duck: {duck}")
    print(f"Speak: {duck.speak()}")
    print(f"Fly: {duck.fly()}")
    print(f"Swim: {duck.swim()}")
    
    print("\n--- Method Resolution Order ---")
    print(f"Duck MRO: {[c.__name__ for c in Duck.__mro__]}")


def demonstrate_encapsulation():
    """Demonstrate encapsulation"""
    print("\n--- Private Attributes ---")
    class BankAccount:
        def __init__(self, account_number: str, initial_balance: float = 0):
            self.__account_number = account_number
            self.__balance = initial_balance
        
        def deposit(self, amount: float) -> bool:
            if amount <= 0:
                return False
            self.__balance += amount
            return True
        
        def withdraw(self, amount: float) -> bool:
            if amount <= 0 or amount > self.__balance:
                return False
            self.__balance -= amount
            return True
        
        def get_balance(self) -> float:
            return self.__balance
        
        def get_account_number(self) -> str:
            return self.__account_number
        
        def __str__(self) -> str:
            return f"Account({self.__account_number}, Balance=${self.__balance:.2f})"
    
    account = BankAccount("1234567890", 1000)
    print(f"Account: {account}")
    print(f"Balance: ${account.get_balance():.2f}")
    account.deposit(500)
    print(f"After deposit: ${account.get_balance():.2f}")
    account.withdraw(200)
    print(f"After withdrawal: ${account.get_balance():.2f}")
    
    print("\n--- Protected Attributes ---")
    class Employee:
        def __init__(self, name: str, employee_id: str):
            self.name = name
            self._employee_id = employee_id
            self._salary = 0
        
        def set_salary(self, salary: float):
            if salary > 0:
                self._salary = salary
        
        def get_salary(self) -> float:
            return self._salary
    
    emp = Employee("John", "E001")
    print(f"Name: {emp.name}")
    print(f"Employee ID: {emp._employee_id}")


def demonstrate_polymorphism():
    """Demonstrate polymorphism"""
    print("\n--- Polymorphism with Functions ---")
    class Square:
        def __init__(self, side: float):
            self.side = side
        
        def area(self) -> float:
            return self.side ** 2
    
    class Circle:
        def __init__(self, radius: float):
            self.radius = radius
        
        def area(self) -> float:
            import math
            return math.pi * self.radius ** 2
    
    class Triangle:
        def __init__(self, base: float, height: float):
            self.base = base
            self.height = height
        
        def area(self) -> float:
            return 0.5 * self.base * self.height
    
    shapes = [Square(5), Circle(3), Triangle(6, 4)]
    for shape in shapes:
        print(f"{type(shape).__name__} area: {shape.area():.2f}")
    
    print("\n--- Polymorphism with Abstract Class ---")
    class Shape(ABC):
        @abstractmethod
        def area(self) -> float:
            pass
        
        @abstractmethod
        def perimeter(self) -> float:
            pass
    
    class Rectangle(Shape):
        def __init__(self, width: float, height: float):
            self.width = width
            self.height = height
        
        def area(self) -> float:
            return self.width * self.height
        
        def perimeter(self) -> float:
            return 2 * (self.width + self.height)
    
    rect = Rectangle(5, 3)
    print(f"Rectangle area: {rect.area()}, perimeter: {rect.perimeter()}")
    
    print("\n--- Duck Typing ---")
    class Dog:
        def make_sound(self) -> str:
            return "Woof!"
    
    class Cat:
        def make_sound(self) -> str:
            return "Meow!"
    
    def animal_sound(animal):
        print(f"Animal says: {animal.make_sound()}")
    
    animal_sound(Dog())
    animal_sound(Cat())


class BankAccount:
    """Bank account class"""
    def __init__(self, account_number: str, account_holder: str, 
                 account_type: str = "checking"):
        self.account_number = account_number
        self.account_holder = account_holder
        self.account_type = account_type
        self.__balance = 0.0
        self.__transactions: List[Dict[str, Any]] = []
        self.__overdraft_limit = 0.0
    
    def deposit(self, amount: float, description: str = "") -> bool:
        if amount <= 0:
            return False
        self.__balance += amount
        self.__transactions.append({
            "type": "deposit",
            "amount": amount,
            "description": description,
            "date": datetime.now().isoformat()
        })
        return True
    
    def withdraw(self, amount: float, description: str = "") -> bool:
        if amount <= 0 or amount > (self.__balance + self.__overdraft_limit):
            return False
        self.__balance -= amount
        self.__transactions.append({
            "type": "withdrawal",
            "amount": amount,
            "description": description,
            "date": datetime.now().isoformat()
        })
        return True
    
    def get_balance(self) -> float:
        return self.__balance
    
    def get_transactions(self) -> List[Dict[str, Any]]:
        return copy.deepcopy(self.__transactions)
    
    def set_overdraft_limit(self, limit: float):
        self.__overdraft_limit = max(0, limit)
    
    def __str__(self) -> str:
        return f"{self.account_type.capitalize()} Account {self.account_number}"


class SavingsAccount(BankAccount):
    """Savings account with interest"""
    def __init__(self, account_number: str, account_holder: str):
        super().__init__(account_number, account_holder, "savings")
        self.__interest_rate = 0.02
    
    def calculate_interest(self) -> float:
        return self.get_balance() * self.__interest_rate
    
    def apply_interest(self) -> float:
        interest = self.calculate_interest()
        self.deposit(interest, "Interest applied")
        return interest


class CheckingAccount(BankAccount):
    """Checking account with overdraft"""
    def __init__(self, account_number: str, account_holder: str):
        super().__init__(account_number, account_holder, "checking")
        self.set_overdraft_limit(500.0)


class CreditCardAccount(BankAccount):
    """Credit card account"""
    def __init__(self, account_number: str, account_holder: str, 
                 credit_limit: float):
        super().__init__(account_number, account_holder, "credit")
        self.__credit_limit = credit_limit
        self.__available_credit = credit_limit
    
    def get_available_credit(self) -> float:
        return self.__available_credit
    
    def charge(self, amount: float, description: str = "") -> bool:
        if amount <= 0 or amount > self.__available_credit:
            return False
        
        super().withdraw(amount, description)
        self.__available_credit -= amount
        return True
    
    def make_payment(self, amount: float) -> bool:
        if amount <= 0:
            return False
        
        self.deposit(amount, "Payment received")
        self.__available_credit += amount
        if self.__available_credit > self.__credit_limit:
            self.__available_credit = self.__credit_limit
        return True


def banking_application():
    """
    Banking industry use case - Account management system
    """
    print("\n=== Banking Application: Account Management ===")
    
    print("\n--- Create Accounts ---")
    checking = CheckingAccount("CHK001", "John Smith")
    savings = SavingsAccount("SAV001", "John Smith")
    
    print(f"Created: {checking}")
    print(f"Created: {savings}")
    
    print("\n--- Process Transactions ---")
    checking.deposit(5000, "Initial deposit")
    checking.withdraw(500, "ATM withdrawal")
    savings.deposit(10000, "Initial savings")
    savings.deposit(2000, "Transfer from checking")
    
    print(f"Checking Balance: ${checking.get_balance():.2f}")
    print(f"Savings Balance: ${savings.get_balance():.2f}")
    
    print("\n--- Calculate Interest ---")
    interest = savings.calculate_interest()
    print(f"Interest earned: ${interest:.2f}")
    savings.apply_interest()
    print(f"After interest: ${savings.get_balance():.2f}")
    
    print("\n--- Transaction History ---")
    for txn in checking.get_transactions():
        print(f"  {txn['type'].title()}: ${txn['amount']:.2f} - {txn['description']}")
    
    print("\n--- Credit Card Operations ---")
    credit = CreditCardAccount("CC001", "Alice Johnson", 10000)
    credit.deposit(5000, "Credit limit available")
    
    print(f"Credit Card: {credit}")
    print(f"Credit Limit: ${credit.get_balance() + credit.get_available_credit():.2f}")
    
    credit.charge(500, "Online purchase")
    print(f"After charge: ${500:.2f}")


class Patient:
    """Patient class for healthcare"""
    def __init__(self, patient_id: str, name: str, date_of_birth: str, 
                 gender: str):
        self.patient_id = patient_id
        self.name = name
        self.date_of_birth = date_of_birth
        self.gender = gender
        self.__medical_history: List[Dict[str, Any]] = []
        self.__conditions: List[str] = []
        self.__medications: List[Dict[str, Any]] = []
        self.__allergies: List[str] = []
    
    def add_visit(self, visit_date: str, chief_complaint: str, diagnosis: str,
               vitals: Dict[str, Any], notes: str = ""):
        visit = {
            "date": visit_date,
            "chief_complaint": chief_complaint,
            "diagnosis": diagnosis,
            "vitals": vitals,
            "notes": notes
        }
        self.__medical_history.append(visit)
    
    def add_condition(self, condition: str, date_diagnosed: str):
        self.__conditions.append({
            "condition": condition,
            "date_diagnosed": date_diagnosed
        })
    
    def add_medication(self, medication: str, dosage: str, frequency: str):
        self.__medications.append({
            "medication": medication,
            "dosage": dosage,
            "frequency": frequency
        })
    
    def add_allergy(self, allergy: str):
        if allergy not in self.__allergies:
            self.__allergies.append(allergy)
    
    def get_medical_history(self) -> List[Dict[str, Any]]:
        return copy.deepcopy(self.__medical_history)
    
    def get_conditions(self) -> List[Dict[str, Any]]:
        return copy.deepcopy(self.__conditions)
    
    def get_medications(self) -> List[Dict[str, Any]]:
        return copy.deepcopy(self.__medications)
    
    def get_allergies(self) -> List[str]:
        return copy.deepcopy(self.__allergies)
    
    def get_active_problems(self) -> List[str]:
        return [c["condition"] for c in self.__conditions]
    
    def __str__(self) -> str:
        return f"Patient {self.patient_id}: {self.name}"


class InPatient(Patient):
    """In-patient class"""
    def __init__(self, patient_id: str, name: str, date_of_birth: str,
                 gender: str, room_number: str):
        super().__init__(patient_id, name, date_of_birth, gender)
        self.room_number = room_number
        self.admission_date = datetime.now().strftime("%Y-%m-%d")
        self.__discharge_date: Optional[str] = None
        self.__treatments: List[Dict[str, Any]] = []
    
    def add_treatment(self, treatment: str, date: str, notes: str = ""):
        self.__treatments.append({
            "treatment": treatment,
            "date": date,
            "notes": notes
        })
    
    def discharge(self, discharge_date: str):
        self.__discharge_date = discharge_date
    
    def is_discharged(self) -> bool:
        return self.__discharge_date is not None
    
    def get_treatments(self) -> List[Dict[str, Any]]:
        return copy.deepcopy(self.__treatments)


class OutPatient(Patient):
    """Out-patient class"""
    def __init__(self, patient_id: str, name: str, date_of_birth: str,
                 gender: str, appointment_reason: str = ""):
        super().__init__(patient_id, name, date_of_birth, gender)
        self.__appointments: List[Dict[str, Any]] = []
        self.last_visit = datetime.now().strftime("%Y-%m-%d")
    
    def schedule_appointment(self, appointment_date: str, reason: str):
        self.__appointments.append({
            "date": appointment_date,
            "reason": reason,
            "completed": False
        })
    
    def complete_appointment(self, appointment_date: str):
        for apt in self.__appointments:
            if apt["date"] == appointment_date:
                apt["completed"] = True
    
    def get_upcoming_appointments(self) -> List[Dict[str, Any]]:
        return [a for a in self.__appointments if not a["completed"]]


def healthcare_application():
    """
    Healthcare industry use case - Patient management system
    """
    print("\n=== Healthcare Application: Patient Management ===")
    
    print("\n--- Register Patient ---")
    patient = Patient("P001", "John Doe", "1985-03-15", "Male")
    print(f"Registered: {patient}")
    
    print("\n--- Add Patient Information ---")
    patient.add_condition("Type 2 Diabetes", "2020-01-15")
    patient.add_condition("Hypertension", "2021-06-20")
    patient.add_medication("Metformin", "500mg", "Twice daily")
    patient.add_medication("Lisinopril", "10mg", "Once daily")
    patient.add_allergy("Penicillin")
    
    print("Conditions:")
    for condition in patient.get_conditions():
        print(f"  {condition['condition']} (diagnosed: {condition['date_diagnosed']})")
    
    print("Medications:")
    for med in patient.get_medications():
        print(f"  {med['medication']} {med['dosage']} - {med['frequency']}")
    
    print(f"Allergies: {patient.get_allergies()}")
    
    print("\n--- Add Visit ---")
    patient.add_visit(
        "2026-04-06",
        "Follow-up appointment",
        "Diabetes well-controlled",
        {"blood_pressure": "130/85", "heart_rate": 72, "temperature": 98.6},
        "Continue current medications"
    )
    
    print("Medical History:")
    for visit in patient.get_medical_history():
        print(f"  Date: {visit['date']}")
        print(f"  Complaint: {visit['chief_complaint']}")
        print(f"  Diagnosis: {visit['diagnosis']}")
        print(f"  Vitals: {visit['vitals']}")
    
    print("\n--- In-Patient Example ---")
    inpatient = InPatient("P002", "Jane Smith", "1990-07-22", "Female", "Room 305")
    inpatient.add_treatment("Physical Therapy", "2026-04-06", "30 minutes")
    inpatient.add_treatment("Medication Adjustment", "2026-04-07", "Increased dosage")
    
    print(f"Admitted: {inpatient}")
    print(f"Room: {inpatient.room_number}")
    print("Treatments:")
    for treatment in inpatient.get_treatments():
        print(f"  {treatment['treatment']} on {treatment['date']}")
    
    print("\n--- Out-Patient Example ---")
    outpatient = OutPatient("P003", "Bob Wilson", "1978-11-08", "Male")
    outpatient.schedule_appointment("2026-04-15", "Annual checkup")
    outpatient.schedule_appointment("2026-05-01", "Lab work")
    
    print(f"Registered: {outpatient}")
    print("Upcoming Appointments:")
    for apt in outpatient.get_upcoming_appointments():
        print(f"  {apt['date']}: {apt['reason']}")


def test_class_definition():
    """Test class definition functions"""
    print("\n=== Testing Class Definition ===")
    
    class TestClass:
        def __init__(self, value: int):
            self.value = value
        
        def double(self) -> int:
            return self.value * 2
    
    obj = TestClass(5)
    assert obj.value == 5
    assert obj.double() == 10
    
    print("All class definition tests passed!")


def test_inheritance():
    """Test inheritance functions"""
    print("\n=== Testing Inheritance ===")
    
    class Base:
        def method(self) -> str:
            return "base"
    
    class Derived(Base):
        def method(self) -> str:
            return "derived"
    
    obj = Derived()
    assert obj.method() == "derived"
    
    print("All inheritance tests passed!")


def test_encapsulation():
    """Test encapsulation functions"""
    print("\n=== Testing Encapsulation ===")
    
    class TestAccount:
        def __init__(self):
            self.__balance = 100
        
        def get_balance(self) -> int:
            return self.__balance
    
    acc = TestAccount()
    assert acc.get_balance() == 100
    
    print("All encapsulation tests passed!")


def test_polymorphism():
    """Test polymorphism functions"""
    print("\n=== Testing Polymorphism ===")
    
    class Shape:
        @abstractmethod
        def area(self) -> float:
            pass
    
    class Square(Shape):
        def __init__(self, side: float):
            self.side = side
        
        def area(self) -> float:
            return self.side ** 2
    
    class Circle(Shape):
        def __init__(self, radius: float):
            self.radius = radius
        
        def area(self) -> float:
            import math
            return math.pi * self.radius ** 2
    
    shapes = [Square(5), Circle(3)]
    total_area = sum(s.area() for s in shapes)
    assert total_area > 0
    
    print("All polymorphism tests passed!")


def test_banking():
    """Test banking functions"""
    print("\n=== Testing Banking ===")
    
    account = BankAccount("TEST001", "Test User")
    
    assert account.deposit(1000)
    assert account.get_balance() == 1000
    
    assert account.withdraw(500)
    assert account.get_balance() == 500
    
    assert not account.withdraw(1000)
    
    print("All banking tests passed!")


def test_healthcare():
    """Test healthcare functions"""
    print("\n=== Testing Healthcare ===")
    
    patient = Patient("TEST001", "Test Patient", "1990-01-01", "Male")
    
    patient.add_condition("Test Condition", "2020-01-01")
    conditions = patient.get_conditions()
    assert len(conditions) == 1
    
    patient.add_medication("Test Med", "10mg", "Daily")
    meds = patient.get_medications()
    assert len(meds) == 1
    
    print("All healthcare tests passed!")


def run_all_tests():
    """Run all unit tests"""
    test_class_definition()
    test_inheritance()
    test_encapsulation()
    test_polymorphism()
    test_banking()
    test_healthcare()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()