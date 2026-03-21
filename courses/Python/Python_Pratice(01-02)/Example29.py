# Example29.py
# Topic: Operators — Augmented Assignment Operators

# Augmented assignment operators combine an operation with assignment
# They do the operation and store the result in one step

# += Addition assignment — adds and assigns in one step
x = 10        # int  — starting value
x += 5        # same as: x = x + 5

print(x)             # 15

# -= Subtraction assignment — subtracts and assigns
x = 10
x -= 3

print(x)             # 7

# *= Multiplication assignment — multiplies and assigns
x = 4
x *= 3

print(x)             # 12

# /= Division assignment — divides and assigns (always returns float)
x = 10.0
x /= 2

print(x)             # 5.0

# //= Floor division assignment — divides and rounds down
x = 10
x //= 3

print(x)             # 3

# **= Exponentiation assignment — raises to power and assigns
x = 2
x **= 4

print(x)             # 16

# %= Modulo assignment — gets remainder and assigns
x = 10
x %= 3

print(x)             # 1

# Why use augmented assignment?
# 1. Less typing — x += 1 is shorter than x = x + 1
# 2. Clearer intent — shows you're updating a value
# 3. Easier to read — less visual clutter

# Real-world example: running total of shopping cart
total = 0                 # int  — start with zero

total += 9.99            # add first item
total += 14.99           # add second item
total += 4.99            # add third item

print(total)             # 29.97

# Real-world example: applying multiple discounts
price = 100.0            # float  — original price

price -= 10.0            # subtract $10 discount
price *= 0.9             # apply 10% off

print(price)             # 81.0

# Real-world example: countdown timer
countdown = 10           # int  — start at 10

countdown -= 1           # tick 1
countdown -= 1           # tick 2
countdown -= 1           # tick 3

print(countdown)         # 7

# Real-world example: building a string
message = "Hello"        # str  — starting text

message += ", "          # add separator
message += "World"       # add more text
message += "!"           # add exclamation

print(message)          # Hello, World!

# Real-world example: doubling investment
investment = 1000       # int  — starting amount in dollars

investment *= 1.05      # 5% growth first year
investment *= 1.05      # 5% growth second year

print(investment)       # 1102.5 (approximately)

# Real-world example: calculating pages read
pages_read = 0          # int  — start with zero pages

pages_read += 25        # read first session
pages_read += 30        # read second session
pages_read += 15        # read third session

print(pages_read)       # 70
