# Example36.py
# Topic: Control Flow — Match Patterns Deep Dive

# This covers more advanced match pattern types:
# - Capture patterns
# - Wildcard pattern (_)
# - OR patterns (|)
# - Sequence patterns
# - Class patterns

# === Wildcard Pattern (_) ===
# Matches anything but doesn't capture it

value = "anything"

match value:
    case "specific":
        print("Matched specific")
    case _:
        print("Matched anything else")  # This catches everything else

# The wildcard doesn't store the value, just matches
# Use it as the "default" case

# === Capture Pattern ===
# Matches anything AND stores it in a variable

command = "delete"

match command:
    case "quit":
        print("Goodbye!")
    case "help":
        print("Showing help...")
    case user_command:
        # This catches ALL other cases and stores in user_command
        print("Unknown command: " + user_command)

# === OR Patterns ===
# Match multiple values in one case

status = "pending"

match status:
    case "active" | "running" | "online":
        print("System is operational")
    case "pending" | "starting":
        print("System is initializing")
    case "stopped" | "offline" | "error":
        print("System is not operational")
    case _:
        print("Unknown status")

# === Sequence Patterns ===
# Match lists and tuples

data = [1, 2, 3]

match data:
    case []:
        print("Empty list")
    case [1, 2, 3]:
        print("Exact match: [1, 2, 3]")
    case [first, second]:
        print("Two elements: " + str(first) + ", " + str(second))
    case _:
        print("Something else")

# Capture first element and rest
data = [1, 2, 3, 4, 5]

match data:
    case [first, *rest]:
        print("First element: " + str(first))
        print("Rest of list: " + str(rest))
    case _:
        print("Not a list")

# Match specific starting elements
data = ["create", "myfile.txt"]

match data:
    case ["create", filename]:
        print("Creating file: " + filename)
    case ["delete", filename]:
        print("Deleting file: " + filename)
    case ["list"]:
        print("Listing files")
    case _:
        print("Unknown command")

# === Guard Clauses ===
# Add extra conditions with 'if'

number = 15

match number:
    case x if x < 0:
        print("Negative number")
    case x if x % 2 == 0:
        print("Even number: " + str(x))
    case x if x % 3 == 0:
        print("Multiple of 3: " + str(x))
    case x:
        print("Other number: " + str(number))

# === Guard with comparison ===
score = 85

match score:
    case s if s >= 90:
        print("A grade")
    case s if s >= 80:
        print("B grade")
    case s if s >= 70:
        print("C grade")
    case s if s >= 60:
        print("D grade")
    case _:
        print("F grade")

# === Real-world example: Command with arguments ===
command = ["send", "alice@example.com", "Hello!"]

match command:
    case ["send", to, message]:
        print("To: " + to)
        print("Message: " + message)
    case ["receive"]:
        print("Receiving message...")
    case _:
        print("Unknown command")

# === Real-world example: Point coordinates ===
# Simulating a point (normally you'd use a class)
point = [5, 0]

match point:
    case [0, 0]:
        print("Origin point")
    case [0, y]:
        print("On Y-axis at y=" + str(y))
    case [x, 0]:
        print("On X-axis at x=" + str(x))
    case [x, y]:
        print("Point at (" + str(x) + ", " + str(y) + ")")

# === Real-world example: API response parsing ===
response = ["error", 500, "Database connection failed"]

match response:
    case ["success", data]:
        print("Success! Data: " + str(data))
    case ["error", code, message]:
        print("Error " + str(code) + ": " + message)
    case ["warning", message]:
        print("Warning: " + message)
    case _:
        print("Unknown response format")

# === Combining patterns ===
action = "update"
item_id = 42

match [action, item_id]:
    case ["create", id] if id > 0:
        print("Creating item " + str(id))
    case ["update", id] if id > 0:
        print("Updating item " + str(id))
    case ["delete", id] if id > 0:
        print("Deleting item " + str(id))
    case ["create", _]:
        print("Invalid: ID must be positive for create")
    case _:
        print("Unknown action")
