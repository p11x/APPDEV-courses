# Example260: Matrix Operations
import numpy as np

# Basic matrix operations (without numpy for pure Python)
def matrix_add(A, B):
    """Add two matrices."""
    return [[A[i][j] + B[i][j] for j in range(len(A[0]))] for i in range(len(A))]

def matrix_multiply(A, B):
    """Multiply two matrices."""
    result = [[0] * len(B[0]) for _ in range(len(A))]
    for i in range(len(A)):
        for j in range(len(B[0])):
            for k in range(len(B)):
                result[i][j] += A[i][k] * B[k][j]
    return result

def matrix_transpose(A):
    """Transpose a matrix."""
    return [[A[j][i] for j in range(len(A))] for i in range(len(A[0]))]

print("Matrix Operations (Pure Python):")
A = [[1, 2], [3, 4]]
B = [[5, 6], [7, 8]]

print(f"A: {A}")
print(f"B: {B}")
print(f"A + B: {matrix_add(A, B)}")
print(f"A * B: {matrix_multiply(A, B)}")
print(f"A transpose: {matrix_transpose(A)}")

# Using numpy
print("\nWith NumPy:")
A = np.array([[1, 2], [3, 4]])
B = np.array([[5, 6], [7, 8]])
print(f"A + B: {A + B}")
print(f"A * B (element-wise): {A * B}")
print(f"A @ B (matrix multiply): {A @ B}")
print(f"A transpose: {A.T}")
print(f"A inverse: {np.linalg.inv(A)}")

# Matrix determinant and eigenvalues
print("\nDeterminant and eigenvalues:")
A = np.array([[4, 2], [3, 1]])
print(f"Matrix: {A}")
print(f"Determinant: {np.linalg.det(A):.2f}")
eigenvalues, eigenvectors = np.linalg.eig(A)
print(f"Eigenvalues: {eigenvalues}")

# Reshape and flatten
print("\nReshape:")
arr = np.arange(12)
print(f"1D: {arr}")
print(f"3x4: {arr.reshape(3, 4)}")
print(f"Flatten: {arr.reshape(3, 4).flatten()}")

# Diagonal matrix
print("\nDiagonal:")
print(f"Diagonal of A: {np.diag([1,2,3])}")
print(f"Extract diag: {np.diag(np.array([[1,2],[3,4]]))}")

# Identity and zeros
print("\nIdentity and zeros:")
print(f"3x3 identity: {np.eye(3)}")
print(f"2x4 zeros: {np.zeros((2, 4))}")
print(f"2x3 ones: {np.ones((2, 3))}")
