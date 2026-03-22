# Example62.py
# Topic: reduce() Function - Aggregation

# This file demonstrates functools.reduce() for aggregating iterables.


# ============================================================
# Example 1: Basic reduce() - Sum
# ============================================================
print("=== Basic reduce() - Sum ===")

from functools import reduce

numbers = [1, 2, 3, 4, 5]

total = reduce(lambda a, b: a + b, numbers)
print(f"Numbers: {numbers}")
print(f"Sum: {total}")

# Alternative with initial value
total_with_initial = reduce(lambda a, b: a + b, numbers, 10)
print(f"Sum with initial 10: {total_with_initial}")


# ============================================================
# Example 2: reduce() - Product
# ============================================================
print("\n=== reduce() - Product ===")

numbers = [1, 2, 3, 4, 5]

product = reduce(lambda a, b: a * b, numbers)
print(f"Numbers: {numbers}")
print(f"Product: {product}")

# Factorial example
fact_5 = reduce(lambda a, b: a * b, range(1, 6))
print(f"5! = {fact_5}")


# ============================================================
# Example 3: reduce() - Max/Min
# ============================================================
print("\n=== reduce() - Max/Min ===")

numbers = [3, 1, 4, 1, 5, 9, 2, 6]

max_val = reduce(lambda a, b: a if a > b else b, numbers)
print(f"Max: {max_val}")

min_val = reduce(lambda a, b: a if a < b else b, numbers)
print(f"Min: {min_val}")


# ============================================================
# Example 4: reduce() - String concatenation
# ============================================================
print("\n=== reduce() - String concatenation ===")

words = ['Hello', ' ', 'World', '!']

joined = reduce(lambda a, b: a + b, words)
print(f"Joined: {joined}")

# With initial value
prefixed = reduce(lambda a, b: a + b, words, "Result:")
print(f"Prefixed: {prefixed}")


# ============================================================
# Example 5: reduce() with list operations
# ============================================================
print("\n=== reduce() with lists ===")

# Flatten a list of lists
nested = [[1, 2], [3, 4], [5, 6]]
flattened = reduce(lambda a, b: a + b, nested)
print(f"Nested: {nested}")
print(f"Flattened: {flattened}")

# Reverse a list
data = [1, 2, 3, 4, 5]
reversed_list = reduce(lambda a, b: [b] + a, data, [])
print(f"Reversed: {reversed_list}")


# ============================================================
# Example 6: reduce() - Finding longest word
# ============================================================
print("\n=== reduce() - Finding longest word ===")

words = ['cat', 'elephant', 'dog', 'giraffe', 'bear']

longest = reduce(lambda a, b: a if len(a) > len(b) else b, words)
print(f"Longest word: {longest}")


# ============================================================
# Example 7: reduce() - Dictionary operations
# ============================================================
print("\n=== reduce() with dictionaries ===")

# Merge dictionaries
dicts = [{'a': 1}, {'b': 2}, {'c': 3}]
merged = reduce(lambda a, b: {**a, **b}, dicts)
print(f"Merged dict: {merged}")

# Count occurrences
items = ['a', 'b', 'a', 'c', 'b', 'a']
counts = reduce(lambda acc, x: {**acc, x: acc.get(x, 0) + 1}, items, {})
print(f"Counts: {counts}")


# ============================================================
# Example 8: reduce() - Complex aggregation
# ============================================================
print("\n=== reduce() - Complex aggregation ===")

# Calculate average
numbers = [10, 20, 30, 40, 50]
count = len(numbers)
total = reduce(lambda a, b: a + b, numbers)
average = total / count
print(f"Average: {average}")

# Running sum (showing each step)
def running_sum():
    result = []
    def inner(a, b):
        result.append(a + b)
        return a + b
    reduce(inner, [1, 2, 3, 4, 5], 0)
    return result

print(f"Running sum: {running_sum()}")


# ============================================================
# Example 9: reduce() - Custom functions
# ============================================================
print("\n=== reduce() with custom functions ===")

def merge_sets(acc, item):
    return acc | item

sets = [{1, 2}, {2, 3}, {3, 4}]
union = reduce(merge_sets, sets, set())
print(f"Union of sets: {union}")


# ============================================================
# Example 10: reduce() vs loops
# ============================================================
print("\n=== reduce() vs loops ===")

numbers = [1, 2, 3, 4, 5]

# Using reduce
reduce_result = reduce(lambda a, b: a + b, numbers)

# Using loop
loop_sum = 0
for n in numbers:
    loop_sum += n

print(f"reduce() result: {reduce_result}")
print(f"Loop result: {loop_sum}")
print(f"Equal: {reduce_result == loop_sum}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: reduce()")
print("=" * 50)
print("""
- reduce() aggregates all items into a single value
- Requires import from functools
- Takes: function(accumulator, item), iterable, optional initial
- Works left to right through the iterable
- Often replaceable with built-in functions (sum, max, min)
""")
