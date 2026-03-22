# Example123.py
# Topic: Practical Data Processing

# Practical data processing examples.


# ============================================================
# Example 1: Data Filtering
# ============================================================
print("=== Filtering ===")

data = [
    {"name": "Alice", "age": 30, "dept": "IT"},
    {"name": "Bob", "age": 25, "dept": "HR"},
    {"name": "Charlie", "age": 35, "dept": "IT"},
    {"name": "Diana", "age": 28, "dept": "Finance"},
]

# Filter IT dept
it_staff = [p for p in data if p["dept"] == "IT"]
print(f"IT: {[p['name'] for p in it_staff]}")

# Age > 28
older = [p for p in data if p["age"] > 28]
print(f"Older: {[p['name'] for p in older]}")


# ============================================================
# Example 2: Data Transformation
# ============================================================
print("\n=== Transformation ===")

data = [1, 2, 3, 4, 5]

# Double
doubled = [x * 2 for x in data]
print(f"Doubled: {doubled}")

# Flatten
matrix = [[1, 2], [3, 4]]
flat = [num for row in matrix for num in row]
print(f"Flat: {flat}")


# ============================================================
# Example 3: Grouping Data
# ============================================================
print("\n=== Grouping ===")

from collections import defaultdict

data = [
    {"name": "Alice", "dept": "IT"},
    {"name": "Bob", "dept": "HR"},
    {"name": "Charlie", "dept": "IT"},
]

by_dept = defaultdict(list)
for person in data:
    by_dept[person["dept"]].append(person["name"])

print(f"By dept: {dict(by_dept)}")


# ============================================================
# Example 4: Aggregation
# ============================================================
print("\n=== Aggregation ===")

from collections import Counter

orders = [
    {"product": "Apple", "qty": 3, "price": 1.50},
    {"product": "Banana", "qty": 2, "price": 0.50},
    {"product": "Apple", "qty": 1, "price": 1.50},
]

# Total revenue
total = sum(o["qty"] * o["price"] for o in orders)
print(f"Total: ${total:.2f}")

# Count by product
counts = Counter(o["product"] for o in orders)
print(f"Counts: {dict(counts)}")


# ============================================================
# Example 5: Sorting
# ============================================================
print("\n=== Sorting ===")

data = [
    {"name": "Alice", "age": 30},
    {"name": "Bob", "age": 25},
    {"name": "Charlie", "age": 35},
]

# By age
by_age = sorted(data, key=lambda x: x["age"])
print(f"By age: {[p['name'] for p in by_age]}")

# By age descending
by_age_desc = sorted(data, key=lambda x: x["age"], reverse=True)
print(f"By age desc: {[p['name'] for p in by_age_desc]}")
