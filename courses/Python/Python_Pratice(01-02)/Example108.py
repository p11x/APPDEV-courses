# Example108.py
# Topic: Iteration Tools — Basic Zip

# Use zip() to pair elements from multiple iterables

# === Basic zip with two lists ===
names = ["Alice", "Bob", "Carol"]
ages = [25, 30, 35]

for name, age in zip(names, ages):
    print(name + " is " + str(age) + " years old")

# === Zip returns list of tuples ===
# zip() returns an iterator of tuples
result = list(zip(names, ages))
print("Zipped: " + str(result))

# === Zip with three lists ===
a = [1, 2, 3]
b = ["x", "y", "z"]
c = [True, False, True]

for x, y, z in zip(a, b, c):
    print(str(x) + ", " + y + ", " + str(z))

# === Zip with strings (iterates by character) ===
# Strings are iterable!
first = "abc"
second = "xyz"

for f, s in zip(first, second):
    print(f + " + " + s)

# === Zip with dictionaries (iterates keys) ===
# dict iteration gives keys only
dict1 = {"a": 1, "b": 2, "c": 3}
dict2 = {"x": 10, "y": 20, "z": 30}

for k1, k2 in zip(dict1, dict2):
    print(k1 + " & " + k2)

# === Practical: Creating dictionary from two lists ===
keys = ["name", "age", "city"]
values = ["Alice", 30, "NYC"]

d = dict(zip(keys, values))
print("Dict: " + str(d))

# === Practical: Parallel iteration ===
prices = [100, 200, 300]
quantities = [2, 3, 1]

print("Total costs:")
for price, qty in zip(prices, quantities):
    print("$" + str(price) + " x " + str(qty) + " = $" + str(price * qty))

# === Practical: Combining lists into tuples ===
headers = ["ID", "Name", "Score"]
rows = [[1, "Alice", 95], [2, "Bob", 87], [3, "Carol", 92]]

for row in rows:
    data = dict(zip(headers, row))
    print(str(data))

# === Zip stops at shortest ===
# zip() stops when the shortest iterable is exhausted
short = [1, 2]
long = [10, 20, 30, 40]

result = list(zip(short, long))
print("Zip stops at shortest: " + str(result))

# === Empty iterables ===
# zip with empty returns empty
empty = []
non_empty = [1, 2, 3]

result = list(zip(empty, non_empty))
print("Empty zip: " + str(result))

# === Building a table ===
products = ["Apple", "Banana", "Cherry"]
prices = [1.5, 0.5, 2.0]
stock = [100, 200, 50]

table = []
for p, pr, s in zip(products, prices, stock):
    table.append({"product": p, "price": pr, "stock": s})

print("Product table:")
for item in table:
    print("  " + item["product"] + ": $" + str(item["price"]) + " (" + str(item["stock"]) + ")")
