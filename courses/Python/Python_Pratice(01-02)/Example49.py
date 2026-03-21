# Example49.py
# Topic: Loops — Common Patterns and Mistakes

# === Common Pattern: Building a list ===
# Instead of appending in a loop, use list comprehension
squares = []

for i in range(10):
    squares.append(i * i)

print(squares)  # [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]

# Better way: list comprehension
squares = [i * i for i in range(10)]
print(squares)  # [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]

# === Common Pattern: Filtering ===
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
evens = []

for n in numbers:
    if n % 2 == 0:
        evens.append(n)

print(evens)  # [2, 4, 6, 8, 10]

# Better way: list comprehension with filter
evens = [n for n in numbers if n % 2 == 0]
print(evens)  # [2, 4, 6, 8, 10]

# === Common Pattern: Finding sum ===
numbers = [1, 2, 3, 4, 5]
total = 0

for n in numbers:
    total = total + n

print(total)  # 15

# Better way: sum()
total = sum(numbers)
print(total)  # 15

# === Common Pattern: Finding max ===
numbers = [5, 2, 8, 1, 9]
max_num = numbers[0]

for n in numbers:
    if n > max_num:
        max_num = n

print(max_num)  # 9

# Better way: max()
max_num = max(numbers)
print(max_num)  # 9

# === Common Pattern: Finding min ===
min_num = min(numbers)
print(min_num)  # 1

# === Common Pattern: String concatenation ===
words = ["Hello", "World", "Python"]
result = ""

for word in words:
    result = result + word + " "

print(result.strip())  # Hello World Python

# Better way: join()
result = " ".join(words)
print(result)  # Hello World Python

# === MISTAKE: Modifying list while iterating ===
numbers = [1, 2, 3, 4, 5]

# WRONG - can cause unexpected behavior!
# for n in numbers:
#     if n % 2 == 0:
#         numbers.remove(n)  # Don't do this!

# CORRECT - iterate over a copy
for n in numbers.copy():
    if n % 2 == 0:
        numbers.remove(n)

print(numbers)  # [1, 3, 5]

# CORRECT - use list comprehension
numbers = [1, 2, 3, 4, 5]
numbers = [n for n in numbers if n % 2 != 0]
print(numbers)  # [1, 3, 5]

# === MISTAKE: Using wrong variable name ===
fruits = ["apple", "banana"]

# WRONG
# for fruit in fruites:  # Typo!
#     print(fruit)

# CORRECT
for fruit in fruits:
    print(fruit)

# === MISTAKE: Forgetting colon ===
# WRONG
# for i in range(5)
#     print(i)  # SyntaxError!

# CORRECT
for i in range(3):
    print(i)

# === MISTAKE: Using mutable default (advanced topic) ===
# WRONG - don't do this
# def add_item(item, list=[]):  # Shares across calls!
#     list.append(item)
#     return list

# CORRECT
def add_item(item, lst=None):
    if lst is None:
        lst = []
    lst.append(item)
    return lst

result = add_item("apple")
result2 = add_item("banana")
print(result)  # ['apple']
print(result2)  # ['banana']

# === Practical: Nested loops ===
matrix = [
    [1, 2, 3],
    [4, 5, 6],
]

for row in matrix:
    for cell in row:
        print(cell, end=" ")
    print()
# 1 2 3
# 4 5 6

# === Practical: Break and continue ===
numbers = [1, 2, 3, 4, 5]

# Break - stop loop early
for n in numbers:
    if n == 3:
        break
    print(n)
# 1, 2

# Continue - skip current iteration
for n in numbers:
    if n == 3:
        continue
    print(n, end=" ")
# 1, 2, 4, 5

# === Practical: Else in for loop ===
numbers = [1, 2, 3, 4, 5]

for n in numbers:
    if n == 10:
        break
else:
    print("Loop completed without break")

# Loop completed without break
