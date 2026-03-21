# Example99.py
# Topic: Pattern Matching — Common Mistakes with Guards and Wildcards

# Common mistakes when using guards and wildcards

# === MISTAKE 1: Wildcard not as last case ===

# WRONG - wildcard before specific patterns
value = 42

# match value:
#     case _:
#         print("Catch all")
#     case 42:  # Never reached!
#         print("Forty two")

# CORRECT - wildcard at end
value2 = 42

match value2:
    case 42:
        print("Forty two")
    case _:
        print("Other: " + str(value2))


# === MISTAKE 2: Guard without pattern ===

# WRONG - guard without any pattern
# x = 5
# match x:
#     case if x > 0:  # Syntax error! Needs a pattern
#         print("Positive")

# CORRECT - use pattern with guard
x = 5

match x:
    case int() if x > 0:
        print("Positive: " + str(x))
    case _:
        print("Other: " + str(x))


# === MISTAKE 3: Guard matching wrong type ===

# WRONG - assumes all values are integers
age = "thirty"

match age:
    case int() if age > 18:  # Error! Can't compare string to int
        print("Adult")
    case _:
        print("Not an adult")


# CORRECT - handle type first
age2 = "thirty"

match age2:
    case int() as n if n > 18:
        print("Adult: " + str(n))
    case int():
        print("Minor: " + str(age2))
    case _:
        print("Not an integer: " + age2)


# === MISTAKE 4: Forgetting wildcard entirely ===

# WRONG - no default case (will error on unmatched)
# status = "unknown"
# match status:
#     case "active":
#         print("Active")
#     case "pending":
#         print("Pending")
#     # No wildcard - will raise MatchError!

# CORRECT - always include wildcard
status = "unknown"

match status:
    case "active":
        print("Active")
    case "pending":
        print("Pending")
    case _:
        print("Unknown: " + status)


# === MISTAKE 5: Guard evaluation order ===

# WRONG - more specific guard after general
number = 25

match number:
    case int() if number > 20:  # This matches first!
        print("Greater than 20")
    case int() if number > 10:  # Never reached!
        print("Greater than 10")
    case _:
        print("Other")


# CORRECT - order from specific to general
number2 = 25

match number2:
    case int() if number2 > 10 and number2 <= 20:
        print("Between 11-20")
    case int() if number2 > 20:
        print("Greater than 20")
    case int() if number2 > 0:
        print("1-10")
    case _:
        print("Other")


# === MISTAKE 6: Using == instead of guard ===

# WRONG - trying to use == inside case
# fruit = "apple"
# match fruit:
#     case "apple" == "fruit":  # Wrong syntax!
#         print("Apple")

# CORRECT - use guard with pattern
fruit = "apple"

match fruit:
    case "apple" if fruit == "apple":
        print("It's an apple")
    case "banana":
        print("It's a banana")
    case _:
        print("Other fruit")


# === MISTAKE 7: Complex guard that could be pattern ===

# WRONG - guard does work that pattern should do
data = "hello"

match data:
    case str() if data.startswith("h"):
        print("Starts with h")
    case str() if data.startswith("w"):
        print("Starts with w")
    case _:
        print("Other")


# CORRECT - use pattern directly
data2 = "hello"

match data2:
    case str() as s if s.startswith("h"):
        print("Starts with h: " + s)
    case str() as s:
        print("Other: " + s)


# === MISTAKE 8: Wildcard with wrong position in sequence ===

# WRONG - wildcard in middle of list pattern
# items = [1, 2, 3]
# match items:
#     case [_, 2, _]:  # Can't use _ in middle like this
#         print("Middle is 2")

# CORRECT - use specific positions
items = [1, 2, 3]

match items:
    case [first, 2, last]:
        print("Middle is 2: " + str(first) + ", " + str(last))
    case _:
        print("Other")


# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Always put wildcard (_) as the LAST case
# 2. Guards need a pattern - can't stand alone
# 3. Check type before applying guard conditions
# 4. Always include wildcard to catch unmatched cases
# 5. Order patterns from specific to general
# 6. Use guards for conditions, patterns for structure
# 7. Don't overuse guards when patterns can do the job
