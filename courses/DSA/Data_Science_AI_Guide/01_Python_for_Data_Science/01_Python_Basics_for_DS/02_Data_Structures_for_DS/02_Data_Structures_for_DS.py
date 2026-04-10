# Topic: Data_Structures_for_DS
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Structures for Data Science

I. INTRODUCTION
   Data structures are fundamental to data science, enabling efficient storage,
   organization, and manipulation of data. This module covers Python's built-in
   data structures including lists, tuples, sets, dictionaries, and specialized
   data structures from collections module.
   Prerequisites: Python fundamentals
   Requirements: Python 3.8+

II. CORE_CONCEPTS
   - Lists: Ordered, mutable sequences
   - Tuples: Ordered, immutable sequences
   - Sets: Unordered collections of unique elements
   - Dictionaries: Key-value pair mappings
   - Collections: deque, Counter, OrderedDict, namedtuple
   - Stacks and Queues using collections

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for data structure selection
   - Detailed comments throughout

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking/Finance - Transaction history management
   - Real-world Application 2: Healthcare - Patient record management

V. OUTPUT_RESULTS
   - Expected outputs
   - Performance analysis

VI. TESTING
   - Unit tests for main functions

VII. ADVANCED_TOPICS
   - List comprehensions
   - Dictionary comprehensions
   - Generator expressions
   - Custom data structures

VIII. CONCLUSION
   - Key takeaways
   - Next steps for learning
"""

from collections import deque, Counter, OrderedDict, namedtuple, defaultdict
from typing import Any, List, Dict, Tuple, Set, Optional, Callable
import copy


def main():
    print("Executing Data Structures for Data Science implementation")
    print("\n=== Lists ===")
    demonstrate_lists()
    
    print("\n=== Tuples ===")
    demonstrate_tuples()
    
    print("\n=== Sets ===")
    demonstrate_sets()
    
    print("\n=== Dictionaries ===")
    demonstrate_dictionaries()
    
    print("\n=== Collections Module ===")
    demonstrate_collections()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_lists():
    """Demonstrate Python lists"""
    print("\n--- Basic List Operations ---")
    numbers = [1, 2, 3, 4, 5]
    print(f"Create list: {numbers}")
    print(f"Length: {len(numbers)}")
    print(f"First element: {numbers[0]}")
    print(f"Last element: {numbers[-1]}")
    
    print("\n--- List Mutation ---")
    fruits = ["apple", "banana", "cherry"]
    fruits.append("date")
    print(f"Append: {fruits}")
    
    fruits.insert(1, "blueberry")
    print(f"Insert at index 1: {fruits}")
    
    fruits.remove("banana")
    print(f"Remove 'banana': {fruits}")
    
    popped = fruits.pop()
    print(f"Pop last: {popped}, remaining: {fruits}")
    
    print("\n--- List Slicing ---")
    data = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]
    print(f"Original: {data}")
    print(f"data[2:5]: {data[2:5]}")
    print(f"data[:3]: {data[:3]}")
    print(f"data[5:]: {data[5:]}")
    print(f"data[::2]: {data[::2]}")
    print(f"data[::-1]: {data[::-1]}")
    
    print("\n--- List Comprehensions ---")
    squares = [x**2 for x in range(1, 6)]
    print(f"Squares [x**2 for x in range(1,6)]: {squares}")
    
    evens = [x for x in range(10) if x % 2 == 0]
    print(f"Evens: {evens}")
    
    matrix = [[i*j for j in range(3)] for i in range(3)]
    print(f"3x3 Matrix: {matrix}")
    
    print("\n--- List Sorting ---")
    unsorted = [3, 1, 4, 1, 5, 9, 2, 6]
    sorted_list = sorted(unsorted)
    print(f"Unsorted: {unsorted}")
    print(f"Sorted: {sorted_list}")
    
    words = ["banana", "apple", "cherry", "date"]
    sorted_words = sorted(words, key=len)
    print(f"Sorted by length: {sorted_words}")


def demonstrate_tuples():
    """Demonstrate Python tuples"""
    print("\n--- Basic Tuple Operations ---")
    coordinates = (10, 20, 30)
    print(f"Create tuple: {coordinates}")
    print(f"Length: {len(coordinates)}")
    print(f"First element: {coordinates[0]}")
    
    print("\n--- Tuple Unpacking ---")
    x, y, z = coordinates
    print(f"Unpack: x={x}, y={y}, z={z}")
    
    point = (5, 10, 15)
    a, *b, c = point
    print(f"Extended unpack: a={a}, b={b}, c={c}")
    
    print("\n--- Named Tuples ---")
    Point = namedtuple("Point", ["x", "y"])
    p = Point(10, 20)
    print(f"Named tuple: {p}")
    print(f"p.x = {p.x}, p.y = {p.y}")
    print(f"As dict: {p._asdict()}")
    
    print("\n--- Tuple Methods ---")
    letters = ("a", "b", "a", "c", "a")
    print(f"Count 'a': {letters.count('a')}")
    print(f"Index 'b': {letters.index('b')}")
    
    print("\n--- Tuple vs List Performance ---")
    import time
    
    n = 100000
    start = time.time()
    for _ in range(n):
        t = (1, 2, 3)
    tuple_time = time.time() - start
    
    start = time.time()
    for _ in range(n):
        l = [1, 2, 3]
    list_time = time.time() - start
    
    print(f"Tuple creation: {tuple_time:.4f}s")
    print(f"List creation: {list_time:.4f}s")


def demonstrate_sets():
    """Demonstrate Python sets"""
    print("\n--- Basic Set Operations ---")
    fruits = {"apple", "banana", "cherry", "apple"}
    print(f"Create set (duplicates removed): {fruits}")
    print(f"Length: {len(fruits)}")
    
    print("\n--- Set Operations ---")
    set_a = {1, 2, 3, 4, 5}
    set_b = {4, 5, 6, 7, 8}
    
    print(f"set_a: {set_a}")
    print(f"set_b: {set_b}")
    print(f"Union: {set_a | set_b}")
    print(f"Intersection: {set_a & set_b}")
    print(f"Difference (A-B): {set_a - set_b}")
    print(f"Symmetric Difference: {set_a ^ set_b}")
    
    print("\n--- Set Methods ---")
    s = {1, 2, 3}
    s.add(4)
    print(f"Add 4: {s}")
    
    s.update([5, 6])
    print(f"Update [5,6]: {s}")
    
    s.remove(6)
    print(f"Remove 6: {s}")
    
    s.discard(100)
    print(f"Discard 100 (no error): {s}")
    
    s.pop()
    print(f"Pop: {s}")
    
    print("\n--- Set Comprehensions ---")
    unique_squares = {x**2 for x in range(-3, 4)}
    print(f"Unique squares: {unique_squares}")


def demonstrate_dictionaries():
    """Demonstrate Python dictionaries"""
    print("\n--- Basic Dictionary Operations ---")
    person = {
        "name": "John Doe",
        "age": 35,
        "city": "New York"
    }
    print(f"Create dict: {person}")
    print(f"Get 'name': {person['name']}")
    print(f"Get with get(): {person.get('email', 'Not provided')}")
    
    print("\n--- Dictionary Methods ---")
    person["email"] = "john@example.com"
    print(f"Add key: {person}")
    
    person.update({"phone": "555-1234", "country": "USA"})
    print(f"Update multiple: {person}")
    
    print(f"Keys: {list(person.keys())}")
    print(f"Values: {list(person.values())}")
    print(f"Items: {list(person.items())}")
    
    print("\n--- Dictionary Comprehensions ---")
    squares = {x: x**2 for x in range(1, 6)}
    print(f"Squares dict: {squares}")
    
    word_lengths = {word: len(word) for word in ["apple", "banana", "cherry"]}
    print(f"Word lengths: {word_lengths}")
    
    print("\n--- Nested Dictionaries ---")
    employees = {
        "emp1": {"name": "Alice", "dept": "Engineering", "salary": 75000},
        "emp2": {"name": "Bob", "dept": "Sales", "salary": 65000},
    }
    print(f"Nested dict: {employees}")
    print(f"Access nested: employees['emp1']['dept'] = {employees['emp1']['dept']}")
    
    print("\n--- OrderedDict ---")
    od = OrderedDict()
    od["first"] = 1
    od["second"] = 2
    od["third"] = 3
    print(f"OrderedDict: {od}")
    
    od.move_to_end("first")
    print(f"After move_to_end: {od}")


def demonstrate_collections():
    """Demonstrate collections module data structures"""
    print("\n--- Deque ---")
    d = deque([1, 2, 3], maxlen=5)
    print(f"Initial deque: {d}")
    
    d.append(4)
    print(f"Append 4: {d}")
    
    d.appendleft(0)
    print(f"Appendleft 0: {d}")
    
    d.pop()
    print(f"Pop: {d}")
    
    d.popleft()
    print(f"Popleft: {d}")
    
    print("\n--- Counter ---")
    letters = ["a", "b", "c", "a", "b", "a"]
    counter = Counter(letters)
    print(f"Counter: {counter}")
    
    text = "hello world"
    word_counter = Counter(text.split())
    print(f"Word counter: {word_counter}")
    
    print(f"Most common: {counter.most_common(2)}")
    
    print("\n--- DefaultDict ---")
    dd = defaultdict(list)
    dd["fruits"].append("apple")
    dd["fruits"].append("banana")
    dd["numbers"].append(1)
    print(f"DefaultDict: {dict(dd)}")
    
    print("\n--- ChainMap ---")
    baseline = {"theme": "default", "language": "en"}
    user_prefs = {"theme": "dark"}
    combined = ChainMap(user_prefs, baseline)
    print(f"ChainMap theme: {combined['theme']}")
    print(f"ChainMap language: {combined['language']}")


def practical_example():
    """Practical demonstration of data structures"""
    print("\n=== Practical Example: Inventory Management ===")
    
    inventory = {
        "A001": {"name": "Laptop", "quantity": 50, "price": 999.99},
        "A002": {"name": "Mouse", "quantity": 200, "price": 29.99},
        "A003": {"name": "Keyboard", "quantity": 150, "price": 79.99},
    }
    
    print("Inventory:")
    for sku, item in inventory.items():
        print(f"  {sku}: {item['name']} - Qty: {item['quantity']}, ${item['price']:.2f}")
    
    total_value = sum(item["quantity"] * item["price"] for item in inventory.values())
    print(f"\nTotal Inventory Value: ${total_value:,.2f}")
    
    low_stock = {sku: item for sku, item in inventory.items() 
                if item["quantity"] < 100}
    print(f"Low Stock Items: {list(low_stock.keys())}")
    
    return inventory


class Transaction:
    """Transaction class for banking application"""
    def __init__(self, transaction_id: str, date: str, amount: float, 
                 transaction_type: str, description: str):
        self.transaction_id = transaction_id
        self.date = date
        self.amount = amount
        self.type = transaction_type
        self.description = description
    
    def __repr__(self):
        return f"Transaction({self.transaction_id}, {self.amount}, {self.type})"


class BankAccount:
    """Bank account with transaction history"""
    def __init__(self, account_number: str, account_holder: str):
        self.account_number = account_number
        self.account_holder = account_holder
        self.balance = 0.0
        self.transactions: List[Transaction] = []
    
    def deposit(self, amount: float, description: str = "") -> bool:
        if amount <= 0:
            return False
        
        self.balance += amount
        txn = Transaction(
            transaction_id=f"TXN{len(self.transactions) + 1:06d}",
            date="2026-04-06",
            amount=amount,
            transaction_type="DEPOSIT",
            description=description or f"Deposit to account"
        )
        self.transactions.append(txn)
        return True
    
    def withdraw(self, amount: float, description: str = "") -> bool:
        if amount <= 0 or amount > self.balance:
            return False
        
        self.balance -= amount
        txn = Transaction(
            transaction_id=f"TXN{len(self.transactions) + 1:06d}",
            date="2026-04-06",
            amount=-amount,
            transaction_type="WITHDRAWAL",
            description=description or f"Withdrawal from account"
        )
        self.transactions.append(txn)
        return True
    
    def get_transaction_history(self, 
                                  transaction_type: Optional[str] = None) -> List[Transaction]:
        if transaction_type is None:
            return self.transactions
        
        return [t for t in self.transactions if t.type == transaction_type]
    
    def get_balance_summary(self) -> Dict[str, Any]:
        deposits = sum(t.amount for t in self.transactions if t.type == "DEPOSIT")
        withdrawals = sum(abs(t.amount) for t in self.transactions if t.type == "WITHDRAWAL")
        
        return {
            "account_number": self.account_number,
            "account_holder": self.account_holder,
            "current_balance": self.balance,
            "total_deposits": deposits,
            "total_withdrawals": withdrawals,
            "transaction_count": len(self.transactions)
        }


def banking_application():
    """
    Banking industry use case - Transaction history management
    """
    print("\n=== Banking Application: Transaction Management ===")
    
    print("\n--- Create Account ---")
    account = BankAccount("1234567890", "John Doe")
    print(f"Created: {account.account_number} - {account.account_holder}")
    
    print("\n--- Process Transactions ---")
    account.deposit(5000.00, "Initial Deposit")
    account.deposit(2500.00, "Salary")
    account.withdraw(500.00, "ATM Withdrawal")
    account.deposit(1500.00, "Transfer from Savings")
    account.withdraw(200.00, "Online Purchase")
    
    print(f"Current Balance: ${account.balance:,.2f}")
    
    print("\n--- Transaction History ---")
    for txn in account.transactions:
        print(f"  {txn.transaction_id}: {txn.type} - ${abs(txn.amount):,.2f}")
        print(f"    Description: {txn.description}")
    
    print("\n--- Balance Summary ---")
    summary = account.get_balance_summary()
    print(f"Account: {summary['account_number']}")
    print(f"Holder: {summary['account_hoder']}")
    print(f"Current Balance: ${summary['current_balance']:,.2f}")
    print(f"Total Deposits: ${summary['total_deposits']:,.2f}")
    print(f"Total Withdrawals: ${summary['total_withdrawals']:,.2f}")
    print(f"Transaction Count: {summary['transaction_count']}")
    
    print("\n--- Filter Transactions ---")
    deposits = account.get_transaction_history("DEPOSIT")
    print(f"Deposit Transactions: {len(deposits)}")
    
    withdrawals = account.get_transaction_history("WITHDRAWAL")
    print(f"Withdrawal Transactions: {len(withdrawals)}")


class Patient:
    """Patient class for healthcare application"""
    def __init__(self, patient_id: str, name: str, dob: str, gender: str):
        self.patient_id = patient_id
        self.name = name
        self.dob = dob
        self.gender = gender
        self.visits: List[Dict[str, Any]] = []
        self.conditions: Set[str] = set()
        self.medications: List[str] = []
    
    def add_visit(self, date: str, chief_complaint: str, diagnosis: str,
                  vitals: Dict[str, float], notes: str = ""):
        visit = {
            "date": date,
            "chief_complaint": chief_complaint,
            "diagnosis": diagnosis,
            "vitals": vitals,
            "notes": notes
        }
        self.visits.append(visit)
    
    def add_condition(self, condition: str):
        self.conditions.add(condition)
    
    def add_medication(self, medication: str):
        if medication not in self.medications:
            self.medications.append(medication)
    
    def get_medical_history(self) -> Dict[str, Any]:
        return {
            "patient_id": self.patient_id,
            "name": self.name,
            "dob": self.dob,
            "gender": self.gender,
            "conditions": list(self.conditions),
            "medications": self.medications,
            "visits": self.visits
        }


class PatientRegistry:
    """Patient registry for healthcare management"""
    def __init__(self):
        self.patients: Dict[str, Patient] = {}
    
    def register_patient(self, patient_id: str, name: str, dob: str, gender: str) -> Patient:
        patient = Patient(patient_id, name, dob, gender)
        self.patients[patient_id] = patient
        return patient
    
    def get_patient(self, patient_id: str) -> Optional[Patient]:
        return self.patients.get(patient_id)
    
    def search_by_condition(self, condition: str) -> List[Patient]:
        return [p for p in self.patients.values() if condition in p.conditions]
    
    def get_all_conditions(self) -> Counter:
        all_conditions = []
        for patient in self.patients.values():
            all_conditions.extend(patient.conditions)
        return Counter(all_conditions)


def healthcare_application():
    """
    Healthcare industry use case - Patient record management
    """
    print("\n=== Healthcare Application: Patient Records ===")
    
    print("\n--- Create Patient Registry ---")
    registry = PatientRegistry()
    
    print("\n--- Register Patients ---")
    p1 = registry.register_patient("P001", "John Smith", "1985-03-15", "Male")
    p1.add_condition("Hypertension")
    p1.add_condition("Type 2 Diabetes")
    p1.add_medication("Lisinopril 10mg")
    p1.add_medication("Metformin 500mg")
    p1.add_visit("2026-01-15", "Follow-up", "Well controlled", 
                {"bp": 130/85, "heart_rate": 72}, "Continue current medications")
    
    p2 = registry.register_patient("P002", "Jane Doe", "1990-07-22", "Female")
    p2.add_condition("Asthma")
    p2.add_medication("Albuterol inhaler")
    p2.add_visit("2026-02-20", "Shortness of breath", "Mild exacerbation",
                {"bp": 120/80, "heart_rate": 88}, "Increase rescue inhaler use")
    
    p3 = registry.register_patient("P003", "Bob Wilson", "1978-11-08", "Male")
    p3.add_condition("Type 2 Diabetes")
    p3.add_condition("High Cholesterol")
    p3.add_medication("Metformin 500mg")
    p3.add_medication("Atorvastatin 20mg")
    p3.add_visit("2026-03-10", "Annual checkup", "Diabetes stable",
                {"bp": 125/82, "heart_rate": 70}, "Good control, continue medications")
    
    print(f"Registered {len(registry.patients)} patients")
    
    print("\n--- Patient Information ---")
    for patient_id, patient in registry.patients.items():
        print(f"\n{patient_id}: {patient.name}")
        print(f"  DOB: {patient.dob}, Gender: {patient.gender}")
        print(f"  Conditions: {', '.join(patient.conditions)}")
        print(f"  Medications: {', '.join(patient.medications)}")
        print(f"  Visits: {len(patient.visits)}")
    
    print("\n--- Search by Condition ---")
    diabetes_patients = registry.search_by_condition("Type 2 Diabetes")
    print(f"Patients with Diabetes: {[p.patient_id for p in diabetes_patients]}")
    
    print("\n--- Condition Statistics ---")
    condition_stats = registry.get_all_conditions()
    print("Most common conditions:")
    for condition, count in condition_stats.most_common():
        print(f"  {condition}: {count} patients")


def test_lists():
    """Test list functions"""
    print("\n=== Testing Lists ===")
    
    test_list = [1, 2, 3, 4, 5]
    
    assert len(test_list) == 5
    assert test_list[0] == 1
    assert test_list[-1] == 5
    
    test_list.append(6)
    assert 6 in test_list
    
    squares = [x**2 for x in range(5)]
    assert squares == [0, 1, 4, 9, 16]
    
    print("All list tests passed!")


def test_tuples():
    """Test tuple functions"""
    print("\n=== Testing Tuples ===")
    
    coord = (10, 20, 30)
    
    assert len(coord) == 3
    assert coord[0] == 10
    
    x, y, z = coord
    assert x == 10
    
    Point = namedtuple("Point", ["x", "y"])
    p = Point(5, 10)
    assert p.x == 5 and p.y == 10
    
    print("All tuple tests passed!")


def test_sets():
    """Test set functions"""
    print("\n=== Testing Sets ===")
    
    s = {1, 2, 3, 2, 1}
    
    assert len(s) == 3
    assert 1 in s
    
    set_a = {1, 2, 3}
    set_b = {2, 3, 4}
    
    assert set_a & set_b == {2, 3}
    assert set_a | set_b == {1, 2, 3, 4}
    assert set_a - set_b == {1}
    assert set_a ^ set_b == {1, 4}
    
    print("All set tests passed!")


def test_dictionaries():
    """Test dictionary functions"""
    print("\n=== Testing Dictionaries ===")
    
    d = {"a": 1, "b": 2}
    
    assert d["a"] == 1
    assert d.get("c", 3) == 3
    
    d["c"] = 3
    assert len(d) == 3
    
    d2 = {k: v**2 for k, v in d.items()}
    assert d2 == {"a": 1, "b": 4, "c": 9}
    
    print("All dictionary tests passed!")


def test_collections():
    """Test collections functions"""
    print("\n=== Testing Collections ===")
    
    d = deque([1, 2, 3])
    d.append(4)
    d.appendleft(0)
    assert list(d) == [0, 1, 2, 3, 4]
    
    c = Counter([1, 2, 2, 3, 3, 3])
    assert c[2] == 2
    assert c.most_common(1)[0][0] == 3
    
    dd = defaultdict(list)
    dd["a"].append(1)
    assert dd["a"] == [1]
    
    print("All collections tests passed!")


def test_banking_functions():
    """Test banking functions"""
    print("\n=== Testing Banking Functions ===")
    
    account = BankAccount("123", "Test User")
    
    assert account.deposit(1000)
    assert account.balance == 1000
    
    assert account.withdraw(500)
    assert account.balance == 500
    
    assert not account.withdraw(1000)
    assert account.balance == 500
    
    print("All banking tests passed!")


def test_healthcare_functions():
    """Test healthcare functions"""
    print("\n=== Testing Healthcare Functions ===")
    
    registry = PatientRegistry()
    p = registry.register_patient("P001", "Test Patient", "1990-01-01", "Male")
    
    assert p.patient_id == "P001"
    assert p.name == "Test Patient"
    
    p.add_condition("Test Condition")
    assert "Test Condition" in p.conditions
    
    results = registry.search_by_condition("Test Condition")
    assert len(results) == 1
    
    print("All healthcare tests passed!")


def run_all_tests():
    """Run all unit tests"""
    test_lists()
    test_tuples()
    test_sets()
    test_dictionaries()
    test_collections()
    test_banking_functions()
    test_healthcare_functions()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()