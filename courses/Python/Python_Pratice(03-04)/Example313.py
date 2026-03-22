# Example313: ChainMap Patterns
from collections import ChainMap

# Basic
print("ChainMap:")
dict1 = {'a': 1, 'b': 2}
dict2 = {'b': 3, 'c': 4}
cm = ChainMap(dict1, dict2)
print(f"Keys: {list(cm.keys())}")
print(f"Get 'a': {cm['a']}")
print(f"Get 'b': {cm['b']}")

# New child
print("\nNew child:")
child = cm.new_child({'d': 5})
print(f"Child keys: {list(child.keys())}")

# Maps list
print("\nMaps:")
for i, m in enumerate(cm.maps):
    print(f"  Map {i}: {m}")
