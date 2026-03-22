# Example189.py
# Topic: List Comprehensions - Nested & Transform

# This file demonstrates nested list comprehensions and complex transformations.


# ============================================================
# Example 1: Nested For Loops
# ============================================================
print("=== Nested For Loops ===")

coords = [(x, y) for x in range(3) for y in range(3)]
print(f"Coords: {coords}")    # [(0,0), (0,1), (0,2), (1,0), ...]


# ============================================================
# Example 2: Matrix Transpose
# ============================================================
print("\n=== Matrix Transpose ===")

matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
transposed = [[row[i] for row in matrix] for i in range(3)]
print(f"Transposed: {transposed}")    # [[1,4,7], [2,5,8], [3,6,9]]


# ============================================================
# Example 3: Flatten Nested List
# ============================================================
print("\n=== Flatten Nested List ===")

nested = [[1, 2], [3, 4], [5, 6]]
flat = [x for row in nested for x in row]
print(f"Flat: {flat}")    # [1, 2, 3, 4, 5, 6]


# ============================================================
# Example 4: Conditional Transform
# ============================================================
print("\n=== Conditional Transform ===")

numbers = [1, 2, 3, 4, 5]
result = [x * 10 if x % 2 == 0 else x for x in numbers]
print(f"Result: {result}")    # [1, 20, 3, 40, 5]


# ============================================================
# Example 5: Matrix Multiplication
# ============================================================
print("\n=== Matrix Diagonal ===")

matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
diagonal = [matrix[i][i] for i in range(min(len(matrix), len(matrix[0])))]
print(f"Diagonal: {diagonal}")    # [1, 5, 9]


# ============================================================
# Example 6: Filter and Flatten
# ============================================================
print("\n=== Filter and Flatten ===")

data = [[1, 2, 3], [4, 5], [6, 7, 8, 9]]
filtered = [x for row in data for x in row if x > 4]
print(f"Filtered: {filtered}")    # [5, 6, 7, 8, 9]


# ============================================================
# Example 7: 3D Array Flatten
# ============================================================
print("\n=== 3D Array ===")

cube = [[[1, 2], [3, 4]], [[5, 6], [7, 8]]]
flat = [x for layer in cube for row in layer for x in row]
print(f"Flat: {flat}")    # [1, 2, 3, 4, 5, 6, 7, 8]


# ============================================================
# Example 8: String Grid
# ============================================================
print("\n=== String Grid ===")

grid = [[chr(65 + j) + str(i) for j in range(3)] for i in range(3)]
for row in grid:
    print(row)


# ============================================================
# Example 9: Combined Transform
# ============================================================
print("\n=== Combined Transform ===")

pairs = [(1, 2), (3, 4), (5, 6)]
result = [a + b for a, b in pairs if (a + b) % 2 == 0]
print(f"Even sums: {result}")    # [6, 12]


# ============================================================
# Example 10: Complex Nested
# ============================================================
print("\n=== Complex Nested ===")

data = {"a": [1, 2], "b": [3, 4]}
result = [(k, v) for k, vals in data.items() for v in vals if v % 2 == 0]
print(f"Result: {result}")    # [('a', 2), ('b', 4)]


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
NESTED COMPREHENSION:
[[x for x in row] for row in matrix]
[x for row in data for x in row if cond]

USES:
- Matrix operations
- Flattening
- Grid generation
- Cartesian products
""")
