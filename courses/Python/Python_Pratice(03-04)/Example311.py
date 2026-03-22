# Example311: Defaultdict Patterns
from collections import defaultdict

# Basic
print("Defaultdict Basics:")
d = defaultdict(list)
d['fruits'].append('apple')
d['fruits'].append('banana')
print(f"Dict: {dict(d)}")

# With int (counter)
print("\nCounter:")
d = defaultdict(int)
for word in ['apple', 'banana', 'apple', 'cherry']:
    d[word] += 1
print(f"Counts: {dict(d)}")

# Nested
print("\nNested:")
tree = lambda: defaultdict(tree)
root = tree()
root['a']['b']['c'] = 1
print(f"Nested: {root}")

# With set
print("\nWith set:")
d = defaultdict(set)
d['tags'].add('python')
d['tags'].add('coding')
print(f"Sets: {dict(d)}")
