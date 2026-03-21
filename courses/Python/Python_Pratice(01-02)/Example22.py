# Example22.py
# Topic: python-guide/01_Foundations/03_Operators/01_arithmetic_operators.md

result_mod1 = 10 % 3 # int  — whole number, no quotes
print("Modulo: 10 % 3 = " + str(result_mod1))  # 10 = 3*3 + 1, remainder is 1
    
# Another example
result_mod2 = 7 % 2 # int  — whole number, no quotes
print("Modulo: 7 % 2 = " + str(result_mod2))  # 7 = 2*3 + 1, remainder is 1
    
# No remainder
result_mod3 = 10 % 5 # int  — whole number, no quotes
print("Modulo: 10 % 5 = " + str(result_mod3))  # 10 = 5*2 + 0, remainder is 0
    
# Even/odd check using modulo
number = 7 # int  — whole number, no quotes
is_odd = number % 2 == 1 # bool — can only be True or False
print("\n" + str(number) + " is odd: " + str(is_odd))
    
number2 = 8 # int  — whole number, no quotes
is_even = number2 % 2 == 0 # bool — can only be True or False
print(str(number2) + " is even: " + str(is_even))
    
print()  # Empty line for spacing
# = Exponentiation (**) =
# Raises to a power
    
# Basic exponentiation
result_exp1 = 2 ** 3 # int  — whole number, no quotes
print("Exponentiation: 2 ** 3 = " + str(result_exp1))  # 2^3 = 8
    
# Square
result_exp2 = 5 ** 2 # int  — whole number, no quotes
print("Square: 5 ** 2 = " + str(result_exp2))  # 5^2 = 25
    
# Cube
result_exp3 = 4 ** 3 # int  — whole number, no quotes
print("Cube: 4 ** 3 = " + str(result_exp3))  # 4^3 = 64
    
# Fractional exponent (square root)
result_sqrt = 2 ** 0.5 # float — decimal number
print("Square root: 2 ** 0.5 = " + str(result_sqrt))  # sqrt(2) ≈ 1.414
    
# Another square root example
result_sqrt2 = 16 ** 0.5 # float — decimal number
print("Square root: 16 ** 0.5 = " + str(result_sqrt2))  # sqrt(16) = 4
    
# Negative exponent (1/x^n)
result_neg_exp = 2 ** -2 # float — decimal number
print("Negative exponent: 2 ** -2 = " + str(result_neg_exp))  # 1/2^2 = 0.25
    
print()  # Empty line for spacing
# = Practical Examples =
    
# Modulo: Check if divisible
num = 15 # int  — whole number, no quotes
is_divisible_by_5 = num % 5 == 0 # bool — can only be True or False
print(str(num) + " is divisible by 5: " + str(is_divisible_by_5))
    
# Modulo: Wrap around (clock-like behavior)
hour = 25 # int  — whole number, no quotes
clock_hour = hour % 12 # int  — whole number, no quotes
if clock_hour == 0:
    clock_hour = 12
print("25-hour format -> 12-hour format: " + str(clock_hour))
    
# Modulo: Extract digits
num2 = 87 # int  — whole number, no quotes
ones_digit = num2 % 10 # int  — whole number, no quotes
tens_digit = num2 // 10 # int  — whole number, no quotes
print("Number: " + str(num2) + ", Tens: " + str(tens_digit) + ", Ones: " + str(ones_digit))
    
# Exponentiation: Compound interest
principal = 1000.0 # float — decimal number
rate = 0.05 # float — decimal number
years = 10 # int  — whole number, no quotes
amount = principal * (1 + rate) ** years # float — decimal number
print("\nCompound interest: $" + str(principal) + " at " + str(rate*100) + "% for " + str(years) + " years = $" + str(amount))
    
# Exponentiation: Calculate volume of sphere
radius = 3.0 # float — decimal number
# Volume = 4/3 * pi * r^3
pi = 3.14159 # float — decimal number
volume = (4/3) * pi * (radius ** 3) # float — decimal number
print("Volume of sphere (r=" + str(radius) + "): " + str(volume))

# Real-world example: