# Example115.py
# Topic: Iteration Tools — Basic any()

# Use any() to check if any element is truthy

# === Basic any() with list ===
numbers = [0, 1, 2, 3]
result = any(numbers)
print("any([0, 1, 2, 3]): " + str(result))

# === any() returns True if ANY element is truthy ===
print("any([0, 0, 0]): " + str(any([0, 0, 0])))  # False
print("any([0, 1, 0]): " + str(any([0, 1, 0])))  # True
print("any([1, 1, 1]): " + str(any([1, 1, 1])))  # True

# === any() with strings ===
print("any(['a', 'b', 'c']): " + str(any(['a', 'b', 'c'])))  # True (non-empty strings)
print("any(['', 'a', '']): " + str(any(['', 'a', ''])))  # True

# === any() with empty list ===
print("any([]): " + str(any([])))  # False

# === any() with generator expression ===
numbers = [1, 2, 3, 4, 5]
result = any(x > 10 for x in numbers)
print("any(x > 10): " + str(result))  # False

result = any(x > 3 for x in numbers)
print("any(x > 3): " + str(result))  # True

# === Practical: Check if any number is positive ===
numbers = [-1, -2, -3, 4, -5]
print("Any positive: " + str(any(n > 0 for n in numbers)))

# === Practical: Check if any string is empty ===
words = ["apple", "", "banana", "cherry"]
print("Any empty: " + str(any(w == "" for w in words)))

# === Practical: Check if any file exists ===
# (Simulated with list of filenames)
files = ["report.pdf", "missing.txt", "data.csv"]
# This would be: any(os.path.exists(f) for f in files)
print("Any file exists (simulated): " + str(any(f == "report.pdf" for f in files)))

# === any() short-circuits ===
# Stops at first True
# This would be slow with many elements but finds True quickly
values = [False, False, True, False]
print("Short-circuit: " + str(any(values)))

# === any() with conditions ===
ages = [12, 15, 18, 21]

# Check if any is adult (18+)
print("Any adult: " + str(any(age >= 18 for age in ages)))

# Check if any is teenager (13-19)
print("Any teenager: " + str(any(13 <= age <= 19 for age in ages)))

# === any() returns boolean ===
# Always returns True or False
result = any([0, "", None])  # All falsy
print("All falsy: " + str(result))  # False

result = any([0, "", None, 1])  # Has truthy
print("One truthy: " + str(result))  # True

# === any() with dict (checks keys) ===
d = {"a": 0, "b": 1, "c": 0}
print("Any key truthy: " + str(any(d)))  # True (keys: a, b, c)
print("Any value truthy: " + str(any(d.values())))  # True

# === any() - what counts as truthy ===
# Falsy: False, None, 0, "", [], {}
# Everything else is truthy
print("any([None]): " + str(any([None])))  # False
print("any([0]): " + str(any([0])))  # False
print("any(['']): " + str(any([''])))  # False
print("any([[]]): " + str(any([[]])))  # False
print("any([{}]): " + str(any([{}])))  # False
