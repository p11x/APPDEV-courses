# 🎯 Indexing and Slicing NumPy Arrays

## 🎯 What You'll Learn

- Basic indexing: accessing single elements
- Slicing: extracting sub-arrays
- Boolean masking: filter arrays with conditions
- Fancy indexing: select specific indices
- Reshaping arrays

## 📦 Prerequisites

- Read [02_array_operations.md](./02_array_operations.md) first

## Basic Indexing

Just like Python lists, but with superpowers for multi-dimensional arrays!

### 1D Array Indexing

```python
import numpy as np

arr: np.ndarray = np.array([10, 20, 30, 40, 50])

print(arr[0])   # 10 - first element (0-indexed!)
print(arr[4])   # 50 - fifth element
print(arr[-1])  # 50 - last element
print(arr[-2])  # 40 - second-to-last
```

### 2D Array Indexing

```python
import numpy as np

matrix: np.ndarray = np.array([
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
])

print(matrix[0, 0])   # 1 - row 0, col 0 (top-left)
print(matrix[1, 2])   # 6 - row 1, col 2
print(matrix[2, 0])   # 7 - row 2, col 0 (bottom-left)
print(matrix[-1, -1])  # 9 - bottom-right corner
```

### 💡 Line-by-Line Breakdown

- `matrix[0, 0]` - Access row 0, column 0 (comma separates dimensions!)
- `matrix[1, 2]` - Second row, third column
- `matrix[-1, -1]` - Negative indices count from the end

## Slicing: Extracting Parts of Arrays

Slicing uses `[start:stop:step]` syntax — same as Python lists!

### 1D Slicing

```python
import numpy as np

arr: np.ndarray = np.array([0, 10, 20, 30, 40, 50, 60, 70, 80, 90])

print(arr[2:7])     # [20 30 40 50 60] - indices 2 to 6
print(arr[:5])     # [0 10 20 30 40] - start to index 4
print(arr[5:])     # [50 60 70 80 90] - index 5 to end
print(arr[::2])    # [0 20 40 60 80] - every other element
print(arr[::-1])   # [90 80 70 60 50 40 30 20 10 0] - reversed!
```

### 2D Slicing

```python
import numpy as np

matrix: np.ndarray = np.array([
    [1, 2, 3, 4],
    [5, 6, 7, 8],
    [9, 10, 11, 12],
    [13, 14, 15, 16]
])

# Get first two rows
print(matrix[:2])   
# [[1 2 3 4]
#  [5 6 7 8]]

# Get last two columns
print(matrix[:, 2:])
# [[ 3  4]
#  [ 7  8]
#  [11 12]
#  [15 16]]

# Get a submatrix (rows 1-2, cols 1-2)
print(matrix[1:3, 1:3])
# [[ 6  7]
#  [10 11]]
```

### 💡 Line-by-Line Breakdown

- `matrix[:2]` - All columns, first 2 rows
- `matrix[:, 2:]` - All rows, columns from index 2 onward
- `matrix[1:3, 1:3]` - Rows 1-2, Columns 1-2 (a 2×2 submatrix)

### Visual: 2D Slicing

```
Original 4×4 Matrix:
┌─────────────────┐
│ 1  2  3  4     │  ← row 0
│ 5  6  7  8     │  ← row 1
│ 9 10 11 12     │  ← row 2
│13 14 15 16     │  ← row 3
└─────────────────┘
   ↑  ↑  ↑  ↑
 col0 col1 col2 col3

matrix[1:3, 1:3] extracts:
┌───────────┐
│ 6  7     │  ← row 1, cols 1-2
│10 11     │  ← row 2, cols 1-2
└───────────┘
```

## Boolean Masking

This is where it gets really powerful! Filter arrays using conditions:

```python
import numpy as np

temperatures: np.ndarray = np.array([22, 25, 18, 31, 29, 15, 27, 33, 20, 28])

# Find days above 30°C
hot_days: np.ndarray = temperatures[temperatures > 30]
print(hot_days)  # [31 33] - days that are too hot!

# Find days between 20 and 28 degrees
pleasant: np.ndarray = temperatures[(temperatures >= 20) & (temperatures <= 28)]
print(pleasant)  # [22 25 27 20 28]

# Use boolean mask to count
print((temperatures > 25).sum())  # 5 - number of warm days
```

### 💡 Line-by-Line Breakdown

- `temperatures[temperatures > 30]` - Only keep elements where temperature > 30
- `(temperatures >= 20) & (temperatures <= 28)` - AND two conditions
- `(temperatures > 25).sum()` - Count True values (warm days)

### 💡 Explanation: Boolean Masking

Think of boolean masking like a filter:
```
Original:   [22, 25, 18, 31, 29, 15, 27, 33, 20, 28]
Mask (>25): [F,  F,  F,  T,  T,  F,  T,  T,  F,  T]
Filtered:        ↑        ↑     ↑     ↑        ↑
              [31, 29, 27, 33, 28]
```

## np.where: Conditional Selection

Replace values based on conditions:

```python
import numpy as np

scores: np.ndarray = np.array([45, 82, 67, 91, 55, 78, 33, 89])

# Pass (>=60) or Fail (<60)
result: np.ndarray = np.where(scores >= 60, "Pass", "Fail")
print(result)  # ['Fail' 'Pass' 'Pass' 'Pass' 'Fail' 'Pass' 'Fail' 'Pass']

# Replace outliers with the mean
data: np.ndarray = np.array([10, 20, 30, 1000, 40, 50])
mean_val: float = data[data < 100].mean()  # 30
cleaned: np.ndarray = np.where(data > 100, mean_val, data)
print(cleaned)  # [10 20 30 30 40 50]
```

### 💡 Line-by-Line Breakdown

- `np.where(condition, if_true, if_false)` - Replace based on condition
- `data[data < 100].mean()` - Calculate mean excluding outliers

## Fancy Indexing

Select specific indices at once:

```python
import numpy as np

arr: np.ndarray = np.array([10, 20, 30, 40, 50, 60, 70, 80])

# Get elements at specific positions
print(arr[[0, 2, 4]])    # [10 30 50] - 1st, 3rd, 5th elements
print(arr[[-1, -2, -3]]) # [80 70 60] - last three in reverse

# With 2D arrays
matrix: np.ndarray = np.arange(12).reshape(3, 4)
# [[ 0  1  2  3]
#  [ 4  5  6  7]
#  [ 8  9 10 11]]

print(matrix[[0, 2], :])  # Rows 0 and 2, all columns
# [[ 0  1  2  3]
#  [ 8  9 10 11]]
```

### 💡 Line-by-Line Breakdown

- `arr[[0, 2, 4]]` - Use a list of indices to select multiple elements
- `matrix[[0, 2], :]` - Fancy index rows 0 and 2, get all columns

## Reshaping Arrays

Change array dimensions without copying data:

```python
import numpy as np

# Create a 1D array and reshape to 2D
arr: np.ndarray = np.arange(12)  # [0 1 2 ... 11]
print(arr.reshape(3, 4))
# [[ 0  1  2  3]
#  [ 4  5  6  7]
#  [ 8  9 10 11]]

# Flatten: 2D → 1D
matrix: np.ndarray = np.array([[1, 2], [3, 4]])
flat: np.ndarray = matrix.flatten()  # [1 2 3 4]
print(flat)

# ravel: same as flatten but may return a view (more memory efficient)
view: np.ndarray = matrix.ravel()

# Add a new axis
arr: np.ndarray = np.array([1, 2, 3])
row_vec: np.ndarray = arr[np.newaxis, :]  # [[1 2 3]] - row vector
col_vec: np.ndarray = arr[:, np.newaxis]  # [[1] [2] [3]] - column vector

# np.newaxis is same as None
row_vec2: np.ndarray = arr[None, :]       # [[1 2 3]]
col_vec2: np.ndarray = arr[:, None]        # [[1] [2] [3]]
```

### 💡 Line-by-Line Breakdown

- `.reshape(3, 4)` - Reshape to 3 rows, 4 columns (must match total size!)
- `.flatten()` - Convert to 1D array (makes a copy)
- `.ravel()` - Same as flatten but might return a view (faster)
- `np.newaxis` or `None` - Add a new dimension

## Mini Project: Find Hot Summer Days

```python
import numpy as np

# Simulate July temperatures (in Celsius)
july_temps: np.ndarray = np.array([
    [28, 31, 33, 35, 32, 29],  # Week 1
    [30, 34, 36, 38, 35, 31],  # Week 2
    [29, 32, 34, 33, 31, 28],  # Week 3
    [27, 30, 31, 29, 32, 30]   # Week 4
])

# Find all days above 30°C
hot_mask: np.ndarray = july_temps > 30
hot_days: np.ndarray = july_temps[hot_mask]
print(f"Hot days (>30°C): {hot_days}")
print(f"Number of hot days: {hot_days.size}")

# Find coordinates of hottest days
hottest_indices: tuple = np.where(july_temps == july_temps.max())
print(f"Hottest temperature at: week {hottest_indices[0][0]+1}, day {hottest_indices[1][0]+1}")
print(f"Maximum temperature: {july_temps.max()}°C")
```

### Expected Output

```
Hot days (>30°C): [31 33 35 32 30 34 36 38 35 31 32 34 33 31 32]
Number of hot days: 15
Hottest temperature at: week 2, day 4
Maximum temperature: 38°C
```

## ✅ Summary

- Basic indexing: `arr[0]`, `arr[2, 3]` for 2D
- Slicing: `arr[start:stop:step]`, `matrix[:, 1:]`
- **Boolean masking**: `arr[arr > 5]` filters by condition
- **Fancy indexing**: `arr[[0, 2, 4]]` selects specific indices
- **Reshaping**: `.reshape()`, `.flatten()`, `np.newaxis`

## ➡️ Next Steps

Ready to generate random numbers and do linear algebra? Head to **[04_numpy_random_and_linalg.md](./04_numpy_random_and_linalg.md)**!

## 🔗 Further Reading

- [Indexing Documentation](https://numpy.org/doc/stable/user/basics.indexing.html)
- [Boolean Indexing](https://numpy.org/doc/stable/reference/routines.indexing.html)
- [Fancy Indexing](https://numpy.org/doc/stable/user/theory.broadcasting.html)
