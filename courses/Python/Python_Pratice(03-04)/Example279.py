# Example279: More Practice with Sets
# Set operations
print("Set Operations:")
a = {1, 2, 3, 4, 5}
b = {4, 5, 6, 7, 8}

print(f"A: {a}")
print(f"B: {b}")
print(f"Union: {a | b}")
print(f"Intersection: {a & b}")
print(f"Difference (A-B): {a - b}")
print(f"Symmetric diff: {a ^ b}")

# Set methods
print("\nSet Methods:")
a = {1, 2, 3}
a.add(4)
print(f"After add: {a}")
a.update([5, 6])
print(f"After update: {a}")
a.remove(6)
print(f"After remove: {a}")
a.discard(10)  # No error if not exists
print(f"After discard: {a}")

# Set comprehension
print("\nSet Comprehension:")
squares = {x**2 for x in range(1, 6)}
print(f"Squares: {squares}")

# Frozen set
print("\nFrozen Set:")
fs = frozenset([1, 2, 3])
print(f"Frozen set: {fs}")
# fs.add(4)  # Error - immutable

# Set with custom objects
print("\nSet with hashable objects:")
class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y
    def __hash__(self):
        return hash((self.x, self.y))
    def __eq__(self, other):
        return self.x == other.x and self.y == other.y

points = {Point(1, 2), Point(3, 4), Point(1, 2)}
print(f"Unique points: {len(points)}")

# Set membership test
print("\nMembership Test:")
a = {1, 2, 3}
print(f"1 in a: {1 in a}")
print(f"5 in a: {5 in a}")

# Subset and superset
print("\nSubset/Superset:")
a = {1, 2, 3, 4}
b = {2, 3}
print(f"b subset a: {b <= a}")
print(f"a superset b: {a >= b}")
