# Example2.py
# Topic: Parameters and Arguments

# This file shows how to define functions with parameters
# and call them by passing arguments in the correct order.
# Each function below focuses on a different real-world use case.


# Takes a name string and prints a personalised greeting to the screen
def greet(name: str) -> None:
    print("Hello, " + name + "!")

# Call greet() three times with different names to see it working
greet("Alice")    # Hello, Alice!
greet("Bob")      # Hello, Bob!
greet("Charlie")  # Hello, Charlie!


# Adds two integers together and returns the total
def add_numbers(a: int, b: int) -> int:
    return a + b

sum_result = add_numbers(5, 10)    # int — result of adding 5 and 10
print("5 + 10 = " + str(sum_result))    # 5 + 10 = 15


# Joins a first and last name with a space in between
def full_name(first: str, last: str) -> str:
    return first + " " + last

name = full_name("John", "Doe")    # str — the complete full name
print("Full name: " + name)        # Full name: John Doe


# Prints a one-line introduction using three pieces of information
def introduce(name: str, age: int, city: str) -> None:
    print("I am " + name + ", " + str(age) + " years old, from " + city)

# Multiple calls show that the same function works for any combination of inputs
introduce("Alice", 25, "New York")       # I am Alice, 25 years old, from New York
introduce("Bob", 30, "Los Angeles")      # I am Bob, 30 years old, from Los Angeles


# Returns a sentence describing a pet using its name and type
def describe_pet(pet_name: str, pet_type: str) -> str:
    return pet_name + " is a " + pet_type

pet_description = describe_pet("Buddy", "dog")    # str — the full description sentence
print(pet_description)                             # Buddy is a dog


# Calculates the area of a rectangle given length and width in any unit
def calculate_area(length: float, width: float) -> float:
    return length * width

area = calculate_area(10.0, 5.0)    # float — area in square units
print("Area: " + str(area))         # Area: 50.0


# Combines a username and domain into a valid email address format
def create_email(username: str, domain: str) -> str:
    return username + "@" + domain

email = create_email("john", "example.com")    # str — formatted email address
print("Email: " + email)                       # Email: john@example.com


# Returns the month name for a given number (1 = January, 12 = December)
# Returns "Invalid" if the number is outside the 1–12 range
def get_month_name(month_number: int) -> str:
    months = ["January", "February", "March", "April", "May", "June",
              "July", "August", "September", "October", "November", "December"]
    # list — all 12 month names in order, used to look up by number
    if 1 <= month_number <= 12:
        return months[month_number - 1]    # subtract 1 because list starts at index 0
    return "Invalid"

month = get_month_name(5)    # str — the name of month number 5
print("Month 5: " + month)    # Month 5: May


# Calculates Body Mass Index using weight in kg and height in metres
# Formula: weight divided by height squared
def calculate_bmi(weight: float, height: float) -> float:
    return weight / (height * height)

bmi = calculate_bmi(70.0, 1.75)    # float — BMI value for 70 kg at 1.75 m
print("BMI: " + str(bmi))          # BMI: 22.857142857142858


# Returns a letter grade A–F based on a numeric score out of 100
def get_grade(score: int) -> str:
    if score >= 90:
        return "A"    # 90 and above
    elif score >= 80:
        return "B"    # 80–89
    elif score >= 70:
        return "C"    # 70–79
    elif score >= 60:
        return "D"    # 60–69
    return "F"        # below 60

grade = get_grade(85)    # str — letter grade for score 85
print("Grade: " + grade)    # Grade: B


# Calculates shipping cost using weight (kg), distance (km), and a fixed rate
def calculate_shipping(weight: float, distance: float) -> float:
    rate = 0.5    # float — cost per kg per km
    return weight * distance * rate

shipping_cost = calculate_shipping(10.0, 100.0)    # float — total cost in dollars
print("Shipping cost: $" + str(shipping_cost))    # Shipping cost: $500.0


# Returns a two-character initials string from first and last name
# Uses index [0] to grab the first character of each name
def get_initials(first_name: str, last_name: str) -> str:
    return first_name[0] + "." + last_name[0] + "."

initials = get_initials("John", "Doe")    # str — initials in J.D. format
print("Initials: " + initials)            # Initials: J.D.


# Calculates compound interest and returns the final amount
# Formula: principal × (1 + rate) ^ time
def calculate_compound_interest(principal: float, rate: float, time: float) -> float:
    amount = principal * (1 + rate) ** time    # float — final amount after growth
    return amount

final_amount = calculate_compound_interest(1000.0, 0.05, 3)    # float — amount after 3 years
print("Final amount: $" + str(final_amount))                  # Final amount: $1157.625


# Converts a distance in miles to kilometres using the conversion factor 1.60934
def convert_distance(miles: float) -> float:
    kilometers = miles * 1.60934    # float — equivalent distance in km
    return kilometers

km = convert_distance(10.0)                  # float — kilometres for 10 miles
print("10 miles = " + str(km) + " km")    # 10 miles = 16.0934 km


# Calculates the area of a triangle using base and height
# Formula: 0.5 × base × height
def get_triangle_area(base: float, height: float) -> float:
    return 0.5 * base * height

triangle_area = get_triangle_area(10.0, 5.0)    # float — area in square units
print("Triangle area: " + str(triangle_area))   # Triangle area: 25.0


# Subtracts deductions from a basic salary to get the take-home pay
def calculate_net_salary(basic_salary: float, deductions: float) -> float:
    return basic_salary - deductions

net = calculate_net_salary(5000.0, 500.0)    # float — net salary after deductions
print("Net salary: $" + str(net))            # Net salary: $4500.0


# Builds a full mailing address string from its individual components
def get_full_address(street: str, city: str, state: str, zip_code: str) -> str:
    return street + ", " + city + ", " + state + " " + zip_code

address = get_full_address("123 Main St", "Springfield", "IL", "62701")
# str — complete formatted address
print("Address: " + address)    # Address: 123 Main St, Springfield, IL 62701


# Calculates the volume of a rectangular box (cuboid)
# Formula: length × width × height
def calculate_volume(length: float, width: float, height: float) -> float:
    return length * width * height

volume = calculate_volume(10.0, 5.0, 2.0)    # float — volume in cubic units
print("Volume: " + str(volume))               # Volume: 100.0


# Assembles a custom message from separate greeting, name, and punctuation parts
def get_message(greeting: str, name: str, punctuation: str) -> str:
    return greeting + " " + name + punctuation

msg = get_message("Hello", "Alice", "!")    # str — the complete assembled message
print(msg)                                   # Hello, Alice!


# Calculates the hourly rate from an annual salary and weekly hours worked
# Formula: annual salary ÷ (hours per week × 52 weeks)
def calculate_hourly_rate(annual_salary: float, hours_per_week: float) -> float:
    weeks_per_year = 52.0    # float — fixed number of working weeks in a year
    return annual_salary / (hours_per_week * weeks_per_year)

hourly = calculate_hourly_rate(52000.0, 40.0)    # float — hourly rate in dollars
print("Hourly rate: $" + str(hourly))             # Hourly rate: $25.0


# Returns the number of days in a given month, accounting for leap years
# February has 29 days in a leap year (divisible by 4, except century years unless div by 400)
def get_days_in_month(month: int, year: int) -> int:
    if month in [1, 3, 5, 7, 8, 10, 12]:
        return 31    # months with 31 days
    elif month in [4, 6, 9, 11]:
        return 30    # months with 30 days
    elif month == 2:
        if (year % 4 == 0 and year % 100 != 0) or (year % 400 == 0):
            return 29    # leap year February
        return 28        # regular February
    return 0             # invalid month number

days = get_days_in_month(2, 2024)              # int — number of days in Feb 2024
print("Days in Feb 2024: " + str(days))       # Days in Feb 2024: 29
