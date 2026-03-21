# Example31.py
# Topic: Operators — Walrus Operator in Loops and Comprehensions

# The walrus operator (:=) is especially useful in:
# 1. While loops — assign and check in one expression
# 2. List comprehensions — avoid recomputing expensive operations

# === Walrus in While Loops ===

# Classic problem: input loop without walrus
# You have to assign BEFORE the loop, then repeat at the end
# This is repetitive and harder to read

# Without walrus — repetitive pattern
# command = input("Enter command: ")    # First assignment
# while command != "quit":
#     print("Processing:", command)
#     command = input("Enter command: ")  # Must repeat!

# With walrus — assign and check in one line!
# The := assigns the input value AND checks the condition
# while (command := input("Enter command: ")) != "quit":
#     print("Processing:", command)

# Simulated version (since we can't use input in this script)
# We'll use a list to simulate the input sequence
command_list = ["start", "process", "stop", "quit"]  # list  — simulated inputs
command_index = 0

# Using walrus pattern with a generator
# This demonstrates the concept without actual input
# The walrus pattern would look like this in real code:
# while (command := input("Enter command: ")) != "quit":
#     print("Processing:", command)

# Alternative: using walrus with a counter
# Walrus assigns the counter value and checks condition in one
count = 0                      # int  — starting count

# (count := count + 1) assigns 1, returns 1, which is > 0, so continues
# This pattern counts up to 3
while (count := count + 1) <= 3:
    print("Count:", count)

# Result: prints 1, 2, 3

# Real-world example: reading input until valid
# Without walrus:
# user_input = input("Enter a number: ")
# while not user_input.isdigit():
#     user_input = input("Invalid. Enter a number: ")

# With walrus in real code would be:
# while not (user_input := input("Enter a number: ")).isdigit():
#     print("Invalid input, try again")

# === Walrus in List Comprehensions ===

# List comprehensions create lists from other iterables
# The walrus helps when you need to use a computed value multiple times

# Problem: computing the same thing twice
# Without walrus in comprehension (conceptual):
# [i**2 for i in range(10) if i**2 % 2 == 0]
# The i**2 is computed TWICE — once for the condition, once for the result

# With walrus: compute ONCE, use TWICE
# This only works in Python 3.8+
# Syntax: [expression for item in iterable if (temp := expression) condition]

# Simulating what would work in Python 3.8+:
# This is the pattern for finding even squares:
# squares = [sq for i in range(10) if (sq := i ** 2) % 2 == 0]

# Let's demonstrate the concept manually
squares = []                     # list  — will hold even squares

for i in range(10):              # i goes from 0 to 9
    square = i ** 2              # int  — square of i
    if square % 2 == 0:          # check if even
        squares.append(square)   # add to list if even

print(squares)                   # [0, 4, 16, 36, 64]

# The walrus would make this:
# squares = [sq for i in range(10) if (sq := i ** 2) % 2 == 0]

# Why is walrus useful in comprehensions?
# 1. Avoid recomputing expensive operations
# 2. Make the code more readable (when appropriate)
# 3. Store intermediate results for debugging

# Real-world example: filtering expensive computations
# Imagine checking a complex condition on data
# data = [complex_object1, complex_object2, ...]
# Without walrus: each object's expensive_check is called twice
# result = [obj for obj in data if obj.expensive_check()]

# With walrus: called once
# result = [obj for obj in data if (result := obj.expensive_check())]

# Note: walrus doesn't work at module level (outside expressions)
# x := 5        # SyntaxError!
# y = (x := 5)  # OK — inside an expression

# Quick demo of walrus behavior
result = (x := 5) + (y := 3)    # both assign and return values

print(x)                         # 5
print(y)                         # 3
print(result)                    # 8

# Walrus returns the assigned value, so it can chain
# This assigns 10 to x, then 20 to y, then compares
result = (x := 10) < (y := 20)

print(result)                    # True
print(x)                        # 10
print(y)                        # 20
