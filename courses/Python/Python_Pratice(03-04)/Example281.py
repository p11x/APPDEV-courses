# Example281: Enum Basics
from enum import Enum, auto

class Color(Enum):
    RED = 1
    GREEN = 2
    BLUE = 3

print("Enum Basics:")
print(f"Color.RED: {Color.RED}")
print(f"Color.RED.value: {Color.RED.value}")
print(f"Color.RED.name: {Color.RED.name}")

# Iteration
print("\nIteration:")
for color in Color:
    print(f"  {color.name} = {color.value}")

# Auto numbering
class Status(Enum):
    PENDING = auto()
    APPROVED = auto()
    REJECTED = auto()

print("\nAuto:")
for s in Status:
    print(f"  {s.name} = {s.value}")

# Compare
print("\nComparison:")
print(f"Color.RED == Color.RED: {Color.RED == Color.RED}")
print(f"Color.RED is Color.RED: {Color.RED is Color.RED}")

# Enum with methods
class Operation(Enum):
    ADD = lambda x, y: x + y
    SUB = lambda x, y: x - y
    MUL = lambda x, y: x * y

print("\nWith functions:")
print(f"ADD(5, 3): {Operation.ADD.value(5, 3)}")
