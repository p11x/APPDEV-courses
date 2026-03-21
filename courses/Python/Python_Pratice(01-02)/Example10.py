# Example10.py
# Topic: Data Types - Boolean (bool)

print("=" * 50)
print("DATA TYPE: BOOLEAN (bool)")                # DATA TYPE: BOOLEAN (bool)
print("=" * 50)
# Boolean Examples
print("\n--- Boolean Examples ---\n")             # \n--- Boolean Examples ---\n
    
# Boolean literals (capital T and F!)
is_active = True       # True (capital T) # bool — can only be True or False
is_complete = False   # False (capital F) # bool — can only be True or False
    
print(""")                                        # "
print("""

)                                        # "
# Boolean From Comparisons
print("\n--- Boolean from Comparisons ---\n")     # \n--- Boolean from Comparisons ---\n
    
a = 10 # int  — whole number, no quotes
b = 5 # int  — whole number, no quotes
    
# Comparison operators return booleans
is_greater = a > b # bool — can only be True or False
is_less = a < b # bool — can only be True or False
is_equal = a == b       # == for comparison # bool — can only be True or False
is_not_equal = a != b # bool — can only be True or False
is_greater_equal = a >= b # bool — can only be True or False
is_less_equal = a <= b # bool — can only be True or False
    
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
# Boolean Operations
print("\n--- Boolean Operations ---\n")           # \n--- Boolean Operations ---\n
    
# Logical AND
result_and = True and True # bool — can only be True or False
print("""

)                                        # "
    
# Logical OR
result_or = True or False # bool — can only be True or False
print(""")                                        # "
    
# Logical NOT
result_not = not True # bool — can only be True or False
print("""

)                                        # "
# Truthy And Falsy Values
print("\n--- Truthy and Falsy Values ---\n")      # \n--- Truthy and Falsy Values ---\n
    
# Falsy values (convert to False)
print("Falsy values (evaluate to False):")        # Falsy values (evaluate to False):
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")  # Empty string
print(f"bool([]) = {bool([])}")  # Empty list
print(f"bool({{}}) = {bool({})}")  # Empty dict
print("""

)  # Empty set
    
# Truthy values (convert to True)
print("\nTruthy values (evaluate to True):")      # \nTruthy values (evaluate to True):
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")  # Non-empty string
print(f"bool([1,2]) = {bool([1,2])}")  # Non-empty list
print(f"bool({{'a':1}}) = {bool({'a':1})}")  # Non-empty dict
# Using Type()
print("\n--- Using type() ---\n")                 # \n--- Using type() ---\n
    
flag = True # bool — can only be True or False
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
# Practical Examples
print("\n--- Practical Examples ---\n")           # \n--- Practical Examples ---\n
    
# User validation
username = "alice" # str  — text, always wrapped in quotes
password = "secret123" # str  — text, always wrapped in quotes
    
is_valid_username = len(username) >= 3 # bool — can only be True or False
is_valid_password = len(password) >= 6 # bool — can only be True or False
    
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
    
# Age check
age = 25 # int  — whole number, no quotes
is_adult = age >= 18 # bool — can only be True or False
is_senior = age >= 65 # bool — can only be True or False
    
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
    
# Number check
number = 42 # int  — whole number, no quotes
is_positive = number > 0 # bool — can only be True or False
is_even = number % 2 == 0 # bool — can only be True or False
is_divisible_by_7 = number % 7 == 0 # bool — can only be True or False
    
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
    
# String check
text = "Hello World" # str  — text, always wrapped in quotes
starts_with_hello = text.startswith("Hello") # bool — can only be True or False
ends_with_world = text.endswith("World") # bool — can only be True or False
contains_python = "Python" in text # bool — can only be True or False
    
print("""

)                                        # "
print(""")                                        # "
print("""

)                                        # "
print(""")                                        # "
# Boolean In Conditionals
print("\n--- Boolean in Conditionals ---\n")      # \n--- Boolean in Conditionals ---\n
    
is_logged_in = True # bool — can only be True or False
    
if is_logged_in:
    print("Welcome back! You are logged in.")         # Welcome back! You are logged in.
else:
    print("Please log in to continue.")               # Please log in to continue.
    
# Ternary operator
status = "Active" if is_logged_in else "Guest" # str  — text, always wrapped in quotes
print("""

)                                        # "
# Summary
print("\n" + "=" * 50)
print("BOOLEAN (bool) SUMMARY")                   # BOOLEAN (bool) SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- Booleans are: True or False (capital T/F)")# - Booleans are: True or False (capital T/F)
print("- Comparison operators return booleans: >, <, ==, !=, >=, <=")# - Comparison operators return booleans: >, <, ==, !=, >=, <=
print("- Logical operators: and, or, not")        # - Logical operators: and, or, not
print("- Falsy: False, None, 0, 0.0, '', [], {}, set()")# - Falsy: False, None, 0, 0.0, '', [], {}, set()
print("- Truthy: Everything else (non-zero, non-empty)")# - Truthy: Everything else (non-zero, non-empty)
print("- Used in conditionals and logical operations")# - Used in conditionals and logical operations

# Real-world example: