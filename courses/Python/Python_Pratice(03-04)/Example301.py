# Example301: String Formatting
# F-strings (Python 3.6+)
name = "Alice"
age = 30

print("String Formatting:")
print(f"Name: {name}, Age: {age}")
print(f"Age in 10 years: {age + 10}")

# Format specifiers
print(f"\nPi: {3.14159:.2f}")
print(f"Hex: {255:#x}")
print(f"Binary: {10:b}")

# Format method
print("\nFormat method:")
print("Hello {}".format("World"))
print("{} + {} = {}".format(1, 2, 3))
print("{0} {1} {0}".format("Hello", "World"))

# Padding
print("\nPadding:")
print(f"|{42:>5}|")
print(f"|{42:<5}|")
print(f"|{42:^5}|")
print(f"|{42:05}|")
