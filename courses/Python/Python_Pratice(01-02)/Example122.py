# Example122.py
# Topic: Iteration Tools — Chain and Islice

# Using itertools.chain() and itertools.islice()

# === Import itertools ===
from itertools import chain, islice

# === Basic chain() - concatenate iterables ===
list1 = [1, 2, 3]
list2 = [4, 5, 6]
list3 = [7, 8, 9]

result = list(chain(list1, list2, list3))
print("Chain result: " + str(result))

# === chain() with different types ===
# Works with any iterable
a = [1, 2, 3]
b = (4, 5, 6)
c = "789"

result = list(chain(a, b, c))
print("Mixed types: " + str(result))

# === chain() - flatten one level ===
nested = [[1, 2], [3, 4], [5, 6]]
result = list(chain(*nested))
print("Flattened: " + str(result))

# === chain.from_iterable (better for flattening) ===
nested = [[1, 2], [3, 4], [5, 6]]
result = list(chain.from_iterable(nested))
print("From iterable: " + str(result))

# === Practical: Combine multiple sources ===
names1 = ["Alice", "Bob"]
names2 = ["Carol", "Dave"]
names3 = ["Eve"]

all_names = list(chain(names1, names2, names3))
print("All names: " + str(all_names))

# === islice() - slice iterator ===
numbers = range(10)

# Get first 5
result = list(islice(numbers, 5))
print("First 5: " + str(result))

# Get from index 2 to 5
numbers = range(10)
result = list(islice(numbers, 2, 5))
print("Index 2-5: " + str(result))

# Get every 2nd element
numbers = range(10)
result = list(islice(numbers, 0, 10, 2))
print("Every 2nd: " + str(result))

# === islice() with None (consume all) ===
numbers = range(100)
# islice(numbers, None) is same as islice(numbers)
result = list(islice(numbers, None))
print("All (via None): " + str(result[:10]) + "...")

# === Practical: Process in chunks ===
data = range(20)

# Process first 5
chunk1 = list(islice(data, 5))
print("Chunk 1: " + str(chunk1))

# Process next 5
chunk2 = list(islice(data, 5))
print("Chunk 2: " + str(chunk2))

# === islice() - stop at end ===
numbers = range(5)
result = list(islice(numbers, 0, 10))
print("Beyond end: " + str(result))

# === chain() returns iterator (lazy) ===
a = [1, 2, 3]
b = [4, 5, 6]
result = chain(a, b)
print("Chain type: " + str(type(result)))

# Consume
print("Consumed: " + str(list(result)))

# === islice() returns iterator (lazy) ===
numbers = range(10)
result = islice(numbers, 3)
print("Islice type: " + str(type(result)))

# Consume
print("Consumed: " + str(list(result)))

# === Practical: Skip header, get data ===
rows = ["header", "row1", "row2", "row3", "row4"]

# Skip header (first row)
data_rows = list(islice(rows, 1, None))
print("Data rows: " + str(data_rows))

# === Combining chain and islice ===
a = [1, 2, 3]
b = [4, 5, 6]
c = [7, 8, 9]

# Chain and take first 5
result = list(islice(chain(a, b, c), 5))
print("Chain + islice: " + str(result))

# === Note: chain vs + for lists ===
# chain() is lazy, + creates new list
# chain() is better for large/unknown length
list_a = [1, 2]
list_b = [3, 4]

result = list(chain(list_a, list_b))
print("Chain: " + str(result))
result = list_a + list_b
print("Plus: " + str(result))
