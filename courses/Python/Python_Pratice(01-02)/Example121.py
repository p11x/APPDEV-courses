# Example121.py
# Topic: Iteration Tools — Common Mistakes with Map and Filter

# Common mistakes when using map() and filter()

# === MISTAKE 1: Forgetting to convert to list ===

# WRONG - map() returns iterator, not list
numbers = [1, 2, 3]
result = map(lambda x: x * 2, numbers)
print("Without list(): " + str(result))  # Shows object!

# CORRECT - wrap in list()
result = list(map(lambda x: x * 2, numbers))
print("With list(): " + str(result))

# === MISTAKE 2: Not using list comprehension when simpler ===

# WRONG - map with lambda when comprehension is cleaner
numbers = [1, 2, 3, 4, 5]
result = list(map(lambda x: x * 2, numbers))

# This is more Pythonic:
result = [x * 2 for x in numbers]
print("Better: " + str(result))

# === MISTAKE 3: Confusing map and filter ===

# WRONG - using filter when you need map
numbers = [1, 2, 3, 4, 5]
# This doubles only the even numbers
result = list(filter(lambda x: x % 2 == 0, numbers))  # Returns [2, 4]

# CORRECT - use map to transform
result = list(map(lambda x: x * 2, numbers))  # Returns [2, 4, 6, 8, 10]
print("Map: " + str(result))

# === MISTAKE 4: Wrong function in filter ===

# WRONG - filter expects True/False, not transformed values
numbers = [1, 2, 3, 4, 5]

# This is wrong! filter keeps truthy results of lambda
# But lambda returns number, which is truthy for non-zero
result = list(filter(lambda x: x * 2, numbers))  # Same as filter(None)!
print("Wrong filter: " + str(result))

# CORRECT - return boolean
result = list(filter(lambda x: x * 2 > 5, numbers))
print("Correct filter: " + str(result))

# === MISTAKE 5: Using map for filtering ===

# WRONG - trying to filter with map
numbers = [1, 2, 3, 4, 5]
# This doubles everything, doesn't remove anything!
result = list(map(lambda x: x if x > 2 else None, numbers))
print("Map filter: " + str(result))

# CORRECT - use filter
result = list(filter(lambda x: x > 2, numbers))
print("Filter: " + str(result))

# === MISTAKE 6: Forgetting that map/filter return iterators ===

# WRONG - trying to iterate twice
result = map(lambda x: x * 2, [1, 2, 3])

first = list(result)
second = list(result)  # Empty!
print("First: " + str(first))
print("Second: " + str(second))

# CORRECT - convert to list first or use itertools.tee
result = list(map(lambda x: x * 2, [1, 2, 3]))
first = result
second = result  # Still available!
print("First copy: " + str(first))
print("Second copy: " + str(second))

# === MISTAKE 7: Wrong number of iterables in map ===

# WRONG - different lengths
a = [1, 2]
b = [10, 20, 30]

# map stops at shortest, but this might not be what you want
result = list(map(lambda x, y: x + y, a, b))
print("Different lengths: " + str(result))

# CORRECT - use zip_longest if needed
from itertools import zip_longest
result = list(map(lambda x, y: (x or 0) + (y or 0), a, b))
print("Handled: " + str(result))

# === MISTAKE 8: Complex lambda vs list comprehension ===

# WRONG - complex map/filter chain
result = list(map(lambda x: x * 2, filter(lambda x: x > 2, [1, 2, 3, 4, 5])))

# More readable as comprehension
result = [x * 2 for x in [1, 2, 3, 4, 5] if x > 2]
print("Readable: " + str(result))

# === MISTAKE 9: Using map for side effects only ===

# WRONG - map is for transformation, not side effects
# This works but is not Pythonic:
# list(map(lambda x: print(x), [1, 2, 3]))

# CORRECT - use for loop for side effects
for x in [1, 2, 3]:
    print("Side effect: " + str(x))

# === Note: map(None, ...) doesn't work in Python 3 ===
# Use lambda instead for identity
result = list(map(lambda x: x, [1, 2, 3]))
print("Identity map: " + str(result))

# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Always convert map/filter to list if needed
# 2. Use list comprehension for simple transformations
# 3. Use map() to transform, filter() to select
# 4. Remember map/filter return iterators (lazy)
# 5. Use zip_longest for different length iterables
# 6. Prefer comprehension over map/filter for readability
# 7. Use for loops for side effects
