# Example83.py
# Topic: Pattern Matching — Multiple Patterns and Wildcard

# Matching multiple values with | and using wildcard _

# === Matching multiple patterns (OR) ===
# Use | to match multiple values

code = 201

match code:
    case 200 | 201 | 202:
        print("Success - code " + str(code))
    case 400 | 401 | 403:
        print("Client error - code " + str(code))
    case 500 | 502 | 503:
        print("Server error - code " + str(code))
    case _:
        print("Other code: " + str(code))

# === HTTP status categories ===
def get_status_category(code):
    match code:
        case 200 | 201 | 202 | 204:
            return "Success"
        case 400 | 401 | 403 | 404:
            return "Client Error"
        case 500 | 502 | 503:
            return "Server Error"
        case _:
            return "Unknown"


print(get_status_category(200))
print(get_status_category(404))
print(get_status_category(999))

# === Wildcard pattern (_) ===
# _ matches anything - acts as default

point = (10, 20)

match point:
    case (0, 0):
        print("Origin point")
    case (x, 0):
        print("On X-axis at x=" + str(x))
    case (0, y):
        print("On Y-axis at y=" + str(y))
    case _:
        print("Somewhere else: " + str(point))

# === Wildcard in different contexts ===
value = "anything"

match value:
    case "specific":
        print("Matched specific")
    case _:
        print("Wildcard - matched anything")

# === Multiple patterns with strings ===
day = "saturday"

match day:
    case "monday" | "tuesday" | "wednesday" | "thursday" | "friday":
        print("Weekday")
    case "saturday" | "sunday":
        print("Weekend")
    case _:
        print("Invalid day")

# === Using both | and _ ===
role = "superuser"

match role:
    case "admin" | "superuser":
        print("Full access")
    case "editor" | "viewer":
        print("Limited access")
    case _:
        print("No access")
