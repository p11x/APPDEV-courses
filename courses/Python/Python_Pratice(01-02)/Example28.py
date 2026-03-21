# Example28.py
# Topic: Operators — Short-Circuit Evaluation

# Short-circuit evaluation means Python stops checking as soon as it knows the answer
# This makes your code faster and can prevent errors

# === Short-circuit with 'and' ===
# If the FIRST value is False, Python doesn't check the second
# Why? False AND anything = False, so no need to evaluate further

# False is first — second condition is never checked
result = False and 5 / 0   # 5/0 would cause error, but it's skipped!
print(result)              # False

# True is first — must check second to get the answer
result = True and 5
print(result)              # 5  (returns the actual value, not just True)

# === Short-circuit with 'or' ===
# If the FIRST value is True, Python doesn't check the second
# Why? True OR anything = True, so no need to evaluate further

# True is first — second condition is never checked
result = True or 5 / 0     # 5/0 would cause error, but it's skipped!
print(result)              # True

# False is first — must check second to get the answer
result = False or 5
print(result)              # 5  (returns the actual value, not just True)

# === Practical use: safe division ===
# Only divide if divisor is not zero
divisor = 0

# If divisor is 0, the division is never attempted
result = divisor != 0 and 10 / divisor
print(result)              # False (short-circuited, no division by zero!)

divisor = 2
result = divisor != 0 and 10 / divisor
print(result)              # 5.0 (division happened normally)

# === Practical use: default values ===
# If name is None or empty, use "Guest" instead
name = ""

# Short-circuit returns name if it's truthy, otherwise returns "Guest"
greeting = name or "Guest"

print(name)        # empty string
print(greeting)    # Guest

name = "Alice"
greeting = name or "Guest"
print(name)        # Alice
print(greeting)    # Alice

# === Practical use: checking list is not empty ===
# Before accessing elements, check if list has items
items = []

# If list is empty, don't try to access items[0]
first_item = items and items[0]
print(first_item)  # [] (empty list, short-circuited)

items = ["apple", "banana"]
first_item = items and items[0]
print(first_item)  # apple

# === Real-world example: permission system ===
# User must be logged in AND have admin role to see settings
is_logged_in = False
is_admin = True  # This would cause error if checked, but isn't!

# Short-circuit: if not logged in, don't even check admin status
can_see_settings = is_logged_in and is_admin

print(is_logged_in)      # False
print(can_see_settings) # False

# === Real-world example: feature flag with fallback ===
# Check if a feature is enabled, use default if not defined
feature_enabled = False
default_value = "default"

# If feature_enabled is False, return default immediately
result = feature_enabled or default_value

print(result)           # default

feature_enabled = "custom value"
result = feature_enabled or default_value
print(result)           # custom value

# === Real-world example: chain of fallbacks ===
# Try multiple options, use first one that works
primary_server = None
secondary_server = None
fallback_server = "https://backup.example.com"

# Returns first non-None/non-empty value
server = primary_server or secondary_server or fallback_server

print(server)           # https://backup.example.com
