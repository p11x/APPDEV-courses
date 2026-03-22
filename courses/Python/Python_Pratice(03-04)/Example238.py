# Example238: functools - reduce
from functools import reduce
import operator

# reduce(function, iterable, initializer) - reduce to single value
print("reduce - accumulate values:")
numbers = [1, 2, 3, 4, 5]

# Sum
result = reduce(lambda a, b: a + b, numbers)
print(f"Sum: {result}")

# Using operator
result = reduce(operator.add, numbers)
print(f"Sum (operator.add): {result}")

# Product
result = reduce(operator.mul, numbers)
print(f"Product: {result}")

# Max
result = reduce(lambda a, b: a if a > b else b, numbers)
print(f"Max: {result}")

# With initializer
print("\nWith initializer:")
result = reduce(lambda a, b: a + b, [], 100)
print(f"Sum of empty with init=100: {result}")

# Find longest string
print("\nFind longest string:")
words = ["cat", "elephant", "dog", "hippopotamus"]
result = reduce(lambda a, b: a if len(a) > len(b) else b, words)
print(f"Longest: {result}")

# Flatten nested list (one level)
print("\nFlatten nested list:")
nested = [[1, 2], [3, 4], [5, 6]]
result = reduce(lambda a, b: a + b, nested)
print(f"Flattened: {result}")

# Group by first letter
print("\nGroup by first letter:")
words = ["apple", "banana", "apricot", "blueberry", "cherry"]
result = reduce(
    lambda acc, word: {**acc, word[0]: acc.get(word[0], []) + [word]},
    words, {}
)
print(f"Grouped: {result}")

# Running difference
print("\nRunning difference:")
numbers = [100, 50, 30, 20]
result = reduce(lambda a, b: a - b, numbers)
print(f"100 - 50 - 30 - 20 = {result}")
