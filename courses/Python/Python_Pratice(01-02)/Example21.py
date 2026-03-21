# Example21.py
# Topic: python-guide/01_Foundations/03_Operators/01_arithmetic_operators.md

dividend = 10 # int  — whole number, no quotes
divisor = 2 # int  — whole number, no quotes
result_div = dividend / divisor # float — decimal number
print("Division: " + str(dividend) + " / " + str(divisor) + " = " + str(result_div) + " (type: " + str(type(result_div).__name__) + ")")# Division: " + str(dividend) + " / " + str(divisor) + " = " + str(result_div) + " (type: " + str(type(result_div).__name__) + ")# Division: " + str(dividend) + " / " + str(divisor) + " = " + str(result_div) + " (type: " + str(type(result_div).__name__) + ")")# Division: " + str(dividend) + " / " + str(divisor) + " = " + str(result_div) + " (type: " + str(type(result_div).__name__) + 
    
# Division that results in decimal
result_decimal = 7 / 3 # float — decimal number
print("Division: 7 / 3 = " + str(result_decimal))
    
# Another example
result_float = 10 / 4 # float — decimal number
print("Division: 10 / 4 = " + str(result_float))
    
# Float division
result_float_div = 5.5 / 2 # float — decimal number
print("Float division: 5.5 / 2 = " + str(result_float_div))
    
print()  # Empty line for spacing
# = Floor Division (//) =
# Divides and rounds DOWN to nearest integer
    
# Basic floor division
result_floor = 10 // 3 # int  — whole number, no quotes
print("Floor division: 10 // 3 = " + str(result_floor) + " (rounds down)")# Floor division: 10 // 3 = " + str(result_floor) + " (rounds down)
    
# Another example
result_floor2 = 7 // 2 # int  — whole number, no quotes
print("Floor division: 7 // 2 = " + str(result_floor2))
    
# IMPORTANT: Negative floor division rounds toward negative infinity!
result_neg_floor = -7 // 2 # int  — whole number, no quotes
print("Negative floor division: -7 // 2 = " + str(result_neg_floor) + " (rounds toward -infinity)")# Negative floor division: -7 // 2 = " + str(result_neg_floor) + " (rounds toward -infinity)
    
# Float floor division - returns float
result_float_floor = 5.5 // 2 # float — decimal number
print("Float floor division: 5.5 // 2 = " + str(result_float_floor))
    
print()  # Empty line for spacing
# = Comparison: / Vs // =
    
# Division (/) always returns float
result_regular = 7 / 2 # float — decimal number
print("Regular division: 7 / 2 = " + str(result_regular) + " (float)")# Regular division: 7 / 2 = " + str(result_regular) + " (float)
    
# Floor division (//) returns int (for integers)
result_floor_div = 7 // 2 # int  — whole number, no quotes
print("Floor division: 7 // 2 = " + str(result_floor_div) + " (int)")# Floor division: 7 // 2 = " + str(result_floor_div) + " (int)
    
print()  # Empty line for spacing
# = Practical Examples =
    
# Split bill evenly (use / for exact amount)
total_bill = 100.0 # float — decimal number
num_people = 3 # int  — whole number, no quotes
per_person = total_bill / num_people # float — decimal number
print("Split bill: $" + str(total_bill) + " / " + str(num_people) + " = $" + str(per_person) + " each")# Split bill: $" + str(total_bill) + " / " + str(num_people) + " = $" + str(per_person) + " each
    
# Get whole groups (use // for number of full groups)
total_items = 17 # int  — whole number, no quotes
items_per_group = 5 # int  — whole number, no quotes
full_groups = total_items // items_per_group # int  — whole number, no quotes
remaining = total_items % items_per_group # int  — whole number, no quotes
print("Items: " + str(total_items) + ", per group: " + str(items_per_group))
print("Full groups: " + str(full_groups) + ", remaining items: " + str(remaining))
    
# Calculate pages needed
total_lines = 100 # int  — whole number, no quotes
lines_per_page = 25 # int  — whole number, no quotes
total_pages = (total_lines + lines_per_page - 1) // lines_per_page  # Ceiling division # int  — whole number, no quotes
print("Total lines: " + str(total_lines) + ", lines per page: " + str(lines_per_page))
print("Total pages needed: " + str(total_pages))

# Real-world example: