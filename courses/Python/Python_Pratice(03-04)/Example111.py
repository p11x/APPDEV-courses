# Example111.py
# Topic: Comprehensive Data Structures Review

# Comprehensive review of all built-in data structures.


# ============================================================
# Example 1: Lists - Complete Operations
# ============================================================
print("=== Lists Complete ===")

# Creation
empty = []
with_items = [1, 2, 3]
from_range = list(range(5))
comprehension = [x ** 2 for x in range(5)]

# Indexing
numbers = [0, 1, 2, 3, 4, 5]
print(f"First: {numbers[0]}, Last: {numbers[-1]}")

# Slicing
print(f"Subset: {numbers[1:4]}")
print(f"Step 2: {numbers[::2]}")

# Methods
my_list = [3, 1, 4, 1, 5]
my_list.append(9)
my_list.sort()
print(f"After sort: {my_list}")


# ============================================================
# Example 2: Tuples - Complete
# ============================================================
print("\n=== Tuples Complete ===")

# Creation
point = (10, 20)
single = (1,)  # Note the comma
packed = 1, 2, 3

# Unpacking
x, y, z = packed
print(f"Unpacked: {x}, {y}, {z}")

# Swap
a, b = 1, 2
a, b = b, a
print(f"Swapped: {a}, {b}")


# ============================================================
# Example 3: Sets - Complete
# ============================================================
print("\n=== Sets Complete ===")

# Creation
fruits = {"apple", "banana", "cherry"}
from_list = set([1, 2, 3, 2, 1])

# Operations
a = {1, 2, 3}
b = {2, 3, 4}
print(f"Union: {a | b}")
print(f"Intersection: {a & b}")
print(f"Difference: {a - b}")
print(f"Symmetric: {a ^ b}")


# ============================================================
# Example 4: Dictionaries - Complete
# ============================================================
print("\n=== Dictionaries Complete ===")

# Creation
user = {"name": "Alice", "age": 30}
from_dict = dict(name="Bob", age=25)

# Access
print(f"Name: {user['name']}")
print(f"Get: {user.get('city', 'Unknown')}")

# Operations
user["city"] = "NYC"
user.update({"country": "USA"})
print(f"After update: {user}")


# ============================================================
# Example 5: When to Use Each
# ============================================================
print("\n=== When to Use What ===")

# List: ordered, mutable, duplicates
items = [1, 2, 3, 2, 1]

# Tuple: ordered, immutable
coordinates = (10, 20, 30)

# Set: unique, unordered
unique_items = {1, 2, 3}

# Dict: key-value pairs
mapping = {"a": 1, "b": 2}


print("Lists: sequences with duplicates")
print("Tuples: fixed data")
print("Sets: unique collections")
print("Dicts: key-value mappings")
