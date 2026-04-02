# 🔓 Python 3.13's Free-Threaded Mode

## 🎯 What You'll Learn

- What the GIL (Global Interpreter Lock) actually is
- Why free-threaded mode matters for CPU-bound Python code
- How to enable and test free-threaded Python
- When to use threading vs asyncio vs multiprocessing

## 📦 Prerequisites

- Understanding of Python threading concepts
- Familiarity with concurrent programming challenges

---

## What is the GIL?

The **Global Interpreter Lock (GIL)** is a fundamental part of CPython (the standard Python implementation). It ensures only one thread executes Python bytecode at a time.

### The Bouncer Analogy

```
🎭 The GIL is like a bouncer at a nightclub:
   
   Thread 1: "I'd like to run some Python code"
   Thread 2: "Me too!"
   GIL (bouncer): "Sorry, one at a time please"
   
   Even if you have 8 cores, only ONE can run Python at any moment!
```

### Why Does the GIL Exist?

The GIL is a design decision in CPython that makes memory management (reference counting) thread-safe:

```python
# Simplified reference counting (what the GIL protects)
import sys

def demo():
    obj = []  # Creates an object with refcount = 1
    obj2 = obj  # refcount = 2
    # When function ends, refcount goes to 0, memory is freed
    # The GIL ensures this happens safely across threads

# Without the GIL, two threads could:
# 1. Read refcount simultaneously
# 2. Both think it's safe to free
# 3. Double-free the memory = CRASH!
```

### 💡 Explanation

The GIL exists because CPython uses reference counting for memory management. Without it, multiple threads could simultaneously modify the reference count of objects, leading to memory corruption and crashes.

---

## GIL vs Free-Threaded: Side by Side

### GIL Mode (Current Default)

```
┌─────────────────────────────────────────┐
│           CPU Cores (4 cores)           │
├─────────────────────────────────────────┤
│ Core 1: ████████████░░░░░░░░░░░░░░░░░░ │ ← Thread 1 runs, others wait
│ Core 2: ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ │
│ Core 3: ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ │
│ Core 4: ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ │
└─────────────────────────────────────────┘

Single lane highway - only one thread executes Python at a time!
```

### Free-Threaded Mode (Python 3.13+)

```
┌─────────────────────────────────────────┐
│           CPU Cores (4 cores)           │
├─────────────────────────────────────────┤
│ Core 1: ████████████████████████████████ │ ← Thread 1 runs
│ Core 2: ████████████████████████████████ │ ← Thread 2 runs simultaneously! 
│ Core 3: ████████████████████████████████ │ ← Thread 3 runs simultaneously!
│ Core 4: ████████████████████████████████ │ ← Thread 4 runs simultaneously!
└─────────────────────────────────────────┘

Multi-lane highway - multiple threads execute Python at once!
```

---

## Enabling Free-Threaded Mode

Python 3.13 introduces optional free-threaded mode. You need to:

### 1. Build Python with --disable-gil

```bash
# Download Python source
git clone https://github.com/python/cpython.git
cd cpython

# Configure with GIL disabled
./configure --disable-gil

# Build
make -j$(nproc)

# Install
./python.exe -m venv free-threaded-venv
```

### 2. Or Use the Experimental Build

```bash
# Check if free-threaded build is available
python3.13t --version  # 't' suffix indicates free-threaded build
```

---

## What Changes in Free-Threaded Mode?

### CPU-Bound Tasks Now Actually Speed Up

```python
import threading
import time

def cpu_work(n: int) -> int:
    """Simulate CPU-bound work."""
    result = 0
    for i in range(n):
        result += i * i
    return result

def benchmark_threads(n_iterations: int, n_threads: int) -> float:
    """Run CPU work using multiple threads."""
    start = time.perf_counter()
    
    threads = []
    chunk_size = n_iterations // n_threads
    
    for _ in range(n_threads):
        t = threading.Thread(target=cpu_work, args=(chunk_size,))
        threads.append(t)
        t.start()
    
    for t in threads:
        t.join()
    
    return time.perf_counter() - start

# Test with different thread counts
N = 10_000_000

print("Testing CPU-bound work with threading:")
for n_threads in [1, 2, 4, 8]:
    elapsed = benchmark_threads(N, n_threads)
    print(f"  {n_threads} thread(s): {elapsed:.3f}s")
```

### What Doesn't Change

```python
# Still need asyncio for I/O-bound concurrency!
import asyncio
import httpx

async def fetch_url(client: httpx.AsyncClient, url: str) -> str:
    """Fetch URL asynchronously."""
    response = await client.get(url)
    return response.text

async def main():
    """I/O-bound work still uses asyncio!"""
    async with httpx.AsyncClient() as client:
        urls = ["https://example.com"] * 10
        # This is where asyncio shines - waiting for network
        results = await asyncio.gather(*[
            fetch_url(client, url) for url in urls
        ])
        return len(results)

asyncio.run(main())
```

---

## When to Use Each Concurrency Model

| Use Case | GIL Impact | Best Solution |
|----------|------------|---------------|
| Network requests | GIL released during I/O | `asyncio` |
| File I/O | GIL released during I/O | `asyncio` or `threading` |
| CPU-heavy math | GIL blocks parallelism | `multiprocessing` or free-threaded |
| Web scraping | GIL released during I/O | `asyncio` with httpx |
| Image processing | GIL blocks parallelism | `multiprocessing` |
| Running LLMs | GIL blocks parallelism | Free-threaded or subprocess |

### Decision Flowchart

```
┌────────────────────────────────────────┐
│ What type of work?                     │
└────────────┬───────────────────────────┘
             │
      ┌──────┴──────┐
      ▼             ▼
   I/O-bound    CPU-bound
      │             │
      ▼             ▼
   asyncio      ┌───────┐
   (preferred)  │Free-  │
   or           │threaded│
   threading    │(3.13+) │
                │ or      │
                │multiprocessing│
                └─────────┘
```

---

## Real-World Example: Parallel Image Processing

```python
import threading
from PIL import Image
import io
from typing import list

def process_image(image_data: bytes) -> bytes:
    """Process a single image - CPU bound."""
    img = Image.open(io.BytesIO(image_data))
    # Apply some CPU-intensive transformation
    img = img.convert("L")  # Grayscale
    img = img.filter(Image.Image.SMOOTH)
    
    output = io.BytesIO()
    img.save(output, format="JPEG")
    return output.getvalue()

def process_images_parallel(images: list[bytes]) -> list[bytes]:
    """Process multiple images in parallel."""
    results: list[bytes] = []
    lock = threading.Lock()
    
    def worker(img_data: bytes):
        result = process_image(img_data)
        with lock:
            results.append(result)
    
    threads = [threading.Thread(target=worker, args=(img,)) for img in images]
    
    for t in threads:
        t.start()
    for t in threads:
        t.join()
    
    return results

# This would scale much better in free-threaded mode!
```

### 💡 Line-by-Line Breakdown

```python
import threading           # Threading module
from PIL import Image     # Pillow for image processing
import io
from typing import List

def process_image(image_data: bytes) -> bytes:  # CPU-bound image processing
    img = Image.open(io.BytesIO(image_data))     # Open from bytes
    img = img.convert("L")                        # Convert to grayscale
    img = img.filter(Image.Image.SMOOTH)        # Apply filter (CPU intensive)
    output = io.BytesIO()                        # Create output buffer
    img.save(output, format="JPEG")             # Save processed image
    return output.getvalue()                     # Return bytes

def process_images_parallel(images: List[bytes]) -> List[bytes]:  # Process list
    results: List[bytes] = []    # Shared results list
    lock = threading.Lock()       # Lock for thread-safe list access
    
    def worker(img_data: bytes):  # Worker function for each thread
        result = process_image(img_data)   # Process single image
        with lock:                # Thread-safe append
            results.append(result)
    
    threads = [threading.Thread(target=worker, args=(img,)) for img in images]  # Create threads
    
    for t in threads:
        t.start()  # Start all threads
    for t in threads:
        t.join()   # Wait for all to finish
    
    return results
```

---

## Current Limitations

Free-threaded mode is **experimental** in Python 3.13:

1. **Not all C extensions support it** — Some packages may not work
2. **Reference counting changes** — Some code patterns may behave differently
3. **Not production-ready for most apps** — Use for experimentation
4. **Build availability** — Need special build of Python

### 🧪 Try It

```bash
# If you have a free-threaded build
python3.13t -c "import sys; print(f'GIL disabled: {not sys.flags.gil_disabled}')"
```

---

## ✅ Summary

- The GIL (Global Interpreter Lock) limits Python to one thread executing at a time
- Python 3.13 introduces optional free-threaded mode without the GIL
- Free-threaded mode helps CPU-bound tasks scale across cores
- For I/O-bound work, asyncio is still the best choice
- Free-threaded is experimental — not ready for production yet

## ➡️ Next Steps

Continue to [02_jit_compiler_intro.md](./02_jit_compiler_intro.md) to learn about Python 3.13's experimental JIT compiler.

## 🔗 Further Reading

- [PEP 703: Making the Global Interpreter Lock Optional](https://peps.python.org/pep-0703/)
- [Python 3.13 Free-Threaded Mode FAQ](https://docs.python.org/3.13/faq/general.html#faq-gil-free)
- [Understanding the Python GIL](https://realpython.com/python-gil/)
