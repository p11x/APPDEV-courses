# Example3.py
# Topic: Variables Explained - Variable Naming Rules and snake_case

user_name = "Alice" # str  — text, always wrapped in quotes
age_in_years = 25 # int  — whole number, no quotes
total_price = 99.99 # float — decimal number
is_active = True # bool — can only be True or False
first_name = "John" # str  — text, always wrapped in quotes
last_name = "Doe" # str  — text, always wrapped in quotes
email_address = "alice@example.com" # str  — text, always wrapped in quotes
number_of_items = 10 # int  — whole number, no quotes
created_at = "2024-01-15" # str  — text, always wrapped in quotes
is_premium_user = False # bool — can only be True or False
    
print("=== Good Variable Names (snake_case) ===") # === Good Variable Names (snake_case) ===
print("user_name: " + str(user_name))
print("age_in_years: " + str(age_in_years))
print("total_price: " + str(total_price))
print("is_active: " + str(is_active))
print("first_name: " + str(first_name))
print("last_name: " + str(last_name))
# Bad Variable Names (Avoid These!)
# These don't follow Python conventions
    
# ❌ camelCase (used in JavaScript/Java)
userName = "Bob"      # Avoid! # str  — text, always wrapped in quotes
    
# ❌ PascalCase (used for classes)
UserName = "Charlie"   # Avoid! # str  — text, always wrapped in quotes
    
# ❌ SCREAMING_SNAKE_CASE (used for constants)
USER_NAME = "David"    # Avoid! # str  — text, always wrapped in quotes
    
# ❌ Too short, unclear
n = "Eric"             # Avoid! # str  — text, always wrapped in quotes
x = 10                 # Avoid! # int  — whole number, no quotes
    
# ❌ Using numbers at start
# 123abc: str = "invalid"   # This would cause a syntax error!
    
# ❌ Using hyphens (not allowed in Python)
# my-variable: str = "invalid"  # This would cause a syntax error!
    
# ❌ Using Python keywords
# class: str = "invalid"  # This would cause a syntax error!
# if: int = 1              # This would cause a syntax error!
    
print("\n=== Bad Variable Names (Avoid These!) ===")# \n=== Bad Variable Names (Avoid These!) ===
print("userName (camelCase): " + str(userName))
print("UserName (PascalCase): " + str(UserName))
print("USER_NAME (SCREAMING_SNAKE): " + str(USER_NAME))
print("n (too short): " + str(n))
print("x (too short): " + str(x))
# Keywords You Cannot Use
# Python reserved words cannot be variable names
# Here's how to check if something is a keyword
    
import keyword
    
print("\n=== Python Keywords (Cannot Use as Variables) ===")# \n=== Python Keywords (Cannot Use as Variables) ===
python_keywords = [
    "and", "as", "assert", "async", "await", "break", "class", 
    "continue", "de", "del", "eli", "else", "except", "False",
    "finally", "for", "from", "global", "i", "import", "in",
    "is", "lambda", "None", "nonlocal", "not", "or", "pass",
    "raise", "return", "True", "try", "while", "with", "yield"
]
print("Total keywords: " + str(len(python_keywords)))
print("Example: 'class' is a keyword: " + str(keyword.iskeyword('class')))
print("Example: 'name' is a keyword: " + str(keyword.iskeyword('name')))
# Practical Naming Examples
print("\n=== Practical Naming Examples ===")      # \n=== Practical Naming Examples ===
    
# Good: Descriptive names for a user profile
user_first_name = "Alice" # str  — text, always wrapped in quotes
user_last_name = "Smith" # str  — text, always wrapped in quotes
user_email = "alice@email.com" # str  — text, always wrapped in quotes
user_age = 30 # int  — whole number, no quotes
is_user_verified = True # bool — can only be True or False
user_registration_date = "2024-01-01" # str  — text, always wrapped in quotes
    
print("User: " + str(user_first_name) + " " + str(user_last_name))
print("Email: " + str(user_email))
print("Age: " + str(user_age))
print("Verified: " + str(is_user_verified))
    
# Good: Descriptive names for a product
product_name = "Wireless Mouse" # str  — text, always wrapped in quotes
product_price = 29.99 # float — decimal number
product_quantity_in_stock = 150 # int  — whole number, no quotes
is_product_available = True # bool — can only be True or False
product_category = "Electronics" # str  — text, always wrapped in quotes
    
print("\nProduct: " + str(product_name))
print("Price: $" + str(product_price))
print("Stock: " + str(product_quantity_in_stock))
print("Available: " + str(is_product_available))

# Real-world example: