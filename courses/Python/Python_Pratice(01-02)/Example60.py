# Example60.py
# Topic: Comprehensions — Set Comprehensions

# Set comprehensions create sets (unique values only)

# === Basic set comprehension ===
# {expression for item in iterable}

# Create set of squares
squares = {n ** 2 for n in range(5)}
print(squares)  # {0, 1, 4, 9, 16}

# === From list (removes duplicates) ===
numbers = [1, 2, 2, 3, 3, 3, 4, 5, 5]

# Unique values
unique = {n for n in numbers}
print(unique)  # {1, 2, 3, 4, 5}

# Unique squares
unique_squares = {n ** 2 for n in numbers}
print(unique_squares)  # {1, 4, 9, 16, 25}

# === With condition (filter) ===
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Only even numbers
evens = {n for n in numbers if n % 2 == 0}
print(evens)  # {2, 4, 6, 8, 10}

# Numbers greater than 5
over_5 = {n for n in numbers if n > 5}
print(over_5)  # {6, 7, 8, 9, 10}

# === From strings ===
text = "hello world"

# Unique letters (alphabetic only)
letters = {c for c in text if c.isalpha()}
print(letters)  # {'h', 'e', 'l', 'o', 'w', 'r', 'd'}

# Unique characters
chars = {c for c in text}
print(chars)  # {'h', 'e', 'l', 'o', ' ', 'w', 'r', 'd'}

# === Real-world examples ===

# Get unique categories
products = [
    {"name": "Laptop", "category": "Electronics"},
    {"name": "Shirt", "category": "Clothing"},
    {"name": "Phone", "category": "Electronics"},
    {"name": "Pants", "category": "Clothing"},
]

categories = {p["category"] for p in products}
print(categories)  # {'Electronics', 'Clothing'}

# Get unique tags
posts = [
    {"tags": ["python", "coding"]},
    {"tags": ["python", "tutorial"]},
    {"tags": ["javascript", "coding"]},
]

all_tags = {tag for post in posts for tag in post["tags"]}
print(all_tags)  # {'python', 'coding', 'javascript', 'tutorial'}

# Get unique first letters from names
names = ["Alice", "Bob", "Anna", "Alex", "Charlie"]
first_letters = {n[0] for n in names}
print(first_letters)  # {'A', 'B', 'C'}

# === Set operations with comprehension ===
set1 = {1, 2, 3, 4, 5}
set2 = {4, 5, 6, 7, 8}

# Union of squares
union = {n ** 2 for n in set1} | {n ** 2 for n in set2}
print(union)  # {1, 4, 9, 16, 25, 36, 49, 64}

# Intersection of squares
intersection = {n ** 2 for n in set1} & {n ** 2 for n in set2}
print(intersection)  # {16, 25}

# === Dictionary to set ===
word_lengths = {"apple": 5, "banana": 6, "cherry": 6}

# Unique lengths
unique_lengths = {len(word) for word in word_lengths}
print(unique_lengths)  # {5, 6}
