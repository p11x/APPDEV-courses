# Example101.py
# Topic: Pattern Matching — Positional Patterns with __match_args__

# Use __match_args__ to define positional arguments for class patterns

# === Basic __match_args__ ===
class Point:
    __match_args__ = ("x", "y")
    
    def __init__(self, x, y):
        self.x = x
        self.y = y


point = Point(10, 20)

match point:
    case Point(10, y):
        print("X is 10, Y is " + str(y))
    case Point(x, y):
        print("Point(" + str(x) + ", " + str(y) + ")")
    case _:
        print("Not a Point")


# === Multiple points with match_args ===
class Color:
    __match_args__ = ("red", "green", "blue")
    
    def __init__(self, r, g, b):
        self.red = r
        self.green = g
        self.blue = b


color = Color(255, 128, 0)

match color:
    case Color(0, 0, 0):
        print("Black")
    case Color(255, 255, 255):
        print("White")
    case Color(r, g, 0):
        print("No blue, R=" + str(r) + ", G=" + str(g))
    case Color(r, g, b):
        print("RGB(" + str(r) + ", " + str(g) + ", " + str(b) + ")")
    case _:
        print("Not a Color")


# === Real-world: HTTP response ===
class HTTPResponse:
    __match_args__ = ("status", "body", "headers")
    
    def __init__(self, status, body, headers=None):
        self.status = status
        self.body = body
        self.headers = headers or {}


response = HTTPResponse(200, "OK", {"Content-Type": "text/html"})

match response:
    case HTTPResponse(200, body, _):
        print("Success: " + body)
    case HTTPResponse(404, body, _):
        print("Not Found: " + body)
    case HTTPResponse(500, _, _):
        print("Server Error")
    case HTTPResponse(status, body, _):
        print("Status " + str(status) + ": " + body)


# === Class without __match_args__ (uses keyword patterns only) ===
class Product:
    # No __match_args__ - must use keyword patterns
    def __init__(self, name, price, quantity):
        self.name = name
        self.price = price
        self.quantity = quantity


product = Product("Laptop", 999.99, 2)

match product:
    case Product(name="Laptop", price=price, quantity=qty):
        print("Laptop: $" + str(price) + " x " + str(qty))
    case Product(name=name, price=price, quantity=qty):
        print(name + ": $" + str(price) + " x " + str(qty))
    case _:
        print("Not a product")


# === __match_args__ with specific values ===
class Event:
    __match_args__ = ("type", "timestamp", "data")
    
    def __init__(self, event_type, timestamp, data=None):
        self.type = event_type
        self.timestamp = timestamp
        self.data = data


event = Event("click", 1234567890, {"x": 100, "y": 200})

match event:
    case Event("click", ts, {"x": x, "y": y}):
        print("Click at (" + str(x) + ", " + str(y) + ") at " + str(ts))
    case Event("keydown", ts, {"key": key}):
        print("Key " + key + " at " + str(ts))
    case Event("resize", ts, {"width": w, "height": h}):
        print("Resize to " + str(w) + "x" + str(h))
    case _:
        print("Unknown event")


# === Nested classes with match_args ===
class Name:
    __match_args__ = ("first", "last")
    
    def __init__(self, first, last):
        self.first = first
        self.last = last


class Person:
    __match_args__ = ("name", "age")
    
    def __init__(self, name, age):
        self.name = name
        self.age = age


person = Person(Name("John", "Doe"), 30)

match person:
    case Person(Name("John", last), age):
        print("John Doe, age " + str(age))
    case Person(Name(first, last), age):
        print(first + " " + last + ", age " + str(age))
    case _:
        print("Unknown person")


# === Dataclass-like pattern ===
# Note: In Python 3.10+, dataclasses work with match_args automatically
from dataclasses import dataclass

@dataclass
class Order:
    order_id: int
    status: str
    total: float


order = Order(123, "shipped", 99.99)

match order:
    case Order(123, "shipped", total):
        print("Order 123 shipped: $" + str(total))
    case Order(id, "pending", total):
        print("Order " + str(id) + " pending: $" + str(total))
    case Order(id, status, total):
        print("Order " + str(id) + " (" + status + "): $" + str(total))
    case _:
        print("Not an order")


# === Benefits of __match_args__ ===
# 1. Clearer pattern matching syntax
# 2. Works without __init__ parameter names
# 3. Can reorder or exclude attributes
# 4. More efficient than attribute access
