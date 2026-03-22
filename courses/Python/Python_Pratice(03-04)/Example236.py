# Example236: functools - partial
from functools import partial

# partial(func, *args, **keywords) - create partial function
def power(base, exponent):
    return base ** exponent

print("partial - create specialized functions:")
square = partial(power, exponent=2)
cube = partial(power, exponent=3)
to_the_fifth = partial(power, 5)  # base = 5

print(f"square(4): {square(4)}")
print(f"cube(3): {cube(3)}")
print(f"to_the_fifth(): {to_the_fifth()}")

# partial with built-in functions
print("\npartial with built-in functions:")
print_at_fixed_point = partial(print, sep=" | ", end=" END\n")
print_at_fixed_point("Hello", "World")

# partial for callback functions
print("\nUsing partial for callbacks:")
def greet(name, greeting="Hello"):
    return f"{greeting}, {name}!"

say_hello = partial(greet, greeting="Hello")
say_hi = partial(greet, greeting="Hi")
say_hey = partial(greet, greeting="Hey")

print(say_hello("Alice"))
print(say_hi("Bob"))
print(say_hey("Charlie"))

# partial with map
print("\npartial with map:")
def multiply(a, b):
    return a * b

double = partial(multiply, 2)
triple = partial(multiply, 3)

numbers = [1, 2, 3, 4, 5]
print(f"Double: {list(map(double, numbers))}")
print(f"Triple: {list(map(triple, numbers))}")

# partial with sorted
print("\npartial with sorted:")
import operator
sort_by_second = partial(sorted, key=operator.itemgetter(1))
data = [(1, 5), (3, 2), (2, 8), (4, 1)]
print(f"Sort by second: {sort_by_second(data)}")
