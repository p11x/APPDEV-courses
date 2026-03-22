# Example99.py
# Topic: Real-World Examples with Lists

# This file provides practical real-world examples using lists.


# ============================================================
# Example 1: Shopping Cart
# ============================================================
print("=== Real-world: Shopping Cart ===")

cart = []
cart.append({"item": "Apple", "price": 0.50, "qty": 4})
cart.append({"item": "Banana", "price": 0.25, "qty": 6})
cart.append({"item": "Orange", "price": 0.75, "qty": 3})

# Calculate total
total = sum(item["price"] * item["qty"] for item in cart)
print(f"Cart: {cart}")
print(f"Total: ${total:.2f}")

# Add item
cart.append({"item": "Milk", "price": 2.50, "qty": 1})
print(f"After adding milk: {total + 2.50:.2f}")

# Remove item
cart.pop(0)  # Remove first item
print(f"After removing first item: {[item['item'] for item in cart]}")


# ============================================================
# Example 2: Task Management
# ============================================================
print("\n=== Real-world: Task List ===")

tasks = []

# Add tasks
tasks.append({"id": 1, "title": "Buy groceries", "done": False})
tasks.append({"id": 2, "title": "Clean house", "done": True})
tasks.append({"id": 3, "title": "Pay bills", "done": False})

# Get pending tasks
pending = [t for t in tasks if not t["done"]]
print(f"Pending tasks: {[t['title'] for t in pending]}")

# Mark as done
tasks[0]["done"] = True
print(f"After marking done: {[t['title'] for t in tasks if t['done']]}")


# ============================================================
# Example 3: Matrix Operations
# ============================================================
print("\n=== Real-world: Matrix ===")

# 2D matrix
matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
]

# Get element
print(f"matrix[1][1]: {matrix[1][1]}")

# Get row
print(f"First row: {matrix[0]}")

# Get column
col = [row[0] for row in matrix]
print(f"First column: {col}")

# Transpose
transposed = list(zip(*matrix))
print(f"Transposed: {transposed}")


# ============================================================
# Example 4: Text Processing
# ============================================================
print("\n=== Real-world: Text Processing ===")

text = "the quick brown fox jumps over the lazy dog"

# Split into words
words = text.split()
print(f"Words: {words}")

# Get unique words (preserving order)
seen = set()
unique = []
for word in words:
    if word not in seen:
        unique.append(word)
        seen.add(word)
print(f"Unique: {unique}")

# Reverse words
reversed_words = list(reversed(words))
print(f"Reversed: {reversed_words}")


# ============================================================
# Example 5: Filtering and Transformation
# ============================================================
print("\n=== Real-world: Filter and Transform ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Filter even numbers
evens = [x for x in numbers if x % 2 == 0]
print(f"Evens: {evens}")

# Transform (square)
squares = [x ** 2 for x in numbers]
print(f"Squares: {squares}")

# Filter and transform
even_squares = [x ** 2 for x in numbers if x % 2 == 0]
print(f"Even squares: {even_squares}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD LIST USES")
print("=" * 50)
print("""
Common uses:
- Shopping carts and inventories
- Task management systems
- Matrix operations
- Text processing
- Filtering and transformation
- Queues and stacks

Key operations:
- Append/add for new items
- Pop for processing
- List comprehensions for transforms
- Slicing for subsets
""")
