# Example90.py
# Topic: Comprehensive Review - Function Signatures

# This file provides a comprehensive review of function signatures.


# ============================================================
# Example 1: All Parameter Types
# ============================================================
print("=== All Parameter Types ===")

def all_types(
    pos_only,           # Positional-only (before /)
    /,                  # Slash: separates pos-only from regular
    pos_or_kw,         # Positional or keyword
    *args,             # Variable positional (*args)
    kw_only,           # Keyword-only (after *)
    **kwargs            # Variable keyword (**kwargs)
):
    return {
        'pos_only': pos_only,
        'pos_or_kw': pos_or_kw,
        'args': args,
        'kw_only': kw_only,
        'kwargs': kwargs
    }

# Test
result = all_types(1, 2, 3, kw_only=4, extra=5)
print(result)


# ============================================================
# Example 2: Common Patterns
# ====================================
print("\n=== Common Patterns ===")

# Pattern 1: Simple function
def simple(a, b, c):
    return a + b + c

# Pattern 2: With defaults
def with_defaults(a, b=10, c=20):
    return a + b + c

# Pattern 3: Keyword-only
def kw_only_func(*, a, b, c):
    return a + b + c

# Pattern 4: Positional-only
def pos_only_func(a, b, /, c):
    return a + b + c


# ============================================================
# Example 3: When to Use Each
# ====================================
print("\n=== When to Use Each ===")

# Use positional-only when:
# - Parameter name might conflict with keyword
# - Function is internal and will be called with position
# Example: Mathematical functions
def pow(base, /, exponent):
    return base ** exponent

# Use keyword-only when:
# - Parameter is optional but should be explicit
# - Many parameters with defaults
# Example: Configuration
def configure(*, host, port=80, ssl=False):
    return {"host": host, "port": port, "ssl": ssl}

# Use *args/**kwargs when:
# - Variable number of arguments
# - Forwarding arguments
def forward(*args, **kwargs):
    return args, kwargs


# ============================================================
# Example 4: Signature in Practice
# ====================================
print("\n=== Signature in Practice ===")

# Library-like example
class APIClient:
    def request(self, method, /, endpoint, *, headers=None, timeout=30):
        """Make API request."""
        return {
            'method': method,
            'endpoint': endpoint,
            'headers': headers,
            'timeout': timeout
        }

client = APIClient()
result = client.request("GET", "/users", headers={"Auth": "token"}, timeout=60)
print(result)


# ============================================================
# Example 5: Summary
# ====================================
print("\n=== Summary ===")

# Syntax examples
def examples():
    # All positional
    def f1(a, b, c):
        pass
    
    # Positional-only
    def f2(a, b, /, c):
        pass
    
    # Keyword-only
    def f3(*, a, b, c):
        pass
    
    # Combined
    def f4(a, /, b, *, c):
        pass
    
    # With *args, **kwargs
    def f5(a, *args, b, **kwargs):
        pass
    
    return f1, f2, f3, f4, f5

print("Function signature patterns defined!")


# ============================================================
# Summary
# ====================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY: Function Signatures")
print("=" * 50)
print("""
PARAMETER TYPES:

1. POSITIONAL_ONLY (before /):
   def f(a, /, b): pass
   - Must be passed by position
   - Cannot use keyword

2. POSITIONAL_OR_KEYWORD (normal):
   def f(a, b): pass
   - Can use either

3. VAR_POSITIONAL (*args):
   def f(*args): pass
   - Captures extra positional args

4. KEYWORD_ONLY (after *):
   def f(*, a, b): pass
   - Must use keyword

5. VAR_KEYWORD (**kwargs):
   def f(**kwargs): pass
   - Captures extra keyword args

USE CASES:
- Positional-only: protect names, internal APIs
- Keyword-only: optional params, configuration
- Both: flexible, clear APIs
""")
