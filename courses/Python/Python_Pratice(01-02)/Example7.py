# Example7.py
# Topic: Data Types - Integer (int)

# Integer (int) — whole numbers without decimals
# Integers are used for counting, indexing, and any whole number

# Creating integers
count = 0              # int  — zero
age = 25               # int  — positive number
negative = -10         # int  — negative number
population = 7800000000 # int  — very large numbers work fine

# Print integer values
print(count)              # 0
print(age)               # 25
print(negative)          # -10
print(population)        # 7800000000

# Integer operations
a = 10
b = 3

# Addition
result = a + b
print("10 + 3 = " + str(result))  # 13

# Subtraction
result = a - b
print("10 - 3 = " + str(result))  # 7

# Multiplication
result = a * b
print("10 * 3 = " + str(result))  # 30

# Division (gives float in Python 3)
result = a / b
print("10 / 3 = " + str(result))  # 3.333...

# Integer division (floor)
result = a // b
print("10 // 3 = " + str(result))  # 3

# Modulo (remainder)
result = a % b
print("10 % 3 = " + str(result))  # 1

# Exponentiation
result = a ** b
print("10 ** 3 = " + str(result))  # 1000

# Using integers in real scenarios
# Shopping cart quantities
laptops = 5
phones = 3
tablets = 2
total_items = laptops + phones + tablets
print("Total items: " + str(total_items))  # 10

# Temperature conversion
celsius = 25
fahrenheit = (celsius * 9/5) + 32
print(str(celsius) + "C = " + str(fahrenheit) + "F")  # 77.0F

# Age calculation
birth_year = 2000
current_year = 2024
age = current_year - birth_year
print("Age: " + str(age))  # 24

# Real-world example: bank account balance (using integers for cents)
# We store money in cents to avoid floating point issues
balance_cents = 10050  # $100.50
withdraw_cents = 2550  # $25.50

new_balance_cents = balance_cents - withdraw_cents
print("New balance: $" + str(new_balance_cents / 100))  # $75.00

# Integer division for grouping
students = 100
groups = 7
students_per_group = students // groups
remaining = students % groups
print("Groups of " + str(students_per_group) + " with " + str(remaining) + " leftover")  # Groups of 14 with 2 leftover
