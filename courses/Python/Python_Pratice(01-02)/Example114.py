# Example114.py
# Topic: Iteration Tools — Common Mistakes with Sorted

# Common mistakes when using sorted()

# === MISTAKE 1: Confusing sorted() with .sort() ===

# WRONG - sorted() returns a new list, .sort() modifies in place
numbers = [5, 2, 8]

# sorted() returns new list
result = sorted(numbers)
print("sorted() result: " + str(result))
print("Original unchanged: " + str(numbers))

# .sort() modifies original
numbers2 = [5, 2, 8]
numbers2.sort()
print(".sort() modifies: " + str(numbers2))

# === MISTAKE 2: Forgetting sorted returns a list ===

# WRONG - sorted() already returns list, don't wrap in list()
numbers = [3, 1, 2]

# This works but is redundant:
# result = list(sorted(numbers))

# CORRECT - just use sorted() directly
result = sorted(numbers)
print("Result: " + str(result))

# === MISTAKE 3: Wrong key function ===

# WRONG - key should return what to sort BY
words = ["banana", "apple", "cherry"]

# This sorts by first letter only
# sorted(words, key=lambda x: x[0])  # same as default!

# CORRECT - sort by length
print("By length: " + str(sorted(words, key=len)))

# === MISTAKE 4: Case-sensitive sorting ===

# WRONG - default is case-sensitive
names = ["alice", "Bob", "carol"]
print("Case-sensitive: " + str(sorted(names)))

# CORRECT - use str.lower as key
print("Case-insensitive: " + str(sorted(names, key=str.lower)))

# === MISTAKE 5: Modifying during iteration ===

# WRONG - don't modify list while sorting
# numbers = [3, 1, 2]
# for n in sorted(numbers):  # This is fine
#     numbers.append(n)  # But don't do this!

# CORRECT - work with copy
numbers = [3, 1, 2]
for n in sorted(numbers):
    print(str(n) + " ", end="")
print("")

# === MISTAKE 6: Comparing incompatible types ===

# WRONG - can't compare int and str
# mixed = [1, "a", 2, "b"]
# sorted(mixed)  # Error!

# CORRECT - convert to same type or use key
mixed = [1, 2, 3]
print("All same type: " + str(sorted(mixed)))

# === MISTAKE 7: Reverse parameter placement ===

# WRONG - reverse is after key
# sorted(numbers, key=len, True)  # Works but confusing

# CORRECT - use keyword argument
print("Correct: " + str(sorted([3, 1, 2], reverse=True)))

# === MISTAKE 8: Forgetting sorted() works on iterables ===

# WRONG - trying to sort non-iterable
# sorted(123)  # Error!

# CORRECT - works on any iterable
print("From string: " + str(sorted("cba")))

# === MISTAKE 9: Sorted doesn't modify original - when you want it to ===

# WRONG - expecting original to change
original = [5, 2, 8]
sorted_copy = sorted(original)
print("Original: " + str(original))
print("Sorted copy: " + str(sorted_copy))

# If you need original to change, use .sort()
original2 = [5, 2, 8]
original2.sort()
print("After .sort(): " + str(original2))

# === MISTAKE 10: Key returning wrong type ===

# WRONG - key returns string, sorting strings
words = ["banana", "apple", "cherry"]

# Sorting by length returns numbers, which works
print("By length: " + str(sorted(words, key=len)))

# But this might cause confusion
# sorted(words, key=lambda x: str(len(x)))  # returns string "5", "6" etc

# CORRECT - return appropriate type
print("By length (correct): " + str(sorted(words, key=len)))

# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Use sorted() when you need a new list
# 2. Use .sort() when modifying in place
# 3. Use key=str.lower for case-insensitive sort
# 4. Remember sorted() works on any iterable
# 5. Don't mix incompatible types
# 6. Use keyword argument for reverse=True
