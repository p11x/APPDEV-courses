# Example50.py
# Topic: Loops — For Loop Practice Examples

# Comprehensive practice examples combining everything

# === Example 1: Student grade analysis ===
students = [
    {"name": "Alice", "scores": [85, 90, 78]},
    {"name": "Bob", "scores": [92, 88, 95]},
    {"name": "Charlie", "scores": [70, 75, 80]},
]

for student in students:
    name = student["name"]
    scores = student["scores"]
    average = sum(scores) / len(scores)
    print(name + ": " + str(average))
# Alice: 84.333...
# Bob: 91.666...
# Charlie: 75.0

# === Example 2: Temperature converter ===
temps_c = [0, 20, 37, 100]

for c in temps_c:
    f = (c * 9/5) + 32
    print(str(c) + "C = " + str(f) + "F")
# 0C = 32.0F
# 20C = 68.0F
# 37C = 98.6F
# 100C = 212.0F

# === Example 3: Password strength checker ===
passwords = ["abc", "password123", "Str0ng!", "admin"]

for pwd in passwords:
    length = len(pwd)
    has_digit = any(c.isdigit() for c in pwd)
    has_upper = any(c.isupper() for c in pwd)
    has_special = not c.isalnum()
    
    if length >= 8 and has_digit and has_upper:
        strength = "Strong"
    elif length >= 6 and has_digit:
        strength = "Medium"
    else:
        strength = "Weak"
    
    print(pwd + ": " + strength)
# abc: Weak
# password123: Medium
# Str0ng!: Strong
# admin: Weak

# === Example 4: Shopping cart with discount ===
cart = [
    {"item": "Laptop", "price": 999, "quantity": 1},
    {"item": "Mouse", "price": 29, "quantity": 2},
    {"item": "Keyboard", "price": 79, "quantity": 1},
]

subtotal = 0

for item in cart:
    item_total = item["price"] * item["quantity"]
    subtotal = subtotal + item_total

if subtotal > 500:
    discount = subtotal * 0.10
    print("Discount: -$" + str(discount))
else:
    discount = 0

tax = subtotal * 0.08
total = subtotal - discount + tax

print("Subtotal: $" + str(subtotal))
print("Tax: $" + str(tax))
print("Total: $" + str(total))

# === Example 5: Fibonacci sequence ===
n = 10
fib = [0, 1]

for i in range(2, n):
    next_num = fib[i-1] + fib[i-2]
    fib.append(next_num)

print(fib)  # [0, 1, 1, 2, 3, 5, 8, 13, 21, 34]

# === Example 6: Prime numbers up to N ===
n = 20
primes = []

for num in range(2, n + 1):
    is_prime = True
    for i in range(2, int(num ** 0.5) + 1):
        if num % i == 0:
            is_prime = False
            break
    if is_prime:
        primes.append(num)

print(primes)  # [2, 3, 5, 7, 11, 13, 17, 19]

# === Example 7: Word counter ===
text = "hello world hello python world hello"
words = text.split()
word_counts = {}

for word in words:
    if word in word_counts:
        word_counts[word] = word_counts[word] + 1
    else:
        word_counts[word] = 1

print(word_counts)  # {'hello': 3, 'world': 2, 'python': 1}

# === Example 8: List intersection ===
list1 = [1, 2, 3, 4, 5]
list2 = [4, 5, 6, 7, 8]

common = []

for item in list1:
    if item in list2:
        common.append(item)

print(common)  # [4, 5]

# === Example 9: Date validator ===
dates = [
    (2024, 2, 29),  # Leap year
    (2023, 2, 29),  # Not leap year
    (2024, 4, 31),  # Invalid
    (2024, 6, 15),  # Valid
]

for year, month, day in dates:
    valid = True
    
    if month in [1, 3, 5, 7, 8, 10, 12]:
        max_day = 31
    elif month in [4, 6, 9, 11]:
        max_day = 30
    elif month == 2:
        if (year % 4 == 0 and year % 100 != 0) or year % 400 == 0:
            max_day = 29
        else:
            max_day = 28
    else:
        valid = False
    
    if day > max_day:
        valid = False
    
    status = "Valid" if valid else "Invalid"
    print(str(month) + "/" + str(day) + "/" + str(year) + ": " + status)

# === Example 10: Two-dimensional array sum ===
matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9],
]

total = 0

for row in matrix:
    for cell in row:
        total = total + cell

print("Sum: " + str(total))  # 45
