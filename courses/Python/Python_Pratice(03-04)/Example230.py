# Example230: itertools - count, cycle, repeat
import itertools

# count(start, step) - infinite iterator
print("Count from 0 to 10:")
for i, num in enumerate(itertools.count()):
    if i > 10:
        break
    print(num, end=" ")
print()

print("\nCount from 5 to 15:")
for num in itertools.count(5):
    if num > 15:
        break
    print(num, end=" ")
print()

print("\nCount with step 3:")
for num in itertools.count(0, 3):
    if num > 20:
        break
    print(num, end=" ")
print()

# cycle(iterable) - cycles through an iterable infinitely
print("\nCycle through colors:")
colors = ['red', 'green', 'blue']
cycle_iter = itertools.cycle(colors)
for i in range(8):
    print(next(cycle_iter), end=" ")
print()

# repeat(elem, n) - repeats element n times (or infinitely)
print("\nRepeat 5 three times:")
print(list(itertools.repeat(5, 3)))

print("\nRepeat 'hello' twice:")
print(list(itertools.repeat('hello', 2)))

# Practical use: repeat with map
print("\nSquare numbers using repeat:")
squares = list(map(lambda x: x**2, range(5)))
print(squares)

# Repeat with zip
print("\nRepeat value with zip:")
for item, count in zip(itertools.repeat('A', 3), range(3)):
    print(f"{item}: {count}")
