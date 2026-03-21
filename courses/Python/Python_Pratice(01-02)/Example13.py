# Example13.py
# Topic: Type Hints - Basic Syntax

print("=" * 50)
print("TYPE HINTS - BASIC SYNTAX")                # TYPE HINTS - BASIC SYNTAX
print("=" * 50)
# Without Type Hints
print("\n--- Without Type Hints ---\n")           # \n--- Without Type Hints ---\n
    
# Python infers types automatically
name = "Alice"
age = 25
price = 19.99
items = ["apple", "banana"]
    
print("name = " + str(name))
print("age = " + str(age))
print("price = " + str(price))
print("items = " + str(items))
# With Type Hints
print("\n--- With Type Hints ---\n")              # \n--- With Type Hints ---\n
    
# Now we explicitly state the types
name = "Alice" # str  — text, always wrapped in quotes
age = 25 # int  — whole number, no quotes
price = 19.99 # float — decimal number
is_valid = True # bool — can only be True or False
    
print("name: str = " + str(name))
print("age: int = " + str(age))
print("price: float = " + str(price))
print("is_valid: bool = " + str(is_valid))
# Type Hints Don'T Change Runtime
print("\n--- Type Hints Don't Change Runtime ---\n")# \n--- Type Hints Don't Change Runtime ---\n
    
# With and without hints work the same
x = 5 # int  — whole number, no quotes
y = 5
    
print("x: int = 5 and y = 5")                     # x: int = 5 and y = 5
print("x == y: " + str(x == y))
print("Both are integers: " + str(type(x) == type(y)))
# Why Use Type Hints?
print("\n--- Why Use Type Hints? ---\n")          # \n--- Why Use Type Hints? ---\n
    
# 1. Self-documenting code
user_data = {"name": "Alice", "age": 25}
print("user_data = " + str(user_data))
print("With hints, we know user_data is a dict")  # With hints, we know user_data is a dict
    
# 2. IDE autocomplete
message = "Hello" # str  — text, always wrapped in quotes
# Type message. and your IDE will show string methods
print("message.upper() = " + str(message.upper()))
    
# 3. Catch bugs early
score = 100 # int  — whole number, no quotes
print("score = " + str(score))
# Practical Example
print("\n--- Practical Example ---\n")            # \n--- Practical Example ---\n
    
# Function without type hints
def process_order_untyped(order_id, items, price):
    return {"id": order_id, "items": items, "total": price}
    
# Function with type hints
def process_order_typed(order_id: int, items: list[str], price: float) -> dict:
    return {"id": order_id, "items": items, "total": price}
    
# Using the function
order = process_order_typed(123, ["apple", "banana"], 29.99)
print("Order: " + str(order))
# Summary
print("\n" + "=" * 50)
print("BASIC TYPE HINTS SUMMARY")                 # BASIC TYPE HINTS SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- Syntax: variable_name: Type = value")    # - Syntax: variable_name: Type = value
print("- Types: int, float, str, bool, list, dict, set, tuple")# - Types: int, float, str, bool, list, dict, set, tuple
print("- Don't change runtime behavior")          # - Don't change runtime behavior
print("- Help with IDE autocomplete and documentation")# - Help with IDE autocomplete and documentation
print("- Optional but recommended in Python 3.12+")# - Optional but recommended in Python 3.12+

# Real-world example: