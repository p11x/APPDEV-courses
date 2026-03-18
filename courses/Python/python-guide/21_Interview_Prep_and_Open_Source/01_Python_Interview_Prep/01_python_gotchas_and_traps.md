# 🐍 Python Gotchas and Traps

> The Python quirks that trip up experienced developers in interviews.

## 🎯 What You'll Learn

- 15 common Python gotchas with explanations
- Why each gotcha happens
- Correct mental models
- Interview-safe answers

## 📦 Prerequisites

- Completion of [03_Functions/03_Functional_Tools/03_functools.md](../../03_Functions/03_Functional_Tools/03_functools.md)
- Understanding of Python basics

---

## 1. Mutable Default Arguments

### The Gotcha
```python
def append_to_list(value, my_list=[]):
    my_list.append(value)
    return my_list

print(append_to_list(1))  # [1]
print(append_to_list(2))  # [1, 2]  ← Wait, what?!
print(append_to_list(3))  # [1, 2, 3]
```

### Why It Happens
- Default arguments are evaluated **once** at function definition time
- The same list object is reused across all calls
- Mutable defaults retain state between calls

### Correct Mental Model
> Default arguments are like class attributes — shared across all instances

### Interview Answer
> "Never use mutable objects as default arguments. Use None instead and create the mutable object inside the function."

### ✅ Fixed Version
```python
def append_to_list(value, my_list=None):
    if my_list is None:
        my_list = []
    my_list.append(value)
    return my_list

print(append_to_list(1))  # [1]
print(append_to_list(2))  # [2]
print(append_to_list(3))  # [3]
```

### 💡 Explanation
- `my_list=None` uses immutable None as default
- Inside function, we check for None and create new list
- Each call gets a fresh list unless explicitly passed one

---

## 2. Late Binding Closures in Loops

### The Gotcha
```python
functions = []
for i in range(3):
    functions.append(lambda: i)

for f in functions:
    print(f())  # Prints 2, 2, 2 instead of 0, 1, 2!
```

### Why It Happens
- Lambda captures the variable `i`, not its value
- All lambdas reference the same variable `i`
- By the time they're called, loop has finished and `i` is 2

### Correct Mental Model
> Closures capture variables by reference, not by value

### Interview Answer
> "Use default arguments to capture the current value, or use functools.partial."

### ✅ Fixed Versions
```python
# Fix 1: Default argument trick
functions = []
for i in range(3):
    functions.append(lambda x=i: x)  # Capture current value

# Fix 2: functools.partial
from functools import partial
functions = []
for i in range(3):
    functions.append(partial(lambda x: x, i))
```

### 💡 Explanation
- Default arguments are evaluated at definition time
- `lambda x=i: x` captures current value of `i` in `x`
- Each lambda gets its own copy of the value

---

## 3. is vs ==

### The Gotcha
```python
# Small integers are cached
a = 256
b = 256
print(a is b)  # True

a = 257
b = 257
print(a is b)  # False? (implementation dependent!)

# String interning
a = "hello"
b = "hello"
print(a is b)  # True

a = "hello world!"
b = "hello world!"
print(a is b)  # False
```

### Why It Happens
- `is` checks identity (same object in memory)
- `==` checks equality (same value)
- Python caches small integers (-5 to 256) and interns some strings
- Behavior depends on implementation and compilation

### Correct Mental Model
> `is` = same object, `==` = same value. Use `==` for equality unless you specifically need identity check.

### Interview Answer
> "Use `==` for comparing values. Only use `is` for checking None or when you specifically need to know if two variables reference the exact same object."

### ✅ Best Practice
```python
# Good
if x is None:  # Identity check for None
    pass

if x == 5:     # Value check for equality
    pass

# Bad - don't do this!
if x is 5:     # Unreliable!
    pass
```

---

## 4. Chained Comparisons

### The Gotcha
```python
print(1 < 2 < 3)      # True
print(1 < 2 > 0)      # Also True!
print(1 < 2 and 2 > 0) # Same as above
```

### Why It Happens
- Chained comparisons are evaluated left to right
- Each comparison is done separately with AND logic
- `1 < 2 < 3` means `(1 < 2) and (2 < 3)`

### Correct Mental Model
> Chained comparisons are convenient but each part is evaluated independently

### Interview Answer
> "Python evaluates chained comparisons left to right, applying AND between each comparison. `a < b < c` is equivalent to `a < b and b < c`."

### 💡 Examples
```python
# These are equivalent
1 < 2 < 3          # True
1 < 2 and 2 < 3    # True

1 < 2 > 0          # True
1 < 2 and 2 > 0    # True

# But this is different!
0 < 1 > 2          # False (0<1 is True, 1>2 is False, True and False = False)
0 < 1 and 1 > 2    # False
```

---

## 5. Walrus Operator in Comprehensions vs Assignments

### The Gotcha
```python
# This works
if (n := len(data)) > 10:
    print(f"List is too long ({n} elements)")

# But this doesn't work as expected!
results = [x for x in data if (n := len(x)) > 3]
print(n)  # What is n? Last value from comprehension!
```

### Why It Happens
- Assignment expressions (`:=`) in comprehensions leak to outer scope
- The variable `n` remains accessible after comprehension completes
- This is intentional but can be confusing

### Correct Mental Model
> Walrus operator in comprehensions follows normal scoping rules - variables leak to enclosing scope

### Interview Answer
> "The walrus operator creates variables in the current scope. In comprehensions, this means the variable remains accessible after the comprehension finishes."

### ✅ Safe Usage
```python
# If you need the value outside, it works
results = [x for x in data if (n := len(x)) > 3]
print(f"Last item had length: {n}")  # OK if you expect this

# If you don't need it outside, use a different name
results = [x for x in data if (length := len(x)) > 3]
# length is still accessible but less likely to conflict
```

### 💡 Explanation
- Comprehensions have their own scope for iteration variables
- But assignment expressions follow LEGB rule normally
- Variable assigned with `:=` in comprehension is in enclosing scope

---

## 6. Dict Ordering

### The Gotcha
```python
# Python 3.6: insertion order preserved (CPython implementation detail)
# Python 3.7+: guaranteed by language spec
d = {}
d['a'] = 1
d['b'] = 2
d['c'] = 3

print(list(d.keys()))  # ['a', 'b', 'c'] in 3.7+
```

### Why It Happens
- Before 3.6: dict order was arbitrary
- 3.6: CPython preserved insertion order as implementation detail
- 3.7+: language specification guarantees insertion order

### Correct Mental Model
> Dicts maintain insertion order as of Python 3.7. This is guaranteed behavior, not implementation detail.

### Interview Answer
> "Since Python 3.7, dictionaries maintain insertion order as a language guarantee. This enables reliable ordered dict behavior without collections.OrderedDict."

### 💡 Practical Impact
```python
# Can now rely on order for:
for key, value in d.items():
    # Process in insertion order
    pass

# No need for OrderedDict in most cases
```

---

## 7. copy vs deepcopy

### The Gotcha
```python
import copy

original = [[1, 2], [3, 4]]
shallow = copy.copy(original)
deep = copy.deepcopy(original)

shallow[0][0] = 999
deep[0][0] = 888

print(original)  # [[999, 2], [3, 4]] - shallow affected it!
print(shallow)   # [[999, 2], [3, 4]]
print(deep)      # [[888, 2], [3, 4]] - deep is independent
```

### Why It Happens
- `copy.copy()` creates new container but references same inner objects
- `copy.deepcopy()` creates completely independent copy
- Nested mutable objects cause surprises with shallow copy

### Correct Mental Model
> Shallow copy = new container, same contents. Deep copy = new everything.

### Interview Answer
> "Shallow copy duplicates the container structure but shares references to nested objects. Deep copy recursively duplicates everything, creating a completely independent copy."

### ✅ When to Use Which
```python
# Use shallow copy for:
- Flat structures (list of immutables)
- When you want to share nested objects intentionally

# Use deep copy for:
- Nested mutable structures
- When you need complete independence
- Configuration objects that shouldn't share state
```

---

## 8. except Exception vs bare except

### The Gotcha
```python
try:
    risky_operation()
except:  # Bare except - catches EVERYTHING
    handle_error()

try:
    risky_operation()
except Exception:  # Better - catches most errors
    handle_error()
```

### Why It Happens
- Bare `except:` catches `BaseException`, which includes:
  - `SystemExit` (from `sys.exit()`)
  - `KeyboardInterrupt` (Ctrl+C)
  - `GeneratorExit` (generator cleanup)
- These are usually not errors you want to catch silently

### Correct Mental Model
> Bare except catches system-exit exceptions. Use `except Exception` for regular errors.

### Interview Answer
> "Bare except catches BaseException, including SystemExit and KeyboardInterrupt, which can prevent graceful shutdown. Always use except Exception unless you specifically need to catch system exceptions."

### ✅ Best Practice
```python
# Good
try:
    risky_operation()
except Exception as e:
    logger.error(f"Operation failed: {e}")
    # Handle or re-raise

# Rarely needed
try:
    risky_operation()
except BaseException as e:
    if isinstance(e, (SystemExit, KeyboardInterrupt)):
        raise  # Re-raise system exits
    handle_error()
```

---

## 9. Generator Exhaustion

### The Gotcha
```python
def count_to_three():
    yield 1
    yield 2
    yield 3

gen = count_to_three()
print(list(gen))  # [1, 2, 3]
print(list(gen))  # [] - exhausted!
```

### Why It Happens
- Generators maintain internal state
- Once exhausted, they stay exhausted
- No automatic reset like lists

### Correct Mental Model
> Generators are iterators that can only be consumed once. Think of them as a stream, not a container.

### Interview Answer
> "Generators are exhausted after one iteration because they maintain internal state. To reuse, you need to recreate the generator or use itertools.tee() to split the stream."

### ✅ Solutions
```python
# Option 1: Recreate generator
def get_numbers():
    return count_to_three()

print(list(get_numbers()))  # [1, 2, 3]
print(list(get_numbers()))  # [1, 2, 3]

# Option 2: tee for splitting (advanced)
import itertools
gen = count_to_three()
gen1, gen2 = itertools.tee(gen, 2)
print(list(gen1))  # [1, 2, 3]
print(list(gen2))  # [1, 2, 3]
```

### 💡 Explanation
- Generators compute values on-demand and don't store them
- Once yielded, values are gone unless you stored them
- tee() works by buffering values for multiple consumers

---

## 10. __del__ is Not a Destructor

### The Gotcha
```python
class FileHandler:
    def __init__(self, filename):
        self.filename = filename
        self.file = open(filename, 'w')
    
    def __del__(self):
        self.file.close()
        print(f"Closed {self.filename}")

# When is __del__ called? Unpredictable!
f = FileHandler("test.txt")
f.write("Hello")
# __del__ might be called much later, or never if program crashes
```

### Why It Happens
- `__del__` is called during garbage collection, not immediately when object goes out of scope
- Garbage collection timing is non-deterministic
- Circular references can prevent `__del__` from being called
- Exception in `__del__` during shutdown is ignored silently

### Correct Mental Model
> `__del__` is a finalizer called by garbage collector, not a destructor. Don't rely on it for critical cleanup.

### Interview Answer
> "__del__ is called during garbage collection, not when the object goes out of scope. Its timing is unpredictable, and it may not be called at all during interpreter shutdown. Use context managers (__enter__/__exit__) for reliable resource cleanup."

### ✅ Better Approach: Context Manager
```python
class FileHandler:
    def __init__(self, filename):
        self.filename = filename
    
    def __enter__(self):
        self.file = open(self.filename, 'w')
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        self.file.close()
    
    def write(self, data):
        self.file.write(data)

# Usage - guaranteed cleanup
with FileHandler("test.txt") as f:
    f.write("Hello")
# File is closed here, even if exception occurs
```

---

## 11. Class Variables vs Instance Variables

### The Gotcha
```python
class Dog:
    tricks = []  # Class variable - shared by all instances!
    
    def __init__(self, name):
        self.name = name  # Instance variable - unique to each
    
    def add_trick(self, trick):
        self.tricks.append(trick)

d1 = Dog("Fido")
d2 = Dog("Buddy")
d1.add_trick("roll over")
d2.add_trick("play dead")

print(d1.tricks)  # ['roll over', 'play dead'] - shared!
print(d2.tricks)  # ['roll over', 'play dead'] - shared!
```

### Why It Happens
- `tricks = []` is defined at class level - shared by all instances
- `self.tricks` looks up the attribute: instance first, then class
- When you assign `self.tricks = [...]`, you create instance attribute
- But `self.tricks.append()` modifies the class attribute!

### Correct Mental Model
> Class variables are shared across all instances. Instance variables are unique to each instance. Be careful with mutable class variables.

### Interview Answer
> "Class variables are shared across all instances of a class. Mutable class variables like lists or dictionaries can cause unexpected sharing. For instance-specific state, always initialize in __init__."

### ✅ Fixed Version
```python
class Dog:
    def __init__(self, name):
        self.name = name
        self.tricks = []  # Now instance-specific
    
    def add_trick(self, trick):
        self.tricks.append(trick)

d1 = Dog("Fido")
d2 = Dog("Buddy")
d1.add_trick("roll over")
d2.add_trick("play dead")

print(d1.tricks)  # ['roll over']
print(d2.tricks)  # ['play dead']
```

### 💡 Explanation
- Initialize mutable defaults in `__init__` to get instance-specific copies
- Class variables are for constants or shared state you explicitly want

---

## 12. __slots__ and Multiple Inheritance Conflict

### The Gotcha
```python
class A:
    __slots__ = ('x', 'y')

class B:
    __slots__ = ('z', 'w')

class C(A, B):
    __slots__ = ('v',)  # Conflict!

c = C()
c.x = 1
c.y = 2
c.z = 3
c.w = 4
c.v = 5
```

### Why It Happens
- `__slots__` creates a specific internal structure
- Multiple inheritance with `__slots__` can cause "instance lay-out conflict"
- Python can't determine a consistent memory layout

### Correct Mental Model
> `__slots__` restricts attributes to a fixed set. Multiple inheritance with slots requires compatible layouts.

### Interview Answer
> "__slots__ defines a fixed attribute structure. Multiple inheritance with __slots__ can fail if the slots create incompatible memory layouts. Either avoid slots in inheritance hierarchies or design compatible slot sets."

### ✅ Solutions
```python
# Option 1: Empty slots in child
class A:
    __slots__ = ('x', 'y')

class B:
    __slots__ = ('z', 'w')

class C(A, B):
    __slots__ = ()  # Empty - inherit parent slots

# Option 2: No slots in child
class A:
    __slots__ = ('x', 'y')

class B:
    __slots__ = ('z', 'w')

class C(A, B):
    pass  # No slots - uses __dict__

# Option 3: Use only one class with slots
class Base:
    __slots__ = ('x', 'y', 'z', 'w', 'v')

class A(Base): pass
class B(Base): pass
class C(Base): pass
```

### 💡 Explanation
- Empty `__slots__ = ()` means "no new slots, just inherit parents"
- No `__slots__` falls back to `__dict__` for attribute storage
- Single inheritance chain with slots works fine

---

## 13. nonlocal vs global

### The Gotcha
```python
def outer():
    x = "outer"
    
    def inner():
        nonlocal x
        x = "inner"
    
    inner()
    print(x)  # Prints "inner"

def outer2():
    x = "outer"
    
    def inner():
        global x  # Creates/modifies global x!
        x = "inner"
    
    inner()
    print(x)  # Still prints "outer" - global x changed!
```

### Why It Happens
- `nonlocal` looks in enclosing scopes (not global)
- `global` refers to module-level scope
- They operate in different scopes

### Correct Mental Model
> `nonlocal` = enclosing function scope. `global` = module scope.

### Interview Answer
> "nonlocal refers to variables in enclosing function scopes (but not global). global refers to module-level variables. Use nonlocal for closures, global for module variables."

### ✅ Usage Examples
```python
# nonlocal - for closures
def counter():
    count = 0
    def increment():
        nonlocal count
        count += 1
        return count
    return increment

# global - for module variables
DEBUG = False

def set_debug():
    global DEBUG
    DEBUG = True
```

### 💡 Explanation
- `nonlocal` lets inner functions modify outer function variables
- `global` lets functions modify module-level variables
- Both avoid creating new local variables

---

## 14. Unpacking Gotcha: a, b = b, a

### The Gotcha
```python
a = 1
b = 2
a, b = b, a
print(a, b)  # 2 1 - swapped!
```

### Why It Happens
- Right side is evaluated first (creates tuple (b, a))
- Then unpacked to left side
- The swap happens atomically - no temp variable needed in Python

### Correct Mental Model
> Tuple unpacking evaluates the right side completely before assignment. The swap is atomic.

### Interview Answer
> "The right side of unpacking is evaluated first, creating a tuple. Then values are assigned to left side variables. This makes a, b = b, a an atomic swap without needing a temporary variable."

### 💡 Explanation
- Step 1: Evaluate `b, a` → creates tuple `(2, 1)`
- Step 2: Unpack tuple to `a, b` → `a=2`, `b=1`
- No race condition because evaluation happens before assignment

### Other Uses
```python
# Unpack with * operator
a, *rest, b = [1, 2, 3, 4, 5]
# a=1, rest=[2,3,4], b=5

# Swap in list
items = [1, 2, 3, 4]
items[0], items[-1] = items[-1], items[0]
# [4, 2, 3, 1]
```

---

## 15. Float Precision

### The Gotcha
```python
print(0.1 + 0.2 == 0.3)  # False!
print(0.1 + 0.2)         # 0.30000000000000004
```

### Why It Happens
- Floating point numbers use binary representation
- 0.1 and 0.2 cannot be represented exactly in binary
- Small errors accumulate in arithmetic operations

### Correct Mental Model
> Floating point arithmetic is approximate. Never compare floats for exact equality.

### Interview Answer
> "Floating point numbers use binary representation which can't exactly represent decimal fractions like 0.1. Use math.isclose() for comparing floats, or use decimal.Decimal for exact decimal arithmetic."

### ✅ Solutions
```python
# Option 1: math.isclose (recommended)
import math
print(math.isclose(0.1 + 0.2, 0.3))  # True

# Option 2: absolute tolerance
abs(0.1 + 0.2 - 0.3) < 1e-10  # True

# Option 3: decimal.Decimal (for financial math)
from decimal import Decimal
print(Decimal('0.1') + Decimal('0.2') == Decimal('0.3'))  # True
```

### 💡 Explanation
- `math.isclose(a, b)` uses relative and absolute tolerance
- Better than fixed epsilon because it scales with magnitude
- `decimal.Decimal` provides exact decimal representation (slower but precise)

---

## Summary

✅ **Mutable defaults** — use None instead

✅ **Late binding** — capture values with default args

✅ **is vs ==** — use == for equality, is for identity

✅ **Chained comparisons** — evaluated left to right with AND

✅ **Walrus in comprehensions** — leaks to enclosing scope

✅ **Dict ordering** — guaranteed since Python 3.7

✅ **copy vs deepcopy** — shallow shares, deep copies everything

✅ **except Exception** — bare except catches SystemExit too

✅ **Generator exhaustion** — generators are one-time use

✅ **__del__ unpredictability** — use context managers instead

✅ **Class vs instance vars** — mutable class vars are shared

✅ **__slots__ inheritance** — can cause layout conflicts

✅ **nonlocal vs global** — different scopes

✅ **Atomic swap** — a, b = b, a works because RHS evaluated first

✅ **Float precision** — use math.isclose() or Decimal

---

## ➡️ Next Steps

Continue to [02_big_o_for_python.md](./02_big_o_for_python.md) to learn about time and space complexity.

---

## 🔗 Further Reading

- [Python Documentation - FAQ](https://docs.python.org/3/faq/programming.html)
- [Gotchas in Python](https://www.ferd.ca/gotchas-in-python.html)
- [Python Tutor](http://pythontutor.com/) - visualize code execution
