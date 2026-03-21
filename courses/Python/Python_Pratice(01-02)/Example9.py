# Example9.py
# Topic: Data Types - String (str)

"""
    Demonstrates String (str) data type in Python.
    Strings hold text - sequences of characters.
    """

    
print("=" * 50)
print("DATA TYPE: STRING (str)")                  # DATA TYPE: STRING (str)
print("=" * 50)
# String Examples
print("\n--- String Examples ---\n")              # \n--- String Examples ---\n
    
# Different ways to create strings
double_quotes = "Hello World" # str  — text, always wrapped in quotes
single_quotes = 'Hello World' # str  — text, always wrapped in quotes
mixed_quotes = "It's a great day!" # str  — text, always wrapped in quotes
escape_example = "He said \"Hello\"" # str  — text, always wrapped in quotes
    
# Empty string
empty = "" # str  — text, always wrapped in quotes
    
# Multiline strings
multiline = "Line 1\nLine 2\nLine 3" # str  — text, always wrapped in quotes
    
print("Double quotes: " + str(double_quotes))
print("Single quotes: " + str(single_quotes))
print("Mixed quotes: " + str(mixed_quotes))
print("Escape example: " + str(escape_example))
print("Empty string: '" + str(empty) + "'")       # Empty string: '" + str(empty) + "'
print("Multiline:\n" + str(multiline))
# String Operations
print("\n--- String Operations ---\n")            # \n--- String Operations ---\n
    
# Concatenation
first = "Hello" # str  — text, always wrapped in quotes
second = "World" # str  — text, always wrapped in quotes
combined = first + " " + second # str  — text, always wrapped in quotes
print("'" + str(first) + "' + ' ' + '" + str(second) + "' = '" + str(combined) + "'")# '" + str(first) + "' + ' ' + '" + str(second) + "' = '" + str(combined) + "'
    
# Repetition
repeated = "Ha" * 3 # str  — text, always wrapped in quotes
print("'Ha' * 3 = '" + str(repeated) + "'")       # 'Ha' * 3 = '" + str(repeated) + "'
    
# Length
text = "Python" # str  — text, always wrapped in quotes
length = len(text) # int  — whole number, no quotes
print("len('" + str(text) + "') = " + str(length))
    
# Indexing
print("\nIndexing '" + str(text) + "':")          # \nIndexing '" + str(text) + "':
print(f"text[0] = '{text[0]}' (first character)")
print(f"text[1] = '{text[1]}' (second character)")
print(f"text[-1] = '{text[-1]}' (last character)")
print(f"text[-2] = '{text[-2]}' (second to last)")
    
# Slicing
print("\nSlicing '" + str(text) + "':")           # \nSlicing '" + str(text) + "':
print(f"text[0:3] = '{text[0:3]}' (first 3 chars)")
print(f"text[2:] = '{text[2:]}' (from index 2 to end)")
print(f"text[:4] = '{text[:4]}' (up to index 4)")
print(f"text[::2] = '{text[::2]}' (every other char)")
print(f"text[::-1] = '{text[::-1]}' (reversed)")
# String Methods
print("\n--- String Methods ---\n")               # \n--- String Methods ---\n
    
text = "  Hello, Python World!  " # str  — text, always wrapped in quotes
print("Original: '" + str(text) + "'")            # Original: '" + str(text) + "'
print(f".strip() = '{text.strip()}'")
print(f".upper() = '{text.strip().upper()}'")
print(f".lower() = '{text.strip().lower()}'")
print(f".capitalize() = '{text.strip().capitalize()}'")
print(f".title() = '{text.strip().title()}'")
    
# Replace and split
replaced = text.replace("Python", "Amazing") # str  — text, always wrapped in quotes
print(f".replace('Python', 'Amazing') = '{replaced.strip()}'")
    
split_result = "apple,banana,cherry".split(",")
print("'apple,banana,cherry'.split(',') = " + str(split_result))
    
# Join
joined = "-".join(["2024", "01", "15"]) # str  — text, always wrapped in quotes
print("'-'.join(['2024', '01', '15']) = '" + str(joined) + "'")# '-'.join(['2024', '01', '15']) = '" + str(joined) + "'
    
# Find and count
sentence = "The quick brown fox jumps over the lazy dog" # str  — text, always wrapped in quotes
print("\n'" + str(sentence) + "'")                # \n'" + str(sentence) + "'
print(f".find('fox') = {sentence.find('fox')}")
print(f".count('o') = {sentence.count('o')}")
# Formatted Strings (F-Strings)
print("\n--- F-strings ---\n")                    # \n--- F-strings ---\n
    
name = "Alice" # str  — text, always wrapped in quotes
age = 30 # int  — whole number, no quotes
price = 19.99 # float — decimal number
    
# Simple f-string
print("My name is " + str(name))
    
# With expressions
print("Next year I'll be " + str(age + 1))
    
# Format numbers
print("Price: $" + str(price))
    
# Format with alignment
print(str('Name') + " " + str('Age'))
print(str(name) + " " + str(age))
# Using Type()
print("\n--- Using type() ---\n")                 # \n--- Using type() ---\n
    
text = "hello" # str  — text, always wrapped in quotes
print("text = '" + str(text) + "'")               # text = '" + str(text) + "'
print(f"type(text) = {type(text)}")
print(f"type('hello') = {type('hello')}")
print(f"type('') = {type('')}")
# Practical Examples
print("\n--- Practical Examples ---\n")           # \n--- Practical Examples ---\n
    
# User name processing
username_input = "  ALICE123  " # str  — text, always wrapped in quotes
cleaned = username_input.strip().lower() # str  — text, always wrapped in quotes
print("Input: '" + str(username_input) + "'")     # Input: '" + str(username_input) + "'
print("Cleaned: '" + str(cleaned) + "'")          # Cleaned: '" + str(cleaned) + "'
    
# Email validation
email = "user@example.com" # str  — text, always wrapped in quotes
has_at = "@" in email # bool — can only be True or False
has_dot = "." in email[email.index("@"):] # bool — can only be True or False
print("\nEmail: " + str(email))
print("Has @: " + str(has_at))
print("Has . after @: " + str(has_dot))
print("Valid: " + str(has_at and has_dot))
    
# Name formatting
first_name = "john" # str  — text, always wrapped in quotes
last_name = "doe" # str  — text, always wrapped in quotes
full_name = f"{first_name.capitalize()} {last_name.capitalize()}" # str  — text, always wrapped in quotes
print("\nFormatted name: " + str(full_name))
# Summary
print("\n" + "=" * 50)
print("STRING (str) SUMMARY")                     # STRING (str) SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- Strings hold text: 'hello', 'world'")    # - Strings hold text: 'hello', 'world'
print("- Created with \"\", '', or triple quotes")# - Created with \"\", '', or triple quotes
print("- Operations: +, *, len(), indexing, slicing")# - Operations: +, *, len(), indexing, slicing
print("- Methods: .upper(), .lower(), .strip(), .replace(), .split()")# - Methods: .upper(), .lower(), .strip(), .replace(), .split()
print("- F-strings for formatting: f'Hello {name}'")# - F-strings for formatting: "Hello " + str(name)
print("- Strings are immutable (can't change in place)")# - Strings are immutable (can't change in place)

# Real-world example: