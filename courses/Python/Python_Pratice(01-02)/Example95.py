# Example95.py
# Topic: Pattern Matching — Common Mistakes with Mappings

# Common mistakes when matching dictionaries

# === MISTAKE 1: Using wrong bracket type ===

# WRONG - using [] instead of {}
# data = {"x": 1}
# match data:
#     case [x]:  # This matches lists, not dicts!
#         print(x)

# CORRECT - use {} for dictionaries
data = {"x": 1}

match data:
    case {"x": x}:
        print("X = " + str(x))
    case _:
        print("No match")


# === MISTAKE 2: Not handling extra keys ===

# WRONG - fails if dict has extra keys
config = {"host": "localhost", "port": 8080, "extra": "value"}

# match config:
#     case {"host": h, "port": p}:  # Fails! extra key exists
#         print(h, p)

# CORRECT - use ** to capture extra keys
config2 = {"host": "localhost", "port": 8080, "extra": "value"}

match config2:
    case {"host": h, "port": p, **rest}:
        print("Host: " + h + ", Port: " + str(p))
        print("Extra: " + str(rest))
    case _:
        print("No match")


# === MISTAKE 3: Forgetting to handle empty dict ===

# WRONG - no handling for empty dict
# empty = {}
# match empty:
#     case {"key": value}:  # Only matches non-empty!
#         print(value)

# CORRECT - handle empty case
empty = {}

match empty:
    case {}:
        print("Empty dictionary")
    case {"key": value}:
        print("Has key: " + str(value))
    case _:
        print("Other")


# === MISTAKE 4: Case sensitivity with string keys ===

# WRONG - keys are case sensitive
user = {"Name": "Alice"}  # Note capital N

match user:
    case {"name": name}:  # Won't match!
        print("Found: " + name)
    case _:
        print("No match - case sensitive!")


# CORRECT - match exact case
user2 = {"Name": "Alice"}

match user2:
    case {"Name": name}:
        print("Found: " + name)
    case _:
        print("No match")


# === MISTAKE 5: Confusing sequence and mapping patterns ===

# WRONG - this is a sequence pattern
point = [10, 20]

match point:
    case {"x": x, "y": y}:  # This won't match!
        print("Dict")
    case [a, b]:
        print("List: " + str(a) + ", " + str(b))
    case _:
        print("No match")


# CORRECT - use correct pattern type
point2 = {"x": 10, "y": 20}

match point2:
    case {"x": x, "y": y}:
        print("Dict: x=" + str(x) + ", y=" + str(y))
    case [a, b]:
        print("List")
    case _:
        print("No match")


# === MISTAKE 6: Not using ** for unknown keys ===

# WRONG - must match all keys exactly
person = {"name": "Bob", "age": 30, "city": "NYC"}

# match person:
#     case {"name": name, "age": age}:  # Fails! city is extra
#         print(name, age)

# CORRECT - use **
match person:
    case {"name": name, "age": age, **rest}:
        print(name + " is " + str(age))
        print("Other: " + str(rest))
    case _:
        print("No match")


# === MISTAKE 7: Nested pattern order ===

# WRONG - order matters in nested patterns
nested = {"outer": {"inner": "value"}}

# This works, but being specific helps
match nested:
    case {"outer": {"inner": val}}:
        print("Found: " + val)
    case _:
        print("No match")


# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Use {} for dictionaries, not []
# 2. Use ** to capture extra keys you don't need
# 3. Handle empty dict case explicitly
# 4. Keys are case sensitive
# 5. Match pattern type to data type (dict vs list)
# 6. Always include wildcard case _ for safety
# 7. Use specific keys when possible, ** for the rest
