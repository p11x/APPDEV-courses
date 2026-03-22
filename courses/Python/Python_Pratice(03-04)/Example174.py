# Example174.py
# Topic: Linear Search Algorithm

# This file demonstrates linear search algorithm implementation.
# Linear search checks each element sequentially until the target
# is found or the list is exhausted. Time complexity: O(n).


# ============================================================
# Example 1: Basic Linear Search
# ============================================================
print("=== Basic Linear Search ===")

# Iterates through list checking each element for match
def linear_search(items: list[int], target: int) -> int:
    for i, item in enumerate(items):
        if item == target:
            return i
    return -1

numbers = [4, 2, 7, 1, 9, 5, 3, 8, 6]
result = linear_search(numbers, 7)    # int — index of 7 in the list
print(f"Index of 7: {result}")    # Index of 7: 2

result = linear_search(numbers, 10)    # int — -1 when not found
print(f"Index of 10: {result}")    # Index of 10: -1


# ============================================================
# Example 2: Linear Search with Strings
# ============================================================
print("\n=== Linear Search with Strings ===")

# Searches for string in list of names
def find_name(names: list[str], target: str) -> int:
    for i, name in enumerate(names):
        if name.lower() == target.lower():
            return i
    return -1

fruits = ["Apple", "Banana", "Cherry", "Date", "Elderberry"]
pos = find_name(fruits, "cherry")    # int — index (case-insensitive)
print(f"Position of cherry: {pos}")    # Position of cherry: 2

pos = find_name(fruits, "Mango")    # int — -1 if not found
print(f"Position of Mango: {pos}")    # Position of Mango: -1


# ============================================================
# Example 3: Linear Search with Custom Objects
# ============================================================
print("\n=== Linear Search with Objects ===")

class Product:
    def __init__(self, sku: str, name: str, price: float):
        self.sku = sku
        self.name = name
        self.price = price
    
    def __repr__(self):
        return f"Product({self.sku}, {self.name}, ${self.price})"

# Finds product by SKU attribute
def find_product_by_sku(products: list[Product], sku: str) -> Product:
    for product in products:
        if product.sku == sku:
            return product
    return None

inventory = [
    Product("A001", "Laptop", 999.99),
    Product("B002", "Mouse", 29.99),
    Product("C003", "Keyboard", 79.99),
    Product("D004", "Monitor", 299.99)
]

found = find_product_by_sku(inventory, "C003")    # Product — found object
print(f"Found: {found}")    # Found: Product(C003, Keyboard, $79.99)

found = find_product_by_sku(inventory, "X999")    # Product — None if not found
print(f"Found: {found}")    # Found: None


# ============================================================
# Example 4: Linear Search Count Occurrences
# ============================================================
print("\n=== Count Occurrences ===")

# Counts how many times target appears in list
def count_occurrences(items: list[int], target: int) -> int:
    count = 0
    for item in items:
        if item == target:
            count += 1
    return count

data = [1, 3, 5, 3, 7, 3, 9, 3, 11]
occurrences = count_occurrences(data, 3)    # int — number of times 3 appears
print(f"Number of 3s: {occurrences}")    # Number of 3s: 4


# ============================================================
# Example 5: Linear Search with Early Exit
# ============================================================
print("\n=== Find First Match ===")

# Returns first element matching predicate function
def find_firstMatching(items: list[int], predicate) -> int:
    for i, item in enumerate(items):
        if predicate(item):
            return i
    return -1

numbers = [1, 4, 6, 7, 9, 12, 15]

first_even = find_firstMatching(numbers, lambda x: x % 2 == 0)    # int — first even number
print(f"First even index: {first_even}")    # First even index: 1

first_large = find_firstMatching(numbers, lambda x: x > 10)    # int — first > 10
print(f"First > 10 index: {first_large}")    # First > 10 index: 5

first_negative = find_firstMatching(numbers, lambda x: x < 0)    # int — -1 if none
print(f"First negative index: {first_negative}")    # First negative index: -1


# ============================================================
# Example 6: Linear Search Performance
# ============================================================
print("\n=== Search Performance ===")

import time

# Measures time to search in lists of different sizes
def benchmark_linear_search():
    sizes = [1000, 10000, 100000]
    target = -1
    
    for size in sizes:
        data = list(range(size))
        
        start = time.time()
        result = linear_search(data, target)
        elapsed = time.time() - start
        
        print(f"Size: {size:6}, Time: {elapsed:.6f}s, Found: {result}")

benchmark_linear_search()


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
LINEAR SEARCH:
- Time Complexity: O(n)
- Space Complexity: O(1)
- Best for: Unsorted data, small lists
- Worst case: Element at end or not present

KEY POINTS:
- Check each element sequentially
- Return index when found, -1 when not found
- Can search any type with equality comparison
- Predicate-based search for flexible conditions
""")
