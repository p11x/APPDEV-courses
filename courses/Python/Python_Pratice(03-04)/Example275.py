# Example275: More Practice with Sorting
from functools import cmp_to_key

# Custom comparator for sorting
def compare_numbers(a, b):
    if abs(a) < abs(b):
        return -1
    elif abs(a) > abs(b):
        return 1
    return 0

print("Custom Comparator:")
nums = [-5, 3, -2, 1, -1, 4]
sorted_nums = sorted(nums, key=cmp_to_key(compare_numbers))
print(f"Original: {nums}")
print(f"Sorted by absolute: {sorted_nums}")

# Sort by multiple conditions
print("\nMultiple Conditions:")
students = [
    ("Alice", "A", 90),
    ("Bob", "B", 85),
    ("Charlie", "A", 85),
    ("Diana", "B", 90)
]
# Sort by grade (desc), then score (desc), then name (asc)
sorted_students = sorted(students, key=lambda x: (-x[1], -x[2], x[0]))
print("Sorted by grade, score, name:")
for s in sorted_students:
    print(f"  {s}")

# Sort dictionary by value
print("\nSort Dictionary:")
d = {"apple": 3, "banana": 1, "cherry": 2, "date": 4}
sorted_d = sorted(d.items(), key=lambda x: x[1], reverse=True)
print(f"By value (desc): {sorted_d}")

# Sort with lambda complex key
print("\nComplex Sorting:")
words = ["hello", "world", "python", "programming"]
sorted_words = sorted(words, key=lambda w: (-len(w), w))
print(f"By length (desc), then alphabetical: {sorted_words}")

# Sort with enumerate
print("\nSort with Enumerate:")
data = [3, 1, 4, 1, 5, 9, 2, 6]
sorted_with_indices = sorted(enumerate(data), key=lambda x: x[1])
print(f"Data: {data}")
print(f"Sorted with indices: {sorted_with_indices}")

# Stable sort
print("\nStable Sort:")
data = [('a', 2), ('b', 1), ('c', 2), ('d', 1)]
result = sorted(data, key=lambda x: x[1])
print(f"Original: [('a', 2), ('b', 1), ('c', 2), ('d', 1)]")
print(f"Sorted by second: {result}")

# Sort strings by custom pattern
print("\nSort by Custom Pattern:")
order = {'a': 3, 'b': 1, 'c': 2}
words = ["abc", "bac", "cab", "acb"]
sorted_words = sorted(words, key=lambda w: [order.get(c, 0) for c in w])
print(f"Words: {words}")
print(f"By pattern: {sorted_words}")
