# Example43.py
# Topic: Control Flow — Truthiness Common Patterns

# Common patterns using truthiness in Python

# === Default Value Pattern ===

# Without truthiness
name = None

if name is None:
    name = "Guest"

print("Hello, " + name)  # Hello, Guest

# With truthiness (using or)
name = None
name = name or "Guest"

print("Hello, " + name)  # Hello, Guest

# With actual name
name = "Alice"
name = name or "Guest"

print("Hello, " + name)  # Hello, Alice

# === Using or for Default Values ===

# User settings
theme = "" or "light"
print("Theme: " + theme)  # Theme: light

theme = "dark"
theme = theme or "light"
print("Theme: " + theme)  # Theme: dark

# Configuration values
config_value = 0 or 100
print("Config: " + str(config_value))  # Config: 100 (0 is falsy!)

config_value = 50 or 100
print("Config: " + str(config_value))  # Config: 50

# === Short-Circuit with and ===

# Only print if list is not empty
my_list = ["item"]

if my_list:
    print(my_list[0])  # item

my_list = []

# This won't try to access [0] because first part is falsy
if my_list and my_list[0]:
    print(my_list[0])

# Using and to conditionally execute
my_list = ["hello"]
my_list and print(my_list[0])  # hello

my_list = []
my_list and print(my_list[0])  # Nothing prints

# === Checking for None vs Zero ===

# Be careful! 0 is falsy but might be valid
value = 0

if value is None:
    print("Value is None")
elif value == 0:
    print("Value is zero")  # This prints
else:
    print("Value is " + str(value))

value = None

if value is None:
    print("Value is None")  # This prints
elif value == 0:
    print("Value is zero")
else:
    print("Value is " + str(value))

# === Using is not None ===

# Best way to check if something exists
result = None

if result is not None:
    print("Got result: " + str(result))
else:
    print("No result")  # This prints

result = 42

if result is not None:
    print("Got result: " + str(result))  # Got result: 42
else:
    print("No result")

# === The "in" operator with truthiness ===

# Check if key exists in dict
config = {"debug": True, "log_level": "info"}

if "debug" in config:
    print("Debug mode is set")  # This prints
else:
    print("Debug mode not set")

# === Common Pitfall: Empty String vs None ===

# Both are falsy but mean different things!
name = ""

if name:
    print("Name is set")
else:
    print("Name is empty string")  # This prints

name = None

if name:
    print("Name is set")
else:
    print("Name is None")  # This prints

# === Ternary with Truthiness ===

status = "active"
is_active = True if status == "active" else False

print(is_active)  # True

# Even simpler
is_active = status == "active"

print(is_active)  # True

# === Real-world: Processing Optional Data ===

def process_data(data):
    # If data is None or empty, use default
    if not data:
        data = "No data provided"
    
    return "Processed: " + data

result = process_data("Some data")
print(result)  # Processed: Some data

result = process_data("")
print(result)  # Processed: No data provided

result = process_data(None)
print(result)  # Processed: No data provided

# === Real-world: Safe Division ===

def safe_divide(a, b):
    # Using truthiness to check divisor
    return b and a / b

print(safe_divide(10, 2))   # 5.0
print(safe_divide(10, 0))   # None (0 is falsy, so returns 0, then and short-circuits)

# Better version
def safe_divide2(a, b):
    if b:
        return a / b
    return None

print(safe_divide2(10, 2))  # 5.0
print(safe_divide2(10, 0))  # None

# === Real-world: Toggle Feature ===

feature_enabled = True

# Using not for toggling
if not feature_enabled:
    print("Feature is OFF")
else:
    print("Feature is ON")  # This prints

# === Real-world: Count-based Logic ===

count = 0

if count:
    print("There are items")
else:
    print("No items")  # This prints

count = 5

if count:
    print("There are items")  # This prints
else:
    print("No items")

# === Summary of Best Practices ===

# DO: Use truthiness for simple checks
if my_list:
    print("Has items")

# DO: Use is not None for explicit None checks
if value is not None:
    print("Value is set")

# DON'T: Use if len(list) > 0
# DO: Use if list

# DON'T: Confuse 0 with None
# They mean different things!

# DON'T: Use or with 0 as default (0 is falsy!)
# value = user_value or 0  # Problem if 0 is valid!
# DO: Use ternary or explicit check
