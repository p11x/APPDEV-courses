# Example116.py
# Topic: Iteration Tools — Basic all()

# Use all() to check if ALL elements are truthy

# === Basic all() with list ===
numbers = [1, 2, 3, 4]
result = all(numbers)
print("all([1, 2, 3, 4]): " + str(result))

# === all() returns True if ALL elements are truthy ===
print("all([1, 1, 1]): " + str(all([1, 1, 1])))  # True
print("all([1, 0, 1]): " + str(all([1, 0, 1])))  # False
print("all([0, 0, 0]): " + str(all([0, 0, 0])))  # False

# === all() with strings ===
print("all(['a', 'b', 'c']): " + str(all(['a', 'b', 'c'])))  # True
print("all(['a', '', 'c']): " + str(all(['a', '', 'c'])))  # False (empty string)

# === all() with empty list ===
print("all([]): " + str(all([])))  # True (vacuous truth!)

# === all() with generator expression ===
numbers = [1, 2, 3, 4, 5]
result = all(x > 0 for x in numbers)
print("all(x > 0): " + str(result))  # True

result = all(x > 2 for x in numbers)
print("all(x > 2): " + str(result))  # False

# === Practical: Check if all numbers are positive ===
numbers = [1, 2, -3, 4, 5]
print("All positive: " + str(all(n > 0 for n in numbers)))  # False

numbers = [1, 2, 3, 4, 5]
print("All positive: " + str(all(n > 0 for n in numbers)))  # True

# === Practical: Check if all strings are non-empty ===
words = ["apple", "banana", "cherry"]
print("All non-empty: " + str(all(len(w) > 0 for w in words)))  # True

words = ["apple", "", "cherry"]
print("All non-empty: " + str(all(len(w) > 0 for w in words)))  # False

# === Practical: Check if all users are valid ===
users = [
    {"name": "Alice", "age": 25},
    {"name": "Bob", "age": 30},
    {"name": "Carol", "age": 35}
]
print("All have name: " + str(all("name" in u for u in users)))
print("All adults: " + str(all(u["age"] >= 18 for u in users)))

# === all() short-circuits ===
# Stops at first False
values = [True, True, False, True]
print("Short-circuit: " + str(all(values)))  # False

# === all() with conditions ===
ages = [18, 21, 25, 30]

# Check if all are adults (18+)
print("All adults: " + str(all(age >= 18 for age in ages)))

# Check if all are under 100
print("All under 100: " + str(all(age < 100 for age in ages)))

# === all() returns boolean ===
# Always returns True or False
result = all([1, 2, 3])  # All truthy
print("All truthy: " + str(result))  # True

result = all([1, 0, 3])  # Has falsy
print("Has falsy: " + str(result))  # False

# === all() with dict (checks keys) ===
d = {"a": 1, "b": 2, "c": 3}
print("All keys truthy: " + str(all(d)))  # True (all keys are strings)

d = {"a": 1, "b": 0, "c": 3}
print("All values truthy: " + str(all(d.values())))  # False

# === all() - what counts as truthy ===
# Falsy: False, None, 0, "", [], {}
# Everything else is truthy
print("all([1]): " + str(all([1])))  # True
print("all([0]): " + str(all([0])))  # False
print("all(['a']): " + str(all(['a'])))  # True
print("all(['']): " + str(all([''])))  # False
print("all([[1]]): " + str(all([[1]])))  # True
print("all([[]]): " + str(all([[]])))  # False

# === Important: all([]) returns True ===
# This is called "vacuous truth"
# It's mathematically correct: "all items in empty set satisfy condition"
print("Empty is True: " + str(all([])))
