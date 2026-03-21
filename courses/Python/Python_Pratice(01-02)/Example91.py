# Example91.py
# Topic: Pattern Matching — Common Mistakes with Sequences

# Common mistakes when matching sequences (lists/tuples)

# === MISTAKE 1: Wrong brackets ===

# WRONG - using () instead of [] for lists
# point = (10, 20)
# match point:
#     case (x, y):  # This matches tuples, not lists!
#         print(x, y)

# CORRECT - use [] for lists
point = [10, 20]

match point:
    case [x, y]:
        print("Point: " + str(x) + ", " + str(y))
    case _:
        print("Not a point")


# === MISTAKE 2: Order matters ===

# WRONG - wrong order of patterns
data = [1, 2]

match data:
    case [a]:  # This won't match [1, 2]
        print("One element")
    case [a, b, c]:  # This also won't match
        print("Three elements")
    case _:
        print("No match - wrong patterns!")

# CORRECT - order from specific to general
data2 = [1, 2]

match data2:
    case [a, b]:
        print("Two elements: " + str(a) + ", " + str(b))
    case [a]:
        print("One element: " + str(a))
    case _:
        print("Other")


# === MISTAKE 3: Not handling empty sequences ===

# WRONG - doesn't handle empty list
items = []

match items:
    case [first, second]:
        print("Two items")
    case [first]:
        print("One item")
    # No wildcard - would error!


# CORRECT - always handle empty case
items2 = []

match items2:
    case []:
        print("Empty list")
    case [first, second]:
        print("Two items")
    case [first]:
        print("One item")
    case _:
        print("Many items")


# === MISTAKE 4: Mixing tuple and list syntax ===

# WRONG - mixing () and []
pair = (1, 2)

match pair:
    case [a, b]:  # Won't match tuple!
        print("List pattern")
    case _:
        print("No match - wrong type")

# CORRECT - match tuple with tuple
pair2 = (1, 2)

match pair2:
    case (a, b):
        print("Tuple: " + str(a) + ", " + str(b))
    case _:
        print("Not a tuple")


# === MISTAKE 5: Forgetting to capture all elements ===

# WRONG - not handling extra elements
coords = [1, 2, 3, 4, 5]

match coords:
    case [x, y]:  # Only captures first 2!
        print("Point: " + str(x) + ", " + str(y))


# CORRECT - use * to capture rest or be specific
coords2 = [1, 2, 3, 4, 5]

match coords2:
    case [x, y, *rest]:
        print("First two: " + str(x) + ", " + str(y))
        print("Rest: " + str(rest))
    case _:
        print("No match")


# === MISTAKE 6: Not using type patterns with mixed types ===

# WRONG - assumes all elements are same type
# This would cause TypeError at runtime if b is not a string
# mixed = ["hello", 123, "world"]
# match mixed:
#     case [a, b, c]:
#         print(a + " " + b + " " + c)  # Error! b is int


# CORRECT - use type patterns
mixed2 = ["hello", "world"]

match mixed2:
    case [str() as a, str() as b]:
        print(a + " " + b)
    case _:
        print("Not two strings")


# === MISTAKE 7: Forgetting wildcard for unmatched ===

# WRONG - no default case
status = "unknown"

# match status:
#     case [a, b]:  # This only matches lists!
#         print("List")


# CORRECT - always include wildcard
status2 = "something"

match status2:
    case [a, b]:
        print("List")
    case (a, b):
        print("Tuple")
    case _:
        print("Not a sequence")


# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Use [] for lists, () for tuples
# 2. Order patterns from specific to general
# 3. Always handle empty sequences
# 4. Use * to capture remaining elements
# 5. Use type patterns for mixed types
# 6. Always include wildcard case
