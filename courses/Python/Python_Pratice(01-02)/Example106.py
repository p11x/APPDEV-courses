# Example106.py
# Topic: Iteration Tools — Replacing Manual Counters

# Use enumerate instead of manual i += 1 counters

# === Old way: Manual counter ===
print("=== Old way: Manual counter ===")
fruits = ["apple", "banana", "cherry"]
i = 0
for fruit in fruits:
    print(str(i) + ": " + fruit)
    i += 1

# === New way: enumerate ===
print("\n=== New way: enumerate ===")
fruits = ["apple", "banana", "cherry"]
for i, fruit in enumerate(fruits):
    print(str(i) + ": " + fruit)

# === Before: While loop with index ===
print("\n=== Before: While loop ===")
items = ["a", "b", "c"]
i = 0
while i < len(items):
    print(str(i) + ": " + items[i])
    i += 1

# === After: For loop with enumerate ===
print("\n=== After: For loop with enumerate ===")
items = ["a", "b", "c"]
for i, item in enumerate(items):
    print(str(i) + ": " + item)

# === Before: Counter with conditional ===
print("\n=== Before: Counter with condition ===")
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
evens = []
i = 0
for n in numbers:
    if n % 2 == 0:
        evens.append((i, n))
    i += 1
print("Evens: " + str(evens))

# === After: enumerate with condition ===
print("\n=== After: enumerate with condition ===")
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
evens = [(i, n) for i, n in enumerate(numbers) if n % 2 == 0]
print("Evens: " + str(evens))

# === Before: Finding index manually ===
print("\n=== Before: Finding index ===")
texts = ["hello", "world", "python"]
target = "python"
i = 0
found = -1
for t in texts:
    if t == target:
        found = i
        break
    i += 1
print("Found at index: " + str(found))

# === After: Using enumerate ===
print("\n=== After: Using enumerate ===")
texts = ["hello", "world", "python"]
target = "python"
found = -1
for i, t in enumerate(texts):
    if t == target:
        found = i
        break
print("Found at index: " + str(found))

# === Before: Building indexed list ===
print("\n=== Before: Building indexed list ===")
data = ["x", "y", "z"]
indexed = []
i = 0
for d in data:
    indexed.append((i, d))
    i += 1
print(str(indexed))

# === After: Using enumerate ===
print("\n=== After: Using enumerate ===")
data = ["x", "y", "z"]
indexed = list(enumerate(data))
print(str(indexed))

# === After: Multiple lists with zip + enumerate ===
print("\n=== After: Multiple lists with zip + enumerate ===")
names = ["Alice", "Bob"]
ages = [25, 30]
for i, (name, age) in enumerate(zip(names, ages)):
    print(name + " is " + str(age))
