# Example75.py
# Topic: Comprehensive Review - functools

# This file provides a comprehensive review of functools module.


# ============================================================
# Example 1: Caching decorators
# ============================================================
print("=== functools Caching ===")

from functools import lru_cache, cache, cached_property

@lru_cache(maxsize=128)
def fib_lru(n):
    """LRU cache with size limit."""
    if n < 2:
        return n
    return fib_lru(n-1) + fib_lru(n-2)

@cache
def fib_cache(n):
    """Unlimited cache (Python 3.9+)."""
    if n < 2:
        return n
    return fib_cache(n-1) + fib_cache(n-2)

print(f"fib_lru(50): {fib_lru(50)}")
print(f"Cache info: {fib_lru.cache_info()}")


# ============================================================
# Example 2: cached_property
# ============================================================
print("\n=== cached_property ===")

from functools import cached_property

class Circle:
    def __init__(self, radius):
        self.radius = radius
    
    @cached_property
    def diameter(self):
        return self.radius * 2
    
    @cached_property
    def area(self):
        return 3.14159 * self.radius ** 2

c = Circle(10)
print(f"Diameter: {c.diameter}")
print(f"Area: {c.area}")


# ============================================================
# Example 3: partial
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
# Example 4: reduce
# ============================================================
print("\n=== reduce ===")

from functools import reduce

numbers = [1, 2, 3, 4, 5]

print(f"Sum: {reduce(lambda a, b: a + b, numbers)}")
print(f"Product: {reduce(lambda a, b: a * b, numbers)}")
print(f"Max: {reduce(lambda a, b: a if a > b else b, numbers)}")


# ============================================================
# Example 5: wraps
# ============================================================
print("\n=== wraps ===")

from functools import wraps

def my_decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@my_decorator
def add(a, b):
    """Adds two numbers."""
    return a + b

print(f"Name: {add.__name__}")
print(f"Doc: {add.__doc__}")


# ============================================================
# Example 6: singledispatch
# ============================================================
print("\n=== singledispatch ===")

from functools import singledispatch

@singledispatch
def process(data):
    print(f"Default: {data}")

@process.register(int)
def process_int(data):
    print(f"Int: {data * 2}")

@process.register(str)
def process_str(data):
    print(f"Str: {data.upper()}")

process(10)
process("hello")


# ============================================================
# Example 7: total_ordering
# ============================================================
print("\n=== total_ordering ===")

from functools import total_ordering

@total_ordering
class Temperature:
    def __init__(self, celsius):
        self.celsius = celsius
    
    def __eq__(self, other):
        return self.celsius == other.celsius
    
    def __lt__(self, other):
        return self.celsius < other.celsius

t1 = Temperature(20)
t2 = Temperature(25)

print(f"t1 < t2: {t1 < t2}")
print(f"t1 > t2: {t1 > t2}")
print(f"t1 <= t2: {t1 <= t2}")
print(f"t1 >= t2: {t1 >= t2}")


# ============================================================
# Example 8: cmp_to_key
# ============================================================
print("\n=== cmp_to_key ===")

from functools import cmp_to_key

def compare(a, b):
    return -1 if a < b else (1 if a > b else 0)

numbers = [5, 2, 8, 1, 9]
sorted_numbers = sorted(numbers, key=cmp_to_key(compare))
print(f"Sorted: {sorted_numbers}")


# ============================================================
# Example 9: reduce with initial value
# ============================================================
print("\n=== reduce with initial ===")

from functools import reduce

numbers = [1, 2, 3]

# Without initial
print(f"Sum: {reduce(lambda a, b: a + b, numbers)}")

# With initial
print(f"Sum (init=10): {reduce(lambda a, b: a + b, numbers, 10)}")


# ============================================================
# Example 10: Chaining functools tools
# ============================================================
print("\n=== Chaining functools tools ===")

from functools import partial, lru_cache, reduce

@lru_cache(maxsize=100)
def fetch_data(endpoint):
    return f"Data from {endpoint}"

# Create specialized fetchers
fetch_users = partial(fetch_data, "users")
fetch_posts = partial(fetch_data, "posts")

print(f"Users: {fetch_users()}")
print(f"Posts: {fetch_posts()}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY: functools")
print("=" * 50)
print("""
CACHING:
  @lru_cache(maxsize=128)  - LRU cache with size limit
  @cache                    - Unlimited cache (Python 3.9+)
  @cached_property          - Cached property (Python 3.8+)

PARTIAL:
  partial(func, *args, **kwargs)
  - Pre-fill function arguments

REDUCE:
  reduce(func, iterable, initial)
  - Aggregate items into single value

DECORATOR HELPERS:
  @wraps(func)              - Preserve function metadata
  @total_ordering           - Auto-generate comparisons
  @singledispatch           - Function overloading

KEY CONVERSION:
  cmp_to_key(cmp_func)     - Convert cmp to key function

OTHER:
  reduce, update_wrapper, WRAPPER_ASSIGNMENTS
""")
