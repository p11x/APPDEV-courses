# Example225.py
# Topic: functools Higher-Order Functions

# This file demonstrates functools module functions.


# ============================================================
# Example 1: reduce
# ============================================================
print("=== reduce ===")

from functools import reduce

result = reduce(lambda x, y: x + y, [1, 2, 3, 4])
print(f"Sum: {result}")


# ============================================================
# Example 2: lru_cache
# ============================================================
print("\n=== lru_cache ===")

from functools import lru_cache

@lru_cache(maxsize=None)
def fib(n):
    if n < 2:
        return n
    return fib(n-1) + fib(n-2)

print(f"Fib 20: {fib(20)}")


# ============================================================
# Example 3: partial
# ============================================================
print("\n=== partial ===")

from functools import partial

def power(base, exponent):
    return base ** exponent

square = partial(power, exponent=2)
cube = partial(power, exponent=3)

print(f"Square 5: {square(5)}")
print(f"Cube 5: {cube(5)}")


# ============================================================
# Example 4: singledispatch
# ============================================================
print("\n=== singledispatch ===")

from functools import singledispatch

@singledispatch
def process(x):
    return str(x)

@process.register(int)
def _(x):
    return f"int: {x}"

@process.register(str)
def _(x):
    return f"str: {x}"

print(process(5))
print(process("hello"))


# ============================================================
# Example 5: total_ordering
# ============================================================
print("\n=== total_ordering ===")

from functools import total_ordering

@total_ordering
class Number:
    def __init__(self, value):
        self.value = value
    
    def __eq__(self, other):
        return self.value == other.value
    
    def __lt__(self, other):
        return self.value < other.value

n1 = Number(5)
n2 = Number(10)
print(f"5 < 10: {n1 < n2}")
print(f"5 <= 10: {n1 <= n2}")
print(f"5 > 10: {n1 > n2}")


# ============================================================
# Example 6: cached_property
# ============================================================
print("\n=== cached_property ===")

from functools import cached_property

class Circle:
    def __init__(self, radius):
        self.radius = radius
    
    @cached_property
    def area(self):
        print("Calculating...")
        return 3.14159 * self.radius ** 2

c = Circle(5)
print(c.area)
print(c.area)


# ============================================================
# Example 7: cmp_to_key
# ============================================================
print("\n=== cmp_to_key ===")

from functools import cmp_to_key

def compare(a, b):
    return b - a

sorted_list = sorted([3, 1, 4, 1, 5], key=cmp_to_key(compare))
print(f"Sorted: {sorted_list}")


# ============================================================
# Example 8: wraps
# ============================================================
print("\n=== wraps ===")

from functools import wraps

def decorator(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@decorator
def my_func():
    """My function doc"""
    pass

print(f"Name: {my_func.__name__}")
print(f"Doc: {my_func.__doc__}")


# ============================================================
# Example 9: reduce with initial
# ============================================================
print("\n=== reduce with initial ===")

from functools import reduce

result = reduce(lambda x, y: x + y, [1, 2, 3], 10)
print(f"Sum with initial: {result}")


# ============================================================
# Example 10: update_wrapper
# ============================================================
print("\n=== update_wrapper ===")

from functools import update_wrapper

def wrapper():
    pass

wrapper.__name__ = "original"
wrapper.__doc__ = "doc"

def inner():
    pass

update_wrapper(inner, wrapper)
print(f"Name: {inner.__name__}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
FUNCTOOLS:
- reduce: accumulate
- lru_cache: memoization
- partial: curry
- singledispatch: overloading
- cached_property: lazy
""")
