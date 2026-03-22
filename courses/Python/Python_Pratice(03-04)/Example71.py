# Example71.py
# Topic: functools - lru_cache, cache, cached_property

# This file demonstrates functools caching utilities.


# ============================================================
# Example 1: functools.lru_cache
# ============================================================
print("=== functools.lru_cache ===")

from functools import lru_cache
import time

@lru_cache(maxsize=128)
def fibonacci(n):
    """Calculate fibonacci with memoization."""
    if n < 2:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

# First call - slow
start = time.perf_counter()
result = fibonacci(30)
first_time = time.perf_counter() - start
print(f"Fibonacci(30): {result}")
print(f"First call: {first_time:.6f}s")

# Second call - fast (cached)
start = time.perf_counter()
result = fibonacci(30)
second_time = time.perf_counter() - start
print(f"Second call: {second_time:.6f}s")

# Cache info
print(f"Cache info: {fibonacci.cache_info()}")


# ============================================================
# Example 2: lru_cache with different maxsize
# ============================================================
print("\n=== lru_cache maxsize ===")

from functools import lru_cache

@lru_cache(maxsize=2)
def cached_func(x):
    print(f"Computing for {x}")
    return x * x

# First calls
print(cached_func(1))
print(cached_func(2))

# Cache full now
print(cached_func(3))

# Oldest entry evicted (1)
print(cached_func(1))  # Recomputes


# ============================================================
# Example 3: functools.cache (Python 3.9+)
# ============================================================
print("\n=== functools.cache ===")

from functools import cache

@cache
def expensive_operation(n):
    """Cache with unlimited size."""
    print(f"Computing for {n}...")
    return n ** 3

# First calls
print(expensive_operation(5))
print(expensive_operation(10))
print(expensive_operation(5))  # Cached

# Check cache
print(f"Cache: {expensive_operation.cache}")


# ============================================================
# Example 4: functools.cached_property
# ============================================================
print("\n=== functools.cached_property ===")

from functools import cached_property

class Circle:
    def __init__(self, radius):
        self.radius = radius
    
    @cached_property
    def diameter(self):
        print("Computing diameter...")
        return self.radius * 2
    
    @cached_property
    def area(self):
        print("Computing area...")
        return 3.14159 * self.radius ** 2

c = Circle(5)

# First access - computes
print(f"Diameter: {c.diameter}")

# Second access - cached
print(f"Diameter again: {c.diameter}")

# Area - computes once
print(f"Area: {c.area}")
print(f"Area again: {c.area}")


# ============================================================
# Example 5: Real-world - API calls
# ============================================================
print("\n=== Real-world: API Caching ===")

from functools import lru_cache
import time

# Simulated API calls
call_count = 0

@lru_cache(maxsize=100)
def fetch_user_data(user_id):
    global call_count
    call_count += 1
    time.sleep(0.1)  # Simulate network delay
    return {"id": user_id, "name": f"User {user_id}"}

# First call
print("Fetching user 1...")
data1 = fetch_user_data(1)
print(f"Got: {data1}")

# Cached call
print("Fetching user 1 again...")
data2 = fetch_user_data(1)
print(f"Got: {data2}")

print(f"Total API calls: {call_count}")


# ============================================================
# Example 6: Real-world - Database query caching
# ============================================================
print("\n=== Real-world: Database Caching ===")

from functools import lru_cache

@lru_cache(maxsize=50)
def get_product_category(category_id):
    """Cache expensive database queries."""
    print(f"Querying DB for category {category_id}...")
    categories = {
        1: "Electronics",
        2: "Clothing", 
        3: "Books"
    }
    return categories.get(category_id, "Unknown")

# First calls - hits database
for cat_id in [1, 2, 3]:
    print(f"Category {cat_id}: {get_product_category(cat_id)}")

# Cached - no database hit
print("Cached lookups:")
for cat_id in [1, 2, 3]:
    print(f"Category {cat_id}: {get_product_category(cat_id)}")


# ============================================================
# Example 7: Clearing cache
# ============================================================
print("\n=== Clearing Cache ===")

from functools import lru_cache

@lru_cache(maxsize=10)
def process_data(x):
    print(f"Processing {x}")
    return x * 2

# Use cache
process_data(5)
process_data(10)

print(f"Cache before: {process_data.cache_info()}")

# Clear cache
process_data.cache_clear()
print(f"Cache after clear: {process_data.cache_info()}")


# ============================================================
# Example 8: Cache with mutable arguments
# ============================================================
print("\n=== Cache with Mutable Arguments ===")

from functools import lru_cache

# Note: mutable args can cause issues
@lru_cache(maxsize=10)
def add_to_list(lst, value):
    """This may not work as expected with mutable args."""
    return lst + [value]

# This won't use cache correctly - list is mutable
try:
    result = add_to_list([1, 2], 3)
    print(f"Result: {result}")
except TypeError as e:
    print(f"Error: {e}")


# ============================================================
# Example 9: Best practices
# ============================================================
print("\n=== Best Practices ===")

from functools import lru_cache

# Use with hashable arguments
@lru_cache(maxsize=128)
def fib_with_cache(n):
    """Cache works best with hashable (immutable) args."""
    if n < 2:
        return n
    return fib_with_cache(n - 1) + fib_with_cache(n - 2)

# Test performance
import time
fib_with_cache.cache_clear()
start = time.perf_counter()
fib_with_cache(500)
elapsed = time.perf_counter() - start
print(f"Fibonacci(500) took: {elapsed:.6f}s")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: functools Caching")
print("=" * 50)
print("""
@lru_cache(maxsize=128):
  - Memoization decorator
  - Least Recently Used eviction
  - Requires hashable arguments

@cache (Python 3.9+):
  - Unlimited size lru_cache
  - Simpler syntax

@cached_property:
  - Caches property after first access
  - Per-instance storage
  - Python 3.8+

cache_clear():
  - Clear all cached values

cache_info():
  - Returns (hits, misses, maxsize, currsize)
""")
