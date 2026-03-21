# Example84.py
# Topic: Pattern Matching — Common Mistakes

# Common mistakes with match statements

# === MISTAKE 1: Forgetting wildcard case ===

# Without wildcard - must handle all cases or get error
status = "unknown"

# In Python 3.10+, this would raise MatchError if no case matches
# match status:
#     case "active":
#         print("Active")
#     # No wildcard - would error on "unknown"!

# CORRECT - always include wildcard
match status:
    case "active":
        print("Active")
    case "inactive":
        print("Inactive")
    case _:
        print("Unknown status")

# === MISTAKE 2: Using 'or' instead of '|' ===

# WRONG - 'or' doesn't work in case
# match value:
#     case 1 or 2:  # Syntax error!
#         print("One or two")

# CORRECT - use |
match 1:
    case 1 | 2:
        print("Matched: 1 or 2")
    case _:
        print("Other")

# === MISTAKE 3: Using == instead of | ===

# WRONG - doesn't work as expected
# match value:
#     case 1 == 2:  # Wrong!
#         pass

# CORRECT - use |
match 3:
    case 1 | 2 | 3:
        print("Matched 1, 2, or 3")
    case _:
        print("Other")

# === MISTAKE 4: Not handling all cases ===

# WRONG - what if value is something else?
# match color:
#     case "red":
#         print("Red")
#     case "blue":
#         print("Blue")
#     # Missing default!

# CORRECT - always use wildcard
color = "green"

match color:
    case "red":
        print("Red")
    case "blue":
        print("Blue")
    case _:
        print("Other color: " + color)

# === Best practices ===
print("\n=== Best Practices ===")

# 1. Always include wildcard case
# 2. Use | for OR patterns
# 3. Order specific to general
# 4. Keep cases simple

# Example: Good match structure
def process_status(status):
    match status:
        case "success":
            return "All good!"
        case "error" | "fail":
            return "Something went wrong"
        case _:
            return "Unknown status"


print(process_status("success"))
print(process_status("error"))
print(process_status("other"))
