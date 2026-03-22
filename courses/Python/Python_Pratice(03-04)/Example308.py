# Example308: Any, All, Zip, Enumerate
# Any and All
print("Any and All:")
print(f"Any [False, True, False]: {any([False, True, False])}")
print(f"All [True, True, True]: {all([True, True, True])}")
print(f"All [True, False]: {all([True, False])}")

# Zip
print("\nZip:")
names = ['Alice', 'Bob', 'Charlie']
ages = [25, 30, 35]
for name, age in zip(names, ages):
    print(f"  {name}: {age}")

# Enumerate
print("\nEnumerate:")
for i, val in enumerate(['a', 'b', 'c']):
    print(f"  {i}: {val}")

# Start index
print("\nEnumerate with start:")
for i, val in enumerate(['a', 'b', 'c'], start=1):
    print(f"  {i}: {val}")
