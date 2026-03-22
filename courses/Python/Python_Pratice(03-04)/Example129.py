# Example129.py
# Topic: Searching Patterns


# ============================================================
# Example 1: Linear Search
# ============================================================
print("=== Linear Search ===")

def linear_search(arr, target):
    for i, val in enumerate(arr):
        if val == target:
            return i
    return -1

numbers = [4, 2, 7, 1, 9, 5]
print(f"Index of 7: {linear_search(numbers, 7)}")
print(f"Index of 3: {linear_search(numbers, 3)}")


# ============================================================
# Example 2: Binary Search (sorted list)
# ============================================================
print("\n=== Binary Search ===")

import bisect

numbers = [1, 3, 5, 7, 9, 11, 13, 15]

index = bisect.bisect_left(numbers, 7)
print(f"Index of 7: {index}")

index = bisect.bisect_left(numbers, 8)
print(f"Index for 8 (insertion point): {index}")


# ============================================================
# Example 3: Find All Occurrences
# ============================================================
print("\n=== Find All Occurrences ===")

def find_all(arr, target):
    return [i for i, val in enumerate(arr) if val == target]

numbers = [1, 2, 3, 2, 4, 2, 5]
print(f"All indices of 2: {find_all(numbers, 2)}")


# ============================================================
# Example 4: Search in List of Dicts
# ============================================================
print("\n=== Search in List of Dicts ===")

users = [
    {"id": 1, "name": "Alice", "email": "alice@example.com"},
    {"id": 2, "name": "Bob", "email": "bob@example.com"},
    {"id": 3, "name": "Charlie", "email": "charlie@example.com"},
]

def find_by_key(arr, key, value):
    return next((item for item in arr if item.get(key) == value), None)

user = find_by_key(users, "name", "Bob")
print(f"Found user: {user}")


# ============================================================
# Example 5: Filter with Multiple Conditions
# ============================================================
print("\n=== Filter Multiple Conditions ===")

products = [
    {"name": "Laptop", "price": 999, "category": "electronics"},
    {"name": "Shirt", "price": 29, "category": "clothing"},
    {"name": "Phone", "price": 599, "category": "electronics"},
    {"name": "Pants", "price": 49, "category": "clothing"},
]

def filter_products(products, **criteria):
    return [p for p in products if all(p.get(k) == v for k, v in criteria.items())]

electronics = filter_products(products, category="electronics")
print(f"Electronics: {[p['name'] for p in electronics]}")

expensive = filter_products(products, category="clothing", price__gt=30)
print(f"Clothing > $30: {[p['name'] for p in expensive]}")


# ============================================================
# Example 6: Real-World: Search Engine Simple
# ============================================================
print("\n=== Real-World: Simple Search ===")

documents = [
    {"id": 1, "title": "Python Tutorial", "content": "Learn Python programming"},
    {"id": 2, "title": "Java Guide", "content": "Java programming basics"},
    {"id": 3, "title": "Python Tips", "content": "Advanced Python tricks"},
]

def search(documents, query):
    query = query.lower()
    results = []
    for doc in documents:
        if query in doc["title"].lower() or query in doc["content"].lower():
            results.append(doc)
    return results

results = search(documents, "python")
for r in results:
    print(f"  - {r['title']}")


# ============================================================
# Example 7: Bisect with Custom Key
# ============================================================
print("\n=== Bisect with Custom Key ===")

import bisect

class Item:
    def __init__(self, name, price):
        self.name = name
        self.price = price
    def __repr__(self):
        return f"Item({self.name}, {self.price})"

items = [Item("a", 10), Item("c", 30), Item("e", 50)]
prices = [i.price for i in items]

index = bisect.bisect_left(prices, 25)
print(f"Insert position for 25: {index}")
