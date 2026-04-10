# Topic: Linear_Algebra_with_NumPy
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Linear Algebra with NumPy

I. INTRODUCTION
   NumPy provides linear algebra operations through numpy.linalg.
   This covers matrix operations, solving linear systems, and eigenvalues.
   Prerequisites: NumPy arrays
   Requirements: NumPy 1.21+

II. CORE_CONCEPTS
   - Matrix multiplication
   - Matrix inverse and determinant
   - Solving linear systems
   - Eigenvalues and eigenvectors
"""

import numpy as np


def main():
    print("Executing Linear Algebra with NumPy")
    demonstrate_matrix_ops()
    demonstrate_solve()
    demonstrate_eigen()
    banking_application()


def demonstrate_matrix_ops():
    """Matrix operations"""
    print("\n--- Matrix Multiplication ---")
    A = np.array([[1, 2], [3, 4]])
    B = np.array([[5, 6], [7, 8]])
    print(f"A:\\n{A}")
    print(f"B:\\n{B}")
    print(f"A @ B:\\n{A @ B}")
    print(f"dot: {np.dot(A, B)}")
    
    print("\n--- Transpose ---")
    print(f"A.T:\\n{A.T}")
    
    print("\n--- Inverse ---")
    A = np.array([[1, 2], [3, 4]])
    A_inv = np.linalg.inv(A)
    print(f"Ainv:\\n{A_inv}")
    print(f"A @ Ainv:\\n{A @ A_inv}")
    
    print("\n--- Determinant ---")
    det = np.linalg.det(A)
    print(f"det(A): {det}")


def demonstrate_solve():
    """Solving linear systems"""
    print("\n--- Solve Ax = b ---")
    A = np.array([[2, 1], [1, 3]])
    b = np.array([3, 4])
    x = np.linalg.solve(A, b)
    print(f"A:\\n{A}")
    print(f"b: {b}")
    print(f"x: {x}")
    print(f"Verification: {A @ x}")


def demonstrate_eigen():
    """Eigenvalues and eigenvectors"""
    print("\n--- Eigenvalues ---")
    A = np.array([[4, 2], [1, 3]])
    eigenvalues, eigenvectors = np.linalg.eig(A)
    print(f"A:\\n{A}")
    print(f"Eigenvalues: {eigenvalues}")
    print(f"Eigenvectors:\\n{eigenvectors}")


def banking_application():
    """Portfolio optimization example"""
    print("\n=== Banking: Portfolio Variance ===")
    
    returns = np.array([[0.1, 0.05], [0.05, 0.15]])
    cov_matrix = np.cov(returns)
    print(f"Covariance matrix:\\n{cov_matrix}")
    
    weights = np.array([0.6, 0.4])
    variance = weights @ cov_matrix @ weights
    print(f"Portfolio variance: {variance:.4f}")


def test_linear():
    A = np.array([[1, 0], [0, 1]])
    assert np.allclose(np.linalg.inv(A), A)
    print("Linear algebra test passed!")


if __name__ == "__main__":
    main()
    test_linear()