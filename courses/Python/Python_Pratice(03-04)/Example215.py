# Example215.py
# Topic: Sorting with Complex Keys

# This file demonstrates sorting with complex key functions.


# ============================================================
# Example 1: Sort by Multiple Criteria
# ============================================================
print("=== Multiple Criteria ===")

data = [("apple", 3), ("banana", 1), ("cherry", 2)]
sorted_data = sorted(data, key=lambda x: (-x[1], x[0]))
print(f"Sorted: {sorted_data}")


# ============================================================
# Example 2: Sort by Length
# ============================================================
print("\n=== By Length ===")

words = ["apple", "hi", "banana", "cat"]
sorted_words = sorted(words, key=len)
print(f"By length: {sorted_words}")


# ============================================================
# Example 3: Sort by Last Character
# ============================================================
print("\n=== By Last Char ===")

words = ["abc", "def", "ghi"]
sorted_words = sorted(words, key=lambda w: w[-1])
print(f"By last: {sorted_words}")


# ============================================================
# Example 4: Sort with abs
# ============================================================
print("\n=== By abs ===")

numbers = [-5, 2, -3, 1, -8, 4]
sorted_nums = sorted(numbers, key=abs)
print(f"By abs: {sorted_nums}")


# ============================================================
# Example 5: Sort with itemgetter
# ============================================================
print("\n=== itemgetter ===")

from operator import itemgetter

data = [(1, 3), (2, 1), (3, 2)]
sorted_data = sorted(data, key=itemgetter(1))
print(f"By 2nd: {sorted_data}")


# ============================================================
# Example 6: Sort with attrgetter
# ============================================================
print("\n=== attrgetter ===")

from operator import attrgetter

class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age

people = [Person("Alice", 30), Person("Bob", 25)]
sorted_people = sorted(people, key=attrgetter("age"))
print(f"By age: {[(p.name, p.age) for p in sorted_people]}")


# ============================================================
# Example 7: Stable Sort
# ============================================================
print("\n=== Stable Sort ===")

data = [("a", 3), ("b", 1), ("c", 3), ("d", 2)]
sorted_data = sorted(data, key=lambda x: x[1])
print(f"Stable: {sorted_data}")


# ============================================================
# Example 8: Sort Reverse
# ============================================================
print("\n=== Reverse ===")

numbers = [3, 1, 4, 1, 5]
sorted_nums = sorted(numbers, reverse=True)
print(f"Reverse: {sorted_nums}")


# ============================================================
# Example 9: Sort with lambda
# ============================================================
print("\n=== Lambda ===")

words = ["Apple", "banana", "CHERRY"]
sorted_words = sorted(words, key=str.lower)
print(f"Case-insensitive: {sorted_words}")


# ============================================================
# Example 10: Partial Sort
# ============================================================
print("\n=== Partial Sort ===")

import heapq

data = [5, 2, 8, 1, 9, 3]
print(f"3 smallest: {heapq.nsmallest(3, data)}")
print(f"3 largest: {heapq.nlargest(3, data)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
SORTING KEYS:
- Multiple: key=lambda x: (-x[1], x[0])
- len: sort by length
- abs: sort by absolute
- itemgetter/attrgetter: efficient
- heapq for partial sort
""")
