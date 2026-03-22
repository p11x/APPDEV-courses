# Example284: Property and Descriptors
class Property:
    def __init__(self):
        self._value = None
    
    @property
    def value(self):
        return self._value
    
    @value.setter
    def value(self, val):
        if val < 0:
            raise ValueError("Value must be non-negative")
        self._value = val

print("Property:")
p = Property()
p.value = 10
print(f"Value: {p.value}")

# Validation
class Temperature:
    def __init__(self, celsius=0):
        self.celsius = celsius
    
    @property
    def fahrenheit(self):
        return self.celsius * 9/5 + 32
    
    @fahrenheit.setter
    def fahrenheit(self, val):
        self.celsius = (val - 32) * 5/9

print("\nTemperature:")
t = Temperature(25)
print(f"25C = {t.fahrenheit:.1f}F")
t.fahrenheit = 100
print(f"100F = {t.celsius:.1f}C")

# Read-only property
class Circle:
    def __init__(self, radius):
        self._radius = radius
    
    @property
    def radius(self):
        return self._radius
    
    @property
    def area(self):
        import math
        return math.pi * self._radius ** 2
    
    @property
    def circumference(self):
        import math
        return 2 * math.pi * self._radius

print("\nCircle:")
c = Circle(5)
print(f"Radius: {c.radius}")
print(f"Area: {c.area:.2f}")
print(f"Circumference: {c.circumference:.2f}")
