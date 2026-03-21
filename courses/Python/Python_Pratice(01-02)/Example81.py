# Example81.py
# Topic: Exception Handling — ExceptionGroups Common Mistakes

# Common mistakes when working with exception groups

# === MISTAKE 1: Not handling ExceptionGroup ===

# WRONG - treating as single exception
# try:
#     risky_code()
# except Exception as e:
#     print(e)  # Only shows top-level message!

# CORRECT - check for ExceptionGroup
# In Python 3.11+:
# except ExceptionGroup as eg:
#     print(eg.exceptions)  # Get individual exceptions

# === MISTAKE 2: Forgetting to iterate ===

# WRONG - not accessing .exceptions
# except* ValueError as eg:
#     print(eg)  # Shows group, not individual errors!

# CORRECT - iterate through exceptions
# except* ValueError as eg:
#     for e in eg.exceptions:
#         print(f"Error: {e}")

# === Traditional way (compatible with all versions) ===
def handle_errors(items):
    errors = []
    
    for item in items:
        try:
            if item < 0:
                raise ValueError(f"Invalid: {item}")
            print(f"Processed: {item}")
        except ValueError as e:
            errors.append(e)
    
    return errors


# Test
print("=== Handling Multiple Errors ===")
errors = handle_errors([1, 2, -1, 3, -2])

print("Errors found:")
for e in errors:  # Iterate through individual errors
    print("  - " + str(e))

# === MISTAKE 3: Using wrong exception type ===

# WRONG - catching wrong type
# try:
#     raise ExceptionGroup("test", [ValueError()])
# except TypeError:
#     pass  # Won't catch ValueError!

# CORRECT - catch the right exception type

# === MISTAKE 4: Not checking Python version ===

# ExceptionGroup requires Python 3.11+
import sys

print("\nPython version: " + sys.version)

if sys.version_info >= (3, 11):
    print("ExceptionGroup is available!")
else:
    print("Use traditional error collection for compatibility")

# === Best practice summary ===
print("\n=== Best Practices ===")
print("1. Check Python version for ExceptionGroup")
print("2. Iterate through eg.exceptions")
print("3. Use traditional approach for compatibility")
print("4. Choose right approach for your use case")
