# ⚡ Python 3.13's Experimental JIT Compiler

## 🎯 What You'll Learn

- What a JIT (Just-In-Time) compiler is
- How Python's JIT differs from interpreted code
- How to enable JIT in Python 3.13
- When JIT provides performance benefits

## 📦 Prerequisites

- Understanding of Python execution model (interpreted vs compiled)
- Familiarity with performance profiling concepts

---

## What is a JIT Compiler?

### The Translation Analogy

Think of how you might communicate in different languages:

```
📚 INTERPRETED (Current Python):
   You speak English, they speak French
   A translator sits between you, converting EVERY sentence on-the-fly
   - Flexible but slow
   - Each sentence translated fresh

🗣️ COMPILED (C/C++):
   You write everything in French first
   A translator converts ALL sentences BEFORE you speak
   - Fast but less flexible
   - Everything translated once, then reused

⚡ JIT (Python 3.13):
   Same translator, but they're clever!
   They listen to what you're saying, memorize common phrases,
   and start speaking those directly without translation
   - The best of both worlds!
   - Adapts to your specific usage patterns
```

### How JIT Works in Python

```
┌─────────────────────────────────────────────────────┐
│              Standard Python Execution              │
├─────────────────────────────────────────────────────┤
│                                                     │
│   Python Code                                       │
│        │                                           │
│        ▼                                           │
│   ┌─────────┐     ┌─────────┐     ┌───────────┐  │
│   │ Parser  │ ──▶ │  Byte   │ ──▶ │  Python   │  │
│   │         │     │  Code   │     │ Interpreter│  │
│   └─────────┘     └─────────┘     └───────────┘  │
│        │              │                 │         │
│        │              │                 │         │
│   (Once)         (Once per        (Runs every    │
│                  module)           time!)         │
│                                                     │
└─────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────┐
│           Python 3.13 with JIT Enabled             │
├─────────────────────────────────────────────────────┤
│                                                     │
│   Python Code                                       │
│        │                                           │
│        ▼                                           │
│   ┌─────────┐     ┌─────────┐     ┌───────────┐  │
│   │ Parser  │ ──▶ │  Byte   │ ──▶ │  Python   │  │
│   │         │     │  Code   │     │ Interpreter│  │
│   └─────────┘     └─────────┘     └───────────┘  │
│        │              │                 │         │
│        │              │                 │         │
│   (Once)         (Once per         (First time)   │
│                  module)              │            │
│                                       ▼            │
│                               ┌───────────────┐    │
│                               │   JIT         │    │
│                               │   Compiler    │    │
│                               └───────────────┘    │
│                                       │            │
│                                       ▼            │
│                               ┌───────────────┐    │
│                               │  Machine Code │    │
│                               │  (Cached!)    │    │
│                               └───────────────┘    │
│                                       │            │
│                                       ▼            │
│                               (Runs directly!     │
│                                10-15% faster)      │
│                                                     │
└─────────────────────────────────────────────────────┘
```

---

## Enabling the JIT Compiler

### Set Environment Variable

```bash
# Enable JIT compilation
export PYTHON_JIT=1

# Or run Python with JIT enabled
python3.13 -X jit on your_script.py
```

### Verify JIT is Running

```python
import sys

# Check if JIT is enabled
if hasattr(sys, "jit_enabled"):
    print(f"JIT enabled: {sys.jit_enabled}")
else:
    print("JIT not available in this Python build")

# Python 3.13+ specific
try:
    import _jit
    print("JIT module available!")
except ImportError:
    print("JIT not compiled in this Python")
```

---

## What is a "Hot Path"?

JIT compilers optimize **hot paths** — code that runs repeatedly:

```python
# Cold code - runs once, not worth optimizing
def one_time_setup():
    config = load_config()  # Runs once, JIT ignores this
    return config

# Hot path - runs millions of times!
def process_data(items: list[int]) -> int:
    """This function runs in a tight loop - perfect for JIT!"""
    total = 0
    for item in items:
        total += item * 2 + 1  # Simple math, runs many times
    return total

# This gets JIT optimization
data = list(range(1_000_000))
for _ in range(100):
    result = process_data(data)  # Runs 100 times - hot path!
```

### 💡 Explanation

- JIT watches your code as it runs
- It identifies which parts execute most frequently
- Those "hot paths" get compiled to machine code
- The rest stays interpreted

---

## Simple Benchmark: Tight Loop with JIT

```python
import time
import sys

# Check if JIT is available
def is_jit_enabled() -> bool:
    """Check if Python JIT is enabled."""
    try:
        return bool(os.environ.get("PYTHON_JIT", ""))
    except Exception:
        return False

def benchmark_loop(n: int) -> float:
    """Benchmark a tight loop."""
    start = time.perf_counter()
    
    total = 0
    for i in range(n):
        total += i * i + i * 2 + 1
    
    elapsed = time.perf_counter() - start
    return elapsed

# Run benchmark
N = 10_000_000
iterations = 5

times = []
for _ in range(iterations):
    t = benchmark_loop(N)
    times.append(t)

avg_time = sum(times) / len(times)
print(f"Average time: {avg_time:.4f}s")
print(f"JIT enabled: {is_jit_enabled()}")

# Run with JIT:
# $ PYTHON_JIT=1 python benchmark.py
# Expected: 10-15% faster than without JIT
```

---

## What Changes with JIT?

### No Code Changes Needed!

```python
# Your existing Python code works with or without JIT
def fibonacci(n: int) -> int:
    """Calculate fibonacci - a classic recursive function."""
    if n <= 1:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

# JIT automatically optimizes hot paths
# Just run with PYTHON_JIT=1
```

### What Gets Optimized

1. **Tight loops** — for/while loops that run many times
2. **Simple operations** — integer math, comparisons
3. **Hot functions** — functions called frequently
4. **Type-stable code** — code where types don't change

### What Doesn't Get Optimized (Yet)

1. **Complex Python features** — comprehensions, generators
2. **Dynamic code** — eval(), exec(), type() creation
3. **I/O operations** — file, network
4. **Rarely-called code** — cold paths stay interpreted

---

## Current Status and Limitations

### Speedup Expectations

```text
┌────────────────────────────────────────────────┐
│  Benchmark Type         │  Expected Speedup    │
├────────────────────────┼──────────────────────┤
│  Tight numeric loops   │    15-30%            │
│  Simple algorithms    │    10-20%            │
│  String operations    │    5-15%             │
│  I/O bound tasks      │    0% (no change)    │
│  Complex Python       │    0-5%              │
└────────────────────────────────────────────────┘
```

### Limitations in Python 3.13

1. **Experimental** — Not production-ready
2. **Limited optimization** — Still maturing
3. **Memory overhead** — JIT cache uses memory
4. **Startup time** — Slower to start

### 🚀 Pro Tip

Use JIT for:
- Long-running scripts
- CPU-bound numerical processing
- Games and simulations

Don't use JIT for:
- Quick one-off scripts
- I/O-bound operations
- Startup-critical applications

---

## Comparison: Python JIT vs Other Languages

| Language | JIT | Use Case |
|----------|-----|----------|
| Python 3.13+ | Optional | General purpose |
| Java | Yes | Enterprise, Android |
| JavaScript | Yes | Web browsers |
| C# | Yes | Windows apps, games |
| Julia | Yes | Scientific computing |
| LuaJIT | Yes | Game scripting |

---

## ✅ Summary

- JIT (Just-In-Time) compilation converts hot paths to machine code at runtime
- Python 3.13 introduces an experimental JIT compiler via PYTHON_JIT=1
- JIT works automatically — no code changes required
- Expect 10-15% speedup on numerical/tight-loop code
- JIT doesn't help I/O-bound or rarely-run code

## ➡️ Next Steps

Continue to [03_tstrings_and_other_changes.md](./03_tstrings_and_other_changes.md) to learn about t-strings, locals() changes, and other quality-of-life improvements in Python 3.13.

## 🔗 Further Reading

- [PEP 744: JIT Compilation](https://peps.python.org/pep-0744/)
- [Python 3.13 Release Notes](https://docs.python.org/3.13/whatsnew/3.13.html)
- [What is JIT compilation?](https://stackoverflow.com/questions/95696/what-is-just-in-time-jit-compilation)
