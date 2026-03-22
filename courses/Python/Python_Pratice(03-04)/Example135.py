# Example135.py
# Topic: functools - Higher-Order Functions


# ============================================================
# Example 1: lru_cache
# ============================================================
print("=== lru_cache ===")

from functools import lru_cache

@lru_cache(maxsize=3)
def fib(n):
    if n < 2:
        return n
    return fib(n-1) + fib(n-2)

print(f"fib(10): {fib(10)}")
print(f"fib cache info: {fib.cache_info()}")


# ============================================================
# Example 2: partial
# ============================================================
print("\n=== partial ===")

from functools import partial

def power(base, exponent):
    return base ** exponent

square = partial(power, exponent=2)
cube = partial(power, exponent=3)

print(f"square(5): {square(5)}")
print(f"cube(5): {cube(5)}")


# ============================================================
# Example 3: reduce
# ============================================================
print("\n=== reduce ===")

from functools import reduce

numbers = [1, 2, 3, 4, 5]
product = reduce(lambda x, y: x * y, numbers)
print(f"Product: {product}")

max_val = reduce(lambda a, b: a if a > b else b, numbers)
print(f"Max: {max_val}")


# ============================================================
# Example 4: singledispatch
# ============================================================
print("\n=== singledispatch ===")

from functools import singledispatch

@singledispatch
def process(value):
    print(f"Unknown: {value}")

@process.register(int)
def process_int(value):
    print(f"Integer: {value * 2}")

@process.register(str)
def process_str(value):
    print(f"String: {value.upper()}")

process(10)
process("hello")
process(3.14)


# ============================================================
# Example 5: total_ordering
# ============================================================
print("\n=== total_ordering ===")

from functools import total_ordering

@total_ordering
class Version:
    def __init__(self, major, minor):
        self.major = major
        self.minor = minor
    
    def __eq__(self, other):
        if not isinstance(other, Version):
            return NotImplemented
        return (self.major, self.minor) == (other.major, other.minor)
    
    def __lt__(self, other):
        if not isinstance(other, Version):
            return NotImplemented
        return (self.major, self.minor) < (other.major, other.minor)

v1 = Version(1, 2)
v2 = Version(2, 0)
v3 = Version(1, 2)

print(f"v1 < v2: {v1 < v2}")
print(f"v1 <= v3: {v1 <= v3}")
print(f"v1 > v2: {v1 > v2}")


# ============================================================
# Example 6: cache (unbounded lru_cache)
# ============================================================
print("\n=== cache ===")

from functools import cache

@cache
def factorial(n):
    if n <= 1:
        return 1
    return n * factorial(n - 1)

print(f"factorial(5): {factorial(5)}")
print(f"factorial(10): {factorial(10)}")


# ============================================================
# Example 7: Real-World: Memoization
# ============================================================
print("\n=== Real-World: API Rate Limit ===")

from functools import cache
import time

class APIClient:
    def __init__(self):
        self._cache = {}
    
    @cache
    def get_user(self, user_id):
        print(f"Fetching user {user_id}...")
        time.sleep(0.1)
        return {"id": user_id, "name": f"User {user_id}"}

client = APIClient()
print(client.get_user(1))
print(client.get_user(1))  # Cached
