# Example180.py
# Topic: operator Module (itemgetter, attrgetter)

# This file demonstrates the operator module's itemgetter and attrgetter.
# These provide efficient, callable alternatives to lambda functions
# for accessing items and attributes.


# ============================================================
# Example 1: itemgetter for Tuples
# ============================================================
print("=== itemgetter for Tuples ===")

from operator import itemgetter

pairs = [(1, 3), (2, 1), (3, 2), (1, 1), (2, 3)]

# Get first element using itemgetter
get_first = itemgetter(0)    # callable — gets first item
result = get_first(pairs[2])    # int — first element of tuple
print(f"First of (3, 2): {result}")    # First of (3, 2): 3

# Sort tuples by first element
sorted_by_first = sorted(pairs, key=itemgetter(0))    # list — sorted by first
print(f"By first: {sorted_by_first}")    # By first: [(1, 1), (1, 3), (2, 1), (2, 3), (3, 2)]

# Sort tuples by second element
sorted_by_second = sorted(pairs, key=itemgetter(1))    # list — sorted by second
print(f"By second: {sorted_by_second}")    # By second: [(1, 1), (2, 1), (3, 2), (1, 3), (2, 3)]


# ============================================================
# Example 2: itemgetter for Dictionaries
# ============================================================
print("\n=== itemgetter for Dictionaries ===")

from operator import itemgetter

data = [
    {"name": "Alice", "score": 85},
    {"name": "Bob", "score": 92},
    {"name": "Carol", "score": 78}
]

# Get specific key value (dict[key] style)
get_name = itemgetter("name")    # callable — gets name key
result = get_name(data[0])    # str — value of name key
print(f"Name: {result}")    # Name: Alice

# Sort list of dicts by key
sorted_by_score = sorted(data, key=itemgetter("score"))    # list — by score
print(f"By score: {sorted_by_score}")    # By score: [{'name': 'Carol', 'score': 78}, ...]


# ============================================================
# Example 3: attrgetter for Objects
# ============================================================
print("\n=== attrgetter for Objects ===")

from operator import attrgetter

class Person:
    def __init__(self, name: str, age: int):
        self.name = name
        self.age = age
    
    def __repr__(self):
        return f"Person({self.name}, {self.age})"

people = [
    Person("Alice", 30),
    Person("Bob", 25),
    Person("Carol", 35)
]

# Get attribute using attrgetter
get_age = attrgetter("age")    # callable — gets age attribute
result = get_age(people[0])    # int — age value
print(f"Age: {result}")    # Age: 30

# Sort by attribute
sorted_by_age = sorted(people, key=attrgetter("age"))    # list — sorted by age
print(f"By age: {sorted_by_age}")    # By age: [Person(Bob, 25), Person(Alice, 30), Person(Carol, 35)]

# Sort by name
sorted_by_name = sorted(people, key=attrgetter("name"))    # list — sorted by name
print(f"By name: {sorted_by_name}")    # By name: [Person(Alice, 30), Person(Bob, 25), Person(Carol, 35)]


# ============================================================
# Example 4: Multiple Index/itemgetter
# ============================================================
print("\n=== Multiple Index/itemgetter ===")

from operator import itemgetter

pairs = [(1, 3), (2, 1), (3, 2)]

# Get multiple items - returns tuple
get_both = itemgetter(0, 1)    # callable — gets multiple items
result = get_both(pairs[0])    # tuple — both values
print(f"Both: {result}")    # Both: (1, 3)

# Sort by multiple indices
sorted_multi = sorted(pairs, key=itemgetter(1, 0))    # list — by second, then first
print(f"By second, first: {sorted_multi}")    # By second, first: [(2, 1), (3, 2), (1, 3)]


# ============================================================
# Example 5: Nested Attribute Access
# ============================================================
print("\n=== Nested Attribute Access ===")

from operator import attrgetter

class Department:
    def __init__(self, name: str):
        self.name = name

class Employee:
    def __init__(self, name: str, dept: Department):
        self.name = name
        self.dept = dept
    
    def __repr__(self):
        return f"Employee({self.name}, {self.dept.name})"

dept1 = Department("Engineering")
dept2 = Department("Sales")

employees = [
    Employee("Alice", dept1),
    Employee("Bob", dept2),
    Employee("Carol", dept1)
]

# Access nested attribute
get_dept_name = attrgetter("dept.name")    # callable — nested access
result = get_dept_name(employees[0])    # str — department name
print(f"Dept name: {result}")    # Dept name: Engineering


# ============================================================
# Example 6: Using with max/min
# ============================================================
print("\n=== Using with max/min ===")

from operator import itemgetter, attrgetter

# With tuples
data = [(1, 3), (2, 1), (3, 2)]
max_by_second = max(data, key=itemgetter(1))    # tuple — with max second
print(f"Max by second: {max_by_second}")    # Max by second: (1, 3)

# With dicts
users = [
    {"name": "Alice", "score": 85},
    {"name": "Bob", "score": 92}
]
top_scorer = max(users, key=itemgetter("score"))    # dict — max score
print(f"Top scorer: {top_scorer}")    # Top scorer: {'name': 'Bob', 'score': 92}


# ============================================================
# Example 7: Using with groupby
# ============================================================
print("\n=== Using with itertools ===")

from itertools import groupby
from operator import itemgetter

data = [("A", 1), ("A", 2), ("B", 1), ("B", 3), ("A", 3)]

# Sort by first element for groupby
data.sort(key=itemgetter(0))    # list — sorted for grouping

# Group by first element
grouped = {}    # dict — grouped results
for key, group in groupby(data, key=itemgetter(0)):
    grouped[key] = list(group)

print(f"Grouped: {grouped}")    # Grouped: {'A': [('A', 1), ('A', 2), ('A', 3)], ...}


# ============================================================
# Example 8: Performance Comparison
# ============================================================
print("\n=== Performance Comparison ===")

import time
from operator import itemgetter

data = [(i, i*2) for i in range(10000)]

# Lambda vs itemgetter
start = time.time()
for _ in range(100):
    sorted(data, key=lambda x: x[1])
lambda_time = time.time() - start

start = time.time()
for _ in range(100):
    sorted(data, key=itemgetter(1))
itemgetter_time = time.time() - start

print(f"Lambda time: {lambda_time:.4f}s")    # Lambda time: seconds
print(f"itemgetter time: {itemgetter_time:.4f}s")    # itemgetter time: seconds


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
operator MODULE:
- itemgetter(n): Gets item at index n
- itemgetter(n, m): Returns tuple of items
- attrgetter("name"): Gets attribute
- attrgetter("a.b"): Gets nested attribute

ADVANTAGES:
- Faster than lambda functions
- More readable than lambda x: x[1]
- Can be reused and pickled
- Supports nested access

USES:
- sorted(), min(), max() with key=
- groupby with key=
- Accessing tuple elements
- Accessing object attributes
""")
