# Example248: OrderedDict - Advanced Patterns
from collections import OrderedDict

# Basic OrderedDict
print("Basic OrderedDict:")
od = OrderedDict()
od['a'] = 1
od['b'] = 2
od['c'] = 3
print(f"Ordered: {od}")
print(f"Keys: {list(od.keys())}")

# Order is preserved
print("\nOrder preservation:")
od2 = OrderedDict()
od2['first'] = 1
od2['second'] = 2
od2['third'] = 3
print(f"Order: {list(od2.items())}")

# Move to end / move to front
print("\nMove operations:")
od = OrderedDict({'a': 1, 'b': 2, 'c': 3})
od.move_to_end('a')
print(f"After move_to_end('a'): {list(od.keys())}")
od.move_to_end('a', last=False)
print(f"After move_to_end('a', last=False): {list(od.keys())}")

# popitem with last=True/False
print("\nPopitem:")
od = OrderedDict({'a': 1, 'b': 2, 'c': 3})
print(f"Pop last: {od.popitem()}")
print(f"Remaining: {od}")
od = OrderedDict({'a': 1, 'b': 2, 'c': 3})
print(f"Pop first: {od.popitem(last=False)}")
print(f"Remaining: {od}")

# LRU Cache with OrderedDict
print("\nLRU Cache implementation:")
class LRUCache:
    def __init__(self, capacity):
        self.capacity = capacity
        self.cache = OrderedDict()
    
    def get(self, key):
        if key in self.cache:
            self.cache.move_to_end(key)
            return self.cache[key]
        return -1
    
    def put(self, key, value):
        if key in self.cache:
            self.cache.move_to_end(key)
        self.cache[key] = value
        if len(self.cache) > self.capacity:
            self.cache.popitem(last=False)

cache = LRUCache(3)
cache.put('a', 1)
cache.put('b', 2)
cache.put('c', 3)
print(f"Get a: {cache.get('a')}")
cache.put('d', 4)  # Evicts 'b'
print(f"Get b: {cache.get('b')}")  # -1

# Sort by value maintaining insertion order
print("\nSort while maintaining order:")
od = OrderedDict({'b': 2, 'a': 1, 'c': 3})
sorted_od = OrderedDict(sorted(od.items(), key=lambda x: x[1]))
print(f"Sorted by value: {sorted_od}")
