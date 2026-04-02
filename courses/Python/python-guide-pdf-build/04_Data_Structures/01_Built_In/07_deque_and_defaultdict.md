# Deque and Defaultdict

## What You'll Learn

- Using collections.deque for O(1) operations
- Defaultdict for automatic value initialization
- Implementing queues and stacks with deque
- When to use each collection type

## Prerequisites

- Read [06_bytearray_and_memoryview.md](./06_bytearray_and_memoryview.md) first

## Deque (Double-Ended Queue)

`deque` provides O(1) appends and pops from both ends.

```python
# deque_demo.py

from collections import deque

# Create deque
dq: deque[int] = deque([1, 2, 3])

# Add to both ends - O(1)
dq.appendleft(0)
dq.append(4)

# Remove from both ends - O(1)
left: int = dq.popleft()
right: int = dq.pop()
print(dq)

# Useful for implementing queues
queue: deque[str] = deque(maxlen=3)
queue.append("first")
queue.append("second")
queue.append("third")
queue.append("fourth")
print(queue)
```

## Defaultdict

`defaultdict` automatically initializes missing keys with a default value.

```python
# defaultdict_demo.py

from collections import defaultdict

# Create with default factory
word_count: defaultdict[str, int] = defaultdict(int)

# Words are automatically initialized to 0
words = ["apple", "banana", "apple", "cherry", "banana", "apple"]
for word in words:
    word_count[word] += 1

print(dict(word_count))

# With list as default factory
items_by_category: defaultdict[str, list[str]] = defaultdict(list)
items_by_category["fruits"].append("apple")
items_by_category["fruits"].append("banana")
```

## Annotated Full Example

```python
# deque_defaultdict_demo.py
"""Complete demonstration of deque and defaultdict."""

from collections import deque, defaultdict


def main() -> None:
    # Deque for queue operations
    queue: deque[str] = deque()
    queue.append("task1")
    queue.append("task2")
    print(f"Processing: {queue.popleft()}")
    print(f"Remaining: {queue}")
    
    # Defaultdict for counting
    scores = defaultdict(int)
    results = ["alice", "bob", "alice", "charlie", "alice", "bob"]
    for name in results:
        scores[name] += 1
    print(f"\nScores: {dict(scores)}")
    
    # Grouping with defaultdict
    animals = ["ant", "bear", "cat", "antelope"]
    by_letter = defaultdict(list)
    for animal in animals:
        by_letter[animal[0]].append(animal)
    print(f"By letter: {dict(by_letter)}")


if __name__ == "__main__":
    main()
```

## Summary

- Using collections.deque for O(1) operations
- Defaultdict for automatic value initialization
- Implementing queues and stacks with deque

## Next Steps

Continue to **[01_heapq_and_priority_queues.md](../02_Advanced/01_heapq_and_priority_queues.md)**
