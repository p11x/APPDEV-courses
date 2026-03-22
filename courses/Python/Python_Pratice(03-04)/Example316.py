# Example316: More Practice with Dictionaries
# Dict comprehension
print("Dict Comprehension:")
squares = {x: x**2 for x in range(5)}
print(f"Squares: {squares}")

# Update
print("\nUpdate:")
d1 = {'a': 1, 'b': 2}
d2 = {'b': 3, 'c': 4}
d1.update(d2)
print(f"After update: {d1}")

# Get with default
print("\nGet with default:")
d = {'a': 1}
print(f"Get 'a': {d.get('a', 0)}")
print(f"Get 'b': {d.get('b', 0)}")

# Items
print("\nItems:")
for k, v in {'a': 1, 'b': 2}.items():
    print(f"  {k}: {v}")
