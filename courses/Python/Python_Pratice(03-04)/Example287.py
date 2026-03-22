# Example287: Mixins and Multiple Inheritance
class WalkMixin:
    def walk(self):
        return f"{self.name} is walking"

class SwimMixin:
    def swim(self):
        return f"{self.name} is swimming"

class FlyMixin:
    def fly(self):
        return f"{self.name} is flying"

class Animal:
    def __init__(self, name):
        self.name = name

class Dog(WalkMixin, Animal):
    pass

class Fish(SwimMixin, Animal):
    pass

class Duck(WalkMixin, SwimMixin, FlyMixin, Animal):
    pass

print("Mixins:")
dog = Dog("Buddy")
print(dog.walk())

fish = Fish("Nemo")
print(fish.swim())

duck = Duck("Donald")
print(duck.walk())
print(duck.swim())
print(duck.fly())

# Method Resolution Order
print("\nMRO:")
print(Duck.__mro__)
