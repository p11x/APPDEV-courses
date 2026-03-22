# Example307: Map, Filter, Reduce
from functools import reduce

# Map
print("Map:")
result = list(map(lambda x: x*2, [1, 2, 3, 4]))
print(f"Doubled: {result}")

# Filter
print("\nFilter:")
result = list(filter(lambda x: x > 2, [1, 2, 3, 4]))
print(f"Greater than 2: {result}")

# Reduce
print("\nReduce:")
result = reduce(lambda a, b: a + b, [1, 2, 3, 4])
print(f"Sum: {result}")

# Combined
print("\nCombined:")
data = [1, 2, 3, 4, 5]
result = reduce(lambda a, b: a + b, 
                 map(lambda x: x**2, 
                     filter(lambda x: x % 2 == 0, data)))
print(f"Sum of squares of evens: {result}")
