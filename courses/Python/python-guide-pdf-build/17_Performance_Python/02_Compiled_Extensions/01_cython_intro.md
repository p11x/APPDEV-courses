# 🔧 Cython: Compile Python to C

## 🎯 What You'll Learn

- Using Cython to speed up Python
- Basic Cython syntax

---

## Installation

```bash
pip install cython
```

---

## Basic Example

```python
# mymodule.pyx - note .pyx extension

def sum_squares(int n):
    """Sum of squares from 0 to n-1."""
    cdef int i
    cdef long total = 0
    
    for i in range(n):
        total += i * i
    
    return total

# setup.py
from setuptools import setup
from Cython.Build import cythonize

setup(
    name="mymodule",
    ext_modules=cythonize("mymodule.pyx"),
)
```

### Build

```bash
python setup.py build_ext --inplace
```

---

## Type Declarations

```python
# cdef for C variables (fastest)
cdef int x = 0
cdef double y = 1.0

# cpdef for functions callable from Python and C
cpdef int add(int a, int b):
    return a + b
```

---

## When to Use Cython

- Tight numerical loops
- CPU-intensive mathematical code
- Not for general Python code

---

## ✅ Summary

- Cython compiles Python to C
- Use cdef/cpdef for type declarations
- Best for numerical loops

## 🔗 Further Reading

- [Cython Documentation](https://cython.org/)
