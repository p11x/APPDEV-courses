# Example12.py
# Topic: Data Types - Type Conversion (Casting)

print("=" * 50)
print("TYPE CONVERSION (CASTING)")                # TYPE CONVERSION (CASTING)
print("=" * 50)
# Implicit Conversion (Automatic)
print("\n--- Implicit Conversion (Automatic) ---\n")# \n--- Implicit Conversion (Automatic) ---\n
    
# Integer + Integer = Integer
result1 = 10 + 5 # int  — whole number, no quotes
print("int + int: 10 + 5 = " + str(result1) + " (type: " + str(type(result1).__name__) + ")")# int + int: 10 + 5 = " + str(result1) + " (type: " + str(type(result1).__name__) + ")# int + int: 10 + 5 = " + str(result1) + " (type: " + str(type(result1).__name__) + ")")# int + int: 10 + 5 = " + str(result1) + " (type: " + str(type(result1).__name__) + 
    
# Float + Float = Float
result2 = 3.14 + 2.86 # float — decimal number
print("float + float: 3.14 + 2.86 = " + str(result2) + " (type: " + str(type(result2).__name__) + ")")# float + float: 3.14 + 2.86 = " + str(result2) + " (type: " + str(type(result2).__name__) + ")# float + float: 3.14 + 2.86 = " + str(result2) + " (type: " + str(type(result2).__name__) + ")")# float + float: 3.14 + 2.86 = " + str(result2) + " (type: " + str(type(result2).__name__) + 
    
# Integer + Float = Float (integer promoted to float)
result3 = 10 + 5.5 # float — decimal number
print("int + float: 10 + 5.5 = " + str(result3) + " (type: " + str(type(result3).__name__) + ")")# int + float: 10 + 5.5 = " + str(result3) + " (type: " + str(type(result3).__name__) + ")# int + float: 10 + 5.5 = " + str(result3) + " (type: " + str(type(result3).__name__) + ")")# int + float: 10 + 5.5 = " + str(result3) + " (type: " + str(type(result3).__name__) + 
    
# Integer + Boolean = Integer
result4 = 10 + True  # True = 1 # int  — whole number, no quotes
print("int + bool: 10 + True = " + str(result4) + " (type: " + str(type(result4).__name__) + ")")# int + bool: 10 + True = " + str(result4) + " (type: " + str(type(result4).__name__) + ")# int + bool: 10 + True = " + str(result4) + " (type: " + str(type(result4).__name__) + ")")# int + bool: 10 + True = " + str(result4) + " (type: " + str(type(result4).__name__) + 
    
# Float + Boolean = Float
result5 = 10.0 + True # float — decimal number
print("float + bool: 10.0 + True = " + str(result5) + " (type: " + str(type(result5).__name__) + ")")# float + bool: 10.0 + True = " + str(result5) + " (type: " + str(type(result5).__name__) + ")# float + bool: 10.0 + True = " + str(result5) + " (type: " + str(type(result5).__name__) + ")")# float + bool: 10.0 + True = " + str(result5) + " (type: " + str(type(result5).__name__) + 
# Explicit Conversion (Manual)
print("\n--- Explicit Conversion (Manual) ---\n") # \n--- Explicit Conversion (Manual) ---\n
    
# String to Integer
age_str = "25" # str  — text, always wrapped in quotes
age_int = int(age_str) # int  — whole number, no quotes
print("int('25') = " + str(age_int) + " (type: " + str(type(age_int).__name__) + ")")# int('25') = " + str(age_int) + " (type: " + str(type(age_int).__name__) + ")# int('25') = " + str(age_int) + " (type: " + str(type(age_int).__name__) + ")")# int('25') = " + str(age_int) + " (type: " + str(type(age_int).__name__) + 
    
# String to Float
price_str = "19.99" # str  — text, always wrapped in quotes
price_float = float(price_str) # float — decimal number
print("float('19.99') = " + str(price_float) + " (type: " + str(type(price_float).__name__) + ")")# float('19.99') = " + str(price_float) + " (type: " + str(type(price_float).__name__) + ")# float('19.99') = " + str(price_float) + " (type: " + str(type(price_float).__name__) + ")")# float('19.99') = " + str(price_float) + " (type: " + str(type(price_float).__name__) + 
    
# Integer to String
count = 42 # int  — whole number, no quotes
count_str = str(count) # str  — text, always wrapped in quotes
print("str(42) = '" + str(count_str) + "' (type: " + str(type(count_str).__name__) + ")")# str(42) = '" + str(count_str) + "' (type: " + str(type(count_str).__name__) + ")# str(42) = '" + str(count_str) + "' (type: " + str(type(count_str).__name__) + ")")# str(42) = '" + str(count_str) + "' (type: " + str(type(count_str).__name__) + 
    
# Float to Integer (truncates decimal!)
pi = 3.14159 # float — decimal number
pi_int = int(pi) # int  — whole number, no quotes
print("int(3.14159) = " + str(pi_int) + " (type: " + str(type(pi_int).__name__) + ") - loses decimal!")# int(3.14159) = " + str(pi_int) + " (type: " + str(type(pi_int).__name__) + ") - loses decimal!
    
# Integer to Float
count = 42 # int  — whole number, no quotes
count_float = float(count) # float — decimal number
print("float(42) = " + str(count_float) + " (type: " + str(type(count_float).__name__) + ")")# float(42) = " + str(count_float) + " (type: " + str(type(count_float).__name__) + ")# float(42) = " + str(count_float) + " (type: " + str(type(count_float).__name__) + ")")# float(42) = " + str(count_float) + " (type: " + str(type(count_float).__name__) + 
# Conversion Functions
print("\n--- Conversion Functions ---\n")         # \n--- Conversion Functions ---\n
    
conversions = [
    ("int('42')", int('42'), "String to integer"),
    ("float('3.14')", float('3.14'), "String to float"),
    ("str(42)", str(42), "Integer to string"),
    ("str(3.14)", str(3.14), "Float to string"),
    ("bool(1)", bool(1), "Integer to boolean"),
    ("bool(0)", bool(0), "Zero to boolean"),
    ("bool('text')", bool('text'), "String to boolean"),
    ("bool('')", bool(''), "Empty string to boolean"),
    ("int(True)", int(True), "Boolean to integer"),
    ("int(False)", int(False), "False to integer"),
    ("list('abc')", list('abc'), "String to list"),
    ("list((1,2,3))", list((1, 2, 3)), "Tuple to list"),
]
    
for func_call, result, description in conversions:
    print(str(func_call) + " = " + str(str(result)) + " # " + str(description))
# Common Conversion Mistakes
print("\n--- Common Conversion Mistakes ---\n")   # \n--- Common Conversion Mistakes ---\n
    
print("ERROR: Cannot convert random text to number")# ERROR: Cannot convert random text to number
print("  int('hello')  → ValueError!")            #   int('hello')  → ValueError!
print("  float('world')  → ValueError!")          #   float('world')  → ValueError!
    
print("\nERROR: Cannot convert decimal string directly to int")# \nERROR: Cannot convert decimal string directly to int
print("  int('3.14')  → ValueError!")             #   int('3.14')  → ValueError!
print("  float('3.14')  → 3.14 (correct!)")       #   float('3.14')  → 3.14 (correct!)
    
print("\nERROR: Truncation when converting float to int")# \nERROR: Truncation when converting float to int
print("  int(3.99)  → 3 (not 4!)")                #   int(3.99)  → 3 (not 4!)
print("  Use round() first: int(round(3.99))  → 4")#   Use round() first: int(round(3.99))  → 4
# Practical Examples
print("\n--- Practical Examples ---\n")           # \n--- Practical Examples ---\n
    
# User input processing
# Simulating input (normally from input())
user_input = "25" # str  — text, always wrapped in quotes
    
# Convert string to integer for calculation
age = int(user_input) # int  — whole number, no quotes
print("User input: '" + str(user_input) + "'")    # User input: '" + str(user_input) + "'
print("Age + 5: " + str(age + 5))
    
# String to float for price
price_input = "29.99" # str  — text, always wrapped in quotes
price = float(price_input) # float — decimal number
tax = price * 0.08 # float — decimal number
total = price + tax # float — decimal number
print("\nPrice input: '" + str(price_input) + "'")# \nPrice input: '" + str(price_input) + "'
print("Price: $" + str(price))
print("Tax (8%): $" + str(tax))
print("Total: $" + str(total))
    
# Number to string for display
count = 42 # int  — whole number, no quotes
message = "You have " + str(count) + " messages" # str  — text, always wrapped in quotes
print("\nCount: " + str(count))
print("Message: " + str(message))
    
# Boolean conversion
is_active = True # bool — can only be True or False
status = "Active" if is_active else "Inactive" # str  — text, always wrapped in quotes
print("\nBool to string: " + str(is_active) + " → '" + str(status) + "'")# \nBool to string: " + str(is_active) + " → '" + str(status) + "'
    
# Integer to boolean
number = 1 # int  — whole number, no quotes
flag = bool(number) # bool — can only be True or False
print("Int to bool: " + str(number) + " → " + str(flag))
    
number_zero = 0 # int  — whole number, no quotes
flag_zero = bool(number_zero) # bool — can only be True or False
print("Zero to bool: " + str(number_zero) + " → " + str(flag_zero))
    
# List conversion
text = "hello" # str  — text, always wrapped in quotes
letters = list(text)
print("\nString to list: '" + str(text) + "' → " + str(letters))
# Type Checking
print("\n--- Type Checking ---\n")                # \n--- Type Checking ---\n
    
value = 42 # int  — whole number, no quotes
print("value = " + str(value))
print("isinstance(value, int): " + str(isinstance(value, int)))
print("isinstance(value, float): " + str(isinstance(value, float)))
print("isinstance(value, str): " + str(isinstance(value, str)))
    
value2 = 3.14 # float — decimal number
print("\nvalue2 = " + str(value2))
print("isinstance(value2, float): " + str(isinstance(value2, float)))
print("isinstance(value2, (int, float)): " + str(isinstance(value2, (int, float))))
# Summary
print("\n" + "=" * 50)
print("TYPE CONVERSION SUMMARY")                  # TYPE CONVERSION SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- Implicit: Python converts automatically (int + float → float)")# - Implicit: Python converts automatically (int + float → float)
print("- Explicit: Use int(), float(), str(), bool()")# - Explicit: Use int(), float(), str(), bool()
print("- int('42') → 42")                         # - int('42') → 42
print("- float('3.14') → 3.14")                   # - float('3.14') → 3.14
print("- str(42) → '42'")                         # - str(42) → '42'
print("- int(3.14) → 3 (truncates!)")             # - int(3.14) → 3 (truncates!)
print("- Use isinstance() to check types")        # - Use isinstance() to check types

# Real-world example: