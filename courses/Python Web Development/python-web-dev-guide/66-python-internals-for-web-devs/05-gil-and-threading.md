# GIL and Threading

## What You'll Learn

- Global Interpreter Lock
- Threading vs multiprocessing
- When to use each

## Prerequisites

- Completed `04-async-internals.md`

## Global Interpreter Lock

The GIL prevents multiple threads from executing Python bytecode simultaneously. This means threading doesn't provide true parallelism for CPU-bound tasks.

## Threading vs Multiprocessing

- **Threading**: Good for I/O-bound tasks
- **Multiprocessing**: Good for CPU-bound tasks

```python
# Threading for I/O
import threading

def fetch_data(url):
    # I/O operation
    pass

threads = [threading.Thread(target=fetch_data, args=(url,)) for url in urls]
for t in threads: t.start()
```

## Summary

- GIL limits CPU parallelism
- Use threading for I/O, multiprocessing for CPU

## Next Steps

Continue to `06-imports-and-modules.md`.
