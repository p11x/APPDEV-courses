# Example78.py
# Topic: Exception Handling — Using except*

# except* handles exception groups (Python 3.11+)

# === Basic except* usage ===
import sys

if sys.version_info < (3, 11):
    print("ExceptionGroup requires Python 3.11+")
else:
    # Create an exception group
    eg = ExceptionGroup("Multiple errors", [
        ValueError("Invalid value"),
        TypeError("Wrong type"),
        ValueError("Another value error"),
    ])
    
    print("Created exception group")
    
    # Note: To actually raise it, we'd need to use: raise eg

# === except* syntax ===
# try:
#     # Code that might raise ExceptionGroup
# except* SpecificException as eg:
#     # Handle this type of exception
#     for e in eg.exceptions:
#         print(e)

# === Simulating without actual exception ===
# Since we can't easily raise ExceptionGroup in older Python

# Traditional approach for compatibility:
def handle_errors():
    # Collect multiple errors
    errors = []
    
    # Simulate validation
    errors.append(ValueError("Invalid value"))
    errors.append(TypeError("Wrong type"))
    
    if errors:
        # Could use ExceptionGroup in 3.11+
        return errors
    return None


result = handle_errors()
if result:
    print("Found " + str(len(result)) + " errors:")
    for e in result:
        print("  - " + str(e))

# === Traditional way (works in all versions) ===
def process_all(items):
    errors = []
    results = []
    
    for item in items:
        try:
            # Simulate processing
            if item < 0:
                raise ValueError("Negative value")
            results.append(item * 2)
        except ValueError as e:
            errors.append(e)
    
    return results, errors


items = [1, -2, 3, -4, 5]
results, errors = process_all(items)

print("Results: " + str(results))
print("Errors: " + str(len(errors)))
for e in errors:
    print("  - " + str(e))
