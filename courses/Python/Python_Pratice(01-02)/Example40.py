# Example40.py
# Topic: Control Flow — Complex Pattern Matching

# Advanced pattern matching scenarios with match statements

# === Nested Match Statements ===

# Parse a command with subcommands
command = ["git", "commit", "-m", "Fixed bug"]

match command:
    case ["git", subcmd]:
        match subcmd:
            case "commit":
                print("Git commit")
            case "push":
                print("Git push")
            case "pull":
                print("Git pull")
            case "status":
                print("Git status")
            case _:
                print("Unknown git command")
    case ["docker", subcmd]:
        match subcmd:
            case "run":
                print("Docker run")
            case "build":
                print("Docker build")
            case "ps":
                print("Docker ps")
            case _:
                print("Unknown docker command")
    case _:
        print("Unknown command")

# === Matching JSON-like Structures ===

# Simulating JSON data
data = {"type": "user", "name": "Alice", "age": 25}

# Match on the type field
data_type = data.get("type", "")

match data_type:
    case "user":
        name = data.get("name", "Unknown")
        age = data.get("age", 0)
        print("User: " + name + ", age " + str(age))
    case "product":
        name = data.get("name", "Unknown")
        price = data.get("price", 0)
        print("Product: " + name + ", $" + str(price))
    case "order":
        order_id = data.get("id", "N/A")
        total = data.get("total", 0)
        print("Order #" + str(order_id) + ", total $" + str(total))
    case _:
        print("Unknown data type")

# === Matching with Multiple Conditions ===

# Shape processor
def process_shape(shape):
    match shape:
        case {"type": "circle", "radius": r}:
            area = 3.14159 * r * r
            print("Circle with radius " + str(r) + ", area = " + str(area))
        case {"type": "rectangle", "width": w, "height": h}:
            area = w * h
            print("Rectangle " + str(w) + "x" + str(h) + ", area = " + str(area))
        case {"type": "triangle", "base": b, "height": h}:
            area = 0.5 * b * h
            print("Triangle base " + str(b) + ", height " + str(h) + ", area = " + str(area))
        case _:
            print("Unknown shape")

process_shape({"type": "circle", "radius": 5})
process_shape({"type": "rectangle", "width": 4, "height": 6})

# === Matching Optional Fields ===

person = {"name": "Bob", "email": "bob@example.com"}

match person:
    case {"name": name, "email": email, "phone": phone}:
        print(name + " - Email: " + email + ", Phone: " + phone)
    case {"name": name, "email": email}:
        print(name + " - Email: " + email + " (no phone)")
    case {"name": name}:
        print(name + " (no email)")
    case _:
        print("Invalid person")

# === Matching with Rest Pattern ===

# Get first and rest
numbers = [1, 2, 3, 4, 5]

match numbers:
    case [first, second, *rest]:
        print("First: " + str(first))
        print("Second: " + str(second))
        print("Rest: " + str(rest))

# Match exactly 3 elements
coords = [10, 20, 30]

match coords:
    case [x, y, z]:
        print("3D point: (" + str(x) + ", " + str(y) + ", " + str(z) + ")")
    case [x, y]:
        print("2D point: (" + str(x) + ", " + str(y) + ")")
    case _:
        print("Invalid coordinates")

# === Complex Guard Conditions ===

def classify_number(n):
    match n:
        case x if x < 0 and abs(x) % 2 == 1:
            return "Negative odd"
        case x if x < 0:
            return "Negative even"
        case x if x > 0 and x % 2 == 1:
            return "Positive odd"
        case x if x > 0:
            return "Positive even"
        case _:
            return "Zero"

print(classify_number(-5))   # Negative odd
print(classify_number(-4))   # Negative even
print(classify_number(7))    # Positive odd
print(classify_number(10))   # Positive even
print(classify_number(0))    # Zero

# === Matching Date Components ===

date = [2024, 3, 15]

match date:
    case [year, month, day] if month == 2 and day > 28:
        print("Invalid February date")
    case [year, 12, 25]:
        print("Christmas!")
    case [year, month, day] if month in [6, 7, 8]:
        print("Summer: " + str(day) + "/" + str(month))
    case [year, month, day]:
        print(str(day) + "/" + str(month) + "/" + str(year))

# === URL Parser ===

url = "https://example.com/path/to/page"

# Simple URL parsing
match url.split("://"):
    case [protocol, rest]:
        match rest.split("/"):
            case [host, *path]:
                print("Protocol: " + protocol)
                print("Host: " + host)
                print("Path: /" + "/".join(path))
            case [host]:
                print("Protocol: " + protocol)
                print("Host: " + host)
                print("Path: /")
    case _:
        print("Invalid URL")

# === State Machine with Match ===

state = "IDLE"

def next_state(current, event):
    match [current, event]:
        case ["IDLE", "start"]:
            return "RUNNING"
        case ["RUNNING", "pause"]:
            return "PAUSED"
        case ["RUNNING", "stop"]:
            return "IDLE"
        case ["PAUSED", "resume"]:
            return "RUNNING"
        case ["PAUSED", "stop"]:
            return "IDLE"
        case _:
            return current

state = next_state(state, "start")
print("After start: " + state)

state = next_state(state, "pause")
print("After pause: " + state)

state = next_state(state, "resume")
print("After resume: " + state)
