# Example80.py
# Topic: Exception Handling — When to Use ExceptionGroups

# When to use ExceptionGroups vs traditional try/except

# === When to use ExceptionGroups ===
# - Running multiple tasks in parallel
# - Collecting all errors
# - Async/concurrent code

# === When to use traditional try/except ===
# - Single operation that might fail
# - Stopping on first error
# - Sequential code

# === Example: Sequential vs Parallel ===

# Traditional approach (stop on first error)
def process_sequential(items):
    results = []
    for item in items:
        if item < 0:
            raise ValueError(f"Invalid: {item}")
        results.append(item * 2)
    return results


# Collect all errors approach
def process_all_errors(items):
    errors = []
    results = []
    
    for item in items:
        try:
            if item < 0:
                raise ValueError(f"Invalid: {item}")
            results.append(item * 2)
        except ValueError as e:
            errors.append(e)
    
    return results, errors


# Test sequential (stops at first error)
print("=== Sequential (stops at first error) ===")
try:
    result = process_sequential([1, 2, -3, 4, 5])
except ValueError as e:
    print("Error: " + str(e))

# Test collecting errors
print("\n=== Collect All Errors ===")
results, errors = process_all_errors([1, 2, -3, 4, -5])
print("Results: " + str(results))
print("Errors: " + str(len(errors)))
for e in errors:
    print("  - " + str(e))

# === When to use each approach ===

# Use traditional try/except when:
# - You need to stop on first error
# - Simple, sequential logic
# - Early return on error

# Use error collection when:
# - Processing multiple independent items
# - Want to report all issues at once
# - Parallel processing

# === Comparison table ===
print("\n=== Comparison ===")
print("Traditional try/except:")
print("  - Use for: Simple validation, single operation")
print("  - Stops at first error")
print("  - Easier to read")

print("\nError collection:")
print("  - Use for: Multiple items, form validation")
print("  - Reports all errors")
print("  - More complex but comprehensive")
