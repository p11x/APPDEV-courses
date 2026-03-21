# Example102.py
# Topic: Pattern Matching — Keyword Patterns in Classes

# Match class instances using keyword patterns

# === Basic keyword pattern ===
class User:
    def __init__(self, name, role):
        self.name = name
        self.role = role


user = User("Alice", "admin")

match user:
    case User(name="Alice", role="admin"):
        print("Admin Alice")
    case User(name="Bob", role="user"):
        print("User Bob")
    case User(name=name, role=role):
        print("User " + name + " with role " + role)
    case _:
        print("Unknown user")


# === Keyword pattern with type matching ===
class Rectangle:
    def __init__(self, width, height):
        self.width = width
        self.height = height


rect = Rectangle(10, 5)

match rect:
    case Rectangle(width=w, height=h) if w == h:
        print("Square: " + str(w) + "x" + str(h))
    case Rectangle(width=w, height=h):
        print("Rectangle: " + str(w) + "x" + str(h))
    case _:
        print("Not a rectangle")


# === Keyword pattern - partial match ===
class Config:
    def __init__(self, host, port, debug=False):
        self.host = host
        self.port = port
        self.debug = debug


config = Config("localhost", 8080, debug=True)

match config:
    case Config(host="localhost", port=8080):
        print("Local server")
    case Config(host=host, port=port):
        print("Server at " + host + ":" + str(port))
    case _:
        print("Unknown config")


# === Keyword pattern with defaults ===
class Server:
    def __init__(self, name, port=80, ssl=False):
        self.name = name
        self.port = port
        self.ssl = ssl


server = Server("web", ssl=True)

match server:
    case Server(name="web", port=80, ssl=False):
        print("HTTP server")
    case Server(name="web", ssl=True):
        print("HTTPS server")
    case Server(name=name):
        print("Server: " + name)
    case _:
        print("Unknown server")


# === Keyword pattern - different order ===
class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y


point = Point(5, 10)

# Can match with keyword args in any order
match point:
    case Point(y=10, x=5):
        print("Point at (5, 10)")
    case Point(x=x, y=y):
        print("Point(" + str(x) + ", " + str(y) + ")")
    case _:
        print("Not a point")


# === Keyword pattern with nested class ===
class Address:
    def __init__(self, city, country):
        self.city = city
        self.country = country


class Person:
    def __init__(self, name, city, country):
        self.name = name
        self.city = city
        self.country = country


person = Person("Alice", "NYC", "USA")

match person:
    case Person(name="Alice", city="NYC", country="USA"):
        print("Alice in NYC, USA")
    case Person(name=name, city=city, country=country):
        print(name + " in " + city + ", " + country)
    case _:
        print("Unknown")


# === Practical: API endpoint handler ===
class Endpoint:
    def __init__(self, path, method="GET", auth_required=True):
        self.path = path
        self.method = method
        self.auth_required = auth_required


endpoint = Endpoint("/api/users", "POST", auth_required=True)

match endpoint:
    case Endpoint(path="/api/users", method="GET", auth_required=True):
        print("Get users (auth required)")
    case Endpoint(path="/api/users", method="POST", auth_required=True):
        print("Create user (auth required)")
    case Endpoint(path="/api/users", method="GET"):
        print("Get users")
    case Endpoint(path=path, method=method):
        print(method + " " + path)
    case _:
        print("Unknown endpoint")


# === Keyword pattern - capturing extra ===
# Note: **rest is not supported in class patterns, use keyword only
class Response:
    def __init__(self, status, body, headers=None):
        self.status = status
        self.body = body
        self.headers = headers or {}


response = Response(200, "OK", headers={"X-Custom": "value"})

match response:
    case Response(status=200, body=body, headers=headers):
        print("Success: " + body)
        print("Headers: " + str(headers))
    case Response(status=status, body=body):
        print("Response " + str(status) + ": " + body)
    case _:
        print("Unknown response")


# === Combining positional and keyword ===
class Container:
    __match_args__ = ("type", "value", "label")
    
    def __init__(self, type, value, label=""):
        self.type = type
        self.value = value
        self.label = label


container = Container("box", 42, label="Answer")

match container:
    case Container("box", value, label=""):
        print("Box with value " + str(value))
    case Container("box", value, label=label):
        print("Box '" + label + "': " + str(value))
    case Container(type, value, label=label):
        print(type + " '" + label + "': " + str(value))
    case _:
        print("Unknown container")


# === When to use keyword patterns ===
# 1. When class doesn't have __match_args__
# 2. When you want to match specific attributes only
# 3. When attribute order might change
# 4. For readability with many attributes
