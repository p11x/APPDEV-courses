# Example48.py
# Topic: Loops — Unpacking in For Loops

# Unpacking lets you extract multiple values from each item

# === Unpacking tuples ===
pairs = [(1, 2), (3, 4), (5, 6)]

for a, b in pairs:
    print("a=" + str(a) + ", b=" + str(b))
# a=1, b=2
# a=3, b=4
# a=5, b=6

# === Unpacking with three values ===
triples = [(1, 2, 3), (4, 5, 6), (7, 8, 9)]

for x, y, z in triples:
    print("Sum: " + str(x + y + z))
# Sum: 6
# Sum: 15
# Sum: 24

# === Unpacking dictionaries ===
users = [
    {"name": "Alice", "age": 25, "city": "NYC"},
    {"name": "Bob", "age": 30, "city": "LA"},
]

for user in users:
    name = user["name"]
    age = user["age"]
    city = user["city"]
    print(name + ", " + str(age) + ", lives in " + city)
# Alice, 25, lives in NYC
# Bob, 30, lives in LA

# === Real-world example: Coordinates ===
points = [(0, 0), (1, 1), (2, 4), (3, 9)]

for x, y in points:
    print("Point (" + str(x) + ", " + str(y) + ")")
# Point (0, 0)
# Point (1, 1)
# ...

# === Real-world example: Name parsing ===
full_names = ["John Smith", "Jane Doe", "Bob Johnson"]

for full_name in full_names:
    first, last = full_name.split()
    print("First: " + first + ", Last: " + last)
# First: John, Last: Smith
# First: Jane, Last: Doe
# First: Bob, Last: Johnson

# === Real-world example: Employee records ===
employees = [
    ("Alice", "Engineer", 75000),
    ("Bob", "Designer", 65000),
    ("Charlie", "Manager", 90000),
]

for name, role, salary in employees:
    print(name + " is a " + role + " earning $" + str(salary))
# Alice is a Engineer earning $75000
# Bob is a Designer earning $65000
# Charlie is a Manager earning $90000

# === Unpacking with *rest ===
data = [[1, 2, 3], [4, 5, 6, 7], [8, 9]]

for first, *rest in data:
    print("First: " + str(first) + ", Rest: " + str(rest))
# First: 1, Rest: [2, 3]
# First: 4, Rest: [5, 6, 7]
# First: 8, Rest: [9]

# === Unpacking nested tuples ===
nested = [(1, (2, 3)), (4, (5, 6)), (7, (8, 9))]

for outer, (inner1, inner2) in nested:
    print("Outer: " + str(outer) + ", Inner: " + str(inner1) + ", " + str(inner2))
# Outer: 1, Inner: 2, 3
# Outer: 4, Inner: 5, 6
# ...

# === Real-world example: Matrix operations ===
matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9],
]

for row in matrix:
    a, b, c = row
    print("Row: " + str(a) + ", " + str(b) + ", " + str(c))
# Row: 1, 2, 3
# Row: 4, 5, 6
# Row: 7, 8, 9

# === Real-world example: Key-value pairs from dict ===
config = {"host": "localhost", "port": 8080, "debug": True}

for key, value in config.items():
    print(key + " = " + str(value))
# host = localhost
# port = 8080
# debug = True

# === Using enumerate with unpacking ===
items = ["apple", "banana", "cherry"]

for index, item in enumerate(items):
    print(str(index) + ": " + item)
# 0: apple
# 1: banana
# 2: cherry

# === Combining zip and unpacking ===
headers = ["name", "age", "city"]
row1 = ["Alice", 25, "NYC"]
row2 = ["Bob", 30, "LA"]

for header, value in zip(headers, row1):
    print(header + ": " + str(value))
# name: Alice
# age: 25
# city: NYC

for header, value in zip(headers, row2):
    print(header + ": " + str(value))
# name: Bob
# age: 30
# city: LA
