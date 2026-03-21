# Example110.py
# Topic: Iteration Tools — Unpacking with *

# Use * to unzip (unpack) zipped data

# === Basic zip creates tuples ===
pairs = [("a", 1), ("b", 2), ("c", 3)]
print("Zipped pairs: " + str(pairs))

# === Unzip with * ===
letters, numbers = zip(*pairs)
print("Letters: " + str(letters))
print("Numbers: " + str(numbers))

# === What * does ===
# zip(*pairs) is equivalent to:
# zip(("a", 1), ("b", 2), ("c", 3))
# Which gives: ("a", "b", "c"), (1, 2, 3)

# === Practical: Unzipping data ===
data = [("Alice", 25), ("Bob", 30), ("Carol", 35)]
names, ages = zip(*data)
print("Names: " + str(names))
print("Ages: " + str(ages))

# === Unzip with more elements ===
pairs = [(1, "a"), (2, "b"), (3, "c"), (4, "d")]
first, second = zip(*pairs)
print("First elements: " + str(first))
print("Second elements: " + str(second))

# === Practical: Separate columns ===
table = [
    (1, "Apple", 1.5),
    (2, "Banana", 0.5),
    (3, "Cherry", 2.0)
]

ids, names, prices = zip(*table)
print("IDs: " + str(ids))
print("Names: " + str(names))
print("Prices: " + str(prices))

# === Unzip with mixed types ===
mixed = [("x", 1, True), ("y", 2, False), ("z", 3, True)]
a, b, c = zip(*mixed)
print("a: " + str(a))
print("b: " + str(b))
print("c: " + str(c))

# === Combining unzip with other operations ===
data = [(1, 4), (2, 5), (3, 6)]

# Unzip and convert to lists
x, y = map(list, zip(*data))
print("x as list: " + str(x))
print("y as list: " + str(y))

# === Reversing using unzip ===
original = [("a", 1), ("b", 2), ("c", 3)]
reversed_pairs = list(zip(*original[::-1]))
print("Reversed: " + str(reversed_pairs))

# === Unzipping single element tuples ===
single = [(1,)]
a, = zip(*single)  # Note the comma!
print("Single element: " + str(a))

# === Using * in function calls ===
def greet(name, greeting):
    return greeting + ", " + name + "!"


messages = [("Alice", "Hello"), ("Bob", "Hi"), ("Carol", "Hey")]

# This doesn't work directly:
# greet(*messages[0])

# But this does:
for name, greeting in messages:
    result = greet(name, greeting)
    print(result)

# === * with enumerate ===
# Not unzipping, but similar unpacking
pairs = [("a", 1), ("b", 2)]

# Unpack both enumerate and inner tuple
for i, (char, num) in enumerate(pairs):
    print(str(i) + ": " + char + " = " + str(num))

# === Practical: Transpose matrix ===
matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
]

# Transpose using zip
transposed = list(zip(*matrix))
print("Transposed: " + str(transposed))

# Convert to list of lists
transposed_list = [list(row) for row in transposed]
print("As list of lists: " + str(transposed_list))
