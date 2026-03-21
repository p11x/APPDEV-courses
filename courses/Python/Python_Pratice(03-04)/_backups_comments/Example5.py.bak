# Example5.py
# Topic: Default Arguments

from typing import Optional

# This file demonstrates how to use default arguments in functions.
# Default arguments allow calling functions with fewer parameters,
# using predefined values when arguments are not provided.


# Greets a name with a default value of "World" if no name is given
def greet(name: str = "World") -> None:
    print("Hello, " + name + "!")

# Call greet() with and without arguments to see the default in action
greet()         # Hello, World!
greet("Alice")  # Hello, Alice!


# Creates an email address, using "example.com" as default domain
def create_email(username: str, domain: str = "example.com") -> str:
    return username + "@" + domain

email1 = create_email("john")            # str — email with default domain
print("Email: " + email1)               # Email: john@example.com

email2 = create_email("alice", "gmail.com")    # str — email with custom domain
print("Email: " + email2)                      # Email: alice@gmail.com


# Calculates total price including tax, with default tax rate of 10%
def calculate_price(price: float, tax_rate: float = 0.1) -> float:
    return price + (price * tax_rate)

total1 = calculate_price(100.0)    # float — uses default 10% tax
print("Total with default tax: $" + str(total1))    # Total with default tax: $110.0

total2 = calculate_price(100.0, 0.15)    # float — uses custom 15% tax
print("Total with custom tax: $" + str(total2))    # Total with custom tax: $115.0


# Returns account status, defaulting to "Active" if not specified
def get_status(active: bool = True) -> str:
    if active:
        return "Active"
    return "Inactive"

status1 = get_status()        # str — default is True (Active)
print("Default status: " + status1)    # Default status: Active

status2 = get_status(False)    # str — explicitly set to False
print("Custom status: " + status2)    # Custom status: Inactive


# Repeats text a certain number of times, defaulting to 3 repetitions
def repeat_text(text: str, times: int = 3) -> None:
    print(text * times)

repeat_text("Hi ")     # Hi Hi Hi  (repeated 3 times by default)
repeat_text("Yo ", 2)  # Yo Yo     (repeated 2 times)


# Calculates rectangle area, using 1.0 as default width for square calculations
def calculate_area(length: float, width: float = 1.0) -> float:
    return length * width

area1 = calculate_area(10.0)       # float — width defaults to 1.0
print("Area with default width: " + str(area1))    # Area with default width: 10.0

area2 = calculate_area(10.0, 5.0)  # float — custom width of 5.0
print("Area with custom width: " + str(area2))    # Area with custom width: 50.0


# Applies a discount to a price, defaulting to no discount (0.0)
def get_discounted_price(price: float, discount: float = 0.0) -> float:
    return price - (price * discount)

price1 = get_discounted_price(100.0)     # float — no discount by default
print("Price without discount: $" + str(price1))    # Price without discount: $100.0

price2 = get_discounted_price(100.0, 0.2)    # float — 20% discount applied
print("Price with 20% discount: $" + str(price2))    # Price with 20% discount: $80.0


# Creates a user profile string with default values for age and city
def create_user(name: str, age: int = 0, city: str = "Unknown") -> str:
    return name + ", " + str(age) + ", " + city

user1 = create_user("Alice")           # str — uses default age and city
print("User with defaults: " + user1)    # User with defaults: Alice, 0, Unknown

user2 = create_user("Bob", 25)        # str — provides age, uses default city
print("User with age: " + user2)        # User with age: Bob, 25, Unknown

user3 = create_user("Charlie", 30, "NYC")    # str — provides all values
print("User with all: " + user3)              # User with all: Charlie, 30, NYC


# Raises a base to a power, defaulting to squaring (exponent = 2)
def power(base: float, exponent: float = 2.0) -> float:
    return base ** exponent

result1 = power(5.0)          # float — 5 squared (default exponent)
print("5 squared: " + str(result1))    # 5 squared: 25.0

result2 = power(5.0, 3.0)     # float — 5 cubed (custom exponent)
print("5 cubed: " + str(result2))    # 5 cubed: 125.0


# Formats a name with optional middle name and title
def format_name(first: str, last: str = "", title: str = "") -> str:
    if title:
        return title + " " + first + " " + last
    return first + " " + last

name1 = format_name("John")              # str — just first name
print("Name: " + name1)                   # Name: John

name2 = format_name("John", "Doe")      # str — first and last name
print("Full name: " + name2)            # Full name: John Doe

name3 = format_name("John", "Doe", "Dr.")    # str — with title
print("With title: " + name3)               # With title: Dr. John Doe


# Calculates BMI using weight and height, with default height of 1.75m
def calculate_bmi(weight: float, height: float = 1.75) -> float:
    return weight / (height * height)

bmi1 = calculate_bmi(70.0)        # float — uses default height
print("BMI with default height: " + str(bmi1))    # BMI with default height: 22.857142857142858

bmi2 = calculate_bmi(70.0, 1.80)    # float — custom height provided
print("BMI with custom height: " + str(bmi2))    # BMI with custom height: 21.604938271604938


# Returns a greeting message with default values for both parameters
def get_message(greeting: str = "Hello", name: str = "Friend") -> str:
    return greeting + ", " + name + "!"

msg1 = get_message()                    # str — uses both defaults
print("Default message: " + msg1)        # Default message: Hello, Friend!

msg2 = get_message("Hi")                 # str — custom greeting only
print("Custom greeting: " + msg2)        # Custom greeting: Hi, Friend!

msg3 = get_message("Hi", "Alice")        # str — both custom values
print("Custom both: " + msg3)           # Custom both: Hi, Alice!


# Creates a user profile dictionary with optional bio and visibility settings
def create_profile(username: str, bio: str = "", is_public: bool = True) -> dict:
    return {
        "username": username,
        "bio": bio,
        "is_public": is_public
    }

profile1 = create_profile("john_doe")    # dict — minimal profile with defaults
print("Profile: " + str(profile1))    # Profile: {'username': 'john_doe', 'bio': '', 'is_public': True}

profile2 = create_profile("alice", "Love coding!")    # dict — with custom bio
print("Profile with bio: " + str(profile2))    # Profile with bio: {'username': 'alice', 'bio': 'Love coding!', 'is_public': True}

profile3 = create_profile("bob", "Developer", False)    # dict — private profile
print("Private profile: " + str(profile3))    # Private profile: {'username': 'bob', 'bio': 'Developer', 'is_public': False}


# Calculates final score with optional bonus points and multiplier
def calculate_score(points: int, bonus: int = 0, multiplier: float = 1.0) -> int:
    return int((points + bonus) * multiplier)

score1 = calculate_score(100)                 # int — basic score with defaults
print("Basic score: " + str(score1))          # Basic score: 100

score2 = calculate_score(100, 50)            # int — with bonus points
print("With bonus: " + str(score2))          # With bonus: 150

score3 = calculate_score(100, 50, 2.0)        # int — with bonus and multiplier
print("With multiplier: " + str(score3))    # With multiplier: 300


# Formats hours, minutes, and seconds into HH:MM:SS format with leading zeros
def get_time(hours: int = 0, minutes: int = 0, seconds: int = 0) -> str:
    return str(hours).zfill(2) + ":" + str(minutes).zfill(2) + ":" + str(seconds).zfill(2)

time1 = get_time()                 # str — all zeros (midnight)
print("Default time: " + time1)    # Default time: 00:00:00

time2 = get_time(10)               # str — only hours provided
print("With hours: " + time2)     # With hours: 10:00:00

time3 = get_time(10, 30)          # str — hours and minutes
print("With minutes: " + time3)  # With minutes: 10:30:00

time4 = get_time(10, 30, 45)      # str — full time provided
print("Full time: " + time4)     # Full time: 10:30:45


# Configures server connection string with default localhost and port 8080
def configure_server(host: str = "localhost", port: int = 8080, ssl: bool = False) -> str:
    protocol = "https" if ssl else "http"    # str — protocol based on SSL setting
    return protocol + "://" + host + ":" + str(port)

server1 = configure_server()                              # str — all defaults
print("Default server: " + server1)                      # Default server: http://localhost:8080

server2 = configure_server("example.com")                 # str — custom host
print("Custom host: " + server2)                         # Custom host: http://example.com:8080

server3 = configure_server("example.com", 443, True)     # str — full custom config
print("Full config: " + server3)                        # Full config: https://example.com:443


# Sends a notification message with optional urgency and recipient list
def send_notification(message: str, urgent: bool = False, recipients: Optional[list] = None) -> None:
    if recipients is None:
        recipients = ["all"]
    priority = "URGENT: " if urgent else ""    # str — prefix for urgent messages
    print("To: " + str(recipients))
    print("Message: " + priority + message)

# Different ways to call send_notification with default arguments
send_notification("Hello")                           # Default: not urgent, all users
send_notification("System update", True)          # Urgent notification
send_notification("Meeting", False, ["team@company.com"])    # Specific recipients
