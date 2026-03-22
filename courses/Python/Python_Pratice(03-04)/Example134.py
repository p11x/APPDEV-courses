# Example134.py
# Topic: Working with Itertools


# ============================================================
# Example 1: count, cycle, repeat
# ============================================================
print("=== Infinite Iterators ===")

import itertools

for i, num in enumerate(itertools.count(10, step=2)):
    if i >= 5:
        break
    print(f"count: {num}")

cycle_list = itertools.cycle([1, 2, 3])
for i in range(6):
    print(f"cycle: {next(cycle_list)}")

print(f"repeat 5 (3 times): {list(itertools.repeat(5, 3))}")


# ============================================================
# Example 2: chain, islice
# ============================================================
print("\n=== Chaining Iterables ===")

import itertools

a = [1, 2, 3]
b = [4, 5, 6]
c = [7, 8, 9]

chained = list(itertools.chain(a, b, c))
print(f"chain: {chained}")

sliced = list(itertools.islice(range(10), 2, 8, 2))
print(f"islice [2:8:2]: {sliced}")


# ============================================================
# Example 3: product, permutations
# ============================================================
print("\n=== Product and Permutations ===")

import itertools

print("Product (2 x 3):")
for p in itertools.product([1, 2], [3, 4, 5]):
    print(f"  {p}")

print("\nPermutations (ABC, 2):")
for p in itertools.permutations("ABC", 2):
    print(f"  {p}", end="")
print()


# ============================================================
# Example 4: combinations
# ============================================================
print("\n=== Combinations ===")

import itertools

print("Combinations (ABC, 2):")
for c in itertools.combinations("ABC", 2):
    print(f"  {c}", end="")
print()

print("\nCombinations with replacement:")
for c in itertools.combinations_with_replacement("AB", 2):
    print(f"  {c}", end="")
print()


# ============================================================
# Example 5: groupby
# ============================================================
print("\n=== Groupby ===")

import itertools

data = [
    {"name": "Alice", "dept": "IT"},
    {"name": "Bob", "dept": "HR"},
    {"name": "Charlie", "dept": "IT"},
]

data.sort(key=lambda x: x["dept"])

for dept, group in itertools.groupby(data, key=lambda x: x["dept"]):
    print(f"Dept: {dept}")
    for person in group:
        print(f"  - {person['name']}")


# ============================================================
# Example 6: zip_longest
# ============================================================
print("\n=== zip_longest ===")

import itertools

a = [1, 2, 3]
b = ["a", "b"]

for item in itertools.zip_longest(a, b, fillvalue="?"):
    print(f"  {item}")


# ============================================================
# Example 7: Real-World: Pagination
# ============================================================
print("\n=== Real-World: Pagination ===")

import itertools

items = list(range(1, 26))
page_size = 5

for page_num, page in enumerate(itertools.batched(items, page_size), 1):
    print(f"Page {page_num}: {page}")
