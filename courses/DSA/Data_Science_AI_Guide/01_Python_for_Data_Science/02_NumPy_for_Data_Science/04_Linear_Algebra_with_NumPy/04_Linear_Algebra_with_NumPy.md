# Linear Algebra with NumPy

## Introduction

Linear algebra is the branch of mathematics concerning linear equations, linear maps, and their representations in vector spaces and through matrices. NumPy provides comprehensive support for linear algebra operations through its `linalg` module, which enables efficient computation of matrix operations, decompositions, and solutions to linear systems. These capabilities are fundamental to many data science and machine learning applications.

The importance of linear algebra in data science cannot be overstated. It forms the mathematical foundation for principal component analysis (PCA), singular value decomposition (SVD), linear regression, and many other machine learning algorithms. In banking, linear algebra is used for portfolio optimization, risk modeling, and option pricing. In healthcare, it enables medical imaging reconstruction, patient data analysis, and epidemiological modeling.

NumPy's linear algebra functions are implemented using highly optimized BLAS (Basic Linear Algebra Subprograms) and LAPACK (Linear Algebra Package) libraries, making them significantly faster than equivalent Python implementations. The library supports both simple operations like dot products and matrix multiplication, as well as advanced decompositions like eigendecomposition and singular value decomposition.

This module covers the fundamentals of linear algebra with NumPy, including matrix operations, solving linear systems, matrix decompositions, and applications in banking and healthcare. You'll learn how to perform complex linear algebra computations efficiently using NumPy's optimized functions.

## Fundamentals

### Matrix Creation and Basic Operations

Creating matrices and performing basic operations is the foundation of linear algebra with NumPy.

```python
import numpy as np

# Creating matrices
# 2x3 matrix
matrix_2x3 = np.array([
    [1, 2, 3],
    [4, 5, 6]
])
print("2x3 Matrix:")
print(matrix_2x3)
print(f"Shape: {matrix_2x3.shape}")

# Square matrix (3x3)
square_matrix = np.array([
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
])
print(f"\nSquare Matrix (3x3):")
print(square_matrix)

# Identity matrix
identity_3 = np.eye(3)
print(f"\nIdentity Matrix (3x3):")
print(identity_3)

# Diagonal matrix
diagonal = np.diag([1, 2, 3, 4])
print(f"\nDiagonal Matrix:")
print(diagonal)

# Zero and ones matrices
zeros = np.zeros((3, 3))
ones = np.ones((3, 3))
print(f"\nZeros Matrix:\n{zeros}")
print(f"\nOnes Matrix:\n{ones}")

# Output:
# 2x3 Matrix:
# [[1 2 3]
#  [4 5 6]]
# Shape: (2, 3)
# 
# Square Matrix (3x3):
# [[1 2 3]
#  [4 5 6]
#  [7 8 9]]
#
# Identity Matrix (3x3):
# [[1. 0. 0.]
#  [0. 1. 0.]
#  [0. 0. 1.]]
#
# Diagonal Matrix:
# [[1 0 0 0]
#  [0 2 0 0]
#  [0 0 3 0]
#  [0 0 0 4]]
```

### Matrix Multiplication

Matrix multiplication is one of the most fundamental operations in linear algebra. NumPy provides several ways to perform matrix multiplication.

```python
# Matrix multiplication using np.matmul and @ operator
A = np.array([
    [1, 2],
    [3, 4]
])

B = np.array([
    [5, 6],
    [7, 8]
])

print("Matrix A:")
print(A)
print("\nMatrix B:")
print(B)

# Using np.matmul
result_matmul = np.matmul(A, B)
print("\nnp.matmul(A, B):")
print(result_matmul)

# Using @ operator (Python 3.5+)
result_at = A @ B
print("\nA @ B:")
print(result_at)

# Using np.dot (for 2D arrays, same as matmul)
result_dot = np.dot(A, B)
print("\nnp.dot(A, B):")
print(result_dot)

# Element-wise multiplication (Hadamard product)
element_wise = A * B
print("\nElement-wise (A * B):")
print(element_wise)

# Scalar multiplication
scalar = 2 * A
print("\n2 * A:")
print(scalar)

# Matrix-vector multiplication
# For a matrix A (m x n) and vector v (n,), result is vector (m,)
v = np.array([1, 2])
result_mv = A @ v
print("\nMatrix-Vector Multiplication A @ v:")
print(f"A:\n{A}")
print(f"v: {v}")
print(f"Result: {result_mv}")

# Output:
# Matrix A:
# [[1 2]
#  [3 4]]
#
# Matrix B:
# [[5 6]
#  [7 8]]
#
# np.matmul(A, B):
# [[19 22]
#  [43 50]]
#
# A @ B:
# [[19 22]
#  [43 50]]
```

### Matrix Transpose and Inverse

Transpose and inverse operations are essential for solving linear systems and various matrix computations.

```python
# Matrix transpose
A = np.array([
    [1, 2, 3],
    [4, 5, 6]
])
print("Matrix A (2x3):")
print(A)

print("\nTranspose A.T (3x2):")
print(A.T)

# Conjugate transpose (Hermitian)
B = np.array([
    [1+1j, 2+1j],
    [3+1j, 4+1j]
])
print("\nComplex Matrix B:")
print(B)
print("\nConjugate transpose B.conj().T:")
print(B.conj().T)

# Matrix inverse
# For square matrix A, A^(-1) satisfies A @ A^(-1) = I
C = np.array([
    [1, 2],
    [3, 4]
])

# Calculate inverse
C_inv = np.linalg.inv(C)
print("\nMatrix C:")
print(C)
print("\nInverse C_inv:")
print(C_inv)

# Verify: C @ C_inv should be identity
identity_check = C @ C_inv
print("\nC @ C_inv (should be identity):")
print(identity_check)

# Pseudo-inverse for non-square matrices
# np.linalg.pinv gives the Moore-Penrose pseudo-inverse
D = np.array([
    [1, 2, 3],
    [4, 5, 6]
])

D_pinv = np.linalg.pinv(D)
print("\nNon-square Matrix D (2x3):")
print(D)
print("\nPseudo-inverse D_pinv (3x2):")
print(D_pinv)

# Verify: D @ D_pinv @ D should approximately equal D
verification = D @ D_pinv @ D
print("\nD @ D_pinv @ D (should approximate D):")
print(verification)

# Using matrix_rank to check rank
rank = np.linalg.matrix_rank(D)
print(f"\nRank of D: {rank}")
```

## Implementation

### Solving Linear Systems

Solving linear systems of equations is a fundamental problem in linear algebra with applications in engineering, physics, and data science.

```python
import numpy as np

# Solving linear systems: Ax = B
# For a system of equations:
# 2x + y = 5
# x - y = 1

# Representing as Ax = B
# A is the coefficient matrix
A = np.array([
    [2, 1],
    [1, -1]
])

# B is the constant vector
B = np.array([5, 1])

print("=" * 60)
print("SOLVING LINEAR SYSTEMS")
print("=" * 60)
print("\nSystem of equations:")
print("  2x + y = 5")
print("  x - y = 1")

print("\nCoefficient Matrix A:")
print(A)
print(f"\nConstant Vector B: {B}")

# Method 1: Using np.linalg.solve
x = np.linalg.solve(A, B)
print(f"\nSolution (np.linalg.solve): x = {x[0]:.2f}, y = {x[1]:.2f}")

# Method 2: Using matrix inverse
x_inv = np.linalg.inv(A) @ B
print(f"Solution (inverse method): x = {x_inv[0]:.2f}, y = {x_inv[1]:.2f}")

# Verify the solution
verification = A @ x
print(f"\nVerification A @ x: {verification}")
print(f"Should equal B: {B}")

# Larger system example
print("\n" + "=" * 60)
print("LARGER SYSTEM EXAMPLE")
print("=" * 60)

# 4x4 system
A_large = np.array([
    [4, 1, 1, 1],
    [1, 4, 1, 1],
    [1, 1, 4, 1],
    [1, 1, 1, 4]
])

B_large = np.array([10, 10, 10, 10])

print("Coefficient Matrix (4x4):")
print(A_large)
print(f"\nConstant Vector: {B_large}")

x_large = np.linalg.solve(A_large, B_large)
print(f"\nSolution: {x_large}")

# Verify solution
verification_large = A_large @ x_large
print(f"Verification: {verification_large}")
```

### Banking Application: Portfolio Optimization

Linear algebra is extensively used in finance for portfolio optimization, risk management, and option pricing.

```python
import numpy as np

# Portfolio optimization using mean-variance optimization
# This is a simplified Markowitz mean-variance model

# Stock data: expected returns and covariance matrix
expected_returns = np.array([0.08, 0.12, 0.06, 0.10])  # 8%, 12%, 6%, 10%
stock_names = ['Stock A', 'Stock B', 'Stock C', 'Stock D']

# Covariance matrix of returns
cov_matrix = np.array([
    [0.0100, 0.0040, 0.0020, 0.0030],
    [0.0040, 0.0200, 0.0030, 0.0050],
    [0.0020, 0.0030, 0.0080, 0020],
    [0.0030, 0.0050, 0.0020, 0.0150]
]) / 100  # Convert to decimal variance

print("=" * 60)
print("PORTFOLIO OPTIMIZATION (Markowitz Model)")
print("=" * 60)

print("\nStock Data:")
print(f"Expected Returns: {expected_returns * 100}%")
print(f"\nCovariance Matrix:")
print(cov_matrix * 10000)  # Scale for display

# Target return for optimization
target_return = 0.09  # 9%

# Number of assets
n = len(expected_returns)

# Using linear algebra for optimal portfolio weights
# Solve for weights that minimize variance for target return
# This is a quadratic optimization problem solved using linear algebra

# Create augmented system for optimization
# Minimize: w.T @ Cov @ w
# Subject to: w.T @ E[r] = target_return, sum(w) = 1

# Simplified: equal weights portfolio
weights_equal = np.ones(n) / n

# Calculate portfolio metrics
portfolio_return = weights_equal @ expected_returns
portfolio_variance = weights_equal @ cov_matrix @ weights_equal
portfolio_std = np.sqrt(portfolio_variance)

print("\nEqual-Weight Portfolio:")
print(f"  Weights: {weights_equal}")
print(f"  Expected Return: {portfolio_return*100:.2f}%")
print(f"  Volatility (Std): {portfolio_std*100:.2f}%")
print(f"  Sharpe Ratio: {portfolio_return/portfolio_std:.2f}")

# Using optimization for minimum variance portfolio
# This uses np.linalg.lstsq for a simple approximation

# Solve for minimum variance (ignoring return constraint for simplicity)
try:
    # For the minimum variance portfolio, we want to equalize risk contributions
    # This is a simplified approach
    inv_cov = np.linalg.inv(cov_matrix)
    ones = np.ones(n)
    weights_mv = inv_cov @ ones / (ones @ inv_cov @ ones)
    
    mv_return = weights_mv @ expected_returns
    mv_variance = weights_mv @ cov_matrix @ weights_mv
    mv_std = np.sqrt(mv_variance)
    
    print("\nMinimum Variance Portfolio:")
    print(f"  Weights: {weights_mv}")
    print(f"  Expected Return: {mv_return*100:.2f}%")
    print(f"  Volatility: {mv_std*100:.2f}%")
except:
    print("\nMinimum variance calculation requires proper optimization")
```

```python
# Risk parity portfolio calculation
print("=" * 60)
print("RISK PARITY PORTFOLIO")
print("=" * 60)

# Risk parity aims to equalize risk contribution from each asset
# This requires solving a system of nonlinear equations
# Using iterative approximation

# Simplified risk parity calculation
assets = len(expected_returns)
weights_rp = np.ones(assets) / assets  # Start with equal weights
volatilities = np.sqrt(np.diag(cov_matrix))

# Iterate to find risk parity weights
for iteration in range(100):
    portfolio_vol = np.sqrt(weights_rp @ cov_matrix @ weights_rp)
    
    # Marginal risk contributions
    marginal_risk = (cov_matrix @ weights_rp) / portfolio_vol
    
    # Risk contributions
    risk_contrib = weights_rp * marginal_risk
    
    # Target: equal risk contributions
    target_risk_contrib = portfolio_vol / assets
    
    # Update weights
    adjustment = target_risk_contrib / (risk_contrib + 1e-10)
    weights_rp = weights_rp * adjustment
    weights_rp = weights_rp / np.sum(weights_rp)  # Normalize

print("\nRisk Parity Portfolio:")
print(f"  Weights: {np.round(weights_rp, 4)}")

# Calculate metrics
rp_return = weights_rp @ expected_returns
rp_variance = weights_rp @ cov_matrix @ weights_rp
rp_std = np.sqrt(rp_variance)

print(f"  Expected Return: {rp_return*100:.2f}%")
print(f"  Volatility: {rp_std*100:.2f}%")
print(f"  Sharpe Ratio: {rp_return/rp_std:.2f}")

# Risk contribution per asset
marginal_risk_rp = (cov_matrix @ weights_rp) / rp_std
risk_contrib_rp = weights_rp * marginal_risk_rp

print("\nRisk Contributions:")
for name, w, rc in zip(stock_names, weights_rp, risk_contrib_rp):
    print(f"  {name}: Weight={w:.3f}, Risk Cont={rc:.4f}")
```

### Healthcare Application: Medical Imaging Processing

Linear algebra is fundamental to medical imaging techniques like CT scans and MRI, including image reconstruction and enhancement.

```python
import numpy as np

# Simulated CT scan reconstruction using linear algebra
# This is a simplified sinogram reconstruction

print("=" * 60)
print("MEDICAL IMAGING - IMAGE RECONSTRUCTION")
print("=" * 60)

# Create a simple 2D phantom (test image)
image_size = 8
phantom = np.zeros((image_size, image_size))

# Add some features to the phantom
# Central square (simulating a body part)
phantom[3:6, 3:6] = 1.0
# Larger circle (simulating another feature)
for i in range(image_size):
    for j in range(image_size):
        if ((i - 1.5)**2 + (j - 1.5)**2) <= 9:
            phantom[i, j] = 0.8

print("Original Phantom Image:")
print(phantom)

# Simulate sinogram (projection data)
# We'll create projection matrices for different angles
n_angles = 4
angles = np.linspace(0, np.pi, n_angles, endpoint=False)

# Create simplified projection matrix for each angle
def create_projection_matrix(size, angle):
    """Create a simple projection matrix at given angle"""
    # Simplified: just use different slicing based on angle
    return np.ones((size, size)) * 0.25

# Simulate sinogram data (reduced dimensions for display)
sinogram = np.zeros((n_angles, image_size))

for idx, angle in enumerate(angles):
    # Simulate projection (sum along certain direction)
    proj_matrix = create_projection_matrix(image_size, angle)
    sinogram[idx] = proj_matrix @ np.ones(image_size) * np.mean(phantom)

print(f"\nSimulated Sinogram ({n_angles} projections):")
for i, (angle, sinline) in enumerate(zip(angles, sinogram)):
    print(f"  Angle {i}: {np.round(sinline, 2)}")

# Reconstruction using simple algebraic reconstruction
# This is a simplified ART (Algebraic Reconstruction Technique)

reconstructed = np.zeros((image_size, image_size))
n_iterations = 10

for iteration in range(n_iterations):
    for i, angle in enumerate(angles):
        # Forward projection
        proj = np.sum(reconstructed) / (image_size * image_size)
        
        # Calculate error
        error = np.mean(sinogram[i]) - proj
        
        # Update (simplified)
        reconstructed += error / (n_angles * n_iterations)

print(f"\nReconstructed Image (after {n_iterations} iterations):")
# Scale to show similarity to original
reconstructed = reconstructed * np.max(phantom) / np.max(reconstructed)
print(np.round(reconstructed, 2))

# Check reconstruction quality
mse = np.mean((phantom - reconstructed) ** 2)
print(f"\nReconstruction MSE: {mse:.4f}")
```

```python
# Medical image enhancement using linear algebra
# Image denoising using matrix operations

print("=" * 60)
print("MEDICAL IMAGE ENHANCEMENT")
print("=" * 60)

# Create a noisy medical image
np.random.seed(42)
image = np.random.rand(10, 10) * 100

# Add noise
noisy_image = image + np.random.randn(10, 10) * 10

print("Original Image (first row):")
print(image[0])
print("\nNoisy Image (first row):")
print(noisy_image[0])

# Image smoothing using Gaussian filtering
# This can be represented as a linear operation

# Create a simple smoothing kernel
kernel_size = 3
kernel = np.ones((kernel_size, kernel_size)) / (kernel_size ** 2)

# Apply convolution manually (linear filtering)
def apply_kernel(image, kernel):
    """Apply a kernel to an image (simple version)"""
    h, w = image.shape
    kh, kw = kernel.shape
    result = np.zeros_like(image)
    
    for i in range(1, h-1):
        for j in range(1, w-1):
            patch = image[i-1:i+2, j-1:j+2]
            result[i, j] = np.sum(patch * kernel)
    
    return result

smoothed = apply_kernel(noisy_image, kernel)

print("\nSmoothed Image (first row):")
print(np.round(smoothed[0], 2))

# Edge enhancement using sharpening kernel
sharpening_kernel = np.array([
    [-1, -1, -1],
    [-1,  9, -1],
    [-1, -1, -1]
])

sharpened = apply_kernel(noisy_image, sharpening_kernel / 9)
sharpened = np.clip(sharpened, 0, 100)

print("\nSharpened Image (first row):")
print(np.round(sharpened[0], 2))

# Calculate quality metrics
def calculate_metrics(original, processed):
    mse = np.mean((original - processed) ** 2)
    original_std = np.std(original)
    noise_level = np.mean(np.abs(processed - original))
    return mse, noise_level

smoothed_mse, smoothed_noise = calculate_metrics(image, smoothed)
sharpened_mse, sharpened_noise = calculate_metrics(image, sharpened)

print(f"\nQuality Metrics:")
print(f"  Smoothed: MSE={smoothed_mse:.2f}, Noise={smoothed_noise:.2f}")
print(f"  Sharpened: MSE={sharpened_mse:.2f}, Noise={sharpened_noise:.2f}")
```

### Computing Eigenvalues and Eigenvectors

Eigenvalue decomposition is essential for understanding linear transformations and principal component analysis.

```python
import numpy as np

# Eigenvalue decomposition
# For a matrix A, find λ and v such that A @ v = λ * v

# Create a symmetric matrix (for real eigenvalues)
A = np.array([
    [4, 2],
    [2, 3]
])

print("=" * 60)
print("EIGENVALUE DECOMPOSITION")
print("=" * 60)

print("\nMatrix A:")
print(A)

# Compute eigenvalues and eigenvectors
eigenvalues, eigenvectors = np.linalg.eig(A)

print(f"\nEigenvalues: {eigenvalues}")
print(f"\nEigenvectors:")
print(f"  v1: {eigenvectors[:, 0]}")
print(f"  v2: {eigenvectors[:, 1]}")

# Verify: A @ v = λ * v
for i, (ev, vec) in enumerate(zip(eigenvalues, eigenvectors.T)):
    result = A @ vec
    expected = ev * vec
    print(f"\nVerification for eigenvalue {i+1}:")
    print(f"  A @ v{i+1} = {result}")
    print(f"  λ * v{i+1} = {expected}")
    print(f"  Match: {np.allclose(result, expected)}")

# For symmetric matrices, eigenvalues are always real
B = np.array([
    [1, 2, 3],
    [2, 4, 5],
    [3, 5, 6]
])

print("\n" + "=" * 60)
print("SYMMETRIC MATRIX EIGENVALUES")
print("=" * 60)

print("\nSymmetric Matrix B:")
print(B)

eigenvalues_b, eigenvectors_b = np.linalg.eig(B)

print(f"\nEigenvalues: {eigenvalues_b}")
print(f"\nEigenvectors (each column):")
for i, vec in enumerate(eigenvectors_b.T):
    print(f"  v{i+1}: {vec}")

# Eigenvalue decomposition for symmetric matrices
# A = Q @ Λ @ Q.T
Q = eigenvectors_b
Lambda = np.diag(eigenvalues_b)

reconstructed = Q @ Lambda @ Q.T
print("\nReconstructed A (Q @ Λ @ Q.T):")
print(reconstructed)
print(f"\nReconstruction matches: {np.allclose(B, reconstructed)}")
```

## Applications

### Singular Value Decomposition (SVD)

SVD is one of the most important matrix decompositions in data science, used for dimensionality reduction, image compression, and collaborative filtering.

```python
import numpy as np

# Singular Value Decomposition
# A = U @ Σ @ V.T
# Where U and V are orthogonal matrices, Σ is diagonal

A = np.array([
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
])

print("=" * 60)
print("SINGULAR VALUE DECOMPOSITION (SVD)")
print("=" * 60)

print("Matrix A:")
print(A)

# Compute SVD
U, S, Vt = np.linalg.svd(A)

print(f"\nLeft Singular Vectors (U) - shape {U.shape}:")
print(U)

print(f"\nSingular Values (S): {S}")

print(f"\nRight Singular Vectors (V.T) - shape {Vt.shape}:")
print(Vt)

# Reconstruct the original matrix
# Need to expand S to match dimensions
Sigma = np.zeros_like(A)
Sigma[:min(A.shape), :min(A.shape)] = np.diag(S)

reconstructed = U @ Sigma @ Vt
print("\nReconstructed Matrix:")
print(reconstructed)
print(f"\nMatches original: {np.allclose(A, reconstructed)}")

# Using SVD for dimensionality reduction
print("\n" + "=" * 60)
print("SVD FOR DIMENSIONALITY REDUCTION")
print("=" * 60)

# Create a data matrix
data = np.random.rand(100, 20) * 10

print(f"Original Data Shape: {data.shape}")

# Perform SVD
U_data, S_data, Vt_data = np.linalg.svd(data)

# Keep only top k components
k = 5

# Reduce dimensions
data_reduced = U_data[:, :k] @ np.diag(S_data[:k])
print(f"\nReduced Data Shape (k={k}): {data_reduced.shape}")

# Reconstruct approximation
data_approx = data_reduced @ Vt_data[:k, :]
print(f"Approximated Data Shape: {data_approx.shape}")

# Calculate approximation error
error = np.mean((data - data_approx) ** 2)
print(f"\nApproximation MSE: {error:.4f}")

# Explained variance
total_variance = np.sum(S_data ** 2)
explained_variance = np.sum(S_data[:k] ** 2) / total_variance
print(f"Explained Variance ({k} components): {explained_variance*100:.1f}%")
```

### Healthcare Application: Patient Data Analysis

Using linear algebra for analyzing patient data and finding patterns.

```python
import numpy as np

# Patient data analysis using PCA (Principal Component Analysis)
# This is based on eigenvalue decomposition of covariance matrix

np.random.seed(42)

# Generate synthetic patient data
# Features: [Age, BMI, Blood Pressure, Heart Rate, Cholesterol]
n_patients = 100

ages = np.random.normal(50, 15, n_patients)
bmi = np.random.normal(25, 5, n_patients)
bp = np.random.normal(120, 20, n_patients)
heart_rate = np.random.normal(70, 10, n_patients)
cholesterol = np.random.normal(200, 40, n_patients)

# Create data matrix
patient_data = np.column_stack([ages, bmi, bp, heart_rate, cholesterol])
feature_names = ['Age', 'BMI', 'BP', 'Heart Rate', 'Cholesterol']

print("=" * 60)
print("PATIENT DATA PCA ANALYSIS")
print("=" * 60)

print("\nOriginal Data (first 5 patients):")
print(patient_data[:5])

# Center the data
data_mean = np.mean(patient_data, axis=0)
centered_data = patient_data - data_mean

print(f"\nMean of each feature:")
for name, mean in zip(feature_names, data_mean):
    print(f"  {name}: {mean:.2f}")

# Compute covariance matrix
cov_matrix = np.cov(patient_data, rowvar=False)

print("\nCovariance Matrix:")
print(cov_matrix)

# Perform eigenvalue decomposition on covariance matrix
eigenvalues, eigenvectors = np.linalg.eig(cov_matrix)

# Sort by eigenvalues (descending)
idx = np.argsort(eigenvalues)[::-1]
eigenvalues = eigenvalues[idx]
eigenvectors = eigenvectors[:, idx]

print("\nEigenvalues (sorted):")
print(eigenvalues)

print("\nPrincipal Component Loadings:")
for i, (ev, vec) in enumerate(zip(eigenvalues, eigenvectors.T)):
    print(f"\nPC{i+1} (explains {ev/np.sum(eigenvalues)*100:.1f}% variance):")
    for name, loading in zip(feature_names, vec):
        print(f"  {name}: {loading:.3f}")

# Transform data to principal components
pc_data = centered_data @ eigenvectors

print("\nTransformed Data (first 5 patients in PC space):")
print(np.round(pc_data[:5], 3))

# Dimensionality reduction: keep top 2 components
reduced_data = pc_data[:, :2]
print(f"\nReduced Data Shape: {reduced_data.shape}")

# Reconstruct from reduced dimensions
reconstructed = reduced_data @ eigenvectors[:, :2].T + data_mean
print(f"\nReconstructed Data Shape: {reconstructed.shape}")

# Reconstruction error
error = np.mean((patient_data - reconstructed) ** 2)
print(f"\nReconstruction MSE: {error:.4f}")
```

### Banking Application: Credit Risk Modeling

Linear algebra is used in credit risk modeling for scorecard development and loss estimation.

```python
import numpy as np

# Credit risk scoring model using logistic regression (linear model)
# This is a simplified version

np.random.seed(42)

# Generate synthetic credit data
n_customers = 1000

# Features
income = np.random.normal(50000, 20000, n_customers)
credit_score = np.random.normal(650, 100, n_customers)
debt = np.random.normal(10000, 5000, n_customers)
num_accounts = np.random.randint(1, 10, n_customers)

# Combine features
X = np.column_stack([income, credit_score, debt, num_accounts])

# Add bias term
X = np.column_stack([np.ones(n_customers), X])

# Define labels (simulated default indicators)
# Lower income + high debt + low credit score -> higher default probability
default_prob = (
    0.03 + 
    0.00001 * debt - 
    0.0001 * income +
    0.001 * (700 - credit_score)
)
default_prob = np.clip(default_prob, 0, 1)

# Generate defaults
defaults = (np.random.rand(n_customers) < default_prob).astype(int)

print("=" * 60)
print("CREDIT RISK SCORE MODEL")
print("=" * 60)

print(f"\nDataset Size: {n_customers} customers")
print(f"Defaults: {np.sum(defaults)} ({np.mean(defaults)*100:.1f}%)")

print(f"\nFeature Statistics:")
print(f"  Income: ${np.mean(income):,.0f} ± ${np.std(income):,.0f}")
print(f"  Credit Score: {np.mean(credit_score):.0f} ± {np.std(credit_score):.0f}")
print(f"  Debt: ${np.mean(debt):,.0f} ± ${np.std(debt):,.0f}")

# Use linear regression to predict default probability
# Using normal equation: w = (X.T @ X)^(-1) @ X.T @ y
X_T = X.T
y = defaults.astype(float)

# Add small regularization
lambda_reg = 0.01
XTX_lambda = X_T @ X + lambda_reg * np.eye(X.shape[1])

weights = np.linalg.solve(XTX_lambda, X_T @ y)

print(f"\nModel Coefficients:")
feature_names = ['Bias', 'Income', 'Credit Score', 'Debt', 'Num Accounts']
for name, w in zip(feature_names, weights):
    print(f"  {name}: {w:.6f}")

# Predict on training data
predictions = X @ weights
predictions = np.clip(predictions, 0, 1)

# Calculate model accuracy (using 0.5 threshold)
predicted_defaults = (predictions > 0.5).astype(int)
accuracy = np.mean(predicted_defaults == defaults)

print(f"\nModel Accuracy: {accuracy*100:.1f}%")

# Calculate risk scores
risk_scores = predictions * 1000  # Scale to 0-1000

print(f"\nRisk Score Distribution:")
print(f"  Mean: {np.mean(risk_scores):.1f}")
print(f"  Std: {np.std(risk_scores):.1f}")
print(f"  Min: {np.min(risk_scores):.1f}")
print(f"  Max: {np.max(risk_scores):.1f}")

# Risk categories
high_risk = np.sum(risk_scores > 500)
medium_risk = np.sum((risk_scores > 200) & (risk_scores <= 500))
low_risk = np.sum(risk_scores <= 200)

print(f"\nRisk Categories:")
print(f"  High Risk (>500): {high_risk} ({high_risk/n_customers*100:.1f}%)")
print(f"  Medium Risk (200-500): {medium_risk} ({medium_risk/n_customers*100:.1f}%)")
print(f"  Low Risk (<200): {low_risk} ({low_risk/n_customers*100:.1f}%)")
```

## Output Results

### Formatted Output for Linear Algebra Results

```python
import numpy as np

# Format matrix output for reports
A = np.array([
    [1.234, 2.456, 3.789],
    [4.123, 5.678, 6.901],
    [7.234, 8.567, 9.123]
])

print("=" * 60)
print("MATRIX OUTPUT FORMATTING")
print("=" * 60)

print("\nDefault:")
print(A)

print("\nFormatted (2 decimals):")
np.set_printoptions(precision=2, suppress=True)
print(A)
np.set_printoptions(precision=8, suppress=False)

# Create readable matrix output
def matrix_to_string(matrix, format_func=lambda x: f"{x:>8.2f}"):
    """Convert matrix to formatted string"""
    lines = []
    for row in matrix:
        line = " ".join(format_func(x) for x in row)
        lines.append(line)
    return "\n".join(lines)

print("\nCustom formatting:")
print(matrix_to_string(A))

# Format for financial report
balances = np.array([
    [1000.00, 2500.50, 3000.75],
    [1500.25, 3500.00, 4500.50],
    [2000.00, 4000.25, 5000.00]
])

print("\nAccount Balances:")
header = "Account 1    Account 2    Account 3"
print(header)
print("-" * len(header))
for row in balances:
    print(" ".join(f"${x:>9,.2f}" for x in row))
```

## Visualization

### ASCII Matrix Visualizations

```python
import numpy as np

# Visualize matrix as heatmap
def matrix_to_heatmap(matrix, symbols=' .:-=+*#@'):
    """Convert matrix to ASCII heatmap"""
    min_val = np.min(matrix)
    max_val = np.max(matrix)
    
    if max_val == min_val:
        return "Matrix has constant values"
    
    result = []
    for row in matrix:
        line = ""
        for val in row:
            normalized = (val - min_val) / (max_val - min_val)
            symbol_index = int(normalized * (len(symbols) - 1))
            line += symbols[symbol_index] + " "
        result.append(line)
    return "\n".join(result)

# Create sample matrices
A = np.array([
    [1, 2, 3, 4],
    [2, 4, 6, 8],
    [3, 6, 9, 12],
    [4, 8, 12, 16]
])

print("=" * 60)
print("MATRIX HEATMAP VISUALIZATION")
print("=" * 60)

print("\nMatrix:")
print(A)

print("\nASCII Heatmap:")
print(matrix_to_heatmap(A))

# Identity matrix visualization
identity = np.eye(5)
print("\nIdentity Matrix (5x5):")
print(matrix_to_heatmap(identity))

# Eigenvalue visualization
def eigenvalues_bar(eigenvalues):
    """Create ASCII bar chart for eigenvalues"""
    max_eigen = np.max(np.abs(eigenvalues))
    max_len = 20
    
    print("\nEigenvalue Magnitudes:")
    for i, ev in enumerate(eigenvalues):
        length = int(np.abs(ev) / max_eigen * max_len)
        bar = "█" * length
        print(f"λ{i+1}: {bar} ({ev:.2f})")

# Sample eigenvalues
sample_eigenvalues = np.array([5.0, 3.0, 1.0, 0.5, 0.1])
eigenvalues_bar(sample_eigenvalues)
```

## Advanced Topics

### Advanced Matrix Operations

```python
import numpy as np

# Kronecker product
A = np.array([[1, 2], [3, 4]])
B = np.array([[0, 5], [6, 7]])

print("=" * 60)
print("ADVANCED MATRIX OPERATIONS")
print("=" * 60)

print("\nMatrix A:")
print(A)
print("\nMatrix B:")
print(B)

# Kronecker product: A ⊗ B
kron = np.kron(A, B)
print("\nKronecker Product (A ⊗ B):")
print(kron)

# Khatri-Rao product (column-wise Kronecker)
A = np.array([[1, 2], [3, 4]])
B = np.array([[5, 6], [7, 8]])

khatri = np.kron(A, B)
print("\nKhatri-Rao Product:")
print(khatri)

# Hadamard product (element-wise)
H = A * B
print("\nHadamard Product (A * B):")
print(H)

# Vectorization and reshaping
matrix = np.array([
    [1, 2, 3],
    [4, 5, 6]
])

# Vectorize (flatten column-major)
vec = matrix.flatten(order='F')
print("\nVectorized (column-major):", vec)

# Reshape back
reshaped = vec.reshape((2, 3), order='F')
print("\nReshaped:")
print(reshaped)

# Trace and determinant
square = np.array([
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
])

trace = np.trace(square)
det = np.linalg.det(square)

print(f"\nTrace: {trace}")
print(f"Determinant: {det}")
```

### Performance Optimization

```python
import numpy as np
import time

# Compare different matrix multiplication methods
np.random.seed(42)
size = 500

A = np.random.rand(size, size)
B = np.random.rand(size, size)

print("=" * 60)
print("MATRIX MULTIPLICATION PERFORMANCE")
print("=" * 60)

# Method 1: np.dot
start = time.time()
C1 = np.dot(A, B)
t1 = time.time() - start
print(f"np.dot: {t1*1000:.2f} ms")

# Method 2: @ operator
start = time.time()
C2 = A @ B
t2 = time.time() - start
print(f"@ operator: {t2*1000:.2f} ms")

# Method 3: np.matmul
start = time.time()
C3 = np.matmul(A, B)
t3 = time.time() - start
print(f"np.matmul: {t3*1000:.2f} ms")

# Method 4: Using einsum (for specific patterns)
# This is particularly efficient for certain operations
start = time.time()
C4 = np.einsum('ij,jk->ik', A, B)
t4 = time.time() - start
print(f"np.einsum: {t4*1000:.2f} ms")

# Verify correctness
print(f"\nResults match: {np.allclose(C1, C2) and np.allclose(C2, C3)}")

# Block matrix multiplication for memory efficiency
def blocked_matmul(A, B, block_size=128):
    """Block matrix multiplication"""
    m, n = A.shape
    n_, p = B.shape
    
    # Handle non-square matrices
    if n != n_:
        raise ValueError("Matrix dimensions don't match")
    
    result = np.zeros((m, p))
    
    for i in range(0, m, block_size):
        for j in range(0, p, block_size):
            for k in range(0, n, block_size):
                i_end = min(i + block_size, m)
                j_end = min(j + block_size, p)
                k_end = min(k + block_size, n)
                
                result[i:i_end, j:j_end] += A[i:i_end, k:k_end] @ B[k:k_end, j:j_end]
    
    return result

print("\nBlocked Matrix Multiplication:")
start = time.time()
C_blocked = blocked_matmul(A[:100, :100], B[:100, :100])
t_blocked = time.time() - start
print(f"Blocked (100x100, block=128): {t_blocked*1000:.2f} ms")
```

## Conclusion

Linear algebra with NumPy provides powerful capabilities for solving complex mathematical problems in data science, finance, and healthcare. Through this comprehensive exploration, you've learned about matrix operations, solving linear systems, eigenvalue and singular value decompositions, and their practical applications in portfolio optimization and medical imaging.

The banking examples demonstrated portfolio optimization using mean-variance theory, risk parity calculations, and credit risk modeling. The healthcare applications showed medical image reconstruction, patient data analysis using PCA, and image enhancement techniques. These applications leverage NumPy's optimized linear algebra functions for efficient computation.

Key takeaways include understanding matrix operations as foundation for machine learning, using eigenvalue decomposition for dimensionality reduction, and applying SVD for data compression and pattern recognition. Continue practicing with real datasets to strengthen your understanding of these linear algebra operations and their applications in data science.