# ⚡ Optimization Techniques

## 🎯 What You'll Learn

- Key optimization techniques
- When to optimize

---

## 1. Local Variables Are Faster

```python
# ❌ Slow: global lookup
result = sum([i**2 for i in range(100000)])

# ✅ Faster: local variable
f = i**2  # Cache in local scope
```

---

## 2. List Comprehensions vs For Loops

```python
# ❌ Slow
result = []
for i in range(10000):
    result.append(i**2)

# ✅ Faster
result = [i**2 for i in range(10000)]
```

---

## 3. Use Sets for Membership

```python
# ❌ Slow: list
my_list = list(range(1000))
if 500 in my_list:  # O(n)
    pass

# ✅ Fast: set
my_set = set(range(1000))
if 500 in my_set:  # O(1)
    pass
```

---

## 4. Use Generators for Memory

```python
# ❌ Memory heavy
result = [i**2 for i in range(10000000)]

# ✅ Memory efficient
result = (i**2 for i in range(10000000))
```

---

## 5. Use __slots__

```python
# ❌ More memory
class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

# ✅ Less memory
class Point:
    __slots__ = ['x', 'y']
    def __init__(self, x, y):
        self.x = x
        self.y = y
```

---

## 6. Use functools.lru_cache

```python
from functools import lru_cache

@lru_cache(maxsize=None)
def fibonacci(n):
    if n < 2:
        return n
    return fibonacci(n-1) + fibonacci(n-2)
```

---

## 7. String Joining

```python
# ❌ Slow
s = ""
for word in words:
    s += word + " "

# ✅ Fast
s = " ".join(words)
```

---

## ✅ Summary

- Profile first, optimize second
- Use list comprehensions over loops
- Use sets for membership tests
- Use generators for memory efficiency

## 🔗 Further Reading

- [Python Performance Tips](https://docs.python-guide.org/writing/performance/)
