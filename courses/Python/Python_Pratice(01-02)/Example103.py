# Example103.py
# Topic: Pattern Matching — Common Mistakes with Class Patterns

# Common mistakes when matching class instances

# === MISTAKE 1: Wrong number of arguments ===

# WRONG - wrong number of positional arguments
class Point:
    __match_args__ = ("x", "y")
    
    def __init__(self, x, y):
        self.x = x
        self.y = y


point = Point(10, 20)

# match point:
#     case Point(10, 20, 30):  # Error! Point only has 2 args
#         print("3D point")

# CORRECT - match with correct number
match point:
    case Point(10, 20):
        print("2D point at (10, 20)")
    case Point(x, y):
        print("2D point at (" + str(x) + ", " + str(y) + ")")
    case _:
        print("Not a point")


# === MISTAKE 2: Not matching correct class ===

# WRONG - matching wrong class type
class Circle:
    __match_args__ = ("radius",)
    
    def __init__(self, radius):
        self.radius = radius


circle = Circle(5)

# match circle:
#     case Point(x, y):  # Error! Circle is not Point
#         print("Point")

# CORRECT - check class type first
match circle:
    case Point(x, y):
        print("Point: " + str(x) + ", " + str(y))
    case Circle(r):
        print("Circle radius: " + str(r))
    case _:
        print("Unknown shape")


# === MISTAKE 3: Using attribute names instead of positional ===

# WRONG - using attribute names as positional
class User:
    __match_args__ = ("name", "age")
    
    def __init__(self, name, age):
        self.name = name
        self.age = age


user = User("Alice", 30)

# match user:
#     case User(name="Alice", age=30):  # Wrong syntax!
#         print("Alice")

# CORRECT - use keyword arguments
match user:
    case User(name="Alice", age=30):
        print("Found Alice, age 30")
    case User(name=name, age=age):
        print(name + " is " + str(age))
    case _:
        print("Unknown user")


# === MISTAKE 4: Forgetting __match_args__ for positional ===

# WRONG - expecting positional to work without match_args
class Color:
    __match_args__ = ("red", "green", "blue")
    
    def __init__(self, red, green, blue):
        self.red = red
        self.green = green
        self.blue = blue


# Note: Without __match_args__, must use keyword or attribute
color = Color(255, 0, 0)

match color:
    case Color(red=255, green=0, blue=0):
        print("Red")
    case Color(red=r, green=g, blue=b):
        print("RGB(" + str(r) + ", " + str(g) + ", " + str(b) + ")")
    case _:
        print("Unknown")


# === MISTAKE 5: Type vs instance confusion ===

# WRONG - matching class instead of instance
value = "hello"

# match value:
#     case str:  # This is wrong! Matches the class, not instance
#         print("String class")

# CORRECT - use str() for type pattern
match value:
    case str():
        print("String instance")
    case int():
        print("Integer instance")
    case _:
        print("Other")


# === MISTAKE 6: Not handling subclass matching ===

# WRONG - not considering inheritance
class Animal:
    __match_args__ = ()
    pass


class Dog(Animal):
    __match_args__ = ("name",)
    
    def __init__(self, name):
        self.name = name


class Cat(Animal):
    __match_args__ = ("name",)
    
    def __init__(self, name):
        self.name = name


pet = Dog("Rex")

# Note: Pattern matching checks exact class by default
match pet:
    case Dog(name):
        print("Dog: " + name)
    case Cat(name):
        print("Cat: " + name)
    case Animal():
        print("Animal")
    case _:
        print("Not an animal")


# === MISTAKE 7: Missing default case ===

# WRONG - no wildcard case
class Shape:
    __match_args__ = ("name",)
    
    def __init__(self, name):
        self.name = name


class Rectangle:
    __match_args__ = ("w", "h")
    
    def __init__(self, w, h):
        self.w = w
        self.h = h


class Circle:
    __match_args__ = ("r",)
    
    def __init__(self, r):
        self.r = r


shape = Shape("unknown")

# match shape:
#     case Rectangle(w, h):
#         print("Rectangle")
#     # No wildcard - would error on unknown!

# CORRECT - always include wildcard
match shape:
    case Rectangle(w, h):
        print("Rectangle")
    case Circle(r):
        print("Circle")
    case _:
        print("Unknown shape")


# === MISTAKE 8: Confusing instance pattern with class attribute ===

# WRONG - trying to match class attributes directly
class Server:
    DEFAULT_PORT = 8080
    __match_args__ = ("port",)
    
    def __init__(self, port):
        self.port = port


server = Server(8080)

# match server:
#     case Server(DEFAULT_PORT):  # Wrong! DEFAULT_PORT is class attr
#         print("Default port")

# CORRECT - match instance attributes
match server:
    case Server(port=8080):
        print("Default port")
    case Server(port=p):
        print("Custom port: " + str(p))
    case _:
        print("Not a server")


# === Best Practices ===
print("\n=== Best Practices ===")

# 1. Match correct number of arguments
# 2. Check class type before attributes
# 3. Use keyword args for classes without __match_args__
# 4. Use __match_args__ for cleaner positional patterns
# 5. Use type() pattern: str(), not str
# 6. Consider inheritance in class matching
# 7. Always include wildcard case
# 8. Don't confuse class attributes with instance attributes
