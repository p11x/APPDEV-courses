# 🚀 NumPy Introduction

## 🎯 What You'll Learn

- What NumPy is and why it's essential for data science
- How to create NumPy arrays from scratch
- Understanding array attributes (shape, dtype, ndim, size)
- The difference between Python lists and NumPy arrays

## 📦 Prerequisites

- Complete the Python basics from Folders 01-04
- Know how to work with Python lists

## Why NumPy?

Imagine trying to do math with an abacus versus a calculator. That's exactly the difference between **Python lists** and **NumPy arrays**! NumPy (Numerical Python) is the foundation of virtually every data science and machine learning operation in Python.

### The Speed Difference

NumPy operations are **10-100x faster** than Python lists because:

1. **Contiguous Memory**: Arrays store data in continuous blocks
2. **Vectorization**: Operations apply to entire arrays at once
3. **C Implementation**: Most NumPy code runs in optimized C

### Installing NumPy

```python
pip install numpy  # Install NumPy package
```

### Importing NumPy

```python
import numpy as np  # Convention: import as 'np' for brevity
```

## Creating NumPy Arrays

### From a Python List

```python
# Create a 1D array from a list
numbers: list[int] = [1, 2, 3, 4, 5]
arr: np.ndarray = np.array(numbers)  # Convert list to NumPy array

print(arr)        # Output: [1 2 3 4 5]
print(type(arr))  # Output: <class 'numpy.ndarray'>
```

### 💡 Line-by-Line Breakdown

- `numbers: list[int] = [1, 2, 3, 4, 5]` - A regular Python list with type hint
- `np.array(numbers)` - Convert the list into a NumPy array
- `type(arr)` - Shows we're working with `numpy.ndarray` (n-dimensional array)

## Array Attributes

Every NumPy array has useful properties:

```python
# Create a 2D array (3 rows, 4 columns)
matrix: np.ndarray = np.array([
    [1, 2, 3, 4],
    [5, 6, 7, 8],
    [9, 10, 11, 12]
])

print(matrix.shape)    # (3, 4) - 3 rows, 4 columns
print(matrix.dtype)    # int64 - data type
print(matrix.ndim)     # 2 - number of dimensions
print(matrix.size)     # 12 - total elements (3 × 4)
```

### 💡 Line-by-Line Breakdown

- `matrix.shape` - Returns a tuple showing (rows, columns)
- `matrix.dtype` - Shows the data type (int64, float64, etc.)
- `matrix.ndim` - Number of dimensions (1D=1, 2D=2, etc.)
- `matrix.size` - Total count of elements

## Visual: Array Shapes

```
1D Array (shape: 5,)
┌─────────────────────────────────────┐
│  [1] [2] [3] [4] [5]                │
└─────────────────────────────────────┘

2D Array (shape: 3, 4)
┌─────────────────────────────────────┐
│  [1]  [2]  [3]  [4]                │
│  [5]  [6]  [7]  [8]                │
│  [9] [10] [11] [12]                │
└─────────────────────────────────────┘

3D Array (shape: 2, 3, 4)
┌──────────────────┐  ┌──────────────────┐
│ 1  2  3  4       │  │ 1  2  3  4       │
│ 5  6  7  8       │  │ 5  6  7  8       │
│ 9 10 11 12       │  │ 9 10 11 12       │
└──────────────────┘  └──────────────────┘
   Layer 0               Layer 1
```

## Quick Array Creation Functions

NumPy provides fast ways to create common arrays:

```python
# All zeros - useful for initialization
zeros: np.ndarray = np.zeros((3, 4))  # 3 rows, 4 columns

# All ones
ones: np.ndarray = np.ones((2, 3))

# Fill with a specific value
full: np.ndarray = np.full((2, 2), 7)  # 2×2 matrix filled with 7

# Identity matrix (square, 1s on diagonal)
identity: np.ndarray = np.eye(4)  # 4×4 identity matrix

# Sequence: start, stop, step
range_arr: np.ndarray = np.arange(0, 10, 2)  # [0, 2, 4, 6, 8]

# Evenly spaced: start, stop, number of points
linspace_arr: np.ndarray = np.linspace(0, 1, 5)  # [0, 0.25, 0.5, 0.75, 1]
```

### 💡 Line-by-Line Breakdown

- `np.zeros((3, 4))` - Create array of zeros with shape (3,4)
- `np.full((2, 2), 7)` - Fill with constant value 7
- `np.eye(4)` - Identity matrix - crucial for linear algebra
- `np.arange(0, 10, 2)` - Like Python's range() but returns array
- `np.linspace(0, 1, 5)` - 5 evenly spaced points from 0 to 1

## ✅ Summary

- NumPy is the backbone of data science in Python — fast, efficient, powerful
- Arrays are created with `np.array()`, or use convenience functions like `zeros()`, `ones()`, `arange()`
- Every array has key attributes: `shape`, `dtype`, `ndim`, `size`
- NumPy uses less memory and runs faster than Python lists for numerical operations

## ➡️ Next Steps

Ready to do math on entire arrays at once? Head to **[02_array_operations.md](./02_array_operations.md)** to learn about vectorized operations!

## 🔗 Further Reading

- [NumPy Official Documentation](https://numpy.org/doc/stable/)
- [NumPy Quickstart Tutorial](https://numpy.org/doc/stable/user/quickstart.html)
- [Why NumPy is Fast](https://numpy.org/doc/stable/dev/internals.html)
