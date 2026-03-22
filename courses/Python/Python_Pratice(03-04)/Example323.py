# Example323: Practice with Range
# Range basics
print("Range:")
print(f"Range 5: {list(range(5))}")
print(f"Range 2-5: {list(range(2, 5))}")
print(f"Range 0-10 step 2: {list(range(0, 10, 2))}")

# Reverse
print("\nReverse:")
print(f"Reverse: {list(range(5, 0, -1))}")

# Index with range
print("\nIndex:")
for i in range(3):
    print(f"Index {i}")
