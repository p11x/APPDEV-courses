# Example294: Comprehensions - Advanced Patterns
# Nested comprehension
print("Nested Comprehensions:")
matrix = [[i * j for j in range(1, 4)] for i in range(1, 4)]
print(f"3x3 multiplication: {matrix}")

# Dictionary comprehension with condition
print("\nDict with condition:")
nums = [1, 2, 3, 4, 5]
squares = {n: n**2 for n in nums if n % 2 == 0}
print(f"Squares of evens: {squares}")

# Set comprehension with multiple conditions
print("\nSet with conditions:")
words = ["hello", "world", "python", "programming"]
lengths = {len(w) for w in words if len(w) > 4}
print(f"Lengths > 4: {lengths}")

# Generator from comprehension
print("\nGenerator:")
gen = (x**2 for x in range(5))
print(list(gen))

# Comprehension with zip
print("\nWith zip:")
keys = ['a', 'b', 'c']
values = [1, 2, 3]
combined = {k: v for k, v in zip(keys, values)}
print(f"Zipped: {combined}")

# Nested dict comprehension
print("\nNested dict:")
students = ['Alice', 'Bob', 'Charlie']
grades = [90, 85, 88]
info = {s: {'grade': g} for s, g in zip(students, grades)}
print(f"Nested: {info}")

# Flatten nested list
print("\nFlatten:")
nested = [[1, 2], [3, 4], [5, 6]]
flat = [item for sublist in nested for item in sublist]
print(f"Flattened: {flat}")
