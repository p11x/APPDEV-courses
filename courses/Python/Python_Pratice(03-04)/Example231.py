# Example231: itertools - takewhile, dropwhile, filterfalse
import itertools

# takewhile(predicate, iterable) - takes items while predicate is True
print("takewhile - take while condition is true:")
numbers = [1, 4, 6, 4, 1]
result = list(itertools.takewhile(lambda x: x < 5, numbers))
print(f"Input: {numbers}")
print(f"takewhile x < 5: {result}")

# dropwhile(predicate, iterable) - drops items while predicate is True, then yields rest
print("\ndropwhile - drop while condition is true:")
result = list(itertools.dropwhile(lambda x: x < 5, numbers))
print(f"Input: {numbers}")
print(f"dropwhile x < 5: {result}")

# filterfalse(predicate, iterable) - filters out items where predicate is True
print("\nfilterfalse - keep items where predicate is False:")
numbers = [1, 2, 3, 4, 5, 6]
result = list(itertools.filterfalse(lambda x: x % 2 == 0, numbers))
print(f"Input: {numbers}")
print(f"filterfalse (even): {result}")

# islice(iterable, stop) or islice(iterable, start, stop, step)
print("\nislice - slicing iterators:")
numbers = range(10)
result = list(itertools.islice(numbers, 5))
print(f"First 5: {result}")

result = list(itertools.islice(numbers, 2, 7))
print(f"From 2 to 7: {result}")

result = list(itertools.islice(numbers, 0, 10, 2))
print(f"Even numbers: {result}")

# Practical example: process file line by line
print("\nPractical - takewhile with data processing:")
data = [10, 20, 30, 40, 50, 60]
threshold = 35
valid = list(itertools.takewhile(lambda x: x <= threshold, data))
print(f"Data: {data}")
print(f"Valid (≤{threshold}): {valid}")

# Filter with complex condition
print("\nFilter numbers greater than 3:")
numbers = [1, 2, 3, 4, 5]
result = list(itertools.filterfalse(lambda x: x <= 3, numbers))
print(f"Input: {numbers}")
print(f"Greater than 3: {result}")
