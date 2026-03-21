# Example8.py
# Topic: Comprehensive Function Example - Calculator

from typing import Optional

# This comprehensive example brings together multiple function concepts:
# basic operations, return values, default arguments, keyword arguments,
# error handling, and chaining function calls for complex calculations.


# Returns the sum of two floating-point numbers
def add(a: float, b: float) -> float:
    return a + b


# Returns the difference of two floating-point numbers
def subtract(a: float, b: float) -> float:
    return a - b


# Returns the product of two floating-point numbers
def multiply(a: float, b: float) -> float:
    return a * b


# Divides two numbers, handling division by zero safely
def divide(a: float, b: float) -> Optional[float]:
    if b == 0:
        print("Cannot divide by zero!")
        return None
    return a / b


# Raises a base to an exponent power
def power(base: float, exponent: float) -> float:
    return base ** exponent


# Returns the remainder when dividing two integers
def modulo(a: int, b: int) -> int:
    return a % b


# Performs integer division, handling division by zero safely
def floor_divide(a: float, b: float) -> Optional[float]:
    if b == 0:
        print("Cannot divide by zero!")
        return None
    return a // b


# Universal calculator function that performs different operations based on parameter
# Uses default argument "add" if no operation is specified
def calculate(a: float, b: float, operation: str = "add") -> Optional[float]:
    match operation:
        case "add":
            return add(a, b)
        case "subtract":
            return subtract(a, b)
        case "multiply":
            return multiply(a, b)
        case "divide":
            return divide(a, b)
        case "power":
            return power(a, b)
        case "modulo":
            return modulo(int(a), int(b))
        case "floor_divide":
            return floor_divide(a, b)
        case _:
            print(f"Unknown operation: {operation}")
            return None


def main() -> None:
    # Demonstrate basic arithmetic operations using individual functions
    print("=== Basic Operations ===")
    a = 10.0    # float — first operand for basic operations
    b = 5.0    # float — second operand for basic operations
    
    print(f"{a} + {b} = {add(a, b)}")
    print(f"{a} - {b} = {subtract(a, b)}")
    print(f"{a} * {b} = {multiply(a, b)}")
    print(f"{a} / {b} = {divide(a, b)}")
    
    # Show how default arguments work with the calculate function
    print("\n=== Using Default Arguments ===")
    result = calculate(100, 20)    # uses default operation "add"
    print(f"Default (add): {result}")
    
    result = calculate(100, 20, operation="subtract")    # specify different operation
    print(f"Subtract: {result}")
    
    result = calculate(100, 20, operation="multiply")
    print(f"Multiply: {result}")
    
    result = calculate(100, 20, operation="divide")
    print(f"Divide: {result}")
    
    # Demonstrate keyword arguments - can specify parameters by name in any order
    print("\n=== Using Keyword Arguments ===")
    result = calculate(a=100, b=20, operation="add")    # all parameters named
    print(f"With keywords: {result}")
    
    result = calculate(b=10, a=100, operation="power")    # different order
    print(f"Power: {result}")
    
    result = calculate(operation="modulo", a=17, b=5)    # operation specified as keyword
    print(f"Modulo: {result}")
    
    # Loop through all supported operations to show versatility
    print("\n=== Different Operations ===")
    operations = ["add", "subtract", "multiply", "divide", "power", "modulo"]    # list — operations to test
    x = 8.0    # float — first value for operation testing
    y = 3.0    # float — second value for operation testing
    
    for op in operations:
        result = calculate(x, y, op)
        print(f"{x} {op} {y} = {result}")
    
    # Demonstrate error handling with division by zero
    print("\n=== Handling Errors ===")
    result = divide(10.0, 0.0)    # directly call divide with zero
    print(f"Division by zero: {result}")
    
    result = calculate(10, 0, "divide")    # call calculate with zero divisor
    print(f"Calculate division by zero: {result}")
    
    # Show how the function handles unknown operations
    print("\n=== Unknown Operation ===")
    result = calculate(10, 5, "invalid")    # invalid operation name
    print(f"Invalid operation result: {result}")
    
    # Chain multiple function calls together for complex calculations
    print("\n=== Chain Calculations ===")
    result1 = add(10, 5)    # float — step 1: 10 + 5 = 15
    result2 = multiply(result1, 2)    # float — step 2: 15 * 2 = 30
    result3 = subtract(result2, 10)    # float — step 3: 30 - 10 = 20
    result4 = divide(result3, 4)    # float — step 4: 20 / 4 = 5
    print(f"Chain: ((10 + 5) * 2 - 10) / 4 = {result4}")
    
    # Complex nested expression showing function composition
    print("\n=== Complex Expression ===")
    expr = add(
        multiply(2, 10),
        divide(100, subtract(50, 25))
    )
    print(f"(2 * 10) + (100 / (50 - 25)) = {expr}")
    
    # Display a menu of supported operations
    print("\n=== Calculator Menu ===")
    print("Supported operations:")
    print("  - add       : Addition")
    print("  - subtract  : Subtraction")
    print("  - multiply : Multiplication")
    print("  - divide   : Division")
    print("  - power    : Exponentiation")
    print("  - modulo   : Remainder")
    print("  - floor_divide : Integer division")


if __name__ == "__main__":
    main()
