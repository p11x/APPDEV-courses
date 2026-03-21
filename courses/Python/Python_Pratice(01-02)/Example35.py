# Example35.py
# Topic: Control Flow — Match Statements (Python 3.10+)

# Match statement is structural pattern matching
# It's like a super-powered switch statement
# Available in Python 3.10 and later

# === Match vs if/elif ===
# Same logic, different syntax

# Using if/elif/else
command = "quit"

if command == "start":
    print("Starting...")
elif command == "stop":
    print("Stopping...")
elif command == "quit":
    print("Quitting...")
else:
    print("Unknown command")

# Using match — cleaner!
command = "quit"

match command:
    case "start":
        print("Starting...")
    case "stop":
        print("Stopping...")
    case "quit":
        print("Quitting...")
    case _:
        print("Unknown command")

# The underscore _ is a wildcard that matches anything

# === Literal Patterns ===
# Match exact values (like status codes)

status_code = 200

match status_code:
    case 200:
        print("OK - Success")
    case 404:
        print("Not Found")
    case 500:
        print("Server Error")
    case _:
        print("Unknown code: " + str(status_code))

# === Multiple values with OR pattern ===
# Use | to match multiple values

day = "saturday"

match day:
    case "monday" | "tuesday" | "wednesday" | "thursday" | "friday":
        print("Weekday")
    case "saturday" | "sunday":
        print("Weekend")
    case _:
        print("Not a day")

# === Capture Patterns ===
# Captures the matched value into a variable

command = "help"

match command:
    case "quit":
        print("Goodbye!")
    case "help":
        print("Available commands...")
    case user_command:
        print("Unknown command: " + user_command)

# === Real-world example: HTTP response handler ===
response_code = 404

match response_code:
    case 200:
        print("Success - Data received")
    case 201:
        print("Created - Resource successfully created")
    case 204:
        print("No Content - Request succeeded, no data to return")
    case 400:
        print("Bad Request - Invalid syntax")
    case 401:
        print("Unauthorized - Authentication required")
    case 403:
        print("Forbidden - Access denied")
    case 404:
        print("Not Found - Resource doesn't exist")
    case 500:
        print("Internal Server Error - Something went wrong on the server")
    case _:
        print("Unknown error code: " + str(response_code))

# === Real-world example: Menu selection ===
menu_choice = "3"

match menu_choice:
    case "1":
        print("New Game selected")
    case "2":
        print("Load Game selected")
    case "3":
        print("Settings selected")
    case "4":
        print("Quit selected")
    case _:
        print("Invalid menu choice")

# === Real-world example: Color conversion ===
color = "red"

match color:
    case "red":
        print("RGB: (255, 0, 0)")
    case "green":
        print("RGB: (0, 255, 0)")
    case "blue":
        print("RGB: (0, 0, 255)")
    case "white":
        print("RGB: (255, 255, 255)")
    case "black":
        print("RGB: (0, 0, 0)")
    case _:
        print("Unknown color")
