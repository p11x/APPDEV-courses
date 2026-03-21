# Example20.py
# Topic: python-guide/01_Foundations/03_Operators/01_arithmetic_operators.md

a = 6 # int  — whole number, no quotes
b = 7 # int  — whole number, no quotes
result_mul_int = a * b # int  — whole number, no quotes
print("Integer multiplication: " + str(a) + " * " + str(b) + " = " + str(result_mul_int))
    
# Float multiplication
price = 3.14 # float — decimal number
quantity = 2.5 # float — decimal number
result_mul_float = price * quantity # float — decimal number
print("Float multiplication: " + str(price) + " * " + str(quantity) + " = " + str(result_mul_float))
    
# Mixed int * float
result_mixed = 5 * 2.5 # float — decimal number
print("Mixed multiplication: 5 * 2.5 = " + str(result_mixed))
    
print()  # Empty line for spacing
# = String Repetition (Special Case!) =
    
# Multiply string by integer to repeat it
text = "Ha" # str  — text, always wrapped in quotes
times = 3 # int  — whole number, no quotes
result_repeat = text * times # str  — text, always wrapped in quotes
print("String repetition: '" + str(text) + "' * " + str(times) + " = '" + str(result_repeat) + "'")# String repetition: '" + str(text) + "' * " + str(times) + " = '" + str(result_repeat) + "'
    
# Another example
separator = "-" # str  — text, always wrapped in quotes
result_separator = separator * 10 # str  — text, always wrapped in quotes
print("String repetition (separator): '" + str(separator) + "' * 10 = '" + str(result_separator) + "'")# String repetition (separator): '" + str(separator) + "' * 10 = '" + str(result_separator) + "'
    
# Space repetition for indentation
indent = " " * 4 # str  — text, always wrapped in quotes
print("\n" + str(indent) + "Indented text (4 spaces)")# \n" + str(indent) + "Indented text (4 spaces)
    
print()  # Empty line for spacing
# = Practical Examples =
    
# Calculate area of a rectangle
length = 10.5 # float — decimal number
width = 4.2 # float — decimal number
area = length * width # float — decimal number
print("Rectangle area: " + str(length) + " * " + str(width) + " = " + str(area))
    
# Calculate total cost
unit_price = 19.99 # float — decimal number
quantity = 5 # int  — whole number, no quotes
total_cost = unit_price * quantity # float — decimal number
print("Total cost: $" + str(unit_price) + " * " + str(quantity) + " = $" + str(total_cost))
    
# Multiple numbers
result_multiple = 2 * 3 * 4 # int  — whole number, no quotes
print("Multiple: 2 * 3 * 4 = " + str(result_multiple))

# Real-world example: