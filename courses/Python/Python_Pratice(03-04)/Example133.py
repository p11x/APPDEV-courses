# Example133.py
# Topic: Enum and Constants


# ============================================================
# Example 1: Basic Enum
# ============================================================
print("=== Basic Enum ===")

from enum import Enum

class Color(Enum):
    RED = 1
    GREEN = 2
    BLUE = 3

print(f"Color.RED: {Color.RED}")
print(f"Color.RED.name: {Color.RED.name}")
print(f"Color.RED.value: {Color.RED.value}")


# ============================================================
# Example 2: Enum Iteration
# ============================================================
print("\n=== Enum Iteration ===")

from enum import Enum

class Status(Enum):
    PENDING = "pending"
    ACTIVE = "active"
    COMPLETED = "completed"

for status in Status:
    print(f"  {status.name} = {status.value}")


# ============================================================
# Example 3: Enum Comparison
# ============================================================
print("\n=== Enum Comparison ===")

from enum import Enum

class Priority(Enum):
    LOW = 1
    MEDIUM = 2
    HIGH = 3

p1 = Priority.LOW
p2 = Priority.HIGH

print(f"p1 == p2: {p1 == p2}")
print(f"p1 < p2: {p1 < p2}")
print(f"Priority.HIGH == 'HIGH': {Priority.HIGH == 'HIGH'}")


# ============================================================
# Example 4: IntEnum
# ============================================================
print("\n=== IntEnum ===")

from enum import IntEnum

class HTTPStatus(IntEnum):
    OK = 200
    NOT_FOUND = 404
    INTERNAL_ERROR = 500

print(f"Status: {HTTPStatus.OK}")
print(f"As int: {int(HTTPStatus.OK)}")
print(f"OK == 200: {HTTPStatus.OK == 200}")


# ============================================================
# Example 5: Enum with Methods
# ============================================================
print("\n=== Enum with Methods ===")

from enum import Enum

class Level(Enum):
    DEBUG = 1
    INFO = 2
    WARNING = 3
    ERROR = 4
    
    @property
    def is_error(self):
        return self.value >= 4
    
    @classmethod
    def from_string(cls, s):
        return cls[s.upper()]

print(f"Level.ERROR.is_error: {Level.ERROR.is_error}")
print(f"Level.INFO.is_error: {Level.INFO.is_error}")
print(f"from_string('warning'): {Level.from_string('warning')}")


# ============================================================
# Example 6: Flag Enum
# ============================================================
print("\n=== Flag Enum ===")

from enum import Flag, auto

class Permission(Flag):
    READ = auto()
    WRITE = auto()
    EXECUTE = auto()

read_write = Permission.READ | Permission.WRITE
print(f"READ | WRITE: {read_write}")
print(f"Has READ: {Permission.READ in read_write}")
print(f"Has EXECUTE: {Permission.EXECUTE in read_write}")


# ============================================================
# Example 7: Real-World: State Machine
# ============================================================
print("\n=== Real-World: Order State ===")

from enum import Enum, auto

class OrderState(Enum):
    CREATED = auto()
    CONFIRMED = auto()
    SHIPPED = auto()
    DELIVERED = auto()
    CANCELLED = auto()

    def next(self):
        transitions = {
            OrderState.CREATED: OrderState.CONFIRMED,
            OrderState.CONFIRMED: OrderState.SHIPPED,
            OrderState.SHIPPED: OrderState.DELIVERED,
        }
        return transitions.get(self)

state = OrderState.CREATED
print(f"Initial: {state.name}")
state = state.next()
print(f"After transition: {state.name}")
