# 🔢 Array Operations

## 🎯 What You'll Learn

- Vectorized operations — do math on entire arrays at once
- Broadcasting — how NumPy handles arrays of different shapes
- Universal functions (ufuncs) for mathematical operations
- Aggregation methods for summarizing data

## 📦 Prerequisites

- Read [01_numpy_intro.md](./01_numpy_intro.md) first

## The Magic of Vectorization

This is where NumPy really shines! Instead of looping through elements one by one, you can apply operations to **entire arrays instantly**.

### ❌ AVOID: Using Python Loops

```python
# SLOW: Loop through each element
numbers: list[int] = [1, 2, 3, 4, 5]
doubled: list[int] = []

for n in numbers:
    doubled.append(n * 2)

print(doubled)  # [2, 4, 6, 8, 10]
```

### ✅ GOOD: Using NumPy Vectorization

```python
# FAST: Vectorized operation on entire array
import numpy as np

numbers: np.ndarray = np.array([1, 2, 3, 4, 5])
doubled: np.ndarray = numbers * 2  # Multiply EVERY element by 2

print(doubled)  # [ 2  4  6  8 10]
```

### 💡 Explanation

In the vectorized version, we don't need a loop at all. NumPy multiplies each element by 2 simultaneously using optimized C code. This is **10-100x faster** for large arrays!

## Element-wise Arithmetic

All basic math operations work element-by-element:

```python
import numpy as np

a: np.ndarray = np.array([1, 2, 3])
b: np.ndarray = np.array([4, 5, 6])

# Addition
print(a + b)      # [5 7 9]

# Subtraction
print(b - a)      # [3 3 3]

# Multiplication (element-wise, NOT matrix multiplication)
print(a * b)      # [4 10 18]

# Division
print(b / a)       # [4.  2.5 2.]

# Power
print(a ** 2)      # [1 4 9]

# Modulo
print(b % a)       # [0 1 0]
```

### 💡 Line-by-Line Breakdown

- `a + b` - Adds corresponding elements: 1+4=5, 2+5=7, 3+6=9
- `b - a` - Subtracts element-wise
- `a * b` - Multiplies element-wise (not matrix multiply!)
- `b / a` - Divides element-wise, returns floats

## Broadcasting

This is one of NumPy's most powerful features! When arrays have different shapes, NumPy automatically "stretches" the smaller one to match.

### Visual: Broadcasting Rules

```
Shape (3,) + Shape ()    →    Shape (3,)
    [1, 2, 3]           →       [1, 2, 3]
    +    10             →    +  [10, 10, 10]
    ────────────        →    ─────────────
    [11, 12, 13]        →    [11, 12, 13]

Shape (3, 3) + Shape (3,) → Shape (3, 3)
┌─────────────────┐
│ 1  2  3         │
│ 4  5  6    +    │ [10, 20, 30]
│ 7  8  9         │
└─────────────────┘
        ↓
┌─────────────────┐
│ 11  22  33     │
│ 14  25  36     │
│ 17  28  39     │
└─────────────────┘
```

### Practical Example

```python
import numpy as np

# Add a scalar to an array
prices: np.ndarray = np.array([10, 20, 30, 40])
tax: float = 1.08  # 8% tax

final_prices: np.ndarray = prices * tax
print(final_prices)  # [10.8 21.6 32.4 43.2]

# Multiply matrices with broadcasting
matrix: np.ndarray = np.array([[1, 2, 3], [4, 5, 6]])
multiplier: np.ndarray = np.array([10, 100, 1000])

result: np.ndarray = matrix * multiplier
# Row 1: [1×10, 2×100, 3×1000] = [10, 200, 3000]
# Row 2: [4×10, 5×100, 6×1000] = [40, 500, 6000]
```

### 💡 Explanation

Broadcasting lets you perform operations without explicitly expanding arrays. The smaller array is "broadcast" across the larger one. Just remember: dimensions must match from the **right side** (trailing dimensions).

## Universal Functions (ufuncs)

NumPy provides fast mathematical functions that operate on arrays:

```python
import numpy as np

numbers: np.ndarray = np.array([1, 4, 9, 16, 25])

# Square root
print(np.sqrt(numbers))    # [1. 2. 3. 4. 5.]

# Absolute value
mixed: np.ndarray = np.array([-3, -1, 0, 2, 5])
print(np.abs(mixed))       # [3 1 0 2 5]

# Exponential (e^x)
print(np.exp(np.array([0, 1, 2])))  # [1.         2.71828183 7.3890562 ]

# Natural log
print(np.log(np.array([1, np.e, np.e**2])))  # [0. 1. 2.]

# Trigonometry
angles: np.ndarray = np.array([0, np.pi/2, np.pi])
print(np.sin(angles))  # [0.0000000e+00 1.0000000e+00 1.2246468e-16]
```

### 💡 Line-by-Line Breakdown

- `np.sqrt()` - Element-wise square root
- `np.abs()` - Absolute value
- `np.exp()` - e raised to power of each element
- `np.log()` - Natural logarithm (base e)
- `np.sin()` - Trigonometric sine

## Aggregations

Summarize arrays with statistical operations:

```python
import numpy as np

data: np.ndarray = np.array([23, 45, 67, 12, 89, 34, 56, 78, 90, 11])

# Sum and mean
print(data.sum())   # 505  - total
print(data.mean())  # 50.5 - average

# Standard deviation and variance
print(data.std())   # 28.5 - how spread out
print(data.var())   # 812.5 - variance

# Min and max
print(data.min())   # 11 - smallest
print(data.max())   # 90 - largest

# Index of min/max
print(data.argmin())  # 9 - index of minimum (11)
print(data.argmax())  # 8 - index of maximum (90)
```

### 💡 Line-by-Line Breakdown

- `.sum()` - Add all elements together
- `.mean()` - Average of all elements
- `.std()` - Standard deviation (spread from mean)
- `.min()` / `.max()` - Smallest/largest value
- `.argmin()` / `.argmax()` - Positions of min/max

## The Axis Parameter

For 2D arrays, `axis` controls which direction to operate on:

```python
import numpy as np

matrix: np.ndarray = np.array([
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
])

# axis=0: operate DOWN each column
print(matrix.sum(axis=0))    # [12 15 18]  - sums each column

# axis=1: operate ACROSS each row
print(matrix.sum(axis=1))    # [6 15 24]   - sums each row

# Mean across rows
print(matrix.mean(axis=1))   # [2. 5. 8.]
```

### 💡 Line-by-Line Breakdown

- `axis=0` - Collapse rows, keep columns (vertical)
- `axis=1` - Collapse columns, keep rows (horizontal)

### Visual: Axis in Action

```
axis=0 (columns):
    Col 0    Col 1    Col 2
      ↓        ↓        ↓
    [ 1        2        3 ]   →  12
    [ 4        5        6 ]   →  15
    [ 7        8        9 ]   →  18

axis=1 (rows):
    Row 0 → [1+2+3] = 6
    Row 1 → [4+5+6] = 15
    Row 2 → [7+8+9] = 24
```

## ✅ Summary

- **Vectorization** is 10-100x faster than Python loops — always prefer it!
- **Broadcasting** automatically handles arrays of different shapes
- **ufuncs** like `np.sqrt()`, `np.exp()` operate on every element
- **Aggregations** (`.sum()`, `.mean()`, `.std()`) summarize entire arrays
- Use `axis=0` for columns, `axis=1` for rows in 2D arrays

## ➡️ Next Steps

Ready to select specific data from arrays? Head to **[03_indexing_and_slicing.md](./03_indexing_and_slicing.md)** to master array access!

## 🔗 Further Reading

- [NumPy Broadcasting](https://numpy.org/doc/stable/user/basics.broadcasting.html)
- [Universal Functions (ufuncs)](https://numpy.org/doc/stable/reference/ufuncs.html)
- [Array Manipulation Routines](https://numpy.org/doc/stable/reference/routines.array-manipulation.html)
