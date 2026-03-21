# Example25.py
# Topic: Operators — Comparison Operators

# Comparison operators compare two values and return True or False
# They are used to make decisions in your code

# Equal to (==) — checks if two values are the same
# Note: uses TWO equal signs, not one!
result = 5 == 5         # bool  — True when both sides are equal
result = 5 == 3         # bool  — False when values differ
result = "hello" == "hello"  # bool  — strings can be compared too

print(result)            # True
print(5 == 5)            # True
print(5 == 3)            # False
print("hello" == "hello")  # True

# Not equal to (!=) — checks if two values are different
result = 5 != 3          # bool  — True when values are different
result = "cat" != "dog"  # bool  — True for different strings

print(5 != 3)            # True
print("cat" != "dog")    # True

# Less than (<) — left side is smaller than right
result = 3 < 5           # bool  — True if left is smaller
result = 5 < 3           # bool  — False if left is larger

print(3 < 5)             # True
print(5 < 3)             # False

# Greater than (>) — left side is larger than right
result = 5 > 3           # bool  — True if left is larger
result = 3 > 5           # bool  — False if left is smaller

print(5 > 3)             # True
print(3 > 5)             # False

# Less than or equal (<=) — left is smaller or equal to right
result = 3 <= 5          # bool  — True if less OR equal
result = 5 <= 5          # bool  — True when equal

print(3 <= 5)            # True
print(5 <= 5)            # True

# Greater than or equal (>=) — left is larger or equal to right
result = 5 >= 3          # bool  — True if greater OR equal
result = 5 >= 5          # bool  — True when equal

print(5 >= 3)            # True
print(5 >= 5)            # True

# Comparing strings — uses alphabetical (lexicographic) order
result = "apple" < "banana"  # bool  — True because 'a' comes before 'b'
result = "cat" > "bird"     # bool  — True because 'c' comes after 'b'

print("apple" < "banana")   # True
print("cat" > "bird")       # True

# Comparing different types — be careful!
# An integer is NEVER equal to a string, even if they look the same
result = 5 == "5"     # bool  — False! int and str are different types

print(5 == "5")       # False

# Real-world example: checking if a user is old enough to vote
age = 18
can_vote = age >= 18   # bool  — must be 18 or older

print(age)             # 18
print(can_vote)        # True

# Real-world example: checking if an item is in stock
quantity = 0
in_stock = quantity > 0   # bool  — True only if quantity is positive

print(quantity)        # 0
print(in_stock)        # False

# Real-world example: checking if a password meets minimum length
password = "python123"
password_ok = len(password) >= 8   # bool  — True if 8 or more characters

print(len(password))    # 8
print(password_ok)      # True
