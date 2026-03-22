# Example286: ABC (Abstract Base Classes)
from abc import ABC, abstractmethod

class Shape(ABC):
    @abstractmethod
    def area(self):
        pass
    
    @abstractmethod
    def perimeter(self):
        pass
    
    def describe(self):
        return f"Area: {self.area()}, Perimeter: {self.perimeter()}"

class Rectangle(Shape):
    def __init__(self, width, height):
        self.width = width
        self.height = height
    
    def area(self):
        return self.width * self.height
    
    def perimeter(self):
        return 2 * (self.width + self.height)

class Circle(Shape):
    def __init__(self, radius):
        self.radius = radius
    
    def area(self):
        import math
        return math.pi * self.radius ** 2
    
    def perimeter(self):
        import math
        return 2 * math.pi * self.radius

print("Abstract Base Classes:")
rect = Rectangle(5, 3)
print(f"Rectangle: {rect.describe()}")

circle = Circle(2)
print(f"Circle: {circle.describe()}")

# Cannot instantiate abstract class
# shape = Shape()  # Error!

# Multiple inheritance with ABC
class Colored(ABC):
    @abstractmethod
    def get_color(self):
        pass

class ColoredRectangle(Rectangle, Colored):
    def __init__(self, width, height, color):
        super().__init__(width, height)
        self.color = color
    
    def get_color(self):
        return self.color

print("\nMultiple Inheritance:")
colored_rect = ColoredRectangle(4, 2, "red")
print(f"Colored rectangle: {colored_rect.describe()}, Color: {colored_rect.get_color()}")
