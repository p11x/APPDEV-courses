# Example305: Itertools Product and Combinations
import itertools

print("Itertools Product:")
for p in itertools.product([1, 2], ['a', 'b']):
    print(f"  {p}")

print("\nCombinations:")
for c in itertools.combinations([1, 2, 3], 2):
    print(f"  {c}")

print("\nCombinations with replacement:")
for c in itertools.combinations_with_replacement([1, 2], 2):
    print(f"  {c}")

print("\nPermutations:")
for p in itertools.permutations([1, 2, 3], 2):
    print(f"  {p}")
