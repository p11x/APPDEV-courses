# Example147.py
# Topic: Match Statements (Python 3.10+)


# ============================================================
# Example 1: Basic Match Statement
# ============================================================
print("=== Basic Match ===")

command: str = "quit"

match command:
    case "start":
        print("Starting...")
    case "stop":
        print("Stopping...")
    case "quit":
        print("Quitting...")
    case _:
        print("Unknown command")


# ============================================================
# Example 2: Literal Patterns
# ============================================================
print("\n=== Literal Patterns ===")

status_code: int = 200

match status_code:
    case 200:
        print("OK - Success")
    case 404:
        print("Not Found")
    case 500:
        print("Server Error")
    case _:
        print(f"Unknown code: {status_code}")


# ============================================================
# Example 3: Capture Patterns
# ============================================================
print("\n=== Capture Patterns ===")

command: str = "help"

match command:
    case "quit":
        print("Goodbye!")
    case "help":
        print("Available commands...")
    case user_command:
        print(f"Unknown command: {user_command}")


# ============================================================
# Example 4: Wildcard Pattern
# ============================================================
print("\n=== Wildcard Pattern ===")

value: str = "anything"

match value:
    case "specific":
        print("Matched specific")
    case _:
        print("Matched anything else")


# ============================================================
# Example 5: OR Patterns
# ============================================================
print("\n=== OR Patterns ===")

day: str = "saturday"

match day:
    case "monday" | "tuesday" | "wednesday" | "thursday" | "friday":
        print("Weekday")
    case "saturday" | "sunday":
        print("Weekend")
    case _:
        print("Not a day")


# ============================================================
# Example 6: Sequence Patterns
# ============================================================
print("\n=== Sequence Patterns ===")

data: list = [1, 2, 3]

match data:
    case []:
        print("Empty list")
    case [1, 2, 3]:
        print("Exact match: [1, 2, 3]")
    case [first, second]:
        print(f"Two elements: {first}, {second}")
    case [first, *rest]:
        print(f"First: {first}, Rest: {rest}")
    case _:
        print("Something else")


# ============================================================
# Example 7: Guard Clauses
# ============================================================
print("\n=== Guard Clauses ===")

number: int = 15

match number:
    case x if x < 0:
        print("Negative")
    case x if x % 2 == 0:
        print(f"Even number: {x}")
    case x if x % 3 == 0:
        print(f"Multiple of 3: {x}")
    case x:
        print(f"Other number: {x}")


# ============================================================
# Example 8: Real-World: Command Parser
# ============================================================
print("\n=== Command Parser ===")

def parse_command(raw_input: str) -> str:
    parts: list[str] = raw_input.split()
    
    if not parts:
        return "No command entered"
    
    action: str = parts[0].lower()
    
    match action:
        case "quit" | "exit" | "q":
            return "Exiting program..."
        
        case "help":
            return "Available commands: help, create, delete, list, quit"
        
        case "create":
            match parts:
                case ["create", filename]:
                    return f"Creating file: {filename}"
                case _:
                    return "Usage: create <filename>"
        
        case "delete":
            match parts:
                case ["delete", filename]:
                    return f"Deleting file: {filename}"
                case _:
                    return "Usage: delete <filename>"
        
        case "list":
            return "Listing files..."
        
        case unknown:
            return f"Unknown command: {unknown}"

test_commands: list[str] = [
    "help",
    "create myfile.txt",
    "delete oldfile.txt",
    "list",
    "quit",
    "make coffee",
]

for cmd in test_commands:
    result: str = parse_command(cmd)
    print(f"Input: '{cmd}' → Output: {result}")
