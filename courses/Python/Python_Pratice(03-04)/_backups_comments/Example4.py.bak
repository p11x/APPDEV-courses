# Example4.py
# Topic: Print vs Return - Understanding the Difference

# This file demonstrates the key difference between using print() and
# return in functions. Print displays values but returns nothing (None),
# while return passes values back for use in further calculations.


# Adds two numbers but only prints the result (does not return it)
def add_with_print(a: int, b: int) -> None:
    print(a + b)

result1 = add_with_print(5, 3)    # None — functions with print() return nothing
print("Result from print function: " + str(result1))    # Result from print function: None


# Adds two numbers and returns the result for use elsewhere
def add_with_return(a: int, b: int) -> int:
    return a + b

result2 = add_with_return(5, 3)    # int — the sum of 5 and 3
print("Result from return function: " + str(result2))    # Result from return function: 8


# Prints a greeting but returns nothing useful
def greet_with_print(name: str) -> None:
    print("Hello, " + name + "!")

message1 = greet_with_print("Alice")    # None — printed but not returned
print("Returned value: " + str(message1))    # Returned value: None


# Returns the greeting message so it can be stored and used
def greet_with_return(name: str) -> str:
    return "Hello, " + name + "!"

message2 = greet_with_return("Alice")    # str — the greeting message
print("Returned value: " + message2)    # Returned value: Hello, Alice!
print("Length of message: " + str(len(message2)))    # Length of message: 13


# Calculates total but only prints it (value is lost)
def calculate_total_print(prices: list) -> None:
    total = 0.0    # float — running total of prices
    for price in prices:
        total = total + price
    print("Total: $" + str(total))

result3 = calculate_total_print([10.0, 20.0, 30.0])    # None — cannot use the total
print("Returned value: " + str(result3))    # Returned value: None


# Returns the total so it can be used in further calculations
def calculate_total_return(prices: list) -> float:
    total = 0.0    # float — running total of prices
    for price in prices:
        total = total + price
    return total

result4 = calculate_total_return([10.0, 20.0, 30.0])    # float — the calculated total
print("Returned value: $" + str(result4))    # Returned value: $60.0
print("Can use in calculation: $" + str(result4 * 1.1))    # Can use in calculation: $66.0


# Squares a number but only prints it (value cannot be reused)
def get_square_print(n: int) -> None:
    print(n * n)

result5 = get_square_print(5)    # None — cannot store or reuse the result
print("Returned value: " + str(result5))    # Returned value: None


# Returns the squared value so it can be stored and used
def get_square_return(n: int) -> int:
    return n * n

result6 = get_square_return(5)    # int — 5 squared equals 25
print("Returned value: " + str(result6))    # Returned value: 25
doubled = result6 * 2    # int — 25 doubled equals 50
print("Doubled: " + str(doubled))    # Doubled: 50


# Finds maximum but only prints it (value is lost)
def find_max_print(numbers: list) -> None:
    max_val = numbers[0]    # int — start with first element as maximum
    for num in numbers:
        if num > max_val:
            max_val = num
    print("Maximum: " + str(max_val))

result7 = find_max_print([1, 5, 3, 9, 2])    # None — cannot use the max value
print("Returned value: " + str(result7))    # Returned value: None


# Returns the maximum value so it can be used in conditions
def find_max_return(numbers: list) -> int:
    max_val = numbers[0]    # int — start with first element as maximum
    for num in numbers:
        if num > max_val:
            max_val = num
    return max_val

result8 = find_max_return([1, 5, 3, 9, 2])    # int — largest number in the list
print("Returned value: " + str(result8))    # Returned value: 9
print("Is max > 5? " + str(result8 > 5))    # Is max > 5? True


# Processes data but only prints average (cannot access the total)
def process_data_print(data: list) -> None:
    total = sum(data)    # int — sum of all data points
    average = total / len(data)    # float — calculated average
    print("Average: " + str(average))

result9 = process_data_print([10, 20, 30])    # None — cannot use the results
print("Returned value: " + str(result9))    # Returned value: None


# Returns a dictionary with both total and average for multiple uses
def process_data_return(data: list) -> dict:
    total = sum(data)    # int — sum of all data points
    average = total / len(data)    # float — calculated average
    return {"total": total, "average": average}

result10 = process_data_return([10, 20, 30])    # dict — contains both values
print("Returned value: " + str(result10))    # Returned value: {'total': 60, 'average': 20.0}
print("Total: " + str(result10["total"]))    # Total: 60
print("Average: " + str(result10["average"]))    # Average: 20.0


# Prints a welcome message but cannot store or reuse it
def create_message_print(name: str) -> None:
    print("Welcome, " + name + "!")

result11 = create_message_print("Alice")    # None — message is only printed
print("Can store message? " + str(result11))    # Can store message? None


# Returns the message so it can be stored in a variable
def create_message_return(name: str) -> str:
    return "Welcome, " + name + "!"

result12 = create_message_return("Alice")    # str — the welcome message
stored_message = result12    # str — stored for later use
print("Stored message: " + stored_message)    # Stored message: Welcome, Alice!


# Doubles a number but only prints it (cannot use in calculations)
def double_number_print(n: int) -> None:
    print(n * 2)

result13 = double_number_print(10)    # None — cannot store the doubled value
print("Returned value: " + str(result13))    # Returned value: None


# Returns the doubled value so it can be used in further math
def double_number_return(n: int) -> int:
    return n * 2

result14 = double_number_return(10)    # int — 10 doubled equals 20
print("Returned value: " + str(result14))    # Returned value: 20
print("Use in math: " + str(result14 + 5))    # Use in math: 25


# Checks if even but only prints the result (cannot use in conditions)
def check_even_print(n: int) -> None:
    if n % 2 == 0:
        print("Even")
    else:
        print("Odd")

result15 = check_even_print(10)    # None — cannot use for logic
print("Returned value: " + str(result15))    # Returned value: None


# Returns a boolean so it can be used in if statements
def check_even_return(n: int) -> bool:
    return n % 2 == 0

result16 = check_even_return(10)    # bool — True because 10 is even
print("Returned value: " + str(result16))    # Returned value: True
if result16:    # can use the boolean in a conditional
    print("Number is even!")    # Number is even!


# Formats a name but only prints it (cannot store or reuse)
def format_name_print(first: str, last: str) -> None:
    print(last + ", " + first)

result17 = format_name_print("John", "Doe")    # None — only displays the result
print("Returned value: " + str(result17))    # Returned value: None


# Returns the formatted name so it can be stored and manipulated
def format_name_return(first: str, last: str) -> str:
    return last + ", " + first

result18 = format_name_return("John", "Doe")    # str — formatted name
print("Returned value: " + result18)    # Returned value: Doe, John
print("Length: " + str(len(result18)))    # Length: 9


# Prints user info but returns nothing useful
def get_user_print(username: str) -> None:
    print("User: " + username)

result19 = get_user_print("alice")    # None — cannot access user data
print("Returned value: " + str(result19))    # Returned value: None


# Returns user data as a dictionary for multiple uses
def get_user_return(username: str) -> dict:
    return {"username": username, "active": True}

result20 = get_user_return("alice")    # dict — user data with multiple fields
print("Returned value: " + str(result20))    # Returned value: {'username': 'alice', 'active': True}
print("Username: " + result20["username"])    # Username: alice
print("Is active: " + str(result20["active"]))    # Is active: True
