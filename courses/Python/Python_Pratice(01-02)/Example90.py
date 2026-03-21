# Example90.py
# Topic: Pattern Matching — Capture Rest with *

# Use * to capture remaining elements in a sequence

# === Capture rest with * ===
# Get first element and capture all remaining elements

numbers = [1, 2, 3, 4, 5]

match numbers:
    case [first, *rest]:
        print("First: " + str(first))
        print("Rest: " + str(rest))
    case _:
        print("No match")


# === Capture first two and rest ===
data = [10, 20, 30, 40, 50]

match data:
    case [a, b, *c]:
        print("a=" + str(a) + ", b=" + str(b) + ", c=" + str(c))
    case _:
        print("Not enough elements")


# === Capture last element with * ===
items = ["apple", "banana", "cherry", "date"]

match items:
    case [*first, last]:
        print("First items: " + str(first))
        print("Last item: " + last)
    case _:
        print("No match")


# === Capture middle elements ===
values = [1, 2, 3, 4, 5, 6]

match values:
    case [1, *middle, 6]:
        print("Starts with 1, ends with 6")
        print("Middle: " + str(middle))
    case _:
        print("Pattern doesn't match")


# === Practical: Command with arguments ===
cmd = ["run", "server", "--port", "8080", "--debug"]

match cmd:
    case ["run", name, *args]:
        print("Running: " + name)
        print("Arguments: " + str(args))
    case _:
        print("Unknown command")


# === Practical: Function parameters ===
params = ["width", "100", "height", "200", "color", "blue"]

match params:
    case [key, value, *rest]:
        print("First pair: " + key + "=" + value)
        print("Remaining: " + str(rest))
    case _:
        print("No params")


# === Capture nothing when not enough elements ===
short = [1]

match short:
    case [first, *rest]:
        print("First: " + str(first))
        print("Rest: " + str(rest))
    case _:
        print("Empty rest captured as []")


# === Rest with empty list ===
empty = []

match empty:
    case [*rest]:
        print("Empty list - rest is: " + str(rest))
    case _:
        print("No match")


# === Rest with type matching ===
mixed = ["hello", 1, 2, 3]

match mixed:
    case [str() as s, *nums]:
        print("String: " + s)
        print("Numbers: " + str(nums))
    case _:
        print("No match")


# === Practical: URL parser ===
url = ["https", "example.com", "8080", "path/to/resource"]

match url:
    case [protocol, host, *parts]:
        print("Protocol: " + protocol)
        print("Host: " + host)
        print("Other parts: " + str(parts))
    case _:
        print("Invalid URL")


# === Nested with rest ===
nested = [[1, 2], [3, 4], [5, 6]]

match nested:
    case [[a, b], *rest]:
        print("First pair: " + str(a) + ", " + str(b))
        print("Rest of pairs: " + str(rest))
    case _:
        print("No match")
