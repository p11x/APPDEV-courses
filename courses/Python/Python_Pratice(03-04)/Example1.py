# Example1.py
# Topic: Defining and Calling Functions

# This file demonstrates the basics of defining and calling functions
# in Python. Each function showcases a different use case, from simple
# operations to practical real-world calculations.


# Prints a simple greeting message to the screen with no parameters needed
def greet() -> None:
    print("Hello, World!")

greet()    # Hello, World!


# Takes two integers and returns their sum
def add(a: int, b: int) -> int:
    return a + b

result = add(5, 3)    # int — result of adding 5 and 3
print("5 + 3 = " + str(result))    # 5 + 3 = 8


# Multiplies two floating-point numbers and returns the product
def multiply(x: float, y: float) -> float:
    return x * y

product = multiply(4.5, 2.0)    # float — result of multiplying 4.5 and 2.0
print("4.5 * 2.0 = " + str(product))    # 4.5 * 2.0 = 9.0


# Takes a name parameter and prints a personalized greeting
def say_hello(name: str) -> None:
    print("Hello, " + name + "!")

# Call say_hello() three times with different names
say_hello("Alice")    # Hello, Alice!
say_hello("Bob")      # Hello, Bob!


# Compares two integers and returns the larger value
def get_max(a: int, b: int) -> int:
    if a > b:
        return a
    return b

max_value = get_max(10, 20)    # int — the larger of 10 and 20
print("Max of 10 and 20: " + str(max_value))    # Max of 10 and 20: 20


# Checks whether a given integer is even by dividing by 2 and checking remainder
def is_even(num: int) -> bool:
    return num % 2 == 0

# Test is_even() with different values to see the boolean result
print("Is 10 even? " + str(is_even(10)))    # Is 10 even? True
print("Is 7 even? " + str(is_even(7)))      # Is 7 even? False


# Combines a greeting and name into a full greeting message
def get_greeting(greeting: str, name: str) -> str:
    return greeting + ", " + name + "!"

message = get_greeting("Hi", "Charlie")    # str — complete greeting message
print(message)                              # Hi, Charlie!


# Calculates the area of a rectangle using length and width
def calculate_area(length: float, width: float) -> float:
    return length * width

area = calculate_area(5.0, 3.0)    # float — area of rectangle in square units
print("Area: " + str(area))          # Area: 15.0


# Prints a person's name and age as a sentence
def describe_person(name: str, age: int) -> None:
    print(name + " is " + str(age) + " years old")

describe_person("Diana", 28)    # Diana is 28 years old


# Joins first and last name with a space to create a full name
def get_full_name(first_name: str, last_name: str) -> str:
    return first_name + " " + last_name

full_name = get_full_name("John", "Doe")    # str — combined first and last name
print("Full name: " + full_name)            # Full name: John Doe


# Adds tax to a price and returns the total amount
def calculate_total(price: float, tax: float) -> float:
    return price + (price * tax)

total = calculate_total(100.0, 0.1)    # float — total price after adding 10% tax
print("Total with tax: $" + str(total))    # Total with tax: $110.0


# Squares a number by multiplying it by itself
def get_square(n: int) -> int:
    return n * n

print("5 squared: " + str(get_square(5)))    # 5 squared: 25


# Cubes a number by multiplying it three times
def get_cube(n: int) -> int:
    return n * n * n

print("3 cubed: " + str(get_cube(3)))    # 3 cubed: 27


# Calculates the average of two numbers by adding them and dividing by 2
def get_average(a: float, b: float) -> float:
    return (a + b) / 2

avg = get_average(10.0, 20.0)    # float — average of 10.0 and 20.0
print("Average: " + str(avg))    # Average: 15.0


# Returns the absolute value of a number (positive version)
def get_absolute_value(n: int) -> int:
    if n < 0:
        return -n
    return n

# Test with negative and positive inputs to see absolute value conversion
print("Absolute value of -15: " + str(get_absolute_value(-15)))    # Absolute value of -15: 15
print("Absolute value of 15: " + str(get_absolute_value(15)))      # Absolute value of 15: 15


# Checks whether a number is greater than zero
def check_positive(n: int) -> bool:
    return n > 0

# Test with positive and negative numbers
print("Is 5 positive? " + str(check_positive(5)))     # Is 5 positive? True
print("Is -5 positive? " + str(check_positive(-5)))   # Is -5 positive? False


# Returns a status string based on whether the user is active or not
def get_status(is_active: bool) -> str:
    if is_active:
        return "Active"
    return "Inactive"

# Test with both boolean values to see different status outputs
print("Status: " + get_status(True))    # Status: Active
print("Status: " + get_status(False))    # Status: Inactive


# Converts temperature from Fahrenheit to Celsius using the standard formula
def convert_to_celsius(fahrenheit: float) -> float:
    return (fahrenheit - 32) * 5 / 9

c_temp = convert_to_celsius(212.0)    # float — Celsius equivalent of 212F
print("212F in Celsius: " + str(c_temp))    # 212F in Celsius: 100.0


# Converts temperature from Celsius to Fahrenheit using the standard formula
def convert_to_fahrenheit(celsius: float) -> float:
    return (celsius * 9 / 5) + 32

f_temp = convert_to_fahrenheit(100.0)    # float — Fahrenheit equivalent of 100C
print("100C in Fahrenheit: " + str(f_temp))    # 100C in Fahrenheit: 212.0


# Applies a discount percentage to a price and returns the reduced amount
def calculate_discount(price: float, discount_percent: float) -> float:
    discount = price * discount_percent    # float — amount to subtract from price
    return price - discount

final_price = calculate_discount(100.0, 0.2)    # float — price after 20% discount
print("Price after 20% discount: $" + str(final_price))    # Price after 20% discount: $80.0


# Calculates the area of a circle using radius and the constant pi
def get_circle_area(radius: float) -> float:
    pi = 3.14159    # float — approximation of the mathematical constant π
    return pi * radius * radius

area = get_circle_area(5.0)    # float — area of circle with radius 5
print("Circle area with radius 5: " + str(area))    # Circle area with radius 5: 78.53975


# Calculates the perimeter of a rectangle using length and width
def get_rectangle_perimeter(length: float, width: float) -> float:
    return 2 * (length + width)

perimeter = get_rectangle_perimeter(10.0, 5.0)    # float — perimeter of rectangle
print("Rectangle perimeter: " + str(perimeter))    # Rectangle perimeter: 30.0
