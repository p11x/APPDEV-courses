# Example310: Functional Programming Patterns
from functools import reduce

# Pipeline
def pipeline(*functions):
    def apply(x):
        return reduce(lambda v, f: f(v), functions, x)
    return apply

print("Pipeline:")
pipeline(
    lambda x: x + 1,
    lambda x: x * 2,
    lambda x: x - 3
)(5)

# Compose (right to left)
def compose(*functions):
    def apply(x):
        return reduce(lambda v, f: f(v), reversed(functions), x)
    return apply

print("\nCompose:")
result = compose(
    lambda x: x - 3,
    lambda x: x * 2,
    lambda x: x + 1
)(5)
print(f"Result: {result}")

# Curry
def curry(func):
    return lambda a: lambda b: func(a, b)

print("\nCurry:")
add = curry(lambda a, b: a + b)
print(f"add(5)(3): {add(5)(3)}")
