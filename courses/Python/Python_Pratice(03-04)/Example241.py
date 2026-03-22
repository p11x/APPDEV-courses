# Example241: Custom Sorting with __lt__ and Sorting Classes
from functools import total_ordering

# Define custom sort order with __lt__
print("Custom __lt__ for sorting:")
class Version:
    def __init__(self, major, minor, patch):
        self.major = major
        self.minor = minor
        self.patch = patch
    
    def __lt__(self, other):
        return (self.major, self.minor, self.patch) < (other.major, other.minor, other.patch)
    
    def __eq__(self, other):
        return (self.major, self.minor, self.patch) == (other.major, other.minor, other.patch)
    
    def __repr__(self):
        return f"v{self.major}.{self.minor}.{self.patch}"

versions = [Version(1, 2, 3), Version(2, 0, 0), Version(1, 10, 0), Version(1, 2, 0)]
result = sorted(versions)
print(f"Sorted versions: {result}")

# Using @total_ordering to reduce boilerplate
@total_ordering
class Temperature:
    def __init__(self, celsius):
        self.celsius = celsius
    
    def __lt__(self, other):
        return self.celsius < other.celsius
    
    def __eq__(self, other):
        return self.celsius == other.celsius
    
    def __repr__(self):
        return f"{self.celsius}°C"

print("\nUsing @total_ordering:")
temps = [Temperature(25), Temperature(10), Temperature(30), Temperature(15)]
result = sorted(temps)
print(f"Sorted: {result}")
print(f"Max: {max(temps)}")
print(f"Min: {min(temps)}")

# Custom priority queue with sorting
print("\nPriority queue with custom order:")
class Task:
    def __init__(self, priority, name):
        self.priority = priority
        self.name = name
    
    def __lt__(self, other):
        return self.priority < other.priority
    
    def __repr__(self):
        return f"Task({self.priority}, {self.name})"

import heapq
tasks = [Task(3, "Low"), Task(1, "High"), Task(2, "Medium")]
heapq.heapify(tasks)
print("Priority order:")
while tasks:
    task = heapq.heappop(tasks)
    print(f"  {task}")

# Sort by distance (custom key function)
print("\nSort points by distance from origin:")
class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y
    
    def __repr__(self):
        return f"({self.x}, {self.y})"

points = [Point(3, 4), Point(1, 1), Point(5, 12), Point(2, 2)]
result = sorted(points, key=lambda p: p.x**2 + p.y**2)
print(f"Sorted by distance: {result}")
