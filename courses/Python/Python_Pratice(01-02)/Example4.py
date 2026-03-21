# Example4.py
# Topic: Variables Explained - Type Hints (Python 3.12+)

name = "Alice"      # Python knows this is a string
age = 25            # Python knows this is an int
height = 5.6       # Python knows this is a float
is_active = True   # Python knows this is a bool
    
print("=== Without Type Hints (Python infers type) ===")# === Without Type Hints (Python infers type) ===
print("name: " + str(name) + " (type: " + str(type(name).__name__) + ")")# name: " + str(name) + " (type: " + str(type(name).__name__) + ")# name: " + str(name) + " (type: " + str(type(name).__name__) + ")")# name: " + str(name) + " (type: " + str(type(name).__name__) + 
print("age: " + str(age) + " (type: " + str(type(age).__name__) + ")")# age: " + str(age) + " (type: " + str(type(age).__name__) + ")# age: " + str(age) + " (type: " + str(type(age).__name__) + ")")# age: " + str(age) + " (type: " + str(type(age).__name__) + 
print("height: " + str(height) + " (type: " + str(type(height).__name__) + ")")# height: " + str(height) + " (type: " + str(type(height).__name__) + ")# height: " + str(height) + " (type: " + str(type(height).__name__) + ")")# height: " + str(height) + " (type: " + str(type(height).__name__) + 
print("is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + ")")# is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + ")# is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + ")")# is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + 
# With Type Hint
# We explicitly tell Python what type it is
name = "Alice" # str  — text, always wrapped in quotes
age = 25 # int  — whole number, no quotes
height = 5.6 # float — decimal number
is_active = True # bool — can only be True or False
    
print("\n=== With Type Hints (Explicit types) ===")# \n=== With Type Hints (Explicit types) ===
print("name: " + str(name) + " (type: " + str(type(name).__name__) + ")")# name: " + str(name) + " (type: " + str(type(name).__name__) + ")# name: " + str(name) + " (type: " + str(type(name).__name__) + ")")# name: " + str(name) + " (type: " + str(type(name).__name__) + 
print("age: " + str(age) + " (type: " + str(type(age).__name__) + ")")# age: " + str(age) + " (type: " + str(type(age).__name__) + ")# age: " + str(age) + " (type: " + str(type(age).__name__) + ")")# age: " + str(age) + " (type: " + str(type(age).__name__) + 
print("height: " + str(height) + " (type: " + str(type(height).__name__) + ")")# height: " + str(height) + " (type: " + str(type(height).__name__) + ")# height: " + str(height) + " (type: " + str(type(height).__name__) + ")")# height: " + str(height) + " (type: " + str(type(height).__name__) + 
print("is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + ")")# is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + ")# is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + ")")# is_active: " + str(is_active) + " (type: " + str(type(is_active).__name__) + 
# Why Use Type Hints?
print("\n=== Why Use Type Hints? ===")            # \n=== Why Use Type Hints? ===
    
# 1. Readability - Others know what to expect
user_name = "Alice" # str  — text, always wrapped in quotes
user_score = 100 # int  — whole number, no quotes
user_rating = 4.5 # float — decimal number
    
# 2. IDE support - Better autocomplete
# When you type "user_name.", your IDE knows it's a string
# and shows string methods like .upper(), .lower(), etc.
    
# 3. Documentation - Acts as inline documentation
# This function clearly shows what types it expects
def greet(user_name: str) -> None:
    print("Hello, " + str(user_name) + "!")           # Hello, " + str(user_name) + "!
    
greet("Alice")
    
# 4. Catches bugs - Tools like mypy can find errors
# If you pass an int where a str is expected, mypy will warn you
# Type Hint Syntax
print("\n=== Type Hint Syntax ===")               # \n=== Type Hint Syntax ===
# Variable name followed by colon, then type, then equals value
# variable_name: Type = value
    
# Basic types
count = 0 # int  — whole number, no quotes
price = 19.99 # float — decimal number
message = "Hello" # str  — text, always wrapped in quotes
is_enabled = True # bool — can only be True or False
nothing = None
    
# Collections
items = [1, 2, 3]
person = {"name": "Alice", "age": 25}
    
print("count: " + str(count) + " (int)")          # count: " + str(count) + " (int)
print("price: " + str(price) + " (float)")        # price: " + str(price) + " (float)
print("message: " + str(message) + " (str)")      # message: " + str(message) + " (str)
print("is_enabled: " + str(is_enabled) + " (bool)")# is_enabled: " + str(is_enabled) + " (bool)
print("nothing: " + str(nothing) + " (None)")     # nothing: " + str(nothing) + " (None)
print("items: " + str(items) + " (list)")         # items: " + str(items) + " (list)
print("person: " + str(person) + " (dict)")       # person: " + str(person) + " (dict)
# Advanced Type Hints
print("\n=== Advanced Type Hints ===")            # \n=== Advanced Type Hints ===
    
# Union types (Python 3.10+)
# status: str | int = "active"  # Can be string or integer
    
# Optional type (can be None)
# nickname: str | None = None
    
# Custom types
# from typing import List, Dict, Tuple
# Practical Example
print("\n=== Practical Example with Type Hints ===")# \n=== Practical Example with Type Hints ===
    
# User profile with type hints
user_id = 12345 # int  — whole number, no quotes
username = "alice_dev" # str  — text, always wrapped in quotes
email = "alice@example.com" # str  — text, always wrapped in quotes
account_balance = 1250.50 # float — decimal number
is_premium = True # bool — can only be True or False
tags = ["python", "developer", "premium"]
profile = {
    "bio": "Python developer",
    "location": "New York",
    "joined": "2024-01-15"
}
    
print("User ID: " + str(user_id))
print("Username: " + str(username))
print("Email: " + str(email))
print("Balance: $" + str(account_balance))
print("Premium: " + str(is_premium))
print("Tags: " + str(tags))
print("Profile: " + str(profile))
    
# Calculate with type-hinted variables
bonus = account_balance * 0.10  # 10% bonus # float — decimal number
print("10% Bonus: $" + str(bonus))

# Real-world example: