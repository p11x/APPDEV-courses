# Example158.py
# Topic: Comprehensions Deep Dive


# ============================================================
# Example 1: List Comprehension
# ============================================================
print("=== List Comprehension ===")

squares: list = [x**2 for x in range(10)]
print(f"Squares: {squares}")

evens: list = [x for x in range(20) if x % 2 == 0]
print(f"Evens: {evens}")


# ============================================================
# Example 2: Dict Comprehension
# ============================================================
print("\n=== Dict Comprehension ===")

squares_dict: dict = {x: x**2 for x in range(5)}
print(f"Squares dict: {squares_dict}")

word_lengths: dict = {word: len(word) for word in ["apple", "banana", "cherry"]}
print(f"Word lengths: {word_lengths}")


# ============================================================
# Example 3: Set Comprehension
# ============================================================
print("\n=== Set Comprehension ===")

text: str = "hello world"
unique_chars: set = {char for char in text}
print(f"Unique chars: {unique_chars}")

numbers: list = [1, 2, 2, 3, 3, 3, 4]
unique_squares: set = {x**2 for x in numbers}
print(f"Unique squares: {unique_squares}")


# ============================================================
# Example 4: Nested List Comprehension
# ============================================================
print("\n=== Nested Comprehension ===")

matrix: list = [[i*3 + j for j in range(3)] for i in range(3)]
print("Matrix:")
for row in matrix:
    print(row)

flat: list = [num for row in matrix for num in row]
print(f"Flattened: {flat}")


# ============================================================
# Example 5: Comprehension with Conditionals
# ============================================================
print("\n=== Conditional Comprehension ===")

numbers: list = [-2, -1, 0, 1, 2, 3]

pos_squares: list = [x**2 for x in numbers if x > 0]
print(f"Positive squares: {pos_squares}")

labeled: list = ["positive" if x > 0 else "non-positive" for x in numbers]
print(f"Labeled: {labeled}")


# ============================================================
# Example 6: Dict Comprehension with Conditional
# ============================================================
print("\n=== Dict Comprehension with Condition ===")

numbers: list = [1, 2, 3, 4, 5]

squared_dict: dict = {x: x**2 for x in numbers if x % 2 == 0}
print(f"Even squares: {squared_dict}")

category: dict = {x: ("even" if x % 2 == 0 else "odd") for x in numbers}
print(f"Category: {category}")


# ============================================================
# Example 7: Generator Expression
# ============================================================
print("\n=== Generator Expression ===")

gen = (x**2 for x in range(5))
print(f"Type: {type(gen)}")

print("Values:")
for val in gen:
    print(val, end=" ")
print()


# ============================================================
# Example 8: Real-World: Data Transformation
# ============================================================
print("\n=== Real-World: Data Transform ===")

products: list[dict] = [
    {"name": "Laptop", "price": 999, "category": "electronics"},
    {"name": "Shirt", "price": 29, "category": "clothing"},
    {"name": "Phone", "price": 599, "category": "electronics"},
]

names: list = [p["name"] for p in products]
print(f"Names: {names}")

by_category: dict = {}
for p in products:
    cat = p["category"]
    if cat not in by_category:
        by_category[cat] = []
    by_category[cat].append(p["name"])

print(f"By category: {by_category}")

expensive: list = [p["name"] for p in products if p["price"] > 100]
print(f"Expensive (> $100): {expensive}")
