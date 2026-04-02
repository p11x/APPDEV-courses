# 🎲 Random Numbers & Linear Algebra

## 🎯 What You'll Learn

- Generate random numbers with the new Python 3.12+ RNG system
- Create random arrays for simulations
- Reproducibility with seeds
- Basic linear algebra operations
- The @ operator for matrix multiplication

## 📦 Prerequisites

- Read [03_indexing_and_slicing.md](./03_indexing_and_slicing.md) first

## Random Numbers in NumPy

Python 3.12+ uses the new `numpy.random` generator system. No more `numpy.random.rand()` — welcome to `numpy.random.default_rng()`!

### Creating a Random Generator

```python
import numpy as np

# Create a new random generator (recommended approach)
rng: np.random.Generator = np.random.default_rng()

# Generate random floats between 0 and 1
uniform: np.ndarray = rng.random(5)  # 5 random numbers
print(uniform)  # e.g., [0.234 0.456 0.789 0.123 0.567]
```

### 💡 Line-by-Line Breakdown

- `np.random.default_rng()` - Create a new random number generator
- `rng.random(5)` - Generate 5 random floats in [0, 1)

### Different Random Distributions

```python
import numpy as np

rng: np.random.Generator = np.random.default_rng()

# Random integers: low, high (exclusive), size
dice_rolls: np.ndarray = rng.integers(1, 7, size=10)  # 10 dice rolls
print(dice_rolls)  # e.g., [3 5 1 6 2 4 6 1 3 5]

# Random from normal (Gaussian) distribution
heights: np.ndarray = rng.normal(loc=170, scale=10, size=1000)
# loc=mean, scale=std_dev

# Random choice from array
choices: np.ndarray = rng.choice(["red", "green", "blue"], size=5)
# Pick 5 colors with replacement

# Shuffle an array in-place
deck: np.ndarray = np.arange(1, 53)  # Cards 1-52
rng.shuffle(deck)  # Shuffle the deck!
print(deck[:5])  # First 5 cards after shuffle
```

### 💡 Line-by-Line Breakdown

- `rng.integers(1, 7, size=10)` - 10 random integers from 1-6
- `rng.normal(loc=170, scale=10)` - Normal distribution, mean=170, std=10
- `rng.choice()` - Randomly pick from given options
- `rng.shuffle()` - Randomly reorder array in-place

## Seeds: Reproducibility

Want the same "random" numbers every time? Use a seed!

```python
import numpy as np

rng1: np.random.Generator = np.random.default_rng(42)
rng2: np.random.Generator = np.random.default_rng(42)

# Same seed = same random numbers!
print(rng1.random(3))  # [0.773  0.439  0.638]
print(rng2.random(3))  # [0.773  0.439  0.638]  - IDENTICAL!

# Different seed = different numbers
rng3: np.random.Generator = np.random.default_rng(123)
print(rng3.random(3))   # [0.226 0.712 0.097] - Different!
```

### 💡 Explanation

Think of a seed as a "starting point" for the random number generator. Same seed = same sequence. This is **crucial** for:
- Debugging machine learning models
- Reproducing scientific experiments
- Sharing code that needs consistent results

## Mini Project: Dice Simulation

```python
import numpy as np

# Create RNG with seed for reproducibility
rng: np.random.Generator = np.random.default_rng(42)

# Simulate 10,000 dice rolls
rolls: np.ndarray = rng.integers(1, 7, size=10000)

# Count each outcome
unique, counts = np.unique(rolls, return_counts=True)
for value, count in zip(unique, counts):
    bar = "█" * (count // 100)  # Scale for display
    print(f"{value}: {count:5d} {bar}")

# Calculate statistics
print(f"\nMean: {rolls.mean():.2f}")  # ~3.5
print(f"Std:  {rolls.std():.2f}")   # ~1.71
```

### Expected Output

```
1:  1719 ███████████████████
2:  1658 ██████████████████
3:  1684 ██████████████████
4:  1650 ██████████████████
5:  1638 ██████████████████
6:  1651 ██████████████████

Mean: 3.50
Std:  1.71
```

Notice how each number appears roughly equally (law of large numbers)! The theoretical mean is 3.5.

## Linear Algebra

NumPy makes matrix operations intuitive and fast!

### Matrix Creation

```python
import numpy as np

# 2×3 matrix
A: np.ndarray = np.array([
    [1, 2, 3],
    [4, 5, 6]
])

# 3×2 matrix
B: np.ndarray = np.array([
    [7, 8],
    [9, 10],
    [11, 12]
])

# Identity matrix (square, 1s on diagonal)
I3: np.ndarray = np.eye(3)  # 3×3 identity

# Zeros matrix
zeros: np.ndarray = np.zeros((2, 4))  # 2×4
```

### Matrix Multiplication

NumPy makes this easy with two approaches:

```python
import numpy as np

A: np.ndarray = np.array([[1, 2], [3, 4]])
B: np.ndarray = np.array([[5, 6], [7, 8]])

# Method 1: @ operator (Python 3.5+, RECOMMENDED!)
result1: np.ndarray = A @ B

# Method 2: np.matmul() function
result2: np.ndarray = np.matmul(A, B)

# Method 3: np.dot() (works but confusing for matrices)
result3: np.ndarray = np.dot(A, B)

print(result1)
# [[19 22]
#  [43 50]]
```

### 💡 Line-by-Line Breakdown

- `A @ B` - Clean, readable matrix multiplication (Python 3.5+)
- `np.matmul()` - Explicit function for matrices
- Both A and B must be compatible: A is (2,2), B is (2,2) → result is (2,2)

### Visual: Matrix Multiplication

```
A @ B = C
┌───────────┐   ┌───────────┐   ┌───────────┐
│ 1  2      │ × │ 5  6      │ = │ 1×5+2×7  1×6+2×8 │
│           │   │           │   │           │
│ 3  4      │   │ 7  8      │   │ 3×5+4×7  3×6+4×8 │
└───────────┘   └───────────┘   └───────────┘
                              =  [[19 22]
                                  [43 50]]
```

### Dot Product of Vectors

```python
import numpy as np

v1: np.ndarray = np.array([1, 2, 3])
v2: np.ndarray = np.array([4, 5, 6])

# Dot product: sum of element-wise products
dot: np.ndarray = np.dot(v1, v2)  # 1×4 + 2×5 + 3×6 = 32

# Or use @ with 1D arrays
dot2: np.ndarray = v1 @ v2  # 32
```

## Linear Algebra Functions

```python
import numpy as np

A: np.ndarray = np.array([
    [3, 1],
    [1, 2]
])

# Matrix norm (magnitude)
norm: float = np.linalg.norm(A)
print(f"Norm: {norm:.2f}")  # 3.87

# Matrix inverse
inv: np.ndarray = np.linalg.inv(A)
print(inv)
# [[ 0.5  -0.25]
#  [-0.25  0.75]]

# Verify: A @ A⁻¹ should equal I
identity: np.ndarray = A @ inv
print(identity.round(10))  # [[1. 0.] [0. 1.]]

# Eigenvalues and eigenvectors
eigenvalues: np.ndarray
eigenvectors: np.ndarray
eigenvalues, eigenvectors = np.linalg.eig(A)
print(f"Eigenvalues: {eigenvalues}")  # [3.618 1.382]
```

### 💡 Line-by-Line Breakdown

- `np.linalg.norm()` - Frobenius norm (size of matrix)
- `np.linalg.inv()` - Matrix inverse (A @ A⁻¹ = I)
- `np.linalg.eig()` - Eigenvalues and eigenvectors

## ✅ Summary

- Use `np.random.default_rng(seed)` for reproducible random numbers
- `rng.random()`, `rng.integers()`, `rng.normal()`, `rng.choice()` - common functions
- The `@` operator is the cleanest way to do matrix multiplication
- `np.dot()`, `np.matmul()` - alternative matrix multiply functions
- `np.linalg` module provides: norm, inv, eig, and more

## ➡️ Next Steps

Ready to work with tabular data? Head to **[../02_Pandas/01_dataframes_and_series.md](../02_Pandas/01_dataframes_and_series.md)** to learn Pandas!

## 🔗 Further Reading

- [Random Sampling](https://numpy.org/doc/stable/reference/random/generator.html)
- [Linear Algebra](https://numpy.org/doc/stable/reference/routines.linalg.html)
- [Matrix Multiplication Explained](https://mathinsight.org/matrix_multiplication)
