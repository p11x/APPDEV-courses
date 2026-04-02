# Match Statements (Python 3.10+)

## What You'll Learn

- The `match`/`case` statement (structural pattern matching)
- Literal patterns, capture patterns, wildcard patterns
- OR patterns, class patterns, and guard clauses
- When to use match instead of if/elif

## Prerequisites

- Read [01_if_elif_else.md](./01_if_elif_else.md) first

## What Is Match?

The `match` statement (introduced in Python 3.10) is **structural pattern matching**. It's like a super-powered switch statement from other languages, but much more powerful.

### Simple Example

```python
# Using if/elif/else
command: str = "quit"

if command == "start":
    print("Starting...")
elif command == "stop":
    print("Stopping...")
elif command == "quit":
    print("Quitting...")
else:
    print("Unknown command")

# Using match (cleaner!)
match command:
    case "start":
        print("Starting...")
    case "stop":
        print("Stopping...")
    case "quit":
        print("Quitting...")
    case _:
        print("Unknown command")  # _ is the wildcard (matches anything)
```

## Pattern Types

### 1. Literal Patterns

Match exact values:

```python
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
```

### 2. Capture Patterns

Capture the matched value into a variable:

```python
command: str = "help"

match command:
    case "quit":
        print("Goodbye!")
    case "help":
        print("Available commands...")
    # Capture pattern - matches anything and stores it
    case user_command:  # This matches anything!
        print(f"Unknown command: {user_command}")
```

### 3. Wildcard Pattern (`_`)

Matches anything but doesn't capture it:

```python
value: str = "anything"

match value:
    case "specific":
        print("Matched specific")
    case _:  # Wildcard - matches anything
        print("Matched anything else")
```

### 4. OR Patterns (`|`)

Match multiple values:

```python
day: str = "saturday"

match day:
    case "monday" | "tuesday" | "wednesday" | "thursday" | "friday":
        print("Weekday")
    case "saturday" | "sunday":
        print("Weekend")
    case _:
        print("Not a day")
```

### 5. Class Patterns

Match object structure:

```python
# Define a simple class
class Point:
    def __init__(self, x: int, y: int) -> None:
        self.x = x
        self.y = y


# Create a point
p: Point = Point(0, 0)

# Match on class structure
match p:
    case Point(x=0, y=0):
        print("Origin point")
    case Point(x=0, y=_):
        print("On Y-axis")
    case Point(x=_, y=0):
        print("On X-axis")
    case Point(x, y):
        print(f"Point at ({x}, {y})")
```

### 6. Sequence Patterns

Match lists and tuples:

```python
# Match a list
data: list = [1, 2, 3]

match data:
    case []:
        print("Empty list")
    case [1, 2, 3]:
        print("Exact match: [1, 2, 3]")
    case [first, second]:  # Capture first two elements
        print(f"Two elements: {first}, {second}")
    case [first, *rest]:  # Capture rest as list
        print(f"First: {first}, Rest: {rest}")
    case _:
        print("Something else")
```

### 7. Guard Clauses

Add extra conditions with `if`:

```python
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
```

## Annotated Example: Command Parser

```python
# command_parser.py
# A command-line command parser using match statements

from dataclasses import dataclass


# Define data classes for structured data
@dataclass
class UserCommand:
    """Represents a user command with arguments."""
    action: str
    args: list[str]


def parse_command(raw_input: str) -> str:
    """Parse and execute a command string.
    
    Args:
        raw_input: The command string from user
    
    Returns:
        Description of what was executed
    """
    # Split input into action and arguments
    # "create file.txt" → ["create", "file.txt"]
    parts: list[str] = raw_input.split()
    
    # Handle empty input
    if not parts:
        return "No command entered"
    
    # Get the action (first word)
    action: str = parts[0].lower()
    
    # Use match to handle different commands
    match action:
        # quit command - literal pattern
        case "quit" | "exit" | "q":
            return "Exiting program..."
        
        # help command
        case "help":
            return """
Available commands:
  help              - Show this message
  create <filename> - Create a file
  delete <filename> - Delete a file
  list              - List all files
  quit/exit/q       - Exit the program
""".strip()
        
        # create command with argument
        case "create":
            match parts:
                # Pattern: ["create", filename]
                case ["create", filename]:
                    return f"Creating file: {filename}"
                case _:
                    return "Usage: create <filename>"
        
        # delete command
        case "delete":
            match parts:
                case ["delete", filename]:
                    return f"Deleting file: {filename}"
                case _:
                    return "Usage: delete <filename>"
        
        # list command - no arguments needed
        case "list":
            return "Listing files: file1.txt, file2.txt, file3.txt"
        
        # Unknown command - capture pattern
        case unknown:
            return f"Unknown command: {unknown}"


def main() -> None:
    # Test commands
    test_commands: list[str] = [
        "help",
        "create myfile.txt",
        "delete oldfile.txt",
        "list",
        "quit",
        "make coffee",
        "",
    ]
    
    print("=== Command Parser Demo ===\n")
    
    for cmd in test_commands:
        result: str = parse_command(cmd)
        print(f"Input: '{cmd}'")
        print(f"Output: {result}")
        print("-" * 40)


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== Command Parser Demo ===

Input: 'help'
Output: Available commands:
  help              - Show this message
  create <filename> - Create a file
  delete <filename> - Delete a file
  list              - List all files
  quit/exit/q       - Exit the program
----------------------------------------
Input: 'create myfile.txt'
Output: Creating file: myfile.txt
----------------------------------------
Input: 'delete oldfile.txt'
Output: Deleting file: oldfile.txt
----------------------------------------
Input: 'list'
Output: Listing files: file1.txt, file2.txt, file3.txt
----------------------------------------
Input: 'quit'
Output: Exiting program...
----------------------------------------
Input: 'make coffee'
Output: Unknown command: make coffee
----------------------------------------
Input: ''
Output: No command entered
----------------------------------------
```

## When to Use Match vs If/Elif

| Use Match When | Use If/Elif When |
|----------------|------------------|
| Comparing one value against many options | Multiple different conditions |
| Pattern matching (structures, sequences) | Complex boolean expressions |
| Replacing complex if/elif chains | Conditions that don't share a common subject |
| Clean, readable dispatch logic | Simple true/false checks |

## Summary

- **`match`/`case`**: Python 3.10+ structural pattern matching
- **Literal patterns**: Match exact values (`case 200:`)
- **Capture patterns**: Capture matched value (`case x:`)
- **Wildcard (`_`)**: Matches anything
- **OR patterns**: Match multiple values (`case "a" | "b":`)
- **Class patterns**: Match object structure
- **Guard clauses**: Add conditions with `if`

## Next Steps

Now let's learn about **truthiness and falsiness** in **[03_truthiness_and_falsy.md](./03_truthiness_and_falsy.md)**.
