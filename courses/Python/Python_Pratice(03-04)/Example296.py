# Example296: Args and Kwargs
def func(*args, **kwargs):
    print(f"Positional args: {args}")
    print(f"Keyword args: {kwargs}")

print("Args and Kwargs:")
func(1, 2, 3, name="Alice", age=30)

# Unpacking
print("\nUnpacking:")
def add(a, b, c):
    return a + b + c

nums = [1, 2, 3]
print(f"From list: {add(*nums)}")

data = {"a": 10, "b": 20, "c": 30}
print(f"From dict: {add(**data)}")

# Flexible function
def flexible(*args, **kwargs):
    result = 0
    for arg in args:
        if isinstance(arg, (int, float)):
            result += arg
    for key, value in kwargs.items():
        if isinstance(value, (int, float)):
            result += value
    return result

print("\nFlexible function:")
print(f"Sum: {flexible(1, 2, 'a', x=3, y=4)}")
