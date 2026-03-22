# Example195.py
# Topic: Counter Basics & Operations

# This file demonstrates the Counter class from collections module
# for efficient frequency counting.


# ============================================================
# Example 1: Create Counter
# ============================================================
print("=== Create Counter ===")

from collections import Counter

items = ["apple", "banana", "apple", "cherry", "banana", "apple"]
c = Counter(items)
print(f"Counter: {c}")    # Counter({'apple': 3, 'banana': 2, 'cherry': 1})


# ============================================================
# Example 2: From String
# ============================================================
print("\n=== From String ===")

c = Counter("hello world")
print(f"Char counts: {c}")    # Counter({'l': 3, 'o': 2, 'h': 1, ...})


# ============================================================
# Example 3: From List
# ============================================================
print("\n=== From List ===")

numbers = [1, 2, 2, 3, 3, 3, 4, 4, 4, 4]
c = Counter(numbers)
print(f"Number counts: {c}")    # Counter({4: 4, 3: 3, 2: 2, 1: 1})


# ============================================================
# Example 4: Access Counts
# ============================================================
print("\n=== Access Counts ===")

c = Counter("abracadabra")
print(f"a: {c['a']}")    # 5
print(f"b: {c['b']}")    # 2
print(f"z: {c['z']}")    # 0 - returns 0, no error


# ============================================================
# Example 5: most_common
# ============================================================
print("\n=== most_common ===")

c = Counter("abracadabra")
top3 = c.most_common(3)
print(f"Top 3: {top3}")    # [('a', 5), ('b', 2), ('r', 2)]


# ============================================================
# Example 6: Elements
# ============================================================
print("\n=== Elements ===")

c = Counter({"a": 3, "b": 1})
elements = list(c.elements())
print(f"Elements: {elements}")    # ['a', 'a', 'a', 'b']


# ============================================================
# Example 7: Update Counter
# ============================================================
print("\n=== Update ===")

c = Counter(["apple", "banana"])
c.update(["apple", "apple", "cherry"])
print(f"Updated: {c}")    # Counter({'apple': 3, 'banana': 1, 'cherry': 1})


# ============================================================
# Example 8: Arithmetic Operations
# ============================================================
print("\n=== Arithmetic ===")

c1 = Counter({"a": 3, "b": 1})
c2 = Counter({"a": 1, "b": 2, "c": 1})

print(f"Add: {c1 + c2}")    # Counter({'a': 4, 'b': 3, 'c': 1})
print(f"Subtract: {c1 - c2}")    # Counter({'a': 2})
print(f"Intersection: {c1 & c2}")    # Counter({'a': 1, 'b': 1})
print(f"Union: {c1 | c2}")    # Counter({'a': 3, 'b': 2, 'c': 1})


# ============================================================
# Example 9: Counter with Words
# ============================================================
print("\n=== Word Count ===")

text = "the quick brown fox jumps over the lazy dog the dog"
words = text.split()
c = Counter(words)
print(f"Word counts: {c}")    # Counter({'the': 3, 'dog': 2, ...})


# ============================================================
# Example 10: Filter Counter
# ============================================================
print("\n=== Filter Counter ===")

c = Counter({"a": 5, "b": 2, "c": 1})
filtered = +c  # Keep only positive counts
print(f"Positive: {filtered}")    # Counter({'a': 5, 'b': 2})


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
COUNTER:
- Counter(iterable): Count elements
- c[key]: Get count (returns 0)
- c.most_common(n): Top n elements
- c.elements(): Iterate with repeats
- c.update(): Add more counts
- +c: Remove zero/negative counts

ARITHMETIC:
- +: Add counts
- -: Subtract counts
- &: Keep min
- |: Keep max
""")
