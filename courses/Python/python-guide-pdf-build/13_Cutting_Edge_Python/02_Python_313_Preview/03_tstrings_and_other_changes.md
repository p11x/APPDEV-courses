# 🎨 Python 3.13's T-Strings and Quality of Life Improvements

## 🎯 What You'll Learn

- T-strings (template strings) — what they are and why they matter
- How t-strings improve security for HTML/SQL templating
- The locals() behavior change in Python 3.13
- Improvements to the REPL
- Migration checklist from Python 3.11/3.12 to 3.13

## 📦 Prerequisites

- Familiarity with f-strings in Python
- Understanding of string formatting in Python

---

## T-Strings: Template Strings (PEP 750)

T-strings (template strings) are a new type of string literal in Python 3.13. They're like f-strings but lazy and safe!

### The Difference: Eager vs Lazy

```python
# F-strings are EAGER - evaluated immediately
name = "Alice"
f_hello = f"Hello, {name}!"  # Evaluates RIGHT NOW
print(f_hello)  # Hello, Alice!

name = "Bob"  # Change name
print(f_hello)  # Still "Hello, Alice!" - already evaluated!

# T-strings are LAZY - evaluated when needed
name = "Alice"
t_hello = t"Hello, {name}!"  # Just stores the template
print(t_hello)  # Hello, Alice!

name = "Bob"  # Change name
print(t_hello)  # Hello, Bob! - re-evaluates when used!
```

### 💡 Line-by-Line Breakdown

```python
name = "Alice"          # Set name to Alice

# f-string - evaluated immediately to a plain string
f_hello = f"Hello, {name}!"   
print(f_hello)  # Hello, Alice! (stored as plain string)

name = "Bob"  # Change name to Bob
print(f_hello)  # Still "Hello, Alice!" - doesn't change!

# t-string - stores a TEMPLATE, evaluates lazily
t_hello = t"Hello, {name}!"  # Stores template object
print(t_hello)  # Hello, Bob! - re-evaluates when printed!

name = "Charlie"  # Change again
print(t_hello)  # Hello, Charlie! - always fresh!
```

---

## Why T-Strings Matter: Security

### The Problem with F-Strings for HTML/SQL

```python
# ⚠️ DANGEROUS: F-strings for HTML can cause injection!
user_input = "<script>alert('xss')</script>"
html = f"<div>{user_input}</div>"
# Result: <div><script>alert('xss')</script></div>
# This executes JavaScript in a browser!

# SQL injection danger!
user_name = "Alice'; DROP TABLE users;--"
query = f"SELECT * FROM users WHERE name = '{user_name}'"
# Result: SELECT * FROM users WHERE name = 'Alice'; DROP TABLE users;--
# This could DELETE your database!
```

### T-Strings for Safe Templating

```python
from string import Template

# Using Template (current approach)
t = Template("<div>$name</div>")
safe_html = t.substitute(name="<script>alert('xss')</script>")
# Result: <div><script>alert('xss')</script></div> - still dangerous!

# T-strings with custom delimiters (safer approach)
# The key benefit: template is separate from data
# Can validate/sanitize BEFORE substitution
```

### How T-Strings Improve Security

```python
# T-string approach for safe templating
def safe_html_template(user_content: str) -> str:
    """Safely create HTML from user content."""
    # Validate first
    sanitized = sanitize(user_content)  # Your sanitization function
    
    # Create template
    template = t"<div>{sanitized}</div>"
    
    # Return safe result
    return str(template)

def sanitize(text: str) -> str:
    """Basic HTML sanitization."""
    # Replace dangerous characters
    return (text
        .replace("&", "&")
        .replace("<", "<")
        .replace(">", ">")
        .replace('"', """)
    )

# Now this is safe!
user_input = "<script>alert('xss')</script>"
result = safe_html_template(user_input)
print(result)  # <div><script>alert(&#x27;xss&#x27;)</script></div>
```

---

## Using T-Strings

### Basic Syntax

```python
# Import Template (built into string module)
from string import Template

# Create a t-string (using t prefix)
greeting = t"Hello, {name}!"

# Fill in values using .substitute()
result = greeting.substitute(name="Alice")
print(result)  # Hello, Alice!

# Or use .safe_substitute() - uses empty string for missing keys
result = greeting.substitute(name="Bob")
print(result)  # Hello, Bob!
```

### T-String Methods

```python
from string import Template

# Basic t-string
t1 = Template("Hello, $name!")

# substitute() - raises KeyError if missing
print(t1.substitute(name="Alice"))  # Hello, Alice!

# safe_substitute() - uses empty string for missing
t2 = Template("Hello, $name! You have $count messages.")
print(t2.safe_substitute(name="Bob"))  # Hello, Bob! You have  messages.

# Get the template's identifier
print(t1.template)  # Hello, $name!
```

### Custom Delimiters

```python
from string import Template

# Use different delimiters for different purposes
html_template = Template("<div>$content</div>", delimiter="$")
sql_template = Template("SELECT * FROM $table WHERE id = $id", delimiter="$")

# Combine templates
page = f"""
<html>
<body>
{html_template.substitute(content="Hello World")}
</body>
</html>
"""
```

---

## locals() Behavior Change

Python 3.13 changes how `locals()` works:

### Old Behavior (Python 3.12 and earlier)

```python
def example():
    x = 10
    y = 20
    
    # Returns LIVE dictionary - changes reflect immediately!
    local_vars = locals()
    local_vars["z"] = 100  # This ADDS z to local scope!
    
    print(z)  # Works! z now exists
```

### New Behavior (Python 3.13+)

```python
def example():
    x = 10
    y = 20
    
    # Returns a SNAPSHOT - changes don't affect local scope!
    local_vars = locals()
    local_vars["z"] = 100  # Only modifies the copy!
    
    # print(z)  # Would still fail - z not in local scope
    print(local_vars["z"])  # 100 - but only in the copy
```

### Migration

```python
def old_code():
    """Old pattern - won't work in 3.13."""
    locals()["dynamic_var"] = 42
    print(dynamic_var)  # Works in 3.12, fails in 3.13

def new_code():
    """New pattern - works in 3.13."""
    local_vars = {"dynamic_var": 42}  # Explicit dict instead
    print(local_vars["dynamic_var"])
```

---

## Better REPL

Python 3.13 includes an improved REPL:

### New Features

1. **Multiline editing** — Write and edit multiline code
2. **Color output** — Syntax highlighting
3. **Block completion** — Better autocomplete for blocks
4. **Persistent history** — Command history across sessions

### Example REPL Session

```python
# In Python 3.13 REPL:

>>> def greet(name: str) -> str:
...     return f"Hello, {name}!"
... # Press Tab after '...' for auto-indent
... 
>>> greet("World")
'Hello, World!'

>>> # Color syntax highlighting is automatic!
>>> for i in range(3):
...     print(f"Count: {i}")
... 
Count: 0
Count: 1
Count: 2
```

---

## Improved `__repr__`

Python 3.13 improves `__repr__` for common types:

```python
# More informative repr in Python 3.13
my_list = [1, 2, 3]
print(repr(my_list))  # [1, 2, 3] - same

# But for some types, more detail
my_dict = {"a": 1, "b": 2}
# In 3.13: {'a': 1, 'b': 2} with better formatting for complex dicts
```

---

## Migration Checklist: 3.11 → 3.12 → 3.13

### Moving to Python 3.12

- [ ] Check for deprecated features (removed in 3.12)
- [ ] Update type hints to use modern syntax (`list[int]` instead of `List[int]`)
- [ ] Test f-string improvements
- [ ] Verify dataclass with slots works

### Moving to Python 3.13

- [ ] Test `locals()` usage — may need refactoring
- [ ] Check for deprecated features removed in 3.13
- [ ] Test t-strings if using template libraries
- [ ] Consider JIT for production (still experimental!)
- [ ] Free-threaded mode — test if using threading heavily

### Code Changes Example

```python
# Python 3.11 style
from typing import List, Dict, Union, Optional
my_list: List[int] = []
my_dict: Dict[str, int] = {}
def process(x: Optional[int]) -> Union[int, str]: ...

# Python 3.12+ style (cleaner!)
my_list: list[int] = []
my_dict: dict[str, int] = {}
def process(x: int | None) -> int | str: ...

# Python 3.13+ can also use:
# - T-strings for template-heavy code
# - locals() snapshot behavior
# - JIT compilation (optional)
```

---

## ✅ Summary

- T-strings (template strings) provide lazy evaluation for safe templating
- T-strings help with HTML/SQL injection by separating template from data
- `locals()` now returns a snapshot, not a live dict
- REPL has multiline editing, color output, and better autocomplete
- Migration from 3.11/3.12 to 3.13 requires minimal changes

## ➡️ Next Steps

Continue to [../03_Metaprogramming/01_metaclasses.md](../03_Metaprogramming/01_metaclasses.md) to dive into metaprogramming with metaclasses.

## 🔗 Further Reading

- [PEP 750: T-Strings](https://peps.python.org/pep-0750/)
- [Python 3.13 Release Notes](https://docs.python.org/3.13/whatsnew/3.13.html)
- [string — Common string operations](https://docs.python.org/3.13/library/string.html)
