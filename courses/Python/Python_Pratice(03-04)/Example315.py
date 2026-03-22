# Example315: More Practice with Lists
# List slicing
print("List Slicing:")
lst = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
print(f"First 3: {lst[:3]}")
print(f"Last 3: {lst[-3:]}")
print(f"Every 2nd: {lst[::2]}")
print(f"Reverse: {lst[::-1]}")

# List operations
print("\nList operations:")
lst = [1, 2, 3]
lst.append(4)
lst.extend([5, 6])
lst.insert(0, 0)
print(f"After ops: {lst}")
lst.remove(0)
print(f"After remove: {lst}")
print(f"Pop: {lst.pop()}")

# List comprehension
print("\nComprehension:")
print(f"Squares: {[x**2 for x in range(5)]}")
