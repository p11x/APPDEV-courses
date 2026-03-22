# Example304: Counter Advanced
from collections import Counter

print("Counter Advanced:")
c = Counter(['a', 'b', 'a', 'c', 'b', 'a'])
print(f"Basic: {dict(c)}")

# Most common
print(f"\nMost common 2: {c.most_common(2)}")

# Elements
print(f"Elements: {list(c.elements())}")

# Arithmetic
c2 = Counter(['a', 'b', 'c'])
print(f"\nCounter +: {dict(c + c2)}")
print(f"Counter -: {dict(c - c2)}")
print(f"Counter &: {dict(c & c2)}")
print(f"Counter |: {dict(c | c2)}")

# Update
c3 = Counter()
c3.update(['a', 'b', 'a'])
print(f"\nAfter update: {dict(c3)}")
