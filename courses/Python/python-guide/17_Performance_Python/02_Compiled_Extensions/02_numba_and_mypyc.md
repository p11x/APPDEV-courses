# 🚀 Numba and mypyc

## 🎯 What You'll Learn

- Using Numba for JIT compilation
- Using mypyc to compile typed Python

---

## Numba

```bash
pip install numba
```

```python
from numba import jit

@jit(nopython=True)
def sum_squares(n):
    total = 0
    for i in range(n):
        total += i * i
    return total

# First call compiles - subsequent calls are fast!
result = sum_squares(10000000)
```

---

## Numba Features

```python
from numba import njit, vectorize

# @njit = nopython mode (faster)
@njit
def fast_function(x):
    return x * 2

# @vectorize = create ufuncs
@vectorize
def vectorized_mul(x, y):
    return x * y
```

---

## mypyc

```bash
pip install mypy[mypyc]
```

```python
# Must have full type annotations!

def process_data(data: list[int]) -> int:
    """Process integer list."""
    return sum(data)

# Compile
# mypyc module.py
```

---

## Comparison

| Tool | Best For | Requirements |
|------|----------|--------------|
| Numba | Numerical loops | NumPy arrays |
| mypyc | Typed libraries | Full annotations |

---

## ✅ Summary

- Numba: JIT for numerical Python
- mypyc: compile typed Python to C

## 🔗 Further Reading

- [Numba Documentation](https://numba.readthedocs.io/)
- [mypyc Documentation](https://mypyc.readthedocs.io/)
