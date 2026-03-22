# Example245: Defaultdict - Advanced Patterns
from collections import defaultdict

# Basic defaultdict
print("Basic defaultdict:")
d = defaultdict(list)
d['fruits'].append('apple')
d['fruits'].append('banana')
print(f"dict: {dict(d)}")

# With int (automatic counter)
print("\nAutomatic counter:")
d = defaultdict(int)
words = "the quick brown fox jumps over the lazy dog the fox".split()
for word in words:
    d[word] += 1
print(f"Word counts: {dict(d)}")

# With set (unique elements)
print("\nAutomatic set:")
d = defaultdict(set)
d['fruits'].add('apple')
d['fruits'].add('banana')
d['fruits'].add('apple')  # won't duplicate
print(f"Sets: {dict(d)}")

# With lambda for custom default
print("\nCustom default with lambda:")
d = defaultdict(lambda: 'N/A')
d['name'] = 'Alice'
print(f"name: {d['name']}")
print(f"age: {d['age']}")  # Returns default

# Practical: group by
print("\nGroup by function:")
data = [1, 10, 20, 30, 100, 200]
d = defaultdict(list)
for num in data:
    d[len(str(num))].append(num)
print(f"Grouped by digit count: {dict(d)}")

# Practical: nested defaultdict
print("\nNested defaultdict:")
tree = lambda: defaultdict(tree)
root = tree()
root['users']['alice']['email'] = 'alice@example.com'
root['users']['bob']['email'] = 'bob@example.com'
print(f"alice email: {root['users']['alice']['email']}")

# Using with complex objects
print("\nWith complex objects:")
class Graph:
    def __init__(self):
        self.edges = []
graph = defaultdict(graph)
# This creates issues, better to use:
graph2 = defaultdict(lambda: {'neighbors': [], 'visited': False})
graph2['A']['neighbors'].append('B')
print(f"Graph: {dict(graph2)}")

# Tracking order with defaultdict
print("\nTrack insertion order:")
d = defaultdict(int)
items = ['apple', 'banana', 'apple', 'cherry', 'banana', 'apple']
for item in items:
    d[item] += 1
print(f"Counts: {dict(d)}")

# Partitioning data
print("\nPartition by condition:")
data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
d = defaultdict(list)
for num in data:
    key = 'even' if num % 2 == 0 else 'odd'
    d[key].append(num)
print(f"Even: {d['even']}")
print(f"Odd: {d['odd']}")
