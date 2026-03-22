# Example319: More Practice with Sets
# Set operations
print("Set Operations:")
a = {1, 2, 3}
b = {2, 3, 4}

print(f"Union: {a | b}")
print(f"Intersection: {a & b}")
print(f"Difference: {a - b}")
print(f"Symmetric: {a ^ b}")

# Methods
print("\nMethods:")
a.add(5)
print(f"Add: {a}")
a.remove(1)
print(f"Remove: {a}")

# Set comprehension
print("\nComprehension:")
squares = {x**2 for x in range(5)}
print(f"Squares: {squares}")
