# Example318: Practice with Tuples
# Tuple operations
print("Tuple Operations:")
t = (1, 2, 3)
print(f"First: {t[0]}")
print(f"Slice: {t[1:]}")

# Unpack
print("\nUnpack:")
a, b, c = (1, 2, 3)
print(f"a={a}, b={b}, c={c}")

# Swap
print("\nSwap:")
a, b = 1, 2
a, b = b, a
print(f"a={a}, b={b}")

# Named tuple
print("\nNamed Tuple:")
from collections import namedtuple
Point = namedtuple('Point', ['x', 'y'])
p = Point(1, 2)
print(f"Point: {p.x}, {p.y}")
