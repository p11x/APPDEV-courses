# Example42.py
# Topic: Control Flow — Pythonic Way to Check Collections

# Using truthiness is more Pythonic than using len()

# === Checking Lists ===

# Old way (verbose)
my_list = [1, 2, 3]

if len(my_list) > 0:
    print("List has items")  # This prints
else:
    print("List is empty")

my_list = []

if len(my_list) > 0:
    print("List has items")
else:
    print("List is empty")    # This prints

# Pythonic way (better!)
my_list = [1, 2, 3]

if my_list:
    print("List has items")  # This prints
else:
    print("List is empty")

my_list = []

if my_list:
    print("List has items")
else:
    print("List is empty")    # This prints

# === Checking Dictionaries ===

# Old way
my_dict = {"name": "Alice", "age": 25}

if len(my_dict) > 0:
    print("Dict has items")  # This prints
else:
    print("Dict is empty")

my_dict = {}

if len(my_dict) > 0:
    print("Dict has items")
else:
    print("Dict is empty")    # This prints

# Pythonic way
my_dict = {"name": "Alice"}

if my_dict:
    print("Dict has items")  # This prints
else:
    print("Dict is empty")

my_dict = {}

if my_dict:
    print("Dict has items")
else:
    print("Dict is empty")    # This prints

# === Checking Strings ===

# Old way
my_str = "hello"

if len(my_str) > 0:
    print("String is not empty")  # This prints
else:
    print("String is empty")

my_str = ""

if len(my_str) > 0:
    print("String is not empty")
else:
    print("String is empty")      # This prints

# Pythonic way
my_str = "hello"

if my_str:
    print("String is not empty")  # This prints
else:
    print("String is empty")

my_str = ""

if my_str:
    print("String is not empty")
else:
    print("String is empty")      # This prints

# === Real-world example: Shopping cart ===

cart = []

if cart:
    print("Cart has items: " + str(cart))
else:
    print("Cart is empty")        # This prints

cart = ["apple", "banana", "orange"]

if cart:
    print("Cart has items: " + str(cart))  # Cart has items: ['apple', 'banana', 'orange']

# === Real-world example: User preferences ===

preferences = {}

if preferences:
    print("Preferences set: " + str(preferences))
else:
    print("No preferences configured")  # This prints

preferences = {"theme": "dark", "notifications": True}

if preferences:
    print("Preferences set: " + str(preferences))  # Preferences set: {'theme': 'dark', 'notifications': True}

# === Real-world example: Form validation ===

form_data = {}

if form_data:
    print("Processing form: " + str(form_data))
else:
    print("No form data submitted")  # This prints

form_data = {"username": "alice", "email": "alice@example.com"}

if form_data:
    print("Processing form: " + str(form_data))  # Processing form: {'username': 'alice', 'email': 'alice@example.com'}

# === Real-world example: Log messages ===

messages = []

if messages:
    for msg in messages:
        print("Log: " + msg)
else:
    print("No log messages")  # This prints

messages = ["User logged in", "File uploaded", "Email sent"]

if messages:
    for msg in messages:
        print("Log: " + msg)
# Log: User logged in
# Log: File uploaded
# Log: Email sent

# === Why Pythonic is better ===
# 1. More readable
# 2. Less typing
# 3. Works for any collection type
# 4. Python community standard

# === Checking with else clause ===

items = []

if items:
    print("Found " + str(len(items)) + " items")
else:
    print("No items found")  # This prints

items = [1, 2, 3]

if items:
    print("Found " + str(len(items)) + " items")  # Found 3 items
else:
    print("No items found")
