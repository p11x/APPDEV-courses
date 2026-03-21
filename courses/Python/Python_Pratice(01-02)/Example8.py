# Example8.py
# Topic: Data Types - Float (float)

# Float (float) — numbers with decimal points
# Floats are used for measurements, prices, and any number that needs precision

# Creating floats
price = 19.99            # float — positive decimal
temperature = -5.5        # float — negative decimal
pi = 3.14159            # float — mathematical constant
zero = 0.0               # float — zero with decimal

# Print float values
print(price)             # 19.99
print(temperature)       # -5.5
print(pi)                # 3.14159
print(zero)              # 0.0

# Float operations
a = 10.5
b = 2.0

# Addition
result = a + b
print("10.5 + 2.0 = " + str(result))  # 12.5

# Subtraction
result = a - b
print("10.5 - 2.0 = " + str(result))  # 8.5

# Multiplication
result = a * b
print("10.5 * 2.0 = " + str(result))  # 21.0

# Division
result = a / b
print("10.5 / 2.0 = " + str(result))  # 5.25

# Using floats in real scenarios
# Shopping with prices
item_price = 29.99
quantity = 3
subtotal = item_price * quantity
tax_rate = 0.08
tax = subtotal * tax_rate
total = subtotal + tax

print("Subtotal: $" + str(subtotal))  # $89.97
print("Tax: $" + str(tax))            # $7.19...
print("Total: $" + str(total))        # $97.16...

# Temperature conversion (Fahrenheit to Celsius)
fahrenheit = 98.6
celsius = (fahrenheit - 32) * 5/9
print(str(fahrenheit) + "F = " + str(celsius) + "C")  # 37.0C

# Circle calculations
radius = 5.0
area = pi * radius ** 2
circumference = 2 * pi * radius

print("Radius: " + str(radius))                    # 5.0
print("Area: " + str(area))                        # 78.53...
print("Circumference: " + str(circumference))     # 31.41...

# Float precision note
# Floats can have precision issues
result = 0.1 + 0.2
print("0.1 + 0.2 = " + str(result))  # 0.30000000000000004

# For precision, use the decimal module
# from decimal import Decimal
# result = Decimal('0.1') + Decimal('0.2')

# Real-world example: tracking running pace
distance_km = 10.0       # kilometers
time_hours = 0.8333     # approximately 50 minutes
pace = distance_km / time_hours  # km per hour

print("Distance: " + str(distance_km) + " km")     # 10.0 km
print("Time: " + str(time_hours) + " hours")      # 0.8333 hours
print("Average speed: " + str(pace) + " km/h")     # 12.0 km/h

# BMI calculation
weight_kg = 70.0
height_m = 1.75
bmi = weight_kg / (height_m ** 2)

print("Weight: " + str(weight_kg) + " kg")  # 70.0 kg
print("Height: " + str(height_m) + " m")    # 1.75 m
print("BMI: " + str(bmi))                   # 22.85...
