# Example298: Assertions and Debugging
def divide(a, b):
    assert b != 0, "Cannot divide by zero"
    return a / b

print("Assertions:")
print(divide(10, 2))

try:
    divide(10, 0)
except AssertionError as e:
    print(f"Caught: {e}")

# Debug with breakpoint (Python 3.7+)
def buggy(x):
    result = x * 2
    # breakpoint() would open debugger
    return result

print("\nFunction with debug:")
print(f"Result: {buggy(5)}")
