# Example278: Itertools Starmap and Cycles
import itertools

# starmap - like map but unpacks arguments
print("itertools.starmap:")
data = [(2, 3), (3, 4), (4, 5)]
result = list(itertools.starmap(pow, data))
print(f"starmap(pow, [(2,3), (3,4), (4,5)]): {result}")

# product - cartesian product
print("\nitertools.product:")
a = [1, 2]
b = ['a', 'b']
result = list(itertools.product(a, b))
print(f"Product [1,2] x [a,b]: {result}")

# combinations and permutations
print("\nCombinations:")
print(f"combinations [1,2,3], 2: {list(itertools.combinations([1,2,3], 2))}")

print("\nPermutations:")
print(f"permutations [1,2,3], 2: {list(itertools.permutations([1,2,3], 2))}")

# cycle with finite limit
print("\nFinite cycle:")
count = 0
for item in itertools.cycle(['A', 'B']):
    if count >= 6:
        break
    print(item, end=" ")
    count += 1
print()

# compress
print("\nCompress:")
letters = 'ABCDEFG'
bools = [True, False, True, True, False, False, True]
print(f"Compress: {list(itertools.compress(letters, bools))}")

# dropwhile and takewhile
print("\nDropwhile and takewhile:")
data = [1, 2, 3, 4, 5, 6]
print(f"dropwhile <= 3: {list(itertools.dropwhile(lambda x: x <= 3, data))}")
print(f"takewhile <= 3: {list(itertools.takewhile(lambda x: x <= 3, data))}")

# groupby with more examples
print("\nGroupby:")
data = [1, 1, 2, 2, 2, 3, 1]
for k, g in itertools.groupby(data):
    print(f"Key: {k}, Group: {list(g)}")
