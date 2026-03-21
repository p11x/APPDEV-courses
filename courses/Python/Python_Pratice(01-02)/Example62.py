# Example62.py
# Topic: Comprehensions — Nested Comprehensions

# Nested comprehensions create lists of lists or complex structures

# === Matrix (list of lists) ===
# Create a 3x3 multiplication table
matrix = [[i * j for j in range(3)] for i in range(3)]
print(matrix)
# [[0, 0, 0],
#  [0, 1, 2],
#  [0, 2, 4]]

# 3x3 multiplication table
mult_table = [[i * j for j in range(1, 4)] for i in range(1, 4)]
print(mult_table)
# [[1, 2, 3],
#  [2, 4, 6],
#  [3, 6, 9]]

# === Flatten a matrix ===
nested = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]

# Flatten using nested comprehension
flat = [num for row in nested for num in row]
print(flat)  # [1, 2, 3, 4, 5, 6, 7, 8, 9]

# Same as:
flat = []
for row in nested:
    for num in row:
        flat.append(num)
print(flat)

# === 2D coordinates ===
# Create list of (x, y) tuples
coords = [(x, y) for x in range(3) for y in range(3)]
print(coords)
# [(0, 0), (0, 1), (0, 2), (1, 0), (1, 1), (1, 2), (2, 0), (2, 1), (2, 2)]

# === Filter within nested ===
matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]

# Get only even numbers
evens = [num for row in matrix for num in row if num % 2 == 0]
print(evens)  # [2, 4, 6, 8]

# === Real-world: Grid creation ===
# 4x4 grid of zeros
grid = [[0 for _ in range(4)] for _ in range(4)]
print(grid)
# [[0, 0, 0, 0],
#  [0, 0, 0, 0],
#  [0, 0, 0, 0],
#  [0, 0, 0, 0]]

# 4x4 grid with row index
indexed_grid = [[i * 4 + j for j in range(4)] for i in range(4)]
print(indexed_grid)
# [[0, 1, 2, 3],
#  [4, 5, 6, 7],
#  [8, 9, 10, 11],
#  [12, 13, 14, 15]]

# === WARNING: Avoid deeply nested! ===

# Hard to read:
# result = [func(x) for sublist in list_of_lists for item in sublist if cond(x) for x in item]

# Better: use regular loops
# Or break into multiple steps

# === Three levels (use carefully!) ===
# Create a list of squares of numbers in nested lists
nested_nums = [[1, 2, 3], [4, 5], [6, 7, 8, 9]]

squares = [[n ** 2 for n in sublist] for sublist in nested_nums]
print(squares)  # [[1, 4, 9], [16, 25], [36, 49, 64, 81]]

# Flatten one level
all_squares = [n ** 2 for sublist in nested_nums for n in sublist]
print(all_squares)  # [1, 4, 9, 16, 25, 36, 49, 64, 81]

# === Transform nested data ===
students = [
    {"name": "Alice", "scores": [85, 90, 78]},
    {"name": "Bob", "scores": [92, 88, 95]},
    {"name": "Charlie", "scores": [70, 75, 80]},
]

# Get all scores
all_scores = [score for student in students for score in student["scores"]]
print(all_scores)  # [85, 90, 78, 92, 88, 95, 70, 75, 80]

# Get max score per student
max_scores = [{"name": s["name"], "max": max(s["scores"])} for s in students]
print(max_scores)
# [{'name': 'Alice', 'max': 90}, {'name': 'Bob', 'max': 95}, {'name': 'Charlie', 'max': 80}]
