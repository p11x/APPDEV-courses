# Example47.py
# Topic: Lambda Functions - Advanced Usage and Common Patterns

# This file demonstrates advanced lambda patterns and when to use
# (or avoid) lambdas in Python.


from functools import reduce


# Lambda with sorted() and multiple keys
print("=== Lambda with Multiple Sort Keys ===")
people = [
    {"name": "Alice", "age": 30, "department": "Engineering"},
    {"name": "Bob", "age": 25, "department": "Marketing"},
    {"name": "Charlie", "age": 30, "department": "Engineering"},
    {"name": "Diana", "age": 25, "department": "Sales"}
]

# Sort by department, then by age
sorted_people = sorted(people, key=lambda p: (p["department"], p["age"]))
print("Sorted by department then age:")
for p in sorted_people:
    print(f"  {p['department']}: {p['name']}, {p['age']}")


# Sort by name length, then alphabetically
sorted_by_length = sorted(people, key=lambda p: (len(p["name"]), p["name"]))
print("\nSorted by name length, then alphabetically:")
for p in sorted_by_length:
    print(f"  {p['name']} ({len(p['name'])} chars)")


# Lambda with reduce() - accumulate values
print("\n=== Lambda with reduce() ===")
numbers = [1, 2, 3, 4, 5]

# Sum all numbers
total = reduce(lambda x, y: x + y, numbers)
print(f"Sum: {total}")  # 15

# Find product
product = reduce(lambda x, y: x * y, numbers)
print(f"Product: {product}")  # 120

# Find maximum
maximum = reduce(lambda x, y: x if x > y else y, numbers)
print(f"Maximum: {maximum}")  # 5


# Lambda with any() and all()
print("\n=== Lambda with any() and all() ===")
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

has_even = any(lambda x: x % 2 == 0, numbers)
print(f"Has even numbers: {has_even}")  # True

all_positive = all(lambda x: x > 0, numbers)
print(f"All positive: {all_positive}")  # True

has_negative = any(lambda x: x < 0, numbers)
print(f"Has negative: {has_negative}")  # False


# Lambda in list comprehension context
print("\n=== Lambda Alternative: List Comprehensions ===")
numbers = [1, 2, 3, 4, 5]

# Map equivalent
doubled = [x * 2 for x in numbers]
print(f"Doubled (comprehension): {doubled}")

# Filter equivalent
evens = [x for x in numbers if x % 2 == 0]
print(f"Evens (comprehension): {evens}")

# Combined (filter then map)
squared_evens = [x ** 2 for x in numbers if x % 2 == 0]
print(f"Squared evens: {squared_evens}")


# Lambda with sorted() on tuples
print("\n=== Lambda with Tuples ===")
tuples = [(1, 5), (3, 2), (2, 8), (4, 1)]

# Sort by first element
sorted_by_first = sorted(tuples, key=lambda t: t[0])
print(f"Sorted by first: {sorted_by_first}")

# Sort by second element
sorted_by_second = sorted(tuples, key=lambda t: t[1])
print(f"Sorted by second: {sorted_by_second}")

# Sort by sum of elements
sorted_by_sum = sorted(tuples, key=lambda t: t[0] + t[1])
print(f"Sorted by sum: {sorted_by_sum}")


# Lambda with enumerate
print("\n=== Lambda with enumerate ===")
fruits = ["apple", "banana", "cherry"]

# Add index to each fruit
indexed = list(map(lambda x: f"{x[0]+1}. {x[1]}", enumerate(fruits)))
print(f"Indexed: {indexed}")

# Filter by index (even index only)
even_index = list(filter(lambda x: x[0] % 2 == 0, enumerate(fruits)))
print(f"Even indexed: {[f[1] for f in even_index]}")


# Lambda with zip()
print("\n=== Lambda with zip() ===")
names = ["Alice", "Bob", "Charlie"]
ages = [25, 30, 35]
cities = ["NYC", "LA", "Chicago"]

# Combine into dictionary
combined = dict(map(lambda x: (x[0], x[1]), zip(names, ages)))
print(f"Name to age: {combined}")

# Combine multiple lists
zipped = list(map(lambda x: f"{x[0][0]} is {x[0][1]} from {x[1]}", zip(zip(names, ages), cities)))
print(f"Zipped: {zipped}")


# Lambda with groupby (simulated)
print("\n=== Lambda: Grouping Simulation ===")
from collections import defaultdict

data = [
    {"category": "fruit", "name": "apple"},
    {"category": "vegetable", "name": "carrot"},
    {"category": "fruit", "name": "banana"},
    {"category": "vegetable", "name": "broccoli"},
    {"category": "fruit", "name": "orange"}
]

# Group by category
grouped = defaultdict(list)
for item in data:
    grouped[item["category"]].append(item["name"])

print("Grouped by category:")
for category, items in grouped.items():
    print(f"  {category}: {items}")


# Real-life Example 1: Data transformation pipeline
print("\n=== Real-life: Data Pipeline ===")
orders = [
    {"id": 1, "items": 3, "total": 150.00, "status": "completed"},
    {"id": 2, "items": 1, "total": 25.50, "status": "pending"},
    {"id": 3, "items": 5, "total": 500.00, "status": "completed"},
    {"id": 4, "items": 2, "total": 75.00, "status": "cancelled"}
]

# Filter completed orders, get their totals
completed_totals = list(map(
    lambda o: o["total"],
    filter(lambda o: o["status"] == "completed", orders)
))
print(f"Completed order totals: {completed_totals}")
print(f"Total revenue: ${sum(completed_totals):.2f}")


# Real-life Example 2: User permissions checker
print("\n=== Real-life: Permission Checker ===")
user_permissions = {
    "alice": ["read", "write", "delete"],
    "bob": ["read"],
    "charlie": ["read", "write"]
}

def has_permission(user, permission):
    return permission in user_permissions.get(user, [])

def has_any_permission(user, permissions):
    return any(lambda p: has_permission(user, p), permissions)

def has_all_permissions(user, permissions):
    return all(lambda p: has_permission(user, p), permissions)

print(f"Alice has write: {has_permission('alice', 'write')}")  # True
print(f"Bob has write: {has_permission('bob', 'write')}")  # False
print(f"Charlie has read or delete: {has_any_permission('charlie', ['read', 'delete'])}")  # True
print(f"Alice has all: {has_all_permissions('alice', ['read', 'write'])}")  # True


# Real-life Example 3: Calculate grade statistics
print("\n=== Real-life: Grade Statistics ===")
grades = [
    {"student": "Alice", "grade": 85},
    {"student": "Bob", "grade": 92},
    {"student": "Charlie", "grade": 78},
    {"student": "Diana", "grade": 88},
    {"student": "Eve", "grade": 95}
]

# Average grade
avg_grade = reduce(lambda x, y: x + y["grade"], grades, 0) / len(grades)
print(f"Average grade: {avg_grade:.1f}")

# Highest and lowest
highest = max(grades, key=lambda g: g["grade"])
lowest = min(grades, key=lambda g: g["grade"])
print(f"Highest: {highest['student']} - {highest['grade']}")
print(f"Lowest: {lowest['student']} - {lowest['grade']}")

# Passing grades (>= 80)
passing = list(filter(lambda g: g["grade"] >= 80, grades))
print(f"Passing: {[g['student'] for g in passing]}")


# Real-life Example 4: Inventory management
print("\n=== Real-life: Inventory Management ===")
inventory = [
    {"item": "Laptop", "stock": 50, "price": 999},
    {"item": "Mouse", "stock": 200, "price": 29},
    {"item": "Keyboard", "stock": 0, "price": 79},
    {"item": "Monitor", "stock": 30, "price": 299}
]

# Out of stock items
out_of_stock = list(filter(lambda i: i["stock"] == 0, inventory))
print(f"Out of stock: {[i['item'] for i in out_of_stock]}")

# Low stock (less than 50)
low_stock = list(filter(lambda i: 0 < i["stock"] < 50, inventory))
print(f"Low stock: {[i['item'] for i in low_stock]}")

# Total inventory value
total_value = reduce(lambda x, i: x + i["stock"] * i["price"], inventory, 0)
print(f"Total inventory value: ${total_value:,}")


# Real-life Example 5: Date-based filtering
print("\n=== Real-life: Date Filtering ===")
events = [
    {"name": "Event A", "date": "2024-01-15"},
    {"name": "Event B", "date": "2024-03-20"},
    {"name": "Event C", "date": "2024-02-10"},
    {"name": "Event D", "date": "2024-01-25"}
]

# Sort by date
sorted_events = sorted(events, key=lambda e: e["date"])
print("Events sorted by date:")
for e in sorted_events:
    print(f"  {e['date']}: {e['name']}")

# Filter January events
jan_events = list(filter(lambda e: e["date"].startswith("2024-01"), events))
print(f"January events: {[e['name'] for e in jan_events]}")


# Real-life Example 6: Nested data processing
print("\n=== Real-life: Nested Data Processing ===")
companies = [
    {
        "name": "TechCorp",
        "employees": [
            {"name": "Alice", "salary": 80000},
            {"name": "Bob", "salary": 75000}
        ]
    },
    {
        "name": "DataInc",
        "employees": [
            {"name": "Charlie", "salary": 90000},
            {"name": "Diana", "salary": 85000}
        ]
    }
]

# Total salary per company
company_totals = list(map(
    lambda c: {"name": c["name"], "total": reduce(lambda x, e: x + e["salary"], c["employees"], 0)},
    companies
))
print("Total salaries by company:")
for ct in company_totals:
    print(f"  {ct['name']}: ${ct['total']:,}")

# Highest paid employee overall
highest_paid = max(
    reduce(lambda x, c: x + c["employees"], companies, []),
    key=lambda e: e["salary"]
)
print(f"Highest paid: {highest_paid['name']} - ${highest_paid['salary']:,}")
