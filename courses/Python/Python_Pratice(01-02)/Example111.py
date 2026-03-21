# Example111.py
# Topic: Iteration Tools — Common Mistakes with Zip

# Common mistakes when using zip()

# === MISTAKE 1: Assuming equal lengths ===

# WRONG - zip silently truncates!
names = ["Alice", "Bob", "Carol", "David"]
ages = [25, 30]

# This misses David!
for name, age in zip(names, ages):
    print(name + ": " + str(age))

# CORRECT - use zip_longest
from itertools import zip_longest

for name, age in zip_longest(names, ages, fillvalue="N/A"):
    print(name + ": " + str(age))

# === MISTAKE 2: Forgetting zip returns iterator ===

# WRONG - trying to index directly
data = zip([1, 2], ["a", "b"])

# print(data[0])  # Error! Can't index iterator

# CORRECT - convert to list first
data = list(zip([1, 2], ["a", "b"]))
print("As list: " + str(data[0]))

# === MISTAKE 3: Not unpacking tuples ===

# WRONG - treating each tuple as one value
pairs = [("a", 1), ("b", 2)]

for pair in pairs:
    print(pair)  # Prints tuples!

# CORRECT - unpack
for letter, num in pairs:
    print(letter + " = " + str(num))

# === MISTAKE 4: Using zip on single iterable ===

# WRONG - this is enumerate!
# zip(["a", "b", "c"])  # Not useful!

# CORRECT - use enumerate
for i, v in enumerate(["a", "b", "c"]):
    print(str(i) + ": " + v)

# === MISTAKE 5: Modifying while iterating ===

# WRONG - don't modify lists while zipping
list1 = [1, 2, 3]
list2 = [10, 20, 30]

# for a, b in zip(list1, list2):
#     list1.append(a)  # Modifies during iteration!

# CORRECT - work with copies
list1 = [1, 2, 3]
list2 = [10, 20, 30]
result = []
for a, b in zip(list1, list2):
    result.append(a + b)
print("Sum: " + str(result))

# === MISTAKE 6: Not converting to list ===

# WRONG - exhausts iterator
data = zip([1, 2], ["a", "b"])

for item in data:
    print("First: " + str(item))

# Second iteration is empty!
# for item in data:
#     print("Second: " + str(item))

# CORRECT - convert to list if needed
data = list(zip([1, 2], ["a", "b"]))
for item in data:
    print("Item: " + str(item))
for item in data:
    print("Again: " + str(item))

# === MISTAKE 7: Using with dictionaries ===

# WRONG - zip only iterates keys
dict1 = {"a": 1, "b": 2}
dict2 = {"x": 10, "y": 20}

# This only gives keys!
for k1, k2 in zip(dict1, dict2):
    print(k1 + " - " + k2)

# CORRECT - use .items()
print("\nWith items:")
for k1, v1 in dict1.items():
    for k2, v2 in dict2.items():
        if k1 != k2:
            print(k1 + "=" + str(v1) + ", " + k2 + "=" + str(v2))

# === MISTAKE 8: Forgetting zip_longest for uneven data ===

# WRONG - missing data silently lost
short = ["a", "b"]
long = [1, 2, 3, 4]

result = list(zip(short, long))
print("Lost data: " + str(result))

# CORRECT - use zip_longest
result = list(zip_longest(short, long, fillvalue=0))
print("All data: " + str(result))

# === MISTAKE 9: Wrong number of variables ===

# WRONG - tuple has 2, trying to unpack 3
pairs = [(1, 2), (3, 4)]

# a, b, c = zip(*pairs)  # Error!

# CORRECT - unpack correctly
first, second = zip(*pairs)
print("First: " + str(first))
print("Second: " + str(second))

# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Use zip_longest for unequal lengths
# 2. Convert to list if you need to iterate twice
# 3. Always unpack tuples: for a, b in zip(...)
# 4. Remember zip only iterates keys for dicts
# 5. Don't modify iterables while zipping
# 6. zip() returns iterator, not list
