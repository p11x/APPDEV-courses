# Example96.py
# Topic: Lists - Creating, Indexing, and Slicing

# This file demonstrates creating lists, indexing, and slicing.


# ============================================================
# Example 1: Creating Lists
# ============================================================
print("=== Creating Lists ===")

# Empty list
empty = []
print(f"Empty list: {empty}")

# With items
fruits = ["apple", "banana", "cherry"]
print(f"Fruits: {fruits}")

# Mixed types
mixed = [1, "hello", True, 3.14]
print(f"Mixed: {mixed}")

# List from range
numbers = list(range(5))
print(f"From range: {numbers}")

# List comprehension
squares = [x ** 2 for x in range(5)]
print(f"Squares: {squares}")

# Type-hinted list
names: list[str] = ["Alice", "Bob"]
print(f"Typed list: {names}")


# ============================================================
# Example 2: Indexing
# ============================================================
print("\n=== Indexing ===")

fruits = ["apple", "banana", "cherry", "date"]

# Positive indexing (starts at 0)
print(f"First: {fruits[0]}")
print(f"Second: {fruits[1]}")
print(f"Third: {fruits[2]}")

# Negative indexing (starts at -1)
print(f"Last: {fruits[-1]}")
print(f"Second to last: {fruits[-2]}")
print(f"Third to last: {fruits[-3]}")

# Index out of range
# print(fruits[10])  # IndexError!


# ============================================================
# Example 3: Slicing
# ============================================================
print("\n=== Slicing ===")

numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

# Basic slicing [start:end]
print(f"numbers[2:5]: {numbers[2:5]}")   # [2, 3, 4]
print(f"numbers[:3]: {numbers[:3]}")      # [0, 1, 2]
print(f"numbers[5:]: {numbers[5:]}")      # [5, 6, 7, 8, 9]

# With step
print(f"numbers[::2]: {numbers[::2]}")    # [0, 2, 4, 6, 8] - evens
print(f"numbers[1::2]: {numbers[1::2]}")  # [1, 3, 5, 7, 9] - odds

# Negative slicing
print(f"numbers[-3:]: {numbers[-3:]}")    # [7, 8, 9]
print(f"numbers[:-3]: {numbers[:-3]}")    # [0, 1, 2, 3, 4, 5, 6]

# Reverse
print(f"numbers[::-1]: {numbers[::-1]}")   # [9, 8, 7, 6, 5, 4, 3, 2, 1, 0]


# ============================================================
# Example 4: Modifying Lists
# ============================================================
print("\n=== Modifying Lists ===")

my_list = [1, 2, 3, 4, 5]

# Change element
my_list[0] = 10
print(f"After change: {my_list}")

# Change slice
my_list[1:3] = [20, 30]
print(f"After slice change: {my_list}")

# Delete element
del my_list[0]
print(f"After delete: {my_list}")

# Delete slice
del my_list[1:3]
print(f"After delete slice: {my_list}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Lists Basics")
print("=" * 50)
print("""
CREATING LISTS:
- [] empty list
- [1, 2, 3] with items
- list(range(5)) from range
- [x for x in items] comprehension

INDEXING:
- list[0] - first element
- list[-1] - last element
- list[index] - raises IndexError if out of range

SLICING:
- list[start:end] - elements from start to end-1
- list[start:] - from start to end
- list[:end] - from beginning to end-1
- list[::step] - with step
- list[::-1] - reverse
""")
