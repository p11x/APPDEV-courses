# Example97.py
# Topic: Pattern Matching — Wildcard Patterns

# The _ wildcard pattern in match statements

# === Basic wildcard - catches everything ===
value = "anything"

match value:
    case "specific":
        print("Specific value")
    case _:
        print("Wildcard caught: " + str(value))


# === Wildcard with numbers ===
score = 45

match score:
    case 100:
        print("Perfect score!")
    case 90 | 80 | 70:
        print("Good score: " + str(score))
    case _:
        print("Other score: " + str(score))


# === Wildcard as last resort ===
status = "unknown"

match status:
    case "active":
        print("Active")
    case "pending":
        print("Pending")
    case _:
        print("Unknown status: " + status)


# === Using as to capture wildcard ===
# Note: _ is special - it doesn't capture the value by itself
# Use _ as name to capture while ignoring

data = 123

match data:
    case int() as x:  # This captures the value as x
        print("Captured: " + str(x))
    case _:
        print("Wildcard without capture")


# === Wildcard at different positions ===
# Note: Only one wildcard per pattern, and it's usually at the end
point = (5, 10)

match point:
    case (0, 0):
        print("Origin")
    case (_, 0):
        print("On X-axis")
    case (0, _):
        print("On Y-axis")
    case _:
        print("Somewhere else: " + str(point))


# === Wildcard in lists ===
items = [1, 2, 3, 4, 5]

match items:
    case []:
        print("Empty list")
    case [_]:
        print("One element")
    case [_, _]:
        print("Two elements")
    case [_, _, _]:
        print("Three elements")
    case _:
        print("Many elements: " + str(len(items)))


# === Wildcard in dicts ===
config = {"mode": "test", "extra": "data"}

match config:
    case {"mode": "production"}:
        print("Production mode")
    case {"mode": mode}:
        print("Mode: " + mode)
    case _:
        print("No config")


# === Wildcard is order sensitive ===
# Place more specific patterns first!
number = 42

match number:
    case int() if number > 0:
        print("Positive")
    case _:
        print("Other")


# === Multiple wildcards - using _ in different places ===
# Note: Can't use multiple _ in one pattern
pair = (1, 2)

match pair:
    case (x, y) if x == y:
        print("Equal: " + str(x) + " = " + str(y))
    case (x, _):
        print("First only: " + str(x))
    case _:
        print("No match")


# === Wildcard with type pattern ===
mixed = "hello"

match mixed:
    case str() as s if len(s) > 5:
        print("Long string: " + s)
    case str() as s:
        print("Short string: " + s)
    case _:
        print("Not a string")


# === Practical: Default handler ===
def handle_response(code):
    match code:
        case 200:
            return "OK"
        case 201:
            return "Created"
        case 404:
            return "Not Found"
        case 500:
            return "Server Error"
        case _:
            return "Unknown code: " + str(code)


print(handle_response(200))
print(handle_response(404))
print(handle_response(999))


# === Wildcard in nested patterns ===
user = {"name": "Alice", "role": "admin", "extra": "ignored"}

match user:
    case {"name": name, "role": "admin", **rest}:
        print("Admin: " + name)
    case {"name": name, **rest}:
        print("User: " + name)
    case _:
        print("No user")


# === When to use wildcard ===
# 1. As default case at the end
# 2. When you don't care about the value
# 3. To make all cases explicit
