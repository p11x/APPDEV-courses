# Example237: functools - lru_cache (memoization)
from functools import lru_cache
import time

# lru_cache(maxsize=128) - memoization decorator
@lru_cache(maxsize=128)
def fibonacci(n):
    if n < 2:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

print("lru_cache - Fibonacci with memoization:")
print("First call (uncached):")
start = time.time()
result = fibonacci(30)
print(f"fibonacci(30) = {result}, Time: {time.time() - start:.6f}s")

print("\nSecond call (cached):")
start = time.time()
result = fibonacci(30)
print(f"fibonacci(30) = {result}, Time: {time.time() - start:.6f}s")

print(f"\nCache info: {fibonacci.cache_info()}")

# Cache with different sizes
@lru_cache(maxsize=3)
def expensive_func(x):
    print(f"Computing for {x}...")
    return x ** 2

print("\nLimited cache (maxsize=3):")
expensive_func(1)
expensive_func(2)
expensive_func(3)
expensive_func(1)  # cached
expensive_func(4)  # evicts 1
expensive_func(1)  # recomputed
print(f"Cache info: {expensive_func.cache_info()}")

# Clear cache
expensive_func.cache_clear()
print(f"After clear: {expensive_func.cache_info()}")

# Without cache (for comparison)
def fibonacci_slow(n):
    if n < 2:
        return n
    return fibonacci_slow(n - 1) + fibonacci_slow(n - 2)

print("\nWithout cache:")
start = time.time()
result = fibonacci_slow(20)
print(f"fibonacci_slow(20) = {result}, Time: {time.time() - start:.6f}s")

# Practical use: API calls
@lru_cache()
def get_user_data(user_id):
    print(f"Fetching user {user_id} from API...")
    return {"id": user_id, "name": f"User {user_id}"}

print("\nPractical - API calls:")
get_user_data(1)
get_user_data(2)
get_user_data(1)  # cached
