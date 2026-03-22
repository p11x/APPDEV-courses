# Example73.py
# Topic: Additional functools utilities

# This file demonstrates more functools utilities.


# ============================================================
# Example 1: reduce (already covered, brief recap)
# ============================================================
print("=== functools.reduce ===")

from functools import reduce

numbers = [1, 2, 3, 4, 5]

total = reduce(lambda a, b: a + b, numbers)
print(f"Sum: {total}")

# With initial
total_100 = reduce(lambda a, b: a + b, numbers, 100)
print(f"Sum with initial 100: {total_100}")


# ============================================================
# Example 2: wraps decorator
# ============================================================
print("\n=== functools.wraps ===")

from functools import wraps

def my_decorator(func):
    @wraps(func)  # Preserves original function metadata
    def wrapper(*args, **kwargs):
        print("Before")
        result = func(*args, **kwargs)
        print("After")
        return result
    return wrapper

@my_decorator
def add(a, b):
    """This adds two numbers."""
    return a + b

print(f"Function name: {add.__name__}")
print(f"Docstring: {add.__doc__}")


# ============================================================
# Example 3: singledispatch
# ============================================================
print("=== functools.singledispatch ===")

from functools import singledispatch

@singledispatch
def process(data):
    print(f"Default: {data}")

@process.register(int)
def process_int(data):
    print(f"Integer: {data * 2}")

@process.register(str)
def process_str(data):
    print(f"String: {data.upper()}")

@process.register(list)
def process_list(data):
    print(f"List: {data[::-1]}")

process(10)      # Calls int version
process("hello") # Calls str version
process([1,2,3]) # Calls list version
process(3.14)    # Default version


# ============================================================
# Example 4: total_ordering
# ============================================================
print("\n=== functools.total_ordering ===")

from functools import total_ordering

@total_ordering
class Version:
    def __init__(self, major, minor, patch=0):
        self.version = (major, minor, patch)
    
    def __eq__(self, other):
        return self.version == other.version
    
    def __lt__(self, other):
        return self.version < other.version
    
    def __repr__(self):
        return f"Version{'.'.join(map(str, self.version))}"

v1 = Version(1, 2)
v2 = Version(1, 3)
v3 = Version(2, 0)

print(f"v1 < v2: {v1 < v2}")
print(f"v2 > v1: {v2 > v1}")
print(f"v1 <= v3: {v1 <= v3}")
print(f"v3 >= v2: {v3 >= v2}")
print(f"v1 == Version(1, 2): {v1 == Version(1, 2)}")


# ============================================================
# Example 5: cmp_to_key
# ============================================================
print("\n=== functools.cmp_to_key ===")

from functools import cmp_to_key

def compare_versions(v1, v2):
    """Compare version strings."""
    parts1 = list(map(int, v1.split('.')))
    parts2 = list(map(int, v2.split('.')))
    
    for p1, p2 in zip(parts1, parts2):
        if p1 < p2:
            return -1
        elif p1 > p2:
            return 1
    
    if len(parts1) < len(parts2):
        return -1
    elif len(parts1) > len(parts2):
        return 1
    return 0

versions = ["1.10", "1.2", "2.0", "1.9", "1.1"]
sorted_versions = sorted(versions, key=cmp_to_key(compare_versions))
print(f"Sorted versions: {sorted_versions}")


# ============================================================
# Example 6: lru_cache with typed
# ============================================================
print("\n=== lru_cache with typed=True ===")

from functools import lru_cache

@lru_cache(maxsize=10, typed=True)
def func(a, b):
    print(f"Computing: {a}, {b}")
    return a + b

# Different types treated differently
func(1, 2)      # Computes
func(1.0, 2.0) # Computes (different type)
func(1, 2)     # Cached


# ============================================================
# Example 7: cache_info and cache_statistics
# ============================================================
print("\n=== Cache info ===")

from functools import lru_cache

@lru_cache(maxsize=5)
def expensive(n):
    return n ** 2

# Use cache
expensive(1)
expensive(2)
expensive(3)
expensive(1)  # Cache hit
expensive(1)  # Cache hit
expensive(4)

print(f"Cache info: {expensive.cache_info()}")


# ============================================================
# Example 8: partialmethod
# ============================================================
print("\n=== functools.partialmethod ===")

from functools import partialmethod

class Database:
    def __init__(self, host, port):
        self.host = host
        self.port = port
    
    def _connect(self, protocol, timeout):
        return f"{protocol}://{self.host}:{self.port} (timeout={timeout}s)"
    
    connect = partialmethod(_connect, "tcp", timeout=30)
    connect_ssl = partialmethod(_connect, "ssl", timeout=60)

db = Database("localhost", 5432)
print(db.connect())
print(db.connect_ssl())


# ============================================================
# Example 9: singledispatchmethod
# ============================================================
print("\n=== singledispatchmethod ===")

from functools import singledispatchmethod

class Parser:
    @singledispatchmethod
    def parse(self, data):
        print(f"Default: {data}")
    
    @parse.register(int)
    def _(self, data):
        print(f"Parse int: {data * 2}")
    
    @parse.register(str)
    def _(self, data):
        print(f"Parse str: {data.strip()}")

p = Parser()
p.parse(10)
p.parse("  hello  ")
p.parse([1, 2, 3])


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Additional functools")
print("=" * 50)
print("""
@wraps:
  - Preserves function metadata in decorators

@singledispatch:
  - Function overloading by type

@total_ordering:
  - Auto-generates comparison methods

cmp_to_key:
  - Convert comparison function to key function

@cache vs @lru_cache:
  - cache: unlimited size
  - lru_cache: with maxsize and eviction
""")
