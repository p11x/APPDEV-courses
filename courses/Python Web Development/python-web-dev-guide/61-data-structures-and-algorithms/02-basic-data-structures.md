# Basic Data Structures

## What You'll Learn

- Lists, arrays, and linked lists
- Stacks and queues
- When to use each structure

## Prerequisites

- Completed `01-big-o-notation.md`

## Lists (Dynamic Arrays)

Python's list is a dynamic array:

```python
# Creating a list
numbers: list[int] = [1, 2, 3, 4, 5]

# Access - O(1)
first = numbers[0]

# Append - O(1) amortized
numbers.append(6)

# Insert at beginning - O(n)
numbers.insert(0, 0)

# Search - O(n)
if 3 in numbers:
    print("Found")
```

## Linked Lists

```python
from dataclasses import dataclass
from typing import Optional

@dataclass
class Node[T]:
    value: T
    next: Optional["Node[T]"] = None

class LinkedList[T]:
    def __init__(self):
        self.head: Optional[Node[T]] = None
    
    def append(self, value: T) -> None:
        new_node = Node(value)
        if not self.head:
            self.head = new_node
            return
        
        current = self.head
        while current.next:
            current = current.next
        current.next = new_node
    
    def prepend(self, value: T) -> None:
        new_node = Node(value, self.head)
        self.head = new_node
    
    def find(self, value: T) -> bool:
        current = self.head
        while current:
            if current.value == value:
                return True
            current = current.next
        return False
    
    def delete(self, value: T) -> bool:
        if not self.head:
            return False
        
        if self.head.value == value:
            self.head = self.head.next
            return True
        
        current = self.head
        while current.next:
            if current.next.value == value:
                current.next = current.next.next
                return True
            current = current.next
        return False
```

🔍 **Line-by-Line Breakdown:**

1. `@dataclass` — Creates automatic init, repr, eq for Node class.
2. `class Node[T]:` — Generic node with type parameter T.
3. `value: T` — Generic value of any type.
4. `next: Optional["Node[T]"]` — Pointer to next node.
5. `class LinkedList[T]:` — Generic linked list class.
6. `def append(self, value: T) -> None:` — Add to end, O(n).
7. `def prepend(self, value: T) -> None:` — Add to beginning, O(1).
8. `def find(self, value: T) -> bool:` — Search, O(n).

## Stack

```python
class Stack[T]:
    def __init__(self):
        self._items: list[T] = []
    
    def push(self, item: T) -> None:
        self._items.append(item)
    
    def pop(self) -> T:
        if not self._items:
            raise IndexError("Pop from empty stack")
        return self._items.pop()
    
    def peek(self) -> T:
        if not self._items:
            raise IndexError("Peek from empty stack")
        return self._items[-1]
    
    def is_empty(self) -> bool:
        return len(self._items) == 0
```

## Queue

```python
from collections import deque

class Queue[T]:
    def __init__(self):
        self._items: deque[T] = deque()
    
    def enqueue(self, item: T) -> None:
        self._items.append(item)
    
    def dequeue(self) -> T:
        if not self._items:
            raise IndexError("Dequeue from empty queue")
        return self._items.popleft()
    
    def peek(self) -> T:
        if not self._items:
            raise IndexError("Peek from empty queue")
        return self._items[0]
    
    def is_empty(self) -> bool:
        return len(self._items) == 0
```

## When to Use What

| Structure | Use Case |
|-----------|----------|
| List | Random access, appending |
| Linked List | Frequent insertions/deletions |
| Stack | Undo functionality, parsing |
| Queue | Task scheduling, BFS |

## Summary

- Lists are great for random access
- Linked lists excel at insertions
- Stacks follow LIFO order
- Queues follow FIFO order

## Next Steps

Continue to `03-hash-tables-and-dictionaries.md`.
