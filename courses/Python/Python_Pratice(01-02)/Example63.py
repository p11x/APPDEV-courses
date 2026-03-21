# Example63.py
# Topic: Comprehensions — Common Mistakes

# Common mistakes when using comprehensions

# === MISTAKE: List vs Generator ===

# WRONG: Using [] for large data
large_data = [x for x in range(10000000)]  # Uses lots of memory!

# CORRECT: Use () for generator
large_data = (x for x in range(10000000))  # Memory efficient

# === MISTAKE: Forgetting parentheses ===

# WRONG: Missing parentheses
# squares = [x ** 2 for x in range(5)]  # This is correct actually

# For generator:
gen = (x ** 2 for x in range(5))  # Must use ()

# === MISTAKE: Modifying original list ===

numbers = [1, 2, 3]

# WRONG: Comprehension creates NEW list
squares = [n ** 2 for n in numbers]
print(numbers)  # Still [1, 2, 3]!

# CORRECT: Modify in place with loop
numbers = [1, 2, 3]
for i in range(len(numbers)):
    numbers[i] = numbers[i] ** 2
print(numbers)  # [1, 4, 9]

# === MISTAKE: Wrong variable in condition ===
numbers = [1, 2, 3, 4, 5]

# WRONG: Using wrong variable
# evens = [n for n in numbers if x > 2]  # x doesn't exist!

# CORRECT: Use same variable
evens = [n for n in numbers if n > 2]
print(evens)  # [3, 4, 5]

# === MISTAKE: Complex nested comprehensions ===

# WRONG: Too complex to read
result = [func(x) for sublist in list_of_lists for item in sublist for x in item if cond(x)]

# CORRECT: Use regular loop
result = []
for sublist in list_of_lists:
    for item in sublist:
        if cond(item):
            result.append(func(item))

# === MISTAKE: Forgetting to convert generator ===

gen = (x ** 2 for x in range(5))

# WRONG: Can't index a generator
# print(gen[0])  # TypeError!

# CORRECT: Convert to list first
gen_list = list(gen)
print(gen_list[0])  # 0

# === MISTAKE: Using comprehension for side effects ===

# WRONG: Comprehension should not have side effects
# [print(x) for x in range(5)]  # Works but bad practice!

# CORRECT: Use regular loop
for x in range(5):
    print(x)

# === MISTAKE: Dictionary key collision ===
names = ["Alice", "Bob", "Alice", "Charlie"]

# WRONG: Duplicate keys will overwrite
name_dict = {name: len(name) for name in names}
print(name_dict)  # {'Alice': 5, 'Bob': 3, 'Charlie': 7}

# === MISTAKE: Using set with unhashable ===

# WRONG: Can't put lists in a set (unhashable)
# numbers = [[1, 2], [3, 4]]
# unique = {n for n in numbers}  # TypeError!

# CORRECT: Use tuples instead
numbers = [(1, 2), (3, 4)]
unique = {n for n in numbers}
print(unique)  # {(1, 2), (3, 4)}

# === MISTAKE: Thinking comprehension is always faster ===

# For simple operations, loop might be clearer
# Don't over-use comprehensions!

# === CORRECT: When to use each ===

# Use list comprehension when:
# - Creating a new list
# - Need all values at once

# Use generator when:
# - Large data
# - Only need first few values

# Use loop when:
# - Complex logic
# - Need to modify original
# - Multiple conditions

# === Best Practice: Keep it simple ===
# Simple transformation
doubled = [n * 2 for n in range(5)]

# Filter
evens = [n for n in range(10) if n % 2 == 0]

# Both
squares_of_evens = [n ** 2 for n in range(10) if n % 2 == 0]
print(squares_of_evens)  # [0, 4, 16, 36, 64]
