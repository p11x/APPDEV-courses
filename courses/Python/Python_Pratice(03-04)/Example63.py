# Example63.py
# Topic: Combining map(), filter(), and reduce()

# This file demonstrates chaining functional tools for complex transformations.


# ============================================================
# Example 1: Pipeline - Filter then Map
# ============================================================
print("=== Pipeline: Filter then Map ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Filter evens, then square them
result = list(map(lambda x: x ** 2, filter(lambda x: x % 2 == 0, numbers)))
print(f"Original: {numbers}")
print(f"Even squares: {result}")


# ============================================================
# Example 2: Pipeline - Map then Filter
# ============================================================
print("\n=== Pipeline: Map then Filter ===")

numbers = [1, 2, 3, 4, 5]

# Square, then keep results > 10
result = list(filter(lambda x: x > 10, map(lambda x: x ** 2, numbers)))
print(f"Original: {numbers}")
print(f"Squares > 10: {result}")


# ============================================================
# Example 3: Pipeline - Map then Filter then Reduce
# ============================================================
print("\n=== Pipeline: Map -> Filter -> Reduce ===")

from functools import reduce

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Keep evens, square them, then sum
result = reduce(
    lambda a, b: a + b,
    map(lambda x: x ** 2, filter(lambda x: x % 2 == 0, numbers))
)
print(f"Original: {numbers}")
print(f"Sum of even squares: {result}")


# ============================================================
# Example 4: Real-world - Process sales data
# ============================================================
print("\n=== Real-world: Sales Processing ===")

sales = [
    {'product': 'Phone', 'price': 800, 'quantity': 10},
    {'product': 'Laptop', 'price': 1200, 'quantity': 5},
    {'product': 'Tablet', 'price': 400, 'quantity': 15},
    {'product': 'Watch', 'price': 200, 'quantity': 20},
]

# Filter expensive items, calculate total revenue
expensive_sales = filter(lambda s: s['price'] > 500, sales)
revenues = map(lambda s: s['price'] * s['quantity'], expensive_sales)
total = reduce(lambda a, b: a + b, revenues, 0)

print(f"Total revenue from items > $500: ${total}")


# ============================================================
# Example 5: Real-world - User processing
# ============================================================
print("\n=== Real-world: User Processing ===")

users = [
    {'name': 'Alice', 'age': 25, 'active': True},
    {'name': 'Bob', 'age': 17, 'active': True},
    {'name': 'Charlie', 'age': 30, 'active': False},
    {'name': 'Diana', 'age': 22, 'active': True},
    {'name': 'Eve', 'age': 19, 'active': True},
]

# Get names of active adults
adult_names = list(map(
    lambda u: u['name'],
    filter(lambda u: u['age'] >= 18 and u['active'], users)
))
print(f"Active adults: {adult_names}")


# ============================================================
# Example 6: Real-world - Text processing
# ============================================================
print("\n=== Real-world: Text Processing ===")

sentences = [
    "hello world",
    "python is awesome",
    "functional programming",
    "map filter reduce"
]

# Get lengths of words in each sentence
word_lengths = list(map(
    lambda s: list(map(len, s.split())),
    sentences
))
print(f"Sentence word lengths: {word_lengths}")

# Total characters (excluding spaces)
total_chars = reduce(
    lambda a, s: a + len(s.replace(' ', '')),
    sentences,
    0
)
print(f"Total characters: {total_chars}")


# ============================================================
# Example 7: Chaining with comprehensions
# ============================================================
print("\n=== Functional vs Comprehension ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Functional approach
functional = reduce(
    lambda a, b: a + b,
    map(lambda x: x ** 2, filter(lambda x: x > 5, numbers))
)

# Comprehension approach
comprehension = sum([x ** 2 for x in numbers if x > 5])

print(f"Functional result: {functional}")
print(f"Comprehension result: {comprehension}")
print(f"Equal: {functional == comprehension}")


# ============================================================
# Example 8: Multiple aggregations
# ============================================================
print("\n=== Multiple aggregations ===")

from functools import reduce

data = [1, 2, 3, 4, 5]

# Single pass aggregation
def analyze(acc, x):
    acc['sum'] += x
    acc['count'] += 1
    acc['max'] = max(acc['max'], x)
    acc['min'] = min(acc['min'], x)
    return acc

result = reduce(analyze, data, {'sum': 0, 'count': 0, 'max': float('-inf'), 'min': float('inf')})
print(f"Analysis: {result}")


# ============================================================
# Example 9: Nested operations
# ============================================================
print("\n=== Nested operations ===")

# Process matrix
matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
]

# Sum of all elements
total = reduce(lambda a, b: a + b, map(lambda r: reduce(lambda x, y: x + y, r), matrix))
print(f"Matrix sum: {total}")

# Sum of row sums
row_sums = list(map(lambda r: reduce(lambda a, b: a + b, r), matrix))
print(f"Row sums: {row_sums}")


# ============================================================
# Example 10: Complex data transformation
# ============================================================
print("\n=== Complex transformation ===")

transactions = [
    {'id': 1, 'amount': 100, 'type': 'credit'},
    {'id': 2, 'amount': 50, 'type': 'debit'},
    {'id': 3, 'amount': 200, 'type': 'credit'},
    {'id': 4, 'amount': 30, 'type': 'debit'},
]

# Calculate net balance
credits = reduce(lambda a, t: a + t['amount'], filter(lambda t: t['type'] == 'credit', transactions), 0)
debits = reduce(lambda a, t: a + t['amount'], filter(lambda t: t['type'] == 'debit', transactions), 0)
net = credits - debits

print(f"Credits: ${credits}")
print(f"Debits: ${debits}")
print(f"Net balance: ${net}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Combining map, filter, reduce")
print("=" * 50)
print("""
- Chain operations: map -> filter -> reduce
- Order matters for performance
- Use list() to materialize intermediate results
- Can often replace with comprehensions
- Useful for data pipelines
""")
