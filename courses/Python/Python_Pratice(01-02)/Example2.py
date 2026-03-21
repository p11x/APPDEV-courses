# Example2.py
# Topic: Variables Explained - Creating, Reading, and Changing Variables

name = "Alice" # str  — text, always wrapped in quotes
    
# Create a variable called 'age' with the value 25
age = 25 # int  — whole number, no quotes
    
# Create a variable called 'height' with the value 5.6
height = 5.6 # float — decimal number
    
# Create a variable called 'is_student' with the value True
is_student = True # bool — can only be True or False
    
# Print all variables
print("=== Created Variables ===")                # === Created Variables ===
print("Name: " + str(name))
print("Age: " + str(age))
print("Height: " + str(height))
print("Is Student: " + str(is_student))
# Reading Variables
# Print the value stored in 'name'
print("\n=== Reading Variables ===")              # \n=== Reading Variables ===
print(name)  # Output: Alice
    
# Use in calculations
next_year_age = age + 1  # 26 # int  — whole number, no quotes
print("Next year, " + str(name) + " will be " + str(next_year_age) + " years old")# Next year, " + str(name) + " will be " + str(next_year_age) + " years old
    
# Double the height
double_height = height * 2 # float — decimal number
print("Double height: " + str(double_height))
# Changing Variables
print("\n=== Changing Variables ===")             # \n=== Changing Variables ===
age = 25       # Initially 25 # int  — whole number, no quotes
print("Initial age: " + str(age))
    
age = 26       # Now 26 - we can change the value # int  — whole number, no quotes
print("Updated age: " + str(age))
    
# Reassign to different type (Python allows this)
age = "twenty-seven"  # Now it's a string! # str  — text, always wrapped in quotes
print("Age as string: " + str(age))
# Practical Examples
print("\n=== Practical Examples ===")             # \n=== Practical Examples ===
    
# Shopping cart example
item_name = "Laptop" # str  — text, always wrapped in quotes
item_price = 999.99 # float — decimal number
quantity = 2 # int  — whole number, no quotes
in_stock = True # bool — can only be True or False
    
total = item_price * quantity # float — decimal number
print("Item: " + str(item_name))
print("Price: $" + str(item_price))
print("Quantity: " + str(quantity))
print("Total: $" + str(total))
print("In Stock: " + str(in_stock))

# Real-world example: