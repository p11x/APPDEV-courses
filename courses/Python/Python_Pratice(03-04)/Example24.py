# Example24.py
# Topic: Return Values - Basic Return Statements

# This file demonstrates the basics of returning values from functions
# in Python. Each function showcases different types of return values.


# Returns a greeting message with the provided name
def greet(name: str) -> str:
    return "Hello, " + name + "!"

message = greet("Alice")    # str — greeting message with name
print(message)              # Hello, Alice!


# Calculates the sum of two numbers and returns the result
def add(a: int, b: int) -> int:
    return a + b

result = add(10, 5)    # int — sum of 10 and 5
print("10 + 5 = " + str(result))    # 10 + 5 = 15


# Returns the larger of two integers
def find_max(a: int, b: int) -> int:
    if a > b:
        return a
    return b

max_value = find_max(25, 17)    # int — larger of 25 and 17
print("Max of 25 and 17: " + str(max_value))    # Max of 25 and 17: 25


# Calculates the area of a rectangle given length and width
def get_area(length: float, width: float) -> float:
    return length * width

area = get_area(8.0, 5.0)    # float — area of rectangle
print("Area: " + str(area))    # Area: 40.0


# Returns True if a number is even, False otherwise
def is_even(num: int) -> bool:
    return num % 2 == 0

print("Is 8 even? " + str(is_even(8)))    # Is 8 even? True
print("Is 7 even? " + str(is_even(7)))    # Is 7 even? False


# Returns the square of a number
def square(n: int) -> int:
    return n * n

print("5 squared: " + str(square(5)))    # 5 squared: 25


# Returns the absolute value of a number
def absolute_value(n: int) -> int:
    if n < 0:
        return -n
    return n

print("Absolute value of -10: " + str(absolute_value(-10)))    # Absolute value of -10: 10
print("Absolute value of 10: " + str(absolute_value(10)))      # Absolute value of 10: 10


from typing import Optional

# Returns the first element of a list or None if empty
def get_first(items: list) -> Optional[str]:
    if len(items) > 0:
        return items[0]
    return None

fruits = ["apple", "banana", "cherry"]
print("First fruit: " + str(get_first(fruits)))    # First fruit: apple
print("First from empty: " + str(get_first([])))    # First from empty: None


# Returns a formatted full name from first and last name
def create_full_name(first: str, last: str) -> str:
    return first + " " + last

full_name = create_full_name("John", "Doe")    # str — full name
print("Full name: " + full_name)                # Full name: John Doe


# Calculates the circumference of a circle given the radius
def get_circumference(radius: float) -> float:
    pi = 3.14159
    return 2 * pi * radius

circumference = get_circumference(7.0)    # float — circumference of circle
print("Circumference: " + str(circumference))    # Circumference: 43.98226


# Returns the price after applying a discount
def apply_discount(price: float, discount: float) -> float:
    discounted_price = price * (1 - discount)
    return discounted_price

final_price = apply_discount(100.0, 0.2)    # float — price after 20% discount
print("Price after discount: $" + str(final_price))    # Price after discount: $80.0


# Returns the factorial of a non-negative integer
def factorial(n: int) -> int:
    if n <= 1:
        return 1
    return n * factorial(n - 1)

print("5! = " + str(factorial(5)))    # 5! = 120


# Returns the length of a string
def get_length(text: str) -> int:
    return len(text)

print("Length of 'Python': " + str(get_length("Python")))    # Length of 'Python': 6


# Returns True if a string contains a specific substring
def contains_substring(text: str, substring: str) -> bool:
    return substring in text

print("'Hello' contains 'll': " + str(contains_substring("Hello", "ll")))    # True
print("'Hello' contains 'world': " + str(contains_substring("Hello", "world")))    # False


# Real-life Example 1: Calculate monthly loan payment
def calculate_monthly_payment(principal: float, annual_rate: float, years: int) -> float:
    monthly_rate = annual_rate / 12 / 100
    num_payments = years * 12
    if monthly_rate == 0:
        return principal / num_payments
    payment = principal * (monthly_rate * (1 + monthly_rate) ** num_payments) / ((1 + monthly_rate) ** num_payments - 1)
    return payment

monthly = calculate_monthly_payment(250000, 6.5, 30)
print("Monthly payment: $" + str(round(monthly, 2)))    # Monthly payment: $1580.17


# Real-life Example 2: Calculate BMI (Body Mass Index)
def calculate_bmi(weight_kg: float, height_m: float) -> float:
    return weight_kg / (height_m ** 2)

bmi = calculate_bmi(70, 1.75)
print("BMI: " + str(round(bmi, 1)))    # BMI: 22.9


# Real-life Example 3: Calculate distance between two GPS coordinates
import math

def calculate_distance(lat1: float, lon1: float, lat2: float, lon2: float) -> float:
    R = 6371  # Earth's radius in kilometers
    lat1_rad = math.radians(lat1)
    lat2_rad = math.radians(lat2)
    delta_lat = math.radians(lat2 - lat1)
    delta_lon = math.radians(lon2 - lon1)
    
    a = math.sin(delta_lat / 2) ** 2 + math.cos(lat1_rad) * math.cos(lat2_rad) * math.sin(delta_lon / 2) ** 2
    c = 2 * math.atan2(math.sqrt(a), math.sqrt(1 - a))
    return R * c

distance = calculate_distance(40.7128, -74.0060, 34.0522, -118.2437)
print("Distance NYC to LA: " + str(round(distance, 0)) + " km")    # Distance NYC to LA: 3944.0 km


# Real-life Example 4: Calculate tax refund or amount owed
def calculate_tax_refund(income: float, tax_paid: float) -> float:
    if income <= 10000:
        tax_due = income * 0.10
    elif income <= 40000:
        tax_due = 1000 + (income - 10000) * 0.12
    elif income <= 85000:
        tax_due = 4600 + (income - 40000) * 0.22
    else:
        tax_due = 14500 + (income - 85000) * 0.24
    
    return tax_paid - tax_due

refund = calculate_tax_refund(50000, 6000)
if refund > 0:
    print("Tax refund: $" + str(round(refund, 2)))
else:
    print("Tax owed: $" + str(round(-refund, 2)))
