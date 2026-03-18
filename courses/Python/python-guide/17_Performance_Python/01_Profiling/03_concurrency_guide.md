# 🔀 Concurrency Guide

## 🎯 What You'll Learn

- threading vs multiprocessing vs asyncio
- When to use each

---

## Quick Comparison

| Approach | GIL Impact | Best For |
|----------|------------|----------|
| threading | Blocked | I/O-bound |
| multiprocessing | Bypassed | CPU-bound |
| asyncio | Released | I/O-bound (async) |

---

## threading for I/O

```python
import threading
import urllib.request

def fetch_url(url):
    response = urllib.request.urlopen(url)
    return response.read()

threads = []
urls = ["https://example.com"] * 10

for url in urls:
    t = threading.Thread(target=fetch_url, args=(url,))
    threads.append(t)
    t.start()

for t in threads:
    t.join()
```

---

## multiprocessing for CPU

```python
from multiprocessing import Pool

def cpu_heavy(n):
    return sum(i*i for i in range(n))

if __name__ == "__main__":
    with Pool(4) as pool:
        results = pool.map(cpu_heavy, [1000000]*8)
```

---

## asyncio for Async I/O

```python
import asyncio
import httpx

async def fetch(client, url):
    response = await client.get(url)
    return response.text

async def main():
    async with httpx.AsyncClient() as client:
        tasks = [fetch(client, "https://example.com") for _ in range(10)]
        results = await asyncio.gather(*tasks)

asyncio.run(main())
```

---

## Decision Flowchart

```
Need concurrency?
    │
    ├─► I/O bound (network, file)?
    │       │
    │       ├─► Many tasks? → asyncio
    │       └─► Few tasks? → threading
    │
    └─► CPU bound (computation)?
            │
            ├─► Python 3.13+? → free-threaded
            └─► Older? → multiprocessing
```

---

## ✅ Summary

- asyncio: async I/O (network, files)
- threading: simple I/O (when asyncio not available)
- multiprocessing: CPU-bound work

## 🔗 Further Reading

- [asyncio documentation](https://docs.python.org/3/library/asyncio.html)
