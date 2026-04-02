# 🐍 Python That Writes Python: Dynamic Code Generation

## 🎯 What You'll Learn

- Using exec() and eval() safely for dynamic code execution
- The compile() function for pre-compiling code objects
- The ast module: parse and transform Python source code
- The inspect module: introspect any Python object
- Building a CLI generator from function signatures

## 📦 Prerequisites

- Understanding of Python classes and functions
- Familiarity with basic Python syntax

---

## exec() and eval(): Running Strings as Code

### The Difference

```python
# eval() - evaluates an expression and returns a value
result = eval("2 + 2")  # Returns 4
print(result)

# exec() - executes statements, returns None
exec("x = 2 + 2")  # Executes assignment
print(x)  # x is now 4
```

### Basic Examples

```python
# eval() - for expressions
expression = "2 ** 10"
result = eval(expression)
print(f"{expression} = {result}")  # 1024

# exec() - for statements
code = """
message = "Hello"
for i in range(3):
    print(f"{message} {i}!")
"""
exec(code)
```

### ⚠️ Security Warning: Never Use with User Input!

```python
# ❌ DANGEROUS - DON'T DO THIS!
user_input = "os.system('rm -rf /')"  # Malicious!
# eval(user_input)  # Would execute the command!

# If you MUST evaluate user input, use restricted globals
restricted_globals = {"__builtins__": {}}  # Remove dangerous builtins
restricted_locals = {}

# This is SAFER but still risky - avoid if possible!
# result = eval(user_input, restricted_globals, restricted_locals)
```

---

## compile(): Pre-compile for Performance

### When to Use compile()

```python
# Without compile - parsed every time
code_string = "result = sum(range(1000))"
for _ in range(100):
    exec(code_string)  # Re-parses each time!

# With compile - parse once, execute many times
compiled_code = compile(code_string, "<string>", "exec")
for _ in range(100):
    exec(compiled_code)  # Uses cached code object!
```

### 💡 Line-by-Line Breakdown

```python
code_string = "result = sum(range(1000))"  # Code as string

# compile(source, filename, mode)
# - source: the code string
# - filename: identifier for error messages
# - mode: "exec", "eval", or "single"

compiled_code = compile(code_string, "<string>", "exec")  # Compile once

for _ in range(100):        # Execute 100 times
    exec(compiled_code)     # Reuses compiled code object - faster!
```

---

## The ast Module: Parse Python into a Tree

### Understanding AST Nodes

```python
import ast

# Parse Python code into an Abstract Syntax Tree
code = """
def greet(name: str) -> str:
    return f"Hello, {name}!"
"""

tree = ast.parse(code)

# The tree is made of nodes - let's see what we have
print(ast.dump(tree, indent=2))
```

### Using ast.NodeVisitor

```python
import ast

class FunctionVisitor(ast.NodeVisitor):
    """Visitor that finds all function definitions."""
    
    def __init__(self):
        self.functions = []
    
    def visit_FunctionDef(self, node: ast.FunctionDef):
        """Called for each function definition."""
        func_info = {
            "name": node.name,
            "args": [arg.arg for arg in node.args.args],
            "returns": ast.unparse(node.returns) if node.returns else None,
            "line": node.lineno,
        }
        self.functions.append(func_info)
        self.generic_visit(node)  # Continue visiting children

# Parse and visit
code = """
def add(a: int, b: int) -> int:
    return a + b

def multiply(x: float, y: float) -> float:
    return x * y
"""

tree = ast.parse(code)
visitor = FunctionVisitor()
visitor.visit(tree)

print("Found functions:")
for func in visitor.functions:
    print(f"  {func['name']}({', '.join(func['args'])}) -> {func['returns']}")
```

### 💡 Line-by-Line Breakdown

```python
import ast

class FunctionVisitor(ast.NodeVisitor):  # Inherit from NodeVisitor
    def __init__(self):
        self.functions = []  # Store found functions
    
    def visit_FunctionDef(self, node: ast.FunctionDef):  # Handle FunctionDef nodes
        func_info = {
            "name": node.name,  # Function name
            "args": [arg.arg for arg in node.args.args],  # Parameter names
            "returns": ast.unparse(node.returns) if node.returns else None,  # Return type
            "line": node.lineno,  # Line number in source
        }
        self.functions.append(func_info)
        self.generic_visit(node)  # Keep visiting for nested functions

code = """def add(a: int, b: int) -> int: return a + b"""
tree = ast.parse(code)  # Parse source to AST
visitor = FunctionVisitor()
visitor.visit(tree)  # Visit all nodes
print(visitor.functions)  # [{'name': 'add', 'args': ['a', 'b'], ...}]
```

---

## Real Example: Simple Code Linter

```python
import ast

class SimpleLinter(ast.NodeVisitor):
    """A linter that checks for common issues."""
    
    def __init__(self):
        self.warnings = []
    
    def visit_For(self, node: ast.For):
        """Check for inefficient patterns."""
        # Warn if iterating over range(len(x))
        if isinstance(node.iter, ast.Call):
            if (isinstance(node.iter.func, ast.Name) and 
                node.iter.func.id == 'len'):
                self.warnings.append(
                    f"Line {node.lineno}: Use enumerate() instead of range(len())"
                )
        self.generic_visit(node)
    
    def visit_Compare(self, node: ast.Compare):
        """Check for identity vs equality."""
        for op in node.ops:
            if isinstance(op, ast.Is):
                self.warnings.append(
                    f"Line {node.lineno}: Use == instead of 'is' for comparisons"
                )
        self.generic_visit(node)

def lint_code(source: str) -> list[str]:
    """Lint Python source code."""
    tree = ast.parse(source)
    linter = SimpleLinter()
    linter.visit(tree)
    return linter.warnings

# Test the linter
code = """
users = ["Alice", "Bob"]
for i in range(len(users)):
    print(users[i])

x = 1
if x is 1:
    print("x is one")
"""

warnings = lint_code(code)
for warning in warnings:
    print(f"⚠️  {warning}")
```

---

## The inspect Module: Introspection Powerhouse

### Getting Function Information

```python
import inspect

def greet(name: str, greeting: str = "Hello") -> str:
    """Greet someone with a custom greeting."""
    return f"{greeting}, {name}!"

# Get function signature
sig = inspect.signature(greet)
print(f"Signature: {sig}")
print(f"Parameters:")
for param_name, param in sig.parameters.items():
    print(f"  {param_name}: default={param.default}, annotation={param.annotation}")

# Get source code
print(f"\nSource:\n{inspect.getsource(greet)}")
```

### Using Signature to Build a CLI

```python
import inspect
from pathlib import Path

def create_cli_from_function(func):
    """Generate a simple CLI interface from a function."""
    sig = inspect.signature(func)
    
    print(f"Function: {func.__name__}")
    print(f"Usage: {func.__name__}", end="")
    
    # Print parameter hints
    for name, param in sig.parameters.items():
        if param.default is inspect.Parameter.empty:
            print(f" <{name}>", end="")
        else:
            print(f" [{name}]", end="")
    
    print()
    print(f"Docstring: {func.__doc__}")

# Example function
def make_coffee(
    style: str = "latte",
    sugar: int = 2,
    milk: bool = True
) -> str:
    """Make a cup of coffee."""
    return f"Making {style} with {sugar} spoons of sugar" + (" and milk" if milk else "")

create_cli_from_function(make_coffee)
```

### 💡 Line-by-Line Breakdown

```python
import inspect

def create_cli_from_function(func):  # Take a function as input
    sig = inspect.signature(func)     # Get signature object
    
    print(f"Function: {func.__name__}")  # Function name
    print(f"Usage: {func.__name__}", end="")  # Start usage line
    
    for name, param in sig.parameters.items():  # Iterate over parameters
        if param.default is inspect.Parameter.empty:  # Required param?
            print(f" <{name}>", end="")  # Show as required
        else:
            print(f" [{name}]", end="")  # Show as optional
    
    print()  # New line
    print(f"Docstring: {func.__doc__}")  # Show docstring
```

---

## Real Example: Auto-generate CLI from Functions

```python
import inspect
import argparse
from typing import Callable, Any

def generate_cli(functions: dict[str, Callable], prog: str = "CLI") -> None:
    """Auto-generate a CLI from a dict of functions."""
    parser = argparse.ArgumentParser(prog=prog)
    
    subparsers = parser.add_subparsers(dest="command", help="Available commands")
    
    for name, func in functions.items():
        sub = subparsers.add_parser(name, help=func.__doc__)
        
        # Add arguments based on function signature
        sig = inspect.signature(func)
        for param_name, param in sig.parameters.items():
            if param.default is inspect.Parameter.empty:
                sub.add_argument(param_name, help=f"Parameter: {param_name}")
            else:
                sub.add_argument(
                    f"--{param_name}",
                    default=param.default,
                    type=str,  # Simplified for demo
                    help=f"Parameter: {param_name} (default: {param.default})"
                )
        
        sub.set_defaults(func=func)
    
    args = parser.parse_args()
    
    if hasattr(args, "func"):
        # Call the function with parsed arguments
        func_args = {
            k: v for k, v in vars(args).items() 
            if k not in ("command", "func")
        }
        result = args.func(**func_args)
        if result:
            print(result)
    else:
        parser.print_help()

# Define some CLI functions
def hello(name: str = "World") -> str:
    """Say hello to someone."""
    return f"Hello, {name}!"

def add(a: int, b: int) -> int:
    """Add two numbers."""
    return a + b

# Generate CLI
# generate_cli({"hello": hello, "add": add})
# Usage: python script.py hello --name Alice
#        python script.py add --a 1 --b 2
```

---

## ✅ Summary

- `exec()` and `eval()` run strings as Python code — use with extreme caution
- `compile()` pre-parses code for repeated execution
- `ast` module parses Python source into a tree for analysis/transformation
- `inspect` module provides powerful introspection of Python objects
- Combining these enables dynamic code generation, but security must come first

## ➡️ Next Steps

Continue to folder [14_AI_and_LLM_Apps/01_Anthropic_Claude_API/01_claude_api_setup.md](../14_AI_and_LLM_Apps/01_Anthropic_Claude_API/01_claude_api_setup.md) to learn how to build AI-powered Python applications with Claude.

## 🔗 Further Reading

- [ast — Abstract Syntax Trees](https://docs.python.org/3/library/ast.html)
- [inspect — Inspect live objects](https://docs.python.org/3/library/inspect.html)
- [ Beware of exec() and eval()](https://realpython.com/python-eval/)
