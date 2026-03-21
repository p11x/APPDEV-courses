# Example124.py
# Topic: Iteration Tools — Common Mistakes with Itertools

# Common mistakes when using itertools

# === MISTAKE 1: Forgetting to import ===

# WRONG - not imported
# result = chain([1, 2], [3, 4])  # NameError!

# CORRECT - import first
from itertools import chain, islice, cycle, repeat

result = list(chain([1, 2], [3, 4]))
print("Chain: " + str(result))

# === MISTAKE 2: Forgetting they return iterators ===

# WRONG - expecting list behavior
numbers = [1, 2, 3]
result = chain(numbers)
# print(result[0])  # Error! Can't index iterator

# CORRECT - iterate or convert to list
result = chain(numbers)
print("First: " + str(next(result)))

# === MISTAKE 3: Infinite loops with cycle() ===

# WRONG - cycle() is infinite!
# colors = cycle(["red", "green"])
# for c in colors:  # Never stops!
#     print(c)

# CORRECT - always limit
colors = cycle(["red", "green"])
for i, c in enumerate(colors):
    if i >= 4:
        break
    print("Color: " + c)

# === MISTAKE 4: Infinite loops with repeat() ===

# WRONG - repeat() without limit is infinite!
# for x in repeat("hello"):  # Never stops!
#     print(x)

# CORRECT - provide times or use islice
for x in repeat("hi", 3):
    print("Repeat: " + x)

# === MISTAKE 5: Using chain instead of + ===

# WRONG - unnecessary complexity
a = [1, 2]
b = [3, 4]
result = list(chain(a, b))

# Simpler:
result = a + b
print("\nSimpler: " + str(result))

# But chain() is lazy, useful for large data

# === MISTAKE 6: Confusing chain with extend ===

# chain() returns NEW iterator
# extend() modifies IN PLACE
list_a = [1, 2]
list_b = [3, 4]

chain_result = list(chain(list_a, list_b))
print("Chain: " + str(chain_result) + ", original: " + str(list_a))

list_a2 = [1, 2]
list_b2 = [3, 4]
list_a2.extend(list_b2)
print("Extend: " + str(list_a2))

# === MISTAKE 7: Forgetting islice limits ===

# WRONG - islice without limits can consume everything
# numbers = range(1000000)
# # This works but processes all!
# result = list(islice(numbers, None))

# CORRECT - specify what you need
numbers = range(1000000)
result = list(islice(numbers, 10))
print("\nFirst 10: " + str(result))

# === MISTAKE 8: Using islice with negative indices ===

# WRONG - islice doesn't support negative indices!
# list(islice([1,2,3], -1))  # Error!

# CORRECT - use positive or convert to list
numbers = [1, 2, 3]
result = numbers[-1]  # For negative, convert to list
print("Last: " + str(result))

# === MISTAKE 9: Forgetting to limit cycle ===

# WRONG - cycle() forever!
# result = cycle([1, 2])

# CORRECT - always limit
result = list(islice(cycle([1, 2]), 5))
print("\nLimited cycle: " + str(result))

# === MISTAKE 10: Not consuming iterator before reusing ===

# WRONG - iterators are consumed
ch = chain([1, 2], [3, 4])
first = list(ch)
second = list(ch)  # Empty!

# CORRECT - store as list if reusing
ch = chain([1, 2], [3, 4])
stored = list(ch)
first = stored
second = stored  # Still available!
print("\nReuse: " + str(first) + ", " + str(second))

# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Always import from itertools
# 2. Remember they return iterators, not lists
# 3. Always limit cycle() and repeat()
# 4. Use chain() for lazy concatenation
# 5. Use islice() for slicing iterators
# 6. Convert to list if you need to reuse
