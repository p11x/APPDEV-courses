# Example269: More functools Tools
from functools import cmp_to_key, lru_cache, partial

# cmp_to_key - convert comparison function to key function
print("cmp_to_key:")

def compare_strings(a, b):
    if len(a) < len(b):
        return -1
    elif len(a) > len(b):
        return 1
    return 0

words = ["apple", "pie", "banana", "a", "cat"]
sorted_words = sorted(words, key=cmp_to_key(compare_strings))
print(f"Sorted by length: {sorted_words}")

# lru_cache with typed arguments
print("\nlru_cache with typed args:")
@lru_cache(maxsize=128)
def fib_cached(n):
    if n < 2:
        return n
    return fib_cached(n - 1) + fib_cached(n - 2)

print(f"fib_cached(30): {fib_cached(30)}")
print(f"Cache info: {fib_cached.cache_info()}")

# Cache with custom key
from functools import cached_property

class DataProcessor:
    def __init__(self, data):
        self.data = data
    
    @cached_property
    def processed(self):
        print("Processing...")
        return [x * 2 for x in self.data]

print("\ncached_property:")
dp = DataProcessor([1, 2, 3])
print(f"First access: {dp.processed}")
print(f"Second access (cached): {dp.processed}")

# partialmethod
class Temperature:
    def __init__(self, celsius):
        self.celsius = celsius
    
    def _to_fahrenheit(self, c):
        return c * 9/5 + 32
    
    to_fahrenheit = partialmethod(_to_fahrenheit)
    
    def _to_kelvin(self, c):
        return c + 273.15
    
    to_kelvin = partialmethod(_to_kelvin)

print("\npartialmethod:")
t = Temperature(25)
print(f"25°C to Fahrenheit: {t.to_fahrenheit()}")
print(f"25°C to Kelvin: {t.to_kelvin()}")

# reduce with initial
from functools import reduce
print("\nreduce with initial:")
numbers = [1, 2, 3, 4]
result = reduce(lambda a, b: a + b, numbers, 10)
print(f"Sum with initial 10: {result}")
