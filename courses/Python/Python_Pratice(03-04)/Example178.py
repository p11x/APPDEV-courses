# Example178.py
# Topic: sorted() vs .sort()

# This file demonstrates the difference between sorted() and .sort().
# sorted() returns a new sorted list, .sort() sorts in place.
# Understanding when to use each is essential for Python code.


# ============================================================
# Example 1: sorted() - Returns New List
# ============================================================
print("=== sorted() - Returns New List ===")

numbers = [3, 1, 4, 1, 5, 9, 2, 6]

# sorted() returns a new sorted list, original unchanged
sorted_nums = sorted(numbers)    # list[int] — new sorted list
print(f"Sorted: {sorted_nums}")    # Sorted: [1, 1, 2, 3, 4, 5, 6, 9]
print(f"Original: {numbers}")    # Original: [3, 1, 4, 1, 5, 9, 2, 6]


# ============================================================
# Example 2: .sort() - Sorts In Place
# ============================================================
print("\n=== .sort() - Sorts In Place ===")

numbers = [3, 1, 4, 1, 5, 9, 2, 6]

# .sort() modifies the list in place, returns None
result = numbers.sort()    # None — modifies list, returns None
print(f"Sorted: {numbers}")    # Sorted: [1, 1, 2, 3, 4, 5, 6, 9]
print(f"Return value: {result}")    # Return value: None


# ============================================================
# Example 3: Use Cases for sorted()
# ============================================================
print("\n=== When to Use sorted() ===")

# Creating a new sorted copy for display
data = [5, 2, 8, 1, 9]
display_sorted = sorted(data)    # list — new list for display
print(f"Original: {data}")    # Original: [5, 2, 8, 1, 9]
print(f"Display: {display_sorted}")    # Display: [1, 2, 5, 8, 9]

# Sorting iterables that aren't lists
tuple_data = (3, 1, 4, 1, 5)
sorted_tuple = sorted(tuple_data)    # list — from tuple
print(f"Sorted tuple: {sorted_tuple}")    # Sorted tuple: [1, 1, 3, 4, 5]

# Chaining with other operations
result = sorted(set([3, 1, 4, 1, 5, 9]))    # list — unique sorted
print(f"Unique sorted: {result}")    # Unique sorted: [1, 3, 4, 5, 9]


# ============================================================
# Example 4: Use Cases for .sort()
# ============================================================
print("\n=== When to Use .sort() ===")

# When you need to modify in place for memory efficiency
large_data = [9, 3, 7, 1, 5, 8, 2, 6, 4]
large_data.sort()    # list — sorted in place
print(f"Sorted in place: {large_data}")    # Sorted in place: [1, 2, 3, 4, 5, 6, 7, 8, 9]

# When working with class instances that persist
class Score:
    def __init__(self, name: str, score: int):
        self.name = name
        self.score = score
    
    def __repr__(self):
        return f"{self.name}:{self.score}"

scores = [Score("Bob", 85), Score("Alice", 92), Score("Carol", 78)]
scores.sort(key=lambda s: s.score)    # list — sorted in place
print(f"Sorted scores: {scores}")    # Sorted scores: [Carol:78, Bob:85, Alice:92]


# ============================================================
# Example 5: reverse Parameter
# ============================================================
print("\n=== reverse Parameter ===")

numbers = [3, 1, 4, 1, 5, 9, 2, 6]

# Sort in descending order
desc = sorted(numbers, reverse=True)    # list — descending order
print(f"Descending: {desc}")    # Descending: [9, 6, 5, 4, 3, 2, 1, 1]

numbers_desc = numbers.copy()
numbers_desc.sort(reverse=True)    # list — descending in place
print(f"In place desc: {numbers_desc}")    # In place desc: [9, 6, 5, 4, 3, 2, 1, 1]


# ============================================================
# Example 6: Stability of Sorting
# ============================================================
print("\n=== Sorting Stability ===")

# Python's sort is stable - preserves order of equal elements
data = [
    ("Alice", 85),
    ("Bob", 92),
    ("Carol", 85),
    ("David", 92),
    ("Eve", 78)
]

# Sort by score twice - maintains original order within same score
by_score = sorted(data, key=lambda x: x[1])    # list — stable sort
print(f"By score: {by_score}")

# Then sort by name - preserves score order within same name
by_name = sorted(by_score, key=lambda x: x[0])    # list — stable sort
print(f"By name: {by_name}")


# ============================================================
# Example 7: Sorting Different Types
# ============================================================
print("\n=== Sorting Different Types ===")

# Sort strings (alphabetical)
words = ["banana", "Apple", "cherry", "date"]
sorted_words = sorted(words)    # list — case-sensitive sort
print(f"Strings: {sorted_words}")    # Strings: ['Apple', 'banana', 'cherry', 'date']

# Sort with case-insensitive
sorted_ci = sorted(words, key=str.lower)    # list — case-insensitive
print(f"Case-insensitive: {sorted_ci}")    # Case-insensitive: ['Apple', 'banana', 'cherry', 'date']

# Sort mixed numbers and strings - must be same type
mixed = ["a", "c", "b"]
sorted_mixed = sorted(mixed)    # list — same type
print(f"Mixed: {sorted_mixed}")    # Mixed: ['a', 'b', 'c']


# ============================================================
# Example 8: Performance Consideration
# ============================================================
print("\n=== Memory Considerations ===")

import sys

# sorted() uses O(n) extra memory for new list
original = [5, 2, 8, 1, 9, 3]
sorted_copy = sorted(original)    # list — new allocation

print(f"Original size: {sys.getsizeof(original)} bytes")    # Original size: bytes
print(f"Sorted copy size: {sys.getsizeof(sorted_copy)} bytes")    # Sorted copy size: bytes


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
sorted() vs .sort():
- sorted(): Returns new list, original unchanged
- .sort(): Sorts in place, returns None
- Both use Timsort: O(n log n) average/worst case

WHEN TO USE:
- Use sorted() for: displaying, chaining, tuples/sets
- Use .sort() for: in-place modification, memory efficiency

KEY FEATURES:
- reverse=True for descending order
- key= for custom sorting
- Stable sort preserves equal element order
""")
