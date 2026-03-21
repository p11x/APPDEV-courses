# Example58.py
# Topic: Comprehensions — List Comprehensions

# List comprehensions create lists in a concise way

# === Traditional loop vs comprehension ===

# Traditional loop
squares = []

for i in range(10):
    squares.append(i * i)

print(squares)  # [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]

# List comprehension - same result, one line!
squares = [i * i for i in range(10)]
print(squares)  # [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]

# === Syntax ===
# [expression for item in iterable if condition]

# === Basic examples ===
numbers = [1, 2, 3, 4, 5]

# Double each number
doubled = [n * 2 for n in numbers]
print(doubled)  # [2, 4, 6, 8, 10]

# Add 10 to each
added = [n + 10 for n in numbers]
print(added)  # [11, 12, 13, 14, 15]

# Square each
squared = [n ** 2 for n in numbers]
print(squared)  # [1, 4, 9, 16, 25]

# === With condition (filter) ===
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Only even numbers
evens = [n for n in numbers if n % 2 == 0]
print(evens)  # [2, 4, 6, 8, 10]

# Only odd numbers
odds = [n for n in numbers if n % 2 != 0]
print(odds)  # [1, 3, 5, 7, 9]

# Numbers greater than 5
over_5 = [n for n in numbers if n > 5]
print(over_5)  # [6, 7, 8, 9, 10]

# === Transform strings ===
words = ["hello", "world", "python"]

# Uppercase
upper = [w.upper() for w in words]
print(upper)  # ['HELLO', 'WORLD', 'PYTHON']

# First letter
first_letters = [w[0] for w in words]
print(first_letters)  # ['h', 'w', 'p']

# Length of each word
lengths = [len(w) for w in words]
print(lengths)  # [5, 5, 6]

# === Real-world examples ===

# Filter prices over $10
prices = [5.99, 15.00, 8.50, 25.00, 9.99]
expensive = [p for p in prices if p > 10]
print(expensive)  # [15.0, 25.0]

# Get names starting with A
names = ["Alice", "Bob", "Anna", "Alex", "Charlie"]
a_names = [n for n in names if n.startswith("A")]
print(a_names)  # ['Alice', 'Anna', 'Alex']

# Extract ages over 18
users = [
    {"name": "Alice", "age": 25},
    {"name": "Bob", "age": 16},
    {"name": "Charlie", "age": 30},
]
adults = [u["name"] for u in users if u["age"] >= 18]
print(adults)  # ['Alice', 'Charlie']

# === Nested ===
numbers = [[1, 2], [3, 4], [5, 6]]

# Flatten: get all numbers from nested lists
flat = [num for sublist in numbers for num in sublist]
print(flat)  # [1, 2, 3, 4, 5, 6]
