# Example65.py
# Topic: Comprehensive Review - map(), filter(), reduce()

# This file provides a comprehensive review with all concepts.


# ============================================================
# Example 1: map() - Complete overview
# ============================================================
print("=== map() - Complete Overview ===")

# Basic transformation
numbers = [1, 2, 3, 4, 5]
doubled = list(map(lambda x: x * 2, numbers))
print(f"Original: {numbers}")
print(f"Doubled: {doubled}")

# Multiple iterables
a = [1, 2, 3]
b = [10, 20, 30]
result = list(map(lambda x, y: x + y, a, b))
print(f"Add lists: {result}")

# Built-in functions
words = ['hello', 'world']
upper = list(map(str.upper, words))
lengths = list(map(len, words))
print(f"Upper: {upper}, Lengths: {lengths}")


# ============================================================
# Example 2: filter() - Complete overview
# ============================================================
print("\n=== filter() - Complete Overview ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

evens = list(filter(lambda x: x % 2 == 0, numbers))
print(f"Original: {numbers}")
print(f"Evens: {evens}")

# Filter with None (truthy values)
values = [0, 1, '', 'text', None, []]
truthy = list(filter(None, values))
print(f"Truthy: {truthy}")

# Complex condition
filtered = list(filter(lambda x: x > 3 and x < 8, numbers))
print(f"3 < x < 8: {filtered}")


# ============================================================
# Example 3: reduce() - Complete overview
# ============================================================
print("\n=== reduce() - Complete Overview ===")

from functools import reduce

numbers = [1, 2, 3, 4, 5]

# Sum
total = reduce(lambda a, b: a + b, numbers)
print(f"Sum: {total}")

# With initial value
total_10 = reduce(lambda a, b: a + b, numbers, 10)
print(f"Sum with initial 10: {total_10}")

# Product
product = reduce(lambda a, b: a * b, numbers)
print(f"Product: {product}")

# Max
max_val = reduce(lambda a, b: a if a > b else b, numbers)
print(f"Max: {max_val}")


# ============================================================
# Example 4: Chaining operations
# ============================================================
print("\n=== Chaining Operations ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Filter -> Map -> Reduce
result = reduce(
    lambda a, b: a + b,
    map(
        lambda x: x ** 2,
        filter(lambda x: x % 2 == 0, numbers)
    )
)
print(f"Original: {numbers}")
print(f"Sum of even squares: {result}")

# Alternative using comprehensions
alt = sum([x ** 2 for x in numbers if x % 2 == 0])
print(f"Using comprehension: {alt}")


# ============================================================
# Example 5: map() vs comprehension
# ============================================================
print("\n=== map() vs Comprehension ===")

numbers = [1, 2, 3, 4, 5]

# map()
mapped = list(map(lambda x: x * 2, numbers))

# Comprehension
comp = [x * 2 for x in numbers]

print(f"map(): {mapped}")
print(f"Comprehension: {comp}")
print(f"Equivalent: {mapped == comp}")


# ============================================================
# Example 6: filter() vs comprehension
# ============================================================
print("\n=== filter() vs Comprehension ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# filter()
filtered = list(filter(lambda x: x > 5, numbers))

# Comprehension
comp = [x for x in numbers if x > 5]

print(f"filter(): {filtered}")
print(f"Comprehension: {comp}")
print(f"Equivalent: {filtered == comp}")


# ============================================================
# Example 7: Working with different types
# ============================================================
print("\n=== Working with Different Types ===")

# Strings
text = "hello"
char_codes = list(map(ord, text))
print(f"Character codes: {char_codes}")

# Tuples
coords = [(1, 2), (3, 4), (5, 6)]
sums = list(map(lambda x, y: x + y, *zip(*coords)))
print(f"Pair sums: {sums}")

# Dictionaries
data = [{'a': 1}, {'b': 2}, {'c': 3}]
keys = list(map(lambda d: list(d.keys())[0], data))
print(f"Keys: {keys}")


# ============================================================
# Example 8: Lazy evaluation
# ============================================================
print("\n=== Lazy Evaluation ===")

numbers = [1, 2, 3, 4, 5]

# map returns an iterator
mapped_iter = map(lambda x: x * 2, numbers)
print(f"map() returns: {type(mapped_iter)}")

# Convert when needed
result = list(mapped_iter)
print(f"Converted: {result}")

# Benefits: can process large datasets
large = range(1000000)
squared = map(lambda x: x ** 2, large)
# Only processes when accessed
print(f"First 5 squared: {list(map(lambda x: x ** 2, range(5)))}")


# ============================================================
# Example 9: Common patterns
# ============================================================
print("\n=== Common Patterns ===")

data = [1, 2, 3, 4, 5]

# Transform and collect
transformed = list(map(lambda x: {'value': x, 'doubled': x * 2}, data))
print(f"Transform to dicts: {transformed}")

# Filter and collect
filtered = list(map(lambda x: x * 3, filter(lambda x: x % 2 == 0, data)))
print(f"Triple evens: {filtered}")

# All/Any patterns
all_positive = all(map(lambda x: x > 0, data))
any_even = any(map(lambda x: x % 2 == 0, data))
print(f"All positive: {all_positive}, Any even: {any_even}")


# ============================================================
# Example 10: Performance considerations
# ============================================================
print("\n=== Performance Considerations ===")

import time

# Generate test data
large_data = list(range(10000))

# Time map()
start = time.perf_counter()
result_map = list(map(lambda x: x * 2, large_data))
time_map = time.perf_counter() - start
print(f"map() time: {time_map:.4f}s")

# Time comprehension
start = time.perf_counter()
result_comp = [x * 2 for x in large_data]
time_comp = time.perf_counter() - start
print(f"Comprehension time: {time_comp:.4f}s")

print(f"Results equal: {result_map == result_comp}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY")
print("=" * 50)
print("""
map() - Transform each item:
  list(map(func, iterable))
  - Returns iterator
  - Works with multiple iterables
  - Use with built-in functions for speed

filter() - Select items by condition:
  list(filter(func, iterable))
  - Keeps items where func(item) is truthy
  - filter(None, iterable) keeps truthy values

reduce() - Aggregate to single value:
  reduce(func, iterable, initial)
  - Requires import from functools
  - func takes (accumulator, item)

Chaining:
  - map -> filter -> reduce
  - Order matters for performance

Comprehensions:
  - Often more readable than map/filter
  - Can be faster for simple operations

Lazy evaluation:
  - map/filter return iterators
  - Save memory for large datasets
""")
