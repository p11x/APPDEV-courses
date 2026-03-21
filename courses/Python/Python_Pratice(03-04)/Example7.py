# Example7.py
# Topic: Function Practice - Mixed Examples

from typing import Optional

# This file provides practice with various function concepts including
# calculator operations, string manipulation, recursion, and real-world
# applications like email validation and temperature conversion.


# Returns the sum of two integers
def add(a: int, b: int) -> int:
    return a + b


# Returns the difference of two integers
def subtract(a: int, b: int) -> int:
    return a - b


# Returns the product of two integers
def multiply(a: int, b: int) -> int:
    return a * b


# Divides two numbers, handling division by zero safely
def divide(a: float, b: float) -> Optional[float]:
    if b == 0:
        print("Cannot divide by zero!")
        return None
    return a / b


# Demonstrates the four basic calculator operations
print("=== Basic Calculator ===")
print("Add: " + str(add(10, 5)))         # Add: 15
print("Subtract: " + str(subtract(10, 5)))    # Subtract: 5
print("Multiply: " + str(multiply(10, 5)))    # Multiply: 50
print("Divide: " + str(divide(10, 5)))      # Divide: 2.0


# Checks if a string reads the same forwards and backwards
def is_palindrome(text: str) -> bool:
    cleaned = text.lower().replace(" ", "")    # str — text with spaces removed and lowercased
    return cleaned == cleaned[::-1]


# Test various strings to check if they are palindromes
print("\n=== Palindrome Check ===")
print("'racecar': " + str(is_palindrome("racecar")))    # 'racecar': True
print("'hello': " + str(is_palindrome("hello")))        # 'hello': False
print("'A man a plan a canal Panama': " + str(is_palindrome("A man a plan a canal Panama")))    # True


# Counts the number of vowels (a, e, i, o, u) in a string
def count_vowels(text: str) -> int:
    vowels = "aeiouAEIOU"    # str — all vowel characters to check against
    count = 0    # int — counter for vowels found
    for char in text:
        if char in vowels:
            count = count + 1
    return count


# Test vowel counting on different strings
print("\n=== Vowel Count ===")
print("'hello': " + str(count_vowels("hello")))      # 'hello': 2
print("'Python': " + str(count_vowels("Python")))    # 'Python': 2
print("'AEIOU': " + str(count_vowels("AEIOU")))      # 'AEIOU': 5


# Reverses a string by slicing it backwards
def reverse_string(text: str) -> str:
    return text[::-1]


# Test string reversal
print("\n=== Reverse String ===")
print("'hello': " + reverse_string("hello"))    # 'hello': olleh
print("'Python': " + reverse_string("Python"))  # 'Python': nohtyP


# Calculates the nth Fibonacci number using recursion
def find_fibonacci(n: int) -> int:
    if n <= 1:
        return n
    return find_fibonacci(n - 1) + find_fibonacci(n - 2)


# Display first 10 Fibonacci numbers
print("\n=== Fibonacci ===")
for i in range(10):
    print("F(" + str(i) + "): " + str(find_fibonacci(i)))    # F(0): 0, F(1): 1, F(2): 1, etc.


# Calculates factorial of a number using recursion
def calculate_factorial(n: int) -> int:
    if n <= 1:
        return 1
    return n * calculate_factorial(n - 1)


# Display factorials for numbers 1 through 5
print("\n=== Factorial ===")
for i in range(1, 6):
    print(str(i) + "! = " + str(calculate_factorial(i)))    # 1! = 1, 2! = 2, 3! = 6, etc.


# Checks if a number is prime by testing divisibility up to its square root
def is_prime(n: int) -> bool:
    if n < 2:
        return False
    for i in range(2, int(n ** 0.5) + 1):
        if n % i == 0:
            return False
    return True


# Test primality for numbers 1 through 10
print("\n=== Prime Check ===")
for num in [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]:
    print(str(num) + " is prime: " + str(is_prime(num)))    # 2 is prime: True, 4 is prime: False, etc.


# Returns a list of all prime numbers up to a given limit
def get_primes(limit: int) -> list:
    primes = []    # list — accumulator for prime numbers found
    for num in range(2, limit + 1):
        if is_prime(num):
            primes.append(num)
    return primes


# Get all primes up to 20
print("\n=== Primes up to 20 ===")
print(str(get_primes(20)))    # [2, 3, 5, 7, 11, 13, 17, 19]


# Converts Celsius temperature to Fahrenheit
def celsius_to_fahrenheit(c: float) -> float:
    return (c * 9 / 5) + 32


# Converts Fahrenheit temperature to Celsius
def fahrenheit_to_celsius(f: float) -> float:
    return (f - 32) * 5 / 9


# Demonstrate temperature conversions
print("\n=== Temperature Conversion ===")
print("100C = " + str(celsius_to_fahrenheit(100.0)) + "F")    # 100C = 212.0F
print("32F = " + str(fahrenheit_to_celsius(32.0)) + "C")     # 32F = 0.0C


# Counts the number of words in a text string
def get_word_count(text: str) -> int:
    words = text.split()    # list — text split into individual words
    return len(words)


# Returns the total number of characters in a string
def get_character_count(text: str) -> int:
    return len(text)


# Analyze a sample text string
print("\n=== String Analysis ===")
sample = "Hello World from Python"    # str — sample text to analyze
print("Text: '" + sample + "'")
print("Word count: " + str(get_word_count(sample)))           # Word count: 4
print("Character count: " + str(get_character_count(sample)))    # Character count: 21


# Calculates the average of numbers in a list
def calculate_average(numbers: list) -> float:
    if not numbers:
        return 0.0
    return sum(numbers) / len(numbers)


# Calculates the median (middle value) of numbers in a list
def calculate_median(numbers: list) -> float:
    if not numbers:
        return 0.0
    sorted_nums = sorted(numbers)    # list — numbers sorted in ascending order
    n = len(sorted_nums)    # int — count of numbers
    if n % 2 == 0:
        return (sorted_nums[n // 2 - 1] + sorted_nums[n // 2]) / 2
    return sorted_nums[n // 2]


# Calculate and display statistics for a dataset
print("\n=== Statistics ===")
data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]    # list — sample data set
print("Data: " + str(data))
print("Average: " + str(calculate_average(data)))    # Average: 5.5
print("Median: " + str(calculate_median(data)))      # Median: 5.5


# Validates an email address by checking for @ and domain
def validate_email(email: str) -> bool:
    if "@" not in email:
        return False
    parts = email.split("@")    # list — email split into local and domain parts
    if len(parts) != 2:
        return False
    if "." not in parts[1]:
        return False
    return True


# Test email validation on various inputs
print("\n=== Email Validation ===")
emails = ["test@example.com", "invalid", "user@domain", "another@test.org"]    # list — test emails
for email in emails:
    print(email + ": " + str(validate_email(email)))    # test@example.com: True, invalid: False, etc.


# Generates a username from a full name by combining lowercase parts
def generate_username(full_name: str) -> str:
    parts = full_name.lower().split()    # list — name parts in lowercase
    username = ""    # str — accumulator for username
    for part in parts:
        username = username + part
    return username


# Generate usernames from full names
print("\n=== Username Generator ===")
names = ["John Doe", "Alice Smith", "Bob Johnson"]    # list — full names to convert
for name in names:
    print(name + " -> " + generate_username(name))    # John Doe -> johndoe, etc.


# Calculates both area and perimeter of a rectangle
def calculate_rectangle(length: float, width: float) -> dict:
    area = length * width    # float — length × width
    perimeter = 2 * (length + width)    # float — 2 × (length + width)
    return {"area": area, "perimeter": perimeter}


# Calculate rectangle properties
print("\n=== Rectangle Calculator ===")
result = calculate_rectangle(10.0, 5.0)    # dict — area and perimeter
print("Area: " + str(result["area"]))       # Area: 50.0
print("Perimeter: " + str(result["perimeter"]))    # Perimeter: 30.0


# Classifies a number as Negative, Zero, Positive Even, or Positive Odd
def classify_number(n: int) -> str:
    if n < 0:
        return "Negative"
    elif n == 0:
        return "Zero"
    elif n % 2 == 0:
        return "Positive Even"
    return "Positive Odd"


# Test number classification
print("\n=== Number Classification ===")
for num in [-5, 0, 3, 8]:
    print(str(num) + ": " + classify_number(num))    # -5: Negative, 0: Zero, 3: Positive Odd, 8: Positive Even


# Determines if a year is a leap year (366 days) or regular year (365 days)
def get_days_in_year(year: int) -> int:
    if (year % 4 == 0 and year % 100 != 0) or (year % 400 == 0):
        return 366    # leap year
    return 365        # regular year


# Check days in different years
print("\n=== Days in Year ===")
print("2024: " + str(get_days_in_year(2024)) + " days")    # 2024: 366 days (leap year)
print("2023: " + str(get_days_in_year(2023)) + " days")    # 2023: 365 days
