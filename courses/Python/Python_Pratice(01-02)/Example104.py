# Example104.py
# Topic: Iteration Tools — Basic Enumerate

# Use enumerate() to get index-value pairs

# === Basic enumerate with list ===
fruits = ["apple", "banana", "cherry"]

for index, fruit in enumerate(fruits):
    print("Index " + str(index) + ": " + fruit)

# === Enumerate with strings ===
message = "hello"

for index, char in enumerate(message):
    print("Position " + str(index) + ": " + char)

# === Enumerate with tuples ===
coords = [(0, 0), (1, 1), (2, 4)]

for index, (x, y) in enumerate(coords):
    print("Point " + str(index) + ": (" + str(x) + ", " + str(y) + ")")

# === Enumerate with dictionaries ===
# Note: dict iteration gives keys only
person = {"name": "Alice", "age": 30, "city": "NYC"}

for index, key in enumerate(person):
    print("Key " + str(index) + ": " + key)

# === Enumerate returns tuples ===
# enumerate() yields (index, value) tuples
items = ["a", "b", "c"]

result = list(enumerate(items))
print("Enumerate result: " + str(result))

# === Enumerate with list comprehension ===
numbers = [10, 20, 30]

indexed = [(i, n) for i, n in enumerate(numbers)]
print("Indexed: " + str(indexed))

# === Practical: Numbered list ===
shopping = ["milk", "bread", "eggs", "butter"]

print("Shopping list:")
for i, item in enumerate(shopping):
    print(str(i + 1) + ". " + item)

# === Practical: Find position of item ===
colors = ["red", "green", "blue", "green", "yellow"]

for index, color in enumerate(colors):
    if color == "blue":
        print("Blue found at index " + str(index))

# === Enumerate with start=0 (default) ===
letters = ["x", "y", "z"]

for i, letter in enumerate(letters, 0):
    print("Letter " + str(i) + ": " + letter)

# === What enumerate does internally ===
# It's equivalent to:
# def enumerate(iterable, start=0):
#     n = start
#     for item in iterable:
#         yield n, item
#         n += 1

# This replaces manual counter incrementing
