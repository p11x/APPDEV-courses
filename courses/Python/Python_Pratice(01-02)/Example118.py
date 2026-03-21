# Example118.py
# Topic: Iteration Tools — Common Mistakes with Any and All

# Common mistakes when using any() and all()

# === MISTAKE 1: Forgetting empty returns False for any() ===

# WRONG - any([]) is False
result = any([])
print("any([]): " + str(result))

# CORRECT - handle empty case if needed
items = []
if len(items) > 0 and any(condition for item in items):
    print("Has matching items")
else:
    print("No items or no matches")

# === MISTAKE 2: Forgetting all([]) returns True ===

# WRONG - all([]) is True (vacuous truth!)
# This can cause bugs if you're not expecting it
result = all([])
print("all([]): " + str(result))

# CORRECT - handle empty case if needed
items = []
if len(items) > 0 and all(condition for item in items):
    print("All meet condition")
else:
    print("No items or not all meet condition")

# === MISTAKE 3: Not using generator expression ===

# WRONG - creates full list in memory
numbers = range(1000000)
# result = any([x > 1000000 for x in numbers])  # Creates huge list!

# CORRECT - use generator expression
result = any(x > 1000000 for x in numbers)
print("\nGenerator (efficient): " + str(result))

# === MISTAKE 4: Using any() instead of all() ===

# WRONG - checking all but using any
scores = [85, 90, 95, 100]

# This is wrong - checks if ANY is >= 90
# but we want to check if ALL are >= 90
any_above_90 = any(s >= 90 for s in scores)
print("\nAny >= 90: " + str(any_above_90))  # True

# CORRECT - use all if you want ALL
all_above_90 = all(s >= 90 for s in scores)
print("All >= 90: " + str(all_above_90))  # False

# === MISTAKE 5: Using all() instead of any() ===

# WRONG - checking any but using all
numbers = [1, 2, 3, 4, 5]

# This is wrong - checks if ALL are > 3
# but we want to check if ANY is > 3
all_greater = all(n > 3 for n in numbers)
print("\nAll > 3: " + str(all_greater))  # False

# CORRECT - use any if you want ANY
any_greater = any(n > 3 for n in numbers)
print("Any > 3: " + str(any_greater))  # True

# === MISTAKE 6: Confusing truthy with True ===

# WRONG - any() returns truthy value, not necessarily True
numbers = [0, 0, 1]
result = any(numbers)  # Returns 1, which is truthy but not True

# This might fail in strict boolean checks
if result == True:
    print("Exact True")
else:
    print("Truthy but not True: " + str(result))

# CORRECT - let Python handle truthiness
if any(numbers):
    print("Using truthiness: works!")

# === MISTAKE 7: Not handling None values ===

# WRONG - None in list can cause issues
data = [1, 2, None, 4]

# any() handles None fine
print("\nWith None:")
print("any: " + str(any(data)))  # True

# But check if any is None specifically
print("any is None: " + str(any(x is None for x in data)))

# === MISTAKE 8: Forgetting parentheses with generator ===

# WRONG - missing parentheses
# any(x > 0 for x in numbers)  # Correct!

# This is wrong without generator:
# any(x > 0 for x in numbers)  # OK
# any(x > 0 for x in numbers)  # Also OK

# Just make sure it's a generator expression
numbers = [1, 2, 3]
result = any(x > 0 for x in numbers)
print("\nGenerator works: " + str(result))

# === MISTAKE 9: Using with dict values incorrectly ===

# WRONG - checking dict directly
d = {"a": 0, "b": 0, "c": 0}

# This checks keys, not values!
print("\nDict checks keys:")
print("any(d): " + str(any(d)))  # True (keys exist)
print("all(d): " + str(all(d)))  # True

# CORRECT - check values
print("any(d.values()): " + str(any(d.values())))  # False
print("all(d.values()): " + str(all(d.values())))  # False

# === MISTAKE 10: Not short-circuiting properly ===

# These both work but have different efficiency:
# any() stops at first True
# all() stops at first False

# If checking expensive operations, this matters
def expensive_check(x):
    print("Checking " + str(x))
    return x > 2

values = [1, 2, 3]

print("\nShort-circuit test:")
print("any: ", end="")
any(expensive_check(v) for v in values)
print("all: ", end="")
all(expensive_check(v) for v in values)

# === Best Practices ===
print("\n=== Best Practices ===")

# 1. any([]) = False, all([]) = True (remember this!)
# 2. Use generator expressions for memory efficiency
# 3. Use any() for "at least one" checks
# 4. Use all() for "every single one" checks
# 5. Let Python handle truthiness, don't compare to True
# 6. Use .values() when checking dict values
# 7. Both short-circuit - useful for expensive checks
