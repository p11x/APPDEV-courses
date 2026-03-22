# Example206.py
# Topic: Advanced List Comprehensions - Matrix Operations

# This file demonstrates advanced list comprehension patterns for matrix operations.


# ============================================================
# Example 1: Create Matrix
# ============================================================
print("=== Create Matrix ===")

matrix = [[i * 3 + j for j in range(3)] for i in range(3)]
for row in matrix:
    print(row)


# ============================================================
# Example 2: Transpose Matrix
# ============================================================
print("\n=== Transpose ===")

matrix = [[1, 2, 3], [4, 5, 6]]
transposed = [[row[i] for row in matrix] for i in range(len(matrix[0]))]
print(f"Transposed: {transposed}")


# ============================================================
# Example 3: Flatten Matrix
# ============================================================
print("\n=== Flatten ===")

matrix = [[1, 2], [3, 4], [5, 6]]
flat = [x for row in matrix for x in row]
print(f"Flat: {flat}")


# ============================================================
# Example 4: Diagonal Elements
# ============================================================
print("\n=== Diagonal ===")

matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
diagonal = [matrix[i][i] for i in range(min(len(matrix), len(matrix[0])))]
print(f"Diagonal: {diagonal}")


# ============================================================
# Example 5: Upper Triangle
# ============================================================
print("\n=== Upper Triangle ===")

matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
upper = [matrix[i][j] for i in range(3) for j in range(3) if i <= j]
print(f"Upper: {upper}")


# ============================================================
# Example 6: Spiral Matrix
# ============================================================
print("\n=== Spiral ===")

def spiral(n):
    matrix = [[0] * n for _ in range(n)]
    num = 1
    for layer in range((n + 1) // 2):
        for c in range(layer, n - layer):
            matrix[layer][c] = num; num += 1
        for r in range(layer + 1, n - layer):
            matrix[r][n - layer - 1] = num; num += 1
        for c in range(n - layer - 2, layer - 1, -1):
            matrix[n - layer - 1][c] = num; num += 1
        for r in range(n - layer - 2, layer, -1):
            matrix[r][layer] = num; num += 1
    return matrix

for row in spiral(3):
    print(row)


# ============================================================
# Example 7: Rotate 90 Degrees
# ============================================================
print("\n=== Rotate 90 ===")

matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
rotated = [[matrix[len(matrix) - 1 - j][i] for j in range(len(matrix))] for i in range(len(matrix[0]))]
print(f"Rotated: {rotated}")


# ============================================================
# Example 8: Find Saddle Point
# ============================================================
print("\n=== Saddle Point ===")

matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
min_in_row = [min(row) for row in matrix]
max_in_col = [max(matrix[r][c] for r in range(len(matrix))) for c in range(len(matrix[0]))]
print(f"Min in rows: {min_in_row}")
print(f"Max in cols: {max_in_col}")


# ============================================================
# Example 9: Matrix Addition
# ============================================================
print("\n=== Matrix Add ===")

a = [[1, 2], [3, 4]]
b = [[5, 6], [7, 8]]
c = [[a[i][j] + b[i][j] for j in range(len(a[0]))] for i in range(len(a))]
print(f"Sum: {c}")


# ============================================================
# Example 10: Matrix Multiplication
# ============================================================
print("\n=== Matrix Multiply ===")

a = [[1, 2], [3, 4]]
b = [[5, 6], [7, 8]]
result = [[sum(a[i][k] * b[k][j] for k in range(len(b))) for j in range(len(b[0]))] for i in range(len(a))]
print(f"Product: {result}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
MATRIX OPERATIONS:
- Transpose: [[row[i] for row in matrix]...]
- Flatten: [x for row for x in row]
- Diagonal: [matrix[i][i]...]
- Rotation, multiplication possible
""")
