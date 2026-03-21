# Example88.py
# Topic: Pattern Matching — Common Mistakes with Literals and Types

# Common mistakes when matching literals and types

# === MISTAKE 1: Forgetting quotes on strings ===

# WRONG - no quotes means variable reference
# match status:
#     case yes:  # Looks for variable 'yes', not string "yes"
#         print("Yes")

# CORRECT - use quotes for string literals
status = "yes"

match status:
    case "yes":
        print("Matched string 'yes'")
    case _:
        print("No match")


# === MISTAKE 2: Case sensitivity ===

# WRONG - "Yes" != "yes"
response = "Yes"

match response:
    case "yes":
        print("Matched lowercase")
    case "Yes":
        print("Matched capitalized")
    case _:
        print("No match - case matters!")

# CORRECT - match exact case or use multiple options
response2 = "yes"

match response2:
    case "yes" | "Yes" | "YES":
        print("Any case of yes!")
    case _:
        print("Not yes")


# === MISTAKE 3: Wrong type pattern syntax ===

# WRONG - this is not how to check type
# val = "hello"
# match val:
#     case str:  # This looks for variable 'str', not type check
#         print("String")

# CORRECT - use parentheses for type patterns
val = "hello"

match val:
    case str():
        print("String type: " + val)
    case int():
        print("Integer")
    case _:
        print("Other")


# === MISTAKE 4: Not handling all types ===

# WRONG - what if value is a float?
number = 3.14

# match number:
#     case 3.14:  # This is a literal match, but floats can vary
#         print("Exactly pi")

# CORRECT - use type matching for numbers that could vary
match number:
    case int():
        print("Integer: " + str(number))
    case float():
        print("Float: " + str(number))
    case _:
        print("Other type")


# === MISTAKE 5: Guard order matters ===

# WRONG - put specific conditions first
x = 10

match x:
    case int():
        print("Any integer")  # This catches ALL integers first!
    case int() if x > 5:
        print("Integer > 5")  # Never reached!


# CORRECT - put more specific guards first
y = 10

match y:
    case int() if y > 5:
        print("Integer > 5: " + str(y))
    case int():
        print("Integer <= 5: " + str(y))
    case _:
        print("Not an integer")


# === MISTAKE 6: Matching None incorrectly ===

# WRONG - "None" is a string, None is a keyword
result = None

match result:
    case "None":  # This is the string "None", not None
        print("String 'None'")
    case None:  # This is the keyword None
        print("None value")
    case _:
        print("Other value")


# === MISTAKE 7: Comparing without guard ===

# WRONG - case patterns are for matching, not comparing
# age = 25
# match age:
#     case age > 18:  # This doesn't work as expected
#         print("Adult")

# CORRECT - use guards for comparisons
age = 25

match age:
    case int() if age >= 18:
        print("Adult (age " + str(age) + ")")
    case int():
        print("Minor (age " + str(age) + ")")
    case _:
        print("Not a valid age")


# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Use quotes for string literals: "text"
# 2. Be case sensitive: "Yes" != "yes"
# 3. Use parentheses for type patterns: str()
# 4. Order cases from specific to general
# 5. Use guards (if) for comparisons
# 6. Handle None with None keyword, not "None"
# 7. Always include wildcard case
