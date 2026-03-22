# Example128.py
# Topic: Sorting Basics and Custom Sorting


# ============================================================
# Example 1: Basic Sorting
# ============================================================
print("=== Basic Sorting ===")

numbers = [5, 2, 8, 1, 9]
print(f"Original: {numbers}")

sorted_asc = sorted(numbers)
print(f"Sorted asc: {sorted_asc}")

sorted_desc = sorted(numbers, reverse=True)
print(f"Sorted desc: {sorted_desc}")

numbers.sort()
print(f"In-place sort: {numbers}")


# ============================================================
# Example 2: Sort with Key Function
# ============================================================
print("\n=== Sort with Key ===")

words = ["apple", "banana", "Cherry", "date"]
print(f"Original: {words}")

by_length = sorted(words, key=len)
print(f"By length: {by_length}")

by_lower = sorted(words, key=str.lower)
print(f"By lowercase: {by_lower}")


# ============================================================
# Example 3: Sort Objects by Attribute
# ============================================================
print("\n=== Sort Objects ===")

class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    def __repr__(self):
        return f"Person({self.name}, {self.age})"

people = [
    Person("Alice", 30),
    Person("Bob", 25),
    Person("Charlie", 35),
]

by_age = sorted(people, key=lambda p: p.age)
print(f"By age: {by_age}")

by_name = sorted(people, key=lambda p: p.name)
print(f"By name: {by_name}")


# ============================================================
# Example 4: Multiple Sort Keys
# ============================================================
print("\n=== Multiple Sort Keys ===")

data = [
    ("apple", 10),
    ("banana", 2),
    ("apple", 5),
    ("banana", 8),
]

sorted_data = sorted(data, key=lambda x: (x[0], x[1]))
print(f"Sorted (name, qty): {sorted_data}")


# ============================================================
# Example 5: Sort with operator.itemgetter
# ============================================================
print("\n=== itemgetter ===")

from operator import itemgetter

data = [
    {"name": "Alice", "age": 30},
    {"name": "Bob", "age": 25},
    {"name": "Charlie", "age": 35},
]

by_name = sorted(data, key=itemgetter("name"))
print(f"By name: {by_name}")


# ============================================================
# Example 6: Sort Strings with Numbers
# ============================================================
print("\n=== Natural Sort ===")

import re

def natural_sort_key(s):
    return [int(c) if c.isdigit() else c.lower() for c in re.split(r'(\d+)', s)]

files = ["file1.txt", "file10.txt", "file2.txt", "file20.txt"]
print(f"Original: {files}")
print(f"Natural sort: {sorted(files, key=natural_sort_key)}")


# ============================================================
# Example 7: Real-World: Sort Products
# ============================================================
print("\n=== Real-World: Products ===")

products = [
    {"name": "Laptop", "price": 999.99, "rating": 4.5},
    {"name": "Mouse", "price": 29.99, "rating": 4.2},
    {"name": "Keyboard", "price": 79.99, "rating": 4.7},
]

by_price = sorted(products, key=lambda p: p["price"])
print(f"By price: {[p['name'] for p in by_price]}")

by_rating = sorted(products, key=lambda p: p["rating"], reverse=True)
print(f"By rating: {[p['name'] for p in by_rating]}")
