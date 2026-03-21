# Example3.py
# Topic: Return Values

# This file demonstrates the importance of using return statements
# in functions. Each function shows how to return different types of
# values and use them in calculations.


# Takes two integers and returns their sum
def add(a: int, b: int) -> int:
    return a + b

result = add(5, 3)    # int — result of adding 5 and 3
print("5 + 3 = " + str(result))    # 5 + 3 = 8


# Squares a number by multiplying it by itself and returns the result
def get_square(n: int) -> int:
    return n * n

print("5 squared = " + str(get_square(5)))    # 5 squared = 25


# Compares two integers and returns the larger one
def get_maximum(a: int, b: int) -> int:
    if a > b:
        return a
    return b

print("Max: " + str(get_maximum(10, 20)))    # Max: 20


# Returns the length (character count) of a string
def find_length(text: str) -> int:
    return len(text)

print("Length of 'Hello': " + str(find_length("Hello")))    # Length of 'Hello': 5


# Sums up all prices in a list and returns the total
def calculate_total(prices: list) -> float:
    total = 0.0    # float — running total of prices
    for price in prices:
        total = total + price
    return total

cart_total = calculate_total([10.0, 20.0, 30.0])    # float — sum of all prices
print("Cart total: $" + str(cart_total))          # Cart total: $60.0


# Calculates the average of numbers in a list by dividing sum by count
def get_average(numbers: list) -> float:
    total = sum(numbers)    # float — sum of all numbers in the list
    return total / len(numbers)

avg = get_average([10, 20, 30, 40])    # float — average value
print("Average: " + str(avg))          # Average: 25.0


# Combines name and age into a descriptive sentence
def get_user_info(name: str, age: int) -> str:
    return name + " is " + str(age) + " years old"

info = get_user_info("Alice", 25)    # str — user information sentence
print(info)                           # Alice is 25 years old


# Iterates through a list to find and return the smallest number
def find_smallest(numbers: list) -> int:
    smallest = numbers[0]    # int — initialise with first element
    for num in numbers:
        if num < smallest:
            smallest = num
    return smallest

min_value = find_smallest([5, 2, 8, 1, 9])    # int — smallest number in list
print("Smallest: " + str(min_value))          # Smallest: 1


# Iterates through a list to find and return the largest number
def find_largest(numbers: list) -> int:
    largest = numbers[0]    # int — initialise with first element
    for num in numbers:
        if num > largest:
            largest = num
    return largest

max_value = find_largest([5, 2, 8, 1, 9])    # int — largest number in list
print("Largest: " + str(max_value))           # Largest: 9


# Counts how many even numbers are in a list and returns the count
def count_even(numbers: list) -> int:
    count = 0    # int — counter for even numbers
    for num in numbers:
        if num % 2 == 0:
            count = count + 1
    return count

even_count = count_even([1, 2, 3, 4, 5, 6])    # int — count of even numbers
print("Even count: " + str(even_count))        # Even count: 3


# Returns the absolute (positive) value of a number
def get_absolute(n: int) -> int:
    if n < 0:
        return -n
    return n

print("|-15| = " + str(get_absolute(-15)))    # |-15| = 15


# Calculates tax amount by multiplying amount by tax rate
def calculate_tax(amount: float, tax_rate: float) -> float:
    return amount * tax_rate

tax = calculate_tax(100.0, 0.1)    # float — calculated tax amount
print("Tax: $" + str(tax))        # Tax: $10.0


# Converts a name to lowercase and combines with a dot for username
def create_username(first: str, last: str) -> str:
    return first.lower() + "." + last.lower()

username = create_username("John", "Doe")    # str — formatted username
print("Username: " + username)                # Username: john.doe


# Returns a human-readable status message for HTTP response codes
def get_status_code(code: int) -> str:
    if code == 200:
        return "OK"
    elif code == 404:
        return "Not Found"
    elif code == 500:
        return "Internal Server Error"
    return "Unknown"

status = get_status_code(404)    # str — status message for code 404
print("Status: " + status)      # Status: Not Found


# Applies a discount percentage to a price and returns the reduced price
def calculate_discount(price: float, discount_percent: float) -> float:
    discount_amount = price * discount_percent    # float — amount being discounted
    return price - discount_amount

final_price = calculate_discount(100.0, 0.2)    # float — price after 20% discount
print("Price after discount: $" + str(final_price))    # Price after discount: $80.0


# Calculates what percentage one value is of a total
def get_percentage(value: float, total: float) -> float:
    return (value / total) * 100

percentage = get_percentage(25, 100)    # float — percentage value
print("Percentage: " + str(percentage) + "%")    # Percentage: 25.0%


# Extracts only digits from a phone number and formats it as (XXX) XXX-XXXX
def format_phone(phone: str) -> str:
    cleaned = ""    # str — accumulator for digits only
    for char in phone:
        if char.isdigit():
            cleaned = cleaned + char
    return "(" + cleaned[0:3] + ") " + cleaned[3:6] + "-" + cleaned[6:]

formatted = format_phone("123-456-7890")    # str — formatted phone number
print("Formatted: " + formatted)             # Formatted: (123) 456-7890


# Calculates bonus based on years of service (5+ years = 10%, 3+ years = 5%)
def calculate_bonus(salary: float, years: int) -> float:
    if years >= 5:
        return salary * 0.1    # 10% bonus
    elif years >= 3:
        return salary * 0.05   # 5% bonus
    return 0.0                   # no bonus for less than 3 years

bonus = calculate_bonus(50000.0, 6)    # float — calculated bonus amount
print("Bonus: $" + str(bonus))        # Bonus: $5000.0


# Returns appropriate greeting based on the hour of day (24-hour format)
def get_greeting(time_hour: int) -> str:
    if time_hour < 12:
        return "Good Morning"
    elif time_hour < 17:
        return "Good Afternoon"
    return "Good Evening"

greeting = get_greeting(14)    # str — greeting based on 2 PM
print(greeting)                 # Good Afternoon


# Calculates miles per gallon by dividing miles driven by gallons used
def calculate_mpg(miles: float, gallons: float) -> float:
    return miles / gallons

mpg = calculate_mpg(350.0, 12.0)    # float — miles per gallon
print("MPG: " + str(mpg))          # MPG: 29.166666666666668


# Extracts and returns the domain portion from an email address
def get_domain(email: str) -> str:
    at_index = email.index("@")    # int — position of the @ symbol
    return email[at_index + 1:]

domain = get_domain("user@example.com")    # str — domain part of email
print("Domain: " + domain)                  # Domain: example.com


# Removes leading/trailing whitespace and converts to lowercase
def normalize_text(text: str) -> str:
    return text.strip().lower()

normalized = normalize_text("  Hello World  ")    # str — cleaned text
print("Normalized: '" + normalized + "'")       # Normalized: 'hello world'


# Adds tax to a subtotal and returns the grand total
def calculate_total_with_tax(subtotal: float, tax_rate: float) -> float:
    tax = subtotal * tax_rate    # float — tax amount to add
    return subtotal + tax

total = calculate_total_with_tax(100.0, 0.08)    # float — total including tax
print("Total with tax: $" + str(total))         # Total with tax: $108.0


# Classifies age into categories: Child (under 13), Teen (13-19), Adult (20+)
def get_age_category(age: int) -> str:
    if age < 13:
        return "Child"
    elif age < 20:
        return "Teen"
    return "Adult"

category = get_age_category(25)    # str — age category
print("Category: " + category)    # Category: Adult


# Calculates simple interest using the formula: principal × rate × time
def calculate_simple_interest(principal: float, rate: float, time: float) -> float:
    interest = principal * rate * time    # float — interest amount earned
    return interest

interest = calculate_simple_interest(1000.0, 0.05, 2)    # float — total interest
print("Interest: $" + str(interest))                    # Interest: $100.0
