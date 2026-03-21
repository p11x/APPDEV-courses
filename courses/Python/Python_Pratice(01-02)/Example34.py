# Example34.py
# Topic: Control Flow — Ternary Operator (One-Liner)

# The ternary operator is a compact way to write simple if/else
# It fits on one line: value_if_true if condition else value_if_false

# === Traditional if/else ===
# This takes up multiple lines

age = 20
if age >= 18:
    status = "adult"
else:
    status = "minor"

print(status)                  # adult

# === Ternary operator (one-liner) ===
# Does the same thing in one line

age = 20
status = "adult" if age >= 18 else "minor"

print(status)                 # adult

# === Syntax breakdown ===
# result = "value if True" if condition else "value if False"

# === Ternary for minimum of two numbers ===

a = 5
b = 10
min_val = a if a < b else b

print(min_val)                # 5

a = 15
b = 10
min_val = a if a < b else b

print(min_val)                # 10

# === Ternary for maximum of two numbers ===

a = 5
b = 10
max_val = a if a > b else b

print(max_val)                # 10

# === Ternary for absolute value ===
# Absolute value is always positive

x = -5
abs_val = x if x > 0 else -x

print(abs_val)                # 5

x = 10
abs_val = x if x > 0 else -x

print(abs_val)                # 10

# === Ternary for even/odd check ===

num = 7
result = "even" if num % 2 == 0 else "odd"

print(result)                 # odd

num = 8
result = "even" if num % 2 == 0 else "odd"

print(result)                 # even

# === Ternary for pass/fail ===

score = 75
result = "PASS" if score >= 60 else "FAIL"

print(result)                 # PASS

score = 50
result = "PASS" if score >= 60 else "FAIL"

print(result)                 # FAIL

# === Ternary for grade (simplified) ===

score = 85
grade = "A" if score >= 90 else "B" if score >= 80 else "C" if score >= 70 else "D" if score >= 60 else "F"

print(grade)                  # B

# Note: chained ternaries can be hard to read
# Usually better to use if/elif/else for multiple conditions

# === Ternary for default value ===
# Use a default if the main value is not available

username = ""
display_name = username if username else "Guest"

print(display_name)           # Guest

username = "Alice"
display_name = username if username else "Guest"

print(display_name)           # Alice

# === Ternary with None check ===
# Return default if value is None

data = None
value = data if data is not None else "No data"

print(value)                  # No data

data = "Hello"
value = data if data is not None else "No data"

print(value)                  # Hello

# === Real-world example: Discount indicator ===
# Show if a discount applies

original_price = 100.0
sale_price = 80.0

is_on_sale = "Yes" if sale_price < original_price else "No"

print(is_on_sale)             # Yes

# === Real-world example: User greeting ===
# Different greeting for different times of day (simulated)

hour = 14                     # int  — hour of day (24-hour format)

greeting = "Good morning" if hour < 12 else "Good afternoon" if hour < 18 else "Good evening"

print(greeting)               # Good afternoon

# === Real-world example: Shipping status ===
# Show shipping status based on delivery speed

delivery_days = 3

status = "Standard" if delivery_days > 5 else "Express" if delivery_days > 1 else "Overnight"

print(status)                 # Express

# === When to use ternary vs if/else ===

# Use ternary when:
# - Condition is simple
# - Only two possible values
# - You want concise code

# Use if/else when:
# - Multiple conditions needed
# - Complex logic
# - Multiple statements per branch
