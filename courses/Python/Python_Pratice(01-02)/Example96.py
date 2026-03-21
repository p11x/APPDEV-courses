# Example96.py
# Topic: Pattern Matching — Guards In Depth

# Advanced guard patterns in match statements

# === Guard with comparisons ===
number = 75

match number:
    case int() if number > 0:
        print("Positive: " + str(number))
    case int() if number < 0:
        print("Negative: " + str(number))
    case int():
        print("Zero")


# === Guard with string operations ===
username = "admin"

match username:
    case str() if username.startswith("admin"):
        print("Admin user detected")
    case str() if len(username) < 3:
        print("Username too short")
    case str():
        print("Regular user: " + username)


# === Guard with multiple conditions ===
age = 25
has_license = True

match (age, has_license):
    case (age, has_license) if age >= 18 and has_license:
        print("Can drive! Age: " + str(age))
    case (age, has_license) if age >= 18 and not has_license:
        print("Needs license. Age: " + str(age))
    case (age, _) if age < 18:
        print("Too young: " + str(age))
    case _:
        print("Invalid")


# === Guard with in operator ===
fruit = "apple"

match fruit:
    case str() if fruit in ["apple", "banana", "orange"]:
        print(fruit + " is a common fruit")
    case str() if fruit in ["mango", "papaya"]:
        print(fruit + " is a tropical fruit")
    case str():
        print("Unknown fruit: " + fruit)


# === Guard with string methods ===
email = "user@example.com"

match email:
    case str() if "@" in email and email.endswith(".com"):
        print("Commercial email")
    case str() if "@" in email and email.endswith(".org"):
        print("Organization email")
    case str() if "@" not in email:
        print("Invalid email - no @")
    case str():
        print("Other email format")


# === Guard with regex-like patterns ===
# Note: Python doesn't have regex in match, but we can simulate
text = "hello_world"

match text:
    case str() if "_" in text:
        print("Contains underscore: " + text)
    case str() if text.isupper():
        print("All uppercase: " + text)
    case str() if text.islower():
        print("All lowercase: " + text)
    case str():
        print("Mixed case: " + text)


# === Guard with type and value combined ===
value = 42

match value:
    case int() if value > 0 and value % 2 == 0:
        print("Positive even: " + str(value))
    case int() if value > 0 and value % 2 != 0:
        print("Positive odd: " + str(value))
    case int() if value < 0:
        print("Negative: " + str(value))
    case _:
        print("Zero or non-integer")


# === Guard with list operations ===
numbers = [1, 2, 3, 4, 5]

match numbers:
    case list() if len(numbers) == 0:
        print("Empty list")
    case list() if len(numbers) == 1:
        print("Single element: " + str(numbers[0]))
    case list() if len(numbers) >= 3:
        print("List has " + str(len(numbers)) + " elements")
    case list():
        print("Short list: " + str(numbers))


# === Guard with dictionary operations ===
config = {"debug": True, "verbose": False}

match config:
    case dict() if config.get("debug"):
        print("Debug mode ON")
    case dict() if config.get("verbose"):
        print("Verbose mode ON")
    case dict():
        print("Normal mode")


# === Practical: Access control ===
def check_access(user_role, time_hour):
    match (user_role, time_hour):
        case ("admin", hour) if hour >= 0 and hour < 24:
            return "Full access anytime"
        case ("user", hour) if 9 <= hour <= 17:
            return "User access during work hours"
        case ("guest", hour) if hour >= 9 and hour <= 18:
            return "Guest access limited hours"
        case (_, hour) if hour < 6 or hour > 22:
            return "No access - off hours"
        case _:
            return "Access denied"


print(check_access("admin", 3))
print(check_access("user", 10))
print(check_access("guest", 12))
print(check_access("user", 20))
print(check_access("guest", 23))
