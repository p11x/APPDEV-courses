# Example312: OrderedDict Patterns
from collections import OrderedDict

# Basic
print("OrderedDict:")
d = OrderedDict()
d['a'] = 1
d['b'] = 2
d['c'] = 3
print(f"Items: {list(d.items())}")

# Move to end
print("\nMove to end:")
d.move_to_end('a')
print(f"After move_to_end('a'): {list(d.keys())}")

# Popitem
print("\nPopitem:")
print(f"Pop last: {d.popitem()}")
print(f"Remaining: {list(d.keys())}")

# LRU Cache
print("\nLRU Cache:")
cache = OrderedDict()
cache['a'] = 1
cache['b'] = 2
cache.move_to_end('a')
print(f"After access: {list(cache.keys())}")
