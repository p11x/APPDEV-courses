# 📊 NumPy and Vectorization

## 🎯 What You'll Learn

- Vectorized operations with NumPy
- Broadcasting
- When to use NumPy

---

## Basic Vectorization

```python
import numpy as np

# ❌ Slow: Python loop
result = [i**2 for i in range(10000)]

# ✅ Fast: NumPy vectorized
arr = np.arange(10000)
result = arr ** 2
```

---

## Broadcasting

```python
import numpy as np

# Add scalar to array
arr = np.array([1, 2, 3])
result = arr + 10  # [11, 12, 13]

# Add arrays of different shapes
a = np.array([[1], [2], [3]])  # 3x1
b = np.array([10, 20, 30])      # 3
# Broadcasting adds them:
# [[11, 21, 31],
#  [12, 22, 32],
#  [13, 23, 33]]
```

---

## Universal Functions (ufuncs)

```python
import numpy as np

# Apply function to entire array
arr = np.array([1, 4, 9, 16])
np.sqrt(arr)   # Square root
np.sin(arr)    # Sine
np.log(arr)    # Logarithm

# np.vectorize for custom functions
def my_func(x):
    return x * 2 + 1

vectorized = np.vectorize(my_func)
result = vectorized(arr)
```

---

## einsum

```python
import numpy as np

a = np.arange(6).reshape(2, 3)
b = np.arange(6).reshape(3, 2)

# Matrix multiplication via einsum
result = np.einsum('ij,jk->ik', a, b)
```

---

## When NOT to Use NumPy

- Very small arrays (overhead > benefit)
- Non-numerical operations
- Sparse data structures

---

## ✅ Summary

- Use NumPy for numerical operations
- Vectorized > Python loops
- Broadcasting handles different array shapes

## 🔗 Further Reading

- [NumPy Documentation](https://numpy.org/doc/)
