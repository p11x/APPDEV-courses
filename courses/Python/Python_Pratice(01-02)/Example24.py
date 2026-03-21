# Example24.py
# Topic: python-guide/01_Foundations/03_Operators/01_arithmetic_operators.md

print("=== Rectangle Calculator ===\n")           # === Rectangle Calculator ===\n
    
# Define rectangle dimensions (using float for precision)
width = 5.0 # float — decimal number
height = 3.0 # float — decimal number
    
# Calculate area: width × height (multiplication)
area = width * height # float — decimal number
print("Rectangle dimensions: " + str(width) + " x " + str(height))
print("Area (width * height): " + str(area))
    
# Calculate perimeter: 2 × (width + height)
# Parentheses ensure addition before multiplication
perimeter = 2 * (width + height) # float — decimal number
print("Perimeter (2 * (width + height)): " + str(perimeter))
    
# Calculate diagonal using Pythagorean theorem
# diagonal = √(width² + height²)
# = (width**2 + height**2)**0.5
diagonal = (width ** 2 + height ** 2) ** 0.5 # float — decimal number
print("Diagonal (√(width² + height²)): " + str(diagonal))
    
print()  # Empty line
# = Integer Dimensions =
    
# Using integer dimensions
width_int = 10 # int  — whole number, no quotes
height_int = 4 # int  — whole number, no quotes
    
area_int = width_int * height_int # int  — whole number, no quotes
perimeter_int = 2 * (width_int + height_int) # int  — whole number, no quotes
    
print("Integer rectangle: " + str(width_int) + " x " + str(height_int))
print("Area: " + str(area_int))
print("Perimeter: " + str(perimeter_int))
    
print()  # Empty line
# = Practical: Floor Plan Calculator =
    
print("=== Floor Plan Calculator ===\n")          # === Floor Plan Calculator ===\n
    
# Room dimensions in feet
room_width = 12.5 # float — decimal number
room_length = 14.0 # float — decimal number
    
# Calculate area in square feet
room_area = room_length * room_width # float — decimal number
print("Room: " + str(room_length) + "' x " + str(room_width) + "'")# Room: " + str(room_length) + "' x " + str(room_width) + "'
print("Floor area: " + str(room_area) + " sq ft") # Floor area: " + str(room_area) + " sq ft
    
# Convert to square meters (1 sq ft = 0.092903 sq m)
sq_ft_to_sq_m = 0.092903 # float — decimal number
room_area_m2 = room_area * sq_ft_to_sq_m # float — decimal number
print("Floor area: " + str(room_area_m2) + " sq m")# Floor area: " + str(room_area_m2) + " sq m
    
print()  # Empty line
# = Practical: Paint Calculator =
    
print("=== Paint Calculator ===\n")               # === Paint Calculator ===\n
    
# Wall dimensions (4 walls)
wall_width = 10.0 # float — decimal number
wall_height = 8.0 # float — decimal number
num_walls = 4 # int  — whole number, no quotes
    
# Total wall area
wall_area = wall_width * wall_height * num_walls # float — decimal number
print("Walls: " + str(num_walls) + " walls, " + str(wall_width) + "' x " + str(wall_height) + "' each")# Walls: " + str(num_walls) + " walls, " + str(wall_width) + "' x " + str(wall_height) + "' each
print("Total wall area: " + str(wall_area) + " sq ft")# Total wall area: " + str(wall_area) + " sq ft
    
# Subtract windows and doors
window_area = 3.0 * 4.0  # 2 windows # float — decimal number
door_area = 3.0 * 7.0    # 1 door # float — decimal number
num_windows = 2 # int  — whole number, no quotes
num_doors = 1 # int  — whole number, no quotes
    
total_window_area = window_area * num_windows # float — decimal number
total_door_area = door_area * num_doors # float — decimal number
    
# Paintable area
paintable_area = wall_area - total_window_area - total_door_area # float — decimal number
print("Window area (" + str(num_windows) + "x): " + str(total_window_area) + " sq ft")# Window area (" + str(num_windows) + "x): " + str(total_window_area) + " sq ft
print("Door area (" + str(num_doors) + "x): " + str(total_door_area) + " sq ft")# Door area (" + str(num_doors) + "x): " + str(total_door_area) + " sq ft
print("Paintable area: " + str(paintable_area) + " sq ft")# Paintable area: " + str(paintable_area) + " sq ft
    
# Calculate paint needed (1 gallon covers ~350 sq ft)
coverage_per_gallon = 350.0 # float — decimal number
gallons_needed = paintable_area / coverage_per_gallon # float — decimal number
gallons_ceiling = int(paintable_area // coverage_per_gallon) + 1 # int  — whole number, no quotes
    
print("\nPaint coverage: " + str(coverage_per_gallon) + " sq ft/gallon")# \nPaint coverage: " + str(coverage_per_gallon) + " sq ft/gallon
print("Exact gallons needed: " + str(gallons_needed))
print("Rounded up (buy): " + str(gallons_ceiling) + " gallons")# Rounded up (buy): " + str(gallons_ceiling) + " gallons
    
print()  # Empty line
# = Summary Of Operators Used =
    
print("=== Operators Used in This Program ===")   # === Operators Used in This Program ===
print("+  Addition: combining dimensions")        # +  Addition: combining dimensions
print("-  Subtraction: subtracting windows/doors from wall area")# -  Subtraction: subtracting windows/doors from wall area
print("*  Multiplication: calculating area (width * height)")# *  Multiplication: calculating area (width * height)
print("/  Division: converting sq ft to sq m, paint coverage")# /  Division: converting sq ft to sq m, paint coverage
print("** Exponentiation: calculating diagonal (width**2 + height**2)")# ** Exponentiation: calculating diagonal (width**2 + height**2)
print("** Exponentiation: square root (number ** 0.5)")# ** Exponentiation: square root (number ** 0.5)

# Real-world example: