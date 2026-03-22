# Example297: Working with Modules
import math
from collections import Counter, defaultdict
import random

print("Module Usage:")
print(f"Math sqrt: {math.sqrt(16)}")
print(f"Math pi: {math.pi}")
print(f"Math factorial: {math.factorial(5)}")

print("\nCollections:")
counter = Counter(['a', 'b', 'a', 'c', 'a'])
print(f"Counter: {counter}")
dd = defaultdict(list)
dd['key'].append(1)
print(f"Defaultdict: {dict(dd)}")

print("\nRandom:")
print(f"Random choice: {random.choice([1,2,3])}")
print(f"Random shuffle: {random.sample([1,2,3,4], 2)}")
