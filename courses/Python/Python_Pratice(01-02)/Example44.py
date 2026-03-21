# Example44.py
# Topic: Loops — For Loops with range()

# The for loop repeats code for each item in a sequence

# === Basic for loop with range() ===

# range(stop) — from 0 to stop-1
for i in range(5):
    print(i)
# Prints: 0, 1, 2, 3, 4

# === range(start, stop) ===

# range(start, stop) — from start to stop-1
for i in range(2, 6):
    print(i)
# Prints: 2, 3, 4, 5

# === range(start, stop, step) ===

# Counting up by 2
for i in range(0, 10, 2):
    print(i)
# Prints: 0, 2, 4, 6, 8

# Counting down
for i in range(5, 0, -1):
    print(i)
# Prints: 5, 4, 3, 2, 1

# Negative step
for i in range(10, 0, -2):
    print(i)
# Prints: 10, 8, 6, 4, 2

# === Real-world example: Counting to 10 ===
for count in range(1, 11):
    print("Count: " + str(count))
# Count: 1 through 10

# === Real-world example: First 5 items ===
items = ["apple", "banana", "cherry", "date", "elderberry", "fig"]

for i in range(5):
    print(items[i])
# Prints first 5 items

# === Real-world example: Even numbers from 1-20 ===
for i in range(2, 21, 2):
    print(i)
# 2, 4, 6, 8, 10, 12, 14, 16, 18, 20

# === Real-world example: Days remaining in week ===
days = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"]
today = 2  # Wednesday (0-indexed as Mon=0)

for i in range(today + 1, 7):
    print(days[i] + " is remaining")
# Thu, Fri, Sat, Sun are remaining

# === Using range with len() ===
fruits = ["apple", "banana", "cherry"]

for i in range(len(fruits)):
    print("Index " + str(i) + ": " + fruits[i])
# Index 0: apple
# Index 1: banana
# Index 2: cherry

# === Real-world example: Print numbered list ===
tasks = ["Buy groceries", "Clean house", "Walk dog", "Call mom"]

for i in range(len(tasks)):
    print(str(i + 1) + ". " + tasks[i])
# 1. Buy groceries
# 2. Clean house
# 3. Walk dog
# 4. Call mom
