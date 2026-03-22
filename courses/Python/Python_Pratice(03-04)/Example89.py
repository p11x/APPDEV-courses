# Example89.py
# Topic: Introspection and Function Signatures

# This file demonstrates inspecting function signatures.


# ============================================================
# Example 1: Using inspect Module
# ============================================================
print("=== Inspecting Function Signatures ===")

import inspect

def example_func(a, b=10, /, c=20, *, d=30, e=40):
    """Example function with various parameter types."""
    pass

sig = inspect.signature(example_func)
print(f"Function: {example_func.__name__}")
print(f"Signature: {sig}")
print("\nParameters:")

for name, param in sig.parameters.items():
    print(f"  {name}:")
    print(f"    kind: {param.kind.name}")
    print(f"    default: {param.default}")


# ============================================================
# Example 2: Parameter Kinds
# ============================================================
print("\n=== Parameter Kinds ===")

def func(a, /, b, *args, c, **kwargs):
    pass

sig = inspect.signature(func)

for name, param in sig.parameters.items():
    print(f"{name}: {param.kind.name}")
# POSITIONAL_ONLY
# POSITIONAL_OR_KEYWORD
# VAR_POSITIONAL (*args)
# KEYWORD_ONLY
# VAR_KEYWORD (**kwargs)


# ============================================================
# Example 3: Binding Arguments
# ============================================================
print("\n=== Binding Arguments ===")

def greet(name, greeting="Hello", punctuation="!"):
    return f"{greeting}, {name}{punctuation}"

sig = inspect.signature(greet)

# Bind positional
bound = sig.bind("John")
print(f"Bound positional: {bound}")
print(f"Call: {greet(*bound.args)}")

# Bind keyword
bound = sig.bind(name="Jane", greeting="Hi")
print(f"Bound keyword: {bound}")
print(f"Call: {greet(**bound.kwargs)}")


# ============================================================
# Example 4: Complete Overview
# ============================================================
print("\n=== Complete Overview ===")

def process(data, /, mode="default", *, verbose=False, output=None):
    """Full example."""
    pass

sig = inspect.signature(process)

print("Function signature:", sig)
print("\nParameters:")
for name, param in sig.parameters.items():
    kind = param.kind
    default = param.default if param.default is not inspect.Parameter.empty else "REQUIRED"
    print(f"  {name}: kind={kind.name}, default={default}")


# ============================================================
# Example 5: Using Signature in Decorators
# ============================================================
print("\n=== Signature in Decorators ===")

import inspect
from functools import wraps

def enforce_types(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        sig = inspect.signature(func)
        bound = sig.bind(*args, **kwargs)
        bound.apply_defaults()
        
        # Check types (simplified)
        for name, value in bound.arguments.items():
            param = sig.parameters[name]
            if param.annotation is not inspect.Parameter.empty:
                if not isinstance(value, param.annotation):
                    print(f"Warning: {name} should be {param.annotation}")
        
        return func(*args, **kwargs)
    return wrapper

@enforce_types
def add(a: int, b: int) -> int:
    return a + b

print(add(1, 2))
print(add("a", "b"))  # Warning!


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("INTROSPECTION AND SIGNATURES")
print("=" * 50)
print("""
Use inspect module:
- inspect.signature() - get function signature
- inspect.Parameter - parameter info
- Parameter kinds:
  * POSITIONAL_ONLY (before /)
  * POSITIONAL_OR_KEYWORD (normal)
  * VAR_POSITIONAL (*args)
  * KEYWORD_ONLY (after *)
  * VAR_KEYWORD (**kwargs)

Use cases:
- Validation
- Documentation
- Type checking
- Binding arguments
""")
