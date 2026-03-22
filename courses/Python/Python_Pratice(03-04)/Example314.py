# Example314: More Sorting Patterns
# Sorted with reverse
print("Sorting with reverse:")
data = [3, 1, 4, 1, 5]
print(f"Reverse: {sorted(data, reverse=True)}")

# Sort stable
print("\nStable sort:")
data = [('a', 2), ('b', 1), ('c', 2)]
print(f"Stable: {sorted(data, key=lambda x: x[1])}")

# Sort by multiple keys
print("\nMultiple keys:")
data = [(1, 'b'), (2, 'a'), (1, 'a')]
print(f"Sorted: {sorted(data)}")

# Sort with lambda
print("\nLambda key:")
words = ['cat', 'elephant', 'dog']
print(f"By length: {sorted(words, key=len)}")
