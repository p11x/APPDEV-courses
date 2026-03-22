# Example299: Object Identity and Type Checking
class Person:
    def __init__(self, name):
        self.name = name

print("Identity and Type:")
p1 = Person("Alice")
p2 = Person("Alice")
p3 = p1

print(f"p1 is p2: {p1 is p2}")
print(f"p1 is p3: {p1 is p3}")
print(f"p1 == p2: {p1 == p2}")
print(f"type(p1): {type(p1)}")
print(f"isinstance(p1, Person): {isinstance(p1, Person)}")

# issubclass
print(f"\nissubclass: {issubclass(Person, object)}")
