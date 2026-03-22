# Example224.py
# Topic: itertools Functions

# This file demonstrates itertools functions.


# ============================================================
# Example 1: count
# ============================================================
print("=== count ===")

import itertools

for i in itertools.count(5, 2):
    if i > 12:
        break
    print(i, end=" ")
print()


# ============================================================
# Example 2: cycle
# ============================================================
print("\n=== cycle ===")

import itertools

cycle = itertools.cycle([1, 2, 3])
for i, x in enumerate(cycle):
    if i >= 7:
        break
    print(x, end=" ")
print()


# ============================================================
# Example 3: repeat
# ============================================================
print("\n=== repeat ===")

import itertools

r = itertools.repeat("hello", 3)
print(list(r))


# ============================================================
# Example 4: chain
# ============================================================
print("\n=== chain ===")

import itertools

result = itertools.chain([1, 2], [3, 4], [5])
print(list(result))


# ============================================================
# Example 5: islice
# ============================================================
print("\n=== islice ===")

import itertools

result = itertools.islice(range(10), 2, 8, 2)
print(list(result))


# ============================================================
# Example 6: tee
# ============================================================
print("\n=== tee ===")

import itertools

it1, it2 = itertools.tee([1, 2, 3])
print(list(it1))
print(list(it2))


# ============================================================
# Example 7: product
# ============================================================
print("\n=== product ===")

import itertools

result = itertools.product([1, 2], [3, 4])
print(list(result))


# ============================================================
# Example 8: permutations
# ============================================================
print("\n=== permutations ===")

import itertools

result = itertools.permutations([1, 2, 3], 2)
print(list(result))


# ============================================================
# Example 9: combinations
# ============================================================
print("\n=== combinations ===")

import itertools

result = itertools.combinations([1, 2, 3], 2)
print(list(result))


# ============================================================
# Example 10: groupby
# ============================================================
print("\n=== groupby ===")

import itertools

data = [1, 1, 2, 2, 2, 3]
for key, group in itertools.groupby(data):
    print(f"{key}: {list(group)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ITERTOOLS:
- count(): infinite counter
- cycle(): repeat infinitely
- repeat(): repeat element
- chain(): chain iterables
- islice(): slice iterator
- product: cartesian product
- permutations, combinations
""")
