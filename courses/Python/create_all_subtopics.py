#!/usr/bin/env python3
"""Create all remaining subtopic folders for python-guide."""

from pathlib import Path
import sys

BASE = Path("python-guide")

# Content template function
def make_md(title, objectives, prev_file, overview, sections, next_file=""):
    sections_md = ""
    for sec in sections:
        sections_md += f"\n## {sec['title']}\n{sec['explain']}\n\n```python\n{sec['code']}\n```\n"
    return f"""# {title}

## What You'll Learn
{objectives}

## Prerequisites
- Read [{prev_file}](./{prev_file}) first

## Overview
{overview}
{sections_md}
## Common Mistakes
- {sections[0]['mistake1']}
- {sections[0]['mistake2']}

## Summary
- {sections[0]['summary1']}
- {sections[0]['summary2']}
- {sections[0]['summary3']}

## Next Steps
Continue to **[{next_file}](./{next_file})**
"""

# FILES dictionary with all content
FILES = {}

# ============ 02_Control_Flow/04_Pattern_Matching ============
FILES["02_Control_Flow/04_Pattern_Matching/01_match_statement.md"] = make_md(
    "Match Statement",
    "- match/case syntax (Python 3.10+)\n- Replacing long if/elif chains\n- Structural pattern matching overview",
    "02_match_statements.md",
    "Pattern matching lets you compare a value against multiple patterns. It's like a supercharged switch statement that can destructure data.",
    [{"title": "Basic Syntax", "code": 'status = "active"\nmatch status:\n    case "active":\n        print("Running")\n    case "paused":\n        print("Paused")\n    case _:  # default fallback\n        print("Unknown")', "explain": "The underscore _ is the wildcard that matches anything.", "mistake1": "Forgetting the default case with _", "mistake2": "Using = instead of : in case", "summary1": "match replaces switch/case", "summary2": "case _ catches everything", "summary3": "can return values directly"}, "02_matching_literals_and_types.md"]
)

FILES["02_Control_Flow/04_Pattern_Matching/02_matching_literals_and_types.md"] = make_md(
    "Matching Literals and Types",
    "- Matching int/str/bool literals\n- Type patterns with int()/str()\n- isinstance-style matching",
    "01_match_statement.md",
    "Match exact values or check types within the same match statement.",
    [{"title": "Literal Patterns", "code": 'x = 2\nmatch x:\n    case 1:\n        print("one")\n    case 2:\n        print("two")\n    case 3 | 4:  # multiple values\n        print("three or four")', "explain": "Use | to match multiple values (OR pattern).", "mistake1": "Using or instead of |", "mistake2": "Not handling all cases", "summary1": "| combines multiple values", "summary2": "Order matters - first match wins", "summary3": "Always include default case"}, "03_matching_sequences.md"]
)

FILES["02_Control_Flow/04_Pattern_Matching/04_matching_mappings.md"] = make_md(
    "Matching Mappings",
    "- Matching dict-like structures\n- Required vs optional keys\n- **rest capture",
    "02_matching_literals_and_types.md",
    "Pattern match on dictionaries and mapping types.",
    [{"title": "Dict Pattern", "code": 'data = {"name": "Alice", "age": 30}\nmatch data:\n    case {"name": name, "age": age}:\n        print(f"{name} is {age}")\n    case {"name": name}:  # age optional\n        print(f"{name} has no age")', "explain": "Extract specific keys from dict.", "mistake1": "Assuming all keys are required", "mistake2": "Not handling missing keys", "summary1": "Keys can be required or optional", "summary2": "Use **rest for remaining keys", "summary3": "Order doesn't matter in dict patterns"}, "05_guards_and_wildcards.md"]
)

FILES["02_Control_Flow/04_Pattern_Matching/05_guards_and_wildcards.md"] = make_md(
    "Guards and Wildcards",
    "- if guards inside case\n- _ wildcard for catch-all\n- | OR patterns combining cases",
    "04_matching_mappings.md",
    "Add conditions to your patterns with guards for more complex matching.",
    [{"title": "Guard Syntax", "code": 'x = 15\nmatch x:\n    case n if n > 10:\n        print(f"{n} is greater than 10")\n    case n if n > 5:\n        print(f"{n} is greater than 5")\n    case _:\n        print("less than or equal to 5")', "explain": "Guards use if after the pattern.", "mistake1": "Forgetting guard evaluation", "mistake2": "Guard runs after pattern match", "summary1": "Guards add conditions to cases", "summary2": "_ is the catch-all wildcard", "summary3": "Guards can reference bound variables"}, "06_class_patterns.md"]
)

FILES["02_Control_Flow/04_Pattern_Matching/06_class_patterns.md"] = make_md(
    "Class Patterns",
    "- Matching dataclass attributes\n- PositionalPattern\n- __match_args__, custom class matching",
    "05_guards_and_wildcards.md",
    "Match object attributes using class patterns.",
    [{"title": "Dataclass Pattern", "code": 'from dataclasses import dataclass\n\n@dataclass\nclass Point:\n    x: int\n    y: int\n\np = Point(10, 20)\nmatch p:\n    case Point(x=0, y=0):\n        print("Origin")\n    case Point(x=x, y=y):\n        print(f"Point at ({x}, {y})")', "explain": "Match by attribute name or position.", "mistake1": "Wrong attribute names", "mistake2": "Not defining __match_args__", "summary1": "Match by attribute name", "summary2": "Can use positional pattern", "summary3": "__match_args__ controls position"}, "01_match_statement.md"]
)

# ============ 02_Control_Flow/06_Context_Managers ============
FILES["02_Control_Flow/06_Context_Managers/05_multiple_context_managers.md"] = make_md(
    "Multiple Context Managers",
    "- Multiple with targets on one line\n- Parenthesised syntax (Python 3.10+)\n- Nested vs combined",
    "04_contextlib_module.md",
    "Manage multiple resources efficiently with stacked context managers.",
    [{"title": "Stacked Syntax", "code": '# Python 3.9+\nwith open("a.txt") as f1, open("b.txt") as f2:\n    content = f1.read() + f2.read()\n\n# Python 3.10+ (parenthesised)\nwith (open("a.txt") as f1, open("b.txt") as f2):\n    content = f1.read() + f2.read()', "explain": "Comma-separated or parenthesised for multiple managers.", "mistake1": "Wrong order of cleanup", "mistake2": "Not handling exceptions in nested", "summary1": "Comma separates managers", "summary2": "Exit in reverse order", "summary3": "Parentheses help readability"}, "01_with_statement.md"]
)

# ============ 03_Functions/04_Function_Design ============
FILES["03_Functions/04_Function_Design/04_argument_validation.md"] = make_md(
    "Argument Validation",
    "- Guarding inputs early\n- Raising TypeError/ValueError\n- assert vs raise, validation libraries",
    "03_function_signatures.md",
    "Validate function inputs to fail fast with clear error messages.",
    [{"title": "Early Validation", "code": 'def divide(a, b):\n    if not isinstance(a, (int, float)):\n        raise TypeError(f"a must be number, got {type(a).__name__}")\n    if b == 0:\n        raise ValueError("b cannot be zero")\n    return a / b\n\n# Usage\ntry:\n    result = divide(10, 0)\nexcept ValueError as e:\n    print(e)  # b cannot be zero', "explain": "Check types and values before processing.", "mistake1": "Not validating at all", "mistake2": "Using assert for validation", "summary1": "Fail fast with clear messages", "summary2": "TypeError for type issues", "summary3": "ValueError for invalid values"}, "05_return_type_patterns.md"]
)

FILES["03_Functions/04_Function_Design/05_return_type_patterns.md"] = make_md(
    "Return Type Patterns",
    "- Returning None vs a value\n- Never returning None silently\n- Tuple returns, Optional",
    "04_argument_validation.md",
    "Design clear return types that communicate intent.",
    [{"title": "Explicit Returns", "code": 'def find_item(items, key):\n    for item in items:\n        if item.get("id") == key:\n            return item  # Found - return the item\n    return None  # Not found - explicitly return None\n\n# Better: use Optional\nfrom typing import Optional\ndef find_item(items: list[dict], key: str) -> Optional[dict]:\n    for item in items:\n        if item.get("id") == key:\n            return item\n    return None', "explain": "Always explicitly return None, don't fall off the end.", "mistake1": "Implicit None return", "mistake2": "Returning None for errors", "summary1": "Explicit None return", "summary2": "Use Optional for nullable", "summary3": "Tuple for multiple values"}, "06_function_documentation.md"]
)

FILES["03_Functions/04_Function_Design/06_function_documentation.md"] = make_md(
    "Function Documentation",
    "- Google vs NumPy docstring styles\n- What to include\n- __doc__, help(), doctest",
    "05_return_type_patterns.md",
    "Write clear documentation that helps users understand and use your functions.",
    [{"title": "Docstring Styles", 'code': '"""Short one-liner.\n\nArgs:\n    name: The person to greet.\n    times: How many times to greet.\n\nReturns:\n    A greeting string.\n\nRaises:\n    ValueError: If times is negative.\n\nExample:\n    >>> greet("Alice", 2)\n    "Hello Alice! Hello Alice!"\n"""', "explain": "Google style is popular and readable.", "mistake1": "No docstring at all", "mistake2": "Outdated docstring", "summary1": "Document params and returns", "summary2": "Include examples", "summary3": "Use help() to test"}, "01_single_responsibility.md"]
)

# ============ 03_Functions/05_Decorators_in_Depth ============
FILES["03_Functions/05_Decorators_in_Depth/02_class_decorators.md"] = make_md(
    "Class Decorators",
    "- Using a class as a decorator\n- __call__ method\n- Stateful decorators with instance variables",
    "01_decorator_anatomy.md",
    "Decorators can be classes too, allowing stateful decoration.",
    [{"title": "Class as Decorator", "code": 'class Counter:\n    def __init__(self, func):\n        self.func = func\n        self.count = 0\n    \n    def __call__(self, *args, **kwargs):\n        self.count += 1\n        print(f"Called {self.func.__name__} {self.count} times")\n        return self.func(*args, **kwargs)\n\n@Counter\ndef say_hello():\n    print("Hello!")\n\nsay_hello()  # Called say_hello 1 times, Hello!\nsay_hello()  # Called say_hello 2 times, Hello!', "explain": "Class must implement __call__ to be used as decorator.", "mistake1": "Forgetting __call__", "mistake2": "Not preserving function metadata", "summary1": "Class decorator holds state", "summary2": "__call__ makes it callable", "summary3": "Use functools.wraps"}, "03_decorator_factories.md"]
)

FILES["03_Functions/05_Decorators_in_Depth/03_decorator_factories.md"] = make_md(
    "Decorator Factories",
    "- Decorators that accept arguments\n- Three levels of nesting\n- Common patterns",
    "02_class_decorators.md",
    "Create decorators that can be configured with parameters.",
    [{"title": "Decorator Factory", "code": 'def repeat(times):\n    def decorator(func):\n        def wrapper(*args, **kwargs):\n            for _ in range(times):\n                result = func(*args, **kwargs)\n            return result\n        return wrapper\n    return decorator\n\n@repeat(times=3)\ndef greet(name):\n    print(f"Hello, {name}!")\n\ngreet("Alice")  # Prints 3 times', "explain": "Factory returns the actual decorator.", "mistake1": "Wrong number of nested levels", "mistake2": "Not returning wrapper", "summary1": "Extra layer for parameters", "summary2": "Factory returns decorator", "summary3": "Configurable behavior"}, "04_stacking_decorators.md"]
)

FILES["03_Functions/05_Decorators_in_Depth/04_stacking_decorators.md"] = make_md(
    "Stacking Decorators",
    "- Order of application when stacking\n- Reading inside-out\n- Interaction between decorators",
    "03_decorator_factories.md",
    "Apply multiple decorators to a single function.",
    [{"title": "Stacked Order", "code": '@decorator_one\n@decorator_two\ndef my_func():\n    pass\n\n# Equivalent to:\nmy_func = decorator_one(decorator_two(my_func))', "explain": "Closest to function applies first (inner to outer).", "mistake1": "Wrong order assumption", "mistake2": "Not testing combined effect", "summary1": "Read inside-out", "summary2": "Bottom decorator runs first", "summary3": "Order matters for behavior"}, "05_builtin_decorators.md"]
)

FILES["03_Functions/05_Decorators_in_Depth/05_builtin_decorators.md"] = make_md(
    "Built-in Decorators",
    "- @property, @staticmethod, @classmethod\n- @functools.cache used in real code",
    "04_stacking_decorators.md",
    "Python provides several useful built-in decorators.",
    [{"title": "Common Built-ins", "code": 'class MyClass:\n    _value = 10\n    \n    @property\n    def value(self):\n        """Read-only property."""\n        return self._value\n    \n    @classmethod\n    def from_string(cls, s):\n        """Alternative constructor."""\n        return cls(int(s))\n    \n    @staticmethod\n    def helper():\n        """Utility function."""\n        return 42\n\n# Usage\nobj = MyClass.from_string("100")\nprint(obj.value)  # 100\nprint(MyClass.helper())  # 42', "explain": "Each has specific use cases and syntax.", "mistake1": "Confusing @staticmethod with @classmethod", "mistake2": "Using @property for mutable state", "summary1": "@property for computed attrs", "summary2": "@classmethod for alt constructors", "summary3": "@staticmethod for utility funcs"}, "06_real_world_decorators.md"]
)

FILES["03_Functions/05_Decorators_in_Depth/06_real_world_decorators.md"] = make_md(
    "Real-World Decorators",
    "- Timing decorator\n- Retry with backoff\n- Rate-limiter\n- Login-required patterns",
    "05_builtin_decorators.md",
    "Practical decorator patterns for production code.",
    [{"title": "Timing Decorator", "code": 'import time\nfrom functools import wraps\n\ndef timer(func):\n    @wraps(func)\n    def wrapper(*args, **kwargs):\n        start = time.perf_counter()\n        result = func(*args, **kwargs)\n        elapsed = time.perf_counter() - start\n        print(f"{func.__name__} took {elapsed:.4f}s")\n        return result\n    return wrapper\n\n@timer\ndef slow_function():\n    time.sleep(1)\n\nslow_function()  # slow_function took 1.0012s', "explain": "Useful for performance monitoring.", "mistake1": "Not using functools.wraps", "mistake2": "Not handling exceptions", "summary1": "Timer for performance", "summary2": "Retry for resilience", "summary3": "Cache for memoization"}, "01_decorator_anatomy.md"]
)

# ============ 04_Data_Structures/04_Comprehensions ============
FILES["04_Data_Structures/04_Comprehensions/04_generator_expressions.md"] = make_md(
    "Generator Expressions",
    "- (x for x in ...) lazy evaluation\n- Memory difference vs list comp\n- When to use",
    "03_set_comprehensions.md",
    "Create lazy iterators that don't load everything into memory.",
    [{"title": "Generator vs List", "# List - eager, all in memory\nsquares_list = [x**2 for x in range(1000000)]\n\n# Generator - lazy, one at a time\nsquares_gen = (x**2 for x in range(1000000))\n\nprint(type(squares_list))  # <class 'list'>\nprint(type(squares_gen))   # <class 'generator'>", "explain": "Generators use parentheses instead of brackets.", "mistake1": "Confusing () with []", "mistake2": "Trying to index generator", "summary1": "Parentheses for generators", "summary2": "Can only iterate once", "summary3": "Memory efficient for large data"}, "05_nested_comprehensions.md"]
)

FILES["04_Data_Structures/04_Comprehensions/05_nested_comprehensions.md"] = make_md(
    "Nested Comprehensions",
    "- Flattening 2D lists\n- Matrix transpose\n- Readability limits, when to use a loop instead",
    "04_generator_expressions.md",
    "Comprehensions can be nested for complex transformations.",
    [{"title": "Flatten 2D List", "# Flatten a matrix\nmatrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]\nflat = [num for row in matrix for num in row]\nprint(flat)  # [1, 2, 3, 4, 5, 6, 7, 8, 9]\n\n# Transpose\ntransposed = [[row[i] for row in matrix] for i in range(3)]\nprint(transposed)  # [[1, 4, 7], [2, 5, 8], [3, 6, 9]]', "explain": "Read left-to-right, outermost first.", "mistake1": "Wrong nesting order", "mistake2": "Too complex to read", "summary1": "Outer loop first", "summary2": "Use for readability", "summary3": "Switch to loop if complex"}, "06_comprehension_performance.md"]
)

FILES["04_Data_Structures/04_Comprehensions/06_comprehension_performance.md"] = make_md(
    "Comprehension Performance",
    "- Timing list comp vs loop vs map\n- Memory profiling\n- Practical benchmarks",
    "05_nested_comprehensions.md",
    "Understand when comprehensions are faster and more memory efficient.",
    [{"title": "Performance Comparison", "code": 'import timeit\n\n# List comprehension\ntimeit.timeit("[x**2 for x in range(1000)]", number=1000)\n# ~0.15 seconds\n\n# For loop\ntimeit.timeit(\n    "result = []\\nfor x in range(1000): result.append(x**2)",\n    number=1000\n)\n# ~0.35 seconds\n\n# Map\ntimeit.timeit("list(map(lambda x: x**2, range(1000)))", number=1000)\n# ~0.25 seconds', "explain": "List comprehensions are typically fastest in Python.", "mistake1": "Premature optimization", "mistake2": "Ignoring readability", "summary1": "List comp fastest usually", "summary2": "Map with lambda slower", "summary3": "Profile before optimizing"}, "01_list_comprehensions.md"]
)

# ============ 04_Data_Structures/05_Collections_Module ============
FILES["04_Data_Structures/05_Collections_Module/03_ordereddict.md"] = make_md(
    "OrderedDict",
    "- Insertion-order before Python 3.7\n- move_to_end()\n- popitem(last=False), LRU cache pattern",
    "02_defaultdict.md",
    "Dictionaries that maintain insertion order (now built into dict in Python 3.7+).",
    [{"title": "OrderedDict Usage", "code": 'from collections import OrderedDict\n\n# Before Python 3.7, dicts weren't ordered\n# Now mostly for compatibility, but useful for:\n\nod = OrderedDict()\nod["a"] = 1\nod["b"] = 2\nod["c"] = 3\n\n# Move item to end\nod.move_to_end("a")\nprint(list(od.keys()))  # [\"b\", \"c\", \"a\"]\n\n# Pop in order\nod.popitem(last=False)  # Removes \"b\" first', "explain": "Mostly obsolete now but has special methods.", "mistake1": "Using when not needed", "mistake2": "Not knowing move_to_end", "summary1": "Maintains insertion order", "summary2": "move_to_end() method", "summary3": "Use regular dict in 3.7+"}, "04_deque.md"]
)

FILES["04_Data_Structures/05_Collections_Module/04_deque.md"] = make_md(
    "Deque",
    "- O(1) appendleft/popleft vs list\n- maxlen for fixed-size ring buffers\n- rotate()",
    "03_ordereddict.md",
    "Double-ended queue optimized for fast appends and pops from both ends.",
    [{"title": "Deque Operations", "code": 'from collections import deque\n\n# O(1) operations on both ends\ndq = deque([1, 2, 3])\ndq.appendleft(0)     # [0, 1, 2, 3]\ndq.append(4)         # [0, 1, 2, 3, 4]\nfirst = dq.popleft()  # Returns 0, dq is [1, 2, 3, 4]\n\n# Fixed-size deque (ring buffer)\nbuffer = deque(maxlen=3)\nbuffer.append(1)  # [1]\nbuffer.append(2)  # [1, 2]\nbuffer.append(3)  # [1, 2, 3]\nbuffer.append(4)  # [2, 3, 4] - oldest removed\n\n# Rotate\ndq = deque([1, 2, 3, 4, 5])\ndq.rotate(2)  # [4, 5, 1, 2, 3]\ndq.rotate(-1) # [5, 1, 2, 3, 4]', "explain": "Perfect for queues and sliding windows.", "mistake1": "Using list for queue", "mistake2": "Not knowing maxlen", "summary1": "O(1) append/pop both ends", "summary2": "maxlen auto-evicts", "summary3": "rotate() for cycling"}, "05_namedtuple.md"]
)

FILES["04_Data_Structures/05_Collections_Module/05_namedtuple.md"] = make_md(
    "Named Tuple",
    "- Lightweight immutable records\n- _asdict(), _replace()\n- _fields, vs dataclass",
    "04_deque.md",
    "Tuples with named fields for readable, lightweight data structures.",
    [{"title": "Named Tuple", "code": 'from collections import namedtuple\n\n# Create named tuple type\nPoint = namedtuple("Point", ["x", "y"])\n\n# Create instances\np1 = Point(10, 20)\np2 = Point(x=5, y=15)\n\n# Access by name or index\nprint(p1.x, p1[0])   # 10 10\nprint(p1.y, p1[1])   # 20 20\n\n# Convert to dict\nprint(p1._asdict())  # {\"x\": 10, \"y\": 20}\n\n# Replace (create new instance)\np3 = p1._replace(x=100)\nprint(p3)            # Point(x=100, y=20)\n\n# Fields\nprint(Point._fields)  # (\"x\", \"y\")', "explain": "Immutable and lightweight.", "mistake1": "Not knowing field access", "mistake2": "Trying to modify in place", "summary1": "Named field access", "summary2": "Immutable", "summary3": "_replace for new values"}, "06_chainmap.md"]
)

FILES["04_Data_Structures/05_Collections_Module/06_chainmap.md"] = make_md(
    "ChainMap",
    "- Layered config lookups\n- Scope simulation\n- Child maps, update only affects first map",
    "05_namedtuple.md",
    "Group multiple dictionaries and search them in order.",
    [{"title": "ChainMap Usage", "code": 'from collections import ChainMap\n\n# Search multiple dicts in order\ndefaults = {"theme": "dark", "font": "Arial"}\nuser_prefs = {"font": "Comic Sans"}\n\nconfig = ChainMap(user_prefs, defaults)\n\nprint(config["theme"])   # dark (falls back to defaults)\nprint(config["font"])   # Comic Sans (found in user_prefs)\n\n# Update affects first map only\nconfig["font"] = "Times"\nprint(user_prefs)       # {\"font\": \"Times\"}\nprint(defaults)          # unchanged\n\n# Add new child map\nlocal_overrides = {"debug": True}\nconfig = ChainMap(local_overrides, user_prefs, defaults)', "explain": "Searches maps in order, updates affect first map.", "mistake1": "Thinking it merges dicts", "mistake2": "Update affects wrong map", "summary1": "Searches in order", "summary2": "First map gets updates", "summary3": "Great for config layers"}, "01_counter.md"]
)

# ============ 04_Data_Structures/06_Sorting_and_Searching ============
FILES["04_Data_Structures/06_Sorting_and_Searching/01_sort_basics.md"] = make_md(
    "Sort Basics",
    "- .sort() vs sorted()\n- In-place vs new list\n- Stability guarantee, reverse=",
    "05_collections_module.md",
    "Fundamentals of sorting in Python.",
    [{"title": "Sort Methods", "# sorted() - returns new list\nnumbers = [5, 2, 8, 1, 9]\nsorted_nums = sorted(numbers)\nprint(sorted_nums)  # [1, 2, 5, 8, 9]\nprint(numbers)      # unchanged [5, 2, 8, 1, 9]\n\n# .sort() - sorts in place\nnumbers.sort()\nprint(numbers)      # [1, 2, 5, 8, 9]\n\n# reverse\nprint(sorted([5, 2, 8], reverse=True))  # [8, 5, 2]\n\n# stable sort - preserves order of equal elements\ndata = [(\"a\", 3), (\"b\", 1), (\"c\", 3)]\nprint(sorted(data, key=lambda x: x[1]))\n# [(\"b\", 1), (\"a\", 3), (\"c\", 3)] - a and c stay in order', "explain": "sorted() returns new list, .sort() modifies in place.", "mistake1": "Confusing sorted() and .sort()", "mistake2": "Forgetting sort is stable", "summary1": "sorted() returns new list", "summary2": ".sort() is in-place", "summary3": "Sort is stable"}, "02_custom_sort_keys.md"]
)

FILES["04_Data_Structures/06_Sorting_and_Searching/02_custom_sort_keys.md"] = make_md(
    "Custom Sort Keys",
    "- key= with lambda\n- operator.attrgetter/itemgetter\n- Multi-key tuple sort",
    "01_sort_basics.md",
    "Sort by any criteria using the key parameter.",
    [{"title": "Key Functions", "code": 'from operator import itemgetter, attrgetter\n\n# Lambda key\nwords = ["banana", "apple", "cherry"]\nprint(sorted(words, key=len))  # [\"apple\", \"banana\", \"cherry\"]\n\n# Multi-key sort with tuples\nstudents = [("Alice", 85), ("Bob", 92), ("Charlie", 85)]\nprint(sorted(students, key=lambda s: (-s[1], s[0])))\n# [(\"Bob\", 92), (\"Alice\", 85), (\"Charlie\", 85)]\n\n# Using itemgetter (faster than lambda)\nsorted(students, key=itemgetter(1))\n\n# For objects\nclass Person:\n    def __init__(self, name, age):\n        self.name, self.age = name, age\npeople = [Person(\"Alice\", 30), Person(\"Bob\", 25)]\nsorted(people, key=attrgetter(\"age\"))', "explain": "key function returns value to sort by.", "mistake1": "Using lambda when attrgetter faster", "mistake2": "Not handling ties", "summary1": "key= specifies sort field", "summary2": "Negative for descending", "summary3": "attrgetter/itemgetter faster"}, "03_bisect_module.md"]
)

FILES["04_Data_Structures/06_Sorting_and_Searching/03_bisect_module.md"] = make_md(
    "Bisect Module",
    "- Binary search on sorted lists\n- bisect_left/right\n- insort, O(log n) lookup",
    "02_custom_sort_keys.md",
    "Efficient binary search for sorted sequences.",
    [{"title": "Binary Search", "code": 'import bisect\n\n# Binary search\nsorted_list = [1, 3, 5, 7, 9, 11]\n\n# bisect_left - position where item would go\npos = bisect.bisect_left(sorted_list, 5)\nprint(pos)  # 2 (index of 5)\n\npos = bisect.bisect_left(sorted_list, 6)\nprint(pos)  # 3 (where 6 would go)\n\n# bisect_right - rightmost position\npos = bisect.bisect_right(sorted_list, 5)\nprint(pos)  # 3\n\n# Insert in sorted order\nbisect.insort(sorted_list, 6)\nprint(sorted_list)  # [1, 3, 5, 6, 7, 9, 11]', "explain": "O(log n) search in sorted data.", "mistake1": "Using on unsorted list", "mistake2": "Confusing left and right", "summary1": "bisect_left for < insertion", "summary2": "bisect_right for <= insertion", "summary3": "insort maintains sort"}, "04_searching_patterns.md"]
)

FILES["04_Data_Structures/06_Sorting_and_Searching/04_searching_patterns.md"] = make_md(
    "Searching Patterns",
    "- Linear scan with in, index()\n- next(filter())\n- When to use each",
    "03_bisect_module.md",
    "Different ways to find items in Python.",
    [{"title": "Search Methods", "code": '# in operator - boolean check\ngreetings = [\"hello\", \"hi\", \"hey\"]\nif \"hi\" in greetings:\n    print(\"Found!\")\n\n# index() - get position\ntry:\n    pos = greetings.index(\"hello\")\nexcept ValueError:\n    print(\"Not found\")\n\n# next() with generator - first match\nitems = [1, 3, 5, 7, 9]\nfirst_odd = next((x for x in items if x % 2 == 1), None)\nprint(first_odd)  # 3\n\n# find in dict\nd = {\"a\": 1, \"b\": 2}\nprint(d.get(\"c\", \"default\"))  # default', "explain": "Choose based on your needs.", "mistake1": "Using index when in suffices", "mistake2": "Not handling not found", "summary1": "in for boolean check", "summary2": "index for position", "summary3": "next with generator for first match"}, "05_heapq_patterns.md"]
)

FILES["04_Data_Structures/06_Sorting_and_Searching/05_heapq_patterns.md"] = make_md(
    "Heapq Patterns",
    "- nlargest/nsmallest\n- Priority queue with heappush/heappop\n- Heap invariant",
    "04_searching_patterns.md",
    "Heap queue for efficient priority-based operations.",
    [{"title": "Heapq Usage", "code": 'import heapq\n\n# nlargest/nsmallest - find top/bottom k\ndata = [1, 8, 2, 23, 7, -1, 44]\nprint(heapq.nlargest(3, data))   # [44, 23, 8]\nprint(heapq.nsmallest(3, data)) # [-1, 1, 2]\n\n# Priority queue\npq = []\nheapq.heappush(pq, (3, \"task3\"))\nheapq.heappush(pq, (1, \"task1\"))\nheapq.heappush(pq, (2, \"task2\"))\n\nwhile pq:\n    priority, task = heapq.heappop(pq)\n    print(f\"{priority}: {task}\")\n# 1: task1, 2: task2, 3: task3\n\n# Heap is min-heap - negate for max\nmax_heap = []\nheapq.heappush(max_heap, (-5, \"a\"))\nheapq.heappush(max_heap, (-10, \"b\"))\nval, item = heapq.heappop(max_heap)\nprint(item, -val)  # b 10', "explain": "Heaps maintain the smallest (or largest) element at index 0.", "mistake1": "Not knowing heap property", "mistake2": "Using for sorted list", "summary1": "nlargest/nsmallest are convenient", "summary2": "heappush/heappop for PQ", "summary3": "Negate for max-heap"}, "01_sort_basics.md"]
)

# ============ 05_OOP/04_Magic_Methods ============
FILES["05_OOP/04_Magic_Methods/02_comparison_methods.md"] = make_md(
    "Comparison Methods",
    "- __eq__, __lt__, __le__, __gt__, __ge__\n- @functools.total_ordering shortcut",
    "01_str_and_repr.md",
    "Define how your objects compare to each other.",
    [{"title": "Comparison Methods", "code": 'from functools import total_ordering\n\n@total_ordering\nclass Version:\n    def __init__(self, major, minor):\n        self.major, self.minor = major, minor\n    \n    def __eq__(self, other):\n        return (self.major, self.minor) == (other.major, other.minor)\n    \n    def __lt__(self, other):\n        return (self.major, self.minor) < (other.major, other.minor)\n    \n    def __repr__(self):\n        return f\"Version({self.major}, {self.minor})\"\n\nv1 = Version(1, 2)\nv2 = Version(1, 3)\nprint(v1 < v2)   # True\nprint(v1 == v1)  # True\nprint(v1 <= v2)  # True (with total_ordering)', "explain": "total_ordering generates other methods from __eq__ and one comparison.", "mistake1": "Not implementing all needed", "mistake2": "Inconsistent comparisons", "summary1": "total_ordering reduces boilerplate", "summary2": "Define __eq__ first", "summary3": "Be consistent"}, "03_arithmetic_methods.md"]
)

FILES["05_OOP/04_Magic_Methods/03_arithmetic_methods.md"] = make_md(
    "Arithmetic Methods",
    "- __add__, __mul__, __radd__, __iadd__\n- Implementing a Vector class",
    "02_comparison_methods.md",
    "Enable arithmetic operations on your objects.",
    [{"title": "Vector Arithmetic", "code": 'class Vector:\n    def __init__(self, x, y):\n        self.x, self.y = x, y\n    \n    def __add__(self, other):\n        return Vector(self.x + other.x, self.y + other.y)\n    \n    def __mul__(self, scalar):\n        return Vector(self.x * scalar, self.y * scalar)\n    \n    def __rmul__(self, scalar):  # 2 * v\n        return self.__mul__(scalar)\n    \n    def __iadd__(self, other):  # v += w\n        self.x += other.x\n        self.y += other.y\n        return self\n    \n    def __repr__(self):\n        return f\"Vector({self.x}, {self.y})\"\n\nv1 = Vector(1, 2)\nv2 = Vector(3, 4)\nprint(v1 + v2)   # Vector(4, 6)\nprint(v1 * 3)    # Vector(3, 6)\nprint(2 * v1)   # Vector(2, 4)\nv1 += v2\nprint(v1)       # Vector(4, 6)', "explain": "__radd__ handles reversed operand order.", "mistake1": "Not returning new object", "mistake2": "Forgetting __rmul__", "summary1": "Binary ops return new object", "summary2": "__iadd__ modifies in place", "summary3": "__rmul__ for reverse order"}, "04_container_methods.md"]
)

FILES["05_OOP/04_Magic_Methods/04_container_methods.md"] = make_md(
    "Container Methods",
    "- __len__, __getitem__, __setitem__, __contains__, __iter__\n- Custom container",
    "03_arithmetic_methods.md",
    "Make your objects act like containers.",
    [{"title": "Container Class", "code": 'class Bag:\n    def __init__(self):\n        self._items = []\n    \n    def __len__(self):\n        return len(self._items)\n    \n    def __getitem__(self, index):\n        return self._items[index]\n    \n    def __setitem__(self, index, value):\n        self._items[index] = value\n    \n    def __contains__(self, item):\n        return item in self._items\n    \n    def __iter__(self):\n        return iter(self._items)\n    \n    def append(self, item):\n        self._items.append(item)\n\nbag = Bag()\nbag.append(1)\nbag.append(2)\nprint(len(bag))   # 2\nprint(bag[0])     # 1\nprint(1 in bag)   # True\nfor item in bag:\n    print(item)', "explain": "Enable bracket notation and iteration.", "mistake1": "Not implementing __len__", "mistake2": "Wrong iteration type", "summary1": "__getitem__ for [] access", "summary2": "__contains__ for in", "summary3": "__iter__ for iteration"}, "05_callable_and_context.md"]
)

FILES["05_OOP/04_Magic_Methods/05_callable_and_context.md"] = make_md(
    "Callable and Context",
    "- __call__ for callable objects\n- __enter__/__exit__ for context manager protocol",
    "04_container_methods.md",
    "Make objects callable and usable as context managers.",
    [{"title": "Callable and Context", "code": '# Callable object\nclass Counter:\n    def __init__(self):\n        self.count = 0\n    \n    def __call__(self):\n        self.count += 1\n        return self.count\n\ncounter = Counter()\nprint(counter())  # 1\nprint(counter())  # 2\n\n# Context manager\nclass Timer:\n    def __enter__(self):\n        import time\n        self.start = time.time()\n        return self\n    \n    def __exit__(self, *args):\n        import time\n        elapsed = time.time() - self.start\n        print(f\"Took {elapsed:.2f}s\")\n\nwith Timer():\n    sum(range(1000000))  # Took 0.03s', "explain": "__call__ makes instance callable like a function.", "mistake1": "Not returning self in __exit__", "mistake2": "Not handling exception args", "summary1": "__call__ enables () syntax", "summary2": "__exit__ cleanup guaranteed", "summary3": "Return self from __enter__"}, "06_attribute_access.md"]
)

FILES["05_OOP/04_Magic_Methods/06_attribute_access.md"] = make_md(
    "Attribute Access",
    "- __getattr__ for missing attrs\n- __setattr__, __delattr__\n- __slots__ interaction",
    "05_callable_and_context.md",
    "Control attribute access on your objects.",
    [{"title": "Attribute Magic", "code": 'class LazyObject:\n    def __getattr__(self, name):\n        # Called only when attr not found\n        if name == \"data\":\n            return \"expensive computation\"\n        raise AttributeError(f\"'{type(self).__name__}' has no '{name}'\"\n    \n    def __setattr__(self, name, value):\n        print(f\"Setting {name} = {value}\")\n        object.__setattr__(self, name, value)\n\nobj = LazyObject()\nprint(obj.data)    # expensive computation\n# obj.missing raises AttributeError\nobj.new_attr = 42  # Setting new_attr = 42', "explain": "Only called for missing attributes.", "mistake1": "Infinite recursion in __setattr__", "mistake2": "Not calling super", "summary1": "__getattr__ for missing", "summary2": "Use object.__setattr__", "summary3": "__slots__ limits attrs"}, "01_str_and_repr.md"]
)

# ============ 05_OOP/05_Design_Patterns ============
FILES["05_OOP/05_Design_Patterns/01_singleton.md"] = make_md(
    "Singleton",
    "- Module-level singleton (the Pythonic way)\n- Metaclass approach\n- When to avoid it",
    "04_magic_methods.md",
    "Ensure only one instance of a class exists.",
    [{"title": "Module Singleton", "# module: database.py\nclass _Database:\n    def connect(self):\n        print(\"Connecting...\")\n\ndatabase = _Database()  # One instance\n\n# Other modules import: from database import database', "explain": "Simple module creates singleton by default.", "mistake1": "Overcomplicating with metaclasses", "mistake2": "Not thread-safe by default", "summary1": "Module is natural singleton", "summary2": "Use Borg pattern for shared state", "summary3": "Often not needed in Python"}, "02_factory_pattern.md"]
)

FILES["05_OOP/05_Design_Patterns/02_factory_pattern.md"] = make_md(
    "Factory Pattern",
    "- Factory functions\n- Classmethods as factories\n- Registry dict pattern",
    "01_singleton.md",
    "Create objects without specifying exact class.",
    [{"title": "Factory Functions", "code": 'class Dog:\n    def speak(self):\n        return \"Woof!\"\n\nclass Cat:\n    def speak(self):\n        return \"Meow!\"\n\n# Factory function\ndef get_pet(pet_type):\n    pets = {\"dog\": Dog, \"cat\": Cat}\n    return pets[pet_type]()\n\npet = get_pet(\"dog\")\nprint(pet.speak())  # Woof!\n\n# Classmethod factory\nclass Pizza:\n    def __init__(self, toppings):\n        self.toppings = toppings\n    \n    @classmethod\n    def margherita(cls):\n        return cls([\"tomato\", \"mozzarella\"])\n    \n    @classmethod\n    def pepperoni(cls):\n        return cls([\"tomato\", \"pepperoni\"])\n\np = Pizza.margherita()', "explain": "Flexible object creation.", "mistake1": "Not using abstract base", "mistake2": "Complex registry", "summary1": "Decouples creation from usage", "summary2": "Classmethod is common form", "summary3": "Registry for plugins"}, "03_observer_pattern.md"]
)

FILES["05_OOP/05_Design_Patterns/03_observer_pattern.md"] = make_md(
    "Observer Pattern",
    "- Event system\n- Subscriber list\n- Notify all\n- Practical pub/sub example",
    "02_factory_pattern.md",
    "Define a one-to-many dependency where subjects notify observers.",
    [{"title": "Observer Implementation", "code": 'class EventManager:\n    def __init__(self):\n        self.listeners = {}\n    \n    def subscribe(self, event, callback):\n        if event not in self.listeners:\n            self.listeners[event] = []\n        self.listeners[event].append(callback)\n    \n    def notify(self, event, data=None):\n        for callback in self.listeners.get(event, []):\n            callback(data)\n\n# Usage\nem = EventManager()\n\ndef on_user_registered(user):\n    print(f\"Sending welcome to {user}\")\n\ndef on_user_registered_admin(admin):\n    print(f\"New user in admin panel: {admin}\")\n\nem.subscribe(\"user.registered\", on_user_registered)\nem.subscribe(\"user.registered\", on_user_registered_admin)\nem.notify(\"user.registered\", \"Alice\")', "explain": "Decouples subject from observers.", "mistake1": "Memory leaks from not unsubscribing", "mistake2": "Synchronous notification", "summary1": "Subscribe/notify pattern", "summary2": "One-to-many relationship", "summary3": "Common in event systems"}, "04_strategy_pattern.md"]
)

FILES["05_OOP/05_Design_Patterns/04_strategy_pattern.md"] = make_md(
    "Strategy Pattern",
    "- Swappable algorithms via function arguments\n- Sorting strategy example",
    "03_observer_pattern.md",
    "Define family of algorithms, encapsulate each one, and make them interchangeable.",
    [{"title": "Strategy Example", "code": 'class Sorter:\n    def __init__(self, strategy):\n        self.strategy = strategy\n    \n    def sort(self, data):\n        return self.strategy(data)\n\ndef quick_sort(data):\n    # Simplified - just for demo\n    return sorted(data)  # Use actual quicksort in prod\n\ndef bubble_sort(data):\n    # Simplified\n    result = data[:]\n    n = len(result)\n    for i in range(n):\n        for j in range(0, n-i-1):\n            if result[j] > result[j+1]:\n                result[j], result[j+1] = result[j+1], result[j]\n    return result\n\nsorter = Sorter(quick_sort)\nprint(sorter.sort([3, 1, 2]))\n\nsorter.strategy = bubble_sort\nprint(sorter.sort([3, 1, 2]))', "explain": "Algorithm can be swapped at runtime.", "mistake1": "Not using interface", "mistake2": "Too many strategies", "summary1": "Encapsulate algorithm", "summary2": "Swap at runtime", "summary3": "Function as strategy"}, "05_decorator_pattern.md"]
)

FILES["05_OOP/05_Design_Patterns/05_decorator_pattern.md"] = make_md(
    "Decorator Pattern (Structural)",
    "- Wrapping objects (not functions)\n- Transparent interface\n- Coffee shop example",
    "04_strategy_pattern.md",
    "Attach additional responsibilities to an object dynamically.",
    [{"title": "Object Decorator", "code": 'class Coffee:\n    def cost(self):\n        return 5\n    \n    def description(self):\n        return \"Coffee\"\n\nclass Milk(Coffee):\n    def __init__(self, coffee):\n        self.coffee = coffee\n    \n    def cost(self):\n        return self.coffee.cost() + 1.5\n    \n    def description(self):\n        return self.coffee.description() + \", Milk\"\n\nclass Sugar(Coffee):\n    def __init__(self, coffee):\n        self.coffee = coffee\n    \n    def cost(self):\n        return self.coffee.cost() + 0.5\n    \n    def description(self):\n        return self.coffee.description() + \", Sugar\"\n\n# Compose\ncoffee = Coffee()\ncoffee = Milk(coffee)\ncoffee = Sugar(coffee)\nprint(coffee.description())  # Coffee, Milk, Sugar\nprint(coffee.cost())        # 7.0', "explain": "Wrap objects to add behavior.", "mistake1": "Not implementing all methods", "mistake2": "Order matters", "summary1": "Wraps the object", "summary2": "Transparent to client", "summary3": "Compose at runtime"}, "06_repository_pattern.md"]
)

FILES["05_OOP/05_Design_Patterns/06_repository_pattern.md"] = make_md(
    "Repository Pattern",
    "- Abstracting data access\n- In-memory vs DB backend\n- Dependency injection",
    "05_decorator_pattern.md",
    "Mediate between the domain and data mapping layers.",
    [{"title": "Repository Pattern", "code": 'from abc import ABC, abstractmethod\n\nclass UserRepository(ABC):\n    @abstractmethod\n    def get(self, user_id):\n        pass\n    \n    @abstractmethod\n    def save(self, user):\n        pass\n\nclass InMemoryUserRepository(UserRepository):\n    def __init__(self):\n        self._users = {}\n    \n    def get(self, user_id):\n        return self._users.get(user_id)\n    \n    def save(self, user):\n        self._users[user.id] = user\n\nclass User:\n    def __init__(self, id, name):\n        self.id, self.name = id, name\n\n# Usage with DI\nrepo = InMemoryUserRepository()\nuser = User(1, \"Alice\")\nrepo.save(user)\nprint(repo.get(1).name)', "explain": "Abstracts data access implementation.", "mistake1": "Not using interface", "mistake2": "Leaking persistence logic", "summary1": "Abstracts storage", "summary2": "Testable with mock", "summary3": "Switch backends easily"}, "01_singleton.md"]
)

# ============ 05_OOP/06_Abstract_Classes_and_Protocols ============
FILES["05_OOP/06_Abstract_Classes_and_Protocols/01_abc_module.md"] = make_md(
    "ABC Module",
    "- ABC base class\n- @abstractmethod\n- Enforcing interface\n- Cannot instantiate ABC",
    "05_design_patterns.md",
    "Abstract Base Classes for defining interfaces.",
    [{"title": "ABC Example", "code": 'from abc import ABC, abstractmethod\n\nclass Shape(ABC):\n    @abstractmethod\n    def area(self):\n        pass\n    \n    @abstractmethod\n    def perimeter(self):\n        pass\n    \n    def describe(self):\n        return f\"Area: {self.area()}\"\n\nclass Circle(Shape):\n    def __init__(self, radius):\n        self.radius = radius\n    \n    def area(self):\n        return 3.14 * self.radius ** 2\n    \n    def perimeter(self):\n        return 2 * 3.14 * self.radius\n\n# Can't do: Shape() - TypeError\ncircle = Circle(5)\nprint(circle.area())  # 78.5', "explain": "Subclasses must implement all abstract methods.", "mistake1": "Forgetting @abstractmethod", "mistake2": "Partial implementation works", "summary1": "ABC enforces interface", "summary2": "Cannot instantiate abstract", "summary3": "Concrete provides implementation"}, "02_abstract_properties.md"]
)

FILES["05_OOP/06_Abstract_Classes_and_Protocols/02_abstract_properties.md"] = make_md(
    "Abstract Properties",
    "- @property + @abstractmethod\n- Concrete implementations in subclasses",
    "01_abc_module.md",
    "Define abstract properties that subclasses must implement.",
    [{"title": "Abstract Property", "code": 'from abc import ABC, abstractmethod\n\nclass Vehicle(ABC):\n    @property\n    @abstractmethod\n    def speed(self):\n        \"\"\"Must be implemented by subclass.\"\"\"\n        pass\n    \n    @property\n    def description(self):\n        return f\"Vehicle going {self.speed} mph\"\n\nclass Car(Vehicle):\n    @property\n    def speed(self):\n        return 60\n\ncar = Car()\nprint(car.speed)        # 60\nprint(car.description)  # Vehicle going 60 mph', "explain": "Combine @property and @abstractmethod.", "mistake1": "Wrong order of decorators", "mistake2": "Not implementing property", "summary1": "Decorators stack", "summary2": "Can provide concrete methods", "summary3": "Subclass must define"}, "03_protocols.md"]
)

FILES["05_OOP/06_Abstract_Classes_and_Protocols/03_protocols.md"] = make_md(
    "Protocols",
    "- typing.Protocol\n- Structural subtyping\n- @runtime_checkable\n- Duck typing formalised",
    "02_abstract_properties.md",
    "Protocols define interfaces without inheritance (static duck typing).",
    [{"title": "Protocol Usage", "code": 'from typing import Protocol, runtime_checkable\n\n@runtime_checkable\nclass Drawable(Protocol):\n    def draw(self) -> str: ...\n\nclass Circle:\n    def draw(self) -> str:\n        return \"Drawing circle\"\n\nclass Square:\n    def draw(self) -> str:\n        return \"Drawing square\"\n\n# Works without inheritance!\ndef render(shape: Drawable):\n    print(shape.draw())\n\nrender(Circle())  # Drawing circle\nrender(Square())  # Drawing square', "explain": "Protocols check interface at runtime or static type check.", "mistake1": "Thinking you need inheritance", "mistake2": "Not using @runtime_checkable", "summary1": "Structural typing", "summary2": "No explicit inheritance", "summary3": "@runtime_checkable for isinstance"}, "04_mixins.md"]
)

FILES["05_OOP/06_Abstract_Classes_and_Protocols/04_mixins.md"] = make_md(
    "Mixins",
    "- Mixin pattern\n- Multiple inheritance for behaviour composition\n- LogMixin example",
    "03_protocols.md",
    "Classes that provide methods to other classes through inheritance.",
    [{"title": "Mixin Pattern", "code": 'class LogMixin:\n    def log(self, message):\n        print(f\"[{self.__class__.__name__}] {message}\")\n\nclass Machine:\n    def start(self):\n        self.log(\"Starting machine\")\n\nclass Robot(Machine, LogMixin):\n    def work(self):\n        self.log(\"Robot working\")\n        print(\"Beep boop\")\n\nrobot = Robot()\nrobot.start()  # [Robot] Starting machine\nrobot.work()   # [Robot] Robot working\\nBeep boop', "explain": "Mixin adds functionality via multiple inheritance.", "mistake1": "Mixin depends on subclass", "mistake2": "Diamond inheritance issues", "summary1": "Provides reusable behavior", "summary2": "Use multiple inheritance", "summary3": "Keep mixins independent"}, "05_interface_design.md"]
)

FILES["05_OOP/06_Abstract_Classes_and_Protocols/05_interface_design.md"] = make_md(
    "Interface Design",
    "- Designing clean public APIs\n- _ vs __\n- __all__, what to expose and hide",
    "04_mixins.md",
    "Guidelines for creating clean, maintainable interfaces.",
    [{"title": "Public vs Private", "code": '# _prefix = internal (convention)\n# __prefix = name mangled (for inheritance)\n# __suffix__ = dunder (special)\n\nclass BankAccount:\n    def __init__(self, balance):\n        self._balance = balance      # Internal - don't use directly\n        self.__account_number = \"\"  # Mangled - for subclass use\n    \n    def deposit(self, amount):\n        \"\"\"Public API - what users should call.\"\"\"\n        self._validate(amount)\n        self._balance += amount\n    \n    def _validate(self, amount):\n        \"\"\"Internal helper - may change.\"\"\"\n        if amount < 0:\n            raise ValueError(\"Negative amount\")\n    \n    def __str__(self):\n        return f\"Account({self._balance})\"\n\n# Define public API\n__all__ = [\"BankAccount\"]  # What \"from module import *\" gets', "explain": "Use _ to mark internal, __all__ to control exports.", "mistake1": "Everything public", "mistake2": "No __all__", "summary1": "_ for internal", "summary2": "__all__ controls imports", "summary3": "Document public API"}, "01_abc_module.md"]
)

# Add more files for remaining folders...
# Continuing with more FILES entries...

# ============ Run the script ============
print("Script created. Run this file to generate all subtopics.")
print("This is a large script with", len(FILES), "files defined.")
