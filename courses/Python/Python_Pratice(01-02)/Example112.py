# Example112.py
# Topic: Iteration Tools — Basic Sorted

# Use sorted() to sort iterables

# === Basic sorted with numbers ===
numbers = [5, 2, 8, 1, 9]
result = sorted(numbers)
print("Original: " + str(numbers))
print("Sorted: " + str(result))

# === Sorted returns NEW list (doesn't modify original) ===
original = [3, 1, 4, 1, 5]
sorted_list = sorted(original)
print("Original after sorted: " + str(original))
print("Sorted list: " + str(sorted_list))

# === Sorted with strings ===
words = ["banana", "apple", "cherry", "date"]
print("Sorted strings: " + str(sorted(words)))

# === Sorted is case-sensitive ===
mixed = ["Banana", "apple", "Cherry", "date"]
print("Case-sensitive sort: " + str(sorted(mixed)))

# === Sorted with reverse=True ===
numbers = [5, 2, 8, 1, 9]
print("Reverse sort: " + str(sorted(numbers, reverse=True)))

# === Sorted with key= parameter ===
# key is a function that returns value to sort by
words = ["banana", "apple", "cherry", "date"]

# Sort by length
print("Sorted by length: " + str(sorted(words, key=len)))

# === Sorted with custom key function ===
names = ["Alice", "Bob", "Carol", "david"]

# Sort by lowercase (case-insensitive)
print("Case-insensitive: " + str(sorted(names, key=str.lower)))

# === Practical: Sort by absolute value ===
numbers = [-5, 2, -8, 1, -9]
print("Sort by absolute: " + str(sorted(numbers, key=abs)))

# === Practical: Sort tuples by second element ===
pairs = [(1, 5), (3, 2), (2, 8), (4, 1)]
print("Sort by second element: " + str(sorted(pairs, key=lambda x: x[1])))

# === Sort dictionary by value ===
scores = {"Alice": 95, "Bob": 87, "Carol": 92, "David": 78}
print("Sort by value: " + str(sorted(scores.items(), key=lambda x: x[1])))

# === Sort with multiple keys ===
data = [("Alice", 95), ("Bob", 87), ("Carol", 92), ("Alice", 88)]
# Sort by name then score
print("Sort by name then score: " + str(sorted(data, key=lambda x: (x[0], x[1]))))

# === Sorted with strings (character by character) ===
# Strings are compared character by character
words = ["apple", "apricot", "banana"]
print("String sort: " + str(sorted(words)))

# === Sorted with negative numbers ===
neg = [-3, -1, -4, -1, -5]
print("Negative sort: " + str(sorted(neg)))

# === Empty list ===
empty = []
print("Empty sorted: " + str(sorted(empty)))

# === Single element ===
single = [42]
print("Single sorted: " + str(sorted(single)))

# === Note: sorted() works on any iterable ===
# Convert to list
result = sorted((3, 1, 4, 1, 5))  # tuple
print("From tuple: " + str(result))

result = sorted({3, 1, 4, 1, 5})  # set (unordered, but sorted result)
print("From set: " + str(result))
