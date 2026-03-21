# Example61.py
# Topic: Comprehensions — Generator Expressions

# Generators are lazy - they produce values one at a time, not all at once

# === List vs Generator ===

# List - creates entire list in memory
squares_list = [x ** 2 for x in range(5)]
print(squares_list)  # [0, 1, 4, 9, 16]
print(type(squares_list))  # <class 'list'>

# Generator - produces values on demand (parentheses!)
squares_gen = (x ** 2 for x in range(5))
print(squares_gen)  # <generator object ...>
print(type(squares_gen))  # <class 'generator'>

# === Iterating over generator ===
squares_gen = (x ** 2 for x in range(5))

for square in squares_gen:
    print(square)
# 0, 1, 4, 9, 16

# === Why use generators? ===

# Memory efficient - doesn't store all values
# List: [0, 1, 4, 9, ... 999999] - lots of memory!
# Generator: only produces one value at a time

# Can stop early
squares_gen = (x ** 2 for x in range(1000000))

count = 0
for square in squares_gen:
    print(square)
    count += 1
    if count >= 5:
        break
# 0, 1, 4, 9, 16

# === Converting to list ===
squares_gen = (x ** 2 for x in range(5))
squares_list = list(squares_gen)
print(squares_list)  # [0, 1, 4, 9, 16]

# === When to use generators ===

# Use when:
# - Working with large datasets
# - Only need first few items
# - Memory is a concern
# - Don't need to reuse the data

# Use list when:
# - Need to use data multiple times
# - Need random access (list[5])
# - Need to know the length

# === Generator with sum() ===
squares_gen = (x ** 2 for x in range(10))
total = sum(squares_gen)
print(total)  # 285

# === Generator with max() ===
numbers_gen = (x * 2 for x in range(10))
max_val = max(numbers_gen)
print(max_val)  # 18

# === Generator with next() ===
squares_gen = (x ** 2 for x in range(5))

print(next(squares_gen))  # 0
print(next(squares_gen))  # 1
print(next(squares_gen))  # 4
print(next(squares_gen))  # 9
print(next(squares_gen))  # 16

# After exhausted:
# print(next(squares_gen))  # StopIteration exception

# === Real-world example: Process large file ===
# In real code:
# def read_large_file(filename):
#     with open(filename) as f:
#         for line in f:  # Generator - reads one line at a time
#             yield line

# Simulated:
lines = ["line1", "line2", "line3", "line4", "line5"]
line_gen = (line for line in lines)

for line in line_gen:
    print("Processing: " + line)
# Processing: line1
# Processing: line2
# ...

# === Generator vs List in function ===
def use_list():
    return [x ** 2 for x in range(100)]

def use_generator():
    return (x ** 2 for x in range(100))

result_list = use_list()
result_gen = use_generator()

print(type(result_list))  # <class 'list'>
print(type(result_gen))  # <class 'generator'>

# === Memory comparison ===
# List of 1 million squares:
# squares_list = [x ** 2 for x in range(1000000)]
# Uses ~8MB of memory

# Generator of 1 million squares:
# squares_gen = (x ** 2 for x in range(1000000))
# Uses only a few bytes of memory
