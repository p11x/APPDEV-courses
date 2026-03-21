# Example41.py
# Topic: Control Flow — Truthiness and Falsiness

# Every value in Python is either truthy (treated as True) or falsy (treated as False)

# === Falsy Values ===
# These are treated as False in conditions

# False - the boolean False
result = False
if result:
    print("True")
else:
    print("False")          # This prints

# None - absence of a value
result = None
if result:
    print("True")
else:
    print("False")          # This prints

# 0 - zero integer
result = 0
if result:
    print("True")
else:
    print("False")          # This prints

# 0.0 - zero float
result = 0.0
if result:
    print("True")
else:
    print("False")          # This prints

# "" - empty string
result = ""
if result:
    print("True")
else:
    print("False")          # This prints

# [] - empty list
result = []
if result:
    print("True")
else:
    print("False")          # This prints

# () - empty tuple
result = ()
if result:
    print("True")
else:
    print("False")          # This prints

# {} - empty dictionary
result = {}
if result:
    print("True")
else:
    print("False")          # This prints

# set() - empty set
result = set()
if result:
    print("True")
else:
    print("False")          # This prints

# range(0) - empty range
result = range(0)
if result:
    print("True")
else:
    print("False")          # This prints

# === Truthy Values ===
# Everything else is truthy!

# True - boolean True
result = True
if result:
    print("True")           # This prints

# Any non-zero number
result = 1
if result:
    print("True")           # This prints

result = -1
if result:
    print("True")           # This prints (negative is still truthy!)

result = 0.1
if result:
    print("True")           # This prints

# Non-empty strings
result = "hello"
if result:
    print("True")           # This prints

result = " "
if result:
    print("True")           # Space is truthy!

# Non-empty collections
result = [1, 2, 3]
if result:
    print("True")           # This prints

result = (1, 2)
if result:
    print("True")           # This prints

result = {"key": "value"}
if result:
    print("True")           # This prints

result = {1, 2, 3}
if result:
    print("True")           # This prints

result = range(1)
if result:
    print("True")           # This prints

# === Using bool() to check truthiness ===

# Convert any value to boolean
print(bool(False))      # False
print(bool(None))       # False
print(bool(0))         # False
print(bool(0.0))       # False
print(bool(""))        # False
print(bool([]))        # False
print(bool({}))        # False

print(bool(True))      # True
print(bool(1))         # True
print(bool(-1))        # True
print(bool("hello"))   # True
print(bool([1]))       # True
print(bool({"a": 1})) # True

# === Real-world example: User input check ===
username = ""

if username:
    print("Welcome, " + username + "!")
else:
    print("Welcome, Guest!")  # This prints

username = "Alice"

if username:
    print("Welcome, " + username + "!")  # This prints
else:
    print("Welcome, Guest!")

# === Real-world example: Product inventory ===
quantity = 0

if quantity:
    print("Product is in stock")
else:
    print("Product is out of stock")  # This prints

quantity = 25

if quantity:
    print("Product is in stock")  # This prints
else:
    print("Product is out of stock")
