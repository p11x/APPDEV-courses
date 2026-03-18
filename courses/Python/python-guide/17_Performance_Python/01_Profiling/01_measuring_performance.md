# ⏱️ Measuring Performance

## 🎯 What You'll Learn

- Using timeit for micro-benchmarks
- Using cProfile for whole-program profiling
- Using line_profiler for line-by-line analysis

---

## timeit for Micro-benchmarks

```python
import timeit

# Simple benchmark
result = timeit.timeit(
    "[i**2 for i in range(1000)]",
    number=10000
)
print(f"Time: {result:.4f}s")

# With setup
result = timeit.timeit(
    "sorted(data)",
    setup="import random; data = [random.random() for _ in range(1000)]",
    number=1000
)
```

---

## cProfile for Whole Program

```bash
python -m cProfile -s cumulative script.py
```

```python
# Or programmatically
import cProfile
import pstats

profiler = cProfile.Profile()
profiler.enable()

# Your code here
result = [i**2 for i in range(100000)]

profiler.disable()
stats = pstats.Stats(profiler)
stats.sort_stats("cumulative")
stats.print_stats(10)  # Top 10
```

---

## line_profiler

```bash
pip install line-profiler
```

```python
# Add @profile decorator
@profile
def slow_function():
    total = 0
    for i in range(100000):
        total += i * i
    return total

# Run with:
# kernprof -l -v script.py
```

---

## memory_profiler

```bash
pip install memory-profiler
```

```python
@profile
def memory_heavy():
    data = [i**2 for i in range(100000)]
    return data

# Run with:
# python -m memory_profiler script.py
```

---

## ✅ Summary

- timeit: simple benchmarks
- cProfile: find slow functions
- line_profiler: find slow lines

## 🔗 Further Reading

- [timeit documentation](https://docs.python.org/3/library/timeit.html)
- [line_profiler](https://github.com/pyutils/line_profiler)
