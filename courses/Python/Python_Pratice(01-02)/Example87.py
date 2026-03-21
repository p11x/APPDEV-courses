# Example87.py
# Topic: Pattern Matching — Guards (if conditions)

# Use guards (if conditions) to add extra checks to patterns

# === Basic guard with if ===
# Add 'if condition' after the case pattern

age = 25

match age:
    case int() if age >= 18:
        print("Adult (age " + str(age) + ")")
    case int() if age < 18:
        print("Minor (age " + str(age) + ")")
    case _:
        print("Not a valid age")


# === Guard with string matching ===
username = "admin"

match username:
    case "admin" if username == "admin":
        print("Welcome, Administrator!")
    case "user" if len(username) > 3:
        print("Welcome, User!")
    case _:
        print("Welcome, Guest!")


# === Guard with number comparison ===
score = 85

match score:
    case int() if score >= 90:
        print("Grade: A")
    case int() if score >= 80:
        print("Grade: B")
    case int() if score >= 70:
        print("Grade: C")
    case int() if score >= 60:
        print("Grade: D")
    case int():
        print("Grade: F")
    case _:
        print("Invalid score")


# === Guard with length check ===
text = "hello"

match text:
    case str() if len(text) == 0:
        print("Empty string")
    case str() if len(text) <= 3:
        print("Short string: " + text)
    case str() if len(text) <= 10:
        print("Medium string: " + text)
    case str():
        print("Long string: " + text)


# === Guard with multiple conditions ===
quantity = 5
price = 10.0

total = quantity * price

match quantity:
    case int() if quantity >= 100:
        print("Bulk discount! Total: $" + str(total * 0.8))
    case int() if quantity >= 50:
        print("Volume discount! Total: $" + str(total * 0.9))
    case int() if quantity >= 10:
        print("Small discount! Total: $" + str(total * 0.95))
    case int():
        print("Regular price! Total: $" + str(total))


# === Guard combined with type matching ===
value = 42

match value:
    case int() if value > 0:
        print("Positive integer: " + str(value))
    case int() if value < 0:
        print("Negative integer: " + str(value))
    case int():
        print("Zero")
    case str() if value == "":
        print("Empty string")
    case str():
        print("String: " + value)
    case _:
        print("Other type")


# === Guard with list patterns ===
numbers = [1, 2, 3]

match numbers:
    case []:
        print("Empty list")
    case [x] if x == 1:
        print("Single element: 1")
    case [x, y] if x < y:
        print("Two elements in order: " + str(x) + " < " + str(y))
    case [x, y]:
        print("Two elements: " + str(x) + ", " + str(y))
    case _:
        print("List with more elements")


# === Practical: User access control ===
def check_access(user_role, age):
    match (user_role, age):
        case ("admin", age) if age >= 18:
            return "Full admin access"
        case ("moderator", age) if age >= 18:
            return "Moderator access"
        case ("user", age) if age >= 18:
            return "Regular user access"
        case (_, age) if age < 18:
            return "Access denied - too young"
        case _:
            return "Access denied"


print(check_access("admin", 25))
print(check_access("moderator", 20))
print(check_access("user", 18))
print(check_access("guest", 30))
print(check_access("user", 15))
