# Example5.py
# Topic: Variables Explained - The Walrus Operator (:=)

print("=== Without Walrus Operator ===")          # === Without Walrus Operator ===
    
result = len("hello")  # First assign # int  — whole number, no quotes
if result > 3:              # Then use
    print("Length is " + str(result))
    
name = input("Enter name: ")  # First get input # str  — text, always wrapped in quotes
if name:                              # Then check
    print("Hello, " + str(name))
    
command = "" # str  — text, always wrapped in quotes
while command != "quit":
    command = input("Enter command (or 'quit' to exit): ")
    if command != "quit":
        print("Processing: " + str(command))
# With Walrus Operator
# Assign and use in the same expression
    
print("\n=== With Walrus Operator (:=) ===")      # \n=== With Walrus Operator (:=) ===
    
if (result := len("hello world")) > 5:
    print("Length is " + str(result))
    
print("\nWalrus operator in loop:")               # \nWalrus operator in loop:
print("(Type 'quit' to exit)")                    # (Type 'quit' to exit)
while (command := input("Enter command: ")) != "quit":
    print("Processing: " + str(command))
# Practical Examples
print("\n=== Practical Walrus Examples ===")      # \n=== Practical Walrus Examples ===
    
text = "Hello World" # str  — text, always wrapped in quotes
print("Without walrus - length: " + str(len(text)))
print("Without walrus - length again: " + str(len(text)))
    
# With walrus - calculates once, stores for reuse
if (length := len(text)) > 10:
    print("Text is long (" + str(length) + " characters)")# Text is long (" + str(length) + " characters)
if length < 20:  # Reuse the stored value!
    print("Text is shorter than 20 characters")       # Text is shorter than 20 characters
    
print("\n=== Walrus in List Comprehension ===")   # \n=== Walrus in List Comprehension ===
# Get numbers from user and filter in one line
# Note: This will prompt for input
# numbers: list = [int(n) for n in input("Enter numbers: ").split() if (n := n.strip())]
    
# Simpler example without input
data = ["apple", "banana", "cherry", "date"]
# Filter items that have length > 5 and print the length
filtered = [item for item in data if len(item) > 5]
print("Items with length > 5: " + str(filtered))
    
# Using walrus to store intermediate result
long_items = [item for item in data if (l := len(item)) > 5]
print("Long items (using walrus): " + str(long_items))
    
print("\n=== Walrus in Conditional ===")          # \n=== Walrus in Conditional ===
    
# Without walrus
value = 10 # int  — whole number, no quotes
if value > 5:
    result1 = value * 2 # int  — whole number, no quotes
    print("Result: " + str(result1))
    
# With walrus - assign and check in one expression
if (result2 := 10 * 2) > 15:
    print("Walrus result: " + str(result2))
    
print("\n=== Reusing Expensive Calculation ===")  # \n=== Reusing Expensive Calculation ===
import time
    
# Simulate expensive operation
def expensive_operation() -> int:
    time.sleep(0.1)  # Simulate delay
    return 42
    
# Without walrus - call twice
print("Without walrus:")                          # Without walrus:
result_a = expensive_operation() # int  — whole number, no quotes
print("First call: " + str(result_a))
result_b = expensive_operation() # int  — whole number, no quotes
print("Second call: " + str(result_b))
    
# With walrus - call once, reuse
print("\nWith walrus:")                           # \nWith walrus:
if (exp_result := expensive_operation()) > 40:
    print("First use: " + str(exp_result))
if exp_result < 50:  # Reuse!
    print("Second use (no extra call): " + str(exp_result))

# Real-world example: