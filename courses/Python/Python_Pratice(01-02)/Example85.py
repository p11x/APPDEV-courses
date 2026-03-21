# Example85.py
# Topic: Pattern Matching — Literal Matching

# Match exact values with literals

# === Matching exact string values ===
# Match checks for exact equality

response = "yes"

match response:
    case "yes":
        print("You agreed!")
    case "no":
        print("You declined")
    case "maybe":
        print("You're unsure")


# === Matching multiple literals with | ===
choice = "y"

match choice:
    case "y" | "yes" | "Y" | "YES":
        print("Affirmative!")
    case "n" | "no" | "N" | "NO":
        print("Negative!")
    case _:
        print("Invalid choice")


# === Matching exact numbers ===
http_code = 404

match http_code:
    case 200:
        print("OK - Success")
    case 201:
        print("Created")
    case 301:
        print("Redirect - Moved permanently")
    case 404:
        print("Not Found")
    case 500:
        print("Server Error")
    case _:
        print("Unknown code: " + str(http_code))


# === Exact matching - case sensitive ===
status = "Active"

match status:
    case "active":
        print("Lowercase matched")
    case "Active":
        print("Capitalized matched")
    case "ACTIVE":
        print("Uppercase matched")
    case _:
        print("No match - case matters!")


# === Matching booleans ===
is_valid = True

match is_valid:
    case True:
        print("Valid - true")
    case False:
        print("Invalid - false")


# === Matching None ===
result = None

match result:
    case None:
        print("No result")
    case _:
        print("Got result")


# === Practical: Command parser ===
command = "quit"

match command:
    case "start":
        print("Starting process...")
    case "stop":
        print("Stopping process...")
    case "restart":
        print("Restarting process...")
    case "quit":
        print("Quitting...")
    case _:
        print("Unknown command")
