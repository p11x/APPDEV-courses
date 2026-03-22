# Example268: More itertools Combinations
import itertools

# Combinations
print("itertools - Combinations:")
print("Combinations of [1,2,3] choose 2:")
for combo in itertools.combinations([1, 2, 3], 2):
    print(f"  {combo}")

# Combinations with replacement
print("\nCombinations with replacement:")
for combo in itertools.combinations_with_replacement([1, 2, 3], 2):
    print(f"  {combo}")

# Permutations
print("\nPermutations:")
for perm in itertools.permutations([1, 2, 3], 2):
    print(f"  {perm}")

# Product
print("\nProduct:")
for prod in itertools.product([1, 2], ['a', 'b']):
    print(f"  {prod}")

# Infinite iterators - cycle
print("\nCycle (limited):")
count = 0
for item in itertools.cycle(['A', 'B', 'C']):
    if count >= 7:
        break
    print(f"  {item}", end=" ")
    count += 1
print()

# Grouper
def grouper(n, iterable, fillvalue=None):
    args = [iter(iterable)] * n
    return itertools.zip_longest(*args, fillvalue=fillvalue)

print("\nGrouper:")
data = [1, 2, 3, 4, 5, 6, 7]
for group in grouper(3, data):
    print(f"  {group}")

# Compress
print("\nCompress:")
data = ['A', 'B', 'C', 'D', 'E']
selectors = [True, False, True, False, True]
result = list(itertools.compress(data, selectors))
print(f"Data: {data}, Selectors: {selectors}")
print(f"Compressed: {result}")

# Slice
print("\nislice:")
data = range(10)
print(f"Take 3: {list(itertools.islice(data, 3))}")
print(f"Slice 2-6: {list(itertools.islice(data, 2, 6))}")
