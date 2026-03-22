# Example249: ChainMap - Multiple Dictionaries
from collections import ChainMap

# Basic ChainMap
print("Basic ChainMap:")
dict1 = {'a': 1, 'b': 2}
dict2 = {'b': 3, 'c': 4}
dict3 = {'c': 5, 'd': 6}

cm = ChainMap(dict1, dict2, dict3)
print(f"Keys: {list(cm.keys())}")
print(f"Values: {list(cm.values())}")
print(f"Get 'a': {cm['a']}")
print(f"Get 'b': {cm['b']}")  # First found
print(f"Get 'c': {cm['c']}")

# Maps are searched left to right
print("\nMaps list:")
print(f"Number of maps: {len(cm.maps)}")
for i, m in enumerate(cm.maps):
    print(f"  Map {i}: {m}")

# New child scope
print("\nNew child map:")
child = cm.new_child({'d': 7, 'e': 8})
print(f"Child keys: {list(child.keys())}")
print(f"Child['d']: {child['d']}")

# Without new child (no change to original)
print("\nWithout child:")
print(f"Original 'd': {cm.get('d')}")

# Practical: config with defaults
print("\nPractical: config with defaults:")
default_config = {'debug': False, 'port': 8080, 'host': 'localhost'}
user_config = {'port': 3000}
env_config = {'debug': True}

config = ChainMap(env_config, user_config, default_config)
print(f"Port: {config['port']}")  # 3000 from user
print(f"Debug: {config['debug']}")  # True from env
print(f"Host: {config['host']}")  # localhost from default

# Updating affects first map only
print("\nUpdating first map:")
config['port'] = 9000
print(f"User config: {user_config}")

# Deleting from first map
print("\nDeleting:")
del config['debug']
print(f"Env config: {env_config}")

# Iterate over ChainMap
print("\nIteration:")
for key in cm:
    print(f"  {key}: {cm[key]}")
