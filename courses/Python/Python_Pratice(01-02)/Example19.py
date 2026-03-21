# Example19.py
# Topic: python-guide/01_Foundations/03_Operators/01_arithmetic_operators.md

a = 5 # int  — whole number, no quotes
b = 3 # int  — whole number, no quotes
result_add_int = a + b # int  — whole number, no quotes
print("Integer addition: " + str(a) + " + " + str(b) + " = " + str(result_add_int))
    
# Float addition
x = 2.5 # float — decimal number
y = 3.1 # float — decimal number
result_add_float = x + y # float — decimal number
print("Float addition: " + str(x) + " + " + str(y) + " = " + str(result_add_float))
    
# Mixed int + float (int gets converted to float)
result_mixed = 5 + 2.5 # float — decimal number
print("Mixed addition: 5 + 2.5 = " + str(result_mixed))
    
# String concatenation (special case!)
first_name = "Hello" # str  — text, always wrapped in quotes
last_name = "World" # str  — text, always wrapped in quotes
result_concat = first_name + " " + last_name # str  — text, always wrapped in quotes
print("String concatenation: '" + str(first_name) + "' + ' ' + '" + str(last_name) + "' = '" + str(result_concat) + "'")# String concatenation: '" + str(first_name) + "' + ' ' + '" + str(last_name) + "' = '" + str(result_concat) + "'
    
print()  # Empty line for spacing
# = Subtraction (-) =
    
# Integer subtraction
result_sub_int = 10 - 4 # int  — whole number, no quotes
print("Integer subtraction: 10 - 4 = " + str(result_sub_int))
    
# Float subtraction
result_sub_float = 5.5 - 2.3 # float — decimal number
print("Float subtraction: 5.5 - 2.3 = " + str(result_sub_float))
    
# Unary minus (negation)
positive = 10 # int  — whole number, no quotes
negative = -positive # int  — whole number, no quotes
print("Unary minus: -" + str(positive) + " = " + str(negative))
    
# Subtraction with negative result
result_negative = -5 + 10 # int  — whole number, no quotes
print("Negative number: -5 + 10 = " + str(result_negative))
    
# Using constants (Final)
PI: Final[float] = 3.14159
radius = 5.0 # float — decimal number
circumference = 2 * PI * radius # float — decimal number
print("\nCircumference of circle (r=" + str(radius) + "): 2 * PI * r = " + str(circumference))

# Real-world example: