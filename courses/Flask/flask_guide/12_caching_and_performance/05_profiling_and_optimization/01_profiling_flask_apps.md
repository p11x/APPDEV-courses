<!-- FILE: 12_caching_and_performance/05_profiling_and_optimization/01_profiling_flask_apps.md -->

## Overview

Profiling helps identify performance bottlenecks in your Flask application. This file covers different profiling techniques.

## Code Walkthrough

```python
# profiling.py
from flask import Flask
import cProfile
import pstats
import io

app = Flask(__name__)

# ============================================
# Method 1: cProfile
# ============================================

@app.route("/profile")
def profile_endpoint():
    """Profile a specific endpoint"""
    profiler = cProfile.Profile()
    profiler.enable()
    
    # Run the code to profile
    result = expensive_operation()
    
    profiler.disable()
    
    # Get stats
    s = io.StringIO()
    stats = pstats.Stats(profiler, stream=s)
    stats.sort_stats("cumulative")
    stats.print_stats(20)
    
    return s.getvalue()

def expensive_operation():
    """Simulated expensive operation"""
    total = 0
    for i in range(100000):
        total += i
    return total

# ============================================
# Method 2: flask-profiler
# ============================================

# pip install flask-profiler

# app.config["FLASK_PROFILER"] = {
#     "enabled": True,
#     "endpoint": "/profiler"
# }

# ============================================
# Method 3: Timing decorator
# ============================================

import time
from functools import wraps

def timeit(f):
    @wraps(f)
    def wrapper(*args, **kwargs):
        start = time.time()
        result = f(*args, **kwargs)
        elapsed = time.time() - start
        print(f"{f.__name__} took {elapsed:.4f}s")
        return result
    return wrapper

@timeit
def slow_function():
    time.sleep(1)
    return "done"

@app.route("/timed")
def timed_endpoint():
    slow_function()
    return "Done!"

if __name__ == "__main__":
    app.run(debug=True)
```

## Next Steps

Continue to [02_response_compression.md](02_response_compression.md)
