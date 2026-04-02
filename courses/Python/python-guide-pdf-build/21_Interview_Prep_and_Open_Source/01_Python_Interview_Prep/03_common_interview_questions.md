# ❓ Common Interview Questions

> The 25 most common Python interview questions with model answers.

## 🎯 What You'll Learn

- Language fundamentals questions
- Data structures questions
- OOP questions
- Functional questions
- Async questions
- Practical questions
- Model answers with code examples

## 📦 Prerequisites

- Completion of [02_big_o_for_python.md](./02_big_o_for_python.md)

---

## Language Fundamentals

### 1. What is the GIL and when does it matter?

**Answer:** The Global Interpreter Lock (GIL) is a mutex that protects access to Python objects, preventing multiple native threads from executing Python bytecodes simultaneously. This means that in CPython, only one thread can execute Python code at a time, even on multi-core systems.

**When it matters:**
- CPU-bound tasks (mathematical computations, data processing) - threading won't help
- I/O-bound tasks (network requests, file operations) - threading can help because threads release GIL during I/O
- Multi-processing or asyncio are better for CPU-bound parallelism

**Example:**
```python
import threading
import time

def cpu_bound():
    count = 0
    for _ in range(10000000):
        count += 1

# With threading - won't be faster due to GIL
start = time.time()
threads = [threading.Thread(target=cpu_bound) for _ in range(2)]
for t in threads: t.start()
for t in threads: t.join()
print(f"Threading time: {time.time() - start:.2f}s")

# With multiprocessing - will be faster
from multiprocessing import Process
start = time.time()
processes = [Process(target=cpu_bound) for _ in range(2)]
for p in processes: p.start()
for p in processes: p.join()
print(f"Multiprocessing time: {time.time() - start:.2f}s")
```

### 2. Explain the difference between @classmethod and @staticmethod

**Answer:**
- `@classmethod` receives the class as first argument (conventionally named `cls`) and can access/modify class state
- `@staticmethod` doesn't receive any special first argument and behaves like a regular function but belongs to the class namespace
- Use `@classmethod` for alternative constructors or when you need to access class attributes
- Use `@staticmethod` for utility functions related to the class but don't need class/instance access

**Example:**
```python
class Date:
    def __init__(self, day, month, year):
        self.day = day
        self.month = month
        self.year = year
    
    @classmethod
    def from_string(cls, date_string):
        """Alternative constructor."""
        day, month, year = map(int, date_string.split('-'))
        return cls(day, month, year)
    
    @staticmethod
    def is_leap_year(year):
        """Utility function - doesn't need class or instance."""
        return year % 4 == 0 and (year % 100 != 0 or year % 400 == 0)

# Usage
date = Date.from_string("2024-01-15")  # Uses classmethod
print(Date.is_leap_year(2024))         # Uses staticmethod
```

### 3. What are Python decorators and how do you write one from scratch?

**Answer:** Decorators are functions that modify the behavior of other functions or classes. They allow you to wrap another function to extend its behavior without permanently modifying it.

**How to write one:**
1. Define a function that takes another function as argument
2. Inside, define a wrapper function that adds behavior before/after calling the original
3. Return the wrapper function
4. Use `@decorator_name` syntax to apply it

**Example:**
```python
def timer(func):
    """Decorator that measures execution time."""
    import time
    def wrapper(*args, **kwargs):
        start = time.time()
        result = func(*args, **kwargs)
        end = time.time()
        print(f"{func.__name__} took {end - start:.4f} seconds")
        return result
    return wrapper

@timer
def slow_function():
    time.sleep(1)
    return "Done"

# Usage
slow_function()  # Prints execution time
```

### 4. Explain generators vs list comprehensions — when to use each

**Answer:**
- **List comprehensions** create a list in memory all at once: `[x*2 for x in range(1000000)]`
- **Generator expressions** create a generator that yields items one at a time: `(x*2 for x in range(1000000))`
- Use list comprehensions when you need the full list immediately or multiple iterations
- Use generators for large datasets, streaming data, or when you only need to iterate once
- Generators use constant memory, list comprehensions use O(n) memory

**Example:**
```python
# List comprehension - creates full list in memory
squares = [x*x for x in range(1000000)]
print(squares[0], squares[-1])  # Fast access but uses memory

# Generator expression - lazy evaluation
squares_gen = (x*x for x in range(1000000))
print(next(squares_gen))  # First value only, memory efficient
for square in squares_gen:
    if square > 1000000:
        break
```

### 5. What is the difference between __str__ and __repr__?

**Answer:**
- `__str__` is for end-users - should return a readable, informal string representation
- `__repr__` is for developers - should return an unambiguous string that ideally could recreate the object
- If `__str__` is not defined, Python falls back to `__repr__`
- `__repr__` should ideally look like a valid Python expression that could recreate the object

**Example:**
```python
from datetime import datetime

now = datetime.now()
print(str(now))    # 2024-01-15 14:30:22.123456 (readable)
print(repr(now))   # datetime.datetime(2024, 1, 15, 14, 30, 22, 123456) (unambiguous)

class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y
    
    def __str__(self):
        return f"Point({self.x}, {self.y})"
    
    def __repr__(self):
        return f"Point({self.x}, {self.y})"

p = Point(3, 4)
print(str(p))   # Point(3, 4)
print(repr(p))  # Point(3, 4)
```

### 6. How does Python's memory management work? (reference counting + GC)

**Answer:** Python uses a combination of reference counting and generational garbage collection:
- **Reference counting:** Each object tracks how many references point to it. When count reaches zero, object is immediately deallocated
- **Generational GC:** Objects are grouped into generations (0, 1, 2). Younger objects are collected more frequently. Circular references that reference counting can't handle are caught by the cyclic garbage collector
- The `gc` module allows manual control: `gc.collect()`, `gc.set_threshold()`, etc.

**Example:**
```python
import gc

# Reference counting in action
a = []
b = a  # ref count = 2
c = a  # ref count = 3
del b  # ref count = 2
del c  # ref count = 1
del a  # ref count = 0 -> object deallocated immediately

# Circular reference - needs GC
def create_cycle():
    a = []
    b = [a]
    a.append(b)  # Circular reference
    return a

# Objects won't be freed until GC runs
cycle = create_cycle()
del cycle  # Still in memory until gc.collect()
gc.collect()  # Forces collection
```

### 7. What is the MRO and how does Python resolve it?

**Answer:** Method Resolution Order (MRO) is the order in which Python looks for a method in a class hierarchy. Python uses the C3 linearization algorithm to compute MRO, which ensures:
- Children precede parents
- If multiple parents, order is preserved as specified
- The relation is monotonic (if a class precedes another in MRO, it always does)

**Example:**
```python
class A: pass
class B: pass
class C(A, B): pass

print(C.__mro__)
# (<class '__main__.C'>, <class '__main__.A'>, <class '__main__.B'>, <class 'object'>)

# Diamond problem
class A: pass
class B(A): pass
class C(A): pass
class D(B, C): pass

print(D.__mro__)
# (<class '__main__.D'>, <class '__main__.B'>, <class '__main__.C'>, <class '__main__.A'>, <class 'object'>)
# B and C both come before A, and B comes before C as specified
```

---

## Data Structures

### 8. When would you use a deque over a list?

**Answer:** Use `collections.deque` when you need efficient appends and pops from both ends. Lists are efficient for appending/popping from the end but O(n) for the beginning. Deque provides O(1) for both ends.

**Use cases for deque:**
- Implementing queues (FIFO) or stacks (LIFO)
- Breadth-first search algorithms
- Sliding window problems
- When you need to frequently add/remove from both ends

**Example:**
```python
from collections import deque
import time

# Queue implementation
queue = deque()
queue.append("task1")  # O(1)
queue.append("task2")  # O(1)
task = queue.popleft()  # O(1) - vs list.pop(0) which is O(n)

# Performance comparison
def test_list():
    lst = []
    for i in range(100000):
        lst.insert(0, i)  # O(n) each time!

def test_deque():
    dq = deque()
    for i in range(100000):
        dq.appendleft(i)  # O(1) each time!

# deque will be much faster
```

### 9. How does a Python dict work internally? (hash table)

**Answer:** Python dictionaries are implemented as hash tables:
- Uses a hash function to compute an index into an array of buckets
- Collisions are resolved using open addressing (specifically, a variant of linear probing)
- When the table becomes 2/3 full, it's resized to maintain performance
- Keys must be hashable (immutable) - their hash value never changes during their lifetime
- Average case O(1) for lookup, insert, delete; worst case O(n) when many collisions occur

**Example:**
```python
# Demonstrating hash table behavior
d = {}
d['apple'] = 1
d['banana'] = 2
d['cherry'] = 3

# Internally, this might look like:
# Index 0: empty
# Index 1: ('banana', 2)
# Index 2: ('apple', 1)
# Index 3: ('cherry', 3)
# Index 4-7: empty
# (Actual implementation is more complex with probing)

# Shows why order is preserved in 3.7+ - items stored in insertion order in the table
print(list(d.keys()))  # ['apple', 'banana', 'cherry']
```

### 10. What is the difference between a set and a frozenset?

**Answer:**
- `set` is mutable - you can add/remove elements after creation
- `frozenset` is immutable - cannot be changed after creation
- Because frozenset is immutable and hashable, it can be used as a dictionary key or stored in another set
- Sets are unordered collections of unique hashable objects

**Example:**
```python
# Mutable set
fruits = {"apple", "banana", "cherry"}
fruits.add("orange")  # OK
fruits.remove("banana")  # OK

# Immutable frozenset
frozen_fruits = frozenset(["apple", "banana", "cherry"])
# frozen_fruits.add("orange")  # AttributeError!
# frozen_fruits.remove("banana")  # AttributeError!

# But frozenset can be used as dict key
collections = {
    frozenset(["apple", "banana"]): "fruit basket",
    frozenset(["carrot", "broccoli"]): "vegetable basket"
}
print(collections[frozenset(["apple", "banana"])])  # "fruit basket"
```

### 11. Explain shallow copy vs deep copy with an example

**Answer:**
- **Shallow copy** creates a new container but references the same inner objects
- **Deep copy** creates a completely independent copy of the object and all objects it references
- Use `copy.copy()` for shallow, `copy.deepcopy()` for deep
- Shallow copy is faster but can cause unexpected sharing; deep copy is slower but safe

**Example:**
```python
import copy

# Nested list
original = [[1, 2], [3, 4]]

# Shallow copy
shallow = copy.copy(original)
shallow[0][0] = 999  # Changes inner list!
print(original)  # [[999, 2], [3, 4]] - affected!

# Deep copy
deep = copy.deepcopy(original)
deep[0][0] = 888  # Doesn't affect original
print(original)  # [[999, 2], [3, 4]] - unchanged
print(deep)      # [[888, 2], [3, 4]] - changed
```

---

## OOP

### 12. What is duck typing and how does it relate to Protocols?

**Answer:** Duck typing is the concept that "if it walks like a duck and quacks like a duck, then it must be a duck." In Python, we care about what an object can do (its methods/attributes), not what type it is. Protocols (from `typing.Protocol`) formalize duck typing by defining interfaces based on methods/attributes rather than inheritance.

**Example:**
```python
# Duck typing - any object with quack() method works
def make_it_quack(duck):
    duck.quack()

class RealDuck:
    def quack(self):
        return "Quack!"

class Person:
    def quack(self):
        return "I'm quacking like a duck!"

make_it_quack(RealDuck())  # Works
make_it_quack(Person())    # Also works!

# Protocol formalizes this
from typing import Protocol

class Quackable(Protocol):
    def quack(self) -> str: ...

def make_it_quack_protocol(duck: Quackable) -> str:
    return duck.quack()

# Still works with both!
make_it_quack_protocol(RealDuck())
make_it_quack_protocol(Person())
```

### 13. Explain the difference between composition and inheritance

**Answer:**
- **Inheritance** ("is-a" relationship): A class inherits from another class, getting its attributes and methods. Can lead to tight coupling and fragile hierarchies.
- **Composition** ("has-a" relationship): A class contains instances of other classes to delegate work to. More flexible and follows the "favor composition over inheritance" principle.

**Example:**
```python
# Inheritance approach
class Animal:
    def __init__(self, name):
        self.name = name
    
    def speak(self):
        raise NotImplementedError

class Dog(Animal):
    def speak(self):
        return "Woof!"

# Composition approach
class Speaker:
    def __init__(self, sound: str):
        self.sound = sound
    
    def speak(self):
        return self.sound

class Dog:
    def __init__(self, name):
        self.name = name
        self.speaker = Speaker("Woof!")
    
    def speak(self):
        return self.speaker.speak()

# Composition is more flexible - can change behavior at runtime
dog = Dog("Fido")
dog.speaker = Speaker("Meow!")  # Now it's a cat-dog!
print(dog.speak())  # Meow!
```

### 14. What are descriptors and when would you use one?

**Answer:** Descriptors are classes that implement `__get__`, `__set__`, or `__delete__` methods. They allow you to customize attribute access. Use them when you need to add logic to getting, setting, or deleting attributes (validation, lazy loading, caching, etc.).

**Example:**
```python
class ValidatedAttribute:
    def __init__(self, min_value, max_value):
        self.min_value = min_value
        self.max_value = max_value
        self.name = None
    
    def __set_name__(self, owner, name):
        self.name = f"_{name}"
    
    def __get__(self, obj, objtype=None):
        if obj is None:
            return self
        return obj.__dict__.get(self.name, 0)
    
    def __set__(self, obj, value):
        if not isinstance(value, int):
            raise TypeError("Value must be integer")
        if value < self.min_value or value > self.max_value:
            raise ValueError(f"Value must be between {self.min_value} and {self.max_value}")
        obj.__dict__[self.name] = value

class Person:
    age = ValidatedAttribute(0, 150)  # Descriptor for age
    
    def __init__(self, name, age):
        self.name = name
        self.age = age  # Triggers __set__

# Usage
p = Person("Alice", 25)
print(p.age)  # 25
p.age = 30    # Still valid
# p.age = 200  # ValueError!
```

### 15. How do abstract base classes differ from Protocols?

**Answer:**
- **Abstract Base Classes (ABCs)** use inheritance - subclasses must inherit from the ABC. They can provide both abstract methods (must be implemented) and concrete methods (can be used as-is).
- **Protocols** use structural subtyping (duck typing) - any class that implements the required methods/attributes matches the protocol, regardless of inheritance hierarchy.
- ABCs are explicit about relationships; Protocols are implicit and more flexible.
- Use ABCs when you want to define a clear "is-a" relationship. Use Protocols for duck typing scenarios.

**Example:**
```python
# ABC approach
from abc import ABC, abstractmethod

class Shape(ABC):
    @abstractmethod
    def area(self) -> float: ...
    
    def describe(self) -> str:  # Concrete method
        return f"I am a shape with area {self.area()}"

class Rectangle(Shape):
    def __init__(self, width, height):
        self.width = width
        self.height = height
    
    def area(self) -> float:
        return self.width * self.height

# Protocol approach
from typing import Protocol

class ShapeProtocol(Protocol):
    def area(self) -> float: ...

def print_area(shape: ShapeProtocol) -> None:
    print(f"Area: {shape.area()}")

# Both work with Protocol!
class Rectangle:
    def __init__(self, width, height):
        self.width = width
        self.height = height
    
    def area(self) -> float:
        return self.width * self.height

class Circle:
    def __init__(self, radius):
        self.radius = radius
    
    def area(self) -> float:
        return 3.14159 * self.radius ** 2

print_area(Rectangle(3, 4))  # Works
print_area(Circle(5))        # Also works - no inheritance needed!
```

---

## Functional

### 16. Explain closures and give a practical use case

**Answer:** A closure is a function that captures variables from its enclosing scope. The inner function "remembers" the environment in which it was created, even after the outer function has finished executing.

**Practical use case:** Creating factory functions, decorators, or maintaining state in functional programming.

**Example:**
```python
def make_multiplier(factor):
    """Returns a function that multiplies by factor."""
    def multiplier(x):
        return factor * x  # factor is captured from enclosing scope
    return multiplier

# Usage
double = make_multiplier(2)
triple = make_multiplier(3)

print(double(5))  # 10
print(triple(5))  # 15

# The factor value is "remembered" by each function
```

### 17. What does functools.lru_cache do and how does it work?

**Answer:** `lru_cache` is a decorator that caches function results using a Least Recently Used (LRU) strategy. When the cache exceeds its maximum size, it removes the least recently used items.

**How it works:**
- Stores function arguments as keys in a dictionary
- Stores results as values
- Maintains access order to identify least recently used items
- When cache is full, removes LRU item before adding new one
- Thread-safe and provides cache statistics

**Example:**
```python
from functools import lru_cache
import time

@lru_cache(maxsize=128)
def fibonacci(n):
    if n < 2:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

# First call - computes and caches
start = time.time()
result = fibonacci(35)
print(f"First call: {time.time() - start:.4f}s")

# Second call - returns cached result instantly
start = time.time()
result = fibonacci(35)
print(f"Second call: {time.time() - start:.4f}s")

# Cache info
print(fibonacci.cache_info())  # CacheHits=1, Misses=36, MaxSize=128, CurrSize=36
```

### 18. What is the difference between map() and a list comprehension?

**Answer:**
- `map(func, iterable)` applies a function to each item in an iterable and returns an iterator (lazy)
- List comprehension `[func(x) for x in iterable]` creates a list in memory immediately (eager)
- `map()` is more memory efficient for large datasets when you only need to iterate once
- List comprehensions are more readable and allow filtering with conditions
- In Python 3, `map()` returns an iterator; in Python 2 it returned a list

**Example:**
```python
# map() - lazy iterator
squared = map(lambda x: x**2, range(1000000))
print(next(squared))  # First value only, memory efficient

# List comprehension - creates full list
squared_list = [x*x for x in range(1000000)]
print(squared_list[0], squared_list[-1])  # Fast access but uses memory

# With filtering - list comprehension wins
evens_squared = [x*x for x in range(1000) if x % 2 == 0]
# With map + filter - less readable
evens_squared_map = list(map(lambda x: x**2, filter(lambda x: x % 2 == 0, range(1000))))
```

---

## Async

### 19. What is the event loop and how does asyncio work?

**Answer:** The event loop is the core of asyncio that manages and executes asynchronous tasks. It runs in a single thread and uses cooperative multitasking - tasks yield control when they're waiting for I/O operations.

**How asyncio works:**
- You define coroutines with `async def`
- You use `await` to pause a coroutine while waiting for something (like network I/O)
- The event loop runs coroutines, switching to another when one awaits
- Tasks are scheduled to run on the event loop
- Uses selectors or I/O completion ports under the hood for efficient I/O multiplexing

**Example:**
```python
import asyncio
import time

async def fetch_data(url):
    print(f"Fetching {url}")
    await asyncio.sleep(2)  # Simulate network delay
    return f"Data from {url}"

async def main():
    # Run concurrently
    start = time.time()
    results = await asyncio.gather(
        fetch_data("url1"),
        fetch_data("url2"),
        fetch_data("url3")
    )
    print(f"Completed in {time.time() - start:.2f}s")
    print(results)

# Usage
asyncio.run(main())
```

### 20. When would you use threading vs asyncio vs multiprocessing?

**Answer:**
- **Threading:** I/O-bound tasks where you can benefit from concurrent waiting (network requests, file reads). Limited by GIL for CPU-bound work.
- **Asyncio:** I/O-bound tasks with high concurrency needs (thousands of connections). Single-threaded, so no GIL issues, but requires async/await throughout.
- **Multiprocessing:** CPU-bound tasks that need true parallelism. Bypasses GIL by using separate processes. Higher memory overhead but full CPU utilization.

**Decision tree:**
```
Is the task CPU-bound?
  Yes → Use multiprocessing
  No  → Is it I/O-bound with many concurrent operations?
        Yes → Use asyncio
        No  → Use threading (for simple concurrency) or just synchronous code
```

**Example scenarios:**
- Web scraper with 1000 URLs → asyncio
- Image processing pipeline → multiprocessing
- Chat server handling many connections → asyncio
- Simple file backup script → threading or synchronous

---

## Practical

### 21. How do you handle exceptions properly in Python?

**Answer:** Proper exception handling involves:
- Catching specific exceptions rather than using bare `except:`
- Using `else` clause for code that should run only if no exception occurred
- Using `finally` clause for cleanup code that should always run
- Using `raise` to re-raise exceptions when appropriate
- Creating custom exception classes for domain-specific errors
- Using exception chaining (`raise ... from ...`) to preserve context
- Logging exceptions appropriately rather than just printing

**Example:**
```python
import logging

def process_file(filename):
    try:
        with open(filename, 'r') as f:
            data = f.read()
        # Process data
        result = len(data.split())
        return result
    except FileNotFoundError:
        logging.error(f"File not found: {filename}")
        raise  # Re-raise if caller should handle
    except PermissionError:
        logging.error(f"Permission denied: {filename}")
        return None
    except Exception as e:
        logging.exception(f"Unexpected error processing {filename}")
        raise  # Re-raise unexpected errors
    else:
        logging.info(f"Successfully processed {filename}")
    finally:
        # Any cleanup code here
        pass
```

### 22. What tools do you use for testing Python code?

**Answer:** Python testing ecosystem includes:
- **unittest** - built-in framework, xUnit style
- **pytest** - popular third-party framework with simpler syntax and powerful features
- **Doctest** - tests embedded in docstrings
- **Coverage.py** - measures code coverage
- **Hypothesis** - property-based testing
- **Tox** - tests across multiple Python versions
- **CI/CD integration** - GitHub Actions, GitLab CI, etc.

**Example pytest:**
```python
# test_calculator.py
def test_add():
    assert add(2, 3) == 5

def test_add_negative():
    assert add(-1, 1) == 0

def test_divide_by_zero():
    with pytest.raises(ZeroDivisionError):
        divide(1, 0)

# Run with: pytest test_calculator.py -v
```

### 23. How do you profile slow Python code?

**Answer:** Python profiling tools:
- **cProfile** - built-in profiler, measures function call frequency and duration
- **line_profiler** - line-by-line profiling (install via `pip install line_profiler`)
- **memory_profiler** - tracks memory usage over time
- **py-spy** - sampling profiler for production use
- **Yappi** - yet another profiler with threading support
- **Visualization tools** - snakeviz, tuna for viewing profiler output

**Example:**
```python
# Using cProfile
import cProfile
import pstats

def slow_function():
    total = 0
    for i in range(1000000):
        total += i
    return total

# Profile and save results
cProfile.run('slow_function()', 'profile_stats')

# View results
p = pstats.Stats('profile_stats')
p.sort_stats('cumulative').print_stats(10)  # Top 10 functions

# Or use command line: python -m cProfile -s cumulative script.py
```

### 24. What is the difference between requirements.txt and pyproject.toml?

**Answer:**
- **requirements.txt** - flat list of package dependencies with versions. Used by pip for installation. No standard format for build system or project metadata.
- **pyproject.toml** - modern standard defined by PEP 517/518. Contains build system requirements, project metadata, dependencies, and tool configuration. Replaces setup.py and requirements.txt for modern Python packaging.

**Example requirements.txt:**
```
requests==2.28.1
numpy>=1.21.0
pandas
```

**Example pyproject.toml:**
```toml
[build-system]
requires = ["setuptools>=61.0"]
build-backend = "setuptools.build_meta"

[project]
name = "myproject"
version = "1.0.0"
dependencies = [
    "requests==2.28.1",
    "numpy>=1.21.0",
    "pandas",
]

[project.optional-dependencies]
dev = [
    "pytest>=7.0",
    "black",
]

[tool.setuptools.packages]
find = where=["src"]

[tool.pytest.ini_options]
testpaths = ["tests"]
```

### 25. Explain Python's import system — what happens when you import a module?

**Answer:** When you import a module, Python:
1. Checks if module is already in `sys.modules` (cached)
2. If not found, searches for the module in `sys.path` (list of directories)
3. Looks for:
   - Built-in module (compiled into Python)
   - `.py` file (source code)
   - `.pyc` file (compiled bytecode)
   - Package directory (contains `__init__.py`)
4. If found, creates a new module object
5. Executes the module's code in the new module's namespace
6. Inserts the module object into `sys.modules` for future imports
7. Returns the module object

**Example:**
```python
# When you do: import math
# 1. Check sys.modules for 'math' - not there
# 2. Search sys.path for math.py, math.pyc, or math/ directory
# 3. Find /usr/lib/python3.12/math.py (or similar)
# 4. Create module object for math
# 5. Execute math.py code in math's namespace
# 6. Insert math module into sys.modules
# 7. Return math module

import sys
print('math' in sys.modules)  # False before import

import math
print('math' in sys.modules)  # True after import
print(sys.modules['math'] is math)  # True - same object
```

---

## Summary

You now have model answers for the 25 most common Python interview questions covering:
- Language fundamentals (GIL, decorators, generators, memory management)
- Data structures (dict internals, set vs frozenset, copy mechanisms)
- OOP (inheritance vs composition, descriptors, ABCs vs Protocols)
- Functional (closures, lru_cache, map vs comprehensions)
- Async (event loop, threading vs asyncio vs multiprocessing)
- Practical (exception handling, testing, profiling, packaging, imports)

---

## ➡️ Next Steps

Continue to [02_Coding_Challenges/01_array_and_string_patterns.md](../02_Coding_Challenges/01_array_and_string_patterns.md) to start solving coding challenges.

---

## 🔗 Further Reading

- [Python Interview Questions](https://realpython.com/python-interview-questions/)
- [Awesome Python Interview Questions](https://github.com/kenzhu12345/awesome-python-interview-questions)
- [Python Documentation](https://docs.python.org/3/)
