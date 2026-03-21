# Example89.py
# Topic: Pattern Matching — Sequence Unpacking

# Match and unpack lists/tuples

# === Basic sequence unpacking ===
# Match a list with exact number of elements

point = [10, 20]

match point:
    case [x, y]:
        print("Point: x=" + str(x) + ", y=" + str(y))
    case _:
        print("Not a 2D point")


# === Unpacking tuples ===
coord = (5, 15, 25)

match coord:
    case (a, b, c):
        print("3D coord: " + str(a) + ", " + str(b) + ", " + str(c))
    case _:
        print("Not a 3D coordinate")


# === Matching with specific values ===
command = ["start", "server"]

match command:
    case ["start", name]:
        print("Starting: " + name)
    case ["stop", name]:
        print("Stopping: " + name)
    case _:
        print("Unknown command")


# === Matching first element + rest ===
data = [1, 2, 3, 4, 5]

match data:
    case [first, second, rest]:
        print("First: " + str(first))
        print("Second: " + str(second))
        print("Rest: " + str(rest))
    case _:
        print("Not enough elements")


# === Matching with mixed types ===
person = ["Alice", 30, "NYC"]

match person:
    case [name, age, city]:
        print(name + " is " + str(age) + " from " + city)
    case _:
        print("Invalid person data")


# === Practical: HTTP request parser ===
request = ["GET", "/home", "HTTP/1.1"]

match request:
    case ["GET", path, version]:
        print("GET request to " + path + " (" + version + ")")
    case ["POST", path, version]:
        print("POST request to " + path + " (" + version + ")")
    case ["PUT", path, version]:
        print("PUT request to " + path + " (" + version + ")")
    case ["DELETE", path, version]:
        print("DELETE request to " + path + " (" + version + ")")
    case _:
        print("Invalid request")


# === Practical: RGB color ===
color = [255, 128, 0]

match color:
    case [r, g, b]:
        print("RGB(" + str(r) + ", " + str(g) + ", " + str(b) + ")")
    case _:
        print("Not an RGB color")


# === Matching empty vs non-empty ===
items = []

match items:
    case []:
        print("Empty list")
    case [first, second]:
        print("Two items: " + str(first) + ", " + str(second))
    case [first]:
        print("One item: " + str(first))
    case _:
        print("Many items")


# === Matching with type + unpacking ===
mixed = [1, "hello", 3.14]

match mixed:
    case [int() as i, str() as s, float() as f]:
        print("Int: " + str(i) + ", String: " + s + ", Float: " + str(f))
    case _:
        print("Different structure")
