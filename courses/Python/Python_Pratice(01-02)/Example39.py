# Example39.py
# Topic: Control Flow — Match Statement Practical Examples

# More real-world examples using match statements

# === Calculator with Match ===

def calculate(a, b, operator):
    match operator:
        case "+":
            return a + b
        case "-":
            return a - b
        case "*":
            return a * b
        case "/":
            return a / b if b != 0 else "Error: Division by zero"
        case _:
            return "Unknown operator"

result = calculate(10, 5, "+")
print("10 + 5 = " + str(result))

result = calculate(10, 5, "*")
print("10 * 5 = " + str(result))

result = calculate(10, 0, "/")
print("10 / 0 = " + str(result))

# === Day Type Classifier ===

def get_day_type(day):
    match day.lower():
        case "monday" | "tuesday" | "wednesday" | "thursday" | "friday":
            return "Weekday"
        case "saturday" | "sunday":
            return "Weekend"
        case _:
            return "Invalid day"

print(get_day_type("Monday"))    # Weekday
print(get_day_type("Saturday"))  # Weekend
print(get_day_type("Funday"))    # Invalid day

# === Traffic Light Controller ===

def get_action(light):
    match light.lower():
        case "red":
            return "STOP"
        case "yellow":
            return "SLOW DOWN"
        case "green":
            return "GO"
        case _:
            return "INVALID"

print("Red light: " + get_action("red"))
print("Green light: " + get_action("green"))

# === Shipping Calculator ===

def calculate_shipping(weight, destination):
    match [weight, destination]:
        case [w, "local"] if w < 1:
            return "$5.00"
        case [w, "local"] if w < 5:
            return "$10.00"
        case [w, "local"]:
            return "$20.00"
        case [w, "national"] if w < 1:
            return "$10.00"
        case [w, "national"] if w < 5:
            return "$25.00"
        case [w, "national"]:
            return "$50.00"
        case [w, "international"] if w < 1:
            return "$20.00"
        case [w, "international"] if w < 5:
            return "$60.00"
        case [w, "international"]:
            return "$100.00"
        case _:
            return "Invalid destination"

print("Local <1kg: " + calculate_shipping(0.5, "local"))
print("National 3kg: " + calculate_shipping(3, "national"))
print("International 10kg: " + calculate_shipping(10, "international"))

# === Grade with Plus/Minus ===

def get_grade(score):
    match score:
        case 100:
            return "A+"
        case s if s >= 90:
            return "A"
        case s if s >= 87:
            return "B+"
        case s if s >= 80:
            return "B"
        case s if s >= 77:
            return "C+"
        case s if s >= 70:
            return "C"
        case s if s >= 67:
            return "D+"
        case s if s >= 60:
            return "D"
        case _:
            return "F"

print("Score 100: " + get_grade(100))
print("Score 92: " + get_grade(92))
print("Score 85: " + get_grade(85))
print("Score 72: " + get_grade(72))
print("Score 55: " + get_grade(55))

# === HTTP Method Handler ===

def handle_request(method, path):
    match [method.upper(), path]:
        case ["GET", _]:
            return "READ"
        case ["POST", _]:
            return "CREATE"
        case ["PUT", path] if "users" in path:
            return "UPDATE user"
        case ["PUT", _]:
            return "UPDATE"
        case ["DELETE", _]:
            return "DELETE"
        case _:
            return "UNKNOWN"

print("GET /api/users: " + handle_request("GET", "/api/users"))
print("POST /api/users: " + handle_request("POST", "/api/users"))
print("PUT /api/users/1: " + handle_request("PUT", "/api/users/1"))

# === Priority Sorter ===

def get_priority(level):
    match level:
        case "critical" | "urgent":
            return 1
        case "high":
            return 2
        case "medium" | "normal":
            return 3
        case "low":
            return 4
        case _:
            return 5

print("critical: " + str(get_priority("critical")))
print("normal: " + str(get_priority("normal")))
print("unknown: " + str(get_priority("maybe")))

# === Temperature Converter ===

def convert_temp(value, unit):
    match unit.upper():
        case "C":
            # Celsius to Fahrenheit
            f = (value * 9/5) + 32
            return str(value) + "C = " + str(f) + "F"
        case "F":
            # Fahrenheit to Celsius
            c = (value - 32) * 5/9
            return str(value) + "F = " + str(c) + "C"
        case _:
            return "Unknown unit"

print(convert_temp(100, "C"))
print(convert_temp(212, "F"))
