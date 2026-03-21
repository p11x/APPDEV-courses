# Example59.py
# Topic: Comprehensions — Dictionary Comprehensions

# Dictionary comprehensions create dictionaries concisely

# === Basic dictionary comprehension ===
# {key: value for item in iterable}

# Create squares dictionary
squares = {i: i ** 2 for i in range(5)}
print(squares)  # {0: 0, 1: 1, 2: 4, 3: 9, 4: 16}

# === From two lists ===
keys = ["a", "b", "c"]
values = [1, 2, 3]

# Combine two lists into dictionary
combined = {k: v for k, v in zip(keys, values)}
print(combined)  # {'a': 1, 'b': 2, 'c': 3}

# === With condition (filter) ===
numbers = [1, 2, 3, 4, 5]

# Only squares over 5
squares_over_5 = {n: n ** 2 for n in numbers if n ** 2 > 5}
print(squares_over_5)  # {3: 9, 4: 16, 5: 25}

# === Real-world examples ===

# Word lengths
words = ["apple", "banana", "cherry"]
word_lengths = {w: len(w) for w in words}
print(word_lengths)  # {'apple': 5, 'banana': 6, 'cherry': 6}

# Phone book
names = ["Alice", "Bob", "Charlie"]
phones = ["555-1234", "555-5678", "555-9012"]
phonebook = {n: p for n, p in zip(names, phones)}
print(phonebook)  # {'Alice': '555-1234', 'Bob': '555-5678', 'Charlie': '555-9012'}

# Filter by value
scores = {"Alice": 95, "Bob": 72, "Charlie": 88, "Diana": 60}

# Passing grades only (>= 70)
passing = {name: score for name, score in scores.items() if score >= 70}
print(passing)  # {'Alice': 95, 'Bob': 72, 'Charlie': 88}

# Failing grades
failing = {name: score for name, score in scores.items() if score < 70}
print(failing)  # {'Diana': 60}

# === Swap keys and values ===
original = {"a": 1, "b": 2, "c": 3}
swapped = {v: k for k, v in original.items()}
print(swapped)  # {1: 'a', 2: 'b', 3: 'c'}

# === Transform values ===
products = {"apple": 0.50, "banana": 0.30, "cherry": 0.75}

# Convert prices to include dollar sign
priced = {k: "$" + str(v) for k, v in products.items()}
print(priced)  # {'apple': '$0.5', 'banana': '$0.3', 'cherry': '$0.75'}

# Double prices
doubled = {k: v * 2 for k, v in products.items()}
print(doubled)  # {'apple': 1.0, 'banana': 0.6, 'cherry': 1.5}

# === Group users by city (loop version) ===
users = [
    {"name": "Alice", "age": 25, "city": "NYC"},
    {"name": "Bob", "age": 30, "city": "LA"},
    {"name": "Charlie", "age": 35, "city": "NYC"},
]

by_city = {}
for user in users:
    city = user["city"]
    if city not in by_city:
        by_city[city] = []
    by_city[city].append(user["name"])

print(by_city)  # {'NYC': ['Alice', 'Charlie'], 'LA': ['Bob']}

# === Create lookup tables ===
# Create a lookup for user IDs
user_ids = [1, 2, 3]
user_names = ["Alice", "Bob", "Charlie"]

id_to_name = {uid: name for uid, name in zip(user_ids, user_names)}
name_to_id = {name: uid for uid, name in zip(user_ids, user_names)}

print(id_to_name)   # {1: 'Alice', 2: 'Bob', 3: 'Charlie'}
print(name_to_id)   # {'Alice': 1, 'Bob': 2, 'Charlie': 3}

# === Create dict from single list ===
names = ["apple", "banana", "cherry"]
name_to_len = {name: len(name) for name in names}
print(name_to_len)  # {'apple': 5, 'banana': 6, 'cherry': 6}
