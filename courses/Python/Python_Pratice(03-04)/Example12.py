# Example12.py
# Topic: Positional-Only Parameters (/)

# This file demonstrates positional-only parameters in Python 3.8+.
# Parameters before the / must be passed by position, not by keyword.
# This is useful for API design and preventing users from using keyword arguments incorrectly.


# Simple function with positional-only parameters
# Parameters 'a' and 'b' must be passed positionally, 'c' can be positional or keyword
def pos_only(a, b, /, c) -> None:
    print("a=" + str(a) + ", b=" + str(b) + ", c=" + str(c))

pos_only(1, 2, 3)    # a=1, b=2, c=3 (all positional)
pos_only(1, 2, c=3)    # a=1, b=2, c=3 (c as keyword)


# Parameters before / cannot be passed as keywords
# The name parameter must be positional-only, age and city can be keyword
def create_user(name, /, age, city) -> None:
    print("Name: " + name + ", Age: " + str(age) + ", City: " + city)

create_user("Alice", 25, "NYC")                 # Name: Alice, Age: 25, City: NYC
create_user("Bob", city="LA", age=30)          # Name: Bob, Age: 30, City: LA


# All three parameters are positional-only
def add_all(a, b, c, /) -> int:
    return a + b + c    # int — sum of all three

result = add_all(1, 2, 3)    # int — 1+2+3 = 6
print("Sum: " + str(result))    # Sum: 6


# Mix positional-only with regular parameters and default values
# a and b are positional-only, operation and multiplier can be keyword
def calculate(a, b, /, operation, multiplier=1) -> int:
    if operation == "add":
        result = a + b
    else:
        result = a * b
    return result * multiplier    # int — apply multiplier

result = calculate(5, 3, operation="add")    # int — (5+3)*1 = 8
print("Result: " + str(result))              # Result: 8

result2 = calculate(5, 3, operation="multiply", multiplier=2)    # int — (5*3)*2 = 30
print("Result: " + str(result2))                                  # Result: 30


# Positional-only with default values for the regular parameters
def greet(name, /, greeting="Hello") -> None:
    print(greeting + ", " + name + "!")

greet("Alice")    # Hello, Alice! (uses default greeting)
greet("Bob", "Hi")    # Hi, Bob! (uses custom greeting)


# Function that mimics str.replace behavior
# All parameters (except the string) are positional-only
def replace_text(text, old, new, /, count=-1) -> str:
    return text.replace(old, new, count)    # str — replaced string

result = replace_text("hello world", "world", "Python")    # str — "hello Python"
print("Result: " + result)                                   # Result: hello Python

result2 = replace_text("aaa aaa", "a", "b", 2)    # str — "bba aaa" (only first 2)
print("Result: " + result2)                       # Result: bba aaa


# Simulates an API request function with positional-only URL
def fetch(url, /, method="GET", headers=None) -> None:
    print("URL: " + url)
    print("Method: " + method)
    if headers:    # check if headers were provided
        print("Headers: " + str(headers))

fetch("https://api.example.com")                            # default GET request
fetch("https://api.example.com", "POST")                    # POST request
fetch("https://api.example.com", headers={"Auth": "token"})    # GET with headers


# Combining positional-only with *args
def func_with_args(a, b, /, *args, keyword_arg) -> None:
    print("a=" + str(a) + ", b=" + str(b))
    print("args=" + str(args))    # tuple of extra positional arguments
    print("keyword_arg=" + str(keyword_arg))    # required keyword argument

func_with_args(1, 2, 3, 4, 5, keyword_arg="value")    # a=1, b=2, args=(3,4,5), keyword_arg=value


# Complex example: math operations with positional-only parameters
def math_op(x, y, /, op="add", precision=2) -> float:
    if op == "add":
        result = x + y
    elif op == "subtract":
        result = x - y
    elif op == "multiply":
        result = x * y
    elif op == "divide":
        result = x / y
    else:
        result = 0
    
    return round(result, precision)    # float — rounded to specified precision

result = math_op(10, 5, op="add")    # float — 15.0
print("Result: " + str(result))        # Result: 15.0

result2 = math_op(10, 5, op="divide", precision=3)    # float — 2.0
print("Result: " + str(result2))                      # Result: 2.0


# Positional-only for performance-critical code (like image processing)
def process_pixels(width, height, /, channels=3) -> None:
    total = width * height * channels    # int — total pixel count
    print("Processing " + str(total) + " pixels")

process_pixels(1920, 1080)               # Processing 6220800 pixels
process_pixels(1920, 1080, channels=4)    # Processing 8294400 pixels (RGBA)


# Prevent keyword arguments for function signature clarity
def send_message(sender, recipient, /, subject="", body="") -> None:
    print("From: " + sender)
    print("To: " + recipient)
    print("Subject: " + subject)
    print("Body: " + body)

send_message("alice@example.com", "bob@example.com", body="Hello!")    # full message with body
