# Example11.py
# Topic: Data Types - NoneType (None)

print("=" * 50)
print("DATA TYPE: NONETYPE (None)")               # DATA TYPE: NONETYPE (None)
print("=" * 50)
# Nonetype Examples
print("\n--- NoneType Examples ---\n")            # \n--- NoneType Examples ---\n
    
# None is the only value of NoneType
nothing = None
result = None
no_value = None
placeholder = None
    
print("nothing = " + str(nothing))
print("result = " + str(result))
print("no_value = " + str(no_value))
print("placeholder = " + str(placeholder))
# Checking For None
print("\n--- Checking for None ---\n")            # \n--- Checking for None ---\n
    
data = None
    
# Using 'is' operator (preferred)
if data is None:
    print("data is None - no data available")         # data is None - no data available
else:
    print("data has value: " + str(data))
    
# Using '==' operator (works but not recommended)
if data == None:
    print("data == None (using ==)")                  # data == None (using ==)
    
# Using 'is not' operator
data2 = "Hello" # str  — text, always wrapped in quotes
if data2 is not None:
    print("data2 is not None: " + str(data2))
# None In Practical Use
print("\n--- None in Practical Use ---\n")        # \n--- None in Practical Use ---\n
    
# Default value (no value provided)
user_name: str | None = None
print("User name: " + str(user_name))
    
# Function with no return value
def greet(name: str) -> None:
    print("Hello, " + str(name) + "!")                # Hello, " + str(name) + "!
    
return_value = greet("Alice")
print("Return value of greet(): " + str(return_value))
    
# Optional parameter
def greet_with_default(name: str, greeting: str | None = None) -> None:
    if greeting is None:
        print("Hello, " + str(name) + "!")                # Hello, " + str(name) + "!
    else:
        print(str(greeting) + ", " + str(name) + "!")
    
greet_with_default("Alice")
greet_with_default("Bob", "Welcome")
# Using Type()
print("\n--- Using type() ---\n")                 # \n--- Using type() ---\n
    
nothing = None
print("nothing = " + str(nothing))
print("type(nothing) = " + str(type(nothing)))
print("type(None) = " + str(type(None)))
    
# Checking type
print("\nnothing is None: " + str(nothing is None))
print("type(nothing) is type(None): " + str(type(nothing) is type(None)))
# None Vs Empty Values
print("\n--- None vs Empty Values ---\n")         # \n--- None vs Empty Values ---\n
    
# None = no value at all
# Empty = has a value, but it's empty
    
none_value = None
empty_string = "" # str  — text, always wrapped in quotes
empty_list = []
empty_dict = {}
zero = 0 # int  — whole number, no quotes
false_value = False # bool — can only be True or False
    
print("None: " + str(none_value) + " - type: " + str(type(none_value)))
print("Empty string: '" + str(empty_string) + "' - bool: " + str(bool(empty_string)))
print("Empty list: " + str(empty_list) + " - bool: " + str(bool(empty_list)))
print("Empty dict: " + str(empty_dict) + " - bool: " + str(bool(empty_dict)))
print("Zero: " + str(zero) + " - bool: " + str(bool(zero)))
print("False: " + str(false_value) + " - bool: " + str(bool(false_value)))
    
print("\nKey difference:")                        # \nKey difference:
print("None is None: " + str(none_value is None))
print("'' is None: " + str(empty_string is None))
print("[] is None: " + str(empty_list is None))
print("0 is None: " + str(zero is None))
print("False is None: " + str(false_value is None))
# Practical Examples
print("\n--- Practical Examples ---\n")           # \n--- Practical Examples ---\n
    
# Database query result
def get_user(user_id: int) -> dict | None:
    # Simulate database lookup
    if user_id == 1:
        return {"name": "Alice", "email": "alice@example.com"}
    else:
        return None
    
# User found
user: dict | None = get_user(1)
if user is not None:
    print("Found user: " + str(user['name']))
else:
    print("User not found")                           # User not found
    
# User not found
user2: dict | None = get_user(999)
if user2 is not None:
    print("Found user: " + str(user2['name']))
else:
    print("User not found")                           # User not found
    
# Optional chaining simulation
config: dict | None = {"settings": {"theme": "dark"}}
    
# Without optional chaining (traditional way)
theme = "default" # str  — text, always wrapped in quotes
if config is not None and "settings" in config and "theme" in config["settings"]:
    theme = config["settings"]["theme"]
print("Theme: " + str(theme))
    
# Using walrus operator (Python 3.8+)
if (settings := config) and (theme_val := settings.get("settings", {}).get("theme")):
    print("Theme with walrus: " + str(theme_val))
else:
    print("Theme not found")                          # Theme not found
# None In Data Structures
print("\n--- None in Data Structures ---\n")      # \n--- None in Data Structures ---\n
    
# List with None
items = [1, None, 3, None, 5]
print("List with None: " + str(items))
    
# Count None values
none_count = items.count(None) # int  — whole number, no quotes
print("Number of None values: " + str(none_count))
    
# Filter None values
filtered = [x for x in items if x is not None]
print("Filtered (no None): " + str(filtered))
    
# Dict with None values
person = {"name": "Alice", "email": None, "phone": None}
print("\nDict with None: " + str(person))
# Summary
print("\n" + "=" * 50)
print("NONETYPE (None) SUMMARY")                  # NONETYPE (None) SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- None is Python's 'nothing' - absence of value")# - None is Python's 'nothing' - absence of value
print("- The only value of NoneType")             # - The only value of NoneType
print("- Use 'is None' or 'is not None' to check")# - Use 'is None' or 'is not None' to check
print("- Different from empty values: None != '' != [] != 0")# - Different from empty values: None != '' != [] != 0
print("- Used for: default values, missing data, no return")# - Used for: default values, missing data, no return
print("- bool(None) = False (falsy)")             # - bool(None) = False (falsy)

# Real-world example: