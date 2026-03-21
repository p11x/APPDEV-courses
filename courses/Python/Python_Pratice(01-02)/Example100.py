# Example100.py
# Topic: Pattern Matching — Basic Class Patterns

# Match class instances using class patterns

# === Basic class pattern ===
# Create a simple class to match

class Point:
    __match_args__ = ("x", "y")
    
    def __init__(self, x, y):
        self.x = x
        self.y = y


point = Point(10, 20)

match point:
    case Point(x, y):
        print("Point at (" + str(x) + ", " + str(y) + ")")
    case _:
        print("Not a Point")


# === Match with specific values ===
class Status:
    __match_args__ = ("name", "code")
    
    def __init__(self, name, code):
        self.name = name
        self.code = code


status = Status("active", 200)

match status:
    case Status("active", 200):
        print("Success status")
    case Status("error", 500):
        print("Error status")
    case Status(name, code):
        print("Status: " + name + ", code: " + str(code))
    case _:
        print("Unknown")


# === Match with type check ===
class Rectangle:
    __match_args__ = ("width", "height")
    
    def __init__(self, width, height):
        self.width = width
        self.height = height


class Circle:
    __match_args__ = ("radius",)
    
    def __init__(self, radius):
        self.radius = radius


shape = Rectangle(10, 5)

match shape:
    case Rectangle(w, h):
        print("Rectangle: " + str(w) + "x" + str(h))
    case Circle(r):
        print("Circle with radius " + str(r))
    case _:
        print("Unknown shape")


# === Match different shapes ===
circle = Circle(7)

match circle:
    case Rectangle(w, h):
        print("Rectangle")
    case Circle(radius):
        print("Circle radius: " + str(radius))
    case _:
        print("Unknown")


# === Class pattern with conditions ===
class User:
    __match_args__ = ("name", "role")
    
    def __init__(self, name, role):
        self.name = name
        self.role = role


user = User("Alice", "admin")

match user:
    case User("admin", "admin"):
        print("Super admin")
    case User(name, "admin"):
        print("Admin user: " + name)
    case User(name, role):
        print("User: " + name + ", role: " + role)
    case _:
        print("Unknown user")


# === Using type() in match ===
# Note: datetime and date don't have __match_args__, use keyword patterns
from datetime import datetime, date

now = datetime.now()
today = date.today()

match now:
    case datetime():
        print("Datetime: " + str(now.year) + "-" + str(now.month) + "-" + str(now.day))
    case date():
        print("Date: " + str(today.year) + "-" + str(today.month) + "-" + str(today.day))
    case _:
        print("Not a date/datetime")


# === Practical: Shape area calculator ===
class Square:
    __match_args__ = ("side",)
    
    def __init__(self, side):
        self.side = side


class Triangle:
    __match_args__ = ("base", "height")
    
    def __init__(self, base, height):
        self.base = base
        self.height = height


def get_area(shape):
    match shape:
        case Square(side):
            return side * side
        case Rectangle(w, h):
            return w * h
        case Circle(r):
            return 3.14 * r * r
        case Triangle(base, height):
            return 0.5 * base * height
        case _:
            return 0


square = Square(5)
rect = Rectangle(4, 6)
circ = Circle(3)
tri = Triangle(10, 5)

print("Square area: " + str(get_area(square)))
print("Rectangle area: " + str(get_area(rect)))
print("Circle area: " + str(get_area(circ)))
print("Triangle area: " + str(get_area(tri)))


# === Class pattern with None check ===
class Config:
    __match_args__ = ("value",)
    
    def __init__(self, value):
        self.value = value


config = Config(None)

match config:
    case Config(None):
        print("No config value")
    case Config(val):
        print("Config value: " + str(val))
    case _:
        print("Not a config")


# === Nested class patterns ===
class Address:
    __match_args__ = ("city", "country")
    
    def __init__(self, city, country):
        self.city = city
        self.country = country


class Person:
    __match_args__ = ("name", "address")
    
    def __init__(self, name, address):
        self.name = name
        self.address = address


person = Person("Alice", Address("NYC", "USA"))

match person:
    case Person(name, Address(city, country)):
        print(name + " lives in " + city + ", " + country)
    case _:
        print("Unknown person")
