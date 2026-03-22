# Example161.py
# Topic: Itertools Advanced


# ============================================================
# Example 1: Infinite Iterators
# ============================================================
print("=== Infinite Iterators ===")

import itertools

print("count(10):")
for i, num in enumerate(itertools.count(10, 2)):
    if i >= 5:
        break
    print(num, end=" ")
print()

print("\ncycle([1,2,3]):")
cycler = itertools.cycle([1, 2, 3])
for i in range(6):
    print(next(cycler), end=" ")
print()

print("\nrepeat(5, 3):")
print(list(itertools.repeat(5, 3)))


# ============================================================
# Example 2: Iterators that Terminate
# ============================================================
print("\n=== Terminating Iterators ===")

import itertools

print("accumulate([1,2,3,4,5]):")
print(list(itertools.accumulate([1, 2, 3, 4, 5])))

print("\nchain(['a','b'], ['c','d']):")
print(list(itertools.chain(['a', 'b'], ['c', 'd'])))

print("\nchain.from_iterable([[1,2],[3,4]]):")
print(list(itertools.chain.from_iterable([[1, 2], [3, 4]])))


# ============================================================
# Example 3: Filtering Iterators
# ============================================================
print("\n=== Filtering Iterators ===")

import itertools

print("compress([1,2,3,4], [1,0,1,1]):")
print(list(itertools.compress([1, 2, 3, 4], [1, 0, 1, 1])))

print("\ndropwhile(lambda x: x<3, [1,2,3,4,5]):")
print(list(itertools.dropwhile(lambda x: x < 3, [1, 2, 3, 4, 5])))

print("\ntakewhile(lambda x: x<3, [1,2,3,4,5]):")
print(list(itertools.takewhile(lambda x: x < 3, [1, 2, 3, 4, 5])))

print("\nfilterfalse(lambda x: x%2==0, [1,2,3,4]):")
print(list(itertools.filterfalse(lambda x: x % 2 == 0, [1, 2, 3, 4])))


# ============================================================
# Example 4: Combinatoric Iterators
# ============================================================
print("\n=== Combinatoric Iterators ===")

import itertools

print("permutations('ABC', 2):")
print(list(itertools.permutations('ABC', 2)))

print("\ncombinations('ABC', 2):")
print(list(itertools.combinations('ABC', 2)))

print("\ncombinations_with_replacement('AB', 2):")
print(list(itertools.combinations_with_replacement('AB', 2)))

print("\nproduct('AB', '12'):")
print(list(itertools.product('AB', '12')))


# ============================================================
# Example 5: Grouping
# ============================================================
print("\n=== Grouping ===")

import itertools

data = [1, 1, 2, 2, 2, 3, 3]
print(f"Data: {data}")

for key, group in itertools.groupby(data):
    print(f"Key: {key}, Group: {list(group)}")

animals = ["dog", "duck", "dove", "rabbit"]
print("\nAnimals grouped by first letter:")
animals.sort()
for key, group in itertools.groupby(animals, key=lambda x: x[0]):
    print(f"{key}: {list(group)}")


# ============================================================
# Example 6: Slice
# ============================================================
print("\n=== Slice ===")

import itertools

data = range(10)
print(f"range(10): {list(data)}")

sliced = list(itertools.islice(range(10), 2, 8, 2))
print(f"islice(range(10), 2, 8, 2): {sliced}")


# ============================================================
# Example 7: Real-World: Batch Processing
# ============================================================
print("\n=== Real-World: Batched ===")

import itertools

items = list(range(1, 26))
batch_size = 5

print("Processing in batches:")
for i, batch in enumerate(itertools.batched(items, batch_size), 1):
    print(f"Batch {i}: {batch}")


# ============================================================
# Example 8: Real-World: Running Statistics
# ============================================================
print("\n=== Running Statistics ===")

import itertools

data = [10, 20, 30, 40, 50]

print("Running sum:")
for i, total in enumerate(itertools.accumulate(data), 1):
    print(f"After {i} items: {total}")

print("\nRunning max:")
for i, m in enumerate(itertools.accumulate(data, max), 1):
    print(f"After {i} items: {m}")
