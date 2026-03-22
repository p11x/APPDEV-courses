# Example302: Walrus Operator (Python 3.8+)
# Walrus operator := assigns and returns value

print("Walrus Operator:")
# Without walrus
data = [1, 2, 3, 4, 5]
if len(data) > 3:
    print(f"Long list: {len(data)}")

# With walrus
if (n := len(data)) > 3:
    print(f"Long list using walrus: {n}")

# Loop with walrus
print("\nLoop with walrus:")
while (line := input("Enter text (quit to exit): ")) != "quit":
    print(f"You entered: {line}")

# List comprehension with walrus
print("\nList comprehension:")
data = [1, 2, 3, 4, 5, 6]
result = [y for x in data if (y := x * 2) > 6]
print(f"Result: {result}")
