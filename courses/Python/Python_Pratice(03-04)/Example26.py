# Example26.py
# Topic: Return Values - Multiple Return Values with Tuples

# This file demonstrates functions that return multiple values as tuples
# in Python. Tuples allow functions to return multiple pieces of data
# that can be easily unpacked by the caller.


# Returns the quotient and remainder when dividing two numbers
def divide(a: int, b: int) -> tuple[int, int]:
    quotient = a // b
    remainder = a % b
    return quotient, remainder

q, r = divide(17, 5)    # tuple[int, int] — quotient and remainder
print("17 divided by 5:")    # 17 divided by 5:
print("  Quotient: " + str(q))    #   Quotient: 3
print("  Remainder: " + str(r))    #   Remainder: 2


# Returns the minimum, maximum, and average of a list of numbers
def get_stats(numbers: list[float]) -> tuple[float, float, float]:
    minimum = min(numbers)
    maximum = max(numbers)
    average = sum(numbers) / len(numbers)
    return minimum, maximum, average

min_val, max_val, avg = get_stats([10, 20, 30, 40, 50])
print("Stats: min=" + str(min_val) + ", max=" + str(max_val) + ", avg=" + str(avg))
# Stats: min=10.0, max=50.0, avg=30.0


# Returns the name and age as a tuple
def get_person_info() -> tuple[str, int]:
    return "Alice", 30

name, age = get_person_info()    # tuple[str, int] — name and age
print(name + " is " + str(age) + " years old")    # Alice is 30 years old


# Returns the result of a division as quotient and remainder
def divide_with_check(a: int, b: int) -> tuple[int, int]:
    if b == 0:
        return 0, -1    # Return -1 as error indicator for division by zero
    return a // b, a % b

result, error = divide_with_check(20, 4)
print("20 / 4: result=" + str(result) + ", error=" + str(error))    # 20 / 4: result=5, error=0


# Returns the first and last name combined and the full name length
def process_name(first: str, last: str) -> tuple[str, int]:
    full_name = first + " " + last
    return full_name, len(full_name)

full, length = process_name("John", "Doe")
print("Full name: " + full + ", length: " + str(length))
# Full name: John Doe, length: 8


# Returns the area and perimeter of a rectangle
def rectangle_properties(length: float, width: float) -> tuple[float, float]:
    area = length * width
    perimeter = 2 * (length + width)
    return area, perimeter

area, perimeter = rectangle_properties(10.0, 5.0)
print("Rectangle: area=" + str(area) + ", perimeter=" + str(perimeter))
# Rectangle: area=50.0, perimeter=30.0


# Returns the real and imaginary parts of a complex number
def get_complex_parts(complex_num: complex) -> tuple[float, float]:
    return complex_num.real, complex_num.imag

real_part, imag_part = get_complex_parts(3 + 4j)
print("Real: " + str(real_part) + ", Imaginary: " + str(imag_part))
# Real: 3.0, Imaginary: 4.0


# Returns a student's name and their grades as a tuple
def get_student_grades() -> tuple[str, list[int]]:
    return "Bob", [85, 90, 78, 92, 88]

student, grades = get_student_grades()
print(student + "'s grades: " + str(grades))    # Bob's grades: [85, 90, 78, 92, 88]


# Returns the x and y coordinates swapped
def swap_coordinates(x: float, y: float) -> tuple[float, float]:
    return y, x

original_x, original_y = 5.0, 10.0
swapped_x, swapped_y = swap_coordinates(original_x, original_y)
print("Original: (" + str(original_x) + ", " + str(original_y) + ")")
print("Swapped: (" + str(swapped_x) + ", " + str(swapped_y) + ")")
# Original: (5.0, 10.0)
# Swapped: (10.0, 5.0)


# Returns the year, month, and day from a date tuple
def get_date_parts(date: tuple[int, int, int]) -> tuple[int, int, int]:
    return date[0], date[1], date[2]

year, month, day = get_date_parts((2024, 3, 15))
print("Year: " + str(year) + ", Month: " + str(month) + ", Day: " + str(day))
# Year: 2024, Month: 3, Day: 15


# Returns the string representation and length
def get_string_info(text: str) -> tuple[str, int]:
    return text, len(text)

text, length = get_string_info("Python")
print("Text: '" + text + "', Length: " + str(length))
# Text: 'Python', Length: 6


# Returns the sine and cosine of an angle
import math
def get_trig_values(angle: float) -> tuple[float, float]:
    return math.sin(angle), math.cos(angle)

sin_val, cos_val = get_trig_values(math.pi / 2)
print("At 90 degrees: sin=" + str(sin_val) + ", cos=" + str(cos_val))
# At 90 degrees: sin=1.0, cos=6.123233995736766e-17


# Real-life Example 1: Get stock quote with change
def get_stock_quote(symbol: str) -> tuple[str, float, float, float]:
    prices = {
        "AAPL": (175.50, 2.35, 1.36),
        "GOOGL": (142.30, -1.20, -0.84),
        "MSFT": (378.90, 5.67, 1.52)
    }
    if symbol in prices:
        price, change, change_pct = prices[symbol]
        return symbol, price, change, change_pct
    return symbol, 0.0, 0.0, 0.0

symbol, price, change, pct = get_stock_quote("AAPL")
print(f"Stock: {symbol} @ ${price} ({change:+.2f}, {pct:+.2f}%)")    # Stock: AAPL @ $175.5 (+2.35, +1.36%)


# Real-life Example 2: Calculate profit/loss for a business
def calculate_profit(revenue: float, cost: float) -> tuple[float, float, str]:
    profit = revenue - cost
    if profit > 0:
        status = "Profit"
    elif profit < 0:
        status = "Loss"
    else:
        status = "Break Even"
    
    margin = (profit / revenue * 100) if revenue > 0 else 0
    return profit, margin, status

profit, margin, status = calculate_profit(50000, 35000)
print(f"Business: {status}, Profit: ${profit}, Margin: {margin:.1f}%")
# Business: Profit, Profit: $15000.0, Margin: 30.0%


# Real-life Example 3: Parse a full address into components
def parse_address(full_address: str) -> tuple[str, str, str, str]:
    parts = full_address.split(",")
    if len(parts) >= 3:
        street = parts[0].strip()
        city = parts[1].strip()
        state = parts[2].strip().split()[0]
        zip_code = parts[2].strip().split()[1] if len(parts[2].strip().split()) > 1 else ""
        return street, city, state, zip_code
    return "", "", "", ""

street, city, state, zip_code = parse_address("123 Main Street, New York, NY 10001")
print(f"Address: {street}, {city}, {state} {zip_code}")
# Address: 123 Main Street, New York, NY 10001


# Real-life Example 4: Get employee details with department info
def get_employee_details(employee_id: int) -> tuple[str, str, str, float]:
    employees = {
        1: ("Alice Johnson", "Engineering", "Senior Developer", 95000),
        2: ("Bob Smith", "Marketing", "Marketing Manager", 78000),
        3: ("Carol White", "HR", "HR Director", 82000)
    }
    
    if employee_id in employees:
        return employees[employee_id]
    return "", "", "", 0.0

name, dept, title, salary = get_employee_details(1)
print(f"Employee: {name}, {title}, Dept: {dept}, Salary: ${salary}")
# Employee: Alice Johnson, Senior Developer, Dept: Engineering, Salary: $95000


# Real-life Example 5: Calculate shipping dimensions and cost
def calculate_shipping_dimensions(length: float, width: float, height: float) -> tuple[float, float, str]:
    volume = length * width * height
    
    if volume < 1000:
        category = "Small"
        rate = 5.99
    elif volume < 5000:
        category = "Medium"
        rate = 12.99
    else:
        category = "Large"
        rate = 24.99
    
    return volume, rate, category

volume, cost, category = calculate_shipping_dimensions(30, 20, 15)
print(f"Package: {category}, Volume: {volume} cu in, Shipping: ${cost}")
# Package: Small, Volume: 9000.0 cu in, Shipping: $12.99


# Real-life Example 6: Parse API response with status and data
def parse_api_response(response_code: int, data: dict) -> tuple[bool, str, dict]:
    if response_code == 200:
        return True, "Success", data
    elif response_code == 404:
        return False, "Not Found", {}
    elif response_code == 401:
        return False, "Unauthorized", {}
    elif response_code == 500:
        return False, "Server Error", {}
    return False, "Unknown Error", {}

success, message, payload = parse_api_response(200, {"user": "alice", "id": 123})
print(f"API Response: Success={success}, Message={message}, Data={payload}")
# API Response: Success=True, Message=Success, Data={'user': 'alice', 'id': 123}
