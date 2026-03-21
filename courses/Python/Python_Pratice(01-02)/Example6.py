# Example6.py
# Topic: Variables Explained - Complete Annotated Example

# Variables are containers for storing data values
# In Python, variables are created when you assign a value

# A variable is a named container that stores a value
# You create one by writing: name = value

name = "Alice"      # str  — text, always wrapped in quotes
age = 25            # int  — whole number, no quotes
height = 5.6       # float — decimal number
is_student = True  # bool — can only be True or False

# Print each variable to see what it holds
print(name)          # Alice
print(age)           # 25
print(height)        # 5.6
print(is_student)    # True

# Reading — you can use a variable anywhere you would use its value
# Here we add 1 to age and store the result in a new variable
next_year_age = age + 1
print(next_year_age)   # 26

# You can also combine strings using + (called concatenation)
greeting = "Hello, " + name + "!"
print(greeting)        # Hello, Alice!

# Changing — assign a new value to the same variable name
# The old value is replaced completely
age = 26
print(age)             # 26  (was 25)

# Variables can change types in Python (dynamic typing)
age = "twenty-seven"   # Now a string!
print(age)             # twenty-seven

# Real-world example: a basic shopping cart
# Each piece of product info is stored in its own variable
item_name = "Laptop"
item_price = 999.99    # float — price in dollars
quantity = 2           # int  — number of items
in_stock = True        # bool — whether the item is available

# Multiply price by quantity to get the total cost
total = item_price * quantity

print(item_name)   # Laptop
print(item_price)  # 999.99
print(quantity)    # 2
print(total)       # 1999.98
print(in_stock)    # True

# Another real-world example: user profile
username = "pythonlearner"    # str  — username
email = "learn@python.com"   # str  — email address
user_age = 28                # int  — age in years
account_balance = 150.50     # float — account balance
is_verified = True           # bool — account verified status

print("=== User Profile ===")
print("Username: " + username)
print("Email: " + email)
print("Age: " + str(user_age))
print("Balance: $" + str(account_balance))
print("Verified: " + str(is_verified))
