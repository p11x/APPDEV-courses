# Example60.py
# Topic: map() Function - Basic Transformations

# This file demonstrates map() function for transforming iterables.


# ============================================================
# Example 1: Basic map() with lambda
# ============================================================
print("=== Basic map() with lambda ===")

numbers = [1, 2, 3, 4, 5]

doubled = list(map(lambda x: x * 2, numbers))
print(f"Original: {numbers}")
print(f"Doubled: {doubled}")

squared = list(map(lambda x: x ** 2, numbers))
print(f"Squared: {squared}")


# ============================================================
# Example 2: map() with built-in functions
# ============================================================
print("\n=== map() with built-in functions ===")

strings = ['hello', 'world', 'python']

uppercase = list(map(str.upper, strings))
print(f"Original: {strings}")
print(f"Uppercase: {uppercase}")

lengths = list(map(len, strings))
print(f"Lengths: {lengths}")

stripped = list(map(str.strip, ['  hello  ', '  world  ']))
print(f"Stripped: {stripped}")


# ============================================================
# Example 3: map() with multiple iterables
# ============================================================
print("\n=== map() with multiple iterables ===")

list1 = [1, 2, 3]
list2 = [10, 20, 30]

added = list(map(lambda x, y: x + y, list1, list2))
print(f"List1 + List2: {added}")

multiplied = list(map(lambda x, y: x * y, list1, list2))
print(f"List1 * List2: {multiplied}")

powers = list(map(pow, [2, 3, 4], [2, 3, 4]))
print(f"Power (2^2, 3^3, 4^4): {powers}")


# ============================================================
# Example 4: map() with named functions
# ============================================================
print("\n=== map() with named functions ===")

def cube(x):
    return x ** 3

def is_positive(x):
    return x > 0

numbers = [-2, -1, 0, 1, 2]

cubed = list(map(cube, numbers))
print(f"Cubed: {cubed}")

to_bool = list(map(is_positive, numbers))
print(f"Is Positive: {to_bool}")


# ============================================================
# Example 5: map() with None as function
# ============================================================
print("\n=== map() with None ===")

list1 = [1, 2, 3]
list2 = [4, 5, 6]

zipped = list(map(None, list1, list2))
print(f"Zipped: {zipped}")


# ============================================================
# Example 6: map() on different iterables
# ============================================================
print("\n=== map() on different iterables ===")

# Tuple
result = list(map(lambda x: x * 2, (1, 2, 3)))
print(f"Tuple result: {result}")

# String (iterates over characters)
result = list(map(lambda c: c.upper(), 'hello'))
print(f"String result: {result}")

# Dictionary (iterates over keys)
result = list(map(lambda k: k.upper(), {'a': 1, 'b': 2}))
print(f"Dict keys result: {result}")

# Range
result = list(map(lambda x: x ** 2, range(5)))
print(f"Range result: {result}")


# ============================================================
# Example 7: map() with type conversion
# ============================================================
print("\n=== map() for type conversion ===")

numbers_str = ['1', '2', '3', '4', '5']
numbers_int = list(map(int, numbers_str))
print(f"String to int: {numbers_int}")

numbers_float = list(map(float, numbers_str))
print(f"String to float: {numbers_float}")

mixed = ['1', '2.5', '3']
converted = list(map(float, mixed))
print(f"Mixed to float: {converted}")


# ============================================================
# Example 8: map() vs list comprehension
# ============================================================
print("\n=== map() vs comprehension ===")

numbers = [1, 2, 3, 4, 5]

# Using map
map_result = list(map(lambda x: x * 2, numbers))

# Using comprehension
comp_result = [x * 2 for x in numbers]

print(f"map() result: {map_result}")
print(f"Comprehension: {comp_result}")
print(f"Equal: {map_result == comp_result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: map()")
print("=" * 50)
print("""
- map() applies a function to each item in an iterable
- Returns an iterator (use list() to materialize)
- Can work with multiple iterables (stop at shortest)
- Often replaceable with list comprehensions
- Use with built-in functions for efficiency
""")
