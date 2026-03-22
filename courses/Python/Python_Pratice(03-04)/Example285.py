# Example285: Class Methods and Static Methods
class MyClass:
    class_var = "I am a class variable"
    
    def __init__(self, value):
        self.instance_var = value
    
    def instance_method(self):
        return f"Instance method called with {self.instance_var}"
    
    @classmethod
    def class_method(cls):
        return f"Class method called with {cls.class_var}"
    
    @staticmethod
    def static_method():
        return "Static method called"

print("Class Methods and Static Methods:")
obj = MyClass(10)

print(f"Instance method: {obj.instance_method()}")
print(f"Class method: {MyClass.class_method()}")
print(f"Static method: {MyClass.static_method()}")

# Factory method pattern
class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    
    @classmethod
    def from_dict(cls, data):
        return cls(data['name'], data['age'])
    
    @classmethod
    def create_adult(cls, name):
        return cls(name, 18)
    
    def __repr__(self):
        return f"Person({self.name}, {self.age})"

print("\nFactory Method:")
data = {'name': 'Alice', 'age': 25}
p1 = Person.from_dict(data)
print(f"From dict: {p1}")
p2 = Person.create_adult('Bob')
print(f"Adult: {p2}")

# Static method for utility
class MathUtils:
    @staticmethod
    def is_even(n):
        return n % 2 == 0
    
    @staticmethod
    def factorial(n):
        if n <= 1:
            return 1
        return n * MathUtils.factorial(n-1)

print("\nStatic Method:")
print(f"is_even(4): {MathUtils.is_even(4)}")
print(f"factorial(5): {MathUtils.factorial(5)}")
