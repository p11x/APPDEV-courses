# Example317: More Practice with Strings
# String methods
print("String Methods:")
s = "Hello, World!"
print(f"Upper: {s.upper()}")
print(f"Lower: {s.lower()}")
print(f"Replace: {s.replace('World', 'Python')}")
print(f"Split: {s.split(',')}")

# Strip
print("\nStrip:")
s = "  hello  "
print(f"Strip: '{s.strip()}'")

# Join
print("\nJoin:")
words = ['Hello', 'World']
print(f"Join: {' '.join(words)}")

# Format
print("\nFormat:")
print(f"Name: {}, Age: {}".format("Alice", 30))
