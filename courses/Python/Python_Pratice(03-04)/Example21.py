# Example21.py
# Topic: Tuple Type Hints

# This file demonstrates how to use type hints with tuples.
# Tuples can have a fixed number of elements with specific types for each.


# Function returning a tuple with fixed length
# tuple[int, int] means exactly 2 integers
def get_coordinates() -> tuple[int, int]:
    return (10, 20)

coords = get_coordinates()    # tuple — (10, 20)
print("Coordinates: " + str(coords))    # Coordinates: (10, 20)


# Function returning tuple with three different types
# tuple[str, int, bool] means (string, integer, boolean)
def get_person() -> tuple[str, int, bool]:
    return ("Alice", 25, True)

person = get_person()    # tuple — ('Alice', 25, True)
print("Person: " + str(person))    # Person: ('Alice', 25, True)


# Function with tuple parameter
def swap_values(pair: tuple[int, int]) -> tuple[int, int]:
    first = pair[0]    # int — first element
    second = pair[1]    # int — second element
    return (second, first)

original = (5, 10)    # tuple — original pair
swapped = swap_values(original)    # tuple — (10, 5)
print("Original: " + str(original) + ", Swapped: " + str(swapped))    # Original: (5, 10), Swapped: (10, 5)


# Function returning tuple of mixed types
def get_result() -> tuple[int, str, bool]:
    return (100, "success", True)

result = get_result()    # tuple — (100, 'success', True)
print("Result: " + str(result))    # Result: (100, 'success', True)


# Unpacking tuple from function return
def get_name_and_age() -> tuple[str, int]:
    return ("Alice", 25)

name, age = get_name_and_age()    # str — "Alice", int — 25
print("Name: " + name + ", Age: " + str(age))    # Name: Alice, Age: 25


# Function returning optional tuple
from typing import Optional

def find_pair(items: list, target: int) -> Optional[tuple[int, int]]:
    for i in range(len(items)):
        for j in range(i + 1, len(items)):
            if items[i] + items[j] == target:
                return (i, j)
    return None

numbers = [1, 2, 3, 4, 5]    # list — numbers to search
pair = find_pair(numbers, 9)    # tuple — (3, 5) because 4+5=9
print("Pair: " + str(pair))    # Pair: (3, 5)

not_found = find_pair(numbers, 100)    # None — no pair adds to 100
print("Pair: " + str(not_found))    # Pair: None


# Function with tuple in parameter
def calculate_distance(point1: tuple[float, float], point2: tuple[float, float]) -> float:
    x1 = point1[0]    # float — first point x
    y1 = point1[1]    # float — first point y
    x2 = point2[0]    # float — second point x
    y2 = point2[1]    # float — second point y
    dx = x2 - x1    # float — difference in x
    dy = y2 - y1    # float — difference in y
    return (dx * dx + dy * dy) ** 0.5    # float — Euclidean distance

point_a = (0.0, 0.0)    # tuple — origin point
point_b = (3.0, 4.0)    # tuple — another point
distance = calculate_distance(point_a, point_b)    # float — 5.0
print("Distance: " + str(distance))    # Distance: 5.0


# Function returning tuple of lists
def split_list(items: list) -> tuple[list, list]:
    mid = len(items) // 2    # int — midpoint
    return (items[:mid], items[mid:])

numbers = [1, 2, 3, 4, 5, 6]    # list — original list
first_half, second_half = split_list(numbers)    # tuple — ([1,2,3], [4,5,6])
print("First: " + str(first_half) + ", Second: " + str(second_half))    # First: [1, 2, 3], Second: [4, 5, 6]


# Tuple type hint without specific length (homogeneous)
# tuple[int, ...] means tuple of any number of integers
def sum_all(numbers: tuple[int, ...]) -> int:
    total = 0    # int — running sum
    for num in numbers:
        total = total + num
    return total

result = sum_all((1, 2, 3, 4, 5))    # int — 15
print("Sum: " + str(result))    # Sum: 15


# Empty tuple type hint
def get_empty_tuple() -> tuple:
    return ()

empty = get_empty_tuple()    # tuple — ()
print("Empty: " + str(empty))    # Empty: ()


# Function returning multiple values as tuple
def min_max(numbers: list) -> tuple[int, int]:
    if not numbers:
        return (0, 0)
    minimum = numbers[0]    # int — current minimum
    maximum = numbers[0]    # int — current maximum
    for num in numbers:
        if num < minimum:
            minimum = num
        if num > maximum:
            maximum = num
    return (minimum, maximum)

data = [5, 2, 8, 1, 9, 3]    # list — numbers to analyze
minimum, maximum = min_max(data)    # int — min=1, max=9
print("Min: " + str(minimum) + ", Max: " + str(maximum))    # Min: 1, Max: 9


# Named tuple-like usage with tuples
def get_rgb_color() -> tuple[int, int, int]:
    return (255, 128, 0)

color = get_rgb_color()    # tuple — RGB values
red = color[0]    # int — 255 (red component)
green = color[1]    # int — 128 (green component)
blue = color[2]    # int — 0 (blue component)
print("RGB: R=" + str(red) + ", G=" + str(green) + ", B=" + str(blue))    # RGB: R=255, G=128, B=0
