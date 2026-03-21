# Example107.py
# Topic: Iteration Tools — Common Mistakes with Enumerate

# Common mistakes when using enumerate()

# === MISTAKE 1: Not unpacking the tuple ===

# WRONG - treating enumerate result as single value
items = ["a", "b", "c"]

# for item in enumerate(items):
#     print(item)  # Prints tuples, not values!

# CORRECT - unpack into two variables
for index, value in enumerate(items):
    print(str(index) + ": " + value)

# === MISTAKE 2: Using wrong variable names ===

# This works but can be confusing
# Better to use descriptive names
data = ["x", "y", "z"]

for position, item in enumerate(data):
    print(str(position) + " -> " + item)

# === MISTAKE 3: Confusing start parameter ===

# WRONG - start is a keyword argument
# for i, v in enumerate(items, 1):  # This actually works!
# But confusing when reading

# CORRECT - use explicit keyword
for i, v in enumerate(items, start=1):
    print(str(i) + ": " + v)

# === MISTAKE 4: Forgetting enumerate gives tuples ===

# WRONG - trying to use index directly
pairs = [("a", 1), ("b", 2)]

# for i, (letter, num) in enumerate(pairs):  # This is correct!
# But forgetting to unpack nested tuple

# CORRECT - unpack both enumerate and inner tuple
for i, (letter, num) in enumerate(pairs):
    print(str(i) + ": " + letter + "=" + str(num))

# === MISTAKE 5: Modifying list while enumerating ===

# WRONG - can cause unexpected behavior
numbers = [1, 2, 3]

# for i, n in enumerate(numbers):
#     if n == 2:
#         numbers.remove(n)  # Modifying during iteration!

# CORRECT - create new list
numbers = [1, 2, 3]
filtered = [n for n in numbers if n != 2]
print("Filtered: " + str(filtered))

# === MISTAKE 6: Using enumerate on non-iterable ===

# WRONG - enumerate needs an iterable
# for i, v in enumerate(123):  # Error!
#     print(i, v)

# CORRECT - wrap in list or use on collections
single_value = "hello"
for i, char in enumerate(single_value):
    print(str(i) + ": " + char)

# === MISTAKE 7: Index confusion with start ===

# WRONG - forgetting start affects displayed index
items = ["a", "b", "c"]

# If you need actual array indices, don't use start
for i, item in enumerate(items, 1):
    actual_index = i - 1
    print("Display: " + str(i) + ", Actual: " + str(actual_index))

# === MISTAKE 8: Trying to use enumerate in list index ===

# WRONG - enumerate is an iterator, not index access
items = ["a", "b", "c"]

# This doesn't work:
# print(items[enumerate(items)])

# CORRECT - convert to list first
indexed = list(enumerate(items))
print("Indexed: " + str(indexed))

# Or use range(len())
for i in range(len(items)):
    print(str(i) + ": " + items[i])

# === MISTAKE 9: Forgetting enumerate returns iterator ===

# WRONG - enumerate is consumed after iteration
en = enumerate(["a", "b", "c"])
for i, v in en:
    print(str(i) + ": " + v)

# Trying to iterate again won't work
# for i, v in en:  # Empty!
#     print(i, v)

# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Always unpack: for i, v in enumerate(items)
# 2. Use start= for human-readable numbering
# 3. Use descriptive variable names
# 4. Don't modify list while iterating
# 5. Remember enumerate returns tuples
# 6. enumerate is an iterator - use once
