# Example232: itertools - chain, chain.from_iterable
import itertools

# chain(*iterables) - chains multiple iterables together
print("chain - connect multiple iterables:")
list1 = [1, 2, 3]
list2 = [4, 5, 6]
list3 = [7, 8, 9]
result = list(itertools.chain(list1, list2, list3))
print(f"chain([1,2,3], [4,5,6], [7,8,9]): {result}")

# chain with strings
print("\nchain with strings:")
result = list(itertools.chain("ABC", "DEF"))
print(f"chain('ABC', 'DEF'): {result}")

# chain.from_iterable(iterable) - chains nested iterables
print("\nchain.from_iterable - flatten nested iterables:")
nested = [[1, 2, 3], [4, 5], [6, 7, 8]]
result = list(itertools.chain.from_iterable(nested))
print(f"chain.from_iterable([[1,2,3], [4,5], [6,7,8]]): {result}")

# Practical example: combining multiple data sources
print("\nPractical - combine data from multiple sources:")
headers = ['name', 'age', 'city']
row1 = ['Alice', '30', 'NYC']
row2 = ['Bob', '25', 'LA']

combined = list(itertools.chain(headers, row1, row2))
print(f"Combined: {combined}")

# Flatten a list of tuples
print("\nFlatten list of tuples:")
tuples = [(1, 2), (3, 4), (5, 6)]
result = list(itertools.chain.from_iterable(tuples))
print(f"Flattened: {result}")

# Use with repeat for padding
print("\nChain with repeat for padding:")
default_value = [None]
data = [1, 2, 3]
padded = list(itertools.chain(data, itertools.repeat(None, 5 - len(data))))
print(f"Pad to 5: {padded}")
