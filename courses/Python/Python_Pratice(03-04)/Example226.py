# Example226.py
# Topic: More Sorting Patterns

# This file demonstrates more advanced sorting patterns.


# ============================================================
# Example 1: Sort by Custom Function
# ============================================================
print("=== Custom Sort ===")

words = ["apple", "banana", "cherry"]
sorted_words = sorted(words, key=lambda w: -len(w))
print(f"By length: {sorted_words}")


# ============================================================
# Example 2: Multiple Keys
# ============================================================
print("\n=== Multiple Keys ===")

data = [(1, "a"), (2, "b"), (1, "c")]
sorted_data = sorted(data, key=lambda x: (-x[0], x[1]))
print(f"Sorted: {sorted_data}")


# ============================================================
# Example 3: Case Insensitive
# ============================================================
print("\n=== Case Insensitive ===")

words = ["Apple", "banana", "CHERRY"]
sorted_words = sorted(words, key=str.lower)
print(f"Sorted: {sorted_words}")


# ============================================================
# Example 4: Stable Sort
# ============================================================
print("\n=== Stable Sort ===")

data = [("a", 2), ("b", 1), ("c", 2)]
sorted_data = sorted(data, key=lambda x: x[1])
print(f"Stable: {sorted_data}")


# ============================================================
# Example 5: Reverse Sort
# ============================================================
print("\n=== Reverse ===")

numbers = [3, 1, 4, 1, 5]
sorted_nums = sorted(numbers, reverse=True)
print(f"Reverse: {sorted_nums}")


# ============================================================
# Example 6: Partial Sort
# ============================================================
print("\n=== Partial Sort ===")

import heapq

data = [5, 2, 8, 1, 9]
smallest = heapq.nsmallest(3, data)
largest = heapq.nlargest(2, data)
print(f"3 smallest: {smallest}")
print(f"2 largest: {largest}")


# ============================================================
# Example 7: Sorted Containers
# ============================================================
print("\n=== Sorted Containers ===")

from bisect import bisect_left

class SortedList:
    def __init__(self):
        self.data = []
    
    def add(self, item):
        pos = bisect_left(self.data, item)
        self.data.insert(pos, item)
    
    def __contains__(self, item):
        pos = bisect_left(self.data, item)
        return pos < len(self.data) and self.data[pos] == item

sl = SortedList()
for x in [3, 1, 4, 1, 5]:
    sl.add(x)
print(f"Contains 4: {4 in sl}")
print(f"Contains 6: {6 in sl}")


# ============================================================
# Example 8: Natural Sort
# ============================================================
print("\n=== Natural Sort ===")

import re

def natural_key(s):
    return [int(c) if c.isdigit() else c.lower() for c in re.split(r'(\d+)', s)]

files = ["file1.txt", "file10.txt", "file2.txt"]
sorted_files = sorted(files, key=natural_key)
print(f"Natural: {sorted_files}")


# ============================================================
# Example 9: Sort with None
# ============================================================
print("\n=== Sort with None ===")

data = [("a", 1), ("b", None), ("c", 2)]
sorted_data = sorted(data, key=lambda x: (x[1] is None, x[1] or 0))
print(f"Sorted: {sorted_data}")


# ============================================================
# Example 10: Attrgetter
# ============================================================
print("\n=== Attrgetter ===")

from operator import attrgetter

class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age

people = [Person("Alice", 30), Person("Bob", 25)]
sorted_people = sorted(people, key=attrgetter("age"))
print(f"By age: {[(p.name, p.age) for p in sorted_people]}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
MORE SORTING:
- Multiple keys with tuples
- Case insensitive
- Partial with heapq
- Natural sort for files
- Handle None values
""")
