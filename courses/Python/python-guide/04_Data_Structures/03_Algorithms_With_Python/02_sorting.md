# Sorting

## What You'll Learn

- sorted() vs .sort()
- key parameter for custom sorting
- reverse sorting
- attrgetter/itemgetter
- Timsort explanation

## Prerequisites

- Read [01_searching.md](./01_searching.md) first

## sorted() vs .sort()

```python
# sorted() - returns new sorted list
numbers = [3, 1, 4, 1, 5]
sorted_nums = sorted(numbers)
print(sorted_nums)  # [1, 1, 3, 4, 5]
print(numbers)  # [3, 1, 4, 1, 5] unchanged

# .sort() - sorts in place, returns None
numbers.sort()
print(numbers)  # [1, 1, 3, 4, 5]
```

## key Parameter

```python
# Sort by length
words = ["apple", "hi", "banana"]
sorted_words = sorted(words, key=len)
print(sorted_words)  # ['hi', 'apple', 'banana']

# Sort by absolute value
numbers = [-5, 2, -3, 1]
sorted_nums = sorted(numbers, key=abs)
print(sorted_nums)  # [1, 2, -3, -5]

# Sort dictionary by value
d = {"apple": 5, "banana": 2, "cherry": 7}
sorted_items = sorted(d.items(), key=lambda x: x[1])
print(sorted_items)  # [('banana', 2), ('apple', 5), ('cherry', 7)]
```

## Using operator Module

```python
from operator import itemgetter, attrgetter

# Sort list of tuples by second element
pairs = [(1, 3), (2, 1), (3, 2)]
sorted_pairs = sorted(pairs, key=itemgetter(1))
print(sorted_pairs)  # [(2, 1), (3, 2), (1, 3)]

# Sort objects by attribute
class Person:
    def __init__(self, name: str, age: int) -> None:
        self.name = name
        self.age = age

people = [Person("Alice", 30), Person("Bob", 25)]
sorted_people = sorted(people, key=attrgetter("age"))
```

## Summary

- Use `sorted()` for new list, `.sort()` for in-place
- Use `key=` for custom sorting
- Use `reverse=True` for descending

## Next Steps

Continue to **[03_stacks_queues_heaps.md](./03_stacks_queues_heaps.md)**
