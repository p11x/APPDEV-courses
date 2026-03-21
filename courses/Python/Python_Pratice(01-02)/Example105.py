# Example105.py
# Topic: Iteration Tools — Enumerate with Custom Start

# Use the start parameter to begin counting from a custom number

# === Starting from 1 (human-readable) ===
days = ["Mon", "Tue", "Wed", "Thu", "Fri"]

for i, day in enumerate(days, 1):
    print("Day " + str(i) + ": " + day)

# === Starting from 0 (default) ===
# This is the same as not specifying start
names = ["Alice", "Bob", "Carol"]

for i, name in enumerate(names, 0):
    print("Index " + str(i) + ": " + name)

# === Starting from 100 (useful for IDs) ===
products = ["Laptop", "Phone", "Tablet"]

for i, product in enumerate(products, 100):
    print("Product ID " + str(i) + ": " + product)

# === Starting from -1 (reverse counting) ===
# Useful for zero-based APIs
data = ["first", "second", "third"]

for i, item in enumerate(data, -1):
    print("API index " + str(i) + ": " + item)

# === Practical: 1-based numbering for display ===
tasks = ["Wake up", "Exercise", "Breakfast", "Work", "Sleep"]

print("My daily routine:")
for i, task in enumerate(tasks, 1):
    print(str(i) + ") " + task)

# === Practical: Database-style IDs ===
users = ["john", "jane", "bob"]

print("User database:")
for i, user in enumerate(users, 1001):
    print("ID: " + str(i) + " -> " + user)

# === Starting from any number ===
matrix = ["a", "b", "c", "d"]

for row, value in enumerate(matrix, 5):
    print("Row " + str(row) + ": " + value)

# === Using start with step-like logic ===
# Note: enumerate doesn't skip, but you can combine
scores = [95, 87, 92, 88]

for position, score in enumerate(scores, 1):
    print("Position " + str(position) + " = " + str(score))

# === Start vs offset ===
# start defines what the FIRST index will be
# You can still use the index for calculations
items = [10, 20, 30, 40]

for i, val in enumerate(items, 1):
    actual_index = i - 1  # Convert to 0-based
    print("Start=" + str(i) + ", Actual=" + str(actual_index) + ", Value=" + str(val))

# === Multiple enumerates in same loop ===
list1 = ["a", "b", "c"]
list2 = [1, 2, 3]

for i, (a, b) in enumerate(zip(list1, list2), 1):
    print("Pair " + str(i) + ": " + a + " = " + str(b))
